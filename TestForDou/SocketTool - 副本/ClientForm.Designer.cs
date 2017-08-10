namespace SocketTool
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.txtHeartTime = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.rtSendData = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.rtHeartData = new System.Windows.Forms.RichTextBox();
            this.cbLog = new System.Windows.Forms.CheckBox();
            this.btnOpenLog = new System.Windows.Forms.Button();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbAscII = new System.Windows.Forms.RadioButton();
            this.rbHex = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbUdp = new System.Windows.Forms.RadioButton();
            this.rbTcp = new System.Windows.Forms.RadioButton();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.rtLoginData = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbAutoSend = new System.Windows.Forms.CheckBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PacketView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtHeartTime);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.rtSendData);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.rtHeartData);
            this.panel1.Controls.Add(this.cbLog);
            this.panel1.Controls.Add(this.btnOpenLog);
            this.panel1.Controls.Add(this.btnClearLog);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnDisconnect);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnSend);
            this.panel1.Controls.Add(this.rtLoginData);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtInterval);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbAutoSend);
            this.panel1.Controls.Add(this.txtPort);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtIP);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(676, 449);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(409, 92);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 35;
            this.label9.Text = "秒";
            // 
            // txtHeartTime
            // 
            this.txtHeartTime.Location = new System.Drawing.Point(363, 87);
            this.txtHeartTime.Name = "txtHeartTime";
            this.txtHeartTime.Size = new System.Drawing.Size(39, 21);
            this.txtHeartTime.TabIndex = 34;
            this.txtHeartTime.Text = "1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 175);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 33;
            this.label7.Text = "数据包：";
            // 
            // rtSendData
            // 
            this.rtSendData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtSendData.Location = new System.Drawing.Point(59, 173);
            this.rtSendData.Name = "rtSendData";
            this.rtSendData.Size = new System.Drawing.Size(612, 46);
            this.rtSendData.TabIndex = 32;
            this.rtSendData.Text = resources.GetString("rtSendData.Text");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 31;
            this.label5.Text = "心跳包：";
            // 
            // rtHeartData
            // 
            this.rtHeartData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtHeartData.Location = new System.Drawing.Point(60, 145);
            this.rtHeartData.Name = "rtHeartData";
            this.rtHeartData.Size = new System.Drawing.Size(612, 22);
            this.rtHeartData.TabIndex = 30;
            this.rtHeartData.Text = "010100020000000000000002";
            // 
            // cbLog
            // 
            this.cbLog.AutoSize = true;
            this.cbLog.Checked = true;
            this.cbLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLog.ForeColor = System.Drawing.Color.Maroon;
            this.cbLog.Location = new System.Drawing.Point(170, 228);
            this.cbLog.Name = "cbLog";
            this.cbLog.Size = new System.Drawing.Size(156, 16);
            this.cbLog.TabIndex = 29;
            this.cbLog.Text = "保存数据到日志文件当中";
            this.cbLog.UseVisualStyleBackColor = true;
            // 
            // btnOpenLog
            // 
            this.btnOpenLog.Location = new System.Drawing.Point(340, 223);
            this.btnOpenLog.Name = "btnOpenLog";
            this.btnOpenLog.Size = new System.Drawing.Size(96, 25);
            this.btnOpenLog.TabIndex = 28;
            this.btnOpenLog.Text = "打开日志目录";
            this.btnOpenLog.UseVisualStyleBackColor = true;
            this.btnOpenLog.Click += new System.EventHandler(this.btnOpenLog_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(80, 223);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 25);
            this.btnClearLog.TabIndex = 27;
            this.btnClearLog.Text = "清空日志";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 229);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 26;
            this.label8.Text = "接收数据：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbAscII);
            this.groupBox2.Controls.Add(this.rbHex);
            this.groupBox2.Location = new System.Drawing.Point(26, 48);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 29);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据格式";
            // 
            // rbAscII
            // 
            this.rbAscII.AutoSize = true;
            this.rbAscII.Checked = true;
            this.rbAscII.Location = new System.Drawing.Point(57, 10);
            this.rbAscII.Name = "rbAscII";
            this.rbAscII.Size = new System.Drawing.Size(65, 16);
            this.rbAscII.TabIndex = 4;
            this.rbAscII.TabStop = true;
            this.rbAscII.Text = "ASCII码";
            this.rbAscII.UseVisualStyleBackColor = true;
            // 
            // rbHex
            // 
            this.rbHex.AutoSize = true;
            this.rbHex.Location = new System.Drawing.Point(128, 10);
            this.rbHex.Name = "rbHex";
            this.rbHex.Size = new System.Drawing.Size(59, 16);
            this.rbHex.TabIndex = 5;
            this.rbHex.Text = "16进制";
            this.rbHex.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbUdp);
            this.groupBox1.Controls.Add(this.rbTcp);
            this.groupBox1.Location = new System.Drawing.Point(268, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(172, 28);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "通信协议";
            // 
            // rbUdp
            // 
            this.rbUdp.AutoSize = true;
            this.rbUdp.Location = new System.Drawing.Point(102, 10);
            this.rbUdp.Name = "rbUdp";
            this.rbUdp.Size = new System.Drawing.Size(41, 16);
            this.rbUdp.TabIndex = 15;
            this.rbUdp.Text = "UDP";
            this.rbUdp.UseVisualStyleBackColor = true;
            // 
            // rbTcp
            // 
            this.rbTcp.AutoSize = true;
            this.rbTcp.Checked = true;
            this.rbTcp.Location = new System.Drawing.Point(55, 10);
            this.rbTcp.Name = "rbTcp";
            this.rbTcp.Size = new System.Drawing.Size(41, 16);
            this.rbTcp.TabIndex = 14;
            this.rbTcp.TabStop = true;
            this.rbTcp.Text = "TCP";
            this.rbTcp.UseVisualStyleBackColor = true;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(543, 87);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 25);
            this.btnDisconnect.TabIndex = 17;
            this.btnDisconnect.Text = "断开连接";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "登录包：";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(456, 87);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 25);
            this.btnSend.TabIndex = 11;
            this.btnSend.Text = "发送数据";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // rtLoginData
            // 
            this.rtLoginData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtLoginData.Location = new System.Drawing.Point(61, 118);
            this.rtLoginData.Name = "rtLoginData";
            this.rtLoginData.Size = new System.Drawing.Size(612, 23);
            this.rtLoginData.TabIndex = 10;
            this.rtLoginData.Text = "010100010000003000000001000000000000000000000061646D696E0000000000000000000000000" +
    "0000000000000000000000000000061646D696E";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(221, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "秒钟发送一次，心跳时间";
            // 
            // txtInterval
            // 
            this.txtInterval.Enabled = false;
            this.txtInterval.Location = new System.Drawing.Point(176, 87);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(39, 21);
            this.txtInterval.TabIndex = 8;
            this.txtInterval.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(141, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "每隔";
            // 
            // cbAutoSend
            // 
            this.cbAutoSend.AutoSize = true;
            this.cbAutoSend.Location = new System.Drawing.Point(61, 92);
            this.cbAutoSend.Name = "cbAutoSend";
            this.cbAutoSend.Size = new System.Drawing.Size(72, 16);
            this.cbAutoSend.TabIndex = 6;
            this.cbAutoSend.Text = "自动发送";
            this.cbAutoSend.UseVisualStyleBackColor = true;
            this.cbAutoSend.CheckedChanged += new System.EventHandler(this.cbAutoSend_CheckedChanged);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(209, 20);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(39, 21);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "10010";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口：";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(61, 20);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(100, 21);
            this.txtIP.TabIndex = 1;
            this.txtIP.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP：";
            // 
            // PacketView
            // 
            this.PacketView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PacketView.BackColor = System.Drawing.SystemColors.Menu;
            this.PacketView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.PacketView.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PacketView.FullRowSelect = true;
            this.PacketView.GridLines = true;
            this.PacketView.LabelEdit = true;
            this.PacketView.Location = new System.Drawing.Point(2, 254);
            this.PacketView.Name = "PacketView";
            this.PacketView.Size = new System.Drawing.Size(671, 183);
            this.PacketView.TabIndex = 2;
            this.PacketView.UseCompatibleStateImageBehavior = false;
            this.PacketView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 44;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "时间";
            this.columnHeader2.Width = 71;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "数据报文";
            this.columnHeader3.Width = 426;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "字节数";
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 449);
            this.Controls.Add(this.PacketView);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClientForm";
            this.Text = "客户端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox rtLoginData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbAutoSend;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView PacketView;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbAscII;
        private System.Windows.Forms.RadioButton rbHex;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbUdp;
        private System.Windows.Forms.RadioButton rbTcp;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbLog;
        private System.Windows.Forms.Button btnOpenLog;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox rtSendData;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox rtHeartData;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtHeartTime;
    }
}