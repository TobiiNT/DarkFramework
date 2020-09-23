namespace PacketDefinition.Definitions.S2C
{
    public class WorldSendGameNumber : BasePacket
    {
        public WorldSendGameNumber(long gameId, string name)
            : base(PacketCmd.PKT_S2C_WORLD_SEND_GAME_NUMBER)
        {
            Write(gameId);
            WriteConstLengthString(name, 128);
        }
    }
}