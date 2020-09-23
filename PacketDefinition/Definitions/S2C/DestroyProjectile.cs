namespace PacketDefinition.Definitions.S2C
{
    public class DestroyProjectile : BasePacket
    {
        public DestroyProjectile(IProjectile p)
            : base(PacketCmd.PKT_S2C_DESTROY_PROJECTILE, p.NetId)
        {

        }
    }
}