﻿using DarkNetwork.Networks.Connections;
using DarkNetwork.Networks.Connections.Events.Arguments;
using DarkSecurityNetwork.Delegates.Connections;
using DarkSecurityNetwork.Events.Arguments;
using DarkSecurityNetwork.Interfaces;
using System;
using System.Net.Sockets;

namespace DarkSecurityNetwork
{
    public class SecurityConnection<A> : ConnectionBase
                               where A : ISecurityNetwork
    {
        public ushort ChannelID { set; get; }
        public uint ClientID { set; get; }
        public ISecurityNetwork SecurityNetwork { private set; get; }
        public SecurityConnection()
        {
            this.EventStartSuccess += this.EventConnectionStartSuccess;
            this.EventStartException += this.EventConnectionStartException;
            this.EventConnectSuccess += this.EventConnectionConnectSuccess;
            this.EventConnectException += this.EventConnectionConnectException;
            this.EventSendSuccess += this.EventConnectionSendSuccess;
            this.EventSendException += this.EventConnectionSendException;
            this.EventReceiveSuccess += this.EventConnectionReceiveSuccess;
            this.EventReceiveException += this.EventConnectionReceiveException;
            this.EventDisposeSuccess += this.EventConnectionDisposeSuccess;
            this.EventDisposeException += this.EventConnectionDisposeException;

            this.SecurityNetwork = (ISecurityNetwork)Activator.CreateInstance(typeof(A));
            this.SecurityNetwork.EventSendData += EventAuthenticationSend;
            this.SecurityNetwork.EventAuthSuccess += EventAuthenticationSuccess;
            this.SecurityNetwork.EventAuthFailed += EventAuthenticationFailed;
            this.SecurityNetwork.EventAuthException += EventAuthenticationException;
        }

        public void ConnectWithSocket(Socket Socket)
        {
            this.Start(Socket);

            while (true)
            {
                if (this.IsRunning)
                {
                    this.SecurityNetwork.SendAsymmetricPublicKeyToClient();
                    break;
                }
            }
        }

        public void ConnectWithIP(string IPAddress, int Port) => this.Start(IPAddress, Port);

        public void SendDataWithEncryption(byte[] Data)
        {
            if (this.SecurityNetwork != null && this.SecurityNetwork.AuthenticationSuccess)
            {
                this.SecurityNetwork.EncryptDataWithSymmetricAlgorithm(ref Data);

                this.Send(Data);
            }
            else throw new Exception("Security protocol is not initialized");
        }
        
        public void EventAuthenticationSend(object Sender, EventArgs Arguments)
        {
            if (Arguments is SendDataArgs SendDataArgs)
            {
                this.Send(SendDataArgs.Data);
            }
        }
        public void EventAuthenticationSuccess(object Sender, EventArgs Arguments)
        {
            this.OnAuthenticationSuccess();
        }
        public void EventAuthenticationFailed(object Sender, EventArgs Arguments)
        {
            this.OnAuthenticationFailed();

            this.Dispose();
        }
        public void EventAuthenticationException(object Sender, EventArgs Arguments)
        {
            if (Arguments is AuthExceptionArgs Args)
            {
                this.OnAuthenticationException(Args.Exception);

                this.Dispose();
            }
        }

        public void EventConnectionStartSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is StartSuccessArgs)
            {
                this.OnConnectionStartSuccess();
            }
        }
        public void EventConnectionStartException(object Sender, EventArgs Arguments)
        {
            if (Arguments is StartExceptionArgs Args)
            {
                this.OnConnectionStartException(Args.Exception);
            }
        }

        public void EventConnectionConnectSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is ConnectSuccessArgs)
            {
                this.OnConnectionConnectSuccess();
            }
        }
        public void EventConnectionConnectException(object Sender, EventArgs Arguments)
        {
            if (Arguments is ConnectExceptionArgs Args)
            {
                this.OnConnectionConnectException(Args.Exception);
            }
        }

        public void EventConnectionSendSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is SendSuccessArgs Args)
            {
                this.OnConnectionSendSuccess(Args.DataSize);
            }
        }
        public void EventConnectionSendException(object Sender, EventArgs Arguments)
        {
            if (Arguments is SendExceptionArgs Args)
            {
                this.OnConnectionSendException(Args.Exception);
            }
        }

        public void EventConnectionReceiveSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is ReceiveSuccessArgs Args)
            {
                if (this.SecurityNetwork.AuthenticationSuccess)
                {
                    this.SecurityNetwork.DecryptDataWithSymmetricAlgorithm(ref Args.PacketData);

                    this.OnConnectionReceiveSuccess(Args.DataSize, Args.PacketData);
                }
                else
                {
                    this.SecurityNetwork.ManagePacket(Args.PacketData);
                }
            }
        }
        public void EventConnectionReceiveException(object Sender, EventArgs Arguments)
        {
            if (Arguments is ReceiveExceptionArgs Args)
            {
                this.OnConnectionReceiveException(Args.Exception);
            }
        }

        public void EventConnectionDisposeSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is DisposeSuccessArgs Args)
            {
                this.OnConnectionDisposeSuccess(Args.Caller);
            }
        }
        public void EventConnectionDisposeException(object Sender, EventArgs Arguments)
        {
            if (Arguments is DisposeExceptionArgs Args)
            {
                this.OnConnectionDisposeException(Args.Caller, Args.Exception);
            }
        }

        #region Delegates
        public event AuthenticationSuccess AuthenticationSuccess;
        public event AuthenticationFailed AuthenticationFailed;
        public event AuthenticationException AuthenticationException;

        public event ConnectionStartSuccess ConnectionStartSuccess;
        public event ConnectionStartException ConnectionStartException;

        public event ConnectionConnectSuccess ConnectionConnectSuccess;
        public event ConnectionConnectException ConnectionConnectException;

        public event ConnectionSendSuccess ConnectionSendSuccess;
        public event ConnectionSendException ConnectionSendException;

        public event ConnectionReceiveSuccess ConnectionReceiveSuccess;
        public event ConnectionReceiveException ConnectionReceiveException;

        public event ConnectionDisposeSuccess ConnectionDisposeSuccess;
        public event ConnectionDisposeException ConnectionDisposeException;

        public void OnAuthenticationSuccess() => AuthenticationSuccess?.Invoke(ChannelID, ClientID);
        public void OnAuthenticationFailed() => AuthenticationFailed?.Invoke(ChannelID, ClientID);
        public void OnAuthenticationException(Exception Exception) => AuthenticationException?.Invoke(ChannelID, ClientID, Exception);

        public void OnConnectionStartSuccess() => ConnectionStartSuccess?.Invoke(ChannelID, ClientID);
        public void OnConnectionStartException(Exception Exception) => ConnectionStartException?.Invoke(ChannelID, ClientID, Exception);

        public void OnConnectionConnectSuccess() => ConnectionConnectSuccess?.Invoke(ChannelID, ClientID);
        public void OnConnectionConnectException(Exception Exception) => ConnectionConnectException?.Invoke(ChannelID, ClientID, Exception);

        public void OnConnectionSendSuccess(int DataSize) => ConnectionSendSuccess?.Invoke(ChannelID, ClientID, DataSize);
        public void OnConnectionSendException(Exception Exception) => ConnectionSendException?.Invoke(ChannelID, ClientID, Exception);

        public void OnConnectionReceiveSuccess(int DataSize, byte[] Data) => ConnectionReceiveSuccess?.Invoke(ChannelID, ClientID, DataSize, Data);
        public void OnConnectionReceiveException(Exception Exception) => ConnectionReceiveException?.Invoke(ChannelID, ClientID, Exception);

        public void OnConnectionDisposeSuccess(string Caller) => ConnectionDisposeSuccess?.Invoke(ChannelID, ClientID, Caller);
        public void OnConnectionDisposeException(string Caller, Exception Exception) => ConnectionDisposeException?.Invoke(ChannelID, ClientID, Caller, Exception);
        #endregion
    }
}