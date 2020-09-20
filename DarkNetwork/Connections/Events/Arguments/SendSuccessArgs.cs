using System;

namespace DarkNetwork.Networks.Connections.Events.Arguments
{
    public class SendSuccessArgs : EventArgs
    {
        public SendSuccessArgs(int DataSize) => this.DataSize = DataSize;
        public int DataSize { private set; get; }
    }
}
