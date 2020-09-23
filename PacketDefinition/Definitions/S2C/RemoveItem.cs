namespace PacketDefinition.Definitions.S2C
{
    public class RemoveItem : BasePacket
    {
        public RemoveItem(IAttackableUnit u, byte slot, short remaining)
            : base(PacketCmd.PKT_S2C_REMOVE_ITEM, u.NetId)
        {
            Write(slot);
            Write(remaining);
        }
    }
}