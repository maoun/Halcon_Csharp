using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.Web.Peccancy
{
    public partial class PeccancySettingMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TabPeccancyTypeSetting.Text = "卡口违法行为配置";
                TabTmsStationManager.Text = "违法上传配置";
                TabPeccancyType.Text = "违法类型管理";
                TabTmsUserLocation.Text = "审核授权管理";
                TabTmsCheckLess.Text = "白名单管理";
                TabPeccancyTypeSetting_Click(null, null);
            }
        }
        /// <summary>
        /// 卡口违法行为配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabPeccancyTypeSetting_Click(object sender, EventArgs e)
        {
            TabPeccancyTypeSetting.ForeColor = Color.Blue;
            TabPeccancyTypeSetting.Font.Bold = true;
            TabTmsStationManager.ForeColor = Color.Black;
            TabTmsStationManager.Font.Bold = false;
            TabPeccancyType.ForeColor = Color.Black;
            TabPeccancyType.Font.Bold = false;
            TabTmsUserLocation.ForeColor = Color.Black;
            TabTmsUserLocation.Font.Bold = false;
            TabTmsCheckLess.ForeColor = Color.Black;
            TabTmsCheckLess.Font.Bold = false;
            IFRAME1.Attributes.Add("src", "../Peccancy/PeccancyTypeSetting.aspx");
        }
        /// <summary>
        /// 违法上传配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabTmsStationManager_Click(object sender, EventArgs e)
        {
            TabPeccancyTypeSetting.ForeColor = Color.Black;
            TabPeccancyTypeSetting.Font.Bold = false;
            TabTmsStationManager.ForeColor = Color.Blue;
            TabTmsStationManager.Font.Bold = true;
            TabPeccancyType.ForeColor = Color.Black;
            TabPeccancyType.Font.Bold = false;
            TabTmsUserLocation.ForeColor = Color.Black;
            TabTmsUserLocation.Font.Bold = false;
            TabTmsCheckLess.ForeColor = Color.Black;
            TabTmsCheckLess.Font.Bold = false;
            IFRAME1.Attributes.Add("src", "../Peccancy/LocationUpload.aspx");
        }
        /// <summary>
        /// 违法类型管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabPeccancyType_Click(object sender, EventArgs e)
        {
            TabPeccancyTypeSetting.ForeColor = Color.Black;
            TabPeccancyTypeSetting.Font.Bold = false;
            TabTmsStationManager.ForeColor = Color.Black;
            TabTmsStationManager.Font.Bold = false;
            TabPeccancyType.ForeColor = Color.Blue;
            TabPeccancyType.Font.Bold = true;
            TabTmsUserLocation.ForeColor = Color.Black;
            TabTmsUserLocation.Font.Bold = false;
            TabTmsCheckLess.ForeColor = Color.Black;
            TabTmsCheckLess.Font.Bold = false;
            IFRAME1.Attributes.Add("src", "../Peccancy/PeccancyType.aspx");
        }
        /// <summary>
        /// 审核授权管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabTmsUserLocation_Click(object sender, EventArgs e)
        {
            TabPeccancyTypeSetting.ForeColor = Color.Black;
            TabPeccancyTypeSetting.Font.Bold = false;
            TabTmsStationManager.ForeColor = Color.Black;
            TabTmsStationManager.Font.Bold = false;
            TabPeccancyType.ForeColor = Color.Black;
            TabPeccancyType.Font.Bold = false;
            TabTmsUserLocation.ForeColor = Color.Blue;
            TabTmsUserLocation.Font.Bold = true;
            TabTmsCheckLess.ForeColor = Color.Black;
            TabTmsCheckLess.Font.Bold = false;
            IFRAME1.Attributes.Add("src", "../Peccancy/TmsUserLocation.aspx");
        }

     
        /// <summary>
        /// 白名单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabTmsCheckLess_Click(object sender, EventArgs e)
        {
            TabPeccancyTypeSetting.ForeColor = Color.Black;
            TabPeccancyTypeSetting.Font.Bold = false;
            TabTmsStationManager.ForeColor = Color.Black;
            TabTmsStationManager.Font.Bold = false;
            TabPeccancyType.ForeColor = Color.Black;
            TabPeccancyType.Font.Bold = false;
            TabTmsUserLocation.ForeColor = Color.Black;
            TabTmsUserLocation.Font.Bold = false;
            TabTmsCheckLess.ForeColor = Color.Blue;
            TabTmsCheckLess.Font.Bold = true;
            IFRAME1.Attributes.Add("src", "../Suspicion/ChecklessManager.aspx");
        }
    }
}