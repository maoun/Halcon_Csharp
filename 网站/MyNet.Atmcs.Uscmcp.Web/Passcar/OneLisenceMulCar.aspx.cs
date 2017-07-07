// ***********************************************************************
// Assembly         : MyNet.Atmcs.Uscmcp.Web
// Author           : zlsyl
// Created          : 11-18-2016
//
// Last Modified By : zlsyl
// Last Modified On : 11-18-2016
// ***********************************************************************
// <copyright file="OneLisenceMulCar.aspx.cs" company="ZKLT">
//     Copyright (c) ZKLT. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;

/// <summary>
/// The Passcar namespace.
/// </summary>
namespace MyNet.Atmcs.Uscmcp.Web.Passcar
{
    /// <summary>
    /// Class OneLisenceMulCar.
    /// </summary>
    public partial class OneLisenceMulCar : System.Web.UI.Page
    {
        #region 变量

        /// <summary>
        /// The TGS data information
        /// </summary>
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();

        /// <summary>
        /// The setting manager
        /// </summary>
        private SettingManager settingManager = new SettingManager();

        /// <summary>
        /// The client
        /// </summary>
        private OtherQueryService.OtherQueryInfo client = new OtherQueryService.OtherQueryInfo();

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        /// <summary>
        /// The client querypasscar
        /// </summary>
        private static QueryService.querypasscar clientQuerypasscar = new QueryService.querypasscar();

        /// <summary>
        /// The vehicle
        /// </summary>
        private Vehicle vehicle = null;

        /// <summary>
        /// The URL
        /// </summary>
        private string url = "";

        /// <summary>
        /// The user login
        /// </summary>
        private UserLogin userLogin = new UserLogin();

        /// <summary>
        /// The BKBH identifier
        /// </summary>
        private static string bkbhId = "";

        /// <summary>
        /// 起止时间
        /// </summary>
        private static string startdate, enddate;

        /// <summary>
        /// 套牌数据集
        /// </summary>
        private static DataTable dt_tp = null;

        /// <summary>
        /// 号牌号码,号牌种类
        /// </summary>
        private static string hphm = "", hpzl = "";

        /// <summary>
        /// 页码
        /// </summary>
        private static int page = 0;

        /// <summary>
        /// The condition
        /// </summary>
        private static PassInfo condition = new PassInfo();

        /// <summary>
        /// The vehicle information
        /// </summary>
        private static VehicleInfo vehicleInfo = new VehicleInfo();

        private static int pageSize = 3;

        /// <summary>
        /// The BLL
        /// </summary>
        private Bll.PasscarManager bll = new PasscarManager();

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
            if (!IsPostBack)
            {
                ParameterCollection para = Store1.AutoLoadParams;
                if (Session["Screen"] != null)
                {
                    string screen = Session["Screen"].ToString();

                    if (screen.Equals("1"))
                    {
                        para.GetParameter("limit").Value = "10";
                        pageSize = 10;
                    }
                    else if (screen.Equals("2"))
                    {
                        para.GetParameter("limit").Value = "4";
                        pageSize = 4;
                    }
                    else if (screen.Equals("3"))
                    {
                        para.GetParameter("limit").Value = "3";
                    }
                    else
                    {
                        para.GetParameter("limit").Value = "3";
                    }
                }
                else
                {
                    para.GetParameter("limit").Value = "3";
                }

                string username = Request.QueryString["username"];
                if (!userLogin.CheckLogin(username))
                {
                    string js = "alert('" + GetLangStr("OneLisenceMulCar29", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                    System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                    return;
                }
                curpage1.Value = "1";
                if (!X.IsAjaxRequest)
                {
                    try
                    {
                        if (Session["Condition"] != null)
                        {
                            ButFirst.Disabled = true;
                            ButLast.Disabled = true;
                            Condition con = Session["Condition"] as Condition;
                            start.InnerText = con.StartTime;
                            end.InnerText = con.EndTime;
                            startdate = con.StartTime;
                            enddate = con.EndTime;
                            cboplate.SetVehicleText(con.Sqjc);
                            txtplate.Text = con.Hphm;
                        }
                        else
                        {
                            start.InnerText = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                            end.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            startdate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                            enddate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
                            if (!string.IsNullOrEmpty(Request.QueryString["hphm"]))
                            {
                                hphm = Request.QueryString["hphm"];
                                if (!string.IsNullOrEmpty(hphm))
                                {
                                    cboplate.SetVehicleText(hphm.Substring(0, 1));
                                    txtplate.Text = hphm.Substring(1);
                                }
                            }
                        }
                        ButQueryClick(null, null);
                        UserInfo userinfo = Session["Userinfo"] as UserInfo;
                        logManager.InsertLogRunning(userinfo.UserName, GetLangStr("OneLisenceMulCar30", "访问：车辆套牌"), userinfo.NowIp, "0");
                    }
                    catch (Exception ex)
                    {
                        logManager.InsertLogError("OneLisenceMulCar.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
                        ILog.WriteErrorLog(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void MapShow(object sender, EventArgs e)
        {
            if (Session["Condition"] != null)
            {
                Session["Condition"] = null;
            }
            Session["Condition"] = condition;
            string js = "OpenQueryPage('../Passcar/OneLisenceMulCarMapShow.aspx')";// "OpenQueryPage('../Map/PathCarQuery.aspx')";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }

        /// <summary>
        /// 加入套牌库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void AddTp(object sender, EventArgs e)
        {
            try
            {
                //当value为0:插入到老表t_tgs_info_blacklist中，当不为0：插入到t_tgs_suspect  和 t_tgs_suspect_recive
                string value = System.Configuration.ConfigurationManager.AppSettings["BkType"].ToString();
                if (value.Equals("0"))
                {
                    Hashtable hs = new Hashtable();
                    hs.Add("xh", tgsDataInfo.GetTgsRecordId());
                    hs.Add("hphm", hphm);
                    hs.Add("clpp", txtClpp.Text);
                    hs.Add("hpzl", hpzl);
                    if (vehicleInfo != null)
                        hs.Add("csys", vehicleInfo.Csysbh);
                    else
                        hs.Add("csys", "");
                    hs.Add("mdlx", "300104");
                    UserInfo userinfo = Session["userinfo"] as UserInfo;
                    hs.Add("sjly", userinfo.DeptCode);
                    if (bll.UpdateSuspicionInfo(hs) > 0)
                    {
                        try
                        {
                            clientQuerypasscar.layout(hs["xh"].ToString(), hs["hphm"].ToString(), hs["hpzl"].ToString(), "");
                        }
                        catch
                        {
                        }
                        Notice(GetLangStr("OneLisenceMulCar31", "提示"), GetLangStr("OneLisenceMulCar32", "成功加入套牌库"));
                    }
                    else
                    {
                        Notice(GetLangStr("OneLisenceMulCar31", "提示"), GetLangStr("OneLisenceMulCar33", "加入套牌库失败，请重试！"));
                    }
                }
                else//当value不为0的时候
                {
                    Hashtable hs = new Hashtable();
                    hs.Add("bkbh", tgsDataInfo.GetTgsRecordId());
                    bkbhId = tgsDataInfo.GetTgsRecordId();
                    hs.Add("hphm", hphm);
                    hs.Add("clpp", txtClpp.Text);
                    hs.Add("hpzl", hpzl);
                    if (vehicleInfo != null)
                        hs.Add("csys", vehicleInfo.Csysbh);
                    else
                        hs.Add("csys", "");
                    hs.Add("bklx", "300104");
                    UserInfo userinfo = Session["userinfo"] as UserInfo;
                    hs.Add("bkrdw", userinfo.DeptCode);
                    hs.Add("bkr", userinfo.UserCode);
                    if (bll.UpdateSuspicionInfoNew(hs) > 0)
                    {
                        try
                        {
                            hs.Remove("bkbh");
                            DataTable dt = bll.GetBkbh(hs);
                            if (dt != null && dt.Rows.Count > 0)//说明以加入过套牌车
                            {
                                string bkbh = dt.Rows[0]["bkbh"].ToString();
                                hs.Add("bkbh", bkbh);
                            }
                            else
                            {
                                hs.Add("bkbh", bkbhId);
                            }

                            bll.UpdateSuspicionInfoNewRecive(hs);//插入数据或跟新数据到表t_tgs_suspect_recive
                            clientQuerypasscar.layout(hs["bkbh"].ToString(), hs["hphm"].ToString(), hs["hpzl"].ToString(), "");//把数据插入到接口中去
                        }
                        catch
                        {
                        }
                        Notice(GetLangStr("OneLisenceMulCar31", "提示"), GetLangStr("OneLisenceMulCar32", "成功加入套牌库"));
                    }
                    else
                    {
                        Notice(GetLangStr("OneLisenceMulCar31", "提示"), GetLangStr("OneLisenceMulCar33", "加入套牌库失败，请重试！"));
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneLisenceMulCar.aspx-AddTp", ex.Message + "；" + ex.StackTrace, "AddTp has an exception");
                Notice(GetLangStr("OneLisenceMulCar31", "提示"), GetLangStr("OneLisenceMulCar33", "加入套牌库失败，请重试！"));
            }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void butleft(object sender, EventArgs e)
        {
            page--;
            if (page < 0)
                page = 0;
            // showimg();
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void butright(object sender, EventArgs e)
        {
            page++;
            //showimg();
        }

        /// <summary>
        /// 起止时间
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
        /// 行选择事件
        /// </summary>
        /// <param name="selhphm"></param>
        /// <param name="selhpzl"></param>
        /// <returns></returns>
        [DirectMethod]
        public void SelectRow(string selhphm, string selhpzl)
        {
            hphm = selhphm;
            hpzl = selhpzl;
            if ((string.IsNullOrEmpty(hphm) && string.IsNullOrEmpty(hpzl)) || (!string.IsNullOrEmpty(hphm) && string.IsNullOrEmpty(hpzl)) || (string.IsNullOrEmpty(hphm) && !string.IsNullOrEmpty(hpzl)))
            {
                this.lblCurpage.Text = "1";
                this.lblAllpage.Text = "0";
                this.lblRealcount.Text = "0";
                Notice(GetLangStr("OneLisenceMulCar34", "信息提示"), GetLangStr("OneLisenceMulCar35", "不存在号牌信息，无法获取车驾管信息"));
                this.Store1.DataSource = CreateTable();
                this.Store1.DataBind();
                txtClpp.Text = "";
                txtCsys.Text = "";
                txtCllx.Text = "";
                txtSyxz.Text = "";
                txtClzt.Text = "";
                txtFzjg.Text = "";
                txtSyr.Text = "";
                txtLxdh.Text = "";
                txtYxqz.Text = "";
                txtXxdz.Text = "";
                return;
            }
            else
            {
                page = 0;
                showimg(1);
                InitPage(selhphm, hpzl);
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>

        public void ButQueryClick(object sender, DirectEventArgs e)
        {
            string _hphm;
            if (txtplate.Text != null && txtplate.Text != "")
                _hphm = cboplate.VehicleText + txtplate.Value.ToString();
            else
                _hphm = "";
            int rows = bll.GetOneLisenceMulCarDataRows(startdate, enddate, _hphm);
            curpage.Value = 0;
            if (rows > 0)
            {
                totalpage.Value = rows / 15 + 1;
                Query(0);
            }
            else
            {
                this.Store1.DataSource = CreateTable();
                this.Store1.DataBind();
                StoreInfo.RemoveAll();
                StoreInfo.DataBind();
                this.lblCurpage.Text = "1";
                this.lblAllpage.Text = "0";
                this.lblRealcount.Text = "0";
                Notice(GetLangStr("OneLisenceMulCar31", "提示"), GetLangStr("OneLisenceMulCar36", "无套牌信息！"));
            }
            //try
            //{
            //    string _hphm;
            //    if (txtplate.Text != null && txtplate.Text != "")
            //        _hphm = cboplate.VehicleText + txtplate.Value.ToString();
            //    else
            //        _hphm = "";
            //    DataSet ds = bll.GetOneLisenceMulCarData(startdate, enddate, _hphm);
            //    dt_tp = bll.GetTpcl(startdate, enddate, _hphm);
            //    if (ds != null && ds.Tables[0].Rows.Count > 0)
            //    {
            //        StoreInfo.DataSource = ds.Tables[0];
            //        StoreInfo.DataBind();
            //        page = 0;
            //        hphm = ds.Tables[0].Rows[0]["hphm"].ToString();
            //        hpzl = ds.Tables[0].Rows[0]["hpzl"].ToString();
            //        RowSelectionModel sm = this.GridRoadManager.SelectionModel.Primary as RowSelectionModel;
            //        sm.SelectedRows.Clear();
            //        sm.SelectedRows.Add(new SelectedRow(0));
            //        sm.UpdateSelection();
            //        showimg();
            //        SelectRow(hphm, hpzl);
            //    }
            //    else
            //    {
            //        StoreInfo.RemoveAll();
            //        StoreInfo.DataBind();
            //        Notice("提示", "无套牌信息！");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ILog.WriteErrorLog(ex);
            //}
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
                Query(0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneLisenceMulCar.aspx-TbutFisrt", ex.Message + "；" + ex.StackTrace, "TbutFisrt has an exception");
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
                Query(int.Parse(totalpage.Value.ToString()) - 1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneLisenceMulCar.aspx-TbutEnd", ex.Message + "；" + ex.StackTrace, "TbutEnd has an exception");
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
                curpage.Value = page;
                Query(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneLisenceMulCar.aspx-TbutLast", ex.Message + "；" + ex.StackTrace, "TbutLast has an exception");
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
                page++;
                curpage.Value = page;
                Query(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneLisenceMulCar.aspx-TbutNext", ex.Message + "；" + ex.StackTrace, "TbutNext has an exception");
            }
        }

        /// <summary>
        /// Sets the disbale.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private void SetDisbale(int page)
        {
            if (page <= 0)
            {
                ButFirst.Disabled = true;
                ButLast.Disabled = true;
            }
            else
            {
                ButFirst.Disabled = false;
                ButLast.Disabled = false;
            }
            if (page >= int.Parse(this.totalpage.Value.ToString()) - 1)
            {
                ButEnd.Disabled = true;
                ButNext.Disabled = true;
            }
            else
            {
                ButEnd.Disabled = false;
                ButNext.Disabled = false;
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private void Query(int page)
        {
            try
            {
                SetDisbale(page);
                GridRoadManager.Title = GetLangStr("OneLisenceMulCar37", "当前") + (page + 1).ToString() + GetLangStr("OneLisenceMulCar38", "页,共") + totalpage.Value.ToString() + GetLangStr("OneLisenceMulCar39", "页");
                string _hphm;
                if (txtplate.Text != null && txtplate.Text != "")
                    _hphm = cboplate.VehicleText + txtplate.Value.ToString();
                else
                    _hphm = "";
                DataSet ds = bll.GetOneLisenceMulCarData(startdate, enddate, _hphm, page * 15, 15);
                dt_tp = bll.GetTpcl(startdate, enddate, _hphm);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    StoreInfo.DataSource = ds.Tables[0];
                    StoreInfo.DataBind();
                    page = 0;
                    hphm = ds.Tables[0].Rows[0]["hphm"].ToString();
                    hpzl = ds.Tables[0].Rows[0]["hpzl"].ToString();
                    RowSelectionModel sm = this.GridRoadManager.SelectionModel.Primary as RowSelectionModel;
                    sm.SelectedRows.Clear();
                    sm.SelectedRows.Add(new SelectedRow(0));
                    sm.UpdateSelection();
                    //showimg(1);
                    SelectRow(hphm, hpzl);
                }
                else
                {
                    this.Store1.DataSource = CreateTable();
                    this.Store1.DataBind();
                    StoreInfo.RemoveAll();
                    StoreInfo.DataBind();
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";
                    Notice(GetLangStr("OneLisenceMulCar31", "提示"), GetLangStr("OneLisenceMulCar36", "无套牌信息！"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneLisenceMulCar.aspx-Query", ex.Message + "；" + ex.StackTrace, "Query has an exception");
            }
        }

        public DataTable CreateTable()
        {
            DataTable dt = CreatePasscarTable();
            if (Session["Screen"] != null)
            {
                string screen = Session["Screen"].ToString();
                if (screen.Equals("1"))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["zjwj1"] = "../images/NoImage.png";
                        dt.Rows.Add(dr);
                    }
                }
                else if (screen.Equals("2"))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["zjwj1"] = "../images/NoImage.png";
                        dt.Rows.Add(dr);
                    }
                }
                else if (screen.Equals("3"))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["zjwj1"] = "../images/NoImage.png";
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["zjwj1"] = "../images/NoImage.png";
                        dt.Rows.Add(dr);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["zjwj1"] = "../images/NoImage.png";
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        /// <summary>
        /// 创建内存表
        /// </summary>
        /// <returns></returns>
        private DataTable CreatePasscarTable()
        {
            DataTable dt = new DataTable("PassCar");

            dt.Columns.Add("zjwj1", typeof(string));
            dt.Columns.Add("hphm", typeof(string));
            dt.Columns.Add("hpzlname", typeof(string));
            dt.Columns.Add("kkidname1", typeof(string));
            dt.Columns.Add("gwsj1", typeof(string));

            return dt;
        }

        #endregion 事件

        #region 方法

        /// <summary>
        /// 测量两点间距离
        /// </summary>
        /// <param name="X1"></param>
        /// <param name="Y1"></param>
        /// <param name="X2"></param>
        /// <param name="Y2"></param>
        /// <returns></returns>
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
            return Math.Round(MilValue, 2);
        }

        /// <summary>
        /// 填充页面
        /// </summary>
        /// <param name="selhphm"></param>
        /// <param name="hpzl"></param>
        /// <returns></returns>
        private void InitPage(string selhphm, string hpzl)
        {
            try
            {
                if (selhphm != "")
                {
                    if (!string.IsNullOrEmpty(selhphm) && !string.IsNullOrEmpty(hpzl))
                    {
                        txtHphm.Text = selhphm;
                        txtHpzl.Text = Bll.Common.GetHpzlms(hpzl);
                        string plateHead = settingManager.GetConfigInfo("00", "06").Rows[0]["col3"].ToString();
                        if (selhphm.Substring(0, 1).Equals(plateHead.Substring(0, 1)))
                        {
                            url = client.Url;
                            vehicle = new Vehicle(url);
                            vehicleInfo = vehicle.GetVehicleInfo(hpzl, selhphm);
                            if (vehicleInfo != null)
                            {
                                txtClpp.Text = vehicleInfo.Clpp1;
                                txtCsys.Text = vehicleInfo.Csys;
                                txtCllx.Text = vehicleInfo.Cllx;
                                txtSyxz.Text = vehicleInfo.Syxzms;
                                txtClzt.Text = vehicleInfo.Zt;
                                txtFzjg.Text = vehicleInfo.Fzjg;
                                txtSyr.Text = vehicleInfo.Syr;
                                txtLxdh.Text = vehicleInfo.Lxdh;
                                txtYxqz.Text = vehicleInfo.Yxqz;
                                txtXxdz.Text = vehicleInfo.Zsxxdz;
                                if (vehicleInfo.Ztbh.Equals("A"))
                                {
                                    txtClzt.StyleSpec = "color:blue";
                                }
                                else if (vehicleInfo.Ztbh.Equals("B"))
                                {
                                }
                                else
                                {
                                    txtClzt.StyleSpec = "color:red";
                                }
                            }
                            else
                            {
                                txtClpp.Text = "";
                                txtCsys.Text = "";
                                txtCllx.Text = "";
                                txtSyxz.Text = "";
                                txtClzt.Text = "";
                                txtFzjg.Text = "";
                                txtSyr.Text = "";
                                txtLxdh.Text = "";
                                txtYxqz.Text = "";
                                txtXxdz.Text = "";
                            }
                        }
                        else
                        {
                            Notice(GetLangStr("OneLisenceMulCar29", GetLangStr("OneLisenceMulCar34", "信息提示")), GetLangStr("OneLisenceMulCar40", "该车牌不是本省车牌，无法获取车驾管信息"));
                        }
                        panelChart.AutoLoad.Url = "../Template/WebComplexPieData.aspx?hphm=" + hphm + "&hpzl=" + hpzl;
                        panelChart.Reload();
                    }
                    else
                    {
                        this.lblCurpage.Text = "1";
                        this.lblAllpage.Text = "0";
                        this.lblRealcount.Text = "0";
                        Notice(GetLangStr("OneLisenceMulCar34", "信息提示"), GetLangStr("OneLisenceMulCar35", "不存在号牌信息，无法获取车驾管信息"));
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("OneLisenceMulCar.aspx-InitPage", ex.Message + "；" + ex.StackTrace, "InitPage has an exception");
            }
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
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

        public DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        /// 展示图片
        /// </summary>
        /// <returns></returns>
        private void showimg(int currentPage)
        {
            try
            {
                if (dt_tp != null)
                {
                    DataRow[] drlist = dt_tp.Select("hphm='" + hphm + "'");
                    DataTable dt = ToDataTable(drlist);
                    realCountImg.Value = dt.Rows.Count;//总数
                    allPage.Value = Math.Ceiling((double)dt.Rows.Count / (double)pageSize);//总页数
                    DataTable dtFenye = new DataTable();
                    dtFenye = dt.Clone();//复制表结构
                    if (currentPage == 1)//第一页
                    {
                        if (currentPage < Convert.ToInt32(allPage.Value))//大于一页
                        {
                            for (int i = (currentPage - 1) * pageSize; i < currentPage * pageSize; i++)  //0 10,10 20,20 30
                            {
                                dtFenye.ImportRow(dt.Rows[i]);
                            }
                        }
                        else//就一页
                        {
                            for (int i = (currentPage - 1) * pageSize; i < dt.Rows.Count; i++)  //0 10,10 20,20 30
                            {
                                dtFenye.ImportRow(dt.Rows[i]);
                            }
                        }
                    }
                    else if (currentPage == Convert.ToInt32(allPage.Value))//最后一页
                    {
                        for (int i = (currentPage - 1) * pageSize; i < dt.Rows.Count; i++)  //0 10,10 20,20 30
                        {
                            dtFenye.ImportRow(dt.Rows[i]);
                        }
                    }
                    else
                    {
                        for (int i = (currentPage - 1) * pageSize; i < currentPage * pageSize; i++)  //0 10,10 20,20 30
                        {
                            dtFenye.ImportRow(dt.Rows[i]);
                        }
                    }

                    this.lblCurpage.Text = curpage1.Value.ToString();
                    this.lblAllpage.Text = allPage.Value.ToString();
                    this.lblRealcount.Text = realCountImg.Value.ToString();
                    SetImgDisbale(currentPage);
                    this.Store1.DataSource = dtFenye;
                    this.Store1.DataBind();
                    //            DataRow[] drlist = dt_tp.Select("hphm='" + hphm + "'");
                    //            if (drlist != null && drlist.Length > page)
                    //            {
                    //                imgzjwj2.ImageUrl = drlist[0]["zjwj2"].ToString();
                    //                labkkid2.Text = "卡口名称：" + drlist[0]["kkidname2"].ToString();//"卡口名称：海淀区东三街2号楼13层";
                    //                labgwsj2.Text = "通过时间：" + drlist[0]["gwsj2"].ToString(); //"通过时间：2015-12-12 12:32:49";
                    //                labcllx2.Text = "号牌种类：" + drlist[0]["hpzlname"].ToString(); //"车辆类型：小型汽车";
                    //                labhphm2.Text = "号牌号码：" + hphm;
                    //                imgzjwj1.ImageUrl = drlist[0]["zjwj1"].ToString();
                    //                labkkid1.Text = "卡口名称：" + drlist[0]["kkidname1"].ToString();//"卡口名称：海淀区东三街2号楼13层";
                    //                labgwsj1.Text = "通过时间：" + drlist[0]["gwsj1"].ToString(); //"通过时间：2015-12-12 12:32:49";
                    //                labcllx1.Text = "号牌种类：" + drlist[0]["hpzlname"].ToString(); //"车辆类型：小型汽车";
                    //                labhphm1.Text = "号牌号码：" + hphm;
                    //                condition.Hphm1 = hphm;
                    //                condition.Hphm2 = hphm;
                    //                condition.Xpos1 = drlist[0]["xpos1"].ToString();
                    //                condition.Ypos1 = drlist[0]["ypos1"].ToString();
                    //                condition.Xpos2 = drlist[0]["xpos2"].ToString();
                    //                condition.Ypos2 = drlist[0]["ypos2"].ToString();
                    //                condition.Lkmc1 = drlist[0]["kkidname1"].ToString();
                    //                condition.Lkmc2 = drlist[0]["kkidname2"].ToString();
                    //                condition.Len = CalcMil(double.Parse(condition.Xpos1), double.Parse(condition.Ypos1), double.Parse(condition.Xpos2), double.Parse(condition.Ypos2)).ToString();
                    //                condition.Gcsj1 = drlist[0]["gwsj1"].ToString();
                    //                condition.Gcsj2 = drlist[0]["gwsj2"].ToString();
                    //                condition.Zjwj1 = "<div id=\"view\" class=\"car-location path-poup-content\" > "
                    // + "<a href=\"#\" class=\"items w-220px\">"
                    //      + "<span class=\"car-brand\">" + condition.Lkmc1 + condition.Gcsj1 + "</span>"
                    //      + "<span class=\"car-img path-poup-img\"><img src=\"" + drlist[0]["zjwj1"].ToString() + "\"  onclick=\"zoom(this,false);\" ondblclick=\"OpenPicPage(this.src);\" class=\" move-scale-b\"></span>"
                    //  + "</a>"
                    //+ "</div>";
                    //                condition.Zjwj2 = "<div id=\"view\" class=\"car-location path-poup-content\" > "
                    // + "<a href=\"#\" class=\"items w-220px\">"
                    //      + "<span class=\"car-brand\">" + condition.Lkmc2 + condition.Gcsj2 + "</span>"
                    //      + "<span class=\"car-img path-poup-img\"><img src=\"" + drlist[0]["zjwj2"].ToString() + "\"  onclick=\"zoom(this,false);\" ondblclick=\"OpenPicPage(this.src);\" class=\" move-scale-b\"></span>"
                    //  + "</a>"
                    //+ "</div>";
                    //            }
                    //            else
                    //            {
                    //            }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("OneLisenceMulCar.aspx-showimg", ex.Message + "；" + ex.StackTrace, "showimg has an exception");
            }
        }

        /// <summary>
        /// 上一页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutLast1(object sender, DirectEventArgs e)
        {
            try
            {
                int page = int.Parse(curpage1.Value.ToString());
                page--;
                if (page < 1)
                {
                    page = 1;
                }
                curpage1.Value = page;
                showimg(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoQuery.aspx-TbutLast", ex.Message + "；" + ex.StackTrace, "TbutLast has an exception");
            }
        }

        /// <summary>
        ///下一页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutNext1(object sender, DirectEventArgs e)
        {
            try
            {
                int page = int.Parse(curpage1.Value.ToString());
                int allpage = int.Parse(allPage.Value.ToString());
                page++;
                if (page > allpage)
                {
                    page = allpage;
                }
                curpage1.Value = page;
                showimg(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoQuery.aspx-TbutNext", ex.Message + "；" + ex.StackTrace, "TbutNext has an exception");
            }
        }

        /// <summary>
        /// Sets the disbale.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private void SetImgDisbale(int page)
        {
            if (page == 1)
            {
                if (page < Convert.ToInt32(allPage.Value))
                {
                    Button1.Disabled = true;
                    Button2.Disabled = false;
                }
                else
                {
                    Button1.Disabled = true;
                    Button2.Disabled = true;
                }
            }
            else if (page == Convert.ToInt32(allPage.Value))
            {
                if (Convert.ToInt32(allPage.Value) == 1)
                {
                    Button1.Disabled = true;
                    Button2.Disabled = true;
                }
                else
                {
                    Button1.Disabled = false;
                    Button2.Disabled = true;
                }
            }
            else
            {
                Button1.Disabled = false;
                Button2.Disabled = false;
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