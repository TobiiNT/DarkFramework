using DarkGamePacket.Definitions.S2C;
using DarkGamePacket.Enums;
using DarkGamePacket.Servers.Interfaces;

namespace DarkGamePacket.Servers
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
                MessageType = MessageType,
                Message = Message
            };
            //this.PacketHandlerManager.SendPacket(ClientID, PacketID.CHAT_MESSAGE, MessageData);
            this.PacketHandler.SendPacketBroadcast(PacketID.CHAT_MESSAGE, MessageData);
        }
    }
}
