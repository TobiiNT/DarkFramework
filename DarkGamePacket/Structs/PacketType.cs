using DarkGamePacket.Definitions;
using System;

namespace DarkGamePacket
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PacketType : Attribute
    {
        public PacketType(Channel Channel, PacketID PacketID)
        {
            this.PacketID = PacketID;
            this.Channel = Channel;
        }

        public PacketID PacketID { get; }
        public Channel Channel { get; }
    }
}
