using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SocketTool.Core;
using System.Diagnostics;

namespace SocketTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private TreeNode rootNode1;
        private TreeNode rootNode2;
        private int index1 = 1;
        private int index2 = 1;
        private int pageIndex = 0;

        private List<Form> pageList = new List<Form>();
        private List<SocketInfo> socketInfoList = new List<SocketInfo>();

        private string XMLFileName = "socketinfo.xml";

        private void Form1_Load(object sender, EventArgs e)
        {
            rootNode1 = new TreeNode("客户终端", 5, 5);
            this.deviceTree.Nodes.Add(rootNode1);
            rootNode2 = new TreeNode("服务器终端", 6, 6);
            this.deviceTree.Nodes.Add(rootNode2);
            
            try
            {
                //SocketInfo[] sis = MySerializer.DeSerialize(XMLFileName);
                socketInfoList = MySerializer.Deserialize<List<SocketInfo>>(XMLFileName);
                //SocketInfo[] sis = MySerializer.Deserialize<SocketInfo[]>(XMLFileName);

                foreach(SocketInfo si in socketInfoList)
                {
                    if(si.Type == "Server")
                    {
                        AddServerFormNode(si.Name, si);
                    }else{
                        AddClientFormNode(si.Name, si);
                    }
                }
            }
            catch (System.Exception ex)
            {
            	Debug.WriteLine(ex.Message);
            	Debug.WriteLine(ex.StackTrace);
            }
        }

        private void AddClientFormNode(string name, SocketInfo si)
        {
            TreeNode ch = rootNode1.Nodes.Add(name, name, 7, 7);
            index1++;
            ClientForm form2 = new ClientForm();
            if(si != null)
               form2.SocketInfo = si;
            TabPage tp = addPage(name, form2);
            tp.ImageIndex = 2;
            ch.Tag = tp;
            rootNode1.ExpandAll();
            deviceTree.SelectedNode = ch;
        }

        private void AddServerFormNode(string name, SocketInfo si)
        {
            TreeNode ch = rootNode2.Nodes.Add(name, name, 8, 8);
            index2++;

            ServerForm form2 = new ServerForm();
            if (si != null)
                form2.SocketInfo = si;
            TabPage tp = addPage(name, form2);
            ch.Tag = tp;
            tp.ImageIndex = 3;
            rootNode2.ExpandAll();
            deviceTree.SelectedNode = ch;
        }


        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem.Name == "tsbAddClient")
            {
                string name = "客户端" + index1;
                AddClientFormNode(name, null);
            }
            else if (e.ClickedItem.Name == "tsbAddServer")
            {
                string name = "服务器端" + index2;
                AddServerFormNode(name, null);
            }
            else if (e.ClickedItem.Name == "tsbAbout")
            {
                Process.Start("iexplore.exe", "www.ltmonitor.com");
            }
            else if (e.ClickedItem.Name == "tsbDelete")
            {
                TreeNode tn = this.deviceTree.SelectedNode;

                if (tn.Level < 1)
                {
                    return;
                }

                TabPage tp = (TabPage)tn.Tag;
                this.tabControl1.TabPages.Remove(tp);

                tn.Parent.Nodes.Remove(tn);
            }
        }

        private TabPage addPage(string pageText, Form form)
        {
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            TabPage Page = this.tabPage1;
            if (pageIndex == 0)
            {
                Page.Controls.Add(form);
            }
            else
            {                
                Page = new TabPage();
                Page.ImageIndex = 3;
                Page.Name = "Page" + pageIndex.ToString();
                Page.TabIndex = pageIndex;
                this.tabControl1.Controls.Add(Page);
                Page.Controls.Add(form);
                this.tabControl1.SelectedTab = Page;

            }
            Page.Text = pageText;
            pageIndex++;

            form.Show();
            pageList.Add(form);
            return Page;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SocketInfo si = new SocketInfo();
            //si.Name = "服务器端";
            //socketList.Add(si);
            socketInfoList = new List<SocketInfo>();
            foreach(TreeNode tn in this.deviceTree.Nodes)
            {
                foreach (TreeNode ctn in tn.Nodes)
                {
                    TabPage tp = (TabPage)ctn.Tag;
                    if(tp != null)
                        tp.Text = ctn.Text;
                    foreach (Form f in tp.Controls)
                    {
                        ISocketInfo isi = (ISocketInfo)f;
                        isi.SocketInfo.Name = tp.Text;
                        f.Close();
                        socketInfoList.Add(isi.SocketInfo);
                    }
                }
            }
            

            MySerializer.Serialize(socketInfoList, XMLFileName);
        }

        private void deviceTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 1)
            {
                TabPage tp = (TabPage)e.Node.Tag;

                this.tabControl1.SelectedTab = tp;
            }
        }

        private void tsbAddClient_Click(object sender, EventArgs e)
        {

        }
    }
}
