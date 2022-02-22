using DarkGamePacket.Interfaces;
using ProtoBuf;

namespace DarkGamePacket.Definitions.S2C
{
    [ProtoContract]
    public class S2C_ChatMessage : ICoreResponse
    {
        [ProtoMember(1)]
        public uint ClientID { get; set; }
        [ProtoMember(2)]
        public byte MessageType { get; set; }
        [ProtoMember(3)]
        public string Message { get; set; }
    }
}