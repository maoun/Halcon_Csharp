namespace develop_calibrate_camera.calibrate_camera
{
    partial class Calibrate
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
            this.startcalibrate = new System.Windows.Forms.Button();
            this.chosefile = new System.Windows.Forms.Button();
            this.txtFiles = new System.Windows.Forms.TextBox();
            this.groupFiles = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.groupImage = new System.Windows.Forms.GroupBox();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupCamPar = new System.Windows.Forms.GroupBox();
            this.txtIy = new System.Windows.Forms.TextBox();
            this.txtIx = new System.Windows.Forms.TextBox();
            this.txtCy = new System.Windows.Forms.TextBox();
            this.txtCx = new System.Windows.Forms.TextBox();
            this.txtSy = new System.Windows.Forms.TextBox();
            this.txtSx = new System.Windows.Forms.TextBox();
            this.txtKappa = new System.Windows.Forms.TextBox();
            this.txtFocus = new System.Windows.Forms.TextBox();
            this.txtCalibtate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupFiles.SuspendLayout();
            this.groupImage.SuspendLayout();
            this.groupCamPar.SuspendLayout();
            this.SuspendLayout();
            // 
            // startcalibrate
            // 
            this.startcalibrate.Location = new System.Drawing.Point(720, 27);
            this.startcalibrate.Name = "startcalibrate";
            this.startcalibrate.Size = new System.Drawing.Size(113, 37);
            this.startcalibrate.TabIndex = 0;
            this.startcalibrate.Text = "开始标定";
            this.startcalibrate.UseVisualStyleBackColor = true;
            this.startcalibrate.Click += new System.EventHandler(this.startcalibrate_Click);
            // 
            // chosefile
            // 
            this.chosefile.Location = new System.Drawing.Point(720, 70);
            this.chosefile.Name = "chosefile";
            this.chosefile.Size = new System.Drawing.Size(113, 37);
            this.chosefile.TabIndex = 1;
            this.chosefile.Text = "选定路径";
            this.chosefile.UseVisualStyleBackColor = true;
            this.chosefile.Click += new System.EventHandler(this.chosefile_Click);
            // 
            // txtFiles
            // 
            this.txtFiles.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFiles.Location = new System.Drawing.Point(213, 70);
            this.txtFiles.Multiline = true;
            this.txtFiles.Name = "txtFiles";
            this.txtFiles.Size = new System.Drawing.Size(479, 37);
            this.txtFiles.TabIndex = 3;
            this.txtFiles.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // groupFiles
            // 
            this.groupFiles.Controls.Add(this.txtFiles);
            this.groupFiles.Controls.Add(this.startcalibrate);
            this.groupFiles.Controls.Add(this.chosefile);
            this.groupFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupFiles.Location = new System.Drawing.Point(0, 564);
            this.groupFiles.Name = "groupFiles";
            this.groupFiles.Size = new System.Drawing.Size(845, 123);
            this.groupFiles.TabIndex = 4;
            this.groupFiles.TabStop = false;
            this.groupFiles.Text = "选择图像路径";
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtLog.Location = new System.Drawing.Point(431, 27);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(411, 327);
            this.txtLog.TabIndex = 6;
            // 
            // groupImage
            // 
            this.groupImage.Controls.Add(this.hWindowControl1);
            this.groupImage.Controls.Add(this.txtLog);
            this.groupImage.Location = new System.Drawing.Point(0, 0);
            this.groupImage.Name = "groupImage";
            this.groupImage.Size = new System.Drawing.Size(845, 357);
            this.groupImage.TabIndex = 7;
            this.groupImage.TabStop = false;
            this.groupImage.Text = "标定图像";
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(3, 27);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(422, 327);
            this.hWindowControl1.TabIndex = 7;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(422, 327);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(317, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 21);
            this.label5.TabIndex = 4;
            this.label5.Text = "中心点x坐标";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(317, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 21);
            this.label6.TabIndex = 5;
            this.label6.Text = "中心点y坐标";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(317, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(170, 21);
            this.label7.TabIndex = 6;
            this.label7.Text = "图像宽（pixel）";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(317, 147);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(170, 21);
            this.label8.TabIndex = 7;
            this.label8.Text = "图像高（pixel）";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "焦距（m）";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "畸变系数";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "象元宽度（um）";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(158, 21);
            this.label4.TabIndex = 3;
            this.label4.Text = "象元高度（um）";
            // 
            // groupCamPar
            // 
            this.groupCamPar.Controls.Add(this.txtIy);
            this.groupCamPar.Controls.Add(this.txtIx);
            this.groupCamPar.Controls.Add(this.txtCy);
            this.groupCamPar.Controls.Add(this.txtCx);
            this.groupCamPar.Controls.Add(this.txtSy);
            this.groupCamPar.Controls.Add(this.txtSx);
            this.groupCamPar.Controls.Add(this.txtKappa);
            this.groupCamPar.Controls.Add(this.txtFocus);
            this.groupCamPar.Controls.Add(this.txtCalibtate);
            this.groupCamPar.Controls.Add(this.label9);
            this.groupCamPar.Controls.Add(this.label6);
            this.groupCamPar.Controls.Add(this.label4);
            this.groupCamPar.Controls.Add(this.label5);
            this.groupCamPar.Controls.Add(this.label8);
            this.groupCamPar.Controls.Add(this.label7);
            this.groupCamPar.Controls.Add(this.label3);
            this.groupCamPar.Controls.Add(this.label1);
            this.groupCamPar.Controls.Add(this.label2);
            this.groupCamPar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupCamPar.Location = new System.Drawing.Point(0, 353);
            this.groupCamPar.Name = "groupCamPar";
            this.groupCamPar.Size = new System.Drawing.Size(845, 211);
            this.groupCamPar.TabIndex = 8;
            this.groupCamPar.TabStop = false;
            this.groupCamPar.Text = "相机参数";
            // 
            // txtIy
            // 
            this.txtIy.Font = new System.Drawing.Font("宋体", 8F);
            this.txtIy.Location = new System.Drawing.Point(482, 144);
            this.txtIy.Multiline = true;
            this.txtIy.Name = "txtIy";
            this.txtIy.Size = new System.Drawing.Size(100, 21);
            this.txtIy.TabIndex = 17;
            // 
            // txtIx
            // 
            this.txtIx.Font = new System.Drawing.Font("宋体", 8F);
            this.txtIx.Location = new System.Drawing.Point(482, 109);
            this.txtIx.Multiline = true;
            this.txtIx.Name = "txtIx";
            this.txtIx.Size = new System.Drawing.Size(100, 21);
            this.txtIx.TabIndex = 16;
            // 
            // txtCy
            // 
            this.txtCy.Font = new System.Drawing.Font("宋体", 8F);
            this.txtCy.Location = new System.Drawing.Point(482, 76);
            this.txtCy.Multiline = true;
            this.txtCy.Name = "txtCy";
            this.txtCy.Size = new System.Drawing.Size(100, 21);
            this.txtCy.TabIndex = 15;
            // 
            // txtCx
            // 
            this.txtCx.Font = new System.Drawing.Font("宋体", 8F);
            this.txtCx.Location = new System.Drawing.Point(482, 42);
            this.txtCx.Multiline = true;
            this.txtCx.Name = "txtCx";
            this.txtCx.Size = new System.Drawing.Size(100, 21);
            this.txtCx.TabIndex = 14;
            // 
            // txtSy
            // 
            this.txtSy.Font = new System.Drawing.Font("宋体", 8F);
            this.txtSy.Location = new System.Drawing.Point(209, 147);
            this.txtSy.Multiline = true;
            this.txtSy.Name = "txtSy";
            this.txtSy.Size = new System.Drawing.Size(100, 21);
            this.txtSy.TabIndex = 13;
            // 
            // txtSx
            // 
            this.txtSx.Font = new System.Drawing.Font("宋体", 8F);
            this.txtSx.Location = new System.Drawing.Point(209, 112);
            this.txtSx.Multiline = true;
            this.txtSx.Name = "txtSx";
            this.txtSx.Size = new System.Drawing.Size(100, 21);
            this.txtSx.TabIndex = 12;
            // 
            // txtKappa
            // 
            this.txtKappa.Font = new System.Drawing.Font("宋体", 8F);
            this.txtKappa.Location = new System.Drawing.Point(209, 76);
            this.txtKappa.Multiline = true;
            this.txtKappa.Name = "txtKappa";
            this.txtKappa.Size = new System.Drawing.Size(100, 21);
            this.txtKappa.TabIndex = 11;
            // 
            // txtFocus
            // 
            this.txtFocus.Font = new System.Drawing.Font("宋体", 8F);
            this.txtFocus.Location = new System.Drawing.Point(209, 39);
            this.txtFocus.Multiline = true;
            this.txtFocus.Name = "txtFocus";
            this.txtFocus.Size = new System.Drawing.Size(100, 21);
            this.txtFocus.TabIndex = 10;
            // 
            // txtCalibtate
            // 
            this.txtCalibtate.Font = new System.Drawing.Font("宋体", 8F);
            this.txtCalibtate.Location = new System.Drawing.Point(714, 42);
            this.txtCalibtate.Multiline = true;
            this.txtCalibtate.Name = "txtCalibtate";
            this.txtCalibtate.Size = new System.Drawing.Size(100, 21);
            this.txtCalibtate.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(592, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 21);
            this.label9.TabIndex = 8;
            this.label9.Text = "标定板规格";
            // 
            // Calibrate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 687);
            this.Controls.Add(this.groupCamPar);
            this.Controls.Add(this.groupFiles);
            this.Controls.Add(this.groupImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Calibrate";
            this.Text = "calibrate";
            this.Load += new System.EventHandler(this.Calibrate_Load);
            this.groupFiles.ResumeLayout(false);
            this.groupFiles.PerformLayout();
            this.groupImage.ResumeLayout(false);
            this.groupImage.PerformLayout();
            this.groupCamPar.ResumeLayout(false);
            this.groupCamPar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button startcalibrate;
        private System.Windows.Forms.Button chosefile;
        private System.Windows.Forms.TextBox txtFiles;
        private System.Windows.Forms.GroupBox groupFiles;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox groupImage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupCamPar;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtIy;
        private System.Windows.Forms.TextBox txtIx;
        private System.Windows.Forms.TextBox txtCy;
        private System.Windows.Forms.TextBox txtCx;
        private System.Windows.Forms.TextBox txtSy;
        private System.Windows.Forms.TextBox txtSx;
        private System.Windows.Forms.TextBox txtKappa;
        private System.Windows.Forms.TextBox txtFocus;
        private System.Windows.Forms.TextBox txtCalibtate;
    }
}