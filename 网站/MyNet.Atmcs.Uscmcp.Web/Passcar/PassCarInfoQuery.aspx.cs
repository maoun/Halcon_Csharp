using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PassCarInfoQuery : System.Web.UI.Page
    {
        #region 成员变量

        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private SettingManager settingManager = new SettingManager();
        private UserLogin userLogin = new UserLogin();
        private DataCommon dataCommon = new DataCommon();
        private const string NoImageUrl = "../images/NoImage.png";
        private static DataTable dtPasscar = null;
        private static DataTable dtStation = null;
        private static DataTable dtXsfx = null;
        private static string starttime = "";
        private static string endtime = "";
        private static string jrwf = "";
        private static DataTable dtZfgg = null;
        private static QueryService.querypasscar client = new QueryService.querypasscar();
        private MapManager mapManager = new MapManager();

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
            if (!userLogin.CheckLogin(username)) { string js = "alert('" + GetLangStr("PassCarInfoQuery70", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }

            if (!IsPostBack)
            {
                if (!X.IsAjaxRequest)
                {
                    try
                    {
                        DataSetDateTime();
                        // BuildTree(TreeStation.Root);
                        StoreDataBind();

                        if (Session["Condition"] != null)
                        {
                            Condition con = Session["Condition"] as Condition;

                            //开始时间
                            start.InnerText = con.StartTime;
                            starttime = con.StartTime;
                            //结束时间
                            end.InnerText = con.EndTime;
                            endtime = con.EndTime;
                            //车牌号牌
                            vehicleHead.SetVehicleText(con.Sqjc);
                            if (con.QueryMode.Equals("0"))
                            {
                                pnhphm.Hidden = false;
                                TxtplateId.Hidden = true;
                                if (con.Hphm.Length < 6)
                                {
                                    int length = con.Hphm.Length;
                                    for (int i = 0; i < 6 - length; i++)
                                    {
                                        con.Hphm = con.Hphm + "_";
                                    }
                                }

                                haopai_name1.Value = con.Hphm.Substring(0, 1);
                                if (haopai_name1.Value.Equals("_"))
                                {
                                    haopai_name1.Value = "";
                                }
                                haopai_name2.Value = con.Hphm.Substring(1, 1);
                                if (haopai_name2.Value.Equals("_"))
                                {
                                    haopai_name2.Value = "";
                                }
                                haopai_name3.Value = con.Hphm.Substring(2, 1);
                                if (haopai_name3.Value.Equals("_"))
                                {
                                    haopai_name3.Value = "";
                                }
                                haopai_name4.Value = con.Hphm.Substring(3, 1);
                                if (haopai_name4.Value.Equals("_"))
                                {
                                    haopai_name4.Value = "";
                                }
                                haopai_name5.Value = con.Hphm.Substring(4, 1);
                                if (haopai_name5.Value.Equals("_"))
                                {
                                    haopai_name5.Value = "";
                                }
                                haopai_name6.Value = con.Hphm.Substring(5, 1);
                                if (haopai_name6.Value.Equals("_"))
                                {
                                    haopai_name6.Value = "";
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(con.Hphm))
                                {
                                    TxtplateId.Text = con.Hphm;
                                }
                            }
                            //TxtplateId.Text = con.Hphm;
                            //号牌种类
                            CmbPlateType.Value = con.Hpzl;
                            //卡口
                            kakou.Value = con.Kkidms;
                            kakouId.Value = con.Kkid;
                            //if (con.Kkid.Contains("Bumen"))
                            //{
                            //    List<string> list = new List<string>();
                            //    string[] strs = con.Kkid.Split(',');
                            //    string s = "";
                            //    for (int i = 0; i < strs.Length; i++)
                            //    {
                            //        if (!strs[i].Contains("Bumen"))
                            //        {
                            //            list.Add(strs[i]);
                            //        }
                            //        else
                            //        {
                            //            s = s + strs[i].Substring(5) + ",";
                            //        }
                            //    }
                            //    string str = s + string.Join(",", list.ToArray());
                            //    FieldStation.SetValue(str, con.Kkidms);
                            //}
                            //else
                            //{
                            //    FieldStation.SetValue(con.Kkid, con.Kkidms);
                            //}

                            //车身颜色
                            CmbCsys.Value = con.Csys;
                            //模糊查询
                            if (con.QueryMode == "1")
                            {
                                cktype.Checked = false;
                            }
                            else
                            {
                                cktype.Checked = true;
                            }
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

                            //CmbClpp.SetValue(con.Clpp);
                            //CmbClpp.Text = con.ClppText;
                            //this.StoreClzpp.DataSource = tgsPproperty.GetVehicleModel("a.BRANDID='" + CmbClpp.Value + "'");
                            //this.StoreClzpp.DataBind();
                            //if (!string.IsNullOrEmpty(con.ClppText))
                            //{
                            //    DataSet dsclxh = mapManager.GetClxh(con.ClppText);
                            //    if (dsclxh != null)
                            //    {
                            //        StoreClzpp.DataSource = dsclxh.Tables[0];
                            //        StoreClzpp.DataBind();
                            //    }
                            //}
                            //else
                            //{
                            //    StoreClzpp.RemoveAll();
                            //    StoreClzpp.DataBind();
                            //}
                            ////车辆子品牌
                            //CmbClzpp.SetValue(con.Clzpp);
                            //CmbClzpp.Text = con.ClzppText;
                            //CmbClzpp.Value = con.Clzpp;
                            //行驶方向
                            CmbXsfx.Value = con.Xsfx;
                            //车道
                            txtXscd.Text = con.Xscd;
                            //低速度
                            txtDsd.Text = con.Dsd;
                            //高速度
                            txtGsd.Text = con.Gsd;
                            ////短车长
                            //TextField3.Text = con.Dcc;
                            ////长车长
                            //TextField4.Text = con.Ccc;
                        }

                        dtStation = mapManager.GetStation();
                        //dtXsfx = mapManager.GetFxcode();

                        //ButCsv.Disabled = true;
                        ButExcel.Disabled = true;
                        // ButXml.Disabled = true;
                        //ButPrint.Disabled = true;
                        this.FormPanel2.Title = GetLangStr("PassCarInfoQuery31", "查询结果：目前查询出符合条件的记录0条,剩余符合条件的尚未加载，请点击下一页进行加载");
                        QueryPasscar(1);
                    }
                    catch (Exception ex)
                    {
                        ILog.WriteErrorLog(ex);
                        logManager.InsertLogError("PassCarInfoQuery.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
                    }
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, GetLangStr("PassCarInfoQuery72", "访问：过往车辆查询"), userinfo.NowIp, "0");
                }
                this.DataBind();
            }
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
                QueryPasscar((int)curpage.Value);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh has an exception");
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
                    string xml = Bll.Common.GetPrintXml(GetLangStr("PassCarInfoQuery73", "违法车辆查询信息列表"), "", "", "printdatatable");
                    string js = "OpenPrintPageH(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-ButPrintClick", ex.Message + "；" + ex.StackTrace, "ButPrintClick has an exception");
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
                QueryPasscar(1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-TbutFisrt", ex.Message + "；" + ex.StackTrace, "TbutFisrt has an exception");
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
                QueryPasscar(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-TbutLast", ex.Message + "；" + ex.StackTrace, "TbutLast has an exception");
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
                QueryPasscar(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-TbutNext", ex.Message + "；" + ex.StackTrace, "TbutNext has an exception");
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
                logManager.InsertLogError("PassCarInfoQuery.aspx-ToXml", ex.Message + "；" + ex.StackTrace, "ToXml has an exception");
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
                //Thread.Sleep(2000);
                DataTable dt = ChangeDataTable();
                ConvertData.ExportExcel(dt, this);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-ToExcel", ex.Message + "；" + ex.StackTrace, "ToExcel has an exception");
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
                logManager.InsertLogError("PassCarInfoQuery.aspx-ToCsv", ex.Message + "；" + ex.StackTrace, "ToCsv has an exception");
            }
        }

        #endregion 控件事件

        #region 私有方法

        /// <summary>
        /// 查询过车记录
        /// </summary>
        /// <param name="currentPage"></param>
        public void QueryPasscar(int currentPage)
        {
            curpage.Value = currentPage;
            int allNum = 0;
            dtPasscar = CreatePasscarTable();
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
                if (con.Kkid.Contains("Bumen"))
                {
                    List<string> list = new List<string>();
                    string[] strs = con.Kkid.Split(',');
                    if (strs.Length > 0)
                    {
                        for (int i = 0; i < strs.Length; i++)
                        {
                            if (!strs[i].Contains("Bumen"))
                            {
                                list.Add(strs[i]);
                            }
                            else
                            {
                            }
                        }
                        string str = string.Join(",", list.ToArray());
                        con.Kkid = str;
                    }
                    else
                    {
                        con.Kkid = "";
                    }
                }
                else
                {
                }
                con.Clpp = tgsDataInfo.GetClzppString(con.Clpp);
                //if (con.Hphm.Contains("-"))
                //{
                //    con.Hphm = con.Hphm.Substring(0, con.Hphm.IndexOf("-"));
                //    con.Hphm = con.Hphm + "%";
                //}
                //if (!string.IsNullOrEmpty(con.Clzpp))
                //{
                //    con.Clpp = con.Clzpp;
                //}
                string xml = Bll.Common.GetPassCarXml(con, ((currentPage - 1) * 50).ToString(), "50");
                string rexml = client.GetPassCarInfo(xml);
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(rexml);

                allNum = Bll.Common.GetRowCount(xmlDoc);
                realCount.Value = allNum.ToString();

                //string rexml = "<?xml version='1.0' encoding='UTF-8'?><Message><Version>1.0</Version><Type>RESPONSE</Type><Body><Return><passcarinfolist totalnum='3'><passcarinfo><kkid>100000010859</kkid><fxbh>04</fxbh><cdbh>10</cdbh><hphm>鲁B686EJ</hphm><hpzl>02</hpzl><gwsj>2016-02-11 13:05:26</gwsj><cshm>210</cshm><clsd>35</clsd><hpys>2</hpys><cllx>K33</cllx><clpp>大众</clpp><zxpp></zxpp><clwx>452.5*180.9*166.5</clwx><csys>I</csys><jllx>0</jllx><zjhsl></zjhsl><zjhqz></zjhqz><zybsl></zybsl><zybqz></zybqz><dzsl></dzsl><dzqz></dzqz><bjsl></bjsl><bjqz></bjqz><njbsl></njbsl><njbqz></njbqz><zjsaqd></zjsaqd><fjsaqd></fjsaqd><zjwj1>http://12345.jpg</zjwj1><zjwj2></zjwj2><zjwj3></zjwj3><tztp1></tztp1><tztp2></tztp2><tztp3></tztp3><tztp4></tztp4><lxwj></lxwj></passcarinfo><passcarinfo><kkid>201409021069</kkid><fxbh>04</fxbh><cdbh>10</cdbh><hphm>鲁B686EJ</hphm><hpzl>02</hpzl><gwsj>2016-02-11 13:00:26</gwsj><cshm>210</cshm><clsd>35</clsd><hpys>2</hpys><cllx>K33</cllx><clpp>大众</clpp><zxpp></zxpp><clwx>452.5*180.9*166.5</clwx><csys>I</csys><jllx>0</jllx><zjhsl></zjhsl><zjhqz></zjhqz><zybsl></zybsl><zybqz></zybqz><dzsl></dzsl><dzqz></dzqz><bjsl></bjsl><bjqz></bjqz><njbsl></njbsl><njbqz></njbqz><zjsaqd></zjsaqd><fjsaqd></fjsaqd><zjwj1>http://12345.jpg</zjwj1><zjwj2></zjwj2><zjwj3></zjwj3><tztp1></tztp1><tztp2></tztp2><tztp3></tztp3><tztp4></tztp4><lxwj></lxwj></passcarinfo><passcarinfo><kkid>100000010859</kkid><fxbh>04</fxbh><cdbh>10</cdbh><hphm>鲁B686EJ</hphm><hpzl>02</hpzl><gwsj>2016-02-11 13:00:26</gwsj><cshm>210</cshm><clsd>35</clsd><hpys>2</hpys><cllx>K33</cllx><clpp>大众</clpp><zxpp></zxpp><clwx>452.5*180.9*166.5</clwx><csys>I</csys><jllx>0</jllx><zjhsl></zjhsl><zjhqz></zjhqz><zybsl></zybsl><zybqz></zybqz><dzsl></dzsl><dzqz></dzqz><bjsl></bjsl><bjqz></bjqz><njbsl></njbsl><njbqz></njbqz><zjsaqd></zjsaqd><fjsaqd></fjsaqd><zjwj1>http://12345.jpg</zjwj1><zjwj2></zjwj2><zjwj3></zjwj3><tztp1></tztp1><tztp2></tztp2><tztp3></tztp3><tztp4></tztp4><lxwj></lxwj></passcarinfo></passcarinfolist></Return></Body></Message>";
                if (!string.IsNullOrEmpty(rexml) && allNum > 0)
                {
                    CXmlToDataTable(xmlDoc);
                }
            }
            if (dtPasscar != null && dtPasscar.Rows.Count > 0)
            {
                this.StorePeccancy.DataSource = Bll.Common.ChangColName(dtPasscar);
                this.StorePeccancy.DataBind();
                ButExcel.Disabled = false;
                if (Session["datatable"] != null)
                {
                    Session["datatable"] = null;
                }
                Session["datatable"] = dtPasscar;
                if (dtPasscar.Rows.Count.Equals(50)) // 有数据 且够50条
                {
                    this.FormPanel2.Title = GetLangStr("PassCarInfoQuery74", "查询结果：当前查询出符合条件的记录") + allNum.ToString() + GetLangStr("PassCarInfoQuery75", "条，当前显示50条，其余结果尚未显示，请点击 下一页 进行显示");
                }
                else
                {
                    if (currentPage.Equals(0)) // 第一页
                    {
                        this.FormPanel2.Title = GetLangStr("PassCarInfoQuery74", "查询结果：当前查询出符合条件的记录") + allNum.ToString() + GetLangStr("PassCarInfoQuery76", "条");
                    }
                    else
                    {
                        this.FormPanel2.Title = GetLangStr("PassCarInfoQuery74", "查询结果：当前查询出符合条件的记录") + allNum.ToString() + GetLangStr("PassCarInfoQuery77", "条,当前显示") + dtPasscar.Rows.Count.ToString() + GetLangStr("PassCarInfoQuery78", "条，其余结果尚未显示，请点击 上一页 进行显示");
                    }
                }
            }
            else
            {
                Notice(GetLangStr("PassCarInfoQuery79", "信息提示"), GetLangStr("PassCarInfoQuery80", "未查询到相关记录"));
                this.FormPanel2.Title = GetLangStr("PassCarInfoQuery74", "查询结果：当前查询出符合条件的记录") + allNum.ToString() + GetLangStr("PassCarInfoQuery76", "条");
                ButExcel.Disabled = true;
                this.StorePeccancy.DataSource = Bll.Common.ChangColName(dtPasscar);
                this.StorePeccancy.DataBind();
            }
            SetButState(currentPage, dtPasscar.Rows.Count, Convert.ToInt32(allNum));
        }

        /// <summary>
        /// 创建内存表
        /// </summary>
        /// <returns></returns>
        private DataTable CreatePasscarTable()
        {
            DataTable dt = new DataTable("PassCar");
            dt.Columns.Add("xh", typeof(string));
            dt.Columns.Add("kkid", typeof(string));
            dt.Columns.Add("kkmc", typeof(string));
            dt.Columns.Add("hphm", typeof(string));
            dt.Columns.Add("hpzl", typeof(string));
            dt.Columns.Add("hpzlms", typeof(string));
            dt.Columns.Add("gwsj", typeof(string));
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
                    DataRow dr = dtPasscar.NewRow();
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
                    dr["gwsj"] = (node.SelectSingleNode("gwsj")).InnerText;
                    // dr["gwsj"] = string.Format((node.SelectSingleNode("gwsj")).InnerText.ToString(), "yyyy-MM-dd HH:mm:ss");
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
                    dtPasscar.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-CXmlToDataTable", ex.Message + "；" + ex.StackTrace, "CXmlToDataTable has an exception");
            }
        }

        /// <summary>
        /// 获得一页显示条数
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
                logManager.InsertLogError("PassCarInfoQuery.aspx-GetRowNum", ex.Message + "；" + ex.StackTrace, "GetRowNum has an exception");
                return 50;
            }
        }

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="page"></param>
        private void SetButState(int page, int rowcount, int allnum)
        {
            try
            {
                // 第一页  获得50条记录  上一页 禁止，下一页 可用
                if (page.Equals(1) && rowcount.Equals(50))
                {
                    ButLast.Disabled = true;
                    ButNext.Disabled = false;
                }
                // 第一页  没有获得50条记录  上一页 禁止，下一页 禁止
                if (page.Equals(1) && !rowcount.Equals(50))
                {
                    ButLast.Disabled = true;
                    ButNext.Disabled = true;
                }
                // 不是第一页  获得50条记录  上一页 可用，下一页 可用
                if (!page.Equals(1) && rowcount.Equals(50))
                {
                    ButLast.Disabled = false;
                    ButNext.Disabled = false;
                }

                //page++;

                double d = Math.Ceiling((double)allnum / (double)50);
                //下一页禁止
                if (page == Convert.ToInt32(d))
                {
                    ButLast.Disabled = false;
                    ButNext.Disabled = true;
                }
                LabNum.Html = "<font >&nbsp;&nbsp;" + GetLangStr("PassCarInfoQuery43", "当前") + page.ToString() + GetLangStr("PassCarInfoQuery41", "页,共") + d.ToString() + GetLangStr("PassCarInfoQuery42", "页") + "</font>";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-SetButState", ex.Message + "；" + ex.StackTrace, "SetButState has an exception");
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
                string hphm = Bll.Common.GetdatabyField(sdata, "col3");
                string hpzl = Bll.Common.GetdatabyField(sdata, "col4");
                string url1 = Bll.Common.GetdatabyField(sdata, "col15");
                string url2 = Bll.Common.GetdatabyField(sdata, "col16");
                string url3 = Bll.Common.GetdatabyField(sdata, "col17");
                if (string.IsNullOrEmpty(url2))
                {
                    url2 = NoImageUrl;
                }
                if (string.IsNullOrEmpty(url3))
                {
                    url3 = NoImageUrl;
                }
                string js = "ShowImage(\"" + dataCommon.ChangePoliceIp(url1) + "\",\"" + dataCommon.ChangePoliceIp(url2) + "\",\"" + dataCommon.ChangePoliceIp(url3) + "\",\"" + hphm + "\",\"" + hpzl + "\");";
                //string js = "ShowImage(\"" + url1 + "\",\"" + url2 + "\",\"" + url3 + "\",\"" + hphm + "\",\"" + hpzl + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-ApplyClick", ex.Message + "；" + ex.StackTrace, "ApplyClick has an exception");
            }
        }

        /// <summary>
        /// 行选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void DirectApplyClick(DirectEventArgs e)
        {
            try
            {
                this.FormPanel1.Collapsed = false;
                string sdata = e.ExtraParams["data"];
                string hphm = Bll.Common.GetdatabyField(sdata, "col3");
                string hpzl = Bll.Common.GetdatabyField(sdata, "col4");
                string url1 = Bll.Common.GetdatabyField(sdata, "col15");
                string url2 = Bll.Common.GetdatabyField(sdata, "col16");
                string url3 = Bll.Common.GetdatabyField(sdata, "col17");
                if (string.IsNullOrEmpty(url2))
                {
                    url2 = NoImageUrl;
                }
                if (string.IsNullOrEmpty(url3))
                {
                    url3 = NoImageUrl;
                }
                string js = "ShowImage(\"" + dataCommon.ChangePoliceIp(url1) + "\",\"" + dataCommon.ChangePoliceIp(url2) + "\",\"" + dataCommon.ChangePoliceIp(url3) + "\",\"" + hphm + "\",\"" + hpzl + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-DirectApplyClick", ex.Message + "；" + ex.StackTrace, "DirectApplyClick has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        private string GetUrl(DataTable dt, string idx)
        {
            try
            {
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][1].ToString() == idx)
                        {
                            return dt.Rows[i][0].ToString();
                        }
                    }
                    return "";
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-GetUrl", ex.Message + "；" + ex.StackTrace, "GetUrl has an exception");
                return "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sdata"></param>
        private void AddWindow(string sdata)
        {
            try
            {
                string page = "MyNet.Atmcs.Uscmcp.Web.PassCarInfoQuery";
                Window win = WindowShow.AddPasscCar(sdata);

                win.Render(this.Form);
                win.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-AddWindow", ex.Message + "；" + ex.StackTrace, "AddWindow has an exception");
            }
        }

        /// <summary>
        ///显示详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowDetails(object sender, DirectEventArgs e)
        {
            try
            {
                string data = jrwf = e.ExtraParams["data"];
                string field = e.ExtraParams["field"];
                AddWindow(data);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-ShowDetails", ex.Message + "；" + ex.StackTrace, "ShowDetails has an exception");
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
                    dt1.Columns.Remove("col0"); dt1.Columns.Remove("col1"); dt1.Columns.Remove("col4");
                    dt1.Columns.Remove("col7"); dt1.Columns.Remove("col11");
                    dt1.Columns.Remove("col13");
                    for (int i = 15; i < dt.Columns.Count; i++)
                    {
                        if (!i.Equals(20))
                        {
                            dt1.Columns.Remove("col" + i.ToString());
                        }
                    }
                    dt1.Columns["col2"].SetOrdinal(0); dt1.Columns["col3"].SetOrdinal(1);
                    dt1.Columns["col5"].SetOrdinal(2); dt1.Columns["col6"].SetOrdinal(3);
                    dt1.Columns["col8"].SetOrdinal(4); dt1.Columns["col9"].SetOrdinal(5);
                    dt1.Columns["col10"].SetOrdinal(6); dt1.Columns["col12"].SetOrdinal(7); dt1.Columns["col14"].SetOrdinal(8);
                    dt1.Columns[0].ColumnName = "卡口名称";
                    dt1.Columns[1].ColumnName = "号牌号码";
                    dt1.Columns[2].ColumnName = "号牌种类";
                    dt1.Columns[3].ColumnName = "过往时间";
                    dt1.Columns[4].ColumnName = "行驶方向";
                    dt1.Columns[5].ColumnName = "车道编号";
                    dt1.Columns[6].ColumnName = "车辆速度";
                    dt1.Columns[7].ColumnName = "数据来源";
                    dt1.Columns[8].ColumnName = "记录类型";
                    //PrintColumns pc = new PrintColumns();
                    //pc.Add(new PrintColumn("违法地点", 8));
                    //pc.Add(new PrintColumn("号牌号码", 3));
                    //pc.Add(new PrintColumn("号牌种类", 2));
                    //pc.Add(new PrintColumn("违法时间", 6));
                    //pc.Add(new PrintColumn("违法行为", 5));
                    //pc.Add(new PrintColumn("通知状态", 20));
                    //pc.Add(new PrintColumn("速度限速", 12));
                    //pc.Add(new PrintColumn("数据来源", 13));
                    //pc.Add(new PrintColumn("所属机构", 14));
                    //dt2 = Bll.Common.GetDataTablePrint(dt, pc);
                }

                return dt1;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-ChangeDataTable", ex.Message + "；" + ex.StackTrace, "ChangeDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        ///  转换查询条件
        /// </summary>
        /// <param name="isALL"></param>
        /// <param name="xzjb"></param>
        /// <param name="jgjb"></param>
        /// <param name="depcode"></param>
        /// <returns></returns>
        public string ConvertCondition(bool isALL, string xzjb, string jgjb, string depcode)
        {
            try
            {
                string strWhere = "";
                if (xzjb == "1")
                {
                    if (isALL == true)
                    {
                        if (jgjb == "2")
                        {
                            strWhere = "and substr(cjjg,0,4)='" + depcode.Substring(0, 4) + "'";
                        }
                        else if (jgjb == "3")
                        {
                            strWhere = "and substr(cjjg,0,8)='" + depcode.Substring(0, 8) + "'";
                        }
                        else if (jgjb == "4")
                        {
                            strWhere = "and cjjg='" + depcode + "'";
                        }
                    }
                    else
                    {
                        if (jgjb == "2")
                        {
                            strWhere = "and substr(cjjg,0,4)='" + depcode.Substring(0, 4) + "'";
                        }
                        else if (jgjb == "3")
                        {
                            strWhere = "and substr(cjjg,0,8)='" + depcode.Substring(0, 8) + "'";
                        }
                        else if (jgjb == "4" || jgjb == "1")
                        {
                            strWhere = "and cjjg='" + depcode + "'";
                        }
                    }
                }
                else if (xzjb == "2")
                {
                    if (isALL == true)
                    {
                        if (jgjb == "2")
                        {
                            strWhere = "and substr(cjjg,0,4)='" + depcode.Substring(0, 4) + "'";
                        }
                        else if (jgjb == "3")
                        {
                            strWhere = "and substr(cjjg,0,8)='" + depcode.Substring(0, 8) + "'";
                        }
                        else if (jgjb == "4")
                        {
                            strWhere = "and cjjg='" + depcode + "'";
                        }
                    }
                    else
                    {
                        if (jgjb == "3")
                        {
                            strWhere = "and substr(cjjg,0,8)='" + depcode.Substring(0, 8) + "'";
                        }
                        else if (jgjb == "4" || jgjb == "2")
                        {
                            strWhere = "and cjjg='" + depcode + "'";
                        }
                    }
                }
                else if (xzjb == "3")
                {
                    if (isALL == true)
                    {
                        if (jgjb == "3")
                        {
                            strWhere = "and substr(cjjg,0,8)='" + depcode.Substring(0, 8) + "'";
                        }
                        else if (jgjb == "4")
                        {
                            strWhere = "and cjjg='" + depcode + "'";
                        }
                    }
                    else
                    {
                        if (jgjb == "4" || jgjb == "3")
                        {
                            strWhere = "and cjjg='" + depcode + "'";
                        }
                    }
                }
                else if (xzjb == "4")
                {
                    strWhere = "and cjjg='" + depcode + "'";
                }

                return strWhere;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-ConvertCondition", ex.Message + "；" + ex.StackTrace, "ConvertCondition has an exception");
                return null;
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
                Icon = Ext.Net.Icon.Information,
                HideDelay = 2000,
                Height = 120,
                Html = "<br></br>" + msg + "!"
            });
        }

        /// <summary>
        /// 行选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        public void DirectApplyClick(string hphm, string hpzl, string url1, string url2, string url3)
        {
            try
            {
                this.FormPanel1.Collapsed = false;

                if (string.IsNullOrEmpty(url2))
                {
                    url2 = NoImageUrl;
                }
                if (string.IsNullOrEmpty(url3))
                {
                    url3 = NoImageUrl;
                }
                string js = "ShowImage(\"" + dataCommon.ChangePoliceIp(url1) + "\",\"" + dataCommon.ChangePoliceIp(url2) + "\",\"" + dataCommon.ChangePoliceIp(url3) + "\",\"" + hphm + "\",\"" + hpzl + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-DirectApplyClick", ex.Message + "；" + ex.StackTrace, "DirectApplyClick has an exception");
            }
        }

        [DirectMethod(Namespace = "OnEvl")]
        public void showmap()
        {
            this.Window1.Reload();
            Session["stationlist"] = null;
            Session["stationlistname"] = null;
            this.Window1.Show();
        }

        [DirectMethod(Namespace = "OnEvl")]
        public void hidemap()
        {
            string nodeid = "";
            string nodeName = "";
            this.Window1.Hide();
            if (Session["stationlist"] != null)
            {
                System.Collections.Generic.List<string> listid = Session["stationlist"] as System.Collections.Generic.List<string>;
                foreach (string str in listid)
                {
                    nodeid += (nodeid == "" ? "" : ",") + str;
                }
            }
            if (Session["stationlistname"] != null)
            {
                System.Collections.Generic.List<string> listName = Session["stationlistname"] as System.Collections.Generic.List<string>;
                foreach (string str in listName)
                {
                    nodeName += (nodeName == "" ? "" : ",") + str;
                }
            }
            string js = "setSelect('" + nodeid + "','" + nodeName + "');";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }

        #endregion 私有方法

        #region 智慧查询

        /// <summary>
        /// 展示政府公告图片
        /// </summary>
        [DirectMethod]
        public void GetZfgg()
        {
            string wfxwid = X.GetCmp<ComboBox>("cmbZfgg").SelectedItem.Value;//得到公告id
            if (!string.IsNullOrEmpty(wfxwid))
            {
                DataRow[] rows = dtZfgg.Select("col0='" + wfxwid + "'");
                if (rows.Length > 0)
                {
                    if (!string.IsNullOrEmpty(rows[0]["col3"].ToString()))
                    {
                        X.GetCmp<ComboBox>("SouthPanel2").Html = WindowShow.GetHtml(rows[0]["col3"].ToString());
                    }
                    else
                    {
                        X.GetCmp<ComboBox>("SouthPanel2").Html = WindowShow.GetHtml(NoImageUrl);
                    }
                }
            }
        }

        /// <summary>
        /// 加入违法
        /// </summary>
        [DirectMethod]
        public void Jrwf()
        {
            try
            {
                string wfxwid = X.GetCmp<ComboBox>("cmbZfgg").SelectedItem.Value;
                if (string.IsNullOrEmpty(wfxwid))
                {
                    Notice(GetLangStr("PassCarInfoQuery79", "信息提示"), GetLangStr("PassCarInfoQuery45", "请选择政府公告图片"));
                    return;
                }
                if (!string.IsNullOrEmpty(jrwf))
                {
                    Hashtable hs = new Hashtable();
                    //序号
                    hs.Add("xh", DateTime.Now.ToString("yyyyMMddHHmmssfff") + SunjiNumber());
                    //号牌种类
                    if (!string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col4")))
                    {
                        hs.Add("hpzl", Bll.Common.GetdatabyField(jrwf, "col4"));
                    }
                    //号牌号码
                    if (!string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col3")))
                    {
                        hs.Add("hphm", Bll.Common.GetdatabyField(jrwf, "col3"));
                    }
                    //违法时间   col6为过车时间
                    if (!string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col6", "")))
                    {
                        hs.Add("wfsj", Bll.Common.GetdatabyField(jrwf, "col6"));
                    }
                    //方向编号
                    if (!string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col7")))
                    {
                        hs.Add("fxbh", Bll.Common.GetdatabyField(jrwf, "col7"));
                    }
                    //车道编号
                    if (!string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col9")))
                    {
                        hs.Add("cdbh", Bll.Common.GetdatabyField(jrwf, "col9"));
                    }

                    //车辆速度
                    if (!string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col10")))
                    {
                        hs.Add("clsd", Bll.Common.GetdatabyField(jrwf, "col10"));
                    }
                    //数据来源
                    if (!string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col11")))
                    {
                        hs.Add("sjly", Bll.Common.GetdatabyField(jrwf, "col11"));
                    }
                    //图片一
                    if (!string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col15")))
                    {
                        hs.Add("zjwj1", Bll.Common.GetdatabyField(jrwf, "col15"));
                    }
                    //图片二
                    if (!string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col16")))
                    {
                        hs.Add("zjwj2", Bll.Common.GetdatabyField(jrwf, "co116"));
                    }
                    //图片三
                    if (!string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col17")))
                    {
                        hs.Add("zjwj3", Bll.Common.GetdatabyField(jrwf, "col17"));
                    }

                    //违法地点编号
                    if (!string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col1")))
                    {
                        hs.Add("wfdd", Bll.Common.GetdatabyField(jrwf, "col1"));
                    }
                    //违法地点名称
                    if (!string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col1")))
                    {
                        DataRow[] listdr = dtStation.Select("STATION_ID= '" + Bll.Common.GetdatabyField(jrwf, "col1") + "'");
                        if (listdr.Length > 0)
                        {
                            hs.Add("wfdz", listdr[0]["STATION_NAME"].ToString());
                        }
                        else
                        {
                        }
                    }
                    //卡口id
                    if (!string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col1")))
                    {
                        hs.Add("kkid", Bll.Common.GetdatabyField(jrwf, "col1"));
                    }
                    //采集机关
                    if (!string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col1")))
                    {
                        string departid = "";
                        DataRow[] rows = dtStation.Select("station_id='" + Bll.Common.GetdatabyField(jrwf, "col1") + "'");
                        if (rows.Length > 0)
                        {
                            if (!string.IsNullOrEmpty(rows[0]["departid"].ToString()))
                            {
                                departid = rows[0]["departid"].ToString();
                                hs.Add("cjjg", departid);
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                    }

                    //采集时间
                    hs.Add("cjsj", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //采集用户
                    UserInfo user = Session["userinfo"] as UserInfo;
                    if (user != null)
                    {
                        hs.Add("cjyh", user.UserCode);
                    }

                    // string wfxw = X.GetCmp<ComboBox>("cmbZfgg").Text;

                    if (!string.IsNullOrEmpty(wfxwid))
                    {
                        DataRow[] rows = dtZfgg.Select("col0='" + wfxwid + "'");
                        if (rows.Length > 0)
                        {
                            if (!string.IsNullOrEmpty(rows[0]["col7"].ToString()))
                            {
                                string wfxw = rows[0]["col7"].ToString();
                                hs.Add("wfxw", wfxw);
                            }
                            if (!string.IsNullOrEmpty(rows[0]["col3"].ToString()))
                            {
                                //图片一
                                if (string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col15")))
                                {
                                    hs.Add("zjwj1", rows[0]["col3"].ToString());
                                }
                                //图片二
                                else if (string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col16")))
                                {
                                    hs.Add("zjwj2", rows[0]["col3"].ToString());
                                }
                                //图片三
                                else if (string.IsNullOrEmpty(Bll.Common.GetdatabyField(jrwf, "col17")))
                                {
                                    hs.Add("zjwj3", rows[0]["col3"].ToString());
                                }
                                else
                                {
                                    string zjwj4 = rows[0]["col3"].ToString();
                                    //图片四
                                    hs.Add("zjwj4", zjwj4);
                                }
                            }
                        }
                    }
                    hs.Add("wfnr", "");
                    int result = tgsDataInfo.InsertPeccancy(hs);//调用违法业务插入数据
                    string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                    if (ipaddress.Length < 9)
                    {
                        ipaddress = "127.0.0.1";
                    }
                    if (result > 0)
                    {
                        DataRow[] rows = dtZfgg.Select("col0='" + wfxwid + "'");
                        logManager.InsertLogRunning(UserLogin.GetUserName(), GetLangStr("PassCarInfoQuery46", "违法加入成功：号牌种类[") + Bll.Common.GetdatabyField(jrwf, "col5") + GetLangStr("PassCarInfoQuery47", "] ,号牌号码[") + Bll.Common.GetdatabyField(jrwf, "col3") + GetLangStr("PassCarInfoQuery50", "] ,违法行为[") + rows[0]["col7"].ToString() + GetLangStr("PassCarInfoQuery51", "] ,新加入的违法记录Id[") + hs["xh"].ToString() + "]" + GetLangStr("PassCarInfoQuery49", " ,加入时间[") + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]", ipaddress, "9", Bll.Common.GetdatabyField(jrwf, "col3"), hs["xh"].ToString());
                        Notice(GetLangStr("PassCarInfoQuery79", "信息提示"), GetLangStr("PassCarInfoQuery52", "加入违法成功"));
                    }
                    else
                    {
                        DataRow[] rows = dtZfgg.Select("col0='" + wfxwid + "'");
                        logManager.InsertLogRunning(UserLogin.GetUserName(), GetLangStr("PassCarInfoQuery53", "违法加入失败：号牌种类[") + Bll.Common.GetdatabyField(jrwf, "col5") + GetLangStr("PassCarInfoQuery47", "] ,号牌号码[") + Bll.Common.GetdatabyField(jrwf, "col3") + GetLangStr("PassCarInfoQuery50", "] ,违法行为[") + rows[0]["col7"].ToString() + "] ," + GetLangStr("PassCarInfoQuery54", " ,加入时间[") + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]", ipaddress, "9", Bll.Common.GetdatabyField(jrwf, "col3"), hs["xh"].ToString());
                        Notice(GetLangStr("PassCarInfoQuery79", "信息提示"), GetLangStr("PassCarInfoQuery55", "加入违法失败"));
                    }
                }
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("PassCarInfoQuery.aspx-DoYes", ex.Message + "；" + ex.StackTrace, "Jrwf has an exception");
            }
        }

        /// <summary>
        /// 得到一个随机数
        /// </summary>
        /// <returns></returns>
        public string SunjiNumber()
        {
            Random ran = new Random();
            int RandKey = ran.Next(10000, 99999);
            return RandKey.ToString();
        }

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
        ///显示Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetWindow(object sender, EventArgs e)
        {
            this.Window1.Reload();
            Session["stationlist"] = null;
            Session["stationlistname"] = null;
            this.Window1.Show();
        }

        /// <summary>
        ///获取选中时间
        /// </summary>
        /// <param name="isstart"></param>
        /// <param name="strtime"></param>
        [DirectMethod]
        public void GetDateTime(bool isstart, string strtime)
        {
            if (isstart)
                starttime = strtime;
            else
                endtime = strtime;
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            if (string.IsNullOrEmpty(CmbPlateType.Text) && string.IsNullOrEmpty(TxtplateId.Text) && string.IsNullOrEmpty(vehicleHead.VehicleText) &&
                  string.IsNullOrEmpty(kakou.Value) && string.IsNullOrEmpty(haopai_name1.Value) && string.IsNullOrEmpty(haopai_name2.Value) && string.IsNullOrEmpty(haopai_name3.Value)
                  && string.IsNullOrEmpty(haopai_name4.Value) && string.IsNullOrEmpty(haopai_name5.Value) && string.IsNullOrEmpty(haopai_name6.Value) && cktype.Checked == false
                  )
            {
                DateTime start = Convert.ToDateTime(starttime);
                DateTime end = Convert.ToDateTime(endtime);
                TimeSpan sp = end.Subtract(start);
                if (sp.TotalMinutes > 120)
                {
                    Notice(GetLangStr("PassCarInfoQuery79", "信息提示"), GetLangStr("PassCarInfoQuery56", "只能选择两个小时之内的时间！"));
                    this.FormPanel2.Title = GetLangStr("PassCarInfoQuery57", "查询结果：当前查询出符合条件的记录0条,现在显示0条");
                    LabNum.Html = "<font >&nbsp;&nbsp;" + GetLangStr("PassCarInfoQuery58", "当前第1页,共0页") + "</font>";
                    StorePeccancy.DataSource = new DataTable();
                    StorePeccancy.DataBind();
                    return;
                }
            }
            if ((!string.IsNullOrEmpty(CmbPlateType.Text) && (!string.IsNullOrEmpty(vehicleHead.VehicleText) && !string.IsNullOrEmpty(TxtplateId.Text))) ||
               (!string.IsNullOrEmpty(CmbPlateType.Text) && cktype.Checked == true)
               )
            {
            }
            else if (!string.IsNullOrEmpty(kakou.Value))
            {
                if (kakou.Value.Contains(","))
                {
                    string[] strs = kakou.Value.Split(',');
                    if (strs.Length > 10)
                    {
                        Notice(GetLangStr("PassCarInfoQuery79", "信息提示"), GetLangStr("PassCarInfoQuery59", "最多只能选择10个卡口！"));
                        this.FormPanel2.Title = GetLangStr("PassCarInfoQuery57", "查询结果：当前查询出符合条件的记录0条,现在显示0条");
                        LabNum.Html = "<font >&nbsp;&nbsp;" + GetLangStr("PassCarInfoQuery58", "当前第1页,共0页") + "</font>";
                        StorePeccancy.DataSource = new DataTable();
                        StorePeccancy.DataBind();
                        return;
                    }
                }
            }
            else
            {
            }
            if (Session["Condition"] != null)
            {
                Session["Condition"] = null;
            }
            try
            {
                Condition condition = new Condition();
                if (!string.IsNullOrEmpty(starttime))
                {
                    condition.StartTime = DateTime.Parse(starttime).ToString("yyyy-MM-dd HH:mm:ss");//starttime  不能直接写这个不然会出错
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    condition.EndTime = DateTime.Parse(endtime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (!string.IsNullOrEmpty(vehicleHead.VehicleText))
                {
                    condition.Sqjc = vehicleHead.VehicleText;
                }

                if (cktype.Checked)
                {
                    condition.QueryMode = "0";
                    condition.Hphm = (string.IsNullOrEmpty(haopai_name1.Value) ? "_" : haopai_name1.Value) +
                    (string.IsNullOrEmpty(haopai_name2.Value) ? "_" : haopai_name2.Value) +
                    (string.IsNullOrEmpty(haopai_name3.Value) ? "_" : haopai_name3.Value) +
                    (string.IsNullOrEmpty(haopai_name4.Value) ? "_" : haopai_name4.Value) +
                    (string.IsNullOrEmpty(haopai_name5.Value) ? "_" : haopai_name5.Value) +
                    (string.IsNullOrEmpty(haopai_name6.Value) ? "_" : haopai_name6.Value);
                    //if (condition.Hphm.Substring(0, 6) == "______")
                    //    condition.Hphm = "%";
                }
                else
                {
                    condition.QueryMode = "1";
                    if (!string.IsNullOrEmpty(TxtplateId.Text))
                    {
                        condition.Hphm = TxtplateId.Text;
                    }
                }
                //号牌种类
                if (CmbPlateType.SelectedIndex != -1)
                {
                    condition.Hpzl = CmbPlateType.SelectedItem.Value;
                }
                //if (CmbClzpp.SelectedIndex != -1)
                //{
                //    condition.Clzpp = CmbClzpp.SelectedItem.Value;
                //    condition.ClzppText = CmbClzpp.SelectedItem.Text;
                //}
                //if (CmbClpp.SelectedIndex != -1)
                //{
                //    condition.Clpp = CmbClpp.SelectedItem.Value;
                //    condition.ClppText = CmbClpp.SelectedItem.Text;
                //    //子品牌赋值
                //    //condition.Clzpp = CmbClzpp.SelectedItem.Value;
                //}
                if (!string.IsNullOrEmpty(ClppChoice.Value))
                {
                    condition.Clpp = ClppChoice.Value;
                }
                if (CmbCsys.SelectedIndex != -1)
                {
                    condition.Csys = CmbCsys.SelectedItem.Value;
                }
                if (CmbXsfx.SelectedIndex != -1)
                {
                    condition.Xsfx = CmbXsfx.SelectedItem.Value;
                }
                if (!string.IsNullOrEmpty(txtXscd.Text))
                {
                    condition.Xscd = txtXscd.Text;
                }
                if (!string.IsNullOrEmpty(this.kakou.Value))
                {
                    string kkid = this.kakouId.Value.ToString();
                    if (!string.IsNullOrEmpty(kkid))
                    {
                        condition.Kkid = kkid;
                        if (Session["tree"] != null)
                        {
                            Session["tree"] = null;
                        }
                        Session["tree"] = kkid;
                    }
                    condition.Kkidms = this.kakou.Value;
                }
                //得到低速度
                if (!string.IsNullOrEmpty(txtDsd.Text))
                {
                    condition.Dsd = txtDsd.Text;
                }
                //得到高速度
                if (!string.IsNullOrEmpty(txtGsd.Text))
                {
                    condition.Gsd = txtGsd.Text;
                }
                ////短车长
                //if (!string.IsNullOrEmpty(TextField3.Text))
                //{
                //    condition.Dcc = TextField3.Text;
                //}
                ////长车长
                //if (!string.IsNullOrEmpty(TextField4.Text))
                //{
                //    condition.Ccc = TextField4.Text;
                //}
                Session["Condition"] = condition;
                QueryPasscar(1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
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
            starttime = DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss");
            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeJs = "clearTime('" + starttime + "','" + endtime + "');";//js方法后面的分号一定要加上
            this.ResourceManager1.RegisterAfterClientInitScript(timeJs);
            //车牌号牌
            vehicleHead.SetVehicleText("");
            TxtplateId.Text = "";
            haopai_name1.Value = "";
            haopai_name2.Value = "";
            haopai_name3.Value = "";
            haopai_name4.Value = "";
            haopai_name5.Value = "";
            haopai_name6.Value = "";
            //号牌种类
            CmbPlateType.Text = "";
            //CmbPlateType.Triggers[0].Icon=TriggerIcon.Empty;
            //CmbCsys.Triggers[0].HideTrigger = true;
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
            //车身颜色
            CmbCsys.Text = "";
            //模糊查询
            cktype.Checked = false;
            ////车辆品牌
            //CmbClpp.Text = "";
            ////车辆子品牌
            //CmbClzpp.Text = "";
            //车辆品牌
            //车辆品牌
            ClppChoice.Value = "";
            clzpp.Value = "";
            clpp.Value = "";
            //行驶方向
            CmbXsfx.Text = "";
            //车速  低速度与高速度
            txtDsd.Text = "";
            txtGsd.Text = "";
            ////车长区间
            //TextField3.Text = "";//短
            //TextField4.Text = "";//长
            //车道
            txtXscd.Text = "";
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private Condition GetQueryInfo()
        {
            try
            {
                Condition condition = new Condition();
                if (!string.IsNullOrEmpty(starttime))
                {
                    condition.StartTime = DateTime.Parse(starttime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    condition.EndTime = DateTime.Parse(endtime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (!string.IsNullOrEmpty(vehicleHead.VehicleText))
                {
                    condition.Sqjc = vehicleHead.VehicleText;
                }
                if (cktype.Checked)
                {
                    condition.QueryMode = "0";
                    condition.Hphm = (string.IsNullOrEmpty(haopai_name1.Value) ? "_" : haopai_name1.Value) +
                    (string.IsNullOrEmpty(haopai_name2.Value) ? "_" : haopai_name2.Value) +
                    (string.IsNullOrEmpty(haopai_name3.Value) ? "_" : haopai_name3.Value) +
                    (string.IsNullOrEmpty(haopai_name4.Value) ? "_" : haopai_name4.Value) +
                    (string.IsNullOrEmpty(haopai_name5.Value) ? "_" : haopai_name5.Value) +
                    (string.IsNullOrEmpty(haopai_name6.Value) ? "_" : haopai_name6.Value);
                    //if (condition.Hphm.Substring(0, 6) == "______")
                    //    condition.Hphm = "%";
                }
                else
                {
                    condition.QueryMode = "1";
                    if (!string.IsNullOrEmpty(TxtplateId.Text))
                    {
                        condition.Hphm = TxtplateId.Text;
                    }
                }
                if (CmbPlateType.SelectedIndex != -1)
                {
                    condition.Hpzl = CmbPlateType.SelectedItem.Value;
                }
                if (string.IsNullOrEmpty(ClppChoice.Value))
                {
                    condition.Clpp = ClppChoice.Value;
                }

                if (CmbCsys.SelectedIndex != -1)
                {
                    condition.Csys = CmbCsys.SelectedItem.Value;
                }
                if (CmbXsfx.SelectedIndex != -1)
                {
                    condition.Xsfx = CmbXsfx.SelectedItem.Value;
                }
                if (!string.IsNullOrEmpty(txtXscd.Text))
                {
                    condition.Xscd = txtXscd.Text;
                }
                //if (this.FieldStation.Value != null)
                //{
                //    string kkid = this.FieldStation.Value.ToString();
                //    if (!string.IsNullOrEmpty(kkid))
                //    {
                //        condition.Kkid = kkid;
                //    }
                //    condition.Kkidms = this.FieldStation.Text;
                //}
                if (!string.IsNullOrEmpty(this.kakou.Value))
                {
                    string kkid = this.kakouId.Value.ToString();
                    if (!string.IsNullOrEmpty(kkid))
                    {
                        condition.Kkid = kkid;
                        if (Session["tree"] != null)
                        {
                            Session["tree"] = null;
                        }
                        Session["tree"] = kkid;
                    }
                    condition.Kkidms = this.kakou.Value;
                }
                //得到低速度
                if (!string.IsNullOrEmpty(txtDsd.Text))
                {
                    condition.Dsd = txtDsd.Text;
                }
                //得到高速度
                if (!string.IsNullOrEmpty(txtGsd.Text))
                {
                    condition.Gsd = txtGsd.Text;
                }
                ////短车长
                //if (!string.IsNullOrEmpty(TextField3.Text))
                //{
                //    condition.Dcc = TextField3.Text;
                //}
                ////长车长
                //if (!string.IsNullOrEmpty(TextField4.Text))
                //{
                //    condition.Ccc = TextField4.Text;
                //}
                return condition;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-GetQueryInfo", ex.Message + "；" + ex.StackTrace, "GetQueryInfo has an exception");
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
                        node.Qtip = "Kakou";
                        DepartNode.Nodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-AddStationTree", ex.Message + "；" + ex.StackTrace, "AddStationTree has an exception");
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
                root.Text = GetLangStr("PassCarInfoQuery60", "卡口列表");
                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;

                // 添加 自己机构节点 和卡口
                UserInfo user = Session["userinfo"] as UserInfo;
                if (user == null)
                {
                    user = new UserInfo();
                    user.DepartName = GetLangStr("PassCarInfoQuery61", "滨州市交通警察支队");
                    user.DeptCode = "371600000000";
                }

                Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                nodeRoot.Text = user.DepartName;
                nodeRoot.Leaf = true;
                nodeRoot.NodeID = user.DeptCode;
                nodeRoot.Icon = Ext.Net.Icon.House;
                nodeRoot.Qtip = "Bumen";
                nodeRoot.Checked = ThreeStateBool.False;//加的部门选择框
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
                logManager.InsertLogError("PassCarInfoQuery.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
                return null;
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
                        nodeRoot.Checked = ThreeStateBool.False;//加的部门选择框
                        nodeRoot.Qtip = "Bumen";
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
                logManager.InsertLogError("PassCarInfoQuery.aspx-AddDepartTree", ex.Message + "；" + ex.StackTrace, "AddDepartTree has an exception");
            }
        }

        /// <summary>
        /// 设置初始时间
        /// </summary>
        private void DataSetDateTime()
        {
            starttime = DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss");
            start.InnerText = starttime;

            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            end.InnerText = endtime;
        }

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                starttime = "";
                endtime = "";
                DataTable dt1 = GetRedisData.GetData("t_sys_code:140001");
                if (dt1 != null)
                {
                    this.StorePlateType.DataSource = Bll.Common.ChangColName(dt1);
                    this.StorePlateType.DataBind();
                }
                else
                {
                    this.StorePlateType.DataSource = tgsPproperty.GetPalteType();
                    this.StorePlateType.DataBind();
                }

                this.storekk.DataSource = tgsPproperty.GetAllStationInfo();
                this.storekk.DataBind();

                DataTable dt2 = GetRedisData.GetData("t_sys_code:240022");
                if (dt2 != null)
                {
                    this.StoreDataSource.DataSource = Bll.Common.ChangColName(dt2);// tgsPproperty.GetDeviceTypeDict("240022");
                    this.StoreDataSource.DataBind();
                }
                else
                {
                    this.StoreDataSource.DataSource = tgsPproperty.GetDeviceTypeDict("240022");
                    this.StoreDataSource.DataBind();
                }

                DataTable dt3 = GetRedisData.GetData("t_sys_code:240013");
                //车身颜色
                if (dt3 != null)
                {
                    this.StoreCsys.DataSource = Bll.Common.ChangColName(dt3);
                    this.StoreCsys.DataBind();
                }
                else
                {
                    this.StoreCsys.DataSource = tgsPproperty.GetCarColorDict();
                    this.StoreCsys.DataBind();
                }
                DataTable dt4 = GetRedisData.GetData("t_sys_code:240025");
                //行驶方向
                if (dt4 != null)
                {
                    this.StoreXsfx.DataSource = dtXsfx = Bll.Common.ChangColName(dt4);
                    this.StoreXsfx.DataBind();
                }
                else
                {
                    this.StoreXsfx.DataSource = dtXsfx = tgsPproperty.GetDirectionDict();
                    this.StoreXsfx.DataBind();
                }
                DataTable dt5 = GetRedisData.GetData("t_cfg_department");
                if (dt5 != null)
                {
                    this.StoreCjjg.DataSource = Bll.Common.ChangColName(dt5);//tgsPproperty.GetDepartmentDict();
                    this.StoreCjjg.DataBind();
                }
                else
                {
                    this.StoreCjjg.DataSource = Bll.Common.ChangColName(tgsPproperty.GetDepartmentDict());
                    this.StoreCjjg.DataBind();
                }
                //政府公告
                this.StoreZfgg.DataSource = dtZfgg = Bll.Common.ChangColName(tgsDataInfo.GetZfgg());
                this.StoreZfgg.DataBind();
                //this.StoreQuery.DataSource = GetDataSource();
                //this.StoreQuery.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 转换查询模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void changtype(object sender, EventArgs e)
        {
            TxtplateId.Hidden = cktype.Checked;
            pnhphm.Hidden = !cktype.Checked;
        }

        #endregion 智慧查询

        #region 多语言转换

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

        #endregion 多语言转换
    }
}