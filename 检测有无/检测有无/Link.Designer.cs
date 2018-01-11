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
            this.txtDNA = new System.Windows.Forms.TextBox();
            this.lblDNA = new System.Windows.Forms.Label();
            this.txtDA1 = new System.Windows.Forms.TextBox();
            this.lblDA1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtDA2 = new System.Windows.Forms.TextBox();
            this.lblDA2 = new System.Windows.Forms.Label();
            this.txtSA2 = new System.Windows.Forms.TextBox();
            this.lblSA2 = new System.Windows.Forms.Label();
            this.txtSA1 = new System.Windows.Forms.TextBox();
            this.lblSA1 = new System.Windows.Forms.Label();
            this.lblSNA = new System.Windows.Forms.Label();
            this.txtSNA = new System.Windows.Forms.TextBox();
            this.gbxIP.SuspendLayout();
            this.gbxPort.SuspendLayout();
            this.gbxLog.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.gbxLog.Location = new System.Drawing.Point(12, 263);
            this.gbxLog.Name = "gbxLog";
            this.gbxLog.Size = new System.Drawing.Size(651, 261);
            this.gbxLog.TabIndex = 7;
            this.gbxLog.TabStop = false;
            this.gbxLog.Text = "Log";
            // 
            // txtDNA
            // 
            this.txtDNA.Location = new System.Drawing.Point(137, 24);
            this.txtDNA.Name = "txtDNA";
            this.txtDNA.Size = new System.Drawing.Size(100, 31);
            this.txtDNA.TabIndex = 23;
            // 
            // lblDNA
            // 
            this.lblDNA.Font = new System.Drawing.Font("宋体", 9F);
            this.lblDNA.Location = new System.Drawing.Point(30, 24);
            this.lblDNA.Name = "lblDNA";
            this.lblDNA.Size = new System.Drawing.Size(76, 31);
            this.lblDNA.TabIndex = 24;
            this.lblDNA.Text = "DNA";
            this.lblDNA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDA1
            // 
            this.txtDA1.Location = new System.Drawing.Point(137, 65);
            this.txtDA1.Name = "txtDA1";
            this.txtDA1.Size = new System.Drawing.Size(100, 31);
            this.txtDA1.TabIndex = 25;
            // 
            // lblDA1
            // 
            this.lblDA1.Font = new System.Drawing.Font("宋体", 9F);
            this.lblDA1.Location = new System.Drawing.Point(30, 65);
            this.lblDA1.Name = "lblDA1";
            this.lblDA1.Size = new System.Drawing.Size(76, 31);
            this.lblDA1.TabIndex = 26;
            this.lblDA1.Text = "DA1";
            this.lblDA1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSA2);
            this.panel1.Controls.Add(this.lblSA2);
            this.panel1.Controls.Add(this.txtSA1);
            this.panel1.Controls.Add(this.lblSA1);
            this.panel1.Controls.Add(this.lblSNA);
            this.panel1.Controls.Add(this.txtSNA);
            this.panel1.Controls.Add(this.txtDA2);
            this.panel1.Controls.Add(this.lblDA2);
            this.panel1.Controls.Add(this.txtDA1);
            this.panel1.Controls.Add(this.lblDA1);
            this.panel1.Controls.Add(this.lblDNA);
            this.panel1.Controls.Add(this.txtDNA);
            this.panel1.Location = new System.Drawing.Point(14, 94);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(649, 163);
            this.panel1.TabIndex = 27;
            // 
            // txtDA2
            // 
            this.txtDA2.Location = new System.Drawing.Point(137, 106);
            this.txtDA2.Name = "txtDA2";
            this.txtDA2.Size = new System.Drawing.Size(100, 31);
            this.txtDA2.TabIndex = 27;
            // 
            // lblDA2
            // 
            this.lblDA2.Font = new System.Drawing.Font("宋体", 9F);
            this.lblDA2.Location = new System.Drawing.Point(30, 106);
            this.lblDA2.Name = "lblDA2";
            this.lblDA2.Size = new System.Drawing.Size(76, 31);
            this.lblDA2.TabIndex = 28;
            this.lblDA2.Text = "DA2";
            this.lblDA2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSA2
            // 
            this.txtSA2.Location = new System.Drawing.Point(440, 106);
            this.txtSA2.Name = "txtSA2";
            this.txtSA2.Size = new System.Drawing.Size(100, 31);
            this.txtSA2.TabIndex = 33;
            // 
            // lblSA2
            // 
            this.lblSA2.Font = new System.Drawing.Font("宋体", 9F);
            this.lblSA2.Location = new System.Drawing.Point(333, 106);
            this.lblSA2.Name = "lblSA2";
            this.lblSA2.Size = new System.Drawing.Size(76, 31);
            this.lblSA2.TabIndex = 34;
            this.lblSA2.Text = "SA2";
            this.lblSA2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSA1
            // 
            this.txtSA1.Location = new System.Drawing.Point(440, 65);
            this.txtSA1.Name = "txtSA1";
            this.txtSA1.Size = new System.Drawing.Size(100, 31);
            this.txtSA1.TabIndex = 31;
            // 
            // lblSA1
            // 
            this.lblSA1.Font = new System.Drawing.Font("宋体", 9F);
            this.lblSA1.Location = new System.Drawing.Point(333, 65);
            this.lblSA1.Name = "lblSA1";
            this.lblSA1.Size = new System.Drawing.Size(76, 31);
            this.lblSA1.TabIndex = 32;
            this.lblSA1.Text = "SA1";
            this.lblSA1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSNA
            // 
            this.lblSNA.Font = new System.Drawing.Font("宋体", 9F);
            this.lblSNA.Location = new System.Drawing.Point(333, 24);
            this.lblSNA.Name = "lblSNA";
            this.lblSNA.Size = new System.Drawing.Size(76, 31);
            this.lblSNA.TabIndex = 30;
            this.lblSNA.Text = "SNA";
            this.lblSNA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSNA
            // 
            this.txtSNA.Location = new System.Drawing.Point(440, 24);
            this.txtSNA.Name = "txtSNA";
            this.txtSNA.Size = new System.Drawing.Size(100, 31);
            this.txtSNA.TabIndex = 29;
            // 
            // Link
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(168F, 168F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(676, 536);
            this.Controls.Add(this.gbxLog);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.gbxPort);
            this.Controls.Add(this.gbxIP);
            this.Controls.Add(this.panel1);
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.TextBox txtDNA;
        private System.Windows.Forms.Label lblDNA;
        private System.Windows.Forms.TextBox txtDA1;
        private System.Windows.Forms.Label lblDA1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtSA2;
        private System.Windows.Forms.Label lblSA2;
        private System.Windows.Forms.TextBox txtSA1;
        private System.Windows.Forms.Label lblSA1;
        private System.Windows.Forms.Label lblSNA;
        private System.Windows.Forms.TextBox txtSNA;
        private System.Windows.Forms.TextBox txtDA2;
        private System.Windows.Forms.Label lblDA2;
    }
}