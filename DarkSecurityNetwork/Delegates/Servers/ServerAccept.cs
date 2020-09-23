using System;
using System.Net.Sockets;

namespace DarkSecurityNetwork.Delegates.Servers
{
    public delegate void ServerAcceptSuccess(ushort ChannelID, Socket Socket);
    public delegate void ServerAcceptException(ushort ChannelID, Exception Exception);
}
