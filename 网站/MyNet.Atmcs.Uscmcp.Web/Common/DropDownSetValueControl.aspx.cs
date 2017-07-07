/*****************
 * 初始化时 需要赋值 ，才能使用正常功能；
 * 页面赋值 在  <ext:DropDownField中     UnderlyingValue="Item22,Item23"            Text="[Milk,Cereal]"
 * 后台赋值  this.FieldPerson.SetValue("000000000001","admin",false);
 * ***************/

using System;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class DropDownSetValueControl : System.Web.UI.Page
    {

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        private SettingManager settingManager = new SettingManager();
        private Bll.ServiceManager servicemansger = new Bll.ServiceManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!X.IsAjaxRequest)
                {
                    BuildTree(TreeStation.Root);
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：用户登录", userinfo.NowIp, "0");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DropDownSetValueControl.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
            }
        }

        public void btnValue_Event(object sender, DirectEventArgs e)
        {
            try
            {
                string value = FieldStation.Value.ToString();
                string text = FieldStation.Text.ToString();
                Notice("", value + "   " + text);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DropDownSetValueControl.aspx-btnValue_Event", ex.Message + "；" + ex.StackTrace, "btnValue_Event发生异常");
            }
        }

        /// <summary>
        /// 提示窗体
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Notice(string title, string msg)
        {
            try
            {
                X.Msg.Alert(title, msg).Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DropDownSetValueControl.aspx-Notice", ex.Message+"；"+ex.StackTrace, "Notice has an exception");
            }
        }

        /// <summary>
        /// 组件卡口列表树
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
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DropDownSetValueControl.aspx-BuildTree", ex.Message+"；"+ex.StackTrace, "BuildTree has an exception");
                return null;
            }
        }

        /// <summary>
        ///绑定下级部门及下级部门卡口
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
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DropDownSetValueControl.aspx-AddDepartTree", ex.Message+"；"+ex.StackTrace, "AddDepartTree has an exception");
            }
        }

        /// <summary>
        /// 添加卡口子节点
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
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DropDownSetValueControl.aspx-AddStationTree", ex.Message+"；"+ex.StackTrace, "AddStationTree has an exception");
            }
        }

        protected void Unnamed_Event()
        {
        }
    }
}