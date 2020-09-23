namespace PacketDefinition.Definitions.S2C
{
    public class NpcDie : BasePacket
    {
        public NpcDie(IAttackableUnit die, IAttackableUnit killer)
            : base(PacketCmd.PKT_S2C_NPC_DIE, die.NetId)
        {
            Write(0);
            Write((byte)0);
            WriteNetId(killer);
            Write((byte)0); // unk
            Write((byte)7); // unk
            Write(0); // Flags?
        }
    }
}