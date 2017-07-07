using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class AlarmInfoQuery : System.Web.UI.Page
    {
        #region 成员变量

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private SettingManager settingManager = new SettingManager();
        private Bll.ServiceManager servicemansger = new Bll.ServiceManager();
        private UserLogin userLogin = new UserLogin();
        private DataCommon dataCommon = new DataCommon();
        private static string starttime = "";
        private static string endtime = "";

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
                string js = "alert('" + GetLangStr("AlarmInfoQuery53", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!IsPostBack)
            {
                if (!X.IsAjaxRequest)
                {
                    Session["tree"] = null;
                    string js1 = "clearMenu();";
                    this.ResourceManager1.RegisterAfterClientInitScript(js1);
                    DataSetDateTime();
                    StoreDataBind();
                    //BuildTree(TreeStation.Root);
                    TbutQueryClick(null, null);
                    this.DataBind();
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, "" + GetLangStr("AlarmInfoQuery54", "访问：") + "" + Request.QueryString["funcname"], userinfo.NowIp, "0");
                }
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
                GetData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoQuery.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
            }
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
                //if (!string.IsNullOrEmpty(FieldStation.Text))//判断卡口是否为空
                //{
                //    string js = "directclear();";
                //    this.ResourceManager1.RegisterAfterClientInitScript(js);
                //}
                //卡口列表
                if (!string.IsNullOrEmpty(kakou.Value))//判断卡口是否为空
                {
                    string js = "clearMenu();";
                    if (Session["tree"] != null)
                    {
                        Session["tree"] = null;
                    }
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
                this.CmbAlarmType.Reset();
                vehicleHead.SetVehicleText("");
                TxtplateId.Text = "";
                this.CmbPlateType.Reset();
                this.TxtplateId.Reset();
                this.ChkLike.Reset();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoQuery.aspx-ButResetClick", ex.Message + "；" + ex.StackTrace, "ButResetClick has an exception");
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
                string field = e.ExtraParams["field"];
                AddWindow(data);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoQuery.aspx-ShowDetails", ex.Message + "；" + ex.StackTrace, "ShowDetails has an exception");
            }
        }

        /// <summary>
        /// 上一页事件
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
                logManager.InsertLogError("AlarmInfoQuery.aspx-TbutLast", ex.Message + "；" + ex.StackTrace, "TbutLast has an exception");
            }
        }

        /// <summary>
        ///下一页事件
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
                logManager.InsertLogError("AlarmInfoQuery.aspx-TbutNext", ex.Message + "；" + ex.StackTrace, "TbutNext has an exception");
            }
        }

        /// <summary>
        ///首页事件
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
                logManager.InsertLogError("AlarmInfoQuery.aspx-TbutFisrt", ex.Message + "；" + ex.StackTrace, "TbutFisrt has an exception");
            }
        }

        /// <summary>
        ///尾页事件
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
                logManager.InsertLogError("AlarmInfoQuery.aspx-TbutEnd", ex.Message + "；" + ex.StackTrace, "TbutEnd has an exception");
            }
        }

        /// <summary>
        /// 刷新数据
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
                logManager.InsertLogError("AlarmInfoQuery.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh has an exception");
            }
        }

        /// <summary>
        /// 打印事件
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
                    string xml = Bll.Common.GetPrintXml(GetLangStr("AlarmInfoQuery55", "报警车辆查询信息列表"), "", "", "printdatatable");
                    string js = "OpenPrintPageH(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoQuery.aspx-ButPrintClick", ex.Message + "；" + ex.StackTrace, "ButPrintClick has an exception");
            }
        }

        /// <summary>
        /// 导出为xml
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
                logManager.InsertLogError("AlarmInfoQuery.aspx-ToXml", ex.Message + "；" + ex.StackTrace, "ToXml has an exception");
            }
        }

        /// <summary>
        /// 导出为excel
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
                logManager.InsertLogError("AlarmInfoQuery.aspx-ToExcel", ex.Message + "；" + ex.StackTrace, "ToExcel has an exception");
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
                logManager.InsertLogError("AlarmInfoQuery.aspx-ToCsv", ex.Message + "；" + ex.StackTrace, "ToCsv has an exception");
            }
        }

        #endregion 控件事件

        #region DirectMethod

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
                logManager.InsertLogError("AlarmInfoQuery.aspx-GetDateTime", ex.Message + "；" + ex.StackTrace, "GetDateTime has an exception");
            }
        }

        [DirectMethod(Namespace = "Ovel")]
        public void SaveClbj(string xh)
        {
            try
            {
                string clbj = X.GetCmp<ComboBox>("cmbClbj").Text;
                string clbjms = X.GetCmp<ComboBox>("cmbClbj").SelectedItem.Text;
                if (string.IsNullOrEmpty(clbj))
                {
                    Notice(GetLangStr("AlarmInfoQuery56", "提示"), GetLangStr("AlarmInfoQuery57", "请选择处理结果"));
                }
                else
                {
                    Hashtable hs = new Hashtable();
                    UserInfo userinfo = Session["userinfo"] as UserInfo;
                    hs.Add("xh", xh);
                    hs.Add("clry", userinfo.UserCode);
                    hs.Add("clyj", "");
                    hs.Add("clbj", clbj);

                    if (tgsDataInfo.DealAlarmInfo(hs) > 0)
                    {
                        Notice(GetLangStr("AlarmInfoQuery58", "信息提示"), GetLangStr("AlarmInfoQuery59", "处理完成"));
                        ShowQuery(Convert.ToInt32(curpage.Value));
                        X.GetCmp<TextField>("txtclzt").Text = clbjms;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DirectMethod

        #region 私有方法

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                //车俩类型
                DataTable dt = GetRedisData.GetData("t_sys_code:140001");
                if (dt != null)
                {
                    this.StorePlateType.DataSource = Bll.Common.ChangColName(dt);
                    this.StorePlateType.DataBind();
                }
                else
                {
                    dt = tgsPproperty.GetPalteType();
                    this.StorePlateType.DataSource = dt;
                    this.StorePlateType.DataBind();
                }

                DataTable dt2 = GetRedisData.GetData("t_sys_code:300100");
                //报警类型
                if (dt2 != null)
                {
                    this.StoreAlarmType.DataSource = Bll.Common.ChangColName(dt2);
                    this.StoreAlarmType.DataBind();
                }
                else
                {
                }

                //报警消息处理结果
                DataTable dt3 = GetRedisData.GetData("t_sys_code:421300");
                if (dt3 != null)
                {
                    this.StoreClbj.DataSource = Bll.Common.ChangColName(dt3);
                    this.StoreClbj.DataBind();
                }
                else
                {
                }

                //报警消息处理状态
                DataTable dt4 = GetRedisData.GetData("t_sys_code:430600");
                if (dt4 != null)
                {
                    this.ResultStore.DataSource = Bll.Common.ChangColName(dt4);
                    this.ResultStore.DataBind();
                }
                else
                {
                }
                DataTable dt5 = GetRedisData.GetData("t_sys_code:430700");
                if (dt5 != null)
                {
                    this.DispatchedStore.DataSource = Bll.Common.ChangColName(dt5);
                    this.DispatchedStore.DataBind();
                }
                else
                {
                }

                //ButCsv.Disabled = true;
                ButExcel.Disabled = true;
                //ButXml.Disabled = true;
                //ButPrint.Disabled = true;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoQuery.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
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
                string page = "MyNet.Atmcs.Uscmcp.Web.AlarmInfoQuery";
                Window win = WindowShow.AddAlarmCar(page, sdata);
                win.Render(this.Form);
                win.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoQuery.aspx-AddWindow", ex.Message + "；" + ex.StackTrace, "AddWindow has an exception");
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
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoQuery.aspx-SetButState", ex.Message + "；" + ex.StackTrace, "SetButState has an exception");
            }
        }

        /// <summary>
        ///获得一页显示条数
        /// </summary>
        /// <returns></returns>
        private int GetRowNum()
        {
            try
            {
                string rownum = "";
                if (CmbQueryNum.SelectedIndex != -1)
                {
                    rownum = CmbQueryNum.SelectedItem.Value.ToString();
                }
                else
                {
                    rownum = "50";
                }
                return int.Parse(rownum);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoQuery.aspx-GetRowNum", ex.Message + "；" + ex.StackTrace, "GetRowNum has an exception");
                return 50;
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
                DataTable dt = tgsDataInfo.GetAlarmInfo(where, startNum, endNum);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ButExcel.Disabled = false;
                    if (dt.Rows.Count.Equals(15))// 有数据 且够提取的条数
                    {
                        this.lblCurpage.Text = curpage.Value.ToString();
                        this.lblAllpage.Text = allPage.Value.ToString();
                        this.lblRealcount.Text = realCount.Value.ToString();
                    }
                    else
                    {
                        this.lblCurpage.Text = curpage.Value.ToString();
                        this.lblAllpage.Text = allPage.Value.ToString();
                        this.lblRealcount.Text = realCount.Value.ToString();
                    }
                    this.StoreAlarmInfo.DataSource = dt;
                    this.StoreAlarmInfo.DataBind();
                }
                else
                {
                    ButExcel.Disabled = true;
                    this.lblCurpage.Text = curpage.Value.ToString();
                    this.lblAllpage.Text = allPage.Value.ToString();
                    this.lblRealcount.Text = realCount.Value.ToString();
                    this.StoreAlarmInfo.DataSource = dt;
                    this.StoreAlarmInfo.DataBind();
                    Notice(GetLangStr("AlarmInfoQuery58", "信息提示"), GetLangStr("AlarmInfoQuery60", "当前没数据"));
                    return;
                }
                Session["datatable"] = dt;
                this.StoreAlarmInfo.DataSource = dt;
                this.StoreAlarmInfo.DataBind();
                int realnum = startNum + dt.Rows.Count - 1;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoQuery.aspx-Query", ex.Message + "；" + ex.StackTrace, "Query has an exception");
            }
        }

        /// <summary>
        /// 展示选中页数据
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
                logManager.InsertLogError("AlarmInfoQuery.aspx-ShowQuery", ex.Message + "；" + ex.StackTrace, "ShowQuery has an exception");
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        private void GetData()
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable tempdt = tgsDataInfo.GetAlarmInfoCount(GetWhere(), 0, 0);
                if (tempdt != null && tempdt.Rows.Count > 0)
                {
                    realCount.Value = tempdt.Rows[0]["col0"].ToString();
                    curpage.Value = 1;
                    int rownum = 15;
                    allPage.Value = (int)Math.Ceiling(double.Parse(realCount.Value.ToString()) / rownum);
                    ShowQuery(1);
                    //    ButExcel.Disabled = false;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoQuery.aspx-GetData", ex.Message + "；" + ex.StackTrace, "GetData has an exception");
            }
        }

        /// <summary>
        /// 提示信息
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
                    Html = "<br></br>" + msg + "!"
                });
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoQuery.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice has an exception");
            }
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            string where = "1=1";
            try
            {
                if (string.IsNullOrEmpty(starttime)) starttime = start.InnerText;
                if (string.IsNullOrEmpty(endtime)) endtime = end.InnerText;
                string kssj = DateTime.Parse(starttime).ToString("yyyy-MM-dd HH:mm:ss");
                string jssj = DateTime.Parse(endtime).ToString("yyyy-MM-dd HH:mm:ss");
                where = "  bjsj >= STR_TO_DATE('" + kssj + "','%Y-%m-%d %H:%i:%s')   and bjsj<=STR_TO_DATE('" + jssj + "','%Y-%m-%d %H:%i:%s')";
                //号牌种类
                if (CmbPlateType.SelectedIndex != -1)
                {
                    where = where + " and  hpzl='" + CmbPlateType.SelectedItem.Value + "' ";
                }
                //处理结果条件
                if (ResultCombo.SelectedIndex != -1)
                {
                    where = where + " and  clbj='" + ResultCombo.SelectedItem.Value + "' ";
                }
                //布控类型
                if (DispatchedComboBox.SelectedIndex != -1)
                {
                    where = where + " and  bklx='" + DispatchedComboBox.SelectedItem.Value + "' ";
                }
                //卡口范围
                if (!string.IsNullOrEmpty(this.kakou.Value))
                {
                    string kkid = this.kakouId.Value.ToString();
                    if (!string.IsNullOrEmpty(kkid))
                    {
                        where = where + " and  kkid in (" + kkid + ") ";
                        //condition.Kkid = kkid;
                        if (Session["tree"] != null)
                        {
                            Session["tree"] = null;
                        }
                        Session["tree"] = kkid;
                    }
                    //condition.Kkidms = this.kakou.Value;
                }
                //报警类型
                if (CmbAlarmType.SelectedIndex != -1)
                {
                    where = where + " and  bjlx='" + CmbAlarmType.SelectedItem.Value + "' ";
                }
                //消息类型
                if (McmbType.SelectedItems.Count > 0)
                {
                    string type = "";
                    if (McmbType.SelectedItems.Count == 1)
                    {
                        foreach (var item in McmbType.SelectedItems)
                        {
                            type = item.Value;
                        }
                        where = where + " and type='" + type + "' ";
                    }
                    else
                    {
                    }
                }
                string QueryHphm = string.Empty;
                if (!string.IsNullOrEmpty(vehicleHead.VehicleText))
                {
                    QueryHphm = vehicleHead.VehicleText + TxtplateId.Text;
                }
                else
                {
                    QueryHphm = TxtplateId.Text;
                }
                if (ChkLike.Checked)
                {
                    if (!string.IsNullOrEmpty(QueryHphm))
                    {
                        where = where + " and  hphm  like '%" + QueryHphm.ToUpper() + "%' ";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(QueryHphm))
                    {
                        where = where + " and  hphm='" + QueryHphm.ToUpper() + "' ";
                    }
                }
                return where;
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("AlarmInfoQuery.aspx-GetWhere", ex.Message + "；" + ex.StackTrace, "GetWhere has an exception");
                ILog.WriteErrorLog(ex);
                return where;
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
                string hpzl = Bll.Common.GetdatabyField(sdata, "col5");

                string url1 = Bll.Common.GetdatabyField(sdata, "col14");
                string url2 = Bll.Common.GetdatabyField(sdata, "col15");
                string url3 = Bll.Common.GetdatabyField(sdata, "col16");
                //string js = "ShowImage(\"" + dataCommon.ChangePoliceIp(url1) + "\",\"" + dataCommon.ChangePoliceIp(url2) + "\",\"" + dataCommon.ChangePoliceIp(url3) + "\",\"" + hphm + "\",\"" + hpzl + "\");";

                string js = "ShowImage(\"" + url1 + "\",\"" + url2 + "\",\"" + url3 + "\",\"" + hphm + "\",\"" + hpzl + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("AlarmInfoQuery.aspx-ApplyClick", ex.Message + "；" + ex.StackTrace, "ApplyClick has an exception");
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 转换datatable
        /// </summary>
        /// <returns></returns>
        private DataTable ChangeDataTable()
        {
            DataTable dt = Session["datatable"] as DataTable;
            try
            {
                DataTable dt1 = dt.Copy();
                if (dt1 != null)
                {
                    dt1.Columns.Remove("col0"); dt1.Columns.Remove("col1"); dt1.Columns.Remove("col4");
                    dt1.Columns.Remove("col7"); dt1.Columns.Remove("col8"); dt1.Columns.Remove("col9");
                    dt1.Columns.Remove("col10"); dt1.Columns.Remove("col11"); dt1.Columns.Remove("col12");
                    dt1.Columns.Remove("col13"); dt1.Columns.Remove("col14"); dt1.Columns.Remove("col15");
                    dt1.Columns.Remove("col16"); dt1.Columns.Remove("col17"); dt1.Columns.Remove("col18");
                    dt1.Columns.Remove("col20"); dt1.Columns.Remove("col22"); dt1.Columns.Remove("col23");
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (dt1.Rows[i]["col21"] != null)
                        {
                            if (dt1.Rows[i]["col21"].ToString() == "1")
                            {
                                dt1.Rows[i]["col21"] = GetLangStr("AlarmInfoQuery13", "报警");
                            }
                            else
                            {
                                dt1.Rows[i]["col21"] = GetLangStr("AlarmInfoQuery12", "预警");
                            }
                        }
                    }
                    //设置内存表中顺序
                    dt1.Columns["col2"].SetOrdinal(0);
                    dt1.Columns["col3"].SetOrdinal(1);
                    dt1.Columns["col5"].SetOrdinal(2);
                    dt1.Columns["col28"].SetOrdinal(3);
                    dt1.Columns["col6"].SetOrdinal(4);
                    dt1.Columns["col19"].SetOrdinal(5);
                    dt1.Columns["col27"].SetOrdinal(6);
                    dt1.Columns["col24"].SetOrdinal(7);
                    dt1.Columns["col21"].SetOrdinal(8);
                    dt1.Columns["col29"].SetOrdinal(9);
                    dt1.Columns["col26"].SetOrdinal(10);
                    dt1.Columns["col25"].SetOrdinal(11);
                    dt1.Columns[0].ColumnName = GetLangStr("AlarmInfoQuery32", "报警卡口").Replace(" ", "_");
                    dt1.Columns[1].ColumnName = GetLangStr("AlarmInfoQuery33", "号牌号码").Replace(" ", "_");
                    dt1.Columns[2].ColumnName = GetLangStr("AlarmInfoQuery34", "号牌种类").Replace(" ", "_");
                    dt1.Columns[3].ColumnName = GetLangStr("AlarmInfoQuery35", "布控类型").Replace(" ", "_");
                    dt1.Columns[4].ColumnName = GetLangStr("AlarmInfoQuery36", "报警时间").Replace(" ", "_");
                    dt1.Columns[5].ColumnName = GetLangStr("AlarmInfoQuery37", "报警类型").Replace(" ", "_");
                    dt1.Columns[6].ColumnName = GetLangStr("AlarmInfoQuery38", "报警原因").Replace(" ", "_");
                    dt1.Columns[7].ColumnName = GetLangStr("AlarmInfoQuery39", "处理状态").Replace(" ", "_");
                    dt1.Columns[8].ColumnName = GetLangStr("AlarmInfoQuery40", "消息类型").Replace(" ", "_");
                    dt1.Columns[9].ColumnName = GetLangStr("AlarmInfoQuery61", "有效时间").Replace(" ", "_");
                    dt1.Columns[10].ColumnName = GetLangStr("AlarmInfoQuery62", "布控人姓名").Replace(" ", "_");
                    dt1.Columns[11].ColumnName = GetLangStr("AlarmInfoQuery63", "布控联系电话").Replace(" ", "_");
                }

                return dt1;
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("AlarmInfoQuery.aspx-ChangeDataTable", ex.Message + "；" + ex.StackTrace, "ChangeDataTable has an exception");
                ILog.WriteErrorLog(ex);
                return dt;
            }
        }

        /// <summary>
        /// 设置初始时间
        /// </summary>
        private void DataSetDateTime()
        {
            starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            start.InnerText = starttime;
            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            end.InnerText = endtime;
            // panelTime.Render();
        }

        /// <summary>
        /// 组件卡口列表树
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private Ext.Net.TreeNodeCollection BuildTree(Ext.Net.TreeNodeCollection nodes)
        {
            try
            {
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }

                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Text = GetLangStr("AlarmInfoQuery64", "卡口列表");
                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;

                // 添加 自己机构节点 和卡口
                UserInfo user = Session["userinfo"] as UserInfo;
                if (user == null)
                {
                    user = new UserInfo();
                    user.DepartName = "滨州市交通警察支队";
                    user.DeptCode = "371600000000";
                }

                Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                nodeRoot.Text = user.DepartName;
                nodeRoot.Leaf = true;
                nodeRoot.NodeID = user.DeptCode;
                nodeRoot.Icon = Icon.House;

                DataTable dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
                DataRow[] rowsStation = dtStation.Select("departid='" + user.DeptCode + "'", "station_name asc");
                AddStationTree(nodeRoot, rowsStation);
                nodeRoot.Expanded = false;
                nodeRoot.Draggable = true;
                nodeRoot.Expandable = ThreeStateBool.True;
                root.Nodes.Add(nodeRoot);

                //绑定下级部门及下级部门卡口
                AddDepartTree(root, user.DeptCode);

                return nodes;
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("AlarmInfoQuery.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///绑定下级部门及下级部门卡口
        /// </summary>
        /// <param name="root"></param>
        private void AddDepartTree(Ext.Net.TreeNode root, string departCode)
        {
            try
            {
                DataTable dtDepart = GetRedisData.GetData("t_cfg_department");
                DataRow[] rows = dtDepart.Select("classcode='" + departCode + "'");
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count(); i++)
                    {
                        Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                        nodeRoot.Text = rows[i]["departname"].ToString();
                        nodeRoot.Leaf = true;
                        nodeRoot.NodeID = rows[i]["departid"].ToString();
                        nodeRoot.Icon = Icon.House;

                        DataTable dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
                        DataRow[] rowsStation = dtStation.Select(" departid='" + nodeRoot.NodeID + "' ", "station_name asc");
                        AddStationTree(nodeRoot, rowsStation);
                        nodeRoot.Expanded = false;
                        nodeRoot.Draggable = true;
                        nodeRoot.Expandable = ThreeStateBool.True;
                        AddDepartTree(nodeRoot, rows[i]["departid"].ToString());
                        root.Nodes.Add(nodeRoot);
                    }
                }
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("AlarmInfoQuery.aspx-AddDepartTree", ex.Message + "；" + ex.StackTrace, "AddDepartTree has an exception");
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 添加卡口子节点
        /// </summary>
        /// <param name="root"></param>
        private void AddStationTree(Ext.Net.TreeNode DepartNode, DataRow[] rows)
        {
            try
            {
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count(); i++)
                    {
                        Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                        node.Text = rows[i]["station_name"].ToString();
                        node.Leaf = true;
                        node.Checked = ThreeStateBool.False;
                        node.NodeID = rows[i]["station_id"].ToString();
                        node.Draggable = false;
                        node.AllowDrag = false;
                        DepartNode.Nodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("AlarmInfoQuery.aspx-AddStationTree", ex.Message + "；" + ex.StackTrace, "AddStationTree has an exception");
                ILog.WriteErrorLog(ex);
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

        #region 卡口选择

        /// <summary>
        /// 清除卡口信息
        /// </summary>
        [DirectMethod]
        public void ClearKakou()
        {
            if (Session["Condition"] != null)
            {
                Condition con = Session["Condition"] as Condition;
                con.Kkid = "";
                con.Kkidms = "";
            }
            if (Session["tree"] != null)
            {
                Session["tree"] = null;
            }
        }

        /// <summary>
        /// 得到符合条件的卡口
        /// </summary>
        [DirectMethod]
        public void GetKakou()
        {
            try
            {
                string value = kakou.Value;

                DataTable dtSelect = null;
                DataTable dt = Session["selectKakou"] as DataTable;//得到整个卡口信息
                DataRow[] rows = dt.Select("STATION_NAME like '" + value + "%'");//选出符合条件的
                if (rows.Length > 0)
                {
                    dtSelect = ToDataTable(rows);
                }
                StringBuilder strs = new StringBuilder();
                strs.AppendLine("[");
                if (dtSelect != null && dtSelect.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSelect.Rows.Count; i++)
                    {
                        if (i == dtSelect.Rows.Count - 1)
                        {
                            strs.AppendLine("{name:'" + dtSelect.Rows[i]["STATION_NAME"] + "',id:'" + dtSelect.Rows[i]["STATION_ID"] + "'}");
                        }
                        else
                        {
                            strs.AppendLine("{name:'" + dtSelect.Rows[i]["STATION_NAME"] + "',id:'" + dtSelect.Rows[i]["STATION_ID"] + "'},");
                        }
                    }
                }
                else
                {
                    strs.AppendLine("{name:'none',id:'none'},");
                }
                strs.AppendLine("]");
                string js = "setUl(" + strs.ToString() + ");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        /// 卡口模糊查询选中的时候给Session["tree"]赋值
        /// </summary>
        [DirectMethod]
        public void SetSession()
        {
            if (Session["tree"] != null)
            {
                Session["tree"] = null;
            }
            Session["tree"] = kakouId.Value;
        }

        #endregion 卡口选择
    }
}