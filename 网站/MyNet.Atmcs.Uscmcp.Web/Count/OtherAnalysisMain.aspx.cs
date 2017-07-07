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
    public partial class OtherAnalysisMain : System.Web.UI.Page
    {
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    TabPassCarInfoCount.Text = "过往车辆统计";
                    TabPassCarFlowCount.Text = "流量查询统计";
                    TabPassCarSpeedCount.Text = "速度查询统计";
                    TabPassCarOcrCount.Text = "识别率查询统计";
                    TabPassCarInfoCount_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OtherAnalysisMain.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
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
                logManager.InsertLogError("OtherAnalysisMain.aspx-PathUrl", ex.Message + "；" + ex.StackTrace, "PathUrl发生异常");
                return null;
            }
        }
        /// <summary>
        /// 过往车辆统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabPassCarInfoCount_Click(object sender, EventArgs e)
        {
            try
            {
                TabPassCarInfoCount.ForeColor = Color.Blue;
                TabPassCarInfoCount.Font.Bold = true;
                TabPassCarFlowCount.ForeColor = Color.Black;
                TabPassCarFlowCount.Font.Bold = false;
                TabPassCarSpeedCount.ForeColor = Color.Black;
                TabPassCarSpeedCount.Font.Bold = false;
                TabPassCarOcrCount.ForeColor = Color.Black;
                TabPassCarOcrCount.Font.Bold = false;

                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/PassCarInfoCount.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OtherAnalysisMain.aspx-TabPassCarInfoCount_Click", ex.Message + "；" + ex.StackTrace, "TabPassCarInfoCount_Click发生异常");
            }
        }
        /// <summary>
        /// 流量查询统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabPassCarFlowCount_Click(object sender, EventArgs e)
        {
            try
            {
                TabPassCarInfoCount.ForeColor = Color.Black;
                TabPassCarInfoCount.Font.Bold = false;
                TabPassCarFlowCount.ForeColor = Color.Blue;
                TabPassCarFlowCount.Font.Bold = true;
                TabPassCarSpeedCount.ForeColor = Color.Black;
                TabPassCarSpeedCount.Font.Bold = false;
                TabPassCarOcrCount.ForeColor = Color.Black;
                TabPassCarOcrCount.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/PassCarFlowCount.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OtherAnalysisMain.aspx-TabPassCarFlowCount_Click", ex.Message + "；" + ex.StackTrace, "TabPassCarFlowCount_Click发生异常");
            }
        }
        /// <summary>
        /// 速度查询统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabPassCarSpeedCount_Click(object sender, EventArgs e)
        {
            try
            {
                TabPassCarInfoCount.ForeColor = Color.Black;
                TabPassCarInfoCount.Font.Bold = false;
                TabPassCarFlowCount.ForeColor = Color.Black;
                TabPassCarFlowCount.Font.Bold = false;
                TabPassCarSpeedCount.ForeColor = Color.Blue;
                TabPassCarSpeedCount.Font.Bold = true;
                TabPassCarOcrCount.ForeColor = Color.Black;
                TabPassCarOcrCount.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/PassCarSpeedCount.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OtherAnalysisMain.aspx-TabPassCarSpeedCount_Click", ex.Message + "；" + ex.StackTrace, "TabPassCarSpeedCount_Click发生异常");
            }
        }
        /// <summary>
        /// 识别率查询统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabPassCarOcrCount_Click(object sender, EventArgs e)
        {
            try
            {
                TabPassCarInfoCount.ForeColor = Color.Black;
                TabPassCarInfoCount.Font.Bold = false;
                TabPassCarFlowCount.ForeColor = Color.Black;
                TabPassCarFlowCount.Font.Bold = false;
                TabPassCarSpeedCount.ForeColor = Color.Black;
                TabPassCarSpeedCount.Font.Bold = false;
                TabPassCarOcrCount.ForeColor = Color.Blue;
                TabPassCarOcrCount.Font.Bold = true;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/PassCarOcrCount.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OtherAnalysisMain.aspx-TabPassCarOcrCount_Click", ex.Message + "；" + ex.StackTrace, "TabPassCarOcrCount_Click发生异常");
            }
        }
     
    }
}