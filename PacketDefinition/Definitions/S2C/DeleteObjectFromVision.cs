namespace PacketDefinition.Definitions.S2C
{
    public class DeleteObjectFromVision : BasePacket
    {
        public DeleteObjectFromVision(IGameObject o)
            : base(PacketCmd.PKT_S2C_DELETE_OBJECT, o.NetId)
        {
        }
    }
}