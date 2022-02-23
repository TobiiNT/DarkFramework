using DarkPacket.Interfaces;
using ProtoBuf;

namespace DarkGamePacket.Definitions.C2S
{
    [ProtoContract]
    public class C2S_ChatMessage : ICoreMessage
    {
        [ProtoMember(1)]
        public uint ClientID { get; set; }
        [ProtoMember(2)]
        public byte MessageType { get; set; }
        [ProtoMember(3)]
        public string Message { get; set; }
    }
}