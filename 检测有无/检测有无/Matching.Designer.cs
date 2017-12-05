namespace 检测有无
{
    partial class Matching
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnLink = new System.Windows.Forms.Button();
            this.txtTrans = new System.Windows.Forms.TextBox();
            this.txtSet = new System.Windows.Forms.TextBox();
            this.btnstop = new System.Windows.Forms.Button();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.btnstart = new System.Windows.Forms.Button();
            this.btnmodel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTrans = new System.Windows.Forms.Label();
            this.lblSet = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblNumber = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.hWindowControl1);
            this.panel1.Location = new System.Drawing.Point(13, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 600);
            this.panel1.TabIndex = 0;
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(-3, 0);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(800, 600);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(800, 600);
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(127, 24);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(155, 31);
            this.txtIP.TabIndex = 1;
            this.txtIP.Text = "192.168.1.1";
            // 
            // btnLink
            // 
            this.btnLink.Location = new System.Drawing.Point(20, 24);
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(76, 31);
            this.btnLink.TabIndex = 2;
            this.btnLink.Text = "Link";
            this.btnLink.UseVisualStyleBackColor = true;
            this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
            // 
            // txtTrans
            // 
            this.txtTrans.Location = new System.Drawing.Point(127, 94);
            this.txtTrans.Name = "txtTrans";
            this.txtTrans.Size = new System.Drawing.Size(378, 31);
            this.txtTrans.TabIndex = 3;
            // 
            // txtSet
            // 
            this.txtSet.Location = new System.Drawing.Point(405, 22);
            this.txtSet.Name = "txtSet";
            this.txtSet.Size = new System.Drawing.Size(100, 31);
            this.txtSet.TabIndex = 7;
            // 
            // btnstop
            // 
            this.btnstop.Location = new System.Drawing.Point(567, 164);
            this.btnstop.Name = "btnstop";
            this.btnstop.Size = new System.Drawing.Size(207, 61);
            this.btnstop.TabIndex = 18;
            this.btnstop.Text = "Stop";
            this.btnstop.UseVisualStyleBackColor = true;
            this.btnstop.Click += new System.EventHandler(this.btnstop_Click);
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(674, 94);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(100, 31);
            this.txtTime.TabIndex = 15;
            // 
            // txtNumber
            // 
            this.txtNumber.Location = new System.Drawing.Point(674, 24);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(100, 31);
            this.txtNumber.TabIndex = 13;
            // 
            // btnstart
            // 
            this.btnstart.Location = new System.Drawing.Point(298, 164);
            this.btnstart.Name = "btnstart";
            this.btnstart.Size = new System.Drawing.Size(207, 61);
            this.btnstart.TabIndex = 19;
            this.btnstart.Text = "Start";
            this.btnstart.UseVisualStyleBackColor = true;
            this.btnstart.Click += new System.EventHandler(this.btnstart_Click);
            // 
            // btnmodel
            // 
            this.btnmodel.Location = new System.Drawing.Point(20, 164);
            this.btnmodel.Name = "btnmodel";
            this.btnmodel.Size = new System.Drawing.Size(207, 61);
            this.btnmodel.TabIndex = 20;
            this.btnmodel.Text = "CreatModel";
            this.btnmodel.UseVisualStyleBackColor = true;
            this.btnmodel.Click += new System.EventHandler(this.btnmodel_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblTrans);
            this.panel2.Controls.Add(this.lblSet);
            this.panel2.Controls.Add(this.lblTime);
            this.panel2.Controls.Add(this.btnmodel);
            this.panel2.Controls.Add(this.txtIP);
            this.panel2.Controls.Add(this.btnstart);
            this.panel2.Controls.Add(this.btnLink);
            this.panel2.Controls.Add(this.btnstop);
            this.panel2.Controls.Add(this.txtTrans);
            this.panel2.Controls.Add(this.txtTime);
            this.panel2.Controls.Add(this.txtSet);
            this.panel2.Controls.Add(this.txtNumber);
            this.panel2.Controls.Add(this.lblNumber);
            this.panel2.Location = new System.Drawing.Point(13, 630);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(797, 244);
            this.panel2.TabIndex = 21;
            // 
            // lblTrans
            // 
            this.lblTrans.Font = new System.Drawing.Font("宋体", 9F);
            this.lblTrans.Location = new System.Drawing.Point(20, 94);
            this.lblTrans.Name = "lblTrans";
            this.lblTrans.Size = new System.Drawing.Size(76, 31);
            this.lblTrans.TabIndex = 24;
            this.lblTrans.Text = "Trans";
            this.lblTrans.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSet
            // 
            this.lblSet.Font = new System.Drawing.Font("宋体", 9F);
            this.lblSet.Location = new System.Drawing.Point(305, 24);
            this.lblSet.Name = "lblSet";
            this.lblSet.Size = new System.Drawing.Size(76, 31);
            this.lblSet.TabIndex = 23;
            this.lblSet.Text = "Set";
            this.lblSet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTime
            // 
            this.lblTime.Font = new System.Drawing.Font("宋体", 9F);
            this.lblTime.Location = new System.Drawing.Point(567, 94);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(76, 31);
            this.lblTime.TabIndex = 21;
            this.lblTime.Text = "Time";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNumber
            // 
            this.lblNumber.Font = new System.Drawing.Font("宋体", 9F);
            this.lblNumber.Location = new System.Drawing.Point(567, 24);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(76, 31);
            this.lblNumber.TabIndex = 22;
            this.lblNumber.Text = "Number";
            this.lblNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Matching
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(168F, 168F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(826, 886);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Matching";
            this.Text = "Matching";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnLink;
        public System.Windows.Forms.TextBox txtTrans;
        private System.Windows.Forms.TextBox txtSet;
        private System.Windows.Forms.Button btnstop;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.TextBox txtNumber;
        private System.Windows.Forms.Button btnstart;
        private System.Windows.Forms.Button btnmodel;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblNumber;
        private System.Windows.Forms.Label lblSet;
        private System.Windows.Forms.Label lblTrans;
        public System.Windows.Forms.TextBox txtIP;
    }
}

