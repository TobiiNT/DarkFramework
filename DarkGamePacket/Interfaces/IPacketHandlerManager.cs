using DarkGamePacket.Structs;
using DarkSecurityNetwork;
using DarkSecurityNetwork.Interfaces;

namespace DarkGamePacket.Interfaces
{
    public interface IPacketHandlerManager<T> where T : ISecurityNetwork
    {
        //bool BroadcastPacket(byte[] data, Channel channelNo, PacketFlags flag = PacketFlags.Reliable);
        //bool BroadcastPacket(Packet packet, Channel channelNo, PacketFlags flag = PacketFlags.Reliable);
        //bool BroadcastPacketTeam(TeamId team, byte[] data, Channel channelNo, PacketFlags flag = PacketFlags.Reliable);
        //bool BroadcastPacketTeam(TeamId team, Packet packet, Channel channelNo, PacketFlags flag = PacketFlags.Reliable);
        //bool BroadcastPacketVision(IGameObject o, byte[] data, Channel channelNo, PacketFlags flag = PacketFlags.Reliable);
        //bool BroadcastPacketVision(IGameObject o, Packet packet, Channel channelNo, PacketFlags flag = PacketFlags.Reliable);
        bool HandleHandshake(uint ClientID, SecurityConnection<T> Connection);
        bool HandlePacket(uint ClientID, byte[] Data);
        bool SendPacket(uint ClientID, byte[] Data);
        bool SendPacket(uint ClientID, Packet Packet);
        bool HandleDisconnect(uint ClientID);
    }
}
