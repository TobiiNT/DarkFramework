using System;
using System.Security.Cryptography;
using System.Text;

namespace DarkSecurity.Securities.RSA
{
    public class RSAKeyGenerator
    {
        public RSAKeyPair GenerateKey(RSAKeySize RSAKeySize)
        {
            int KeySize = (int)RSAKeySize;
            if (KeySize % 2 != 0 || KeySize < 512)
                throw new Exception("Key should be multiple of two and greater than 512.");
            
            using (RSACryptoServiceProvider RSAprovider = new RSACryptoServiceProvider(KeySize))
            {
                string PublicKeyRaw = RSAprovider.ToXmlString(false);
                string PrivateKeyRaw = RSAprovider.ToXmlString(true);
                string PublicKeyWithSize = IncludeKeyInEncryptionString(PublicKeyRaw, KeySize);
                string PrivateKeyWithSize = IncludeKeyInEncryptionString(PrivateKeyRaw, KeySize);

                RSAKeyPair RSAKeyPair = new RSAKeyPair(PublicKeyWithSize, PrivateKeyWithSize);

                return RSAKeyPair;
            }
            
        }
        private string IncludeKeyInEncryptionString(string PublicKey, int KeySize)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(KeySize.ToString() + "!" + PublicKey));
        }
    }
}
