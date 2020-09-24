using DarkGamePacket.Definitions;
using DarkGamePacket.Interfaces;
using DarkPacket.Packets;

namespace DarkGamePacket.Structs
{
    public class Packet : PacketWriter, IPacket
    {
        public Packet(Channel Channel, PacketID ID)
        {
            WriteByte((byte)Channel);
            WriteUShort((ushort)ID);
        }

        public override byte[] GetPacketData()
        {
            this.Data = base.GetPacketData();
            this.Dispose();
            return this.Data;
        }

        private byte[] Data { set; get; }
    }
}
