using System;
using System.Collections.Generic;
using System.Net;

namespace SocketTool.Core
{
    public class UdpConnection :IConnection
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(TcpConnection));

        public string ID { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime OnlineDate { get; set; }

        public IPEndPoint ClientIP { get; set; }


        public UdpConnection()
        {

            CreateDate = DateTime.Now;
            OnlineDate = DateTime.Now;
        }

        public void Send(byte[] data, int length)
        {

        }
    }
}
