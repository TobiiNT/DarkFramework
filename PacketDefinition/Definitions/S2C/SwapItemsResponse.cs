namespace PacketDefinition.Definitions.S2C
{
    public class SwapItemsResponse : BasePacket
    {
        public SwapItemsResponse(IChampion c, byte slotFrom, byte slotTo)
            : base(PacketCmd.PKT_S2C_SWAP_ITEMS, c.NetId)
        {
            Write(slotFrom);
            Write(slotTo);
        }
    }
}