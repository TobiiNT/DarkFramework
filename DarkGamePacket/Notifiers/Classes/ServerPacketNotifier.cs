using DarkGamePacket.Definitions;
using DarkGamePacket.Definitions.S2C;
using DarkGamePacket.Handlers.Interfaces;
using DarkGamePacket.Notifiers.Interfaces;
using DarkGamePacket.Packets;

namespace DarkGamePacket.Notifiers.Classes
{
    public class ServerPacketNotifier : IServerPacketNotifier
    {
        private readonly IServerPacketHandler PacketHandler;

        public ServerPacketNotifier(IServerPacketHandler PacketHandlerManager)
        {
            this.PacketHandler = PacketHandlerManager;
        }

        public void NotifyChatMessage(uint ClientID, byte MessageType, string Message)
        {
            var MessageData = new S2C_ChatMessage()
            {
                ClientID = ClientID,
                MessageType = MessageType,
                Message = Message
            };
            //this.PacketHandlerManager.SendPacket(ClientID, PacketID.CHAT_MESSAGE, MessageData);
            this.PacketHandler.SendPacketBroadcast(PacketID.CHAT_MESSAGE, MessageData);
        }
    }
}
