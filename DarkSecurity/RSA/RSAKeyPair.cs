using System;
using System.Text;

namespace DarkSecurity.Securities.RSA
{
    public struct RSAKeyPair
    {
        public RSAKeyPair(string RawPublicKey, string RawPrivateKey)
        {
            this.PublicKeyRaw = RawPublicKey;
            this.PrivateKeyRaw = RawPrivateKey;
            this.PublicKeyXml = string.Empty;
            this.PrivateKeyXml = string.Empty;
            this.PublicKeySize = 0;
            this.PrivateKeySize = 0;

            if (this.PrivateKeyXml != null && this.PrivateKeyXml.Length > 0)
            {
                var KeyData = Encoding.Unicode.GetString(Convert.FromBase64String(this.PrivateKeyXml));
                if (KeyData.Contains("!"))
                {
                    var KeyArrays = KeyData.Split('!', 2);
                    try
                    {
                        this.PrivateKeySize = int.Parse(KeyArrays[0]);
                        this.PrivateKeyXml = KeyArrays[1];
                    }
                    catch (Exception Exception)
                    {
                        throw Exception;
                    }
                }
            }
            if (this.PublicKeyXml != null && this.PublicKeyXml.Length > 0)
            {
                var KeyData = Encoding.Unicode.GetString(Convert.FromBase64String(this.PublicKeyXml));
                if (KeyData.Contains("!"))
                {
                    var KeyArrays = KeyData.Split('!', 2);
                    try
                    {
                        this.PublicKeySize = int.Parse(KeyArrays[0]);
                        this.PublicKeyXml = KeyArrays[1];
                    }
                    catch (Exception Exception)
                    {
                        throw Exception;
                    }
                }
            }
        }
        public string PublicKeyRaw { set; get; }
        public string PrivateKeyRaw { set; get; }
        public string PublicKeyXml { get; set; }
        public string PrivateKeyXml { get; set; }     
        public int PublicKeySize { set; get; }
        public int PrivateKeySize { set; get; }
    }
}
