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

            var IPAddress = "127.0.0.1";
            Console.Write("Input server's port: ");
            int Port = int.Parse(Console.ReadLine());
            TestWithMultipleClient(IPAddress, Port, 10);

            Console.ReadKey();
        }
        static void TestWithMultipleClient(string IPAddress, int Port, int ClientAmount)
        {
            var AllClients = new List<ClientGame>();

            for (int i = 0; i < ClientAmount; i++)
            {
                var ClientGame = new ClientGame();
                ClientGame.ConnectWithIP(IPAddress, Port);
                AllClients.Add(ClientGame);
            }

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
                            Client.LogicGame.PacketNotifier.NotifyChatMessage(Client.ClientID, 1, Content);
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
        }

        static void TestWithOneClient(string IPAddress, int Port)
        {
            var ClientGame = new ClientGame();
            ClientGame.ConnectWithIP(IPAddress, Port);

            while (true)
            {
                var Content = Console.ReadLine();
                if (Content == "exit")
                    break;

                var Success = 0;
                var Failed = 0;

                try
                {
                    if (ClientGame.IsRunning())
                    {
                        ClientGame.LogicGame.PacketNotifier.NotifyChatMessage(ClientGame.ClientID, 1, Content);
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

                Logging.WriteLine($"Send message to server : {Success} Success, {Failed} Failed");
            }
        }
    }
}
