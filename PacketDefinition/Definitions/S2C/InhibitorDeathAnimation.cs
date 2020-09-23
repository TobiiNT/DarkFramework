namespace PacketDefinition.Definitions.S2C
{
    public class InhibitorDeathAnimation : BasePacket
    {
        public InhibitorDeathAnimation(IInhibitor inhi, IGameObject killer)
            : base(PacketCmd.PKT_S2C_INHIBITOR_DEATH_ANIMATION, inhi.NetId)
        {
            WriteNetId(killer);
            Write(0); //unk
        }
    }
}