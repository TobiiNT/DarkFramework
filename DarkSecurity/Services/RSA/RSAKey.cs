using DarkSecurity.Interfaces.Keys;
using System;
using System.Text;

namespace DarkSecurity.Services.RSA
{
    public struct RSAKey : ICryptoKey
    {
        public RSAKey(string RawPublicKey)
        {
            this.PublicKeyRaw = RawPublicKey;
            this.PrivateKeyRaw = string.Empty;
            this.PublicKeyXml = string.Empty;
            this.PrivateKeyXml = string.Empty;
            this.PublicKeySize = 0;
            this.PrivateKeySize = 0;

            this.ExtractKeyXml();
        }

        public RSAKey(string RawPublicKey, string RawPrivateKey)
        {
            this.PublicKeyRaw = RawPublicKey;
            this.PrivateKeyRaw = RawPrivateKey;
            this.PublicKeyXml = string.Empty;
            this.PrivateKeyXml = string.Empty;
            this.PublicKeySize = 0;
            this.PrivateKeySize = 0;

            this.ExtractKeyXml();
        }

        private void ExtractKeyXml()
        {
            if (this.PrivateKeyRaw != null && this.PrivateKeyRaw.Length > 0)
            {
                var KeyData = Encoding.Unicode.GetString(Convert.FromBase64String(this.PrivateKeyRaw));
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
            if (this.PublicKeyRaw != null && this.PublicKeyRaw.Length > 0)
            {
                var KeyData = Encoding.Unicode.GetString(Convert.FromBase64String(this.PublicKeyRaw));
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

        public string PublicKeyRaw { get; }
        public string PrivateKeyRaw { get; }
        public string PublicKeyXml { private set; get; }
        public string PrivateKeyXml { private set; get; }
        public int PublicKeySize { private set; get; }
        public int PrivateKeySize { private set; get; }
    }
}
