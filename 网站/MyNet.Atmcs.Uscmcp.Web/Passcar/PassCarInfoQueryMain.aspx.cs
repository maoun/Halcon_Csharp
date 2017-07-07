using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PassCarInfoQueryMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TabInfo.Text = "列表展示";
                TabImg.Text = "图片展示";
                TabInfo_Click(null, null);
            }
        }
        protected void TabInfo_Click(object sender, EventArgs e)
        {
            TabInfo.ForeColor = Color.Blue;
            TabInfo.Font.Bold = true;
            TabImg.ForeColor = Color.Black;
            TabImg.Font.Bold = false;
            IFRAME1.Attributes.Add("src", "../Passcar/PassCarInfoQuery.aspx");
        }

        protected void TabImg_Click(object sender, EventArgs e)
        {
            TabImg.ForeColor = Color.Blue;
            TabImg.Font.Bold = true;
            TabInfo.ForeColor = Color.Black;
            TabInfo.Font.Bold = false;
            IFRAME1.Attributes.Add("src", "../Passcar/PassCarInfoQueryImg.aspx");
        }
    }
}