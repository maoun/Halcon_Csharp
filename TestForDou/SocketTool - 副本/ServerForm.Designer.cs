namespace SocketTool
{
    partial class ServerForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbConnections = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cbLog = new System.Windows.Forms.CheckBox();
            this.btnOpenLog = new System.Windows.Forms.Button();
            this.PacketView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtConn = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.connectionView = new System.Windows.Forms.ListView();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbAscII = new System.Windows.Forms.RadioButton();
            this.rbHex = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbUdp = new System.Windows.Forms.RadioButton();
            this.rbTcp = new System.Windows.Forms.RadioButton();
            this.btnStopListen = new System.Windows.Forms.Button();
            this.btnListen = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.rtbData = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbAutoSend = new System.Windows.Forms.CheckBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.refreshConnectionWorker = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            this.tbConnections.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbConnections);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnStopListen);
            this.panel1.Controls.Add(this.btnListen);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.rtbData);
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
            this.panel1.Size = new System.Drawing.Size(905, 449);
            this.panel1.TabIndex = 1;
            // 
            // tbConnections
            // 
            this.tbConnections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConnections.Controls.Add(this.tabPage1);
            this.tbConnections.Controls.Add(this.tabPage2);
            this.tbConnections.Location = new System.Drawing.Point(0, 180);
            this.tbConnections.Name = "tbConnections";
            this.tbConnections.SelectedIndex = 0;
            this.tbConnections.Size = new System.Drawing.Size(902, 266);
            this.tbConnections.TabIndex = 24;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cbLog);
            this.tabPage1.Controls.Add(this.btnOpenLog);
            this.tabPage1.Controls.Add(this.PacketView);
            this.tabPage1.Controls.Add(this.btnClearLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(884, 241);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "接收数据";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cbLog
            // 
            this.cbLog.AutoSize = true;
            this.cbLog.Checked = true;
            this.cbLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLog.ForeColor = System.Drawing.Color.Maroon;
            this.cbLog.Location = new System.Drawing.Point(106, 11);
            this.cbLog.Name = "cbLog";
            this.cbLog.Size = new System.Drawing.Size(156, 16);
            this.cbLog.TabIndex = 24;
            this.cbLog.Text = "保存数据到日志文件当中";
            this.cbLog.UseVisualStyleBackColor = true;
            // 
            // btnOpenLog
            // 
            this.btnOpenLog.Location = new System.Drawing.Point(276, 6);
            this.btnOpenLog.Name = "btnOpenLog";
            this.btnOpenLog.Size = new System.Drawing.Size(96, 25);
            this.btnOpenLog.TabIndex = 23;
            this.btnOpenLog.Text = "打开日志目录";
            this.btnOpenLog.UseVisualStyleBackColor = true;
            this.btnOpenLog.Click += new System.EventHandler(this.btnOpenLog_Click);
            // 
            // PacketView
            // 
            this.PacketView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PacketView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.PacketView.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PacketView.FullRowSelect = true;
            this.PacketView.Location = new System.Drawing.Point(3, 36);
            this.PacketView.Name = "PacketView";
            this.PacketView.Size = new System.Drawing.Size(881, 202);
            this.PacketView.TabIndex = 2;
            this.PacketView.UseCompatibleStateImageBehavior = false;
            this.PacketView.View = System.Windows.Forms.View.Details;
            this.PacketView.SelectedIndexChanged += new System.EventHandler(this.PacketView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 46;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "连接ID";
            // 
            // columnHeader6
            // 
            this.columnHeader6.DisplayIndex = 3;
            this.columnHeader6.Text = "IP地址";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader6.Width = 118;
            // 
            // columnHeader2
            // 
            this.columnHeader2.DisplayIndex = 2;
            this.columnHeader2.Text = "时间";
            this.columnHeader2.Width = 72;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "数据报文";
            this.columnHeader3.Width = 520;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "字节数";
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(13, 6);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 25);
            this.btnClearLog.TabIndex = 21;
            this.btnClearLog.Text = "清空日志";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnDisconnect);
            this.tabPage2.Controls.Add(this.btnSend);
            this.tabPage2.Controls.Add(this.txtConn);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.richTextBox2);
            this.tabPage2.Controls.Add(this.connectionView);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(894, 241);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "当前客户端连接";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(557, 10);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 25);
            this.btnDisconnect.TabIndex = 28;
            this.btnDisconnect.Text = "断开连接";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(470, 10);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 25);
            this.btnSend.TabIndex = 27;
            this.btnSend.Text = "发送数据";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // txtConn
            // 
            this.txtConn.Enabled = false;
            this.txtConn.Location = new System.Drawing.Point(70, 13);
            this.txtConn.Name = "txtConn";
            this.txtConn.Size = new System.Drawing.Size(127, 21);
            this.txtConn.TabIndex = 26;
            this.txtConn.Text = "127.0.0.1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 25;
            this.label5.Text = "当前连接：";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(199, 13);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(265, 21);
            this.richTextBox2.TabIndex = 11;
            this.richTextBox2.Text = "请选择某一连接后发送数据";
            // 
            // connectionView
            // 
            this.connectionView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader10,
            this.columnHeader9,
            this.columnHeader11});
            this.connectionView.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.connectionView.FullRowSelect = true;
            this.connectionView.Location = new System.Drawing.Point(3, 41);
            this.connectionView.Name = "connectionView";
            this.connectionView.Size = new System.Drawing.Size(891, 197);
            this.connectionView.TabIndex = 3;
            this.connectionView.UseCompatibleStateImageBehavior = false;
            this.connectionView.View = System.Windows.Forms.View.Details;
            this.connectionView.SelectedIndexChanged += new System.EventHandler(this.connectionView_SelectedIndexChanged);
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "序号";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "连接ID";
            this.columnHeader8.Width = 83;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "创建时间";
            this.columnHeader10.Width = 110;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "IP地址";
            this.columnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader9.Width = 123;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "在线时间";
            this.columnHeader11.Width = 333;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbAscII);
            this.groupBox2.Controls.Add(this.rbHex);
            this.groupBox2.Location = new System.Drawing.Point(48, 48);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 29);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "显示格式";
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
            this.groupBox1.Location = new System.Drawing.Point(290, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(172, 28);
            this.groupBox1.TabIndex = 22;
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
            // btnStopListen
            // 
            this.btnStopListen.Location = new System.Drawing.Point(387, 17);
            this.btnStopListen.Name = "btnStopListen";
            this.btnStopListen.Size = new System.Drawing.Size(75, 25);
            this.btnStopListen.TabIndex = 20;
            this.btnStopListen.Text = "停止监听";
            this.btnStopListen.UseVisualStyleBackColor = true;
            this.btnStopListen.Click += new System.EventHandler(this.btnStopListen_Click);
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(290, 17);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(75, 25);
            this.btnListen.TabIndex = 19;
            this.btnListen.Text = "开始监听";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Maroon;
            this.label6.Location = new System.Drawing.Point(304, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(287, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "应答数据包(默认是原文回复，如要定制回复请填写：";
            // 
            // rtbData
            // 
            this.rtbData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbData.Location = new System.Drawing.Point(61, 118);
            this.rtbData.Name = "rtbData";
            this.rtbData.Size = new System.Drawing.Size(832, 56);
            this.rtbData.TabIndex = 10;
            this.rtbData.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(221, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "秒钟发送一次";
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(176, 87);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(39, 21);
            this.txtInterval.TabIndex = 8;
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
            this.cbAutoSend.Checked = true;
            this.cbAutoSend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoSend.Location = new System.Drawing.Point(61, 91);
            this.cbAutoSend.Name = "cbAutoSend";
            this.cbAutoSend.Size = new System.Drawing.Size(72, 16);
            this.cbAutoSend.TabIndex = 6;
            this.cbAutoSend.Text = "自动应答";
            this.cbAutoSend.UseVisualStyleBackColor = true;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(209, 20);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(39, 21);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "8899";
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
            // refreshConnectionWorker
            // 
            this.refreshConnectionWorker.WorkerReportsProgress = true;
            this.refreshConnectionWorker.WorkerSupportsCancellation = true;
            this.refreshConnectionWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.refreshConnectionWorker_DoWork);
            this.refreshConnectionWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.refreshConnectionWorker_ProgressChanged);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 449);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServerForm";
            this.Text = "客户端";
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tbConnections.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox rtbData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbAutoSend;
        private System.Windows.Forms.RadioButton rbHex;
        private System.Windows.Forms.RadioButton rbAscII;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView PacketView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton rbUdp;
        private System.Windows.Forms.RadioButton rbTcp;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btnStopListen;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tbConnections;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView connectionView;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.ComponentModel.BackgroundWorker refreshConnectionWorker;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtConn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnOpenLog;
        private System.Windows.Forms.CheckBox cbLog;
    }
}