using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class AlarmInfoCount : System.Web.UI.Page
    {
        #region 成员变量

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private SettingManager settingManager = new SettingManager();
        private Bll.ServiceManager servicemansger = new Bll.ServiceManager();
        private UserLogin userLogin = new UserLogin();
        private static string starttime = "";
        private static string endtime = "";

        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("AlarmInfoCount28", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                Session["tree"] = null;
                string js1 = "clearMenu();";
                this.ResourceManager1.RegisterAfterClientInitScript(js1);
                //userLogin.IsLoginPage(this);
                //BuildTree(TreeStation.Root);
                DataSetDateTime();
                this.WebChartViewer1.Visible = false;
                winShow.Hide();
                ButPie.Hidden = true;
                Session["pc_fxbh"] = "";
                if (this.CmbCountType.Items.Count > 0)
                {
                    this.CmbCountType.SelectedIndex = 0;
                }

                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "" + GetLangStr("AlarmInfoCount29", "访问：") + "" + Request.QueryString["funcname"], userinfo.NowIp, "0");
            }
            this.DataBind();
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(kakou.Value.ToString()))
                {
                    Notice(GetLangStr("AlarmInfoCount30", "信息提示"), GetLangStr("AlarmInfoCount31", "请在卡口列表中选择卡口"));
                    return;
                }
                if (!string.IsNullOrEmpty(CmbCountType.SelectedItem.Value))
                {
                    string flowdate = starttime.Substring(0, 10);
                    DataTable dt = tgsDataInfo.GetAlarmCount(CmbCountType.SelectedItem.Value, GetWhere());
                    if (dt != null)
                    {
                        StoreCount.DataSource = dt;
                        StoreCount.DataBind();
                        Session["alarmcount"] = dt;
                        ButPie.Hidden = false;
                        if (dt.Rows.Count > 0)
                        {
                            // 默认加载第一条记录并绘制图
                            string fxbh = dt.Rows[0]["col1"].ToString();
                            string fxmc = dt.Rows[0]["col2"].ToString();
                            CreateData(dt, fxbh, fxmc);
                        }
                        else
                        {
                            Notice(GetLangStr("AlarmInfoCount30", "信息提示"), GetLangStr("AlarmInfoCount32", "未查询到符合条件的任何记录！"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoCount.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        /// <summary>
        /// 重置按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeJs = "clearTime('" + starttime + "','" + endtime + "');";//js方法后面的分号一定要加上
            this.ResourceManager1.RegisterAfterClientInitScript(timeJs);
            this.CmbCountType.Text = "";
            //if (!string.IsNullOrEmpty(FieldStation.Text))
            //{
            //    string js = "directclear();";//调用的前台js方法名
            //    this.ResourceManager1.RegisterAfterClientInitScript(js);
            //}
            //卡口列表
            if (!string.IsNullOrEmpty(kakou.Value))//判断卡口是否为空
            {
                string js = "clearMenu();";
                if (Session["tree"] != null)
                {
                    Session["tree"] = null;
                }
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }

            this.CmbCountType.Reset();
        }

        #endregion 控件事件

        #region DirectMethod 方法

        /// <summary>
        ///显示图表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ButPie_Click(object sender, DirectEventArgs e)
        {
            winShow.Reload();
            winShow.Show();
        }

        /// <summary>
        ///打印按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ButPrint_Click(object sender, DirectEventArgs e)
        {
            string startTime = starttime.Substring(0, 10);
            string endTime = endtime.Substring(0, 10);
            string title = "" + GetLangStr("AlarmInfoCount33", "报警车辆") + " - " + GetLangStr("AlarmInfoCount34", "按照") + "[" + CmbCountType.SelectedItem.Text + "]" + GetLangStr("AlarmInfoCount35", "统计") + "";
            string time = "" + GetLangStr("AlarmInfoCount36", "统计时间") + " ：" + startTime + "～" + endTime;
            this.ResourceManager1.RegisterAfterClientInitScript("Preview(\"" + title + "\",\"" + time + "\")");
        }

        /// <summary>
        /// 打印报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ButPrintChart_Click(object sender, DirectEventArgs e)
        {
            string startTime = starttime.Substring(0, 10);
            string endTime = endtime.Substring(0, 10);
            string time = "" + GetLangStr("AlarmInfoCount36", "统计时间") + "：" + startTime + "～" + endTime;
            this.ResourceManager1.RegisterAfterClientInitScript("PreviewChart(\"" + time + "\")");
        }

        /// <summary>
        /// 显示详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ShowDetails(object sender, DirectEventArgs e)
        {
            string data = e.ExtraParams["data"];

            DataTable dt = Session["alarmcount"] as DataTable;
            string fxbh = Bll.Common.GetdatabyField(data, "col1");
            string fxmc = Bll.Common.GetdatabyField(data, "col2");
            CreateData(dt, fxbh, fxmc);
            winShow.Reload();
            winShow.Show();
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="isstart"></param>
        /// <param name="strtime"></param>
        [DirectMethod]
        public void GetDateTime(bool isstart, string strtime)
        {
            try
            {
                if (isstart)
                    starttime = strtime;
                else
                    endtime = strtime;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoCount.aspx-GetDateTime", ex.Message + "；" + ex.StackTrace, "GetDateTime has an exception");
            }
        }

        #endregion DirectMethod 方法

        #region 私有方法

        /// <summary>
        /// 加载图示
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fxbh"></param>
        /// <param name="fxmc"></param>
        private void CreateData(DataTable dt, string fxbh, string fxmc)
        {
            //if (Session["pc_fxbh"] as string != fxbh)
            //{
            if (Session["pc_fxbh"] != null)
            {
                Session["pc_fxbh"] = null;
            }
            Session["pc_fxbh"] = fxbh;
            if (dt != null)
            {
                DataRow[] drs = dt.Select("col1 ='" + fxbh + "'");
                Dictionary<string, double> dictdata = new Dictionary<string, double>();
                for (int i = 0; i < drs.Length; i++)
                {
                    dictdata.Add(drs[i]["col3"].ToString(), double.Parse(drs[i]["col4"].ToString()));
                }

                ShowChart(dictdata, " " + fxmc + " - " + GetLangStr("AlarmInfoCount34", "按照") + "[" + CmbCountType.SelectedItem.Text + "]" + GetLangStr("AlarmInfoCount35", "统计") + "");
            }
            //}
        }

        /// <summary>
        /// 显示图表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="title"></param>
        private void ShowChart(Dictionary<string, double> data, string title)
        {
            MyNet.Atmcs.Uscmcp.Bll.Common.CreatePicChart2(this.WebChartViewer1, data, title);
            this.WebChartViewer1.Visible = true;
            pnlData.Render(this.WebChartViewer1, RenderMode.Auto);
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            string where = "1=1";
            if (string.IsNullOrEmpty(starttime)) starttime = start.InnerText;
            if (string.IsNullOrEmpty(endtime)) endtime = end.InnerText;
            string kssj = DateTime.Parse(starttime).ToString("yyyy-MM-dd HH:mm:ss");
            string jssj = DateTime.Parse(endtime).ToString("yyyy-MM-dd HH:mm:ss");

            where = "  bjsj >= STR_TO_DATE('" + kssj + "','%Y-%m-%d %H:%i:%s')   and bjsj<=STR_TO_DATE('" + jssj + "','%Y-%m-%d %H:%i:%s')";

            //if (!string.IsNullOrEmpty(FieldStation.Value.ToString()))
            //{
            //    where = where + " and kkid  in  (" + FieldStation.Value.ToString() + ") ";
            //}
            if (!string.IsNullOrEmpty(this.kakou.Value))
            {
                string kkid = this.kakouId.Value.ToString();
                if (!string.IsNullOrEmpty(kkid))
                {
                    where = where + " and kkid  in  (" + kkid + ") ";
                    //condition.Kkid = kkid;
                    if (Session["tree"] != null)
                    {
                        Session["tree"] = null;
                    }
                    Session["tree"] = kkid;
                }
                // condition.Kkidms = this.kakou.Value;
            }

            return where;
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Notice(string title, string msg)
        {
            Notification.Show(new NotificationConfig
            {
                Title = title,
                Icon = Icon.Error,
                HideDelay = 2000,
                Height = 120,
                Html = "<br></br>" + msg + "!"
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private List<string> GetList()
        {
            List<string> lst = new List<string>();

            string[] str = GridData.Text.Split(',');
            lst.AddRange(str);
            return lst;
        }

        /// <summary>
        /// 设置初始时间
        /// </summary>
        private void DataSetDateTime()
        {
            starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            start.InnerText = starttime;

            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            end.InnerText = endtime;
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
                root.Text = GetLangStr("AlarmInfoCount37", "卡口列表");
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

                DataTable dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
                DataRow[] rowsStation = dtStation.Select("departid='" + user.DeptCode + "'", "station_name asc");
                AddStationTree(nodeRoot, rowsStation);

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
                logManager.InsertLogError("AlarmInfoCount.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
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
                DataTable dtDepart = GetRedisData.GetData("t_cfg_department");
                DataRow[] rows = dtDepart.Select("classcode='" + departCode + "'");
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count(); i++)
                    {
                        Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                        nodeRoot.Text = rows[i]["departname"].ToString();
                        nodeRoot.Leaf = true;
                        nodeRoot.NodeID = rows[i]["departid"].ToString();
                        nodeRoot.Icon = Icon.House;

                        DataTable dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
                        DataRow[] rowsStation = dtStation.Select(" departid='" + nodeRoot.NodeID + "' ", "station_name asc");
                        AddStationTree(nodeRoot, rowsStation);
                        nodeRoot.Expanded = false;
                        nodeRoot.Draggable = true;
                        nodeRoot.Expandable = ThreeStateBool.True;
                        AddDepartTree(nodeRoot, rows[i]["departid"].ToString());
                        root.Nodes.Add(nodeRoot);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoCount.aspx-AddDepartTree", ex.Message + "；" + ex.StackTrace, "AddDepartTree has an exception");
            }
        }

        /// <summary>
        /// 添加卡口子节点
        /// </summary>
        /// <param name="root"></param>
        private void AddStationTree(Ext.Net.TreeNode DepartNode, DataRow[] rows)
        {
            try
            {
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count(); i++)
                    {
                        Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                        node.Text = rows[i]["station_name"].ToString();
                        node.Leaf = true;
                        node.Checked = ThreeStateBool.False;
                        node.NodeID = rows[i]["station_id"].ToString();
                        node.Draggable = false;
                        node.AllowDrag = false;
                        DepartNode.Nodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoCount.aspx-AddStationTree", ex.Message + "；" + ex.StackTrace, "AddStationTree has an exception");
            }
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

        #region 卡口选择

        /// <summary>
        /// 清除卡口信息
        /// </summary>
        [DirectMethod]
        public void ClearKakou()
        {
            if (Session["Condition"] != null)
            {
                Condition con = Session["Condition"] as Condition;
                con.Kkid = "";
                con.Kkidms = "";
            }
            if (Session["tree"] != null)
            {
                Session["tree"] = null;
            }
        }

        /// <summary>
        /// 得到符合条件的卡口
        /// </summary>
        [DirectMethod]
        public void GetKakou()
        {
            try
            {
                string value = kakou.Value;

                DataTable dtSelect = null;
                DataTable dt = Session["selectKakou"] as DataTable;//得到整个卡口信息
                DataRow[] rows = dt.Select("STATION_NAME like '" + value + "%'");//选出符合条件的
                if (rows.Length > 0)
                {
                    dtSelect = ToDataTable(rows);
                }
                StringBuilder strs = new StringBuilder();
                strs.AppendLine("[");
                if (dtSelect != null && dtSelect.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSelect.Rows.Count; i++)
                    {
                        if (i == dtSelect.Rows.Count - 1)
                        {
                            strs.AppendLine("{name:'" + dtSelect.Rows[i]["STATION_NAME"] + "',id:'" + dtSelect.Rows[i]["STATION_ID"] + "'}");
                        }
                        else
                        {
                            strs.AppendLine("{name:'" + dtSelect.Rows[i]["STATION_NAME"] + "',id:'" + dtSelect.Rows[i]["STATION_ID"] + "'},");
                        }
                    }
                }
                else
                {
                    strs.AppendLine("{name:'none',id:'none'},");
                }
                strs.AppendLine("]");
                string js = "setUl(" + strs.ToString() + ");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        /// 卡口模糊查询选中的时候给Session["tree"]赋值
        /// </summary>
        [DirectMethod]
        public void SetSession()
        {
            if (Session["tree"] != null)
            {
                Session["tree"] = null;
            }
            Session["tree"] = kakouId.Value;
        }

        #endregion 卡口选择
    }
}