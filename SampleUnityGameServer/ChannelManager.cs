using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;
using DarkThread;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace SampleUnityGameServer
{
    public class ChannelManager
    {
        private UniqueIDFactory ChannelUniqueIDFactory { set; get; }
        private UniqueIDFactory ClientUniqueIDFactory { set; get; }
        public ThreadSafeDictionary<ushort, SecurityServer> Channels { set; get; }

        public ChannelManager()
        {
            this.ChannelUniqueIDFactory = new UniqueIDFactory(1);
            this.ClientUniqueIDFactory = new UniqueIDFactory(100000);

            this.Channels = new ThreadSafeDictionary<ushort, SecurityServer>();

        }

        public void StartNewChannel(int Port)
        {
            try
            {
                SecurityServer Channel = CreateNewChannel();

                Channel.StartListening(Port);

                if (!this.Channels.ContainsKey(Channel.ChannelID))
                {
                    Channels.Add(Channel.ChannelID, Channel);

                    Logging.WriteLine($"Create new channel with port {Port}");
                }
            }
            catch (Exception Exception)
            {
                Logging.WriteError($"Failed to create new channel with port {Port}", Exception);
            }
        }

        public void RemoveChannel(ushort ChannelID)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                Channel.Dispose();
                this.Channels.RemoveSafe(ChannelID);
                this.ChannelUniqueIDFactory.Release(ChannelID);
            }
        }

        public SecurityServer CreateNewChannel()
        {
            SecurityServer Channel = new SecurityServer();

            Channel.ChannelID = (ushort)ChannelUniqueIDFactory.GetNext();
            Channel.ServerAcceptSuccess += OnServerAcceptSuccess;
            Channel.ServerAcceptException += OnServerAcceptException;
            Channel.ServerListenSuccess += OnServerListenSuccess;
            Channel.ServerListenException += OnServerListenException;
            Channel.ServerDisposeSuccess += OnServerDisposeSuccess;
            Channel.ServerDisposeException += OnServerDisposeException;

            return Channel;
        }

        public SecurityConnection<ServerSecurityNetwork> CreateNewConnection()
        {
            SecurityConnection<ServerSecurityNetwork> NewConnection = new SecurityConnection<ServerSecurityNetwork>();

            NewConnection.ClientID = (uint)ClientUniqueIDFactory.GetNext();
            NewConnection.AuthenticationSuccess += this.OnConnectionAuthenticationSuccess;
            NewConnection.AuthenticationFailed += this.OnConnectionAuthenticationFailed;
            NewConnection.AuthenticationException += this.OnConnectionAuthenticationException;
            NewConnection.ConnectionConnectSuccess += this.OnConnectionConnectSuccess;
            NewConnection.ConnectionConnectException += this.OnConnectionConnectException;
            NewConnection.ConnectionStartSuccess += this.OnConnectionStartSuccess;
            NewConnection.ConnectionStartException += this.OnConnectionStartException;
            NewConnection.ConnectionSendSuccess += this.OnConnectionSendSuccess;
            NewConnection.ConnectionSendException += this.OnConnectionSendException;
            NewConnection.ConnectionReceiveSuccess += this.OnConnectionReceiveSuccess;
            NewConnection.ConnectionReceiveException += this.OnConnectionReceiveException;
            NewConnection.ConnectionDisposeSuccess += this.OnConnectionDisposeSuccess;
            NewConnection.ConnectionDisposeException += this.OnConnectionDisposeException;

            return NewConnection;
        }

        private void OnServerAcceptSuccess(ushort ChannelID, Socket Socket)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                try
                {
                    SecurityConnection<ServerSecurityNetwork> NewConnection = this.CreateNewConnection();

                    NewConnection.ConnectWithSocket(Socket);

                    if (!Channel.Connections.ContainsKey(NewConnection.ClientID))
                    {
                        Channel.Connections.Add(NewConnection.ClientID, NewConnection);

                        Logging.WriteLine($"Accept new client connection from {(IPEndPoint)Socket.RemoteEndPoint}");
                    }
                }
                catch (Exception Exception)
                {
                    Logging.WriteError($"Accept new client exception", Exception);
                }
            }
        }

        private void OnServerAcceptException(ushort ChannelID, Exception Exception)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                Logging.WriteError($"Channel {Channel.ChannelID} : Accepting socket has an exception", Exception);
            }
        }

        private void OnServerListenSuccess(ushort ChannelID, int Port)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                Logging.WriteError($"Channel {Channel.ChannelID} : Start listening to connection at port {Port}");
            }
        }
        private void OnServerListenException(ushort ChannelID, int Port, Exception Exception)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                Logging.WriteError($"Channel {Channel.ChannelID} : Listening to connection at port {Port} has an exception", Exception);

                this.RemoveChannel(ChannelID);
            }
        }


        private void OnServerDisposeSuccess(ushort ChannelID, string Caller)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                Logging.WriteError($"Channel {Channel.ChannelID} : Server socket has been disposed by function {Caller}");

                foreach (var Connection in Channel.Connections.Values.ToList())
                {
                    Connection.Dispose();

                    Channel.Connections.RemoveSafe(Connection.ClientID);
                }

                this.RemoveChannel(ChannelID);
            }
        }
        private void OnServerDisposeException(ushort ChannelID, string Caller, Exception Exception)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                Logging.WriteError($"Channel {Channel.ChannelID} : Server socket disposing by function {Caller} has an exception", Exception);

                this.RemoveChannel(ChannelID);
            }
        }

        private void OnConnectionAuthenticationSuccess(ushort ChannelID, uint ClientID)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                if (Channel.Connections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Setup secure connection success");                    
                }
            }
        }
        private void OnConnectionAuthenticationFailed(ushort ChannelID, uint ClientID)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                if (Channel.Connections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Fail to setup secure connection");
                }
            }
        }
        private void OnConnectionAuthenticationException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                if (Channel.Connections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Setup secure connection exception", Exception);
                }
            }
        }
        private void OnConnectionStartSuccess(ushort ChannelID, uint ClientID)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                if (Channel.Connections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Started to {Client.IPEndPoint}");
                }
            }
        }
        private void OnConnectionStartException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                if (Channel.Connections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Started to {Client.IPEndPoint}", Exception);
                }
            }
        }
        private void OnConnectionConnectSuccess(ushort ChannelID, uint ClientID)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                if (Channel.Connections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Connected to {Client.IPEndPoint} exception");
                }
            }
        }
        private void OnConnectionConnectException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                if (Channel.Connections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Connected to {Client.IPEndPoint} exception", Exception);
                }
            }
        }
        private void OnConnectionSendSuccess(ushort ChannelID, uint ClientID, int DataSize)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                if (Channel.Connections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Send to {Client.IPEndPoint} {DataSize} bytes");
                }
            }
        }
        private void OnConnectionSendException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                if (Channel.Connections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Send to {Client.IPEndPoint} exception", Exception);
                }
            }
        }
        private void OnConnectionReceiveSuccess(ushort ChannelID, uint ClientID, int DataSize, byte[] Data)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                if (Channel.Connections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Receive from {Client.IPEndPoint} {DataSize} bytes");
                }
            }
        }
        private void OnConnectionReceiveException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                if (Channel.Connections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Receive from {Client.IPEndPoint}", Exception);
                }
            }
        }
        private void OnConnectionDisposeSuccess(ushort ChannelID, uint ClientID, string Caller)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                if (Channel.Connections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Disposed by {Caller}");
                }
            }
        }
        private void OnConnectionDisposeException(ushort ChannelID, uint ClientID, string Caller, Exception Exception)
        {
            if (this.Channels.TryGetValue(ChannelID, out SecurityServer Channel))
            {
                if (Channel.Connections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Disposed by {Caller} exception", Exception);
                }
            }
        }
    }
}
