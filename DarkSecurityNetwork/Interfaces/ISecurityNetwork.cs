namespace DarkSecurityNetwork.Interfaces
{
    public interface ISecurityNetwork : ISecurityNetworkEvent, ISecurityProtocol
    {
        bool AuthenticationSuccess { get; }
        void ManagePacket(byte[] Data);

        void ImportAsymmetricKeyFromServer(string RawPublicKey);
        void ImportSymmetricKeyFromClient(int KeySize, byte[] Key, byte[] IV);
        void SendSymmetricKeyToServer();
        void SendAsymmetricPublicKeyToClient();
        void CompleteAuthentication();
    }
}
