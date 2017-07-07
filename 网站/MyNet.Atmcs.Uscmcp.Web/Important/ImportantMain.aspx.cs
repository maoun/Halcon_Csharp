using System;
using System.Drawing;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web.Important
{
    public partial class ImportantMain : System.Web.UI.Page
    {
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    TabRule.Text = "2-5s客运车辆";
                    TabArea.Text = "区间超速设置";
                    TabRule_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantMain.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
                throw;
            }
        }

        protected void TabRule_Click(object sender, EventArgs e)
        {
            try
            {
                TabRule.ForeColor = Color.Blue;
                TabRule.Font.Bold = true;
                TabArea.ForeColor = Color.Black;
                TabArea.Font.Bold = false;
                IFRAME1.Attributes.Add("src", "../Important/ImportantRule.aspx");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantMain.aspx-TabRule_Click", ex.Message + "；" + ex.StackTrace, "TabRule_Click发生异常");
                throw;
            }
        }

        protected void TabArea_Click(object sender, EventArgs e)
        {
            try
            {
                TabArea.ForeColor = Color.Blue;
                TabArea.Font.Bold = true;
                TabRule.ForeColor = Color.Black;
                TabRule.Font.Bold = false;
                IFRAME1.Attributes.Add("src", "../PeccancyArea/PeccancyAreaSetting.aspx");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantMain.aspx-TabArea_Click", ex.Message + "；" + ex.StackTrace, "TabArea_Click发生异常");
                throw;
            }
        }
    }
}