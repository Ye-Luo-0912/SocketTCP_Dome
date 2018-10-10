using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;

namespace Game_Server.Servers
{
    internal enum RoomState
    {
        WaitingJoin,
        WaitingBattle,
        Battle,
        End,
    }

    internal class Room
    {
        private List<Client> roomClents;
        private RoomState state;
        private Server server;
        private RoomInfo roomInfo;

        public bool IsWaitingJoin { get => state == RoomState.WaitingJoin; }

        public string RoomInfo { get => $"{roomInfo.Id}:{roomInfo.Name}:{roomInfo.Info}"; }

        public string HouseOwnerData { get => roomClents[0].UserDatas; }

        public int ID { get => roomClents.Count > 0 ? roomClents[0].UserID : -1; }

        public bool IsHouseOwner (Client c) => c == roomClents[0];

        public Room (Server s, Client c, string v)
        {
            Init();
            this.server = s;
            this.roomInfo = new RoomInfo(c.UserID, c.UserName, v);
            this.AddClient(c);
        }

        /// <summary>
        /// 向客户端 广播消息
        /// </summary>
        public void BroadcasetMessage (Client exclude, ActionCode action, string data)
        {
            foreach (var c in roomClents)
            {
                if (c != exclude)
                {
                    server.SendRequest(c, action, data);
                }
            }
        }

        private void Init ()
        {
            roomClents = new List<Client>();
            state = RoomState.WaitingJoin;
        }

        public void SetRoom (int id, string name, string info)
        {
            roomInfo = new RoomInfo(id, name, info);
        }

        public void SetRoomInfo (string value)
        {
            roomInfo.Info = value;
        }

        public void AddClient (Client client)
        {
            roomClents.Add(client);
            client.Room = this;

            if (roomClents.Count >= 2)
                state = RoomState.WaitingBattle;
        }

        /// <summary>
        /// 关闭当前 房间
        /// </summary>
        public void Colse (Client client, Action action)
        {
            if (IsHouseOwner(client))
            {
                foreach (var r in roomClents)
                {
                    r.Room = null;
                }

                roomClents.Clear();
                roomClents = null;
                server.RemoveRoom(this);
            }
            else
            {
                roomClents.Remove(client);
                action?.Invoke();

                client.Room = null;
                if (roomClents.Count < 2)
                    state = RoomState.WaitingJoin;
            }
        }

        public string GetRoomData ()
        {
            var strBur = new StringBuilder();
            foreach (var r in roomClents)
            {
                strBur.Append($"{r.UserName}|");
            }

            if (strBur.Length == 0)
                strBur.Append("0");
            else
                strBur.Remove(strBur.Length - 1, 1);

            return strBur.ToString();
        }

        public async void StartTimer (int times)
        {
            await Task.Run(() =>
            {
                Thread.Sleep(233);

                for (int i = times; i > 0; i--)
                {
                    BroadcasetMessage(null, ActionCode.ShowTimer, i.ToString());
                    Thread.Sleep(1000);
                }
                BroadcasetMessage(null, ActionCode.StartPlay, "StartPlay");
            });
        }
    }
}
