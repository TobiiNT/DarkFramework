using DarkPacket.Packets;
using DarkSecurity.Interfaces.Keys;
using DarkSecurityNetwork.Enums;
using DarkSecurity.Services.AES;

namespace DarkSecurityNetwork.Packets
{
    public class PacketClientSendSymmetricKey
    {
        public byte[] Data { private set; get; }
        public PacketClientSendSymmetricKey(ICryptoKey CryptoKey)
        {
            if (CryptoKey == null)
                return;

            using (var Packet = new PacketWriter())
            {
                Packet.WriteShort((byte)ProtocolFunction.ClientSendSymmetricKeyToServer);

                if (CryptoKey is AESKey AESKey)
                {
                    Packet.WriteInt(AESKey.KeySize);
                    Packet.WriteBytes(AESKey.Key);
                    Packet.WriteBytes(AESKey.IV);
                }
                else return;

                Data = Packet.GetPacketData();
            }
        }
    }
}
