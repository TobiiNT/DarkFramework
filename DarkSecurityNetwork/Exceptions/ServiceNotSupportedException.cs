using DarkSecurityNetwork.Interfaces;
using System;

namespace DarkSecurityNetwork.Exceptions
{
    [Serializable]
    public class ServiceNotSupportedException : Exception
    {
        public ServiceNotSupportedException(string Function, ISecurityProtocol SecurityProtocol) :
                                       base($"{Function} is not support in {SecurityProtocol}")
        {

        }
    }
}
