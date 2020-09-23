using System;

namespace DarkSecurityNetwork.Delegates.Connections
{
    public delegate void AuthenticationSuccess(ushort ChannelID, uint ClientID);
    public delegate void AuthenticationFailed(ushort ChannelID, uint ClientID);
    public delegate void AuthenticationException(ushort ChannelID, uint ClientID, Exception Exception);
}
