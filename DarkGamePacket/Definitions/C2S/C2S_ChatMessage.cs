using DarkGamePacket.Interfaces;
using ProtoBuf;

namespace DarkGamePacket.Definitions.C2S
{
    [ProtoContract]
    public class C2S_ChatMessage : ICoreRequest
    {
        [ProtoMember(1)]
        public uint ClientID;
        [ProtoMember(2)]
        public byte MessageType;
        [ProtoMember(3)]
        public string Message;
    }
}