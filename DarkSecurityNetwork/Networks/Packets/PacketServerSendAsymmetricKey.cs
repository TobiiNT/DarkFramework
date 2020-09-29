using DarkPacket.Writer;
using DarkSecurity.Interfaces.Keys;
using DarkSecurityNetwork.Enums;
using DarkSecurity.Services.RSA;
using DarkSecurity.Enums;

namespace DarkSecurityNetwork.Networks.Packets
{
    public class PacketServerSendAsymmetricKey
    {
        public byte[] Data { get; }
        public PacketServerSendAsymmetricKey(ICryptoKey CryptoKey, ushort ChannelID, uint ClientID, CryptoKeySize SymmetricKeySize)
        {
            using (var Packet = new PacketWriter())
            {
                Packet.WriteShort((byte)ProtocolFunction.ServerSendAsymmetricKeyToClient);
                Packet.WriteUShort(ChannelID);
                Packet.WriteUInt(ClientID);

                if (CryptoKey is RSAKey RSAKey)
                {
                    Packet.WriteString(RSAKey.PublicKeyRaw);
                }
                else return;

                Packet.WriteUInt((ushort)SymmetricKeySize);

                Data = Packet.GetPacketData();
            }
        }
    }
}
