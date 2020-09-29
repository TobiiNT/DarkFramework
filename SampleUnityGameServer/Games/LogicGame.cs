using DarkGamePacket.Definitions;
using DarkGamePacket.Definitions.C2S;
using DarkGamePacket.Interfaces;
using DarkGamePacket.Servers;
using DarkGamePacket.Servers.Interfaces;
using SampleUnityGameServer.Games.Handlers.Packets;

namespace SampleUnityGameServer.Games
{
    public class LogicGame
    {
        public ServerNetworkHandler<ICoreRequest> FunctionHandler { get; }
        public IServerPacketNotifier PacketNotifier { get; }
        public IServerPacketHandler PacketHandler { get; }
        public LogicGame()
        {
            this.FunctionHandler = new ServerNetworkHandler<ICoreRequest>();
            this.PacketHandler = new ServerPacketHandler(FunctionHandler);
            this.PacketNotifier = new ServerPacketNotifier(PacketHandler);

            InitializeHandler();
        }
        public void InitializeHandler()
        {
            this.FunctionHandler.Register<C2S_ChatMessage>(new HandleChatMessage(this).HandlePacket);
        }
    }
}
