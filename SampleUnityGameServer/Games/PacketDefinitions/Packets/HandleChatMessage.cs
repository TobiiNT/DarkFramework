using DarkGamePacket.Definitions.C2S;
using SampleUnityGameServer.Games.Handlers;

namespace SampleUnityGameServer.Games.PacketDefinitions.Packets
{
    public class HandleChatMessage : PacketHandlerBase<ChatMessageRequest>
    {
        private readonly LogicGame Game;

        public HandleChatMessage(LogicGame Game)
        {
            this.Game = Game;
        }

        public override bool HandlePacket(uint ClientID, ChatMessageRequest Request)
        {
            this.Game.PacketNotifier.NotifyChatMessage(ClientID, Request.MessageType, Request.Message);
            return true;
        }
    }
}
