using DarkGamePacket.Attributes;
using DarkGamePacket.Definitions.S2C;
using DarkGamePacket.Enums;
using DarkGamePacket.Interfaces;

namespace DarkGamePacket.Handlers.Packets
{
    public class S2C_ClientSide
    {
        [PacketType(PacketID.CHAT_MESSAGE)]
        public static ICoreResponse ChatMessageResponse(byte[] Data) => PacketDeserializer.Deserialize<S2C_ChatMessage>(Data);
    }
}
