using System;

namespace DarkNetwork.Networks.Connections.Events.Arguments
{
    public class ListenExceptionArgs : EventArgs
    {
        public ListenExceptionArgs(Exception Exception) => this.Exception = Exception;
        public Exception Exception { private set; get; }
    }
}
