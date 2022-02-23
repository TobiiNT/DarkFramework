using System;

namespace SampleUnityGameServer
{
    public class Logging
    {
        public static void WriteLine(int ChannelID, uint ClientID, string Message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now} [Server] {Message}");
        }

        public static void WriteError(int ChannelID, uint ClientID, string Message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now} [Server] {Message}");
        }

        public static void WriteError(int ChannelID, uint ClientID, string Message, Exception Exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now} [Server] {Message} ({Exception.Message})");
        }

    }
}
