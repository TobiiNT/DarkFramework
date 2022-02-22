using DarkGamePacket.Attributes;
using DarkGamePacket.Definitions.S2C;
using DarkGamePacket.Enums;
using DarkGamePacket.Handlers;
using DarkGamePacket.Interfaces;

namespace DarkGamePacket.Servers.Packets
{
    public class S2C_ServeriSide
    {
        [PacketType(PacketID.CHAT_MESSAGE)]
        public static byte[] ChatMessageResponse(ICoreResponse Response) => PacketSerializer.Serialize(Response as S2C_ChatMessage);


    }
}
