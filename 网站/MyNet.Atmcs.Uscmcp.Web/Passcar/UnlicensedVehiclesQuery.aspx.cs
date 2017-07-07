using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class UnlicensedVehiclesQuery : System.Web.UI.Page
    {
        #region 成员变量

        private const string NoImageUrl = "../images/NoImage.png";
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();
        private DataCommon dataCommon = new DataCommon();
        private static string starttime = "";
        private static string endtime = "";
        private static QueryService.querypasscar client = new QueryService.querypasscar();
        private MapManager mapManager = new MapManager();
        private static DataTable dtStation = null;
        private static DataTable dtXsfx = null;
        private static int sumBiaoji = 1;
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        #endregion 成员变量

        #region 页面事件

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"]; if (!userLogin.CheckLogin(username)) { string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }
            if (!IsPostBack)
            {
                if (!X.IsAjaxRequest)
                {
                    try
                    {
                        //BuildTree(TreeStation.Root);
                        StoreDataBind();

                        if (Session["Condition"] != null)
                        {
                            Condition con = Session["Condition"] as Condition;
                            starttime = start.InnerText = con.StartTime;
                            endtime = end.InnerText = con.EndTime;
                            SBcsys.Value = con.Csys;
                            //卡口
                            kakou.Value = con.Kkidms;
                            kakouId.Value = con.Kkid;
                            //车辆品牌
                            if (con.Clpp.ToString().Contains("-"))
                            {
                                int i = con.Clpp.ToString().IndexOf("-");
                                ClppChoice.Value = con.Clpp.ToString().Substring(1, i - 1);
                            }
                            else
                            {
                                ClppChoice.Value = con.Clpp;
                            }
                            Cnjbz.Checked = con.Njb;
                            Czjh.Checked = con.Zjh;
                            Czyb.Checked = con.Zyb;
                            Cdz.Checked = con.Dz;
                            Cbj.Checked = con.Bj;
                        }
                        else
                        {
                            start.InnerText = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                            end.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            starttime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        curpage.Value = "1";
                        changePage(1);
                    }
                    catch { }
                }
            }
            UserInfo userinfo = Session["Userinfo"] as UserInfo;
            logManager.InsertLogRunning(userinfo.UserName, "访问：无车牌分析", userinfo.NowIp, "0");
        }

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                DataTable dt1 = GetRedisData.GetData("t_sys_code:240013");
                if (dt1 != null)
                {
                    Scsys.DataSource = dt1;
                    Scsys.DataBind();
                }
                else
                {
                    this.Scsys.DataSource = tgsPproperty.GetCarColorDict();
                    this.Scsys.DataBind();
                }

                dtStation = mapManager.GetStation();
                DataTable dt3 = GetRedisData.GetData("t_sys_code:240025");
                //行驶方向
                if (dt3 != null)
                {
                    dtXsfx = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240025"));
                }
                else
                {
                    dtXsfx = Bll.Common.ChangColName(mapManager.GetFxcode());
                }
                DataTable dt4 = GetRedisData.GetData("t_sys_code:140001");
                if (dt4 != null)
                {
                    this.cllx.DataSource = Bll.Common.ChangColName(dt4);
                    this.cllx.DataBind();
                }
                else
                {
                    this.cllx.DataSource = tgsPproperty.GetPalteType();
                    this.cllx.DataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        public DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_DirectClick(object sender, DirectEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(kakou.Value) && string.IsNullOrEmpty(cbocllx.Text))
                {
                    DateTime start = Convert.ToDateTime(starttime);
                    DateTime end = Convert.ToDateTime(endtime);
                    TimeSpan sp = end.Subtract(start);
                    if (sp.TotalMinutes > 120)
                    {
                        Notice("信息提示", "只能选择两个小时之内的时间！");
                        this.FormPanel2.Title = "查询结果：当前查询出符合条件的记录0条,现在显示0条";
                        LabNum.Html = "<font >&nbsp;&nbsp;当前1页,共0页</font>";
                        ButNext.Disabled = true;
                        ButLast.Disabled = true;
                        Store1.DataSource = new DataTable();
                        Store1.DataBind();
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
                            this.FormPanel2.Title = "查询结果：当前查询出符合条件的记录0条,现在显示0条";
                            LabNum.Html = "<font >&nbsp;&nbsp;当前1页,共0页</font>";
                            ButNext.Disabled = true;
                            ButLast.Disabled = true;
                            Store1.DataSource = new DataTable();
                            Store1.DataBind();
                            return;
                        }
                    }
                }
                if (Session["Condition"] != null)
                {
                    Session["Condition"] = null;
                }
                Condition con = new Condition();
                con.StartTime = starttime;
                con.EndTime = endtime;
                if (SBcsys.Value != null)
                {
                    con.Csys = SBcsys.Value.ToString();
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
                if (!string.IsNullOrEmpty(ClppChoice.Value))
                {
                    con.Clpp = ClppChoice.Value;
                }
                if (cbocllx.SelectedIndex != -1)
                {
                    con.Hpzl = cbocllx.SelectedItem.Value;
                }
                con.Njb = Cnjbz.Checked;
                con.Zjh = Czjh.Checked;
                con.Zyb = Czyb.Checked;
                con.Dz = Cdz.Checked;
                con.Bj = Cbj.Checked;
                Session["Condition"] = con;
                changePage(1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-Button1_DirectClick", ex.Message + "；" + ex.StackTrace, "Button1_DirectClick has an exception");
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Store1_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                changePage(1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-Store1_RefreshData", ex.Message + "；" + ex.StackTrace, "Store1_RefreshData has an exception");
            }
        }

        /// <summary>
        ///导出为 xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToXml(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ChangeDataTable();
                ConvertData.ExportXml(dt, this);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-ToXml", ex.Message + "；" + ex.StackTrace, "ToXml has an exception");
            }
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
                DataTable dt = ChangeDataTable();
                ConvertData.ExportExcel(dt, this);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-ToExcel", ex.Message + "；" + ex.StackTrace, "ToExcel has an exception");
            }
        }

        /// <summary>
        /// 导出为csv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToCsv(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ChangeDataTable();
                ConvertData.ExportCsv(dt, this);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-ToCsv", ex.Message + "；" + ex.StackTrace, "ToCsv has an exception");
            }
        }

        /// <summary>
        ///打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButPrintClick(object sender, DirectEventArgs e)
        {
            try
            {
                DataTable dt = ChangeDataTable();
                if (dt != null)
                {
                    Session["printdatatable"] = ChangeDataTable();
                    string xml = Bll.Common.GetPrintXml("无牌车辆信息表", "", "", "printdatatable");
                    string js = "OpenPrintPageH(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-ButPrintClick", ex.Message + "；" + ex.StackTrace, "ButPrintClick has an exception");
            }
        }

        /// <summary>
        /// 上一页事件
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

                changePage(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-TbutLast", ex.Message + "；" + ex.StackTrace, "TbutLast has an exception");
            }
        }

        /// <summary>
        ///下一页事件
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

                changePage(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-TbutNext", ex.Message + "；" + ex.StackTrace, "TbutNext has an exception");
            }
        }

        /// <summary>
        ///首页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutFisrt(object sender, DirectEventArgs e)
        {
            try
            {
                curpage.Value = 1;
                ShowQuery(1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-TbutFisrt", ex.Message + "；" + ex.StackTrace, "TbutFisrt has an exception");
            }
        }

        /// <summary>
        ///尾页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutEnd(object sender, DirectEventArgs e)
        {
            try
            {
                curpage.Value = allPage.Value;
                int page = int.Parse(curpage.Value.ToString());
                ShowQuery(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-TbutEnd", ex.Message + "；" + ex.StackTrace, "TbutEnd has an exception");
            }
        }

        #endregion 页面事件

        #region 私有方法

        /// <summary>
        /// 设置初始时间
        /// </summary>
        private void DataSetDateTime()
        {
            starttime = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
            start.InnerText = starttime;

            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            end.InnerText = endtime;
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
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
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
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-AddStationTree", ex.Message + "；" + ex.StackTrace, "AddStationTree has an exception");
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
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-AddDepartTree", ex.Message + "；" + ex.StackTrace, "AddDepartTree has an exception");
            }
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="currentPage"></param>
        private void changePage(int currentPage)
        {
            DataTable table = new DataTable();
            try
            {
                table = getData(currentPage);
                if (Session["datatable"] != null)
                {
                    Session["datatable"] = null;
                }
                Session["datatable"] = table;
                if (table != null && table.Rows.Count > 0)
                {
                    this.Store1.DataSource = table;
                    this.Store1.DataBind();

                    //ButCsv.Disabled = false;
                    ButExcel.Disabled = false;
                    // ButXml.Disabled = false;
                    //ButPrint.Disabled = false;
                    if (table.Rows.Count.Equals(15))
                    {
                        this.FormPanel2.Title = "查询结果：共计查询出符合条件的记录" + realCount.Value.ToString() + "条,现在显示" + ((currentPage - 1) * 15 + 1).ToString() + " - " + (currentPage * 15) + "条,其余结果尚未显示，请点击 下一页 进行显示";
                        // this.FormPanel2.Title = "查询结果：当前查询出符合条件的记录" + realCount.Value.ToString() + "条,其余结果尚未显示，请点击 下一页 进行显示";
                    }
                    else
                    {
                        if (currentPage.Equals(1))
                        {
                            this.FormPanel2.Title = "查询结果：当前查询出符合条件的记录" + realCount.Value.ToString() + "条,现在显示" + ((currentPage - 1) * 15 + 1).ToString() + " - " + realCount.Value.ToString() + "条";
                        }
                        else
                        {
                            this.FormPanel2.Title = "查询结果：当前查询出符合条件的记录" + realCount.Value.ToString() + "条,现在显示" + ((currentPage - 1) * 15 + 1).ToString() + " - " + realCount.Value.ToString() + "条,其余结果尚未显示，请点击 上一页 进行显示";
                        }
                    }
                }
                else
                {
                    this.Store1.DataSource = new DataTable();
                    this.Store1.DataBind();
                    Notice("信息提示", "未查询到相关记录");
                    this.FormPanel2.Title = "查询结果：当前查询出符合条件的记录0条,现在显示0条";
                    //ButCsv.Disabled = true;
                    ButExcel.Disabled = true;
                    //ButXml.Disabled = true;
                    //ButPrint.Disabled = true;
                }
                if (table != null)
                {
                    SetButState(currentPage, table.Rows.Count);
                }
                else
                {
                    LabNum.Html = "<font >&nbsp;&nbsp;当前1页,共0页</font>";
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-changePage", ex.Message + "；" + ex.StackTrace, "changePage has an exception");
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        private DataTable getData(int currentPage)
        {
            curpage.Value = currentPage;
            try
            {
                DataTable table = new DataTable();
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
                    con.Hphm = "无牌";
                    con.Clpp = tgsDataInfo.GetClzppString(con.Clpp);
                    string xml = Bll.Common.GetPassCarXml(con, ((currentPage - 1) * 15).ToString(), "15");
                    string rexml = client.GetPassCarInfo(xml);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(rexml);
                    try
                    {
                        if (xmlDoc.SelectSingleNode("//passcarinfolist").Attributes["totalnum"] != null)
                        {
                            realCount.Value = xmlDoc.SelectSingleNode("//passcarinfolist").Attributes["totalnum"].Value;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    if (!string.IsNullOrEmpty(rexml) && Bll.Common.GetPassCount(rexml) > 0)
                    {
                        return CXmlToDataTable(rexml, table);
                    }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-getData", ex.Message + "；" + ex.StackTrace, "getData has an exception");
                return null;
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            //把session中的条件设置为空
            if (Session["Condition"] != null)
            {
                Session["Condition"] = null;
            }
            starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeJs = "clearTime('" + starttime + "','" + endtime + "');";//js方法后面的分号一定要加上
            this.ResourceManager1.RegisterAfterClientInitScript(timeJs);
            //卡口列表
            //if (!string.IsNullOrEmpty(FieldStation.Text))//判断卡口是否为空
            //{
            //    string js = "directclear();";
            //    this.ResourceManager1.RegisterAfterClientInitScript(js);
            //}
            if (!string.IsNullOrEmpty(kakou.Value))//判断卡口是否为空
            {
                string js = "clearMenu();";
                if (Session["tree"] != null)
                {
                    Session["tree"] = null;
                }
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            ////卡口列表
            //if (!string.IsNullOrEmpty(SBkkmc.Text))//判断卡口是否为空
            //{
            //    SBkkmc.Text = "";
            //}
            //车身颜色
            SBcsys.Text = "";

            ClppChoice.Value = "";
            clzpp.Value = "";
            clpp.Value = "";

            Cnjbz.Checked = false;
            Czjh.Checked = false;
            Czyb.Checked = false;
            Cdz.Checked = false;
            Cbj.Checked = false;
        }

        /// <summary>
        /// xml数据转换为datatable
        /// </summary>
        /// <param name="xmlStr"></param>
        private DataTable CXmlToDataTable(string xmlStr, DataTable table)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);
                CreatePasscarTable(table);

                XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/passcarinfolist/passcarinfo");

                int i = 0;
                foreach (XmlNode node in listNodes)
                {
                    i++;
                    //ds.ReadXml(node.OuterXml);
                    DataRow dr = table.NewRow();
                    dr["xh"] = i.ToString();
                    dr["kkid"] = (node.SelectSingleNode("kkid")).InnerText;
                    DataRow[] listdr = dtStation.Select("STATION_ID= '" + (node.SelectSingleNode("kkid")).InnerText + "'");
                    if (listdr.Length > 0)
                    {
                        dr["kkmc"] = listdr[0]["STATION_NAME"].ToString();
                    }
                    else
                    {
                        dr["kkmc"] = (node.SelectSingleNode("kkid")).InnerText;
                    }
                    dr["hphm"] = (node.SelectSingleNode("hphm")).InnerText;
                    dr["hpzl"] = (node.SelectSingleNode("hpzl")).InnerText;
                    dr["hpzlms"] = Bll.Common.GetHpzlms((node.SelectSingleNode("hpzl")).InnerText);
                    dr["gwsj"] = DateTime.Parse((node.SelectSingleNode("gwsj")).InnerText);
                    dr["fxbh"] = (node.SelectSingleNode("fxbh")).InnerText;
                    DataRow[] listdrfx = dtXsfx.Select("col0= '" + (node.SelectSingleNode("fxbh")).InnerText + "'");
                    if (listdrfx.Length > 0)
                    { dr["fxmc"] = listdrfx[0]["col1"].ToString(); }
                    else
                    { dr["fxmc"] = ""; }
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
                    table.Rows.Add(dr);
                }
                return table;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-CXmlToDataTable", ex.Message + "；" + ex.StackTrace, "CXmlToDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        /// 为DataTable添加列
        /// </summary>
        /// <returns></returns>
        private void CreatePasscarTable(DataTable dt)
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-CreatePasscarTable", ex.Message + "；" + ex.StackTrace, "CreatePasscarTable has an exception");
            }
        }

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
                    dt1.Columns.Remove("fxbh"); dt1.Columns.Remove("sjly"); dt1.Columns.Remove("jllx");
                    dt1.Columns.Remove("zjwj1"); dt1.Columns.Remove("zjwj2"); dt1.Columns.Remove("zjwj3");

                    dt1.Columns["kkmc"].ColumnName = "卡口名称";
                    dt1.Columns["hphm"].ColumnName = "号牌号码";
                    dt1.Columns["hpzlms"].ColumnName = "号牌种类";
                    dt1.Columns["gwsj"].ColumnName = "过往时间";
                    dt1.Columns["fxmc"].ColumnName = "行驶方向";
                    dt1.Columns["cdbh"].ColumnName = "车道";
                    dt1.Columns["clsd"].ColumnName = "车辆速度";
                    dt1.Columns["sjlyms"].ColumnName = "数据来源";
                    dt1.Columns["jllxms"].ColumnName = "记录类型";
                }

                return dt1;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-ChangeDataTable", ex.Message + "；" + ex.StackTrace, "ChangeDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="page"></param>
        private void SetButState(int page, int rowcount)
        {
            try
            {
                // 第一页  获得50条记录  上一页 禁止，下一页 可用
                if (page.Equals(1) && rowcount.Equals(15))
                {
                    ButLast.Disabled = true;
                    ButNext.Disabled = false;
                }
                // 第一页  没有获得50条记录  上一页 禁止，下一页 禁止
                if (page.Equals(1) && !rowcount.Equals(15))
                {
                    ButLast.Disabled = true;
                    ButNext.Disabled = true;
                }
                // 不是第一页  获得50条记录  上一页 可用，下一页 可用
                if (!page.Equals(1) && rowcount.Equals(15))
                {
                    ButLast.Disabled = false;
                    ButNext.Disabled = false;
                }
                double d = Math.Ceiling(Convert.ToInt32(realCount.Value.ToString()) / 15.0);
                allPage.Value = Convert.ToInt32(d);
                //下一页禁止
                if (page == Convert.ToInt32(d))
                {
                    ButNext.Disabled = true;
                }
                //page++;
                LabNum.Html = "<font >&nbsp;&nbsp;当前" + page.ToString() + "页,共" + Convert.ToInt32(d.ToString()) + "页</font>";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-SetButState", ex.Message + "；" + ex.StackTrace, "SetButState has an exception");
            }
        }

        /// <summary>
        /// 展示选中页数据
        /// </summary>
        /// <param name="currentPage"></param>
        private void ShowQuery(int currentPage)
        {
            try
            {
                int rownum = GetRowNum();
                int startNum = (currentPage - 1) * rownum + 1;
                int endNum = currentPage * rownum;
                //Query(GetWhere(), startNum, endNum);
                //SetButState(currentPage);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-ShowQuery", ex.Message + "；" + ex.StackTrace, "ShowQuery has an exception");
            }
        }

        /// <summary>
        ///获得一页显示条数
        /// </summary>
        /// <returns></returns>
        private int GetRowNum()
        {
            try
            {
                string rownum = "";

                rownum = "50";

                return int.Parse(rownum);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-GetRowNum", ex.Message + "；" + ex.StackTrace, "GetRowNum has an exception");
                return 50;
            }
        }

        /// <summary>
        ///提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Notice(string title, string msg)
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

        #endregion 私有方法

        #region [DirectMethod]

        /// <summary>
        /// 行选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ApplyClick(object sender, DirectEventArgs e)
        {
            try
            {
                this.FormPanel1.Collapsed = false;
                string sdata = e.ExtraParams["data"];
                string hphm = Bll.Common.GetdatabyField(sdata, "hphm");
                string hpzl = Bll.Common.GetdatabyField(sdata, "hpzl");
                string url1 = Bll.Common.GetdatabyField(sdata, "zjwj1");
                string url2 = Bll.Common.GetdatabyField(sdata, "zjwj2");
                string url3 = Bll.Common.GetdatabyField(sdata, "zjwj3");
                if (string.IsNullOrEmpty(url2))
                {
                    url2 = NoImageUrl;
                }
                if (string.IsNullOrEmpty(url3))
                {
                    url3 = NoImageUrl;
                }

                //string js = "ShowImage(\"" +url1+ "\",\"" + url2 + "\",\"" + url3 + "\",\"" + hphm + "\",\"" + hpzl + "\");";
                string js = "ShowImage(\"" + url1 + "\",\"" + url2 + "\",\"" + url3 + "\",\"" + hphm + "\",\"" + hpzl + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-ApplyClick", ex.Message + "；" + ex.StackTrace, "ApplyClick has an exception");
            }
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
                logManager.InsertLogError("UnlicensedVehiclesQuery.aspx-GetDateTime", ex.Message + "；" + ex.StackTrace, "GetDateTime has an exception");
            }
        }

        #endregion [DirectMethod]

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

        #endregion 卡口选择
    }
}