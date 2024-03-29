namespace PacketDefinition.Definitions.S2C
{
    public class InhibitorStateUpdate : BasePacket
    {
        public InhibitorStateUpdate(IInhibitor inhi)
            : base(PacketCmd.PKT_S2C_INHIBITOR_STATE, inhi.NetId)
        {
            Write((byte)inhi.InhibitorState);
            Write((byte)0);
            Write((byte)0);
        }
    }
}