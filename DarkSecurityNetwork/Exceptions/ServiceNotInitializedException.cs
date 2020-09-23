using DarkSecurity.Interfaces.Services;
using System;

namespace DarkSecurityNetwork.Exceptions
{
    [Serializable]
    public class ServiceNotInitializedException : Exception
    {
        public ServiceNotInitializedException(ICryptoService CryptoService, Exception Exception) :
            base($"{CryptoService} is not initialized", Exception)
        {

        }
    }
}
