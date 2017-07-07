using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using System;
using System.Text;
using System.Xml;

namespace MyNet.Atmcs.Uscmcp.Web.Passcar
{
    public partial class DriverRelationAnalysis : System.Web.UI.Page
    {
        #region 成员变量

        private OtherQueryService.OtherQueryInfo client = new OtherQueryService.OtherQueryInfo();
        private Vehicle vehicle = null;
        private string url = "";
        private SettingManager settingManager = new SettingManager();
        private UserLogin userLogin = new UserLogin();

        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"]; 
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'"; 
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); 
                return; 
            }
            if (!X.IsAjaxRequest)
            {
                url = client.Url;
                vehicle = new Vehicle(url);

                InitPage();
            }
        }

        #endregion 控件事件

        /// <summary>
        /// 填充页面
        /// </summary>
        private void InitPage()
        {
            if (Session["Condition"] != null)
            {
                Condition con = Session["Condition"] as Condition;
                string hphm = con.Sqjc + con.Hphm;
                if (!string.IsNullOrEmpty(hphm) && !string.IsNullOrEmpty(con.Hpzl))
                {
                    string plateHead = settingManager.GetConfigInfo("00", "06").Rows[0]["col3"].ToString();
                    if (con.Sqjc.Equals(plateHead.Substring(0, 1)))
                    {
                        VehicleInfo vehicleInfo = vehicle.GetVehicleInfo(con.Hpzl, con.Hphm);
                        if (vehicleInfo != null)
                        {
                            string sfzmhm = vehicleInfo.Sfzmhm;
                            string syr = vehicleInfo.Syr;
                            ShowChartForce(sfzmhm, syr);
                        }
                    }
                    else
                    {
                        X.Msg.Alert("提示", "异地车辆无法进行关联查询").Show();
                    }
                }
            }
            else
            {
                string hphm = "";
                string hpzl = "";
                if (!string.IsNullOrEmpty(Request["hphm"]) && !string.IsNullOrEmpty(Request["hpzl"]))
                {
                    hphm = Request["hphm"];
                    hpzl = Request["hpzl"];
                    VehicleInfo vehicleInfo = vehicle.GetVehicleInfo(hphm, hpzl);
                    if (vehicleInfo != null)
                    {
                        string sfzmhm = vehicleInfo.Sfzmhm;
                        string syr = vehicleInfo.Syr;
                        ShowChartForce(sfzmhm, syr);
                    }
                }
            }
        }

        private void ShowChartForce(string sfzmhm, string syr)
        {
            StringBuilder html = new StringBuilder();
            string reXml = client.QueryPersonRelationAnalysis(sfzmhm);
            StringBuilder strRy = new StringBuilder();
            StringBuilder strGx = new StringBuilder();
            if (!string.IsNullOrEmpty(reXml))
            {
                string name = "";
                string relation = "";
                string bz = "";
                string weight = "";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reXml);
                XmlNodeList list = doc.SelectNodes("//Person");
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        name = list[i].SelectSingleNode("name").InnerText;
                        relation = list[i].SelectSingleNode("relation").InnerText;
                        bz = list[i].SelectSingleNode("bz").InnerText;
                        if (relation.Equals("0"))
                        {
                            strRy.Append("{category:1, name: '" + name + "',value : 6},");
                        }
                        else if (relation.Equals("1"))
                        {
                            strRy.Append("{category:2, name: '" + name + "',value : 4},");
                        }

                        if (relation.Equals("0"))
                        {
                            weight = "2";
                        }
                        else if (relation.Equals("1"))
                        {
                            weight = "4";
                        }
                        strGx.Append("   {source : '" + name + "', target : '" + syr + "', weight : " + weight + ", name: '" + bz + "'},");
                    }
                }
                else
                {
                }
            }
            else
            {
            }

            html.AppendLine(CreateForceChart("main", syr, strRy, strGx));

            this.ResourceManager1.RegisterAfterClientInitScript(html.ToString());
        }

        private string CreateForceChart(string divId, string syr, StringBuilder strRy, StringBuilder strGx)
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
            js.AppendLine("         option = {");
            js.AppendLine("               title : {");
            js.AppendLine("                   text: '人物关系：" + syr + "',");
            js.AppendLine("                  subtext: '数据来自公安大数据',");
            js.AppendLine("                   x:'right',");
            js.AppendLine("                  y:'bottom'");
            js.AppendLine("                 },");
            js.AppendLine("                tooltip : {");
            js.AppendLine("                  trigger: 'item',");
            js.AppendLine("                     formatter: '{a} : {b}'");
            js.AppendLine("                 },");
            js.AppendLine("                toolbox: {");
            js.AppendLine("                  show : true,");
            js.AppendLine("                 feature : {");
            js.AppendLine("                    restore : {show: true},");
            js.AppendLine("                    magicType: {show: true, type: ['force', 'chord']},");
            js.AppendLine("                    saveAsImage : {show: true}");
            js.AppendLine("                  }");
            js.AppendLine("                },");
            js.AppendLine("                 legend: {");
            js.AppendLine("                     x: 'left',");
            js.AppendLine("                     data:['家人','朋友']");
            js.AppendLine("                },");
            js.AppendLine("                 series : [");
            js.AppendLine("                 {");
            js.AppendLine("                  type:'force',");
            js.AppendLine("                   name : '人物关系',");
            js.AppendLine("                    ribbonType: false,");
            js.AppendLine("               categories : [");
            js.AppendLine("                 {");
            js.AppendLine("                       name: '人物'");
            js.AppendLine("                  },");
            js.AppendLine("                   {");
            js.AppendLine("                       name: '家人'");
            js.AppendLine("                  },");
            js.AppendLine("                  {");
            js.AppendLine("                       name:'朋友'");
            js.AppendLine("                     }");
            js.AppendLine("                 ],");
            js.AppendLine("                    itemStyle: {");
            js.AppendLine("                    normal: {");
            js.AppendLine("                    label: {");
            js.AppendLine("                     show: true,");
            js.AppendLine("                        textStyle: {");
            js.AppendLine("                           color: '#333',");
            js.AppendLine("                           fontSize: 18, ");
            js.AppendLine("                       }");
            js.AppendLine("                   },");
            js.AppendLine("                nodeStyle : {");
            js.AppendLine("                    brushType : 'both',");
            js.AppendLine("                      borderColor : 'rgba(255,215,0,0.4)',");
            js.AppendLine("                     borderWidth : 1");
            js.AppendLine("                 },");
            js.AppendLine("                      linkStyle: {");
            js.AppendLine("                          type: 'curve'");
            js.AppendLine("                         }");
            js.AppendLine("                  },");
            js.AppendLine("                  emphasis: {");
            js.AppendLine("                         label: {");
            js.AppendLine("                           show: false");
            js.AppendLine("                         },");
            js.AppendLine("                         nodeStyle : {");
            js.AppendLine("                         },");
            js.AppendLine("                        linkStyle : {}");
            js.AppendLine("                      }");
            js.AppendLine("                   },");
            js.AppendLine("                   useWorker: false,");
            js.AppendLine("                  minRadius : 30,");
            js.AppendLine("                maxRadius : 60,");
            js.AppendLine("                gravity: 1.1,");
            js.AppendLine("                  scaling: 1.1,");
            js.AppendLine("                  roam: 'move',");
            js.AppendLine("                  nodes:[");
            js.AppendLine("                 {category:0, name: '" + syr + "', value : 10, label: '" + syr + "（车主）'},");
            js.AppendLine(strRy.ToString());
            //js.AppendLine("                {category:1, name: '张爸',value : 3},");
            //js.AppendLine("                  {category:1, name: '张妈',value : 3},");
            //js.AppendLine("                   {category:1, name: '张儿',value : 4},");
            //js.AppendLine("                  {category:1, name: '张女',value : 4},");
            //js.AppendLine("                  {category:1, name: '张女婿',value : 3},");
            //js.AppendLine("                  {category:2, name: '李四',value : 5},");
            //js.AppendLine("                {category:2, name: '王五',value : 5},");
            //js.AppendLine("                {category:2, name: '赵六',value : 5},");
            //js.AppendLine("                  {category:2, name: '刘七',value : 4},");
            js.AppendLine("                 ],");
            js.AppendLine("                links : [");
            js.AppendLine(strGx.ToString());
            //js.AppendLine("               {source : '张爸', target : '" + syr + "', weight : 1, name: '父亲'},");
            //js.AppendLine("                {source : '张妈', target : '" + syr + "', weight : 1, name: '母亲'},");
            //js.AppendLine("                {source : '张女', target : '" + syr + "', weight : 1, name: '女儿'},");
            //js.AppendLine("                {source : '张女婿', target : '" + syr + "', weight : 1, name: '女婿'},");
            //js.AppendLine("               {source : '张儿', target : '" + syr + "', weight : 6,name:'儿子'},");
            //js.AppendLine("                {source : '李四', target : '" + syr + "', weight : 3, name: '合伙人'},");
            //js.AppendLine("                 {source : '王五', target : '" + syr + "', weight : 1, name: '狐朋狗友'},");
            //js.AppendLine("             {source : '赵六', target : '" + syr + "', weight : 6, name: '竞争对手'},");
            //js.AppendLine("                    {source : '刘七', target : '" + syr + "', weight : 1, name: '爱将'},");
            //js.AppendLine("                     {source : '张爸', target : '张妈', weight : 1,name:'夫妻'},");
            //js.AppendLine("                     {source : '张女', target : '张女婿', weight : 1,name:'夫妻'},");
            js.AppendLine("                ]");
            js.AppendLine("                 }");
            js.AppendLine("               ]");
            js.AppendLine("             };");
            js.AppendLine("             myChart.setOption(option );");
            js.AppendLine("             }");
            js.AppendLine("              );");
            return js.ToString();
        }

        private string GetTestXML()
        {
            return @"<?xml version='1.0' encoding='UTF-8'?>
<Message>
<Version>1.0</Version>
<Type>PUSH</Type>
<Body>
<Persons family='2' friend='3'>
	<Person>
	 <id>132457196511112456</id>
	 <name>郭秋媛</name>
	 <relation>0</relation>
	 <bz>父亲</bz>
	</Person>
	<Person>
	 <id>542187196804210541</id>
	 <name>于海燕</name>
	 <relation>0</relation>
	 <bz>母亲</bz>
	</Person>
	<Person>
 	 <id>784525199412123546</id>
	 <name>纪风超</name>
	 <relation>1</relation>
	 <bz>同学</bz>
	</Person>
	<Person>
 	 <id>154216198812214513</id>
	 <name>闫光景</name>
	 <relation>1</relation>
	 <bz>同事</bz>
	</Person>
	<Person>
 	 <id>475124198907172451</id>
	 <name>张伟</name>
	 <relation>1</relation>
	 <bz>朋友</bz>
	</Person>
</Persons>
</Body>
</Message>";
        }
    }
}