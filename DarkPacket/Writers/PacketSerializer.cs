using ProtoBuf;
using System.IO;

namespace DarkPacket.Writers
{
    public static class PacketSerializer
    {
        public static byte[] Serialize<T>(T record) where T : class
        {
            if (null == record) return null;

            try
            {
                using var stream = new MemoryStream();
                Serializer.Serialize(stream, record);
                return stream.ToArray();
            }
            catch { throw; }
        }
    }
}
