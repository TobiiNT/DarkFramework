using DarkGamePacket.Attributes;
using DarkGamePacket.Definitions.C2S;
using DarkGamePacket.Enums;
using DarkGamePacket.Handlers;

namespace DarkGamePacket.Servers.Packets
{
    public class C2S_ServerSide
    {
        [PacketType(PacketID.CHAT_MESSAGE)]
        public static C2S_ChatMessage ChatMessageRequest(byte[] Data) => PacketDeserializer.Deserialize<C2S_ChatMessage>(Data);


    }
}
