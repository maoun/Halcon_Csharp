using System;
using System.Drawing;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Bll;
namespace MyNet.Atmcs.Uscmcp.Web.Count
{
    public partial class AllAnalysisMain : System.Web.UI.Page
    {
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    TabIllegalPlateType.Text = "违法车辆号牌种类分析";
                    TabIllegalVehicleType.Text = "违法车辆类型分析";
                    TabIllegalVehicleProperty.Text = "违法车辆使用性质分析";
                    TabWfDrivingage.Text = "违法驾驶人年龄分析";
                    TabWfDrivingExperience.Text = "违法驾驶人驾龄分析";
                    TabIllegalPlateType_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AllAnalysisMain.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
            }
        }

        /// <summary>
        /// 违法车辆号牌种类分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabIllegalPlateType_Click(object sender, EventArgs e)
        {
            try
            {
                TabIllegalPlateType.ForeColor = Color.Blue;
                TabIllegalPlateType.Font.Bold = true;
                TabIllegalVehicleType.ForeColor = Color.Black;
                TabIllegalVehicleType.Font.Bold = false;
                TabIllegalVehicleProperty.ForeColor = Color.Black;
                TabIllegalVehicleProperty.Font.Bold = false;
                TabWfDrivingage.ForeColor = Color.Black;
                TabWfDrivingage.Font.Bold = false;
                TabWfDrivingExperience.ForeColor = Color.Black;
                TabWfDrivingExperience.Font.Bold = false;

                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/illegalPlateType.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AllAnalysisMain.aspx-TabIllegalPlateType_Click", ex.Message + "；" + ex.StackTrace, "TabIllegalPlateType_Click发生异常");
                throw;
            }
        }

        /// <summary>
        /// 违法车辆类型分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabIllegalVehicleType_Click(object sender, EventArgs e)
        {
            try
            {
                TabIllegalPlateType.ForeColor = Color.Black;
                TabIllegalPlateType.Font.Bold = false;
                TabIllegalVehicleType.ForeColor = Color.Blue;
                TabIllegalVehicleType.Font.Bold = true;
                TabIllegalVehicleProperty.ForeColor = Color.Black;
                TabIllegalVehicleProperty.Font.Bold = false;
                TabWfDrivingage.ForeColor = Color.Black;
                TabWfDrivingage.Font.Bold = false;
                TabWfDrivingExperience.ForeColor = Color.Black;
                TabWfDrivingExperience.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/illegalVehicleType.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AllAnalysisMain.aspx-TabIllegalVehicleType_Click", ex.Message + "；" + ex.StackTrace, "TabIllegalVehicleType_Click发生异常");
                throw;
            }
        }

        /// <summary>
        /// 违法车辆使用性质分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabIllegalVehicleProperty_Click(object sender, EventArgs e)
        {
            try
            {
                TabIllegalPlateType.ForeColor = Color.Black;
                TabIllegalPlateType.Font.Bold = false;
                TabIllegalVehicleType.ForeColor = Color.Black;
                TabIllegalVehicleType.Font.Bold = false;
                TabIllegalVehicleProperty.ForeColor = Color.Blue;
                TabIllegalVehicleProperty.Font.Bold = true;
                TabWfDrivingage.ForeColor = Color.Black;
                TabWfDrivingage.Font.Bold = false;
                TabWfDrivingExperience.ForeColor = Color.Black;
                TabWfDrivingExperience.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/illegalVehicleProperty.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AllAnalysisMain.aspx-TabIllegalVehicleProperty_Click", ex.Message + "；" + ex.StackTrace, "TabIllegalVehicleProperty_Click发生异常");
            }
        }

        /// <summary>
        /// 违法驾驶人年龄分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabWfDrivingage_Click(object sender, EventArgs e)
        {
            try
            {
                TabIllegalPlateType.ForeColor = Color.Black;
                TabIllegalPlateType.Font.Bold = false;
                TabIllegalVehicleType.ForeColor = Color.Black;
                TabIllegalVehicleType.Font.Bold = false;
                TabIllegalVehicleProperty.ForeColor = Color.Black;
                TabIllegalVehicleProperty.Font.Bold = false;
                TabWfDrivingage.ForeColor = Color.Blue;
                TabWfDrivingage.Font.Bold = true;
                TabWfDrivingExperience.ForeColor = Color.Black;
                TabWfDrivingExperience.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/wfDrivingage.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AllAnalysisMain.aspx-TabIllegalVehicleProperty_Click", ex.Message + "；" + ex.StackTrace, "TabIllegalVehicleProperty_Click发生异常");
            }
        }

        /// <summary>
        /// 违法驾驶人驾龄分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabWfDrivingExperience_Click(object sender, EventArgs e)
        {
            try
            {
                TabIllegalPlateType.ForeColor = Color.Black;
                TabIllegalPlateType.Font.Bold = false;
                TabIllegalVehicleType.ForeColor = Color.Black;
                TabIllegalVehicleType.Font.Bold = false;
                TabIllegalVehicleProperty.ForeColor = Color.Black;
                TabIllegalVehicleProperty.Font.Bold = false;
                TabWfDrivingage.ForeColor = Color.Black;
                TabWfDrivingage.Font.Bold = false;
                TabWfDrivingExperience.ForeColor = Color.Blue;
                TabWfDrivingExperience.Font.Bold = true;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/wfDrivingExperience.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AllAnalysisMain.aspx-TabWfDrivingExperience_Click", ex.Message + "；" + ex.StackTrace, "TabWfDrivingExperience_Click发生异常");
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
                logManager.InsertLogError("AllAnalysisMain.aspx-PathUrl", ex.Message + "；" + ex.StackTrace, "PathUrl发生异常");
                return null;
            }
        }
    }
}