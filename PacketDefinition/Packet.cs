using DarkPacket.Packets;
using PacketDefinition.Definitions;
using PacketDefinition.Interfaces;

namespace PacketDefinition
{
    public class Packet : PacketWriter, IPacket
    {
        public Packet(PacketCmd cmd)
        {
            WriteShort((short)cmd);
        }
    }
}
