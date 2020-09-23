namespace PacketDefinition.Definitions.S2C
{
    public class ChampionDeathTimer : BasePacket
    {
        public ChampionDeathTimer(IChampion die)
            : base(PacketCmd.PKT_S2C_CHAMPION_DEATH_TIMER, die.NetId)
        {
            Write(die.RespawnTimer / 1000.0f); // Respawn timer, float
        }
    }
}