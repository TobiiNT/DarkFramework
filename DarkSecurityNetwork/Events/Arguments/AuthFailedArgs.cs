using System;

namespace DarkSecurityNetwork.Events.Arguments
{
    public class AuthFailedArgs : EventArgs
    {
        public AuthFailedArgs(string Function, string Reason)
        {
            this.Function = Function;
            this.Reason = Reason;
        }
        public string Function { get; }
        public string Reason { get; }
    }
}
