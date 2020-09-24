using DarkGamePacket.Interfaces;
using DarkGamePacket.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleUnityGameServer.Games
{
    public class Game
    {
        public NetworkHandler<ICoreRequest> RequestHandler { get; }
        public NetworkHandler<ICoreResponse> ResponseHandler { get; }
    }
}
