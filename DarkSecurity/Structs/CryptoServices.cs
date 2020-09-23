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
            switch (Key)
            {
                case RSAKey: return new RSAKeyGenerator();
                case AESKey: return new AESKeyGenerator();
                default: return null;
            }
        }
    }
}
