using DarkPacket.Readers;
using DarkSecurity.Enums;
using DarkSecurityNetwork.Enums;
using DarkSecurityNetwork.Events.Arguments;
using DarkSecurityNetwork.Exceptions;
using DarkSecurityNetwork.Interfaces;
using DarkSecurityNetwork.Packets;
using DarkSecurityNetwork.Protocols;
using System;

namespace DarkSecurityNetwork.Networks
{
    public class ClientSecurityNetwork : ClientSecurityProtocol, ISecurityNetwork
    {
        public ClientSecurityNetwork(CryptoKeySize KeySize)
        {
            this.GenerateNewSymmetricKey(KeySize);
        }
        
        public bool AuthenticationSuccess { private set; get; }

        public void ManagePacket(byte[] Data)
        {
            try
            {
                using (var Packet = new NormalPacketReader(Data))
                {
                    ProtocolFunction Function = (ProtocolFunction)Packet.ReadShort();

                    switch (Function)
                    {
                        case ProtocolFunction.ServerSendAsymmetricKeyToClient:
                            {
                                ushort ChannelID = Packet.ReadUShort();
                                uint ClientID = Packet.ReadUInt();
                                string RawPublicKey = Packet.ReadString();

                                this.ImportChannelAndClientData(ChannelID, ClientID);
                                this.ImportAsymmetricKeyFromServer(RawPublicKey);
                                this.SendSymmetricKeyToServer();
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
                byte[] Packet = new PacketClientSendSymmetricKey(this.SymmetricService.CryptoKey).Data;

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