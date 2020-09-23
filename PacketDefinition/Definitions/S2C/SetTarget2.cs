namespace PacketDefinition.Definitions.S2C
{
    public class SetTarget2 : BasePacket
    {
        public SetTarget2(IAttackableUnit attacker, IAttackableUnit attacked)
            : base(PacketCmd.PKT_S2C_SET_TARGET2, attacker.NetId)
        {
            WriteNetId(attacked);
        }
    }
}