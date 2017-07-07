using System;
using System.Drawing;

namespace MyNet.Atmcs.Uscmcp.Web.LogManager
{
    public partial class LogMain : System.Web.UI.Page
    {
        /// <summary>
        /// 功能模块名称（例如：综合查询 ）
        /// </summary>
        public static string dyzgnmkmc = "";

        /// <summary>
        /// 功能模块编号
        /// </summary>
        public static string dyzgnmkbh = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] fstrs = Request.QueryString["funcname"].Split('-');
            if (fstrs.Length > 0)
            {
                dyzgnmkmc = fstrs[1];
            }

            dyzgnmkbh = Request.QueryString["funcid"].ToString();
            if (!IsPostBack)
            {
                TabUser.Text = "操作日志查询";
                TabPriv.Text = "交互日志查询";
                TabRole.Text = "错误日志查询";
                TabUser_Click(null, null);
            }
        }

        /// <summary>
        /// 运行日志查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabUser_Click(object sender, EventArgs e)
        {
            TabUser.ForeColor = Color.Blue;
            TabUser.Font.Bold = true;
            TabPriv.ForeColor = Color.Black;
            TabPriv.Font.Bold = false;
            TabRole.ForeColor = Color.Black;
            TabRole.Font.Bold = false;
            IFRAME1.Attributes.Add("src", "../LogManager/LogRunning.aspx");
        }

        /// <summary>
        /// 业务日志查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabPriv_Click(object sender, EventArgs e)
        {
            TabUser.ForeColor = Color.Black;
            TabUser.Font.Bold = false;
            TabPriv.ForeColor = Color.Blue;
            TabPriv.Font.Bold = true;
            TabRole.ForeColor = Color.Black;
            TabRole.Font.Bold = false;

            IFRAME1.Attributes.Add("src", "../LogManager/LogBusiness.aspx");
        }

        /// <summary>
        /// 错误日志查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabRole_Click(object sender, EventArgs e)
        {
            TabUser.ForeColor = Color.Black;
            TabUser.Font.Bold = false;
            TabPriv.ForeColor = Color.Black;
            TabPriv.Font.Bold = false;
            TabRole.ForeColor = Color.Blue;
            TabRole.Font.Bold = true;
            IFRAME1.Attributes.Add("src", "../LogManager/LogError.aspx");
        }
    }
}