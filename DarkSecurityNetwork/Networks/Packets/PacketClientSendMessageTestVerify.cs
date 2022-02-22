using DarkPacket.Writer;
using DarkSecurityNetwork.Enums;

namespace DarkSecurityNetwork.Networks.Packets
{
    public class PacketClientSendMessageTestVerify
    {
        public byte[] Data { get; }
        public PacketClientSendMessageTestVerify(byte[] MessageData)
        {
            using var Packet = new PacketWriter();
            Packet.WriteShort((byte)ProtocolFunction.ClientSendMessageTestVerify);
            Packet.WriteBytes(MessageData);

            Data = Packet.GetPacketData();
        }
    }
}
