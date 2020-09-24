using DarkGamePacket.Definitions;
using DarkGamePacket.Interfaces;
using DarkPacket.Readers;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Interfaces;
using DarkSecurityNetwork.Networks;
using DarkThread;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DarkGamePacket.Structs
{
    public class PacketHandlerManager<T> : IPacketHandlerManager where T : ISecurityNetwork
    {
        private readonly ThreadSafeDictionary<uint, SecurityConnection<T>> ClientPlayers;

        private delegate ICoreRequest RequestConvertor(byte[] data);
        private readonly Dictionary<Tuple<Channel, PacketID>, RequestConvertor> ConvertorTable;
        

        private readonly NetworkHandler<ICoreRequest> NetworkRequest;
        private readonly NetworkHandler<ICoreResponse> NetworkResponse;
        
        public PacketHandlerManager(NetworkHandler<ICoreRequest> NetworkRequest, NetworkHandler<ICoreResponse> NetworkResponse)
        {
            this.ClientPlayers = new ThreadSafeDictionary<uint, SecurityConnection<T>>();
            this.NetworkRequest = NetworkRequest;
            this.NetworkResponse = NetworkResponse;
            this.ConvertorTable = new Dictionary<Tuple<Channel, PacketID>, RequestConvertor>();
            InitializePacketConvertors();
        }
        internal void InitializePacketConvertors()
        {
            foreach (var Method in typeof(PacketExtractor).GetMethods())
            {
                foreach (Attribute Attribute in Method.GetCustomAttributes(true))
                {
                    if (Attribute is PacketType Packet)
                    {
                        var Key = new Tuple<Channel, PacketID>(Packet.Channel, Packet.PacketID);
                        var DelegateMethod = (RequestConvertor)Delegate.CreateDelegate(typeof(RequestConvertor), Method);
                        this.ConvertorTable.Add(Key, DelegateMethod);
                    }
                }
            }
        }
        private RequestConvertor GetConvertor(Channel Channel, PacketID PacketID)
        {
            var Key = new Tuple<Channel, PacketID>(Channel, PacketID);
            if (this.ConvertorTable.ContainsKey(Key))
            {
                return this.ConvertorTable[Key];
            }

            return null;
        }
        public bool SendPacket(uint ClientID, Packet Packet)
        {
            return SendPacket(ClientID, Packet.GetPacketData());
        }
        public bool SendPacket(uint ClientID, byte[] Data)
        {
            if (this.ClientPlayers.TryGetValue(ClientID, out SecurityConnection<T> Connection))
            {
                return Connection.SendDataWithEncryption(Data);
            }
            return false;
        }
        public bool HandlePacket(uint ClientID, byte[] Data)
        {
            var Header = new PacketHeader(Data);
            var Convertor = GetConvertor(Header.Channel, Header.PacketID);

            if (Convertor != null)
            {
                if (this.ClientPlayers.ContainsKey(ClientID))
                {
                    dynamic Request = Convertor(Data);

                    this.NetworkRequest.OnMessage(ClientID, Request);
                    return true;
                }
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
