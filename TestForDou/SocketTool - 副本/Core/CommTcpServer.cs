using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Collections;
using System.Net;

namespace SocketTool.Core
{
    public class CommTcpServer : IServer
    {
        public IPAddress ListenOnLocalIP { get; set; }
        public int ListenPort { get; set; }

        private  Hashtable ClientConnections = new Hashtable();

        public List<IConnection> ConnectionList = new List<IConnection>();

        private TcpListener TcpListenerObject { get; set; }

        private readonly int MaxSimultaneousIncomingConnections = 10;

        public bool WantExit { get; set; }

        private Thread AcceptIncomingThreads;
        private Thread ProcessIncomingThreads;
        private Thread CheckDisconnectThreads;

        public Queue DataQueue { get; set; }

        //public DataHandler RecvDataHandler { get; set; }
        public event ReceivedHandler OnDataReceived;
        public event SocketErrorHandler OnSocketError;

        private static int connectId = 0;

        private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CommTcpServer));

        public void Init(String serverIp, int port)
        {
            if (string.IsNullOrEmpty(serverIp))
            {
                ListenOnLocalIP = IPAddress.Any;
            }
            else
            {
                ListenOnLocalIP = IPAddress.Parse(serverIp);
            }
            this.ListenPort = port;
        }


        public List<IConnection> GetConnectionList()
        {
            if (ClientConnections.Count == 0)
            {
                return new List<IConnection>();
            }
            ConnectionList.Clear();
            foreach (IConnection ic in ClientConnections.Values) 
            {
                ConnectionList.Add(ic);
            }
            return ConnectionList;
        }


        //开始监听客户端的请求
        public int Listen()
        {
            ClientConnections = new Hashtable();
            WantExit = false;
            
            TcpListenerObject = new TcpListener(ListenOnLocalIP, ListenPort);

            TcpListenerObject.Start(MaxSimultaneousIncomingConnections);
           
            AcceptIncomingThreads = new Thread(new ThreadStart(AcceptIncoming));

            AcceptIncomingThreads.Start();

            ProcessIncomingThreads = new Thread(new ThreadStart(ProcessIncomingConnectionWorker));

            ProcessIncomingThreads.Start();
            //CheckDisconnectThreads.Start();

            return 1;
        }



        private void AcceptIncoming()
        {
            while (WantExit == false)
            {
                try
                {
                    if (TcpListenerObject.Pending())
                    {
                        TcpClient tc = TcpListenerObject.AcceptTcpClient();
                        TcpConnection vc = new TcpConnection(tc);

                        string clientIP = ((IPEndPoint)tc.Client.RemoteEndPoint).Address.ToString();

                        if (connectId < 100000)
                            Interlocked.Increment(ref connectId);
                        else
                            connectId = 1;
                        vc.ID = ""+connectId;

                        if (OnSocketError != null)
                            vc.OnSocketError += OnSocketError;

                        try
                        {
                            Monitor.Enter(ClientConnections);
                            ClientConnections[vc.ID] = vc;
                            //vc.ConnectId = connectId;
                        }
                        catch (System.Exception ex)
                        {
                            logger.Error(ex.Message);
                            logger.Error(ex.StackTrace);
                        }
                        finally
                        {
                            //Monitor.Exit(ClientConnections);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                }

                Thread.Sleep(500);
            }

        }

        public void Send(string Id, byte[] data, int length)
        {
            TcpConnection conn = (TcpConnection)ClientConnections[Id];

            if (conn == null || conn.IsConnected == false)
            {
                throw new Exception("没有连接，无法发送数据！");
            }
            else
            {
                conn.Send(data, length);
            }

        }
        private void ProcessVc(TcpConnection vc)
        {
            if (vc != null)
            {
                byte[] data = vc.RecvData();

                if (data != null)
                {
                    if (OnDataReceived != null)
                        OnDataReceived(vc.ID, new ReceivedEventArgs(vc.ClientIP, data));
                    //RecvDataHandler(data, data.Length,vc.ID);
                    //DataQueue.Enqueue(data);
                }
            }
        }

        private void ProcessIncomingConnectionWorker()
        {
            try
            {
                while (WantExit == false)
                {
                    try
                    {
                        //ArrayList conns = new ArrayList(ConnectionList);
                        ArrayList connections = new ArrayList(ClientConnections.Values);
                        int index = 0;
                        foreach (TcpConnection vc in connections)
                        {
                            try
                            {
                                ProcessVc(vc);
                            }
                            catch (Exception ex)
                            {
                                //log("error", "处理VC,出现错误:" + ex.Message);
                                //log("error", "处理VC,出现错误:" + ex.StackTrace);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //log("error", "IncomingC线程错误：" + e.Message);
                        //log("error", "IncomingC线程错误：" + e.StackTrace);
                    }

                    Thread.Sleep(200);
                }
            }
            catch (Exception e)
            {
                //log("error", "ProcessIncomingConnectionWorker线程出现异常错误：" + e.Message);
                //log("error", "ProcessIncomingConnectionWorker线程出现异常错误：" + e.StackTrace);
            }

            //log("Info", "ProcessIncomingConnectionWorker线程正常退出");
        }
       

        public void Close()
        {
            WantExit = true;
            try
            {
                if (TcpListenerObject != null && TcpListenerObject.Server != null)
                    TcpListenerObject.Server.Close();
            }
            catch (System.Exception ex)
            {

            }

            try
            {
                ArrayList connections = new ArrayList(ClientConnections.Values);
                int index = 0;
                foreach (TcpConnection vc in connections)
                {
                    try
                    {
                        vc.Close();
                    }
                    catch (Exception ex)
                    {
                        //log("error", "处理VC,出现错误:" + ex.Message);
                        //log("error", "处理VC,出现错误:" + ex.StackTrace);
                    }
                }
                ClientConnections.Clear();
            }
            catch (System.Exception ex)
            {
            	
            }
            //AcceptIncomingThreads.Join(1000);//等待主线程关闭
            try
            {
                if(AcceptIncomingThreads != null)
                AcceptIncomingThreads.Abort();
            }
            catch (Exception e)
            {
               
            }
            try
            {
                if (ProcessIncomingThreads != null)
                ProcessIncomingThreads.Abort();
            }
            catch (Exception e)
            {
            }
            try
            {
                if (CheckDisconnectThreads != null)
                CheckDisconnectThreads.Abort();
            }
            catch (Exception e)
            {
            }           

        }
    }
}
