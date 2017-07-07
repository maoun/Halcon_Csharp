using System;
using System.Collections.Generic;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PeccancyCount : System.Web.UI.Page
    {
        #region 成员变量

        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        #endregion 成员变量

        #region 事件

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
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            try
            {
                if (!X.IsAjaxRequest)
                {
                    this.DateStartTime.Value = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-1));
                    this.DateEndTime.Value = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    this.TimeStart.Text = DateTime.Now.ToString("00:00:01");
                    this.TimeEnd.Text = DateTime.Now.ToString("23:59:59");
                    BuildTree(TreePanel1.Root);
                    this.WebChartViewer1.Visible = false;
                    ButPie.Hidden = true;

                    Session["pc_wfdd"] = "";
                    if (this.CmbCountType.Items.Count > 0)
                    {
                        this.CmbCountType.SelectedIndex = 0;
                    }
                    this.DataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyCount.aspx-Page_Load", ex.Message+"；"+ex.StackTrace, "Page_Load has an exception");
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
                if (string.IsNullOrEmpty(GridData.Value.ToString()))
                {
                    Notice("信息提示", "请选择违法地点！");
                    return;
                }
                if (!string.IsNullOrEmpty(CmbCountType.SelectedItem.Value))
                {
                    string flowdate = DateStartTime.Value.ToString();
                    string ctype = "1";
                    if (!string.IsNullOrEmpty(GridData.Value.ToString()))
                    {
                        ctype = "0";
                    }

                    DataTable dt = tgsDataInfo.GetPeccancyCount(CmbCountType.SelectedItem.Value, GetWhere(), ctype);
                    StoreCount.DataSource = dt;
                    StoreCount.DataBind();
                    Session["peccancycount"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        ButPie.Hidden = false;
                        string fxbh = dt.Rows[0]["col0"].ToString();
                        string fxmc = dt.Rows[0]["col1"].ToString();
                        CreateData(dt, fxbh, fxmc, true);
                    }
                    else
                    {
                        Notice("信息提示", "未查询到符合条件的信息");
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyCount.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            try
            {
                this.DateStartTime.Value = string.Format("{0:yyyy-MM-dd}", DateTime.Now.AddDays(-1));
                this.DateEndTime.Value = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                if (this.CmbCountType.Items.Count > 0)
                {
                    this.CmbCountType.SelectedIndex = 0;
                }
                string js = "ClearCheckState();";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyCount.aspx-ButResetClick", ex.Message+"；"+ex.StackTrace, "ButResetClick has an exception");
            }
        }

        /// <summary>
        /// 显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ShowDetails(object sender, DirectEventArgs e)
        {
            try
            {
                string data = e.ExtraParams["data"];

                DataTable dt = Session["peccancycount"] as DataTable;
                string wfdd = Bll.Common.GetdatabyField(data, "col0");
                string wfdz = Bll.Common.GetdatabyField(data, "col1");
                CreateData(dt, wfdd, wfdz, false);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyCount.aspx-ShowDetails", ex.Message+"；"+ex.StackTrace, "ShowDetails has an exception");
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
                PanChart.Collapsed = !PanChart.Collapsed;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyCount.aspx-ButPie_Click", ex.Message+"；"+ex.StackTrace, "ButPie_Click has an exception");
            }
        }

        /// <summary>
        /// 打印统计报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ButPrint_Click(object sender, DirectEventArgs e)
        {
            try
            {
                string startTime = DateStartTime.SelectedDate.ToString("yyyy-MM-dd");
                string endTime = DateEndTime.SelectedDate.ToString("yyyy-MM-dd");
                string title = " 违法车辆 - 按照[" + CmbCountType.SelectedItem.Text + "]统计";
                string time = "统计时间:" + startTime + "～" + endTime;
                this.ResourceManager1.RegisterAfterClientInitScript("Preview(\"" + title + "\",\"" + time + "\")");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyCount.aspx-ButPrint_Click", ex.Message+"；"+ex.StackTrace, "ButPrint_Click has an exception");
            }
        }

        /// <summary>
        /// 打印图表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ButPrintChart_Click(object sender, DirectEventArgs e)
        {
            try
            {
                string startTime = DateStartTime.SelectedDate.ToString("yyyy-MM-dd");
                string endTime = DateEndTime.SelectedDate.ToString("yyyy-MM-dd");
                string time = "统计时间:" + startTime + "～" + endTime;
                this.ResourceManager1.RegisterAfterClientInitScript("PreviewChart(\"" + time + "\")");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyCount.aspx-ButPrintChart_Click", ex.Message+"；"+ex.StackTrace, "ButPrintChart_Click has an exception");
            }
        }

        #endregion 事件

        #region 私有方法

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            string where = "1=1";

            try
            {
                string startTime = DateStartTime.SelectedDate.ToString("yyyy-MM-dd") + " " + DateTime.Parse(this.TimeStart.Text).ToString("HH:mm:00");
                string endTime = DateEndTime.SelectedDate.ToString("yyyy-MM-dd") + " " + DateTime.Parse(this.TimeEnd.Text).ToString("HH:mm:59");
                where = "  wfsj >= STR_TO_DATE('" + startTime + "','%Y-%m-%d %H:%i:%s')   and wfsj<=STR_TO_DATE('" + endTime + "','%Y-%m-%d %H:%i:%s')";
                if (!string.IsNullOrEmpty(GridData.Value.ToString()))
                {
                    where = where + " and  kkid  in  (" + GridData.Value.ToString() + ") ";
                }

                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyCount.aspx-GetWhere", ex.Message+"；"+ex.StackTrace, "GetWhere has an exception");
                return "";
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
                    AlignCfg = new NotificationAlignConfig
                    {
                        ElementAnchor = AnchorPoint.BottomRight,
                        OffsetY = -60
                    },
                    Html = "<br></br>" + msg + "!"
                });
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyCount.aspx-Notice", ex.Message+"；"+ex.StackTrace, "Notice has an exception");
            }
        }

        /// <summary>
        /// 建立卡口树
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
                root.Text = "Root";
                nodes.Add(root);

                DataTable dt = tgsPproperty.GetStationInfo("b.istmsshow ='1'");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    node.Text = dt.Rows[i][2].ToString();
                    node.Icon = Icon.Camera;
                    node.NodeID = dt.Rows[i][1].ToString();
                    node.Leaf = true;
                    node.Checked = ThreeStateBool.False;
                    root.Nodes.Add(node);
                }
                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyCount.aspx-BuildTree", ex.Message+"；"+ex.StackTrace, "BuildTree has an exception");
                return null;
            }
        }

        /// <summary>
        /// 赋值饼图
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
                logManager.InsertLogError("PeccancyCount.aspx-ShowChart", ex.Message+"；"+ex.StackTrace, "ShowChart has an exception");
            }
        }

        /// <summary>
        /// 将选择的卡口的方向分隔存入List<>
        /// </summary>
        /// <returns></returns>
        private List<string> GetList()
        {
            List<string> lst = new List<string>();

            try
            {
                string[] str = GridData.Text.Split(',');
                lst.AddRange(str);
                return lst;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyCount.aspx-GetList", ex.Message+"；"+ex.StackTrace, "GetList has an exception");
                return null;
            }
        }

        /// <summary>
        /// 赋值饼图
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="wfdd"></param>
        /// <param name="wfdz"></param>
        /// <param name="isRefresh"></param>
        private void CreateData(DataTable dt, string wfdd, string wfdz, bool isRefresh)
        {
            try
            {
                if (Session["pc_wfdd"] as string != wfdd || isRefresh)
                {
                    Session["pc_wfdd"] = wfdd;
                    if (dt != null)
                    {
                        DataRow[] drs = dt.Select("col0 ='" + wfdd + "'");
                        Dictionary<string, double> dictdata = new Dictionary<string, double>();
                        for (int i = 0; i < drs.Length; i++)
                        {
                            dictdata.Add(drs[i]["col2"].ToString(), double.Parse(drs[i]["col3"].ToString()));
                        }
                        string title = wfdz + " - 按照[" + CmbCountType.SelectedItem.Text + "]统计";
                        ShowChart(dictdata, title);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyCount.aspx-CreateData", ex.Message+"；"+ex.StackTrace, "CreateData has an exception");
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
                logManager.InsertLogError("PeccancyCount.aspx-CreateData", ex.Message + "；" + ex.StackTrace, "CreateData发生异常");
                return null;
            }
        }
        #endregion 私有方法
    }
}