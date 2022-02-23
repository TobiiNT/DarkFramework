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
        bool SendPacket(uint ClientID, ListPacketID PacketID, ICoreMessage Request);
        bool SendPacketBroadcast(ListPacketID PacketID, ICoreMessage Response);
        bool HandleDisconnect(uint ClientID);
    }
}
