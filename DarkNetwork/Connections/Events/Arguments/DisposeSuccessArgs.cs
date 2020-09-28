using System;

namespace DarkNetwork.Connections.Events.Arguments
{
    public class DisposeSuccessArgs : EventArgs
    {
        public DisposeSuccessArgs(string Caller) => this.Caller = Caller;
        public string Caller { private set; get; }
    }
}
