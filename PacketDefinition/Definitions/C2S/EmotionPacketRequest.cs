using System.IO;

namespace PacketDefinition.Definitions.C2S
{
    public class EmotionPacketRequest
    {
        public PacketCmd Cmd;
        public uint NetId;
        public EmotionType Id;

        public EmotionPacketRequest(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                Cmd = (PacketCmd)reader.ReadByte();
                NetId = reader.ReadUInt32();
                Id = (EmotionType)reader.ReadByte();
            }
        }
    }
}