namespace DarkPacket.Readers
{
    public class NormalPacketReader : PacketReader
    {
        public NormalPacketReader(byte[] Data)
        {
            this.Data = Data;
            this.Length = Data.Length;
            this.Index = 0;
        }
    }
}
