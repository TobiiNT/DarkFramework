using DarkGamePacket.Definitions.S2C;
using DarkGamePacket.Enums;
using DarkSecurity.Enums;
using SampleUnityGameClient.Games;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SampleUnityGameClient.Networks;

namespace SampleUnityGameClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            LogicGameManager Manager = new LogicGameManager();
            List<ClientGame> AllClients = new List<ClientGame>();

            string IPAddress = "127.0.0.1";
            int FromPort = 3000;
            int ToPort = 3005;

            for (int Port = FromPort; Port <= ToPort; Port++)
            {
                var Client = new ClientGame();
                Client.ConnectWithIP(IPAddress, Port);

                Manager.StartGame(Client);
                AllClients.Add(Client);
            }

            while (true)
            {
                string Content = Console.ReadLine();
                if (Content == "exit")
                    break;

                int Success = 0;
                int Failed = 0;
                
               
                //using (var Chat = new ChatMessageResponse(Channel.C2S, 2, Content))
                //{
                //    byte[] ChatData = Chat.GetPacketData();
                //    foreach (var Client in AllClients)
                //    {
                //        try
                //        {
                //            Client.SendDataWithEncryption(ChatData);
                //            Success++;
                //        }
                //        catch
                //        {
                //            Failed++;
                //        }
                //    }
                //}

                Logging.WriteLine($"Send message to {AllClients.Count} connections : {Success} Success, {Failed} Failed");
            }

            Console.ReadKey();
        }
    }
}
