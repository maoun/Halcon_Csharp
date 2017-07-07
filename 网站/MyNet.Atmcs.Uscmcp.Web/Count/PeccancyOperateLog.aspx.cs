using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;
using System.Data;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PeccancyOperateLog : System.Web.UI.Page
    {
        #region 成员变量

        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private static string starttime = "";
        private static string endtime = "";
        private UserLogin userLogin = new UserLogin();

        /// <summary>
        /// 操作用户
        /// </summary>
        private static DataTable dtCzyh = null;

        /// <summary>
        /// 操作类型
        /// </summary>
        private static DataTable dtCzlx = null;

        #endregion 成员变量

        #region 事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //string username = Request.QueryString["username"];
            //if (!userLogin.CheckLogin(username))
            //{
            //    string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
            //    System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
            //    return;
            //}
            if (!IsPostBack)
            {
                if (!X.IsAjaxRequest)
                {
                    try
                    {
                        StoreDataBind();
                        //DataSetDateTime();
                        List<string> list = (List<string>)Session["LogXiangxi"];
                        cmbCzyh.Value = list[0];
                        CmbQueryType.Value = list[3];
                        start.InnerText = starttime = list[1];
                        end.InnerText = endtime = list[2];
                        txtHphm.Text = list[4];
                        this.DataBind();
                        TbutQueryClick(null, null);
                        UserInfo userinfo = Session["Userinfo"] as UserInfo;
                        logManager.InsertLogRunning(userinfo.UserName, "访问：" + Request.QueryString["funcname"], userinfo.NowIp, "0");
                    }
                    catch (Exception ex)
                    {
                        ILog.WriteErrorLog(ex);
                        logManager.InsertLogError("LogRunning.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
                    }
                }
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
                this.StoreRunning.DataSource = GetData();
                this.StoreRunning.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick发生异常");
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
                string wfid = Bll.Common.GetdatabyField(data, "col8");
                DataTable dt = null;
                if (!string.IsNullOrEmpty(wfid))
                {
                    dt = tgsDataInfo.GetPeccancyInfo(" xh='" + wfid + "' ", 0, 15);
                    if (dt != null)
                    {
                        Window win = WindowShow.AddPeccancy(dt.Rows[0]);
                        win.Render(this.Form);
                        win.Show();
                    }
                    else
                    {
                        Notice("信息提示提示", "没查询到违法信息");
                        return;
                    }
                }
                else
                {
                    Notice("信息提示提示", "没查询到违法信息");
                    return;
                }

                //string sdata = e.ExtraParams["data"];
                //string filepath = Server.MapPath("./data/QQWry.Dat");
                //Window win = WindowShow.AddLogInfo(sdata, filepath);
                //win.Render(this.Form);
                //win.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-ShowDetails", ex.Message + "；" + ex.StackTrace, "ShowDetails发生异常");
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
                this.StoreRunning.DataSource = GetData();
                this.StoreRunning.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh发生异常");
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
                logManager.InsertLogError("LogRunning.aspx-ToXml", ex.Message + "；" + ex.StackTrace, "ToXml发生异常");
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
                logManager.InsertLogError("LogRunning.aspx-ToExcel", ex.Message + "；" + ex.StackTrace, "ToExcel发生异常");
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
                logManager.InsertLogError("LogRunning.aspx-ToCsv", ex.Message + "；" + ex.StackTrace, "ToCsv发生异常");
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
                    string xml = Bll.Common.GetPrintXml(GetLangStr("LogRunning17", "运行日志查询列表"), "", "", "printdatatable");
                    string js = "OpenPrintPageV(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-ButPrintClick", ex.Message + "；" + ex.StackTrace, "ButPrintClick发生异常");
            }
        }

        #endregion 事件

        #region 私有方法

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                DataTable dt = dtCzlx = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240027"));
                if (dt == null || dt.Rows.Count <= 0)
                {
                    dt = dtCzlx = logManager.GetLogType("00");
                }
                StoreQuery.DataSource = dt;
                StoreQuery.DataBind();

                UserManager user = new UserManager();

                StoreCzyh.DataSource = dtCzyh = user.GetAllUserName();
                StoreCzyh.DataBind();

                ButExcel.Disabled = true;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind发生异常");
            }
        }

        /// <summary>
        ///提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Notice(string title, string msg)
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

        /// <summary>
        /// 获得查询数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetData()
        {
            try
            {
                DataTable dt = logManager.GetLogRunning(starttime, endtime, CmbQueryType.SelectedItem.Value, cmbCzyh.SelectedItem.Value, "", txtHphm.Text);
                Session["datatable"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    ButExcel.Disabled = false;
                }
                else
                {
                    ButExcel.Disabled = true;
                    Notice("提示", "未查询到相关记录!");
                }
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-GetData", ex.Message + "；" + ex.StackTrace, "GetData发生异常");
                return null;
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
                DataTable dt2 = dt.Copy();
                if (dt != null)
                {
                    dt2.Columns.Remove("col5");
                    dt2.Columns["col0"].SetOrdinal(0);
                    dt2.Columns["col1"].SetOrdinal(1);
                    dt2.Columns["col6"].SetOrdinal(2);
                    dt2.Columns["col2"].SetOrdinal(3);
                    dt2.Columns["col3"].SetOrdinal(4);
                    dt2.Columns["col4"].SetOrdinal(5);
                    dt2.Columns["col7"].SetOrdinal(6);
                    dt2.Columns[0].ColumnName = GetLangStr("LogRunning7", "记录编号");
                    dt2.Columns[1].ColumnName = GetLangStr("LogRunning8", "操作用户");
                    dt2.Columns[2].ColumnName = "操作类型";
                    dt2.Columns[3].ColumnName = GetLangStr("LogRunning9", "操作事件");
                    dt2.Columns[4].ColumnName = GetLangStr("LogRunning10", "用户IP");
                    dt2.Columns[5].ColumnName = GetLangStr("LogRunning11", "记录时间");
                    dt2.Columns[6].ColumnName = "号牌号码";

                    //PrintColumns pc = new PrintColumns();
                    //pc.Add(new PrintColumn("记录编号", 0));
                    //pc.Add(new PrintColumn("系统名称", 7));
                    //pc.Add(new PrintColumn("操作用户", 2));
                    //pc.Add(new PrintColumn("操作事件", 3));
                    //pc.Add(new PrintColumn("用户IP", 4));
                    //pc.Add(new PrintColumn("记录时间", 5));
                    //dt2 = Bll.Common.GetDataTablePrint(dt, pc);
                }

                return dt2;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogRunning.aspx-ChangeDataTable", ex.Message + "；" + ex.StackTrace, "ChangeDataTable发生异常");
                return null;
            }
        }

        #endregion 私有方法

        #region [DirectMethod]

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
                logManager.InsertLogError("LogRunning.aspx-GetDateTime", ex.Message + "；" + ex.StackTrace, "GetDateTime发生异常");
            }
        }

        #endregion [DirectMethod]

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