using System.IO;

namespace PacketDefinition.Definitions.C2S
{
    public class HeartBeat
    {
        public PacketCmd Cmd;
        public int NetId;
        public float ReceiveTime;
        public float AckTime;
        public HeartBeat(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                Cmd = (PacketCmd)reader.ReadByte();
                NetId = reader.ReadInt32();
                ReceiveTime = reader.ReadSingle();
                AckTime = reader.ReadSingle();
                reader.Close();
            }
        }
    }
}