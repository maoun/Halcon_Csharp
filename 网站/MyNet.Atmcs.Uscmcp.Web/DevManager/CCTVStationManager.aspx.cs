using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class CCTVStationManager : System.Web.UI.Page
    {
        #region 成员变量

        private SettingManager settingManager = new SettingManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private Bll.DeviceManager deviceManager = new Bll.DeviceManager();
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
      
        static string uName;
        static string nowIp;
        /// <summary>
        /// 监测点类型
        /// </summary>
        static string jcdlx;
        /// <summary>
        /// 查询监测点名称
        /// </summary>
        static string cxjcd;
        /// <summary>
        ///  监测点名称
        /// </summary>
        static string jcdmc;
        /// <summary>
        /// 实时监视设备
        /// </summary>
        static string ssjcsb;
        /// <summary>
        /// 监控通道号
        /// </summary>
        static string jktdh;
        /// <summary>
        /// 录像回放设备
        /// </summary>
        static string lxhfsb;
        /// <summary>
        /// 回放通道号
        /// </summary>
        static string hftdh;
        /// <summary>
        /// 所属方向
        /// </summary>
        static string ssfx;
        /// <summary>
        /// 矩阵编号
        /// </summary>
        static string jzbh;
        /// <summary>
        /// 是否启用
        /// </summary>
        static string sgqy;

     static   DataTable dtsfsy=null;

        static DataTable dtname = null;
        #endregion 成员变量

        #region 事件集合

        /// <summary>
        /// 加载页面
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
            try
            {
                if (!X.IsAjaxRequest)
                {
                    ToolExport.Disabled = true;
                    StoreDataBind();
                    SelectNode();
                    this.DataBind();
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    uName = userinfo.UserName;
                    nowIp = userinfo.NowIp;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：视频监控连接管理", userinfo.NowIp, "0");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-Page_Load", ex.Message+"；"+ex.StackTrace, "Page_Load has an exception");
            }
        }

        /// <summary>
        /// 查询连接数据
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
                logManager.InsertLogError("CCTVStationManager.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        /// <summary>
        /// 绑定监测点名称数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                string where = " 1=1  and scount>0  and dcount>0";
                if (!string.IsNullOrEmpty(TxtQStationName.Text))
                {
                    where = where + " and  station_name like '%" + TxtQStationName.Text + "%'";
                }
                if (CmbStationType.SelectedIndex != -1)
                {
                    where = where + " and station_type_id = '" + CmbStationType.Value.ToString() + "'";
                }
                this.StoreStation.DataSource = tgsPproperty.GetLocationStationByWhere(where);
                this.StoreStation.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-ButQueryClick", ex.Message+"；"+ex.StackTrace, "ButQueryClick has an exception");
            }
        }

        /// <summary>
        /// 刷新连接数据
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
                logManager.InsertLogError("CCTVStationManager.aspx-ButRefreshClick", ex.Message+"；"+ex.StackTrace, "ButRefreshClick has an exception");
            }
        }

        /// <summary>
        /// 刷新控件绑定的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyData_Refresh(object sender, DirectEventArgs e)
        {
            try
            {
                DataTable dt = deviceManager.GetDevice(" where 1=1 ");
                StoreDeviceManager.DataSource = dt;
                StoreDeviceManager.DataBind();

                Session["datatable"] = dt;
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        ToolExport.Disabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-MyData_Refresh", ex.Message+"；"+ex.StackTrace, "MyData_Refresh has an exception");
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
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-ButResetClick", ex.Message+"；"+ex.StackTrace, "ButResetClick has an exception");
            }
        }

        /// <summary>
        /// 刷新控件绑定的数据
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
                logManager.InsertLogError("CCTVStationManager.aspx-MyData_Refresh", ex.Message+"；"+ex.StackTrace, "MyData_Refresh has an exception");
            }
        }

        /// <summary>
        /// 添加连接信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButDevAdd_Click(object sender, DirectEventArgs e)
        {
            try
            {
                HidSaveFlag.Value = "1";
                Button5.Hidden = false;
                Window4.Title = GetLangStr("CCTVStationManager48", "添加连接设备");
                CmbDevice.AllowBlank = false;
                CmbStationType.AllowBlank = false;
                CmbStationType.Reset();
                TxtQStationName.Reset();
                CmbDevice.Reset();
                CmbStation.Reset();
                MNumChannel.Reset();
                RNumChannel.Reset();
                CmbDirection.Reset();
                CmbDeviceRecord.Reset();
                TxtMasterId.Reset();
                CmbIsuse.Reset();
                ButQuery.Hidden = false;
                CmbDeviceRecord.Hidden = false;
                CmbStationType.Hidden = false;
                TxtQStationName.Hidden = false;
                CmbDevice.Hidden = false;
                CmbStation.Hidden = false;
                CompositeField1.Hidden = false;
                Window4.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-ButDevAdd_Click", ex.Message+"；"+ex.StackTrace, "ButDevAdd_Click has an exception");
            }
        }

        /// <summary>
        /// 添加连接信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateDevice(object sender, DirectEventArgs e)
        {
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("masterid", TxtMasterId.Text);
                if (CmbDirection.SelectedIndex != -1)
                {
                    hs.Add("direction_id", CmbDirection.Value);
                }
                else
                {
                    Notice(GetLangStr("CCTVStationManager49", "信息提示"), GetLangStr("CCTVStationManager50", "请选择所属方向"));
                    return;
                }
                if (CmbIsuse.SelectedIndex != -1)
                {
                    hs.Add("isuse", CmbIsuse.Value);
                }
                else
                {
                    Notice(GetLangStr("CCTVStationManager49", "信息提示"), GetLangStr("CCTVStationManager51", "请选择是否启用"));
                    return;
                }
                if (!string.IsNullOrEmpty(MNumChannel.Text))
                {
                    hs.Add("mchannelid", MNumChannel.Text);
                }
                else
                {
                    Notice(GetLangStr("CCTVStationManager49", "信息提示"), GetLangStr("CCTVStationManager52", "请输入实时监视通道"));
                    return;
                }
                if (!string.IsNullOrEmpty(RNumChannel.Text))
                {
                    hs.Add("rchannelid", RNumChannel.Text);
                }
                else
                {
                    Notice(GetLangStr("CCTVStationManager49", "信息提示"), GetLangStr("CCTVStationManager53", "请输入录像回放通道"));
                    return;
                }
                if (HidSaveFlag.Value.ToString() == "1")
                {
                    if (CmbStation.SelectedIndex != -1)
                    {
                        hs.Add("station_id", CmbStation.Value);
                    }
                    else
                    {
                        Notice(GetLangStr("CCTVStationManager49", "信息提示"), GetLangStr("CCTVStationManager54", "请选择监测点名称"));
                        return;
                    }
                    if (CmbDevice.SelectedIndex != -1)
                    {
                        hs.Add("mdevice_id", CmbDevice.Value);
                    }
                    else
                    {
                        Notice(GetLangStr("CCTVStationManager49", "信息提示"), GetLangStr("CCTVStationManager55", "请选择连接监视设备"));
                        return;
                    }
                    if (CmbDeviceRecord.SelectedIndex != -1)
                    {
                        hs.Add("rdevice_id", CmbDeviceRecord.Value);
                    }
                    else
                    {
                        Notice(GetLangStr("CCTVStationManager49", "信息提示"), GetLangStr("CCTVStationManager56", "请选择连接录像回放设备"));
                        return;
                    }
                    string Id = tgsDataInfo.GetTgsRecordId();
                    hs.Add("id", Id);
                    if (deviceManager.InsertCctvSetting(hs) > 0)
                    {
                        LedDataBind(" 1=1 ");
                        jcdlx=CmbStationType.SelectedItem.Text;
                        jcdmc=CmbStation.SelectedItem.Text;
                        ssjcsb=CmbDevice.SelectedItem.Text;
                        string lblname = "";
                        lblname += Bll.Common.AssembleRunLog("", jcdmc, "监测点名称", "0");
                        lblname += Bll.Common.AssembleRunLog("", jcdlx, "监测点类型", "0");
                        lblname += Bll.Common.AssembleRunLog("", ssjcsb, "实时监测设备", "0");
                        logManager.InsertLogRunning(uName, "添加：监测点[" + jcdmc + "]" + lblname, nowIp, "1");
                        Notice(GetLangStr("CCTVStationManager49", "信息提示"), GetLangStr("CCTVStationManager57", "添加成功"));
                        Window4.Hide();
                    }
                }
                if (HidSaveFlag.Value.ToString() == "2")
                {
                    RowSelectionModel sm = this.GridDeviceManager.SelectionModel.Primary as RowSelectionModel;
                    sm.SelectedRow.ToBuilder();
                    string Id = sm.SelectedRow.RecordID;
                    hs.Add("id", Id);
                    if (deviceManager.UptateCctvSetting(hs) > 0)
                    {
                        LedDataBind(" 1=1");
                        DataRow[] dt = dtname.Select("col0='" + Id + "'");
                        string jcmc = dt[0]["col2"].ToString();
                        string jcname = dt[0]["col2"].ToString();
                        string jktds = MNumChannel.Text;
                        string hftds = RNumChannel.Text;
                        string ssfxs = CmbDirection.SelectedItem.Text;
                        string jzbhs = TxtMasterId.Text;
                        string sfsts = CmbIsuse.SelectedItem.Text;
                        string lblname = "";
                        lblname += Bll.Common.AssembleRunLog(jktdh, jktds, "监控通道号", "1");
                        lblname += Bll.Common.AssembleRunLog(hftdh, hftds, "回放通道号", "1");
                        lblname += Bll.Common.AssembleRunLog(ssfx, ssfxs, "所属方向", "1");
                        lblname += Bll.Common.AssembleRunLog(jzbh, jzbhs, "矩阵编号", "1");
                        lblname += Bll.Common.AssembleRunLog(sgqy, sfsts, "是否启用", "1");
                        logManager.InsertLogRunning(uName, "修改：[" + jcmc + "]监测点;" + lblname, nowIp, "2");
                        Notice(GetLangStr("CCTVStationManager49", "信息提示"), GetLangStr("CCTVStationManager58", "修改成功"));
                        Window4.Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-UpdateDevice", ex.Message+"；"+ex.StackTrace, "UpdateDevice has an exception");
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
                    string xml = Bll.Common.GetPrintXml(GetLangStr("CCTVStationManager59", "服务器信息管理"), "", "", "printdatatable");
                    string js = "OpenPrintPageV(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-ButPrintClick", ex.Message+"；"+ex.StackTrace, "ButPrintClick has an exception");
            }
        }

        /// <summary>
        /// 导出Xml
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
                logManager.InsertLogError("CCTVStationManager.aspx-ToXml", ex.Message+"；"+ex.StackTrace, "ToXml has an exception");
            }
        }

        /// <summary>
        /// 导出Excel
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
                logManager.InsertLogError("CCTVStationManager.aspx-ToExcel", ex.Message+"；"+ex.StackTrace, "ToExcel has an exception");
            }
        }

        /// <summary>
        /// 导出Csv
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
                logManager.InsertLogError("CCTVStationManager.aspx-ToCsv", ex.Message+"；"+ex.StackTrace, "ToCsv has an exception");
            }
        }

        #endregion 事件集合

        #region 私有方法

        /// <summary>
        /// 展示是否启用下拉数据、绑定监测点类型
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                this.StoreisUse.DataSource = dtsfsy = Bll.Common.ChangColName(tgsPproperty.GetCommonDict("240034"));
                this.StoreisUse.DataBind();
                this.StoreStationType.DataSource = Bll.Common.ChangColName(tgsPproperty.GetStationTypeInfo("iscctvshow='1' and isuse='1' "));
                this.StoreStationType.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-StoreDataBind", ex.Message+"；"+ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 查询连接数据
        /// </summary>
        /// <param name="where"></param>
        private void LedDataBind(string where)
        {
            try
            {
                DataTable dt=dtname= Bll.Common.ChangColName(deviceManager.GetCctvSetting("*", where));
                //PagingToolbar1.PageSize = 50;
                StoreDeviceManager.DataSource = dt;
                StoreDeviceManager.DataBind();

                Session["datatable"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    ToolExport.Disabled = false;
                }
                else
                {
                    ToolExport.Disabled = true;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-LedDataBind", ex.Message+"；"+ex.StackTrace, "LedDataBind has an exception");
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
                    dt2.Columns.Remove("col0"); dt2.Columns.Remove("col1"); dt2.Columns.Remove("col3"); dt2.Columns.Remove("col6");
                    dt2.Columns.Remove("col9"); dt2.Columns.Remove("col10"); dt2.Columns.Remove("col11"); dt2.Columns.Remove("col12");
                    dt2.Columns.Remove("col13"); dt2.Columns.Remove("col15"); dt2.Columns.Remove("col16"); dt2.Columns.Remove("col19");
                    dt2.Columns.Remove("col22"); dt2.Columns.Remove("col23"); dt2.Columns.Remove("col24"); dt2.Columns.Remove("col26");
                    dt2.Columns.Remove("col27"); dt2.Columns.Remove("col28"); dt2.Columns.Remove("col29"); dt2.Columns.Remove("col30");
                    dt2.Columns.Remove("col31"); dt2.Columns.Remove("col32");
                    dt2.Columns["col2"].SetOrdinal(0); dt2.Columns["col17"].SetOrdinal(1);
                    dt2.Columns["col4"].SetOrdinal(2); dt2.Columns["col7"].SetOrdinal(3);
                    dt2.Columns["col14"].SetOrdinal(4); dt2.Columns["col8"].SetOrdinal(5);
                    dt2.Columns["col5"].SetOrdinal(6); dt2.Columns["col20"].SetOrdinal(7);
                    dt2.Columns["col25"].SetOrdinal(8); dt2.Columns["col21"].SetOrdinal(9); dt2.Columns["col18"].SetOrdinal(10);
                    dt2.Columns[0].ColumnName = GetLangStr("CCTVStationManager31", "监测点名称");
                    dt2.Columns[1].ColumnName = GetLangStr("CCTVStationManager16", "设备状态");
                    dt2.Columns[2].ColumnName = GetLangStr("CCTVStationManager17", "所属方向");
                    dt2.Columns[3].ColumnName = GetLangStr("CCTVStationManager18", "实时视频设备");
                    dt2.Columns[4].ColumnName = GetLangStr("CCTVStationManager65", "实时设备类型");
                    dt2.Columns[5].ColumnName = GetLangStr("CCTVStationManager20", "实时设备IP");
                    dt2.Columns[6].ColumnName = GetLangStr("CCTVStationManager21", "实时通道");
                    dt2.Columns[7].ColumnName = GetLangStr("CCTVStationManager22", "录像回放设备");
                    dt2.Columns[8].ColumnName = GetLangStr("CCTVStationManager23", "回放设备类型");
                    dt2.Columns[9].ColumnName = GetLangStr("CCTVStationManager24", "回放设备IP");
                    dt2.Columns[10].ColumnName = GetLangStr("CCTVStationManager25", "回放通道");
                }
                return dt2;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-ChangeDataTable", ex.Message+"；"+ex.StackTrace, "ChangeDataTable has an exception");
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
                string where = " 1=1 ";
                if (!string.IsNullOrEmpty(TxtDeviceName.Text))
                {
                    where = where + " and     mdevice_name like '%" + TxtDeviceName.Text + "%'";
                }
                if (CmbDeviceType.SelectedIndex != -1)
                {
                    where = where + " and  mdevice_type_id='" + CmbDeviceType.SelectedItem.Value + "' ";
                }
                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-GetWhere", ex.Message+"；"+ex.StackTrace, "GetWhere has an exception");
                return "";
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
                logManager.InsertLogError("CCTVStationManager.aspx-Notice", ex.Message+"；"+ex.StackTrace, "Notice has an exception");
            }
        }

        ///// <summary>
        ///// 多语言转换
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="desc"></param>
        ///// <returns></returns>
        //public string GetLangStr(string value, string desc)
        //{
        //    string className = this.GetType().BaseType.FullName;
        //    return MyNet.Common.Lang.Language.CreateInstance(className).GetLanguageStr(value, desc, className);
        //}

        #endregion 私有方法

        #region [DirectMethod]

        /// <summary>
        /// 绑定实时监视设备、设备所属方向
        /// </summary>
        [DirectMethod]
        public void SelectDevice()
        {
            try
            {
                string type = CmbStation.SelectedItem.Value;
                this.StoreDevice.DataSource = tgsPproperty.GetDeviceInfoByStation("station_id = '" + type + "'");
                this.StoreDevice.DataBind();
                this.StoreDirection.DataSource = tgsPproperty.GetDirectionInfoByStation(type);
                this.StoreDirection.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-SelectDevice", ex.Message+"；"+ex.StackTrace, "SelectDevice has an exception");
            }
        }

        /// <summary>
        /// 绑定设备类型
        /// </summary>
        [DirectMethod]
        public void SelectNode()
        {
            try
            {
                this.StoreDeviceType.DataSource = deviceManager.GetCctvSetting("mdevice_type_id,mdevice_type_name", " 1=1  group by mdevice_type_id,mdevice_type_name");
                this.StoreDeviceType.DataBind();
                LedDataBind(" 1=1 ");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-SelectNode", ex.Message+"；"+ex.StackTrace, "SelectNode has an exception");
            }
        }

        /// <summary>
        /// 修改连接数据
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
                    Window4.Title = GetLangStr("CCTVStationManager73", "修改连接设备信息");
                    CmbDevice.AllowBlank = true;
                    CmbStationType.AllowBlank = true;
                    HiddenId.Value = id;
                    CmbStationType.Reset();
                    TxtQStationName.Reset();
                    CmbDevice.Reset();
                    CmbDeviceRecord.Reset();
                    TxtMasterId.Reset();
                    CmbDirection.Reset();
                    CmbStation.Reset();
                    MNumChannel.Reset();
                    RNumChannel.Reset(); ;
                    CmbIsuse.Reset();
                    TxtMasterId.Reset();
                    ButQuery.Hidden = true;
                    CmbStationType.Hidden = true;
                    TxtQStationName.Hidden = true;
                    CmbDevice.Hidden = true;
                    CmbDeviceRecord.Hidden = true;
                    CmbStation.Hidden = true;
                    CompositeField1.Hidden = true;

                    //StringBuilder mySqlBuild = new StringBuilder("STATION_ID,MCHANNELID,RCHANNELID,MASTERID,ISUSE,DIRECTION_ID");
                    //string mySql = mySqlBuild.ToString();

                    DataTable dt2 = deviceManager.GetCctvSetting("*", " id = '" + id + "'");

                    if (dt2 != null)
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            this.StoreDirection.DataSource = tgsPproperty.GetDirectionInfoByStation(dt2.Rows[0]["col1"].ToString());
                            this.StoreDirection.DataBind();
                            MNumChannel.Text = dt2.Rows[0]["col5"].ToString();
                            RNumChannel.Text = dt2.Rows[0]["col18"].ToString();
                            TxtMasterId.Text = dt2.Rows[0]["col31"].ToString();

                            CmbIsuse.Value = dt2.Rows[0]["col32"].ToString();
                            CmbDirection.Value = dt2.Rows[0]["col3"].ToString();
                        }
                    }
                    Button5.Hidden = false;
                    DataRow[] dr = dt2.Select("col0='"+id+"'");
                    jktdh = dr[0]["col5"].ToString();
                    hftdh = dr[0]["col18"].ToString();
                    ssfx = dr[0]["col4"].ToString();
                    jzbh = dr[0]["col31"].ToString();
                    DataRow[] dr1 = dtsfsy.Select("col0='" + dt2.Rows[0]["col32"].ToString() + "'");
                    sgqy = dr1[0]["col1"].ToString();
                    Window4.Show();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-Modify", ex.Message+"；"+ex.StackTrace, "Modify has an exception");
            }
        }

        /// <summary>
        /// 删除连接数据
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
                    X.Msg.Confirm(GetLangStr("CCTVStationManager74", "信息"), GetLangStr("CCTVStationManager75", "这样会删除设备连接信息，确认要删除这条记录吗？"), new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "CCTVStationManager.DoYes()",
                            Text = GetLangStr("CCTVStationManager76", "是")
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "CCTVStationManager.DoNo()",
                            Text = GetLangStr("CCTVStationManager77", "否")
                        }
                    }).Show();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-DoConfirm", ex.Message+"；"+ex.StackTrace, "DoConfirm has an exception");
            }
        }

        /// <summary>
        /// 确定删除
        /// </summary>
        [DirectMethod]
        public void DoYes()
        {
            try
            {
                RowSelectionModel sm = this.GridDeviceManager.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRow.ToBuilder();
                string Id = sm.SelectedRow.RecordID;

                DataRow[] dt = dtname.Select("col0='"+Id+"'");

                  string jcname= dt[0]["col2"].ToString();

                  DataRow[] d1 = dtname.Select("col0='" + Id + "'");

                  string sssb = dt[0]["col14"].ToString();

                if (deviceManager.DeleteCctvSetting(Id) > 0)
                {
                    //LedDataBind(" 1=1 and service_ip='" + HideIpaddress.Value.ToString() + "'");
                    LedDataBind(" 1=1");
                    sm.ClearSelections();
                    sm.UpdateSelection();


                    logManager.InsertLogRunning(uName, "删除：[" + jcname + "]监测点;实时设备:["+sssb+"]", nowIp, "3");

                    Notice(GetLangStr("CCTVStationManager49", "信息提示"), GetLangStr("CCTVStationManager78", "删除成功"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-DoYes", ex.Message+"；"+ex.StackTrace, "DoYes has an exception");
            }
        }

        /// <summary>
        /// 取消删除
        /// </summary>
        [DirectMethod]
        public void DoNo()
        {
        }

        #endregion [DirectMethod]

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
                logManager.InsertLogError("CCTVStationManager.aspx-GetdatabyField", ex.Message+"；"+ex.StackTrace, "GetdatabyField has an exception");
                return null;
            }
        }

        protected void SelectDev_Click(object sender, DirectEventArgs e)
        {
            try
            {
                string id = e.ExtraParams["id"];
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-SelectDev_Click", ex.Message+"；"+ex.StackTrace, "SelectDev_Click has an exception");
            }
        }

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
                root.Text = GetLangStr("CCTVStationManager71", "视频接入网关服务器");

                nodes.Add(root);

                Ext.Net.TreeNode node = new Ext.Net.TreeNode();

                DataTable dt2 = deviceManager.GetCctvSetting("sum(isuse)||'/'|| count(*)", " 1=1 ");
                string countvalue = "0";
                if (dt2 != null)
                {
                    if (dt2.Rows.Count > 0)
                    {
                        countvalue = dt2.Rows[0][0].ToString();
                    }
                    else
                    {
                        node.Disabled = true;
                    }
                }
                node.Text = GetLangStr("CCTVStationManager72", "视频接入点") + "(" + countvalue + ")";
                node.Listeners.Click.Handler = "CCTVStationManager.SelectNode() ;";
                node.Icon = Icon.Monitor;
                node.NodeID = "100000001";
                node.Expanded = true;
                root.Nodes.Add(node);

                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CCTVStationManager.aspx-BuildTree", ex.Message+"；"+ex.StackTrace, "BuildTree has an exception");
                return null;
            }
        }

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
                logManager.InsertLogError("CCTVStationManager.aspx-ConventDataTable", ex.Message+"；"+ex.StackTrace, "ConventDataTable has an exception");
                return null;
            }
        }

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
                logManager.InsertLogError("CCTVStationManager.aspx-Addree", ex.Message+"；"+ex.StackTrace, "Addree has an exception");
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
                logManager.InsertLogError("CCTVStationManager.aspx-GetLangStr", ex.Message + "；" + ex.StackTrace, "GetLangStr发生异常");
                return null;
            }
        }
    }
}