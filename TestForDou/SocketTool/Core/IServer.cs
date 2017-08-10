using System;
using System.Collections.Generic;

namespace SocketTool.Core
{
    public interface IServer
    {
        //初始化
        void Init(string serverIp, int port);
        //从服务器端给某个连接发送数据
        void Send(string connId, byte[] data, int length);
        //监听
        int Listen();
        //得到当前的连接
        List<IConnection> GetConnectionList();
        //Socket事件
        event ReceivedHandler OnDataReceived;
        event SocketErrorHandler OnSocketError;

        void Close();
    }
}
