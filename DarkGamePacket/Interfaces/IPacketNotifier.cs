using System;
using System.Collections.Generic;
using System.Text;

namespace DarkGamePacket.Interfaces
{
    public interface IPacketNotifier
    {
        void NotifyChatMessage(uint ClientID, byte MessageType, string Message);

    }
}
