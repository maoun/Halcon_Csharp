using ChartDirector;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
// ***********************************************************************
// Assembly         : MyNet.Atmcs.Uscmcp.Web
// Author           : zlsyl
// Created          : 10-20-2016
//
// Last Modified By : zlsyl
// Last Modified On : 10-26-2016
// ***********************************************************************
// <copyright file="PeccancyAreaCount.aspx.cs" company="ZKLT">
//     Copyright (c) ZKLT. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Xml;

/// <summary>
/// The Web namespace.
/// </summary>
namespace MyNet.Atmcs.Uscmcp.Web
{
    /// <summary>
    /// Class PeccancyAreaCount.
    /// </summary>
    public partial class PeccancyAreaCount : System.Web.UI.Page
    {
        #region 成员变量

        /// <summary>
        /// The TGS pproperty
        /// </summary>
        private TgsPproperty tgsPproperty = new TgsPproperty();

        /// <summary>
        /// The TGS data information
        /// </summary>
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();

        /// <summary>
        /// The user login
        /// </summary>
        private UserLogin userLogin = new UserLogin();

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        /// <summary>
        /// The starttime
        /// </summary>
        private static string starttime = "";

        /// <summary>
        /// The endtime
        /// </summary>
        private static string endtime = "";

        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断用户是否登录
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");

                return;
            }
            //判断用户是否登录结束
            if (!X.IsAjaxRequest)
            {
                DataSetDateTime();
                StoreDataBind();
                this.lblCurpage.Text = "1";
                this.lblAllpage.Text = "0";
                this.lblRealcount.Text = "0";
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "访问：" + Request.QueryString["funcname"], userinfo.NowIp, "0");
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            GetData();
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            try
            {
                starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string timeJs = "clearTime('" + starttime + "','" + endtime + "');";//js方法后面的分号一定要加上
                this.ResourceManager1.RegisterAfterClientInitScript(timeJs);
                this.CmbPlateType.Reset();
                this.CmbStartStation.Reset();
                this.CmbEndStation.Reset();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaCount.aspx-ButResetClick", ex.Message+"；"+ex.StackTrace, "ButResetClick has an exception");
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            GetData();
        }

        /// <summary>
        /// 结束卡口刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TgsRefresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                DataTable data = tgsPproperty.GetEndStationInfo(this.CmbStartStation.SelectedItem.Value);
                this.StoreEndStation.DataSource = data;
                this.StoreEndStation.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaCount.aspx-TgsRefresh", ex.Message+"；"+ex.StackTrace, "TgsRefresh has an exception");
            }
        }

        #endregion 控件事件

        #region DirectMethod方法

        /// <summary>
        ///显示详细
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [DirectMethod]
        public void ShowDetails(string data)
        {
            try
            {
                StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(data, null);
                XmlNode xml = eSubmit.Xml;
                AddWindow(xml.InnerXml);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaCount.aspx-ShowDetails", ex.Message+"；"+ex.StackTrace, "ShowDetails has an exception");
            }
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="isstart"></param>
        /// <param name="strtime"></param>
        /// <returns></returns>
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
                logManager.InsertLogError("PeccancyAreaQuery.aspx-GetDateTime", ex.Message+"；"+ex.StackTrace, "GetDateTime has an exception");
            }
        }

        #endregion DirectMethod方法

        #region 私有方法

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        private void GetData()
        {
            try
            {
                string where = GetWhere();
                if (!string.IsNullOrEmpty(where))
                {
                    DataTable dt = tgsDataInfo.GetPeccancyAreaCountCount(where);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        realCount.Value = dt.Rows[0]["col0"].ToString();
                        curpage.Value = 1;
                        int rownum = 15;
                        allPage.Value = (int)Math.Ceiling(double.Parse(realCount.Value.ToString()) / rownum);
                        ShowQuery(1);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-GetData", ex.Message+"；"+ex.StackTrace, "GetData has an exception");
            }
        }

        /// <summary>
        /// Notices the specified title.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private void Notice(string title, string msg)
        {
            try { 
            Notification.Show(new NotificationConfig
            {
                Title = title,
                Icon = Ext.Net.Icon.Error,
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
                logManager.InsertLogError("PeccancyAreaQuery.aspx-Notice", ex.Message+"；"+ex.StackTrace, "Notice has an exception");
            }
        }

        /// <summary>
        /// Gets the where.
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            try { 
            string where = "1=1";
            if (string.IsNullOrEmpty(starttime)) starttime = start.InnerText;
            if (string.IsNullOrEmpty(endtime)) endtime = end.InnerText;
            string kssj = DateTime.Parse(starttime).ToString("yyyy-MM-dd HH:mm:ss");
            string jssj = DateTime.Parse(endtime).ToString("yyyy-MM-dd HH:mm:ss");
            where = where + " and wfjssj >= STR_TO_DATE('" + kssj + "','%Y-%m-%d %H:%i:%s')   and wfjssj<=STR_TO_DATE('" + jssj + "','%Y-%m-%d %H:%i:%s')";

            if (CmbPlateType.SelectedIndex != -1)
            {
                where = where + " and  hpzl='" + CmbPlateType.SelectedItem.Value + "' ";
            }
            if (CmbStartStation.SelectedIndex != -1)
            {
                where = where + " and  kskkid='" + CmbStartStation.SelectedItem.Value + "' ";
            }
            if (CmbEndStation.SelectedIndex != -1)
            {
                where = where + " and  jskkid='" + CmbEndStation.SelectedItem.Value + "' ";
            }
            return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-Notice", ex.Message+"；"+ex.StackTrace, "Notice has an exception");
            
            }
            return null;
        }

        /// <summary>
        /// 设置初始时间
        /// </summary>
        /// <returns></returns>
        private void DataSetDateTime()
        {
            try { 
            starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            start.InnerText = starttime;
            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            end.InnerText = endtime;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-DataSetDateTime", ex.Message+"；"+ex.StackTrace, "DataSetDateTime has an exception");

            }
        }

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        /// <returns></returns>
        private void StoreDataBind()
        {
            try
            {
                this.StorePlateType.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:140001")); // tgsPproperty.GetPalteType();
                this.StorePlateType.DataBind();
                this.StoreStartStation.DataSource = tgsPproperty.GetStartStationInfo();
                this.StoreStartStation.DataBind();
                this.WebChartViewer1.Visible = false;
                this.WebChartViewer2.Visible = false;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-StoreDataBind", ex.Message+"；"+ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// Pecs the area count WFXW.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private void PecAreaCountWfxw(DataTable dt, string name)
        {

            try { 
            string[] wfxwms = { "正常车辆", "超速低于50%", "超速50-70%", "超速70-100%", "超速100%以上" };
            string[] wfxw = { "0", "13031", "17215", "17216", "17217" };
            double[] dxcl = new double[5];
            double[] xxcl = new double[5];
            double[] gacl = new double[5];
            double[] qtcl = new double[5];
            for (int i = 0; i < wfxw.Length; i++)
            {
                System.Data.DataRow[] drs = dt.Select(@"wfxw = '" + wfxw[i] + "'");
                if (drs != null && drs.Length > 0)
                {
                    for (int j = 0; j < drs.Length; j++)
                    {
                        dxcl[i] = dxcl[i] + double.Parse(drs[j]["dxcl"].ToString());
                        xxcl[i] = xxcl[i] + double.Parse(drs[j]["xxcl"].ToString());
                        gacl[i] = qtcl[i] + double.Parse(drs[j]["gacl"].ToString());
                        qtcl[i] = qtcl[i] + double.Parse(drs[j]["qtcl"].ToString());
                    }
                }
                else
                {
                    dxcl[i] = 0;
                    xxcl[i] = 0;
                    gacl[i] = 0;
                    qtcl[i] = 0;
                }
            }
            double dc = 0;
            double xc = 0;
            double ga = 0;
            double qt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dc = dc + double.Parse(dt.Rows[i]["dxcl"].ToString());
                xc = xc + double.Parse(dt.Rows[i]["xxcl"].ToString());
                ga = ga + double.Parse(dt.Rows[i]["gacl"].ToString());
                qt = qt + double.Parse(dt.Rows[i]["qtcl"].ToString());
            }
            string msg = "共计大型车辆：" + dc.ToString() + "条，小型车辆：" + xc.ToString() + "条，公安车辆：" + ga.ToString() + "条，其它车辆" + qt.ToString() + "条";

            XYChart c = new XYChart(800, 500);
            c.addTitle("区间违法行为数据统计图表" + name, "Times New Roman Bold", 15).setBackground(Chart.metalColor(0x8888ff));
            c.addText(110, 475, msg);
            c.setBackground(Chart.metalColor(0xccccff), 0x000000, 1);
            c.addLegend(55, 45, false, "", 8).setBackground(Chart.Transparent);
            c.setPlotArea(80, 80, 640, 360, 0xffffc0, 0xffffe0);
            c.yAxis().setTitle("通过车辆");
            c.yAxis().setTopMargin(20);
            c.xAxis().setLabels(wfxwms);
            BarLayer layer = c.addBarLayer2(Chart.Side, 3);
            layer.addDataSet(dxcl, 0xff8080, "大型车辆");
            layer.addDataSet(xxcl, 0x80ff80, "小型车辆");
            layer.addDataSet(gacl, 0xff80ff, "公安车辆");
            layer.addDataSet(qtcl, 0x8080ff, "其它类型");
            layer.set3D(10);
            layer.setBarShape(Chart.CircleShape);
            WebChartViewer1.Image = c.makeWebImage(Chart.PNG);
            WebChartViewer1.ImageMap = c.getHTMLImageMap("", "", "title='{dataSetName} on {xLabel}: {value} 辆'");
            this.WebChartViewer1.Visible = true;
            pnlWfxwData.Render(this.WebChartViewer1, RenderMode.Auto);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-PecAreaCountWfxw", ex.Message+"；"+ex.StackTrace, "PecAreaCountWfxw has an exception");
            }
        }

        /// <summary>
        /// Pecs the area count XSSD.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private void PecAreaCountXssd(DataTable dt, string name)
        {
            try { 
            string[] sdlxms = { "40KM/H以下", "40-50KM/H", "50-60KM/H", "60-70KM/H", "70-80KM/H", "80-90KM/H", "90-100KM/H", "100-110KM/H", "110-120KM/H", "120KM/H 以上" };
            string[] sdlx = { "000400", "040050", "050060", "060070", "070080", "080090", "090100", "100110", "110120", "120000" };
            double[] dxcl = new double[10];
            double[] xxcl = new double[10];
            double[] gacl = new double[10];
            double[] qtcl = new double[10];

            for (int i = 0; i < sdlx.Length; i++)
            {
                System.Data.DataRow[] drs = dt.Select(@"sdlx = '" + sdlx[i] + "'");
                if (drs != null && drs.Length > 0)
                {
                    for (int j = 0; j < drs.Length; j++)
                    {
                        dxcl[i] = dxcl[i] + double.Parse(drs[j]["dxcl"].ToString());
                        xxcl[i] = xxcl[i] + double.Parse(drs[j]["xxcl"].ToString());
                        gacl[i] = gacl[i] + double.Parse(drs[j]["gacl"].ToString());
                        qtcl[i] = qtcl[i] + double.Parse(drs[j]["qtcl"].ToString());
                    }
                }
                else
                {
                    dxcl[i] = 0;
                    xxcl[i] = 0;
                    gacl[i] = 0;
                    qtcl[i] = 0;
                }
            }
            double dc = 0;
            double xc = 0;
            double ga = 0;
            double qt = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dc = dc + double.Parse(dt.Rows[i]["dxcl"].ToString());
                xc = xc + double.Parse(dt.Rows[i]["xxcl"].ToString());
                ga = ga + double.Parse(dt.Rows[i]["gacl"].ToString());
                qt = qt + double.Parse(dt.Rows[i]["qtcl"].ToString());
            }
            string msg = "共计大型车辆：" + dc.ToString() + "条，小型车辆：" + xc.ToString() + "条，公安车辆：" + ga.ToString() + "条，其它车辆" + qt.ToString() + "条";
            XYChart c = new XYChart(960, 500);
            c.addTitle("区间行驶速度数据统计图表" + name, "Times New Roman Bold", 15).setBackground(Chart.metalColor(0x8888ff));
            c.addText(110, 475, msg);
            c.setBackground(Chart.metalColor(0xccccff), 0x000000, 1);
            c.addLegend(55, 45, false, "", 8).setBackground(Chart.Transparent);
            c.setPlotArea(80, 80, 800, 360, 0xffffc0, 0xffffe0);
            c.yAxis().setTitle("通过车辆");
            c.yAxis().setTopMargin(20);
            c.xAxis().setLabels(sdlxms);
            BarLayer layer = c.addBarLayer2(Chart.Side, 3);
            layer.addDataSet(dxcl, 0xff8080, "大型车辆");
            layer.addDataSet(xxcl, 0x80ff80, "小型车辆");
            layer.addDataSet(gacl, 0xff80ff, "公安车辆");
            layer.addDataSet(qtcl, 0x8080ff, "其它类型");
            layer.set3D(10);
            layer.setBarShape(Chart.CircleShape);
            WebChartViewer2.Image = c.makeWebImage(Chart.PNG);
            WebChartViewer2.ImageMap = c.getHTMLImageMap("", "", "title='{dataSetName} on {xLabel}: {value} 辆'");
            this.WebChartViewer2.Visible = true;
            pnlXssdData.Render(this.WebChartViewer2, RenderMode.Auto);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-PecAreaCountXssd", ex.Message+"；"+ex.StackTrace, "PecAreaCountXssd has an exception");
            }
        }

        /// <summary>
        /// Adds the window.
        /// </summary>
        /// <param name="sdata"></param>
        /// <returns></returns>
        private void AddWindow(string sdata)
        {
            Window win = WindowShow.AddPeccancyArea(sdata);
            win.Render(this.Form);
            win.Show();
        }

        #endregion 私有方法

        [DirectMethod]

        /// <summary>
        /// Tbuts the last.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutLast(object sender, DirectEventArgs e)
        {
            try { 
            int page = int.Parse(curpage.Value.ToString());
            page--;
            if (page < 1)
            {
                page = 1;
            }
            curpage.Value = page;
            ShowQuery(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-TbutLast", ex.Message+"；"+ex.StackTrace, "TbutLast has an exception");
            }
        }

        /// <summary>
        /// Tbuts the next.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutNext(object sender, DirectEventArgs e)
        {
            try { 
            int page = int.Parse(curpage.Value.ToString());
            int allpage = int.Parse(allPage.Value.ToString());
            page++;
            if (page > allpage)
            {
                page = allpage;
            }
            curpage.Value = page;
            ShowQuery(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-TbutNext", ex.Message+"；"+ex.StackTrace, "TbutNext has an exception");
            }
        }

        /// <summary>
        /// Tbuts the fisrt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutFisrt(object sender, DirectEventArgs e)
        {
            curpage.Value = 1;
            ShowQuery(1);
        }

        /// <summary>
        /// Tbuts the end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutEnd(object sender, DirectEventArgs e)
        {
            try { 
            curpage.Value = allPage.Value;
            int page = int.Parse(curpage.Value.ToString());
            ShowQuery(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-TbutEnd", ex.Message+"；"+ex.StackTrace, "TbutEnd has an exception");
            }
        }

        /// <summary>
        /// Sets the state of the but.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private void SetButState(int page)
        {
            try { 
            curpage.Value = page;
            int allpage = int.Parse(allPage.Value.ToString());

            if (allpage > 1)
            {
                ButLast.Disabled = false;
                ButNext.Disabled = false;
                ButFisrt.Disabled = false;
                ButEnd.Disabled = false;
            }
            if (page == 1)
            {
                ButLast.Disabled = true;
                ButFisrt.Disabled = true;
            }
            if (page == allpage)
            {
                ButNext.Disabled = true;
                ButEnd.Disabled = true;
            }
            if (allpage <= 1)
            {
                ButFisrt.Disabled = true;
                ButNext.Disabled = true;
                ButLast.Disabled = true;
                ButEnd.Disabled = true;
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-TbutEnd", ex.Message+"；"+ex.StackTrace, "TbutEnd has an exception");
            }
        }

        /// <summary>
        /// Queries the specified where.
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startNum"></param>
        /// <param name="endNum"></param>
        /// <returns></returns>
        private void Query(string where, int startNum, int endNum)
        {

            try
            {
                DataTable dt = tgsDataInfo.GetPeccancyAreaCountNew(where, startNum, endNum);//tgsDataInfo.GetPeccancyAreaCount(where, startNum, endNum);

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";

                    if (dt.Rows.Count.Equals(15))// 有数据 且够提取的条数
                    {
                        this.lblCurpage.Text = curpage.Value.ToString();
                        this.lblAllpage.Text = allPage.Value.ToString();
                        this.lblRealcount.Text = realCount.Value.ToString();
                    }
                    else
                    {
                        this.lblCurpage.Text = curpage.Value.ToString();
                        this.lblAllpage.Text = allPage.Value.ToString();
                        this.lblRealcount.Text = realCount.Value.ToString();
                    }
                    this.StorePecArea.DataSource = dt;
                    this.StorePecArea.DataBind();
                }
                else
                {
                    this.lblCurpage.Text = curpage.Value.ToString();
                    this.lblAllpage.Text = allPage.Value.ToString();
                    this.lblRealcount.Text = realCount.Value.ToString();
                    this.StorePecArea.DataSource = dt;
                    this.StorePecArea.DataBind();
                    Notice("信息提示", "当前没数据");
                    return;
                }
                string name = "";
                if (CmbStartStation.SelectedIndex != -1)
                {
                    name = "(" + CmbStartStation.SelectedItem.Text;
                }
                if (CmbEndStation.SelectedIndex != -1)
                {
                    name = name + "-->" + CmbEndStation.SelectedItem.Text + ")";
                }
                DataTable wfxw = tgsDataInfo.GetPeccancyAreaCountForWfxw(where);
                PecAreaCountWfxw(wfxw, name);

                DataTable xssd = tgsDataInfo.GetPeccancyAreaCountForXssd(where);
                PecAreaCountXssd(xssd, name);
                this.pnlXssdData.Hide();
                this.pnlWfxwData.ActiveIndex = 0;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-Query", ex.Message+"；"+ex.StackTrace, "Query has an exception");
            }
        }

        /// <summary>
        /// Shows the query.
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        private void ShowQuery(int currentPage)
        {
            try { 
            int rownum = 15;

            int startNum = 0;
            int endNum = 0;
            if (currentPage == 1)
            {
                startNum = 0;
                endNum = rownum;
            }
            else
            {
                startNum = (currentPage - 1) * rownum;
                endNum = currentPage * rownum;
            }
            Query(GetWhere(), startNum, endNum);
            SetButState(currentPage);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-ShowQuery", ex.Message+"；"+ex.StackTrace, "ShowQuery has an exception");
            }
        }

    }
}