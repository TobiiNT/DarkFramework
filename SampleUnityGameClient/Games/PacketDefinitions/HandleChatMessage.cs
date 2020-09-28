using DarkGamePacket.Definitions.C2S;
using DarkGamePacket.Definitions.S2C;
using SampleUnityGameClient.Games.PacketDefinitions.Handlers;

namespace SampleUnityGameClient.Games.PacketDefinitions
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
            //this.Game..NotifyChatMessage(ClientID, Request.MessageType, Request.Message);
            Logging.WriteLine(Request.Message);
            return true;
        }
    }
}
