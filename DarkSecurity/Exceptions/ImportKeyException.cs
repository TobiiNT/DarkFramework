using DarkSecurity.Interfaces.Keys;
using System;

namespace DarkSecurity.Exceptions
{
    [Serializable]
    public class ImportKeyException : Exception
    {
        public ImportKeyException(ICryptoKey CryptoKey, Exception Exception) :
            base($"Exception while importing key {CryptoKey}", Exception)
        {

        }
    }
}
