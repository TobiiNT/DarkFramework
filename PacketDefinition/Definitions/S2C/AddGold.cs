namespace PacketDefinition.Definitions.S2C
{
    public class AddGold : BasePacket
    {
        public AddGold(IChampion richMan, IAttackableUnit died, float gold)
            : base(PacketCmd.PKT_S2C_ADD_GOLD, richMan.NetId)
        {
            WriteNetId(richMan);
            WriteNetId(died);
            Write(gold);
        }
    }
}