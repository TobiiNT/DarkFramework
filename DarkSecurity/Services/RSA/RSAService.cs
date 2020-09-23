using DarkSecurity.Exceptions;
using DarkSecurity.Interfaces.Services;
using DarkSecurity.Services.Keys;
using System;
using System.Security.Cryptography;
using System.Text;

namespace DarkSecurity.Services.RSA
{
    public class RSAService : CryptoKeyManager<RSAKey>, ICryptoService
    {
        public void Encrypt(ref byte[] Data)
        {
            if (this.CryptoKey is RSAKey Key)
            {
                byte[] EncryptedData = EncryptData(Key, Data);

                Data = Encoding.Unicode.GetBytes(Convert.ToBase64String(EncryptedData));
            }
            else throw new EncryptionException(this, this.CryptoKey, Data, new Exception("Invalid key type"));
        }
        public void Decrypt(ref byte[] Data)
        {
            if (CryptoKey is RSAKey Key)
            {
                string RawEncryptedData = Encoding.Unicode.GetString(Data);

                Data = DecryptData(Key, Convert.FromBase64String(RawEncryptedData));
            }
            else throw new DecryptionException(this, this.CryptoKey, Data, new Exception("Invalid key type"));
        }

        private byte[] EncryptData(RSAKey CryptoKey, byte[] Data)
        {
            if (Data == null || Data.Length == 0)
            {
                throw new ArgumentException("Data are empty", "Data");
            }

            if (Data.Length > GetMaxDataLength(CryptoKey.PublicKeySize))
            {
                throw new ArgumentException($"Maximum data length is {GetMaxDataLength(CryptoKey.PublicKeySize)}", "Data");
            }

            if (!IsKeySizeValid(CryptoKey.PublicKeySize))
            {
                throw new ArgumentException("Key size is not valid", "KeySize");
            }

            if (string.IsNullOrEmpty(CryptoKey.PublicKeyXml))
            {
                throw new ArgumentException("Key is null or empty", "PublicKey");
            }

            using (var RSAServiceProvider = new RSACryptoServiceProvider(CryptoKey.PublicKeySize))
            {
                RSAServiceProvider.FromXmlString(CryptoKey.PublicKeyXml);

                return RSAServiceProvider.Encrypt(Data, false);
            }
        }
        private byte[] DecryptData(RSAKey CryptoKey, byte[] Data)
        {
            if (Data == null || Data.Length == 0)
            {
                throw new ArgumentException("Data are empty", "Data");
            }

            if (!IsKeySizeValid(CryptoKey.PrivateKeySize))
            {
                throw new ArgumentException("Key size is not valid", "KeySize");
            }
            
            if (string.IsNullOrEmpty(CryptoKey.PrivateKeyXml))
            {
                throw new ArgumentException("Key is null or empty", "PrivateKey");
            }

            using (var RSAServiceProvider = new RSACryptoServiceProvider(CryptoKey.PrivateKeySize))
            {
                RSAServiceProvider.FromXmlString(CryptoKey.PrivateKeyXml);

                return RSAServiceProvider.Decrypt(Data, false);
            }
        }

        private int GetMaxDataLength(int KeySize) => ((KeySize - 384) / 8) + 37;
        private bool IsKeySizeValid(int KeySize) => KeySize >= 384 && KeySize <= 16384 && KeySize % 8 == 0;
    }
}
