using DarkGamePacket.Enums;
using DarkGamePacket.Interfaces;
using DarkPacket.Readers;
using ProtoBuf;

namespace DarkGamePacket.Definitions.C2S
{
    [ProtoContract]
    public class ChatMessageRequest : ICoreRequest
    {
        [ProtoMember(1)]
        public PacketID PacketID { get; set; }
        [ProtoMember(2)]
        public byte MessageType;
        [ProtoMember(3)]
        public string Message;
    }
}