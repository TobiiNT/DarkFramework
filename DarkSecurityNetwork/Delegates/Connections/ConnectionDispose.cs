using System;

namespace DarkSecurityNetwork.Delegates.Connections
{
    public delegate void ConnectionDisposeSuccess(ushort ChannelID, uint ClientID, string Caller);
    public delegate void ConnectionDisposeException(ushort ChannelID, uint ClientID, string Caller, Exception Exception);
}
