using DarkGamePacket.Attributes;
using DarkGamePacket.Enums;
using DarkGamePacket.Handlers.Clients.Interfaces;
using DarkGamePacket.Handlers.Packets;
using DarkGamePacket.Interfaces;
using DarkPacket.Readers;
using DarkPacket.Writer;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;
using System;
using System.Collections.Generic;

namespace DarkGamePacket.Handlers.Clients
{
    public class ClientPacketHandler : IClientPacketHandler
    {
        private SecurityConnection<ClientSecurityNetwork> UserClient;


        private readonly Dictionary<PacketID, RequestHandle> RequestTable;
        private delegate byte[] RequestHandle(ICoreRequest Response);

        private readonly Dictionary<PacketID, ResponseHandle> ResponseTable;
        private delegate ICoreResponse ResponseHandle(byte[] data);

        private readonly ClientNetworkHandler<ICoreResponse> NetworkResponse;

        public ClientPacketHandler(ClientNetworkHandler<ICoreResponse> NetworkResponse)
        {
            this.NetworkResponse = NetworkResponse;
            this.RequestTable = new Dictionary<PacketID, RequestHandle>();
            this.ResponseTable = new Dictionary<PacketID, ResponseHandle>();
            this.InitializeResponseHandlers();
            this.InitializeRequestHandlers();
        }
        private void InitializeResponseHandlers()
        {
            foreach (var Method in typeof(S2C_ClientSide).GetMethods())
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
        private void InitializeRequestHandlers()
        {
            foreach (var Method in typeof(C2S_ClientSide).GetMethods())
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
        public bool SendPacket(PacketID PacketID, ICoreRequest Request)
        {
            var RequestHandle = GetRequestHandle(PacketID);

            if (RequestHandle != null)
            {
                dynamic HandleRequest = RequestHandle(Request);

                using var Packet = new PacketWriter();
                Packet.WriteUShort((ushort)PacketID);
                Packet.WriteBytes(HandleRequest);

                return this.UserClient != null && this.UserClient.SendDataWithEncryption(Packet.GetPacketData());
            }
            return false;
        }
        public bool HandlePacket(byte[] Data)
        {
            using var Reader = new NormalPacketReader(Data);
            var PacketID = (PacketID)Reader.ReadUShort();
            var PacketData = Reader.ReadBytes();
       
            var ResponseHandle = GetResponseHandle(PacketID);

            if (ResponseHandle != null)
            {
                dynamic HandleResponse = ResponseHandle(PacketData);

                this.NetworkResponse.OnMessage(HandleResponse);
                return true;
            }
            return false;
        }
        public bool HandleHandshake(SecurityConnection<ClientSecurityNetwork> Connection)
        {
            this.UserClient = Connection;
            return true;
        }
        public bool HandleDisconnect()
        {
            this.UserClient = null;
            return true;
        }
    }
}
