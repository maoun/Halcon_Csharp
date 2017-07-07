using System;
using System.Data;
using System.Text;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebDevicePieData1 : System.Web.UI.Page
    {
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        private DataCountInfo dataCountInfo = new DataCountInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            string datetime = Request.QueryString["datetime"];
            if (!string.IsNullOrEmpty(datetime))
            {
                CreateChartDivPie2(datetime);
            }
            else
            {
                CreateChartDivPie2(DateTime.Now.ToString("yyyy-MM-dd"));
            }
        }

        private void CreateChartDivPie2(string datetime)
        {
            try
            {
                StringBuilder html = new StringBuilder();
                string title = GetLangStr("WebDevicePieData11", "厂家维度分析统计");

                DataTable dtCount = GetRedisData.GetData("DeviceDataCount:CJ");  //dataCountInfo.GetDeviceInfoByType(datetime, "CJ");

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
                html.AppendLine(CreatePieChart2("main", title, GetLangStr("WebDevicePieData12", "设备厂家"), data1));
                this.ResourceManager1.RegisterAfterClientInitScript(html.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebDevicePieData1.aspx-CreateChartDivPie2", ex.Message+"；"+ex.StackTrace, "CreateChartDivPie2 has an exception");
            }
        }

        private string CreatePieChart2(string divId, string title, string name, string data2)
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
                js.AppendLine("         var myChart2 = ec.init(document.getElementById('" + divId + "'));");
                js.AppendLine("         var i = 0, j = 0");
                js.AppendLine("         var colors = ['#2fc8ca', '#b7a3df', '#d97b81', '#6798f3', '#8cae3e', '#f5b989']");
                js.AppendLine("         var option2 = {");
                js.AppendLine("            tooltip: {");
                js.AppendLine("            trigger: 'item',");
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
                js.AppendLine("                    radius: [100, 140],");
                js.AppendLine("                    x: '75%',       ");
                js.AppendLine("                    width: '35%',");
                js.AppendLine("                    funnelAlign: 'left',");
                js.AppendLine("                    max: 1048,            ");
                js.AppendLine("                    itemStyle : {");
                js.AppendLine("                    normal : {");
                js.AppendLine("                    color:function(){return colors[i++];},");
                js.AppendLine("                   	}");
                js.AppendLine("                   },");
                js.AppendLine(data2);
                js.AppendLine("                }");
                js.AppendLine("                 ]");
                js.AppendLine("              };");
                js.AppendLine("             myChart2.setOption(option2 );");
                js.AppendLine("             }");
                js.AppendLine("              );");
                return js.ToString();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebDevicePieData1.aspx-CreatePieChart2", ex.Message+"；"+ex.StackTrace, "CreatePieChart2 has an exception");
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