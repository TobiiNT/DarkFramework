using System;

namespace DarkNetwork.Connections.Events.Arguments
{
    public class ReceiveSuccessArgs : EventArgs
    {
        public ReceiveSuccessArgs(int DataSize, byte[] PacketData)
        {
            this.DataSize = DataSize;
            this.PacketData = PacketData;
        }
        public int DataSize { private set; get; }
        public byte[] PacketData;
    }
}
