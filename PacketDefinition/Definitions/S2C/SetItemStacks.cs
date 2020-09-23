namespace PacketDefinition.Definitions.S2C
{
    public class SetItemStacks : BasePacket
    {
        public SetItemStacks(IAttackableUnit unit, byte slot, byte stack1, byte stack2)
            : base(PacketCmd.PKT_S2C_SET_ITEM_STACKS, unit.NetId)
        {
            Write(slot);
            Write(stack1); // Needs more research
            Write(stack2); //
        }
    }
}