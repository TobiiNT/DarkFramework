using DarkGamePacket.Definitions;
using DarkPacket.Readers;
using System.IO;

namespace DarkGamePacket.Structs
{
    public class PacketHeader
    {
        public PacketHeader(byte[] Data)
        {
            using (var Reader = new NormalPacketReader(Data))
            {
                this.Channel = (Channel)Reader.ReadByte();
                this.PacketID = (PacketID)Reader.ReadShort();
                this.ClientID = Reader.ReadUInt();
            }
        }

        public Channel Channel;
        public PacketID PacketID;
        public uint ClientID;
    }
}
