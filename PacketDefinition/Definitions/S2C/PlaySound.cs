namespace PacketDefinition.Definitions.S2C
{
    public class PlaySound : BasePacket
    {
        public PlaySound(IAttackableUnit unit, string soundName)
            : base(PacketCmd.PKT_S2C_PLAY_SOUND, unit.NetId)
        {
			WriteConstLengthString(soundName, 1024);
            WriteNetId(unit); // audioEventNetworkID?
        }
    }
}