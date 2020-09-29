using System;

namespace DarkSecurityNetwork.Events.Arguments
{
    public class ChannelDataArgs : EventArgs
    {
        public ChannelDataArgs(ushort ChannelID, uint ClientID)
        {
            this.ChannelID = ChannelID;
            this.ClientID = ClientID;
        }
        public ushort ChannelID { get; }
        public uint ClientID { get; }
    }
}
