using System;

namespace DarkSecurityNetwork.Delegates.Connections
{
    public delegate void ConnectionStartSuccess(ushort ChannelID, uint ClientID);
    public delegate void ConnectionStartException(ushort ChannelID, uint ClientID, Exception Exception);
}
