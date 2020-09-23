namespace PacketDefinition.Definitions.S2C
{
    public class QueryStatus : BasePacket
    {
        public QueryStatus()
            : base(PacketCmd.PKT_S2C_QUERY_STATUS_ANS)
        {
            Write((byte)1); //ok
        }
    }
}