using DarkGamePacket.Interfaces;

namespace SampleUnityGameClient.Games.PacketDefinitions.Handlers
{
    public abstract class PacketHandlerBase<T> : IPacketHandler<T> where T : ICoreRequest
    {
        public abstract bool HandlePacket(uint ClientID, T Request);
    }
}
