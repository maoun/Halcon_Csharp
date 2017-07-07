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
// <copyright file="CarIllegalQuery.aspx.cs" company="ZKLT">
//     Copyright (c) ZKLT. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// The Map namespace.
/// </summary>
namespace MyNet.Atmcs.Uscmcp.Web.Map
{
    /// <summary>
    /// Class CarIllegalQuery.
    /// </summary>
    public partial class CarIllegalQuery : System.Web.UI.Page
    {
        #region 变量

        /// <summary>
        /// The BLL
        /// </summary>
        private MyNet.Atmcs.Uscmcp.Bll.MapManager bll = new Bll.MapManager();

        /// <summary>
        /// The user login
        /// </summary>
        private UserLogin userLogin = new UserLogin();

        /// <summary>
        /// 查询结果数据集
        /// </summary>
        private static DataSet dsquery = new DataSet();

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
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); 
                return; 
            }
            if (!X.IsAjaxRequest)
            {
                try
                {
                    CmbYear.Show();
                    CmbDay.Show();
                    CmbMonth.Show();
                    AddYear();
                    AddMonth();
                    AddWeek();
                    AddDay();
                    CmbYear.SelectedItem.Value = DateTime.Now.Year.ToString();
                    CmbMonth.SelectedItem.Value = DateTime.Now.Month.ToString();
                    CmbDay.SelectedItem.Value = DateTime.Now.Day.ToString();
                    CmbCountType.SelectedIndex = 0;
                    //SelectRow("116.386522", "37.466293", "");
                    this.Department.DataSource = GetRedisData.GetData("t_cfg_department");
                    this.Department.DataBind();
                    cbodepart.SelectedIndex = 0;
                    //DataSet ds = bll.GetDepart();
                    //if (ds != null && ds.Tables[0].Rows.Count > 0)
                    //{
                    //    this.Department.DataSource = ds.Tables[0];
                    //    this.Department.DataBind();
                    //    cbodepart.SelectedIndex = 0;
                    //}
                    this.DataBind();
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：" + Request.QueryString["funcname"], userinfo.NowIp, "0");
                }
                catch
                { }
            }
        }

        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void ButQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                string departid = cbodepart.Value.ToString();
                string zqlx = this.CmbCountType.Value.ToString();
                string rq = "";
                switch (zqlx)
                {
                    case "0":
                        //rq = string.Format("{0:D2}", int.Parse(CmbMonth.SelectedItem.Value));
                        rq = CmbYear.SelectedItem.Value + string.Format("{0:D2}", int.Parse(CmbMonth.SelectedItem.Value)) + string.Format("{0:D2}", int.Parse(CmbDay.SelectedItem.Value));
                        break;

                    case "1":
                        rq = CmbYear.SelectedItem.Value + string.Format("{0:D2}", int.Parse(CmbMonth.SelectedItem.Value));
                        break;

                    case "2":
                        rq = CmbWeek.SelectedItem.Value;
                        break;

                    case "3":
                        rq = CmbYear.SelectedItem.Value;
                        break;
                }
                DataSet ds = bll.GetIllegalAnalyze(departid, zqlx, rq, cbonum.Value.ToString());
                this.ResourceManager1.RegisterAfterClientInitScript(" BMAP.ClearCircle();BMAP.ClearTempLine();BMAP.Clear();");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    StoreInfo.DataSource = ds.Tables[0];
                    StoreInfo.DataBind();
                    addstation(ds);
                    dsquery = ds;
                }
                else
                {
                    dsquery = null;
                    StoreInfo.RemoveAll();
                    StoreInfo.DataBind();
                    Notice("提示", "无违法信息！");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CarIllegalQuery.aspx-ButQueryClick", ex.Message+"；"+ex.StackTrace, "ButQueryClick has an exception");
            }
        }

        /// <summary>
        /// 列表单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void Cell_Click(object sender, DirectEventArgs e)
        {
            CellSelectionModel sm = this.GridRoadManager.SelectionModel.Primary as CellSelectionModel;
            //this.Label1.Html = string.Format("RecordID: {0}<br />Name: {1}<br />Value: {2}<br />Row: {3}<br />Column: {4}", sm.SelectedCell.RecordID, sm.SelectedCell.Name, sm.SelectedCell.Value, sm.SelectedCell.RowIndex.ToString(), sm.SelectedCell.ColIndex.ToString());
            string HTML = "<div class=\"car-location OverCar-Location\" style=\"top:150px; left:400px;\"> " +
       "<div class=\"items w-220px\">" +
          "<b class=\"tips-arrow\"></b>" +
            "<span class=\"car-brand text-center\">106国道400米卡口（疑似）</span>" +
            "<section class=\"car-img write-bg\">" +
              " <div class=\"CarIllegal-list\">" +
                   "<ul class=\"data-list-h clearfix \">" +
                     "<li class=\"w-1 font-10 fb\">违法行为</li>" +
                     "<li class=\"w-2 font-10 fb\">数量</li>" +
                     "<li class=\"w-2 font-10 fb\">正常</li>" +
                     "<li class=\"w-2 font-10 fb\">比率</li>" +
                   "</ul>" +
                   "<ul class=\"OverCar-data-list clearfix\">" +
                    "  <li>" +
                     "      <span class=\"w-1 text-center font-10\">超车</span>" +
                      "     <span class=\"w-2 font-10\">1000</span>" +
                       "    <span class=\"w-2 font-10\">400</span>" +
                        "   <span class=\"w-2 font-10\">220%</span>" +
                      "</li>" +
                      "<li>" +
                          " <span class=\"w-1 text-center font-10\">超车</span>" +
                         "  <span class=\"w-2 font-10\">1000</span>" +
                        "   <span class=\"w-2 font-10\">400</span>" +
                       "    <span class=\"w-2 font-10\">220%</span>" +
                      "</li>" +
                      "<li>" +
                          " <span class=\"w-1 text-center font-10\">超车</span>" +
                         "  <span class=\"w-2 font-10\">1000</span>" +
                        "   <span class=\"w-2 font-10\">400</span>" +
                       "    <span class=\"w-2 font-10\">220%</span>" +
                      "</li>" +
                      "<li>" +
                          " <span class=\"w-1 text-center font-10\">超车</span>" +
                         "  <span class=\"w-2 font-10\">1000</span>" +
                        "   <span class=\"w-2 font-10\">400</span>" +
                       "    <span class=\"w-2 font-10\">220%</span>" +
                      "</li>" +
                      "<li>" +
                          " <span class=\"w-1 text-center font-10\">超车</span>" +
                         "  <span class=\"w-2 font-10\">1000</span>" +
                        "   <span class=\"w-2 font-10\">400</span>" +
                       "    <span class=\"w-2 font-10\">220%</span>" +
                      "</li>" +
                      "<li>" +
                     "      <span class=\"w-1 text-center font-10\">超车</span>" +
                    "       <span class=\"w-2 font-10\">1000</span>" +
                   "        <span class=\"w-2 font-10\">400</span>" +
                  "         <span class=\"w-2 font-10\">220%</span>" +
                 "     </li>" +
                "      <li>" +
               "            <span class=\"w-1 text-center font-10\">超车</span>" +
              "             <span class=\"w-2 font-10\">1000</span>" +
             "              <span class=\"w-2 font-10\">400</span>" +
            "               <span class=\"w-2 font-10\">220%</span>" +
           "           </li>" +

          "          </ul>" +

        "    </div>" +
         "   </section>" +
       " </div>" +
      "</div>";
            Notice("", HTML);
        }

        /// <summary>
        /// 选择行事件（出发监测点气泡）
        /// </summary>
        /// <param name="xpoint"></param>
        /// <param name="ypoint"></param>
        /// <param name="stationid"></param>
        /// <param name="stationname"></param>
        /// <returns></returns>
        [DirectMethod]
        public void SelectRow(string xpoint, string ypoint, string stationid, string stationname)
        {
            try
            {
                string zqlx = this.CmbCountType.Value.ToString();
                string rq = "";
                switch (zqlx)
                {
                    case "0":
                        //rq = string.Format("{0:D2}", int.Parse(CmbMonth.SelectedItem.Value));
                        rq = CmbYear.SelectedItem.Value + string.Format("{0:D2}", int.Parse(CmbMonth.SelectedItem.Value)) + string.Format("{0:D2}", int.Parse(CmbDay.SelectedItem.Value));
                        break;

                    case "1":
                        rq = CmbYear.SelectedItem.Value + string.Format("{0:D2}", int.Parse(CmbMonth.SelectedItem.Value));
                        break;

                    case "2":
                        rq = CmbWeek.SelectedItem.Value;
                        break;

                    case "3":
                        rq = CmbYear.SelectedItem.Value;
                        break;
                }
                DataTable dt = bll.GetIllegalDetail(zqlx, rq, stationid);
                string HTML = "";
                string head = "<div class=\"car-location OverCar-Location \">" +
         "<div class=\"items w-220px\">" +
                    //"<b class=\"tips-arrow\"></b>" +
              "<span  class=\"car-brand text-center\" >" + stationname +
              "</span>" +
              "<section class=\"car-img write-bg\">" +
               " <div class=\"CarIllegal-list\">" +
                     "<ul class=\"data-list-h clearfix \">" +
                       "<li class=\"w-1 font-10 fb w-150px\">违法行为</li>" +
                       "<li class=\"w-2 font-10 fb w-20px\">数量</li>" +
                    //"<li class=\"w-2 font-10 fb\">正常</li>" +
                    //"<li class=\"w-2 font-10 fb\">比率</li>" +
                     "</ul>" +
                     "<ul class=\"OverCar-data-list clearfix\">";
                string content = "";
                string end = "</ul></div></section></div></div>";
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        content += "  <li>" +
                       "      <span class=\"w-1 text-center font-10 w-150px\">" + dr["wfxwname"] + "</span>" +
                        "     <span class=\"w-2 font-10 w-20px\">" + dr["wfzs"] + "</span>" +
                            //"    <span class=\"w-2 font-10\">" + dr["zs"] + "</span>" +
                            // "   <span class=\"w-2 font-10\">" + dr["wfbl"] + "</span>" +
                        "</li>";
                    }
                    HTML = head + content + end;
                }
                else
                {
                    return;
                }

                string js = "BMAP.openWindow('" + HTML + "','" + xpoint + "','" + ypoint + "');";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
                RowSelectionModel sm = this.GridRoadManager.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRows.Clear();
                sm.UpdateSelection();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CarIllegalQuery.aspx-SelectRow", ex.Message+"；"+ex.StackTrace, "SelectRow has an exception");
            }
        }

        /// <summary>
        /// 日期刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void StoreDayRefresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                List<object> data = new List<object>();

                if (CmbYear.SelectedItem.Value == "")
                {
                    CmbYear.SelectedItem.Value = DateTime.Now.Year.ToString();
                }
                int day = DateTime.Parse(CmbYear.SelectedItem.Value + "-" + CmbMonth.SelectedItem.Value + "-01").AddMonths(1).AddDays(-1).Day;
                for (int i = 1; i <= day; i++)
                {
                    string id = i.ToString();
                    string name = i.ToString() + "日";

                    data.Add(new { col0 = id, col1 = name });
                }
                this.StoreDay.DataSource = data;
                this.StoreDay.DataBind();
                this.CmbDay.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CarIllegalQuery.aspx-SelectRow", ex.Message+"；"+ex.StackTrace, "SelectRow has an exception");
            }
        }

        /// <summary>
        /// 分析类型触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void CmbCountType_Select(object sender, DirectEventArgs e)
        {
            switch (CmbCountType.SelectedItem.Value)
            {
                case "3"://年
                    CmbYear.Show();
                    CmbDay.Hide();
                    CmbMonth.Hide();
                    CmbWeek.Hide();

                    break;

                case "2"://周
                    CmbYear.Hide();
                    CmbDay.Hide();
                    CmbMonth.Hide();
                    CmbWeek.Show();
                    //CmbWeek.SelectedIndex = 0;
                    break;

                case "1"://月
                    CmbYear.Show();
                    CmbMonth.Show();
                    CmbDay.Hide();
                    CmbWeek.Hide();
                    break;

                case "0"://日
                    CmbYear.Show();
                    CmbDay.Show();
                    CmbMonth.Show();
                    CmbWeek.Hide();

                    break;
            }
        }

        #endregion 事件

        #region 方法

        /// <summary>
        /// 增加监测点
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private void addstation(System.Data.DataSet ds)
        {
            try
            {
                string points = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (points != "")
                        points += ",";
                    try
                    {
                        points += "{\"lng\":" + ds.Tables[0].Rows[i]["xpoint"].ToString() + ", \"lat\":"
                       + ds.Tables[0].Rows[i]["ypoint"].ToString() + ", \"count\":" + ds.Tables[0].Rows[i]["wfzs"].ToString() + "}";
                        //"[    { \"lng\": 116.418261, \"lat\": 39.921984, \"count\": 50 },    { \"lng\": 116.423332, \"lat\": 39.916532, \"count\": 51 }]";
                    }
                    catch
                    { }
                }
                if (points != "")
                {
                    string js = "BMAP.OpenHeatmap([" + points + "]);";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CarIllegalQuery.aspx-addstation", ex.Message+"；"+ex.StackTrace, "addstation has an exception");
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
        /// 增加年list
        /// </summary>
        /// <returns></returns>
        private void AddYear()
        {
            try
            {
                List<object> data = new List<object>();
                for (int i = -5; i < 1; i++)
                {
                    string id = DateTime.Now.AddYears(i).ToString("yyyy");
                    string name = DateTime.Now.AddYears(i).ToString("yyyy") + "年"; ;
                    data.Add(new { col0 = id, col1 = name });
                }

                this.StoreYear.DataSource = data;
                this.StoreYear.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CarIllegalQuery.aspx-AddYear", ex.Message+"；"+ex.StackTrace, "AddYear has an exception");
            }
        }

        /// <summary>
        /// 增加月list
        /// </summary>
        /// <returns></returns>
        private void AddMonth()
        {
            try
            {
                List<object> data = new List<object>();

                for (int i = 1; i < 13; i++)
                {
                    string id = i.ToString();
                    string name = i.ToString() + "月";

                    data.Add(new { col0 = id, col1 = name });
                }
                this.StoreMonth.DataSource = data;
                this.StoreMonth.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CarIllegalQuery.aspx-AddMonth", ex.Message+"；"+ex.StackTrace, "AddMonth has an exception");
            }
        }

        /// <summary>
        /// 增加周list
        /// </summary>
        /// <returns></returns>
        private void AddWeek()
        {
            try
            {
                List<object> data = new List<object>();

                for (int i = 1; i < 52; i++)
                {
                    string id = i.ToString();
                    string name = i.ToString() + "周";

                    data.Add(new { col0 = id, col1 = name });
                }

                this.StoreWeek.DataSource = data;
                this.StoreWeek.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CarIllegalQuery.aspx-AddWeek", ex.Message+"；"+ex.StackTrace, "AddWeek has an exception");
            }
        }

        /// <summary>
        /// 增加日list
        /// </summary>
        /// <returns></returns>
        private void AddDay()
        {
            try
            {
                List<object> data = new List<object>();
                int day = DateTime.Now.Day;
                for (int i = 1; i <= day; i++)
                {
                    string id = i.ToString();
                    string name = i.ToString() + "日";
                    data.Add(new { col0 = id, col1 = name });
                }

                this.StoreDay.DataSource = data;
                this.StoreDay.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CarIllegalQuery.aspx-AddDay", ex.Message+"；"+ex.StackTrace, "AddDay has an exception");
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