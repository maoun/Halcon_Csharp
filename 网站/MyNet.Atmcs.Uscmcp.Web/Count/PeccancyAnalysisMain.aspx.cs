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
    public partial class PeccancyAnalysisMain : System.Web.UI.Page
    {
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    TabWfxw.Text = "违法行为分析";
                    TabPenishTypeAnalysis.Text = "处罚种类分析";
                    TabIllegalScoreAnalysis.Text = "违法记分数分析";
                    TabIllegalPeriodAnalysis.Text = "违法时段分析";
                    TabWfPlateTopTen.Text = "违法车辆种类前十分析";
                    TabPeccancyCount.Text = "识别有效率统计";
                    TabWfxw_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAnalysisMain.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
            }
        }
        /// <summary>
        /// 违法行为分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabWfxw_Click(object sender, EventArgs e)
        {
            try
            {
                TabWfxw.ForeColor = Color.Blue;
                TabWfxw.Font.Bold = true;
                TabPenishTypeAnalysis.ForeColor = Color.Black;
                TabPenishTypeAnalysis.Font.Bold = false;
                TabIllegalScoreAnalysis.ForeColor = Color.Black;
                TabIllegalScoreAnalysis.Font.Bold = false;
                TabIllegalPeriodAnalysis.ForeColor = Color.Black;
                TabIllegalPeriodAnalysis.Font.Bold = false;
                TabWfPlateTopTen.ForeColor = Color.Black;
                TabWfPlateTopTen.Font.Bold = false;
                TabPeccancyCount.ForeColor = Color.Black;
                TabPeccancyCount.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/WFXWTopTen.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAnalysisMain.aspx-TabWfxw_Click", ex.Message + "；" + ex.StackTrace, "TabWfxw_Click发生异常");
            }
        }
        /// <summary>
        /// 处罚种类分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabPenishTypeAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                TabWfxw.ForeColor = Color.Black;
                TabWfxw.Font.Bold = false;
                TabPenishTypeAnalysis.ForeColor = Color.Blue;
                TabPenishTypeAnalysis.Font.Bold = true;
                TabIllegalScoreAnalysis.ForeColor = Color.Black;
                TabIllegalScoreAnalysis.Font.Bold = false;
                TabIllegalPeriodAnalysis.ForeColor = Color.Black;
                TabIllegalPeriodAnalysis.Font.Bold = false;
                TabWfPlateTopTen.ForeColor = Color.Black;
                TabWfPlateTopTen.Font.Bold = false;
                TabPeccancyCount.ForeColor = Color.Black;
                TabPeccancyCount.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/penishTypeAnalysis.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAnalysisMain.aspx-TabPenishTypeAnalysis_Click", ex.Message + "；" + ex.StackTrace, "TabPenishTypeAnalysis_Click发生异常");
            }
        }
        /// <summary>
        /// 违法记分数分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabIllegalScoreAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                TabWfxw.ForeColor = Color.Black;
                TabWfxw.Font.Bold = false;
                TabPenishTypeAnalysis.ForeColor = Color.Black;
                TabPenishTypeAnalysis.Font.Bold = false;
                TabIllegalScoreAnalysis.ForeColor = Color.Blue;
                TabIllegalScoreAnalysis.Font.Bold = true;
                TabIllegalPeriodAnalysis.ForeColor = Color.Black;
                TabIllegalPeriodAnalysis.Font.Bold = false;
                TabWfPlateTopTen.ForeColor = Color.Black;
                TabWfPlateTopTen.Font.Bold = false;
                TabPeccancyCount.ForeColor = Color.Black;
                TabPeccancyCount.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/illegalScoreAnalysis.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAnalysisMain.aspx-TabIllegalScoreAnalysis_Click", ex.Message + "；" + ex.StackTrace, "TabIllegalScoreAnalysis_Click发生异常");
            }
        }
        /// <summary>
        /// 违法时段分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabIllegalPeriodAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                TabWfxw.ForeColor = Color.Black;
                TabWfxw.Font.Bold = false;
                TabPenishTypeAnalysis.ForeColor = Color.Black;
                TabPenishTypeAnalysis.Font.Bold = false;
                TabIllegalScoreAnalysis.ForeColor = Color.Black;
                TabIllegalScoreAnalysis.Font.Bold = false;
                TabIllegalPeriodAnalysis.ForeColor = Color.Blue;
                TabIllegalPeriodAnalysis.Font.Bold = true;
                TabWfPlateTopTen.ForeColor = Color.Black;
                TabWfPlateTopTen.Font.Bold = false;
                TabPeccancyCount.ForeColor = Color.Black;
                TabPeccancyCount.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/illegalPeriodAnalysis.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAnalysisMain.aspx-TabIllegalPeriodAnalysis_Click", ex.Message + "；" + ex.StackTrace, "TabIllegalPeriodAnalysis_Click发生异常");
            }
        }
        /// <summary>
        /// 违法车辆种类前十分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabWfPlateTopTen_Click(object sender, EventArgs e)
        {
            try
            {
                TabWfxw.ForeColor = Color.Black;
                TabWfxw.Font.Bold = false;
                TabPenishTypeAnalysis.ForeColor = Color.Black;
                TabPenishTypeAnalysis.Font.Bold = false;
                TabIllegalScoreAnalysis.ForeColor = Color.Black;
                TabIllegalScoreAnalysis.Font.Bold = false;
                TabIllegalPeriodAnalysis.ForeColor = Color.Black;
                TabIllegalPeriodAnalysis.Font.Bold = false;
                TabWfPlateTopTen.ForeColor = Color.Blue;
                TabWfPlateTopTen.Font.Bold = true;
                TabPeccancyCount.ForeColor = Color.Black;
                TabPeccancyCount.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/wfPlateTopTen.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAnalysisMain.aspx-TabWfPlateTopTen_Click", ex.Message + "；" + ex.StackTrace, "TabWfPlateTopTen_Click发生异常");
            }
        }
        /// <summary>
        /// 识别有效率统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabPeccancyCount_Click(object sender, EventArgs e)
        {
            try
            {
                TabWfxw.ForeColor = Color.Black;
                TabWfxw.Font.Bold = false;
                TabPenishTypeAnalysis.ForeColor = Color.Black;
                TabPenishTypeAnalysis.Font.Bold = false;
                TabIllegalScoreAnalysis.ForeColor = Color.Black;
                TabIllegalScoreAnalysis.Font.Bold = false;
                TabIllegalPeriodAnalysis.ForeColor = Color.Black;
                TabIllegalPeriodAnalysis.Font.Bold = false;
                TabWfPlateTopTen.ForeColor = Color.Black;
                TabWfPlateTopTen.Font.Bold = false;
                TabPeccancyCount.ForeColor = Color.Blue;
                TabPeccancyCount.Font.Bold = true;
                IFRAME1.Attributes.Add("src", "../count/PeccancyCount.aspx");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAnalysisMain.aspx-TabPeccancyCount_Click", ex.Message + "；" + ex.StackTrace, "TabPeccancyCount_Click发生异常");
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
                logManager.InsertLogError("PeccancyAnalysisMain.aspx-PathUrl", ex.Message + "；" + ex.StackTrace, "PathUrl发生异常");
                throw;
            }
        }
    }
}