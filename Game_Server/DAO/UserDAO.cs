using System;
using Game_Server.Model;
using Game_Server.Tool;
using MySql.Data.MySqlClient;

namespace Game_Server.DAO
{
    internal class UserDAO
    {
        private MySqlDataReader reader;
        private static MySqlCommand cmd;

        //验证登录
        public Users VeifyUser (MySqlConnection conn, string name, string pwd)
        {
            using (cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"select id from user where username = '{name}' and password = '{pwd}';";

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var id = reader.GetInt32("id");
                    return new Users(id, name, pwd);
                }
                else
                    return null;
            }
        }

        public bool GetUserByUserName (MySqlConnection conn, string name)
        {
            using (cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"select id from user where username = '{name}';";

                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    return true;
                else
                    return false;
            }
        }

        public bool AddUser (MySqlConnection conn, string name, string pwd)
        {
            using (cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"insert into user set username = '{name}', password = '{pwd}';";

                return cmd.ExecuteNonQuery() == 1;
            }
        }

        //private T ExeSQL<T> (MySqlConnection conn, string name, string pwd, Func<MySqlCommand, string, string, T> func)
        //{
        //    using (cmd = conn.CreateCommand())
        //    {
        //        return func(cmd, name, pwd);
        //    }
        //}
    }
}
