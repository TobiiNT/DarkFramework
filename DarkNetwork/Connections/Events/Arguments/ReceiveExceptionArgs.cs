using System;

namespace DarkNetwork.Connections.Events.Arguments
{
    public class ReceiveExceptionArgs : EventArgs
    {
        public ReceiveExceptionArgs(Exception Exception) => this.Exception = Exception;
        public Exception Exception { get; }
    }
}
