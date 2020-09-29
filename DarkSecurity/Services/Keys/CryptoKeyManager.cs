using DarkSecurity.Enums;
using DarkSecurity.Exceptions;
using DarkSecurity.Interfaces.Keys;
using DarkSecurity.Structs;
using System;

namespace DarkSecurity.Services.Keys
{
    public class CryptoKeyManager<T> : ICryptoKeyManager where T : ICryptoKey 
    {
        public ICryptoKey CryptoKey { private set; get; }

        public void GenerateKey(CryptoKeySize KeySize, bool ReplaceOldKey)
        {
            var KeyGenerator = CryptoServices.GetGenerator<T>();
            try
            {
                if (KeyGenerator == null)
                {
                    throw new GenerateKeyException(KeyGenerator, CryptoKey, KeySize, new Exception("Unsupported key generator"));
                }

                this.ImportKey(KeyGenerator.GenerateKey(KeySize), ReplaceOldKey);
            }
            catch (Exception Exception)
            {
                throw new GenerateKeyException(KeyGenerator, CryptoKey, KeySize, Exception);
            }
        }

        public void ImportKey(ICryptoKey CryptoKey, bool ReplaceOldKey)
        {
            if (CryptoKey is T)
            {
                if (this.CryptoKey == null || ReplaceOldKey)
                {
                    this.CryptoKey = CryptoKey;
                }
                else throw new ImportKeyException(CryptoKey, new Exception("Current key has already been generated"));
            }
            else throw new ImportKeyException(CryptoKey, new Exception("Invalid key type"));
        }
    }
}
