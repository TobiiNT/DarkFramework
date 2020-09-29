using DarkGamePacket.Interfaces;

namespace SampleUnityGameClient.Games.PacketDefinitions.Handlers
{
    public interface IPacketHandler<in T> where T : ICoreResponse
    {
        bool HandlePacket(T Request);
    }
}
