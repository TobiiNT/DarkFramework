using System;

namespace DarkNetwork.Connections.Events.Arguments
{
    public class SendExceptionArgs : EventArgs
    {
        public SendExceptionArgs(Exception Exception) => this.Exception = Exception;
        public Exception Exception { get; }
    }
}
