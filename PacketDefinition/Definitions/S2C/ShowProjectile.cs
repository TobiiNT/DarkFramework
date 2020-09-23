namespace PacketDefinition.Definitions.S2C
{
    public class ShowProjectile : BasePacket
    {
        public ShowProjectile(IProjectile p)
            : base(PacketCmd.PKT_S2C_SHOW_PROJECTILE, p.Owner.NetId)
        {
            WriteNetId(p);
        }
    }
}