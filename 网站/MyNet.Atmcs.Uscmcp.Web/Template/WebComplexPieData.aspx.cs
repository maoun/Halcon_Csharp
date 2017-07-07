using System;
using System.Text;
using System.Xml;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebComplexPieData : System.Web.UI.Page
    {
        #region 成员变量
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
 
        private OtherQueryService.OtherQueryInfo client = new OtherQueryService.OtherQueryInfo();
        private DataCountInfo dataCountInfo = new DataCountInfo();

        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string hphm = Request.QueryString["hphm"];
            string hpzl = Request.QueryString["hpzl"];
            CreateChartDivPie(hphm, hpzl);
        }

        #endregion 控件事件

        #region 私有方法

        /// <summary>
        /// 创建echart图表
        /// </summary>
        /// <param name="hphm"></param>
        /// <param name="hpzl"></param>
        private void CreateChartDivPie(string hphm, string hpzl)
        {
            try
            {
                StringBuilder html = new StringBuilder();
                string wfzs = GetLlegalCount(hphm, hpzl);
                string kfzs = GetLlegalScoreCount(hphm, hpzl);
                string wclzs = QueryUntreatedLlegalInfo(hphm, hpzl).ToString();
                string xfrs = QueryHandleLlegalPersons(hphm, hpzl).ToString();
                html.AppendLine(CreateNewPieChart("main", wfzs, kfzs, wclzs, xfrs));
                this.ResourceManager1.RegisterAfterClientInitScript(html.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebComplexPieData.aspx-CreateChartDivPie", ex.Message+"；"+ex.StackTrace, "CreateChartDivPie has an exception");
            }
        }

        /// <summary>
        /// 组装echart脚本
        /// </summary>
        /// <param name="divId"></param>
        /// <returns></returns>
        private string CreateNewPieChart(string divId, string wfzs, string kfzs, string wclzs, string jkrzs)
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
            js.AppendLine("          var myChart = ec.init(document.getElementById('" + divId + "'));");
            js.AppendLine("var labelTop = {");
            js.AppendLine("    normal : {");
            js.AppendLine("        label : {");
            js.AppendLine("            show : true,");
            js.AppendLine("            position : 'center',");
            js.AppendLine("            formatter : '{b}',");
            js.AppendLine("            textStyle: {");
            js.AppendLine("                baseline : 'bottom',");
            js.AppendLine("                fontSize:16");
            js.AppendLine("            }");
            js.AppendLine("        },");
            js.AppendLine("        labelLine : {");
            js.AppendLine("            show : false");
            js.AppendLine("        }");
            js.AppendLine("    }");
            js.AppendLine("};");
            js.AppendLine("var labelFromatter = {");
            js.AppendLine("    normal : {");
            js.AppendLine("        label : {");
            js.AppendLine("            formatter : function (params){");
            js.AppendLine("                return params.value");
            js.AppendLine("            },");
            js.AppendLine("            textStyle: {");
            js.AppendLine("                baseline : 'top',");
            js.AppendLine("                fontSize:16");
            js.AppendLine("            }");
            js.AppendLine("        }");
            js.AppendLine("    },");
            js.AppendLine("}");
            js.AppendLine("var labelBottom = {");
            js.AppendLine("    normal : {");
            js.AppendLine("        color: '#ccc',");
            js.AppendLine("        label : {");
            js.AppendLine("           show : true,");
            js.AppendLine("            position : 'center'");
            js.AppendLine("        },");
            js.AppendLine("        labelLine : {");
            js.AppendLine("            show : false");
            js.AppendLine("        }");
            js.AppendLine("    },");
            js.AppendLine("    emphasis: {");
            js.AppendLine("        color: 'rgba(0,0,0,0)'");
            js.AppendLine("    }");
            js.AppendLine("};");
            js.AppendLine("var radius = [60,110];");
            js.AppendLine("option = {");
            js.AppendLine("    legend: {");
            js.AppendLine("        x : 'center',");
            js.AppendLine("        y : 'bottom',");
            js.AppendLine("        data:[");
            js.AppendLine("            '违章总数','扣分总数','未处理违章数'");
            js.AppendLine("        ]");
            js.AppendLine("    },");
            js.AppendLine("    title : {");
            js.AppendLine("        text: '综合统计',");
            js.AppendLine("        subtext: '2016至今',");
            js.AppendLine("        x: 'center'");
            js.AppendLine("    },");
            js.AppendLine("    series : [");
            js.AppendLine("        {");
            js.AppendLine("            type : 'pie',");
            js.AppendLine("            center : ['20%', '40%'],");
            js.AppendLine("            radius : radius,");
            js.AppendLine("            x: '0%', // for funnel");
            js.AppendLine("            itemStyle : labelFromatter,");
            js.AppendLine("            data : [");
            js.AppendLine("                {name:'other', value:" + wfzs + ", itemStyle : labelBottom},");
            js.AppendLine("                {name:'违章总数', value:99999999999999999,itemStyle : labelTop}");
            js.AppendLine("            ]");
            js.AppendLine("        },");
            js.AppendLine("        {");
            js.AppendLine("            type : 'pie',");
            js.AppendLine("            center : ['50%', '40%'],");
            js.AppendLine("            radius : radius,");
            js.AppendLine("            x:'25%', // for funnel");
            js.AppendLine("            itemStyle : labelFromatter,");
            js.AppendLine("            data : [");
            js.AppendLine("                 {name:'other', value:" + kfzs + ", itemStyle : labelBottom},");
            js.AppendLine("                 {name:'扣分总数', value:99999999999999999,itemStyle : labelTop}");
            js.AppendLine("            ]");
            js.AppendLine("        },");
            js.AppendLine("        {");
            js.AppendLine("            type : 'pie',");
            js.AppendLine("   			center : ['80%', '40%'],");
            js.AppendLine("            radius : radius,");
            js.AppendLine("            x:'50%', // for funnel");
            js.AppendLine("            itemStyle : labelFromatter,");
            js.AppendLine("            data : [");
            js.AppendLine("                {name:'other', value:" + wclzs + ", itemStyle : labelBottom},");
            js.AppendLine("                {name:'未处理违章数', value:99999999999999999,itemStyle : labelTop}");
            js.AppendLine("            ]");
            js.AppendLine("        }");
            js.AppendLine("    ]");
            js.AppendLine("};");
            js.AppendLine("                  myChart.setOption(option);");
            js.AppendLine("                  }");
            js.AppendLine("               );");
            return js.ToString();
        }

        /// <summary>
        /// 组装echart脚本 备份
        /// </summary>
        /// <param name="divId"></param>
        /// <returns></returns>
        private string CreateOldPieChart(string divId, string wfzs, string kfzs, string wclzs, string jkrzs)
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
            js.AppendLine("          var myChart = ec.init(document.getElementById('" + divId + "'));");
            js.AppendLine("var labelTop = {");
            js.AppendLine("    normal : {");
            js.AppendLine("        label : {");
            js.AppendLine("            show : true,");
            js.AppendLine("            position : 'center',");
            js.AppendLine("            formatter : '{b}',");
            js.AppendLine("            textStyle: {");
            js.AppendLine("                baseline : 'bottom',");
            js.AppendLine("                fontSize:16");
            js.AppendLine("            }");
            js.AppendLine("        },");
            js.AppendLine("        labelLine : {");
            js.AppendLine("            show : false");
            js.AppendLine("        }");
            js.AppendLine("    }");
            js.AppendLine("};");
            js.AppendLine("var labelFromatter = {");
            js.AppendLine("    normal : {");
            js.AppendLine("        label : {");
            js.AppendLine("            formatter : function (params){");
            js.AppendLine("                return params.value");
            js.AppendLine("            },");
            js.AppendLine("            textStyle: {");
            js.AppendLine("                baseline : 'top',");
            js.AppendLine("                fontSize:16");
            js.AppendLine("            }");
            js.AppendLine("        }");
            js.AppendLine("    },");
            js.AppendLine("}");
            js.AppendLine("var labelBottom = {");
            js.AppendLine("    normal : {");
            js.AppendLine("        color: '#ccc',");
            js.AppendLine("        label : {");
            js.AppendLine("           show : true,");
            js.AppendLine("            position : 'center'");
            js.AppendLine("        },");
            js.AppendLine("        labelLine : {");
            js.AppendLine("            show : false");
            js.AppendLine("        }");
            js.AppendLine("    },");
            js.AppendLine("    emphasis: {");
            js.AppendLine("        color: 'rgba(0,0,0,0)'");
            js.AppendLine("    }");
            js.AppendLine("};");
            js.AppendLine("var radius = [60,110];");
            js.AppendLine("option = {");
            js.AppendLine("    legend: {");
            js.AppendLine("        x : 'center',");
            js.AppendLine("        y : 'bottom',");
            js.AppendLine("        data:[");
            js.AppendLine("            '违章总数','扣分总数','未处理违章数','违章缴款人数'");
            js.AppendLine("        ]");
            js.AppendLine("    },");
            js.AppendLine("    title : {");
            js.AppendLine("        text: '综合统计',");
            js.AppendLine("        subtext: '2016至今',");
            js.AppendLine("        x: 'center'");
            js.AppendLine("    },");
            js.AppendLine("    series : [");
            js.AppendLine("        {");
            js.AppendLine("            type : 'pie',");
            js.AppendLine("            center : ['20%', '40%'],");
            js.AppendLine("            radius : radius,");
            js.AppendLine("            x: '0%', // for funnel");
            js.AppendLine("            itemStyle : labelFromatter,");
            js.AppendLine("            data : [");
            js.AppendLine("                {name:'other', value:" + wfzs + ", itemStyle : labelBottom},");
            js.AppendLine("                {name:'违章总数', value:99999999999999999,itemStyle : labelTop}");
            js.AppendLine("            ]");
            js.AppendLine("        },");
            js.AppendLine("        {");
            js.AppendLine("            type : 'pie',");
            js.AppendLine("            center : ['40%', '40%'],");
            js.AppendLine("            radius : radius,");
            js.AppendLine("            x:'25%', // for funnel");
            js.AppendLine("            itemStyle : labelFromatter,");
            js.AppendLine("            data : [");
            js.AppendLine("                 {name:'other', value:" + kfzs + ", itemStyle : labelBottom},");
            js.AppendLine("                 {name:'扣分总数', value:99999999999999999,itemStyle : labelTop}");
            js.AppendLine("            ]");
            js.AppendLine("        },");
            js.AppendLine("        {");
            js.AppendLine("            type : 'pie',");
            js.AppendLine("   			center : ['60%', '40%'],");
            js.AppendLine("            radius : radius,");
            js.AppendLine("            x:'50%', // for funnel");
            js.AppendLine("            itemStyle : labelFromatter,");
            js.AppendLine("            data : [");
            js.AppendLine("                {name:'other', value:" + wclzs + ", itemStyle : labelBottom},");
            js.AppendLine("                {name:'未处理违章数', value:99999999999999999,itemStyle : labelTop}");
            js.AppendLine("            ]");
            js.AppendLine("        },");
            js.AppendLine("        {");
            js.AppendLine("            type : 'pie',");
            js.AppendLine("   			center : ['80%', '40%'],");
            js.AppendLine("            radius : radius,");
            js.AppendLine("            x:'75%', // for funnel");
            js.AppendLine("            itemStyle : labelFromatter,");
            js.AppendLine("            data : [");
            js.AppendLine("                    {name:'other', value:" + jkrzs + ", itemStyle : labelBottom},");
            js.AppendLine("                    {name:'违章缴款人数', value:99999999999999999,itemStyle : labelTop}");
            js.AppendLine("            ]");
            js.AppendLine("        }");
            js.AppendLine("    ]");
            js.AppendLine("};");
            js.AppendLine("                  myChart.setOption(option);");
            js.AppendLine("                  }");
            js.AppendLine("               );");
            return js.ToString();
        }
        /// <summary>
        /// 获取总违章次数
        /// </summary>
        /// <param name="hphm"></param>
        /// <param name="hpzl"></param>
        /// <returns></returns>
        private string GetLlegalCount(string hphm, string hpzl)
        {
            try
            {
                int i = client.QueryLlegalCount(hphm, hpzl);
                if (i < 0)
                {
                    i = 0;
                }
                return i.ToString();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebComplexPieData.aspx-GetLlegalCount", ex.Message+"；"+ex.StackTrace, "GetLlegalCount has an exception");
                return "0";
            }
        }

        /// <summary>
        /// 获取总扣分数
        /// </summary>
        /// <param name="hphm"></param>
        /// <param name="hpzl"></param>
        /// <returns></returns>
        private string GetLlegalScoreCount(string hphm, string hpzl)
        {
            try
            {
                int i = client.QueryLlegalScoreCount(hphm, hpzl);
                if (i < 0)
                {
                    i = 0;
                }
                return i.ToString();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebComplexPieData.aspx-GetLlegalScoreCount", ex.Message+"；"+ex.StackTrace, "GetLlegalScoreCount has an exception");
                return "0";
            }
        }

        /// <summary>
        /// 查询所有未处理的违章信息
        /// </summary>
        /// <param name="hphm"></param>
        /// <param name="hpzl"></param>
        private int QueryUntreatedLlegalInfo(string hphm, string hpzl)
        {
            try
            {
               string reXml = client.QueryUntreatedLlegalInfo(hphm, hpzl);
               // string reXml = GetTmepUntreatedLlegalInfo();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reXml);
                XmlNodeList list = doc.SelectNodes("Message/Body/LllegalList/Lllegal");
                return list.Count;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebComplexPieData.aspx-QueryUntreatedLlegalInfo", ex.Message+"；"+ex.StackTrace, "QueryUntreatedLlegalInfo has an exception");
                return 0;
            }
        }

        /// <summary>
        /// 查询销分人数
        /// </summary>
        /// <param name="hphm"></param>
        /// <param name="hpzl"></param>
        private int QueryHandleLlegalPersons(string hphm, string hpzl)
        {
            try
            {
                string reXml = client.QueryHandleLlegalPersons(hphm, hpzl);
                //string reXml = GetTempHandleLlegalPersons();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reXml);
                XmlNodeList list = doc.SelectNodes("Message/Body/Persons/Person");
                return list.Count;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebComplexPieData.aspx-QueryHandleLlegalPersons", ex.Message+"；"+ex.StackTrace, "QueryHandleLlegalPersons has an exception");
                return 0;
            }
        }

        /// <summary>
        /// 获得未处理违法临时数据
        /// </summary>
        /// <returns></returns>
        private string GetTmepUntreatedLlegalInfo()
        {
            return @"<?xml version='1.0' encoding='GBK'?>
<Message>
	<Version>1.0</Version>
	<Type>PUSH</Type>
	<Body>
		<LllegalList  number='10' >
         <Lllegal>
			<wfbh>违法编号</wfbh>
 <hpzl>号牌种类</hpzl>
 <hphm>号牌号码</hphm>
			 <wfsj>违法时间</wfsj>
			 <wfdd>违法地点</wfdd>
           <wfdz>违法地址</wfdz>
           <wfxw>违法行为</wfxw>
 <wfjfs>违法记分数</wfjfs>
 <fkje>罚款金额</fkje>
 <jkbj>交款标记</jkbj>
        </Lllegal>
        <Lllegal>
			<wfbh>违法编号</wfbh>
 <hpzl>号牌种类</hpzl>
 <hphm>号牌号码</hphm>
			 <wfsj>违法时间</wfsj>
			 <wfdd>违法地点</wfdd>
           <wfdz>违法地址</wfdz>
           <wfxw>违法行为</wfxw>
 <wfjfs>违法记分数</wfjfs>
 <fkje>罚款金额</fkje>
 <jkbj>交款标记</jkbj>
<jkrq>交款日期</jkrq>
        </Lllegal>
		</LllegalList >
	</Body>
</Message>
";
        }

        /// <summary>
        /// 获得销分人数临时数据
        /// </summary>
        /// <returns></returns>
        private string GetTempHandleLlegalPersons()
        {
            return @"<?xml version='1.0' encoding='UTF-8' ?>
<Message>
	<Version>1.0</Version>
	<Type>PUSH</Type>
	<Body>
		<Persons number='3'>
         <Person Handle='10'  id='' name=''  relation=''>
            	<jkjl>
				 	<jkrq>交款日期</jkrq>
					<jkje>交款金额</jkje>
					<kfqk>扣分情况</kfqk>
</jkjl>
  	<jkjl>
					<jkrq>交款日期</jkrq>
					<jkje>交款金额</jkje>
					<kfqk>扣分情况</kfqk>
</jkjl>
        </Person>
 		  <Person handle='10'  id='' name=''  relation=''>
          		<jkjl>
					<jkrq>交款日期</jkrq>
					<jkje>交款金额</jkje>
					<kfqk>扣分情况</kfqk>
</jkjl>
<jkjl>
					<jkrq>交款日期</jkrq>
					<jkje>交款金额</jkje>
					<kfqk>扣分情况</kfqk>
</jkjl>
        </Person>
		</Persons>
	</Body>
</Message>

";
        }

        #endregion 私有方法
    }
}