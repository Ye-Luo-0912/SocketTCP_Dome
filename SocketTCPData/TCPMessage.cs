using System;
using System.Collections.Generic;
using System.Text;

namespace SocketTCPData
{
    internal class TCPMessage
    {
        private const int BUFFERSIZE = 2048;
        private const int DATASIZE = 2;

        private byte[] buffer;
        private int curIndex;

        public byte[] Buffer { get => buffer; }

        /// <summary>
        /// 获取当前索引;
        /// </summary>
        public int CurIndex { get => curIndex; }

        /// <summary>
        /// 获取当前剩余字节;
        /// </summary>
        public int RemainSize { get => buffer.Length - curIndex; }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// /// <param name="count">读取的长度</param>
        public void ReadMessage (int count)
        {
            curIndex += count;
            if (curIndex <= 4) return;

            var num = BitConverter.ToInt16(buffer, 0);
            if ((curIndex - DATASIZE) >= count)
            {
            }
            else
                return;
        }

        //public static byte[] GetBytes(byte[] data)
        //{
        //    byte[] 
        //}

        public TCPMessage (int bufferSize)
        {
            buffer = new byte[bufferSize];
            curIndex = 0;
        }

        public TCPMessage ()
        {
            buffer = new byte[BUFFERSIZE];
            curIndex = 0;
        }
    }
}
