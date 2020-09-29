using System;

namespace DarkSecurityNetwork.Events.Arguments
{
    public class SendDataArgs : EventArgs
    {
        public SendDataArgs(byte[] Data) => this.Data = Data;
        public byte[] Data { get; }
    }
}
