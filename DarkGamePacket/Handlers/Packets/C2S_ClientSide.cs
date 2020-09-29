using DarkGamePacket.Attributes;
using DarkGamePacket.Definitions.C2S;
using DarkGamePacket.Enums;
using DarkGamePacket.Interfaces;

namespace DarkGamePacket.Handlers.Packets
{
    public class C2S_ClientSide
    {
        [PacketType(PacketID.CHAT_MESSAGE)]
        public static byte[] ChatMessageRequest(ICoreRequest Response) => PacketSerializer.Serialize(Response as C2S_ChatMessage);
    }
}
