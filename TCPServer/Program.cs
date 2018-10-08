using System.Net;
using SocketTCPData;
using System.Net.Sockets;
using System;
using System.Text;

namespace TCPServer
{
    internal class Server
    {
        private static ServerInfo ser = ServerInfo.Ref;
        private static Socket socketServer;

        //缓冲区
        private static byte[] buffer;

        private static int count;
        private const int buffSize = 2048;

        private static void Main (string[] args)
        {
            //设置缓冲区大小
            buffer = new byte[buffSize];

            //实例化socketServer对象
            socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //绑定IP节点
            socketServer.Bind(ser.serverEndPoint);
            //设置连接队列的最大长度 并开始监听
            socketServer.Listen(ser.Backlog);

            Console.WriteLine("服务端 已启动----");

            //接受客户端链接 并返回客户端对象
            Socket socketClient = socketServer.Accept();

            string msg = "hello";
            byte[] buffers = Encoding.Unicode.GetBytes(msg);

            socketClient.Send(buffers);

            socketClient.BeginReceive(buffer, 0, buffSize, SocketFlags.None, ReceiveCall, socketClient);

            Console.ReadKey();
        }

        private static void ReceiveCall (IAsyncResult ar)
        {
            var Client = ar.AsyncState as Socket;

            int count = Client.EndReceive(ar);
            string msg = Encoding.Unicode.GetString(buffer, 0, count);
            Console.WriteLine(msg);
        }

        ~Server ()
        {
            socketServer.Close();
            socketServer.Dispose();
        }
    }
}
