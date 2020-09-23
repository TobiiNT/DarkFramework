namespace PacketDefinition.Definitions.S2C
{
    public class GameSpeed : BasePacket
    {
        public GameSpeed(float gameSpeed)
            : base(PacketCmd.PKT_S2C_GAME_SPEED)
        {
            Write(gameSpeed);
        }
    }
}