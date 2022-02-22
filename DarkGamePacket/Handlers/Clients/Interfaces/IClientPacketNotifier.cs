namespace DarkGamePacket.Handlers.Clients.Interfaces
{
    public interface IClientPacketNotifier
    {
        void NotifyChatMessage(uint ClientID, byte MessageType, string Message);
    }
}
