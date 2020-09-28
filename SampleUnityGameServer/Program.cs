using DarkGamePacket.Definitions.C2S;
using DarkGamePacket.Definitions.S2C;
using DarkGamePacket.Enums;
using ProtoBuf;
using SampleUnityGameServer.Games;
using System;
using System.IO;
using System.Linq;
using System.Text;
using SampleUnityGameServer.Configurations;
using SampleUnityGameServer.Networks;

namespace SampleUnityGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            ChannelManager ChannelManager = new ChannelManager();
            LogicGameManager LogicGameManager = new LogicGameManager();

            foreach (var ChannelInfo in Configuration.Channels)
            {
                var Channel = ChannelManager.StartNewChannel(ChannelInfo.Item1, ChannelInfo.Item2);
                if (Channel != null)
                {
                    LogicGameManager.StartGame(Channel);
                }
            }

            var Message = new ChatMessageRequest
            {
                MessageType = 253,
                Message = "12345"
            };
            byte[] MessageData = ProtoSerialize(Message);
            var MessageOut = ProtoDeserialize<ChatMessageRequest>(MessageData);
            Console.WriteLine($"{MessageOut.Message}");

            while (true)
            {
                string Content = Console.ReadLine();
                if (Content == "exit")
                    break;

                int TotalChannel = 0;
                int TotalClient = 0;
                int Success = 0;
                int Failed = 0;

                //using (var Chat = new ChatMessageResponse(Channel.S2C, 2, Content))
                //{
                //    byte[] ChatData = Chat.GetPacketData();
                //    foreach (var Channel in ChannelManager.Channels.Values.ToList())
                //    {
                //        TotalChannel++;
                //        foreach (var Client in Channel.ClientConnections.Values.ToList())
                //        {
                //            TotalClient++;
                //            try
                //            {
                //                Client.SendDataWithEncryption(ChatData);
                //                Success++;
                //            }
                //            catch
                //            {
                //                Failed++;
                //            }
                //        }
                //    }
                //}

                Logging.WriteLine($"Send message to {TotalChannel} channels and {TotalClient} clients : {Success} Success, {Failed} Failed");
            }

            Console.ReadKey();
        }

        public static byte[] ProtoSerialize<T>(T record) where T : class
        {
            if (null == record) return null;

            try
            {
                using (var stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, record);
                    return stream.ToArray();
                }
            }
            catch
            {
                // Log error
                throw;
            }
        }
        public static T ProtoDeserialize<T>(byte[] data) where T : class
        {
            if (null == data) return null;

            try
            {
                using (var stream = new MemoryStream(data))
                {
                    return Serializer.Deserialize<T>(stream);
                }
            }
            catch
            {
                // Log error
                throw;
            }
        }
    }
}