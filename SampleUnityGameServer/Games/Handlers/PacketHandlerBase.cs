using DarkGamePacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleUnityGameServer.Games.Handlers
{
    public abstract class PacketHandlerBase<T> : IPacketHandler<T> where T : ICoreRequest
    {
        public abstract bool HandlePacket(uint ClientID, T Request);
    }
}
