using System;

namespace DarkNetwork.Networks.Connections.Events.Arguments
{
    public class ListenSuccessArgs : EventArgs
    {
        public ListenSuccessArgs(int Port) => this.Port = Port;
        public int Port { private set; get; }
    }
}
