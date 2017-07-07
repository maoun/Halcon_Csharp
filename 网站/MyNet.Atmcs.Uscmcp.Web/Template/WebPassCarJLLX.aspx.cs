using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebPassCarJLLX : System.Web.UI.Page
    {
        private DataCountInfo dataCountInfo = new DataCountInfo();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            string datetime = Request.QueryString["datetime"];
            if (!string.IsNullOrEmpty(datetime))
            {
                GetChartPie(datetime);
            }
            else
            {
                GetChartPie(DateTime.Now.ToString("yyyy-MM-dd"));
            }
            
        }

        private void GetChartPie(string datetime)
        {
            CreateChartDivPie3(datetime);
        }

        private void CreateChartDivPie3(string datetime)
        {
            try
            {
                int zs = int.Parse(dataCountInfo.GetAlarmCarCountDay(datetime));
                DataTable dt = dataCountInfo.GetAlarmCarCountByType(datetime, "BJLX");
                List<string> dataList = new List<string>();
                string spp = string.Empty;
                if (dt != null&&dt.Rows.Count>0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int sy = zs - int.Parse(dt.Rows[i][1].ToString());
                        double MyCount = double.Parse(dt.Rows[i][1].ToString()) / zs * 100;
                        double bfb = Math.Round(MyCount, 2);
                        string title = dt.Rows[i][0].ToString() + "(" + bfb.ToString() + "%)";
                        spp = spp + "'" + title + "',";
                        dataList.Add(" data:[{value:" + dt.Rows[i][1].ToString() + ", name:'"+GetLangStr("WebPassCarJLLX2","+title+") +"'},{value:" + sy + ",	name:'',itemStyle : placeHolderStyle}]");
                    }
                }
                if (spp.Length > 0)
                {
                    spp = spp.Substring(0, spp.Length - 1);
                }
                string legend = " data: [" + spp + "]";

                StringBuilder html = new StringBuilder();
                html.AppendLine(CreatePieChart3(GetLangStr("WebPassCarJLLX1", "报警类型统计分析"), "main", legend, dataList));
                this.ResourceManager1.RegisterAfterClientInitScript(html.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebPassCarJLLX.aspx-CreateChartDivPie3", ex.Message+"；"+ex.StackTrace, "CreateChartDivPie3 has an exception");
            }
        }

        private string CreatePieChart3(string title, string divId, string legend, List<string> dataList)
        {
            try
            {
                StringBuilder js = new StringBuilder();
                js.AppendLine("  require.config({");
                js.AppendLine("  paths: {");
                js.AppendLine("  echarts: '../build/dist'");
                js.AppendLine("    }");
                js.AppendLine("    });");
                js.AppendLine("   require(");
                js.AppendLine("   [");
                js.AppendLine("        'echarts',");
                js.AppendLine("         'echarts/chart/pie'");
                js.AppendLine("     ],");
                js.AppendLine("      function (ec) {");
                js.AppendLine("                  var myChart3 = ec.init(document.getElementById('" + divId + "'));");
                js.AppendLine("                  var dataStyle = {");
                js.AppendLine("                      normal: {");
                js.AppendLine("                          label: { show: false, },");
                js.AppendLine("                          labelLine: { show: false }");
                js.AppendLine("                      }");
                js.AppendLine("                  };");
                js.AppendLine("                  var placeHolderStyle = {");
                js.AppendLine("                      normal: {");
                js.AppendLine("                          color: '#F2F8FA',");
                js.AppendLine("                          label: { show: false },");
                js.AppendLine("                          labelLine: { show: false }");
                js.AppendLine("                      },");
                js.AppendLine("                      emphasis: {");
                js.AppendLine("                          color: 'rgba(0,0,0,0)'");
                js.AppendLine("                      }");
                js.AppendLine("                  };");
                js.AppendLine("                  option3 = {");
                js.AppendLine("                      tooltip: {");
                js.AppendLine("                          show: true,");
                js.AppendLine("                          formatter: '{a} <br/>{b} : {c} ({d}%)'");
                js.AppendLine("                      },");
                js.AppendLine("                      title : {");
                js.AppendLine("                      text: '" + title + "',");
                js.AppendLine("                      x:'center',textStyle: {color:'#FC5004'}");
                js.AppendLine("                      },");
                js.AppendLine("                      legend: {");
                js.AppendLine("                          orient: 'vertical',");
                js.AppendLine("                          x: document.getElementById('" + divId + "').offsetWidth -200,");
                js.AppendLine("                          y: 60,");
                js.AppendLine("                          itemGap: 15,");
                js.AppendLine("                          textStyle: {");
                js.AppendLine("                              color: 'auto',");
                js.AppendLine("                              fontSize: 15,");
                js.AppendLine("                              fontFamily: 'Arial',");
                js.AppendLine("                              fontWeight: 'bold'");
                js.AppendLine("                          },");
                js.AppendLine(legend);
                js.AppendLine("                      },");
                js.AppendLine("                      series: [");
                if (dataList.Count>0)
                {
                    //int kd = 25;
                    //kd = 130 / dataList.Count;
                    for (int i = 0; i < dataList.Count; i++)
                    {
                        int xs = i + 1;
                        int maxradius = 130;
                        int min = maxradius - xs * 25;
                        int max = maxradius - i * 25;

                        js.AppendLine("                          {");
                        js.AppendLine("                              name: '" + xs.ToString() + "',");
                        js.AppendLine("                              type: 'pie',");
                        js.AppendLine("                              clockWise: false,");
                        js.AppendLine("                              radius: [" + min + ", " + max + "],");
                        js.AppendLine("                              itemStyle: dataStyle,");
                        js.AppendLine(dataList[i]);
                        js.AppendLine("                          },");
                    }
                }
                else
                {
                    js.AppendLine("                          {");
                    js.AppendLine("                              name: '',");
                    js.AppendLine("                              type: 'pie',");
                    js.AppendLine("                              clockWise: false,");
                    js.AppendLine("                              radius: [75, 100],");
                    js.AppendLine("                              itemStyle: dataStyle,");
                    js.AppendLine("                              data:[] }");
                }
                js.AppendLine("                      ]");
                js.AppendLine("                  };");
                js.AppendLine("                  myChart3.setOption(option3);");
                js.AppendLine("              }");
                js.AppendLine("          );");

                return js.ToString();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebPassCarJLLX.aspx-CreatePieChart3", ex.Message+"；"+ex.StackTrace, "CreatePieChart3 has an exception");
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