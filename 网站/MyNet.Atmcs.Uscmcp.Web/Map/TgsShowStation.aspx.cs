using System;
using System.Data;
using System.Text;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using System.Collections.Generic;
using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Atmcs.Uscmcp.Model;
namespace MyNet.Atmcs.Uscmcp.Web.Map
{
    public partial class TgsShowStation : System.Web.UI.Page
    {
        #region 成员变量
        private MapManager dll = new MapManager();
        public delegate DataTable GetFlowDelegate(string directione, string date);
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        #endregion
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = "", datetime = "", type = "";
            if (!X.IsAjaxRequest)
            {
                if (Request != null)
                {
                    id = Request.QueryString["id"];
                    datetime = Request.QueryString["datetime"];
                    type = Request.QueryString["type"];
                    Session["id"] = id;
                    Session["datetime"] = datetime;
                    Session["type"] = type;
                }
                else
                {
                    datetime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                if (!string.IsNullOrEmpty(datetime))
                {
                    if (type == "station")
                        GetChartLine(datetime);
                    else
                    {
                        this.Button1.Visible = false;
                        GetChartLineRoad(datetime);
                    }
                }
                else
                {
                    if (type == "station")
                        GetChartLine(DateTime.Now.ToString("yyyy-MM-dd"));
                    else
                    {
                        this.Button1.Visible = false;
                        GetChartLineRoad(DateTime.Now.ToString("yyyy-MM-dd"));
                    }
                }
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "访问：用户登录", userinfo.NowIp, "0");
            }
        }
        protected void showdev(object sender, EventArgs e)
        {
            winstation.Show();
            string kkid = Session["id"] as string;
            string time = Session["datetime"] as string;
            DataTable devdt = Bll.Common.ChangColName(dll.GetWorkStatic(kkid, time));
            if (devdt != null && devdt.Rows.Count > 0)
            {
                this.StoreDevice.DataSource = devdt;
                this.StoreDevice.DataBind();
                this.LblDevice.Text = "监测点名称：" + devdt.Rows[0]["col1"].ToString() + "";
            }
        }
     
        /// <summary>
        /// 绘制图标
        /// </summary>
        /// <param name="datetime"></param>
        private void GetChartLine(string datetime)
        {
            try
            {
                DataTable dtPassCar = null;
                LblFlow.Text = "";
                if (DateTime.Now.ToString("yyyy-MM-dd").Equals(datetime))
                {
                    dtPassCar = GetRedisData.GetData("PassCarCountStation:" + Session["id"] as string);
                }
                else if (DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd").Equals(datetime))
                {
                    dtPassCar = GetRedisData.GetData("PassCarCountStationLast:" + Session["id"] as string);
                }
                else
                {
                    dtPassCar = dll.GetFlowByStation(Session["id"].ToString(), datetime);
                }
                if (dtPassCar != null)
                {
                    CreateChartDiv(GetFlowStr(dtPassCar), datetime);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TgsShowStation.aspx-GetChartLine", ex.Message+"；"+ex.StackTrace, "GetChartLine has an exception");
            }
        }

        private void GetChartLineRoad(string datetime)
        {
            try
            {
                LblFlow.Text = "车流总量：" + dll.GetFlowByRoadCount(Session["id"].ToString(), datetime);
                DataTable dtPassCar = null, dtPassAvg = null;
                if (DateTime.Now.ToString("yyyy-MM-dd").Equals(datetime))
                {
                    dtPassCar = GetRedisData.GetData("PassCarCountRoad:" + Session["id"] as string);
                    dtPassAvg = GetRedisData.GetData("PassCarCountRoadAvg:" + Session["id"] as string);
                }
                else if (DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd").Equals(datetime))
                {
                    dtPassCar = GetRedisData.GetData("PassCarCountRoadLast:" + Session["id"] as string);
                    dtPassAvg = GetRedisData.GetData("PassCarCountRoadAvgLast:" + Session["id"] as string);
                }
                else
                {
                    dtPassCar = dll.GetFlowByRoad(Session["id"].ToString(), datetime);
                    dtPassAvg = dll.GetFlowByRoadAvg(Session["id"].ToString(), datetime);
                }
                if (dtPassCar != null)
                {
                    CreateChartDiv(GetFlowRoad(dtPassCar,dtPassAvg), datetime);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TgsShowStation.aspx-GetChartLineRoad", ex.Message+"；"+ex.StackTrace, "GetChartLineRoad has an exception");
            }
        }
        private List<string> GetFlowRoad(DataTable dt,DataTable dtavg)
        {
            
            string rq = "", sp = "";
            List<string> lt = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["rq"].ToString() != rq)
                {
                    rq = dt.Rows[i]["rq"].ToString();
                    if (sp != "")
                        lt.Add(sp);
                    sp = (rq=="avg"?"均值":rq.Substring(0,10)) + ":" + dt.Rows[i]["LL"].ToString() + ",";
                }
                else
                {
                    sp = sp + dt.Rows[i]["LL"].ToString() + ",";
                }
            }
            for (int i = 0; i < dtavg.Rows.Count; i++)
            {
                if (dtavg.Rows[i]["rq"].ToString() != rq)
                {
                    rq = dtavg.Rows[i]["rq"].ToString();
                    if (sp != "")
                        lt.Add(sp);
                    sp = (rq == "avg" ? "均值" : rq.Substring(0, 10)) + ":" + dtavg.Rows[i]["LL"].ToString() + ",";
                }
                else
                {
                    sp = sp + dtavg.Rows[i]["LL"].ToString() + ",";
                }
            }
            if (sp != "")
                lt.Add(sp);
            return lt;
        }
        private List<string> GetFlowStr(DataTable dt)
        {
            string fx = "", sp = "";
            List<string> lt = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["direction_desc"].ToString() != fx)
                {
                    fx = dt.Rows[i]["direction_desc"].ToString();
                    if (sp != "")
                        lt.Add(sp);
                    sp = fx+":"+dt.Rows[i]["LL"].ToString() + ",";
                }
                else
                {
                    sp = sp + dt.Rows[i]["LL"].ToString() + ",";
                }
            }
            if (sp != "")
                lt.Add(sp);
            return lt;
        }
      
        /// <summary>
        /// 组装图表并填充至div
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <param name="datetime"></param>
        private void CreateChartDiv(List<string> flowstr, string datetime)
        {
            try
            {
                StringBuilder html = new StringBuilder();
                string legend = "data: [";//'实际在线','通过车次']";
                foreach (string data in flowstr)
                {
                    legend += "'" + data.Split(':')[0] + "',";
                }
                legend = legend.Substring(0, legend.Length - 1) + "]";
                string xAxisData = " data: [ '0时','1时', '2时', '3时', '4时', '5时', '6时', '7时', '8时', '9时', '10时', '11时', '12时', '13时', '14时', '15时', '16时','17时', '18时', '19时', '20时', '21时', '22时', '23时']";
                html.AppendLine(CreateChart(DateTime.Parse(datetime).ToLongDateString().ToString() + "   24小时过车流量分析", "main", legend, xAxisData,flowstr));
                this.ResourceManager1.RegisterAfterClientInitScript(html.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TgsShowStation.aspx-CreateChartDiv", ex.Message+"；"+ex.StackTrace, "CreateChartDiv has an exception");
            }
        }

        private string CreateChart(string title, string divId, string legend, string xAxisData, List<string> datalist)
        {
            try
            {
                StringBuilder js = new StringBuilder();
                js.AppendLine("require.config({");
                js.AppendLine("  paths: {");
                js.AppendLine("echarts: '../build/dist'");
                js.AppendLine("    }");
                js.AppendLine("    });");
                js.AppendLine("   require(");
                js.AppendLine("   [");
                js.AppendLine("        'echarts',");
                js.AppendLine("         'echarts/chart/line',");
                js.AppendLine("         'echarts/chart/bar'");
                js.AppendLine("     ],");
                js.AppendLine("      function (ec) {");
                js.AppendLine("         var myChart = ec.init(document.getElementById('" + divId + "'));");
                js.AppendLine("           var option = {");
                js.AppendLine("              tooltip: {");
                js.AppendLine("                trigger: 'axis',");
                js.AppendLine("                axisPointer:{");
                js.AppendLine("                      type: 'line',");
                js.AppendLine("                      lineStyle: { color: '#48b', width: 4, type: 'solid' }");
                js.AppendLine("                }");
                js.AppendLine("              },");
                js.AppendLine("            toolbox: {");
                js.AppendLine("                             show : true,");
                js.AppendLine("                              feature : {magicType : {show: true, type: ['line', 'bar', 'stack', 'tiled']},");
                js.AppendLine("                                               restore : {show: true},");
                js.AppendLine("                                               saveAsImage : {show: true,type :'jpeg'}");
                js.AppendLine("                                               }");
                js.AppendLine("                              },");
                js.AppendLine("                title : {");
                js.AppendLine("                text: '" + title + "',");
                js.AppendLine("                x:'center'");
                js.AppendLine("                      },");
                js.AppendLine("                legend: {");
                js.AppendLine("                x :'left',");
                js.AppendLine(legend);
                js.AppendLine("              },");
                js.AppendLine("               calculable: false,");
                js.AppendLine("              xAxis: [");
                js.AppendLine("                   {");
                js.AppendLine("                      type: 'category',");
                js.AppendLine("               boundaryGap: false,");
                js.AppendLine(xAxisData);
                js.AppendLine("                    }");
                js.AppendLine("                ],");
                js.AppendLine("                yAxis: [");
                js.AppendLine("                    {");
                js.AppendLine("                      type: 'value'");
                js.AppendLine("                    }");
                js.AppendLine("              ],");
                js.AppendLine("                series: [");
                string data = "";
                for (int i = 0; i < datalist.Count;i++ )
                {
                    if(i>0)
                        js.AppendLine("                 ,");
                    data = datalist[i];
                    js.AppendLine("                  {");
                    js.AppendLine("                      name: '" + data.Split(':')[0] + "',");
                    js.AppendLine("                      type: 'line',");
                    js.AppendLine("                      smooth: true,");
                    js.AppendLine("                      itemStyle: { normal: { areaStyle: { type: 'default' }, lineStyle: { borderColor: 'red', color: 'red', type: 'type', width: '1' }, label: { show: true, position: 'top', textStyle: { color: 'red' } } } },");
                    js.AppendLine("data: ["+data.Split(':')[1]+"]");
                    js.AppendLine("                 }");
                }
                js.AppendLine("               ]");
                js.AppendLine("           };");
                js.AppendLine("                  myChart.setOption(option);");
                js.AppendLine("                  }");
                js.AppendLine("               );");
                return js.ToString();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TgsShowStation.aspx-CreateChart", ex.Message+"；"+ex.StackTrace, "CreateChart has an exception");
                return "";
            }
        }
   
    }
}