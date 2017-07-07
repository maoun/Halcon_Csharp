using System;
using System.Drawing;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class SystemMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TabConfig.Text = "系统参数管理";
                TabNotice.Text = "政府公告管理";
             
                TabConfig_Click(null, null);
            }
        }

        /// <summary>
        /// 系统参数管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabConfig_Click(object sender, EventArgs e)
        {
            TabConfig.ForeColor = Color.Blue;
            TabConfig.Font.Bold = true;
            TabNotice.ForeColor = Color.Black;
            TabNotice.Font.Bold = false;

            IFRAME1.Attributes.Add("src", "../System/SystemConfig.aspx");
        }

        /// <summary>
        /// 政府公告管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabNotice_Click(object sender, EventArgs e)
        {
            TabConfig.ForeColor = Color.Black;
            TabConfig.Font.Bold = false;
            TabNotice.ForeColor = Color.Blue;
            TabNotice.Font.Bold = true;

            IFRAME1.Attributes.Add("src", "../System/NoticePicManager.aspx");
        }
    }
}