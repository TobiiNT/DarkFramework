using System;
using System.Security.Cryptography;
using System.Text;

namespace DarkSecurity.Securities.RSA
{
    public class RSAService : ICryptoService
    {
        private RSAKeyPair RSAKey { set; get; }
        public RSAService(RSAKeyPair RSAKeyPair) => this.RSAKey = RSAKeyPair;

        public byte[] Encrypt(byte[] PlainData)
        {
            byte[] EncryptedData = EncryptData(PlainData);

            return Encoding.Unicode.GetBytes(Convert.ToBase64String(EncryptedData));
        }

        private byte[] EncryptData(byte[] Data)
        {
            if (Data == null || Data.Length == 0)
                throw new ArgumentException("Data are empty", "Data");

            if (Data.Length > GetMaxDataLength(this.RSAKey.PublicKeySize))
                throw new ArgumentException($"Maximum data length is {GetMaxDataLength(this.RSAKey.PublicKeySize)}", "Data");

            if (!IsKeySizeValid(this.RSAKey.PublicKeySize))
                throw new ArgumentException("Key size is not valid", "KeySize");

            if (string.IsNullOrEmpty(this.RSAKey.PublicKeyXml))
                throw new ArgumentException("Key is null or empty", "PublicKey");

            using (RSACryptoServiceProvider RSAServiceProvider = new RSACryptoServiceProvider(this.RSAKey.PublicKeySize))
            {
                RSAServiceProvider.FromXmlString(this.RSAKey.PublicKeyXml);

                return RSAServiceProvider.Encrypt(Data, false);
            }
        }

        public byte[] Decrypt(byte[] EncryptedData)
        {
            string RawEncryptedData = Encoding.Unicode.GetString(EncryptedData);

            byte[] DecryptedData = DecryptData(Convert.FromBase64String(RawEncryptedData));

            return DecryptedData;
        }
        private byte[] DecryptData(byte[] Data)
        {
            if (Data == null || Data.Length == 0)
                throw new ArgumentException("Data are empty", "Data");

            if (!IsKeySizeValid(this.RSAKey.PrivateKeySize))
                throw new ArgumentException("Key size is not valid", "KeySize");

            if (string.IsNullOrEmpty(this.RSAKey.PrivateKeyXml))
                throw new ArgumentException("Key is null or empty", "PrivateKey");

            using (RSACryptoServiceProvider RSAServiceProvider = new RSACryptoServiceProvider(this.RSAKey.PrivateKeySize))
            {
                RSAServiceProvider.FromXmlString(this.RSAKey.PrivateKeyXml);

                return RSAServiceProvider.Decrypt(Data, false);
            }
        }

        private int GetMaxDataLength(int KeySize) => ((KeySize - 384) / 8) + 37;
        private bool IsKeySizeValid(int KeySize) => KeySize >= 384 && KeySize <= 16384 && KeySize % 8 == 0;
    }
}
