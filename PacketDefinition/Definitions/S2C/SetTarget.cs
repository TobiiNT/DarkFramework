namespace PacketDefinition.Definitions.S2C
{
    public class SetTarget : BasePacket
    {
        public SetTarget(IAttackableUnit attacker, IAttackableUnit attacked)
            : base(PacketCmd.PKT_S2C_SET_TARGET, attacker.NetId)
        {
            WriteNetId(attacked);
        }
    }
}