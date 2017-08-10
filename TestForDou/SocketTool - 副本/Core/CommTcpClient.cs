using System;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace SocketTool.Core
{
    //接收到数据的回调处理函数
    //public delegate void DataHandler(byte[] data, int length, object state);
    public class CommTcpClient : IClient
    {        
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CommTcpClient));      

        private TcpClient tcpClient;

        public String ServerIP { get; set; }

        public int Port { get; set; }
        //线程休息时间
        public int ThreadSleepInterval { get; set; }

        public Boolean IsConnected { get; set; }

        //public DataHandler RecvDataHandler { get; set; } 
        public event ReceivedHandler OnDataReceived;
        public event SocketErrorHandler OnSocketError;


        Thread recvThread;

        public CommTcpClient()
        {
            ThreadSleepInterval = 100;
        }

        public void Init(String serverIp, int port)
        {
            this.ServerIP = serverIp;
            this.Port = port;
        }

        public Boolean Connect()
        {
            try
            {
                if (recvThread != null)
                    recvThread.Abort();
            }
            catch (Exception ex)
            {
            }

            tcpClient = new TcpClient();
            try
            {

                tcpClient.Connect(ServerIP, Port);
                tcpClient.Client.Blocking = true;
                //tcpClient.Client.ReceiveTimeout = 1000;
                tcpClient.Client.LingerState = new LingerOption(true, 0);
                recvThread = new Thread(new ThreadStart(RecvRequestFromClient));
                recvThread.Start();
                IsConnected = true;
                return true;
            }
            catch (SocketException ex)
            {
                if (OnSocketError != null)
                    OnSocketError(0, new SocketEventArgs((int)ex.ErrorCode, ex.Message));                       
            }
            return false;
            
        }

        public void Send(byte[] data)
        {
            if (tcpClient == null || tcpClient.Connected == false)
                Connect();

            tcpClient.Client.Send(data);
        }

        public void Reconnect()
        {

            this.Close();
            this.Connect();
            
        }
        public void Close()
        {
            IsConnected = false;
            try
            {
                if (tcpClient != null)
                {
                    tcpClient.Client.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }
        public void RecvRequestFromClient()
        {
            int availableBytes = 0;           

            while (IsConnected)
            {
                if (tcpClient == null || tcpClient.Client == null || tcpClient.Connected == false)
                {
                    return;
                }
                try
                {
                    availableBytes = tcpClient.Available;
                }
                catch (Exception ex)
                {

                    return;
                }

                if (availableBytes > 0)
                {

                    byte[] byteArrayIn = new byte[availableBytes];
                    try
                    {
                        SocketError errCode;
                        int bytesRead = 0;
                        int recvLen = availableBytes;
                        bytesRead = tcpClient.Client.Receive(byteArrayIn, 0, recvLen, SocketFlags.None, out errCode);

                        if(OnDataReceived != null)
                            OnDataReceived(0, new ReceivedEventArgs((IPEndPoint)tcpClient.Client.RemoteEndPoint, byteArrayIn));
                    }
                    catch(SocketException se)
                    {
                        if (OnSocketError != null)
                            OnSocketError(0, new SocketEventArgs((int)se.ErrorCode, se.Message));
                        return;
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        logger.Error(ex.StackTrace);

                    }
                }
                Thread.Sleep(ThreadSleepInterval);
            }
        }
    }
}
