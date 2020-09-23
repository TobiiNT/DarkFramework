namespace PacketDefinition.Definitions.S2C
{
    public class SetCapturePoint : BasePacket
    {
        public SetCapturePoint(IAttackableUnit unit, byte capturePointId)
            : base(PacketCmd.PKT_S2C_SET_CAPTURE_POINT)
        {
            Write(capturePointId);
            WriteNetId(unit);
            Fill(0, 6);
        }
    }
}