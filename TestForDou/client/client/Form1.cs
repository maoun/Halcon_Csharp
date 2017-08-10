using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;//
using System.Net;//
using System.IO;//
using System.Threading;//

namespace client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public TcpClient TcpClient;   // TCP连接     
        StreamReader ClientReader;  // 网络流 读数据
        StreamWriter ClientWriter;  // 网络流 写数据
        NetworkStream Stream; // 网络流
        Thread Thd;

        void GetMessage()  //  接收服务器传的数据
        {
            if (Stream != null && Stream.DataAvailable)
            {
                //设定一个缓冲区，大小默认为1024字节
                const int bufferSize = 1024;
                byte[] buffer = new byte[bufferSize];
                int readBytes = 0;
                Stream.ReadTimeout = 1000;     
                readBytes = Stream.Read(buffer, 0, bufferSize);
                string str = Encoding.ASCII.GetString(buffer).Substring(0, readBytes);//ascii码转换为string
                rtxChatInfo.AppendText(DateTime.Now.ToString());
                rtxChatInfo.AppendText(" 服务器说:\n");
                rtxChatInfo.AppendText(str + "\n");
                //下拉框
                rtxChatInfo.SelectionStart = rtxChatInfo.Text.Length;
                rtxChatInfo.Focus();
                rtxSendMessage.Focus();
            }          
        }
        void GetConn()   //  连接函数
        {
            CheckForIllegalCrossThreadCalls = false;
            while (true)
            {
                try
                {
                    TcpClient = new TcpClient(txtServerIp.Text, int.Parse(txtPort.Text.Trim()));
                    Stream = TcpClient.GetStream();
                    ClientReader = new StreamReader(Stream);
                    ClientWriter = new StreamWriter(Stream);
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

                    //MessageBox.Show("连接失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)  //  连接按钮 执行
        { // 开始监听 代码
            if (txtServerIp.Text.Trim() == "")  // 服务器IP
            {
                MessageBox.Show("请输入服务器IP", "客户端信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                Thd = new Thread(new ThreadStart(GetConn));// 先执行 连接函数 ，在执行现场启动，新线程
                Thd.Start();// 线程启动
            }
        }


        private void btnSend_Click(object sender, EventArgs e)  // 发送按钮  执行
        {
            try
            {
                if (rtxSendMessage.Text.Trim() != "") // 发送消息不为空
                {
                    //HexToAsscii.HexToAsci(rtxSendMessage.Text)
                    string abc = HexToAsscii.HexToAsci(rtxSendMessage.Text);
                    if (abc!=null)
                    {
                        ClientWriter.Write(abc);//信息写入流
                        ClientWriter.Flush();
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


        private void tmrGetMess_Tick(object sender, EventArgs e)  // 执行的接收信息
        {
            GetMessage();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("这样会中断与服务器的连接,你要关闭该窗体吗？", "客户端信息", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); ;
            if (DialogResult.Yes == dr)
            {
                e.Cancel = false;
                if (Thd != null)
                {
                    Thd.Abort();
                }
               
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
       
       
    }
    class HexToAsscii
    {
        public static string HexToAsci(string strHex)
        {
            //hex转ascii
            //去掉空格
            CharEnumerator CEnumerator = strHex.GetEnumerator();
            string tmp = null;
            while (CEnumerator.MoveNext())
            {
                byte[] array = new byte[1];
                array = System.Text.Encoding.ASCII.GetBytes(CEnumerator.Current.ToString());
                int asciicode = (short)(array[0]);
                if (asciicode != 32)
                {
                    tmp += CEnumerator.Current.ToString();
                }
            }

            try
            {
                tmp = tmp.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");//去掉换行和回车
                byte[] buff = new byte[tmp.Length / 2];
                int index = 0;
                for (int i = 0; i < tmp.Length; i += 2)
                {
                    buff[index] = Convert.ToByte(tmp.Substring(i, 2), 16);
                    ++index;
                }
                string result = Encoding.Default.GetString(buff);
                return result;
            }
            catch
            {
                MessageBox.Show("不是十六进制数");
                string result = null;
                return result;
            }
        }
        public static string AssciiToHex(string strAsscii)
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
}