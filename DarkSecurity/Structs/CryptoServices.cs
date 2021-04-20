using DarkSecurity.Interfaces.Keys;
using DarkSecurity.Services.AES;
using DarkSecurity.Services.RSA;

namespace DarkSecurity.Structs
{
    public class CryptoServices
    {
        public static ICryptoKeyGenerator GetGenerator<T>()
        {
            T Key = default;
            return Key switch
            {
                RSAKey => new RSAKeyGenerator(),
                AESKey => new AESKeyGenerator(),
                _ => null,
            };
        }
    }
}
