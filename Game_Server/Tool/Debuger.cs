using System;

namespace Game_Server.Tool
{
    public static class Debuger
    {
        public static void Log (this Object obj, string msg)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[{obj.GetType().Name}_{DateTime.Now}]: {msg}");
        }

        public static void LogError (this Object obj, string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Error][{obj.GetType().Name}_{DateTime.Now}]: {msg}");
        }

        public static void LogWarning (this Object obj, string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Warning][{obj.GetType().Name}_{DateTime.Now}]: {msg}");
        }
    }
}
