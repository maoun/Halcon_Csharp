using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;
using System.Data;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PeccancyOperateAnalysis : System.Web.UI.Page
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
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!IsPostBack)
            {
                if (!X.IsAjaxRequest)
                {
                    try
                    {
                        StoreDataBind();
                        DataSetDateTime();
                        this.DataBind();
                        TbutQueryClick(null, null);
                        UserInfo userinfo = Session["Userinfo"] as UserInfo;
                        logManager.InsertLogRunning(userinfo.UserName, "访问：运行日志查询", userinfo.NowIp, "0");
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
                string sdata = e.ExtraParams["data"];
                string czyh = Bll.Common.GetdatabyField(sdata, "col1");//操作用户
                string kssj = starttime;//开始时间
                string jssj = endtime;//结束时间
                string czlx = Bll.Common.GetdatabyField(sdata, "col2");//操作类型
                string hphm = Bll.Common.GetdatabyField(sdata, "col4");//号牌号码
                string cishu = Bll.Common.GetdatabyField(sdata, "col5");//次数
                List<string> list = new List<string>();
                list.Add(czyh); list.Add(kssj); list.Add(jssj);
                list.Add(czlx); list.Add(hphm); list.Add(cishu);
                if (Session["LogXiangxi"] != null)
                {
                    Session["LogXiangxi"] = null;
                }
                Session["LogXiangxi"] = list;//存日志详细
                string js = "OpenPeccancyOperateLog();";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
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
                DataTable dt = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240027"));
                if (dt == null || dt.Rows.Count <= 0)
                {
                    dt = logManager.GetLogType("00");
                }
                if (dt.Rows.Count > 0)
                {
                    dt.Rows.RemoveAt(0);
                    dt.Rows.RemoveAt(0);
                    dt.Rows.RemoveAt(0);
                    dt.Rows.RemoveAt(0);
                }

                StoreQuery.DataSource = dt;
                StoreQuery.DataBind();

                UserManager user = new UserManager();

                StoreCzyh.DataSource = user.GetAllUserName();
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
                DataTable dt = logManager.GetLogRunning(starttime, endtime, CmbQueryType.SelectedItem.Value, cmbCzyh.SelectedItem.Value, txtCishu.Text);
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
                    dt2.Columns.Remove("col2"); dt2.Columns.Remove("col6");
                    dt2.Columns[0].ColumnName = GetLangStr("LogRunning7", "记录编号");
                    dt2.Columns[1].ColumnName = GetLangStr("LogRunning8", "操作用户");
                    dt2.Columns[2].ColumnName = "操作类型";
                    dt2.Columns[3].ColumnName = "号牌号码";
                    dt2.Columns[4].ColumnName = "次数";

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