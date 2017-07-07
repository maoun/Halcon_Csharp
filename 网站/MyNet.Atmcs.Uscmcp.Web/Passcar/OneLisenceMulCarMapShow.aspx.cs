using System;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web.Passcar
{
    public partial class OneLisenceMulCarMapShow : System.Web.UI.Page
    {
        private UserLogin userLogin = new UserLogin();

        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"]; if (!userLogin.CheckLogin(username)) { string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" +StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }
            if (!X.IsAjaxRequest)
            {
                try
                {
                    if (Session["Condition"] != null)
                    {
                        PassInfo con = Session["Condition"] as PassInfo;
                        labhphm.FieldLabel = "&nbsp&nbsp&nbsp&nbsp号牌号码：" + con.Hphm1 + "&nbsp&nbsp&nbsp&nbsp距离：" + con.Len + "米" + "&nbsp&nbsp&nbsp&nbsp时间点：" + con.Gcsj1 + "---" + con.Gcsj2;
                        string points = con.Xpos1 + "," + con.Ypos1 + "|" + con.Xpos2 + "," + con.Ypos2;
                        string js = "BMAP.addMarkerbs('../Map/img/com.png','" + con.Xpos1 + "','" + con.Ypos1 + "','" + con.Lkmc1 + "','','" + con.Zjwj1 + "');";
                        this.ResourceManager1.RegisterAfterClientInitScript(js);
                        //js="BMAP.addMarkerlabel('../Map/img/com.png','" + con.Xpos2 + "','" + con.Ypos2 + "','" + con.Lkmc2 + "','');";
                        js = "BMAP.addMarkerbs('../Map/img/com.png','" + con.Xpos2 + "','" + con.Ypos2 + "','" + con.Lkmc2 + "','','" + con.Zjwj2 + "');";
                        js = js + "BMAP.addPolyline2('#ff0000','" + points + "', '" + con.Len + "'); ";
                        this.ResourceManager1.RegisterAfterClientInitScript(js);
                        //js= "BMAP.openWindow('" + con.Zjwj1 + "','" + con.Xpos1 + "','" + con.Ypos1 + "');";
                        //js += "BMAP.openWindow('" + con.Zjwj2 + "','" + con.Xpos2 + "','" + con.Ypos2 + "');";
                        // this.ResourceManager1.RegisterAfterClientInitScript(js);
                    }
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex.Message);
                }
            }
        }
    }
}