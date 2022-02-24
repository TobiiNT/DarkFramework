using DarkGamePacket.Definitions;
using DarkGamePacket.Packets;
using DarkPacket.Interfaces;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;
using System;

namespace DarkGamePacket.Handlers.Interfaces
{
    public interface IClientPacketHandler
    {        
        bool HandleServerHandshake(SecurityConnection<ClientSecurityNetwork> Connection);
        bool HandleServerIncomingPacket(uint ClientID, byte[] Data);
        bool SendPacketToServer(PacketID PacketID, ICoreMessage Request);
        bool HandleServerDisconnect();
    }
}
