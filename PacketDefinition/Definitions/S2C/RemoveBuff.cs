namespace PacketDefinition.Definitions.S2C
{
    public class RemoveBuff : BasePacket
    {
        public RemoveBuff(IAttackableUnit u, string name, byte slot)
            : base(PacketCmd.PKT_S2C_REMOVE_BUFF, u.NetId)
        {
            Write(slot);
            WriteStringHash(name);
            Write(0x0);
            //WriteNetId(u);//source?
        }
    }
}