using System;
using MyNet.Atmcs.Uscmcp.Bll;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class Blank : System.Web.UI.Page
    {
        private UserLogin userLogin = new UserLogin();

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" +StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            string querystring = Request.QueryString["type"];
            label1.Text = querystring;
        }
    }
}