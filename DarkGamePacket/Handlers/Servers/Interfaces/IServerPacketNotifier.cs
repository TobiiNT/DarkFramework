namespace DarkGamePacket.Servers.Interfaces
{
    public interface IServerPacketNotifier
    {
        void NotifyChatMessage(uint ClientID, byte MessageType, string Message);
    }
}
