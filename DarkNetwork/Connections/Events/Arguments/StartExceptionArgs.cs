using System;

namespace DarkNetwork.Networks.Connections.Events.Arguments
{
    public class StartExceptionArgs : EventArgs
    {
        public StartExceptionArgs(Exception Exception) => this.Exception = Exception;
        public Exception Exception { private set; get; }
    }
}
