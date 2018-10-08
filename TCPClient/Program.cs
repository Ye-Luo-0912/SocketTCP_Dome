using SocketTCPData;
using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TCPClient
{
    internal class Client
    {
        private static ServerInfo ser = ServerInfo.Ref;

        private static Socket socketClient;

        [STAThread]
        private static void Main (string[] args)
        {
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketClient.Connect(ser.serverEndPoint);

            Console.ReadKey();
        }
    }
}
