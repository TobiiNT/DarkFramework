using PacketDefinition.Definitions;
using System;

namespace PacketDefinition
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PacketType : Attribute
    {
        public PacketCmd PacketId { get; }

        public PacketType(PacketCmd PacketID) => this.PacketId = PacketID;
    }
}
