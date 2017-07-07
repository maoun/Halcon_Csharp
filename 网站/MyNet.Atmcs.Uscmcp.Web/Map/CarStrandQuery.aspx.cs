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
    public partial class CarStrandQuery : System.Web.UI.Page
    {
        #region 变量

        private static DataTable Dt_Station = new DataTable();
        private static DataTable Dt_passcar = new DataTable();
        private static DataTable Dt_result = new DataTable();
        private static DataTable Dt_xsfx = new DataTable();
        private static double EARTH_RADIUS = 6378.137;//地球半径
        private static QueryService.querypasscar client = new QueryService.querypasscar();
        private MapManager bll = new MapManager();
        private static string startdate, enddate;
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        #endregion 变量

        #region 事件

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
                try
                {
                    Dt_Station = bll.GetStation();
                    Dt_passcar = GetData();
                    Dt_xsfx = bll.GetFxcode();
                    this.cllxstore.DataSource = GetRedisData.GetData("t_sys_code:140001");
                    this.cllxstore.DataBind();
                    this.csysstore.DataSource = GetRedisData.GetData("t_sys_code:240013");
                    this.csysstore.DataBind();

                    DataSet dsclpp = bll.GetClpp();
                    if (dsclpp != null && dsclpp.Tables[0].Rows.Count > 0)
                    {
                        this.Clpp.DataSource = dsclpp.Tables[0];
                        this.Clpp.DataBind();
                    }

                    start.InnerText = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    end.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    startdate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    enddate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    this.DataBind();
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：" + Request.QueryString["funcname"], userinfo.NowIp, "0");
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex.Message);
                    logManager.InsertLogError("CarStrandQuery.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
                }
            }
        }

        /// 查询
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private void query(int page)
        {
            try
            {
                if (startdate == null || enddate == null)
                {
                    Notice("提示", "时间范围未选择！");
                    return;
                }
                if ((DateTime.Parse(enddate) - DateTime.Parse(startdate)).TotalHours > 24)
                {
                    Notice("提示", "时间范围不能多于24个小时！");
                    return;
                }

                ButAddgrid.Enabled = false;
                string cllx = cbocllx.Text;
                string csys = cbocsys.Text;
                //string clpp = (cboclpp.SelectedItem.Text == null ? "" : cboclpp.SelectedItem.Text.ToString());
                //string clxh = (cboclxh.SelectedItem.Text == null ? "" : cboclxh.SelectedItem.Text.ToString());
                //if (!string.IsNullOrEmpty(clxh))
                //{
                //    clpp = clxh;
                //}
                string clpp = ClppChoice.Value.ToString();
                string hphm = "";
                hphm = (cboplate.VehicleText == "" ? "_" : cboplate.VehicleText) +
                (string.IsNullOrEmpty(haopai_name1.Value) ? "_" : haopai_name1.Value) +
                    (string.IsNullOrEmpty(haopai_name2.Value) ? "_" : haopai_name2.Value) +
                    (string.IsNullOrEmpty(haopai_name3.Value) ? "_" : haopai_name3.Value) +
                    (string.IsNullOrEmpty(haopai_name4.Value) ? "_" : haopai_name4.Value) +
                    (string.IsNullOrEmpty(haopai_name5.Value) ? "_" : haopai_name5.Value) +
                    (string.IsNullOrEmpty(haopai_name6.Value) ? "_" : haopai_name6.Value);
                //(labelhm1.Text == "" ? "_" : labelhm1.Text.ToUpper()) +
                //(labelhm2.Text == "" ? "_" : labelhm2.Text.ToUpper()) +
                //(labelhm3.Text == "" ? "_" : labelhm3.Text.ToUpper()) +
                //(labelhm4.Text == "" ? "_" : labelhm4.Text.ToUpper()) +
                //(labelhm5.Text == "" ? "_" : labelhm5.Text.ToUpper()) +
                //(labelhm6.Text == "" ? "_" : labelhm6.Text.ToUpper());
                if (hphm.Substring(1, 6) == "______")
                    hphm = cboplate.VehicleText == "" ? "" : cboplate.VehicleText + "%";
                if (hphm == "")
                {
                    Notice("提示", "号牌号码不能为空！");
                    return;
                }
                List<string> listkkid = new List<string>();
                RowSelectionModel sm = this.kkgp.SelectionModel.Primary as RowSelectionModel;
                foreach (SelectedRow row in sm.SelectedRows)
                {
                    listkkid.Add(row.RecordID);
                }

                if (listkkid.Count < 1)
                {
                    Notice("提示", "未选择卡口！");
                    return;
                }

                query(startdate, enddate, cllx, "", hphm, clpp, csys, listkkid, page);

                ButAddgrid.Enabled = true;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("CarStrandQuery.aspx-query", ex.Message + "；" + ex.StackTrace, "query has an exception");
            }
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
                logManager.InsertLogError("CarStrandQuery.aspx-TbutFisrt", ex.Message + "；" + ex.StackTrace, "TbutFisrt has an exception");
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
                logManager.InsertLogError("CarStrandQuery.aspx-TbutEnd", ex.Message + "；" + ex.StackTrace, "TbutEnd has an exception");
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
                logManager.InsertLogError("CarStrandQuery.aspx-TbutLast", ex.Message + "；" + ex.StackTrace, "TbutLast has an exception");
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
                if (page < int.Parse(totalpage.Value.ToString()) - 1)
                    page++;
                curpage.Value = page;
                query(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CarStrandQuery.aspx-TbutNext", ex.Message + "；" + ex.StackTrace, "TbutNext has an exception");
            }
        }

        [DirectMethod]
        public void clpp_chang()
        {
            //if (cboclpp.Value != null)
            //{
            //    DataSet dsclxh = bll.GetClxh(cboclpp.SelectedItem.Text.ToString());
            //    if (dsclxh != null)
            //    {
            //        Clxh.DataSource = dsclxh.Tables[0];
            //        Clxh.DataBind();
            //        //if (dsclxh.Tables[0].Rows.Count > 0)
            //        //    cboclxh.SelectedIndex = 0;
            //    }
            //}
            //else
            //{
            //    Clxh.RemoveAll();
            //    Clxh.DataBind();
            //}
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
        /// 查询事件
        /// </summary>
        [DirectMethod]
        public void ButQueryClick()
        {
            try
            {
                totalinfo = 0;
                curpage.Value = 0;
                query(0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("CarStrandQuery.aspx-ButQueryClick", ex.Message + "；" + ex.StackTrace, "ButQueryClick has an exception");
            }
        }

        /// <summary>
        /// 行选择事件
        /// </summary>
        /// <param name="xpoint">x坐标</param>
        /// <param name="ypoint">y坐标</param>
        /// <param name="hphm">号牌号码</param>
        /// <param name="zjwj">证据文件</param>
        [DirectMethod]
        public void SelectRow(string xpoint, string ypoint, string hphm, string zjwj, string gwsj, string fx, string kkname)
        {
            //zjwj = "http://192.168.1.249:8001/capture/2016/01/01/13/100000010762/100000010762_黄_鲁NA7506_20160101134049_2_01_0_0_0.JPG";
            try
            {
                string HTML = "<div class=\"car-location\" > "
         + "<a href=\"#\" class=\"items w-220px\">"
                    //+ "<b class=\"tips-arrow\"></b>"
              + "<span class=\"car-brand\">当前车-" + hphm + "</span>"
                    //+ "<span class=\"car-brand\">"+hphm+"[" + gwsj +"]</span>"
              + "<span class=\"car-img\"><img src=\"" + zjwj + "\" style=\"cursor: pointer\" onclick=\"zoom(this,false);\" ondblclick=\"OpenPicPage(this.src);\" class=\" move-scale-b\"></span>"
          + "</a>"
        + "</div>";
                string js = "BMAP.openWindow('" + HTML + "','" + xpoint + "','" + ypoint + "');";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("CarStrandQuery.aspx-SelectRow", ex.Message + "；" + ex.StackTrace, "SelectRow has an exception");
            }
        }

        /// <summary>
        /// 单击地图触发事件
        /// </summary>
        /// <param name="xpos">x坐标</param>
        /// <param name="ypos">y坐标</param>
        [DirectMethod]
        public void AddPosPoints(string xpos, string ypos)
        {
            int r = 1000;
            try
            { r = int.Parse(arear.Text); }
            catch
            { arear.Text = "1000"; }
            string pos = xpos + "," + ypos;
            Pantrack.Title = "案发地点  " + pos;
            string js = "BMAP.addMarker('img/COM.png','" + pos.Split(',')[0] + "','" + pos.Split(',')[1] + "','案发地点','');";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
            js = "BMAP.addCircleEvent('" + pos.Split(',')[0] + "','" + pos.Split(',')[1] + "','" + r.ToString() + "');";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
            QueryMarkArea(xpos, ypos, r);
        }

        protected void QueryClick_event(object sender, EventArgs e)
        {
            ButQueryClick();
        }

        #endregion 事件

        #region 方法

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
        private string getxml(int rowstart, int len, string start, string end, string cllx, string clxh, string clpp, string csys, string hphm, string kkid)
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
                                    "<LogInfo>" +
                                    "<userName>" + username + "</userName>" +
                                    "<userIp>" + userip + "</userIp>" +
                                    "<userCode>" + usercode + "</userCode>" +
                                    "<dyzgnmkbh>" + dyzgnmkbh + "</dyzgnmkbh>" +
                                    "<dyzgnmkmc>" + dyzgnmkmc + "</dyzgnmkmc>" +
                                "</LogInfo>" +
                           "</Body>" +
                       "</Message>";
                ILog.WriteErrorLog("相似串并xml:" + xml);
                return xml;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("CarStrandQuery.aspx-getxml", ex.Message + "；" + ex.StackTrace, "getxml has an exception");
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
        private void query(string start, string end, string cllx, string clxh, string hphm, string clpp, string csys, List<string> listkkid, int page)
        {
            try
            {
                int startrow = 0, len = 15, endrow = 15;
                startrow = startrow + len * page;
                endrow = endrow + len * page;
                string xml = "", rsxml = "";
                Dt_passcar.Clear();
                Dt_passcar = GetData();
                string kkid = "";
                foreach (string id in listkkid)
                {
                    kkid += (kkid == "" ? "" : ",") + id;
                }
                xml = getxml(startrow, endrow, start, end, cllx, clxh, clpp, csys, hphm, kkid);
                try
                {
                    rsxml = client.GetPassCarInfo(xml);
                }
                catch (Exception ex)
                {
                    Notice("提示", "接口服务报错！");
                    ILog.WriteErrorLog(ex.Message);
                    logManager.InsertLogError("CarStrandQuery.aspx-query", ex.Message + "；" + ex.StackTrace, "query has an exception");
                }
                Dt_passcar.Clear();
                Dt_passcar = GetData();
                //大数据查询接口
                if (rsxml != "" && getlenxml(rsxml) > 0)
                {
                    CXmlToDataTable(rsxml);

                    if (Dt_passcar != null && Dt_passcar.Rows.Count > 0)
                    {
                        this.StoreInfo.DataSource = Dt_passcar;
                        this.StoreInfo.DataBind();
                    }
                    labpage.Text = "共" + totalinfo + "条记录，当前" + (page + 1).ToString() + "页,共" + totalpage.Value.ToString() + "页";
                }
                else
                {
                    this.StoreInfo.RemoveAll();
                    this.StoreInfo.DataBind();
                    labpage.Text = "共0条记录，当前0页,共0页";
                    Notice("提示", "无数据！");
                }
                RowSelectionModel sm = this.GridStation.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRows.Clear();
                sm.UpdateSelection();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("CarStrandQuery.aspx-query", ex.Message + "；" + ex.StackTrace, "query has an exception");
            }
        }

        /// <summary>
        /// xml文件解析
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
                    dr[0] = (node.SelectSingleNode("hphm")).InnerText;
                    dr[1] = (node.SelectSingleNode("kkid")).InnerText;
                    DataRow[] listdr = Dt_Station.Select("STATION_ID= '" + (node.SelectSingleNode("kkid")).InnerText + "'");
                    if (listdr.Length > 0)
                    {
                        dr["lkmc"] = listdr[0]["STATION_NAME"].ToString();
                        dr["xpoint"] = listdr[0]["xpoint"].ToString();
                        dr["ypoint"] = listdr[0]["ypoint"].ToString();
                        dr["kkid"] = listdr[0]["STATION_ID"].ToString();
                    }
                    else
                    {
                        dr["lkmc"] = "";
                        dr["xpoint"] = "";
                        dr["ypoint"] = "";
                        dr["ypoint"] = node.SelectSingleNode("kkid").InnerText;
                    }
                    DataRow[] listdrfx = Dt_xsfx.Select("code= '" + (node.SelectSingleNode("fxbh")).InnerText + "'");
                    if (listdrfx.Length > 0)
                        dr["fxmc"] = listdrfx[0]["codedesc"].ToString();
                    else
                        dr["fxmc"] = "";
                    dr["cdbh"] = (node.SelectSingleNode("cdbh")).InnerText;
                    dr["gwsj"] = (node.SelectSingleNode("gwsj")).InnerText;
                    dr["zjwj"] = (node.SelectSingleNode("zjwj1")).InnerText;
                    dr["clpp"] = Bll.Common.Changenull((node.SelectSingleNode("clpp")).InnerText);
                    dr["csys"] = (node.SelectSingleNode("csys")).InnerText;
                    Dt_passcar.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                Notice("提示", "接口服务错误！(版本)");
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("CarStrandQuery.aspx-CXmlToDataTable", ex.Message + "；" + ex.StackTrace, "CXmlToDataTable has an exception");
            }
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
                XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/passcarinfolist");
                if (int.Parse(listNodes[0].Attributes[0].Value) == 0)
                {
                    totalpage.Value = 0;
                    totalinfo = 0;
                }
                else
                {
                    totalpage.Value = (int.Parse(listNodes[0].Attributes[0].Value) % 15 == 0 ? int.Parse(listNodes[0].Attributes[0].Value) / 15 : int.Parse(listNodes[0].Attributes[0].Value) / 15 + 1);
                    totalinfo = int.Parse(listNodes[0].Attributes[0].Value);
                }
                return int.Parse(listNodes[0].Attributes[0].Value);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("CarStrandQuery.aspx-getlenxml", ex.Message + "；" + ex.StackTrace, "getlenxml has an exception");
                return 0;
            }
        }

        private static int totalinfo = 0;

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
        /// 依据坐标点获取监测点
        /// </summary>
        /// <param name="xpos">x坐标</param>
        /// <param name="ypos">y坐标</param>
        /// <param name="r">半径</param>
        public void QueryMarkArea(string xpos, string ypos, int r)
        {
            try
            {
                PointF dot = new PointF();
                dot.X = float.Parse(xpos);
                dot.Y = float.Parse(ypos);
                DataTable dt = bll.GetStation();
                DataTable dtOut;
                if (dt != null)
                    dtOut = dt.Copy();
                else
                    return;
                PointF QueryPoint = new PointF();
                Hashtable hs = new Hashtable();
                double l;
                for (int n = dtOut.Rows.Count - 1; n >= 0; n--)
                {
                    if (dtOut.Rows[n]["xpoint"].ToString() == "" || dtOut.Rows[n]["ypoint"].ToString() == "")
                    {
                        dtOut.Rows[n].Delete();
                        dtOut.AcceptChanges();
                        continue;
                    }
                    QueryPoint.X = float.Parse(dtOut.Rows[n]["xpoint"].ToString());
                    QueryPoint.Y = float.Parse(dtOut.Rows[n]["ypoint"].ToString());
                    l = GetDistance(QueryPoint.X, QueryPoint.Y, dot.X, dot.Y);

                    if (l > r)
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
                    Pantrack.Height = 800;
                }
                else
                {
                    //panelkk.Collapsed = true;
                    Pantrack.Height = 240;
                    StoreKK.RemoveAll();
                    StoreKK.DataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("CarStrandQuery.aspx-QueryMarkArea", ex.Message + "；" + ex.StackTrace, "QueryMarkArea has an exception");
            }
        }

        /// <summary>
        /// 地图标注监测点
        /// </summary>
        /// <param name="dt">监测点数据集</param>
        private void showstation(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string js = " BMAP.addMarker('img/" + dr["ICOREMARK"].ToString() + ".gif','" + dr["xpoint"].ToString() + "','" + dr["ypoint"].ToString() + "','" + dr["STATION_NAME"].ToString() + "');;";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
        }

        #region 地图计算公式

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

        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            //double radLat1 = rad(lat1);
            //double radLat2 = rad(lat2);
            //double a = radLat1 - radLat2;
            //double b = rad(lng1) - rad(lng2);
            //double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
            // Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            //s = s * EARTH_RADIUS;
            //s = Math.Round(s * 10000 * 1000) / 10000;//米

            double ll = CalcMil(lat1, lng1, lat2, lng2);

            return ll;
        }

        private static double CalcMil(double X1, double Y1, double X2, double Y2)
        {
            double PI = 3.1415926535898;
            double EARTH_RADIUS = 6378137;  //地球半径

            double CurRadLong = 0;	//两点经纬度的弧度
            double CurRadLat = 0;
            double PreRadLong = 0;
            double PreRadLat = 0;
            double a = 0, b = 0;              //经纬度弧度差
            double MilValue = 0;

            //将经纬度换算成弧度
            CurRadLong = (double)(X1);
            CurRadLong = CurRadLong * PI / 180.0;

            PreRadLong = (double)(X2);
            PreRadLong = PreRadLong * PI / 180.0;

            CurRadLat = (double)(Y1);
            CurRadLat = CurRadLat * PI / 180.0f;

            PreRadLat = (double)(Y2);
            PreRadLat = PreRadLat * PI / 180.0f;

            //计算经纬度差值
            if (CurRadLat > PreRadLat)
            {
                a = CurRadLat - PreRadLat;
            }
            else
            {
                a = PreRadLat - CurRadLat;
            }

            if (CurRadLong > PreRadLong)
            {
                b = CurRadLong - PreRadLong;
            }
            else
            {
                b = PreRadLong - CurRadLong;
            }

            MilValue = 2 * Math.Asin(Math.Sqrt(Math.Sin(a / 2.0) * Math.Sin(a / 2.0) + Math.Cos(CurRadLat) * Math.Cos(PreRadLat) * Math.Sin(b / 2.0) * Math.Sin(b / 2.0)));
            MilValue = (double)(EARTH_RADIUS * MilValue);
            return MilValue;
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
            catch
            {
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

        #endregion 地图计算公式

        /// <summary>
        /// 获取passcar结构
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
            dt.Columns.Add("zjwj", Type.GetType("System.String"));
            dt.Columns.Add("csys", Type.GetType("System.String"));
            dt.Columns.Add("clpp", Type.GetType("System.String"));
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