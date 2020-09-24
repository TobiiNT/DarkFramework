﻿using DarkSecurity.Enums;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;
using System;

namespace SampleUnityGameClient
{
    public class ClientManager : SecurityConnection<ClientSecurityNetwork>
    {
        public ClientManager() 
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
            this.ConnectionDisposeSuccess += this.OnConnectionDisposeSuccess;
            this.ConnectionDisposeException += this.OnConnectionDisposeException;
        }

        private void OnConnectionAuthenticationSuccess(ushort ChannelID, uint ClientID)
        {
            Logging.WriteLine($"Channel {ChannelID}, Client {ClientID} : Setup secure connection success");
        }
        private void OnConnectionAuthenticationFailed(ushort ChannelID, uint ClientID)
        {
            Logging.WriteError($"Channel {ChannelID}, Client {ClientID} : Fail to setup secure connection");
        }
        private void OnConnectionAuthenticationException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            Logging.WriteError($"Channel {ChannelID}, Client {ClientID} : Setup secure connection exception", Exception);
        }
        private void OnConnectionStartSuccess(ushort ChannelID, uint ClientID)
        {
            Logging.WriteLine($"Channel {ChannelID}, Client {ClientID} : Started to {IPEndPoint}");
        }
        private void OnConnectionStartException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            Logging.WriteError($"Channel {ChannelID}, Client {ClientID} : Started to {IPEndPoint}", Exception);
        }
        private void OnConnectionConnectSuccess(ushort ChannelID, uint ClientID)
        {
            Logging.WriteLine($"Channel {ChannelID}, Client {ClientID} : Connected to {IPEndPoint}");
        }
        private void OnConnectionConnectException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            Logging.WriteError($"Channel {ChannelID}, Client {ClientID} : Connected to {IPEndPoint} exception", Exception);
        }
        private void OnConnectionSendSuccess(ushort ChannelID, uint ClientID, int DataSize)
        {
            Logging.WriteLine($"Channel {ChannelID}, Client {ClientID} : Send to {IPEndPoint} {DataSize} bytes");
        }
        private void OnConnectionSendException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            Logging.WriteError($"Channel {ChannelID}, Client {ClientID} : Send to {IPEndPoint} exception", Exception);
        }
        private void OnConnectionReceiveSuccess(ushort ChannelID, uint ClientID, int DataSize, byte[] Data)
        {
            Logging.WriteLine($"Channel {ChannelID}, Client {ClientID} : Receive from {IPEndPoint} {DataSize} bytes");
        }
        private void OnConnectionReceiveException(ushort ChannelID, uint ClientID, Exception Exception)
        {
            Logging.WriteError($"Channel {ChannelID}, Client {ClientID} : Receive from {IPEndPoint}", Exception);
        }
        private void OnConnectionDisposeSuccess(ushort ChannelID, uint ClientID, string Caller)
        {
            Logging.WriteLine($"Channel {ChannelID}, Client {ClientID} : Disposed by {Caller}");
        }
        private void OnConnectionDisposeException(ushort ChannelID, uint ClientID, string Caller, Exception Exception)
        {
            Logging.WriteError($"Channel {ChannelID}, Client {ClientID} : Disposed by {Caller} exception", Exception);
        }
    }
}
