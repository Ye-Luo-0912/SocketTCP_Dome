using Common;
using Game_Server.Servers;

namespace Game_Server.Controller
{
    internal abstract class BaseController
    {
        protected RequestCode requestCode = RequestCode.None;

        public RequestCode GetRequestCode { get => requestCode; }

        internal virtual string DefaultHandle (string pre, Client client, Server server)
        {
            return null;
        }

        public virtual string ExecuteAction (ActionCode code, string data, Client client, Server server)
        {
            return null;
        }
    }
}
