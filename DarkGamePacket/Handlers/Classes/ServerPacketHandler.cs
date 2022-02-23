using DarkGamePacket.Attributes;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;
using System;
using System.Collections.Generic;
using DarkThreading;
using DarkPacket.Readers;
using DarkPacket.Writers;
using System.Linq;
using DarkPacket.Interfaces;
using DarkPacket.Handlers;
using DarkGamePacket.Handlers.Interfaces;
using DarkGamePacket.Packets;

namespace DarkGamePacket.Handlers.Classes
{
    public class ServerPacketHandler : IServerPacketHandler
    {
        private readonly ThreadSafeDictionary<uint, SecurityConnection<ServerSecurityNetwork>> ClientPlayers;


        private readonly Dictionary<ListPacketID, RequestHandle> RequestTable;
        private delegate ICoreMessage RequestHandle(byte[] data);

        private readonly Dictionary<ListPacketID, ResponseHandle> ResponseTable;
        private delegate byte[] ResponseHandle(ICoreMessage Response);

        private readonly NetworkHandler<ICoreMessage> NetworkRequest;

        public ServerPacketHandler(NetworkHandler<ICoreMessage> NetworkRequest)
        {
            this.ClientPlayers = new ThreadSafeDictionary<uint, SecurityConnection<ServerSecurityNetwork>>();
            this.NetworkRequest = NetworkRequest;
            this.RequestTable = new Dictionary<ListPacketID, RequestHandle>();
            this.ResponseTable = new Dictionary<ListPacketID, ResponseHandle>();
            this.InitializeRequestHandlers();
            this.InitializeResponseHandlers();
        }
        private void InitializeRequestHandlers()
        {
            foreach (var Method in typeof(ListPacketClient).GetMethods())
            {
                foreach (Attribute Attribute in Method.GetCustomAttributes(true))
                {
                    if (Attribute is PacketType Packet)
                    {
                        var DelegateMethod = (RequestHandle)Delegate.CreateDelegate(typeof(RequestHandle), Method);

                        this.RequestTable.Add(Packet.PacketID, DelegateMethod);
                    }
                }
            }
        }
        private void InitializeResponseHandlers()
        {
            foreach (var Method in typeof(ListPacketServer).GetMethods())
            {
                foreach (Attribute Attribute in Method.GetCustomAttributes(true))
                {
                    if (Attribute is PacketType Packet)
                    {
                        var DelegateMethod = (ResponseHandle)Delegate.CreateDelegate(typeof(ResponseHandle), Method);

                        this.ResponseTable.Add(Packet.PacketID, DelegateMethod);
                    }
                }
            }
        }
        private RequestHandle GetRequestHandle(ListPacketID PacketID)
        {
            if (this.RequestTable.ContainsKey(PacketID))
            {
                return this.RequestTable[PacketID];
            }
            return null;
        }
        private ResponseHandle GetResponseHandle(ListPacketID PacketID)
        {
            if (this.ResponseTable.ContainsKey(PacketID))
            {
                return this.ResponseTable[PacketID];
            }
            return null;
        }

        public bool SendPacket(uint ClientID, ListPacketID PacketID, ICoreMessage Response)
        {
            var ResponseHandle = GetResponseHandle(PacketID);

            if (ResponseHandle != null)
            {
                if (this.ClientPlayers.TryGetValue(ClientID, out var Connection))
                {
                    dynamic HandleResponse = ResponseHandle(Response);

                    using var Packet = new PacketWriter();
                    Packet.WriteUShort((ushort)PacketID);
                    Packet.WriteBytes(HandleResponse);

                    return Connection != null && Connection.SendDataWithEncryption(Packet.GetPacketData());
                }
            }
            return false;
        }
        public bool SendPacketBroadcast(ListPacketID PacketID, ICoreMessage Response)
        {
            var ResponseHandle = GetResponseHandle(PacketID);

            if (ResponseHandle != null)
            {
                dynamic HandleResponse = ResponseHandle(Response);

                using var Packet = new PacketWriter();
                Packet.WriteUShort((ushort)PacketID);
                Packet.WriteBytes(HandleResponse);

                foreach (var Connection in this.ClientPlayers.Values.ToList())
                {
                    Connection.SendDataWithEncryption(Packet.GetPacketData());
                }
                return true;
            }
            return false;
        }

        public bool HandlePacket(uint ClientID, byte[] Data)
        {
            using var Reader = new NormalPacketReader(Data);
            var PacketID = (ListPacketID)Reader.ReadUShort();
            var PacketData = Reader.ReadBytes();

            var Request = GetRequestHandle(PacketID);

            if (Request != null)
            {
                if (this.ClientPlayers.ContainsKey(ClientID))
                {
                    dynamic HandleRequest = Request(PacketData);

                    this.NetworkRequest.OnMessage(ClientID, HandleRequest);
                    return true;
                }
            }
            return false;
        }

        public bool HandleHandshake(uint ClientID, SecurityConnection<ServerSecurityNetwork> Connection)
        {
            if (!this.ClientPlayers.ContainsKey(ClientID))
            {
                this.ClientPlayers.Add(ClientID, Connection);
                return true;
            }
            return false;
        }

        public bool HandleDisconnect(uint ClientID)
        {
            if (this.ClientPlayers.ContainsKey(ClientID))
            {
                this.ClientPlayers.RemoveSafe(ClientID);
                return true;
            }
            return false;
        }
    }
}
