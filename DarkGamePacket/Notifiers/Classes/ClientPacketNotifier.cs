using DarkGamePacket.Definitions;
using DarkGamePacket.Definitions.C2S;
using DarkGamePacket.Handlers.Interfaces;
using DarkGamePacket.Notifiers.Interfaces;
using DarkGamePacket.Packets;

namespace DarkGamePacket.Notifiers.Classes
{
    public class ClientPacketNotifier : IClientPacketNotifier
    {
        private readonly IClientPacketHandler PacketHandlerManager;

        public ClientPacketNotifier(IClientPacketHandler PacketHandlerManager)
        {
            this.PacketHandlerManager = PacketHandlerManager;
        }

        public void NotifyChatMessage(uint ClientID, byte MessageType, string Message)
        {
            var MessageData = new C2S_ChatMessage()
            {
                ClientID = ClientID,
                MessageType = MessageType,
                Message = Message
            };
            this.PacketHandlerManager.SendPacketToServer(PacketID.CHAT_MESSAGE, MessageData);
        }
    }
}
