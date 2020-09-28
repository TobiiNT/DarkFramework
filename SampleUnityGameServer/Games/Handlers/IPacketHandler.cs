using DarkGamePacket.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleUnityGameServer.Games.Handlers
{
    public interface IPacketHandler<in T> where T : ICoreRequest
    {
        bool HandlePacket(uint ClientID, T Request);
    }
}
