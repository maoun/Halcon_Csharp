using System;
using System.Collections.Generic;

namespace SocketTool.Core
{
    [Serializable]
    public class SocketInfo
    {
        public string Name { get; set; }
        //Server端或客户端类型
        public string Type { get; set; }
        //16进制格式或AscII
        public string Format { get; set; }

        public string ServerIp { get; set; }

        public int Port { get; set; }
        //TCP或UDP
        public string Protocol { get; set; }
        //报文数据
        public string Data {get;set;}
        //是否自动发送或接收数据
        public Boolean IsAuto {get;set;}

        public SocketInfo()
        {
            Format = "AscII";
            Protocol = "Tcp";
            Port = 8890;
            ServerIp = "127.0.0.1";
            Data = "请录入测试数据";
        }
    }
}
