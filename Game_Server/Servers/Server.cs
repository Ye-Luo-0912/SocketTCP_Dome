using System;
using Common;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Game_Server.Controller;
using Game_Server.Tool;

namespace Game_Server.Servers
{
    internal class Server
    {
        private const string CLIENTNAME = "Client";

        //等待连接数
        private const int BACKLOG = 20;

        private List<Client> clients;
        private List<Room> rooms;

        private ControllerManager controllerManager;
        private IPEndPoint serverEndPoint;
        private Socket socketServer;
        private Socket clientSocket;
        private Client clientObject;

        private int i;

        #region 构造函数

        public Server ()
        {
            InitServerSocket();
        }

        public Server (string ip, int port)
        {
            SetIPAndPort(ip, port);
            InitServerSocket();
        }

        public void InitServerSocket ()
        {
            clients = new List<Client>();
            rooms = new List<Room>();
            controllerManager = new ControllerManager(this);
            socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        #endregion 构造函数

        public void CreateRoom (Client client, string value)
        {
            var room = new Room(this, client, value);
            rooms.Add(room);
        }

        public void RemoveRoom (Room r)
        {
            if (rooms != null && r != null)
                rooms.Remove(r);
        }

        public Room GetRoomByID (int id)
        {
            foreach (var r in rooms)
            {
                if (r.ID == id)
                    return r;
            }
            return null;
        }

        /// <summary>
        /// 设置Ip和端口号
        /// </summary>
        public void SetIPAndPort (string ip, int port)
        {
            serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        /// <summary>
        /// 启动Server端服务
        /// </summary>
        public void Start ()
        {
            if (socketServer == null) return;

            //绑定端口 开始监听, 并开始接收客户端的连接
            socketServer.Bind(serverEndPoint);
            socketServer.Listen(BACKLOG);
            socketServer.BeginAccept(AcceptCall, null);

            this.Log("Server::Start");
        }

        private void AcceptCall (IAsyncResult ar)
        {
            clientSocket = socketServer.EndAccept(ar);
            StartClient(clientSocket);
            socketServer.BeginAccept(AcceptCall, null);
        }

        /// <summary>
        /// 创建客户端连接对象, 并启动
        /// </summary>
        private void StartClient (Socket c)
        {
            clientObject = new Client(c, this, $"{CLIENTNAME}{i++}");
            clientObject.StarConnention();
            clients.Add(clientObject);

            this.Log($"{clientObject.Name}_已连接");
        }

        /// <summary>
        /// 移除客户端
        /// </summary>
        public void RemoveClient (Client client)
        {
            lock (clients)
            {
                clients.Remove(client);
            }
        }

        /// <summary>
        /// 发送响应到客户端
        /// </summary>
        /// <param name="client">接受消息的客户端</param>
        /// <param name="request">响应类型</param>
        /// <param name="pre">数据</param>
        public void SendRequest (Client client, ActionCode actionCode, string pre)
        {
            client.SendMessage(actionCode, pre);
        }

        public void HeadleRequest (RequestCode request, ActionCode action, string pre, Client client)
        {
            controllerManager.HeadleRequest(request, action, pre, client);
        }

        public List<Room> GetRooms ()
        {
            return rooms;
        }
    }
}
