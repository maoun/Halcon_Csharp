using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.Web.Station
{
    public partial class RoadMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TabRoadManager.Text =GetLangStr("RoadMain1", "道路管理");
                TabKeyRoad.Text = GetLangStr("RoadMain2", "重点道路管理");
                TabRoadManager_Click(null, null);
                this.DataBind();
            }
        }
        /// <summary>
        /// 道路管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabRoadManager_Click(object sender, EventArgs e)
        {
            TabRoadManager.ForeColor = Color.Blue;
            TabRoadManager.Font.Bold = true;
            TabKeyRoad.ForeColor = Color.Black;
            TabKeyRoad.Font.Bold = false;

            IFRAME1.Attributes.Add("src", "../Map/GisRoadBrowse.aspx");
        }
        /// <summary>
        /// 重点道路管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabKeyRoad_Click(object sender, EventArgs e)
        {
            TabRoadManager.ForeColor = Color.Black;
            TabRoadManager.Font.Bold = false;
            TabKeyRoad.ForeColor = Color.Blue;
            TabKeyRoad.Font.Bold = true;
            IFRAME1.Attributes.Add("src", "../Station/KeyRoadManager.aspx");
        }

        /// <summary>
        /// 多语言转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public string GetLangStr(string value, string desc)
        {
            string className = this.GetType().BaseType.FullName;
            return MyNet.Common.Lang.Language.CreateInstance(className).GetLanguageStr(value, desc, className);
        }
    }
}