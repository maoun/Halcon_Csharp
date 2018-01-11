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
using System.Text.RegularExpressions;

namespace client
{
    public partial class Form1 : Form
    {
        int mode=0; 
        public Thread thread;
        private IPEndPoint ipLocalPoint;
        private EndPoint RemotePoint;
        private Socket mySocket;
        private bool RunningFlag = false;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;            
        }

       
        private void btnConnect_Click(object sender, EventArgs e)  //  连接按钮 执行
        {
            if(mode==0)
            {
                Listen();
            }
            else
            {
                UDPListen();
            }
                  
        }

        private void btnSend_Click(object sender, EventArgs e)  // 发送按钮  执行
        {
            if(mode==0)
            {
                SendMessage();
            }
            else
            {
                UDPSend();
            }
            
        }

        private void tmrGetMess_Tick(object sender, EventArgs e)  // 执行的接收信息
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("这样会中断与服务器的连接,你要关闭该窗体吗？", "客户端信息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (DialogResult.Yes == dr)
            {
                e.Cancel = false;
                thread.Abort();
            }
            else
            {
                e.Cancel = true;
            }
           
        }

        private void rtxChatInfo_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void grpChatInfo_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text=="TCP/IP")
            {
                mode = 0;
            }
            else if(comboBox1.Text=="UDP")
            {
                mode = 1;
            }
        }
       

        #region  UDP通讯          
        public void UDPListen()
        {

            try
            {
                //得到本机IP，设置UDP端口号     
                ipLocalPoint = new IPEndPoint(IPAddress.Parse(txtServerIp.Text), getValidPort(txtPort.Text) + 1); //本地端口号和客户机端口号不能相同              
                //定义网络类型，数据连接类型和网络协议UDP  
                mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                //绑定网络地址  
                mySocket.Bind(ipLocalPoint);

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
                this.Text = "无效的IP：";
                return null;
            }
            return lip;
        }
       
        private void ReceiveHandle()
        {
            //接收数据处理线程  
            string msg;
            byte[] data = new byte[1024];
            Control.CheckForIllegalCrossThreadCalls = false; //跨线程调用控件 
            while (RunningFlag)
            {              
                try
                {
                    if (mySocket == null || mySocket.Available < 1)
                    {                     
                        continue;
                    }                
                    int rlen = mySocket.ReceiveFrom(data, ref RemotePoint);
                    msg = Encoding.Default.GetString(data, 0, rlen);
                    string seversay = HexToAscii.AsciiToHex(msg);
                    seversay =Regex.Replace(seversay,@"(\d{2}(?!$))","$1 ");  
                    rtxChatInfo.AppendText(DateTime.Now.ToString());
                    rtxChatInfo.AppendText(" 服务器说:\n");
                    rtxChatInfo.AppendText(seversay + " " + "[" + rlen + "]" + "\n");
                    //下拉框
                    rtxChatInfo.SelectionStart = rtxChatInfo.Text.Length;
                    rtxChatInfo.Focus();
                    rtxSendMessage.Focus();
                }
                catch 
                {
                    //MessageBox.Show("远程主机无响应");
                }           
            }
        }
        private void UDPSend()
        {
            try
            {
                if (rtxSendMessage.Text.Trim() != "") // 发送消息不为空
                {
                    if (rtxSendMessage.Text != null)
                    {
                        byte[] data = HexToAscii.HexToAsci(rtxSendMessage.Text);
                        //得到客户机IP  
                        IPEndPoint ipep = new IPEndPoint(getValidIP(txtServerIp.Text), getValidPort(txtPort.Text));
                        RemotePoint = (EndPoint)(ipep);
                        mySocket.SendTo(data, data.Length, SocketFlags.None, RemotePoint);  
                        rtxChatInfo.AppendText(DateTime.Now.ToString()); // 显示框 rtxChatInfo
                        rtxChatInfo.AppendText(" 客户端说: \n");
                        rtxChatInfo.AppendText(rtxSendMessage.Text + "\n");
                        //rtxSendMessage.Clear();
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

        #region TCP通讯       
        public void GetConn()   //  连接函数
        {
            CheckForIllegalCrossThreadCalls = false;
            while (true)
            {
                try
                {
                    txtServerIp.Enabled = false;
                    btnConnect.Enabled = false;
                    this.Text = "客户端   " + "正在与" + txtServerIp.Text.Trim() + txtPort.Text.Trim() + "连接……";
                    return;
                }
                catch
                {
                    txtServerIp.Enabled = true;
                    btnConnect.Enabled = true;
                    this.Text = "连接失败……";
                }
            }
        }
        public void Listen()
        {
            // 开始监听 代码
            if (txtServerIp.Text.Trim() == "")  // 服务器IP
            {
                MessageBox.Show("请输入服务器IP", "客户端信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {

                try
                {
                    if(mySocket!=null)
                    {
                        mySocket.Close();
                    }
                    
                    //定义网络类型，数据连接类型和网络协议TCP 
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(txtServerIp.Text), System.Convert.ToInt32(txtPort.Text));
                    mySocket = new Socket(endPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    //连接网络地址
                    mySocket.Connect(endPoint);
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
        }
        public void SendMessage()
        {
            try
            {
                if (rtxSendMessage.Text.Trim() != "") // 发送消息不为空
                {
                    byte[] data = HexToAscii.HexToAsci(rtxSendMessage.Text);
                    if (data != null)
                    {
                        //得到客户机IP  
                        IPEndPoint ipep = new IPEndPoint(getValidIP(txtServerIp.Text), getValidPort(txtPort.Text));
                        RemotePoint = (EndPoint)(ipep);
                        mySocket.SendTo(data, data.Length, SocketFlags.None, RemotePoint);  
                        rtxChatInfo.AppendText(DateTime.Now.ToString()); // 显示框 rtxChatInfo
                        rtxChatInfo.AppendText(" 客户端说: \n");
                        rtxChatInfo.AppendText(rtxSendMessage.Text + "\n");
                        //rtxSendMessage.Clear();
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
    class HexToAscii
    {
        public static byte[] HexToAsci(string strHex)
        {
            //hex转ascii           
            strHex = strHex.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");//去掉换行和回车
            try
            {
                byte[] buff = new byte[strHex.Length / 2];
                int index = 0;
                for (int i = 0; i < strHex.Length; i += 2)
                {
                    buff[index] = Convert.ToByte(strHex.Substring(i, 2), 16);
                    ++index;
                }
                return buff;
            }
            catch
            {
                MessageBox.Show("不是十六进制数");
                //string result = null;
                return null;
            }
        }
        public static string AsciiToHex(string strAsscii)
        {
            ////ascii转hex
            //ascii转字符串
            byte[] array = System.Text.Encoding.ASCII.GetBytes(strAsscii);
            string strB = System.Text.Encoding.ASCII.GetString(array);
            //字符串转hex
            byte[] bytes = System.Text.Encoding.Default.GetBytes(strB);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
            }
            return str;
        }
    }
