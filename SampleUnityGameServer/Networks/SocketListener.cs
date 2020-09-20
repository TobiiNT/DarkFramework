using DarkNetwork.Networks.Connections;
using DarkNetwork.Networks.Connections.Events.Arguments;
using System;
using System.Net;
using System.Net.Sockets;

namespace SampleUnityGameServer.Networks
{
    public class SocketListener : SocketBase
    {
        public SocketListener()
        {
            this.EventListenSuccess += this.SocketListener_EventListenSuccess;
            this.EventListenException += this.SocketListener_EventListenException;
            this.EventAcceptSuccess += this.SocketListener_EventAcceptSuccess;
            this.EventAcceptException += this.SocketListener_EventAcceptException;
            this.EventDisposeSuccess += this.SocketListener_EventDisposeSuccess;
            this.EventDisposeException += this.SocketListener_EventDisposeException;
        }

        private void SocketListener_EventListenSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is ListenSuccessArgs Args)
            {
                int Port = Args.Port;

                Logging.WriteLine($"Start listen at port {Port}");
            }
        }
        private void SocketListener_EventListenException(object Sender, EventArgs Arguments)
        {
            if (Arguments is ListenExceptionArgs Args)
            {                
                Exception Exception = Args.Exception;

                Logging.WriteLine($"Start listen has an exception", Exception);
            }
        }

        private void SocketListener_EventAcceptSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is AcceptSuccessArgs Args)
            {
                Socket Socket = Args.Socket;

                ConnectionListener NewConnection = new ConnectionListener();
                NewConnection.Start(Socket);
                World.Connections.Add(NewConnection);

                Logging.WriteLine($"Accept new client connection from {(IPEndPoint)Socket.RemoteEndPoint}");
            }
        }

        private void SocketListener_EventAcceptException(object Sender, EventArgs Arguments)
        {
            if (Arguments is DisposeExceptionArgs Args)
            {
                Exception Exception = Args.Exception;

                Logging.WriteLine($"Accept new client has an exception", Exception);
            }
        }

        private void SocketListener_EventDisposeSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is DisposeSuccessArgs Args)
            {
                string Caller = Args.Caller;

                Logging.WriteLine($"Listener has been disposed by {Caller}");
            }
        }

        private void SocketListener_EventDisposeException(object Sender, EventArgs Arguments)
        {
            if (Arguments is DisposeExceptionArgs Args)
            {
                string Caller = Args.Caller;
                Exception Exception = Args.Exception;

                Logging.WriteLine($"Listener disposed by {Caller} has an exception", Exception);
            }
        }
    }
}
