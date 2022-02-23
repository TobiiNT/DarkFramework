using DarkGamePacket.Definitions;
using DarkGamePacket.Handlers.Interfaces;
using DarkGamePacket.Packets;
using DarkPacket.Handlers;
using DarkPacket.Interfaces;
using DarkPacket.Readers;
using DarkPacket.Writers;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;

namespace DarkGamePacket.Handlers.Classes
{
    public class ClientPacketHandler : IClientPacketHandler
    {
        private SecurityConnection<ClientSecurityNetwork> UserClient;
        private RouteHandler RouteHandler;
       
        public ClientPacketHandler(NetworkHandler<ICoreMessage> NetworkHandler)
        {
            this.RouteHandler = new RouteHandler(NetworkHandler);
            this.RouteHandler.LoadResponseHandlers(typeof(ListPacketServer));
            this.RouteHandler.LoadRequestHandlers(typeof(ListPacketClient));
        }

        public bool SendPacketToServer(PacketID PacketID, ICoreMessage Request)
        {
            var RequestHandle = this.RouteHandler.GetRequestHandle(PacketID);

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

        public bool HandleServerIncomingPacket(uint ClientID, byte[] Data)
        {
            using var Reader = new NormalPacketReader(Data);
            var PacketID = (PacketID)Reader.ReadUShort();
            var PacketData = Reader.ReadBytes();

            var ResponseHandle = this.RouteHandler.GetResponseHandle(PacketID);

            if (ResponseHandle != null)
            {
                dynamic HandleResponse = ResponseHandle(PacketData);

                this.RouteHandler.NetworkHandle.OnMessage(ClientID, HandleResponse);
                return true;
            }
            return false;
        }

        public bool HandleServerHandshake(SecurityConnection<ClientSecurityNetwork> Connection)
        {
            this.UserClient = Connection;
            return true;
        }

        public bool HandleServerDisconnect()
        {
            this.UserClient = null;
            return true;
        }

    }
}
