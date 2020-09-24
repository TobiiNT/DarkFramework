using DarkGamePacket.Structs;

namespace DarkGamePacket.Definitions.S2C
{
    public class ChatMessageResponse : BasePacket
    {
        public ChatMessageResponse(byte MessageType, string Message) : base(Channel.S2C, PacketID.CHAT_MESSAGE)
        {
            WriteByte(MessageType);
			WriteString(Message);
        }
    }
}