using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace MyNet.Atmcs.Uscmcp.Web.Passcar
{
    public partial class OneCarMulLisence : System.Web.UI.Page
    {
        #region 变量

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private MyNet.Atmcs.Uscmcp.Bll.PasscarManager bll = new MyNet.Atmcs.Uscmcp.Bll.PasscarManager();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private static QueryService.querypasscar client = new QueryService.querypasscar();
        private static DataTable Dt_Station = new DataTable();
        private static DataTable Dt_passcar = new DataTable();
        private static DataTable Dt_xsfx = new DataTable();
        private static DataTable dtCsys = new DataTable();
        private static string startdate = "", enddate = "";
        private UserLogin userLogin = new UserLogin();
        private const string NoImageUrl = "../images/NoImage.png";

        #endregion 变量

        #region 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"]; if (!userLogin.CheckLogin(username)) { string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }
            try
            {
                if (!X.IsAjaxRequest)
                {
                    Dt_passcar = GetData();
                    InitStation();
                    ButEnable(true);
                    //BuildTree(TreeStation.Root);
                    curpage.Value = "1";
                    if (Session["Condition"] != null)
                    {
                        Condition con = Session["Condition"] as Condition;
                        start.InnerText = con.StartTime;
                        end.InnerText = con.EndTime;
                        startdate = con.StartTime;
                        enddate = con.EndTime;
                        cbocllx.Value = con.Hpzl;
                        cbocsys.Value = con.Csys;
                        //cbostation.Value = con.Kkid;
                        kakou.Value = con.Kkidms;
                        kakouId.Value = con.Kkid;
                        cboxNjb.Checked = con.Njb;
                        cboxZjh.Checked = con.Zjh;
                        cboxZyb.Checked = con.Zyb;
                        cboxDz.Checked = con.Dz;
                        cboxBj.Checked = con.Bj;
                        // cboclpp.Value = con.Clpp;
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
                        ButQueryClick();
                    }
                }
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "访问：一车多牌", userinfo.NowIp, "0");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
                logManager.InsertLogError("OneCarMulLisence.aspx-ApplyClick", ex.Message + "；" + ex.StackTrace, "ApplyClick has an exception");
            }
        }

        /// <summary>
        /// 获取起止时间
        /// </summary>
        /// <param name="isstart">时间类型（true开始时间false结束时间）</param>
        /// <param name="strtime">时间</param>
        [DirectMethod]
        public void GetDateTime(bool isstart, string strtime)
        {
            if (isstart)
                startdate = strtime;
            else
                enddate = strtime;
        }

        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                //QueryPasscar((int)curpage.Value);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneCarMulLisence.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh has an exception");
            }
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        [DirectMethod]
        public void ButQueryClick()
        {
            if (string.IsNullOrEmpty(kakou.Value) && string.IsNullOrEmpty(cbocllx.Text))
            {
                DateTime start = Convert.ToDateTime(startdate);
                DateTime end = Convert.ToDateTime(enddate);
                TimeSpan sp = end.Subtract(start);
                if (sp.TotalMinutes > 120)
                {
                    Notice("信息提示", "只能选择两个小时之内的时间！");
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";
                    totalpage.Value = "0";
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
            curpage.Value = 1;
            Query(1);
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
                Query(1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneCarMulLisence.aspx-TbutFisrt", ex.Message + "；" + ex.StackTrace, "TbutFisrt has an exception");
            }
        }

        /// <summary>
        /// 尾页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutEnd(object sender, DirectEventArgs e)
        {
            try
            {
                curpage.Value = (int.Parse(totalpage.Value.ToString())).ToString();
                Query(int.Parse(totalpage.Value.ToString()));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneCarMulLisence.aspx-TbutEnd", ex.Message + "；" + ex.StackTrace, "TbutEnd has an exception");
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
                if (page < 0)
                {
                    page = 0;
                }
                curpage.Value = page;
                Query(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneCarMulLisence.aspx-TbutLast", ex.Message + "；" + ex.StackTrace, "TbutLast has an exception");
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
                page++;
                curpage.Value = page;
                Query(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneCarMulLisence.aspx-TbutNext", ex.Message + "；" + ex.StackTrace, "TbutNext has an exception");
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
                    string xml = Bll.Common.GetPrintXml("违法车辆查询信息列表", "", "", "printdatatable");
                    string js = "OpenPrintPageH(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneCarMulLisence.aspx-ButPrintClick", ex.Message + "；" + ex.StackTrace, "ButPrintClick has an exception");
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
                logManager.InsertLogError("OneCarMulLisence.aspx-ToXml", ex.Message + "；" + ex.StackTrace, "ToXml has an exception");
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
                logManager.InsertLogError("OneCarMulLisence.aspx-ToExcel", ex.Message + "；" + ex.StackTrace, "ToExcel has an exception");
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
                logManager.InsertLogError("OneCarMulLisence.aspx-ToCsv", ex.Message + "；" + ex.StackTrace, "ToCsv has an exception");
            }
        }

        #endregion 事件

        #region 方法

        /// <summary>
        ///转换datatable
        /// </summary>
        /// <returns></returns>
        private DataTable ChangeDataTable()
        {
            try
            {
                DataTable dt1 = Dt_passcar.Copy();
                if (dt1 != null)
                {
                    dt1.Columns[0].ColumnName = "详情";
                    dt1.Columns[1].ColumnName = "卡口名称";
                    dt1.Columns[2].ColumnName = "号牌号码";
                    dt1.Columns[3].ColumnName = "号牌种类";
                    dt1.Columns[4].ColumnName = "车身颜色";
                    dt1.Columns[5].ColumnName = "过往时间";
                    dt1.Columns[6].ColumnName = "行车方向";
                    dt1.Columns[7].ColumnName = "车道";
                    dt1.Columns[8].ColumnName = "车辆速度";
                    dt1.Columns[9].ColumnName = "数据来源";
                    dt1.Columns[10].ColumnName = "记录类型";
                    dt1.Columns[11].ColumnName = "车辆品牌";
                }

                return dt1;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneCarMulLisence.aspx-ChangeDataTable", ex.Message + "；" + ex.StackTrace, "ChangeDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        /// 初始化监测点
        /// </summary>
        private void InitStation()
        {
            try
            {
                DataTable dt2 = GetRedisData.GetData("Station:t_cfg_set_station_type_istmsshow");//

                if (dt2 != null)
                {
                    this.station.DataSource = Dt_Station = Bll.Common.ChangColName(dt2);
                    this.station.DataBind();
                }
                else
                {
                    Dt_Station = dt2 = Bll.Common.ChangColName(bll.GetStation().Tables[0]);
                }
                DataTable dt3 = GetRedisData.GetData("t_sys_code:240025");
                //行驶方向
                if (dt3 != null)
                {
                    Dt_xsfx = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240025"));
                }
                else
                {
                    Dt_xsfx = Bll.Common.ChangColName(bll.GetFxcode());
                }
                dtCsys = GetRedisData.GetData("t_sys_code:240013"); //bll.GetCsys();
                if (dtCsys != null)
                {
                    this.csys.DataSource = Bll.Common.ChangColName(dtCsys);
                    this.csys.DataBind();
                }
                else
                {
                    dtCsys = Bll.Common.ChangColName(bll.GetCsys().Tables[0]);
                }
                DataTable dt1 = GetRedisData.GetData("t_sys_code:140001"); //bll.GetCllx();
                if (dt1 != null)
                {
                    this.cllx.DataSource = Bll.Common.ChangColName(dt1);
                    this.cllx.DataBind();
                }
                else
                {
                    dt1 = Bll.Common.ChangColName(bll.GetCllx().Tables[0]);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneCarMulLisence.aspx-InitStation", ex.Message + "；" + ex.StackTrace, "InitStation has an exception");
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="page">页码</param>
        private void Query(int page)
        {
            try
            {
                int startrow = 0, len = 15, endrow = 15;
                string cllx = cbocllx.Text;
                // string kkid = cbostation.Text;
                string kkid = "";
                string clpp = ClppChoice.Value == null ? "" : ClppChoice.Value.ToString(), clxh = cbocllx.Value == null ? "" : cbocllx.Value.ToString(), csys = cbocsys.Value == null ? "" : cbocsys.Value.ToString();
                string zjhsl = cboxZjh.Checked ? "1" : "", zybsl = cboxZyb.Checked ? "1" : "", dzsl = cboxDz.Checked ? "1" : "", njbsl = cboxNjb.Checked ? "1" : "", bjsl = cboxBj.Checked ? "1" : "";
                //if (cbostation.Value != null)
                //    kkid = cbostation.Value.ToString();
                if (this.kakouId.Value != null)
                {
                    kkid = this.kakouId.Value;
                }
                if (page == 1)
                {
                    startrow = 0;
                }
                else
                {
                    startrow = (page - 1) * len;
                }
                clpp = tgsDataInfo.GetClzppString(clpp);
                string xml = getxml(startrow, endrow, startdate, enddate, kkid, cllx, csys, clpp, clxh, zjhsl, zybsl, dzsl, njbsl, bjsl);
                string rsxml = client.GetPassCarInfo(xml);
                Dt_passcar.Clear();
                CXmlToDataTable(rsxml);
                int lens = getlenxml(rsxml);
                realCount.Value = lens;
                totalpage.Value = Math.Ceiling(double.Parse(lens.ToString()) / Convert.ToInt32(len)).ToString();
                if (rsxml != "" && lens > 0)
                {
                    this.lblCurpage.Text = (Convert.ToInt32(curpage.Value.ToString())).ToString();
                    this.lblAllpage.Text = totalpage.Value.ToString();
                    this.lblRealcount.Text = realCount.Value.ToString();
                }
                else
                {
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";
                    //GridPanel2.Title = "当前0页,共0页";
                    totalpage.Value = "0";
                }
                if (Dt_passcar != null && Dt_passcar.Rows.Count > 0)
                {
                    Store2.DataSource = Dt_passcar;
                    Store2.DataBind();
                    SetButState(Convert.ToInt32(curpage.Value));
                }
                else
                {
                    Notice("信息提示", "未查询到相关记录");
                    Store2.RemoveAll();
                    Store2.DataBind();
                    ButEnable(true);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneCarMulLisence.aspx-Query", ex.Message + "；" + ex.StackTrace, "Query has an exception");
            }
        }

        /// <summary>
        /// 按钮有效设置
        /// </summary>
        /// <param name="enable">状态</param>
        private void ButEnable(bool enable)
        {
            ButLast.Disabled = enable;
            ButNext.Disabled = enable;
            //ButCsv.Disabled = enable;
            //ButExcel.Disabled = enable;
            //ButXml.Disabled = enable;
            //ButPrint.Disabled = enable;
            //ButFirst.Disabled = enable;
            //ButEnd.Disabled = enable;
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
                int allpage = int.Parse(totalpage.Value.ToString());

                if (allpage > 1)
                {
                    ButLast.Disabled = false;
                    ButNext.Disabled = false;
                    //ButFirst.Disabled = false;
                    //ButEnd.Disabled = false;
                }
                if (page == 1)
                {
                    ButLast.Disabled = true;
                    // ButFirst.Disabled = true;
                }
                if (page == allpage)
                {
                    ButNext.Disabled = true;
                    // ButEnd.Disabled = true;
                }
                if (allpage <= 1)
                {
                    //ButFirst.Disabled = true;
                    ButNext.Disabled = true;
                    ButLast.Disabled = true;
                    // ButEnd.Disabled = true;
                    page = 0;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneCarMulLisence.aspx-SetButState", ex.Message + "；" + ex.StackTrace, "SetButState has an exception");
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
                logManager.InsertLogError("OneCarMulLisence.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
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
                logManager.InsertLogError("OneCarMulLisence.aspx-AddStationTree", ex.Message + "；" + ex.StackTrace, "AddStationTree has an exception");
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
                logManager.InsertLogError("OneCarMulLisence.aspx-AddDepartTree", ex.Message + "；" + ex.StackTrace, "AddDepartTree has an exception");
            }
        }

        /// <summary>
        /// xml解析
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        public void CXmlToDataTable(string xmlStr)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);

                XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/passcarinfolist/passcarinfo");
                foreach (XmlNode node in listNodes)
                {
                    DataRow dr = Dt_passcar.NewRow();
                    dr["hphm"] = (node.SelectSingleNode("hphm")).InnerText;
                    dr["hpzl"] = (node.SelectSingleNode("hpzl")).InnerText;
                    dr["hpzlms"] = Bll.Common.GetHpzlms((node.SelectSingleNode("hpzl")).InnerText);
                    dr["kkid"] = (node.SelectSingleNode("kkid")).InnerText;
                    DataRow[] listdr = Dt_Station.Select("col0= '" + (node.SelectSingleNode("kkid")).InnerText + "'");
                    if (listdr.Length > 0)
                    {
                        dr["lkmc"] = listdr[0]["col1"].ToString();
                        dr["kkid"] = listdr[0]["col0"].ToString();
                    }
                    else
                    {
                        dr["lkmc"] = "";
                        dr["kkid"] = (node.SelectSingleNode("kkid")).InnerText;
                    }
                    DataRow[] listdrfx = Dt_xsfx.Select("col0= '" + (node.SelectSingleNode("fxbh")).InnerText + "'");
                    if (listdrfx.Length > 0)
                        dr["fxmc"] = listdrfx[0]["col1"].ToString();
                    else
                        dr["fxmc"] = "";
                    dr["fxbh"] = (node.SelectSingleNode("fxbh")).InnerText;

                    dr["cdbh"] = (node.SelectSingleNode("cdbh")).InnerText;
                    dr["gwsj"] = (node.SelectSingleNode("gwsj")).InnerText;
                    dr["clpp"] = Bll.Common.Changenull((node.SelectSingleNode("clpp")).InnerText);
                    dr["csys"] = (node.SelectSingleNode("csys")).InnerText;
                    DataRow[] dtCsysrow = dtCsys.Select("col0= '" + (node.SelectSingleNode("csys")).InnerText + "'");
                    if (dtCsysrow.Length > 0)
                        dr["csysmc"] = dtCsysrow[0]["col1"].ToString();
                    else
                        dr["csysmc"] = "";
                    dr["clsd"] = (node.SelectSingleNode("clsd")).InnerText;
                    dr["zjwj1"] = (node.SelectSingleNode("zjwj1")).InnerText;
                    dr["zjwj2"] = (node.SelectSingleNode("zjwj2")).InnerText;
                    dr["zjwj3"] = (node.SelectSingleNode("zjwj3")).InnerText;
                    dr["bz"] = "";
                    dr["sjly"] = "";
                    dr["jllx"] = (node.SelectSingleNode("jllx")).InnerText;
                    dr["jllxmc"] = Bll.Common.GetJllxms((node.SelectSingleNode("jllx")).InnerText);
                    Dt_passcar.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("OneCarMulLisence.aspx-CXmlToDataTable", ex.Message + "；" + ex.StackTrace, "CXmlToDataTable has an exception");
            }
        }

        /// <summary>
        /// xml查询条件组织
        /// </summary>
        /// <param name="rowstart"></param>
        /// <param name="len"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="kkid"></param>
        /// <param name="cllx"></param>
        /// <param name="csys"></param>
        /// <param name="clpp"></param>
        /// <param name="clxh"></param>
        /// <param name="zjhsl"></param>
        /// <param name="zybsl"></param>
        /// <param name="dzsl"></param>
        /// <param name="njbsl"></param>
        /// <param name="bjsl"></param>
        /// <returns></returns>
        private string getxml(int rowstart, int len, string start, string end, string kkid, string cllx, string csys, string clpp, string clxh, string zjhsl, string zybsl, string dzsl, string njbsl, string bjsl)
        {
            try
            {
                string username = ""; string usercode = ""; string userip = ""; string dyzgnmkmc = ""; string dyzgnmkbh = "";
                if (Session["userinfo"] != null)
                {
                    username = (Session["userinfo"] as UserInfo).UserName;//用户名称
                    usercode = (Session["userinfo"] as UserInfo).UserCode;//用户编号
                }
                string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                if (ipaddress.Length < 9)
                {
                    ipaddress = "127.0.0.1";
                }
                userip = ipaddress;//用户Ip地址
                dyzgnmkmc = PasscarAllQuery.dyzgnmkmc;//功能模块名称
                dyzgnmkbh = PasscarAllQuery.dyzgnmkbh;//功能模块编号
                string xml = "<?xml version='1.0' encoding='UTF-8'?>" +
                            "<Message>" +
                                     "<Version>版本号</Version>" +
                                     "<Type>REQUEST</Type>" +
                                     "<Body>" +
                                         "<Cmd>" +
                                            "<kkid>" + kkid + "</kkid>" +
                                            "<fxbh></fxbh>" +
                                            "<cdbh></cdbh>" +
                                            "<hphm></hphm>" +
                                            "<hpzl>" + cllx + "</hpzl>" +
                                            "<kssj>" + start + "</kssj>" +
                                            "<jssj>" + end + "</jssj>" +
                                            "<clpp>" + clpp + "</clpp>" +
                                            "<clsd></clsd>" +
                                            "<csys>" + csys + "</csys>" +
                                            "<jllx></jllx>" +
                                            "<zjhsl>" + zjhsl + "</zjhsl>" +
                                            "<zybsl>" + zybsl + "</zybsl>" +
                                            "<dzsl>" + dzsl + "</dzsl>" +
                                            "<bjsl>" + bjsl + "</bjsl>" +
                                            "<njbsl>" + njbsl + "</njbsl>" +
                                            "<zjsaqd></zjsaqd>" +
                                            "<fjsaqd></fjsaqd>" +
                                            "<begnum>" + rowstart.ToString() + "</begnum>" +
                                            "<num>" + len.ToString() + "</num>" +
                                        "</Cmd>" +
                                          "<LogInfo>" +
                                    "<userName>" + username + "</userName>" +
                                    "<userIp>" + userip + "</userIp>" +
                                    "<userCode>" + usercode + "</userCode>" +
                                    "<dyzgnmkbh>" + dyzgnmkbh + "</dyzgnmkbh>" +
                                    "<dyzgnmkmc>" + dyzgnmkmc + "</dyzgnmkmc>" +
                                "</LogInfo>" +
                                   "</Body>" +
                               "</Message>";
                return xml;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("OneCarMulLisence.aspx-getxml", ex.Message + "；" + ex.StackTrace, "getxml has an exception");
                return "";
            }
        }

        private string getxml(int rowstart, int len, string start, string end, string cllx, string clxh, string clpp, string csys, string hphm, string kkid)
        {
            string xml = "<?xml version='1.0' encoding='UTF-8'?>" +
                "<Message>" +
                         "<Version>版本号</Version>" +
                         "<Type>REQUEST</Type>" +
                         "<Body>" +
                             "<Cmd>" +
                                "<kkid>" + kkid + "</kkid>" +
                                "<fxbh></fxbh>" +
                                "<cdbh></cdbh>" +
                                "<hphm>" + hphm + "</hphm>" +
                                "<hpzl>" + cllx + "</hpzl>" +
                                "<kssj>" + start + "</kssj>" +
                                "<jssj>" + end + "</jssj>" +
                                "<clpp>" + clpp + "</clpp>" +
                                "<clsd></clsd>" +
                                "<csys>" + csys + "</csys>" +
                                "<jllx></jllx>" +
                                "<zjhsl></zjhsl>" +
                                "<zybsl></zybsl>" +
                                "<dzsl></dzsl>" +
                                "<bjsl></bjsl>" +
                                "<njbsl></njbsl>" +
                                "<zjsaqd></zjsaqd>" +
                                "<fjsaqd></fjsaqd>" +
                                "<begnum>" + rowstart.ToString() + "</begnum>" +
                                "<num>" + len.ToString() + "</num>" +
                            "</Cmd>" +
                       "</Body>" +
                   "</Message>";
            return xml;
        }

        /// <summary>
        /// 解析xml获取记录条数
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        /// <returns></returns>
        public int getlenxml(string xmlStr)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);

                XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/passcarinfolist");
                return int.Parse(listNodes[0].Attributes[0].Value);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("OneCarMulLisence.aspx-getlenxml", ex.Message + "；" + ex.StackTrace, "getlenxml has an exception");
                return 0;
            }
        }

        /// <summary>
        /// 提示框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">内容</param>
        private void Notice(string title, string msg)
        {
            Notification.Show(new NotificationConfig
            {
                Title = title,
                Icon = Ext.Net.Icon.Information,
                HideDelay = 2000,
                Height = 120,
                Html = "<br></br>" + msg + "!"
            });
        }

        /// <summary>
        /// passcar数据结构
        /// </summary>
        /// <returns></returns>
        private DataTable GetData()
        {
            DataTable dt = new DataTable("PassCar");
            dt.Columns.Add("bz", Type.GetType("System.String"));
            dt.Columns.Add("lkmc", Type.GetType("System.String"));
            dt.Columns.Add("hphm", Type.GetType("System.String"));
            dt.Columns.Add("hpzl", Type.GetType("System.String"));
            dt.Columns.Add("hpzlms", Type.GetType("System.String"));
            dt.Columns.Add("cllx", Type.GetType("System.String"));
            dt.Columns.Add("csys", Type.GetType("System.String"));
            dt.Columns.Add("csysmc", Type.GetType("System.String"));
            dt.Columns.Add("gwsj", Type.GetType("System.String"));
            dt.Columns.Add("fxmc", Type.GetType("System.String"));
            dt.Columns.Add("cdbh", Type.GetType("System.String"));
            dt.Columns.Add("clsd", Type.GetType("System.String"));
            dt.Columns.Add("sjly", Type.GetType("System.String"));
            dt.Columns.Add("jllx", Type.GetType("System.String"));
            dt.Columns.Add("jllxmc", Type.GetType("System.String"));
            dt.Columns.Add("clpp", Type.GetType("System.String"));
            dt.Columns.Add("kkid", Type.GetType("System.String"));
            dt.Columns.Add("fxbh", Type.GetType("System.String"));
            dt.Columns.Add("zjwj1", Type.GetType("System.String"));
            dt.Columns.Add("zjwj2", Type.GetType("System.String"));
            dt.Columns.Add("zjwj3", Type.GetType("System.String"));

            return dt;
        }

        #endregion 方法

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

        public static DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        #endregion 卡口选择
    }
}