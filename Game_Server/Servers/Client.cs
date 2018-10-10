using System;
using MySql.Data.MySqlClient;
using System.Net.Sockets;
using Game_Server.Tool;
using Common;
using Game_Server.Model;

namespace Game_Server.Servers
{
    internal class Client
    {
        private Message msg;
        private Users   user;
        private Result  userResult;
        private Socket  socketClient;
        private Server  server;
        private Room    room;
        private MySqlConnection mySqlConn;

        public readonly string Name;

        public MySqlConnection Conn => mySqlConn;

        /// <summary>
        /// 用户数据
        /// </summary>
        public string UserDatas => $"{user.Id}:{user.Name}:{userResult.TotalCount}:{userResult.WinCount}";

        public string UserData => $"{user.Id}:{user.Name}";

        public bool IsHouseOwner => room.IsHouseOwner(this);

        public Room Room { set => room = value; get => room; }

        public int UserID => user.Id;

        public string UserName => user.Name;

        #region 构造函数

        public Client (Socket socketClient, Server server, string name)
        {
            this.Name = name;
            this.mySqlConn = ConnHelper.OpenConnection();
            this.server = server;
            this.socketClient = socketClient;
            this.msg = new Message();
        }

        #endregion 构造函数

        /// <summary>
        /// 设置用户数据
        /// </summary>
        public void SetUserData (Users u, Result r)
        {
            user = u;
            userResult = r;
        }

        /// <summary>
        /// 异步接受消息
        /// </summary>
        public void StarConnention ()
        {
            socketClient.BeginReceive(msg.Buffer, msg.CurIndex, msg.RemainSize, SocketFlags.None, ReceiveCall, null);
        }

        /// <summary>
        /// 发送消息给客户端
        /// </summary>
        /// <param name="request">请求类型</param>
        /// <param name="data">所发送的数据</param>
        public void SendMessage (ActionCode actionCode, string data)
        {
            //将 请求类型和数据打包 以发送到客户端
            var bytes = Message.PackData(actionCode, data);
            this.Log($"请求-{actionCode},数据-{data}");
            socketClient.Send(bytes);
        }

        /// <summary>
        /// 异步接受消息 回调
        /// </summary>
        private void ReceiveCall (IAsyncResult ar)
        {
            if (socketClient == null || !socketClient.Connected)
            {
                this.LogWarning($"客户端 {this.Name} ---已下线");
                return;
            }

            try
            {
                int count = socketClient.EndReceive(ar);
                if (count == 0) Close();

                msg.ServerReadMessage(count, onProcessMessage);
                StarConnention();
            }
            catch (Exception e)
            {
                this.LogError("异常:" + e.Message);
                Close();
            }
        }

        private void onProcessMessage (RequestCode request, ActionCode action, string data)
        {
            server.HeadleRequest(request, action, data, this);
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        private void Close ()
        {
            if (socketClient != null)
                socketClient.Close();

            if (room != null)
                room.Colse(this, null);

            socketClient = null;
            server.RemoveClient(this);

            ConnHelper.CloseConnection(mySqlConn);

            this.LogWarning($"{Name}: 以关闭连接");
        }
    }
}
