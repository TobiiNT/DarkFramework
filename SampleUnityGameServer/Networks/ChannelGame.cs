using DarkGamePacket.Interfaces;
using DarkGamePacket.Servers;
using DarkGamePacket.Servers.Interfaces;
using DarkSecurity.Enums;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;
using SampleUnityGameServer.Games;
using System;
using System.Net.Sockets;
using DarkThreading;
using SampleUnityGameServer.Configurations;

namespace SampleUnityGameServer.Networks
{
    public class ChannelGame : SecurityServer
    {
        public ThreadSafeDictionary<uint, SecurityConnection<ServerSecurityNetwork>> ClientConnections { set; get; }
        public IServerPacketHandler PacketHandlerManager { private set; get; }
        public ChannelGame(ushort ChannelID, uint Capacity)
        {
            this.ChannelID = ChannelID;
            this.Capacity = Capacity;
            this.ClientConnections = new ThreadSafeDictionary<uint, SecurityConnection<ServerSecurityNetwork>>();
        }

        public void ImportGame(LogicGame Game)
        {
            this.PacketHandlerManager = new ServerPacketHandler(Game.PacketHandler);
        }

        public bool CreateNewConnection(uint ClientID, Socket Socket)
        {
            var NewConnection = new SecurityConnection<ServerSecurityNetwork>();

            NewConnection.SetKeySize(Configuration.AsymmetricKeySize, Configuration.SymmetricKeySize, Configuration.MessageTestLength);

            NewConnection.ChannelID = ChannelID;
            NewConnection.ClientID = ClientID;
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

            NewConnection.ConnectWithSocket(Socket);

            if (!this.ClientConnections.ContainsKey(NewConnection.ClientID))
            {
                this.PacketHandlerManager?.HandleHandshake(ClientID, NewConnection);
                this.ClientConnections.Add(NewConnection.ClientID, NewConnection);

                return true;
            }
            return false;
        }

        private void OnConnectionAuthenticationSuccess(ushort ChannelID, uint ClientID)
        {
            if (this.ChannelID == ChannelID)
            {
                if (this.ClientConnections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteLine($"Channel {Client.ChannelID}, Client {Client.ClientID} : Setup secure connection success");
                }
            }
        }
        private void OnConnectionAuthenticationFailed(ushort ChannelID, uint ClientID)
        {
            if (this.ChannelID == ChannelID)
            {
                if (this.ClientConnections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Fail to setup secure connection");
                }
            }
        }
        private void OnConnectionAuthenticationException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            if (this.ChannelID == ChannelID)
            {
                if (this.ClientConnections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Setup secure connection exception", Exception);
                }
            }
        }
        private void OnConnectionStartSuccess(ushort ChannelID, uint ClientID)
        {
            if (this.ChannelID == ChannelID)
            {
                if (this.ClientConnections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteLine($"Channel {Client.ChannelID}, Client {Client.ClientID} : Started to {Client.IPEndPoint}");
                }
            }
        }
        private void OnConnectionStartException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            if (this.ChannelID == ChannelID)
            {
                if (this.ClientConnections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Started to {Client.IPEndPoint}", Exception);
                }
            }
        }
        private void OnConnectionConnectSuccess(ushort ChannelID, uint ClientID)
        {
            if (this.ChannelID == ChannelID)
            {
                if (this.ClientConnections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteLine($"Channel {Client.ChannelID}, Client {Client.ClientID} : Connected to {Client.IPEndPoint}");
                }
            }
        }
        private void OnConnectionConnectException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            if (this.ChannelID == ChannelID)
            {
                if (this.ClientConnections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Connected to {Client.IPEndPoint} exception", Exception);


                }
            }
        }
        private void OnConnectionSendSuccess(ushort ChannelID, uint ClientID, int DataSize)
        {
            if (this.ChannelID == ChannelID)
            {
                if (this.ClientConnections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteLine($"Channel {Client.ChannelID}, Client {Client.ClientID} : Send to {Client.IPEndPoint} {DataSize} bytes");
                }
            }
        }
        private void OnConnectionSendException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            if (this.ChannelID == ChannelID)
            {
                if (this.ClientConnections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Send to {Client.IPEndPoint} exception", Exception);
                }
            }
        }
        private void OnConnectionReceiveSuccess(ushort ChannelID, uint ClientID, int DataSize, byte[] Data)
        {
            if (this.ChannelID == ChannelID)
            {
                if (this.ClientConnections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteLine($"Channel {Client.ChannelID}, Client {Client.ClientID} : Receive from {Client.IPEndPoint} {DataSize} bytes");

                    this.PacketHandlerManager?.HandlePacket(ClientID, Data);
                }
            }
        }
        private void OnConnectionReceiveException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            if (this.ChannelID == ChannelID)
            {
                if (this.ClientConnections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Receive from {Client.IPEndPoint}", Exception);
                }
            }
        }
        private void OnConnectionDisposeSuccess(ushort ChannelID, uint ClientID, string Caller)
        {
            if (this.ChannelID == ChannelID)
            {
                if (this.ClientConnections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteLine($"Channel {Client.ChannelID}, Client {Client.ClientID} : Disposed by {Caller}");

                    this.ClientConnections.RemoveSafe(ClientID);

                    this.PacketHandlerManager?.HandleDisconnect(ClientID);
                }
            }
        }
        private void OnConnectionDisposeException(ushort ChannelID, uint ClientID, string Caller, Exception Exception)
        {
            if (this.ChannelID == ChannelID)
            {
                if (this.ClientConnections.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Client))
                {
                    Logging.WriteError($"Channel {Client.ChannelID}, Client {Client.ClientID} : Disposed by {Caller} exception", Exception);

                    this.ClientConnections.RemoveSafe(ClientID);

                    this.PacketHandlerManager?.HandleDisconnect(ClientID);
                }
            }
        }
    }
}
