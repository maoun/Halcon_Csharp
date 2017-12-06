namespace 检测有无
{
    partial class Link
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
            this.txtServerIp = new System.Windows.Forms.TextBox();
            this.gbxIP = new System.Windows.Forms.GroupBox();
            this.gbxPort = new System.Windows.Forms.GroupBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.gbxLog = new System.Windows.Forms.GroupBox();
            this.gbxIP.SuspendLayout();
            this.gbxPort.SuspendLayout();
            this.gbxLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtServerIp
            // 
            this.txtServerIp.Location = new System.Drawing.Point(6, 30);
            this.txtServerIp.Name = "txtServerIp";
            this.txtServerIp.Size = new System.Drawing.Size(233, 31);
            this.txtServerIp.TabIndex = 0;
            // 
            // gbxIP
            // 
            this.gbxIP.Controls.Add(this.txtServerIp);
            this.gbxIP.Location = new System.Drawing.Point(12, 12);
            this.gbxIP.Name = "gbxIP";
            this.gbxIP.Size = new System.Drawing.Size(245, 76);
            this.gbxIP.TabIndex = 1;
            this.gbxIP.TabStop = false;
            this.gbxIP.Text = "IP";
            // 
            // gbxPort
            // 
            this.gbxPort.Controls.Add(this.txtPort);
            this.gbxPort.Location = new System.Drawing.Point(263, 12);
            this.gbxPort.Name = "gbxPort";
            this.gbxPort.Size = new System.Drawing.Size(200, 76);
            this.gbxPort.TabIndex = 2;
            this.gbxPort.TabStop = false;
            this.gbxPort.Text = "Port";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(6, 30);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(188, 31);
            this.txtPort.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(579, 40);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(92, 31);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(473, 40);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(101, 31);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(6, 30);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(639, 225);
            this.txtLog.TabIndex = 6;
            this.txtLog.TextChanged += new System.EventHandler(this.txtLog_TextChanged);
            // 
            // gbxLog
            // 
            this.gbxLog.Controls.Add(this.txtLog);
            this.gbxLog.Location = new System.Drawing.Point(13, 95);
            this.gbxLog.Name = "gbxLog";
            this.gbxLog.Size = new System.Drawing.Size(651, 261);
            this.gbxLog.TabIndex = 7;
            this.gbxLog.TabStop = false;
            this.gbxLog.Text = "Log";
            // 
            // Link
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(168F, 168F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(676, 368);
            this.Controls.Add(this.gbxLog);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.gbxPort);
            this.Controls.Add(this.gbxIP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Link";
            this.Text = "Link";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Link_FormClosing);
            this.Load += new System.EventHandler(this.Link_Load);
            this.gbxIP.ResumeLayout(false);
            this.gbxIP.PerformLayout();
            this.gbxPort.ResumeLayout(false);
            this.gbxPort.PerformLayout();
            this.gbxLog.ResumeLayout(false);
            this.gbxLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxIP;
        private System.Windows.Forms.GroupBox gbxPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox gbxLog;
        public System.Windows.Forms.TextBox txtServerIp;
    }
}