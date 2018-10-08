using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Server.Model
{
    internal class Result
    {
        private int     id;
        private int     userId;
        private int     winCount;
        private int     totalCount;

        /// <summary>
        ///  用户ID
        /// </summary>
        internal int Id { get => id; }

        /// <summary>
        /// 用户ID;
        /// </summary>
        public int UserId { get => userId; }

        /// <summary>
        /// 总战斗记录;
        /// </summary>
        public int TotalCount { get => totalCount; }

        /// <summary>
        /// 胜利记录;
        /// </summary>
        public int WinCount { get => winCount; }

        internal Result (int id, int userId, int totalCount, int winCount)
        {
            this.id = id;
            this.userId = userId;
            this.totalCount = totalCount;
            this.winCount = winCount;
        }
    }
}
