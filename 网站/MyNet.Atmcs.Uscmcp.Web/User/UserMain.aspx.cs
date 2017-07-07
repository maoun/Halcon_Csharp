using System;
using System.Drawing;

namespace MyNet.Atmcs.Uscmcp.Web.User
{
    public partial class UserMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TabUser.Text = "用户信息管理";
                TabPriv.Text = "用户权限管理";
                TabRole.Text = "用户角色管理";

                TabUser_Click(null, null);
            }
        }

        /// <summary>
        /// 用户信息管理
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

            IFRAME1.Attributes.Add("src", "../User/TGSUserManager.aspx");
        }

        /// <summary>
        /// 用户权限管理
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

            IFRAME1.Attributes.Add("src", "../User/PrivManager.aspx");
        }

        /// <summary>
        /// 用户角色管理
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
            IFRAME1.Attributes.Add("src", "../User/RoleManager.aspx");
        }
    }
}