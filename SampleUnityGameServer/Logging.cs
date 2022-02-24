using System;

namespace SampleUnityGameServer
{
    public class Logging
    {
        public static void WriteLine(string Message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Server] {DateTime.Now} {Message}");
        }
        public static void WriteError(string Message, Exception Exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Server] {DateTime.Now} {Message} ({Exception.Message})");
        }

        public static void WriteLine(int ChannelID, string Message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Channel {ChannelID}] [{DateTime.Now}] - {Message}");
        }

        public static void WriteLine(int ChannelID, uint ClientID, string Message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Channel {ChannelID}] [{DateTime.Now}] - [{ClientID}] {Message}");
        }

        public static void WriteError(int ChannelID, string Message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Channel {ChannelID}] [{DateTime.Now}] - {Message}");
        }

        public static void WriteError(int ChannelID, string Message, Exception Exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Channel {ChannelID}] [{DateTime.Now}] - {Message} ({Exception.Message})");
        }

        public static void WriteError(int ChannelID, uint ClientID, string Message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Channel {ChannelID}] [{DateTime.Now}] - [{ClientID}] {Message}");
        }

        public static void WriteError(int ChannelID, uint ClientID, string Message, Exception Exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Channel {ChannelID}] [{DateTime.Now}] - [{ClientID}] {Message} ({Exception.Message})"); 
        }

    }
}
