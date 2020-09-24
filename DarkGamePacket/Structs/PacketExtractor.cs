using DarkGamePacket.Definitions;
using DarkGamePacket.Definitions.C2S;

namespace DarkGamePacket
{
    public class PacketExtractor
    {
        [PacketType(Channel.C2S, PacketID.CHAT_MESSAGE)]
        public static ChatMessageRequest ReadChatMessageRequest(byte[] Data)
        {
            return new ChatMessageRequest(Data);
        }
    }
}
