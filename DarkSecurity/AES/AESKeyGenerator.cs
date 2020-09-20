using System.Security.Cryptography;

namespace DarkSecurity.Securities.AES
{
    public class AESKeyGenerator
    {
        public AESKeyPair GenerateKey(AESKeySize AESKeySize)
        {
            using (AesManaged AES = new AesManaged())
            {
                AES.KeySize = (int)AESKeySize;
                AES.Padding = PaddingMode.PKCS7;

                AES.GenerateKey();
                AES.GenerateIV();

                AESKeyPair AESKeyPair = new AESKeyPair(AES.KeySize, AES.Key, AES.IV);

                return AESKeyPair;
            }
        }
    }
}
