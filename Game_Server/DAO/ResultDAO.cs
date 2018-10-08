using Game_Server.Model;
using MySql.Data.MySqlClient;

namespace Game_Server.DAO
{
    internal class ResultDAO
    {
        private MySqlDataReader reader;
        private MySqlCommand cmd;

        public Result GetResultByUserId (MySqlConnection conn, int userId)
        {
            using (cmd = conn.CreateCommand())
            {
                cmd.CommandText = $"select * from result where userId = '{userId}';";

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var id = reader.GetInt32("Id");
                    var winCount = reader.GetInt32("totalCount");
                    var totalCount = reader.GetInt32("totalCount");

                    return new Result(id, userId, totalCount, winCount);
                }
                else
                    return new Result(0, 0, 0, 0);
            }
        }
    }
}
