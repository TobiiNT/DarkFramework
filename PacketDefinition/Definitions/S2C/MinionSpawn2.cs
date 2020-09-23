namespace PacketDefinition.Definitions.S2C
{
    public class MinionSpawn2 : Packet // shhhhh....
    {
        public MinionSpawn2(uint netId)
            : base(PacketCmd.PKT_S2C_OBJECT_SPAWN)
        {
            Write(netId);
            Fill(0, 3);
        }
    }
}