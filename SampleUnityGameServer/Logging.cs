using System;

namespace SampleUnityGameServer
{
    public class Logging
    {
        public static void WriteLine(string Message, Exception Exception = null)
        {
            Console.ForegroundColor = Exception != null ? ConsoleColor.Red : ConsoleColor.Cyan;
            Console.WriteLine($"[{DateTime.Now}] [MAIN] {Message} {(Exception != null ? $"[{Exception.Message}]" : "")}");
        }

        public static void WriteLine(int ChannelID, string Message, Exception Exception = null)
        {
            Console.ForegroundColor = Exception != null ? ConsoleColor.Red : ConsoleColor.Cyan;
            Console.WriteLine($"[{DateTime.Now}] [CHANNEL {ChannelID}] : {Message} {(Exception != null ? $"[{Exception.Message}]" : "")}");
        }

        public static void WriteLine(int ChannelID, uint ClientID, string Message, Exception Exception = null)
        {
            Console.ForegroundColor = Exception != null ? ConsoleColor.Red : ConsoleColor.Cyan;
            Console.WriteLine($"[{DateTime.Now}] [CHANNEL {ChannelID}] : [{ClientID}] {Message} {(Exception != null ? $"[{Exception.Message}]" : "")}"); 
        }

    }
}
