using DarkPacket.Writer;
using DarkSecurityNetwork.Enums;

namespace DarkSecurityNetwork.Packets
{
    public class PacketClientSendMessageTestVerify
    {
        public byte[] Data { private set; get; }
        public PacketClientSendMessageTestVerify(byte[] MessageData)
        {
            using (var Packet = new PacketWriter())
            {
                Packet.WriteShort((byte)ProtocolFunction.ClientSendMessageTestVerify);
                Packet.WriteBytes(MessageData);

                Data = Packet.GetPacketData();
            }
        }
    }
}
