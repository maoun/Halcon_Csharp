using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebPassCarType : System.Web.UI.Page
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
            CreateChartDivPie(datetime);
        }

        private void CreateChartDivPie(string datetime)
        {
            try
            {
                StringBuilder html = new StringBuilder();
                string name = "";
                DataTable dtPassCarCount = null;
                if (DateTime.Now.ToString("yyyy-MM-dd").Equals(datetime))
                {
                    dtPassCarCount = GetRedisData.GetData("PassCarCount:hpzl");
                }
                else if (DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd").Equals(datetime))
                {
                    dtPassCarCount = GetRedisData.GetData("PassCarCount:hpzl_last");
                }
                else
                {
                    dtPassCarCount = dataCountInfo.GetPassCarCountByType(datetime, "HPZL");
                }
                int zs = 0;
                List<decimal> bfbs = new List<decimal>();
                for (int j = 0; j < dtPassCarCount.Rows.Count; j++)
                {
                    zs = zs + Convert.ToInt32(dtPassCarCount.Rows[j][1]);
                }
                string sp = string.Empty;
                try
                {
                    for (int i = 0; i < dtPassCarCount.Rows.Count; i++)
                    {
                        decimal bfb = Math.Round(((decimal)Convert.ToInt32(dtPassCarCount.Rows[i][1]) / zs) * 100, 2);

                        sp = sp + "{ value: " + dtPassCarCount.Rows[i][1].ToString() + ", name: '" + dtPassCarCount.Rows[i][0].ToString() + " " + (bfb).ToString() + "%' },";
                    }
                }
                catch
                {
                }
                string data1 = " data: [" + sp + "]";

                html.AppendLine(CreatePieChart(GetLangStr("WebPassCarType1", "车辆号牌种类统计分析"), "main", name, data1));
                this.ResourceManager1.RegisterAfterClientInitScript(html.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebPassCarType.aspx-CreateChartDivPie", ex.Message+"；"+ex.StackTrace, "CreateChartDivPie has an exception");
               
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
                js.AppendLine("                    radius: [60, 110],");
                js.AppendLine("                    center: ['50%', 200],");
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
                logManager.InsertLogError("WebPassCarType.aspx-CreateChartDivPie", ex.Message+"；"+ex.StackTrace, "CreateChartDivPie has an exception");
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