using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class ServerManager : System.Web.UI.Page
    {
        #region 成员变量

        private SettingManager settingManager = new SettingManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private SystemManager systemManager = new SystemManager();
        private MyNet.Atmcs.Uscmcp.Bll.DeviceManager deviceManager = new MyNet.Atmcs.Uscmcp.Bll.DeviceManager();
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        /// <summary>
        /// 登录名
        /// </summary>
        static string uName;
        /// <summary>
        /// 主机ip
        /// </summary>
        static string nowIp;
        /// <summary>
        /// 设备名称
        /// </summary>
        static string tabname = "";
        /// <summary>
        /// 服务器类型
        /// </summary>
        static string fwqlx;
        /// <summary>
        /// 服务器名称
        /// </summary>
        static string fwqmc;
        /// <summary>
        /// 服务器型号
        /// </summary>
        static string fwqxh;
        /// <summary>
        /// 服务器通道数
        /// </summary>
        static string fwqtds;
        /// <summary>
        /// 是否使用
        /// </summary>
        static string sfsy;
        /// <summary>
        /// 服务器ip
        /// </summary>
        static string fwqip;
        /// <summary>
        /// 服务器端口
        /// </summary>
        static string fwqdk;
        /// <summary>
        /// 通讯类型
        /// </summary>
        static string txlx;
        /// <summary>
        /// 通讯协议
        /// </summary>
        static string txxy;
        /// <summary>
        /// 用户名
        /// </summary>
        static string yhm;
        /// <summary>
        /// 密码
        /// </summary>
        static string mm;

        /// <summary>
        /// 建设单位
        /// </summary>
        static string jsdw;
        /// <summary>
        /// 维护单位
        /// </summary>
        static string whdw;
        /// <summary>
        /// 设备制造厂商
        /// </summary>
        static string sbzzcs;

        /// <summary>
        /// 访问ip
        /// </summary>
        static string nowid;
        /// <summary>
        /// 获取服务器类型
        /// </summary>
        private static DataTable dtServer = null;
        /// <summary>
        /// 获取服务器型号
        /// </summary>
        private static DataTable dtType = null;
        /// <summary>
        /// 获取通讯类型
        /// </summary>
        private static DataTable dtTxlx = null;
        /// <summary>
        /// 获取协议
        /// </summary>
        private static DataTable dtTxxy = null;
        /// <summary>
        /// 是否使用
        /// </summary>
        private static DataTable dtSfsy = null;

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
                string js = "alert('" + GetLangStr("ServerManager32", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                BuildTree(TreePanel1.Root);
                ButExcel.Disabled = true;
                //ButPrint.Disabled = true;
                StoreDataBind();

                this.DataBind();

                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                uName = userinfo.UserName;
                nowIp = userinfo.NowIp;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("ServerManager13", "访问：服务器信息管理"), userinfo.NowIp, "0");
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
                LedDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        /// <summary>
        ///  刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButRefreshClick(object sender, DirectEventArgs e)
        {
            try
            {
                LedDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-ButRefreshClick", ex.Message + "；" + ex.StackTrace, "ButRefreshClick has an exception");
            }
        }

        /// <summary>
        ///  刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyData_Refresh(object sender, DirectEventArgs e)
        {
            try
            {
                DataTable dt = deviceManager.GetServer(" 1=1 ");
                StoreDeviceManager.DataSource = dt;

                StoreDeviceManager.DataBind();

                Session["datatable"] = dt;
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        ButExcel.Disabled = false;
                        //ButPrint.Disabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh has an exception");
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
                this.ResourceManager1.RegisterAfterClientInitScript("clearTime();");
                CmbDeviceType.Reset();
                TxtDeviceName.Reset();
                CmbSBXH.Reset();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-ButResetClick", ex.Message + "；" + ex.StackTrace, "ButResetClick has an exception");
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
                LedDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh has an exception");
            }
        }

        /// <summary>
        /// 选中设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectDev_Click(object sender, DirectEventArgs e)
        {
            string id = e.ExtraParams["id"];

        }

        protected void ButDevAdd_Click(object sender, DirectEventArgs e)
        {
            try
            {
                HidSaveFlag.Value = "1";
                Button5.Hidden = false;
                Text_SBBH.Text = tgsPproperty.GetRecordId();
                Cob_SBLX.Reset();
                Text_SBMC.Reset();
                Cob_SBXH.Reset();
                Cob_SFSY.Reset();
                Text_IP.Reset();
                Text_DK.Reset();
                Text_TYDS.Reset();
                Cob_TXLX.Reset();
                Cob_TXXY.Reset();
                Text_YHM.Reset();
                Text_MM.Reset();
                Cob_JSDW.Reset();
                Cob_ZZCS.Reset();
                Cob_WHDW.Reset();
                Window4.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-ButDevAdd_Click", ex.Message + "；" + ex.StackTrace, "ButDevAdd_Click has an exception");
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
                    string xml = Bll.Common.GetPrintXml(GetLangStr("ServerManager54", "服务器信息管理"), "", "", "printdatatable");
                    string js = "OpenPrintPageV(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-ButPrintClick", ex.Message + "；" + ex.StackTrace, "ButPrintClick has an exception");
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
                logManager.InsertLogError("ServerManager.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
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
                logManager.InsertLogError("ServerManager.aspx-ToExcel", ex.Message + "；" + ex.StackTrace, "ToExcel has an exception");
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
                logManager.InsertLogError("ServerManager.aspx-ToCsv", ex.Message + "；" + ex.StackTrace, "ToCsv has an exception");
            }
        }

        /// <summary>
        ///  更新设备信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateDevice(object sender, DirectEventArgs e)
        {
            try
            {
                Hashtable hs = new Hashtable();
                string server_id = string.Empty;
                if (!string.IsNullOrEmpty(Text_SBBH.Text))
                {
                    server_id = Text_SBBH.Text.ToString();
                }
                else
                {
                    if (HidSaveFlag.Value.ToString() == "1")
                    {
                        server_id = tgsPproperty.GetMinRecordId();
                    }
                    if (HidSaveFlag.Value.ToString() == "2")
                    {
                        server_id = HiddenId.Value.ToString();
                    }
                }

                hs.Add("server_id", server_id);
                hs.Add("server_idext", "");

                if (Cob_SBLX.SelectedIndex != -1)
                {
                    hs.Add("server_type_id", Cob_SBLX.Value.ToString());
                }
                else
                {
                    hs.Add("server_type_id", "");
                }
                if (Cob_SBXH.SelectedIndex != -1)
                {
                    hs.Add("server_mode_id", Cob_SBXH.Value.ToString());
                }
                else
                {
                    hs.Add("server_mode_id", "");
                }
                hs.Add("server_name", Text_SBMC.Text);

                if (Cob_SFSY.SelectedIndex != -1)
                {
                    hs.Add("isuse", Cob_SFSY.Value.ToString());
                }
                else
                {
                    hs.Add("isuse", "0");
                }
                hs.Add("ipaddress", Text_IP.Text);

                hs.Add("port", Text_DK.Text);

                hs.Add("channels", Text_TYDS.Text.ToString());

                if (Cob_TXLX.SelectedIndex != -1)
                {
                    hs.Add("comm_type_id", Cob_TXLX.Value.ToString());
                }
                else
                {
                    hs.Add("comm_type_id", "");
                }
                if (Cob_TXXY.SelectedIndex != -1)
                {
                    hs.Add("protocol_id", Cob_TXXY.Value.ToString());
                }
                else
                {
                    hs.Add("protocol_id", "");
                }
                hs.Add("username", Text_YHM.Text);
                hs.Add("password", Text_MM.Text);
                if (Cob_JSDW.SelectedIndex != -1)
                {
                    hs.Add("build_id", Cob_JSDW.Value.ToString());
                }
                else
                {
                    hs.Add("build_id", "");
                }
                if (Cob_ZZCS.SelectedIndex != -1)
                {
                    hs.Add("make_company_id", Cob_ZZCS.Value.ToString());
                }
                else
                {
                    hs.Add("make_company_id", "");
                }
                if (Cob_WHDW.SelectedIndex != -1)
                {
                    hs.Add("maintain_company_id", Cob_WHDW.Value.ToString());
                }
                else
                {
                    hs.Add("maintain_company_id", "");
                }
                hs.Add("jkxlh", "");
                if (deviceManager.UptateServerInfo(hs) > 0)
                {
                    if (HidSaveFlag.Value != null)
                    {
                        if (HidSaveFlag.Value.ToString() == "1")
                        {
                            Window4.Hide();
                            LedDataBind(" 1=1 and server_type_id='" + Cob_SBLX.Value.ToString() + "'");
                            string lblname = "";
                            lblname += Bll.Common.AssembleRunLog("", Cob_SBLX.SelectedItem.Text, GetLangStr("ServerManager2", "服务器类型"), "0");
                            lblname += Bll.Common.AssembleRunLog("", Cob_SBXH.SelectedItem.Text, GetLangStr("ServerManager4", "服务器型号"), "0");
                            lblname += Bll.Common.AssembleRunLog("", Text_TYDS.Text, GetLangStr("ServerManager42", "服务器通道数"), "0");
                            lblname += Bll.Common.AssembleRunLog("", Cob_SFSY.SelectedItem.Text, GetLangStr("ServerManager54", "是否使用"), "0");
                            lblname += Bll.Common.AssembleRunLog("", Cob_JSDW.SelectedItem.Text, GetLangStr("ServerManager46", "建设单位"), "0");
                            lblname += Bll.Common.AssembleRunLog("", Cob_WHDW.SelectedItem.Text, GetLangStr("ServerManager48", "维护单位"), "0");
                            lblname += Bll.Common.AssembleRunLog("", Cob_ZZCS.SelectedItem.Text, GetLangStr("ServerManager50", "设备制造厂商"), "0");

                            logManager.InsertLogRunning(uName, GetLangStr("ServerManager56", "添加：服务器[") + Text_SBMC.Text + "]" + lblname, nowIp, "1");
                            Notice(GetLangStr("ServerManager55", "信息提示"), GetLangStr("ServerManager58", "添加成功"));
                            HidSaveFlag.Value = null;
                            return;
                        }
                    }
                    Window4.Hide();
                    LedDataBind(" 1=1 and server_type_id='" + Cob_SBLX.Value.ToString() + "'");
                    Notice(GetLangStr("ServerManager55", "信息提示"), GetLangStr("ServerManager59", "修改成功"));

                    string fwqlxs = Cob_SBLX.SelectedItem.Text;
                    string fwqmcs = Text_SBMC.Text;
                    string fwqxhs = Cob_SBXH.SelectedItem.Text;
                    string fwqips = Text_IP.Text;
                    string fwqdks = Text_DK.Text;
                    string txlxs = Cob_TXLX.SelectedItem.Text;
                    string txxys = Cob_TXXY.SelectedItem.Text;
                    string yhms = Text_YHM.Text;
                    string mms = Text_MM.Text;
                    string fwqtdss = Text_TYDS.Text;
                    string sfsys = Cob_SFSY.SelectedItem.Text;
                    string jsdws = Cob_JSDW.SelectedItem.Text;
                    string whdws = Cob_WHDW.SelectedItem.Text;
                    string sbzzcss = Cob_ZZCS.SelectedItem.Text;
                    string lname = "";
                    lname += Bll.Common.AssembleRunLog(fwqlx, fwqlxs, GetLangStr("ServerManager2 ", "服务器类型"), "1");
                    lname += Bll.Common.AssembleRunLog(fwqmc, fwqmcs, GetLangStr("ServerManager1", "服务器名称"), "1");
                    lname += Bll.Common.AssembleRunLog(fwqxh, fwqxhs, GetLangStr("ServerManager4", "服务器型号"), "1");
                    lname += Bll.Common.AssembleRunLog(fwqip, fwqips, GetLangStr("ServerManager20", "服务器ip"), "1");
                    lname += Bll.Common.AssembleRunLog(fwqdk, fwqdks, GetLangStr("ServerManager21", "服务器端口"), "1");
                    lname += Bll.Common.AssembleRunLog(txlx, txlxs, GetLangStr("ServerManager36", "通讯类型"), "1");
                    lname += Bll.Common.AssembleRunLog(txxy, txxys, GetLangStr("ServerManager38", "通讯协议"), "1");
                    lname += Bll.Common.AssembleRunLog(yhm, yhms, GetLangStr("ServerManager40", "用户名"), "1");
                    lname += Bll.Common.AssembleRunLog(mm, mms, GetLangStr("ServerManager41", "密码"), "1");
                    lname += Bll.Common.AssembleRunLog(fwqtds, fwqtdss, GetLangStr("ServerManager42", "服务器通道数"), "1");
                    lname += Bll.Common.AssembleRunLog(sfsy, sfsys, GetLangStr("ServerManager57", "是否使用"), "1");
                    lname += Bll.Common.AssembleRunLog(jsdw, jsdws, GetLangStr("ServerManager46", "建设单位"), "1");
                    lname += Bll.Common.AssembleRunLog(whdw, whdws, GetLangStr("ServerManager48", "维护单位"), "1");
                    lname += Bll.Common.AssembleRunLog(sbzzcs, sbzzcss, GetLangStr("ServerManager50", "设备制造厂商"), "1");
                    logManager.InsertLogRunning(uName, GetLangStr("ServerManager62", "修改服务器：[") + fwqmc + "];" + lname, nowIp, "2");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-UpdateDevice", ex.Message + "；" + ex.StackTrace, "UpdateDevice has an exception");
            }
        }

        #endregion 控件事件

        #region DirectMethod 方法

        /// <summary>
        /// 选中设备树事件
        /// </summary>
        [DirectMethod]
        public void SelectDevice()
        {
            try
            {
                string type = Cob_SBLX.SelectedItem.Value;//CmbDeviceType.SelectedItem.Value;
                DataTable da = deviceManager.GetServerTypeMode("device_type_id='" + type + "'");
                CmbSBXH.Reset();
                Cob_SBXH.Reset();
                this.StoreSBXH.DataSource = da;
                this.StoreSBXH.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-SelectDevice", ex.Message + "；" + ex.StackTrace, "SelectDevice has an exception");
            }
        }

        /// <summary>
        /// 设备点击事件
        /// </summary>
        /// <param name="code"></param>
        [DirectMethod]
        public void SelectNode(string code)
        {
            try
            {
                LedDataBind(" 1=1 and server_type_id='" + code + "'");
                CmbDeviceType.Value = code;
                this.StoreSBXH.DataSource = deviceManager.GetServerTypeMode("device_type_id='" + code + "'");
                this.StoreSBXH.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-SelectNode", ex.Message + "；" + ex.StackTrace, "SelectNode has an exception");
            }
        }

        /// <summary>
        ///Grid列表双击事件
        /// </summary>
        [DirectMethod]
        public void RowDblClickShow()
        {
            try
            {
                RowSelectionModel sm = this.GridDeviceManager.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string id = sm.SelectedRow.RecordID;
                    Window4.Title = GetLangStr("ServerManager61", "查看服务器信息");
                    DataTable dt = deviceManager.GetServerInfo("server_id='" + id + "'");
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            Text_SBBH.Text = dt.Rows[0]["SERVER_ID"].ToString();
                            Cob_SBLX.Value = dt.Rows[0]["SERVER_TYPE_ID"].ToString();
                            Text_SBMC.Value = dt.Rows[0]["SERVER_NAME"].ToString();
                            Cob_SBXH.Value = dt.Rows[0]["SERVER_MODE_ID"].ToString();
                            Cob_SFSY.Value = dt.Rows[0]["ISUSE"].ToString();
                            Text_IP.Text = dt.Rows[0]["IPADDRESS"].ToString();
                            Text_DK.Text = dt.Rows[0]["PORT"].ToString();
                            Text_TYDS.Text = dt.Rows[0]["CHANNELS"].ToString();
                            Cob_TXLX.Text = dt.Rows[0]["COMM_TYPE_ID"].ToString();
                            Cob_TXXY.Text = dt.Rows[0]["PROTOCOL_ID"].ToString();
                            Text_YHM.Text = dt.Rows[0]["USERNAME"].ToString();
                            Text_MM.Text = dt.Rows[0]["PASSWORD"].ToString();
                            Cob_JSDW.Value = dt.Rows[0]["BUILD_ID"].ToString();
                            Cob_ZZCS.Value = dt.Rows[0]["MAKE_COMPANY_ID"].ToString(); ;
                            Cob_WHDW.Value = dt.Rows[0]["MAINTAIN_COMPANY_ID"].ToString();
                            Button5.Hidden = true;
                            Window4.Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-RowDblClickShow", ex.Message + "；" + ex.StackTrace, "RowDblClickShow has an exception");
            }
        }

        /// <summary>
        ///修改事件
        /// </summary>
        [DirectMethod]
        public void Modify()
        {
            try
            {
                RowSelectionModel sm = this.GridDeviceManager.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string id = sm.SelectedRow.RecordID;
                    HidSaveFlag.Value = "2";
                    Window4.Title = GetLangStr("ServerManager63", "修改服务器信息");
                    HiddenId.Value = id;
                    DataTable dt = deviceManager.GetServerInfo("server_id='" + id + "'");
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            Text_SBBH.Text = dt.Rows[0]["SERVER_ID"].ToString();
                            Cob_SBLX.Value = dt.Rows[0]["SERVER_TYPE_ID"].ToString();
                            Text_SBMC.Value = dt.Rows[0]["SERVER_NAME"].ToString();

                            DataTable da = deviceManager.GetServerTypeMode("device_type_id='" + Cob_SBLX.Value.ToString() + "'");
                            Cob_SBXH.Reset();
                            this.StoreSBXH.DataSource = da;
                            this.StoreSBXH.DataBind();
                            Cob_SBXH.Value = dt.Rows[0]["SERVER_MODE_ID"].ToString();
                            Cob_SFSY.Value = dt.Rows[0]["ISUSE"].ToString();
                            Text_IP.Text = dt.Rows[0]["IPADDRESS"].ToString();
                            Text_DK.Text = dt.Rows[0]["PORT"].ToString();
                            Text_TYDS.Text = dt.Rows[0]["CHANNELS"].ToString();
                            Cob_TXLX.Text = dt.Rows[0]["COMM_TYPE_ID"].ToString();
                            Cob_TXXY.Text = dt.Rows[0]["PROTOCOL_ID"].ToString();
                            Text_YHM.Text = dt.Rows[0]["USERNAME"].ToString();
                            Text_MM.Text = dt.Rows[0]["PASSWORD"].ToString();
                            Cob_JSDW.Value = dt.Rows[0]["BUILD_ID"].ToString();
                            Cob_ZZCS.Value = dt.Rows[0]["MAKE_COMPANY_ID"].ToString(); ;
                            Cob_WHDW.Value = dt.Rows[0]["MAINTAIN_COMPANY_ID"].ToString();
                            Button5.Hidden = false;
                            Window4.Show();
                            if (dtServer != null)
                            {
                                DataRow[] rows = dtServer.Select("col0='" + dt.Rows[0]["SERVER_TYPE_ID"].ToString() + "'");
                                if (rows.Length > 0)
                                {
                                    fwqlx = rows[0]["col1"].ToString();
                                }
                            }

                            if(dtTxlx!=null){
                                DataRow[] dr = dtTxlx.Select("col1='" + dt.Rows[0]["COMM_TYPE_ID"].ToString() + "'");
                                if (dr.Length > 0) {
                                    txlx = dr[0]["col2"].ToString();
                                }
                            }

                            if (dtTxxy != null)
                            {
                                DataRow[] dd = dtTxxy.Select("col1='" + dt.Rows[0]["PROTOCOL_ID"].ToString() + "'");
                                if (dd.Length > 0)
                                {
                                    txxy = dd[0]["col2"].ToString();
                                }
                            }

                            if (dtSfsy != null)
                            {
                                DataRow[] dl = dtSfsy.Select("col1='" + dt.Rows[0]["ISUSE"].ToString() + "'");
                                if (dl.Length > 0)
                                {
                                    sfsy = dl[0]["col2"].ToString();
                                }
                            }
                            fwqmc = Text_SBMC.Text;
                            fwqxh = da.Rows[0]["col3"].ToString();
                            fwqip = Text_IP.Text;
                            fwqdk = Text_DK.Text;
                            yhm = Text_YHM.Text;
                            mm = Text_MM.Text;
                            fwqtds = Text_TYDS.Text;
                            jsdw = Cob_JSDW.SelectedItem.Text;
                            whdw = Cob_WHDW.SelectedItem.Text;
                            sbzzcs = Cob_ZZCS.SelectedItem.Text;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-Modify", ex.Message + "；" + ex.StackTrace, "Modify has an exception");
            }
        }

        /// <summary>
        /// 删除确认事件
        /// </summary>
        [DirectMethod]
        public void DoConfirm()
        {
            try
            {
                RowSelectionModel sm = this.GridDeviceManager.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string Id = sm.SelectedRow.RecordID;
                    nowid = Id;
                    DataRow[] rows = (Session["datatable"] as DataTable).Select("col0=" + nowid);
                    if (rows.Length > 0)
                    {
                        tabname = rows[0]["col1"].ToString();
                    }

                    X.Msg.Confirm(GetLangStr("ServerManager64", "信息"), GetLangStr("ServerManager65", "这样会删除服务器信息，确认要删除[") + tabname + "]？", new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "ServerManager.DoYes()",
                            Text = GetLangStr("ServerManager66", "是")
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "ServerManager.DoNo()",
                            Text = GetLangStr("ServerManager67", "否")
                        }
                    }).Show();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-DoConfirm", ex.Message + "；" + ex.StackTrace, "DoConfirm has an exception");
            }
        }

        /// <summary>
        /// 确定事件
        /// </summary>
        [DirectMethod]
        public void DoYes()
        {
            try
            {
                RowSelectionModel sm = this.GridDeviceManager.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRow.ToBuilder();
                string Id = sm.SelectedRow.RecordID;
                DataRow[] rows = (Session["datatable"] as DataTable).Select("col0=" + nowid);
                if (rows.Length > 0)
                {
                    tabname = rows[0]["col1"].ToString();
                }
                if (deviceManager.DeleteServerInfo(Id) > 0)
                {
                    LedDataBind("1=1");
                    logManager.InsertLogRunning(uName, GetLangStr("","删除：[") + tabname + GetLangStr("","]服务器"), nowIp, "3");
                    Notice(GetLangStr("ServerManager55", "信息提示"), GetLangStr("ServerManager70", "删除成功"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-DoYes", ex.Message + "；" + ex.StackTrace, "DoYes has an exception");
            }
        }

        /// <summary>
        /// 不删除事件
        /// </summary>
        [DirectMethod]
        public void DoNo()
        {
        }

        #endregion DirectMethod 方法

        #region 私有方法

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {

                this.StoreDeviceType.DataSource = dtServer = deviceManager.GetServerType();
                this.StoreDeviceType.DataBind();

                this.StoreTXLX.DataSource =dtTxlx =  systemManager.GetCodeData("260003");
                this.StoreTXLX.DataBind();
                this.StoreTXXY.DataSource =dtTxxy= systemManager.GetCodeData("260004");
                this.StoreTXXY.DataBind();
                this.StoreDepart.DataSource = settingManager.GetDepartmentDict("00");
                this.StoreDepart.DataBind();
                this.StoreSY.DataSource = dtSfsy= systemManager.GetCodeData("240034");
                this.StoreSY.DataBind();
                this.StoreJSDW.DataSource = deviceManager.GetBussiness("type='1'");
                this.StoreJSDW.DataBind();
                this.StoreZZCS.DataSource = deviceManager.GetBussiness("type='2'");
                this.StoreZZCS.DataBind();
                this.StoreWHDW.DataSource = deviceManager.GetBussiness("type='3'");
                this.StoreWHDW.DataBind();

                LedDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 加载设备信息
        /// </summary>
        /// <param name="where"></param>
        private void LedDataBind(string where)
        {
            try
            {
                PagingToolbar1.PageSize = 15;
                DataTable dt = deviceManager.GetServer(where);
                StoreDeviceManager.DataSource = dt;
                StoreDeviceManager.DataBind();
                Session["datatable"] = dt;



                if (dt != null && dt.Rows.Count > 0)
                {
                    ButExcel.Disabled = false;
                    // ButPrint.Disabled = false;
                }
                else
                {
                    ButExcel.Disabled = true;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-LedDataBind", ex.Message + "；" + ex.StackTrace, "LedDataBind has an exception");
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
                DataTable dt1 = dt.Copy();
                if (dt1 != null)
                {
                    dt1.Columns.Remove("col0"); dt1.Columns.Remove("col2"); dt1.Columns.Remove("col4");
                    dt1.Columns.Remove("col8");
                    dt1.Columns["col1"].SetOrdinal(0); dt1.Columns["col3"].SetOrdinal(1); dt1.Columns["col5"].SetOrdinal(2);
                    dt1.Columns["col6"].SetOrdinal(3); dt1.Columns["col7"].SetOrdinal(4);
                    string n = GetLangStr("ServerManager17", "服务器名称");
                    dt1.Columns[0].ColumnName = n;
                    dt1.Columns[1].ColumnName = GetLangStr("ServerManager18", "服务器类型");
                    dt1.Columns[2].ColumnName = GetLangStr("ServerManager19", "服务器型号");
                    dt1.Columns[3].ColumnName = GetLangStr("ServerManager20", "服务器IP");
                    dt1.Columns[4].ColumnName = GetLangStr("ServerManager21", "服务器端口");
                    //PrintColumns pc = new PrintColumns();
                    //pc.Add(new PrintColumn("设备编号", 1));
                    //pc.Add(new PrintColumn("设备名称", 2));
                    //pc.Add(new PrintColumn("设备类型", 3));
                    //pc.Add(new PrintColumn("设备型号", 4));
                    //dt2 = Bll.Common.GetDataTablePrint(dt, pc);
                }

                return dt1;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-ChangeDataTable", ex.Message + "；" + ex.StackTrace, "ChangeDataTable has an exception");
                return null;
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
                string where = "   1=1 ";
                if (!string.IsNullOrEmpty(TxtDeviceName.Text))
                {
                    where = where + " and  server_name='" + TxtDeviceName.Text.ToUpper() + "' ";
                }
                if (CmbDeviceType.SelectedIndex != -1)
                {
                    where = where + " and  server_type_id='" + CmbDeviceType.SelectedItem.Value + "' ";
                }
                if (CmbSBXH.SelectedIndex != -1)
                {
                    where = where + " and  server_mode_id='" + CmbSBXH.SelectedItem.Value + "' ";
                }

                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-GetWhere", ex.Message + "；" + ex.StackTrace, "GetWhere has an exception");
                return "";
            }
        }

        /// <summary>
        /// 创建设备树
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public Ext.Net.TreeNodeCollection BuildTree(Ext.Net.TreeNodeCollection nodes)
        {
            try
            {
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }

                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Expanded = true;
                root.Text = GetLangStr("ServerManager62", "服务器信息列表");

                nodes.Add(root);

                DataTable dt = deviceManager.GetServerType();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    string code = dt.Rows[i]["col0"].ToString();
                    DataTable dt2 = deviceManager.GetServer("a.server_type_id='" + code + "'");
                    string countvalue = "0";
                    if (dt2 != null)
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            countvalue = Convert.ToString(dt2.Rows.Count);
                        }
                        else
                        {
                            node.Disabled = true;
                        }
                    }
                    node.Text = dt.Rows[i]["col1"].ToString() + "(" + countvalue + ")";
                    node.Listeners.Click.Handler = "ServerManager.SelectNode('" + code + "') ;";
                    node.Icon = Icon.Monitor;
                    node.NodeID = dt.Rows[i]["col0"].ToString();
                    node.Expanded = true;
                    root.Nodes.Add(node);
                }
                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
                return null;
            }
        }

        /// <summary>
        ///转换datatable为hashtable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public Hashtable ConventDataTable(DataTable dt)
        {
            try
            {
                Hashtable hts = new Hashtable();
                int j = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string kkType = dt.Rows[i]["col1"].ToString();
                    if (!hts.ContainsKey(kkType))
                    {
                        j++;
                        hts.Add(kkType, dt.Rows[i]["col0"].ToString());
                    }
                }
                return hts;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-ConventDataTable", ex.Message + "；" + ex.StackTrace, "ConventDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        /// 读取Grid选中行数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private string GetdatabyField(string data, string field)
        {
            try
            {
                string f1 = "<" + field + ">";
                string f2 = "</" + field + ">";
                int i = data.IndexOf(f1);
                int j = data.IndexOf(f2);
                if (i >= 0 && j >= 0)
                {
                    return data.Substring(i + f1.Length, j - i - f2.Length + 1);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("ServerManager.aspx-GetdatabyField", ex.Message + "；" + ex.StackTrace, "GetdatabyField has an exception");

                ILog.WriteErrorLog(ex); return "";
            }
        }

        /// <summary>
        ///提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        public void Notice(string title, string msg)
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
        ///  添加设备至设备树
        /// </summary>
        /// <param name="root"></param>
        /// <param name="dr"></param>
        private void Addree(Ext.Net.TreeNode root, DataRow dr)
        {
            try
            {
                Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                node.Text = dr["col5"].ToString();
                node.Leaf = true;
                node.Icon = Icon.House;
                node.NodeID = dr["col6"].ToString();
                root.Nodes.Add(node);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ServerManager.aspx-Addree", ex.Message + "；" + ex.StackTrace, "Addree has an exception");
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