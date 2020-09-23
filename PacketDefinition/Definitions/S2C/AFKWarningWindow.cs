namespace PacketDefinition.Definitions.S2C
{
    public class AfkWarningWindow : Packet
    {
        public AfkWarningWindow()
            : base(PacketCmd.PKT_S2C_AFK_WARNING_WINDOW)
        {
            // The following structure might be incomplete or wrong
        }
    }
}