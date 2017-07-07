using System;
using System.Data;
using MyNet.Atmcs.Uscmcp.Bll;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web.Map
{
    public partial class TgsShowDevice : System.Web.UI.Page
    {
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        private MapManager bll = new MapManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                string kkid = Session["id"] as string;
                string time = Session["datetime"] as string;
                DataTable devdt =Bll.Common.ChangColName(bll.GetWorkStatic(kkid,time));
                if (devdt != null && devdt.Rows.Count > 0)
                {
                    this.StoreDevice.DataSource = devdt;
                    this.StoreDevice.DataBind();
                    this.LblDevice.Text = "监测点名称：" + devdt.Rows[0]["col1"].ToString() + "";
                }
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "访问：监测点名称", userinfo.NowIp, "0");
            }
        }
    }
}