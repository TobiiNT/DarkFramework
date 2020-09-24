using System;
using System.Collections.Generic;
using System.Text;

namespace DarkGamePacket.Interfaces
{
    public interface IPacket
    {
        byte[] GetPacketData();
    }
}
