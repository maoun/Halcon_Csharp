using System;
using System.Drawing;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class ODAnalysisMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TabOD.Text = "OD分析";
                TabODConfig.Text = "OD配置";
                TabOD_Click(null, null);
            }
        }

        /// <summary>
        /// OD分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabOD_Click(object sender, EventArgs e)
        {
            TabOD.ForeColor = Color.Blue;
            TabOD.Font.Bold = true;
            TabODConfig.ForeColor = Color.Black;
            TabODConfig.Font.Bold = false;


            IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/gisjs/gisExhibition.jsp");
        }

        /// <summary>
        /// OD配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabODConfig_Click(object sender, EventArgs e)
        {
            TabOD.ForeColor = Color.Black;
            TabOD.Font.Bold = false;
            TabODConfig.ForeColor = Color.Blue;
            TabODConfig.Font.Bold = true;

            IFRAME1.Attributes.Add("src", PathUrl() + "pages/collect/map/example.jsp");
        }


        //得到配置文件中的路径
        public string PathUrl()
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
    }
}