using System;
using System.Net;
using System.Net.Sockets;
using DarkNetwork.Connections.Events;
using DarkNetwork.Connections.Events.Arguments;

namespace DarkNetwork.Connections
{
    public class HostBase : HostEventHandler
    {
        private Socket Socket { set; get; }
        private bool SocketIsRunning { set; get; }
        private bool IsDisposed { set; get; }

        public HostBase()
        {
            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.SocketIsRunning = false;
            this.IsDisposed = false;
        }

        public void StartListening(int Port, int MaximumQueueConnection)
        {
            try
            {
                this.Socket.Bind(new IPEndPoint(IPAddress.Any, Port));
                this.Socket.Listen(MaximumQueueConnection);
                this.SocketIsRunning = true;

                this.ResetSocket();

                OnListenSuccess(this, new ListenSuccessArgs(Port));
            }
            catch (Exception Exception)
            {
                OnListenException(this, new ListenExceptionArgs(Port, Exception));

                this.StopListening();
            }
        }
        public void StopListening()
        {
            var Caller = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;

            try
            {
                this.SocketIsRunning = false;

                if (this.Socket != null)
                {
                    this.Socket.Shutdown(SocketShutdown.Both);
                }

                if (this.Socket != null)
                {
                    this.Socket.Close();
                }

                OnStopSuccess(this, new StopSuccessArgs(Caller));
            }
            catch (Exception Exception)
            {
                OnStopException(this, new StopExceptionArgs(Caller, Exception));
            }
            finally
            {
                this.Socket = null;
            }
        }
        public void Dispose()
        {
            if (!this.IsDisposed)
            {
                var Caller = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;

                try
                {
                    this.StopListening();

                    OnDisposeSuccess(this, new DisposeSuccessArgs(Caller));
                }
                catch (Exception Exception)
                {
                    OnDisposeException(this, new DisposeExceptionArgs(Caller, Exception));
                }
                finally
                {
                    this.IsDisposed = true;
                }
            }
        }

        private void AcceptSocket(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                OnAcceptSuccess(this, new AcceptSuccessArgs(e.AcceptSocket));

                this.ResetSocket();
            }
            catch (Exception Exception)
            {
                OnAcceptException(this, new AcceptExceptionArgs(Exception));
            }
        }

        private void ResetSocket()
        {
            using var EventArgs = new SocketAsyncEventArgs();
            EventArgs.UserToken = this;
            EventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptSocket);

            this.Socket.AcceptAsync(EventArgs);
        }
    }
}
