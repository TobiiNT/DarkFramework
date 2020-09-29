using DarkPacket.Writer;
using DarkSecurityNetwork.Enums;

namespace DarkSecurityNetwork.Networks.Packets
{
    public class PacketServerSendAuthenticationComplete
    {
        public byte[] Data { get; }
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
