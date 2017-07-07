using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;

// ***********************************************************************
// Assembly         : MyNet.Atmcs.Uscmcp.Web
// Author           : zlsyl
// Created          : 08-11-2016
//
// Last Modified By : zlsyl
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="FootHold.aspx.cs" company="ZKLT">
//     Copyright (c) ZKLT. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Xml;

/// <summary>
/// The Map namespace.
/// </summary>
namespace MyNet.Atmcs.Uscmcp.Web.Map
{
    /// <summary>
    /// Class FootHold.
    /// </summary>
    public partial class FootHold : System.Web.UI.Page
    {
        #region 变量

        /// <summary>
        /// The isdefault
        /// </summary>
        private static bool isdefault;

        /// <summary>
        /// 起止时间
        /// </summary>
        private static string startdate, enddate;

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
        /// 行驶方向
        /// </summary>
        private static DataTable Dt_xsfx = new DataTable();

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        /// <summary>
        /// The client
        /// </summary>
        private static QueryService.querypasscar client = new QueryService.querypasscar();

        /// <summary>
        /// The BLL
        /// </summary>
        private MapManager bll = new MapManager();

        /// <summary>
        /// The user login
        /// </summary>
        private UserLogin userLogin = new UserLogin();

        #endregion 变量

        #region 事件

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("FootHold28", "您没有登录或操作超时，请重新登陆!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                this.DataBind();
                try
                {
                    isdefault = true;
                    DataTable dt4 = GetRedisData.GetData("t_sys_code:240025");
                    //行驶方向
                    if (dt4 != null)
                    {
                        Dt_xsfx = dt4;
                    }
                    else
                    {
                        Dt_xsfx = bll.GetFxcode();
                    }

                    Dt_Station = bll.GetStation();
                    Dt_passcar = GetData();
                    Dt_result = GetResultData();
                    DataTable dt1 = GetRedisData.GetData("t_sys_code:140001");
                    if (dt1 != null)
                    {
                        this.cllx.DataSource = dt1;
                        this.cllx.DataBind();
                    }

                    startdate = "";
                    enddate = "";
                    string hphm, hpzl;
                    if (Session["Condition"] != null)
                    {
                        Condition con = Session["Condition"] as Condition;
                        startdate = con.StartTime;
                        end.InnerText = enddate = con.EndTime;
                        hphm = con.Sqjc + con.Hphm;
                        cboplate.SetVehicleText(hphm.Substring(0, 1));
                        txtplate.Text = hphm.Substring(1);
                        hpzl = con.Hpzl;
                    }
                    else
                    {
                        //starttime = Request.QueryString["startTime"];
                        //endtime = Request.QueryString["endTime"];
                        hphm = Request.QueryString["hphm"];
                        hpzl = Request.QueryString["hpzl"];
                    }
                    if (!string.IsNullOrEmpty(hphm) && !string.IsNullOrEmpty(hpzl))
                    {
                        //start.InnerText = starttime;
                        //end.InnerText = endtime;
                        //startdate = starttime;
                        //enddate = endtime;
                        cboplate.SetVehicleText(hphm.Substring(0, 1));
                        txtplate.Text = hphm.Substring(1);
                        cbocllx.Value = hpzl;
                        start.InnerText = startdate;
                        end.InnerText = enddate;
                        ButQueryClick();
                    }
                }
                catch
                { }
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("FootHold29", "访问：") + Request.QueryString["funcname"], userinfo.NowIp, "0");
                this.DataBind();
            }
        }

        /// <summary>
        /// 默认查询
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void panelclose()
        {
            isdefault = true;
        }

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void panelopen()
        {
            isdefault = false;
        }

        /// <summary>
        /// panel修改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void panelchanges(object sender, EventArgs e)
        {
            if (Panel1.Collapsed)
            {
                Pantrack.Height = 200;
            }
            else
            {
                Pantrack.Height = 400;
            }
        }

        /// <summary>
        /// 获取起止时间
        /// </summary>
        /// <param name="isstart"></param>
        /// <param name="strtime"></param>
        /// <returns></returns>
        [DirectMethod]
        public void GetDateTime(bool isstart, string strtime)
        {
            if (isstart)
                startdate = strtime;
            else
                enddate = strtime;
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void ButQueryClick()
        {
            string cllx = cbocllx.Text;
            string hphm = cboplate.VehicleText + txtplate.Text;

            string ljsc = txtljsc.Text;
            string ljcs = txtljcs.Text;
            //if (string.IsNullOrEmpty(startdate)) startdate = start.InnerText;

            //if (string.IsNullOrEmpty(enddate)) enddate = end.InnerText;

            if (cllx == "" || txtplate.Text == "")
            {
                if (cllx == "" || txtplate.Text == "")
                    Notice(GetLangStr("FootHold36", "提示"), GetLangStr("FootHold35", "请输入车辆信息！"));
            }
            else
            {
                //if (isdefault)
                //    query(hphm, cllx, "", "", "", "", "");
                //else
                query(hphm, cllx, startdate, enddate, "", ljsc, ljcs);
            }
            //if()
        }

        protected void unnamed_event(object sender, EventArgs e)
        {
            ButQueryClick();
        }

        /// <summary>
        /// 行触发事件
        /// </summary>
        /// <param name="xpoint"></param>
        /// <param name="ypoint"></param>
        /// <param name="stationid"></param>
        /// <param name="stationname"></param>
        /// <param name="ljcs"></param>
        /// <returns></returns>
        [DirectMethod]
        public void SelectRow(string xpoint, string ypoint, string stationid, string stationname, string ljcs)
        {
            string HTML = "";
            string head = "<div class=\"car-location OverCar-Location \">" +
     "<div class=\"items w-220px\">" +
          "<span class=\"car-brand text-center\">" + stationname + "[落脚" + ljcs + "次]" +
          "</span>" +
          "<section class=\"car-img write-bg\">" +
           " <div class=\"CarIllegal-list\">" +
                 "<ul class=\"clearfix display-table\">" +
                //"<li class=\"font-10 fb table-cell w-20px\">序号</li>" +
                   "<li class=\"font-10 fb table-cell w-100px\">时间</li>" +
                   "<li class=\"font-10 fb table-cell w-60px\">方向</li>" +
                   "<li class=\"font-10 fb table-cell w-20px\">落脚时长</li>" +
                 "</ul>" +
                 "<ul class=\"OverCar-data-list  clearfix\">";
            string content = "";
            string end = "</ul></div></section></div></div>";
            if (Dt_passcar != null)
            {
                DataRow[] drs = Dt_passcar.Select("kkid='" + stationid + "'");
                int row = 0;
                foreach (DataRow dr in drs)
                {
                    row++;
                    content += "  <li>" +
                   "      <span class=\"text-center font-10 table-cell w-20px\">" + row.ToString("D2") + "</span>" +
                    "     <span class=\"text-cente font-10 table-cell w-100px\">" + dr["kssj"] + "</span>" +
                     "    <span class=\"text-cente font-10 table-cell w-60px\">" + dr["fxmc"] + "</span>" +
                      "   <span class=\"text-cente font-10 table-cell w-20px\">" + dr["sc"] + "</span>" +
                    "</li>";
                }
                HTML = head + content + end;
            }
            else
            {
                return;
            }
            if (xpoint == "" || ypoint == "")
                return;
            string js = "BMAP.openWindow('" + HTML + "','" + xpoint + "','" + ypoint + "');";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
            js = "BMAP.GotoXY('" + xpoint + "','" + ypoint + "');";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
            this.ResourceManager1.RegisterAfterClientInitScript("isScroll()");
            RowSelectionModel sm = this.GridStation.SelectionModel.Primary as RowSelectionModel;
            sm.SelectedRows.Clear();
            sm.UpdateSelection();
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void ButResetClick()
        {
            txtljcs.Text = "";
            txtljsc.Text = "";
            cboplate.SetVehicleText("");
            cbocllx.Text = "";
            txtplate.Text = "";
            start.InnerText = "";
            end.InnerText = "";
            startdate = "";
            startdate = "";
            string starttime = "";
            string endtime = "";
            string timeJs = "clearTime('" + starttime + "','" + endtime + "');";//js方法后面的分号一定要加上
            this.ResourceManager1.RegisterAfterClientInitScript(timeJs);
        }

        #endregion 事件

        #region 方法

        /// <summary>
        /// 组织xml查询条件
        /// </summary>
        /// <param name="rowstart"></param>
        /// <param name="len"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="cllx"></param>
        /// <param name="hphm"></param>
        /// <param name="gccs"></param>
        /// <param name="ljsc"></param>
        /// <param name="ljcs"></param>
        /// <returns></returns>
        private string getxml(int rowstart, int len, string start, string end, string cllx, string hphm, string gccs, string ljsc, string ljcs)
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
                                "<hphm>" + hphm + "</hphm>" +
                                "<hpzl>" + cllx + "</hpzl>" +
                                "<kssj>" + start + "</kssj>" +
                                "<jssj>" + end + "</jssj>" +
                                "<ljcs>" + ljcs + "</ljcs>" +
                                "<gccs>" + gccs + "</gccs>" +
                                "<ljsc>" + ljsc + "</ljsc>" +
                                "<cllx>" + cllx + "</cllx>" +
                                "<csys></csys>" +
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

        /// <summary>
        /// 解析xml文件获取记录条数
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public int getlenxml(string xmlStr)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);

                XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/kkidlist");
                return int.Parse(listNodes[0].Attributes[0].Value);
            }
            catch { return 0; }
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="hphm"></param>
        /// <param name="cllx"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="gccs"></param>
        /// <param name="ljsc"></param>
        /// <param name="ljcs"></param>
        /// <returns></returns>
        private void query(string hphm, string cllx, string start, string end, string gccs, string ljsc, string ljcs)
        {
            int startrow = 0, len = 30, endrow = 29;
            string xml = "", rsxml = "";
            Dt_passcar.Clear();
            Dt_result.Clear();
            xml = getxml(startrow, len, start, end, cllx, hphm, gccs, ljsc, ljcs);
            try
            {
                rsxml = client.GetFootholdInfo(xml);
            }
            catch (Exception ex)
            {
                Notice(GetLangStr("FootHold36", "提示"), GetLangStr("FootHold30", "接口服务报错！"));
                ILog.WriteErrorLog(ex.Message);
            }
            //rsxml = "<?xml version='1.0' encoding='UTF-8'?><Message><Version>1.0</Version><Type>RESPONSE</Type><Body><Return><kkidlist totalnum='1'><kkid name='701111007000' ljcs='1' ljsj='1' type='1'><passcar><kssj>2016-04-25 19:03:03 123</kssj><jssj>2016-04-26 19:03:03 123</jssj><fx>01</fx><sc>1</sc></passcar></kkid><kkid name='609381055000' ljcs='1' ljsj='1' type='2'><passcar><kssj>2016-04-25 19:03:03 123</kssj><jssj>2016-04-26 19:03:03 123</jssj><fx>01</fx><sc>1</sc></passcar></kkid><kkid name='609321057000' ljcs='1' ljsj='1' type='0'><passcar><kssj>2016-04-25 19:03:03 123</kssj><jssj>2016-04-26 19:03:03 123</jssj><fx>01</fx><sc>1</sc></passcar></kkid><kkid name='709011060000' ljcs='1' ljsj='1' type='1'><passcar><kssj>2016-04-25 19:03:03 123</kssj><jssj>2016-04-26 19:03:03 123</jssj><fx>01</fx><sc>1</sc></passcar></kkid><kkid name='601181002000' ljcs='1' ljsj='1' type='2'><passcar><kssj>2016-04-25 19:03:03 123</kssj><jssj>2016-04-26 19:03:03 123</jssj><fx>01</fx><sc>1</sc></passcar></kkid></kkidlist></Return></Body></Message>";

            if (rsxml != "" && getlenxml(rsxml) > 0)
            {
                CXmlToDataTable(rsxml);
                while (getlenxml(rsxml) > endrow)
                {
                    startrow = startrow + len;
                    endrow = endrow + len;
                    xml = getxml(startrow, endrow, start, end, cllx, hphm, gccs, ljsc, ljcs);
                    rsxml = client.GetFootholdInfo(xml);
                    CXmlToDataTable(rsxml);
                }
            }
            if (Dt_result != null && Dt_result.Rows.Count > 0)
            {
                DataTable dtCopy = Dt_result.Copy();
                System.Data.DataView dv = Dt_result.DefaultView;
                dv.Sort = "type desc";
                dtCopy = dv.ToTable();
                this.StoreInfo.DataSource = dtCopy;// Dt_result;
                this.StoreInfo.DataBind();
                if (start == "" && end == "" && gccs == "" && ljsc == "" && ljcs == "")
                    ShowFoot(false);
                else
                    ShowFoot(true);
            }
            else
            {
                this.StoreInfo.RemoveAll();
                this.StoreInfo.DataBind();

                Notice(GetLangStr("FootHold36", "提示"), GetLangStr("FootHold31", "无数据！"));
            }
        }

        /// <summary>
        /// 地图清空
        /// </summary>
        /// <returns></returns>
        private void clearmap()
        {
            string js = "BMAP.Clear();BMAP.ClearLine();;";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }

        /// <summary>
        /// 落脚点标注
        /// </summary>
        /// <param name="isline"></param>
        /// <returns></returns>
        private void ShowFoot(bool isline)
        {
            try
            {
                string js = "";
                string points = "";
                string type = "";
                clearmap();
                foreach (DataRow dr in Dt_result.Rows)
                {
                    type = dr["type"].ToString();

                    js = "BMAP.addMarkerlabel('img/" + (type == "0" ? "f_other" : (type == "1" ? "f_home" : "f_depart")) + ".png','" + dr["xpoint"].ToString() + "','" + dr["ypoint"].ToString() + "','" + dr["kkmc"].ToString() + "','');";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                    points += (points == "" ? "" : "|") + dr["xpoint"].ToString() + "," + dr["ypoint"].ToString();
                }
                //if (isline)
                //{
                //    js = "BMAP.addPolyline2('#ff0000','" + points + "','');";
                //    this.ResourceManager1.RegisterAfterClientInitScript(js);
                //}
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("FootHold.aspx-ShowFoot", ex.Message + "；" + ex.StackTrace, "ShowFoot has an exception");
            }
        }

        /// <summary>
        /// xml文件解析
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public void CXmlToDataTable(string xmlStr)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);
                XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/kkidlist/kkid ");
                foreach (XmlNode node in listNodes)
                {
                    DataRow dr = Dt_result.NewRow();
                    dr["kkid"] = (node.Attributes["name"]).InnerText;
                    dr["ljcs"] = (node.Attributes["ljcs"]).InnerText;
                    dr["ljsj"] = (node.Attributes["ljsj"]).InnerText;
                    dr["type"] = (node.Attributes["type"]).InnerText;
                    string type = (node.Attributes["type"]).InnerText;
                    DataRow[] listdr = Dt_Station.Select("STATION_ID= '" + (node.Attributes["name"]).InnerText + "'");
                    if (listdr.Length > 0)
                    {
                        dr["kkmc"] = (type == "0" ? GetLangStr("FootHold32", "(其他)") : type == "1" ? GetLangStr("FootHold33", "(家)") : GetLangStr("FootHold34", "(单位)")) + listdr[0]["STATION_NAME"].ToString();
                        dr["xpoint"] = listdr[0]["xpoint"].ToString();
                        dr["ypoint"] = listdr[0]["ypoint"].ToString();
                        dr["kkid"] = listdr[0]["STATION_ID"].ToString();
                        dr["ICOREMARK"] = listdr[0]["ICOREMARK"].ToString();
                    }
                    else
                    {
                        dr["kkmc"] = "";
                        dr["xpoint"] = "";
                        dr["ypoint"] = "";
                        dr["ICOREMARK"] = "com";
                    }
                    Dt_result.Rows.Add(dr);
                    XmlNodeList list = node.SelectNodes("passcar");
                    foreach (XmlNode xn in list)
                    {
                        DataRow drpass = Dt_passcar.NewRow();
                        drpass["kkid"] = dr["kkid"].ToString();
                        DataRow[] listdrfx = Dt_xsfx.Select("code= '" + (xn.SelectSingleNode("fx")).InnerText + "'");
                        if (listdrfx.Length > 0)
                            drpass["fxmc"] = listdrfx[0]["codedesc"].ToString();
                        else
                            drpass["fxmc"] = "";

                        drpass["kssj"] = (xn.SelectSingleNode("kssj")).InnerText.Substring(0, 19);
                        drpass["jssj"] = (xn.SelectSingleNode("jssj")).InnerText;
                        drpass["sc"] = (xn.SelectSingleNode("sc")).InnerText;
                        Dt_passcar.Rows.Add(drpass);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("FootHold.aspx-CXmlToDataTable", ex.Message + "；" + ex.StackTrace, "CXmlToDataTable has an exception");
                //Notice("GetLangStr("FootHold36", "提示")", "接口服务报错！（版本）");
            }
        }

        /// <summary>
        /// GetLangStr("FootHold36", "提示")框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
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
        /// 获取passcar查询结果结构
        /// </summary>
        /// <returns></returns>
        private DataTable GetResultData()
        {
            DataTable dt = new DataTable("PassResult");
            dt.Columns.Add("kkid", Type.GetType("System.String"));
            dt.Columns.Add("kkmc", Type.GetType("System.String"));
            dt.Columns.Add("ljcs", Type.GetType("System.String"));//落脚次数
            dt.Columns.Add("ljsj", Type.GetType("System.String"));//落脚时间
            dt.Columns.Add("xpoint", Type.GetType("System.String"));
            dt.Columns.Add("ypoint", Type.GetType("System.String"));
            dt.Columns.Add("ICOREMARK", Type.GetType("System.String"));
            dt.Columns.Add("type", Type.GetType("System.String"));
            return dt;
        }

        /// <summary>
        /// 获取passcar结构
        /// </summary>
        /// <returns></returns>
        private DataTable GetData()
        {
            DataTable dt = new DataTable("PassCar");
            dt.Columns.Add("kkid", Type.GetType("System.String"));//kkid
            dt.Columns.Add("kssj", Type.GetType("System.String"));//开始时间
            dt.Columns.Add("jssj", Type.GetType("System.String"));//结束时间
            dt.Columns.Add("fxmc", Type.GetType("System.String"));//方向
            dt.Columns.Add("sc", Type.GetType("System.String"));//时长
            return dt;
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