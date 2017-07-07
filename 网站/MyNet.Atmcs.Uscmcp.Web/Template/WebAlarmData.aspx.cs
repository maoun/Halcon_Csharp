using System;
using System.Data;
using System.Text;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebAlarmData : System.Web.UI.Page
    {
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private DataCountInfo dataCountInfo = new DataCountInfo();
        private static string time = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string datetime = Request.QueryString["datetime"];
            time = datetime;
            if (!string.IsNullOrEmpty(datetime))
            {
                GetChartLine(datetime);
            }
            else
            {
                GetChartLine(DateTime.Now.ToString("yyyy-MM-dd"));
            }
        }

        private void CreateChartDiv(string data1, string data2)
        {
            try
            {
                StringBuilder html = new StringBuilder();
                string legend = "data:['" + GetLangStr("WebAlarmData1", "历史平均") + "','" + GetLangStr("WebAlarmData2", "报警总计") + "']";
                string times = GetLangStr("WebAlarmData3", "时");
                string xAxisData = " data: [ '0" + times + "','1" + times + "', '2" + times + "', '3" + times + "', '4" + times + "', '5" + times + "', '6" + times + "', '7" + times + "', '8" + times + "', '9" + times + "', '10" + times + "', '11" + times + "', '12" + times + "', '13" + times + "', '14" + times + "', '15" + times + "', '16" + times + "','17" + times + "', '18" + times + "', '19" + times + "', '20" + times + "', '21" + times + "', '22" + times + "', '23" + times + "']";
              
                //DateTime.Parse(time).ToLongDateString().ToString()
                html.AppendLine(CreateChart(time.ToString() + "  " + GetLangStr("WebAlarmData4", "24小时通过车次与实际在线对比分析"), "main", legend, xAxisData, data1, data2));
                this.ResourceManager1.RegisterAfterClientInitScript(html.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        private void GetChartLine(string datetime)
        {
            try
            {
                DataTable dtAlarmCar = null;
                DataTable dtAlarmAvg = null;
                //查询当天的从Redis中查询
                if (DateTime.Now.ToString("yyyy-MM-dd").Equals(datetime))
                {
                    dtAlarmCar = GetRedisData.GetData("AlarmCount:AlarmATotalOf24HoursADay");
                    dtAlarmAvg = GetRedisData.GetData("AlarmCount:HistoricalAverageComparisonAndAnalysis");
                }
                else if (DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd").Equals(datetime))//昨天的从Redis中查询
                {
                    dtAlarmCar = GetRedisData.GetData("AlarmCount:AlarmATotalOf24HoursYesterday");
                    dtAlarmAvg = GetRedisData.GetData("AlarmCount:HistoricalAverageComparisonAndAnalysis");
                }
                else//查询非昨天和今天去数据库中查询
                {
                    dtAlarmCar = dataCountInfo.GetAlarmCarCountHour(datetime);
                    dtAlarmAvg = dataCountInfo.GetAlarmCarCountAvgDay(datetime, 5);
                }
                int zs = 0;
                string sp = string.Empty;
                if (dtAlarmCar != null)
                {
                    for (int i = 0; i < dtAlarmCar.Rows.Count; i++)
                    {
                        sp = sp + dtAlarmCar.Rows[i][1].ToString() + ",";
                        zs = zs + int.Parse(dtAlarmCar.Rows[i][1].ToString());
                    }
                }
                string data1 = string.Empty;
                if(string.IsNullOrEmpty(sp))
                {
                     data1 = " data: []";
                }
                else
                {
                     data1 = " data: [" + sp + "]";
                }
            
                
                string so = string.Empty;
                if (dtAlarmAvg != null)
                {
                    for (int i = 0; i < dtAlarmAvg.Rows.Count; i++)
                    {
                        so = so + dtAlarmAvg.Rows[i][1].ToString() + ",";
                    }
                }
                string data2 = string.Empty;
                if (string.IsNullOrEmpty(sp))
                {
                     data2 = " data: []";
                }
                else
                {
                     data2 = " data: [" + so + "]";
                }
               
                CreateChartDiv(data2, data1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmData.aspx-GetChartLine", ex.Message+"；"+ex.StackTrace, "GetChartLine has an exception");
            }
        }

        private string CreateChart(string title, string divId, string legend, string xAxisData, string data1, string data2)
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
                js.AppendLine("                x:'center',textStyle: {color:'#FC5004'}");
                js.AppendLine("                      },");
                js.AppendLine("                legend: {");
                js.AppendLine("                x :'left',");
                js.AppendLine(legend);
                js.AppendLine("             ,textStyle: {color:'black',fontWeight:'bold'} },");
                js.AppendLine("               calculable: false,");
                js.AppendLine("              xAxis: [");
                js.AppendLine("                   {");
                js.AppendLine("                      type: 'category',");
                js.AppendLine("                       boundaryGap: false,axisLabel: {textStyle: {color:'black'}},");
                js.AppendLine(xAxisData);
                js.AppendLine("                    }");
                js.AppendLine("                ],");
                js.AppendLine("                yAxis: [");
                js.AppendLine("                    {");
                js.AppendLine("                      type: 'value',axisLabel: {textStyle: {color:'black'}},");
                js.AppendLine("                    }");
                js.AppendLine("              ],");
                js.AppendLine("                series: [");
                js.AppendLine("                  {");
                js.AppendLine("                      name: '" + GetLangStr("WebAlarmData1", "历史平均") + "',"); 
                js.AppendLine("                      type: 'line',");
                js.AppendLine("                      smooth: true,");
                js.AppendLine("                      itemStyle: { normal: { areaStyle: { type: 'default' }, lineStyle: { borderColor: 'red', color: 'red', type: 'type', width: '1' }, label: { show: true, position: 'top', textStyle: { color: 'red' } } } },");
                js.AppendLine(data1);
                js.AppendLine("                 }");
                js.AppendLine("                 ,");
                js.AppendLine("                   {");
                js.AppendLine("                       name:'" + GetLangStr("WebAlarmData2", "报警总计") + "',");
                js.AppendLine("                       type: 'line',");
                js.AppendLine("                       smooth: true,");
                js.AppendLine("                       itemStyle: { normal: { areaStyle: {  type: 'default' }, lineStyle: { borderColor: 'blue', color: '#000', type: 'type', width: '1' }, label: { show: true, textStyle: { color: 'black' } } } },");
                js.AppendLine(data2);
                js.AppendLine("                    }");
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
                 logManager.InsertLogError("WebAlarmData.aspx-GetChartLine", ex.Message+"；"+ex.StackTrace, "GetChartLine has an exception");
                return "";
            }
        }
        #region 语言转换

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

        #endregion 语言转换
    }
}