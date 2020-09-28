using DarkGamePacket.Definitions;
using DarkGamePacket.Definitions.C2S;
using DarkGamePacket.Interfaces;
using DarkGamePacket.Servers;
using DarkGamePacket.Servers.Interfaces;
using DarkSecurityNetwork.Networks;
using SampleUnityGameServer.Games.PacketDefinitions;
using SampleUnityGameServer.Games.PacketDefinitions.Packets;

namespace SampleUnityGameServer.Games
{
    public class LogicGame
    {
        public NetworkHandler<ICoreRequest> PacketHandler { private set; get; }
        public IServerPacketNotifier PacketNotifier { private set; get; }

        public LogicGame(IServerPacketHandler PacketHandler)
        {
            this.PacketHandler = new NetworkHandler<ICoreRequest>();
            this.PacketNotifier = new ServerPacketNotifier(PacketHandler);
        }
        public void InitializeHandler()
        {

            this.PacketHandler.Register<ChatMessageRequest>(new HandleChatMessage(this).HandlePacket);
        }
    }
}
