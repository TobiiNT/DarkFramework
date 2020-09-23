namespace PacketDefinition.Definitions.S2C
{
    public class AddUnitFow : BasePacket
    {
        public AddUnitFow(IAttackableUnit u)
            : base(PacketCmd.PKT_S2C_ADD_UNIT_FOW)
        {
            WriteNetId(u);
        }
    }
}