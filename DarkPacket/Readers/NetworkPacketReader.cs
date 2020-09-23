using System;

namespace DarkPacket.Readers
{
    public class NetworkPacketReader : PacketReader
    {
        public NetworkPacketReader(byte[] Data)
        {
            if (Data.Length < 8)
                throw new Exception($"Invalid Packet");

            this.Length = Data.Length - 4;
            this.Index = 0;
            this.Data = new byte[this.Length];
            Buffer.BlockCopy(Data, 2, this.Data, 0, this.Length);
        }
    }
}
