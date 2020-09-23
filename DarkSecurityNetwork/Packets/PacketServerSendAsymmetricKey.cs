using DarkPacket.Packets;
using DarkSecurity.Interfaces.Keys;
using DarkSecurityNetwork.Enums;
using DarkSecurity.Services.RSA;

namespace DarkSecurityNetwork.Packets
{
    public class PacketServerSendAsymmetricKey
    {
        public byte[] Data { private set; get; }
        public PacketServerSendAsymmetricKey(ICryptoKey CryptoKey)
        {
            using (var Packet = new PacketWriter())
            {
                Packet.WriteShort((byte)ProtocolFunction.ServerSendAsymmetricKeyToClient);

                if (CryptoKey is RSAKey RSAKey)
                {
                    Packet.WriteString(RSAKey.PublicKeyRaw);
                }
                else return;

                Data = Packet.GetPacketData();
            }
        }
    }
}
