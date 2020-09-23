namespace PacketDefinition.Definitions.S2C
{
    public class MessageBoxTop : BasePacket
    {
        public MessageBoxTop(string message)
            : base(PacketCmd.PKT_S2C_MESSAGE_BOX_TOP)
        {
            // The following structure might be incomplete or wrong
			Write(message);
            Write(0x00);
        }
    }
}