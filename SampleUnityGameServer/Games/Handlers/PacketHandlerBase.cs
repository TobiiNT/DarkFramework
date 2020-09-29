using DarkGamePacket.Interfaces;

namespace SampleUnityGameServer.Games.Handlers
{
    public abstract class PacketHandlerBase<T> : IPacketHandler<T> where T : ICoreRequest
    {
        public abstract bool HandlePacket(uint ClientID, T Request);
    }
}
