using DarkGamePacket.Definitions.S2C;

namespace DarkGamePacket.Interfaces
{
    public class PacketNotifier : IPacketNotifier
    {
        private readonly IPacketHandlerManager PacketHandlerManager;

        public PacketNotifier(IPacketHandlerManager PacketHandlerManager)
        {
            this.PacketHandlerManager = PacketHandlerManager;            
        }

        public void NotifyChatMessage(uint ClientID, byte MessageType, string Message)
        {
            var MessageData = new ChatMessageResponse(MessageType, Message);

            this.PacketHandlerManager.SendPacket(ClientID, MessageData);
        }
    }
}
