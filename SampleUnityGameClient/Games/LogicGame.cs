using DarkGamePacket.Definitions.C2S;
using DarkGamePacket.Definitions.S2C;
using DarkGamePacket.Interfaces;
using DarkGamePacket.Servers;
using DarkSecurityNetwork.Networks;
using SampleUnityGameClient.Games.PacketDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleUnityGameClient.Games
{
    public class LogicGame
    {
        //public NetworkHandler<ICoreRequest> RequestHandler { get; }
        //public NetworkHandler<ICoreResponse> ResponseHandler { get; }
        //public IPacketNotifier PacketNotifier { private set; get; }
        //
        //public LogicGame()
        //{
        //    this.RequestHandler = new NetworkHandler<ICoreRequest>();
        //    this.ResponseHandler = new NetworkHandler<ICoreResponse>();
        //}
        //public void Initialize(IPacketHandlerManager<ClientSecurityNetwork> PacketHandler)
        //{
        //    this.PacketNotifier = new ServerPacketNotifier<ClientSecurityNetwork>(PacketHandler);
        //    this.InitializePacketHandlers();
        //}
        //public void InitializePacketHandlers()
        //{
        //    this.RequestHandler.Register<ChatMessageRequest>(new HandleChatMessage(this).HandlePacket);
        //}
    }
}
