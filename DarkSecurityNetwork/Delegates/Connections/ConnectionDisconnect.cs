using System;

namespace DarkSecurityNetwork.Delegates.Connections
{
    public delegate void ConnectionDisconnectSuccess(ushort ChannelID, uint ClientID);
    public delegate void ConnectionDisconnectException(ushort ChannelID, uint ClientID, Exception Exception);
}
