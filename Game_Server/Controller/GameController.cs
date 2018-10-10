using System;
using Common;
using Game_Server.Servers;

namespace Game_Server.Controller
{
    internal class GameController : BaseController
    {
        public GameController ()
        {
            requestCode = RequestCode.Game;
        }

        public override string ExecuteAction (ActionCode code, string data, Client client, Server server)
        {
            switch (code)
            {
                case ActionCode.StartGame:
                    return StartGame(data, client, server);

                case ActionCode.Move:
                    return Move(data, client, server);

                default:
                    return null;
            }
        }

        private string Move (string data, Client client, Server server)
        {
            if (client.Room != null)
                client.Room.BroadcasetMessage(client, ActionCode.Move, data);
            return null;
        }

        public string StartGame (string data, Client client, Server server)
        {
            var r = client.Room;

            if (r.IsWaitingJoin)
                return ((int)ReturningCode.Fail).ToString();

            if (client.IsHouseOwner)
            {
                r.BroadcasetMessage(client, ActionCode.StartGame, ((int)ReturningCode.Success).ToString());
                r.StartTimer(3);
                return ((int)ReturningCode.Success).ToString();
            }

            return ((int)ReturningCode.Fail).ToString();
        }
    }
}
