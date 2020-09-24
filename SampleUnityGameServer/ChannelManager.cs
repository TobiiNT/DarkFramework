using DarkGamePacket.Structs;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;
using DarkThread;
using SampleUnityGameServer.Networks;
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
        public ThreadSafeDictionary<ushort, GameChannel> Channels { set; get; }
        public ChannelManager()
        {
            this.ChannelUniqueIDFactory = new UniqueIDFactory(0);
            this.ClientUniqueIDFactory = new UniqueIDFactory(100000);

            this.Channels = new ThreadSafeDictionary<ushort, GameChannel>();

        }

        public void StartNewChannel(int Port)
        {
            try
            {
                var Channel = CreateNewChannel();

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
            if (this.Channels.TryGetValue(ChannelID, out GameChannel Channel))
            {
                Channel.Dispose();
                this.Channels.RemoveSafe(ChannelID);
                this.ChannelUniqueIDFactory.Release(ChannelID);
            }
        }

        public GameChannel CreateNewChannel()
        {
            var Channel = new GameChannel((ushort)ChannelUniqueIDFactory.GetNext());

            Channel.ServerAcceptSuccess += OnServerAcceptSuccess;
            Channel.ServerAcceptException += OnServerAcceptException;
            Channel.ServerListenSuccess += OnServerListenSuccess;
            Channel.ServerListenException += OnServerListenException;
            Channel.ServerDisposeSuccess += OnServerDisposeSuccess;
            Channel.ServerDisposeException += OnServerDisposeException;

            return Channel;
        }

        

        private void OnServerAcceptSuccess(ushort ChannelID, Socket Socket)
        {
            if (this.Channels.TryGetValue(ChannelID, out GameChannel Channel))
            {
                try
                {
                    if (Channel.CreateNewConnection((uint)ClientUniqueIDFactory.GetNext(), Socket))
                    {
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
            if (this.Channels.TryGetValue(ChannelID, out GameChannel Channel))
            {
                Logging.WriteError($"Channel {Channel.ChannelID} : Accepting socket has an exception", Exception);
            }
        }

        private void OnServerListenSuccess(ushort ChannelID, int Port)
        {
            if (this.Channels.TryGetValue(ChannelID, out GameChannel Channel))
            {
                Logging.WriteError($"Channel {Channel.ChannelID} : Start listening to connection at port {Port}");
            }
        }
        private void OnServerListenException(ushort ChannelID, int Port, Exception Exception)
        {
            if (this.Channels.TryGetValue(ChannelID, out GameChannel Channel))
            {
                Logging.WriteError($"Channel {Channel.ChannelID} : Listening to connection at port {Port} has an exception", Exception);

                this.RemoveChannel(ChannelID);
            }
        }


        private void OnServerDisposeSuccess(ushort ChannelID, string Caller)
        {
            if (this.Channels.TryGetValue(ChannelID, out GameChannel Channel))
            {
                Logging.WriteError($"Channel {Channel.ChannelID} : Server socket has been disposed by function {Caller}");

                foreach (var Connection in Channel.ClientConnections.Values.ToList())
                {
                    Connection.Dispose();

                    Channel.ClientConnections.RemoveSafe(Connection.ClientID);
                }

                this.RemoveChannel(ChannelID);
            }
        }
        private void OnServerDisposeException(ushort ChannelID, string Caller, Exception Exception)
        {
            if (this.Channels.TryGetValue(ChannelID, out GameChannel Channel))
            {
                Logging.WriteError($"Channel {Channel.ChannelID} : Server socket disposing by function {Caller} has an exception", Exception);

                this.RemoveChannel(ChannelID);
            }
        }
    }
}
