using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test1
{
    public partial class ImageForm : Form
    {
        public ImageForm()
        {
            InitializeComponent();
        }

        private void ImageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();            
        }

        //private void ImageForm_Load(object sender, EventArgs e)
        //{
        //    Form1 f1 = new Form1();
        //    pictureBox1.Image = f1.pictureBox1.Image;

        //}

    }
}
