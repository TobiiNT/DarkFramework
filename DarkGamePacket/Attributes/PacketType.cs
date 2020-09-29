using DarkGamePacket.Enums;
using System;

namespace DarkGamePacket.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PacketType : Attribute
    {
        public PacketType(PacketID PacketID)
        {
            this.PacketID = PacketID;
        }

        public PacketID PacketID { get; }
    }
}
