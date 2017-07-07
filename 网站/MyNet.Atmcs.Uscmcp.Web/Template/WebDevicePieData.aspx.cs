using System;
using System.Data;
using System.Text;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebDevicePieData : System.Web.UI.Page
    {
        private DataCountInfo dataCountInfo = new DataCountInfo();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
 
        protected void Page_Load(object sender, EventArgs e)
        {
            string datetime = Request.QueryString["datetime"];
            if (!string.IsNullOrEmpty(datetime))
            {
                CreateChartDivPie(datetime);
            }
            else
            {
                CreateChartDivPie(DateTime.Now.ToString("yyyy-MM-dd"));
            }
        }

        private void CreateChartDivPie(string datetime)
        {
            try
            {
                StringBuilder html = new StringBuilder();
                string title = GetLangStr("WebDevicePieData1", "设备辖区维度分析统计");

                DataTable dtCount = GetRedisData.GetData("DeviceDataCount:CJJG"); //dataCountInfo.GetDeviceInfoByType(datetime, "CJJG");

                string sp = string.Empty;
                for (int i = 0; i < dtCount.Rows.Count; i++)
                {
                    sp = sp + "{ value: " + dtCount.Rows[i][1].ToString() + ", name: '" + dtCount.Rows[i][0].ToString() + "' },";
                }
                string data1;
                if (string.IsNullOrEmpty(sp))
                {
                    data1 = " data: []";
                }
                else
                {
                    data1 = " data: [" + sp + "]";
                   
                }


                html.AppendLine(CreatePieChart("main", title, GetLangStr("WebDevicePieData2", "设备辖区"), data1));
                this.ResourceManager1.RegisterAfterClientInitScript(html.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebDevicePieData.aspx-CreateChartDivPie", ex.Message+"；"+ex.StackTrace, "CreateChartDivPie has an exception");
            }
        }

        private string CreatePieChart(string divId, string title, string name, string data1)
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
                js.AppendLine("          var option1 = {");
                js.AppendLine("            tooltip: {");
                js.AppendLine("               trigger: 'item',");
                js.AppendLine("              formatter: '{a} <br/>{b} : {c} ({d}%)'");
                js.AppendLine("            },");
                js.AppendLine("          title : {");
                js.AppendLine("          text: '" + title + "',");
                js.AppendLine("          x:'center',textStyle: {color:'#FC5004'}");
                js.AppendLine("          },");
                js.AppendLine("            calculable: false,");
                js.AppendLine("            series: [");
                js.AppendLine("                {");
                js.AppendLine("                    name: '" + name + "',");
                js.AppendLine("                    type: 'pie',");
                js.AppendLine("                    radius: [60, 130],");
                js.AppendLine("                    center: ['50%', 200],");
                // js.AppendLine("                    roseType: 'area',");
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
                logManager.InsertLogError("WebDevicePieData.aspx-CreatePieChart", ex.Message+"；"+ex.StackTrace, "CreatePieChart has an exception");
                return "";
            }
        }

        #region 多语言转换

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

        #endregion 多语言转换
    }
}