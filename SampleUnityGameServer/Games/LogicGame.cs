using DarkGamePacket.Definitions.C2S;
using DarkGamePacket.Handlers.Classes;
using DarkGamePacket.Handlers.Interfaces;
using DarkGamePacket.Notifiers.Classes;
using DarkGamePacket.Notifiers.Interfaces;
using DarkPacket.Handlers;
using DarkPacket.Interfaces;
using SampleUnityGameServer.Games.Handlers.Packets;

namespace SampleUnityGameServer.Games
{
    public class LogicGame
    {
        public NetworkHandler<ICoreMessage> FunctionHandler { get; }
        public IServerPacketNotifier PacketNotifier { get; }
        public IServerPacketHandler PacketHandler { get; }
        public LogicGame()
        {
            this.FunctionHandler = new NetworkHandler<ICoreMessage>();
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
