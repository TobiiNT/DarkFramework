using System;

namespace DarkNetwork.Connections.Events.Arguments
{
    public class DisposeExceptionArgs : EventArgs
    {
        public DisposeExceptionArgs(string Caller, Exception Exception)
        {
            this.Caller = Caller;
            this.Exception = Exception;
        }
        public string Caller { get; }
        public Exception Exception { get; }
    }
}
