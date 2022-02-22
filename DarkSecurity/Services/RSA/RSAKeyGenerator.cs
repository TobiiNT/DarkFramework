using DarkSecurity.Enums;
using DarkSecurity.Exceptions;
using DarkSecurity.Interfaces.Keys;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DarkSecurity.Services.RSA
{
    public class RSAKeyGenerator : ICryptoKeyGenerator
    {
        public ICryptoKey GenerateKey(CryptoKeySize RSAKeySize)
        {
            var KeySize = (int)RSAKeySize;
            if (KeySize % 2 != 0 || KeySize < 512)
            {
                throw new GenerateKeyException(this, null, RSAKeySize, new Exception("Key should be multiple of two and greater than 512."));
            }

            try
            {
                using var RSAprovider = new RSACryptoServiceProvider(KeySize);
                var PublicKeyRaw = RSAprovider.ToXmlString(false);
                var PrivateKeyRaw = RSAprovider.ToXmlString(true);
                var PublicKeyWithSize = IncludeKeyInEncryptionString(PublicKeyRaw, KeySize);
                var PrivateKeyWithSize = IncludeKeyInEncryptionString(PrivateKeyRaw, KeySize);

                return new RSAKey(PublicKeyWithSize, PrivateKeyWithSize);
            }
            catch (Exception Exception)
            {
                throw new GenerateKeyException(this, null, RSAKeySize, Exception);
            }
        }
        private string IncludeKeyInEncryptionString(string PublicKey, int KeySize)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(KeySize.ToString() + "!" + PublicKey));
        }
    }
}
