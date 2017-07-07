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
    public partial class PassCarInfoQueryImg : System.Web.UI.Page
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
            if (!userLogin.CheckLogin(username)) { string js = "alert('" + GetLangStr("PassCarInfoQueryImg24", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }
            if (!X.IsAjaxRequest)
            {
                this.DataBind();
                try
                {
                    StoreDataBind();
                    DataSetDateTime();
                    //BuildTree(TreeStation.Root);

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
                        ////车辆品牌
                        //CmbClpp.Value = con.Clpp;
                        //CmbClpp.Text = con.ClppText;
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
                        //行驶方向
                        CmbXsfx.Value = con.Xsfx;
                        //车道
                        txtXscd.Text = con.Xscd;
                        txtDsd.Text = con.Dsd;
                        txtGsd.Text = con.Gsd;
                    }

                    this.FormPanel2.Title = GetLangStr("PassCarInfoQueryImg25", "查询结果：目前查询出符合条件的记录0条,剩余符合条件的尚未加载，请点击下一页进行加载");
                    QueryPasscar(1);
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, GetLangStr("PassCarInfoQueryImg26", "访问：过往车辆查询图片展示"), userinfo.NowIp, "0");
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex);
                    logManager.InsertLogError("PassCarInfoQueryImg.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
                }
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
                logManager.InsertLogError("PassCarInfoQueryImg.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh has an exception");
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
                logManager.InsertLogError("PassCarInfoQueryImg.aspx-TbutFisrt", ex.Message + "；" + ex.StackTrace, "TbutFisrt has an exception");
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
                logManager.InsertLogError("PassCarInfoQueryImg.aspx-TbutLast", ex.Message + "；" + ex.StackTrace, "TbutLast has an exception");
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
                logManager.InsertLogError("PassCarInfoQueryImg.aspx-TbutNext", ex.Message + "；" + ex.StackTrace, "TbutNext has an exception");
            }
        }

        //[DirectMethod]
        //public void OpenPic(int num)
        //{
        //    string[] imgs = { imgzjwj1.ImageUrl, imgzjwj2.ImageUrl, imgzjwj3.ImageUrl, imgzjwj4.ImageUrl, imgzjwj5.ImageUrl, imgzjwj6.ImageUrl, imgzjwj7.ImageUrl, imgzjwj8.ImageUrl };
        //    string js = "OpenPicPage('" + imgs[num - 1] + "');";
        //    this.ResourceManager1.RegisterAfterClientInitScript(js);
        //}

        #endregion 控件事件

        #region 私有方法

        /// <summary>
        /// 查询过车记录
        /// </summary>
        /// <param name="currentPage"></param>
        private void QueryPasscar(int currentPage)
        {
            curpage.Value = currentPage;
            string allNum = "0";
            int pagesize = 8;
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
                //if (con.Kkid.Contains("Bumen"))
                //{
                //    List<string> list = new List<string>();
                //    string[] strs = con.Kkid.Split(',');
                //    if (strs.Length > 0)
                //    {
                //        for (int i = 0; i < strs.Length; i++)
                //        {
                //            if (!strs[i].Contains("Bumen"))
                //            {
                //                list.Add(strs[i]);
                //            }
                //            else
                //            {
                //            }
                //        }
                //        string str = string.Join(",", list.ToArray());
                //        con.Kkid = str;
                //    }
                //    else
                //    {
                //        con.Kkid = "";
                //    }
                //}
                //else
                //{
                //}
                con.Clpp = tgsDataInfo.GetClzppString(con.Clpp);
                //if (con.Hphm.Contains("-"))
                //{
                //    con.Hphm = con.Hphm.Substring(0, con.Hphm.IndexOf("-"));
                //}
                string xml;

                if (Session["Screen"] != null)
                {
                    string screen = Session["Screen"].ToString();
                    if (screen.Equals("1"))
                    {
                        pagesize = 15;
                        xml = Bll.Common.GetPassCarXml(con, ((currentPage - 1) * 18).ToString(), "18");
                    }
                    else if (screen.Equals("2"))
                    {
                        pagesize = 10;
                        xml = Bll.Common.GetPassCarXml(con, ((currentPage - 1) * 10).ToString(), "10");
                    }
                    else if (screen.Equals("3"))
                    {
                        xml = Bll.Common.GetPassCarXml(con, ((currentPage - 1) * 8).ToString(), "8");
                    }
                    else
                    {
                        xml = Bll.Common.GetPassCarXml(con, ((currentPage - 1) * 8).ToString(), "8");
                    }
                }
                else
                {
                    xml = Bll.Common.GetPassCarXml(con, ((currentPage - 1) * 8).ToString(), "8");
                }

                string rexml = client.GetPassCarInfo(xml);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rexml);
                try
                {
                    if (xmlDoc.SelectSingleNode("//passcarinfolist").Attributes["totalnum"] != null)
                    {
                        allNum = xmlDoc.SelectSingleNode("//passcarinfolist").Attributes["totalnum"].Value;
                    }
                }
                catch (Exception ex)
                {
                    logManager.InsertLogError("PassCarInfoQueryImg.aspx-QueryPasscar", ex.Message + "；" + ex.StackTrace, "QueryPasscar has an exception");
                }

                if (!string.IsNullOrEmpty(rexml) && Bll.Common.GetPassCount(rexml) > 0)
                {
                    CXmlToDataTable(xmlDoc);
                }
            }
            if (dtPasscar != null && dtPasscar.Rows.Count > 0)
            {
                if (Session["datatable"] != null)
                {
                    Session["datatable"] = null;
                }
                Session["datatable"] = dtPasscar;

                //showimg();
                this.Store1.DataSource = dtPasscar;
                this.Store1.DataBind();
                if (dtPasscar.Rows.Count.Equals(pagesize)) // 有数据 且够多少条
                {
                    this.FormPanel2.Title = GetLangStr("PassCarInfoQueryImg27", "查询结果：当前查询出符合条件的记录") + allNum + GetLangStr("PassCarInfoQueryImg28", "条，当前显示8条，其余结果尚未显示，请点击 下一页 进行显示");
                }
                else
                {
                    if (currentPage.Equals(0)) // 第一页
                    {
                        this.FormPanel2.Title = GetLangStr("PassCarInfoQueryImg27", "查询结果：当前查询出符合条件的记录" + allNum + GetLangStr("PassCarInfoQueryImg29", "条"));
                    }
                    else
                    {
                        this.FormPanel2.Title = GetLangStr("PassCarInfoQueryImg27", "查询结果：当前查询出符合条件的记录") + allNum + GetLangStr("PassCarInfoQueryImg30", "条,当前显示") + dtPasscar.Rows.Count + GetLangStr("31", "条，其余结果尚未显示，请点击 上一页 进行显示");
                    }
                }
            }
            else
            {
                this.Store1.DataSource = CreateTable();
                this.Store1.DataBind();
                Notice(GetLangStr("PassCarInfoQueryImg32", "信息提示"), GetLangStr("PassCarInfoQueryImg33", "未查询到相关记录"));
            }
            SetButState(currentPage, dtPasscar.Rows.Count, Convert.ToInt32(allNum));
        }

        public DataTable CreateTable()
        {
            DataTable dt = CreatePasscarTable();
            if (Session["Screen"] != null)
            {
                string screen = Session["Screen"].ToString();
                if (screen.Equals("1"))
                {
                    for (int i = 0; i < 18; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["zjwj1"] = "../images/NoImage.png";
                        dt.Rows.Add(dr);
                    }
                }
                else if (screen.Equals("2"))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["zjwj1"] = "../images/NoImage.png";
                        dt.Rows.Add(dr);
                    }
                }
                else if (screen.Equals("3"))
                {
                    for (int i = 0; i < 8; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["zjwj1"] = "../images/NoImage.png";
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["zjwj1"] = "../images/NoImage.png";
                        dt.Rows.Add(dr);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["zjwj1"] = "../images/NoImage.png";
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        //private void showimg()
        //{
        //    for (int i = 0; i < dtPasscar.Rows.Count; i++)
        //    {
        //        switch (i)
        //        {
        //            case 0:
        //                imgzjwj1.ImageUrl = dtPasscar.Rows[0]["zjwj1"].ToString();

        //                labkkid1.Text = "卡口名称：" + dtPasscar.Rows[i]["kkmc"].ToString();
        //                labgwsj1.Text = "通过时间：" + dtPasscar.Rows[i]["gwsj"].ToString();
        //                labhphm1.Text = "号牌号码：" + dtPasscar.Rows[i]["hphm"].ToString();

        //                break;

        //            case 1:
        //                imgzjwj2.ImageUrl = dtPasscar.Rows[1]["zjwj1"].ToString();
        //                labkkid2.Text = "卡口名称：" + dtPasscar.Rows[i]["kkmc"].ToString();
        //                labgwsj2.Text = "通过时间：" + dtPasscar.Rows[i]["gwsj"].ToString();
        //                labhphm2.Text = "号牌号码：" + dtPasscar.Rows[i]["hphm"].ToString();
        //                break;

        //            case 2:
        //                imgzjwj3.ImageUrl = dtPasscar.Rows[2]["zjwj1"].ToString();
        //                labkkid3.Text = "卡口名称：" + dtPasscar.Rows[i]["kkmc"].ToString();
        //                labgwsj3.Text = "通过时间：" + dtPasscar.Rows[i]["gwsj"].ToString();
        //                labhphm3.Text = "号牌号码：" + dtPasscar.Rows[i]["hphm"].ToString();
        //                break;

        //            case 3:
        //                imgzjwj4.ImageUrl = dtPasscar.Rows[3]["zjwj1"].ToString();
        //                labkkid4.Text = "卡口名称：" + dtPasscar.Rows[i]["kkmc"].ToString();
        //                labgwsj4.Text = "通过时间：" + dtPasscar.Rows[i]["gwsj"].ToString();
        //                labhphm4.Text = "号牌号码：" + dtPasscar.Rows[i]["hphm"].ToString();
        //                break;

        //            case 4:
        //                imgzjwj5.ImageUrl = dtPasscar.Rows[4]["zjwj1"].ToString();
        //                labkkid5.Text = "卡口名称：" + dtPasscar.Rows[i]["kkmc"].ToString();
        //                labgwsj5.Text = "通过时间：" + dtPasscar.Rows[i]["gwsj"].ToString();
        //                labhphm5.Text = "号牌号码：" + dtPasscar.Rows[i]["hphm"].ToString();
        //                break;

        //            case 5:
        //                imgzjwj6.ImageUrl = dtPasscar.Rows[5]["zjwj1"].ToString();
        //                labkkid6.Text = "卡口名称：" + dtPasscar.Rows[i]["kkmc"].ToString();
        //                labgwsj6.Text = "通过时间：" + dtPasscar.Rows[i]["gwsj"].ToString();
        //                labhphm6.Text = "号牌号码：" + dtPasscar.Rows[i]["hphm"].ToString();
        //                break;

        //            case 6:
        //                imgzjwj7.ImageUrl = dtPasscar.Rows[6]["zjwj1"].ToString();
        //                labkkid7.Text = "卡口名称：" + dtPasscar.Rows[i]["kkmc"].ToString();
        //                labgwsj7.Text = "通过时间：" + dtPasscar.Rows[i]["gwsj"].ToString();
        //                labhphm7.Text = "号牌号码：" + dtPasscar.Rows[i]["hphm"].ToString();
        //                break;

        //            case 7:
        //                imgzjwj8.ImageUrl = dtPasscar.Rows[7]["zjwj1"].ToString();
        //                labkkid8.Text = "卡口名称：" + dtPasscar.Rows[i]["kkmc"].ToString();
        //                labgwsj8.Text = "通过时间：" + dtPasscar.Rows[i]["gwsj"].ToString();
        //                labhphm8.Text = "号牌号码：" + dtPasscar.Rows[i]["hphm"].ToString();
        //                break;
        //        }
        //    }
        //}

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
                    dtPasscar.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQueryImg.aspx-CXmlToDataTable", ex.Message + "；" + ex.StackTrace, "CXmlToDataTable has an exception");
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
                int pagesize = 8;
                string screen = Session["Screen"].ToString();
                if (screen.Equals("1"))
                {
                    pagesize = 18;
                }
                else if (screen.Equals("2"))
                {
                    pagesize = 10;
                }
                else if (screen.Equals("3"))
                {
                    pagesize = 8;
                }
                else
                {
                    pagesize = 8;
                }

                // 第一页  获得50条记录  上一页 禁止，下一页 可用
                if (page.Equals(1) && rowcount.Equals(pagesize))
                {
                    ButLast.Disabled = true;
                    ButNext.Disabled = false;
                }
                // 第一页  没有获得50条记录  上一页 禁止，下一页 禁止
                if (page.Equals(1) && !rowcount.Equals(pagesize))
                {
                    ButLast.Disabled = true;
                    ButNext.Disabled = true;
                }
                // 不是第一页  获得50条记录  上一页 可用，下一页 可用
                if (!page.Equals(1) && rowcount.Equals(pagesize))
                {
                    ButLast.Disabled = false;
                    ButNext.Disabled = false;
                }

                //page++;

                double d = Math.Ceiling((double)allnum / (double)pagesize);
                //下一页禁止
                if (page == Convert.ToInt32(d))
                {
                    ButLast.Disabled = false;
                    ButNext.Disabled = true;
                }
                LabNum.Html = "<font >&nbsp;&nbsp;" + GetLangStr("PassCarInfoQueryImg36", "当前") + page.ToString() + GetLangStr("PassCarInfoQueryImg35", "页,共") + d.ToString() + GetLangStr("PassCarInfoQueryImg34", "页") + "</font>";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQueryImg.aspx-SetButState", ex.Message + "；" + ex.StackTrace, "SetButState has an exception");
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
                Window win = WindowShow.AddPeccancy(sdata);
                win.Render(this.Form);
                win.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQueryImg.aspx-AddWindow", ex.Message + "；" + ex.StackTrace, "AddWindow has an exception");
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

        public static DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        [DirectMethod]
        public void SetSession()
        {
            if (Session["tree"] != null)
            {
                Session["tree"] = null;
            }
            Session["tree"] = kakouId.Value;
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
                    Notice(GetLangStr("PassCarInfoQueryImg32", "信息提示"), GetLangStr("PassCarInfoQueryImg37", "只能选择两个小时之内的时间！"));
                    this.FormPanel2.Title = GetLangStr("PassCarInfoQueryImg38", "查询结果：当前查询出符合条件的记录0条,现在显示0条");
                    LabNum.Html = "<font >&nbsp;&nbsp;" + GetLangStr("PassCarInfoQueryImg39", "当前第1页,共0页") + "</font>";
                    Store1.DataSource = CreateTable();
                    Store1.DataBind();
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
                        Notice(GetLangStr("PassCarInfoQueryImg32", "信息提示"), GetLangStr("PassCarInfoQueryImg40", "最多只能选择10个卡口！"));
                        this.FormPanel2.Title = GetLangStr("PassCarInfoQueryImg", GetLangStr("PassCarInfoQueryImg38", "查询结果：当前查询出符合条件的记录0条,现在显示0条"));
                        LabNum.Html = "<font >&nbsp;&nbsp;" + GetLangStr("PassCarInfoQueryImg39", "当前第1页,共0页") + "</font>";
                        Store1.DataSource = CreateTable();
                        Store1.DataBind();
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
                if (!string.IsNullOrEmpty(ClppChoice.Value))
                {
                    condition.Clpp = ClppChoice.Value;
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
                logManager.InsertLogError("PassCarInfoQueryImg.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
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
            haopai_name5.Value = "";
            haopai_name4.Value = "";
            haopai_name6.Value = "";
            //号牌种类
            CmbPlateType.Text = "";
            //卡口列表
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
                return condition;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQueryImg.aspx-GetQueryInfo", ex.Message + "；" + ex.StackTrace, "GetQueryInfo has an exception");
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
                logManager.InsertLogError("PassCarInfoQueryImg.aspx-AddStationTree", ex.Message + "；" + ex.StackTrace, "AddStationTree has an exception");
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
                    user.DepartName = GetLangStr("PassCarInfoQueryImg41", "滨州市交通警察支队");
                    user.DeptCode = "371600000000";
                }
                Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                nodeRoot.Text = user.DepartName;
                nodeRoot.Leaf = true;
                nodeRoot.NodeID = user.DeptCode;
                nodeRoot.Icon = Ext.Net.Icon.House;
                nodeRoot.Checked = ThreeStateBool.False;//加的部门选择框
                nodeRoot.Qtip = "Bumen";
                dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
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
                logManager.InsertLogError("PassCarInfoQueryImg.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
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
                logManager.InsertLogError("PassCarInfoQueryImg.aspx-AddDepartTree", ex.Message + "；" + ex.StackTrace, "AddDepartTree has an exception");
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
                dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
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

                //this.StoreQuery.DataSource = GetDataSource();
                //this.StoreQuery.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQueryImg.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

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
    }
}