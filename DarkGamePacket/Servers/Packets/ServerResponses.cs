using System.IO;
using DarkGamePacket.Attributes;
using DarkGamePacket.Definitions.S2C;
using DarkGamePacket.Enums;
using DarkGamePacket.Interfaces;
using ProtoBuf;

namespace DarkGamePacket.Servers.Packets
{
    public class ServerResponses
    {
        [PacketType(PacketID.CHAT_MESSAGE)]
        public static byte[] ChatMessageResponse(ICoreResponse Response) => ProtoSerialize(Response as ChatMessageResponse);

        public static byte[] ProtoSerialize<T>(T record) where T : class
        {
            if (null == record) return null;

            try
            {
                using (var stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, record);
                    return stream.ToArray();
                }
            }
            catch { throw; }
        }
    }
}
