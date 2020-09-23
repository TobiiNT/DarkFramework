namespace PacketDefinition.Definitions.S2C
{
    public class EnableFow : BasePacket
    {
        public EnableFow(bool activate)
            : base(PacketCmd.PKT_S2C_ENABLE_FOW)
        {
            Write(activate ? 0x01 : 0x00);
        }
    }
}