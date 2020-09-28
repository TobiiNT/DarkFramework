using DarkGamePacket.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkGamePacket.Interfaces
{
    public interface ICoreMessage 
    {
        PacketID PacketID { get; set; }
    }
}
