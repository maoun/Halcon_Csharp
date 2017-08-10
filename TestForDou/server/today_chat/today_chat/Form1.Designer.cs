namespace today_chat
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
            this.btnBeginServer = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.tmrGetMess = new System.Windows.Forms.Timer(this.components);
            this.grpServer = new System.Windows.Forms.GroupBox();
            this.grpChatInfo = new System.Windows.Forms.GroupBox();
            this.rtxChatInfo = new System.Windows.Forms.RichTextBox();
            this.grpSend = new System.Windows.Forms.GroupBox();
            this.rtxSendMessage = new System.Windows.Forms.RichTextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpServer.SuspendLayout();
            this.grpChatInfo.SuspendLayout();
            this.grpSend.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBeginServer
            // 
            this.btnBeginServer.Location = new System.Drawing.Point(77, 13);
            this.btnBeginServer.Name = "btnBeginServer";
            this.btnBeginServer.Size = new System.Drawing.Size(75, 23);
            this.btnBeginServer.TabIndex = 0;
            this.btnBeginServer.Text = "启动服务器";
            this.btnBeginServer.UseVisualStyleBackColor = true;
            this.btnBeginServer.Click += new System.EventHandler(this.btnBeginServer_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(248, 75);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
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
            // grpServer
            // 
            this.grpServer.Controls.Add(this.btnBeginServer);
            this.grpServer.Location = new System.Drawing.Point(12, 0);
            this.grpServer.Name = "grpServer";
            this.grpServer.Size = new System.Drawing.Size(423, 42);
            this.grpServer.TabIndex = 5;
            this.grpServer.TabStop = false;
            this.grpServer.Text = "服务器选项";
            this.grpServer.Enter += new System.EventHandler(this.grpServer_Enter);
            // 
            // grpChatInfo
            // 
            this.grpChatInfo.Controls.Add(this.rtxChatInfo);
            this.grpChatInfo.Location = new System.Drawing.Point(12, 48);
            this.grpChatInfo.Name = "grpChatInfo";
            this.grpChatInfo.Size = new System.Drawing.Size(423, 240);
            this.grpChatInfo.TabIndex = 6;
            this.grpChatInfo.TabStop = false;
            this.grpChatInfo.Text = "通讯信息";
            // 
            // rtxChatInfo
            // 
            this.rtxChatInfo.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.rtxChatInfo.ForeColor = System.Drawing.SystemColors.Window;
            this.rtxChatInfo.Location = new System.Drawing.Point(6, 14);
            this.rtxChatInfo.Name = "rtxChatInfo";
            this.rtxChatInfo.ReadOnly = true;
            this.rtxChatInfo.Size = new System.Drawing.Size(411, 220);
            this.rtxChatInfo.TabIndex = 2;
            this.rtxChatInfo.Text = "";
            // 
            // grpSend
            // 
            this.grpSend.Controls.Add(this.rtxSendMessage);
            this.grpSend.Controls.Add(this.btnClose);
            this.grpSend.Controls.Add(this.btnSend);
            this.grpSend.Location = new System.Drawing.Point(12, 294);
            this.grpSend.Name = "grpSend";
            this.grpSend.Size = new System.Drawing.Size(423, 109);
            this.grpSend.TabIndex = 7;
            this.grpSend.TabStop = false;
            this.grpSend.Text = "信息发送";
            // 
            // rtxSendMessage
            // 
            this.rtxSendMessage.ForeColor = System.Drawing.Color.Red;
            this.rtxSendMessage.Location = new System.Drawing.Point(3, 17);
            this.rtxSendMessage.Name = "rtxSendMessage";
            this.rtxSendMessage.Size = new System.Drawing.Size(410, 52);
            this.rtxSendMessage.TabIndex = 4;
            this.rtxSendMessage.Text = "";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(338, 75);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 415);
            this.Controls.Add(this.grpSend);
            this.Controls.Add(this.grpChatInfo);
            this.Controls.Add(this.grpServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "服务器端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.grpServer.ResumeLayout(false);
            this.grpChatInfo.ResumeLayout(false);
            this.grpSend.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBeginServer;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Timer tmrGetMess;
        private System.Windows.Forms.GroupBox grpServer;
        private System.Windows.Forms.GroupBox grpChatInfo;
        private System.Windows.Forms.GroupBox grpSend;
        private System.Windows.Forms.RichTextBox rtxChatInfo;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.RichTextBox rtxSendMessage;
    }
}

