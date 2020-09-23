namespace PacketDefinition.Definitions.S2C
{
    public class FloatingText : BasePacket
    {
        public FloatingText(IAttackableUnit u, string text)
            : base(PacketCmd.PKT_S2C_FLOATING_TEXT, u.NetId)
        {
            Write(0); // netid?
            Fill(0, 10);
            Write(0); // netid?
            Write(text);
            Write((byte)0x00);
        }
    }
}