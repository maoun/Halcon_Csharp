using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using System;
using System.Data;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebFlowData : System.Web.UI.Page
    {
        #region 成员变量

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private DataCountInfo dataCountInfo = new DataCountInfo();
        private ILog logManger = new ILog();

        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                string datetime = Request.QueryString["datetime"];
                if (!string.IsNullOrEmpty(datetime))
                {
                    GetChartLine(datetime);
                }
                else
                {
                    GetChartLine(DateTime.Now.ToString("yyyy-MM-dd"));
                }
            }
        }

        #endregion 控件事件

        /// <summary>
        /// 绘制图标
        /// </summary>
        /// <param name="datetime"></param>
        private void GetChartLine(string datetime)
        {
            try
            {
                DataTable dtPassCar = null;
                DataTable dtOnlineCar = null;
                if (DateTime.Now.ToString("yyyy-MM-dd").Equals(datetime))
                {
                    dtPassCar = GetRedisData.GetData("PassCarCount:passcar_flow");
                    dtOnlineCar = GetRedisData.GetData("PassCarCount:passcar_online");
                }
                else if (DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd").Equals(datetime))
                {
                    dtPassCar = GetRedisData.GetData("PassCarCount:passcar_flow_last");
                    dtOnlineCar = GetRedisData.GetData("PassCarCount:passcar_online_last");
                }
                else
                {
                    dtPassCar = dataCountInfo.GetPassCarCountHour(datetime);
                    dtOnlineCar = dataCountInfo.GetOnlineCarCountHour(datetime);
                }
                if (dtPassCar == null)
                {
                }
                string sp = string.Empty;
                for (int i = 0; i < dtPassCar.Rows.Count; i++)
                {
                    sp = sp + dtPassCar.Rows[i][1].ToString() + ",";
                }
                string data1 = " data: [" + sp + "]";
                string so = string.Empty;
                for (int i = 0; i < dtOnlineCar.Rows.Count; i++)
                {
                    so = so + dtOnlineCar.Rows[i][1].ToString() + ",";
                }
                string data2 = " data: [" + so + "]";
                CreateChartDiv(data2, data1, datetime);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebFlowData.aspx-GetChartLine", ex.Message + "；" + ex.StackTrace, "GetChartLine has an exception");
            }
        }

        /// <summary>
        /// 组装图表并填充至div
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <param name="datetime"></param>
        private void CreateChartDiv(string data1, string data2, string datetime)
        {
            try
            {
                StringBuilder html = new StringBuilder();
                string legend = "data: ['" + GetLangStr("WebFlowData1", "实际在线") + "','" + GetLangStr("WebFlowData2", "通过车次") + "']";
                string time = GetLangStr("WebFlowData5", "时");

                string xAxisData = " data: [ '0" + time + "','1" + time + "', '2" + time + "', '3" + time + "', '4" + time + "', '5" + time + "', '6" + time + "', '7" + time + "', '8" + time + "', '9" + time + "', '10" + time + "', '11" + time + "', '12" + time + "', '13" + time + "', '14" + time + "', '15" + time + "', '16" + time + "','17" + time + "', '18" + time + "', '19" + time + "', '20" + time + "', '21" + time + "', '22" + time + "', '23" + time + "']";
                //CreateChart(DateTime.Parse(datetime).ToLongDateString().ToString()
                html.AppendLine(CreateChart(datetime.ToString()+"  "+ GetLangStr("WebFlowData3", "24小时通过车次与实际在线对比分析"), "main", legend, xAxisData, data1, data2));
                this.ResourceManager1.RegisterAfterClientInitScript(html.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebFlowData.aspx-CreateChartDiv", ex.Message + "；" + ex.StackTrace, "CreateChartDiv has an exception");
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
                js.AppendLine("              	,textStyle: {color:'black',fontWeight:'bold'}},");
                js.AppendLine("               calculable: false,");
                js.AppendLine("              xAxis: [");
                js.AppendLine("                   {");
                js.AppendLine("                      type: 'category',");
                js.AppendLine("               boundaryGap: false,axisLabel: {textStyle: {color:'black'}},");
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
                js.AppendLine("                      name: '" + GetLangStr("WebFlowData1", "实际在线") + "',");
                js.AppendLine("                      type: 'line',");
                js.AppendLine("                      smooth: true,");
                js.AppendLine("                      itemStyle: { normal: { areaStyle: { type: 'default' }, lineStyle: { borderColor: 'red', color: 'red', type: 'type', width: '1' }, label: { show: true, position: 'top', textStyle: { color: 'red' } } } },");
                js.AppendLine(data1);
                js.AppendLine("                 }");
                js.AppendLine("                 ,");
                js.AppendLine("                   {");
               
                js.AppendLine("                       name: '"+GetLangStr("WebFlowData2", "通过车次")+"',");
                js.AppendLine("                       type: 'line',");
                js.AppendLine("                       smooth: true,");
                js.AppendLine("                       itemStyle: { normal: { areaStyle: { type: 'default' }, lineStyle: { borderColor: 'blue', color: '#000', type: 'type', width: '1' }, label: { show: true, textStyle: { color: 'black' } } } },");
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
                logManager.InsertLogError("WebFlowData.aspx-CreateChart", ex.Message + "；" + ex.StackTrace, "CreateChart has an exception");
                ILog.WriteErrorLog(ex);
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