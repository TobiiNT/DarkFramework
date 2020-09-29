using System;

namespace DarkNetwork.Connections.Events.Arguments
{
    public class ConnectExceptionArgs : EventArgs
    {
        public ConnectExceptionArgs(Exception Exception) => this.Exception = Exception;
        public Exception Exception { get; }
    }
}
