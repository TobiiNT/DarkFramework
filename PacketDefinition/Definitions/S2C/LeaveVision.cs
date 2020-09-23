namespace PacketDefinition.Definitions.S2C
{
    public class LeaveVision : BasePacket
    {
        public LeaveVision(IGameObject o)
            : base(PacketCmd.PKT_S2C_LEAVE_VISION, o.NetId)
        {
        }
    }
}