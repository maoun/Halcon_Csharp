using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.Script.Serialization;
using System.Xml;

namespace MyNet.Atmcs.Uscmcp.Web.Map
{
    public partial class FrequentOverCar : System.Web.UI.Page
    {
        #region 变量

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private MapManager bll = new MapManager();

        /// <summary>
        /// 起止时间
        /// </summary>
        private static string startdate = "", enddate = "";

        /// <summary>
        /// 监测点
        /// </summary>
        private static DataTable Dt_Station = new DataTable();

        /// <summary>
        /// 过车数据
        /// </summary>
        private static DataTable Dt_passcar = new DataTable();

        /// <summary>
        /// 查询结果
        /// </summary>
        private static DataTable Dt_result = new DataTable();

        /// <summary>
        /// 行车方向
        /// </summary>
        private static DataTable Dt_xsfx = new DataTable();

        private static QueryService.querypasscar client = new QueryService.querypasscar();
        private UserLogin userLogin = new UserLogin();

        #endregion 变量

        #region 事件

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("FrequentOverCar43", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                try
                {
                    this.ResourceManager1.RegisterAfterClientInitScript("GetScreen();");
                    Dt_Station = bll.GetStation();
                    Dt_passcar = GetData();
                    Dt_result = GetResultData();
                    Dt_xsfx = GetRedisData.GetData("t_sys_code:240025"); //bll.GetFxcode();
                    this.cllx.DataSource = GetRedisData.GetData("t_sys_code:140001");
                    this.cllx.DataBind();
                    this.csys.DataSource = GetRedisData.GetData("t_sys_code:240013");
                    this.csys.DataBind();

                    //DataSet dsclpp = bll.GetClpp();
                    //if (dsclpp != null && dsclpp.Tables[0].Rows.Count > 0)
                    //{
                    //    this.Clpp.DataSource = dsclpp.Tables[0];
                    //    this.Clpp.DataBind();
                    //}
                    start.InnerText = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    end.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    startdate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    enddate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    this.DataBind();
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, GetLangStr("FrequentOverCar44", "访问：") + Request.QueryString["funcname"], userinfo.NowIp, "0");
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex);
                    logManager.InsertLogError("FrequentOverCar.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
                }
            }
        }

        protected void showpath(object sender, EventArgs e)
        {
            try
            {
                string js = "MenuItemClick('PathCarQuery.aspx.');";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FrequentOverCar.aspx-showpath", ex.Message + "；" + ex.StackTrace, "showpath has an exception");
            }
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Query_event(object sender, DirectEventArgs e)
        {
            ButQueryClick();
        }

        [DirectMethod]
        public void clpp_chang()
        {
            //if (cboclpp.Value != null)
            //{
            //    DataSet dsclxh = bll.GetClxh(cboclpp.SelectedItem.Text);
            //    if (dsclxh != null)
            //    {
            //        Clxh.DataSource = dsclxh.Tables[0];
            //        Clxh.DataBind();

            //        return;
            //    }
            //}
            //Clxh.RemoveAll();
            //Clxh.DataBind();
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        [DirectMethod]
        public void ButQueryClick()
        {
            wingcjl.Hidden = true;
            winhphm.Hidden = true;
            ButAddgrid.Enabled = false;
            curpage.Value = 0;
            query(0);

            ButAddgrid.Enabled = true;
        }

        private string gethpzl(string hpzl)
        {
            DataTable dthpzl = GetRedisData.GetData("t_sys_code:140001");
            if (dthpzl != null)
            {
                DataRow[] drs = dthpzl.Select("code='" + hpzl + "'");
                if (drs != null && drs.Length > 0)
                    return drs[0]["codedesc"].ToString();
            }
            return "";
        }

        /// <summary>
        /// 行选择事件
        /// </summary>
        /// <param name="xypoint">坐标</param>
        /// <param name="hphm">号牌号码</param>
        [DirectMethod]
        public void SelectRow(string xypoint, string hphm, string clpp, string csys, string hpzl, string clzt)
        {
            ShowLine(xypoint);
            strhphm = hphm;
            strhpzl = hpzl;
            DataRow[] drs = Dt_passcar.Select("hphm='" + hphm + "'");
            DataTable dt = GetData();
            foreach (DataRow dr in drs)
            {
                dt.Rows.Add(dr.ItemArray);
            }
            winhphm.Title = hphm;
            clppwin.Text = clpp;
            csyswin.Text = csys;
            hpzlwin.Text = (hpzl != "" ? gethpzl(hpzl) : "");

            winhphm.Show();
            wingcjl.Title = hphm + GetLangStr("FrequentOverCar45", "_过车记录");
            wingcjl.Show();
            if (dt != null && dt.Rows.Count > 0)
            {
                Storegcjl.DataSource = dt;
                Storegcjl.DataBind();
                this.ResourceManager1.RegisterAfterClientInitScript("isScroll()");
            }
            else
            {
                Storegcjl.RemoveAll();
                Storegcjl.DataBind();
            }
            RowSelectionModel sm1 = this.GridRoadManager.SelectionModel.Primary as RowSelectionModel;
            sm1.SelectedRows.Clear();
            sm1.UpdateSelection();
        }

        private static string strhpzl = "", strhphm = "";

        [DirectMethod]
        public void ShowMoreInfo(string x, string y, string hphm, string gcsj, string kkmc, string url, string clwz)
        {
            string js = "var _gcsj ;var _type;var _hphm; _type='IMAGE'; _gcsj='" + gcsj + "';_hphm='" + hphm + "';_clwz='" + clwz + "';BMAP.addmarkerstation(false,'../Map/img/COM.gif','" + x + "','" + y + "','" + kkmc + "','" + url + "',{ gcsj:_gcsj,hphm: _hphm, type: _type,clwz:_clwz });";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
            //lblPassInfo.Text = hphm + "    在过往时间：" + gcsj+"  通过卡口："+  kkmc;
            //winPassInfo.Show();
        }

        [DirectMethod]
        public void showwin(string url, string title)
        {
            var win = new Window
            {
                ID = "Window1",
                Title = title,
                Width = 1000,
                Height = 600,
                Modal = true,
                Collapsible = true,
                Maximizable = true,
                Hidden = true
            };
            //switch (title)
            //{
            //    case "车辆轨迹":
            //        url += "?&hphm=" + strhphm + "&hpzl=" + strhpzl + "&startTime=" + startdate + "&endTime=" + enddate + "";
            //        break;
            //    case "违法信息查询":
            //    case "过车记录查询":
            //    case "落脚点":
            Condition condition = new Condition();
            if (!string.IsNullOrEmpty(startdate))
            {
                condition.StartTime = DateTime.Parse(startdate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(enddate))
            {
                condition.EndTime = DateTime.Parse(enddate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(strhphm))
            {
                condition.Sqjc = strhphm.Trim().Substring(0, 1);
                condition.Hphm = strhphm.Trim().Substring(1);
            }
            if (!string.IsNullOrEmpty(strhpzl))
            {
                condition.Hpzl = strhpzl;
            }
            condition.QueryMode = "1";
            Session["Condition"] = condition;
            //        break;
            //}
            win.AutoLoad.Url = url;
            win.AutoLoad.Mode = LoadMode.IFrame;
            win.Render(this.Form);
            win.Show();
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
        /// 单击地图触发事件
        /// </summary>
        /// <param name="points">坐标</param>
        [DirectMethod]
        public void AddPosPoints(string points)
        {
            QueryMarkArea(points);
        }

        /// <summary>
        /// 首页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutFisrt(object sender, DirectEventArgs e)
        {
            try
            {
                curpage.Value = 0;
                query(0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FrequentOverCar.aspx-TbutFisrt", ex.Message + "；" + ex.StackTrace, "TbutFisrt has an exception");
            }
        }

        /// <summary>
        /// 尾页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutEnd(object sender, DirectEventArgs e)
        {
            try
            {
                curpage.Value = (int.Parse(totalpage.Value.ToString()) - 1).ToString();
                query(int.Parse(totalpage.Value.ToString()) - 1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FrequentOverCar.aspx-TbutEnd", ex.Message + "；" + ex.StackTrace, "TbutEnd has an exception");
            }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
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
                query(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FrequentOverCar.aspx-TbutLast", ex.Message + "；" + ex.StackTrace, "TbutLast has an exception");
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutNext(object sender, DirectEventArgs e)
        {
            try
            {
                int page = int.Parse(curpage.Value.ToString());
                if (page < int.Parse(totalpage.Value.ToString()))
                    page++;
                curpage.Value = page;
                query(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FrequentOverCar.aspx-TbutNext", ex.Message + "；" + ex.StackTrace, "TbutNext has an exception");
            }
        }

        #endregion 事件

        #region 方法

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="page">页数</param>
        private void query(int page)
        {
            if (string.IsNullOrEmpty(startdate) || string.IsNullOrEmpty(enddate))
            { Notice(GetLangStr("FrequentOverCar46", "提示"), GetLangStr("FrequentOverCar47", "请选择开始时间及结束时间！")); return; }
            List<string> listkkid = new List<string>();
            RowSelectionModel sm = this.kkgp.SelectionModel.Primary as RowSelectionModel;
            if (sm.SelectedRows.Count <= 0)
            {
                Notice(GetLangStr("FrequentOverCar46", "提示"), GetLangStr("FrequentOverCar48", "请选择要进行比对的卡口！"));
                return;
            }

            string cllx = cbocllx.Text;
            string csys = cbocsys.Text;
            string clpp = ClppChoice.Value.ToString();
            string hphm = "";
            hphm = (cboplate.VehicleText == "" ? "_" : cboplate.VehicleText) +
                (string.IsNullOrEmpty(haopai_name1.Value) ? "_" : haopai_name1.Value) +
                (string.IsNullOrEmpty(haopai_name2.Value) ? "_" : haopai_name2.Value) +
                    (string.IsNullOrEmpty(haopai_name3.Value) ? "_" : haopai_name3.Value) +
                    (string.IsNullOrEmpty(haopai_name4.Value) ? "_" : haopai_name4.Value) +
                    (string.IsNullOrEmpty(haopai_name5.Value) ? "_" : haopai_name5.Value) +
                    (string.IsNullOrEmpty(haopai_name6.Value) ? "_" : haopai_name6.Value);
            if (hphm.Substring(1, 6) == "______")
                hphm = cboplate.VehicleText == "" ? "" : cboplate.VehicleText + "%";
            string gccs = cbotimes.Text;

            foreach (SelectedRow row in sm.SelectedRows)
            {
                listkkid.Add(row.RecordID);
            }
            int startrow = 0, len = 15, endrow = 15;
            startrow = startrow + len * page;
            endrow = endrow + len * page;
            string kkid = "";
            foreach (string id in listkkid)
            {
                kkid += (kkid == "" ? "" : ",") + id;
            }
            string xml = getxml(startrow, endrow, startdate, enddate, hphm, cllx, clpp, csys, kkid, gccs);
            string rsxml = "";
            try
            {
                rsxml = client.GetFrequentlyPassCarInfo(xml);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FrequentOverCar.aspx-query", ex.Message + "；" + ex.StackTrace, "query has an exception");
            }
            Dt_passcar.Clear();
            Dt_result.Clear();
            int allCount = getlenxml(rsxml);
            //大数据查询接口
            if (!string.IsNullOrEmpty(rsxml) && allCount > 0)
            {
                CXmlToDataTable(rsxml);
                if (Dt_result != null && Dt_result.Rows.Count > 0)
                {
                    this.StoreInfo.DataSource = Dt_result;
                    this.StoreInfo.DataBind();
                    labpage.Text = GetLangStr("FrequentOverCar49", "共") + allCount + GetLangStr("FrequentOverCar50", "条记录，当前") + (page + 1).ToString() + GetLangStr("FrequentOverCar51", "页,共") + totalpage.Value.ToString() + GetLangStr("FrequentOverCar52", "页");
                    ShowLine("");
                }
                else
                {
                    this.StoreInfo.RemoveAll();
                    this.StoreInfo.DataBind();
                    labpage.Text = GetLangStr("FrequentOverCar53", "共0条记录，当前0页,共0页");

                    string js = "BMAP.Clear();BMAP.ClearLine()";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                    Notice(GetLangStr("FrequentOverCar46", "提示"), GetLangStr("FrequentOverCar54", "没有符合条件的数据！"));
                }
            }
            else
            {
                this.StoreInfo.RemoveAll();
                this.StoreInfo.DataBind();
                labpage.Text = GetLangStr("FrequentOverCar53", "共0条记录，当前0页,共0页");
                string js = "BMAP.Clear();BMAP.ClearLine()";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
                Notice(GetLangStr("FrequentOverCar46", "提示"), GetLangStr("FrequentOverCar54", "没有符合条件的数据！"));
            }
        }

        /// <summary>
        /// 展示路线
        /// </summary>
        /// <param name="xypoint">坐标</param>
        private void ShowLine(string xypoint)
        {
            try
            {
                string js = "BMAP.Clear();BMAP.ClearLine();BMAP.ClearCircle();;";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
                if (Dt_result != null)
                {
                    foreach (DataRow dr in Dt_result.Rows)
                    {
                        string pos = dr["xypoint"].ToString();
                        if (pos == "")
                            continue;
                        if (dr["xypoint"].ToString().Split('|').Length > 1)
                            js = "BMAP.addPolyline2('#ff0000','" + dr["xypoint"].ToString() + "','');";
                        else
                            js = "BMAP.addMarkerlabel('img/" + dr["ICOREMARK"].ToString() + ".gif','" + dr["xypoint"].ToString().Split(',')[0] + "','" + dr["xypoint"].ToString().Split(',')[1] + "','" + dr["hphm"].ToString() + dr["lxmc"].ToString() + "');";
                        this.ResourceManager1.RegisterAfterClientInitScript(js);
                    }
                }

                //string[] list = xypoint.Split('|');
                //foreach (string xy in list)
                //{
                //    js = "BMAP.addMarker('img/cctv.png','" + xy.Split(',')[0] + "','" + xy.Split(',')[1] + "','');;";
                //    this.ResourceManager1.RegisterAfterClientInitScript(js);
                //}
                if (xypoint != "")
                {
                    js = "BMAP.addPolyline2('#00ff00','" + xypoint + "','');;";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("FrequentOverCar.aspx-ShowLine", ex.Message + "；" + ex.StackTrace, "ShowLine has an exception");
            }
        }

        /// <summary>
        /// 组织xml查询文件
        /// </summary>
        /// <param name="rowstart">记录起始行</param>
        /// <param name="len">记录条数</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="cllx">车辆类型</param>
        /// <param name="hphm">号牌号码</param>
        /// <param name="gssj">更新时间</param>
        /// <param name="txkk">卡口</param>
        /// <returns></returns>
        private string getxml(int rowstart, int len, string start, string end, string hphm, string cllx, string clpp, string csys, string kkid, string gccs)
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
            string[] fstrs = Request.QueryString["funcname"].Split('-');//功能模块名称
            if (fstrs.Length > 0)
            {
                dyzgnmkmc = fstrs[1];
            }
            dyzgnmkbh = Request.QueryString["funcid"].ToString(); ;//功能模块编号
            string xml = "<?xml version='1.0' encoding='UTF-8'?>" +
                "<Message>" +
                         "<Version>版本号</Version>" +
                         "<Type>REQUEST</Type>" +
                         "<Body>" +
                             "<Cmd>" +
                                "<gccs>" + gccs + "</gccs>" +
                                "<hpzl>" + cllx + "</hpzl>" +
                                "<kssj>" + start + "</kssj>" +
                                "<jssj>" + end + "</jssj>" +
                                "<clpp>" + clpp + "</clpp>" +
                                    "<hphm>" + hphm + "</hphm>" +
                                "<cllx></cllx>" +
                                "<csys>" + csys + "</csys>" +
                                "<kkids>" + kkid + "</kkids>" +
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
            ILog.WriteErrorLog("频繁过车xml:" + xml);
            return xml;
        }

        /// <summary>
        /// xml文件解析
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        public void CXmlToDataTable(string xmlStr)
        {
            //xmlStr = "<?xml version='1.0' encoding='UTF-8'?><Message><Version>1.0</Version><Type>REQUEST</Type><Body><Return><carlist totalnum='2'><car kkids='100000010765,201410021310,201410021312,201420011125' cs='15' hphm='京B12345' clpp='clpp1' clwx='clwx1' csys='csys1'><gcjl><kkid>100000010765</kkid><gcsj>2016-04-29 13:43:00 000</gcsj><zjwj></zjwj><clwz></clwz></gcjl><gcjl><kkid>100000010765</kkid><gcsj>2016-04-29 13:43:00 000</gcsj><zjwj></zjwj></gcjl><gcjl><kkid>201410021310</kkid><gcsj>2016-04-29 13:44:00 00</gcsj><zjwj>http://111.jpg</zjwj></gcjl></car><car kkids='201420011133' cs='10' hphm='京A12345' clpp='clpp1' clwx='clwx1' csys='csys1'><gcjl><kkid>201420011133</kkid><gcsj>2016-04-29 13:43:00 000</gcsj><zjwj></zjwj></gcjl></car></carlist></Return></Body></Message>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);

            XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/carlist/car");
            try
            {
                foreach (XmlNode node in listNodes)
                {
                    DataRow dr = Dt_result.NewRow();
                    dr["hphm"] = (node.Attributes["hphm"]).InnerText;
                    dr["kkid"] = (node.Attributes["kkids"]).InnerText;
                    dr["csys"] = Bll.Common.GetCsysms((node.Attributes["csys"]).InnerText);
                    dr["cs"] = (node.Attributes["cs"]).InnerText;
                    dr["clwx"] = (node.Attributes["clwx"]).InnerText;
                    dr["clpp"] = Bll.Common.Changenull((node.Attributes["clpp"]).InnerText);
                    dr["lxmc"] = "";
                    dr["xypoint"] = "";
                    dr["ICOREMARK"] = "";
                    DataRow[] drstation = Dt_Station.Select("STATION_ID= '" + (node.Attributes["kkids"]).InnerText + "'");
                    if (drstation.Length > 0)
                        dr["ICOREMARK"] = drstation[0]["ICOREMARK"].ToString();
                    foreach (string kkid in dr["kkid"].ToString().Split(','))
                    {
                        DataRow[] listdr = Dt_Station.Select("STATION_ID= '" + kkid + "'");
                        if (listdr.Length > 0)
                        {
                            dr["lxmc"] += listdr[0]["STATION_NAME"].ToString();
                            dr["xypoint"] += (dr["xypoint"].ToString() == "" ? "" : "|") + listdr[0]["xpoint"].ToString() + "," + listdr[0]["ypoint"].ToString();
                        }
                    }
                    Dt_result.Rows.Add(dr);
                    XmlNodeList listcar = node.SelectNodes("gcjl");
                    foreach (XmlNode xn in listcar)
                    {
                        DataRow drcar = Dt_passcar.NewRow();
                        drcar["hphm"] = dr["hphm"].ToString();
                        drcar["kkid"] = xn.SelectSingleNode("kkid").InnerText;
                        DataRow[] listdr = Dt_Station.Select("STATION_ID= '" + xn.SelectSingleNode("kkid").InnerText + "'");
                        if (listdr.Length > 0)
                        {
                            drcar["lkmc"] = listdr[0]["STATION_NAME"].ToString();
                            drcar["xpoint"] = listdr[0]["xpoint"].ToString();
                            drcar["ypoint"] = listdr[0]["ypoint"].ToString();
                        }
                        drcar["gcsj"] = xn.SelectSingleNode("gcsj").InnerText.Substring(0, 19);
                        drcar["zjwj"] = xn.SelectSingleNode("zjwj").InnerText;
                        drcar["clwz"] = xn.SelectSingleNode("clwz").InnerText;
                        Dt_passcar.Rows.Add(drcar);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("FrequentOverCar.aspx-CXmlToDataTable", ex.Message + "；" + ex.StackTrace, "CXmlToDataTable has an exception");
            }
        }

        /// <summary>
        /// 查询布控
        /// </summary>
        /// <param name="data">坐标集合</param>
        public void QueryMarkArea(string data)
        {
            List<PointF> points = new List<PointF>();
            object pointList = JSONToObject<object>(data);
            Array aPoint = (Array)pointList;
            for (int i = 0; i < aPoint.Length; i++)
            {
                Dictionary<string, object> spoint = (Dictionary<string, object>)aPoint.GetValue(i);
                PointF PF = new PointF();
                foreach (KeyValuePair<string, object> kv in spoint)
                {
                    if (kv.Key == "lng")
                    {
                        PF.X = float.Parse(kv.Value.ToString());
                    }
                    else if (kv.Key == "lat")
                    {
                        PF.Y = float.Parse(kv.Value.ToString());
                    }
                }
                points.Add(PF);
            }
            PointF center = new PointF();
            PointF maxCenter = new PointF();
            double area = 0;
            double maxLength = 0;
            float maxX = 0;
            List<double> lengths = new List<double>();
            int s = polyCentriod(points, points.Count, ref center, ref area);
            for (int i = 0; i < points.Count; i++)
            {
                double l = GetDistance(points[i].Y, points[i].X, center.Y, center.X);
                lengths.Add(l);
                if (l > maxLength)
                {
                    maxLength = l;
                    maxCenter = points[i];
                }
                if (points[i].X > maxX)
                {
                    maxX = points[i].X;
                }
            }
            double Maxl = Math.Sqrt(Math.Abs(maxCenter.X - center.X) * Math.Abs(maxCenter.X - center.X) + Math.Abs(maxCenter.Y - center.Y) * Math.Abs(maxCenter.Y - center.Y)) * 10000;

            string where1 = "sqrt(pow((x_values-" + center.X.ToString() + "),2)+pow((y_values-" + center.Y.ToString() + "),2))*10000 < " + Maxl.ToString();

            //DataTable dt = Dt_Station;// this.bll.GetStation();
            DataTable dtOut;
            if (Dt_Station != null)
                dtOut = Dt_Station.Copy();
            else
                return;
            PointF QueryPoint = new PointF();
            Hashtable hs = new Hashtable();
            for (int n = dtOut.Rows.Count - 1; n >= 0; n--)
            {
                if (dtOut.Rows[n]["xpoint"].ToString() != "")
                {
                    QueryPoint.X = float.Parse(dtOut.Rows[n]["xpoint"].ToString());
                    QueryPoint.Y = float.Parse(dtOut.Rows[n]["ypoint"].ToString());
                    if (!IsVisible(QueryPoint, points, maxX))
                    {
                        dtOut.Rows[n].Delete();
                        dtOut.AcceptChanges();
                    }
                }
                else
                {
                    dtOut.Rows[n].Delete();
                    dtOut.AcceptChanges();
                }
            }
            if (dtOut != null && dtOut.Rows.Count > 0)
            {
                showstation(dtOut);
                StoreKK.RemoveAll();
                StoreKK.DataSource = dtOut;
                StoreKK.DataBind();
                panelkk.Collapsed = false;
                Pantrack.Height = 550;
            }
            else
            {
                //panelkk.Collapsed = true;
                Pantrack.Height = 240;
                StoreKK.RemoveAll();
                StoreKK.DataBind();
            }
        }

        #region 布控范围标注

        private void showstation(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string js = " BMAP.addMarker('img/" + dr["ICOREMARK"].ToString() + ".gif','" + dr["xpoint"].ToString() + "','" + dr["ypoint"].ToString() + "','" + dr["STATION_NAME"].ToString() + "');;";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
        }

        public static T JSONToObject<T>(string jsonText)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<T>(jsonText);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }

        private int polyCentriod(List<PointF> p, int n, ref PointF Centroid, ref double area)
        {
            int i, j;
            double ai, atmp = 0.0, xtmp = 0, ytmp = 0, ltmp = 0;
            if (n < 3)
                return 1;
            for (i = n - 1, j = 0; j < n; i = j, j++)
            {
                ai = p[i].X * p[j].Y - p[j].X * p[i].Y;
                atmp += ai;
                xtmp += (p[j].X + p[i].X) * ai;
                ytmp += (p[j].Y + p[i].Y) * ai;
            }
            area = atmp / 2;
            if (atmp != 0)
            {
                Centroid.X = (float)(xtmp / (3 * atmp));
                Centroid.Y = (float)(ytmp / (3 * atmp));
                if (area < 0)
                    area = -area;
                return 0;
            }
            return 2;
        }

        private static double EARTH_RADIUS = 6378.137;//地球半径

        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000 * 1000) / 10000;//米
            return s;
        }

        public string Getwhere(string datasb, string datass)
        {
            string where1 = string.Empty;
            string where2 = string.Empty;
            string where = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(datasb))
                {
                    where1 = "  device_mode_id  in (" + datasb + ")  or";
                }
                if (!string.IsNullOrEmpty(datass))
                {
                    where2 = "  purposeid  in (" + datass + ")     or";
                }
                where = where1 + where2;
                if (where.Length > 2)
                {
                    where = where.Substring(0, where.Length - 2);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FrequentOverCar.aspx-Getwhere", ex.Message + "；" + ex.StackTrace, "Getwhere has an exception");
            }

            return where;
        }

        private bool IsVisible(PointF point, List<PointF> ListPoint, float maxX)
        {
            int count = 0;
            for (int i = 0; i < ListPoint.Count; i++)
            {
                PointF a = new PointF();
                PointF b = new PointF();
                a.X = point.X;
                a.Y = point.Y;
                b.X = maxX;
                b.Y = point.Y;
                if (Judge(a, b, ListPoint[i], ListPoint[(i + 1) % ListPoint.Count]) == true)
                {
                    count++;
                }
            }
            if (count % 2 == 0)
                return false;
            else
                return true;
        }

        private bool Judge(PointF p0, PointF p1, PointF p2, PointF p3)
        {
            return ((Max(p0.X, p1.X) >= Min(p2.X, p3.X)) &&
                (Max(p2.X, p3.X) >= Min(p0.X, p1.X)) &&
                (Max(p0.Y, p1.Y) >= Min(p2.Y, p3.Y)) &&
                (Max(p2.Y, p3.Y) >= Min(p0.Y, p1.Y)) &&
                (Multiply(p2, p1, p0) * Multiply(p1, p3, p0) >= 0) &&
                (Multiply(p0, p3, p2) * Multiply(p3, p1, p2) >= 0)
                );
        }

        private double Multiply(PointF p1, PointF p2, PointF p0)//叉乘
        {
            return ((p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y));
        }

        private double Max(double a, double b)
        {
            if (a >= b)
                return a;
            return b;
        }

        private double Min(double a, double b)
        {
            if (a <= b)
                return a;
            return b;
        }

        #endregion 布控范围标注

        public int getlenxml(string xmlStr)
        {
            try
            {
                if (!string.IsNullOrEmpty(xmlStr))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlStr);
                    XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/carlist");
                    if (int.Parse(listNodes[0].Attributes[0].Value) == 0)
                    {
                        totalpage.Value = 0;
                        return 0;
                    }
                    else
                    {
                        totalpage.Value = (int.Parse(listNodes[0].Attributes[0].Value) % 15 == 0 ? int.Parse(listNodes[0].Attributes[0].Value) / 15 : int.Parse(listNodes[0].Attributes[0].Value) / 15 + 1);
                        return int.Parse(listNodes[0].Attributes[0].Value);
                    }
                }
                return 0;
            }
            catch { return 0; }
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
        /// 获取passcar结构
        /// </summary>
        /// <returns></returns>
        private DataTable GetData()
        {
            try
            {
                DataTable dt = new DataTable("PassCar");
                dt.Columns.Add("hphm", Type.GetType("System.String"));
                dt.Columns.Add("kkid", Type.GetType("System.String"));
                dt.Columns.Add("lkmc", Type.GetType("System.String"));
                dt.Columns.Add("xpoint", Type.GetType("System.String"));
                dt.Columns.Add("ypoint", Type.GetType("System.String"));
                dt.Columns.Add("gcsj", Type.GetType("System.String"));
                dt.Columns.Add("zjwj", Type.GetType("System.String"));
                dt.Columns.Add("clwz", Type.GetType("System.String"));
                dt.Columns.Add("ICOREMARK", Type.GetType("System.String"));
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FrequentOverCar.aspx-GetData", ex.Message + "；" + ex.StackTrace, "GetData has an exception");
            }
            return null;
        }

        /// <summary>
        /// 获取查询结果结构
        /// </summary>
        /// <returns></returns>
        private DataTable GetResultData()
        {
            try
            {
                DataTable dt = new DataTable("Car");
                dt.Columns.Add("kkid", Type.GetType("System.String"));
                dt.Columns.Add("hphm", Type.GetType("System.String"));
                dt.Columns.Add("lxmc", Type.GetType("System.String"));
                dt.Columns.Add("xypoint", Type.GetType("System.String"));
                dt.Columns.Add("csys", Type.GetType("System.String"));
                dt.Columns.Add("clpp", Type.GetType("System.String"));
                dt.Columns.Add("cs", Type.GetType("System.String"));
                dt.Columns.Add("clwx", Type.GetType("System.String"));
                dt.Columns.Add("ICOREMARK", Type.GetType("System.String"));
                //DataRow dr = dt.NewRow();
                //dr["hphm"] = "hphm01";
                //dr["lxmc"] = "lxmc01";
                //dr["xypoint"] = "'116.302009','37.449335'|'116.392271','37.430081'|'116.491731','37.378711'";
                //dr["csys"] = "黑色";
                //dr["zs"] = "20";
                //dt.Rows.Add(dr);
                //dr = dt.NewRow();
                //dr["hphm"] = "hphm02";
                //dr["lxmc"] = "lxmc02";
                //dr["xypoint"] = "116.40147,37.519434|116.396296,37.448418|116.385947,37.405317";
                //dr["csys"] = "红色";
                //dr["zs"] = "25";
                //dt.Rows.Add(dr);
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FrequentOverCar.aspx-GetResultData", ex.Message + "；" + ex.StackTrace, "GetResultData has an exception");
            }
            return null;
        }

        #endregion 方法

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