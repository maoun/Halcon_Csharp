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
// Last Modified On : 10-24-2016
// ***********************************************************************
// <copyright file="PeccancyAreaCheck.aspx.cs" company="ZKLT">
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
    /// Class PeccancyAreaCheck.
    /// </summary>
    public partial class PeccancyAreaCheck : System.Web.UI.Page
    {
        #region 成员变量
        private const string NoImageUrl = "../images/NoImage.png";
        /// <summary>
        /// The TGS pproperty
        /// </summary>
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        private static string sdr;

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
        /// 初始化页面
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
                this.lblCurpage.Text = "1";
                this.lblAllpage.Text = "0";
                this.lblRealcount.Text = "0";
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "访问：" + Request.QueryString["funcname"], userinfo.NowIp, "0");
            }
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
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            try
            {

          

            string sdr = Request.ServerVariables.Get("Remote_Addr").ToString();
            if (sdr.Length < 9)
            {
                sdr = "127.0.0.1";
            }
            string rownum = "";
            if (CmbQueryNum.SelectedItem.Value != null)
            {
                rownum = CmbQueryNum.SelectedItem.Value.ToString();
            }
            else
            {
                rownum = "50";
            }
            tgsDataInfo.UnAlllockAreaAll(DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), sdr); //将超过1个小时未解锁或者自己的的违法记录全部解锁
            //  tgsDataInfo.LockAreaPeccancy(GetWhere(), sdr, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToInt32(CmbQueryNum.SelectedItem.Value)); //对自己查询的信息进行加锁
            tgsDataInfo.LockAreaPeccancy(GetWhere(), sdr, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToInt32(rownum));
            GetData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");

            }

        }

        /// <summary>
        /// 刷新记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                GetData();
            }
            catch (Exception ex)
            {
                
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-MyData_Refresh", ex.Message+"；"+ex.StackTrace, "MyData_Refresh has an exception");
            }
            
        }

        /// <summary>
        /// 关联结束卡口
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
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            try { 
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
            this.WindowEditor1.Reset();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-ApplyCButResetClicklick", ex.Message+"；"+ex.StackTrace, "ButResetClick has an exception");
            }
        }
        /// <summary>
        /// 开始审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void ButCheckClick(object sender, DirectEventArgs e)
        {
            try
            {
                string js = "OpenAreaCheckModelPage();";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-TgsRefresh", ex.Message+"；"+ex.StackTrace, "TgsRefresh has an exception"); 
               
            }
        }

        #endregion 控件事件

        #region DirectMethod方法

     
        /// 显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowDetails(object sender, DirectEventArgs e)
        {
            string data = e.ExtraParams["data"];
            AddWindow(data);
        }
        //[DirectMethod]
        //public void ShowDetails(string data)
        //{
        //    StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(data, null);
        //    XmlNode xml = eSubmit.Xml;
        //    AddWindow(xml.InnerXml);
        //}

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

        /// <summary>
        /// 设置初始时间
        /// </summary>
        /// <returns></returns>
        private void DataSetDateTime()
        {
            starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            start.InnerText = starttime;
            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            end.InnerText = endtime;
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

                this.StorePecType.DataSource = GetRedisData.ChangColName(GetRedisData.GetData("Peccancy:WFXW"), true);//tgsPproperty.GetPeccancyType("isuse='1'");
                this.StorePecType.DataBind();

                this.StoreDealType.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240019")); // tgsPproperty.GetProcessType();
                this.StoreDealType.DataBind();

                this.StoreQueryNum.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:140006"));// tgsPproperty.GetQueryNum();
                this.StoreQueryNum.DataBind();

               CmbQueryNum.SelectedIndex = 0;
                CmbDealType.SelectedIndex = 0;
                //ButCheck.Disabled = true;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-StoreDataBind", ex.Message+"；"+ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns></returns>
        private void GetData()
        {
            //string rownum = "";
            //if (CmbQueryNum.SelectedIndex != -1)
            //{
            //    rownum = CmbQueryNum.SelectedItem.Value.ToString();
            //}
            //else
            //{
            //    rownum = "50";
            //}
            //string where = GetWhere();
            //if (!string.IsNullOrEmpty(where))
            //{
            sdr = Request.ServerVariables.Get("Remote_Addr").ToString();
            if (sdr.Length < 9)
            {
                sdr = "127.0.0.1";
            }
            DataTable tempdt = tgsDataInfo.GetPeccancyAreaMaxWfsjCount(GetWhere() + "  and sdr='" + sdr + "'");
            if (tempdt != null && tempdt.Rows.Count > 0)
            {
                realCount.Value = tempdt.Rows[0]["col0"].ToString();
                curpage.Value = 1;
                int rownum = 15;
                allPage.Value = (int)Math.Ceiling(double.Parse(realCount.Value.ToString()) / rownum);
                ShowQuery(1);
            }
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private void Notice(string title, string msg)
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

        /// <summary>
        /// 组装查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            string where = "1=1 and wfxw!='0'";
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
            if (CmbPecType.SelectedIndex != -1)
            {
                where = where + " and  wfxw='" + CmbPecType.SelectedItem.Value + "' ";
            }

            if (CmbDealType.Value != null)
            {
                where = where + " and  jcbj='" + CmbDealType.SelectedItem.Value + "' ";
            }
            else
            {
                where = where + " and  jcbj='0' ";
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

        #endregion 私有方法

        private void Query(string where, int startNum, int endNum)
        {

            try { 
            DataTable dt = tgsDataInfo.GetPeccancyAreaInfo(where, startNum, endNum);
            Session["areacheckjson"] = ConvertData.DataTableToJson(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
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
                this.lblCurpage.Text = "1";
                this.lblAllpage.Text = "0";
                this.lblRealcount.Text = "0";
                this.StorePecArea.DataSource = dt;
                this.StorePecArea.DataBind();
                Notice("信息提示", "当前没数据");
                return;
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-ApplyCButResetClicklick", ex.Message+"；"+ex.StackTrace, "ButResetClick has an exception");
            }
        }

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
        /// <param name="sender"></p
        /// aram>
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
        {try
        { 
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
        /// 页码条
        /// </summary>
        /// <param name="currentPage"></param>
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

            Query(GetWhere() + "  and sdr='" + sdr + "'", startNum, endNum);
            SetButState(currentPage);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaQuery.aspx-ShowQuery", ex.Message+"；"+ex.StackTrace, "ShowQuery has an exception");
            }
        }

        /// <summary>
        /// 分页按钮
        /// </summary>
        /// <param name="page"></param>
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
    }
}