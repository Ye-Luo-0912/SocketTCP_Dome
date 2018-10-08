using System;
using Common;
using Game_Server.Servers;

namespace Game_Server
{
    internal class Program
    {
        private static Server server;

        private static void Main (string[] args)
        {
            server = new Server(ServerConnInfo.SERVERIP, ServerConnInfo.SERVERPORT);
            server.Start();

            Console.ReadKey();
        }
    }
}
