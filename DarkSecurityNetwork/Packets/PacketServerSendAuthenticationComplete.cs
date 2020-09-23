using DarkPacket.Packets;
using DarkSecurityNetwork.Enums;

namespace DarkSecurityNetwork.Packets
{
    public class PacketServerSendAuthenticationComplete
    {
        public byte[] Data { private set; get; }
        public PacketServerSendAuthenticationComplete()
        {
            using (var Packet = new PacketWriter())
            {
                Packet.WriteShort((byte)ProtocolFunction.ServerSendAuthenticationComplete);

                Data = Packet.GetPacketData();
            }
        }
    }
}
