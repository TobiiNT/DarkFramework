using System.IO;

namespace PacketDefinition.Definitions.C2S
{
    public class BuyItemRequest
    {
        private PacketCmd _cmd;
        public int NetId;
        public int Id;
        public BuyItemRequest(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                _cmd = (PacketCmd)reader.ReadByte();
                NetId = reader.ReadInt32();
                Id = reader.ReadInt32();
            }
        }
    }
}