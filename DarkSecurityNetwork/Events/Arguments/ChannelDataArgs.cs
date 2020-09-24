using System;
using System.Collections.Generic;
using System.Text;

namespace DarkSecurityNetwork.Events.Arguments
{
    public class ChannelDataArgs : EventArgs
    {
        public ChannelDataArgs(ushort ChannelID, uint ClientID)
        {
            this.ChannelID = ChannelID;
            this.ClientID = ClientID;
        }
        public ushort ChannelID { private set; get; }
        public uint ClientID { private set; get; }
    }
}
