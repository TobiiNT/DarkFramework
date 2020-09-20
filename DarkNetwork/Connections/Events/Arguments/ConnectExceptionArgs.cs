using System;

namespace DarkNetwork.Networks.Connections.Events.Arguments
{
    public class ConnectExceptionArgs : EventArgs
    {
        public ConnectExceptionArgs(Exception Exception) => this.Exception = Exception;
        public Exception Exception { private set; get; }
    }
}
