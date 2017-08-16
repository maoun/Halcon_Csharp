using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace today_chat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        TcpListener Listener; //  监听
        public  Socket SocketClient; // 
        NetworkStream NetStream;   // 网络流
        StreamReader ServerReader; // 服务器 读 
        StreamWriter ServerWriter; // 服务器 写
        Thread Thd;   // 线程
        Thread thread;
        #region TCP通讯
        public void BeginLister()     // 打开服务器 子函数
        {            
            IPAddress[] Ips = Dns.GetHostAddresses(""); // 本机 IP地址 定义
            string GetIp = Ips[0].ToString(); // 获取到IP 地址
            Listener = new TcpListener(IPAddress.Parse("192.168.8.21"), 9600); // 监听
            Listener.Start(); // 开始监听  
             while (true)
            {
                string a = SocketError.SocketError.ToString();
                CheckForIllegalCrossThreadCalls = false;
                btnBeginServer.Enabled = false;// 开始服务器 按键控件使能关闭
                // MessageBox.Show("服务器已经开启！", "服务器消息", MessageBoxButtons.OK, MessageBoxIcon.Information);  
                this.Text = "服务器 已经开启……";
                SocketClient = Listener.AcceptSocket();//接受挂起----监听到的Socket
                NetStream = new NetworkStream(SocketClient); // 网络流
                ServerWriter = new StreamWriter(NetStream);
                ServerReader = new StreamReader(NetStream);
                if (SocketClient.Connected)  // 监听 ScoketClient 
                {
                    MessageBox.Show("客户端连接成功!", "服务器消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }                   
            }
                  
        }

        private void GetMessage()  // 获取消息
        {
            if (NetStream != null && NetStream.DataAvailable) // 网络流 非空 或者数据可用
            {     
                //设定一个缓冲区，大小默认为1024字节
                const int bufferSize = 1024;
                byte[] buffer = new byte[bufferSize];
                int readBytes = 0;
                NetStream.ReadTimeout = 1000; 
                readBytes = NetStream.Read(buffer, 0, bufferSize);
                string str = Encoding.ASCII.GetString(buffer).Substring(0, readBytes);//ascii码转换为string
                rtxChatInfo.AppendText(DateTime.Now.ToString());
                rtxChatInfo.AppendText("  客户端说:\n");
                              
                rtxChatInfo.AppendText(str + "\n");
                //下拉框
                rtxChatInfo.SelectionStart = rtxChatInfo.Text.Length;
                rtxChatInfo.Focus();
                rtxSendMessage.Focus();
            }
           
        }

        private void tmrGetMess_Tick(object sender, EventArgs e)  // 定时器 执行函数 
        {
            GetMessage();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)  // 关闭 执行函数
        {
            DialogResult Dr = MessageBox.Show("这样会中断与客户端的连接,你要关闭该窗体吗？", "服务器信息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); ;
            if (DialogResult.Yes == Dr)
            {
                try
                {                   
                    this.Thd.Abort();
                    Listener.Stop();
                    e.Cancel = false;
                }
                catch { }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)  // 关闭 按钮  执行函数
        {
            try
            {
                this.Thd.Abort();
                this.Close();//如果有线程则关闭线程
            }
            catch { this.Close(); }//出错 则说明没有线程  直接关闭窗体           
        }

        private void btnSend_Click(object sender, EventArgs e)   // 发送  按钮  执行函数
        {
            try
            {
                if (rtxSendMessage.Text.Trim() != "")
                {
                    ServerWriter.Write(rtxSendMessage.Text);//信息写入流
                    ServerWriter.Flush();
                    rtxChatInfo.AppendText( DateTime.Now.ToString());
                    rtxChatInfo.AppendText("  服务器说:\n");//.Text += "服务器说:           " + rtxSendMessage.Text + "\n";
                    rtxChatInfo.AppendText(rtxSendMessage.Text+ "\n");
                    rtxSendMessage.Clear();
                    //滚动条
                    rtxChatInfo.SelectionStart = rtxChatInfo.Text.Length;
                    rtxChatInfo.Focus();
                    rtxSendMessage.Focus();
                }
                else
                {
                    MessageBox.Show("信息不能为空!", "服务器消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    rtxSendMessage.Focus();
                    return;
                }
            }
            catch
            {
                MessageBox.Show("客户端连接失败……", "服务器消息", MessageBoxButtons.OK, MessageBoxIcon.Error);             
                return;
            }
        }

        private void btnBeginServer_Click(object sender, EventArgs e) // 按键 打开服务器 执行函数
        {
            
            Thd = new Thread(new ThreadStart(BeginLister));//创建线程
            Thd.Start();    //启动线程
        }

        private void grpServer_Enter(object sender, EventArgs e)
        {

        }
        #endregion

        #region  UDP通讯
        private IPEndPoint ipLocalPoint;
        private EndPoint RemotePoint;
        private Socket mySocket;
        private bool RunningFlag = false;
        public void UDPListen()
        {

            try
            {
                //得到本机IP，设置UDP端口号     
                ipLocalPoint = new IPEndPoint(IPAddress.Parse("192.168.8.21"), 9601);

                //定义网络类型，数据连接类型和网络协议UDP  
                mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                //绑定网络地址  
                mySocket.Bind(ipLocalPoint);

                //得到客户机IP  
                IPEndPoint ipep = new IPEndPoint(getValidIP(txtServerIp.Text), getValidPort(txtPort.Text));
                RemotePoint = (EndPoint)(ipep);

                //启动一个新的线程，执行方法this.ReceiveHandle，  
                //以便在一个独立的进程中执行数据接收的操作  

                RunningFlag = true;
                thread = new Thread(new ThreadStart(ReceiveHandle));
                thread.Start();
                this.Text = "连接成功";
            }
            catch
            {
                this.Text = "连接失败……";
                return;
            }
        }

        private IPAddress getIPAddress()//
        {
            // 获得本机局域网IP地址  
            IPAddress[] AddressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            if (AddressList.Length < 1)
            {
                this.Text = "本机IP未获取";
                return null;
            }
            return AddressList[0];
        }

        private int getValidPort(string port)
        {
            int lport;
            //测试端口号是否有效  
            try
            {
                //是否为空  
                if (port == "")
                {
                    this.Text = "端口号无效，不能启动DUP";
                }
                lport = System.Convert.ToInt32(port);
            }
            catch (Exception e)
            {
                //txtServerIp.Enabled = true;
                //btnConnect.Enabled = true;
                MessageBox.Show("服务器连失败!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Text = "连接失败……";
                return 1;
            }
            return lport;
        }

        private IPAddress getValidIP(string ip)
        {
            IPAddress lip = null;
            //测试IP是否有效  
            try
            {
                //是否为空  
                if (!IPAddress.TryParse(ip, out lip))
                {
                    this.Text = "IP无效，不能启动DUP";
                }
            }
            catch (Exception e)
            {
                this.Text = "无效的IP：" + e.ToString() + "\n";
                return null;
            }
            return lip;
        }

        private void ReceiveHandle()
        {
            //接收数据处理线程  
            string msg;
            byte[] data = new byte[1024];
            Control.CheckForIllegalCrossThreadCalls = false;
            while (RunningFlag)
            {
                if (mySocket == null || mySocket.Available < 1)
                {
                    //Thread.Sleep(200);
                    continue;
                }
                //跨线程调用控件  
                //接收UDP数据报，引用参数RemotePoint获得源地址  
                try
                {
                    int rlen = mySocket.ReceiveFrom(data, ref RemotePoint);
                    msg = Encoding.Default.GetString(data, 0, rlen);
                    rtxChatInfo.AppendText(DateTime.Now.ToString());
                    rtxChatInfo.AppendText(" 服务器说:\n");
                    rtxChatInfo.AppendText(msg + "\n");
                    //下拉框
                    rtxChatInfo.SelectionStart = rtxChatInfo.Text.Length;
                    rtxChatInfo.Focus();
                    rtxSendMessage.Focus();
                }
                catch
                {
                    MessageBox.Show("远程主机无响应");
                }
            }
        }

        private void UDPSend()
        {
            try
            {
                if (rtxSendMessage.Text.Trim() != "") // 发送消息不为空
                {
                    //HexToAsscii.HexToAsci(rtxSendMessage.Text)
                    string abc = HexToAsscii.HexToAsci(rtxSendMessage.Text);
                    if (abc != null)
                    {
                        byte[] data = Encoding.Default.GetBytes(abc);
                        mySocket.SendTo(data, data.Length, SocketFlags.None, RemotePoint);
                        rtxChatInfo.AppendText(DateTime.Now.ToString()); // 显示框 rtxChatInfo
                        rtxChatInfo.AppendText(" 客户端说: \n");
                        rtxChatInfo.AppendText(rtxSendMessage.Text + "\n");
                        rtxSendMessage.Clear();
                    }
                    //下拉框
                    rtxChatInfo.SelectionStart = rtxChatInfo.Text.Length;
                    rtxChatInfo.Focus();
                    rtxSendMessage.Focus();
                }
                else
                {
                    MessageBox.Show("信息不能为空!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtServerIp.Focus();
                    return;
                }
            }
            catch
            {
                txtServerIp.Enabled = true;
                btnConnect.Enabled = true;
                MessageBox.Show("服务器连失败!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Text = "连接失败……";
                return;
            }
        }
        #endregion
    }
}