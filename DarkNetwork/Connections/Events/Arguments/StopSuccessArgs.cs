using System;

namespace DarkNetwork.Connections.Events.Arguments
{
    public class StopSuccessArgs : EventArgs
    {
        public StopSuccessArgs(string Caller) => this.Caller = Caller;
        public string Caller { get; }
    }
}
