using System;
using System.Data;
using System.Web;
using System.Xml;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class BGisBrowse : System.Web.UI.Page
    {
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        //GisShow gs = new GisShow();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();

        private TgsPproperty tgsPproperty = new TgsPproperty();

        //MapDataOperate mapDataOperate = new MapDataOperate();
        private DataCommon dataCommon = new DataCommon();

        private UserLogin userLogin = new UserLogin();
        public int count = 0;

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
                string type = Request["type"];
                if (type != null)
                {
                    if (type.Equals("full"))
                    {
                    }
                }
                BuildTree(TreePanel1.Root);
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "访问：BGisBrowse.aspx树获取值", userinfo.NowIp, "0");
            }
        }

        [DirectMethod]
        public void MapShowTypeCheck(string msg)
        {
            //string s = "[" + msg + "]";
            //DataTable dt = Bll.Common.ToDataTable(s);
            //List<string> marklist = mapDataOperate.GetMarkJsList(false, dt);

            //for (int i = 0; i < marklist.Count; i++)
            //{
            //    this.ResourceManager1.RegisterAfterClientInitScript(marklist[i]);
            //}
        }

        #region Gis报警

        [DirectMethod]
        public void ShowAlarmInfo()
        {
            try
            {
                DataTable dt = GetDataTable("1");
                if (dt != null && dt.Rows.Count > 0)
                {
                    string hphm = dt.Rows[0][3].ToString();
                    string id = dt.Rows[0][0].ToString();
                    string kkid = dt.Rows[0][1].ToString();
                    string bjyy = dt.Rows[0][19].ToString();
                    string zjwj1 = dt.Rows[0][14].ToString();
                    if (plateNo.Value.ToString() != hphm)
                    {
                        count = 0;
                        plateNo.Value = hphm;
                        //DataTable dt2 = gs.GetTgsStation("b.relationid='" + kkid + "'");
                        //if (dt2 != null && dt2.Rows.Count > 0)
                        //{
                        //    string x = dt2.Rows[0][3].ToString();
                        //    string y = dt2.Rows[0][4].ToString();
                        //    string msg = "号牌号码：" + hphm + "    报警原因：" + bjyy;
                        //    string mark = " BMAP.RemoveAlarmMarker();BMAP.addMarkerAlarm('img/Alarm.gif','" + x + "','" + y + "','卡口布控报警',{id:'" + id + "',name: '" + hphm + "',bjyy: '" + msg + "',zjwj: '" + zjwj1 + "'},true);BMAP.GotoXY(" + x + ", " + y + ");";
                        //    this.ResourceManager1.RegisterAfterClientInitScript(mark);
                        //    //mapDataOperate.Notice("布控报警", msg);
                        //    //string js = "soundPlay('Sound/hmdalarm.WAV');";
                        //    //this.ResourceManager1.RegisterAfterClientInitScript(js);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("BGisBrowse.aspx-ShowAlarmInfo", ex.Message+"；"+ex.StackTrace, "ShowAlarmInfo has an exception");
            }
        }

        [DirectMethod]
        public void ClearAlarmInfo()
        {
            try
            {
                plateNo.Value = "";
                Session["AlarmDataInfo"] = "";
                string mark = " BMAP.RemoveAlarmMarker();";
                this.ResourceManager1.RegisterAfterClientInitScript(mark);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("BGisBrowse.aspx-ClearAlarmInfo", ex.Message+"；"+ex.StackTrace, "ClearAlarmInfo has an exception");
            }
        }

        protected DataTable GetDataTable(string rownum)
        {
            try
            {
                string where = " rownum <=" + rownum + "";
                where = "  gwsj >= STR_TO_DATE('" + DateTime.Now.AddSeconds(-30) + "','%Y-%m-%d %H:%i:%s')    and " + where;
                return tgsDataInfo.GetAlarmMonitor(where + " order by gwsj desc", rownum);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("BGisBrowse.aspx-GetDataTable", ex.Message+"；"+ex.StackTrace, "GetDataTable has an exception");
            }

            return null;
        }

        #endregion Gis报警

        private void BuildTree(Ext.Net.TreeNodeCollection nodes)
        {
            try
            {
                TreePanel1.RemoveAll(true);
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }

                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Text = "Root";
                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(HttpContext.Current.Server.MapPath("MapLayerConfig.xml"));
                foreach (XmlNode plantNode in xmlDoc.SelectNodes("layers/layer"))
                {
                    Ext.Net.TreeNode Node = new Ext.Net.TreeNode();
                    Node.NodeID = plantNode.Attributes["id"].Value;
                    Node.Text = plantNode.Attributes["name"].Value;
                    Node.IconCls = plantNode.Attributes["icon"].Value;
                    root.Nodes.Add(Node);
                    foreach (XmlNode SubNode in plantNode.ChildNodes)
                    {
                        Ext.Net.TreeNode subNode = new Ext.Net.TreeNode();
                        subNode.Leaf = true;
                        subNode.Checked = ThreeStateBool.False;
                        subNode.NodeID = SubNode.Attributes["id"].Value;
                        subNode.Text = SubNode.Attributes["name"].Value;
                        subNode.IconCls = SubNode.Attributes["icon"].Value;
                        ConfigItem ci0 = new ConfigItem("marktype", SubNode.Attributes["marktype"].Value, ParameterMode.Value);
                        subNode.CustomAttributes.Add(ci0);
                        ConfigItem ci1 = new ConfigItem("markicon", SubNode.Attributes["markicon"].Value, ParameterMode.Value);
                        subNode.CustomAttributes.Add(ci1);
                        Node.Nodes.Add(subNode);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("BGisBrowse.aspx-BuildTree", ex.Message+"；"+ex.StackTrace, "BuildTree has an exception");
            }
        }
    }
}