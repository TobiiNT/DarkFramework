namespace PacketDefinition.Definitions.S2C
{
    public class SetItemStacks2 : BasePacket
    {
        public SetItemStacks2(IAttackableUnit unit, byte slot, byte stack)
            : base(PacketCmd.PKT_S2C_SET_ITEM_STACKS2, unit.NetId)
        {
            Write(slot);
            Write(stack); // Needs more research
        }
    }
}