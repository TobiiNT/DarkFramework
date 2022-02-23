namespace DarkGamePacket.Notifiers.Interfaces
{
    public interface IClientPacketNotifier
    {
        void NotifyChatMessage(uint ClientID, byte MessageType, string Message);
    }
}
