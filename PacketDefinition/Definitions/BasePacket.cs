using PacketDefinition.Definitions;

namespace PacketDefinition
{
    public abstract class BasePacket : Packet
    {
        protected BasePacket(PacketCmd PacketID, uint netId = 0) : base(PacketID)
        {
            WriteUInt(netId);
        }
    }
}
