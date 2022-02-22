using ProtoBuf;
using SampleUnityGameServer.Games;
using System;
using System.IO;
using System.Linq;
using System.Text;
using SampleUnityGameServer.Configurations;
using SampleUnityGameServer.Networks;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace SampleUnityGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var ChannelManager = new ChannelManager();
            var LogicGameManager = new LogicGameManager();

            foreach (var ChannelInfo in Configuration.Channels)
            {
                var Channel = ChannelManager.StartNewChannel(ChannelInfo.Item1, ChannelInfo.Item2);
                if (Channel != null)
                {
                    LogicGameManager.StartGame(Channel);
                }
            }

            while (true)
            {
                var Content = Console.ReadLine();
                if (Content == "exit")
                    break;

                var TotalChannel = 0;
                var TotalClient = 0;
                var Success = 0;
                var Failed = 0;

                foreach (var Channel in ChannelManager.Channels.Values.ToList())
                {
                    TotalChannel++;
                    foreach (var Client in Channel.ClientConnections.Values.ToList())
                    {
                        TotalClient++;
                        try
                        {
                            LogicGameManager.Games[Client.ChannelID].PacketNotifier.NotifyChatMessage(Client.ClientID, 1, Content);
                            Success++;
                        }
                        catch
                        {
                            Failed++;
                        }
                    }
                }

                Logging.WriteLine($"Send message to {TotalChannel} channels and {TotalClient} clients : {Success} Success, {Failed} Failed");
            }

            Console.ReadKey();
        }
    }
}