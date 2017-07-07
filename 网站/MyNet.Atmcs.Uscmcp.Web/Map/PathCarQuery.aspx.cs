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
// <copyright file="PathCarQuery.aspx.cs" company="ZKLT">
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
    /// Class PathCarQuery.
    /// </summary>
    public partial class PathCarQuery : System.Web.UI.Page
    {
        #region 变量

        /// <summary>
        /// The BLL
        /// </summary>
        private MyNet.Atmcs.Uscmcp.Bll.MapManager bll = new Bll.MapManager();

        /// <summary>
        /// The client
        /// </summary>
        private static QueryService.querypasscar client = new QueryService.querypasscar();

        /// <summary>
        /// The user login
        /// </summary>
        private UserLogin userLogin = new UserLogin();

        /// <summary>
        /// 监测点
        /// </summary>
        private static DataTable Dt_Station = new DataTable();

        /// <summary>
        /// 过车集合
        /// </summary>
        private static DataTable Dt_passcar = new DataTable();

        /// <summary>
        /// 行驶方向
        /// </summary>
        private static DataTable Dt_xsfx = new DataTable();

        /// <summary>
        /// 起止时间，播放状态
        /// </summary>
        private static string startdate = "", enddate = "", play = "";

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
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
                string js = "alert('" + GetLangStr("PathCarQuery33", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                this.DataBind();
                try
                {
                    ButPlay.Text = GetLangStr("PathCarQuery34", "播放");
                    Dt_Station = bll.GetStation();
                    Dt_passcar = GetData();
                    DataTable dt1 = GetRedisData.GetData("t_sys_code:240025");//
                    if (dt1 != null)
                    {
                        Dt_xsfx = dt1;
                    }
                    else
                    {
                        Dt_xsfx = bll.GetFxcode();
                    }
                    DataTable dt2 = GetRedisData.GetData("t_sys_code:140001");
                    if (dt2 != null)
                    {
                        this.cllx.DataSource = dt2;
                        this.cllx.DataBind();
                    }

                    string hphm, hpzl;

                    if (Session["Condition"] != null)
                    {
                        Condition con = Session["Condition"] as Condition;
                        startdate = con.StartTime;
                        enddate = con.EndTime;
                        hphm = con.Sqjc + con.Hphm;
                        cboplate.SetVehicleText(hphm.Substring(0, 1));
                        txtplate.Text = hphm.Substring(1);
                        hpzl = con.Hpzl;
                    }
                    else
                    {
                        startdate = Request.QueryString["startTime"];
                        enddate = Request.QueryString["endTime"];

                        hphm = Request.QueryString["hphm"];
                        hpzl = Request.QueryString["hpzl"];
                    }

                    if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
                    {
                        start.InnerText = startdate;
                        end.InnerText = enddate;
                    }
                    else
                    {
                        startdate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                        enddate = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
                        start.InnerText = startdate;
                        end.InnerText = enddate;
                    }
                    if (!string.IsNullOrEmpty(hphm))
                    {
                        cboplate.SetVehicleText(hphm.Trim().Substring(0, 1));
                        txtplate.Text = hphm.Trim().Substring(1);
                    }
                    if (!string.IsNullOrEmpty(hpzl))
                        cbocllx.SetValue(hpzl);
                    if (!string.IsNullOrEmpty(hphm) && !string.IsNullOrEmpty(hpzl))
                    {
                        ButQueryClick();
                    }

                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, GetLangStr("PathCarQuery35", "访问：") + Request.QueryString["funcname"], userinfo.NowIp, "0");
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex.Message);
                    logManager.InsertLogError("PathCarQuery.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
                }
            }
        }

        protected void unnamed_event(object sender, EventArgs e)
        {
            ButQueryClick();
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
            try
            {
                if (isstart)
                    startdate = strtime;
                else
                    enddate = strtime;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("PathCarQuery.aspx-GetDateTime", ex.Message + "；" + ex.StackTrace, "GetDateTime has an exception");
            }
        }

        /// <summary>
        /// 行选择事件
        /// </summary>
        /// <param name="xpoint"></param>
        /// <param name="ypoint"></param>
        /// <param name="hphm"></param>
        /// <param name="zjwj"></param>
        /// <returns></returns>
        [DirectMethod]
        public void SelectRow(string xpoint, string ypoint, string hphm, string zjwj)
        {
            try
            {
                //zjwj = "http://192.168.1.249:8001/capture/2016/01/01/13/100000010762/100000010762_黄_鲁NA7506_20160101134049_2_01_0_0_0.JPG";
                string HTML = "<div id=\"view\" class=\"car-location path-poup-content\" > "
         + "<a href=\"#\" class=\"items w-220px\">"
              + "<span class=\"car-brand\">" + GetLangStr("PathCarQuery36", " 当前车-") + hphm + "</span>"
              + "<span class=\"car-img path-poup-img\"><img src=\"" + zjwj + "\"  onclick=\"zoom(this,false);\" ondblclick=\"OpenPicPage(this.src);\" class=\" move-scale-b\"></span>"
          + "</a>"
        + "</div>";
                if (xpoint != "")
                {
                    string js = "BMAP.openWindow('" + HTML + "','" + xpoint + "','" + ypoint + "');";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("PathCarQuery.aspx-SelectRow", ex.Message + "；" + ex.StackTrace, "SelectRow has an exception");
            }
            RowSelectionModel sm = this.GridRoadManager.SelectionModel.Primary as RowSelectionModel;
            sm.SelectedRows.Clear();
            sm.UpdateSelection();
        }

        /// <summary>
        /// Getwinhtmls the specified HPHM.
        /// </summary>
        /// <param name="hphm"></param>
        /// <param name="zjwj"></param>
        /// <returns></returns>
        private string getwinhtml(string hphm, string zjwj)
        {
            //return "<div  class=\"car-location path-poup-content\" > "
            //     + "<a href=\"#\" class=\"items w-220px\">"
            //          + "<span class=\"car-brand\">当前车-" + hphm + "</span>"
            //          + "<span class=\"car-img path-poup-img\"><img src=\"" + zjwj + "\" ondblclick=\"OpenPicPage(this.src);\" class=\" move-scale-b\"></span>"
            //      + "</a>"
            //    + "</div>";
            string HTML = "<div id=\"view\" class=\"car-location path-poup-content\" > "
        + "<a href=\"#\" class=\"items w-220px\">"
             + "<span class=\"car-brand\">" + GetLangStr("PathCarQuery36", " 当前车-") + hphm + "</span>"
             + "<span class=\"car-img path-poup-img\"><img src=\"" + zjwj + "\"  onclick=\"zoom(this,false);\" ondblclick=\"OpenPicPage(this.src);\" class=\" move-scale-b\"></span>"
         + "</a>"
       + "</div>";
            return HTML;
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void ButQueryClick()
        {
            try
            {
                ButAddgrid.Enabled = false;
                ButPlay.Hidden = true;
                rdgpsd.Hidden = true;
                ButPlay.Text = GetLangStr("PathCarQuery34", "播放");
                play = GetLangStr("PathCarQuery34", "播放");
                string hphm = cboplate.VehicleText + txtplate.Text;
                string cllx = cbocllx.Text;
                if (string.IsNullOrEmpty(startdate)) startdate = start.InnerText;
                if (string.IsNullOrEmpty(enddate)) enddate = end.InnerText;
                if (startdate == "" || hphm == "" || cllx == "")
                    Notice(GetLangStr("PathCarQuery37", "提示"), GetLangStr("PathCarQuery38", "轨迹查询条件不完整"));
                else
                    query(0);
                curpage.Value = 0;
                ButAddgrid.Enabled = true;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("PathCarQuery.aspx-ButQueryClick", ex.Message + "；" + ex.StackTrace, "ButQueryClick has an exception");
            }
        }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void ButPlayClick(object sender, EventArgs e)
        {
            #region
            //    switch (play)
            //    {
            //        case "播放":
            //            play = "暂停";
            //            ButPlay.Text = "暂停";
            //            //playline();
            //            //mythread = new System.Threading.Thread(playline);
            //            //mythread.Start();
            //            break;
            //        case "暂停":
            //            play = "继续";
            //            ButPlay.Text = "继续";
            //            break;
            //        case "继续":
            //            play = "暂停";
            //            ButPlay.Text = "暂停";
            //            break;
            //    }
            #endregion 事件
            RowSelectionModel sm = this.GridRoadManager.SelectionModel.Primary as RowSelectionModel;
            sm.SelectedRows.Clear();
            sm.SelectedRows.Add(new SelectedRow(Dt_passcar.Rows[row]["gwsj"].ToString()));
            sm.UpdateSelection();

            row++;
            Reset();
            //playcar();
        }

        /// <summary>
        /// Buts the play click.
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void ButPlayClick()
        {
            this.GridRoadManager.GetView().FocusRow(Dt_passcar.Rows.Count - 1);
            playcar();
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void Reset()
        {
            txtplate.Text = "";
            cbocllx.Clear();
            cboplate.SetVehicleText("");
            string starttime = "";
            string endtime = "";
            string timeJs = "clearTime('" + starttime + "','" + endtime + "');";//js方法后面的分号一定要加上
            this.ResourceManager1.RegisterAfterClientInitScript(timeJs);
        }

        /// <summary>
        /// The row
        /// </summary>
        private static int row = 0;

        #endregion
        #region 方法

        /// <summary>
        /// 播放行车路线
        /// </summary>
        /// <returns></returns>
        private void playcar()
        {
            try
            {
                string js = "BMAP.ClearLine();;";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
                if (Dt_passcar != null)
                {
                    System.Data.DataView dv = Dt_passcar.DefaultView;
                    dv.Sort = "gwsj Asc";
                    DataTable dt2 = dv.ToTable();
                    string zjwj = "";
                    int rows = dt2.Rows.Count;
                    string x1 = "", x2 = "", y1 = "", y2 = "", xh = "";
                    for (int row = 0; row < dt2.Rows.Count; row++)
                    {
                        x1 = x2;
                        y1 = y2;
                        if (dt2.Rows[row]["xpoint"].ToString() != "")
                        {
                            x2 = dt2.Rows[row]["xpoint"].ToString();
                            y2 = dt2.Rows[row]["ypoint"].ToString();
                            zjwj = getwinhtml(dt2.Rows[row]["hphm"].ToString(), dt2.Rows[row]["zjwj"].ToString());
                            xh = dt2.Rows[row]["xh"].ToString();
                            string sjjg = "2000";
                            if (rd01.Checked)
                                sjjg = "4000";
                            if (rd03.Checked)
                                sjjg = "1000";
                            this.ResourceManager1.RegisterAfterClientInitScript("DrawLine('" + x1 + "','" + y1 + "','" + x2 + "','" + y2 + "','" + zjwj + "','" + (row + 1).ToString() + "','" + xh + "','" + sjjg + "');");
                            //setrow(gwsj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("PathCarQuery.aspx-playcar", ex.Message + "；" + ex.StackTrace, "playcar has an exception");
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
                logManager.InsertLogError("PathCarQuery.aspx-TbutFisrt", ex.Message + "；" + ex.StackTrace, "TbutFisrt has an exception");
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
                logManager.InsertLogError("PathCarQuery.aspx-TbutEnd", ex.Message + "；" + ex.StackTrace, "TbutEnd has an exception");
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
                logManager.InsertLogError("PathCarQuery.aspx-TbutLast", ex.Message + "；" + ex.StackTrace, "TbutLast has an exception");
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
                logManager.InsertLogError("PathCarQuery.aspx-TbutNext", ex.Message + "；" + ex.StackTrace, "TbutNext has an exception");
            }
        }

        /// <summary>
        /// Setrows the specified xh.
        /// </summary>
        /// <param name="xh"></param>
        /// <returns></returns>
        [DirectMethod]
        public void setrow(string xh)
        {
            try
            {
                RowSelectionModel sm = this.GridRoadManager.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRows.Clear();

                sm.SelectedRows.Add(new SelectedRow(int.Parse(xh)));
                sm.UpdateSelection();

                foreach (SelectedRow row in sm.SelectedRows)
                {
                    this.GridRoadManager.GetView().FocusRow(row.RowIndex);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("PathCarQuery.aspx-setrow", ex.Message + "；" + ex.StackTrace, "setrow has an exception");
            }
        }

        /// <summary>
        /// Playlineses this instance.
        /// </summary>
        /// <returns></returns>
        private void playlines()
        {
            try
            {
                string js = "";
                int i = 0, j = 0;
                string points = "";
                foreach (DataRow dr in Dt_Station.Rows)
                {
                    if (dr["xpoint"].ToString() != "")
                    {
                        js = " BMAP.addMarkerlabel('img/car.png','" + dr["xpoint"].ToString() + "','" + dr["ypoint"].ToString() + "','');;";
                        this.ResourceManager1.RegisterAfterClientInitScript(js);
                        points += (points == "" ? "" : "|") + dr["xpoint"].ToString() + "," + dr["ypoint"].ToString();
                        if (i > 0)
                        {
                            js = "BMAP.addPolyline2('#ffff00','" + points + "','');";
                            this.ResourceManager1.RegisterAfterClientInitScript(js);
                        }
                        while (j < 1000000)
                        {
                            j++;
                        }
                        j = 0;
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("PathCarQuery.aspx-playlines", ex.Message + "；" + ex.StackTrace, "playlines has an exception");
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private void query(int page)
        {
            try
            {
                ButAddgrid.Enabled = false;
                ButPlay.Hidden = true;
                rdgpsd.Hidden = true;
                ButPlay.Text = GetLangStr("PathCarQuery34", "播放");
                play = GetLangStr("PathCarQuery34", "播放");
                string hphm = cboplate.VehicleText + txtplate.Text;
                string cllx = cbocllx.Text;
                if (string.IsNullOrEmpty(startdate)) startdate = start.InnerText;
                if (string.IsNullOrEmpty(enddate)) enddate = end.InnerText;
                if (startdate == "" || hphm == "" || cllx == "")
                {
                    Notice(GetLangStr("PathCarQuery37", "提示"), GetLangStr("PathCarQuery38", "轨迹查询条件不完整"));
                    return;
                }
                int startrow = 0, len = 50, endrow = 50;
                startrow = startrow + len * page;
                endrow = endrow + len * page;
                string xml = getxml(startrow, len, hphm, startdate, enddate, cllx);
                string rsxml = "";
                try
                {
                    rsxml = client.GetPassCarInfo(xml);
                }
                catch (Exception ex)
                {
                    Notice(GetLangStr("PathCarQuery37", "提示"), GetLangStr("PathCarQuery39", "接口服务报错！"));
                    ILog.WriteErrorLog(ex.Message);
                }
                Dt_passcar.Clear();
                Dt_passcar = GetData();
                //大数据查询接口
                if (rsxml != "" && getlenxml(rsxml) > 0)
                {
                    CXmlToDataTable(rsxml, page);
                    //while (getlenxml(rsxml) > endrow)
                    //{
                    //    startrow = startrow + len;
                    //    endrow = endrow + len;
                    //    xml = getxml(startrow, endrow, hphm, start, end, cllx);
                    //    rsxml = client.GetPassCarInfo(xml);
                    //    CXmlToDataTable(rsxml);
                    //}
                    if (Dt_passcar != null && Dt_passcar.Rows.Count > 0)
                    {
                        //System.Data.DataView dv = Dt_passcar.DefaultView;
                        //dv.Sort = "gwsj desc";
                        //Dt_passcar = dv.ToTable();
                        this.StoreInfo.DataSource = Dt_passcar;
                        this.StoreInfo.DataBind();
                        ShowStatoin();
                        ButPlay.Hidden = false;
                        rdgpsd.Hidden = false;
                        RowSelectionModel sm = this.GridRoadManager.SelectionModel.Primary as RowSelectionModel;
                        sm.SelectedRows.Add(new SelectedRow(0));
                        sm.UpdateSelection();
                    }
                    labpage.Text = GetLangStr("PathCarQuery40", "当前") + (page + 1).ToString() + GetLangStr("PathCarQuery41", "页,共") + totalpage.Value.ToString() + GetLangStr("PathCarQuer42", "页(共") + totalinfo + GetLangStr("PathCarQuer43", "条)");
                }
                else
                {
                    this.StoreInfo.RemoveAll();
                    this.StoreInfo.DataBind();
                    string js = "BMAP.Clear();BMAP.ClearLine();";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                    Notice(GetLangStr("PathCarQuery37", "提示"), GetLangStr("PathCarQuery44", "无数据！"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("PathCarQuery.aspx-query", ex.Message + "；" + ex.StackTrace, "query has an exception");
            }
        }

        /// <summary>
        /// 提示框
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
        /// 组织xml查询条件
        /// </summary>
        /// <param name="rowstart"></param>
        /// <param name="len"></param>
        /// <param name="hphm"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="cllx"></param>
        /// <returns></returns>
        private string getxml(int rowstart, int len, string hphm, string start, string end, string cllx)
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
                                "<kkid></kkid>" +
                                "<fxbh></fxbh>" +
                                "<cdbh></cdbh>" +
                                "<hphm>" + hphm + "</hphm>" +
                                "<hpzl>" + cllx + "</hpzl>" +
                                "<kssj>" + start + "</kssj>" +
                                "<jssj>" + end + "</jssj>" +
                                "<clpp></clpp>" +
                                "<clsd></clsd>" +
                                "<csys></csys>" +
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
            ILog.WriteErrorLog("xml:" + xml);
            return xml;
        }

        /// <summary>
        /// 监测点标注
        /// </summary>
        /// <returns></returns>
        private void ShowStatoin()
        {
            try
            {
                string js = "BMAP.Clear();BMAP.ClearLine();;";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
                foreach (DataRow dr in Dt_passcar.Rows)
                {
                    if (dr["xpoint"].ToString() != "")
                    {
                        js = "BMAP.addMarkerlabel('img/" + dr["ICOREMARK"].ToString() + ".gif','" + dr["xpoint"].ToString() + "','" + dr["ypoint"].ToString() + "','" + dr["lkmc"].ToString() + "','');";
                        this.ResourceManager1.RegisterAfterClientInitScript(js);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("PathCarQuery.aspx-ShowStatoin", ex.Message + "；" + ex.StackTrace, "ShowStatoin has an exception");
            }
        }

        /// <summary>
        /// 获取xml返回记录数
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public int getlenxml(string xmlStr)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);
                XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/passcarinfolist");
                if (int.Parse(listNodes[0].Attributes[0].Value) == 0)
                    totalpage.Value = 0;
                else
                    totalpage.Value = (int.Parse(listNodes[0].Attributes[0].Value) % 50 == 0 ? int.Parse(listNodes[0].Attributes[0].Value) / 50 : int.Parse(listNodes[0].Attributes[0].Value) / 50 + 1);
                totalinfo = int.Parse(listNodes[0].Attributes[0].Value);
                return int.Parse(listNodes[0].Attributes[0].Value);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("PathCarQuery.aspx-getlenxml", ex.Message + "；" + ex.StackTrace, "getlenxml has an exception");
                return 0;
            }
        }

        private static int totalinfo = 0;

        /// <summary>
        /// xml数据解析
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public void CXmlToDataTable(string xmlStr, int page)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);
                XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/passcarinfolist/passcarinfo");

                int xh = page * 50 + 1;
                foreach (XmlNode node in listNodes)
                {
                    //ds.ReadXml(node.OuterXml);
                    DataRow dr = Dt_passcar.NewRow();
                    dr[0] = (node.SelectSingleNode("hphm")).InnerText;
                    dr[1] = (node.SelectSingleNode("kkid")).InnerText;
                    dr["xh"] = xh.ToString();
                    DataRow[] listdr = Dt_Station.Select("STATION_ID= '" + (node.SelectSingleNode("kkid")).InnerText + "'");
                    if (listdr.Length > 0)
                    {
                        dr["lkmc"] = listdr[0]["STATION_NAME"].ToString();
                        dr["xpoint"] = listdr[0]["xpoint"].ToString();
                        dr["ypoint"] = listdr[0]["ypoint"].ToString();
                        dr["kkid"] = listdr[0]["STATION_ID"].ToString();
                        dr["ICOREMARK"] = listdr[0]["ICOREMARK"].ToString();
                    }
                    else
                    {
                        dr["lkmc"] = "";
                        dr["xpoint"] = "";
                        dr["ypoint"] = "";
                        dr["kkid"] = node.SelectSingleNode("kkid").InnerText;
                        dr["ICOREMARK"] = "com";
                    }
                    DataRow[] listdrfx = Dt_xsfx.Select("code= '" + (node.SelectSingleNode("fxbh")).InnerText + "'");
                    if (listdrfx.Length > 0)
                        dr["fxmc"] = listdrfx[0]["codedesc"].ToString();
                    else
                        dr["fxmc"] = "";
                    dr["cdbh"] = (node.SelectSingleNode("cdbh")).InnerText;
                    dr["gwsj"] = (node.SelectSingleNode("gwsj")).InnerText;
                    dr["zjwj"] = (node.SelectSingleNode("zjwj1")).InnerText;

                    Dt_passcar.Rows.Add(dr);
                    xh++;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("PathCarQuery.aspx-CXmlToDataTable", ex.Message + "；" + ex.StackTrace, "CXmlToDataTable has an exception");
            }
        }

        /// <summary>
        /// 获取passcar数据结构
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
            dt.Columns.Add("ICOREMARK", Type.GetType("System.String"));
            dt.Columns.Add("xh", Type.GetType("System.String"));
            return dt;
        }

        #endregion
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

        #endregion
    }
}