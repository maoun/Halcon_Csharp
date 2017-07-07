using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;

// ***********************************************************************
// Assembly         : MyNet.Atmcs.Uscmcp.Web
// Author           : zlsyl
// Created          : 07-26-2016
//
// Last Modified By : zlsyl
// Last Modified On : 08-10-2016
// ***********************************************************************
// <copyright file="PeccancyDataCount.aspx.cs" company="ZKLT">
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
    /// Class PeccancyDataCount.
    /// </summary>
    public partial class PeccancyDataCount : System.Web.UI.Page
    {
        /// <summary>
        /// The data count information
        /// </summary>
        private DataCountInfo dataCountInfo = new DataCountInfo();

        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("PeccancyDataCount6", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!this.IsPostBack)
            {
                lblDayCount.Text = GetRedisData.GetData("PeccancyCount:TodayItIsIllegalTo").Rows[0]["zs"].ToString(); //dataCountInfo.GetPeccCarCountDay(DateTime.Now.ToString("yyyy-MM-dd"));

                lblLastDay.Text = GetRedisData.GetData("PeccancyCount:YesterdayIsIllegal").Rows[0]["zs"].ToString();//dataCountInfo.GetPeccCarCountDay(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                int LastWeek = WeekOfYear(DateTime.Now) - 1;
                lblLastWeek.Text = GetRedisData.GetData("PeccancyCount:ThisWeekTheIllegal").Rows[0]["zs"].ToString(); //dataCountInfo.GetPeccCarCountWeek(LastWeek.ToString(), DateTime.Now.ToString("yyyy"));
                lblLastMonth.Text = GetRedisData.GetData("PeccancyCount:ThisMonthTheIllegal").Rows[0]["zs"].ToString();//dataCountInfo.GetPeccCarCountMonth(DateTime.Now.ToString("yyyy-MM"));
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                this.ResourceManager1.RegisterAfterClientInitScript("chooseDate('" + date + "')");
                this.DataBind();
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
        /// 得到当前选择时间的报警情况
        /// </summary>
        [DirectMethod]
        public void GetDangqianTime(string date)
        {
            if (date.Equals(DateTime.Now.ToString("yyyy-MM-dd")))
            {
                Image1.Hidden = true;
                lbXzDayPeccCount.Hidden = true;
            }
            else
            {
                Image1.Hidden = false;
                lbXzDayPeccCount.Hidden = false;
                lbXzDayPeccCount.Text = dataCountInfo.GetPeccCarCountDay(date);
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