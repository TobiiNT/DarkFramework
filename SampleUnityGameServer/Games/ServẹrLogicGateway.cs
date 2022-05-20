using SampleUnityGameServer.Networks;
using System;
using DarkThreading;

namespace SampleUnityGameServer.Games
{
    public class ServẹrLogicGateway
    {
        public ThreadSafeDictionary<ushort, ServerLogic> Games { set; get; }
        public ServẹrLogicGateway()
        {
            this.Games = new ThreadSafeDictionary<ushort, ServerLogic>();
        }

        public void StartGame(ChannelGame ChannelGame)
        {
            try
            {
                var LogicGame = new ServerLogic();

                ChannelGame.ImportGame(LogicGame);

                if (!this.Games.ContainsKey(ChannelGame.ChannelID))
                {
                    this.Games.Add(ChannelGame.ChannelID, LogicGame);

                    Logging.WriteLine(ChannelGame.ChannelID, $"Started game logic");
                }
            }
            catch (Exception Exception)
            {
                Logging.WriteLine(ChannelGame.ChannelID, $"Failed to create game", Exception);
            }
        }
    }
}
