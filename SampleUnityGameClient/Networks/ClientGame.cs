using System;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;
using SampleUnityGameClient.Games;

namespace SampleUnityGameClient.Networks
{
    public class ClientGame : SecurityConnection<ClientSecurityNetwork>
    {
        public ClientLogic LogicGame { private set; get; }
        public ClientGame()
        {
            this.AuthenticationSuccess += this.OnConnectionAuthenticationSuccess;
            this.AuthenticationFailed += this.OnConnectionAuthenticationFailed;
            this.AuthenticationException += this.OnConnectionAuthenticationException;
            this.ConnectionConnectSuccess += this.OnConnectionConnectSuccess;
            this.ConnectionConnectException += this.OnConnectionConnectException;
            this.ConnectionStartSuccess += this.OnConnectionStartSuccess;
            this.ConnectionStartException += this.OnConnectionStartException;
            this.ConnectionSendSuccess += this.OnConnectionSendSuccess;
            this.ConnectionSendException += this.OnConnectionSendException;
            this.ConnectionReceiveSuccess += this.OnConnectionReceiveSuccess;
            this.ConnectionReceiveException += this.OnConnectionReceiveException;
            this.ConnectionDisconnectSuccess += this.OnConnectionDisconnectSuccess;
            this.ConnectionDisconnectException += this.OnConnectionDisconnectException;
            this.ConnectionDisposeSuccess += this.OnConnectionDisposeSuccess;
            this.ConnectionDisposeException += this.OnConnectionDisposeException;

            this.LogicGame = new ClientLogic();
        }
        private void OnConnectionAuthenticationSuccess(ushort ChannelID, uint ClientID)
        {
            Logging.WriteLine(ChannelID, ClientID, $"Setup secured connection success");
        }
        private void OnConnectionAuthenticationFailed(ushort ChannelID, uint ClientID)
        {
            Logging.WriteLine(ChannelID, ClientID, $"Fail to setup secured connection");
        }
        private void OnConnectionAuthenticationException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            Logging.WriteLine(ChannelID, ClientID, $"Setup secured connection", Exception);
        }
        private void OnConnectionStartSuccess(ushort ChannelID, uint ClientID)
        {
            Logging.WriteLine(ChannelID, ClientID, $"Initialize connection to {this.GetIPEndpoint()}");
            this.LogicGame.PacketHandler?.HandleServerHandshake(this);
        }
        private void OnConnectionStartException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            Logging.WriteLine(ChannelID, ClientID, $"Initialize connection to {this.GetIPEndpoint()}", Exception);
        }
        private void OnConnectionConnectSuccess(ushort ChannelID, uint ClientID)
        {
            Logging.WriteLine(ChannelID, ClientID, $"Connected to {this.GetIPEndpoint()}");
        }
        private void OnConnectionConnectException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            Logging.WriteLine(ChannelID, ClientID, $"Connected to {this.GetIPEndpoint()}", Exception);
        }
        private void OnConnectionSendSuccess(ushort ChannelID, uint ClientID, int DataSize)
        {
            Logging.WriteLine(ChannelID, ClientID, $"Sent to {this.GetIPEndpoint()} {DataSize} bytes");
        }
        private void OnConnectionSendException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            Logging.WriteLine(ChannelID, ClientID, $"Sent to {this.GetIPEndpoint()}", Exception);
        }
        private void OnConnectionReceiveSuccess(ushort ChannelID, uint ClientID, int DataSize, byte[] Data)
        {
            Logging.WriteLine(ChannelID, ClientID, $"Received from {this.GetIPEndpoint()} {DataSize} bytes");
            this.LogicGame.PacketHandler?.HandleServerIncomingPacket(ClientID, Data);
        }
        private void OnConnectionReceiveException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            Logging.WriteLine(ChannelID, ClientID, $"Received from {this.GetIPEndpoint()}", Exception);
        }
        private void OnConnectionDisconnectSuccess(ushort ChannelID, uint ClientID)
        {
            Logging.WriteLine(ChannelID, ClientID, $"Disconnected from {this.GetIPEndpoint()}");
            this.LogicGame.PacketHandler?.HandleServerDisconnect();
        }
        private void OnConnectionDisconnectException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            Logging.WriteLine(ChannelID, ClientID, $"Disconnected from {this.GetIPEndpoint()}", Exception);
            this.LogicGame.PacketHandler?.HandleServerDisconnect();
        }
        private void OnConnectionDisposeSuccess(ushort ChannelID, uint ClientID, string Caller)
        {
            Logging.WriteLine(ChannelID, ClientID, $"Disposed by {Caller}");
            this.LogicGame.PacketHandler?.HandleServerDisconnect();
        }
        private void OnConnectionDisposeException(ushort ChannelID, uint ClientID, string Caller, Exception Exception)
        {
            Logging.WriteLine(ChannelID, ClientID, $"Disposed by {Caller}", Exception);
            this.LogicGame.PacketHandler?.HandleServerDisconnect();
        }
    }
}
