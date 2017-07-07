using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebPassCarAnalysis : System.Web.UI.Page
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
                    ChartWindowShow(datetime, type);
                }
                else
                {
                    ChartWindowShow(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), "4");
                }
            }
        }

        public void ChartWindowShow(string datetime, string type)
        {
            switch (type)
            {
                case "1":
                    CreatePie(datetime, "HPZL", "本地车辆按号牌种类统计分析");
                    break;

                case "2":
                    CreatePie(datetime, "FZJG", "本省车辆统计分析");
                    break;

                case "3":
                    CreatePie(datetime, "WDFZJG", "外省车辆统计分析");
                    break;

                case "4":
                    CreatePie(datetime, "HPZLTJ300108", "黑名单车辆按号牌种类统计分析");
                    break;

                case "5": CreatePie(datetime, "HPZLTJ300109", "初次入城按号牌种类统计分析");
                    break;

                case "6": CreatePie(datetime, "HPZLTJ300104", "套牌车辆按号牌种类统计分析");
                    break;

                case "7": CreatePie(datetime, "HPZLTJ300102", "未年审按号牌种类统计分析");
                    break;

                case "8": CreatePie(datetime, "HPZLTJ300107", "黄标车按号牌种类统计分析");
                    break;
            }
        }
        /// <summary>
        /// 创建图表
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public void CreatePie(string datetime, string type, string name)
        {
            try
            {
                DataTable dtPassCarCount = dataCountInfo.GetPassCarCountByType(datetime, type);
                CreateChartDivPie(dtPassCarCount, "main", datetime, name);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebPassCarAnalysis.aspx-CreatePie", ex.Message+"；"+ex.StackTrace, "CreatePie has an exception");
            }
        }

        private void CreateChartDivPie(DataTable dtPassCarCount, string divId, string datetime, string title)
        {
            try
            {
                StringBuilder html = new StringBuilder();
                if (dtPassCarCount != null)
                {
                    int zs = 0;
                    List<decimal> bfbs = new List<decimal>();
                    for (int j = 0; j < dtPassCarCount.Rows.Count; j++)
                    {
                        zs = zs + Convert.ToInt32(dtPassCarCount.Rows[j][1]);
                    }

                    string sp = string.Empty;
                    for (int i = 0; i < dtPassCarCount.Rows.Count; i++)
                    {
                        decimal bfb = Math.Round(((decimal)Convert.ToInt32(dtPassCarCount.Rows[i][1]) / zs) * 100, 2);

                        sp = sp + "{ value: " + dtPassCarCount.Rows[i][1].ToString() + ", name: '" + dtPassCarCount.Rows[i][0].ToString() + " " + (bfb).ToString() + "%' },";
                    }
                    string data1 = " data: [" + sp.Substring(0, sp.Length - 1) + "]";

                    html.AppendLine(CreatePieChart(title, divId, title, data1));
                    this.ResourceManager1.RegisterAfterClientInitScript(html.ToString());
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebPassCarAnalysis.aspx-CreateChartDivPie", ex.Message+"；"+ex.StackTrace, "CreateChartDivPie has an exception");
            }
        }

        private string CreatePieChart(string title, string divId, string name, string data1)
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
                js.AppendLine("         'echarts/chart/pie'");
                js.AppendLine("     ],");
                js.AppendLine("      function (ec) {");
                js.AppendLine("          var myChart1 = ec.init(document.getElementById('" + divId + "'));");
                js.AppendLine("          var i = 0;");
                js.AppendLine("          var colors = ['#40cdce', '#c2a3fe', '#48b2fc', '#ff9034', '#e1606b', '#698ad9', '#e6d903', '#b3ea5a'];");
                js.AppendLine("          var option1 = {");
                js.AppendLine("            tooltip: {");
                js.AppendLine("               trigger: 'item',");
                js.AppendLine("              formatter: '{a} <br/>{b} : {c} ({d}%)'");
                js.AppendLine("            },");
                js.AppendLine("             title : {");
                js.AppendLine("             text: '" + title + "',");
                js.AppendLine("             x:'center',textStyle: {color:'#FC5004'}");
                js.AppendLine("             },");
                js.AppendLine("            calculable: false,");
                js.AppendLine("            series: [");
                js.AppendLine("                {");
                js.AppendLine("                    name: '" + name + "',");
                js.AppendLine("                    type: 'pie',");
                js.AppendLine("                    radius: [100, 200],");
                js.AppendLine("                    center: ['50%', 300],");
                js.AppendLine("                    roseType: 'area',");
                js.AppendLine("                    x: '75%',       ");
                js.AppendLine("                    max: 50,            ");
                js.AppendLine("                    sort: 'ascending', ");
                js.AppendLine("                   itemStyle: {");
                js.AppendLine("                      normal: {");
                js.AppendLine("                           label: {");
                js.AppendLine("                              textStyle: {");
                js.AppendLine("                                    fontSize: 15,");
                js.AppendLine("                                    fontFamily: 'Arial'");
                js.AppendLine("                                },");
                js.AppendLine("                           },");
                js.AppendLine("                          labelLine: {");
                js.AppendLine("                              show: true,");
                js.AppendLine("                              lineStyle: {");
                js.AppendLine("                             }");
                js.AppendLine("                          }");
                js.AppendLine("                       }");
                js.AppendLine("                   },");
                js.AppendLine(data1);
                js.AppendLine("                }");
                js.AppendLine("                 ]");
                js.AppendLine("              };");
                js.AppendLine("             myChart1.setOption(option1);");
                js.AppendLine("             }");
                js.AppendLine("              );");
                return js.ToString();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebPassCarAnalysis.aspx-CreatePieChart", ex.Message+"；"+ex.StackTrace, "CreatePieChart has an exception");
                return "";
            }
        }
    }
}