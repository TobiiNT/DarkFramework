using ProtoBuf;
using System.IO;

namespace DarkGamePacket.Handlers
{
    public static class PacketDeserializer
    {
        public static T Deserialize<T>(byte[] data) where T : class
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
