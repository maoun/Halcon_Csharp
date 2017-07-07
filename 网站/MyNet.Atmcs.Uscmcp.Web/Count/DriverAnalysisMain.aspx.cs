using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web.Count
{
    public partial class DriverAnalysisMain : System.Web.UI.Page
    {
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager=new Bll.LogManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    TabDriveAgeAnalysis.Text = "驾龄分析";
                    TabQuarterDrivingFrom.Text = "来源分析";
                    TabQuarterDrivingAge.Text = "年龄分析";
                    TabRemoteDriver.Text = "异地驾驶人分析";
                    TabCarlienseAnalysis.Text = "准驾车型分析";
                    TabDriveAgeAnalysis_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DriverAnalysisMain.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
                throw;
            }
        }
        /// <summary>
        /// 驾龄分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabDriveAgeAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                TabDriveAgeAnalysis.ForeColor = Color.Blue;
                TabDriveAgeAnalysis.Font.Bold = true;
                TabQuarterDrivingFrom.ForeColor = Color.Black;
                TabQuarterDrivingFrom.Font.Bold = false;
                TabQuarterDrivingAge.ForeColor = Color.Black;
                TabQuarterDrivingAge.Font.Bold = false;
                TabRemoteDriver.ForeColor = Color.Black;
                TabRemoteDriver.Font.Bold = false;
                TabCarlienseAnalysis.ForeColor = Color.Black;
                TabCarlienseAnalysis.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/driveAgeAnalysis.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DriverAnalysisMain.aspx-TabDriveAgeAnalysis_Click", ex.Message + "；" + ex.StackTrace, "TabDriveAgeAnalysis_Click发生异常");
                throw;
            }
        }
        /// <summary>
        /// 来源分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabQuarterDrivingFrom_Click(object sender, EventArgs e)
        {
            try
            {
                TabDriveAgeAnalysis.ForeColor = Color.Black;
                TabDriveAgeAnalysis.Font.Bold = false;
                TabQuarterDrivingFrom.ForeColor = Color.Blue;
                TabQuarterDrivingFrom.Font.Bold = true;
                TabQuarterDrivingAge.ForeColor = Color.Black;
                TabQuarterDrivingAge.Font.Bold = false;
                TabRemoteDriver.ForeColor = Color.Black;
                TabRemoteDriver.Font.Bold = false;
                TabCarlienseAnalysis.ForeColor = Color.Black;
                TabCarlienseAnalysis.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/quarterDrivingFrom.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DriverAnalysisMain.aspx-TabQuarterDrivingFrom_Click", ex.Message + "；" + ex.StackTrace, "TabQuarterDrivingFrom_Click发生异常");
                throw;
            }
        }
        /// <summary>
        /// 年龄分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabQuarterDrivingAge_Click(object sender, EventArgs e)
        {
            try
            {
                TabDriveAgeAnalysis.ForeColor = Color.Black;
                TabDriveAgeAnalysis.Font.Bold = false;
                TabQuarterDrivingFrom.ForeColor = Color.Black;
                TabQuarterDrivingFrom.Font.Bold = false;
                TabQuarterDrivingAge.ForeColor = Color.Blue;
                TabQuarterDrivingAge.Font.Bold = true;
                TabRemoteDriver.ForeColor = Color.Black;
                TabRemoteDriver.Font.Bold = false;
                TabCarlienseAnalysis.ForeColor = Color.Black;
                TabCarlienseAnalysis.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/quarterDrivingAge.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DriverAnalysisMain.aspx-TabQuarterDrivingAge_Click", ex.Message + "；" + ex.StackTrace, "TabQuarterDrivingAge_Click发生异常");
                throw;
            }
        }
        /// <summary>
        /// 异地驾驶人分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabRemoteDriver_Click(object sender, EventArgs e)
        {
            try
            {
                TabDriveAgeAnalysis.ForeColor = Color.Black;
                TabDriveAgeAnalysis.Font.Bold = false;
                TabQuarterDrivingFrom.ForeColor = Color.Black;
                TabQuarterDrivingFrom.Font.Bold = false;
                TabQuarterDrivingAge.ForeColor = Color.Black;
                TabQuarterDrivingAge.Font.Bold = false;
                TabRemoteDriver.ForeColor = Color.Blue;
                TabRemoteDriver.Font.Bold = true;
                TabCarlienseAnalysis.ForeColor = Color.Black;
                TabCarlienseAnalysis.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/quarterDrivingElsewhere.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DriverAnalysisMain.aspx-TabRemoteDriver_Click", ex.Message + "；" + ex.StackTrace, "TabRemoteDriver_Click发生异常");
                throw;
            }
        }
        /// <summary>
        /// 准驾车型分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabCarlienseAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                TabDriveAgeAnalysis.ForeColor = Color.Black;
                TabDriveAgeAnalysis.Font.Bold = false;
                TabQuarterDrivingFrom.ForeColor = Color.Black;
                TabQuarterDrivingFrom.Font.Bold = false;
                TabQuarterDrivingAge.ForeColor = Color.Black;
                TabQuarterDrivingAge.Font.Bold = false;
                TabRemoteDriver.ForeColor = Color.Black;
                TabRemoteDriver.Font.Bold = false;
                TabCarlienseAnalysis.ForeColor = Color.Blue;
                TabCarlienseAnalysis.Font.Bold = true;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/carlienseAnalysis.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DriverAnalysisMain.aspx-TabCarlienseAnalysis_Click", ex.Message + "；" + ex.StackTrace, "TabCarlienseAnalysis_Click发生异常");
                throw;
            }
        }
        //得到配置文件中的路径
        public string PathUrl()
        {
            try
            {
                string itgsurl = System.Configuration.ConfigurationManager.AppSettings["itgs"].ToString();
                if (!String.IsNullOrEmpty(itgsurl))
                {
                    if (!itgsurl.Substring(itgsurl.Length - 1).Equals("/"))
                    {
                        itgsurl = itgsurl + "/";
                    }
                }
                return itgsurl;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DriverAnalysisMain.aspx-PathUrl", ex.Message + "；" + ex.StackTrace, "PathUrl发生异常");
                return null;
            }
        }
    }
}