namespace PacketDefinition.Definitions.S2C
{
    public class GameTimer : BasePacket
    {
        public GameTimer(float fTime)
            : base(PacketCmd.PKT_S2C_GAME_TIMER, 0)
        {
            Write(fTime);
        }
    }
}