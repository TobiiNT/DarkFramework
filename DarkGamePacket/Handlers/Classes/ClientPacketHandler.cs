using DarkGamePacket.Attributes;
using DarkGamePacket.Handlers.Interfaces;
using DarkGamePacket.Packets;
using DarkPacket.Handlers;
using DarkPacket.Interfaces;
using DarkPacket.Readers;
using DarkPacket.Writers;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;
using System;
using System.Collections.Generic;

namespace DarkGamePacket.Handlers.Classes
{
    public class ClientPacketHandler : IClientPacketHandler
    {
        private SecurityConnection<ClientSecurityNetwork> UserClient;


        private readonly Dictionary<ListPacketID, RequestHandle> RequestTable;
        private delegate byte[] RequestHandle(ICoreMessage Response);

        private readonly Dictionary<ListPacketID, ResponseHandle> ResponseTable;
        private delegate ICoreMessage ResponseHandle(byte[] data);

        private readonly NetworkHandler<ICoreMessage> NetworkResponse;

        public ClientPacketHandler(NetworkHandler<ICoreMessage> NetworkResponse)
        {
            this.NetworkResponse = NetworkResponse;
            this.RequestTable = new Dictionary<ListPacketID, RequestHandle>();
            this.ResponseTable = new Dictionary<ListPacketID, ResponseHandle>();
            this.InitializeResponseHandlers();
            this.InitializeRequestHandlers();
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

        public bool SendPacket(ListPacketID PacketID, ICoreMessage Request)
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
        public bool SendPacket(uint ClientID, ListPacketID PacketID, ICoreMessage Request)
        {
            throw new NotImplementedException();
        }

        public bool HandlePacket(uint ClientID, byte[] Data)
        {
            using var Reader = new NormalPacketReader(Data);
            var PacketID = (ListPacketID)Reader.ReadUShort();
            var PacketData = Reader.ReadBytes();

            var ResponseHandle = GetResponseHandle(PacketID);

            if (ResponseHandle != null)
            {
                dynamic HandleResponse = ResponseHandle(PacketData);

                this.NetworkResponse.OnMessage(ClientID, HandleResponse);
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
