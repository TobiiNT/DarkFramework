using SampleUnityGameServer.Networks;
using System;
using DarkThreading;

namespace SampleUnityGameServer.Games
{
    public class LogicGameManager
    {
        public ThreadSafeDictionary<ushort, LogicGame> Games { set; get; }
        public LogicGameManager()
        {
            this.Games = new ThreadSafeDictionary<ushort, LogicGame>();
        }

        public void StartGame(ChannelGame ChannelGame)
        {
            try
            {
                var LogicGame = new LogicGame();
                //LogicGame.Initialize();

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
