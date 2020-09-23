using System;

namespace DarkSecurityNetwork.Delegates.Connections
{
    public delegate void ConnectionConnectSuccess(ushort ChannelID, uint ClientID);
    public delegate void ConnectionConnectException(ushort ChannelID, uint ClientID, Exception Exception);
}
