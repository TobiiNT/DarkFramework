namespace PacketDefinition.Definitions.S2C
{
    public class DisconnectedAnnouncement : BasePacket
    {
        public DisconnectedAnnouncement(IAttackableUnit unit)
            : base(PacketCmd.PKT_S2C_DISCONNECTED_ANNOUNCEMENT)
        {
            WriteNetId(unit);
        }
    }
}