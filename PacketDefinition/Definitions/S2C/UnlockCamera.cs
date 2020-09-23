namespace PacketDefinition.Definitions.S2C
{
    public class UnlockCamera : BasePacket
    {
        public UnlockCamera()
            : base(PacketCmd.PKT_S2C_UNLOCK_CAMERA)
        {

        }
    }
}