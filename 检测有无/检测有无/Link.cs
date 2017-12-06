using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;  

namespace 检测有无
{
    public partial class Link : Form
    {

        public Thread thread;
        //private IPEndPoint ipLocalPoint;
        private EndPoint RemotePoint;
        private Socket mySocket;
        private bool RunningFlag = false;
        Matching fm = new Matching();

        public Link()
        {
            InitializeComponent();
        }

        private void Link_Load(object sender, EventArgs e)
        {
            txtServerIp.Text = fm.txtIP.Text;
        }

        private void Link_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("这样会中断与服务器的连接,你要关闭该窗体吗？", "客户端信息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); ;
            if (DialogResult.Yes == dr)
            {
                e.Cancel = false;
                try
                {
                    thread.Abort();
                }
                catch { }                
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            GiveValue(txtServerIp.Text);
            Listen();
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                thread.Abort();
                txtLog.AppendText(DateTime.Now.ToString() + "\n");
                txtLog.AppendText("连接已断开" + "\n");
                txtLog.Focus();
                btnConnect.Enabled = true;
            }
            catch
            {
                txtLog.AppendText(DateTime.Now.ToString() + "\n");
                txtLog.AppendText("连接不存在或未断开" + "\n");
                txtLog.Focus();
                btnConnect.Enabled = true;
            }
        }


        #region 功能
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
                    if (mySocket != null)
                    {
                        mySocket.Close();
                    }
                    //定义网络类型，数据连接类型和网络协议TCP 
                    IPAddress ip = getValidIP(txtServerIp.Text);
                    Ping pingSender = new Ping();
                    PingReply reply = pingSender.Send(ip, 1);//第一个参数为ip地址，第二个参数为ping的时间ms
                    if (reply.Status == IPStatus.Success)
                    {
                        //ping的通
                        txtLog.AppendText("IP地址有效" + "\n");
                        int port = getValidPort(txtPort.Text);
                        if (port != 0)
                        {
                            IPEndPoint endPoint = new IPEndPoint(ip, port);
                            mySocket = new Socket(endPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                            //连接网络地址
                            mySocket.Connect(endPoint);
                            //启动一个新的线程，执行方法this.ReceiveHandle，  
                            //以便在一个独立的进程中执行数据接收的操作 
                            RunningFlag = true;
                            thread = new Thread(new ThreadStart(ReceiveHandle));
                            thread.Start();
                            txtLog.AppendText(DateTime.Now.ToString() + "\n");
                            txtLog.AppendText("连接成功" + "\n");
                            txtLog.Focus();
                            btnConnect.Enabled = false;
                        }
                    }
                    else
                    {
                        //ping不通
                        txtLog.AppendText("IP地址无效" + "\n");
                    }                              
                }
                catch
                {
                    txtLog.AppendText(DateTime.Now.ToString() + "\n");
                    txtLog.AppendText("连接失败" + "\n");
                    txtLog.Focus();
                    btnConnect.Enabled = true;
                    return;
                }
            }
        }

        //接收数据处理线程
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
                    seversay = Regex.Replace(seversay, @"(\d{2}(?!$))", "$1 ");
                    txtLog.AppendText(DateTime.Now.ToString()+"\n");
                    txtLog.AppendText(" 服务器说:\n");
                    txtLog.AppendText(seversay + " " + "[" + rlen + "]" + "\n");
                    //下拉框
                    txtLog.SelectionStart = txtLog.Text.Length;
                    txtLog.Focus();
                }
                catch { }
            }
        }

        //发送数据
        public void SendMessage()
        {
            try
            {
                if (fm.txtTrans.Text.Trim() != "") // 发送消息不为空
                {
                    byte[] data = HexToAscii.HexToAsci(fm.txtTrans.Text);
                    if (data != null)
                    {
                        //得到客户机IP  
                        IPEndPoint ipep = new IPEndPoint(getValidIP(txtServerIp.Text), getValidPort(txtPort.Text));
                        RemotePoint = (EndPoint)(ipep);
                        mySocket.SendTo(data, data.Length, SocketFlags.None, RemotePoint);
                        txtLog.AppendText(DateTime.Now.ToString()); // 显示框 rtxChatInfo
                        txtLog.AppendText(" 客户端说: \n");
                        txtLog.AppendText(fm.txtTrans.Text + "\n");
                        txtLog.Focus();
                        //rtxSendMessage.Clear();
                    }
                    //下拉框
                    txtLog.SelectionStart = txtLog.Text.Length;
                    txtLog.Focus();
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

        //测试IP转换格式
        private IPAddress getValidIP(string ip)
        {
            IPAddress lip = null;
            //测试IP是否有效  
            try
            {
                txtLog.AppendText(DateTime.Now.ToString() + "\n");
                txtLog.AppendText("正在尝试连接IP..." + "\n");
                txtLog.Focus();
                IPAddress.TryParse(ip, out lip);
            }
            catch
            {
                txtLog.AppendText(DateTime.Now.ToString() + "\n");
                txtLog.AppendText("无效的IP" + "\n");
                txtLog.Focus();
                return null;
            }
            return lip;
        }

        //测试端口号并转换格式
        private int getValidPort(string port)
        {
            int lport;
            //测试端口号是否有效  
            try
            {
                lport = System.Convert.ToInt32(port);
            }
            catch
            {
                //txtServerIp.Enabled = true;
                //btnConnect.Enabled = true;
                txtLog.AppendText(DateTime.Now.ToString() + "\n");
                txtLog.AppendText("无效的端口号" + "\n");
                txtLog.Focus();
                return 1;
            }
            return lport;
        }

        //委托
        public Action<string> GiveValue;

        //控制txt行数
        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            if (this.txtLog.Lines.Length > 20)
            {
                string[] newlines = new string[19];
                Array.Copy(this.txtLog.Lines, this.txtLog.Lines.Length - 20, newlines, 0, 19);
                this.txtLog.Lines = newlines;
                //this.txtLog.Focus();//获取焦点
                txtLog.AppendText("\n");
            }
        }
        #endregion 功能



        #region FINS/TCP通讯      
      
        #endregion FINS/TCP通讯
    }
}
