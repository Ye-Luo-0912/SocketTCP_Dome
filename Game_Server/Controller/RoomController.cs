using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Game_Server.Servers;

namespace Game_Server.Controller
{
    internal class RoomController : BaseController
    {
        public RoomController ()
        {
            requestCode = RequestCode.Room;
        }

        public override string ExecuteAction (ActionCode code, string data, Client client, Server server)
        {
            switch (code)
            {
                case ActionCode.CreateRoom:
                    return CreateRoom(data, client, server);

                case ActionCode.ListRoom:
                    return ListRoom(data, client, server);

                case ActionCode.JoinRoom:
                    return JoinRoom(data, client, server);

                case ActionCode.ExitRoom:
                    return QuitRoom(data, client, server);

                default:
                    return null;
            }
        }

        private string CreateRoom (string data, Client client, Server server)
        {
            server.CreateRoom(client, data);

            return $"{((int)ReturningCode.Success)}:{(int)RoleType.Blue}";
        }

        private string ListRoom (string data, Client client, Server server)
        {
            var strBur = new StringBuilder();

            foreach (var r in server.GetRooms())
            {
                if (r.IsWaitingJoin)
                    strBur.Append($"{r.RoomInfo}-");
            }

            if (strBur.Length == 0)
                strBur.Append("0");
            else
                strBur.Remove(strBur.Length - 1, 1);

            return strBur.ToString();
        }

        private string JoinRoom (string data, Client client, Server server)
        {
            var id = int.Parse(data);
            var r = server.GetRoomByID(id);

            if (r == null)
                return $"{(int)ReturningCode.NotFound}-";
            else if (!r.IsWaitingJoin)
                return $"{(int)ReturningCode.Fail}-";
            else
            {
                r.AddClient(client);

                var rd = r.GetRoomData();
                r.BroadcasetMessage(client, ActionCode.UpdateRoom, rd);
                return $"{(int)ReturningCode.Success}:{(int)RoleType.Red}-{rd}";
            }
        }

        private string QuitRoom (string data, Client client, Server server)
        {
            var r = client.Room;

            if (client.IsHouseOwner)
            {
                r.BroadcasetMessage(client, ActionCode.ExitRoom, $"{(int)ReturningCode.Success}");
                r.Colse(client, null);
                return $"{((int)ReturningCode.Success)}";
            }
            else
            {
                r.Colse(client, () =>
                {
                    r.BroadcasetMessage(client, ActionCode.UpdateRoom, r.GetRoomData());
                });

                return $"{((int)ReturningCode.Success)}";
            }
        }
    }
}
