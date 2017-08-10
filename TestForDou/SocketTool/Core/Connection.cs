using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Net;

namespace SocketTool.Core
{
    //客户端连接
    public class Connection
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Connection));  

        public int ID { get; set; }

        public IPEndPoint EndPoint { get; set; }

        public Queue DataQueue { get; set; }
        public string IP { get; set; }
        //连接Socket
        public TcpClient tcpClient { get; set; }

        public Boolean IsConnected { get; set; }

        public int ThreadSleepInterval { get; set; }

        private Thread SendOutgoingThread;

        public event SocketErrorHandler OnSocketError;

        public Connection()
        {
            ThreadSleepInterval = 100;
        }
        public Connection(TcpClient client)
        {
            this.tcpClient = client;
            this.EndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            tcpClient.Client.Blocking = true;
            tcpClient.Client.SendBufferSize = 64 * 1024; //缓冲区设置为64K
            IP = tcpClient.Client.RemoteEndPoint.ToString();
            DataQueue = new Queue();
            SendOutgoingThread = new Thread(new ThreadStart(RecvRequestFromClient));
            SendOutgoingThread.Start();
            IsConnected = true;
        }

        public byte[] RecvData()
        {
            if(DataQueue.Count > 0)
               return (byte[])DataQueue.Dequeue();
            return null;
        }

        public void Close()
        {
            try
            {
                if (SendOutgoingThread != null)
                    SendOutgoingThread.Abort();
            }
            catch (System.Exception ex)
            {

            }
            if (tcpClient == null)
            {
                return;
            }

            if (tcpClient.Client != null)
            {
                try { tcpClient.Client.Shutdown(SocketShutdown.Both); }
                catch { }
                try { tcpClient.Client.Close(); }
                catch { }
            }
            try { tcpClient.Close(); }
            catch { }
            tcpClient = null;

            
        }

        public int SendData(byte[] data, int length)
        {
            SocketError errCode;
            int sendBytes = 0;
            sendBytes = tcpClient.Client.Send(data, 0, length, SocketFlags.None, out errCode);

            return sendBytes;
        }

        public void RecvRequestFromClient()
        {
            int availableBytes = 0;         

            while (IsConnected)
            {
                if (tcpClient == null || tcpClient.Client == null || tcpClient.Connected == false)
                {
                    if (OnSocketError != null)
                        OnSocketError(ID, new SocketEventArgs((int)SocketError.Shutdown, ""));
                    return;
                }

                try
                {
                    availableBytes = tcpClient.Available;
                }
                catch (Exception ex)
                {
                    if (OnSocketError != null)
                        OnSocketError(ID, new SocketEventArgs((int)SocketError.Disconnecting, ""));
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
                        if (bytesRead > 0)
                        {
                            byte[] data = new byte[bytesRead];
                            Array.Copy(byteArrayIn, data, bytesRead);
                            DataQueue.Enqueue(data);
                        }
                        else
                        {
                            if(OnSocketError != null)
                                OnSocketError(ID, new SocketEventArgs((int)errCode, ""));
                            //logger.Error("connection broken error:" + errCode);
                        }

                        //if (RecvDataHandler != null)
                            //RecvDataHandler(byteArrayIn, bytesRead);
                    }
                    catch (SocketException ex)
                    {
                        if (OnSocketError != null)
                            OnSocketError(ID, new SocketEventArgs((int)ex.ErrorCode, ex.Message));                       
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
