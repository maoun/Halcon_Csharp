using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using System;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebPassCarBarData : System.Web.UI.Page
    {
        private DataCountInfo dataCountInfo = new DataCountInfo();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
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
        }

        /// <summary>
        /// 创建图表
        /// </summary>
        /// <param name="datetime"></param>
        private void GetChartLine(string datetime)
        {
            try
            {
                string zs = "0";
                if (DateTime.Now.ToString("yyyy-MM-dd").Equals(datetime))
                {
                    zs = GetRedisData.GetCount("PassCarCount:passcount");
                }
                else if (DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd").Equals(datetime))
                {
                    zs = GetRedisData.GetCount("PassCarCount:passcount_last");
                }
                else
                {
                    zs = dataCountInfo.GetPassCarCountDay(datetime);
                }
                int MyCount = 99 / 10;
                string bfb = MyCount.ToString() + ".9" + "%";
                string html = "<ul  width=\"100%\">";
                html = html + GetHtml("data1", GetLangStr("WebPassCarBarData1", "本市车辆"), zs, GetCount(datetime, "1"), bfb, "1", datetime);
                html = html + GetHtml("data2", GetLangStr("WebPassCarBarData2", "本省车辆"), zs, GetCount(datetime, "2"), bfb, "2", datetime);
                html = html + GetHtml("data3", GetLangStr("WebPassCarBarData3", "外省车辆"), zs, GetCount(datetime, "3"), bfb, "3", datetime);
                html = html + GetHtml("data4", GetLangStr("WebPassCarBarData4", "黑名单"), zs, GetCount(datetime, "4"), bfb, "4", datetime);
                html = html + GetHtml("data5", GetLangStr("WebPassCarBarData5", "初入车"), zs, GetCount(datetime, "5"), bfb, "5", datetime);
                html = html + GetHtml("data6",GetLangStr("WebPassCarBarData6", "套牌车"), zs, GetCount(datetime, "6"), bfb, "6", datetime);
                html = html + GetHtml("data7",GetLangStr("WebPassCarBarData7", "未年审"), zs, GetCount(datetime, "7"), bfb, "7", datetime);
                html = html + GetHtml("data8", GetLangStr("WebPassCarBarData8", "黄标车"), zs, GetCount(datetime, "8"), bfb, "8", datetime);
                html = html + "</ul>";
                divCount.Html = html;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LoginWebPassCarBarDataaspx-GetChartLine", ex.Message + "；" + ex.StackTrace, "GetChartLine has an exception");
            }
        }

        /// <summary>
        /// 计算百分比
        /// </summary>
        /// <param name="zs"></param>
        /// <param name="tj"></param>
        /// <returns></returns>
        private string Getbfb(string zs, string tj)
        {
            try
            {
                double bfb = (double.Parse(tj) / double.Parse(zs)) * 100;
                if (bfb >= 100)
                {
                    bfb = 100;
                }
                return Math.Round(bfb, 0).ToString();
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("LoginWebPassCarBarDataaspx-Getbfb", ex.Message + "；" + ex.StackTrace, "Getbfb has an exception");
                ILog.WriteErrorLog(ex);
                return "0";
            }
        }

        /// <summary>
        /// 按时间和类型统计总数
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetCount(string datetime, string type)
        {
            DataTable dt = new DataTable();

            switch (type)
            {
                // 本市
                case "1":
                    dt = GetTypeCount(datetime, "BDFZJGTJ");
                    break;
                //本省
                case "2":
                    dt = GetTypeCount(datetime, "BSFZJGTJ");
                    break;
                // 外省
                case "3":
                    dt = GetTypeCount(datetime, "WDFZJGTJ");
                    break;
                //黑名单
                case "4":
                    dt = SelectHmd(GetTypeCount(datetime, "TJ300108"), "300108");
                    break;
                // 初入车
                case "5":
                    dt = SelectHmd(GetTypeCount(datetime, "TJ300109"), "300109");
                    break;
                // 套牌车
                case "6":
                    dt = SelectHmd(GetTypeCount(datetime, "TJ300104"), "300104");
                    break;
                //未年审
                case "7":
                    dt = SelectHmd(GetTypeCount(datetime, "TJ300102"), "300102");
                    break;
                // 黄标车
                case "8":
                    dt = SelectHmd(GetTypeCount(datetime, "TJ300107"), "300107");
                    break;
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                if (("45678").IndexOf(type) >= 0)
                {
                    return dt.Rows[0][1].ToString();
                }
                return dt.Rows[0][0].ToString();
            }
            return "0";
        }

        private DataTable GetTypeCount(string datetime, string type)
        {
            if (DateTime.Now.ToString("yyyy-MM-dd").Equals(datetime))
            {
                if (type.IndexOf("TJ300") >= 0)
                {
                    type = "HMDTJ";
                }
                return GetRedisData.GetData("PassCarCount:" + type);
            }
            else if (DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd").Equals(datetime))
            {
                if (type.IndexOf("TJ300") >= 0)
                {
                    type = "HMDTJ";
                }
                return GetRedisData.GetData("PassCarCount:" + type + "_last");
            }
            else
            {
                return dataCountInfo.GetPassCarCountByType(datetime, type);
            }
        }

        private DataTable SelectHmd(DataTable dt, string type)
        {
            DataTable dtNew = dt.Clone();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] drs = dt.Select("bjlx='" + type + "'");
                if (drs.Length > 0)
                {
                    dtNew = drs.CopyToDataTable();
                }
            }
            return dtNew;
        }

        /// <summary>
        /// 组装显示html
        /// </summary>
        /// <param name="mclass"></param>
        /// <param name="title"></param>
        /// <param name="zs"></param>
        /// <param name="number"></param>
        /// <param name="wid"></param>
        /// <param name="type"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        private string GetHtml(string mclass, string title, string zs, string number, string wid, string type, string datetime)
        {
            try
            {
                string html = "<a href=\"#\"  onclick=\"WindowShow('" + type + "','" + datetime + "');\"><li class=\"" + mclass + "\" style=\"width: " + wid + "\" >";
                string per = Getbfb(zs, number) + "%";
                html = html + " <span class=\"title\">" + title + "</span>";
                html = html + "<p><em style=\"width: " + per + "\"></em></p>";
                html = html + "<span class=\"number\">" + number + "</span>";
                html = html + "</li><a>";

                return html;
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("LoginWebPassCarBarDataaspx-GetHtml", ex.Message + "；" + ex.StackTrace, "GetHtml has an exception");
                ILog.WriteErrorLog(ex);
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