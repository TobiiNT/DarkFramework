using DarkPacket.Writer;
using DarkSecurityNetwork.Enums;

namespace DarkSecurityNetwork.Networks.Packets
{
    public class PacketServerSendMessageTest
    {
        public byte[] Data { get; }
        public PacketServerSendMessageTest(byte[] MessageData)
        {
            using var Packet = new PacketWriter();
            Packet.WriteShort((byte)ProtocolFunction.ServerSendMessageTest);
            Packet.WriteBytes(MessageData);

            Data = Packet.GetPacketData();
        }
    }
}
