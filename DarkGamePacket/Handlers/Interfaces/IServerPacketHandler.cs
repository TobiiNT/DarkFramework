﻿using DarkGamePacket.Definitions;
using DarkGamePacket.Packets;
using DarkPacket.Interfaces;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Networks;

namespace DarkGamePacket.Handlers.Interfaces
{
    public interface IServerPacketHandler
    {
        bool HandleClientHandshake(uint ClientID, SecurityConnection<ServerSecurityNetwork> Connection);
        bool HandleClientIncomingPacket(uint ClientID, byte[] Data);
        bool HandleClientDisconnect(uint ClientID);
        bool SendPacketToClient(uint ClientID, PacketID PacketID, ICoreMessage Request);
        bool SendPacketBroadcast(PacketID PacketID, ICoreMessage Response);
    }
}
