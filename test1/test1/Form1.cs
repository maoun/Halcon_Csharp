using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace test1
{
    public partial class Form1 : Form
    {


        ImageForm ImgPicture = new ImageForm();
         


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bindListCiew();
        }
     
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count>0)
            {
                try
                {
                    pictureBox1.Image = null;
                    string Image_path = this.listView1.FocusedItem.SubItems[9].Text;
                    var path = Image_path.Split(';');
                    pictureBox1.Image = Image.FromFile(@"\\192.168.0.250/web_ser/" + path[0]);
                }
                catch
                {
                    pictureBox1.Image = Image.FromFile(@"\\192.168.0.250/web_ser/error.JPG");
                }                
            }
        }
        private void bindListCiew()
        {
            this.listView1.Columns.Add("序号");
            this.listView1.Columns.Add("北京龙格技术部样机");
            this.listView1.Columns.Add("品牌");
            this.listView1.Columns.Add("来源库");
            this.listView1.Columns.Add("类别");
            this.listView1.Columns.Add("样机型号");
            this.listView1.Columns.Add("样机生产批号");
            this.listView1.Columns.Add("入库日期");
            this.listView1.Columns.Add("样品状态");
            this.listView1.Columns.Add("照片信息索引");
            this.listView1.View = System.Windows.Forms.View.Details;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            pictureBox1.Image = null;
            if (ImgPicture.Visible)
            {
                ImgPicture.Hide();
            }
            string pinpai = textBox1.Text;
            string laiyuanku = textBox2.Text;
            string leibie = textBox3.Text;
            string yangjixinghao = textBox4.Text;
            string shengchanpihao = textBox5.Text;
            string rukuriqi = textBox6.Text;
            string zhuangtai = textBox7.Text;
            
            MySqlConnection myconn = null;
            MySqlCommand mycom = null;
            //MySqlCommand mycom2 = null;
            //MySqlDataAdapter myrec = null;
            myconn = new MySqlConnection("Host =192.168.0.250;Database=bec_yangji_db;Username=root;Password=maounfeng");
            myconn.Open();
            //查询
            mycom = myconn.CreateCommand();
            try
            {
                string sql = string.Format("select * from bec_yangji_code where `品牌` like '%" + pinpai + "'and `来源库` like'%" + laiyuanku + "'and `类别` like'%" + leibie + "'and `样机型号` like'%" + yangjixinghao + "'and`样机生产批号`like'%" + shengchanpihao + "'and`入库日期`like '%" + rukuriqi + "%'and`样品状态`like'%" + zhuangtai + "';");
                mycom.CommandText = sql;
                mycom.CommandType = CommandType.Text;
                MySqlDataReader sdr = mycom.ExecuteReader();
                int i = 0;
                while (sdr.Read())
                {
                    listView1.Items.Add(sdr[0].ToString());
                    listView1.Items[i].SubItems.Add(sdr[1].ToString());
                    listView1.Items[i].SubItems.Add(sdr[2].ToString());
                    listView1.Items[i].SubItems.Add(sdr[3].ToString());
                    listView1.Items[i].SubItems.Add(sdr[4].ToString());
                    listView1.Items[i].SubItems.Add(sdr[5].ToString());
                    listView1.Items[i].SubItems.Add(sdr[6].ToString());
                    listView1.Items[i].SubItems.Add(sdr[7].ToString());
                    listView1.Items[i].SubItems.Add(sdr[8].ToString());
                    listView1.Items[i].SubItems.Add(sdr[9].ToString());
                    i++;
                }
                if (sdr[0]==null)
                {
                    MessageBox.Show("", "无数据");
                }
            }
            catch
            {
                MessageBox.Show("查询内容不存在", "错误");
            }
            myconn.Close();
            button1.Focus();
        }

        private int ImgNumber = 0;
        private void next_Click(object sender, EventArgs e)
        {
            ImgNumber += 1;
            if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
            {
                try
                {
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();//释放内存，防止溢出
                    }
                    string Image_path = this.listView1.FocusedItem.SubItems[9].Text;
                    //复数图片显示
                    var path = Image_path.Split(';');
                    //去除字符串数组中空字符串
                    List<string> list = new List<string>();
                    foreach (string s in path)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            list.Add(s);
                        }
                    }
                    path = list.ToArray();                    
                    //组装
                    int n = path.Length;
                    if (ImgNumber >= n)
                    {
                        ImgNumber = 0;
                    }
                    pictureBox1.Image = Image.FromFile(@"\\192.168.0.250/web_ser/" + path[ImgNumber]);
                    if (ImgPicture.Visible)
                    {
                        ImgPicture.pictureBox1.Image.Dispose();//释放内存，防止溢出
                        ImgPicture.pictureBox1.Image = this.pictureBox1.Image;
                    }                   
                }
                catch
                {
                    pictureBox1.Image = Image.FromFile(@"\\192.168.0.250/web_ser/error.JPG");
                }
            }
        }

        private void last_Click(object sender, EventArgs e)
        {
            ImgNumber -= 1;
            if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
            {
                try
                {
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();//释放内存，防止溢出
                    }
                    pictureBox1.Image = null;
                    string Image_path = this.listView1.FocusedItem.SubItems[9].Text;
                    //复数图片显示
                    var path = Image_path.Split(';');
                    //去除字符串数组中空字符串
                    List<string> list = new List<string>();
                    foreach (string s in path)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            list.Add(s);
                        }
                    }
                    path = list.ToArray();
                    //组装
                    int n = path.Length;
                    if (ImgNumber < 0)
                    {
                        ImgNumber = n - 1;
                    }
                    pictureBox1.Image = Image.FromFile(@"\\192.168.0.250/web_ser/" + path[ImgNumber]);                    
                    if (ImgPicture.Visible)
                    {
                        ImgPicture.pictureBox1.Image.Dispose();//释放内存，防止溢出
                        ImgPicture.pictureBox1.Image = this.pictureBox1.Image;
                    }     
                }
                catch
                {
                    pictureBox1.Image = Image.FromFile(@"\\192.168.0.250/web_ser/error.JPG");
                }
            }
        }

        public void pictureBox1_Click(object sender, EventArgs e)
        {
            ImgPicture.Size = new Size(500, 500);
            if (ImgPicture.Visible || pictureBox1.Image == null)
            {
                ImgPicture.Hide();
            }
            else
            {               
                ImgPicture.Show();
                ImgPicture.pictureBox1.Image = this.pictureBox1.Image;
            }              
        }
    }
}
