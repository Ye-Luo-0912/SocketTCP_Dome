using System.Net;

namespace SocketTCPData
{
    public class ServerInfo
    {
        public readonly int Backlog;
        public readonly int Port;

        public IPAddress Ip;
        public readonly IPEndPoint serverEndPoint;

        private static ServerInfo instance = new ServerInfo("127.0.0.1", 10240, 20);

        /// <summary>
        ///返回对象的引用
        /// </summary>
        public static ServerInfo Ref { get { return instance; } }

        private ServerInfo (string ip, int port, int backlog)
        {
            Ip = IPAddress.Parse("127.0.0.1");
            Backlog = backlog;
            Port = port;
            serverEndPoint = new IPEndPoint(Ip, Port);
        }
    }
}
