using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class FaceShelterQuery : System.Web.UI.Page
    {
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();
        private static QueryService.querypasscar client = new QueryService.querypasscar();
        private static string starttime = "";
        private static string endtime = "";
        private static DataTable dtFaceShelterQuery = null;
        private static DataTable dtStation = null;
        private static DataTable dtXsfx = null;
        private const string NoImageUrl = "../images/NoImage.png";
        private MapManager mapManager = new MapManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"]; if (!userLogin.CheckLogin(username)) { string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }
            if (!X.IsAjaxRequest)
            {
                try
                {
                    dtStation = tgsPproperty.GetStationInfo("1=1");
                    // BuildTree(TreeStation.Root);
                    //CBkk.DataSource = dtStation;
                    //CBkk.DataBind();
                    DataTable dt1 = GetRedisData.GetData("t_sys_code:240025");
                    if (dt1 != null)
                    {
                        dtXsfx = Bll.Common.ChangColName(dt1);
                    }
                    else
                    {
                        dtXsfx = mapManager.GetFxcode();
                    }

                    if (Session["Condition"] != null)
                    {
                        Condition con = Session["Condition"] as Condition;
                        starttime = start.InnerText = con.StartTime;
                        endtime = end.InnerText = con.EndTime;
                        //FieldStation.SetValue(con.Kkid, con.Kkidms);
                        kakou.Value = con.Kkidms;
                        kakouId.Value = con.Kkid;
                        //CBkk2.Value = con.Kkid;
                        CBZjs.Checked = con.Zjsaqd;
                        CBFjs.Checked = con.Fjsaqd;
                        con.Zdmb = "1";
                        Session["Condition"] = con;
                    }
                    QueryFaceShelter(1);
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：遮挡面部", userinfo.NowIp, "0");
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex);
                    logManager.InsertLogError("FaceShelterQuery.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
                }
            }
        }

        /// <summary>
        /// 行选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApplyClick(object sender, DirectEventArgs e)
        {
            try
            {
                this.FormPanel1.Collapsed = false;
                string sdata = e.ExtraParams["data"];
                string hphm = Bll.Common.GetdatabyField(sdata, "hphm");
                string hpzl = Bll.Common.GetdatabyField(sdata, "hpzlms");

                string url1 = Bll.Common.GetdatabyField(sdata, "zjwj1");
                string url2 = Bll.Common.GetdatabyField(sdata, "zjwj2");
                string url3 = Bll.Common.GetdatabyField(sdata, "zjwj3");
                if (string.IsNullOrEmpty(url1))
                {
                    url1 = NoImageUrl;
                }
                if (string.IsNullOrEmpty(url2))
                {
                    url2 = NoImageUrl;
                }
                if (string.IsNullOrEmpty(url3))
                {
                    url3 = NoImageUrl;
                }
                string js = "ShowImage(\"" + url1 + "\",\"" + url2 + "\",\"" + url3 + "\",\"" + hphm + "\",\"" + hpzl + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-ApplyClick", ex.Message + "；" + ex.StackTrace, "ApplyClick has an exception");
            }
        }

        /// <summary>
        /// 查询过车记录
        /// </summary>
        /// <param name="currentPage"></param>
        public void QueryFaceShelter(int currentPage)
        {
            curpage.Value = currentPage;
            dtFaceShelterQuery = CreateFaceShelterQueryTable();
            if (Session["Condition"] != null)
            {
                Condition con = Session["Condition"] as Condition;
                if (Session["userinfo"] != null)
                {
                    con.UserName = (Session["userinfo"] as UserInfo).UserName;//用户名称
                    con.UserCode = (Session["userinfo"] as UserInfo).UserCode;//用户编号
                }
                string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                if (ipaddress.Length < 9)
                {
                    ipaddress = "127.0.0.1";
                }
                con.UserIp = ipaddress;//用户Ip地址
                con.Dyzgnmkmc = PasscarAllQuery.dyzgnmkmc;//功能模块名称
                con.Dyzgnmkbh = PasscarAllQuery.dyzgnmkbh;//功能模块编号
                string xml = Bll.Common.GetPassCarXml(con, ((currentPage - 1) * 50).ToString(), "50");

                string rexml = client.GetPassCarInfo(xml);
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.LoadXml(rexml);
                }
                catch (Exception)
                {
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";
                    ButNext.Disabled = false;
                    ButLast.Disabled = false;
                    throw;
                }

                realCount.Value = Bll.Common.GetRowCount(xmlDoc);
                allPage.Value = (int)Math.Ceiling(Convert.ToInt32(realCount.Value) / 50.0);

                //string rexml = "<?xml version='1.0' encoding='UTF-8'?><Message><Version>1.0</Version><Type>RESPONSE</Type><Body><Return><passcarinfolist totalnum='3'><passcarinfo><kkid>100000010859</kkid><fxbh>04</fxbh><cdbh>10</cdbh><hphm>鲁B686EJ</hphm><hpzl>02</hpzl><gwsj>2016-02-11 13:05:26</gwsj><cshm>210</cshm><clsd>35</clsd><hpys>2</hpys><cllx>K33</cllx><clpp>大众</clpp><zxpp></zxpp><clwx>452.5*180.9*166.5</clwx><csys>I</csys><jllx>0</jllx><zjhsl></zjhsl><zjhqz></zjhqz><zybsl></zybsl><zybqz></zybqz><dzsl></dzsl><dzqz></dzqz><bjsl></bjsl><bjqz></bjqz><njbsl></njbsl><njbqz></njbqz><zjsaqd></zjsaqd><fjsaqd></fjsaqd><zjwj1>http://12345.jpg</zjwj1><zjwj2></zjwj2><zjwj3></zjwj3><tztp1></tztp1><tztp2></tztp2><tztp3></tztp3><tztp4></tztp4><lxwj></lxwj></passcarinfo><passcarinfo><kkid>201409021069</kkid><fxbh>04</fxbh><cdbh>10</cdbh><hphm>鲁B686EJ</hphm><hpzl>02</hpzl><gwsj>2016-02-11 13:00:26</gwsj><cshm>210</cshm><clsd>35</clsd><hpys>2</hpys><cllx>K33</cllx><clpp>大众</clpp><zxpp></zxpp><clwx>452.5*180.9*166.5</clwx><csys>I</csys><jllx>0</jllx><zjhsl></zjhsl><zjhqz></zjhqz><zybsl></zybsl><zybqz></zybqz><dzsl></dzsl><dzqz></dzqz><bjsl></bjsl><bjqz></bjqz><njbsl></njbsl><njbqz></njbqz><zjsaqd></zjsaqd><fjsaqd></fjsaqd><zjwj1>http://12345.jpg</zjwj1><zjwj2></zjwj2><zjwj3></zjwj3><tztp1></tztp1><tztp2></tztp2><tztp3></tztp3><tztp4></tztp4><lxwj></lxwj></passcarinfo><passcarinfo><kkid>100000010859</kkid><fxbh>04</fxbh><cdbh>10</cdbh><hphm>鲁B686EJ</hphm><hpzl>02</hpzl><gwsj>2016-02-11 13:00:26</gwsj><cshm>210</cshm><clsd>35</clsd><hpys>2</hpys><cllx>K33</cllx><clpp>大众</clpp><zxpp></zxpp><clwx>452.5*180.9*166.5</clwx><csys>I</csys><jllx>0</jllx><zjhsl></zjhsl><zjhqz></zjhqz><zybsl></zybsl><zybqz></zybqz><dzsl></dzsl><dzqz></dzqz><bjsl></bjsl><bjqz></bjqz><njbsl></njbsl><njbqz></njbqz><zjsaqd></zjsaqd><fjsaqd></fjsaqd><zjwj1>http://12345.jpg</zjwj1><zjwj2></zjwj2><zjwj3></zjwj3><tztp1></tztp1><tztp2></tztp2><tztp3></tztp3><tztp4></tztp4><lxwj></lxwj></passcarinfo></passcarinfolist></Return></Body></Message>";
                if (!string.IsNullOrEmpty(rexml) && Convert.ToInt32(realCount.Value) > 0)
                {
                    CXmlToDataTable(xmlDoc);
                }
            }
            if (dtFaceShelterQuery != null && dtFaceShelterQuery.Rows.Count > 0)
            {
                this.Store2.DataSource = dtFaceShelterQuery;
                this.Store2.DataBind();
                if (Session["datatable"] != null)
                {
                    Session["datatable"] = null;
                }
                Session["datatable"] = dtFaceShelterQuery;
                if (dtFaceShelterQuery.Rows.Count.Equals(50)) // 有数据 且够50条
                {
                    this.lblCurpage.Text = curpage.Value.ToString();
                    this.lblAllpage.Text = allPage.Value.ToString();
                    this.lblRealcount.Text = realCount.Value.ToString();
                    SetButState(currentPage);
                }
                else
                {
                    if (currentPage.Equals(allPage.Value))
                    {
                        this.lblCurpage.Text = curpage.Value.ToString();
                        this.lblAllpage.Text = allPage.Value.ToString();
                        this.lblRealcount.Text = realCount.Value.ToString();
                        ButNext.Disabled = true;
                        ButLast.Disabled = false;
                    }
                    if (currentPage.Equals(0)) // 第一页
                    {
                        this.lblCurpage.Text = curpage.Value.ToString();
                        this.lblAllpage.Text = allPage.Value.ToString();
                        this.lblRealcount.Text = realCount.Value.ToString();
                        ButNext.Disabled = false;
                        ButLast.Disabled = true;
                    }
                    //else
                    //{
                    //    this.lblCurpage.Text = curpage.Value.ToString();
                    //    this.lblAllpage.Text = allPage.Value.ToString();
                    //    this.lblRealcount.Text = realCount.Value.ToString();
                    //    ButNext.Disabled = false;
                    //    ButLast.Disabled = false;
                    //}
                }
            }
            else
            {
                Notice("信息提示", "未查询到相关记录");
                this.lblCurpage.Text = "1";
                this.lblAllpage.Text = "0";
                this.lblRealcount.Text = "0";
                this.Store2.DataSource = dtFaceShelterQuery;
                this.Store2.DataBind();
            }
        }

        /// <summary>
        /// 过车记录转换为datatable
        /// </summary>
        /// <param name="xmlStr"></param>
        public void CXmlToDataTable(XmlDocument xmlDoc)
        {
            XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/passcarinfolist/passcarinfo");
            try
            {
                int i = 0;
                foreach (XmlNode node in listNodes)
                {
                    i++;
                    //ds.ReadXml(node.OuterXml);
                    DataRow dr = dtFaceShelterQuery.NewRow();
                    dr["xh"] = i.ToString();
                    dr["kkid"] = (node.SelectSingleNode("kkid")).InnerText;
                    DataRow[] listdr = dtStation.Select("col1= '" + (node.SelectSingleNode("kkid")).InnerText + "'");
                    if (listdr.Length > 0)
                    {
                        dr["kkmc"] = listdr[0]["col2"].ToString();
                    }
                    else
                    {
                        dr["kkmc"] = (node.SelectSingleNode("kkid")).InnerText;
                    }
                    dr["hphm"] = (node.SelectSingleNode("hphm")).InnerText;
                    dr["hpzl"] = (node.SelectSingleNode("hpzl")).InnerText;
                    dr["hpzlms"] = Bll.Common.GetHpzlms((node.SelectSingleNode("hpzl")).InnerText);
                    dr["gwsj"] = DateTime.Parse((node.SelectSingleNode("gwsj")).InnerText);
                    // dr["gwsj"] = string.Format((node.SelectSingleNode("gwsj")).InnerText.ToString(), "yyyy-MM-dd HH:mm:ss");
                    dr["fxbh"] = (node.SelectSingleNode("fxbh")).InnerText;
                    DataRow[] listdrfx = dtXsfx.Select("col0= '" + (node.SelectSingleNode("fxbh")).InnerText + "'");
                    if (listdrfx.Length > 0)
                    { dr["fxmc"] = listdrfx[0]["col1"].ToString(); }
                    else
                    {
                        dr["fxmc"] = "";
                    }
                    dr["cdbh"] = (node.SelectSingleNode("cdbh")).InnerText;
                    dr["clsd"] = (node.SelectSingleNode("clsd")).InnerText;
                    //dr["sjly"] = (node.SelectSingleNode("sjly")).InnerText;
                    //dr["sjlyms"] = Bll.Common.GetSjlyms((node.SelectSingleNode("sjly")).InnerText);
                    dr["sjly"] = "2";
                    dr["sjlyms"] = Bll.Common.GetSjlyms(dr["sjly"].ToString());
                    dr["jllx"] = (node.SelectSingleNode("jllx")).InnerText;
                    dr["jllxms"] = Bll.Common.GetJllxms((node.SelectSingleNode("jllx")).InnerText);
                    dr["zjwj1"] = (node.SelectSingleNode("zjwj1")).InnerText;
                    dr["zjwj2"] = (node.SelectSingleNode("zjwj2")).InnerText;
                    dr["zjwj3"] = (node.SelectSingleNode("zjwj3")).InnerText;
                    dtFaceShelterQuery.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-CXmlToDataTable", ex.Message + "；" + ex.StackTrace, "CXmlToDataTable has an exception");
            }
        }

        /// <summary>
        /// 创建内存表
        /// </summary>
        /// <returns></returns>
        private DataTable CreateFaceShelterQueryTable()
        {
            DataTable dt = new DataTable("FaceShelterQuery");
            dt.Columns.Add("xh", typeof(string));
            dt.Columns.Add("kkid", typeof(string));
            dt.Columns.Add("kkmc", typeof(string));
            dt.Columns.Add("hphm", typeof(string));
            dt.Columns.Add("hpzl", typeof(string));
            dt.Columns.Add("hpzlms", typeof(string));
            dt.Columns.Add("gwsj", typeof(DateTime));
            dt.Columns.Add("fxbh", typeof(string));
            dt.Columns.Add("fxmc", typeof(string));
            dt.Columns.Add("cdbh", typeof(string));
            dt.Columns.Add("clsd", typeof(string));
            dt.Columns.Add("sjly", typeof(string));
            dt.Columns.Add("sjlyms", typeof(string));
            dt.Columns.Add("jllx", typeof(string));
            dt.Columns.Add("jllxms", typeof(string));
            dt.Columns.Add("zjwj1", typeof(string));
            dt.Columns.Add("zjwj2", typeof(string));
            dt.Columns.Add("zjwj3", typeof(string));
            return dt;
        }

        /// <summary>
        ///导出为excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToExcel(object sender, EventArgs e)
        {
            try
            {
                //Thread.Sleep(2000);
                DataTable dt = ChangeDataTable();
                ConvertData.ExportExcel(dt, this);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-ToExcel", ex.Message + "；" + ex.StackTrace, "ToExcel has an exception");
            }
        }

        /// <summary>
        /// 卡口模糊查询选中的时候给Session["tree"]赋值
        /// </summary>
        [DirectMethod]
        public void SetSession()
        {
            if (Session["tree"] != null)
            {
                Session["tree"] = null;
            }
            Session["tree"] = kakouId.Value;
        }

        #region 卡口选择

        /// <summary>
        /// 清除卡口信息
        /// </summary>
        [DirectMethod]
        public void ClearKakou()
        {
            if (Session["Condition"] != null)
            {
                Condition con = Session["Condition"] as Condition;
                con.Kkid = "";
                con.Kkidms = "";
            }
            if (Session["tree"] != null)
            {
                Session["tree"] = null;
            }
        }

        /// <summary>
        /// 得到符合条件的卡口
        /// </summary>
        [DirectMethod]
        public void GetKakou()
        {
            try
            {
                string value = kakou.Value;

                DataTable dtSelect = null;
                DataTable dt = Session["selectKakou"] as DataTable;//得到整个卡口信息
                DataRow[] rows = dt.Select("STATION_NAME like '" + value + "%'");//选出符合条件的
                if (rows.Length > 0)
                {
                    dtSelect = ToDataTable(rows);
                }
                StringBuilder strs = new StringBuilder();
                strs.AppendLine("[");
                if (dtSelect != null && dtSelect.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSelect.Rows.Count; i++)
                    {
                        if (i == dtSelect.Rows.Count - 1)
                        {
                            strs.AppendLine("{name:'" + dtSelect.Rows[i]["STATION_NAME"] + "',id:'" + dtSelect.Rows[i]["STATION_ID"] + "'}");
                        }
                        else
                        {
                            strs.AppendLine("{name:'" + dtSelect.Rows[i]["STATION_NAME"] + "',id:'" + dtSelect.Rows[i]["STATION_ID"] + "'},");
                        }
                    }
                }
                else
                {
                    strs.AppendLine("{name:'none',id:'none'},");
                }
                strs.AppendLine("]");
                string js = "setUl(" + strs.ToString() + ");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        #endregion 卡口选择

        /// <summary>
        ///转换datatable
        /// </summary>
        /// <returns></returns>
        private DataTable ChangeDataTable()
        {
            try
            {
                DataTable dt = Session["datatable"] as DataTable;
                DataTable dt1 = dt.Copy();
                if (dt1 != null)
                {
                    dt1.Columns.Remove("xh"); dt1.Columns.Remove("kkid"); dt1.Columns.Remove("hpzl");
                    dt1.Columns.Remove("fxbh"); dt1.Columns.Remove("sjly"); dt1.Columns.Remove("sjlyms"); dt1.Columns.Remove("jllx");
                    dt1.Columns.Remove("zjwj1"); dt1.Columns.Remove("zjwj2"); dt1.Columns.Remove("zjwj3");
                    dt1.Columns["kkmc"].SetOrdinal(0); dt1.Columns["hphm"].SetOrdinal(1);
                    dt1.Columns["hpzlms"].SetOrdinal(2); dt1.Columns["gwsj"].SetOrdinal(3);
                    dt1.Columns["fxmc"].SetOrdinal(4); dt1.Columns["cdbh"].SetOrdinal(5);
                    dt1.Columns["clsd"].SetOrdinal(6); dt1.Columns["jllxms"].SetOrdinal(7);
                    dt1.Columns[0].ColumnName = "卡口名称";
                    dt1.Columns[1].ColumnName = "号牌号码";
                    dt1.Columns[2].ColumnName = "号牌种类";
                    dt1.Columns[3].ColumnName = "过往时间";
                    dt1.Columns[4].ColumnName = "行驶方向";
                    dt1.Columns[5].ColumnName = "车道";
                    dt1.Columns[6].ColumnName = "车辆速度";
                    dt1.Columns[7].ColumnName = "记录类型";
                }

                return dt1;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-ChangeDataTable", ex.Message + "；" + ex.StackTrace, "ChangeDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        /// 首页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutFisrt(object sender, DirectEventArgs e)
        {
            try
            {
                curpage.Value = 1;
                QueryFaceShelter(1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-TbutFisrt", ex.Message + "；" + ex.StackTrace, "TbutFisrt has an exception");
            }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutLast(object sender, DirectEventArgs e)
        {
            try
            {
                int page = int.Parse(curpage.Value.ToString());
                page--;
                if (page < 1)
                {
                    page = 1;
                }
                curpage.Value = page;
                QueryFaceShelter(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-TbutLast", ex.Message + "；" + ex.StackTrace, "TbutLast has an exception");
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutNext(object sender, DirectEventArgs e)
        {
            try
            {
                int page = int.Parse(curpage.Value.ToString());
                int allpage = int.Parse(allPage.Value.ToString());
                page++;
                if (page > allpage)
                {
                    page = allpage;
                }
                curpage.Value = page;
                QueryFaceShelter(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-TbutNext", ex.Message + "；" + ex.StackTrace, "TbutNext has an exception");
            }
        }

        /// <summary>
        /// 尾页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutEnd(object sender, DirectEventArgs e)
        {
            try
            {
                curpage.Value = allPage.Value;
                int page = int.Parse(curpage.Value.ToString());
                QueryFaceShelter(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-TbutEnd", ex.Message + "；" + ex.StackTrace, "TbutEnd has an exception");
            }
        }

        /// <summary>
        /// 显示指定页面的数据
        /// </summary>
        /// <param name="currentPage"></param>
        private void ShowQuery(int currentPage)
        {
            try
            {
                int rownum = 15;
                int startNum = (currentPage - 1) * rownum;
                int endNum = currentPage * rownum;
                Query("", startNum, endNum);
                SetButState(currentPage);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-ShowQuery", ex.Message + "；" + ex.StackTrace, "ShowQuery has an exception");
            }
        }

        /// <summary>
        /// 组件卡口列表树
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private Ext.Net.TreeNodeCollection BuildTree(Ext.Net.TreeNodeCollection nodes)
        {
            try
            {
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }

                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Text = "卡口列表";
                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;

                // 添加 自己机构节点 和卡口
                UserInfo user = Session["userinfo"] as UserInfo;
                if (user == null)
                {
                    user = new UserInfo();
                    user.DepartName = "滨州市交通警察支队";
                    user.DeptCode = "371600000000";
                }

                Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                nodeRoot.Text = user.DepartName;
                nodeRoot.Leaf = true;
                nodeRoot.NodeID = user.DeptCode;
                nodeRoot.Icon = Ext.Net.Icon.House;

                DataTable dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
                DataRow[] rowsStation = dtStation.Select("departid='" + user.DeptCode + "'", "station_name asc");
                AddStationTree(nodeRoot, rowsStation);
                nodeRoot.Expanded = false;
                nodeRoot.Draggable = true;
                nodeRoot.Expandable = ThreeStateBool.True;
                root.Nodes.Add(nodeRoot);

                //绑定下级部门及下级部门卡口
                AddDepartTree(root, user.DeptCode);

                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
                return null;
            }
        }

        /// <summary>
        /// 添加卡口子节点
        /// </summary>
        /// <param name="root"></param>
        private void AddStationTree(Ext.Net.TreeNode DepartNode, DataRow[] rows)
        {
            try
            {
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count(); i++)
                    {
                        Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                        node.Text = rows[i]["station_name"].ToString();
                        node.Leaf = true;
                        node.Checked = ThreeStateBool.False;
                        node.NodeID = rows[i]["station_id"].ToString();
                        node.Draggable = false;
                        node.AllowDrag = false;
                        DepartNode.Nodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-AddStationTree", ex.Message + "；" + ex.StackTrace, "AddStationTree has an exception");
            }
        }

        /// <summary>
        ///绑定下级部门及下级部门卡口
        /// </summary>
        /// <param name="root"></param>
        private void AddDepartTree(Ext.Net.TreeNode root, string departCode)
        {
            try
            {
                DataTable dtDepart = GetRedisData.GetData("t_cfg_department");
                DataRow[] rows = dtDepart.Select("classcode='" + departCode + "'");
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count(); i++)
                    {
                        Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                        nodeRoot.Text = rows[i]["departname"].ToString();
                        nodeRoot.Leaf = true;
                        nodeRoot.NodeID = rows[i]["departid"].ToString();
                        nodeRoot.Icon = Icon.House;

                        DataTable dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
                        DataRow[] rowsStation = dtStation.Select(" departid='" + nodeRoot.NodeID + "' ", "station_name asc");
                        AddStationTree(nodeRoot, rowsStation);
                        nodeRoot.Expanded = false;
                        nodeRoot.Draggable = true;
                        nodeRoot.Expandable = ThreeStateBool.True;
                        AddDepartTree(nodeRoot, rows[i]["departid"].ToString());
                        root.Nodes.Add(nodeRoot);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-AddDepartTree", ex.Message + "；" + ex.StackTrace, "AddDepartTree has an exception");
            }
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startNum"></param>
        /// <param name="endNum"></param>
        private void Query(string where, int startNum, int endNum)
        {
            try
            {
                DataTable dt = tgsDataInfo.GetPeccancyInfo(where, startNum, endNum);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Session["datatable"] = dt;
                    this.Store2.DataSource = dt;
                    this.Store2.DataBind();
                    this.lblCurpage.Text = curpage.Value.ToString();
                    this.lblAllpage.Text = allPage.Value.ToString();
                    this.lblRealcount.Text = realCount.Value.ToString();
                }
                else
                {
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";
                    Notice("信息提示", "未查询到相关记录");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-Query", ex.Message + "；" + ex.StackTrace, "Query has an exception");
            }
        }

        /// <summary>
        ///提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Notice(string title, string msg)
        {
            try
            {
                Notification.Show(new NotificationConfig
                {
                    Title = title,
                    Icon = Icon.Information,
                    HideDelay = 2000,
                    Height = 120,
                    Html = "<br></br>" + msg + "!"
                });
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice has an exception");
            }
        }

        protected void Button5_DirectClick(object sender, DirectEventArgs e)
        {
            if (string.IsNullOrEmpty(kakou.Value))
            {
                DateTime start = Convert.ToDateTime(starttime);
                DateTime end = Convert.ToDateTime(endtime);
                TimeSpan sp = end.Subtract(start);
                if (sp.TotalMinutes > 120)
                {
                    Notice("信息提示", "只能选择两个小时之内的时间！");
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";
                    ButNext.Disabled = true;
                    ButLast.Disabled = true;
                    Store2.DataSource = new DataTable();
                    Store2.DataBind();
                    return;
                }
            }
            if (!string.IsNullOrEmpty(kakou.Value))
            {
                if (kakou.Value.Contains(","))
                {
                    string[] strs = kakou.Value.Split(',');
                    if (strs.Length > 10)
                    {
                        Notice("信息提示", "最多只能选择10个卡口！");
                        this.lblCurpage.Text = "1";
                        this.lblAllpage.Text = "0";
                        this.lblRealcount.Text = "0";
                        ButNext.Disabled = true;
                        ButLast.Disabled = true;
                        Store2.DataSource = new DataTable();
                        Store2.DataBind();
                        return;
                    }
                }
            }
            if (Session["Condition"] != null)
            {
                Session["Condition"] = null;
            }
            Condition con = new Condition();
            if (!string.IsNullOrEmpty(starttime))
            {
                con.StartTime = starttime;
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                con.EndTime = endtime;
            }
            if (!string.IsNullOrEmpty(this.kakou.Value))
            {
                string kkid = this.kakouId.Value.ToString();
                if (!string.IsNullOrEmpty(kkid))
                {
                    con.Kkid = kkid;
                    if (Session["tree"] != null)
                    {
                        Session["tree"] = null;
                    }
                    Session["tree"] = kkid;
                }
                con.Kkidms = this.kakou.Value;
            }
            //if (CBkk2.Value != null)
            //{
            //    con.Kkid = CBkk2.Value.ToString();
            //}
            if (CBZjs.Checked == false && CBFjs.Checked == false)
            {
                con.Zdmb = "0";
            }
            else if (CBZjs.Checked == true && CBFjs.Checked == false)
            {
                con.Zdmb = "2";
            }
            else if (CBZjs.Checked == false && CBFjs.Checked == true)
            {
                con.Zdmb = "3";
            }
            else if (CBZjs.Checked == true && CBFjs.Checked == true)
            {
                con.Zdmb = "4";
            }
            else
            {
                con.Zdmb = "1";
            }
            Session["Condition"] = con;
            QueryFaceShelter(1);
            //ShowQuery(1);
            //string kk2 = objectToString(CBkk2.Value);
            //string begin = objectToString(DateField3.Value);
            //string end = DateTimeToString(DateField4.Value);
            //this.Store2.DataSource = VehiclesBLL.FaceShelterDataQuery(begin, end, kk2);
            //this.Store2.DataBind();
        }

        /// <summary>
        /// Ext控件中的值没填写时，判断并赋值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string objectToString(object obj)
        {
            return obj != null ? obj.ToString() : "";
        }

        protected void Store2_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //string kk2 = ValueSelectedToString.objectToString(SelectBox6.Value);
            //string begin = ValueSelectedToString.objectToString(DateField3.Value);
            //string end = ValueSelectedToString.DateTimeToString(DateField4.Value);
            //this.Store2.DataSource = VehiclesBLL.FaceShelterDataQuery(begin, end, kk2);
            //this.Store2.DataBind();
        }

        protected void Store2_Submit(object sender, StoreSubmitDataEventArgs e)
        {
            string format = this.FormatType.Value.ToString();

            XmlNode xml = e.Xml;

            this.Response.Clear();

            switch (format)
            {
                case "xml":
                    string strXml = xml.OuterXml;
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xml");
                    this.Response.AddHeader("Content-Length", strXml.Length.ToString());
                    this.Response.ContentType = "application/xml";
                    this.Response.Write(strXml);

                    break;

                case "xls":
                    this.Response.ContentType = "application/vnd.ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
                    XslCompiledTransform xtExcel = new XslCompiledTransform();
                    xtExcel.Load(Server.MapPath("../Export/Excel.xsl"));
                    xtExcel.Transform(xml, null, Response.OutputStream);

                    break;

                case "csv":
                    this.Response.ContentType = "application/octet-stream";
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.csv");
                    XslCompiledTransform xtCsv = new XslCompiledTransform();
                    xtCsv.Load(Server.MapPath("Csv.xsl"));
                    xtCsv.Transform(xml, null, Response.OutputStream);
                    break;
            }

            this.Response.End();
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="isstart"></param>
        /// <param name="strtime"></param>
        [DirectMethod]
        public void GetDateTime(bool isstart, string strtime)
        {
            try
            {
                if (isstart)
                    starttime = strtime;
                else
                    endtime = strtime;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-GetDateTime", ex.Message + "；" + ex.StackTrace, "GetDateTime has an exception");
            }
        }

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="page"></param>
        private void SetButState(int page)
        {
            try
            {
                curpage.Value = page;
                int allpage = int.Parse(allPage.Value.ToString());

                if (allpage > 1)
                {
                    ButLast.Disabled = false;
                    ButNext.Disabled = false;
                    // ButFisrt.Disabled = false;
                    //ButEnd.Disabled = false;
                }
                if (page == 1)
                {
                    ButLast.Disabled = true;
                    //ButFisrt.Disabled = true;
                }
                if (page == allpage)
                {
                    ButNext.Disabled = true;
                    // ButEnd.Disabled = true;
                }
                if (allpage <= 1)
                {
                    //ButFisrt.Disabled = true;
                    ButNext.Disabled = true;
                    ButLast.Disabled = true;
                    // ButEnd.Disabled = true;
                    page = 0;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FaceShelterQuery.aspx-SetButState", ex.Message + "；" + ex.StackTrace, "SetButState has an exception");
            }
        }
    }
}