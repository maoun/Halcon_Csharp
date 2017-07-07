using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web.DevManager
{
    public partial class DevManagerMain : System.Web.UI.Page
    {
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    TabTGS.Text = "卡口接入配置";
                    TabCCTV.Text = "视频接入配置";
                    TabTGS_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DevManagerMain.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
            }
        }
        /// <summary>
        /// 卡口接入配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabTGS_Click(object sender, EventArgs e)
        {
            try
            {
                TabTGS.ForeColor = Color.Blue;
                TabTGS.Font.Bold = true;
                TabCCTV.ForeColor = Color.Black;
                TabCCTV.Font.Bold = false;
                IFRAME1.Attributes.Add("src", "../DevManager/TGSStationManager.aspx");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DevManagerMain.aspx-TabTGS_Click", ex.Message + "；" + ex.StackTrace, "TabTGS_Click发生异常");
            }
        }
        protected void TabCCTV_Click(object sender, EventArgs e)
        {
            try
            {
                TabTGS.ForeColor = Color.Black;
                TabTGS.Font.Bold = false;
                TabCCTV.ForeColor = Color.Blue;
                TabCCTV.Font.Bold = true;
                IFRAME1.Attributes.Add("src", "../DevManager/CCTVStationManager.aspx");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DevManagerMain.aspx-TabTGS_Click", ex.Message + "；" + ex.StackTrace, "TabTGS_Click发生异常");
            }
        }
    }
}