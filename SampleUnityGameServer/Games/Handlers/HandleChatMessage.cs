using DarkGamePacket.Definitions.C2S;
using DarkPacket.Interfaces;

namespace SampleUnityGameServer.Games.Handlers.Packets
{
    public class HandleChatMessage : IPacketHandle<C2S_ChatMessage>
    {
        private readonly ServerLogic Game;

        public HandleChatMessage(ServerLogic Game)
        {
            this.Game = Game;
        }

        public bool HandlePacket(uint ClientID, C2S_ChatMessage Request)
        {
            this.Game.PacketNotifier.NotifyChatMessage(ClientID, Request.MessageType, Request.Message);
            return true;
        }
    }
}
