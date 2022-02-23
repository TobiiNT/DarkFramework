using DarkGamePacket.Attributes;
using DarkGamePacket.Definitions;
using DarkGamePacket.Handlers.Delegates;
using DarkPacket.Handlers;
using DarkPacket.Interfaces;
using System;
using System.Collections.Generic;

namespace DarkGamePacket.Handlers.Classes
{
    public class RouteHandler
    {
        private readonly Dictionary<PacketID, RequestHandle> RequestTable;
        private readonly Dictionary<PacketID, ResponseHandle> ResponseTable;
        public readonly NetworkHandler<ICoreMessage> NetworkHandle;

        public RouteHandler(NetworkHandler<ICoreMessage> NetworkHandle)
        {
            this.NetworkHandle = NetworkHandle;
            this.RequestTable = new Dictionary<PacketID, RequestHandle>();
            this.ResponseTable = new Dictionary<PacketID, ResponseHandle>();
        }

        public void LoadResponseHandlers(Type Routes)
        {
            foreach (var Method in Routes.GetMethods())
            {
                foreach (Attribute Attribute in Method.GetCustomAttributes(true))
                {
                    if (Attribute is PacketType Packet && Packet.Direction == PacketDirection.IN)
                    {
                        var DelegateMethod = (ResponseHandle)Delegate.CreateDelegate(typeof(ResponseHandle), Method);

                        this.ResponseTable.Add(Packet.PacketID, DelegateMethod);
                    }
                }
            }
        }
        public void LoadRequestHandlers(Type Routes)
        {
            foreach (var Method in Routes.GetMethods())
            {
                foreach (Attribute Attribute in Method.GetCustomAttributes(true))
                {
                    if (Attribute is PacketType Packet && Packet.Direction == PacketDirection.OUT)
                    {
                        var DelegateMethod = (RequestHandle)Delegate.CreateDelegate(typeof(RequestHandle), Method);

                        this.RequestTable.Add(Packet.PacketID, DelegateMethod);
                    }
                }
            }
        }
        public RequestHandle GetRequestHandle(PacketID PacketID)
        {
            if (this.RequestTable.ContainsKey(PacketID))
            {
                return this.RequestTable[PacketID];
            }
            return null;
        }
        public ResponseHandle GetResponseHandle(PacketID PacketID)
        {
            if (this.ResponseTable.ContainsKey(PacketID))
            {
                return this.ResponseTable[PacketID];
            }
            return null;
        }
    }
}
