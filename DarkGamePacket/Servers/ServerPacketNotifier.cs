using DarkGamePacket.Definitions;
using DarkGamePacket.Definitions.S2C;
using DarkGamePacket.Enums;
using DarkGamePacket.Interfaces;
using DarkGamePacket.Servers.Interfaces;
using DarkSecurityNetwork.Interfaces;

namespace DarkGamePacket.Servers
{
    public class ServerPacketNotifier : IServerPacketNotifier 
    {
        private readonly IServerPacketHandler PacketHandlerManager;

        public ServerPacketNotifier(IServerPacketHandler PacketHandlerManager)
        {
            this.PacketHandlerManager = PacketHandlerManager;            
        }

        public void NotifyChatMessage(uint ClientID, byte MessageType, string Message)
        {
            var MessageData = new ChatMessageResponse()
            {
                MessageType = MessageType,
                Message = Message
            };
            this.PacketHandlerManager.SendPacket(ClientID, PacketID.CHAT_MESSAGE, MessageData);
        }
    }
}
