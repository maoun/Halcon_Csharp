using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.Web.Station
{
    public partial class StationMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TabStationManager.Text = "监测点管理";
                TabStationMark.Text = "监测点标注";
                TabStationSpeed.Text = "卡口限速设置";
                TabStationManager_Click(null, null);
            }
        }
        /// <summary>
        /// 监测点管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabStationManager_Click(object sender, EventArgs e)
        {
            TabStationManager.ForeColor = Color.Blue;
            TabStationManager.Font.Bold = true;
            TabStationMark.ForeColor = Color.Black;
            TabStationMark.Font.Bold = false;
            TabStationSpeed.ForeColor = Color.Black;
            TabStationSpeed.Font.Bold = false;
            IFRAME1.Attributes.Add("src", "../Station/StationManager.aspx");
        }
        /// <summary>
        /// 监测点标注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabStationMark_Click(object sender, EventArgs e)
        {
            TabStationManager.ForeColor = Color.Black;
            TabStationManager.Font.Bold = false;
            TabStationMark.ForeColor = Color.Blue;
            TabStationMark.Font.Bold = true;
            TabStationSpeed.ForeColor = Color.Black;
            TabStationSpeed.Font.Bold = false;
            IFRAME1.Attributes.Add("src", "../Station/MarkerManager.aspx");
        }
        /// <summary>
        /// 卡口限速设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabStationSpeed_Click(object sender, EventArgs e)
        {
            TabStationManager.ForeColor = Color.Black;
            TabStationManager.Font.Bold = false;
            TabStationMark.ForeColor = Color.Black;
            TabStationMark.Font.Bold = false;
            TabStationSpeed.ForeColor = Color.Blue;
            TabStationSpeed.Font.Bold = true;
            IFRAME1.Attributes.Add("src", "../Station/TGSLimitSpeed.aspx");
        }
    }
}