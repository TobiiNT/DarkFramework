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

                    Logging.WriteLine($"Started game logic on channel {ChannelGame.ChannelID}");
                }
            }
            catch (Exception Exception)
            {
                Logging.WriteError($"Failed to create game on channel {ChannelGame.ChannelID}", Exception);
            }
        }
    }
}
