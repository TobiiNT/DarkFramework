using DarkSecurity.Enums;
using DarkSecurity.Exceptions;
using DarkSecurity.Interfaces.Keys;
using System;
using System.Security.Cryptography;

namespace DarkSecurity.Services.AES
{
    public class AESKeyGenerator : ICryptoKeyGenerator
    {
        public ICryptoKey GenerateKey(CryptoKeySize AESKeySize)
        {
            var KeySize = (int)AESKeySize;
            if (KeySize % 2 != 0 || KeySize > 256)
            {
                throw new GenerateKeyException(this, null, AESKeySize, new Exception("Key should be multiple of two and smaller than 256."));
            }

            try
            {
                using (var AES = new AesManaged())
                {
                    AES.KeySize = KeySize;

                    AES.GenerateKey();
                    AES.GenerateIV();

                    return new AESKey(AES.KeySize, AES.Key, AES.IV);
                }
            }
            catch (Exception Exception)
            {
                throw new GenerateKeyException(this, null, AESKeySize, Exception);
            }
        }
    }
}
