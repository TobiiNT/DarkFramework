using DarkGamePacket.Definitions.C2S;

namespace SampleUnityGameServer.Games.Handlers.Packets
{
    public class HandleChatMessage : PacketHandlerBase<C2S_ChatMessage>
    {
        private readonly LogicGame Game;

        public HandleChatMessage(LogicGame Game)
        {
            this.Game = Game;
        }

        public override bool HandlePacket(uint ClientID, C2S_ChatMessage Request)
        {
            this.Game.PacketNotifier.NotifyChatMessage(ClientID, Request.MessageType, Request.Message);
            return true;
        }
    }
}
