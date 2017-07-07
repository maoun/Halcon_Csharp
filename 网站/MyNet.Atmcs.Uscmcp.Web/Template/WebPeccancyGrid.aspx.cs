using System;
using System.Data;
using System.Text;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebPeccancyGrid : System.Web.UI.Page
    {
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        private DataCountInfo dataCountInfo = new DataCountInfo();
        private SettingManager settingManager = new SettingManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            //违法多发数据分布表
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
                this.show.InnerHtml = GetTable(datetime);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);

                logManager.InsertLogError("WebPeccancyGrid.aspx-GetChartLine", ex.Message+"；"+ex.StackTrace, "GetChartLine has an exception");
            }
        }

        private string GetTable(string datetime)
        {
            StringBuilder strhead = new StringBuilder();
            StringBuilder strbody = new StringBuilder();
            DataTable dtWfxw = null; //dataCountInfo.GetPeccCarCountByType(datetime, "WFXW");
            if (DateTime.Now.ToString("yyyy-MM-dd").Equals(datetime))
            {
                dtWfxw = GetRedisData.GetData("PeccancyCount:wfxwToday");
            }
            else if (DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd").Equals(datetime))//昨天的从Redis中查询
            {
                dtWfxw = GetRedisData.GetData("PeccancyCount:wfxwYesterday");
            }
            else
            {
                dtWfxw = dataCountInfo.GetPeccCarCountByType(datetime, "WFXW");
            }
            strhead.Append("<th>" + GetLangStr("WebPeccancyGrid2", "单位名称") + "</th>");
            if (dtWfxw != null && dtWfxw.Rows.Count > 0)
            {
                foreach (DataRow item in dtWfxw.Rows)
                {
                    //组装头
                    strhead.Append("  <th>" + item[0].ToString() + "</th>");
                }
            }
            else
            {
                strhead.Append("  <th>" + GetLangStr("WebPeccancyGrid3", "无记录") + "</th>");
            }

            //从Redis中读取所属机构
            DataTable dtCjjg = GetRedisData.GetData("PeccancyCount:ReadTheBayonet"); //settingManager.GetDepartmentDict("00");
            foreach (DataRow itemcjjg in dtCjjg.Rows)
            {
                string where = "a.wfsj=STR_TO_DATE('" + datetime + "', '%Y-%m-%d')  and b.departid='" + itemcjjg[0].ToString() + "'";
                //获得该机关所有的违法行为总数
                DataTable dt = dataCountInfo.GetPeccCarCountByWhere(where);
                string str = "<tr><td>" + itemcjjg[1].ToString() + "</td>";
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dtWfxw.Rows)
                    {
                        DataRow[] drs = dt.Select("wfxw='" + item[1].ToString() + "'");
                        if (drs.Length > 0)
                        {
                            str += "<td>" + drs[0][1].ToString() + "</td>";
                        }
                        else
                        {
                            str += "<td>0</td>";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dtWfxw.Rows.Count; i++)
                    {
                        str += "<td>0</td>";
                    }
                }
                str += "  </tr>";
                strbody.Append(str);
            }

            return @"  <table style='visibility: hidden; width: 100%; height: 100%'  id='data'>
            <thead> <tr> " + strhead.ToString() + " </tr></thead><tbody>" + strbody + "  </tbody> </table>";
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