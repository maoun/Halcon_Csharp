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
        public void BeginLister()     // 打开服务器 子函数
        {
            
                //try
                //{
                   
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
                //}
                //catch 
                //{             
                //}// 不做处理 继续测试监听
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
    }
}