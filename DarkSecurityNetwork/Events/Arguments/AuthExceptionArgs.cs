using System;

namespace DarkSecurityNetwork.Events.Arguments
{
    public class AuthExceptionArgs : EventArgs
    {
        public AuthExceptionArgs(Exception Exception) => this.Exception = Exception;
        public Exception Exception { private set; get; }
    }
}
