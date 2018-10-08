using System;
using MySql.Data.MySqlClient;

namespace Game_Server.Tool
{
    internal class ConnHelper
    {
        public const string CONNSTR = "server=localhost;user=root;pwd=MySQL-Pwd:520;database=game";

        public static MySqlConnection OpenConnection ()
        {
            var conn = new MySqlConnection(CONNSTR);
            conn.Open();

            return conn;
        }

        public static void CloseConnection (MySqlConnection conn)
        {
            if (conn != null)
                conn.Close();
            else
                Console.WriteLine($"[Error]: {conn}不能为空");
        }
    }
}
