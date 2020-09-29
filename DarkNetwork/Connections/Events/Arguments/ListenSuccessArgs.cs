using System;

namespace DarkNetwork.Connections.Events.Arguments
{
    public class ListenSuccessArgs : EventArgs
    {
        public ListenSuccessArgs(int Port) => this.Port = Port;
        public int Port { get; }
    }
}
