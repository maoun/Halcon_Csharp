using System;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WinPassAnalysis : System.Web.UI.Page
    {

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        private UserLogin userLogin = new UserLogin();

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                this.EnableViewState = false;
                if (!X.IsAjaxRequest)
                {
                    /* ResourceManager1.SetTheme(Ext.Net.Theme.Slate);*/
                    //userLogin.IsLogin();
                    string datetime = Request.QueryString["datetime"];
                    string type = Request.QueryString["type"];
                    InitPage(datetime, type);
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：用户登录", userinfo.NowIp, "0");

                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WinPassAnalysis.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
            }
        }

        private void InitPage(string datetime, string type)
        {
            try
            {
                if (type.Equals("1") || type.Equals("2") || type.Equals("3"))
                {
                    TabAnalysis.AutoLoad.Url = "../Template/WebPassCarAnalysis.aspx?datetime=" + datetime + "&type=" + type;
                    TabAnalysis.Title = "综合图表分析";
                }
                else
                {
                    TabAnalysis.AutoLoad.Url = "../Template/WebAlarmQuery.aspx?datetime=" + datetime + "&type=" + type;
                    TabAnalysis.Title = "综合图表分析";
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WinPassAnalysis.aspx-InitPage", ex.Message + "；" + ex.StackTrace, "InitPage发生异常");
            }
            //TabHot.AutoLoad.Url = "../Template/WebPassCarHot.aspx?datetime=" + datetime + "&type=" + type;
            //TabHot.Title = "车辆热力分布";
        }
    }
}