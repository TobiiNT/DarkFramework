using System;

namespace DarkNetwork.Connections.Events.Arguments
{
    public class ListenExceptionArgs : EventArgs
    {
        public ListenExceptionArgs(int Port, Exception Exception)
        {
            this.Port = Port;
            this.Exception = Exception;
        }
        public int Port { get; }
        public Exception Exception { get; }
    }
}
