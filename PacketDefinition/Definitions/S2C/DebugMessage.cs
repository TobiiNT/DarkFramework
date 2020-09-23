namespace PacketDefinition.Definitions.S2C
{
    public class DebugMessage : BasePacket
    {
        public DebugMessage(string message)
            : base(PacketCmd.PKT_S2C_DEBUG_MESSAGE)
        {
            Write(0);
			WriteConstLengthString(message, 512);
        }
    }
}