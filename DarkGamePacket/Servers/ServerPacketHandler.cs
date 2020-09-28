using DarkGamePacket.Attributes;
using DarkGamePacket.Definitions;
using DarkGamePacket.Enums;
using DarkGamePacket.Interfaces;
using DarkGamePacket.Servers.Interfaces;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Interfaces;
using DarkSecurityNetwork.Networks;
using System;
using System.Collections.Generic;
using DarkGamePacket.Servers.Packets;
using DarkThreading;

namespace DarkGamePacket.Servers
{
    public class ServerPacketHandler : IServerPacketHandler
    {
        private readonly ThreadSafeDictionary<uint, SecurityConnection<ServerSecurityNetwork>> ClientPlayers;


        private readonly Dictionary<PacketID, RequestHandle> RequestTable;        
        private delegate ICoreRequest RequestHandle(byte[] data);

        private readonly Dictionary<PacketID, ResponseHandle> ResponseTable;
        private delegate byte[] ResponseHandle(ICoreResponse Response);

        private readonly NetworkHandler<ICoreRequest> NetworkRequest;

        public ServerPacketHandler(NetworkHandler<ICoreRequest> NetworkRequest)
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
            foreach (var Method in typeof(ServerRequests).GetMethods())
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
            foreach (var Method in typeof(ServerResponses).GetMethods())
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
                if (this.ClientPlayers.TryGetValue(ClientID, out SecurityConnection<ServerSecurityNetwork> Connection))
                {
                    dynamic HandleResponse = ResponseHandle(Response);

                    return Connection.SendDataWithEncryption(HandleResponse);
                }
            }
            return false;
        }
        public bool HandlePacket(uint ClientID, byte[] Data)
        {
            PacketID PacketID = ServerRequests.ProtoDeserialize<ICoreMessage>(Data).PacketID;
            var Request = GetRequestHandle(PacketID);

            if (Request != null)
            {
                if (this.ClientPlayers.ContainsKey(ClientID))
                {
                    dynamic HandleRequest = Request(Data);

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
