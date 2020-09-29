using System;
using System.Net;
using System.Net.Sockets;
using DarkNetwork.Connections.Events;
using DarkNetwork.Connections.Events.Arguments;

namespace DarkNetwork.Connections
{
    public class SocketBase : SocketEventHandler
    {
        private Socket Socket { set; get; }
        public bool SocketOn { private set; get; }
        public bool Disposed { private set; get; }

        public SocketBase()
        {
            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.SocketOn = false;
            this.Disposed = false;
        }

        public void StartListening(int Port)
        {
            try
            {
                this.Socket.Bind(new IPEndPoint(IPAddress.Any, Port));
                this.Socket.Listen(10000);
                this.SocketOn = true;

                this.ResetSocket();

                OnListenSuccess(this, new ListenSuccessArgs(Port));
            }
            catch (Exception Exception)
            {
                OnListenException(this, new ListenExceptionArgs(Port, Exception));

                this.SocketOn = false;
                this.Socket?.Shutdown(SocketShutdown.Both);
                this.Socket?.Close();
            }
        }

        public void Dispose()
        {
            var Caller = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;

            try
            {
                if (!this.Disposed)
                {
                    this.SocketOn = false;
                    this.Disposed = true;

                    this.Socket?.Shutdown(SocketShutdown.Both);
                    this.Socket?.Close();
                    this.Socket = null;

                    OnDisposeSuccess(this, new DisposeSuccessArgs(Caller));
                }
            }
            catch (Exception Exception)
            {
                OnDisposeException(this, new DisposeExceptionArgs(Caller, Exception));
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
            using (var EventArgs = new SocketAsyncEventArgs())
            {
                EventArgs.UserToken = this;
                EventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptSocket);

                this.Socket.AcceptAsync(EventArgs);
            }
        }
    }
}
