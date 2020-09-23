using DarkSecurity.Exceptions;
using DarkSecurity.Services.Keys;
using System;
using System.IO;
using System.Security.Cryptography;
using DarkSecurity.Interfaces.Services;

namespace DarkSecurity.Services.AES
{
    public class AESService : CryptoKeyManager<AESKey>, ICryptoService
    {
        public void Encrypt(ref byte[] Data)
        {
            if (this.CryptoKey is AESKey Key)
            {
                try
                {
                    using (var AES = this.CreateCryptoraphyService(Key))
                    {
                        using (var Encryptor = AES.CreateEncryptor(AES.Key, AES.IV))
                        {
                            Data = PerformCryptography(Data, Encryptor);
                        }
                    }
                }
                catch (Exception Exception)
                {
                    throw new EncryptionException(this, this.CryptoKey, Data, Exception);
                }

            }
            else throw new EncryptionException(this, this.CryptoKey, Data, new Exception("Invalid key type"));
        }

        public void Decrypt(ref byte[] Data)
        {
            if (this.CryptoKey is AESKey Key)
            {
                try
                {
                    using (var AES = this.CreateCryptoraphyService(Key))
                    {
                        using (var Decryptor = AES.CreateDecryptor(AES.Key, AES.IV))
                        {
                            Data = PerformCryptography(Data, Decryptor);
                        }
                    }
                }
                catch (Exception Exception)
                {
                    throw new DecryptionException(this, this.CryptoKey, Data, Exception);
                }
            }
            else throw new DecryptionException(this, this.CryptoKey, Data, new Exception("Invalid key type"));
        }

        private Aes CreateCryptoraphyService(AESKey Key)
        {
            Aes AES = Aes.Create();

            AES.KeySize = Key.KeySize;
            AES.Key = Key.Key;
            AES.IV = Key.IV;

            return AES;
        }
        private byte[] PerformCryptography(byte[] Data, ICryptoTransform CryptoTransform)
        {
            using (MemoryStream MemoryStream = new MemoryStream())
            {
                using (CryptoStream CryptoStream = new CryptoStream(MemoryStream, CryptoTransform, CryptoStreamMode.Write))
                {
                    CryptoStream.Write(Data, 0, Data.Length);
                    CryptoStream.FlushFinalBlock();

                    return MemoryStream.ToArray();
                }
            }
        }
    }
}
