using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class LogError : System.Web.UI.Page
    {
        #region 成员变量

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private static string starttime = "";
        private static string endtime = "";
        private UserLogin userLogin = new UserLogin();

        #endregion 成员变量

        #region 事件

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
                string js = "alert('" + GetLangStr("LogError9", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                try
                {
                    //ButCsv.Disabled = true;
                    ButExcel.Disabled = true;
                    // ButXml.Disabled = true;
                    //ButPrint.Disabled = true;
                    DataSetDateTime();

                    this.DataBind();
                    TbutQueryClick(null, null);
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex);

                    logManager.InsertLogError("LogError.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
                }
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("LogError10", "访问：错误日志查询"), userinfo.NowIp, "0");
                this.DataBind();
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
                this.StoreRunning.DataSource = GetData();
                this.StoreRunning.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogError.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
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
        /// 显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowDetails(object sender, DirectEventArgs e)
        {
            try
            {
                string sdata = e.ExtraParams["data"];
                Window win = WindowShow.AddLogErrInfo(sdata);
                win.Render(this.Form);
                win.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogError.aspx-ShowDetails", ex.Message + "；" + ex.StackTrace, "ShowDetails has an exception");
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
                logManager.InsertLogError("LogError.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh has an exception");
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
                logManager.InsertLogError("LogError.aspx-ToXml", ex.Message + "；" + ex.StackTrace, "ToXml has an exception");
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
                logManager.InsertLogError("LogError.aspx-ToExcel", ex.Message + "；" + ex.StackTrace, "ToExcel has an exception");
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
                logManager.InsertLogError("LogError.aspx-ToCsv", ex.Message + "；" + ex.StackTrace, "ToCsv has an exception");
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
                    string xml = Bll.Common.GetPrintXml(GetLangStr("LogError11", "错误日志查询列表"), "", "", "printdatatable");
                    string js = "OpenPrintPageV(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogError.aspx-ButPrintClick", ex.Message + "；" + ex.StackTrace, "ButPrintClick has an exception");
            }
        }

        #endregion 事件

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
                logManager.InsertLogError("LogError.aspx-GetDateTime", ex.Message + "；" + ex.StackTrace, "GetDateTime has an exception");
            }
        }

        #endregion [DirectMethod]

        #region 私有方法

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
            string startTime = starttime;
            string endTime = endtime;
            try
            {
                DataTable dt = logManager.GetLogError(startTime, endTime);
                Session["datatable"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    //ButCsv.Disabled = false;
                    ButExcel.Disabled = false;
                    //ButXml.Disabled = false;
                    //ButPrint.Disabled = false;
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
                logManager.InsertLogError("LogError.aspx-GetData", ex.Message + "；" + ex.StackTrace, "GetData has an exception");
                ILog.WriteErrorLog(ex);
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
                    dt2.Columns.Remove("col0");
                    dt2.Columns[0].ColumnName = GetLangStr("LogError5", "错误源");
                    dt2.Columns[1].ColumnName = GetLangStr("LogError6", "错误信息");
                    dt2.Columns[2].ColumnName = GetLangStr("LogError7", "产生日期");
                    dt2.Columns[3].ColumnName = GetLangStr("LogError8", "错误描述");

                    //PrintColumns pc = new PrintColumns();
                    //pc.Add(new PrintColumn("错误源", 0));
                    //pc.Add(new PrintColumn("错误信息", 1));
                    //pc.Add(new PrintColumn("产生日期", 2));
                    //pc.Add(new PrintColumn("错误描述", 3));
                    //dt2 = Bll.Common.GetDataTablePrint(dt, pc);
                }
                return dt2;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogError.aspx-ChangeDataTable", ex.Message + "；" + ex.StackTrace, "ChangeDataTable has an exception");
                return null;
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