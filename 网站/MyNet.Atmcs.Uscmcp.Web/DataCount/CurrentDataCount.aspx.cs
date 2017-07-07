using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;

// ***********************************************************************
// Assembly         : MyNet.Atmcs.Uscmcp.Web
// Author           : zlsyl
// Created          : 07-26-2016
//
// Last Modified By : zlsyl
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="CurrentDataCount.aspx.cs" company="ZKLT">
//     Copyright (c) ZKLT. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

/// <summary>
/// The Web namespace.
/// </summary>
namespace MyNet.Atmcs.Uscmcp.Web
{
    /// <summary>
    /// Class CurrentDataCount.
    /// </summary>
    public partial class CurrentDataCount : System.Web.UI.Page
    {
        /// <summary>
        /// The data count information
        /// </summary>
        private DataCountInfo dataCountInfo = new DataCountInfo();

        /// <summary>
        /// The BLL
        /// </summary>
        private MapManager bll = new MapManager();

        /// <summary>
        /// The user login
        /// </summary>
        private UserLogin userLogin = new UserLogin();

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        ///
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
    
                string js = "alert('" + GetLangStr("CurrentDataCount13", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                CountDay.Value = DateTime.Now.ToString("yyyy-MM-dd");

                lblDayCount.Text = GetRedisData.GetCount("PassCarCount:passcount");
                //dataCountInfo.GetPassCarCountDay(DateTime.Now.ToString("yyyy-MM-dd"));
                lblDayOnline.Text = GetRedisData.GetCount("PassCarCount:passcount_online");
                //dataCountInfo.GetOnlineCarCountDay(DateTime.Now.ToString("yyyy-MM-dd"));

                lblLastDay.Text = GetRedisData.GetCount("PassCarCount:passcount_last");
                //dataCountInfo.GetPassCarCountDay(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                lblLastDayOnline.Text = GetRedisData.GetCount("PassCarCount:passcount_online_last");
                //dataCountInfo.GetOnlineCarCountDay(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));

                int LastWeek = WeekOfYear(DateTime.Now) - 1;
                lblLastWeek.Text = GetRedisData.GetCount("PassCarCount:passcount_week");
                //dataCountInfo.GetPassCarCountWeek(LastWeek.ToString(), DateTime.Now.ToString("yyyy"));
                lblLastMonth.Text = GetRedisData.GetCount("PassCarCount:passcount_month");
                //dataCountInfo.GetPassCarCountMonth(DateTime.Now.ToString("yyyy-MM"));

                string date = DateTime.Now.ToString("yyyy-MM-dd");
                this.ResourceManager1.RegisterAfterClientInitScript("chooseDate('" + date + "')");
                this.DataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("CurrentDataCount14", "访问：") + Request.QueryString["funcname"], userinfo.NowIp, "0");
            }
        }

        /// <summary>
        /// Weeks the of year.
        /// </summary>
        /// <param name="curDay"></param>
        /// <returns></returns>
        public int WeekOfYear(DateTime curDay)
        {
            int firstdayofweek = Convert.ToInt32(Convert.ToDateTime(curDay.Year.ToString() + "- " + "1-1 ").DayOfWeek);

            int days = curDay.DayOfYear;
            int daysOutOneWeek = days - (7 - firstdayofweek);

            if (daysOutOneWeek <= 0)
            {
                return 1;
            }
            else
            {
                int weeks = daysOutOneWeek / 7;
                if (daysOutOneWeek % 7 != 0)
                    weeks++;

                return weeks + 1;
            }
        }

        /// <summary>
        /// Feeds the back date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [DirectMethod]
        public void FeedBackDate(string date)
        {
            CountDay.Value = date;
        }

        /// <summary>
        /// 得到当前选择时间的车流情况
        /// </summary>
        [DirectMethod]
        public void GetDangqianTime(string date)
        {
            if (date.Equals(DateTime.Now.ToString("yyyy-MM-dd")))
            {
                Image1.Hidden = true;
                Image2.Hidden = true;
                lbXzDayCount.Hidden = true;
                lbXzDayOnline.Hidden = true;
                DisFiled1.Hidden = true;
                DisFiled2.Hidden = true;
            }
            else
            {
                Image1.Hidden = false;
                Image2.Hidden = false;
                lbXzDayCount.Hidden = false;
                lbXzDayOnline.Hidden = false;
                DisFiled1.Hidden = false;
                DisFiled2.Hidden = false;
                lbXzDayCount.Text = dataCountInfo.GetPassCarCountDay(date);
                lbXzDayOnline.Text = dataCountInfo.GetOnlineCarCountDay(date);
            }
        }

        #region 多语言转换

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

        #endregion 多语言转换
    }
}