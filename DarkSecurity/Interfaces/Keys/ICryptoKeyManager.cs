using DarkSecurity.Enums;

namespace DarkSecurity.Interfaces.Keys
{
    public interface ICryptoKeyManager
    {
        ICryptoKey CryptoKey { get; }
        void GenerateKey(CryptoKeySize KeySize, bool ReplaceOldKey);
        void ImportKey(ICryptoKey CryptoKey, bool ReplaceOldKey);
    }
}
