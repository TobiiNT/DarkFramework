using System;

namespace DarkNetwork.Connections.Events.Arguments
{
    public class StopExceptionArgs : EventArgs
    {
        public StopExceptionArgs(string Caller, Exception Exception)
        {
            this.Caller = Caller;
            this.Exception = Exception;
        }
        public string Caller { get; }
        public Exception Exception { get; }
    }
}
