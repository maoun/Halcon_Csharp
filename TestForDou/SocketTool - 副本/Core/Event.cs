using System;
using System.Collections.Generic;
using System.Net;

namespace SocketTool.Core
{
    //数据接收事件
    public delegate void ReceivedHandler(object o, ReceivedEventArgs e);
    //Socket通信错误
    public delegate void SocketErrorHandler(object o, SocketEventArgs e);
    //日志消息
    public delegate void MsgHandler(object ID, string type, string msg);

    public class ReceivedEventArgs : EventArgs
    {
        public readonly IPEndPoint RemoteHost;
        public readonly byte[] Data;

        public ReceivedEventArgs(IPEndPoint Remote, byte[] ReceivedData)
        {
            Data = ReceivedData;
            RemoteHost = Remote;
        }
    }

    public class SocketEventArgs : EventArgs
    {
        public readonly int ErrorCode;
        public readonly string Message;

        public SocketEventArgs(int code, string msg)
        {
            ErrorCode = code;
            Message = msg;
        }
    }

}
