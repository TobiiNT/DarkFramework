using DarkGamePacket.Definitions.S2C;
using DarkSecurityNetwork.Interfaces;

namespace DarkGamePacket.Interfaces
{
    public class PacketNotifier<T> : IPacketNotifier where T : ISecurityNetwork
    {
        private readonly IPacketHandlerManager<T> PacketHandlerManager;

        public PacketNotifier(IPacketHandlerManager<T> PacketHandlerManager)
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
