using System;

namespace DarkNetwork.Networks.Connections.Events.Arguments
{
    public class SendExceptionArgs : EventArgs
    {
        public SendExceptionArgs(Exception Exception) => this.Exception = Exception;
        public Exception Exception { private set; get; }
    }
}
