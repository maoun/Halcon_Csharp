using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.Web.Station
{
    public partial class DeviceMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TabDeviceStatus.Text = "设备状态列表";
                TabDeviceManager.Text = "设备信息管理";
                TabDeviceOperation.Text = "设备运维管理";
                TabDeviceStatistics.Text = "设备信息统计";
                TabServerManager.Text = "服务器信息管理";
                TabDeviceStatus_Click(null, null);
            }
        }
        /// <summary>
        /// 设备状态列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabDeviceStatus_Click(object sender, EventArgs e)
        {
            TabDeviceStatus.ForeColor = Color.Blue;
            TabDeviceStatus.Font.Bold = true;
            TabDeviceManager.ForeColor = Color.Black;
            TabDeviceManager.Font.Bold = false;
            TabDeviceOperation.ForeColor = Color.Black;
            TabDeviceOperation.Font.Bold = false;
            TabDeviceStatistics.ForeColor = Color.Black;
            TabDeviceStatistics.Font.Bold = false;
            TabServerManager.ForeColor = Color.Black;
            TabServerManager.Font.Bold = false;
            IFRAME1.Attributes.Add("src", "../Station/DeviceStatus.aspx");
        }
        /// <summary>
        /// 设备信息管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabDeviceManager_Click(object sender, EventArgs e)
        {
            TabDeviceStatus.ForeColor = Color.Black;
            TabDeviceStatus.Font.Bold = false;
            TabDeviceManager.ForeColor = Color.Blue;
            TabDeviceManager.Font.Bold = true;
            TabDeviceOperation.ForeColor = Color.Black;
            TabDeviceOperation.Font.Bold = false;
            TabDeviceStatistics.ForeColor = Color.Black;
            TabDeviceStatistics.Font.Bold = false;
            TabServerManager.ForeColor = Color.Black;
            TabServerManager.Font.Bold = false;
            IFRAME1.Attributes.Add("src", "../Station/DeviceManager.aspx");
        }
        /// <summary>
        /// 设备运维管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabDeviceOperation_Click(object sender, EventArgs e)
        {
            TabDeviceStatus.ForeColor = Color.Black;
            TabDeviceStatus.Font.Bold = false;
            TabDeviceManager.ForeColor = Color.Black;
            TabDeviceManager.Font.Bold = false;
            TabDeviceOperation.ForeColor = Color.Blue;
            TabDeviceOperation.Font.Bold = true;
            TabDeviceStatistics.ForeColor = Color.Black;
            TabDeviceStatistics.Font.Bold = false;
            TabServerManager.ForeColor = Color.Black;
            TabServerManager.Font.Bold = false;
            IFRAME1.Attributes.Add("src", "../Station/DeviceOperation.aspx");
        }
        /// <summary>
        /// 设备信息统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabDeviceStatistics_Click(object sender, EventArgs e)
        {
            TabDeviceStatus.ForeColor = Color.Black;
            TabDeviceStatus.Font.Bold = false;
            TabDeviceManager.ForeColor = Color.Black;
            TabDeviceManager.Font.Bold = false;
            TabDeviceOperation.ForeColor = Color.Black;
            TabDeviceOperation.Font.Bold = false;
            TabDeviceStatistics.ForeColor = Color.Blue;
            TabDeviceStatistics.Font.Bold = true;
            TabServerManager.ForeColor = Color.Black;
            TabServerManager.Font.Bold = false;
            IFRAME1.Attributes.Add("src", "../Station/DeviceStatistics.aspx");
        }
        /// <summary>
        /// 服务器信息管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabServerManager_Click(object sender, EventArgs e)
        {
            TabDeviceStatus.ForeColor = Color.Black;
            TabDeviceStatus.Font.Bold = false;
            TabDeviceManager.ForeColor = Color.Black;
            TabDeviceManager.Font.Bold = false;
            TabDeviceOperation.ForeColor = Color.Black;
            TabDeviceOperation.Font.Bold = false;
            TabDeviceStatistics.ForeColor = Color.Black;
            TabDeviceStatistics.Font.Bold = false;
            TabServerManager.ForeColor = Color.Blue;
            TabServerManager.Font.Bold = true;
            IFRAME1.Attributes.Add("src", "../Station/ServerManager.aspx");
        }
    }
}