using DarkGamePacket.Interfaces;

namespace SampleUnityGameClient.Games.PacketDefinitions.Handlers
{
    public interface IPacketHandler<in T> where T : ICoreRequest
    {
        bool HandlePacket(uint ClientID, T Request);
    }
}
