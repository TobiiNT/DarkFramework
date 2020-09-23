namespace PacketDefinition.Definitions.S2C
{
    public class DestroyParticle : BasePacket
    {
        public DestroyParticle(IParticle p)
            : base(PacketCmd.PKT_S2C_DESTROY_OBJECT, p.NetId)
        {
            WriteNetId(p);
        }
    }
}