using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class DeviceManager : System.Web.UI.Page
    {
        #region 成员变量

        private SettingManager settingManager = new SettingManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private SystemManager systemManager = new SystemManager();
        private MyNet.Atmcs.Uscmcp.Bll.DeviceManager deviceManager = new MyNet.Atmcs.Uscmcp.Bll.DeviceManager();
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private static int qbnum = 0;//区分添加修改
        private static string value = null;

        /// <summary>
        /// 设备名称
        /// </summary>
        private static string sbmc;

        /// <summary>
        /// 设备类型
        /// </summary>
        private static string sblx;

        /// <summary>
        /// 卡口设备
        /// </summary>
        private static string kksb;

        /// <summary>
        /// 是否使用
        /// </summary>
        private static string sfsy;

        /// <summary>
        /// 设备ip
        /// </summary>
        private static string sbip;

        /// <summary>
        /// 设备端口
        /// </summary>
        private static string sbdk;

        /// <summary>
        /// 设备通道数
        /// </summary>
        private static string sbtds;

        /// <summary>
        /// 外部编号
        /// </summary>
        private static string wbbh;

        /// <summary>
        /// 通讯类型
        /// </summary>
        private static string txlx;

        /// <summary>
        /// 通讯协议
        /// </summary>
        private static string txxy;

        /// <summary>
        /// 用户名
        /// </summary>
        private static string yhm;

        /// <summary>
        /// 密码
        /// </summary>
        private static string mm;

        /// <summary>
        /// 设备长度
        /// </summary>
        private static string sbcd;

        /// <summary>
        /// 设备宽度
        /// </summary>
        private static string sbkd;

        /// <summary>
        /// 串口号
        /// </summary>
        private static string ckh;

        /// <summary>
        /// 串口参数
        /// </summary>
        private static string ckcs;

        /*
         /资产信息
         */

        /// <summary>
        /// 建设单位
        /// </summary>
        private static string jsdw;

        /// <summary>
        /// 维护单位
        /// </summary>
        private static string whdw;

        /// <summary>
        ///  设备制造商
        /// </summary>
        private static string sbzzs;

        /// <summary>
        ///访问登录名
        /// </summary>
        private static string uName;

        /// <summary>
        /// 机器ip
        /// </summary>
        private static string nowIp;

        private static int types = 0;

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
                string js = "alert('" + GetLangStr("DeviceManager35", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'"; 
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
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("DeviceManager39", "访问：设别信息管理"), userinfo.NowIp, "0");
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
                logManager.InsertLogError("DeviceManager.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");
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
                logManager.InsertLogError("DeviceManager.aspx-ButRefreshClick", ex.Message+"；"+ex.StackTrace, "ButRefreshClick has an exception");
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
                DataTable dt = deviceManager.GetDevice(" 1=1 ");
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
                logManager.InsertLogError("DeviceManager.aspx-MyData_Refresh", ex.Message+"；"+ex.StackTrace, "MyData_Refresh has an exception");
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

                hs.Add("device_id", Text_SBBH.Text.ToString());
                hs.Add("device_idext", TxtDeviceIdExt.Text);
                hs.Add("device_name", Text_SBMC.Text.ToString());
                if (Cob_SBLX.SelectedIndex != -1)
                {
                    hs.Add("device_type_id", Cob_SBLX.Value.ToString());
                }
                else
                {
                    NoticeError(GetLangStr("DeviceManager67", "信息错误"), GetLangStr("DeviceManager68", "设备类型未选择"));
                    return;
                }
                if (Cob_SBXH.SelectedIndex != -1)
                {
                    hs.Add("device_mode_id", Cob_SBXH.Value.ToString());
                }
                else
                {
                    NoticeError(GetLangStr("DeviceManager67", "信息错误"), GetLangStr("DeviceManager68", "设备型号未选择"));
                    return;
                }
                hs.Add("ipaddress", Text_IP.Text.ToString());
                hs.Add("port", Text_DK.Text.ToString());
                if (!string.IsNullOrEmpty(Text_TYDS.Text))
                {
                    hs.Add("channels", Text_TYDS.Text.ToString());
                }
                else
                {
                    hs.Add("channels", "0");
                }
                hs.Add("username", Text_YHM.Text.ToString());
                hs.Add("password", Text_MM.Text.ToString());
                hs.Add("createdate", "now()");
                hs.Add("dev_length", Text_Ledth.Text.ToString());
                hs.Add("dev_width", Text_Width.Text.ToString());
                hs.Add("port_num", Text_CKH.Text.ToString());
                hs.Add("port_param", Text_CKCS.Text.ToString());
                hs.Add("protocol_id", CommonExt.IsNullComboBox(Cob_TXXY, ""));
                hs.Add("comm_type_id", CommonExt.IsNullComboBox(Cob_TXLX, ""));
                hs.Add("build_id", CommonExt.IsNullComboBox(Cob_JSDW, ""));
                hs.Add("maintain_company_id", CommonExt.IsNullComboBox(Cob_WHDW, ""));
                hs.Add("make_company_id", CommonExt.IsNullComboBox(Cob_ZZCS, ""));
                hs.Add("isuse", CommonExt.IsNullComboBox(Cob_SFSY, "1"));
                if (deviceManager.UptateDeviceInfo(hs) > 0)
                {
                    if (HidSaveFlag.Value.Equals("1"))
                    {
                        if (CheckSave.Checked)
                        {
                            Window4.Hide();
                        }
                        else
                        {
                            Text_SBMC.Reset();
                            Text_SBBH.Reset();
                            Text_IP.Reset();
                        }
                    }
                    else
                    {
                        Window4.Hide();
                    }
                    if (Cob_SBLX.SelectedIndex != -1)
                    {
                        LedDataBind(" 1=1 and a.device_type_id='" + Cob_SBLX.Value.ToString() + "'");
                    }
                    else
                    {
                        LedDataBind(" 1=1");
                    }

                    if (types == 1)
                    {
                        string sbmcs = Text_SBMC.Text;
                        string sblxs = Cob_SBLX.SelectedItem.Text;
                        string kksbs = Cob_SBXH.SelectedItem.Text;
                        string sfsys = Cob_SFSY.SelectedItem.Text;
                        string sbips = Text_IP.Text;
                        string sbdks = Text_DK.Text;
                        string sbtdss = Text_TYDS.Text;
                        string wbbhs = TxtDeviceIdExt.Text;
                        string txlxs = Cob_TXLX.SelectedItem.Text;
                        string txxys = Cob_TXXY.SelectedItem.Text;
                        string yhms = Text_YHM.Text;
                        string mms = Text_MM.Text;
                        string sbcds = Text_Ledth.Text;
                        string sbkds = Text_Width.Text;
                        string ckhs = Text_CKH.Text;
                        string ckcss = Text_CKCS.Text;
                        string jsdws = Cob_JSDW.SelectedItem.Text;
                        string whdws = Cob_WHDW.SelectedItem.Text;
                        string sbzzss = Cob_ZZCS.SelectedItem.Text;
                        string lblname = "";

                        lblname += Bll.Common.AssembleRunLog(sblx, sblxs, GetLangStr("DeviceManager2", "设备类型"), "1");
                        lblname += Bll.Common.AssembleRunLog(kksb, kksbs, GetLangStr("DeviceManager71", "卡口设备"), "1");
                        lblname += Bll.Common.AssembleRunLog(sfsy, sfsys, GetLangStr("DeviceManager31", "是否使用"), "1");
                        lblname += Bll.Common.AssembleRunLog(sbip, sbips, GetLangStr("DeviceManager19", "设备Ip"), "1");
                        lblname += Bll.Common.AssembleRunLog(sbdk, sbdks, GetLangStr("DeviceManager20", "设备端口"), "1");
                        lblname += Bll.Common.AssembleRunLog(sbtds, sbtdss, GetLangStr("DeviceManager37", "设备通道数"), "1");
                        lblname += Bll.Common.AssembleRunLog(wbbh, wbbhs, GetLangStr("DeviceManager21", "外部编号"), "1");
                        lblname += Bll.Common.AssembleRunLog(txlx, txlxs, GetLangStr("DeviceManager41", "通讯类型"), "1");
                        lblname += Bll.Common.AssembleRunLog(txxy, txxys, GetLangStr("DeviceManager43", "通讯协议"), "1");
                        lblname += Bll.Common.AssembleRunLog(yhm, yhms, GetLangStr("DeviceManager45", "用户名"), "1");
                        lblname += Bll.Common.AssembleRunLog(mm, mms, GetLangStr("DeviceManager47", "密码"), "1");
                        lblname += Bll.Common.AssembleRunLog(sbcd, sbcds, GetLangStr("DeviceManager49", "设备长度"), "1");
                        lblname += Bll.Common.AssembleRunLog(sbkd, sbkds, GetLangStr("DeviceManager51", "设备宽度"), "1");
                        lblname += Bll.Common.AssembleRunLog(ckh, ckhs, GetLangStr("DeviceManager53", "串口号"), "1");
                        lblname += Bll.Common.AssembleRunLog(ckcs, ckcss, GetLangStr("DeviceManager55", "串口参数"), "1");
                        lblname += Bll.Common.AssembleRunLog(jsdw, jsdws, GetLangStr("DeviceManager58", "建设单位"), "1");
                        lblname += Bll.Common.AssembleRunLog(whdw, whdws, GetLangStr("DeviceManager60", "维护单位"), "1");
                        lblname += Bll.Common.AssembleRunLog(sbzzs, sbzzss, GetLangStr("DeviceManager62", "设备制造商"), "1");
                        logManager.InsertLogRunning(uName, GetLangStr("DeviceManager72", "修改:[") + sbmc + "]" + lblname, nowIp, "2");
                        Notice(GetLangStr("DeviceManager70", "信息提示"), GetLangStr("DeviceManager69", "信息保存成功"));
                        //页面加载方法
                        BuildTree(TreePanel1.Root);
                        TreePanel1.Render();
                        types = 0;
                    }
                    else {
                        InsertLog();
                    }

                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-UpdateDevice", ex.Message+"；"+ex.StackTrace, "UpdateDevice has an exception");
            }
        }

        public void InsertLog()
        {
            string ty = "0";
            string lblname = "";
            lblname += MyNet.Atmcs.Uscmcp.Bll.Common.AssembleRunLog("", Cob_SBLX.SelectedItem.Text, GetLangStr("DeviceManager2", "设备类型"), ty);
            //   lblname += MyNet.Atmcs.Uscmcp.Bll.Common.AssembleRunLog("", Text_SBMC.Text, "设备名称", ty);
            lblname += MyNet.Atmcs.Uscmcp.Bll.Common.AssembleRunLog("", Cob_SBXH.SelectedItem.Text, GetLangStr("DeviceManager71", "卡口设备"), ty);
            lblname += MyNet.Atmcs.Uscmcp.Bll.Common.AssembleRunLog("", Cob_SFSY.SelectedItem.Text, GetLangStr("DeviceManager31", "是否使用"), ty);
            lblname += MyNet.Atmcs.Uscmcp.Bll.Common.AssembleRunLog("", Cob_JSDW.SelectedItem.Text, GetLangStr("DeviceManager58", "建设单位"), ty);
            lblname += MyNet.Atmcs.Uscmcp.Bll.Common.AssembleRunLog("", Cob_WHDW.SelectedItem.Text, GetLangStr("DeviceManager60", "维护单位"), ty);
            lblname += MyNet.Atmcs.Uscmcp.Bll.Common.AssembleRunLog("", Cob_ZZCS.SelectedItem.Text, GetLangStr("DeviceManager62", "设备制造商"), ty);
            UserInfo userinfo = Session["Userinfo"] as UserInfo;
            logManager.InsertLogRunning(userinfo.UserName, GetLangStr("DeviceManager75", "添加:[") + Text_SBMC.Text + GetLangStr("DeviceManager15", "]设备:") + lblname, userinfo.NowIp, "1");
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
                logManager.InsertLogError("DeviceManager.aspx-MyData_Refresh", ex.Message+"；"+ex.StackTrace, "MyData_Refresh has an exception");
            }
        }

        /// <summary>
        /// 下载批量导入excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonDownload_Download(object sender, DirectEventArgs e)
        {
            try
            {
                Response.ContentType = "application/x-zip-compressed";
                string fileName = GetLangStr("DeviceManager79", "设备批量导入表.xls");
                Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                string filename = Server.MapPath("../Export/suspicion.xsl");
                Response.TransmitFile(filename);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-ButtonDownload_Download", ex.Message+"；"+ex.StackTrace, "ButtonDownload_Download has an exception");
            }
        }

        /// <summary>
        /// 选中设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectDev_Click(object sender, DirectEventArgs e)
        {
            try
            {
                string id = e.ExtraParams["id"];
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-SelectDev_Click", ex.Message+"；"+ex.StackTrace, "SelectDev_Click has an exception");
            }
        }

        /// <summary>
        ///添加新设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButDevAdd_Click(object sender, DirectEventArgs e)
        {
            try
            {
                qbnum = 1;

                HidSaveFlag.Value = "1";
                Button5.Hidden = false;
                Text_SBBH.Reset();
                Text_SBBH.Text = tgsPproperty.GetRecordId();
                TxtDeviceIdExt.Reset();
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
                Text_Ledth.Reset();
                Text_Width.Reset();
                Text_CKH.Reset();
                Text_CKCS.Reset();
                Cob_JSDW.Reset();
                Cob_ZZCS.Reset();
                Cob_WHDW.Reset();

                Window4.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-ButDevAdd_Click", ex.Message+"；"+ex.StackTrace, "ButDevAdd_Click has an exception");
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
                logManager.InsertLogError("DeviceManager.aspx-ToXml", ex.Message+"；"+ex.StackTrace, "ToXml has an exception");
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
                logManager.InsertLogError("DeviceManager.aspx-ToExcel", ex.Message+"；"+ex.StackTrace, "ToExcel has an exception");
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
                logManager.InsertLogError("DeviceManager.aspx-ToCsv", ex.Message+"；"+ex.StackTrace, "ToCsv has an exception");
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
                    string xml = Bll.Common.GetPrintXml(GetLangStr("DeviceManager80", "设备信息管理"), "", "", "printdatatable");
                    string js = "OpenPrintPageV(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-ButPrintClick", ex.Message+"；"+ex.StackTrace, "ButPrintClick has an exception");
            }
        }

        #endregion 控件事件

        #region DirectMethod 方法

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            CmbDeviceType.Reset();
            TxtDeviceName.Reset();
            this.ResourceManager1.RegisterAfterClientInitScript("clearTime();");
            CmbSBXH.Reset();
        }

        /// <summary>
        /// 选中设备树事件
        /// </summary>
        [DirectMethod]
        public void SelectDevice()
        {
            try
            {
                string type = Cob_SBLX.SelectedItem.Value;
                DataTable da = deviceManager.GetDeviceTypeMode("device_type_id='" + type + "'");
                CmbSBXH.Reset();
                Cob_SBXH.Reset();
                this.StoreDeviceMode.DataSource = da;
                this.StoreDeviceMode.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-SelectDevice", ex.Message+"；"+ex.StackTrace, "SelectDevice has an exception");
            }
        }

        /// <summary>
        ///根据设备类型关联查询设备厂家
        /// </summary>
        [DirectMethod]
        public void SelectQDevice()
        {
            try
            {
                string type = CmbDeviceType.SelectedItem.Value;
                DataTable da = deviceManager.GetDeviceTypeMode("device_type_id='" + type + "'");
                this.StoreSDeviceMode.DataSource = da;
                this.StoreSDeviceMode.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-SelectQDevice", ex.Message+"；"+ex.StackTrace, "SelectQDevice has an exception");
            }
        }

        /// <summary>
        /// 设备点击事件
        /// </summary>
        /// <param name="code"></param>
        [DirectMethod]
        public void SelectNode(string code)
        {
            value = code;
            try
            {
                LedDataBind(" 1=1 and a.device_type_id='" + code + "'");
                CmbDeviceType.Value = code;
                DataTable da = deviceManager.GetDeviceTypeMode("device_type_id='" + code + "'");
                this.StoreSDeviceMode.DataSource = da;
                this.StoreSDeviceMode.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-SelectNode", ex.Message+"；"+ex.StackTrace, "SelectNode has an exception");
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
                    Window4.Title = GetLangStr("DeviceManager81", "查看设备信息");
                    DataTable dt = deviceManager.GetDeviceInfo("device_id='" + id + "'");
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            Text_SBBH.Text = dt.Rows[0]["DEVICE_ID"].ToString();
                            Cob_SBLX.Value = dt.Rows[0]["DEVICE_TYPE_ID"].ToString();
                            Text_SBMC.Value = dt.Rows[0]["DEVICE_NAME"].ToString();
                            Cob_SBXH.Value = dt.Rows[0]["DEVICE_MODE_ID"].ToString();
                            Cob_SFSY.Value = dt.Rows[0]["ISUSE"].ToString();
                            Text_IP.Text = dt.Rows[0]["IPADDRESS"].ToString();
                            Text_DK.Text = dt.Rows[0]["PORT"].ToString();
                            Text_TYDS.Text = dt.Rows[0]["CHANNELS"].ToString();
                            Cob_TXLX.Text = dt.Rows[0]["COMM_TYPE_ID"].ToString();
                            Cob_TXXY.Text = dt.Rows[0]["PROTOCOL_ID"].ToString();
                            Text_YHM.Text = dt.Rows[0]["USERNAME"].ToString();
                            Text_MM.Text = dt.Rows[0]["PASSWORD"].ToString();
                            Text_Ledth.Text = dt.Rows[0]["DEV_LENGTH"].ToString();
                            Text_Width.Text = dt.Rows[0]["DEV_WIDTH"].ToString();
                            Text_CKH.Text = dt.Rows[0]["PORT_NUM"].ToString();
                            Text_CKCS.Text = dt.Rows[0]["PORT_PARAM"].ToString();
                            Cob_JSDW.Value = dt.Rows[0]["BUILD_ID"].ToString();
                            Cob_ZZCS.Value = dt.Rows[0]["MAKE_COMPANY_ID"].ToString(); ;
                            Cob_WHDW.Value = dt.Rows[0]["MAINTAIN_COMPANY_ID"].ToString();
                            Button5.Hidden = true;
                            sbmc = dt.Rows[0]["DEVICE_NAME"].ToString();

                            Window4.Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-RowDblClickShow", ex.Message+"；"+ex.StackTrace, "RowDblClickShow has an exception");
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
                types = 1;
                RowSelectionModel sm = this.GridDeviceManager.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string id = sm.SelectedRow.RecordID;
                    HidSaveFlag.Value = "2";
                    Window4.Title = GetLangStr("DeviceManager82", "修改设备信息");
                    HiddenId.Value = id;
                    DataTable dt = deviceManager.GetDeviceInfo("device_id='" + id + "'");
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            Text_SBBH.Text = dt.Rows[0]["DEVICE_ID"].ToString();
                            Cob_SBLX.Value = dt.Rows[0]["DEVICE_TYPE_ID"].ToString();
                            Text_SBMC.Value = dt.Rows[0]["DEVICE_NAME"].ToString();
                            DataTable da = deviceManager.GetDeviceTypeMode("device_type_id='" + dt.Rows[0]["DEVICE_TYPE_ID"].ToString() + "'");
                            this.StoreDeviceMode.DataSource = da;
                            this.StoreDeviceMode.DataBind();
                            Cob_SBXH.Value = dt.Rows[0]["DEVICE_MODE_ID"].ToString();
                            Cob_SFSY.Value = dt.Rows[0]["ISUSE"].ToString();
                            Text_IP.Text = dt.Rows[0]["IPADDRESS"].ToString();
                            Text_DK.Text = dt.Rows[0]["PORT"].ToString();
                            Text_TYDS.Text = dt.Rows[0]["CHANNELS"].ToString();
                            Cob_TXLX.Text = dt.Rows[0]["COMM_TYPE_ID"].ToString();
                            Cob_TXXY.Text = dt.Rows[0]["PROTOCOL_ID"].ToString();
                            Text_YHM.Text = dt.Rows[0]["USERNAME"].ToString();
                            Text_MM.Text = dt.Rows[0]["PASSWORD"].ToString();
                            Text_Ledth.Text = dt.Rows[0]["DEV_LENGTH"].ToString();
                            Text_Width.Text = dt.Rows[0]["DEV_WIDTH"].ToString();
                            Text_CKH.Text = dt.Rows[0]["PORT_NUM"].ToString();
                            Text_CKCS.Text = dt.Rows[0]["PORT_PARAM"].ToString();
                            Cob_JSDW.Value = dt.Rows[0]["BUILD_ID"].ToString();
                            Cob_ZZCS.Value = dt.Rows[0]["MAKE_COMPANY_ID"].ToString(); ;
                            Cob_WHDW.Value = dt.Rows[0]["MAINTAIN_COMPANY_ID"].ToString();
                            TxtDeviceIdExt.Text = dt.Rows[0]["DEVICE_IDEXT"].ToString();
                            Button5.Hidden = false;
                            Window4.Show();

                            sbmc = Text_SBMC.Text;
                            sblx = Cob_SBLX.SelectedItem.Text;
                            kksb = Cob_SBXH.SelectedItem.Text;
                            sfsy = Cob_SFSY.SelectedItem.Text;
                            sbip = Text_IP.Text;
                            sbdk = Text_DK.Text;
                            sbtds = Text_TYDS.Text;
                            wbbh = TxtDeviceIdExt.Text;
                            txlx = Cob_TXLX.SelectedItem.Text;
                            txxy = Cob_TXXY.SelectedItem.Text;
                            yhm = Text_YHM.Text;
                            mm = Text_MM.Text;
                            sbcd = Text_Ledth.Text;
                            sbkd = Text_Width.Text;
                            ckh = Text_CKH.Text;
                            ckcs = Text_CKCS.Text;
                            jsdw = Cob_JSDW.SelectedItem.Text;
                            whdw = Cob_WHDW.SelectedItem.Text;
                            sbzzs = Cob_ZZCS.SelectedItem.Text;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-Modify", ex.Message+"；"+ex.StackTrace, "Modify has an exception");
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
                    X.Msg.Confirm(GetLangStr("DeviceManager83", "信息"), GetLangStr("DeviceManager84", "这样会删除设备信息，确认要删除这台设备吗？"), new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "DeviceManager.DoYes()",
                            Text = GetLangStr("DeviceManager85", "是")
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "DeviceManager.DoNo()",
                            Text = GetLangStr("DeviceManager86", "否")
                        }
                    }).Show();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-DoConfirm", ex.Message+"；"+ex.StackTrace, "DoConfirm has an exception");
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

                DataTable dt = deviceManager.GetDeviceInfo("device_id='" + Id + "'");
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        sbmc = dt.Rows[0]["DEVICE_NAME"].ToString();
                    }
                }
                if (deviceManager.DeleteDevice(Id) > 0)
                {
                    // value = Cob_SBLX.Value.ToString();

                    if (!string.IsNullOrEmpty(value))
                    {
                        LedDataBind(" 1=1 and a.device_type_id='" + value + "'");
                    }
                    else
                    {
                        LedDataBind(" 1=1");
                    }
                    logManager.InsertLogRunning(uName, GetLangStr("DeviceManager13", "删除:[") + sbmc + GetLangStr("DeviceManager15", "]设备"), nowIp, "3");
                    Notice(GetLangStr("DeviceManager70", "信息提示"), GetLangStr("DeviceManager71", "删除成功"));
                    //左边树重新加载
                    BuildTree(TreePanel1.Root);
                    TreePanel1.Render();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-DoYes", ex.Message+"；"+ex.StackTrace, "DoYes has an exception");
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
                this.StoreDeviceType.DataSource = deviceManager.GetDeviceType();
                this.StoreDeviceType.DataBind();

                this.StoreTXLX.DataSource = systemManager.GetCodeData("260003");
                this.StoreTXLX.DataBind();
                this.StoreTXXY.DataSource = systemManager.GetCodeData("260004");
                this.StoreTXXY.DataBind();
                this.StoreDepart.DataSource = settingManager.GetDepartmentDict("00");
                this.StoreDepart.DataBind();
                this.StoreSY.DataSource = systemManager.GetCodeData("240034");
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
                logManager.InsertLogError("DeviceManager.aspx-StoreDataBind", ex.Message+"；"+ex.StackTrace, "StoreDataBind has an exception");
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
                DataTable dt = deviceManager.GetDevice(where);
                StoreDeviceManager.DataSource = dt;
                StoreDeviceManager.DataBind();

                Session["datatable"] = dt;
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        ButExcel.Disabled = false;
                        // ButPrint.Disabled = false;
                    }
                    else
                    {
                        ButExcel.Disabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-LedDataBind", ex.Message+"；"+ex.StackTrace, "LedDataBind has an exception");
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
                    dt1.Columns["col1"].SetOrdinal(0); dt1.Columns["col3"].SetOrdinal(1); dt1.Columns["col5"].SetOrdinal(2);
                    dt1.Columns["col6"].SetOrdinal(3); dt1.Columns["col7"].SetOrdinal(4); dt1.Columns["col8"].SetOrdinal(5);
                    dt1.Columns[0].ColumnName = GetLangStr("DeviceManager1", "设备名称");
                    dt1.Columns[1].ColumnName = GetLangStr("DeviceManager2", "设备类型");
                    dt1.Columns[2].ColumnName = GetLangStr("DeviceManager4", "设备型号");
                    dt1.Columns[3].ColumnName = GetLangStr("DeviceManager19", "设备IP");
                    dt1.Columns[4].ColumnName = GetLangStr("DeviceManager20", "设备端口");
                    dt1.Columns[5].ColumnName = GetLangStr("DeviceManager21", "外部编号");
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
                logManager.InsertLogError("DeviceManager.aspx-ChangeDataTable", ex.Message+"；"+ex.StackTrace, "ChangeDataTable has an exception");
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
                string where = "1=1 ";
                if (!string.IsNullOrEmpty(TxtDeviceName.Text))
                {
                    where = where + " and  a.device_name='" + TxtDeviceName.Text.ToUpper() + "' ";
                }
                if (CmbDeviceType.SelectedIndex != -1)
                {
                    where = where + " and  a.device_type_id='" + CmbDeviceType.SelectedItem.Value + "' ";
                }
                if (CmbSBXH.SelectedIndex != -1)
                {
                    where = where + " and  a.device_mode_id='" + CmbSBXH.SelectedItem.Value + "' ";
                }

                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-GetWhere", ex.Message+"；"+ex.StackTrace, "GetWhere has an exception");
                return "";
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
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-GetdatabyField", ex.Message+"；"+ex.StackTrace, "GetdatabyField has an exception");
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
                root.Text = GetLangStr("DeviceManager78", "设备信息列表");

                nodes.Add(root);

                DataTable dt = deviceManager.GetDeviceType();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    string code = dt.Rows[i]["col0"].ToString();
                    DataTable dt2 = deviceManager.GetDevice("a.device_type_id='" + code + "'");
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
                    node.Listeners.Click.Handler = "DeviceManager.SelectNode('" + code + "') ;";
                    node.Icon = Icon.DriveNetwork;
                    node.NodeID = dt.Rows[i]["col0"].ToString();
                    node.Expanded = true;
                    root.Nodes.Add(node);
                }
                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceManager.aspx-BuildTree", ex.Message+"；"+ex.StackTrace, "BuildTree has an exception");
                return null;
            }
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
                logManager.InsertLogError("DeviceManager.aspx-Addree", ex.Message+"；"+ex.StackTrace, "Addree has an exception");
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
                logManager.InsertLogError("DeviceManager.aspx-ConventDataTable", ex.Message+"；"+ex.StackTrace, "ConventDataTable has an exception");
                return null;
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
        ///
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        public void NoticeError(string title, string msg)
        {
            Notification.Show(new NotificationConfig
            {
                Title = title,
                Icon = Icon.Error,
                HideDelay = 2000,
                Height = 120,
                Html = "<br></br>" + msg + "!"
            });
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