using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.UI
{
    public partial class UIDepartment : System.Web.UI.UserControl
    {
        #region 成员变量

        /// <summary>
        /// 定义委托
        /// </summary>
        /// <param name="depertId"></param>
        /// <param name="e"></param>
        public delegate void DepartmentSelectHandler(string depertId, string e);

        /// <summary>
        /// 声明委托事件
        /// </summary>
        public event DepartmentSelectHandler DepartmentSelectEvent;

        private SettingManager settingManager = new SettingManager();

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
                BuildTree(TreeDepartment.Root);
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            if (DepartmentSelectEvent != null)
            {
                DepartmentSelectEvent("123", "123");
            }
        }

        /// <summary>
        /// 重置方法
        /// </summary>
        public void Reset()
        {
            FieldDepartment.Text = "";
            DepaertMentName.Value = "";
            DepaertMentId.Value = "";
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
                return this.FieldDepartment.Width;
            }
            set
            {
                this.FieldDepartment.Width = value;
            }
        }

        /// <summary>
        /// Tree宽度
        /// </summary>
        public Unit ListWidth
        {
            get
            {
                return this.TreeDepartment.Width;
            }
            set
            {
                this.TreeDepartment.Width = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string DepertId
        {
            get
            {
                if (DepaertMentId.Value != null)
                {
                    return DepaertMentId.Value.ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                DepaertMentId.Value = value;
                DataTable dt = settingManager.GetDepartmentByWhere("1=1 and departid='" + value + "'");
                if (dt != null && dt.Rows.Count > 0)
                {
                    FieldDepartment.Text = dt.Rows[0][2].ToString();
                    DepaertMentName.Value = FieldDepartment.Text;
                }
                else
                {
                    FieldDepartment.Text = " ";
                    DepaertMentName.Value =" ";
                }
                
            }
        }
        /// <summary>
        ///
        /// </summary>
        public string FieldLabel
        {
            get
            {
                return FieldDepartment.FieldLabel;
            }
            set
            {
                FieldDepartment.FieldLabel = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string AnchorHorizontal
        {
            get
            {
                return FieldDepartment.AnchorHorizontal;
            }
            set
            {
                FieldDepartment.AnchorHorizontal = value;
            }
        }

        private string depertName = string.Empty;

        /// <summary>
        /// 选中的部门名称
        /// </summary>
        public string DepertName
        {
            get
            {
                if (DepaertMentName.Value != null)
                {
                    return DepaertMentName.Value.ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                FieldDepartment.Text = value;
                DepaertMentName.Value = value;
            }
        }

        #endregion 属性

        #region 私有方法

        #region 递归产生系统表树形菜单节点

        /// <summary>
        ///将部门信息绑定至tree
        /// </summary>
        /// <param name="nodes"></param>
        private void BuildTree(Ext.Net.TreeNodeCollection nodes)
        {
            try
            {
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }

                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Text = "组织机构";
                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;

                DataTable dt = Bll.Common.ChangColName(ToDataTable(GetRedisData.GetData("t_cfg_department").Select("", "class asc,departid asc")));
                // DataTable dt = settingManager.GetDepartmentByWhere("1=1 and systemid='00' and class<='3'");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Addree(dt, dt.Rows[0]["col2"].ToString(), root, null);
                    }
                }
            }
            catch
            {
            }
        }

        public DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        /// 遍历将子部门挂接至父部门
        /// </summary>
        /// <param name="allNodeTable"></param>
        /// <param name="parentColValue"></param>
        /// <param name="root"></param>
        /// <param name="ParentNode"></param>
        private void Addree(DataTable allNodeTable, string parentColValue, Ext.Net.TreeNode root, Ext.Net.TreeNode ParentNode)
        {
            DataRow[] myDataRows = allNodeTable.Select("col2 ='" + parentColValue + "'");
            foreach (DataRow myDataRow in myDataRows)
            {
                Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                node.Text = myDataRow[1].ToString();
                node.NodeID = myDataRow[0].ToString();

                node.Leaf = true;
                node.Draggable = true;
                node.Expandable = ThreeStateBool.True;
                node.Expanded = true;
                node.Icon = Icon.House;
                if (ParentNode != null)
                {
                    ParentNode.Nodes.Add(node);
                    Addree(allNodeTable, myDataRow["col0"].ToString(), ParentNode, node);
                }
                else
                {
                    root.Nodes.Add(node);
                    Addree(allNodeTable, myDataRow["col0"].ToString(), ParentNode, node);
                }
            }
        }

        #endregion 递归产生系统表树形菜单节点

        #endregion 私有方法
    }
}