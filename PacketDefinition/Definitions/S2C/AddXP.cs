namespace PacketDefinition.Definitions.S2C
{
    public class AddXp : BasePacket
    {
        public AddXp(IAttackableUnit u, float xp)
            : base(PacketCmd.PKT_S2C_ADD_XP)
        {
            WriteNetId(u);
            Write((float)xp);
        }
    }
}