using DarkNetwork.Networks.Connections;
using DarkNetwork.Networks.Connections.Events.Arguments;
using DarkPacket.Packets;
using System;

namespace SampleUnityGameServer.Networks
{
    public class ConnectionListener : ConnectionBase
    {
        public ConnectionListener()
        {
            this.EventStartSuccess += this.ConnectionListener_EventStartSuccess;
            this.EventStartException += this.ConnectionListener_EventStartException;
            this.EventConnectSuccess += this.ConnectionListener_EventConnectSuccess;
            this.EventConnectException += this.ConnectionListener_EventConnectException;
            this.EventSendSuccess += this.ConnectionListener_EventSendSuccess;
            this.EventSendException += this.ConnectionListener_EventSendException;
            this.EventReceiveSuccess += this.ConnectionListener_EventReceiveSuccess;
            this.EventReceiveException += this.ConnectionListener_EventReceiveException;
            this.EventDisposeSuccess += this.ConnectionListener_EventDisposeSuccess;
            this.EventDisposeException += this.ConnectionListener_EventDisposeException;
        }

        private void ConnectionListener_EventStartSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is StartSuccessArgs)
            {
                Logging.WriteLine($"Started listening to {this.IPEndPoint}");
            }
        }

        private void ConnectionListener_EventStartException(object Sender, EventArgs Arguments)
        {
            if (Arguments is StartExceptionArgs Args)
            {
                Exception Exception = Args.Exception;

                Logging.WriteLine($"Started listening to {this.IPEndPoint} has an exception", Exception);
            }
        }

        private void ConnectionListener_EventConnectSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is ConnectSuccessArgs)
            {
                Logging.WriteLine($"Connected to {this.IPEndPoint}");
            }
        }

        private void ConnectionListener_EventConnectException(object Sender, EventArgs Arguments)
        {
            if (Arguments is ConnectExceptionArgs Args)
            {
                Exception Exception = Args.Exception;

                Logging.WriteLine($"Connect to {this.IPEndPoint} has an exception", Exception);
            }
        }

        private void ConnectionListener_EventSendSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is SendSuccessArgs Args)
            {
                int DataSize = Args.DataSize;

                Logging.WriteLine($"Sended to {this.IPEndPoint} {DataSize} bytes");
            }
        }

        private void ConnectionListener_EventSendException(object Sender, EventArgs Arguments)
        {
            if (Arguments is SendExceptionArgs Args)
            {
                Exception Exception = Args.Exception;

                Logging.WriteLine($"Send to {this.IPEndPoint} has an exception", Exception);
            }
        }

        private void ConnectionListener_EventReceiveSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is ReceiveSuccessArgs Args)
            {
                int DataSize = Args.DataSize;
                byte[] PacketData = Args.PacketData;

                //Logging.WriteLine($"Received {DataSize} bytes");
                using (PacketReader Reader = new PacketReader(PacketData))
                {
                    Logging.WriteLine($"Receive from {this.IPEndPoint} {DataSize} bytes : {Reader.ReadString()}");
                }
            }
        }

        private void ConnectionListener_EventReceiveException(object Sender, EventArgs Arguments)
        {
            if (Arguments is ReceiveExceptionArgs Args)
            {
                Exception Exception = Args.Exception;

                Logging.WriteLine($"Receive from {this.IPEndPoint} has an exception", Exception);
            }
        }

        private void ConnectionListener_EventDisposeSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is DisposeSuccessArgs Args)
            {
                string Caller = Args.Caller;

                World.Connections.Remove(this);

                Logging.WriteLine($"Connection to {this.IPEndPoint} has been disposed {Caller}");
            }
        }

        private void ConnectionListener_EventDisposeException(object Sender, EventArgs Arguments)
        {
            if (Arguments is DisposeExceptionArgs Args)
            {
                string Caller = Args.Caller;
                Exception Exception = Args.Exception;

                World.Connections.Remove(this);

                Logging.WriteLine($"Connection to {this.IPEndPoint} disposed by {Caller} has an exception", Exception);
            }
        }
    }
}
