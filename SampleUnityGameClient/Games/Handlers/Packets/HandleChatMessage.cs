using DarkGamePacket.Definitions.S2C;
using SampleUnityGameClient.Games.PacketDefinitions.Handlers;

namespace SampleUnityGameClient.Games.PacketDefinitions
{
    public class HandleChatMessage : PacketHandlerBase<S2C_ChatMessage>
    {
        private readonly LogicGame Game;

        public HandleChatMessage(LogicGame Game)
        {
            this.Game = Game;
        }

        public override bool HandlePacket(S2C_ChatMessage Request)
        {
            Logging.WriteLine($"Chat: [{Request.ClientID}]: {Request.Message}");
            return true;
        }
    }
}
