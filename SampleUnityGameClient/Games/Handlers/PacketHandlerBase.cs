using DarkGamePacket.Interfaces;

namespace SampleUnityGameClient.Games.PacketDefinitions.Handlers
{
    public abstract class PacketHandlerBase<T> : IPacketHandler<T> where T : ICoreResponse
    {
        public abstract bool HandlePacket(T Request);
    }
}
