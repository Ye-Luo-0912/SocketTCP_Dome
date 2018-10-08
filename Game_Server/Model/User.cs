using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Server.Model
{
    internal class Users
    {
        private int id;
        private string userName;
        private string password;

        /// <summary>
        ///  用户ID
        /// </summary>
        internal int Id { get => id; }

        /// <summary>
        /// 用户名
        /// </summary>
        internal string Name { get => userName; }

        /// <summary>
        /// 用户密码
        /// </summary>
        internal string Password { get => password; }

        internal Users (int id, string name, string pwd)
        {
            this.id = id;
            this.userName = name;
            this.password = pwd;
        }
    }
}
