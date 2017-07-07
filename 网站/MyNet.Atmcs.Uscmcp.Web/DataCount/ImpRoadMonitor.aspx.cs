using System;
using System.Data;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class ImpRoadMonitor : System.Web.UI.Page
    {
        private DataCountInfo dataCountInfo = new DataCountInfo();
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("ImpRoadMonitor6", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!this.IsPostBack)
            {
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                InitRoad(date);
                this.DataBind();
            }
            UserInfo userinfo = Session["Userinfo"] as UserInfo;
            logManager.InsertLogRunning(userinfo.UserName, GetLangStr("ImpRoadMonitor7", "访问：") + Request.QueryString["funcname"], userinfo.NowIp, "0");
        }

        private void InitRoad(string datetime)
        {
            try
            {
                //string head = "<div class=\"map-up-r-container\"><ul class=\"progress-bar\">";
                //string end = "</ul></div>";
                //string content = "";
                //string color = "green-bg";
                DataTable dt = GetRedisData.GetData("ImpRoadMonitor:ZDDL");//dataCountInfo.GetPassCarHotSpot(datetime, "ZDDL");
                if (dt != null)
                {
                    StoreFlow.DataSource = dt; ;
                    StoreFlow.DataBind();
                    //for (int row = 0; row < dt.Rows.Count; row++)
                    //{
                    //    if (float.Parse(dt.Rows[row]["gwbl"].ToString()) < 30)
                    //    {
                    //        color = "green-bg";
                    //    }
                    //    if (float.Parse(dt.Rows[row]["gwbl"].ToString()) > 30)
                    //    {
                    //        color = "yellow-bg";
                    //    }

                    //    if (float.Parse(dt.Rows[row]["gwbl"].ToString()) > 70)
                    //    {
                    //        color = "orange_bg";
                    //    }
                    //    content += "<li class=\"display-table row\">" +
                    //             " <span class=\"table-cell small-6 pro-ad\">" + row.ToString() + "." + dt.Rows[row]["kkmc"].ToString() + "</span>" +
                    //             "<div class=\"table-cell small-4 pro-bar\">" +
                    //             " <span class=\"pro-bg\">" +
                    //             "<i class=\"pro-step " + color + "\" style=\"width:" + dt.Rows[row]["gwbl"].ToString() + "%;\"></i>" +
                    //             "</span>" + " </div>" +
                    //             "<span class=\"table-cell small-2 pro-persent text-right\">" + dt.Rows[row]["gwbl"].ToString() + "%</span>" +
                    //             "</li>";

                    //}
                    //Tab2.Html = head + content + end;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImpRoadMonitor.aspx-InitRoad", ex.Message+"；"+ex.StackTrace, "InitRoad has an exception");
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