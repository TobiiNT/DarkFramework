using System.IO;
using DarkGamePacket.Attributes;
using DarkGamePacket.Definitions.C2S;
using DarkGamePacket.Enums;
using ProtoBuf;

namespace DarkGamePacket.Servers.Packets
{
    public class ServerRequests
    {
        [PacketType(PacketID.CHAT_MESSAGE)]
        public static ChatMessageRequest ChatMessageRequest(byte[] Data) => ProtoDeserialize<ChatMessageRequest>(Data);

        public static T ProtoDeserialize<T>(byte[] data) where T : class
        {
            if (null == data) return null;

            try
            {
                using (var stream = new MemoryStream(data))
                {
                    return Serializer.Deserialize<T>(stream);
                }
            }
            catch { throw; }
        }
    }
}
