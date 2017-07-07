using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebAlarmPieData : System.Web.UI.Page
    {
        private DataCountInfo dataCountInfo = new DataCountInfo();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        protected void Page_Load(object sender, EventArgs e)
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

        private void CreateChartDivPie(string divId, string legend, List<string> dataList)
        {
            try
            {
                StringBuilder html = new StringBuilder();
                string name = GetLangStr("WebAlarmPieData1", "车辆报警类型对比");

                html.AppendLine(CreatePieChart(divId, name, legend, dataList));
                this.ResourceManager1.RegisterAfterClientInitScript(html.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmPieData.aspx-CreateChartDivPie", ex.Message+"；"+ex.StackTrace, "CreateChartDivPie has an exception");
            }
        }

        private void GetChartLine(string datetime)
        {
            try
            {
                DataTable dt = null;//存报警类型
                DataTable dtAlarmCar = null;
                //查询当天的从Redis中查询
                if (DateTime.Now.ToString("yyyy-MM-dd").Equals(datetime))
                {
                    dtAlarmCar = GetRedisData.GetData("AlarmCount:AlarmATotalOf24HoursADay");
                }
                else if (DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd").Equals(datetime))//昨天的从Redis中查询
                {
                    dtAlarmCar = GetRedisData.GetData("AlarmCount:AlarmATotalOf24HoursYesterday");
                }
                else//查询非昨天和今天去数据库中查询
                {
                    dtAlarmCar = dataCountInfo.GetAlarmCarCountHour(datetime);
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
                if (DateTime.Now.ToString("yyyy-MM-dd").Equals(datetime))
                {
                    dt = GetRedisData.GetData("AlarmCount:bjlx-today");
                }
                else if (DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd").Equals(datetime))
                {
                    dt = GetRedisData.GetData("AlarmCount:bjlx-yesterday");
                }
                else
                {
                    dt = dataCountInfo.GetAlarmCarCountByType(datetime, "BJLX");
                }
                List<string> dataList = new List<string>();
                string spp = string.Empty;
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int sy = zs - int.Parse(dt.Rows[i][1].ToString());
                        double MyCount = double.Parse(dt.Rows[i][1].ToString()) / (zs + 1) * 100;
                        double bfb = Math.Round(MyCount, 2);
                        string title = dt.Rows[i][0].ToString() + "(" + bfb.ToString() + "%)";
                        spp = spp + "'" + title + "',";
                        dataList.Add(" data:[{value:" + dt.Rows[i][1].ToString() + ", name:'" + title + "',itemStyle : labelTop},{value:" + sy + ",	name:'',itemStyle : labelBottom	}]");
                    }
                }
                if (spp.Length > 0)
                {
                    spp = spp.Substring(0, spp.Length - 1);
                }
                string legend;
                if(string.IsNullOrEmpty(spp))
                {
                     legend = " data: []";
                }
                else
                {
                     legend = " data: [" + spp + "]";

                }
     
                CreateChartDivPie("main", legend, dataList);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmPieData.aspx-GetChartLine", ex.Message+"；"+ex.StackTrace, "GetChartLine has an exception");

            }
        }

        private string CreatePieChart(string divId, string name, string legend, List<string> dataList)
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
                js.AppendLine("         'echarts',");
                js.AppendLine("         'echarts/chart/pie'");
                js.AppendLine("     ],");
                js.AppendLine("      function (ec) {");
                js.AppendLine("          var myChart3 = ec.init(document.getElementById('" + divId + "'));");
                js.AppendLine("          var labelTop = {");
                js.AppendLine("          normal : {");
                js.AppendLine("          label : {");
                js.AppendLine("          show : true,");
                js.AppendLine("          position : 'center',");
                js.AppendLine("          formatter : '{b}',");
                js.AppendLine("          textStyle: {");
                js.AppendLine("               baseline : 'bottom'");
                js.AppendLine("              }");
                js.AppendLine("            },");
                js.AppendLine("            labelLine : {");
                js.AppendLine("               show : false");
                js.AppendLine("           }");
                js.AppendLine("           }");
                js.AppendLine("         };");
                js.AppendLine("          var labelBottom = {");
                js.AppendLine("          normal : {");
                js.AppendLine("          color: '#F2F8FA',");
                js.AppendLine("          label : {");
                js.AppendLine("               show : true,");
                js.AppendLine("               position : 'center'");
                js.AppendLine("           },");
                js.AppendLine("           labelLine : {");
                js.AppendLine("                show : false");
                js.AppendLine("             }");
                js.AppendLine("           },");
                js.AppendLine("          emphasis: {");
                js.AppendLine("              color: 'rgba(0,0,0,0)'");
                js.AppendLine("          }");
                js.AppendLine("         };");
                js.AppendLine("          var radius = [50, 100];");
                js.AppendLine("          var option3 = {");
                js.AppendLine("            legend: {");
                js.AppendLine("                   x : 'center',");
                js.AppendLine("                   y : 'bottom',");
                js.AppendLine(legend);
                js.AppendLine("              },");
                //加的标题开始
                js.AppendLine("     title : {");
                js.AppendLine("   text: '"+name+"',");
                js.AppendLine("subtext: '',");
                js.AppendLine(" x: 'center',textStyle: {color:'#FC5004'}");
                js.AppendLine(" },");
                //加的标题结束
                js.AppendLine("            calculable: false,");
                js.AppendLine("                      series: [");
                if (dataList.Count<=0)
                {
                    js.AppendLine("                          {");
                    js.AppendLine("                              type: 'pie',");
                    js.AppendLine("                              name: '0',");
                    js.AppendLine("                              center : ['10%', '40%'],");
                    js.AppendLine("                              radius : radius,");
                    js.AppendLine("                              x: '20%',");
                    js.AppendLine(" data:[]");
                    js.AppendLine("                          },");
                }
                for (int i = 0; i < dataList.Count; i++)
                {
                    int xs = i;
                    int min = xs * 20 + 10;
                    int max = xs * 20 + 20;

                    js.AppendLine("                          {");
                    js.AppendLine("                              type: 'pie',");
                    js.AppendLine("                              name: '" + xs.ToString() + "',");
                    js.AppendLine("                              center : ['" + min + "%', '40%'],");
                    js.AppendLine("                              radius : radius,");
                    js.AppendLine("                              x: '" + max + "%',");
                    js.AppendLine(dataList[i]);
                    js.AppendLine("                          },");
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
                logManager.InsertLogError("WebAlarmPieData.aspx-CreatePieChart", ex.Message+"；"+ex.StackTrace, "CreatePieChart has an exception");
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