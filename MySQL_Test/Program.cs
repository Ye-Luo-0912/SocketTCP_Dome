using System;
using MySql.Data.MySqlClient;

namespace MySQL_Test
{
    internal class Program
    {
        private static string connStr;
        private static MySqlDataReader data;
        private static MySqlConnection conn;

        private static void Main (string[] args)
        {
            connStr = "server=localhost;user=root; pwd=MySQL-Pwd:520;database=test";
            conn = new MySqlConnection(connStr);

            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();

            #region 查询

            //cmd.CommandText = "SELECT * FROM user";
            //data = cmd.ExecuteReader();

            //if (data.HasRows)
            //{
            //    while (data.Read())
            //    {
            //        var username = data.GetString("username");
            //        var userpwd = data.GetString("password");
            //        Console.WriteLine(username + ":" + userpwd);
            //    }
            //}

            #endregion 查询

            #region 插入

            var name = "ddd";
            var pwd = "lcker'; delete from user;";

            cmd.CommandText = $"insert into user set username = '{name}', password = '{pwd}'";
            cmd.ExecuteNonQuery();

            #endregion 插入

            Console.WriteLine("------执行完成------");

            Console.ReadKey();
            data.Close();
            conn.Close();
        }
    }
}
