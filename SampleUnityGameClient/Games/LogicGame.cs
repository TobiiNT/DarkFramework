using DarkGamePacket.Definitions.S2C;
using DarkGamePacket.Handlers.Clients;
using DarkGamePacket.Handlers.Clients.Interfaces;
using DarkGamePacket.Interfaces;
using SampleUnityGameClient.Games.PacketDefinitions;

namespace SampleUnityGameClient.Games
{
    public class LogicGame
    {
        public ClientNetworkHandler<ICoreResponse> FunctionHandler { get; }
        public IClientPacketNotifier PacketNotifier { get; }
        public IClientPacketHandler PacketHandler { get; }
        public LogicGame()
        {
            this.FunctionHandler = new ClientNetworkHandler<ICoreResponse>();
            this.PacketHandler = new ClientPacketHandler(FunctionHandler);
            this.PacketNotifier = new ClientPacketNotifier(PacketHandler);

            InitializeHandler();
        }
        public void InitializeHandler()
        {
            this.FunctionHandler.Register<S2C_ChatMessage>(new HandleChatMessage(this).HandlePacket);
        }
    }
}
