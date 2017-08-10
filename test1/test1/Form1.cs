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
                    pictureBox1.Image = Image.FromFile(@Image_path);
                }
                catch
                {
                    MessageBox.Show("没有图片","提示");
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
            string pinpai = textBox1.Text;
            string laiyuanku = textBox2.Text;
            string leibie = textBox3.Text;
            string yangjixinghao = textBox4.Text;
            string shengchanpihao = textBox5.Text;
            string rukuriqi = textBox6.Text;
            string zhuangtai = textBox7.Text;
            
            MySqlConnection myconn = null;
            MySqlCommand mycom = null;
            MySqlCommand mycom2 = null;
            MySqlDataAdapter myrec = null;
            myconn = new MySqlConnection("Host =192.168.0.250;Database=bec_yangji_db;Username=root;Password=maounfeng");
            myconn.Open();
            //查询
            mycom = myconn.CreateCommand();
            try
            {
                string sql = string.Format("select * from bec_yangji_code where `品牌` like '%"+ pinpai +"'and `来源库` like'%"+laiyuanku+"'and `类别` like'%"+leibie+"' ;");
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
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
