namespace PacketDefinition.Definitions.S2C
{
    public class EditMessageBoxRight : BasePacket
    {
        public EditMessageBoxRight(string message)
            : base(PacketCmd.PKT_S2C_EDIT_MESSAGE_BOX_RIGHT)
        {
            // The following structure might be incomplete or wrong
            Write(message);
            Write(0x00);
        }
    }
}