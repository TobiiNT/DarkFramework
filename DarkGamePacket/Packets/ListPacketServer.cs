using DarkGamePacket.Attributes;
using DarkGamePacket.Definitions.S2C;
using DarkPacket.Interfaces;
using DarkPacket.Readers;
using DarkPacket.Writers;

namespace DarkGamePacket.Packets
{
    public class ListPacketServer
    {
        [PacketType(PacketDirection.IN, ListPacketID.CHAT_MESSAGE)]
        public static ICoreMessage ChatMessageResponse(byte[] Data) => PacketDeserializer.Deserialize<S2C_ChatMessage>(Data);
        [PacketType(PacketDirection.OUT, ListPacketID.CHAT_MESSAGE)]
        public static byte[] ChatMessageResponse(ICoreMessage Response) => PacketSerializer.Serialize(Response as S2C_ChatMessage);

    }
}
