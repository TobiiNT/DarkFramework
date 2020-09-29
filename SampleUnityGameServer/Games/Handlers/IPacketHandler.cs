using DarkGamePacket.Interfaces;

namespace SampleUnityGameServer.Games.Handlers
{
    public interface IPacketHandler<in T> where T : ICoreRequest
    {
        bool HandlePacket(uint ClientID, T Request);
    }
}
