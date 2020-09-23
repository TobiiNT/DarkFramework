namespace PacketDefinition.Definitions.S2C
{
    public class HideUi : BasePacket
    {
        public HideUi()
            : base(PacketCmd.PKT_S2C_HIDE_UI)
        {

        }
    }
}