using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Atmcs.Uscmcp.UI;
using MyNet.Common.Log;
using System;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PeccancyQuery : System.Web.UI.Page
    {
        #region 成员变量

        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private SettingManager settingManager = new SettingManager();
        private UserLogin userLogin = new UserLogin();
        private DataCommon dataCommon = new DataCommon();
        private const string NoImageUrl = "../images/NoImage.png";
        private static DataTable dtFangxiang = null;
        private static string starttime = "";
        private static string endtime = "";
        private static string maxtime = "";
        private static string mintime = "";

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
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("PeccancyQuery44", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            uiDepartment.DepartmentSelectEvent += new UIDepartment.DepartmentSelectHandler(uiDepartment_DepartmentSelectEvent);
            if (!IsPostBack)
            {
                if (!X.IsAjaxRequest)
                {
                    try
                    {
                        DataSetDateTime();
                        StoreDataBind();
                        if (!string.IsNullOrEmpty(Request.QueryString["hphm"]))
                        {
                            string hphm = Request.QueryString["hphm"];
                            if (!string.IsNullOrEmpty(hphm))
                            {
                                WindowEditor1.SetVehicleText(hphm.Substring(0, 1));
                                TxtplateId.Text = hphm.Substring(1, hphm.Length - 1);
                            }
                        }
                        if (!string.IsNullOrEmpty(Request.QueryString["hpzl"]))
                        {
                            string hpzl = Request.QueryString["hpzl"];
                            CmbPlateType.Value = hpzl;
                        }
                        if (!string.IsNullOrEmpty(Request.QueryString["xsfx"]))
                        {
                            string xsfx = Request.QueryString["xsfx"];
                            CmbDirection.Value = xsfx;
                        }
                        if (!string.IsNullOrEmpty(Request.QueryString["startTime"]))
                        {
                            starttime = Request.QueryString["startTime"];
                            start.InnerText = starttime;
                        }
                        if (!string.IsNullOrEmpty(Request.QueryString["endTime"]))
                        {
                            endtime = Request.QueryString["endTime"];
                            end.InnerText = endtime;
                        }
                        TbutQueryClick(null, null);
                    }
                    catch (Exception ex)
                    {
                        ILog.WriteErrorLog(ex);
                        logManager.InsertLogError("PeccancyQuery.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
                    }
                    this.DataBind();
                }
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("PeccancyQuery45","访问：") + Request.QueryString["funcname"], userinfo.NowIp, "0");
            }
        }

        /// <summary>
        /// 根据选中部门信息关联查询卡口信息
        /// </summary>
        /// <param name="depertId"></param>
        /// <param name="e"></param>
        private void uiDepartment_DepartmentSelectEvent(string depertId, string e)
        {
            try
            {
                DataTable dt = tgsPproperty.GetStationInfo("a.station_type_id in (01,02,03,06,07,08)  and  a.departid='" + uiDepartment.DepertId + "'");
                this.StoreLocation.DataSource = dt;
                this.StoreLocation.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-uiDepartment_DepartmentSelectEvent", ex.Message + "；" + ex.StackTrace, "uiDepartment_DepartmentSelectEvent has an exception");
            }
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                //this.lblTitle.Text = GetLangStr("PeccancyQuery45", "查询结果：当前是第1页，共有0页，共有0条记录");
                if (string.IsNullOrEmpty(starttime) || string.IsNullOrEmpty(endtime))
                {
                    Notice(GetLangStr("PeccancyQuery46", "提示"), GetLangStr("PeccancyQuery47", "请选择要查询的开始时间和结束时间"));
                    return;
                }
                GetData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DataSourceRefresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.uiDepartment.DepertId))
                {
                    if (this.CmbDataSource.Value != null && CmbDataSource.SelectedIndex != -1)
                    {
                        DataTable ddbh = tgsPproperty.GetStationInfo("   b.datasource='" + this.CmbDataSource.Value + "' and b.istmsshow ='1'  and  a.departid='" + uiDepartment.DepertId + "'");
                        this.StoreLocation.DataSource = ddbh;
                        this.StoreLocation.DataBind();
                        Session["location"] = ddbh;
                    }
                    else
                    {
                        DataTable ddbh = GetRedisData.GetData("Station:t_cfg_set_station_type_istmsshow");
                        if (ddbh != null)
                        {
                            this.StoreLocation.DataSource = ChangColName(ddbh);
                            this.StoreLocation.DataBind();
                        }
                        else
                        {
                            DataTable ddbh1 = tgsPproperty.GetStationInfo("b.istmsshow ='1'");
                            this.StoreLocation.DataSource = ddbh1;
                            this.StoreLocation.DataBind();
                        }
                        Session["location"] = null;
                    }
                }
                else
                {
                    if (this.CmbDataSource.Value != null && CmbDataSource.SelectedIndex != -1)
                    {
                        DataTable ddbh = tgsPproperty.GetStationInfo(" b.istmsshow ='1'  and  b.datasource='" + this.CmbDataSource.Value + "'");
                        this.StoreLocation.DataSource = ddbh;
                        this.StoreLocation.DataBind();
                        Session["location"] = ddbh;
                    }
                    else
                    {
                        DataTable ddbh = GetRedisData.GetData("Station:t_cfg_set_station_type_istmsshow");
                        if (ddbh != null)
                        {
                            this.StoreLocation.DataSource = ChangColName(ddbh);
                            this.StoreLocation.DataBind();
                        }
                        else
                        {
                            DataTable ddbh1 = tgsPproperty.GetStationInfo("b.istmsshow ='1'");
                            this.StoreLocation.DataSource = ddbh1;
                            this.StoreLocation.DataBind();
                        }
                        Session["location"] = null;
                    }
                }
                Session["location"] = null;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-DataSourceRefresh", ex.Message + "；" + ex.StackTrace, "DataSourceRefresh has an exception");
            }
        }

        /// <summary>
        /// 地点刷新后 更新方向信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TgsRefresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                string where = string.Empty;
                string kkid = GetMultiCombo(CmbLocation.SelectedItems);
                string[] strs = kkid.Split(',');
                if (strs.Length > 1)
                {
                    //行驶方向
                    this.StoreDirection.DataSource = GetRedisData.ChangColName(GetRedisData.GetData("t_sys_code:240025"), true);
                    this.StoreDirection.DataBind();
                    return;
                }
                if (!string.IsNullOrEmpty(kkid))
                {
                    where = where + "  station_id  " + kkid;
                }
                else
                {
                    where = " 1=2";
                }
                DataTable data = tgsPproperty.GetDirectionInfoByWhere(where);
                this.StoreDirection.DataSource = data;
                this.StoreDirection.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-TgsRefresh", ex.Message + "；" + ex.StackTrace, "TgsRefresh has an exception");
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            try
            {
                starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string timeJs = "clearTime('" + starttime + "','" + endtime + "');";//js方法后面的分号一定要加上
                this.ResourceManager1.RegisterAfterClientInitScript(timeJs);
                this.uiDepartment.Reset();
                this.CmbPecType.Reset();
                this.CmbPlateType.Reset();
                this.CmbDataSource.Reset();
                this.CmbDealType.Reset();
                this.CmbLocation.Reset();
                this.TxtplateId.Reset();
                this.ChkLike.Reset();
                this.CmbDirection.Reset();
                this.WindowEditor1.SetVehicleText("");
                if (Session["location"] != null)
                {
                    Session["location"] = null;
                }
                //数据来源
                DataTable dt1 = GetRedisData.GetData("t_sys_code:240022");
                if (dt1 != null)
                {
                    this.StoreDataSource.DataSource = GetRedisData.ChangColName(dt1, true);
                    this.StoreDataSource.DataBind();
                }
                else
                {
                }
                DataTable dt2 = GetRedisData.GetData("Station:t_cfg_set_station_type_istmsshow");
                if (dt2 != null)
                {
                    this.StoreLocation.DataSource = ChangColName(dt2);
                    this.StoreLocation.DataBind();
                }
                else
                {
                    this.StoreLocation.DataSource = tgsPproperty.GetStationInfo("b.istmsshow ='1'");
                    this.StoreLocation.DataBind();
                }
                //行驶方向
                DataTable dt3 = GetRedisData.GetData("t_sys_code:240025");
                if (dt3 != null)
                {
                    this.StoreDirection.DataSource = GetRedisData.ChangColName(dt3, true);
                    this.StoreDirection.DataBind();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-ButResetClick", ex.Message + "；" + ex.StackTrace, "ButResetClick has an exception");
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
                GetData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh has an exception");
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
                    string xml = Bll.Common.GetPrintXml(GetLangStr("PeccancyQuery48", "违法车辆查询信息列表"), "", "", "printdatatable");
                    string js = "OpenPrintPageH(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-ButPrintClick", ex.Message + "；" + ex.StackTrace, "ButPrintClick has an exception");
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
                ShowQuery(1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-TbutFisrt", ex.Message + "；" + ex.StackTrace, "TbutFisrt has an exception");
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
                if (page < 1)
                {
                    page = 1;
                }
                curpage.Value = page;
                ShowQuery(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-TbutLast", ex.Message + "；" + ex.StackTrace, "TbutLast has an exception");
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
                int allpage = int.Parse(allPage.Value.ToString());
                page++;
                if (page > allpage)
                {
                    page = allpage;
                }
                curpage.Value = page;
                ShowQuery(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-TbutNext", ex.Message + "；" + ex.StackTrace, "TbutNext has an exception");
            }
        }

        /// <summary>
        /// 尾页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutEnd(object sender, DirectEventArgs e)
        {
            try
            {
                curpage.Value = allPage.Value;
                int page = int.Parse(curpage.Value.ToString());
                ShowQuery(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-TbutEnd", ex.Message + "；" + ex.StackTrace, "TbutEnd has an exception");
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
                logManager.InsertLogError("PeccancyQuery.aspx-ToXml", ex.Message + "；" + ex.StackTrace, "ToXml has an exception");
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
                DataTable dt = ChangeDataTable();
                ConvertData.ExportExcel(dt, this);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-ToExcel", ex.Message + "；" + ex.StackTrace, "ToExcel has an exception");
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
                logManager.InsertLogError("PeccancyQuery.aspx-ToCsv", ex.Message + "；" + ex.StackTrace, "ToCsv has an exception");
            }
        }

        #endregion 控件事件

        #region DirectMethod方法

        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="command"></param>
        /// <param name="url"></param>
        [DirectMethod]
        public void VideoShow(string command, string url)
        {
            //Window win = WindowShow.AddPlayVideo(dataCommon.ChangePoliceIp(url));
            //win.Render(this.Form);
            //win.Show();
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="isstart"></param>
        /// <param name="strtime"></param>
        [DirectMethod]
        public void GetDateTime(bool isstart, string strtime)
        {
            try
            {
                if (isstart)
                    starttime = strtime;
                else
                    endtime = strtime;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-GetDateTime", ex.Message + "；" + ex.StackTrace, "GetDateTime has an exception");
            }
        }

        #endregion DirectMethod方法

        #region 私有方法

        /// <summary>
        /// 转换查询模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void changtype(object sender, EventArgs e)
        {
            TxtplateId.Hidden = ChkLike.Checked;
            pnhphm.Hidden = !ChkLike.Checked;
        }

        public DataTable ToDataTable(DataRow[] rows)
        {
            try
            {
                if (rows == null || rows.Length == 0) return null;
                DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
                foreach (DataRow row in rows)
                    tmp.ImportRow(row);  // 将DataRow添加到DataTable中
                return tmp;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-GetDateTime", ex.Message + "；" + ex.StackTrace, "GetDateTime has an exception");
            }
            return null;
        }

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                //ButCsv.Disabled = true;
                ButExcel.Disabled = true;
                //ButXml.Disabled = true;
                // ButPrint.Disabled = true;
                //车俩类型
                DataTable dt1 = GetRedisData.GetData("t_sys_code:140001");
                if (dt1 != null)
                {
                    this.StorePlateType.DataSource = GetRedisData.ChangColName(dt1, true);
                    this.StorePlateType.DataBind();
                }
                else
                {
                    this.StorePlateType.DataSource = tgsPproperty.GetPalteType();
                    this.StorePlateType.DataBind();
                }

                //违法地点
                DataTable dt2 = GetRedisData.GetData("Station:t_cfg_set_station_type_istmsshow");
                if (dt2 != null)
                {
                    this.StoreLocation.DataSource = ChangColName(dt2);
                    this.StoreLocation.DataBind();
                }
                else
                {
                    this.StoreLocation.DataSource = tgsPproperty.GetStationInfo("b.istmsshow ='1'");
                    this.StoreLocation.DataBind();
                }

                //违法行为
                DataTable dt3 = GetRedisData.GetData("Peccancy:WFXW");
                if (dt3 != null)
                {
                    this.StorePecType.DataSource = GetRedisData.ChangColName(dt3, true);
                    this.StorePecType.DataBind();
                }
                else
                {
                    this.StorePecType.DataSource = tgsPproperty.GetPeccancyType("isuse='1'");
                    this.StorePecType.DataBind();
                }

                //数据来源
                DataTable dt4 = GetRedisData.GetData("t_sys_code:240022");
                if (dt4 != null)
                {
                    this.StoreDataSource.DataSource = GetRedisData.ChangColName(dt4, true);
                    this.StoreDataSource.DataBind();
                }
                else
                {
                    this.StoreDataSource.DataSource = tgsPproperty.GetDeviceTypeDict("240022");
                    this.StoreDataSource.DataBind();
                }

                //处理状态
                DataTable dt5 = GetRedisData.GetData("t_sys_code:240019");
                if (dt5 != null)
                {
                    this.StoreDealType.DataSource = GetRedisData.ChangColName(dt5, true);
                    this.StoreDealType.DataBind();
                }
                else
                {
                    this.StoreDealType.DataSource = tgsPproperty.GetProcessType();
                    this.StoreDealType.DataBind();
                }

                //行驶方向
                DataTable dt6 = dtFangxiang = GetRedisData.GetData("t_sys_code:240025");
                if (dt6 != null)
                {
                    this.StoreDirection.DataSource = GetRedisData.ChangColName(dt6, true);
                    this.StoreDirection.DataBind();
                }
                else
                {
                }
                this.StoreDirection.AutoLoad = true;
                DataTable dt = tgsPproperty.GetQueryNum();
                //this.StoreQueryNum.DataSource = dt;
                //this.StoreQueryNum.DataBind();

                //if (dt.Rows.Count > 0)
                //    CmbQueryNum.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        public static DataTable ChangColName(DataTable dt)
        {
            try
            {
                if (dt != null)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dt.Columns[i].ColumnName = "col" + (i + 1);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);

                return null;
            }
        }

        /// <summary>
        /// 设置初始时间
        /// </summary>
        private void DataSetDateTime()
        {
            try
            {
                starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                start.InnerText = starttime;

                endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                end.InnerText = endtime;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-DataSetDateTime", ex.Message + "；" + ex.StackTrace, "DataSetDateTime has an exception");
            }
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startNum"></param>
        /// <param name="endNum"></param>
        private void Query(string where, int startNum, int endNum)
        {
            try
            {
                DataTable dt = tgsDataInfo.GetPeccancyInfo(where, startNum, endNum);
                Session["datatable"] = dt;
                this.StorePeccancy.DataSource = dt;
                this.StorePeccancy.DataBind();

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.lblCurpage.Text = curpage.Value.ToString();
                    this.lblAllpage.Text = allPage.Value.ToString();
                    this.lblRealcount.Text = realCount.Value.ToString();
                    ButExcel.Disabled = false;
                }
                else
                {
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";
                    ButExcel.Disabled = false;
                    Notice(GetLangStr("PeccancyQuery51", "信息提示"), GetLangStr("PeccancyQuery52", "未查询到相关记录"));
                    ButExcel.Disabled = true;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-Query", ex.Message + "；" + ex.StackTrace, "Query has an exception");
            }
        }

        /// <summary>
        /// 显示指定页面的数据
        /// </summary>
        /// <param name="currentPage"></param>
        private void ShowQuery(int currentPage)
        {
            try
            {
                int rownum = 15;
                int startNum = (currentPage - 1) * rownum;
                int endNum = currentPage * rownum;
                Query(GetWhere(), startNum, endNum);
                SetButState(currentPage);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-ShowQuery", ex.Message + "；" + ex.StackTrace, "ShowQuery has an exception");
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        private void GetData()
        {
            try
            {
                maxtime = "";
                mintime = "";

                DataTable tempdt = tgsDataInfo.GetPeccancyMaxWfsj(GetWhere());
                if (tempdt != null && tempdt.Rows.Count > 0)
                {
                    maxtime = tempdt.Rows[0]["col0"].ToString();
                    mintime = tempdt.Rows[0]["col1"].ToString();
                    realCount.Value = tempdt.Rows[0]["col2"].ToString();
                    curpage.Value = 1;
                    int rownum = 15;
                    allPage.Value = (int)Math.Ceiling(double.Parse(realCount.Value.ToString()) / rownum);
                    ShowQuery(1);
                }
                else
                {
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";
                    Notice(GetLangStr("PeccancyQuery51", "信息提示"), GetLangStr("PeccancyQuery54", "未查询到符合条件的任何记录！"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-GetData", ex.Message + "；" + ex.StackTrace, "GetData has an exception");
            }
        }

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="page"></param>
        private void SetButState(int page)
        {
            try
            {
                curpage.Value = page;
                int allpage = int.Parse(allPage.Value.ToString());
                //this.PagingToolbar1.PageIndex = 0;
                if (allpage > 1)
                {
                    ButLast.Disabled = false;
                    ButNext.Disabled = false;
                    ButFisrt.Disabled = false;
                    ButEnd.Disabled = false;
                }
                if (page == 1)
                {
                    ButLast.Disabled = true;
                    ButFisrt.Disabled = true;
                }
                if (page == allpage)
                {
                    ButNext.Disabled = true;
                    ButEnd.Disabled = true;
                }
                if (allpage <= 1)
                {
                    ButFisrt.Disabled = true;
                    ButNext.Disabled = true;
                    ButLast.Disabled = true;
                    ButEnd.Disabled = true;
                    page = 0;
                }
                // LabNum.Html = "<font >&nbsp;&nbsp;" + GetLangStr("PeccancyQuery56", "当前") + page.ToString() + GetLangStr("PeccancyQuery57", "页,共") + allpage.ToString() + GetLangStr("PeccancyQuery58", "页") + "</font>";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-SetButState", ex.Message + "；" + ex.StackTrace, "SetButState has an exception");
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
                string hpzl = Bll.Common.GetdatabyField(sdata, "col1");
                string url1 = Bll.Common.GetdatabyField(sdata, "col23");
                string url2 = Bll.Common.GetdatabyField(sdata, "col24");
                string url3 = Bll.Common.GetdatabyField(sdata, "col25");
                if (string.IsNullOrEmpty(url2))
                {
                    url2 = NoImageUrl;
                }
                if (string.IsNullOrEmpty(url3))
                {
                    url3 = NoImageUrl;
                }
                string js = "ShowImage(\"" + url1 + "\",\"" + url2 + "\",\"" + url3 + "\",\"" + hphm + "\",\"" + hpzl + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-ApplyClick", ex.Message + "；" + ex.StackTrace, "ApplyClick has an exception");
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
                logManager.InsertLogError("PeccancyQuery.aspx-GetUrl", ex.Message + "；" + ex.StackTrace, "GetUrl has an exception");
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
                Window win = WindowShow.AddPeccancy(sdata);
                win.Render(this.Form);
                win.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-AddWindow", ex.Message + "；" + ex.StackTrace, "AddWindow has an exception");
            }
        }

        /// <summary>
        ///组装查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            try
            {
                string where = " jcbj<>'6'";
                //starttime = start.InnerText;
                //endtime = end.InnerText;

                if (!string.IsNullOrEmpty(maxtime))
                {
                    endtime = maxtime;
                }
                if (!string.IsNullOrEmpty(mintime))
                {
                    starttime = mintime;
                }
                string kssj = DateTime.Parse(starttime).ToString("yyyy-MM-dd HH:mm:ss");
                string jssj = DateTime.Parse(endtime).ToString("yyyy-MM-dd HH:mm:ss");

                where = where + " and  wfsj >= STR_TO_DATE('" + kssj + "','%Y-%m-%d %H:%i:%s')   and wfsj<=STR_TO_DATE('" + jssj + "','%Y-%m-%d %H:%i:%s')";
                if (CmbPlateType.SelectedIndex != -1)
                {
                    where = where + " and  hpzl='" + CmbPlateType.SelectedItem.Value + "' ";
                }

                if (!string.IsNullOrEmpty(uiDepartment.DepertId))
                {
                    where = where + " and  cjjg='" + uiDepartment.DepertId + "' ";
                }
                string kkid = GetMultiCombo(CmbLocation.SelectedItems);
                if (!string.IsNullOrEmpty(kkid))
                {
                    where = where + " and  kkid  " + kkid;
                    if (CmbDirection.SelectedIndex != -1)
                    {
                        if (CmbDirection.SelectedItem.Value.Equals("00") || CmbDirection.SelectedItem.Value.Equals("0"))
                        {
                            string fxs = "";//方向编号集合
                            for (int i = 1; i < dtFangxiang.Rows.Count; i++)
                            {
                                if (i == dtFangxiang.Rows.Count - 1)
                                {
                                    fxs = fxs + "'" + dtFangxiang.Rows[i]["col0"].ToString() + "'";
                                }
                                else
                                {
                                    fxs = fxs + "'" + dtFangxiang.Rows[i]["col0"].ToString() + "',";
                                }
                            }
                            where = where + " and  fxbh in (" + fxs + ") ";
                        }
                        else
                        {
                            where = where + " and  fxbh='" + CmbDirection.SelectedItem.Value + "' ";
                        }
                        //where = where + " and  fxbh='" + CmbDirection.SelectedItem.Value + "' ";
                    }
                }
                else
                {
                    DataTable location = Session["location"] as DataTable;
                    if (location != null && location.Rows.Count > 0)
                    {
                        string temp = "";
                        for (int i = 0; i < location.Rows.Count; i++)
                        {
                            if (i != location.Rows.Count - 1)
                            {
                                temp = temp + "'" + location.Rows[i]["col1"].ToString() + "',";
                            }
                            else
                            {
                                temp = temp + "'" + location.Rows[i]["col1"].ToString() + "'";
                            }
                        }
                        where = where + " and  kkid in ( " + temp + " )";
                    }
                }
                if (CmbPecType.SelectedIndex != -1)
                {
                    where = where + " and  wfxw='" + CmbPecType.SelectedItem.Value + "' ";
                }
                if (CmbDealType.SelectedIndex != -1)
                {
                    where = where + " and  jcbj='" + CmbDealType.SelectedItem.Value + "' ";
                }
                if (CmbDataSource.SelectedIndex != -1)
                {
                    where = where + " and  sjly='" + CmbDataSource.SelectedItem.Value + "' ";
                }
                string QueryHphm = string.Empty;
                if (ChkLike.Checked)
                {
                    //if (!string.IsNullOrEmpty(QueryHphm))
                    //{
                    string hphm = (string.IsNullOrEmpty(haopai_name1.Value) ? "" : haopai_name1.Value) +
                 (string.IsNullOrEmpty(haopai_name2.Value) ? "" : haopai_name2.Value) +
                 (string.IsNullOrEmpty(haopai_name3.Value) ? "" : haopai_name3.Value) +
                 (string.IsNullOrEmpty(haopai_name4.Value) ? "" : haopai_name4.Value) +
                 (string.IsNullOrEmpty(haopai_name5.Value) ? "" : haopai_name5.Value) +
                 (string.IsNullOrEmpty(haopai_name6.Value) ? "" : haopai_name6.Value);
                    QueryHphm = WindowEditor1.VehicleText + hphm;
                    if (!string.IsNullOrEmpty(QueryHphm))
                    {
                        where = where + " and  hphm  like '%" + QueryHphm.ToUpper() + "%' ";
                    }
                    else
                    {
                        where = where + " and  hphm  like '%" + QueryHphm.ToUpper() + "%' ";
                    }

                    // }
                }
                else
                {
                    QueryHphm = WindowEditor1.VehicleText + TxtplateId.Text;
                    if (!string.IsNullOrEmpty(QueryHphm))
                    {
                        where = where + " and  hphm='" + QueryHphm.ToUpper() + "' ";
                    }
                }
                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-GetWhere", ex.Message + "；" + ex.StackTrace, "GetWhere has an exception");
                return "";
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
                string data = e.ExtraParams["data"];
                string field = e.ExtraParams["field"];
                AddWindow(data);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-ShowDetails", ex.Message + "；" + ex.StackTrace, "ShowDetails has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sic"></param>
        /// <returns></returns>
        private string GetMultiCombo(SelectedListItemCollection sic)
        {
            try
            {
                string kkid = string.Empty;
                string kkid2 = string.Empty;
                for (int i = 0; i < sic.Count; i++)
                {
                    kkid2 = kkid2 + "'" + sic[i].Value + "',";
                }
                if (!string.IsNullOrEmpty(kkid2))
                {
                    kkid = "  in (" + kkid2.Substring(0, kkid2.Length - 1) + ")";
                }

                return kkid;
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("PeccancyQuery.aspx-GetMultiCombo", ex.Message + "；" + ex.StackTrace, "GetMultiCombo has an exception");
                ILog.WriteErrorLog(ex);
                return "";
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
                    dt1.Columns.Remove("col7"); dt1.Columns.Remove("col9"); dt1.Columns.Remove("col10");
                    dt1.Columns.Remove("col13");
                    for (int i = 15; i < dt.Columns.Count; i++)
                    {
                        if (!i.Equals(20))
                        {
                            dt1.Columns.Remove("col" + i.ToString());
                        }
                    }
                    dt1.Columns["col8"].SetOrdinal(0); dt1.Columns["col2"].SetOrdinal(2);
                    dt1.Columns["col6"].SetOrdinal(3); dt1.Columns["col20"].SetOrdinal(7);
                    dt1.Columns[0].ColumnName = GetLangStr("PeccancyQuery35", "违法地点");
                    dt1.Columns[1].ColumnName = GetLangStr("PeccancyQuery36", "号牌号码");
                    dt1.Columns[2].ColumnName = GetLangStr("PeccancyQuery37", "号牌种类");
                    dt1.Columns[3].ColumnName = GetLangStr("PeccancyQuery38", "违法时间");
                    dt1.Columns[4].ColumnName = GetLangStr("PeccancyQuery39", "违法行为");
                    dt1.Columns[5].ColumnName = GetLangStr("PeccancyQuery40", "行驶方向");
                    dt1.Columns[6].ColumnName = GetLangStr("PeccancyQuery41", "速度限速");
                    dt1.Columns[7].ColumnName = GetLangStr("PeccancyQuery42", "处理状态");
                    dt1.Columns[8].ColumnName = GetLangStr("PeccancyQuery43", "所属机构");
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
                logManager.InsertLogError("PeccancyQuery.aspx-ChangeDataTable", ex.Message + "；" + ex.StackTrace, "ChangeDataTable has an exception");
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
                logManager.InsertLogError("PeccancyQuery.aspx-ConvertCondition", ex.Message + "；" + ex.StackTrace, "ConvertCondition has an exception");
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
            try
            {
                Notification.Show(new NotificationConfig
                {
                    Title = title,
                    Icon = Icon.Information,
                    HideDelay = 2000,
                    Height = 120,
                    AlignCfg = new NotificationAlignConfig
                    {
                        ElementAnchor = AnchorPoint.BottomRight,
                        OffsetY = -60
                    },
                    Html = "<br></br>" + msg + "!"
                });
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyQuery.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice has an exception");
            }
        }

        #endregion 私有方法

        #region 语言转换

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

        #endregion 语言转换
    }
}