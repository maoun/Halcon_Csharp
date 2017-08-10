namespace client
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblServerIp = new System.Windows.Forms.Label();
            this.txtServerIp = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.tmrGetMess = new System.Windows.Forms.Timer(this.components);
            this.grpClient = new System.Windows.Forms.GroupBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.grpChatInfo = new System.Windows.Forms.GroupBox();
            this.rtxChatInfo = new System.Windows.Forms.RichTextBox();
            this.grpSend = new System.Windows.Forms.GroupBox();
            this.rtxSendMessage = new System.Windows.Forms.RichTextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpClient.SuspendLayout();
            this.grpChatInfo.SuspendLayout();
            this.grpSend.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblServerIp
            // 
            this.lblServerIp.AutoSize = true;
            this.lblServerIp.Location = new System.Drawing.Point(29, 33);
            this.lblServerIp.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblServerIp.Name = "lblServerIp";
            this.lblServerIp.Size = new System.Drawing.Size(95, 21);
            this.lblServerIp.TabIndex = 8;
            this.lblServerIp.Text = "服务器IP";
            // 
            // txtServerIp
            // 
            this.txtServerIp.Location = new System.Drawing.Point(138, 24);
            this.txtServerIp.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtServerIp.Name = "txtServerIp";
            this.txtServerIp.Size = new System.Drawing.Size(305, 31);
            this.txtServerIp.TabIndex = 6;
            this.txtServerIp.Text = "192.168.250.1";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(675, 24);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(105, 40);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(502, 112);
            this.btnSend.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(138, 40);
            this.btnSend.TabIndex = 11;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tmrGetMess
            // 
            this.tmrGetMess.Enabled = true;
            this.tmrGetMess.Interval = 200;
            this.tmrGetMess.Tick += new System.EventHandler(this.tmrGetMess_Tick);
            // 
            // grpClient
            // 
            this.grpClient.Controls.Add(this.txtPort);
            this.grpClient.Controls.Add(this.lblPort);
            this.grpClient.Controls.Add(this.btnConnect);
            this.grpClient.Controls.Add(this.txtServerIp);
            this.grpClient.Controls.Add(this.lblServerIp);
            this.grpClient.Location = new System.Drawing.Point(24, 4);
            this.grpClient.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.grpClient.Name = "grpClient";
            this.grpClient.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.grpClient.Size = new System.Drawing.Size(790, 77);
            this.grpClient.TabIndex = 13;
            this.grpClient.TabStop = false;
            this.grpClient.Text = "服务器连接";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(521, 24);
            this.txtPort.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(114, 31);
            this.txtPort.TabIndex = 16;
            this.txtPort.Text = "9600";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(457, 33);
            this.lblPort.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(52, 21);
            this.lblPort.TabIndex = 9;
            this.lblPort.Text = "端口";
            // 
            // grpChatInfo
            // 
            this.grpChatInfo.Controls.Add(this.rtxChatInfo);
            this.grpChatInfo.Location = new System.Drawing.Point(24, 91);
            this.grpChatInfo.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.grpChatInfo.Name = "grpChatInfo";
            this.grpChatInfo.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.grpChatInfo.Size = new System.Drawing.Size(790, 424);
            this.grpChatInfo.TabIndex = 14;
            this.grpChatInfo.TabStop = false;
            this.grpChatInfo.Text = "聊天信息";
            this.grpChatInfo.Enter += new System.EventHandler(this.grpChatInfo_Enter);
            // 
            // rtxChatInfo
            // 
            this.rtxChatInfo.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.rtxChatInfo.ForeColor = System.Drawing.Color.Transparent;
            this.rtxChatInfo.Location = new System.Drawing.Point(11, 35);
            this.rtxChatInfo.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.rtxChatInfo.Name = "rtxChatInfo";
            this.rtxChatInfo.ReadOnly = true;
            this.rtxChatInfo.Size = new System.Drawing.Size(763, 386);
            this.rtxChatInfo.TabIndex = 10;
            this.rtxChatInfo.TabStop = false;
            this.rtxChatInfo.Text = "";
            this.rtxChatInfo.TextChanged += new System.EventHandler(this.rtxChatInfo_TextChanged);
            // 
            // grpSend
            // 
            this.grpSend.Controls.Add(this.rtxSendMessage);
            this.grpSend.Controls.Add(this.btnClose);
            this.grpSend.Controls.Add(this.btnSend);
            this.grpSend.Location = new System.Drawing.Point(22, 525);
            this.grpSend.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.grpSend.Name = "grpSend";
            this.grpSend.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.grpSend.Size = new System.Drawing.Size(792, 166);
            this.grpSend.TabIndex = 15;
            this.grpSend.TabStop = false;
            this.grpSend.Text = "信息发送";
            // 
            // rtxSendMessage
            // 
            this.rtxSendMessage.ForeColor = System.Drawing.Color.Red;
            this.rtxSendMessage.Location = new System.Drawing.Point(6, 30);
            this.rtxSendMessage.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.rtxSendMessage.Name = "rtxSendMessage";
            this.rtxSendMessage.Size = new System.Drawing.Size(770, 69);
            this.rtxSendMessage.TabIndex = 12;
            this.rtxSendMessage.Text = "";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(651, 112);
            this.btnClose.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(138, 40);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 696);
            this.Controls.Add(this.grpSend);
            this.Controls.Add(this.grpChatInfo);
            this.Controls.Add(this.grpClient);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "客户端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpClient.ResumeLayout(false);
            this.grpClient.PerformLayout();
            this.grpChatInfo.ResumeLayout(false);
            this.grpSend.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblServerIp;
        private System.Windows.Forms.TextBox txtServerIp;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Timer tmrGetMess;
        private System.Windows.Forms.GroupBox grpClient;
        private System.Windows.Forms.GroupBox grpChatInfo;
        private System.Windows.Forms.GroupBox grpSend;
        private System.Windows.Forms.RichTextBox rtxChatInfo;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.RichTextBox rtxSendMessage;
    }
}

