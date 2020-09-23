using System;

namespace DarkSecurityNetwork.Delegates.Connections
{
    public delegate void ConnectionSendSuccess(ushort ChannelID, uint ClientID, int DataSize);
    public delegate void ConnectionSendException(ushort ChannelID, uint ClientID, Exception Exception);
}
