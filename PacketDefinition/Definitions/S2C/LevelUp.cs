namespace PacketDefinition.Definitions.S2C
{
    public class LevelUp : BasePacket
    {
        public LevelUp(IChampion c)
            : base(PacketCmd.PKT_S2C_LEVEL_UP, c.NetId)
        {
            Write(c.Stats.Level);
            Write(c.SkillPoints);
        }
    }
}