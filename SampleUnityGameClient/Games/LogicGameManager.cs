using System;
using System.Collections.Generic;
using System.Text;
using SampleUnityGameClient.Networks;

namespace SampleUnityGameClient.Games
{
    public class LogicGameManager
    {
        public void StartGame(ClientGame ClientGame)
        {
            try
            {
                var LogicGame = new LogicGame();
                //LogicGame.Initialize(ClientGame.PacketHandlerManager);

                ClientGame.ImportGame(LogicGame);
            }
            catch (Exception Exception)
            {
              
            }
        }
    }
}
