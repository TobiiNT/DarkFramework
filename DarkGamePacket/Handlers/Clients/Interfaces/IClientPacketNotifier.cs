namespace DarkGamePacket.Handlers.Clients.Interfaces
{
    public interface IClientPacketNotifier
    {
        void NotifyChatMessage(byte MessageType, string Message);
    }
}
