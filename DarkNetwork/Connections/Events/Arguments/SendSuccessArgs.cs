using System;

namespace DarkNetwork.Connections.Events.Arguments
{
    public class SendSuccessArgs : EventArgs
    {
        public SendSuccessArgs(int DataSize) => this.DataSize = DataSize;
        public int DataSize { get; }
    }
}
