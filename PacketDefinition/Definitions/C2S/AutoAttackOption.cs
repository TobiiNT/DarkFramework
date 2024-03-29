using System.IO;

namespace PacketDefinition.Definitions.C2S
{
    public class AutoAttackOption
    {
        public byte Cmd;
        public int Netid;
        public byte Activated;

        public AutoAttackOption(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                Cmd = reader.ReadByte();
                Netid = reader.ReadInt32();
                Activated = reader.ReadByte();
            }
        }
    }
}