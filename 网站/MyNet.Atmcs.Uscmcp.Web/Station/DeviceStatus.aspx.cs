using System;
using System.Collections;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class DeviceStatus : System.Web.UI.Page
    {
        #region 成员变量

        private SettingManager settingManager = new SettingManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private MyNet.Atmcs.Uscmcp.Bll.DeviceManager deviceManager = new MyNet.Atmcs.Uscmcp.Bll.DeviceManager();
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("DeviceStatus1", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!IsPostBack)
            {
                if (!X.IsAjaxRequest)
                {
                    if (QueryData.Value == null)
                    {
                        QueryData.Value = "1=1";
                    }
                    BuildTree(TreePanel1.Root);
                    Button2.Disabled = true;
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, GetLangStr("DeviceStatus2", "访问：设备状态列表"), userinfo.NowIp, "0");
                    //this.DataBind();
                }
            }
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            QueryData.Value = GetWhere();
            GetData();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            //GridDataBind("rownum <100");
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            CmbState.Reset();
            QueryData.Value = "1=1";

            this.ResourceManager1.RegisterAfterClientInitScript("clearTime();");
        }

        protected void RefreshTimes(object sender, DirectEventArgs e)
        {
            GetData();
        }

        protected void RefreshTimeClick(object sender, DirectEventArgs e)
        {
            RefreshTimes(null, null);
        }

        /// <summary>
        /// 导出为xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToXml(object sender, EventArgs e)
        {
            DataTable dt = ChangeDataTable();
            ConvertData.ExportXml(dt, this);
        }

        /// <summary>
        /// 导出为excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToExcel(object sender, EventArgs e)
        {
            DataTable dt = ChangeDataTable();
            ConvertData.ExportExcel(dt, this);
        }

        /// <summary>
        /// 导出为csv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToCsv(object sender, EventArgs e)
        {
            DataTable dt = ChangeDataTable();
            ConvertData.ExportCsv(dt, this);
        }

        #endregion 控件事件

        #region DirectMethod 方法

        /// <summary>
        /// 设备点击事件
        /// </summary>
        /// <param name="code"></param>
        [DirectMethod]
        public void SelectNode(string code)
        {
            HidStationType.Value = code;
            HidQueryFlag.Value = "1";
            GetData();
        }

        [DirectMethod]
        public void SelectNodeByDevice()
        {
            HidQueryFlag.Value = "2";
            GetData();
        }

        [DirectMethod]
        public void SelectNodeByService()
        {
            HidQueryFlag.Value = "3";
            GetData();
        }

        #endregion DirectMethod 方法

        #region 私有方法

        /// <summary>
        /// 创建设备树
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public Ext.Net.TreeNodeCollection BuildTree(Ext.Net.TreeNodeCollection nodes)
        {
            if (nodes == null)
            {
                nodes = new Ext.Net.TreeNodeCollection();
            }

            Ext.Net.TreeNode root = new Ext.Net.TreeNode();
            root.Expanded = true;
            root.Text = GetLangStr("DeviceStatus28", "设备列表");

            nodes.Add(root);

            Ext.Net.TreeNode node1 = new Ext.Net.TreeNode();
            node1.Expanded = true;
            node1.Text = GetLangStr("DeviceStatus29", "已关联设备监测点列表");
            root.Nodes.Add(node1);

            Ext.Net.TreeNode node2 = new Ext.Net.TreeNode();
            node2.Expanded = true;
            node2.Text = GetLangStr("DeviceStatus30", "未关联设备监测点列表");
            root.Nodes.Add(node2);

            Ext.Net.TreeNode node3 = new Ext.Net.TreeNode();
            node3.Expanded = true;
            node3.Text = GetLangStr("DeviceStatus31", "其它设备列表");
            node3.Listeners.Click.Handler = "DeviceStatus.SelectNodeByDevice();";
            HidQueryFlag.Value = "2";
            node3.Icon = Icon.Drive;
            node2.Nodes.Add(node3);

            Ext.Net.TreeNode node4 = new Ext.Net.TreeNode();
            node4.Expanded = true;
            node4.Listeners.Click.Handler = "DeviceStatus.SelectNodeByService();";
            node4.Icon = Icon.Monitor;
            node4.Text = GetLangStr("DeviceStatus32", "服务器列表");
            HidQueryFlag.Value = "3";
            node2.Nodes.Add(node4);

            DataTable dt = deviceManager.GetStationTypeByDevice();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    string code = dt.Rows[i]["col0"].ToString();
                    string countvalue = dt.Rows[i]["col2"].ToString();
                    node.Text = dt.Rows[i]["col1"].ToString() + "(" + countvalue + ")";
                    node.Listeners.Click.Handler = "DeviceStatus.SelectNode('" + code + "') ;";
                    node.Icon = Icon.House;
                    node.NodeID = dt.Rows[i]["col0"].ToString();
                    node.Expanded = true;
                    if (i == 0)
                    {
                        HidStationType.Value = code;
                        HidQueryFlag.Value = "1";
                    }
                    node1.Nodes.Add(node);
                }
            }
            return nodes;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        private void GetData()
        {
            if (HidQueryFlag.Value != null)
            {
                if (!string.IsNullOrEmpty(HidQueryFlag.Value.ToString()))
                {
                    string query = HidQueryFlag.Value.ToString();
                    DataTable dt = null;
                    switch (query)
                    {
                        case "1":
                            this.GridState.ColumnModel.SetColumnHeader(0, GetLangStr("DeviceStatus3", "监测点名称"));
                            QueryData.Value = GetWhere();
                            dt = deviceManager.GetStationDeviceState("*", QueryData.Value.ToString());
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                Button2.Disabled = false;
                                Session["datatable"] = dt;
                                StoreState.DataSource = dt;
                                StoreState.DataBind();
                            }
                            else
                            {
                                Button2.Disabled = true;
                                Session["datatable"] = null;
                                Notice(GetLangStr("DeviceStatus4", "提示信息"), GetLangStr("DeviceStatus5", "没查询到数据"));
                                StoreState.DataSource = dt;
                                StoreState.DataBind();
                            }
                            break;

                        case "2":
                            this.GridState.ColumnModel.SetColumnHeader(0, GetLangStr("DeviceStatus6", "设备类型"));
                            QueryData.Value = GetWhere2();
                            dt = deviceManager.GetNoDeviceState("*", QueryData.Value.ToString());
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                Button2.Disabled = false;
                                Session["datatable"] = dt;
                                StoreState.DataSource = dt;
                                StoreState.DataBind();
                            }
                            else
                            {
                                Button2.Disabled = true;
                                Session["datatable"] = null;
                                Notice(GetLangStr("DeviceStatus4", "提示信息"), GetLangStr("DeviceStatus7", "没查询到数据"));
                                StoreState.DataSource = dt;
                                StoreState.DataBind();
                            }
                            break;

                        case "3":
                            this.GridState.ColumnModel.SetColumnHeader(0, GetLangStr("DeviceStatus8", "服务器类型"));
                            QueryData.Value = GetWhere2();
                            dt = deviceManager.GetServiceState("*", QueryData.Value.ToString());
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                Button2.Disabled = false;
                                Session["datatable"] = dt;
                                StoreState.DataSource = dt;
                                StoreState.DataBind();
                            }
                            else
                            {
                                Button2.Disabled = true;
                                Session["datatable"] = null;
                                Notice(GetLangStr("DeviceStatus4", "提示信息"), GetLangStr("DeviceStatus7", "没查询到数据"));
                                StoreState.DataSource = new DataTable();
                                StoreState.DataBind();
                            }
                            break;
                    }
                    string js = "ChangeHeaderInfo(\"" + query + "\");";
                    this.ResourceManager1.RegisterBeforeClientInitScript(js);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        public void show(string id)
        {
            DataTable dt = deviceManager.GetDeviceState_lock_id(id);
            Storelock.DataSource = dt;
            Storelock.DataBind();
            Window2.Show();
            Window2.Center();
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            string where = " 1=1 ";
            if (HidStationType.Value != null)
            {
                where = where + " and  station_type_id='" + HidStationType.Value.ToString() + "' ";
            }

            if (CmbState.SelectedIndex != -1)
            {
                where = where + " and  connect_state='" + CmbState.SelectedItem.Value + "' ";
            }

            return where;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private string GetWhere2()
        {
            string where = " 1=1 ";
            if (CmbState.SelectedIndex != -1)
            {
                where = where + " and  connect_state='" + CmbState.SelectedItem.Value + "' ";
            }
            return where;
        }

        /// <summary>
        /// 转换datatable
        /// </summary>
        /// <returns></returns>
        private DataTable ChangeDataTable()
        {
            DataTable dt = Session["datatable"] as DataTable;
            DataTable dt1 = dt.Copy();
            if (dt1 != null)
            {
                dt1.Columns.Remove("col0"); dt1.Columns.Remove("col1"); dt1.Columns.Remove("col2");
                //dt1.Columns[0].ColumnName = GetLangStr("DeviceStatus33", "监测点名称");
                if (HidQueryFlag.Value != null && HidQueryFlag.Value.ToString() != "")
                {
                    if (HidQueryFlag.Value.ToString() == "2")
                    {
                        dt1.Columns[0].ColumnName = GetLangStr("DeviceStatus6", "设备类型");
                    }
                    else if (HidQueryFlag.Value.ToString() == "3")
                    {
                        dt1.Columns[0].ColumnName = GetLangStr("DeviceStatus8", "服务器类型");
                    }
                    else
                    {
                        dt1.Columns[0].ColumnName = GetLangStr("DeviceStatus3", "监测点名称");
                    }
                }

                dt1.Columns[1].ColumnName = GetLangStr("DeviceStatus35", "设备名称");
                dt1.Columns[2].ColumnName = GetLangStr("DeviceStatus36", "设备IP");
                dt1.Columns[3].ColumnName = GetLangStr("DeviceStatus37", "设备状态");
                dt1.Columns[4].ColumnName = GetLangStr("DeviceStatus38", "网络延时");
                dt1.Columns[5].ColumnName = GetLangStr("DeviceStatus39", "在线率");
                dt1.Columns[6].ColumnName = GetLangStr("DeviceStatus40", "更新时间");
                //PrintColumns pc = new PrintColumns();
                //pc.Add(new PrintColumn("设备类型", 5));
                //pc.Add(new PrintColumn("设备名称", 2));
                //pc.Add(new PrintColumn("设备Ip", 1));
                //pc.Add(new PrintColumn("设备状态", 3));
                //dt2 = Bll.Common.GetDataTablePrint(dt, pc);
            }
            return dt1;
        }

        /// <summary>
        /// 读取Grid选中行数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private string GetdatabyField(string data, string field)
        {
            string f1 = "<" + field + ">";
            string f2 = "</" + field + ">";
            int i = data.IndexOf(f1);
            int j = data.IndexOf(f2);
            if (i >= 0 && j >= 0)
            {
                return data.Substring(i + f1.Length, j - i - f2.Length + 1);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///转换datatable为hashtable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public Hashtable ConventDataTable(DataTable dt)
        {
            Hashtable hts = new Hashtable();
            int j = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string kkType = dt.Rows[i]["col1"].ToString();
                if (!hts.ContainsKey(kkType))
                {
                    j++;
                    hts.Add(kkType, dt.Rows[i]["col0"].ToString());
                }
            }
            return hts;
        }

        /// <summary>
        ///提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Notice(string title, string msg)
        {
            Notification.Show(new NotificationConfig
            {
                Title = title,
                Icon = Icon.Information,
                HideDelay = 2000,
                Height = 120,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.BottomRight,
                    OffsetY = -60
                },
                Html = "<br></br>" + msg + "!"
            });
        }

        /// <summary>
        /// 多语言转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public string GetLangStr(string value, string desc)
        {
            string className = this.GetType().BaseType.FullName;
            return MyNet.Common.Lang.Language.CreateInstance(className).GetLanguageStr(value, desc, className);
        }

        #endregion 私有方法
    }
}