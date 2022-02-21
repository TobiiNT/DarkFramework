using System;

namespace DarkNetwork.Connections.Events.Arguments
{
    public class DisconnectExceptionArgs : EventArgs
    {
        public DisconnectExceptionArgs(Exception Exception)
        {
            this.Exception = Exception;
        }
        public Exception Exception { get; }
    }
}
