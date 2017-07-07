using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PeccancyDelete : System.Web.UI.Page
    {
        #region 成员变量

        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private SettingManager settingManager = new SettingManager();
        private UserLogin userLogin = new UserLogin();
        private const string NoImageUrl = "../images/NoImage.png";
        private DataCommon dataCommon = new DataCommon();
        private static string starttime = "";
        private static string endtime = "";

        private static string bl;

        /// <summary>
        /// 获取用户名
        /// </summary>
        private static string uName;

        /// <summary>
        /// 获取ip
        /// </summary>
        private static string nowIp;

        #endregion 成员变量

        #region 事件集合

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //new UserLogin().IsLogin();
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username)) { string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }
            try
            {
                if (!IsPostBack)
                {
                    if (!X.IsAjaxRequest)
                    {
                        //userLogin.IsLoginPage(this);
                        //UserInfo userInfo = System.Web.HttpContext.Current.Session["userinfo"] as UserInfo;
                        DataSetDateTime();
                        StoreDataBind();
                        this.DataBind();
                        TbutQueryClick(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyDelete.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
            }
            UserInfo userinfo = Session["Userinfo"] as UserInfo;
            logManager.InsertLogRunning(userinfo.UserName, "访问：" + Request.QueryString["funcname"], userinfo.NowIp, "0");
        }

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

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                GetData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyDelete.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
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
                this.CmbLocation.Reset();
                this.CmbDataSource.Reset();
                this.TxtplateId.Reset();
                this.ChkLike.Reset();
                this.CmbDealType.Reset();
                this.WindowEditor1.SetVehicleText("");
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
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyDelete.aspx-ButResetClick", ex.Message + "；" + ex.StackTrace, "ButResetClick has an exception");
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
        /// 显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowDetails(object sender, DirectEventArgs e)
        {
            try
            {
                string data = e.ExtraParams["data"];
                AddWindow(data);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyDelete.aspx-ShowDetails", ex.Message + "；" + ex.StackTrace, "ShowDetails has an exception");
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
                bl = sdata;
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
                logManager.InsertLogError("PeccancyDelete.aspx-ApplyClick", ex.Message + "；" + ex.StackTrace, "ApplyClick has an exception");
            }
        }

        /// <summary>
        /// 删除违法记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButDeleteClick(object sender, DirectEventArgs e)
        {
            try
            {
                List<string> delIds = new List<string>();
                //RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
                RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
                foreach (SelectedRow row in sm.SelectedRows)
                {
                    delIds.Add(row.RecordID);
                }
                X.Msg.Confirm(GetLangStr("PeccancyQuery36", "信息"), "确认要删除选中的吗?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "PeccancyDelete.DoYes()",
                        Text = GetLangStr("PeccancyQuery39", "是")
                    },
                    No = new MessageBoxButtonConfig
                    {
                        Handler = "PeccancyDelete.DoNo()",
                        Text = GetLangStr("PeccancyQuery40", "否")
                    }
                }).Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyDelete.aspx-ButDeleteClick", ex.Message + "；" + ex.StackTrace, "ButDeleteClick has an exception");
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
                if (this.CmbDataSource.Value != null && CmbDataSource.SelectedIndex != -1)
                {
                    DataTable ddbh = new DataTable();
                    this.StoreLocation.DataSource = tgsPproperty.GetStationInfo("b.datasource='" + this.CmbDataSource.Value.ToString() + "' and b.istmsshow ='1'");
                    this.StoreLocation.DataBind();
                }
                else
                {
                    //DataTable ddbh = new DataTable();
                    //this.StoreLocation.DataSource = tgsPproperty.GetStationInfo("b.istmsshow ='1'");
                    //this.StoreLocation.DataBind();

                    this.StoreLocation.DataSource = ChangColName(GetRedisData.GetData("Station:t_cfg_set_station_type_istmsshow"));
                    this.StoreLocation.DataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyDelete.aspx-DataSourceRefresh", ex.Message + "；" + ex.StackTrace, "DataSourceRefresh has an exception");
            }
        }

        #endregion 事件集合

        #region [DirectMethod]

        /// <summary>
        /// 确定事件
        /// </summary>
        [DirectMethod]
        public void DoYes()
        {
            List<string> delIds = new List<string>();
            try
            {
                RowSelectionModel sm = this.GridPanel1.SelectionModel.Primary as RowSelectionModel;
                string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                foreach (SelectedRow row in sm.SelectedRows)
                {
                    delIds.Add(row.RecordID);

                    if (ipaddress.Length < 9)
                    {
                        ipaddress = "127.0.0.1";
                    }
                }

                if (tgsDataInfo.DeletePeccancyInfo(delIds) > 0)
                {
                    string log = "";
                    DataTable dt = Session["datatable"] as DataTable;
                    for (int i = 0; i < delIds.Count; i++)
                    {
                        DataRow[] rows = dt.Select("col0='" + delIds[i] + "'");
                        logManager.InsertLogRunning(UserLogin.GetUserName(), "违法删除成功：号牌种类[" + rows[0]["col2"].ToString() + "] ,号牌号码[" + rows[0]["col3"].ToString() + "] ,违法记录Id[" + rows[0]["col0"].ToString() + "] ,违法时间[" + rows[0]["col6"].ToString() + "] ,违法地点[" + rows[0]["col8"].ToString() + "] ,违法行为[" + rows[0]["col5"].ToString() + "]", ipaddress, "8", rows[0]["col3"].ToString(), rows[0]["col0"].ToString());
                    }
                    Notice(GetLangStr("PeccancyQuery42", "信息提示"), GetLangStr("PeccancyQuery43", "删除成功"));

                    string jd = Bll.Common.GetdatabyField(bl, "col8");
                    string hp = Bll.Common.GetdatabyField(bl, "col3");
                    string hl = Bll.Common.GetdatabyField(bl, "col2");
                    string xf = Bll.Common.GetdatabyField(bl, "col11");
                    // logManager.InsertLogRunning(uName, "违法删除:检测地点:[" + jd + "];号牌种类:[" + hl + "];号牌:[" + hp + "];行驶方向:[" + xf + "]", nowIp, "3");
                    GetData();
                    sm.ClearSelections();
                    sm.UpdateSelection();
                }
                else
                {
                    logManager.InsertLogRunning(UserLogin.GetUserName(), "违法删除失败", ipaddress, "6", "", "");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyDelete.aspx-DoYes", ex.Message + "；" + ex.StackTrace, "DoYes has an exception");
            }
        }

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
        /// 不删除事件
        /// </summary>
        [DirectMethod]
        public void DoNo()
        {
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
                logManager.InsertLogError("PeccancyDelete.aspx-GetDateTime", ex.Message + "；" + ex.StackTrace, "GetDateTime has an exception");
            }
        }

        #endregion [DirectMethod]

        #region 私有方法

        public DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                UserInfo userInfo = new UserInfo();
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

                //处理状态
                DataTable dt5 = GetRedisData.GetData("t_sys_code:240019");
                if (dt5 != null)
                {
                    dt5.Rows.RemoveAt(1);
                    dt5.Rows.RemoveAt(1);
                    dt5.Rows.RemoveAt(1);
                    dt5.Rows.RemoveAt(1);
                    this.StoreDealType.DataSource = GetRedisData.ChangColName(dt5, true);
                    this.StoreDealType.DataBind();
                }
                else
                {
                    DataTable dt8 = tgsPproperty.GetProcessType();
                    dt8.Rows.RemoveAt(1);
                    dt8.Rows.RemoveAt(1);
                    dt8.Rows.RemoveAt(1);
                    dt8.Rows.RemoveAt(1);
                    this.StoreDealType.DataSource = dt8;
                    this.StoreDealType.DataBind();
                }
                //DataTable deal = GetRedisData.GetData("t_sys_code:240019");
                //deal.Rows.RemoveAt(1);
                //deal.Rows.RemoveAt(1);
                //deal.Rows.RemoveAt(1);
                //deal.Rows.RemoveAt(1);
                //this.StoreDealType.DataSource = deal;
                //this.StoreDealType.DataBind();

                //DataTable dt = tgsPproperty.GetQueryNum();
                //this.StoreQueryNum.DataSource = dt;
                //this.StoreQueryNum.DataBind();

                ButDelete.Disabled = true;

                //if (dt.Rows.Count > 0)
                //    CmbQueryNum.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyDelete.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
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
                logManager.InsertLogError("PeccancyDelete.aspx-DataSetDateTime", ex.Message + "；" + ex.StackTrace, "DataSetDateTime has an exception");
            }
        }

        /// <summary>
        /// 获得查询数据
        /// </summary>
        /// <returns></returns>
        private void GetData()
        {
            try
            {
                string rownum = "15";
                DataTable dtCount = tgsDataInfo.GetPeccancyInfoCount(GetWhere());//获得总记录
                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    realCount.Value = dtCount.Rows[0]["col0"].ToString();
                    curpage.Value = 1;
                    allPage.Value = (int)Math.Ceiling(double.Parse(realCount.Value.ToString()) / Convert.ToInt32(rownum));
                    ShowQuery(1);
                }
                else
                {
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";
                    Notice("信息提示", "未查询到符合条件的任何记录！");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyDelete.aspx-GetData", ex.Message + "；" + ex.StackTrace, "GetData has an exception");
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
                logManager.InsertLogError("PeccancyDelete.aspx-TbutFisrt", ex.Message + "；" + ex.StackTrace, "TbutFisrt has an exception");
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
                logManager.InsertLogError("PeccancyDelete.aspx-TbutLast", ex.Message + "；" + ex.StackTrace, "TbutLast has an exception");
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
                logManager.InsertLogError("PeccancyDelete.aspx-TbutNext", ex.Message + "；" + ex.StackTrace, "TbutNext has an exception");
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
                logManager.InsertLogError("PeccancyDelete.aspx-TbutEnd", ex.Message + "；" + ex.StackTrace, "TbutEnd has an exception");
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
                logManager.InsertLogError("PeccancyDelete.aspx-ShowQuery", ex.Message + "；" + ex.StackTrace, "ShowQuery has an exception");
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
                DataTable dt = tgsDataInfo.GetPeccancyInfo(GetWhere(), startNum, endNum);
                Session["datatable"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    ButDelete.Disabled = false;
                }
                else
                {
                    ButDelete.Disabled = true;
                }

                this.StorePeccancy.DataSource = dt;
                this.StorePeccancy.DataBind();

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.lblCurpage.Text = curpage.Value.ToString();
                    this.lblAllpage.Text = allPage.Value.ToString();
                    this.lblRealcount.Text = realCount.Value.ToString();

                    ButDelete.Disabled = false;
                }
                else
                {
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";
                    Notice("信息提示", "未查询到相关记录");
                    this.StorePeccancy.DataSource = new DataTable();
                    this.StorePeccancy.DataBind();
                    ButDelete.Disabled = true;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyDelete.aspx-Query", ex.Message + "；" + ex.StackTrace, "Query has an exception");
            }
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            try
            {
                UserInfo userInfo = System.Web.HttpContext.Current.Session["userinfo"] as UserInfo;
                string where = " jcbj<>'6'";
                if (string.IsNullOrEmpty(starttime)) starttime = start.InnerText;
                if (string.IsNullOrEmpty(endtime)) endtime = end.InnerText;
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
                if (CmbLocation.SelectedIndex != -1)
                {
                    where = where + " and  kkid='" + CmbLocation.SelectedItem.Value + "' ";
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
                logManager.InsertLogError("PeccancyDelete.aspx-GetWhere", ex.Message + "；" + ex.StackTrace, "GetWhere has an exception");
                return null;
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
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyDelete.aspx-SetButState", ex.Message + "；" + ex.StackTrace, "SetButState has an exception");
            }
        }

        /// <summary>
        /// 转换datatable
        /// </summary>
        /// <returns></returns>
        private DataTable ChangeDataTable()
        {
            try
            {
                DataTable dt = Session["datatable"] as DataTable;
                DataTable dt2 = null; ;
                if (dt != null)
                {
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

                return dt2;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyDelete.aspx-ChangeDataTable", ex.Message + "；" + ex.StackTrace, "ChangeDataTable has an exception");
                return null;
            }
        }

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
                logManager.InsertLogError("PeccancyDelete.aspx-GetUrl", ex.Message + "；" + ex.StackTrace, "GetUrl has an exception");
                return null;
            }
        }

        private void AddWindow(string sdata)
        {
            //DataTable dt = tgsDataInfo.GetPassCarImageUrl(Bll.Common.GetdatabyField(sdata, "col0"));
            Window win = WindowShow.AddPeccancy(sdata);
            win.Render(this.Form);
            win.Show();
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
                logManager.InsertLogError("PeccancyDelete.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice has an exception");
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

        #endregion 私有方法
    }
}