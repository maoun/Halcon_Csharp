using System;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebPassCarHot : System.Web.UI.Page
    {
        private DataCountInfo dataCountInfo = new DataCountInfo();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                string datetime = Request.QueryString["datetime"];
                string type = Request.QueryString["type"];
                if (!string.IsNullOrEmpty(datetime))
                {
                    HotWindowShow(datetime, type);
                }
                else
                {
                    HotWindowShow(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), "4");
                }

            }
        }

        public void HotWindowShow(string datetime, string type)
        {
            switch (type)
            {
                case "1":

                    DataTable dtPassHot = dataCountInfo.GetPassCarCountByType(datetime, "BDFZJGHOT");
                    ShowStation(dtPassHot);
                    break;

                case "2":

                    DataTable dtPassHot2 = dataCountInfo.GetPassCarCountByType(datetime, "BSFZJGHOT");
                    ShowStation(dtPassHot2);
                    break;

                case "3":

                    DataTable dtPassHot3 = dataCountInfo.GetPassCarCountByType(datetime, "WDFZJGHOT");
                    ShowStation(dtPassHot3);

                    break;

                case "4":
                    DataTable dtPassHot4 = dataCountInfo.GetPassCarCountByType(datetime, "HOT300108");
                    ShowStation(dtPassHot4);
                    break;

                case "5":
                    DataTable dtPassHot5 = dataCountInfo.GetPassCarCountByType(datetime, "HOT300109");
                    ShowStation(dtPassHot5);
                    break;

                case "6":
                    DataTable dtPassHot6 = dataCountInfo.GetPassCarCountByType(datetime, "HOT300104");
                    ShowStation(dtPassHot6);
                    break;

                case "7":
                    DataTable dtPassHot7 = dataCountInfo.GetPassCarCountByType(datetime, "HOT300102");
                    ShowStation(dtPassHot7);
                    break;

                case "8":
                    DataTable dtPassHot8 = dataCountInfo.GetPassCarCountByType(datetime, "HOT300107");
                    ShowStation(dtPassHot8);
                    break;
            }
        }

        /// <summary>
        /// 显示热点
        /// </summary>
        /// <param name="dsstation"></param>
        public void ShowStation(DataTable dsstation)
        {
            string points = "";
            string js = "BMAP.ClearLine();;";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
            if (dsstation != null)
            {
                for (int row = 0; row < dsstation.Rows.Count; row++)
                {
                    if (dsstation.Rows[row]["xpoint"].ToString() != null && dsstation.Rows[row]["xpoint"].ToString() != "")
                    {
                        if (points != "")
                            points += ",";
                        try
                        {
                            points += "{\"lng\":" + dsstation.Rows[row]["xpoint"].ToString() + ", \"lat\":" + dsstation.Rows[row]["ypoint"].ToString() + ", \"count\":" + dsstation.Rows[row]["zs"].ToString() + "}";
                        }
                        catch
                        { }
                    }
                }
                if (points != "")
                {
                    js = "BMAP.OpenHeatmap([" + points + "]);";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
        }
    }
}