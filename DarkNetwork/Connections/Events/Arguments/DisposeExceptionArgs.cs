using System;

namespace DarkNetwork.Networks.Connections.Events.Arguments
{
    public class DisposeExceptionArgs : EventArgs
    {
        public DisposeExceptionArgs(string Caller, Exception Exception)
        {
            this.Caller = Caller;
            this.Exception = Exception;
        }
        public string Caller { private set; get; }
        public Exception Exception { private set; get; }
    }
}
