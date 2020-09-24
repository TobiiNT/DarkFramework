using DarkPacket.Packets;
using DarkSecurity.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SampleUnityGameClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            List<ClientManager> AllClients = new List<ClientManager>();

            string IPAddress = "127.0.0.1";
            int FromPort = 3300;
            int ToPort = 3300;
            int ClientEachChannel = 1;

            try
            {
                Console.Write("IP Address: ");
                IPAddress = Console.ReadLine();
                if (IPAddress.Length == 0)
                    IPAddress = "127.0.0.1";
                Console.Write("From Port: ");
                FromPort = int.Parse(Console.ReadLine());
                Console.Write("To Port: ");
                ToPort = int.Parse(Console.ReadLine());
                Console.Write("Client Each Channel: ");
                ClientEachChannel = int.Parse(Console.ReadLine());
            }
            catch 
            {

            }
            
            for (int Port = FromPort; Port <= ToPort; Port++)
            {
                for (int i = 0; i < ClientEachChannel; i++)
                {
                    var Client = new ClientManager();
                    Client.ConnectWithIP(IPAddress, Port);
                    AllClients.Add(Client);
                }
            }

            int TestTimes = 10000000;
            int PacketSize = 1;

            try
            {
                Console.Write("Test Times: ");
                TestTimes = int.Parse(Console.ReadLine());
                Console.Write("Packet Size: ");
                PacketSize = int.Parse(Console.ReadLine());
            }
            catch
            {

            }

            for (int i = 0; i < TestTimes; i++)
            {
                using (PacketWriter Packet = new PacketWriter())
                {
                    Packet.WriteBytes(new byte[PacketSize]);

                    foreach (var Client in AllClients)
                    {
                        try
                        {
                            Client.SendDataWithEncryption(Packet.GetPacketData());
                        }
                        catch
                        {

                        }
                    }
                }
            }

            Console.ReadKey();
            return;
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
