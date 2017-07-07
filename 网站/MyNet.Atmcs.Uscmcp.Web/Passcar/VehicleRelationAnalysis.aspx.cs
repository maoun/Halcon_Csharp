using System;
using System.Text;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web.Passcar
{
    public partial class VehicleRelationAnalysis : System.Web.UI.Page
    {
        private UserLogin userLogin = new UserLogin();

        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username)) { string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" +StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }
            this.EnableViewState = false;
            if (!X.IsAjaxRequest)
            {
                /* ResourceManager1.SetTheme(Ext.Net.Theme.Slate);*/
                //userLogin.IsLogin();
                InitPage();
            }
        }

        private void InitPage()
        {
            panelMain.AutoLoad.Url = "../Map/FootHold.aspx";
            if (Session["Condition"] != null)
            {
                Condition con = Session["Condition"] as Condition;
                string hphm = con.Sqjc + con.Hphm;
                if (!string.IsNullOrEmpty(hphm) && !string.IsNullOrEmpty(con.Hpzl))
                {
                    ShowChartForce("main", hphm);
                }
            }
        }

        private void ShowChartForce(string divId, string hphm)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine(CreateForceChart(divId, hphm));
            this.ResourceManager1.RegisterAfterClientInitScript(html.ToString());
        }

        private string CreateForceChart(string divId, string hphm)
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
            js.AppendLine("         'echarts/chart/force',");
            js.AppendLine("         'echarts/chart/chord'");
            js.AppendLine("     ],");
            js.AppendLine("      function (ec) {");
            js.AppendLine("         var myChart= ec.init(document.getElementById('" + divId + "'));");
            js.AppendLine("option = {");
            js.AppendLine("    title : {");
            js.AppendLine("      text: '车人关系：" + hphm + "',");
            js.AppendLine("        subtext: '数据来自车管所',");
            js.AppendLine("       x:'right',");
            js.AppendLine("      y:'bottom'");
            js.AppendLine("   },");
            js.AppendLine("   tooltip : {");
            js.AppendLine("       trigger: 'item',");
            js.AppendLine("       formatter: '{a} : {b}'");
            js.AppendLine("    },");
            js.AppendLine("    toolbox: {");
            js.AppendLine("      show : true,");
            js.AppendLine("      feature : {");
            js.AppendLine("           restore : {show: true},");
            js.AppendLine("          magicType: {show: true, type: ['force', 'chord']},");
            js.AppendLine("          saveAsImage : {show: true}");
            js.AppendLine("     }");
            js.AppendLine("  },");
            js.AppendLine("    legend: {");
            js.AppendLine("     x: 'left',");
            js.AppendLine("        data:['缴款人']");
            js.AppendLine("   },");
            js.AppendLine("   series : [");
            js.AppendLine("        {");
            js.AppendLine("           type:'force',");
            js.AppendLine("          name : '说明',");
            js.AppendLine("          ribbonType: false,");
            js.AppendLine("          categories : [");
            js.AppendLine("          {");
            js.AppendLine("                 name: '人物'");
            js.AppendLine("              },");
            js.AppendLine("             {");
            js.AppendLine("                 name: '缴款人',");
            //js.AppendLine("                symbol: 'diamond'"); 标志图形类型，默认自动选择（8种类型循环使用，不显示标志图形可设为'none'），默认循环选择类型有：'circle' | 'rectangle' | 'triangle' | 'diamond' |'emptyCircle' | 'emptyRectangle' | 'emptyTriangle' | 'emptyDiamond' 另外，还支持五种更特别的标志图形'heart'（心形）、'droplet'（水滴）、'pin'（标注）、'arrow'（箭头）和'star'（五角星），这并不出现在常规的8类图形中，但无论是在系列级还是数据级上你都可以指定使用，同时，'star' + n（n>=3)可变化出N角星，如指定为'star6'则可以显示6角星 自1.3.5起支持symbol为自定义图片，格式为'image://' + '图片路径'， 如'image://../asset/ico/favicon.png'
            js.AppendLine("            }");
            js.AppendLine("       ],");
            js.AppendLine("       itemStyle: {");
            js.AppendLine("          normal: {");
            js.AppendLine("           label: {");
            js.AppendLine("                show: true,");
            js.AppendLine("               textStyle: {");
            js.AppendLine("                 color: '#333',");
            js.AppendLine("                  fontSize:13");
            js.AppendLine("              }");
            js.AppendLine("          },");
            js.AppendLine("         nodeStyle : {");
            js.AppendLine("            brushType : 'both',");
            js.AppendLine("          borderColor : 'rgba(255,215,0,0.4)',");
            js.AppendLine("           borderWidth : 1");
            js.AppendLine("       }");
            js.AppendLine("   },");
            js.AppendLine("     emphasis: {");
            js.AppendLine("     label: {");
            js.AppendLine("            show: false");
            js.AppendLine("             // textStyle: null      // 默认使用全局文本样式，详见TEXTSTYLE");
            js.AppendLine("            },");
            js.AppendLine("            nodeStyle : {");
            js.AppendLine("              //r: 30");
            js.AppendLine("            },");
            js.AppendLine("            linkStyle : {}");
            js.AppendLine("           }");
            js.AppendLine("       },");
            js.AppendLine("      minRadius : 25,");
            js.AppendLine("     maxRadius : 45,");
            js.AppendLine("       gravity: 1.1,");
            js.AppendLine("     scaling: 1.2,");
            js.AppendLine("       draggable: false,");
            js.AppendLine("     linkSymbol: 'arrow',");
            js.AppendLine("      steps: 10,");
            js.AppendLine("       coolDown: 0.9,");
            js.AppendLine("        nodes:[");
            js.AppendLine("           {");
            js.AppendLine("              category:0, name: '" + hphm + "', value : 10,");
            //js.AppendLine("           symbol: 'image://../image/hpTemp.png',");
            //js.AppendLine("          symbolSize: [60, 35],");
            js.AppendLine("           draggable: true,");
            js.AppendLine("           itemStyle: {");
            js.AppendLine("               normal: {");
            js.AppendLine("                  label: {");
            //js.AppendLine("                position: 'right',");
            js.AppendLine("                     textStyle: {");
            js.AppendLine("                            color: 'blue',");
            js.AppendLine("                             fontSize:15");
            js.AppendLine("                                }");
            js.AppendLine("                       }");
            js.AppendLine("                    }");
            js.AppendLine("                }");
            js.AppendLine("          },");
            js.AppendLine("           {category:1, name: '丽萨-乔布斯',value : 2},");
            js.AppendLine("            {category:1, name: '保罗-乔布斯',value : 3},");
            js.AppendLine("            {category:1, name: '克拉拉-乔布斯',value : 3},");
            js.AppendLine("          {category:1, name: '劳伦-鲍威尔',value : 7},");
            js.AppendLine("       ],");
            js.AppendLine("     links : [");
            js.AppendLine("		{source : '丽萨-乔布斯', target : '" + hphm + "', weight : 3, name: '缴款2次，扣6分，罚200元'},");
            js.AppendLine("          {source : '保罗-乔布斯', target : '" + hphm + "', weight : 2, name: '缴款3次，扣9分，罚800元'},");
            js.AppendLine("          {source : '克拉拉-乔布斯', target : '" + hphm + "', weight : 4, name: '缴款1次，扣6分，罚200元'},");
            js.AppendLine("          {source : '劳伦-鲍威尔', target : '" + hphm + "', weight : 1, name: '缴款4次，扣6分，罚1000元'},");
            js.AppendLine("        ]");
            js.AppendLine("      }");
            js.AppendLine("    ]");
            js.AppendLine("           };");
            js.AppendLine("             myChart.setOption(option );");
            js.AppendLine("             }");
            js.AppendLine("              );");
            return js.ToString();
        }
    }
}