using DarkGamePacket.Definitions.S2C;
using DarkGamePacket.Handlers.Classes;
using DarkGamePacket.Handlers.Interfaces;
using DarkGamePacket.Notifiers.Classes;
using DarkGamePacket.Notifiers.Interfaces;
using DarkPacket.Handlers;
using DarkPacket.Interfaces;
using SampleUnityGameClient.Games.PacketDefinitions;

namespace SampleUnityGameClient.Games
{
    public class ClientLogic
    {
        public NetworkHandler<ICoreMessage> FunctionHandler { get; }
        public IClientPacketNotifier PacketNotifier { get; }
        public IClientPacketHandler PacketHandler { get; }
        public ClientLogic()
        {
            this.FunctionHandler = new NetworkHandler<ICoreMessage>();
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
