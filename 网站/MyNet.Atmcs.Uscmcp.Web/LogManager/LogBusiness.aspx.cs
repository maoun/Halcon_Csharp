using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Atmcs.Uscmcp.Web.LogManager;
using MyNet.Common.Log;
using System;
using System.Data;
using System.Xml;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class LogBusiness : System.Web.UI.Page
    {
        #region 成员变量

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private static string starttime = "";
        private static string endtime = "";
        private UserLogin userLogin = new UserLogin();
        private static LogService.BigDataLogService clientLog = new LogService.BigDataLogService();
        private DataTable dtBusiness = null;

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
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                try
                {
                    StoreQuery.DataSource = GetRedisData.GetData("t_sys_code:240027");
                    StoreQuery.DataBind();
                    //this.StoreQuery.DataSource = logManager.GetLogType(SystemID);
                    //this.StoreQuery.DataBind();
                    //ButCsv.Disabled = true;
                    ButExcel.Disabled = true;
                    DataSetDateTime();
                    //ButXml.Disabled = true;
                    //ButPrint.Disabled = true;
                    this.DataBind();
                    TbutQueryClick(null, null);
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex);
                    logManager.InsertLogError("LogBusiness.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
                }
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "访问：业务日志查询", userinfo.NowIp, "0");
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
                logManager.InsertLogError("LogBusiness.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick发生异常");
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
                // string filepath = Server.MapPath("./data/QQWry.Dat");
                //Window win = WindowShow.AddLogInfo(sdata);
                //win.Render(this.Form);
                //win.Show();
                winDetails.Hidden = false;
                txtUserIp.Text = Bll.Common.GetdatabyField(sdata, "col1");
                // txtUserId.Text = Bll.Common.GetdatabyField(sdata, "col2");
                txtUserNmae.Text = Bll.Common.GetdatabyField(sdata, "col3");
                // txtFunctionId.Text = Bll.Common.GetdatabyField(sdata, "col4");
                txtFunctionName.Text = Bll.Common.GetdatabyField(sdata, "col5");
                txtUserParameter.Text = Bll.Common.GetdatabyField(sdata, "col6");

                //txtServiceId.Text = Bll.Common.GetdatabyField(sdata, "col7");
                txtServiceName.Text = Bll.Common.GetdatabyField(sdata, "col8");
                txtServiceDz.Text = Bll.Common.GetdatabyField(sdata, "col9");
                txtServiceInterfaceName.Text = Bll.Common.GetdatabyField(sdata, "col10");
                txaResult.Text = Bll.Common.GetdatabyField(sdata, "col11");
                txtJlsj.Text = Bll.Common.GetdatabyField(sdata, "col12");
                winDetails.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogBusiness.aspx-ShowDetails", ex.Message + "；" + ex.StackTrace, "ShowDetails发生异常");
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
                logManager.InsertLogError("LogBusiness.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh发生异常");
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
                logManager.InsertLogError("LogBusiness.aspx-ToXml", ex.Message + "；" + ex.StackTrace, "ToXml发生异常");
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
                logManager.InsertLogError("LogBusiness.aspx-ToExcel", ex.Message + "；" + ex.StackTrace, "ToExcel发生异常");
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
                logManager.InsertLogError("LogBusiness.aspx-ToCsv", ex.Message + "；" + ex.StackTrace, "ToCsv发生异常");
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
                    string xml = Bll.Common.GetPrintXml(GetLangStr("LogBusiness17", "运行日志查询列表"), "", "", "printdatatable");
                    string js = "OpenPrintPageV(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogBusiness.aspx-ButPrintClick", ex.Message + "；" + ex.StackTrace, "ButPrintClick发生异常");
            }
        }

        #endregion 控件事件

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
                logManager.InsertLogError("LogBusiness.aspx-GetDateTime", ex.Message + "；" + ex.StackTrace, "GetDateTime发生异常");
            }
        }

        #endregion [DirectMethod]

        #region 私有方法

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
                logManager.InsertLogError("LogBusiness.aspx-DataSetDateTime", ex.Message + "；" + ex.StackTrace, "DataSetDateTime发生异常");
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
                logManager.InsertLogError("LogBusiness.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice发生异常");
            }
        }

        /// <summary>
        /// 获得查询数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetData()
        {
            try
            {
                //DataTable dt = logManager.GetLogBusiness(starttime, endtime, CmbQueryType.SelectedItem.Value);
                dtBusiness = CreateBusinessTable();
                //获得ip地址
                string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                if (ipaddress.Length < 9)
                {
                    ipaddress = "10.2.111.133";
                }
                string usercode = (Session["userinfo"] as UserInfo).UserCode;
                string funcid = LogMain.dyzgnmkbh;
                string xml = Bll.Common.GetBusinessXml(starttime, endtime, "", "", "", "0", "10");
                string rexml = clientLog.BigdataSearchLog(xml);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(rexml);
                int allNum = Bll.Common.GetBusinessRowCount(xmlDoc);
                if (!string.IsNullOrEmpty(rexml) && allNum > 0)
                {
                    CXmlToDataTable(xmlDoc);
                }
                if (dtBusiness != null && dtBusiness.Rows.Count > 0)
                {
                    this.StoreRunning.DataSource = Bll.Common.ChangColName(dtBusiness);
                    this.StoreRunning.DataBind();
                }

                Session["datatable"] = dtBusiness;
                if (dtBusiness != null && dtBusiness.Rows.Count > 0)
                {
                    // ButCsv.Disabled = false;
                    ButExcel.Disabled = false;
                    //ButXml.Disabled = false;
                    // ButPrint.Disabled = false;
                }
                else
                {
                    ButExcel.Disabled = true;
                    Notice("提示", "未查询到相关记录!");
                }
                return dtBusiness;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogBusiness.aspx-GetData", ex.Message + "；" + ex.StackTrace, "GetData发生异常");
                return null;
            }
        }

        /// <summary>
        /// 过车记录转换为datatable
        /// </summary>
        /// <param name="xmlStr"></param>
        public void CXmlToDataTable(XmlDocument xmlDoc)
        {
            XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/Loglist/Loginfo");
            try
            {
                int i = 0;
                foreach (XmlNode node in listNodes)
                {
                    //                    <DYZYHIP>调用者用户IP</DYZYHIP>
                    //          <DYZYHID>调用者用户ID</DYZYHID>
                    //                <DYZYHMC>调用者用户名称</DYZYHMC>
                    //              <DYZGNMKBH>调用者功能模块编号</DYZGNMKBH>
                    //            <DYZGNMKMC>调用者功能模块名称</DYZGNMKMC>
                    //<DYZCC>调用者传参</DYZCC>
                    //<BDYZFWBH>被调用者服务编号</BDYZFWBH>
                    //<BDYZFWMC>被调用者服务名称</BDYZFWMC>
                    //          <BDYZFWDZ>被调用者服务地址</BDYZFWDZ>
                    //          <BDYZFWJKMC>被调用者服务接口名称</BDYZFWJKMC>
                    //            <BDYZFHJG>被调用者返回结果</BDYZFHJG>
                    //          <JLSJ>记录时间</JLSJ>
                    i++;
                    DataRow dr = dtBusiness.NewRow();
                    dr["xh"] = i.ToString();

                    dr["DYZYHIP"] = (node.SelectSingleNode("DYZYHIP")).InnerText;
                    dr["DYZYHID"] = (node.SelectSingleNode("DYZYHID")).InnerText;
                    dr["DYZYHMC"] = (node.SelectSingleNode("DYZYHMC")).InnerText;
                    dr["DYZGNMKBH"] = (node.SelectSingleNode("DYZGNMKBH")).InnerText;
                    dr["DYZGNMKMC"] = (node.SelectSingleNode("DYZGNMKMC")).InnerText;

                    dr["DYZCC"] = (node.SelectSingleNode("DYZCC")).InnerText;
                    dr["BDYZFWBH"] = (node.SelectSingleNode("BDYZFWBH")).InnerText;
                    dr["BDYZFWMC"] = (node.SelectSingleNode("BDYZFWMC")).InnerText;
                    dr["BDYZFWDZ"] = (node.SelectSingleNode("BDYZFWDZ")).InnerText;
                    dr["BDYZFWJKMC"] = (node.SelectSingleNode("BDYZFWJKMC")).InnerText;

                    dr["BDYZFHJG"] = (node.SelectSingleNode("BDYZFHJG")).InnerText;
                    dr["JLSJ"] = (node.SelectSingleNode("JLSJ")).InnerText;
                    dtBusiness.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 创建内存表
        /// </summary>
        /// <returns></returns>
        private DataTable CreateBusinessTable()
        {
            DataTable dt = new DataTable("Business");
            dt.Columns.Add("xh", typeof(string));//序号
            dt.Columns.Add("DYZYHIP", typeof(string));//调用者用户IP
            dt.Columns.Add("DYZYHID", typeof(string));//调用者用户ID
            dt.Columns.Add("DYZYHMC", typeof(string));//调用者用户名称
            dt.Columns.Add("DYZGNMKBH", typeof(string));//调用者功能模块编号
            dt.Columns.Add("DYZGNMKMC", typeof(string));//调用者功能模块名称
            dt.Columns.Add("DYZCC", typeof(string));//调用者传参
            dt.Columns.Add("BDYZFWBH", typeof(string));//被调用者服务编号
            dt.Columns.Add("BDYZFWMC", typeof(string));//被调用者服务名称
            dt.Columns.Add("BDYZFWDZ", typeof(string));//被调用者服务地址
            dt.Columns.Add("BDYZFWJKMC", typeof(string));//被调用者服务接口名称
            dt.Columns.Add("BDYZFHJG", typeof(string));//被调用者返回结果
            dt.Columns.Add("JLSJ", typeof(string));//记录时间
            return dt;
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
                DataTable dt1 = dt.Copy();
                if (dt != null)
                {
                    dt1.Columns.Remove("col2"); dt1.Columns.Remove("col4"); dt1.Columns.Remove("col6"); dt1.Columns.Remove("col7");
                    dt1.Columns.Remove("col8"); dt1.Columns.Remove("col11");
                    dt1.Columns["col0"].SetOrdinal(0);
                    dt1.Columns["col1"].SetOrdinal(3);
                    dt1.Columns["col3"].SetOrdinal(1);
                    dt1.Columns["col5"].SetOrdinal(2);
                    dt1.Columns["col9"].SetOrdinal(4);
                    dt1.Columns["col10"].SetOrdinal(5);
                    dt1.Columns["col12"].SetOrdinal(6);
                    dt1.Columns[0].ColumnName = GetLangStr("LogBusiness7", "记录编号");
                    dt1.Columns[1].ColumnName = GetLangStr("LogBusiness8", "操作用户");
                    dt1.Columns[2].ColumnName = GetLangStr("LogBusiness9", "操作事件");
                    dt1.Columns[3].ColumnName = GetLangStr("LogBusiness10", "用户IP");
                    dt1.Columns[4].ColumnName = GetLangStr("LogBusiness11", "接口地址");
                    dt1.Columns[5].ColumnName = GetLangStr("LogBusiness12", "接口名称");
                    dt1.Columns[6].ColumnName = GetLangStr("LogBusiness13", "记录时间");

                    //PrintColumns pc = new PrintColumns();
                    //pc.Add(new PrintColumn("记录编号", 0));
                    //pc.Add(new PrintColumn("系统名称", 7));
                    //pc.Add(new PrintColumn("操作用户", 2));
                    //pc.Add(new PrintColumn("操作事件", 3));
                    //pc.Add(new PrintColumn("用户IP", 4));
                    //pc.Add(new PrintColumn("记录时间", 5));
                    //dt2 = Bll.Common.GetDataTablePrint(dt, pc);
                }

                return dt1;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogBusiness.aspx-ChangeDataTable", ex.Message + "；" + ex.StackTrace, "ChangeDataTable发生异常");
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
            try
            {
                string className = this.GetType().BaseType.FullName;
                return MyNet.Common.Lang.Language.CreateInstance(className).GetLanguageStr(value, desc, className);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LogBusiness.aspx-GetLangStr", ex.Message + "；" + ex.StackTrace, "GetLangStr发生异常");
                throw;
            }
        }

        #endregion 私有方法
    }
}