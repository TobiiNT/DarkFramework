using DarkGamePacket.Definitions;

namespace DarkGamePacket.Structs
{
    public abstract class BasePacket : Packet
    {
        protected BasePacket(Channel Channel, PacketID PacketID, uint ClientID = 0) : base(Channel, PacketID)
        {
            WriteUInt(ClientID);
        }
    }
}
