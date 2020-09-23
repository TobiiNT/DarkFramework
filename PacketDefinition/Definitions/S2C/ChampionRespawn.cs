namespace PacketDefinition.Definitions.S2C
{
    public class ChampionRespawn : BasePacket
    {
        public ChampionRespawn(IChampion c) :
            base(PacketCmd.PKT_S2C_CHAMPION_RESPAWN, c.NetId)
        {
            Write((float)c.X);
            Write((float)c.Y);
            Write((float)c.GetZ());
        }
    }
}