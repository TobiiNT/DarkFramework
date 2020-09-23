using DarkSecurity.Enums;
using DarkSecurity.Services.AES;
using DarkSecurity.Services.RSA;

namespace DarkSecurityNetwork.Interfaces
{
    public interface ISecurityProtocol
    {
        AESService SymmetricService { get; }
        RSAService AsymmetricService { get; }

        void ImportAsymmetricPublicKey(string RawPublicKey);
        void ImportSymmetricKey(int KeySize, byte[] Key, byte[] IV);

        void GenerateNewAsymmetricKey(CryptoKeySize KeySize);
        void GenerateNewSymmetricKey(CryptoKeySize KeySize);

        void EncryptDataWithAsymmetricPublicKey(ref byte[] Data);

        void DecryptDataWithAsymmetricPrivateKey(ref byte[] Data);

        void EncryptDataWithSymmetricAlgorithm(ref byte[] Data);

        void DecryptDataWithSymmetricAlgorithm(ref byte[] Data);
    }
}