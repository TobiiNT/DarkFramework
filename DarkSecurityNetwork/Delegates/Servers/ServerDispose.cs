using System;

namespace DarkSecurityNetwork.Delegates.Servers
{
    public delegate void ServerDisposeSuccess(ushort ChannelID, string Caller);
    public delegate void ServerDisposeException(ushort ChannelID, string Caller, Exception Exception);
}
