using DarkGamePacket.Enums;
using DarkGamePacket.Interfaces;
using ProtoBuf;

namespace DarkGamePacket.Definitions.S2C
{
    [ProtoContract]
    public class ChatMessageResponse : ICoreResponse
    {
        [ProtoMember(1)]
        public PacketID PacketID { get; set; }

        [ProtoMember(2)]
        public byte MessageType;
        [ProtoMember(3)]
        public string Message;
    }
}