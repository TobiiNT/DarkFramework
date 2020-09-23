using DarkSecurity.Interfaces.Keys;
using DarkSecurity.Interfaces.Services;
using System;

namespace DarkSecurity.Exceptions
{
    [Serializable]
    public class DecryptionException : Exception
    {
        public DecryptionException(ICryptoService CryptoService, ICryptoKey CryptoKey, byte[] Data, Exception Exception) :
            base($"Exception while decrypting with {CryptoService} and {CryptoKey}", Exception)
        {
            this.Data.Add("CryptoService", CryptoService);
            this.Data.Add("CryptoKey", CryptoKey);
            this.Data.Add("Data", Data);
        }
    }
}
