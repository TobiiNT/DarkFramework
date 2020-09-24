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