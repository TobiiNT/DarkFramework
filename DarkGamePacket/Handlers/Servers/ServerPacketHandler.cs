using DarkGamePacket.Attributes;
using DarkGamePacket.Definitions;
using DarkGamePacket.Enums;
using DarkGamePacket.Interfaces;
using DarkGamePacket.Servers.Interfaces;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;
using System;
using System.Collections.Generic;
using DarkGamePacket.Servers.Packets;
using DarkThreading;
using DarkPacket.Readers;
using DarkPacket.Writer;
using System.Linq;

namespace DarkGamePacket.Servers
{
    public class ServerPacketHandler : IServerPacketHandler
    {
        private readonly ThreadSafeDictionary<uint, SecurityConnection<ServerSecurityNetwork>> ClientPlayers;


        private readonly Dictionary<PacketID, RequestHandle> RequestTable;        
        private delegate ICoreRequest RequestHandle(byte[] data);

        private readonly Dictionary<PacketID, ResponseHandle> ResponseTable;
        private delegate byte[] ResponseHandle(ICoreResponse Response);

        private readonly ServerNetworkHandler<ICoreRequest> NetworkRequest;

        public ServerPacketHandler(ServerNetworkHandler<ICoreRequest> NetworkRequest)
        {
            this.ClientPlayers = new ThreadSafeDictionary<uint, SecurityConnection<ServerSecurityNetwork>>();
            this.NetworkRequest = NetworkRequest;
            this.RequestTable = new Dictionary<PacketID, RequestHandle>();
            this.ResponseTable = new Dictionary<PacketID, ResponseHandle>();
            this.InitializeRequestHandlers();
            this.InitializeResponseHandlers();
        }
        private void InitializeRequestHandlers()
        {
            foreach (var Method in typeof(C2S_ServerSide).GetMethods())
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
            foreach (var Method in typeof(S2C_ServeriSide).GetMethods())
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
        private RequestHandle GetRequestHandle(PacketID PacketID)
        {
            if (this.RequestTable.ContainsKey(PacketID))
            {
                return this.RequestTable[PacketID];
            }
            return null;
        }
        private ResponseHandle GetResponseHandle(PacketID PacketID)
        {
            if (this.ResponseTable.ContainsKey(PacketID))
            {
                return this.ResponseTable[PacketID];
            }
            return null;
        }
        public bool SendPacket(uint ClientID, PacketID PacketID, ICoreResponse Response)
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
        public bool SendPacketBroadcast(PacketID PacketID, ICoreResponse Response)
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
            var PacketID = (PacketID)Reader.ReadUShort();
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
