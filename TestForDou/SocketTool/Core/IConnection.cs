using System;
using System.Collections.Generic;
using System.Net;

namespace SocketTool.Core
{
    public interface IConnection
    {
        string ID { get; set;}
        IPEndPoint ClientIP { get; }

        DateTime CreateDate { get; }

        DateTime OnlineDate { get; set; }

        void Send(byte[] data, int length);
    }


}
