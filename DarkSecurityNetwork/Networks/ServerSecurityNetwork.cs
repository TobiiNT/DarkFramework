using DarkPacket.Readers;
using DarkSecurity.Enums;
using DarkSecurityNetwork.Enums;
using DarkSecurityNetwork.Events.Arguments;
using DarkSecurityNetwork.Exceptions;
using DarkSecurityNetwork.Interfaces;
using DarkSecurityNetwork.Networks.Packets;
using DarkSecurityNetwork.Protocols;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace DarkSecurityNetwork.Networks
{
    public class ServerSecurityNetwork : ServerSecurityProtocol, ISecurityNetwork
    {
        private CryptoKeySize SymmetricKeySize { set; get; }
        public void SetKeySize(CryptoKeySize AsymmetricKeySize, CryptoKeySize SymmetricKeySize, int MessageTestLength)
        {
            this.GenerateNewAsymmetricKey(AsymmetricKeySize);
            this.SymmetricKeySize = SymmetricKeySize;
            this.MessageTestLength = MessageTestLength;
        }
        private byte[] MessageTest { set; get; }
        private int MessageTestLength { set; get; }
        public bool AuthenticationSuccess { private set; get; }

        public void ManagePacket(byte[] Data)
        {
            try
            {
                this.DecryptDataWithAsymmetricPrivateKey(ref Data);

                using (var Packet = new NormalPacketReader(Data))
                {
                    var Function = (ProtocolFunction)Packet.ReadShort();

                    switch (Function)
                    {
                        case ProtocolFunction.ClientSendSymmetricKeyToServer:
                            {
                                var KeySize = Packet.ReadInt();
                                var Key = Packet.ReadBytes();
                                var IV = Packet.ReadBytes();

                                this.ImportSymmetricKeyFromClient(KeySize, Key, IV);
                                this.SendRandomMessageTest();
                            }
                            break;

                        case ProtocolFunction.ClientSendMessageTestVerify:
                            {
                                var MessageVerify = Packet.ReadBytes();

                                this.VerifyMessageTest(MessageVerify);
                            }
                            break;

                        default:
                            OnAuthFailed(this, new AuthFailedArgs("Manage packet", "Invalid packet"));
                            break;
                    }
                }
            }
            catch (Exception Exception)
            {
                OnAuthException(this, new AuthExceptionArgs(Exception));
            }
        }

        public void SendAsymmetricPublicKeyAndChannelInfoToClient(ushort ChannelID, uint ClientID)
        {
            try
            {
                var Packet = new PacketServerSendAsymmetricKey(this.AsymmetricService.CryptoKey, ChannelID, ClientID, this.SymmetricKeySize).Data;

                if (Packet != null)
                {
                    OnSendData(this, new SendDataArgs(Packet));
                }
                else
                {
                    OnAuthFailed(this, new AuthFailedArgs("Send asymmetric key to client", "Invalid asymmetric key"));
                }
            }
            catch (Exception Exception)
            {
                OnAuthException(this, new AuthExceptionArgs(Exception));
            }
        }

        public void ImportSymmetricKeyFromClient(int KeySize, byte[] Key, byte[] IV)
        {
            try
            {
                if ((int)this.SymmetricKeySize == KeySize)
                {
                    this.ImportSymmetricKey(KeySize, Key, IV);
                }
                else
                {
                    OnAuthFailed(this, new AuthFailedArgs("Import symmetric key from client", "Invalid symmetric key size"));
                }
            }
            catch (Exception Exception)
            {
                OnAuthException(this, new AuthExceptionArgs(Exception));
            }
        }

        public void SendRandomMessageTest()
        {
            try
            {
                var data = new byte[4];
                new RNGCryptoServiceProvider().GetBytes(data);
                var Randomizer = new Random(BitConverter.ToInt32(data, 0));

                this.MessageTest = new byte[MessageTestLength];
                Randomizer.NextBytes(this.MessageTest);

                var MessageData = this.MessageTest;

                this.EncryptDataWithSymmetricAlgorithm(ref MessageData);

                var Packet = new PacketServerSendMessageTest(MessageData).Data;

                if (Packet != null)
                {
                    OnSendData(this, new SendDataArgs(Packet));
                }
                else
                {
                    OnAuthFailed(this, new AuthFailedArgs("Send message test to client", "Invalid symmetric key"));
                }
            }
            catch (Exception Exception)
            {
                OnAuthException(this, new AuthExceptionArgs(Exception));
            }
        }

        public void VerifyMessageTest(byte[] MessageTest)
        {
            try
            {
                this.DecryptDataWithSymmetricAlgorithm(ref MessageTest);
                this.DecryptDataWithSymmetricAlgorithm(ref MessageTest);

                if (this.MessageTest.SequenceEqual(MessageTest))
                {
                    this.CompleteAuthentication();
                }
                else
                {
                    OnAuthFailed(this, new AuthFailedArgs("Verify message test from client", "Invalid message test"));
                }
            }
            catch (Exception Exception)
            {
                OnAuthException(this, new AuthExceptionArgs(Exception));
            }
        }

        public void CompleteAuthentication()
        {
            if (!this.AuthenticationSuccess)
            {
                this.AuthenticationSuccess = true;

                var Packet = new PacketServerSendAuthenticationComplete().Data;

                if (Packet != null)
                {
                    OnSendData(this, new SendDataArgs(Packet));
                }
            }

            if (this.AuthenticationSuccess)
            {
                OnAuthSuccess(this, new AuthSuccessArgs());
            }
        }

        public void ImportAsymmetricKeyFromServer(string RawPublicKey) => throw new ServiceNotSupportedException("ImportAsymmetricKeyFromServer", this);
        public void SendSymmetricKeyToServer() => throw new ServiceNotSupportedException("SendSymmetricKeyToServer", this);
    }
}
