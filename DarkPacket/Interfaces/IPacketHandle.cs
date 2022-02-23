namespace DarkPacket.Interfaces
{
    public interface IPacketHandle<T> where T : ICoreMessage
    {
        bool HandlePacket(uint ClientID, T Request);
    }
}
