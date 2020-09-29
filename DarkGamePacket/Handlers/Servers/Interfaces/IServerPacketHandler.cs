using DarkGamePacket.Enums;
using DarkGamePacket.Interfaces;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;

namespace DarkGamePacket.Servers.Interfaces
{
    public interface IServerPacketHandler
    {
        bool HandleHandshake(uint ClientID, SecurityConnection<ServerSecurityNetwork> Connection);
        bool HandlePacket(uint ClientID, byte[] Data);
        bool SendPacket(uint ClientID, PacketID PacketID, ICoreResponse Response);
        bool SendPacketBroadcast(PacketID PacketID, ICoreResponse Response);
        bool HandleDisconnect(uint ClientID);
    }
}
