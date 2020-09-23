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
    public class ServerSecurityNetwork : ServerSecurityProtocol, ISecurityNetwork
    {
        public ServerSecurityNetwork()
        {
            this.GenerateNewAsymmetricKey(CryptoKeySize.Key1024);
        }

        public bool AuthenticationSuccess { private set; get; }

        public void ManagePacket(byte[] Data)
        {
            try
            {
                this.DecryptDataWithAsymmetricPrivateKey(ref Data);

                using (var Packet = new NormalPacketReader(Data))
                {
                    ProtocolFunction Function = (ProtocolFunction)Packet.ReadShort();

                    switch (Function)
                    {
                        case ProtocolFunction.ClientSendSymmetricKeyToServer:
                            {
                                int KeySize = Packet.ReadInt();
                                byte[] Key = Packet.ReadBytes();
                                byte[] IV = Packet.ReadBytes();

                                this.ImportSymmetricKeyFromClient(KeySize, Key, IV);
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

        public void SendAsymmetricPublicKeyToClient()
        {
            try
            {
                byte[] Packet = new PacketServerSendAsymmetricKey(this.AsymmetricService.CryptoKey).Data;

                if (Packet != null)
                {
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

        public void ImportSymmetricKeyFromClient(int KeySize, byte[] Key, byte[] IV)
        {
            try
            {
                this.ImportSymmetricKey(KeySize, Key, IV);

                this.CompleteAuthentication();
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

                byte[] Packet = new PacketServerSendAuthenticationComplete().Data;

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
