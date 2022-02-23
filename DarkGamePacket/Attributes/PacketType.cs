using DarkGamePacket.Definitions;
using DarkGamePacket.Packets;
using System;

namespace DarkGamePacket.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PacketType : Attribute
    {
        public PacketType(PacketDirection Direction, PacketID PacketID)
        {
            this.Direction = Direction;
            this.PacketID = PacketID;
        }

        public PacketDirection Direction { get; }
        public PacketID PacketID { get; }
    }

    public enum PacketDirection
    {
        IN,
        OUT
    }
}
