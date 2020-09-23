namespace PacketDefinition.Interfaces
{
    public interface IPacketHandlerManager
    {
        bool BroadcastPacket(byte[] data, Channel channelNo, PacketFlags flag = PacketFlags.Reliable);
        bool BroadcastPacket(Packet packet, Channel channelNo, PacketFlags flag = PacketFlags.Reliable);
        bool BroadcastPacketTeam(TeamId team, byte[] data, Channel channelNo, PacketFlags flag = PacketFlags.Reliable);
        bool BroadcastPacketTeam(TeamId team, Packet packet, Channel channelNo, PacketFlags flag = PacketFlags.Reliable);
        bool BroadcastPacketVision(IGameObject o, byte[] data, Channel channelNo, PacketFlags flag = PacketFlags.Reliable);
        bool BroadcastPacketVision(IGameObject o, Packet packet, Channel channelNo, PacketFlags flag = PacketFlags.Reliable);
        bool HandlePacket(Peer peer, byte[] data, Channel channelId);
        bool SendPacket(int playerId, byte[] source);
        bool SendPacket(int playerId, Packet packet);
        // TODO: is this really should be in PacketHandler?
        void UnpauseGame();
        bool HandleDisconnect(Peer peer);
    }
}
