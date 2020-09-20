using DarkNetwork.Networks.Connections;
using DarkNetwork.Networks.Connections.Events.Arguments;
using DarkPacket.Packets;
using System;

namespace SampleUnityGameClient.Networks
{
    public class ConnectionClient : ConnectionBase
    {
        public ConnectionClient()
        {
            this.EventStartSuccess += this.ConnectionClient_EventStartSuccess;
            this.EventStartException += this.ConnectionClient_EventStartException;
            this.EventConnectSuccess += this.ConnectionClient_EventConnectSuccess;
            this.EventConnectException += this.ConnectionClient_EventConnectException;
            this.EventSendSuccess += this.ConnectionClient_EventSendSuccess;
            this.EventSendException += this.ConnectionClient_EventSendException;
            this.EventReceiveSuccess += this.ConnectionClient_EventReceiveSuccess;
            this.EventReceiveException += this.ConnectionClient_EventReceiveException;
            this.EventDisposeSuccess += this.ConnectionClient_EventDisposeSuccess;
            this.EventDisposeException += this.ConnectionClient_EventDisposeException;
        }

        private void ConnectionClient_EventStartSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is StartSuccessArgs)
            {
                Logging.WriteLine($"Started listening to {this.IPEndPoint}");
            }
        }

        private void ConnectionClient_EventStartException(object Sender, EventArgs Arguments)
        {
            if (Arguments is StartExceptionArgs Args)
            {
                Exception Exception = Args.Exception;

                Logging.WriteLine($"Started listening to {this.IPEndPoint} has an exception", Exception);
            }
        }

        private void ConnectionClient_EventConnectSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is ConnectSuccessArgs)
            {
                Logging.WriteLine($"Connected to {this.IPEndPoint}");
            }
        }

        private void ConnectionClient_EventConnectException(object Sender, EventArgs Arguments)
        {
            if (Arguments is ConnectExceptionArgs Args)
            {
                Exception Exception = Args.Exception;

                Logging.WriteLine($"Connect to {this.IPEndPoint} has an exception", Exception);
            }
        }

        private void ConnectionClient_EventSendSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is SendSuccessArgs Args)
            {
                int DataSize = Args.DataSize;

                Logging.WriteLine($"Sended to {this.IPEndPoint} {DataSize} bytes");
            }
        }

        private void ConnectionClient_EventSendException(object Sender, EventArgs Arguments)
        {
            if (Arguments is SendExceptionArgs Args)
            {
                Exception Exception = Args.Exception;

                Logging.WriteLine($"Send to {this.IPEndPoint} has an exception", Exception);
            }
        }

        private void ConnectionClient_EventReceiveSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is ReceiveSuccessArgs Args)
            {
                int DataSize = Args.DataSize;
                byte[] PacketData = Args.PacketData;

                //Logging.WriteLine($"Received {DataSize} bytes");
                using (PacketReader Reader = new PacketReader(PacketData))
                {
                    Logging.WriteLine($"Received from {this.IPEndPoint} {DataSize} bytes : {Reader.ReadString()}");
                }
            }
        }

        private void ConnectionClient_EventReceiveException(object Sender, EventArgs Arguments)
        {
            if (Arguments is ReceiveExceptionArgs Args)
            {
                Exception Exception = Args.Exception;

                Logging.WriteLine($"Receive from {this.IPEndPoint} has an exception", Exception);
            }
        }

        private void ConnectionClient_EventDisposeSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is DisposeSuccessArgs Args)
            {
                string Caller = Args.Caller;

                Logging.WriteLine($"Connection to {this.IPEndPoint} has been disposed by {Caller}");
            }
        }

        private void ConnectionClient_EventDisposeException(object Sender, EventArgs Arguments)
        {
            if (Arguments is DisposeExceptionArgs Args)
            {
                string Caller = Args.Caller;
                Exception Exception = Args.Exception;

                Logging.WriteLine($"Connection to {this.IPEndPoint} disposed by {Caller} has an exception", Exception);
            }
        }
    }
}
