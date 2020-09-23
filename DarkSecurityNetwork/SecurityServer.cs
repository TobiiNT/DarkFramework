﻿using DarkNetwork.Networks.Connections;
using DarkNetwork.Networks.Connections.Events.Arguments;
using DarkSecurityNetwork.Delegates.Servers;
using DarkSecurityNetwork.Networks;
using DarkThread;
using System;
using System.Net.Sockets;

namespace DarkSecurityNetwork
{
    public class SecurityServer : SocketBase
    {
        public ushort ChannelID { set; get; }
        public ThreadSafeDictionary<uint, SecurityConnection<ServerSecurityNetwork>> Connections { set; get; }

        public SecurityServer()
        {
            this.Connections = new ThreadSafeDictionary<uint, SecurityConnection<ServerSecurityNetwork>>();

            this.EventListenSuccess += this.OnListenSuccess;
            this.EventListenException += this.OnListenException;
            this.EventAcceptSuccess += this.OnAcceptSuccess;
            this.EventAcceptException += this.OnAcceptException;
            this.EventDisposeSuccess += this.OnDisposeSuccess;
            this.EventDisposeException += this.OnDisposeException;
        }

        private void OnListenSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is ListenSuccessArgs Args)
            {
                this.OnServerListenSuccess(Args.Port);
            }
        }
        private void OnListenException(object Sender, EventArgs Arguments)
        {
            if (Arguments is ListenExceptionArgs Args)
            {
                this.OnServerListenException(Args.Port, Args.Exception);
            }
        }

        private void OnAcceptSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is AcceptSuccessArgs Args)
            {
                this.OnServerAcceptSuccess(Args.Socket);
            }
        }

        private void OnAcceptException(object Sender, EventArgs Arguments)
        {
            if (Arguments is AcceptExceptionArgs Args)
            {
                this.OnServerAcceptException(Args.Exception);
            }
        }

        private void OnDisposeSuccess(object Sender, EventArgs Arguments)
        {
            if (Arguments is DisposeSuccessArgs Args)
            {
                this.OnServerDisposeSuccess(Args.Caller);
            }
        }

        private void OnDisposeException(object Sender, EventArgs Arguments)
        {
            if (Arguments is DisposeExceptionArgs Args)
            {
                this.OnServerDisposeException(Args.Caller, Args.Exception);
            }
        }

        #region Delegates
        public event ServerListenSuccess ServerListenSuccess;
        public event ServerListenException ServerListenException;

        public event ServerAcceptSuccess ServerAcceptSuccess;
        public event ServerAcceptException ServerAcceptException;

        public event ServerDisposeSuccess ServerDisposeSuccess;
        public event ServerDisposeException ServerDisposeException;

        public void OnServerListenSuccess(int Port) => ServerListenSuccess?.Invoke(ChannelID, Port);
        public void OnServerListenException(int Port, Exception Exception) => ServerListenException?.Invoke(ChannelID, Port, Exception);

        public void OnServerAcceptSuccess(Socket Socket) => ServerAcceptSuccess?.Invoke(ChannelID, Socket);
        public void OnServerAcceptException(Exception Exception) => ServerAcceptException?.Invoke(ChannelID, Exception);

        public void OnServerDisposeSuccess(string Caller) => ServerDisposeSuccess?.Invoke(ChannelID, Caller);
        public void OnServerDisposeException(string Caller, Exception Exception) => ServerDisposeException?.Invoke(ChannelID, Caller, Exception);
        #endregion
    }
}