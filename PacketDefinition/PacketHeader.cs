using PacketDefinition.Definitions;
using System.IO;

namespace PacketDefinition
{
    public class PacketHeader
    {
        public PacketHeader(byte[] bytes)
        {
            var reader = new BinaryReader(new MemoryStream(bytes));
            Cmd = (PacketCmd)reader.ReadByte();
            NetId = reader.ReadInt32();
            reader.Close();
        }

        public PacketCmd Cmd;
        public int NetId;
    }
}
