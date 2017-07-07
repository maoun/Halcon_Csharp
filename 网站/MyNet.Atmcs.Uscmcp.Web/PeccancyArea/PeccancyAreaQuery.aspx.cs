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
// Last Modified On : 10-21-2016
// ***********************************************************************
// <copyright file="PeccancyAreaQuery.aspx.cs" company="ZKLT">
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
    /// Class PeccancyAreaQuery.
    /// </summary>
    public partial class PeccancyAreaQuery : System.Web.UI.Page
    {
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

        /// <summary>
        /// The data common
        /// </summary>
        private DataCommon dataCommon = new DataCommon();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        private static string starttime = "";
        private static string endtime = "";
        private const string NoImageUrl = "../images/NoImage.png";
        /// <summary>
        /// Handles the Load event of the Page control.
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
                string js = "alert('您没有登录或操作超时，请重新登陆!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            //判断用户是否登录结束
            if (!X.IsAjaxRequest)
            {
                DataSetDateTime();
                StoreDataBind();
                TbutQueryClick(null, null);
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "访问：" + Request.QueryString["funcname"], userinfo.NowIp, "0");
            }
        }


        /// <summary>
        /// 转换查询模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void changtype(object sender, EventArgs e)
        {
            TxtplateId.Hidden = ChkLike.Checked;
            pnhphm.Hidden = !ChkLike.Checked;
        }
        /// <summary>
        /// 行选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApplyClick(object sender, DirectEventArgs e)
        {
            try
            {
                this.FormPanel1.Collapsed = false;
                string sdata = e.ExtraParams["data"];
                string hphm = Bll.Common.GetdatabyField(sdata, "col2");
                string hpzl = Bll.Common.GetdatabyField(sdata, "col4");

                string url1 = Bll.Common.GetdatabyField(sdata, "col29");
                string url2 = Bll.Common.GetdatabyField(sdata, "col30");
                string url3 = Bll.Common.GetdatabyField(sdata, "");
                if (string.IsNullOrEmpty(url1))
                {
                    url1 = NoImageUrl;
                }
                if (string.IsNullOrEmpty(url2))
                {
                    url2 = NoImageUrl;
                }
                if (string.IsNullOrEmpty(url3))
                {
                    url3 = NoImageUrl;
                }
                string js = "ShowImage(\"" + url1 + "\",\"" + url2 + "\",\"" + url3 + "\",\"" + hphm + "\",\"" + hpzl + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-ApplyClick", ex.Message+"；"+ex.StackTrace, "ApplyClick has an exception");
            }
        }

        /// <summary>
        /// Tbuts the query click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            //this.Panel2.Title = "查询结果：共计查询出符合条件的记录0条,现在显示0条";
            GetData();
        }
        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                this.StorePlateType.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:140001")); // tgsPproperty.GetPalteType();
                this.StorePlateType.DataBind();
                this.StoreStartStation.DataSource = tgsPproperty.GetStartStationInfo();
                this.StoreStartStation.DataBind();

                this.StorePecType.DataSource = GetRedisData.ChangColName(GetRedisData.GetData("Peccancy:WFXW"), true);//tgsPproperty.GetPeccancyType("isuse='1'");
                this.StorePecType.DataBind();
                DataTable deal = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240019")); // tgsPproperty.GetProcessType();
                this.StoreDealType.DataSource = deal;
                this.StoreDealType.DataBind();

                DataTable dt = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:140006"));// tgsPproperty.GetQueryNum();
                //  this.StoreQueryNum.DataSource = dt;
                // this.StoreQueryNum.DataBind();
                // if (dt.Rows.Count > 0)
                //    CmbQueryNum.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-StoreDataBind", ex.Message+"；"+ex.StackTrace, "StoreDataBind has an exception");
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
                logManager.InsertLogError("PeccancyAreaQuery.aspx-DataSetDateTime", ex.Message+"；"+ex.StackTrace, "DataSetDateTime has an exception");
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
                logManager.InsertLogError("PeccancyAreaQuery.aspx-Notice", ex.Message+"；"+ex.StackTrace, "Notice has an exception");
            }

        }

        /// <summary>
        /// Handles the Refresh event of the MyData control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            GetData();
        }

        /// <summary>
        /// TGSs the refresh.
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
                logManager.InsertLogError("PeccancyAreaQuery.aspx-TgsRefresh", ex.Message+"；"+ex.StackTrace, "TgsRefresh has an exception");
            }
        }

        /// <summary>
        /// Buts the reset click.
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
            this.CmbPecType.Reset();
            this.CmbPlateType.Reset();
            this.CmbStartStation.Reset();
            this.CmbEndStation.Reset();
            this.TxtplateId.Reset();
            this.ChkLike.Reset();
            this.CmbDealType.Reset();
            this.WindowEditor1.SetVehicleText("");
            //   this.CmbQueryNum.Reset();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-ButResetClick", ex.Message+"；"+ex.StackTrace, "ButResetClick has an exception");
            }

        }
        /// <summary>
        /// Gets the where.
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {


            try { 
            string where = "1=1 and wfxw!='0'";
            starttime = start.InnerText;
            endtime = end.InnerText;
            where = where + " and wfjssj >= STR_TO_DATE('" + starttime + "','%Y-%m-%d %H:%i:%s')   and wfjssj<=STR_TO_DATE('" + endtime + "','%Y-%m-%d %H:%i:%s')";
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
            if (CmbPecType.SelectedIndex != -1)
            {
                where = where + " and  wfxw='" + CmbPecType.SelectedItem.Value + "' ";
            }

            if (CmbDealType.SelectedIndex != -1)
            {
                where = where + " and  jcbj='" + CmbDealType.SelectedItem.Value + "' ";
            }
            string QueryHphm = string.Empty;
            if (!string.IsNullOrEmpty(WindowEditor1.VehicleText))
            {
                QueryHphm = WindowEditor1.VehicleText + TxtplateId.Text;
            }
            else
            {
                QueryHphm = TxtplateId.Text;
            }
            if (ChkLike.Checked)
            {
                if (!string.IsNullOrEmpty(QueryHphm))
                {
                    where = where + " and  hphm  like '%" + QueryHphm.ToUpper() + "%' ";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(QueryHphm))
                {
                    where = where + " and  hphm='" + QueryHphm.ToUpper() + "' ";
                }
            }
            return where;

            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-GetWhere", ex.Message+"；"+ex.StackTrace, "GetWhere has an exception");
            }
            return null;
        }

        /// 显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowDetails(object sender, DirectEventArgs e)
        {
            try { 
            string data = e.ExtraParams["data"];
            AddWindow(data);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-GetWhere", ex.Message+"；"+ex.StackTrace, "GetWhere has an exception");
            }
        }

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
        /// <summary>
        /// Adds the window.
        /// </summary>
        /// <param name="sdata"></param>
        /// <returns></returns>
        private void AddWindow(string sdata)
        {
            try { 
            Window win = WindowShow.AddPeccancyArea(sdata);
            win.Render(this.Form);
            win.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-AddWindow", ex.Message+"；"+ex.StackTrace, "AddWindow has an exception");
            }
        }
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
            try { 
            curpage.Value = 1;
            ShowQuery(1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-TbutFisrt", ex.Message+"；"+ex.StackTrace, "TbutFisrt has an exception");
            }
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
                logManager.InsertLogError("PeccancyAreaQuery.aspx-SetButState", ex.Message+"；"+ex.StackTrace, "SetButState has an exception");
            }
        }

        /// <summary>
        /// Gets the row number.
        /// </summary>
        /// <returns></returns>
        //private int GetRowNum()
        //{
        //    try
        //    {
        //        string rownum = "";
        //     //   if (CmbQueryNum.SelectedIndex != -1)
        //        {
        //            rownum = CmbQueryNum.SelectedItem.Value.ToString();
        //        }
        //        else
        //        {
        //            rownum = "50";
        //        }
        //        return int.Parse(rownum);
        //    }
        //    catch
        //    {
        //        return 50;
        //    }
        //}

        /// <summary>
        /// Queries the specified where.
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startNum"></param>
        /// <param name="endNum"></param>
        /// <returns></returns>
        private void Query(string where, int startNum, int endNum)
        {
            try { 
            DataTable dt = tgsDataInfo.GetPeccancyAreaInfo(where, startNum, endNum);

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

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns></returns>
        private void GetData()
        {
            try { 
            DataTable tempdt = tgsDataInfo.GetPeccancyAreaMaxWfsj(GetWhere());
            if (tempdt != null && tempdt.Rows.Count > 0)
            {
                realCount.Value = tempdt.Rows[0]["col2"].ToString();
                curpage.Value = 1;
                int rownum = 15;
                allPage.Value = (int)Math.Ceiling(double.Parse(realCount.Value.ToString()) / rownum);
                ShowQuery(1);
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-GetData", ex.Message+"；"+ex.StackTrace, "GetData has an exception");
            }
        }
    }
}