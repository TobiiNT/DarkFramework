using System;

namespace DarkSecurityNetwork.Delegates.Connections
{
    public delegate void ConnectionReceiveSuccess(ushort ChannelID, uint ClientID, int DataSize, byte[] Data);
    public delegate void ConnectionReceiveException(ushort ChannelID, uint ClientID, Exception Exception);
}
