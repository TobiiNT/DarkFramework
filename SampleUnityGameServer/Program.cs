using DarkPacket.Packets;
using System;
using System.Linq;
using System.Text;

namespace SampleUnityGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            ChannelManager World = new ChannelManager();

            int ChannelCount = 1;
            int FromPort = 3300;

            try
            {
                Console.Write("Channel Count: ");
                ChannelCount = int.Parse(Console.ReadLine());
                Console.Write("From Port: ");
                FromPort = int.Parse(Console.ReadLine());
            }
            catch
            {

            }

            for (int ChannelPort = FromPort; ChannelPort < FromPort + ChannelCount; ChannelPort++)
            {
                World.StartNewChannel(ChannelPort);
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

                    foreach (var Channel in World.Channels.Values.ToList())
                    {
                        foreach (var Client in Channel.ClientConnections.Values.ToList())
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
            }

            Console.ReadKey();
            return;

            while (true)
            {
                string Content = Console.ReadLine();
                if (Content == "exit")
                    break;

                int TotalChannel = 0;
                int TotalClient = 0;
                int Success = 0;
                int Failed = 0;

                using (PacketWriter Packet = new PacketWriter())
                {
                    Packet.WriteString(Content);
                    foreach (var Channel in World.Channels.Values.ToList())
                    {
                        TotalChannel++;
                        foreach (var Client in Channel.ClientConnections.Values.ToList())
                        {
                            TotalClient++;
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
                }

                Logging.WriteLine($"Send message to {TotalChannel} channels and {TotalClient} clients : {Success} Success, {Failed} Failed");
            }

            Console.ReadKey();
        }
    }
}