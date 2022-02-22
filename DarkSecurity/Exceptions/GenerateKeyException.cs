using DarkSecurity.Enums;
using DarkSecurity.Interfaces.Keys;
using System;

namespace DarkSecurity.Exceptions
{
    [Serializable]
    public class GenerateKeyException : Exception
    {
        public GenerateKeyException(ICryptoKeyGenerator CryptoKeyGenerator, ICryptoKey CryptoKey, CryptoKeySize CryptoKeySize, Exception Exception) :
            base($"Exception while {CryptoKeyGenerator} generating {CryptoKey} with {CryptoKeySize}", Exception)
        {
            this.Data.Add("CryptoKeyGenerator", CryptoKeyGenerator);
            this.Data.Add("CryptoKey", CryptoKey);
            this.Data.Add("CryptoKeySize", CryptoKeySize);
        }
    }
}
