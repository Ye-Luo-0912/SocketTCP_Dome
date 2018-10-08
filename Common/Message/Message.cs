using System;
using Common;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Common
{
    public class Message
    {
        public const char SEPARATOR = ':';

        private const int BUFFERSIZE = 2048;
        private const int REQUESTSIZE = 4;
        private const int ACTIONPOS = REQUESTSIZE * 2;

        #region 读取数据的临时变量

        private RequestCode requestCode;
        private ActionCode  actionCode;
        private string      data;

        #endregion 读取数据的临时变量

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
        /// 读取数据---- 服务端
        /// </summary>
        /// <param name="BufferAmount">读取的长度</param>
        /// <param name="processDataCallback">回调函数 用于处理读取到的数据</param>
        public void ServerReadMessage (int BufferAmount, Action<RequestCode, ActionCode, string> processDataCallback)
        {
            curIndex += BufferAmount;

            while (true)
            {
                if (curIndex <= REQUESTSIZE) return;

                var number = BitConverter.ToInt32(buffer, 0);
                if ((curIndex - REQUESTSIZE) >= number)
                {
                    //读取 消息
                    ServerReadMessage(number);
                    //将 读取到的消息 发送到回调
                    processDataCallback(requestCode, actionCode, data);
                    //更新 缓存区
                    UpdatBuffer(number);
                }
                else
                    return;
            }
        }

        /// <summary>
        /// 读取数据----  客户端;
        /// </summary>
        /// <param name="BufferAmount">读取的字节数</param>
        /// <param name="processDataCallback">回调函数 用于处理读取到的数据</param>
        public void ClientReadMessage (int BufferAmount, Action<ActionCode, string> processDataCallback)
        {
            curIndex += BufferAmount;

            while (true)
            {
                if (curIndex <= REQUESTSIZE) return;

                var number = BitConverter.ToInt32(buffer, 0);
                if ((curIndex - REQUESTSIZE) >= number)
                {
                    ClientReadMessage(number);
                    processDataCallback(actionCode, data);
                    UpdatBuffer(number);
                }
                else
                    return;
            }
        }

        private void ServerReadMessage (int number)
        {
            requestCode = (RequestCode)BitConverter.ToInt32(buffer, REQUESTSIZE);
            actionCode = (ActionCode)BitConverter.ToInt32(buffer, ACTIONPOS);
            data = Encoding.UTF8.GetString(buffer, REQUESTSIZE + ACTIONPOS, number - ACTIONPOS);
        }

        private void ClientReadMessage (int number)
        {
            actionCode = (ActionCode)BitConverter.ToInt32(buffer, REQUESTSIZE);
            data = Encoding.UTF8.GetString(buffer, ACTIONPOS, number - REQUESTSIZE);
        }

        /// <summary>
        /// 打包数据
        /// </summary>
        /// <param name="actionCode">请求类型</param>
        /// <param name="data">数据</param>
        /// <returns>将请求类型和数据打包成byte数据</returns>
        public static byte[] PackData (ActionCode actionCode, string data)
        {
            return DataToBuyes(CodeToBytes((int)actionCode), Encoding.UTF8.GetBytes(data));
        }

        /// <summary>
        /// 打包数据
        /// </summary>
        /// <param name="request">请求类型</param>
        /// <param name="data">数据</param>
        public static byte[] PackData (RequestCode request, ActionCode action, string data)
        {
            var resBytes = CodeToBytes((int)request);
            var actBytes = CodeToBytes((int)action);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            return DataToBuyes(resBytes, actBytes, dataBytes);
        }

        private static byte[] CodeToBytes (int code)
        {
            return BitConverter.GetBytes(code);
        }

        private static byte[] DataToBuyes (params byte[][] vs)
        {
            if (vs.Length == 0) return null;

            var num = 0;
            var bs = new byte[0];

            foreach (var i in vs)
            {
                num += i.Length;
                bs = bs.Concat(i).ToArray();
            }

            return BitConverter.GetBytes(num).Concat(bs).ToArray();
        }

        private void UpdatBuffer (int number)
        {
            Array.Copy(buffer, number + REQUESTSIZE, buffer, 0, curIndex - REQUESTSIZE - number);
            curIndex -= (number + REQUESTSIZE);
        }

        //public static byte[] GetBytes(byte[] data)
        //{
        //    byte[]
        //}

        #region 构造函数

        public Message (int bufferSize)
        {
            buffer = new byte[bufferSize];
            curIndex = 0;
        }

        public Message ()
        {
            buffer = new byte[BUFFERSIZE];
            curIndex = 0;
        }

        #endregion 构造函数
    }
}
