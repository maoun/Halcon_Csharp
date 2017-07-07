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
    public partial class VehicleAnalysisMain : System.Web.UI.Page
    {
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    TabCllx.Text = "车辆类型分析";
                    TabHpzl.Text = "号牌种类分析";
                    TabSyxz.Text = "使用性质分析";
                    TabSyq.Text = "所有权分析";
                    TabWnj.Text = "逾期未年检分析";
                    TabCllx_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("VehicleAnalysisMain.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
            }
        }
        /// <summary>
        ///号牌种类分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabCllx_Click(object sender, EventArgs e)
        {
            try
            {
                TabCllx.ForeColor = Color.Blue;
                TabCllx.Font.Bold = true;
                TabHpzl.ForeColor = Color.Black;
                TabHpzl.Font.Bold = false;
                TabSyxz.ForeColor = Color.Black;
                TabSyxz.Font.Bold = false;
                TabSyq.ForeColor = Color.Black;
                TabSyq.Font.Bold = false;
                TabWnj.ForeColor = Color.Black;
                TabWnj.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/vehicleTypeAnalysis.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("VehicleAnalysisMain.aspx-TabCllx_Click", ex.Message + "；" + ex.StackTrace, "TabCllx_Click发生异常");
            }
        }
        /// <summary>
        /// 号牌种类分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabHpzl_Click(object sender, EventArgs e)
        {
            try
            {
                TabCllx.ForeColor = Color.Black;
                TabCllx.Font.Bold = false;
                TabHpzl.ForeColor = Color.Blue;
                TabHpzl.Font.Bold = true;
                TabSyxz.ForeColor = Color.Black;
                TabSyxz.Font.Bold = false;
                TabSyq.ForeColor = Color.Black;
                TabSyq.Font.Bold = false;
                TabWnj.ForeColor = Color.Black;
                TabWnj.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/plateTypeAnalysis.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("VehicleAnalysisMain.aspx-TabHpzl_Click", ex.Message + "；" + ex.StackTrace, "TabHpzl_Click发生异常");
            }
        }
        /// <summary>
        /// 使用性质分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabSyxz_Click(object sender, EventArgs e)
        {
            try
            {
                TabCllx.ForeColor = Color.Black;
                TabCllx.Font.Bold = false;
                TabHpzl.ForeColor = Color.Black;
                TabHpzl.Font.Bold = false;
                TabSyxz.ForeColor = Color.Blue;
                TabSyxz.Font.Bold = true;
                TabSyq.ForeColor = Color.Black;
                TabSyq.Font.Bold = false;
                TabWnj.ForeColor = Color.Black;
                TabWnj.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/propertyAnalysis.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("VehicleAnalysisMain.aspx-TabSyxz_Click", ex.Message + "；" + ex.StackTrace, "TabSyxz_Click发生异常");
            }
        }
        /// <summary>
        /// 所有权分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabSyq_Click(object sender, EventArgs e)
        {
            try
            {
                TabCllx.ForeColor = Color.Black;
                TabCllx.Font.Bold = false;
                TabHpzl.ForeColor = Color.Black;
                TabHpzl.Font.Bold = false;
                TabSyxz.ForeColor = Color.Black;
                TabSyxz.Font.Bold = false;
                TabSyq.ForeColor = Color.Blue;
                TabSyq.Font.Bold = true;
                TabWnj.ForeColor = Color.Black;
                TabWnj.Font.Bold = false;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/ownershipAnalysis.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("VehicleAnalysisMain.aspx-TabSyq_Click", ex.Message + "；" + ex.StackTrace, "TabSyq_Click发生异常");
            }
        }
        /// <summary>
        /// 逾期未年检分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabWnj_Click(object sender, EventArgs e)
        {
            try
            {
                TabCllx.ForeColor = Color.Black;
                TabCllx.Font.Bold = false;
                TabHpzl.ForeColor = Color.Black;
                TabHpzl.Font.Bold = false;
                TabSyxz.ForeColor = Color.Black;
                TabSyxz.Font.Bold = false;
                TabSyq.ForeColor = Color.Black;
                TabSyq.Font.Bold = false;
                TabWnj.ForeColor = Color.Blue;
                TabWnj.Font.Bold = true;
                IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/OverdueCar.jsp");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("VehicleAnalysisMain.aspx-TabWnj_Click", ex.Message + "；" + ex.StackTrace, "TabWnj_Click发生异常");
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
                logManager.InsertLogError("VehicleAnalysisMain.aspx-PathUrl", ex.Message + "；" + ex.StackTrace, "PathUrl发生异常");
                throw;
            }
        }
    }
}