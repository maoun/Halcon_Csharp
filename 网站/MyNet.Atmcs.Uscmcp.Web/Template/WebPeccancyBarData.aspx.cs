using System;
using System.Data;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebPeccancyBarData : System.Web.UI.Page
    {
        private DataCountInfo dataCountInfo = new DataCountInfo();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        protected void Page_Load(object sender, EventArgs e)
        {
            string datetime = Request.QueryString["datetime"];
            if (!string.IsNullOrEmpty(datetime))
            {
                GetChartLine(datetime);
            }
            else
            {
                GetChartLine(DateTime.Now.ToString("yyyy-MM-dd"));
            }
        }

        private void GetChartLine(string datetime)
        {
            try
            {
                DataTable dtpecc = null;
                // DataTable dtpecc = dataCountInfo.GetPeccCarCountHour(datetime);
                if (DateTime.Now.ToString("yyyy-MM-dd").Equals(datetime))
                {
                    dtpecc = GetRedisData.GetData("PeccancyCount:24hoursIsIllegal");
                }
                else if (DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd").Equals(datetime))//昨天的从Redis中查询
                {
                    dtpecc = GetRedisData.GetData("PeccancyCount:Yesterday24hoursIsIllegal");
                }
                else
                {
                    dtpecc = dataCountInfo.GetPeccCarCountHour(datetime);
                }
                int zs = 0;
                string sp = string.Empty;
                for (int i = 0; i < dtpecc.Rows.Count; i++)
                {
                    zs = zs + int.Parse(dtpecc.Rows[i][1].ToString());
                }
                DataTable dt = null;
                // DataTable dt = dataCountInfo.GetPeccCarCountByType(datetime, "WFXW");
                //if (DateTime.Now.ToString("yyyy-MM-dd").Equals(datetime))
                //{
                //    dt = GetRedisData.GetData("PeccancyCount:wfxwToday");
                //}
                //else if (DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd").Equals(datetime))//昨天的从Redis中查询
                //{
                //    dt = GetRedisData.GetData("PeccancyCount:wfxwYesterday");
                //}
                //else
                //{
                    dt = dataCountInfo.GetPeccCarCountByType(datetime, "WFXW");
                //}
                int MyCount = 100 / (dt.Rows.Count + 1);
                string bfb = MyCount.ToString() + "%";
                string html = "<ul  width=\"100%\">";
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int cls = i + 1;
                        double bl = (double.Parse(dt.Rows[i][2].ToString()) / zs) * 100;
                        html = html + GetHtml("data" + cls.ToString(), dt.Rows[i][0].ToString(), Math.Round(bl, 2) + "%", dt.Rows[i][2].ToString(), bfb);
                    }
                }
                else
                {
                    html = GetLangStr("WebPeccancyBarData1", "违法类型统计暂无数据");
                    
                }
                html = html + "</ul>";
                divCount.Html = html;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebPeccancyBarData.aspx-GetChartLine", ex.Message+"；"+ex.StackTrace, "GetChartLine has an exception");
            }
        }

        private string GetHtml(string mclass, string title, string per, string number, string wid)
        {
            try
            {
                string html = "<a href=\"#\"><li class=\"" + mclass + "\" style=\"width: " + wid + "\" >";
                html = html + " <span class=\"title\">" + title + "</span>";
                html = html + "<p><em style=\"width: " + per + "\"></em></p>";
                html = html + "<span class=\"number\">" + number + "</span>";
                html = html + "</li><a>";

                return html;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebPeccancyBarData.aspx-GetHtml", ex.Message+"；"+ex.StackTrace, "GetHtml has an exception");
                return "";
            }
        }
        #region 语言转换

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

        #endregion 语言转换


    }
}