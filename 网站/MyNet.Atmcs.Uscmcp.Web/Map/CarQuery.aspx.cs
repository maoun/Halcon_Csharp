using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;
using System.Xml;

namespace MyNet.Atmcs.Uscmcp.Web.Map
{
    public partial class CarQuery : System.Web.UI.Page
    {
        #region 变量

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private static DataTable Dt_Station = new DataTable();
        private static DataTable Dt_passcar = new DataTable();
        private static DataTable Dt_result = new DataTable();
        private static DataTable Dt_xsfx = new DataTable();
        private static DataTable Dt_cllx = new DataTable();
        private MyNet.Atmcs.Uscmcp.Bll.MapManager bll = new Bll.MapManager();
        private static string startdate = "", enddate = "";
        private static QueryService.querypasscar client = new QueryService.querypasscar();
        private UserLogin userLogin = new UserLogin();

        #endregion 变量

        #region 事件

        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("CarQuery41", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                try
                {
                    this.ResourceManager1.RegisterAfterClientInitScript("GetScreen();");
                    DataTable dt1 = GetRedisData.GetData("t_sys_code:140001");
                    if (dt1 != null)
                    {
                        this.cllx.DataSource = Dt_cllx = Bll.Common.ChangColName(dt1);
                        this.cllx.DataBind();
                    }
                    else
                    {
                        System.Data.DataSet ds = bll.GetCllx();
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            this.cllx.DataSource = ds.Tables[0];
                            this.cllx.DataBind();
                            Dt_cllx = ds.Tables[0];
                        }
                    }

                    this.csys.DataSource = GetRedisData.GetData("t_sys_code:240013");
                    this.csys.DataBind();
                    Dt_Station = bll.GetStation();
                    Dt_passcar = GetData();
                    Dt_result = GetResultData();
                    start.InnerText = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    end.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    startdate = start.InnerText;
                    enddate = end.InnerText;
                    if (Session["Condition"] != null)
                    {
                        Condition con = Session["Condition"] as Condition;
                        start.InnerText = con.StartTime;
                        end.InnerText = con.EndTime;
                        startdate = con.StartTime;
                        enddate = con.EndTime;
                        cboplate.SetVehicleText(con.Sqjc);
                        txtplate.Text = con.Hphm;
                        cbocllx.SetValue(con.Hpzl);
                        ButQueryClick();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["hphm"]))
                        {
                            string hphm = Request.QueryString["hphm"];
                            if (!string.IsNullOrEmpty(hphm))
                            {
                                cboplate.SetVehicleText(hphm.Substring(0, 1));
                                txtplate.Text = hphm.Substring(1, hphm.Length - 1);
                            }
                        }
                        if (!string.IsNullOrEmpty(Request.QueryString["hpzl"]))
                        {
                            string hpzl = Request.QueryString["hpzl"];
                            cbocllx.Value = hpzl;
                        }

                        if (!string.IsNullOrEmpty(Request.QueryString["startTime"]))
                        {
                            startdate = Request.QueryString["startTime"];
                            start.InnerText = startdate;
                        }
                        if (!string.IsNullOrEmpty(Request.QueryString["endTime"]))
                        {
                            enddate = Request.QueryString["endTime"];
                            end.InnerText = enddate;
                        }
                    }
                }
                catch
                { }
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("CarQuery36", "访问：") + Request.QueryString["funcname"], userinfo.NowIp, "0");
            }
            this.DataBind();
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
            {
                startdate = strtime;
            }
            else
            {
                enddate = strtime;
            }
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        [DirectMethod]
        public void ButQueryClick()
        {
            string hphm = cboplate.VehicleText + txtplate.Text;
            string cllx = cbocllx.Text;
            string txkk = txttxkk.Text;
            string csys = cbocsys.Text;
            string clpp = ClppChoice.Value.ToString();
            if (int.Parse(txtgssj.Text) > 60)
            {
                Notice(GetLangStr("CarQuery42", "提示"), GetLangStr("CarQuery43", "跟随时间不能大于60秒！"));
                return;
            }
            if (Convert.ToInt32(txkk) < 2)
            {
                Notice(GetLangStr("CarQuery42", "提示"), GetLangStr("CarQuery44", "同行卡口必须大于等于2个！"));
                return;
            }
            int gssj = int.Parse(txtgssj.Text) * 1000;
            if (startdate == "" || enddate == "" || txtplate.Text == "")
            {
                if (startdate == "" || enddate == "")
                    Notice(GetLangStr("CarQuery42", "提示"), GetLangStr("CarQuery45", "时间范围未选择！"));
                if (txtplate.Text == "")
                    Notice(GetLangStr("CarQuery42", "提示"), GetLangStr("CarQuery46", "号牌号码未录入！"));
            }
            else
                if ((DateTime.Parse(enddate) - DateTime.Parse(startdate)).TotalHours > 24)
                {
                    Notice(GetLangStr("CarQuery42", "提示"), GetLangStr("CarQuery47", "时间范围不能多于24个小时！"));
                }
                else
                    query(startdate, enddate, cllx, hphm, gssj.ToString(), txkk, clpp, csys);
        }

        /// <summary>
        /// 列表行选择触发事件
        /// </summary>
        /// <param name="hphm">号牌号码</param>
        [DirectMethod]
        public void SelectRow(string hphm, string clpp, string csys, string hpzl, string clzt)
        {
            clearmap();
            showline("#ff0000", cboplate.VehicleText + txtplate.Text);
            showline("#00ff00", hphm);
            //showwin(hphm);
            showwin(hphm, clpp, csys, hpzl, clzt);
            RowSelectionModel sm = this.GridRoadManager.SelectionModel.Primary as RowSelectionModel;
            sm.SelectedRows.Clear();
            sm.UpdateSelection();
        }

        [DirectMethod]
        public void ShowMoreInfo(string x, string y, string hphm, string gcsj, string kkmc, string url, string clwz)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = "../images/NoImage.png";
            }
            string js = "var _gcsj ;var _type;var _hphm; _type='IMAGE'; _gcsj='" + gcsj + "';_hphm='" + hphm + "';_clwz='" + clwz + "';BMAP.addmarkerstation(false,'../Map/img/COM.gif','" + x + "','" + y + "','" + kkmc + "','" + url + "',{ gcsj:_gcsj,hphm: _hphm, type: _type,clwz:_clwz});";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }

        private DataTable GetClData()
        {
            DataTable dt = new DataTable("PassCar");
            dt.Columns.Add("cartype", Type.GetType("System.String"));
            dt.Columns.Add("hphm", Type.GetType("System.String"));
            dt.Columns.Add("hpzl", Type.GetType("System.String"));
            dt.Columns.Add("clpp", Type.GetType("System.String"));
            dt.Columns.Add("csys", Type.GetType("System.String"));
            return dt;
        }

        [DirectMethod]
        public void showotherwin(string url, string title)
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
            win.AutoLoad.Url = url;
            win.AutoLoad.Mode = LoadMode.IFrame;
            win.Render(this.Form);
            win.Show();
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

        [DirectMethod]
        public void showwin(string hphm, string clpp, string csys, string hpzl, string clzt)
        {
            strhphm = hphm;
            strhpzl = hpzl;
            DataRow[] drs = Dt_passcar.Select("hphm='" + hphm + "' or hphm='" + cboplate.VehicleText + txtplate.Text + "'", "gwsj asc");
            DataTable dt = GetData();
            foreach (DataRow dr in drs)
            {
                if (Dt_passcar.Select("kkid='" + dr["kkid"] + "' and hphm='" + hphm + "'").Length > 0)
                    dt.Rows.Add(dr.ItemArray);
            }
            winhphm.Title = hphm;
            //clppwin.Text = clpp;
            //csyswin.Text = csys;
            //hpzlwin.Text = (hpzl != "" ? gethpzl(hpzl) : "");
            showcarinfo(hphm, clpp, csys, hpzl);

            //clztwin.Text = clzt;
            winhphm.Show();
            wingcjl.Title = hphm + "_过车记录";
            wingcjl.Show();
            if (dt != null && dt.Rows.Count > 0)
            {
                Storegcjl.DataSource = dt;
                Storegcjl.DataBind();
                //this.ResourceManager1.RegisterAfterClientInitScript("isScroll();");
            }
            else
            {
                Storegcjl.RemoveAll();
                Storegcjl.DataBind();
            }
        }

        public void showcarinfo(string hphm, string clpp, string csys, string hpzl)
        {
            DataTable dt = GetClData();
            DataRow dr = dt.NewRow();
            dr["cartype"] = "伴随";
            dr["hphm"] = hphm.Trim();
            dr["clpp"] = Bll.Common.Changenull(clpp);
            dr["csys"] = Bll.Common.GetCsysms(csys);
            dr["hpzl"] = (hpzl != "" ? gethpzl(hpzl) : "");
            DataRow[] results = Dt_result.Select(" hphm='" + cboplate.VehicleText + txtplate.Text + "'");
            DataRow dr1 = dt.NewRow();
            if (results.Length > 0)
            {
                dr1["cartype"] = "主车";
                dr1["hphm"] = cboplate.VehicleText + txtplate.Text.Trim();
                dr1["clpp"] = Bll.Common.Changenull(results[0]["clpp"].ToString());
                dr1["csys"] = Bll.Common.GetCsysms(results[0]["csys"].ToString());
                dr1["hpzl"] = (results[0]["hpzl"].ToString() != "" ? gethpzl(hpzl) : "");
            }
            dt.Rows.Add(dr1);
            dt.Rows.Add(dr);
            Store1.DataSource = dt;
            Store1.DataBind();
        }

        private static string strhpzl = "", strhphm = "";

        protected void unnamed_event(object sender, EventArgs e)
        {
            //this.ResourceManager1.RegisterBeforeClientInitScript("getTime();");
            ButQueryClick();
        }

        #endregion 事件

        #region 方法

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
        /// xml文件解析
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        public void CXmlToDataTable(string xmlStr)
        {
            try
            {
                ILog.WriteErrorLog("result:" + xmlStr);
                //xmlStr = "<?xml version=‘1.0‘ encoding=‘UTF-8‘?><Message><Version>1.0</Version><Type>RESPONSE</Type><Body><Return><CarList totalnum=‘19‘><carinfo hphm=‘渝A8T663‘ hpzl =‘02‘ txsl=‘4‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>1476356305000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>1476356319000</gwsj></passinfo><passinfo><kkid>201629028231</kkid><gwsj>1476356362000</gwsj></passinfo><passinfo><kkid>201629958370</kkid><gwsj>1476356489000</gwsj></passinfo></carinfo><carinfo hphm=‘渝B7T035‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629028231</kkid><gwsj>2016-10-13 18:59:37 000</gwsj></passinfo><passinfo><kkid>201629958370</kkid><gwsj>2016-10-13 19:02:15 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝B08T91‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:52 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:52 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝C2Q120‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:47 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:47 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝BQD418‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:59:00 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:59:00 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝AYU675‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:51 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:51 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝BMW306‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:43 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:43 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝AUA937‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:59:12 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:59:12 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝A9T269‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:59:01 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:59:01 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝A576S0‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:59:03 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:59:03 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝B381D0‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:50 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:50 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝BFA796‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:44 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:44 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝B10T93‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:55 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:55 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝DL9271‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:59 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:59 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝DS7711‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:52 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:52 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝AAP735‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:55 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:55 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝DS2296‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:47 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:47 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝AEH256‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:39 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:58:39 000</gwsj></passinfo></carinfo><carinfo hphm=‘渝BZM815‘ hpzl =‘02‘ txsl=‘2‘ csys=‘‘><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:59:07 000</gwsj></passinfo><passinfo><kkid>201629665204</kkid><gwsj>2016-10-13 18:59:07 000</gwsj></passinfo></carinfo></CarList></Return></Body></Message>";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);
                Dt_result.Clear();
                Dt_passcar.Clear();
                XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/CarList/carinfo ");
                foreach (XmlNode node in listNodes)
                {
                    DataRow dr = Dt_result.NewRow();
                    dr["hphm"] = node.Attributes[0].InnerText;//.Attributes["hphm"].ToString();
                    DataRow[] listcllx = Dt_cllx.Select("col0= '" + node.Attributes["hpzl"].InnerText + "'");
                    if (listcllx.Length > 0)
                        dr["cllx"] = listcllx[0]["col1"].ToString();
                    else
                        dr["cllx"] = "";
                    dr["txsl"] = node.Attributes["txsl"].InnerText;
                    dr["csys"] = node.Attributes["csys"].InnerText;
                    try
                    {
                        dr["clpp"] = Bll.Common.Changenull(node.Attributes["clpp"].InnerText);
                    }
                    catch
                    {
                        dr["clpp"] = "";
                    }
                    try
                    { dr["hpzl"] = node.Attributes["hpzl"].InnerText; }
                    catch
                    { dr["hpzl"] = ""; }
                    Dt_result.Rows.Add(dr);
                    XmlNodeList list = node.SelectNodes("passinfo");
                    foreach (XmlNode xn in list)
                    {
                        DataRow drpass = Dt_passcar.NewRow();
                        drpass["hphm"] = dr["hphm"].ToString();
                        drpass["kkid"] = (xn.SelectSingleNode("kkid")).InnerText;
                        DataRow[] listdr = Dt_Station.Select("STATION_ID= '" + (xn.SelectSingleNode("kkid")).InnerText + "'");
                        if (listdr.Length > 0)
                        {
                            drpass["lkmc"] = listdr[0]["STATION_NAME"].ToString();
                            drpass["xpoint"] = listdr[0]["xpoint"].ToString();
                            drpass["ypoint"] = listdr[0]["ypoint"].ToString();
                            drpass["ICOREMARK"] = listdr[0]["ICOREMARK"].ToString();
                        }
                        else
                        {
                            drpass["lkmc"] = "";
                            drpass["xpoint"] = "";
                            drpass["ypoint"] = "";
                            drpass["ICOREMARK"] = "com";
                        }
                        drpass["gwsj"] = (xn.SelectSingleNode("gwsj")).InnerText.Length < 19 ? (xn.SelectSingleNode("gwsj")).InnerText : (xn.SelectSingleNode("gwsj")).InnerText.Substring(0, 19);
                        try
                        { drpass["bssj"] = (xn.SelectSingleNode("bssj")).InnerText; }
                        catch
                        { drpass["bssj"] = ""; }
                        try
                        { drpass["zjwj"] = (xn.SelectSingleNode("zjwj")).InnerText; }
                        catch
                        { drpass["zjwj"] = ""; }
                        try
                        { drpass["clwz"] = (xn.SelectSingleNode("clwz")).InnerText; }
                        catch
                        { drpass["clwz"] = ""; }
                        Dt_passcar.Rows.Add(drpass);
                    }
                }
            }
            catch (Exception ex)
            {
                //Notice(GetLangStr("CarQuery42","提示"), "接口服务错误！(版本)");
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("CarQuery.aspx-CXmlToDataTable", ex.Message + "；" + ex.StackTrace, "CXmlToDataTable has an exception");
            }
        }

        /// <summary>
        /// 地图清空
        /// </summary>
        private void clearmap()
        {
            string js = "BMAP.Clear();BMAP.ClearLine();";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }

        /// <summary>
        /// 获取结果数据集结果
        /// </summary>
        /// <returns></returns>
        private DataTable GetResultData()
        {
            DataTable dt = new DataTable("PassResult");
            dt.Columns.Add("hphm", Type.GetType("System.String"));
            dt.Columns.Add("cllx", Type.GetType("System.String"));
            dt.Columns.Add("txsl", Type.GetType("System.String"));
            dt.Columns.Add("csys", Type.GetType("System.String"));
            dt.Columns.Add("clwx", Type.GetType("System.String"));
            dt.Columns.Add("hpzl", Type.GetType("System.String"));
            dt.Columns.Add("clpp", Type.GetType("System.String"));
            return dt;
        }

        /// <summary>
        /// 获取passcar数据集结果
        /// </summary>
        /// <returns></returns>
        private DataTable GetData()
        {
            DataTable dt = new DataTable("PassCar");
            dt.Columns.Add("hphm", Type.GetType("System.String"));
            dt.Columns.Add("kkid", Type.GetType("System.String"));
            dt.Columns.Add("lkmc", Type.GetType("System.String"));
            dt.Columns.Add("cdbh", Type.GetType("System.String"));
            dt.Columns.Add("fxmc", Type.GetType("System.String"));
            dt.Columns.Add("xpoint", Type.GetType("System.String"));
            dt.Columns.Add("ypoint", Type.GetType("System.String"));
            dt.Columns.Add("gwsj", Type.GetType("System.String"));
            dt.Columns.Add("clwz", Type.GetType("System.String"));
            dt.Columns.Add("bssj", Type.GetType("System.String"));
            dt.Columns.Add("zjwj", Type.GetType("System.String"));
            dt.Columns.Add("ICOREMARK", Type.GetType("System.String"));
            return dt;
        }

        /// <summary>
        /// 气泡框提示（展示车辆详细信息）
        /// </summary>
        /// <param name="hphm">号牌号码</param>
        public void showwin(string hphm)
        {
            try
            {
                string HTML = "";
                string head = "<div class=\"car-location OverCar-Location \">" +
         "<div class=\"items w-220px\">" +
                    //"<b class=\"tips-arrow\"></b>" +
              "<span class=\"car-brand text-center\">" + hphm +
              "</span>" +
              "<section class=\"car-img write-bg\">" +
               " <div class=\"CarIllegal-list\">" +
                     "<ul class=\"data-list-h clearfix \">" +
                       "<li class=\"w-1 font-10 fb w-100px\">" + GetLangStr("CarQuery28", "卡口名称") + "</li>" +
                       "<li class=\"w-1 font-10 fb w-100px\">" + GetLangStr("CarQuery49", "过车时间") + "</li>" +
                    //"<li class=\"w-1 font-10 fb\">行车方向</li>" +
                     "</ul>" +
                     "<ul class=\"OverCar-data-list clearfix\">";
                string content = "";
                string end = "</ul></div></section></div></div>";
                string xpos = "", ypos = ""; int i = 0;
                if (Dt_passcar != null)
                {
                    DataRow[] drlist = Dt_passcar.Select("hphm='" + hphm + "'");
                    foreach (DataRow dr in drlist)
                    {
                        if (i == 0)
                        {
                            i++;
                            xpos = dr["xpoint"].ToString();
                            ypos = dr["ypoint"].ToString();
                            if (xpos == "" || ypos == "")
                                i = 0;
                        }
                        DataRow[] KKlist = Dt_passcar.Select("kkid='" + dr["kkid"].ToString() + "'");
                        content += "  <li>" +
                       "      <span class=\"w-1 text-center font-10 w-100px\">" + dr["lkmc"].ToString() + "</span>" +
                        "     <span class=\"w-1 font-10  w-100px\">" + dr["gwsj"].ToString().Substring(0, 19) + "</span>" +
                            //"    <span class=\"w-1 font-10\">" + dr["fxmc"] + "</span>" +
                        "</li>";
                    }
                    HTML = head + content + end;
                }
                else
                {
                    return;
                }
                if (xpos != "" && ypos != "")
                {
                    string js = "BMAP.openWindow('" + HTML + "','" + xpos + "','" + ypos + "');";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                    js = "BMAP.GotoXY('" + xpos + "','" + ypos + "');";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);

                    this.ResourceManager1.RegisterAfterClientInitScript("isScroll()");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("CarQuery.aspx-showwin", ex.Message + "；" + ex.StackTrace, "showwin has an exception");
            }
        }

        /// <summary>
        /// 车辆路线描绘
        /// </summary>
        /// <param name="color">路线颜色</param>
        /// <param name="hphm">号牌号码</param>
        private void showline(string color, string hphm)
        {
            try
            {
                string js = "", xpos = "", ypos = "", points = "";
                string html = "";
                DataRow[] drlist = Dt_passcar.Select("hphm='" + hphm + "'");
                if (drlist.Length > 0)
                {
                    foreach (DataRow dr in drlist)
                    {
                        xpos = dr["xpoint"].ToString();
                        ypos = dr["ypoint"].ToString();
                        if (xpos == "" || ypos == "")
                        {
                            continue;
                        }
                        DataRow[] KKlist = Dt_passcar.Select("kkid='" + dr["kkid"].ToString() + "'");
                        html = gethtml(dr["lkmc"].ToString(), KKlist);
                        if (color == "#ff0000")
                        {
                            js = " BMAP.addMarker('img/" + dr["ICOREMARK"].ToString() + ".gif','" + dr["xpoint"].ToString() + "','" + dr["ypoint"].ToString() + "','" + dr["lkmc"].ToString() + "');;";
                            //js = " BMAP.addMarkerbs('img/" + dr["ICOREMARK"].ToString() + ".gif','" + dr["xpoint"].ToString() + "','" + dr["ypoint"].ToString() + "','','" + html + "');;";
                            this.ResourceManager1.RegisterAfterClientInitScript(js);
                        }
                        points += (points == "" ? "" : "|") + xpos + "," + ypos;
                    }
                    if (points != "")
                        js = "BMAP.addPolyline2('" + color + "','" + points + "','" + hphm + "');";

                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
                else
                { }
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("CarQuery.aspx-showline", ex.Message + "；" + ex.StackTrace, "showline has an exception");
                ILog.WriteErrorLog(ex.Message);
            }
        }

        private string gethtml(string stationname, DataRow[] list)
        {
            string head = "<div class=\"car-location OverCar-Location \">" +
     "<div class=\"items w-220px\">" +
        "<b class=\"tips-arrow\"></b>" +
          "<span class=\"car-brand text-center\">" + stationname +
          "</span>" +
          "<section class=\"car-img write-bg\">" +
           " <div class=\"CarIllegal-list\">" +
                 "<ul class=\"data-list-h clearfix \">" +
                   "<li class=\"w-1 font-10 fb\">" + GetLangStr("CarQuery22", "号牌号码") + " </li>" +
                   "<li class=\"w-2 font-10 fb\">" + GetLangStr("CarQuery49", "过车时间") + "</li>" +
                 "</ul>" +
                 "<ul class=\"OverCar-data-list clearfix\">";
            string content = "";
            string end = "</ul></div></section></div></div>";
            foreach (DataRow dr in list)
            {
                content += "  <li>" +
               "      <span class=\"w-1 text-center font-10\">" + dr["hphm"] + "</span>" +
                "     <span class=\"w-2 font-10\">" + dr["gwsj"] + "</span>" +
                "</li>";
            }
            return head + content + end;
        }

        /// <summary>
        /// 解析xml文件获取记录条数
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        /// <returns></returns>
        public int getlenxml(string xmlStr)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);

                XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/CarList ");
                return int.Parse(listNodes[0].Attributes[0].Value);
            }
            catch { return 0; }
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
        private string getxml(int rowstart, int len, string start, string end, string cllx, string hphm, string gssj, string txkk, string clpp, string csys)
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
                                    "<gssj>" + gssj + "</gssj>" +
                                    "<txkk>" + txkk + "</txkk>" +
                                    "<hphm>" + hphm + "</hphm>" +
                                    "<cllx>" + cllx + "</cllx>" +
                                    "<kssj>" + start + "</kssj>" +
                                    "<jssj>" + end + "</jssj>" +
                                "<csys>" + csys + "</csys>" +
                                "<clpp>" + clpp + "</clpp>" +
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
                ILog.WriteErrorLog("xml:" + xml);
                return xml;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("CarQuery.aspx-getxml", ex.Message + "；" + ex.StackTrace, "getxml has an exception");
                return "";
            }
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="cllx">车辆类型</param>
        /// <param name="hphm">号牌号码</param>
        /// <param name="gssj">更新时间</param>
        /// <param name="txkk">卡口</param>
        private void query(string start, string end, string cllx, string hphm, string gssj, string txkk, string clpp, string csys)
        {
            try
            {
                int startrow = 0, len = 30, endrow = 29;
                string xml = "", rsxml = "";
                Dt_passcar.Clear();
                Dt_result.Clear();

                xml = getxml(startrow, endrow, start, end, cllx, hphm, gssj, txkk, clpp, csys);
                try
                {
                    rsxml = client.GetFollowPassCarInfo(xml);
                }
                catch (Exception ex)
                {
                    Notice(GetLangStr("CarQuery42", "提示"), GetLangStr("CarQuery50", "接口服务报错！"));
                    ILog.WriteErrorLog(ex.Message);
                }
                //rsxml = "<?xml version='1.0' encoding='UTF-8'?><Message><Version>1.0</Version><Type>RESPONSE</Type><Body><Return><CarList totalnum='1'><carinfo hphm='京A12345' hpzl ='02' txsl='1' csys='1'><passinfo><kkid>100000010765</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>201410021310</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>201410021312</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>201420011125</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>201420011133</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo></carinfo><carinfo hphm='京A12346' hpzl ='02' txsl='1' csys='1'><passinfo><kkid>100000010765</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>201410021310</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>201410021312</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo></carinfo><carinfo hphm='京A12347' hpzl ='02' txsl='1' csys='1'><passinfo><kkid>201410021310</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>201410021312</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>201420011125</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>201420011133</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo></carinfo></CarList></Return></Body></Message>";

                //rsxml = "<?xml version='1.0' encoding='UTF-8'?><Message><Version>1.0</Version><Type>RESPONSE</Type><Body><Return><CarList totalnum='1'><carinfo hphm='京A12345' hpzl ='02' txsl='1' csys='1'><passinfo><kkid>100000010859</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>3716230000000901</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>201409021069</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>401060000000</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>601051014300</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo></carinfo><carinfo hphm='京A12346' hpzl ='02' txsl='1' csys='1'><passinfo><kkid>601121003000</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>601151003000</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>601121006000</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo></carinfo><carinfo hphm='京A12347' hpzl ='02' txsl='1' csys='1'><passinfo><kkid>601121010000</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>601151003000</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>201420011125</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo><passinfo><kkid>601151003000</kkid><gwsj>2016-04-25 19:03:03 123</gwsj></passinfo></carinfo></CarList></Return></Body></Message>";
                //大数据查询接口
                if (rsxml != "" && getlenxml(rsxml) > 0)
                {
                    CXmlToDataTable(rsxml);
                    while (getlenxml(rsxml) > endrow)
                    {
                        startrow = startrow + len;
                        endrow = endrow + len;
                        xml = getxml(startrow, endrow, start, end, cllx, hphm, gssj, txkk, clpp, csys);
                        try
                        {
                            rsxml = client.GetFollowPassCarInfo(xml);
                        }
                        catch (Exception ex)
                        {
                            Notice(GetLangStr("CarQuery42", "提示"), GetLangStr("CarQuery50", "接口服务报错！"));
                            ILog.WriteErrorLog(ex.Message);
                        }
                        CXmlToDataTable(rsxml);
                    }
                }
                if (Dt_result != null && Dt_result.Rows.Count > 0)
                {
                    DataTable dtcopy = Dt_result.Copy();
                    DataRow[] drmain = dtcopy.Select("hphm='" + cboplate.VehicleText + txtplate.Text + "'");
                    if (drmain.Length > 0)
                    {
                        dtcopy.Rows.Remove(drmain[0]);
                    }
                    clearmap();
                    if (dtcopy == null || dtcopy.Rows.Count == 0)
                    {
                        StoreInfo.RemoveAll();
                        StoreInfo.DataBind();
                        Notice(GetLangStr("CarQuery42", "提示"), GetLangStr("CarQuery51", "无伴随车辆！"));
                    }
                    else
                    {
                        DataTable dtCopy = dtcopy.Copy();

                        System.Data.DataView dv = dtcopy.DefaultView;
                        dv.Sort = "txsl";
                        dtCopy = dv.ToTable();
                        StoreInfo.DataSource = dtCopy;
                        StoreInfo.DataBind();
                        showline("#ff0000", cboplate.VehicleText + txtplate.Text);
                    }
                }
                else
                {
                    StoreInfo.RemoveAll();
                    StoreInfo.DataBind();
                    Notice(GetLangStr("CarQuery42", "提示"), GetLangStr("CarQuery52", "无数据！"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("CarQuery.aspx-query", ex.Message + "；" + ex.StackTrace, "query has an exception");
            }
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