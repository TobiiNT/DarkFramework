using DarkGamePacket.Interfaces;
using ProtoBuf;

namespace DarkGamePacket.Definitions.C2S
{
    [ProtoContract]
    public class C2S_ChatMessage : ICoreRequest
    {
        [ProtoMember(1)]
        public byte MessageType;
        [ProtoMember(2)]
        public string Message;
    }
}