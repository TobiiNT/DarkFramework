using System;

namespace DarkSecurityNetwork.Delegates.Servers
{
    public delegate void ServerListenSuccess(ushort ChannelID, int Port);
    public delegate void ServerListenException(ushort ChannelID, int Port, Exception Exception);
}
