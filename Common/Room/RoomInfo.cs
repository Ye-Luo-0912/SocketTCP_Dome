using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public struct RoomInfo
    {
        private int id;
        private string name;
        private string info;

        public string Name { get => name; }
        public string Info { get => info; set => info = value; }
        public int Id { get => id; }

        public RoomInfo (int id, string name, string info)
        {
            this.id = id;
            this.name = name;
            this.info = info;
        }

        public RoomInfo (string data)
        {
            var dt = data.Split(':');
            id = int.Parse(dt[0]);
            name = dt[1];
            info = dt[2];
        }
    }
}
