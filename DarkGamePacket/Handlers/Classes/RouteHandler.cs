using DarkGamePacket.Definitions;
using DarkGamePacket.Handlers.Delegates;
using DarkPacket.Handlers;
using DarkPacket.Interfaces;
using System.Collections.Generic;

namespace DarkGamePacket.Handlers.Classes
{
    public class RouteHandler
    {
        private readonly Dictionary<PacketID, RequestHandle> RequestTable;
        private readonly Dictionary<PacketID, ResponseHandle> ResponseTable;
        private readonly NetworkHandler<ICoreMessage> NetworkHandle;

        public RouteHandler(NetworkHandler<ICoreMessage> NetworkHandle)
        {
            this.NetworkHandle = NetworkHandle;
            this.RequestTable = new Dictionary<PacketID, RequestHandle>();
            this.ResponseTable = new Dictionary<PacketID, ResponseHandle>();
        }
    }
}
