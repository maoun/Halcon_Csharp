using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;


namespace Transport
{
    public class Program
    {
        static bool _continue;
        static SerialPort _serialPort;
        public static void Main()
        {
            string name;
            string message;
            StringComparer stringcomparer =StringComparer.OrdinalIgnoreCase;//不区分大小写
            Thread readThread= new Thread(Read);//创建线程
            //创建一个默认设置的串口工程
            _serialPort=new SerialPort();
            //设置属性
            _serialPort.PortName = SetPortName(_serialPort.PortName);//串口名称
            _serialPort.BaudRate = SetPortBaudRate(_serialPort.BaudRate);//串口波特率
            _serialPort.Parity = SetPortParity(_serialPort.Parity);//奇偶校验
            _serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);//数据位长度
            _serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);//停止位数
            _serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);//握手协议

            //设置写入或读取超时
            _serialPort.WriteTimeout = 500;
            _serialPort.ReadTimeout = 500;

            //打开串口
            _serialPort.Open();
            _continue = true;
            readThread.Start();

            Console.Write("Port Name:");
            name = Console.ReadLine();

            Console.WriteLine("Type QUIT to exit");

            while (_continue) ;
            {
                message = Console.ReadLine;

            }

        }

    }
}
