using System;
using System.Collections.Generic;

namespace SocketTool.Core
{
    public interface IClient
    {
        void Init(string serverIp, int port);

        //void Send(string Data);

        void Send(byte[] data);

        event ReceivedHandler OnDataReceived;

        event SocketErrorHandler OnSocketError;

        void Close();
    }
}
