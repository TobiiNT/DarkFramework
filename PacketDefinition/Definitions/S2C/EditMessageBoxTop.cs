namespace PacketDefinition.Definitions.S2C
{
    public class EditMessageBoxTop : BasePacket
    {
        public EditMessageBoxTop(string message)
            : base(PacketCmd.PKT_S2C_EDIT_MESSAGE_BOX_TOP)
        {
            // The following structure might be incomplete or wrong
            Write(message);
            Write(0x00);
        }
    }
}