using DarkSecurity.Enums;

namespace DarkSecurity.Interfaces.Keys
{
    public interface ICryptoKeyGenerator
    {
        ICryptoKey GenerateKey(CryptoKeySize KeySize);
    }
}
