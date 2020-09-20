using System.IO;
using System.Security.Cryptography;

namespace DarkSecurity.Securities.AES
{
    public class AESService : ICryptoService
    {
        private AESKeyPair AESKey { set; get; }
        public AESService(AESKeyPair AESKeyPair) => this.AESKey = AESKeyPair;

        public byte[] Encrypt(byte[] Data)
        {
            using (Aes AES = Aes.Create())
            {
                AES.KeySize = AESKey.KeySize;
                AES.Key = AESKey.Key;
                AES.IV = AESKey.IV;

                using (ICryptoTransform Encryptor = AES.CreateEncryptor(AES.Key, AES.IV))
                {
                    return PerformCryptography(Data, Encryptor);
                }
            }
        }

        public byte[] Decrypt(byte[] Data)
        {
            using (Aes AES = Aes.Create())
            {
                AES.KeySize = AESKey.KeySize;
                AES.Key = AESKey.Key;
                AES.IV = AESKey.IV;

                using (ICryptoTransform Decryptor = AES.CreateDecryptor(AES.Key, AES.IV))
                {
                    return PerformCryptography(Data, Decryptor);
                }
            }
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
