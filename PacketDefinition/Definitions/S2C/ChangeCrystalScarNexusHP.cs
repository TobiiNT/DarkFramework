namespace PacketDefinition.Definitions.S2C
{
    public class ChangeCrystalScarNexusHp : BasePacket
    {
        public ChangeCrystalScarNexusHp(TeamId team, int hp)
            : base(PacketCmd.PKT_S2C_CHANGE_CRYSTAL_SCAR_NEXUS_HP)
        {
            Write((uint)team);
            Write(hp);
        }
    }
}