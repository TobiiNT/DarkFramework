using DarkGamePacket.Enums;
using DarkGamePacket.Interfaces;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;

namespace DarkGamePacket.Handlers.Clients.Interfaces
{
    public interface IClientPacketHandler
    {
        bool HandleHandshake(SecurityConnection<ClientSecurityNetwork> Connection);
        bool HandlePacket(byte[] Data);
        bool SendPacket(PacketID PacketID, ICoreRequest Request);
        bool HandleDisconnect();
    }
}
