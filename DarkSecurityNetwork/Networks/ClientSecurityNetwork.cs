using DarkPacket.Readers;
using DarkSecurity.Enums;
using DarkSecurityNetwork.Enums;
using DarkSecurityNetwork.Events.Arguments;
using DarkSecurityNetwork.Exceptions;
using DarkSecurityNetwork.Interfaces;
using DarkSecurityNetwork.Networks.Packets;
using DarkSecurityNetwork.Protocols;
using System;

namespace DarkSecurityNetwork.Networks
{
    public class ClientSecurityNetwork : ClientSecurityProtocol, ISecurityNetwork
    {
        public bool AuthenticationSuccess { private set; get; }

        public void ManagePacket(byte[] Data)
        {
            try
            {
                using (var Packet = new NormalPacketReader(Data))
                {
                    var Function = (ProtocolFunction)Packet.ReadShort();

                    switch (Function)
                    {
                        case ProtocolFunction.ServerSendAsymmetricKeyToClient:
                            {
                                var ChannelID = Packet.ReadUShort();
                                var ClientID = Packet.ReadUInt();
                                var RawPublicKey = Packet.ReadString();
                                var SymmetricKeySize = (CryptoKeySize)Packet.ReadUInt();

                                this.GenerateNewSymmetricKey(SymmetricKeySize);
                                this.ImportChannelAndClientData(ChannelID, ClientID);
                                this.ImportAsymmetricKeyFromServer(RawPublicKey);
                                this.SendSymmetricKeyToServer();
                            }
                            break;

                        case ProtocolFunction.ServerSendMessageTest:
                            {
                                var Message = Packet.ReadBytes();

                                this.SendMessageTestVerifyToServer(Message);
                            }
                            break;

                        case ProtocolFunction.ServerSendAuthenticationComplete:
                            {
                                this.CompleteAuthentication();
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

        public void ImportChannelAndClientData(ushort ChannelID, uint ClientID)
        {
            try
            {
                OnChannelData(this, new ChannelDataArgs(ChannelID, ClientID));
            }
            catch (Exception Exception)
            {
                OnAuthException(this, new AuthExceptionArgs(Exception));
            }
        }

        public void ImportAsymmetricKeyFromServer(string RawPublicKey)
        {
            try
            {
                this.ImportAsymmetricPublicKey(RawPublicKey);
            }
            catch (Exception Exception)
            {
                OnAuthException(this, new AuthExceptionArgs(Exception));
            }
        }

        public void SendSymmetricKeyToServer()
        {
            try
            {
                var Packet = new PacketClientSendSymmetricKey(this.SymmetricService.CryptoKey).Data;

                if (Packet != null)
                {
                    this.EncryptDataWithAsymmetricPublicKey(ref Packet);

                    OnSendData(this, new SendDataArgs(Packet));
                }
                else
                {
                    OnAuthFailed(this, new AuthFailedArgs("Send symmetric key to server", "Invalid symmetric key"));
                }
            }
            catch (Exception Exception)
            {
                OnAuthException(this, new AuthExceptionArgs(Exception));
            }
        }

        public void SendMessageTestVerifyToServer(byte[] Data)
        {
            try
            {
                this.EncryptDataWithSymmetricAlgorithm(ref Data);

                var Packet = new PacketClientSendMessageTestVerify(Data).Data;

                if (Packet != null)
                {
                    this.EncryptDataWithAsymmetricPublicKey(ref Packet);

                    OnSendData(this, new SendDataArgs(Packet));
                }
                else
                {
                    OnAuthFailed(this, new AuthFailedArgs("Send message test verify to server", "Invalid symmetric key"));
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
            }

            if (this.AuthenticationSuccess)
            {
                OnAuthSuccess(this, new AuthSuccessArgs());
            }
        }

        public void ImportSymmetricKeyFromClient(int KeySize, byte[] Key, byte[] IV) => throw new ServiceNotSupportedException("ImportSymmetricKeyFromClient", this);
        public void SendAsymmetricPublicKeyAndChannelInfoToClient(ushort ChannelID, uint ClientID) => throw new ServiceNotSupportedException("SendAsymmetricPublicKeyAndChannelInfoToClient", this);
    }
}