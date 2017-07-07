using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebPassCarVehHead : System.Web.UI.Page
    {
        private DataCountInfo dataCountInfo = new DataCountInfo();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                string datetime = Request.QueryString["datetime"];
                if (!string.IsNullOrEmpty(datetime))
                {
                    GetChartPie(datetime);
                }
                else
                {
                    GetChartPie(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                }
            }
        }

        private void GetChartPie(string datetime)
        {
            try
            {
                DataTable dtPassCarCount = null;
                if (DateTime.Now.ToString("yyyy-MM-dd").Equals(datetime))
                {
                    dtPassCarCount = GetRedisData.GetData("PassCarCount:fzjg");
                }
                else if (DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd").Equals(datetime))
                {
                    dtPassCarCount = GetRedisData.GetData("PassCarCount:fzjg_last");
                }
                else
                {
                    dtPassCarCount = dataCountInfo.GetPassCarCountByType(datetime, "FZJG");
                }
                CreateChartDivPie2(dtPassCarCount, "main", datetime);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebPassCarVehHead.aspx-GetChartPie", ex.Message+"；"+ex.StackTrace, "GetChartPie has an exception");
            }
        }

        private void CreateChartDivPie2(DataTable dtPassCarCount, string divId, string datetime)
        {
            try
            {
                StringBuilder html = new StringBuilder();
                string name = "";
                int ibd = 0; int iyd = 0;
                List<string> bdcpList = null;

                for (int j = 0; j < dtPassCarCount.Rows.Count; j++)
                {
                    if (bdcpList == null)
                    {
                        bdcpList = new List<string>();
                        string[] bdcp = dtPassCarCount.Rows[j][5].ToString().Split(',');
                        foreach (string item in bdcp)
                        {
                            bdcpList.Add(item);
                        }
                    }
                    // 本市
                    if (bdcpList.IndexOf(dtPassCarCount.Rows[j][0].ToString()) >= 0)
                    {
                        ibd += Convert.ToInt32(dtPassCarCount.Rows[j][1]);
                    }
                    else
                    {
                        // 异市本省车辆
                        if (dtPassCarCount.Rows[j][0].ToString().Substring(0, 1).Equals(dtPassCarCount.Rows[j][6].ToString()))
                        {
                            iyd += Convert.ToInt32(dtPassCarCount.Rows[j][1]);
                        }
                    }
                }

                string bd = ""; string yd = "";
                try
                {
                    bd = "{ value: " + ibd.ToString() + ", name: '+"+GetLangStr("WebPassCarVehHead2", "本地")+"+" + Math.Round(((decimal)ibd / (ibd + iyd)) * 100, 2).ToString() + "%' },";
                    yd = "{ value: " + iyd.ToString() + ", name: '+" + GetLangStr("WebPassCarVehHead3", "异地") + "+" + Math.Round(((decimal)iyd / (ibd + iyd)) * 100, 2).ToString() + "%' },";
                }
                catch
                {
                }
                string data1 = " data: [" + yd + bd + "]";
                string sp = string.Empty;
                for (int i = dtPassCarCount.Rows.Count - 1; i >= 0; i--)
                {
                    sp = sp + "{ value: " + dtPassCarCount.Rows[i][1].ToString() + ", name: '" + dtPassCarCount.Rows[i][0].ToString() + "(" + dtPassCarCount.Rows[i][1].ToString() + ")' },";
                }
                string data2 = " data: [" + sp + "]";
                html.AppendLine(CreatePieChart2(GetLangStr("WebPassCarVehHead1", "本省车辆统计分析"), divId, name, data1, data2));
                this.ResourceManager1.RegisterAfterClientInitScript(html.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebPassCarVehHead.aspx-CreateChartDivPie2", ex.Message+"；"+ex.StackTrace, "CreateChartDivPie2 has an exception");
            }
        }

        private string CreatePieChart2(string title, string divId, string name, string data1, string data2)
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
                js.AppendLine("         var colors = ['#2fc8ca', '#b7a3df', '#d97b81', '#6798f3', '#8cae3e', '#f5b989']"); ;
                js.AppendLine("         var colors1 = ['#c2d97b', '#b6c9ec', '#cbb57f']");
                js.AppendLine("         var option2 = {");
                js.AppendLine("            tooltip: {");
                js.AppendLine("            trigger: 'item',");
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
                js.AppendLine("                    selectedMode: 'single',");
                js.AppendLine("                    radius: [10, 70],");
                js.AppendLine("                    x: '50%',       ");
                js.AppendLine("                    width: '40%',");
                js.AppendLine("                    funnelAlign: 'right',");
                js.AppendLine("                    max: 1548,");
                js.AppendLine("                    itemStyle : {");
                js.AppendLine("                    normal : {");
                js.AppendLine("                    label : {");
                js.AppendLine("                   	position : 'inner'");
                js.AppendLine("                   	},");
                js.AppendLine("                   	labelLine : {");
                js.AppendLine("                   		show : false");
                js.AppendLine("                   	},");
                js.AppendLine("                   	}");
                js.AppendLine("                   	},");
                js.AppendLine(data1);
                js.AppendLine("                },");
                js.AppendLine("                {");
                js.AppendLine("                    name: '" + name + "',");
                js.AppendLine("                    type: 'pie',");
                js.AppendLine("                    radius: [80, 120],");
                js.AppendLine("                    x: '75%',       ");
                js.AppendLine("                    width: '35%',");
                js.AppendLine("                    startAngle: 30,");
                js.AppendLine("                    funnelAlign: 'left',");
                js.AppendLine("                    avoidLabelOverlap: true,");
                js.AppendLine("                    max: 1048,            ");
                js.AppendLine("                    itemStyle : {");
                js.AppendLine("                    normal : {");
                js.AppendLine("                    label : {");
                js.AppendLine("                   			fontSize:9,");
                js.AppendLine("                   			}");
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
                logManager.InsertLogError("WebPassCarVehHead.aspx-CreatePieChart2", ex.Message+"；"+ex.StackTrace, "CreatePieChart2 has an exception");
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