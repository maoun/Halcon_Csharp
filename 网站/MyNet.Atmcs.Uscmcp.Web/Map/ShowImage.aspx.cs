using Ext.Net;
using System;

namespace MyNet.Atmcs.Uscmcp.Web.Map
{
    public partial class ShowImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                try
                {
                    string hphm = "", gcsj = "", kkmc = "", url = "", clwz = "";
                    hphm = Request.QueryString["hphm"];
                    gcsj = Request.QueryString["gcsj"];
                    kkmc = Request.QueryString["kkmc"];
                    url = Request.QueryString["url"];
                    urlimg.Text = url;
                    clwz = Request.QueryString["clwz"];
                    lblPassInfo.Text = hphm + " ：" + gcsj;
                    //    url = "http://192.168.1.249:8001/capture/2016/01/01/12/100000010700/100000010700_其它_无牌_20160101125620_4_03_0_0_0.JPG";
                    //string js = "canvas_tu('http://192.168.1.249:8001/capture/2016/01/01/12/100000010700/100000010700_其它_无牌_20160101125620_4_03_0_0_0.JPG', '100,100,100,100');";//
                    string js = "canvas_tu('" + url + "', '" + clwz + "');"; //
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
                catch (Exception ex)
                {
                }
            }
        }

        [DirectMethod]
        public void OpenImage()
        {
            string js = "OpenPic('" + urlimg.Text + "');";

            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }
    }
}