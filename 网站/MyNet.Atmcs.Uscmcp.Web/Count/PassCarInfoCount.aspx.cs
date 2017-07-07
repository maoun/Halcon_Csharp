using System;
using System.Collections.Generic;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using ServiceStack.Redis;
using MyNet.Common.Lang;
using System.Linq;
using MyNet.Atmcs.Uscmcp.Model;
namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PassCarInfoCount : System.Web.UI.Page
    {
        #region 成员变量

        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private SettingManager settingManager = new SettingManager();
        private UserLogin userLogin = new UserLogin();
        private static string starttime = "";
        private static string endtime = "";
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //new UserLogin().IsLogin();
            try
            {
                if (!X.IsAjaxRequest)
                {
                    //userLogin.IsLoginPage(this);
                    DataSetDateTime();
                    BuildTree(TreeStation.Root);
                    this.WebChartViewer1.Visible = false;
                    winShow.Hide();
                    ButPie.Hidden = true;
                    Session["pc_fxbh"] = "";
                    if (this.CmbCountType.Items.Count > 0)
                    {
                        this.CmbCountType.SelectedIndex = 0;
                    }
                    this.DataBind();
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：用户登录", userinfo.NowIp, "0");

                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoCount.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
            }
        }

        /// <summary>
        /// 设置初始时间
        /// </summary>
        private void DataSetDateTime()
        {
            try
            {
                starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                start.InnerText = starttime;

                endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                end.InnerText = endtime;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoCount.aspx-DataSetDateTime", ex.Message + "；" + ex.StackTrace, "DataSetDateTime发生异常");
            }
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

           
            if (!string.IsNullOrEmpty(CmbCountType.SelectedItem.Value))
            {
                if (string.IsNullOrEmpty(FieldStation.Value.ToString()))
                {
                    Notice("信息提示", "请在卡口列表中选择卡口");
                    return;
                }
                DataTable dt = tgsDataInfo.GetPasscarCount(CmbCountType.SelectedItem.Value, GetWhere());
                StoreCount.DataSource = dt;
                StoreCount.DataBind();
                Session["passcarcount"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    ButPie.Hidden = false;
                    string fxbh = dt.Rows[0]["col1"].ToString();
                    string fxmc = dt.Rows[0]["col0"].ToString() + dt.Rows[0]["col2"].ToString();
                    CreateData(dt, fxbh, fxmc, true);
                }
                else
                {
                    Notice("信息提示", "未查询到任何符合条件的信息");
                }
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoCount.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");
            }


        }

        /// <summary>
        /// 重置按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            try
            {

          
            starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeJs = "clearTime('" + starttime + "','" + endtime + "');";//js方法后面的分号一定要加上
            this.ResourceManager1.RegisterAfterClientInitScript(timeJs);
            if (this.CmbCountType.Items.Count > 0)
            {
                this.CmbCountType.SelectedIndex = 0;
            }
            if (!string.IsNullOrEmpty(FieldStation.Text))
            {
                string js = "directclear();";//调用的前台js方法名
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoCount.aspx-ButResetClick", ex.Message+"；"+ex.StackTrace, "ButResetClick has an exception");
              
            }

        }

        #endregion 控件事件

        #region DirectMethod 方法

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
                logManager.InsertLogError("PassCarInfoCount.aspx-GetDateTime", ex.Message+"；"+ex.StackTrace, "GetDateTime has an exception");
            }
        }

        /// <summary>
        /// 显示图表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ButPie_Click(object sender, DirectEventArgs e)
        {
            try
            {
                winShow.Reload();
                winShow.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoCount.aspx-ButPie_Click", ex.Message + "；" + ex.StackTrace, "ButPie_Click发生异常");
            }
        }

        /// <summary>
        /// 打印按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ButPrint_Click(object sender, DirectEventArgs e)
        {
            //string startTime = DateStartTime.SelectedDate.ToString("yyyy-MM-dd");
            //string endTime = DateEndTime.SelectedDate.ToString("yyyy-MM-dd");
            //string title = " 过往车辆 - 按照[" + CmbCountType.SelectedItem.Text + "]统计";
            //string time = "统计时间:" + startTime + "～" + endTime;
            //this.ResourceManager1.RegisterAfterClientInitScript("Preview(\"" + title + "\",\"" + time + "\")");
        }

        /// <summary>
        /// 打印报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ButPrintChart_Click(object sender, DirectEventArgs e)
        {
            //string startTime = DateStartTime.SelectedDate.ToString("yyyy-MM-dd");
            //string endTime = DateEndTime.SelectedDate.ToString("yyyy-MM-dd");
            //string time = "统计时间:" + startTime + "～" + endTime;
            //this.ResourceManager1.RegisterAfterClientInitScript("PreviewChart(\"" + time + "\")");
        }

        /// <summary>
        /// 显示详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ShowDetails(object sender, DirectEventArgs e)
        {
            try
            {
                string data = e.ExtraParams["data"];
                DataTable dt = Session["passcarcount"] as DataTable;
                string fxbh = Bll.Common.GetdatabyField(data, "col1");
                string fxmc = Bll.Common.GetdatabyField(data, "col2");
                CreateData(dt, fxbh, fxmc, false);
                winShow.Reload();
                winShow.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoCount.aspx-ShowDetails", ex.Message + "；" + ex.StackTrace, "ShowDetails发生异常");
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
        /// <param name="isRefresh"></param>
        private void CreateData(DataTable dt, string fxbh, string fxmc, bool isRefresh)
        {
            try
            {
                if (Session["pc_fxbh"] as string != fxbh || isRefresh)
                {
                    Session["pc_fxbh"] = fxbh;
                    if (dt != null)
                    {
                        DataRow[] drs = dt.Select("col1 ='" + fxbh + "'");
                        Dictionary<string, double> dictdata = new Dictionary<string, double>();
                        for (int i = 0; i < drs.Length; i++)
                        {
                            dictdata.Add(drs[i]["col3"].ToString(), double.Parse(drs[i]["col4"].ToString()));
                        }
                        string title = fxmc + " - 按照[" + CmbCountType.SelectedItem.Text + "]统计";
                        ShowChart(dictdata, title);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoCount.aspx-CreateData", ex.Message + "；" + ex.StackTrace, "CreateData发生异常");
            }
        }

        /// <summary>
        /// 显示图表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="title"></param>
        private void ShowChart(Dictionary<string, double> data, string title)
        {
            try
            {
                MyNet.Atmcs.Uscmcp.Bll.Common.CreatePicChart2(this.WebChartViewer1, data, title);
                this.WebChartViewer1.Visible = true;
                pnlData.Render(this.WebChartViewer1, RenderMode.Auto);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoCount.aspx-ShowChart", ex.Message + "；" + ex.StackTrace, "ShowChart发生异常");
            }
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            try
            {
                string where = "1=1";
                if (string.IsNullOrEmpty(starttime)) starttime = start.InnerText;
                if (string.IsNullOrEmpty(endtime)) endtime = end.InnerText;
                string kssj = DateTime.Parse(starttime).ToString("yyyy-MM-dd HH:mm:ss");
                string jssj = DateTime.Parse(endtime).ToString("yyyy-MM-dd HH:mm:ss");
                where = "  rq >= STR_TO_DATE('" + kssj + "','%Y-%m-%d %H:%i:%s')   and rq<=STR_TO_DATE('" + jssj + "','%Y-%m-%d %H:%i:%s')";

                if (!string.IsNullOrEmpty(FieldStation.Value.ToString()))
                {
                    where = where + " and kkid  in  (" + FieldStation.Value.ToString() + ") ";
                }

                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoCount.aspx-GetWhere", ex.Message + "；" + ex.StackTrace, "GetWhere发生异常");
                return null;
            }
        }

        /// <summary>
        ///提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Notice(string title, string msg)
        {
            try
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
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoCount.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice发生异常");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private List<string> GetList()
        {
            try
            {
                List<string> lst = new List<string>();

                string[] str = GridData.Text.Split(',');
                lst.AddRange(str);
                return lst;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoCount.aspx-GetList", ex.Message + "；" + ex.StackTrace, "GetList发生异常");
                throw;
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
                logManager.InsertLogError("PassCarInfoCount.aspx-BuildTree", ex.Message+"；"+ex.StackTrace, "BuildTree has an exception");
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
                logManager.InsertLogError("PassCarInfoCount.aspx-AddDepartTree", ex.Message+"；"+ex.StackTrace, "AddDepartTree has an exception");
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
                logManager.InsertLogError("PassCarInfoCount.aspx-AddStationTree", ex.Message+"；"+ex.StackTrace, "AddStationTree has an exception");
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
            try
            {
                string className = this.GetType().BaseType.FullName;
                return MyNet.Common.Lang.Language.CreateInstance(className).GetLanguageStr(value, desc, className);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoCount.aspx-GetLangStr", ex.Message + "；" + ex.StackTrace, "GetLangStr发生异常");
                return null;
            }
        }
        #endregion 私有方法
    }
}