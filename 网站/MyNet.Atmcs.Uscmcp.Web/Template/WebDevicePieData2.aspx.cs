using System;
using System.Collections.Generic;
using System.Text;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebDevicePieData2 : System.Web.UI.Page
    {
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        private DataCountInfo dataCountInfo = new DataCountInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            string datetime = Request.QueryString["datetime"];
            if (!string.IsNullOrEmpty(datetime))
            {
                CreateChartDivPie3(datetime);
            }
            else
            {
                CreateChartDivPie3(DateTime.Now.ToString("yyyy-MM-dd"));
            }
        }

        private void CreateChartDivPie3(string datetime)
        {
            try
            {
                StringBuilder html = new StringBuilder();
                string title = GetLangStr("WebDevicePieData21", "设备异常分析");
                //string legend = "data: ['疑似异常  70%', '异常数据  20%', '无数据  10%']";
                //List<string> dataList = new List<string>();
                //dataList.Add(" data:[{value:70, name:'疑似异常  70%'},{value:30,	name:'',	itemStyle : placeHolderStyle	}]");
                //dataList.Add(" data:[{value:20, name:'异常数据  20%'},{value:80,	name:'',	itemStyle : placeHolderStyle	}]");
                //dataList.Add(" data:[{value:10, name:'无数据  10%'},{value:90,	name:'',	itemStyle : placeHolderStyle	}]");        
                
                //DataTable dtCount = dataCountInfo.GetDeviceInfoByType(datetime, "YC");
                //double sum = (Convert.ToInt32(dtCount.Rows[0]["col0"].ToString()) + Convert.ToInt32(dtCount.Rows[0]["col1"].ToString()) + Convert.ToInt32(dtCount.Rows[0]["col2"].ToString()))*1.00;//总和
                //string legend = "data: ['网络异常 "+((sum - Convert.ToInt32(dtCount.Rows[0]["col1"].ToString()) - Convert.ToInt32(dtCount.Rows[0]["col2"].ToString())) / sum * 100).ToString("0.00") + "%', '网络正常,数据异常 "+((sum - Convert.ToInt32(dtCount.Rows[0]["col0"].ToString()) - Convert.ToInt32(dtCount.Rows[0]["col2"].ToString())) / sum * 100).ToString("0.00") + "%', '正常" +((sum - Convert.ToInt32(dtCount.Rows[0]["col1"].ToString()) - Convert.ToInt32(dtCount.Rows[0]["col0"].ToString())) / sum * 100).ToString("0.00") + "%']";
                //List<string> dataList = new List<string>();
                //dataList.Add(" data:[{ value: " + dtCount.Rows[0]["col0"].ToString() + ", name: '网络异常 " + ((sum - Convert.ToInt32(dtCount.Rows[0]["col1"].ToString()) - Convert.ToInt32(dtCount.Rows[0]["col2"].ToString())) / sum * 100).ToString("0.00") + "%' },{value:" + (sum - Convert.ToInt32(dtCount.Rows[0]["col0"].ToString())) + ",	name:'',	itemStyle : placeHolderStyle}]");
                //dataList.Add(" data:[{ value: " + dtCount.Rows[0]["col1"].ToString() + ", name: '网络正常,数据异常 " + ((sum - Convert.ToInt32(dtCount.Rows[0]["col0"].ToString()) - Convert.ToInt32(dtCount.Rows[0]["col2"].ToString())) / sum * 100 ).ToString("0.00")+ "%' },{value:" + (sum - Convert.ToInt32(dtCount.Rows[0]["col1"].ToString())) + ",	name:'',	itemStyle : placeHolderStyle	}]");
                //dataList.Add(" data:[{ value: " + dtCount.Rows[0]["col2"].ToString() + ", name: '正常" +((sum - Convert.ToInt32(dtCount.Rows[0]["col1"].ToString()) - Convert.ToInt32(dtCount.Rows[0]["col0"].ToString())) / sum * 100 ).ToString("0.00")+ "%' },{value:" + (sum - Convert.ToInt32(dtCount.Rows[0]["col2"].ToString())) + ",	name:'',	itemStyle : placeHolderStyle	}]");
                DataTable dtCount = GetRedisData.GetData("DeviceDataCount:YC"); //dataCountInfo.GetDeviceInfoByType(datetime, "YC");//YC 表示异常
                double sum = (Convert.ToInt32(dtCount.Rows[0]["networkAnomalies"].ToString()) + Convert.ToInt32(dtCount.Rows[0]["networkNormalDataAnomalies"].ToString()) + Convert.ToInt32(dtCount.Rows[0]["normal"].ToString())) * 1.00;//总和
                string sp = string.Empty;
                sp = sp + "{ value: " + dtCount.Rows[0]["networkAnomalies"].ToString() + ", name: '"+GetLangStr("WebDevicePieData22","网络异常")+"" + ((sum - Convert.ToInt32(dtCount.Rows[0]["networkNormalDataAnomalies"].ToString()) - Convert.ToInt32(dtCount.Rows[0]["normal"].ToString())) / sum * 100).ToString("0.00") + "%' },";
                sp = sp + "{ value: " + dtCount.Rows[0]["networkNormalDataAnomalies"].ToString() + ", name: '" + GetLangStr("WebDevicePieData23", "网络正常,数据异常") + " " + ((sum - Convert.ToInt32(dtCount.Rows[0]["networkAnomalies"].ToString()) - Convert.ToInt32(dtCount.Rows[0]["normal"].ToString())) / sum * 100).ToString("0.00") + "%' },";
                sp = sp + "{ value: " + dtCount.Rows[0]["normal"].ToString() + ", name: '" + GetLangStr("WebDevicePieData24", "正常") + " " + ((sum - Convert.ToInt32(dtCount.Rows[0]["networkNormalDataAnomalies"].ToString()) - Convert.ToInt32(dtCount.Rows[0]["networkAnomalies"].ToString())) / sum * 100).ToString("0.00") + "%' },";
              // string  data1 = " data: [" + sp + "]";
                string data1;
                if (string.IsNullOrEmpty(sp))
                {
                    data1 = " data: []";
                }
                else
                {
                    data1 = " data: [" + sp + "]";
          
                }
                html.AppendLine(CreatePieChart("main", title, "异常区域", data1));
                this.ResourceManager1.RegisterAfterClientInitScript(html.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebDevicePieData2.aspx-CreateChartDivPie3", ex.Message+"；"+ex.StackTrace, "CreateChartDivPie3 has an exception");
            }
        }

        private string CreatePieChart3(string divId, string title, string legend, List<string> dataList)
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
                js.AppendLine("                          color: 'rgba(0,0,0,0)',");
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
                js.AppendLine("                     title : {");
                js.AppendLine("                     text: '" + title + "',");
                js.AppendLine("                     x:'center'");
                js.AppendLine("                     },");
                js.AppendLine("                      legend: {");
                js.AppendLine("                          orient: 'vertical',");
                js.AppendLine("                          x: document.getElementById('" + divId + "').offsetWidth / 2,");
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

                for (int i = 0; i < dataList.Count; i++)
                {
                    int xs = i + 1;
                    int maxradius = 150;
                    int min = maxradius - xs * 30;
                    int max = maxradius - i * 30;
                    js.AppendLine("                          {");
                    js.AppendLine("                              name: '" + xs.ToString() + "',");
                    js.AppendLine("                              type: 'pie',");
                    js.AppendLine("                              clockWise: false,");
                    js.AppendLine("                              radius: [" + min + ", " + max + "],");
                    js.AppendLine("                              itemStyle: dataStyle,");
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
                logManager.InsertLogError("WebDevicePieData2.aspx-CreateChartDivPie3", ex.Message+"；"+ex.StackTrace, "CreateChartDivPie3 has an exception");
                return "";
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
                js.AppendLine("          var option3 = {");
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
                js.AppendLine("             myChart1.setOption(option3);");
                js.AppendLine("             }");
                js.AppendLine("              );");
                return js.ToString();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebDevicePieData2.aspx-CreatePieChart", ex.Message+"；"+ex.StackTrace, "CreatePieChart has an exception");
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