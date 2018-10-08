using System;
using System.Collections.Generic;
using Common;
using Game_Server.Servers;
using Game_Server.Tool;

namespace Game_Server.Controller
{
    internal class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDic;

        private Server server;

        #region 构造函数

        /// <summary>
        /// 负责处理有客户端数据的 管理类;
        /// </summary>
        /// <param name="server">服务端</param>
        public ControllerManager (Server server)
        {
            this.server = server;
            controllerDic = new Dictionary<RequestCode, BaseController>();
            InitController();
        }

        #endregion 构造函数

        /// <summary>
        /// 初始化Controller
        /// </summary>
        private void InitController ()
        {
            var cor1 = new DefaultController();
            var con2 = new UserController();
            var con3 = new RoomController();
            var con4 = new GameController();

            controllerDic.Add(cor1.GetRequestCode, cor1);
            controllerDic.Add(con2.GetRequestCode, con2);
            controllerDic.Add(con3.GetRequestCode, con3);
            controllerDic.Add(con4.GetRequestCode, con4);
        }

        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="request">请求类型</param>
        /// <param name="action">执行该请求所用的方法</param>
        /// <param name="pre">该方法的参数</param>
        /// <param name="client">负责接受数据的客户端</param>
        public void HeadleRequest (RequestCode request, ActionCode action, string pre, Client client)
        {
            BaseController con;
            if (!controllerDic.TryGetValue(request, out con))
            {
                this.LogError($"获取:{con}错误");
                return;
            }

            //var merodName = Enum.GetName(typeof(ActionCode), action);

            //var methodInfo = con.GetType().GetMethod(merodName);
            //if (methodInfo == null)
            //{
            //    this.LogError($"{methodInfo}为 null ");
            //    return;
            //}

            //var res = methodInfo.Invoke(con, new object[]{ pre, client, server});
            var res = con.ExecuteAction(action, pre, client, server);
            if (res != null || !string.IsNullOrEmpty(res as string))
            {
                server.SendRequest(client, action, res as string);
                this.Log($"请求{action.ToString()} 客户端{client.Name} 数据:{res}");
            }
        }
    }
}
