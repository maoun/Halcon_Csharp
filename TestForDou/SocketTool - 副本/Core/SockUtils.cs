using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections;

namespace SocketTool.Core
{
    public class SocketUtil
    {
        public static string LastError = string.Empty;

        private static Hashtable ErrorMsgMap = new Hashtable();
        public SocketUtil()
        {

           
        }

        /// <summary>
        /// Turn on keep alive on a socket.</summary>
        /// <param name="turnOnAfter">
        /// Specifies the timeout, in milliseconds, with no activity until the first keep-alive packet is sent
        /// <param name="keepAliveInterval">
        /// Specifies the interval in milliseconds to send the keep alive packet.</param>
        /// <remarks>The keepAliveInternal doesn't seem to do any difference!</remarks>
        public static bool SetKeepAlive(Socket socket, ulong turnOnAfter, ulong keepAliveInterval)
        {
            int bytesperlong = 4;   // in c++ a long is four bytes long
            int bitsperbyte = 8;

            try
            {
                // Enables or disables the per-connection setting of the TCP keep-alive option which 
                // specifies the TCP keep-alive timeout and interval. The argument structure for 
                // SIO_KEEPALIVE_VALS is specified in the tcp_keepalive structure defined in the Mstcpip.h 
                // header file. This structure is defined as follows: 
                // /* Argument structure for SIO_KEEPALIVE_VALS */
                // struct tcp_keepalive {
                //    u_long  onoff;
                //    u_long  keepalivetime;
                //    u_long  keepaliveinterval;
                //};
                // SIO_KEEPALIVE_VALS is supported on Windows 2000 and later.
                byte[] SIO_KEEPALIVE_VALS = new byte[3 * bytesperlong];
                ulong[] input = new ulong[3];

                // put input arguments in input array
                if (turnOnAfter == 0 || keepAliveInterval == 0) // enable disable keep-alive
                    input[0] = (0UL); // off
                else
                    input[0] = (1UL); // on

                input[1] = (turnOnAfter);
                input[2] = (keepAliveInterval); 

                // pack input into byte struct
                for (int i = 0; i < input.Length; i++)
                {
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 3] = (byte)(input[i] >> ((bytesperlong - 1) * bitsperbyte) & 0xff);
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 2] = (byte)(input[i] >> ((bytesperlong - 2) * bitsperbyte) & 0xff);
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 1] = (byte)(input[i] >> ((bytesperlong - 3) * bitsperbyte) & 0xff);
                    SIO_KEEPALIVE_VALS[i * bytesperlong + 0] = (byte)(input[i] >> ((bytesperlong - 4) * bitsperbyte) & 0xff);
                }
                // create bytestruct for result (bytes pending on server socket)
                byte[] result = BitConverter.GetBytes(0);
                
                // write SIO_VALS to Socket IOControl
                socket.IOControl(IOControlCode.KeepAliveValues, SIO_KEEPALIVE_VALS, result);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        private static void SetErrorMsg()
        {
            ErrorMsgMap[10004] = "操作被取消";
            ErrorMsgMap[10013] = "请求的地址是一个广播地址，但不设置标志";
            ErrorMsgMap[10014] = "无效的参数";
            ErrorMsgMap[10022] = "套接字没有绑定，无效的地址，或听不调用之前接受";
            ErrorMsgMap[10024] = "没有更多的文件描述符，接受队列是空的";
            ErrorMsgMap[10035] = "套接字是非阻塞的，指定的操作将阻止";
            ErrorMsgMap[10036] = "一个阻塞的Winsock操作正在进行中";
            ErrorMsgMap[10037] = "操作完成，没有阻塞操作正在进行中";
            ErrorMsgMap[10038] = "描述符不是一个套接字";
            ErrorMsgMap[10039] = "目标地址是必需的";
            ErrorMsgMap[10040] = "数据报太大，无法进入缓冲区，将被截断";
            ErrorMsgMap[10041] = "指定的端口是为这个套接字错误类型";
            ErrorMsgMap[10042] = "股权不明，或不支持的";
            ErrorMsgMap[10043] = "指定的端口是不支持";
            ErrorMsgMap[10044] = "套接字类型不支持在此地址族";
            ErrorMsgMap[10045] = " Socket是不是一个类型，它支持面向连接的服务";
            ErrorMsgMap[10047] = "地址族不支持";
            ErrorMsgMap[10048] = "地址在使用中";
            ErrorMsgMap[10049] = "地址是不是可以从本地机器";
            ErrorMsgMap[10050] = "网络子系统失败";
            ErrorMsgMap[10051] = "网络可以从这个主机在这个时候不能达到";
            ErrorMsgMap[10052] = "连接超时设置SO_KEEPALIVE时";
            ErrorMsgMap[10053] = "连接被中止，由于超时或其他故障";
            ErrorMsgMap[10054] = "连接被重置连接被远程端重置远程端";
            ErrorMsgMap[10055] = "无缓冲区可用空间";
            ErrorMsgMap[10056] = "套接字已连接";
            ErrorMsgMap[10057] = "套接字未连接";
            ErrorMsgMap[10058] = "套接字已关闭";
            ErrorMsgMap[10060] = "尝试连接超时";
            ErrorMsgMap[10061] = "连接被强制拒绝";
            ErrorMsgMap[10101] = "监听服务已关闭";
            ErrorMsgMap[10201] = "套接字已创建此对象";
            ErrorMsgMap[10202] = "套接字尚未创建此对象";
            ErrorMsgMap[11001] = "权威的答案：找不到主机";
            ErrorMsgMap[11002] = "非权威的答案：找不到主机";
            ErrorMsgMap[11003] = "非可恢复的错误";
            ErrorMsgMap[11004] = "有效的名称，没有请求类型的数据记录";
        }

        public static string DescrError(int ErrorCode)
        {
            if (ErrorMsgMap.Count == 0)
                SetErrorMsg();
            return ""+ ErrorMsgMap[ErrorCode];
        }


        public static bool HandleSocketError(SocketException socketExc)
        {
            bool handled = false;
            if (socketExc != null)
            {
                /**
                switch (socketExc.ErrorCode)
                {
                    case (int)WsaError.WSAEINTR:
                        LastError = string.Format("Socket call interrupted [code {0}].", socketExc.ErrorCode);
                        break;
                    case (int)WsaError.WSAEADDRINUSE:
                        LastError = string.Format("The address is already in use [code {0}].", socketExc.ErrorCode);
                        break;
                    case (int)WsaError.WSACONNABORTED:
                        LastError = string.Format("The connection was aborted [code {0}].", socketExc.ErrorCode);
                        break;
                    case (int)WsaError.WSAECONNRESET:
                        LastError = string.Format("Connection reset by peer [code {0}].", socketExc.ErrorCode);
                        break;
                    case (int)WsaError.WSAECONNREFUSED:
                        LastError = string.Format("The connection was refused by the remote host [code {0}].", socketExc.ErrorCode);
                        break;
                    case (int)WsaError.WSAEADDRNOTAVAIL:
                        LastError = string.Format("The requested address is not valid [code {0}].", socketExc.ErrorCode);
                        break;
                    default:
                        LastError = string.Format("Socket error [code {0}].", socketExc.ErrorCode);
                        break;
                }
                 */
                handled = true;
            }
            /**
            ObjectDisposedException disposedExc = socketExc as ObjectDisposedException;
            if (disposedExc != null)
            {
                LastError = "The socket has been closed.";
                handled = true;
            }

            if (LastError != string.Empty)
                Trace.Write(LastError);
            */
            //Trace.Write(exc.Message);
            //Trace.Write(exc.StackTrace);


            return handled;
        }
    }   // SockUtils
}
