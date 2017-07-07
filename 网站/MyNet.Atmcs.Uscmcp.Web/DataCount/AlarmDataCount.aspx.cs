using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using System;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class AlarmDataCount : System.Web.UI.Page
    {
        private DataCountInfo dataCountInfo = new DataCountInfo();
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string username = Request.QueryString["username"];
                if (!userLogin.CheckLogin(username))
                {
                    string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                    System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                    return;
                }
                if (!this.IsPostBack)
                {
                    lblDayCount.Text = GetRedisData.GetData("AlarmCount:TotalNumberOfAlarmToday").Rows[0]["zs"].ToString(); //dataCountInfo.GetAlarmCarCountDay(DateTime.Now.ToString("yyyy-MM-dd"));

                    lblLastDay.Text = GetRedisData.GetData("AlarmCount:TotalNumberOfAlarmYesterday").Rows[0]["zs"].ToString(); //dataCountInfo.GetAlarmCarCountDay(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                    int LastWeek = WeekOfYear(DateTime.Now) - 1;
                    lblLastWeek.Text = GetRedisData.GetData("AlarmCount:ThisWeekTheTotalNumberOfAlarm").Rows[0]["zs"].ToString(); //dataCountInfo.GetAlarmCarCountWeek(LastWeek.ToString(), DateTime.Now.ToString("yyyy"));
                    lblLastMonth.Text = GetRedisData.GetData("AlarmCount:TheTotalNumberOfMonthlyReportToThePolice").Rows[0]["zs"].ToString();//dataCountInfo.GetAlarmCarCountMonth(DateTime.Now.ToString("yyyy-MM"));

                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    this.ResourceManager1.RegisterAfterClientInitScript("chooseDate('" + date + "')");
                    this.DataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmDataCount.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
            }
        }

        public int WeekOfYear(DateTime curDay)
        {
            try
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
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmDataCount.aspx-WeekOfYear", ex.Message + "；" + ex.StackTrace, "WeekOfYear发生异常");
                return 0;
            }
        }

        private string GetHtml(string mclass, string title, string per, string number, string wid)
        {
            try
            {
                string html = "<a href=\"#\"  onclick=\"Window4.show();MapCenter();\"><li class=\"" + mclass + "\" style=\"width: " + wid + "\" >";

                html = html + " <span class=\"title\">" + title + "</span>";
                html = html + "<p><em style=\"width: " + per + "\"></em></p>";
                html = html + "<span class=\"number\">" + number + "</span>";
                html = html + "</li><a>";

                return html;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmDataCount.aspx-GetHtml", ex.Message + "；" + ex.StackTrace, "GetHtml发生异常");
                return null;
            }
        }

        /// <summary>
        /// 得到当前选择时间的报警情况
        /// </summary>
        [DirectMethod]
        public void GetDangqianTime(string date)
        {
            try
            {
                if (date.Equals(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    Image1.Hidden = true;
                    lbXzDayAlarmCount.Hidden = true;
                }
                else
                {
                    Image1.Hidden = false;
                    lbXzDayAlarmCount.Hidden = false;
                    lbXzDayAlarmCount.Text = dataCountInfo.GetAlarmCarCountDay(date);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmDataCount.aspx-GetDangqianTime", ex.Message + "；" + ex.StackTrace, "GetDangqianTime发生异常");
                throw;
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
            try
            {
                string className = this.GetType().BaseType.FullName;
                return MyNet.Common.Lang.Language.CreateInstance(className).GetLanguageStr(value, desc, className);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmDataCount.aspx-GetLangStr", ex.Message + "；" + ex.StackTrace, "GetLangStr发生异常");
                return null;
            }
        }

        #endregion 多语言转换
    }
}