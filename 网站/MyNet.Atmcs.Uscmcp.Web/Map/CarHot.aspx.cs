// ***********************************************************************
// Assembly         : MyNet.Atmcs.Uscmcp.Web
// Author           : zlsyl
// Created          : 07-26-2016
//
// Last Modified By : zlsyl
// Last Modified On : 11-09-2016
// ***********************************************************************
// <copyright file="CarHot.aspx.cs" company="ZKLT">
//     Copyright (c) ZKLT. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;

/// <summary>
/// The Map namespace.
/// </summary>
namespace MyNet.Atmcs.Uscmcp.Web.Map
{
    /// <summary>
    /// Class CarHot.
    /// </summary>
    public partial class CarHot : System.Web.UI.Page
    {
        #region 成员变量

        /// <summary>
        /// The BLL
        /// </summary>
        private MapManager bll = new MapManager();

        /// <summary>
        /// 监测点数据集
        /// </summary>
        private static System.Data.DataTable dsstation = new DataTable();

        /// <summary>
        /// 道路数据集
        /// </summary>
        private static System.Data.DataTable dsroad = new DataTable();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        /// <summary>
        /// The user login
        /// </summary>
        private UserLogin userLogin = new UserLogin();

        /// <summary>
        /// The timestation
        /// </summary>
        private string timestation = DateTime.Now.ToString("yyyy-MM-dd");

        private string timeroad = DateTime.Now.ToString("yyyy-MM-dd");


        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
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
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                hidSelectDate.Value = date;
                InitStation(date);
                ShowStation();
                InitRoad(date);
                this.DataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "访问：" + Request.QueryString["funcname"], userinfo.NowIp, "0");
            }
        }
        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TabChange_event(object sender, EventArgs e)
        {
            if (TabPanel1.ActiveTabIndex == 0)
                ShowStation();
            else
                ShowRoad();
        }

        [DirectMethod]
        public void btnchange(string datetime)
        {

            try
            {
            datetime = DateTime.Parse(datetime).ToString("yyyy-MM-dd");
            hidSelectDate.Value = datetime;
            InitStation(datetime);
            InitRoad(datetime);
            if (TabPanel1.ActiveTabIndex == 0)
            {
                ShowStation();
            }
            else
            {
                ShowRoad();
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-btnchange", ex.Message+"；"+ex.StackTrace, "btnchange has an exception");
            }
        }

        #endregion 控件事件

        #region DirectMethod

        /// <summary>
        /// 地图展示监测点
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void ShowStation()
        {

            try
            {
            string points = "";
            string js = "BMAP.ClearLine();;";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
            if (dsstation != null)
            {
                for (int row = 0; row < dsstation.Rows.Count; row++)
                {
                    if (dsstation.Rows[row]["xpoint"].ToString() != null && dsstation.Rows[row]["xpoint"].ToString() != "")
                    {
                        if (points != "")
                            points += ",";
                        try
                        {
                            points += "{\"lng\":" + dsstation.Rows[row]["xpoint"].ToString() + ", \"lat\":"
                           + dsstation.Rows[row]["ypoint"].ToString() + ", \"count\":" + dsstation.Rows[row]["zs"].ToString() + "}";
                        }
                        catch
                        { }
                    }
                }
                if (points != "")
                {
                    js = "BMAP.OpenHeatmap([" + points + "]);";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            }
            catch (Exception ex)
            {

                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-ShowStation", ex.Message+"；"+ex.StackTrace, "ShowStation has an exception");
            }
        }

        /// <summary>
        /// 地图展示道路
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void ShowRoad()
        {
            try
            {
            string js = "";
            string color = "";
            js = "BMAP.Clear();BMAP.CloseHeatmap();;";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
            if (dsroad != null)
            {
                float gwbl = 0;
                for (int i = 0; i < dsroad.Rows.Count; i++)
                {
                    if (dsroad.Rows[i]["gwbl"].ToString() == "")
                        gwbl = 0;
                    else
                        gwbl = float.Parse(dsroad.Rows[i]["gwbl"].ToString());
                    if (gwbl < 60)
                        color = "#ffff00";
                    if (gwbl < 30)
                        color = "#00ff00";
                    if (gwbl >= 60)
                        color = "#ff0000";
                    js = "BMAP.addPolyline2('" + color + "','" + bll.GetRoadPoint(dsroad.Rows[i]["dlid"].ToString()) + "','" + dsroad.Rows[i]["dlmc"].ToString() + "');";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-ShowRoad", ex.Message+"；"+ex.StackTrace, "ShowRoad has an exception");
            }
        }

        /// <summary>
        /// Selects the row station.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="kkid"></param>
        /// <param name="kkmc"></param>
        /// <returns></returns>
        [DirectMethod]
        public void SelectRowStation(string x, string y, string kkid, string kkmc)
        {
            try
            {
                timestation = hidSelectDate.Value.ToString();
                string js = "var _id ;var _type; _type='STATION'; _id='" + kkid + "';BMAP.addmarkerstation(false,'../Map/img/COM.gif','" + x + "','" + y + "','" + kkmc + "','" + timestation + "',{ id: _id, type: _type });";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-SelectRowStation", ex.Message+"；"+ex.StackTrace, "SelectRowStation has an exception");
            }
        }

        /// <summary>
        /// Selects the row road.
        /// </summary>
        /// <param name="dlid"></param>
        /// <param name="dlmc"></param>
        /// <returns></returns>
        [DirectMethod]
        public void SelectRowRoad(string dlid, string dlmc)
        {
            try
            {
                string x = "", y = "";
                string xy = bll.GetRoadPoint(dlid);
                if (xy.Split('|').Length > 0)
                {
                    x = xy.Split('|')[0].Split(',')[0];
                    y = xy.Split('|')[0].Split(',')[1];
                    timestation = hidSelectDate.Value.ToString();
                    string js = "var _id ;var _type; _type='ROAD'; _id='" + dlid + "';BMAP.addmarkerstation(false,'../Map/img/COM.gif','" + x + "','" + y + "','" + dlmc + "','" + timestation + "',{ id: _id, type: _type });";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-SelectRowRoad", ex.Message+"；"+ex.StackTrace, "SelectRowRoad has an exception");
            }
        }

        /// <summary>
        /// Btnyestodays this instance.
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void btnyestoday()
        {
            try
            {
            string selectDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            hidSelectDate.Value = selectDate;
            InitStation(selectDate);
            InitRoad(selectDate);
            if (TabPanel1.ActiveTabIndex == 0)
                ShowStation();
            else
                ShowRoad();

            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-btnyestoday", ex.Message+"；"+ex.StackTrace, "btnyestoday has an exception");
            }

        }

        /// <summary>
        /// Btntodays this instance.
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void btntoday()
        {
            try
            {

           

            string selectDate = DateTime.Now.ToString("yyyy-MM-dd");
            hidSelectDate.Value = selectDate;
            InitStation(selectDate);
            InitRoad(selectDate);
            if (TabPanel1.ActiveTabIndex == 0)
            {
                ShowStation();
            }
            else
            {
                ShowRoad();
            }

            }
            catch (Exception ex)
            {

                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-btntoday", ex.Message+"；"+ex.StackTrace, "btntoday has an exception");
            }
        }
        #endregion DirectMethod

        #region 私有方法

        /// <summary>
        /// 初始化道路信息
        /// </summary>
        /// <returns></returns>
        private void InitRoad()
        {
            string head = "<div class=\"map-up-r-container\"><ul class=\"progress-bar\">";
            string end = "</ul></div>";
            string content = "";
            string color = "green-bg";
            dsroad = GetRedisData.GetData("PassCarCount:hotspot_keyroad");//bll.GetCarHotRoad();
            if (dsroad != null && dsroad.Rows.Count > 0)
            {
                try
                {
                    float gwbl;
                    for (int row = 0; row < dsroad.Rows.Count; row++)
                    {
                        gwbl = float.Parse(dsroad.Rows[row]["gwbl"].ToString());
                        if (gwbl < 30)
                        {
                            color = "green-bg";
                        }
                        if (gwbl > 30)
                        {
                            color = "yellow-bg";
                        }

                        if (gwbl > 70)
                        {
                            color = "orange_bg";
                        }
                        content += "<li class=\"display-table row\">" +
                                 " <span class=\"table-cell small-6 pro-ad\">" + row.ToString() + "." + dsroad.Rows[row]["dlmc"].ToString() + "</span>" +
                                 "<div class=\"table-cell small-4 pro-bar\">" +
                                 " <span class=\"pro-bg\">" +
                                 "<i class=\"pro-step " + color + "\" style=\"width:" + gwbl.ToString() + "%;\"></i>" +
                                 "</span>" + " </div>" +
                                 "<span class=\"table-cell small-2 pro-persent text-right\">" + gwbl.ToString() + "%</span>" +
                                 "</li>";
                    }
                    Tab2.Html = head + content + end;
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex);
                    logManager.InsertLogError("LogRunning.aspx-InitRoad", ex.Message+"；"+ex.StackTrace, "InitRoad has an exception");
                }
            }
        }

        /// <summary>
        /// 初始化监测点
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        private void InitStation(string datetime)
        {
            try
            {
                if (datetime == System.DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    dsstation = GetRedisData.GetData("PassCarCount:hotspot_station");//dataCountInfo.GetPassCarHotSpot(datetime, "ZDDL");
                }
                else if (datetime == System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"))
                {
                    dsstation = GetRedisData.GetData("PassCarCount:hotspot_station_last");
                }
                else
                {
                    dsstation = bll.GetCarHotStation(datetime);
                }
                if (dsstation != null)
                {
                    StoreStation.DataSource = dsstation; ;
                    StoreStation.DataBind();
                    this.ResourceManager1.RegisterAfterClientInitScript("isScroll();");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-InitStation", ex.Message+"；"+ex.StackTrace, "InitStation has an exception");
            }
        }

        /// <summary>
        /// 初始化监测点
        /// </summary>
        /// <returns></returns>
        private void InitStation()
        {
            try
            {

            string head = "<div class=\"map-up-r-container\"><ul class=\"progress-bar\">";
            string end = "</ul></div>";
            string content = "";
            string color = "green-bg";
            dsstation = GetRedisData.GetData("PassCarCount:hotspot_station");// bll.GetCarHotStation();
            if (dsstation != null && dsstation.Rows.Count > 0)
            {
                float gwbl = 0;
                for (int row = 0; row < dsstation.Rows.Count; row++)
                {
                    if (dsstation.Rows[row]["gwbl"].ToString() == "")
                        gwbl = 0;
                    else
                        gwbl = float.Parse(dsstation.Rows[row]["gwbl"].ToString());
                    if (gwbl < 30)
                    {
                        color = "green-bg";
                    }
                    if (gwbl > 30)
                    {
                        color = "yellow-bg";
                    }

                    if (gwbl > 70)
                    {
                        color = "orange_bg";
                    }
                    content += "<li class=\"display-table row\">" +
                             " <span class=\"table-cell small-6 pro-ad\">" + row.ToString() + "." + dsstation.Rows[row]["kkmc"].ToString() + "</span>" +
                             "<div class=\"table-cell small-4 pro-bar\">" +
                             " <span class=\"pro-bg\">" +
                             "<i class=\"pro-step " + color + "\" style=\"width:" + dsstation.Rows[row]["gwbl"].ToString() + "%;\"></i>" +
                             "</span>" + " </div>" +
                             "<span class=\"table-cell small-2 pro-persent text-right\">" + dsstation.Rows[row]["gwbl"].ToString() + "%</span>" +
                             "</li>";
                }
                Tab1.Html = head + content + end;
            }

            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-InitStation", ex.Message+"；"+ex.StackTrace, "InitStation has an exception");
            }

        }

        /// <summary>
        /// 初始化道路
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        private void InitRoad(string datetime)
        {
            try
            {
                if (datetime == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    dsroad = GetRedisData.GetData("PassCarCount:hotspot_keyroad");//dataCountInfo.GetPassCarHotSpot(datetime, "ZDDL");
                }
                if (datetime == DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"))
                {
                    dsroad = GetRedisData.GetData("PassCarCount:hotspot_keyroad_last");
                }
                else
                {
                    dsroad = bll.GetCarHotRoad(datetime);
                }
                //GetRedisData.GetData("PassCarCount:hotspot_keyroad");//dataCountInfo.GetPassCarHotSpot(datetime, "ZDDL");
                if (dsroad != null)
                {
                    StoreRoad.DataSource = dsroad; ;
                    StoreRoad.DataBind();
                    this.ResourceManager1.RegisterAfterClientInitScript("isScroll();");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-InitRoad", ex.Message+"；"+ex.StackTrace, "InitRoad has an exception");
            }
        }

        #endregion 私有方法

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