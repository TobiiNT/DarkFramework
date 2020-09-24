using System;

namespace SampleUnityGameClient
{
    public class Logging
    {
        public static void WriteLine(string Message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{DateTime.Now}:{DateTime.Now.Millisecond} [Client] {Message}");
        }
        public static void WriteError(string Message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now}:{DateTime.Now.Millisecond} [Client] {Message}");
        }
        public static void WriteError(string Message, Exception Exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now}:{DateTime.Now.Millisecond} [Client] {Message} ({Exception.Message})");
        }
    }
}
