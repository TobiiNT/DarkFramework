using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;
using DarkThreading;
using DarkPacket.Readers;
using DarkPacket.Writers;
using System.Linq;
using DarkPacket.Interfaces;
using DarkPacket.Handlers;
using DarkGamePacket.Handlers.Interfaces;
using DarkGamePacket.Packets;
using DarkGamePacket.Definitions;

namespace DarkGamePacket.Handlers.Classes
{
    public class ServerPacketHandler : IServerPacketHandler
    {
        private readonly ThreadSafeDictionary<uint, SecurityConnection<ServerSecurityNetwork>> UserClients;
        private RouteHandler RouteHandler;

        public ServerPacketHandler(NetworkHandler<ICoreMessage> NetworkHandler)
        {          
            this.UserClients = new ThreadSafeDictionary<uint, SecurityConnection<ServerSecurityNetwork>>();
            this.RouteHandler = new RouteHandler(NetworkHandler);
            this.RouteHandler.LoadResponseHandlers(typeof(ListPacketClient));
            this.RouteHandler.LoadRequestHandlers(typeof(ListPacketServer));
        }

        public bool SendPacketToClient(uint ClientID, PacketID PacketID, ICoreMessage Response)
        {
            var ResponseHandle = this.RouteHandler.GetRequestHandle(PacketID);

            if (ResponseHandle != null)
            {
                if (this.UserClients.TryGetValue(ClientID, out var Connection))
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
        public bool SendPacketBroadcast(PacketID PacketID, ICoreMessage Response)
        {
            var ResponseHandle = this.RouteHandler.GetRequestHandle(PacketID);

            if (ResponseHandle != null)
            {
                dynamic HandleResponse = ResponseHandle(Response);

                using var Packet = new PacketWriter();
                Packet.WriteUShort((ushort)PacketID);
                Packet.WriteBytes(HandleResponse);

                foreach (var Connection in this.UserClients.Values.ToList())
                {
                    Connection.SendDataWithEncryption(Packet.GetPacketData());
                }
                return true;
            }
            return false;
        }

        public bool HandleClientIncomingPacket(uint ClientID, byte[] Data)
        {
            using var Reader = new NormalPacketReader(Data);
            var PacketID = (PacketID)Reader.ReadUShort();
            var PacketData = Reader.ReadBytes();

            var Request = this.RouteHandler.GetResponseHandle(PacketID);

            if (Request != null)
            {
                if (this.UserClients.ContainsKey(ClientID))
                {
                    dynamic HandleRequest = Request(PacketData);

                    this.RouteHandler.NetworkHandle.OnMessage(ClientID, HandleRequest);
                    return true;
                }
            }
            return false;
        }

        public bool HandleClientHandshake(uint ClientID, SecurityConnection<ServerSecurityNetwork> Connection)
        {
            if (!this.UserClients.ContainsKey(ClientID))
            {
                this.UserClients.Add(ClientID, Connection);
                return true;
            }
            return false;
        }

        public bool HandleClientDisconnect(uint ClientID)
        {
            if (this.UserClients.ContainsKey(ClientID))
            {
                this.UserClients.RemoveSafe(ClientID);
                return true;
            }
            return false;
        }
    }
}
