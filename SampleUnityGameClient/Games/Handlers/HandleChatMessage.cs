using DarkGamePacket.Definitions.S2C;
using DarkPacket.Interfaces;

namespace SampleUnityGameClient.Games.PacketDefinitions
{
    public class HandleChatMessage : IPacketHandle<S2C_ChatMessage>
    {
        private readonly ClientLogic Game;

        public HandleChatMessage(ClientLogic Game)
        {
            this.Game = Game;
        }

        public bool HandlePacket(uint ClientID, S2C_ChatMessage Request)
        {
            Logging.WriteLine($"Chat: [{Request.ClientID}]: {Request.Message}");
            return true;
        }
    }
}
