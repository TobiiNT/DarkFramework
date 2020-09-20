using DarkPacket.Packets;
using SampleUnityGameClient.Networks;
using System;
using System.Text;

namespace SampleUnityGameClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            ConnectionClient Client = new ConnectionClient();
            Client.Start("127.0.0.1", 3333);

            while (true)
            {
                string Content = Console.ReadLine();
                if (Content == "exit")
                    break;

                using (PacketWriter Packet = new PacketWriter())
                {
                    Packet.WriteString(Content);

                    byte[] Data = Packet.GetPacketData();
                    Client.Send(Data, Data.Length);
                }
            }

            Console.ReadKey();
        }
    }
}
