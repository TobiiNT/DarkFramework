using System;

namespace DarkNetwork.Networks.Connections.Events.Arguments
{
    public class AcceptExceptionArgs : EventArgs
    {
        public AcceptExceptionArgs(Exception Exception) => this.Exception = Exception;
        public Exception Exception { private set; get; }
    }
}
