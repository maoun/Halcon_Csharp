namespace 检测有无
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.btnstop = new System.Windows.Forms.Button();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.btnstart = new System.Windows.Forms.Button();
            this.btnmodel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
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
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(127, 20);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 31);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 31);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(20, 90);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(76, 31);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(127, 90);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 31);
            this.textBox2.TabIndex = 3;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(298, 92);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(76, 31);
            this.button5.TabIndex = 10;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(405, 92);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 31);
            this.textBox5.TabIndex = 9;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(298, 22);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(76, 31);
            this.button6.TabIndex = 8;
            this.button6.Text = "button6";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(405, 22);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 31);
            this.textBox6.TabIndex = 7;
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
            this.panel2.Controls.Add(this.lblTime);
            this.panel2.Controls.Add(this.btnmodel);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.btnstart);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.btnstop);
            this.panel2.Controls.Add(this.textBox2);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.txtTime);
            this.panel2.Controls.Add(this.textBox6);
            this.panel2.Controls.Add(this.button6);
            this.panel2.Controls.Add(this.txtNumber);
            this.panel2.Controls.Add(this.textBox5);
            this.panel2.Controls.Add(this.button5);
            this.panel2.Controls.Add(this.lblNumber);
            this.panel2.Location = new System.Drawing.Point(13, 630);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(797, 244);
            this.panel2.TabIndex = 21;
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
            // Form1
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
            this.Name = "Form1";
            this.Text = "Matching";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Button btnstop;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.TextBox txtNumber;
        private System.Windows.Forms.Button btnstart;
        private System.Windows.Forms.Button btnmodel;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblNumber;
    }
}

