using DarkPacket.Readers;

namespace DarkGamePacket.Definitions.C2S
{
    public class ChatMessageRequest 
    {
        public PacketID Packet;
        public ushort MessageType;
        public string Message;

        public ChatMessageRequest(byte[] Data)
        {
            using (var Reader = new NormalPacketReader(Data))
            {
                Packet = (PacketID)Reader.ReadShort();
                MessageType = Reader.ReadUShort();
                Message = Reader.ReadString();
            }
        }
    }
}