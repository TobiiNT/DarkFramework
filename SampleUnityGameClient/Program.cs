using System;
using System.Collections.Generic;
using System.Text;
using SampleUnityGameClient.Networks;

namespace SampleUnityGameClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var AllClients = new List<ClientGame>();

            var IPAddress = "127.0.0.1";
            Console.Write("Write port: ");
            int Port = int.Parse(Console.ReadLine());

            var ClientGame = new ClientGame();
            ClientGame.ConnectWithIP(IPAddress, Port);
            AllClients.Add(ClientGame);

            while (true)
            {
                var Content = Console.ReadLine();
                if (Content == "exit")
                    break;

                var Success = 0;
                var Failed = 0;

                foreach (var Client in AllClients)
                {
                    try
                    {
                        if (Client.IsRunning())
                        {
                            Client.LogicGame.PacketNotifier.NotifyChatMessage(1, Content);
                            Success++;
                        }
                        else
                        {
                            Failed++;
                        }
                       
                    }
                    catch
                    {
                        Failed++;
                    }
                }

                Logging.WriteLine($"Send message to {AllClients.Count} connections : {Success} Success, {Failed} Failed");
            }

            Console.ReadKey();
        }
    }
}
