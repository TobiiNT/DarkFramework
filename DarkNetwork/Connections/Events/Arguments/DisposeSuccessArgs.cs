using System;

namespace DarkNetwork.Networks.Connections.Events.Arguments
{
    public class DisposeSuccessArgs : EventArgs
    {
        public DisposeSuccessArgs(string Caller) => this.Caller = Caller;
        public string Caller { private set; get; }
    }
}
