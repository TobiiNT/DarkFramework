using DarkGamePacket.Definitions.C2S;
using DarkGamePacket.Enums;
using DarkGamePacket.Handlers.Clients.Interfaces;

namespace DarkGamePacket.Handlers.Clients
{
    public class ClientPacketNotifier : IClientPacketNotifier
    {
        private readonly IClientPacketHandler PacketHandlerManager;

        public ClientPacketNotifier(IClientPacketHandler PacketHandlerManager)
        {
            this.PacketHandlerManager = PacketHandlerManager;
        }

        public void NotifyChatMessage(byte MessageType, string Message)
        {
            var MessageData = new C2S_ChatMessage()
            {
                MessageType = MessageType,
                Message = Message
            };
            this.PacketHandlerManager.SendPacket(PacketID.CHAT_MESSAGE, MessageData);
        }
    }
}
