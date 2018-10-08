using Common;
using Game_Server.DAO;
using Game_Server.Model;
using Game_Server.Servers;
using Game_Server.Tool;

namespace Game_Server.Controller
{
    internal class UserController : BaseController
    {
        private UserDAO userDAO = new UserDAO();
        private ResultDAO resultDAO = new ResultDAO();
        private Users user;
        private Result result;

        public UserController ()
        {
            requestCode = RequestCode.User;
        }

        /// <summary>
        /// 登录请求
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        private string Login (string data, Client client, Server server)
        {
            var accountStr = data.Split(':', 2);
            user = userDAO.VeifyUser(client.Conn, accountStr[0], accountStr[1]);

            this.Log($"用户登录请求 账号{accountStr[0]}, 密码{accountStr[1]}");

            if (user == null)
                return ((int)ReturningCode.Fail).ToString();

            result = resultDAO.GetResultByUserId(client.Conn, user.Id);
            client.SetUserData(user, result);

            return $"{(int)ReturningCode.Success}:{user.Name}:{result.TotalCount}:{result.WinCount}";
        }

        /// <summary>
        /// 注册请求
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        private string Register (string data, Client client, Server server)
        {
            var strs = data.Split(':', 2);
            var name = strs[0];
            var pwd = strs[1];

            if (userDAO.GetUserByUserName(client.Conn, name))
                return ((int)ReturningCode.Fail).ToString();

            if (!userDAO.AddUser(client.Conn, name, pwd))
                return ((int)ReturningCode.Fail).ToString();

            return ((int)ReturningCode.Success).ToString();
        }

        public override string ExecuteAction (ActionCode code, string data, Client client, Server server)
        {
            switch (code)
            {
                case ActionCode.Login:
                    return Login(data, client, server);

                case ActionCode.Register:
                    return Register(data, client, server);

                default:
                    return null;
            }
        }
    }
}
