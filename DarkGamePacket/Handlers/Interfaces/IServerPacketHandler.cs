using DarkGamePacket.Definitions;
using DarkGamePacket.Packets;
using DarkPacket.Interfaces;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;

namespace DarkGamePacket.Handlers.Interfaces
{
    public interface IServerPacketHandler
    {
        bool HandleHandshake(uint ClientID, SecurityConnection<ServerSecurityNetwork> Connection);
        bool HandlePacket(uint ClientID, byte[] Data);
        bool HandleDisconnect(uint ClientID);
        bool SendPacket(uint ClientID, PacketID PacketID, ICoreMessage Request);
        bool SendPacketBroadcast(PacketID PacketID, ICoreMessage Response);
    }
}
