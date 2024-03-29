namespace PacketDefinition.Definitions.S2C
{
    public class SoundSettings : BasePacket
    {
        public SoundSettings(byte soundCategory, bool mute)
            : base(PacketCmd.PKT_S2C_SOUND_SETTINGS)
        {
            Write(soundCategory);
            Write(mute);
        }
    }
}