using System;
using System.Collections.Generic;
using System.Text;

namespace DarkGamePacket.Servers.Interfaces
{
    public interface IServerPacketNotifier
    {
        void NotifyChatMessage(uint ClientID, byte MessageType, string Message);
    }
}
