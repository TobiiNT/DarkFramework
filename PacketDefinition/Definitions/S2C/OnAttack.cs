namespace PacketDefinition.Definitions.S2C
{
    public class OnAttack : BasePacket
    {
        public OnAttack(IAttackableUnit attacker, IAttackableUnit attacked, AttackType attackType)
            : base(PacketCmd.PKT_S2C_ON_ATTACK, attacker.NetId)
        {
            Write((byte)attackType);
            Write(attacked.X);
            Write(attacked.GetZ());
            Write(attacked.Y);
            WriteNetId(attacked);
        }
    }
}