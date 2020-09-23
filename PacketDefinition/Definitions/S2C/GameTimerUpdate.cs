namespace PacketDefinition.Definitions.S2C
{
    public class GameTimerUpdate : BasePacket
    {
        public GameTimerUpdate(float fTime)
            : base(PacketCmd.PKT_S2C_GAME_TIMER_UPDATE, 0)
        {
            Write(fTime);
        }
    }
}