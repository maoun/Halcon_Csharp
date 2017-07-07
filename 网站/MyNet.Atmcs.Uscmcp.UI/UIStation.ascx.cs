using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.UI
{
    public partial class UIStation : System.Web.UI.UserControl
    {
        #region 成员变量

        private SettingManager settingManager = new SettingManager();
        private Bll.ServiceManager servicemansger = new Bll.ServiceManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();

        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                BuildTree(TreeStation.Root);
            }
        }

        #endregion 控件事件

        #region 属性

        /// <summary>
        /// 显示框宽度
        /// </summary>
        public Unit Width
        {
            get
            {
                return this.FieldStation.Width;
            }
            set
            {
                this.FieldStation.Width = value;
            }
        }

        /// <summary>
        /// Tree宽度
        /// </summary>
        public Unit ListWidth
        {
            get
            {
                return this.TreeStation.Width;
            }
            set
            {
                this.TreeStation.Width = value;
            }
        }

        #endregion 属性

        #region 私有方法

        /// <summary>
        /// 组件人员列表树
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private Ext.Net.TreeNodeCollection BuildTree(Ext.Net.TreeNodeCollection nodes)
        {
            try
            {
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }

                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Text = "卡口列表";
                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;

                // 添加 自己机构节点 和卡口
                UserInfo user = Session["userinfo"] as UserInfo;
                if (user == null)
                {
                    user = new UserInfo();
                    user.DepartName = "滨州市交通警察支队";
                    user.DeptCode = "371600000000";
                }

                Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                nodeRoot.Text = user.DepartName;
                nodeRoot.Leaf = true;
                nodeRoot.NodeID = user.DeptCode;
                nodeRoot.Icon = Icon.House;

                DataTable dtStation = tgsPproperty.GetStationInfo(" departid='" + user.DeptCode + "' ");
                AddStationTree(nodeRoot, dtStation);
                nodeRoot.Expanded = false;
                nodeRoot.Draggable = true;
                nodeRoot.Expandable = ThreeStateBool.True;
                root.Nodes.Add(nodeRoot);

                //绑定下级部门及下级部门卡口
                AddDepartTree(root, user.DeptCode);

                return nodes;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///绑定下级部门及下级部门人员
        /// </summary>
        /// <param name="root"></param>
        private void AddDepartTree(Ext.Net.TreeNode root, string departCode)
        {
            try
            {
                DataTable dtDepart = settingManager.GetLowerDepartment(departCode);

                if (dtDepart != null && dtDepart.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDepart.Rows.Count; i++)
                    {
                        Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                        nodeRoot.Text = dtDepart.Rows[i][2].ToString();
                        nodeRoot.Leaf = true;
                        nodeRoot.NodeID = dtDepart.Rows[i][1].ToString();
                        nodeRoot.Icon = Icon.House;
                        DataTable dtStation = tgsPproperty.GetStationInfo(" departid='" + nodeRoot.NodeID + "' ");
                        AddStationTree(nodeRoot, dtStation);
                        nodeRoot.Expanded = false;
                        nodeRoot.Draggable = true;
                        nodeRoot.Expandable = ThreeStateBool.True;
                        AddDepartTree(nodeRoot, dtDepart.Rows[i][1].ToString());
                        root.Nodes.Add(nodeRoot);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="root"></param>
        private void AddStationTree(Ext.Net.TreeNode DepartNode, DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                        node.Text = dt.Rows[i]["col2"].ToString();
                        node.Leaf = true;
                        node.Checked = ThreeStateBool.False;
                        node.NodeID = dt.Rows[i]["col1"].ToString();
                        node.Draggable = false;
                        node.AllowDrag = false;
                        DepartNode.Nodes.Add(node);
                    }
                }
            }
            catch
            {
            }
        }

        #endregion 私有方法
    }
}