using DarkPacket.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleUnityGameClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            List<ClientManager> AllClients = new List<ClientManager>();

            for (int i = 0; i < 100; i++)
            {
                var Client = new ClientManager();
                Client.ConnectWithIP("127.0.0.1", 3333);
                AllClients.Add(Client);
            }

            for (int i = 0; i < 100; i++)
            {
                var Client = new ClientManager();
                Client.ConnectWithIP("127.0.0.1", 3334);
                AllClients.Add(Client);
            }


            while (true)
            {
                string Content = Console.ReadLine();
                if (Content == "exit")
                    break;

                int Success = 0;
                int Failed = 0;

                using (PacketWriter Packet = new PacketWriter())
                {
                    Packet.WriteString(Content);

                    foreach (var Client in AllClients)
                    {
                        try
                        {
                            Client.SendDataWithEncryption(Packet.GetPacketData());
                            Success++;
                        }
                        catch
                        {
                            Failed++;
                        }
                    }
                }

                Logging.WriteLine($"Send message to {AllClients.Count} connections : {Success} Success, {Failed} Failed");
            }

            Console.ReadKey();
        }        
    }
}
