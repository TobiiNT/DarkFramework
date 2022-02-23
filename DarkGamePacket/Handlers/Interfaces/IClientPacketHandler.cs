using DarkGamePacket.Packets;
using DarkPacket.Interfaces;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;

namespace DarkGamePacket.Handlers.Interfaces
{
    public interface IClientPacketHandler
    {        
        bool HandleHandshake(SecurityConnection<ClientSecurityNetwork> Connection);
        bool HandlePacket(uint ClientID, byte[] Data);
        bool SendPacket(ListPacketID PacketID, ICoreMessage Request);
        bool HandleDisconnect();
    }
}
