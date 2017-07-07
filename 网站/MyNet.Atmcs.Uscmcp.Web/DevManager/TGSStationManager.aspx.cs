using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class TGSStationManager : System.Web.UI.Page
    {
        #region 成员变量
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        private SettingManager settingManager = new SettingManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private MyNet.Atmcs.Uscmcp.Bll.DeviceManager deviceManager = new MyNet.Atmcs.Uscmcp.Bll.DeviceManager();
        private UserLogin userLogin = new UserLogin();


        static string uName;
        static string nowIp;
        /// <summary>
        ///转换是否文件扫描
        /// </summary>
        static DataTable dtsfqy=null;
        /// <summary>
        /// 转换是否启用
        /// </summary>
        static DataTable dttype = null;

        /// <summary>
        /// 获取设备名称
        /// </summary>
            static DataTable dtsbmc=null;
        /// <summary>
        /// 获取删除id
        /// </summary>
            static string deleId;
        /// <summary>
        /// 删除名称
        /// </summary>
            static string delName;
      
        /// <summary>
        /// 监测点类型
        /// </summary>
            static string jcdlx;
        /// <summary>
        /// 查询监测点
        /// </summary>
            static string cxjcd;
        /// <summary>
        /// 监测点名称
        /// </summary>
            static string jcdmc;
        /// <summary>
        /// 连接的设备
        /// </summary>
            static string ljdsb;
        /// <summary>
        /// 所属方向
        /// </summary>
            static string ssfx;
        /// <summary>
        /// 设备通道号
        /// </summary>
            static string sbtd;
        /// <summary>
        /// 是否文件扫描
        /// </summary>
            static string sfwjsm;
        /// <summary>
        /// 图片保存路径
        /// </summary>
            static string tpbclj;
        /// <summary>
        /// 本地连接ip
        /// </summary>
    
        static string bdljip;
        /// <summary>
        /// 本地连接端口
        /// </summary>
        static string bdljdk;
        /// <summary>
        /// 使用特殊规则
        /// </summary>
        static string sytrgz;
        /// <summary>
        /// 是否使用
        /// </summary>
        static string sfsy;
        static DataTable dtsygz = null;



        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
                    BuildTree(TreePanel1.Root);
                    ButExcel.Disabled = true;
                    //ButPrint.Disabled = true;
                    StoreDataBind();
                    this.DataBind();
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    uName = userinfo.UserName;
                    nowIp = userinfo.NowIp;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：卡口电警管理", userinfo.NowIp, "0");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSStationManager.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
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
                logManager.InsertLogError("TGSStationManager.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        /// <summary>
        /// 查询事件
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
                logManager.InsertLogError("TGSStationManager.aspx-ButQueryClick", ex.Message+"；"+ex.StackTrace, "ButQueryClick  has an exception");
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
                logManager.InsertLogError("TGSStationManager.aspx-ButRefreshClick", ex.Message+"；"+ex.StackTrace, "ButRefreshClick  has an exception");
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
                DataTable dt = deviceManager.GetDevice(" where 1=1 ");
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
                logManager.InsertLogError("TGSStationManager.aspx-MyData_Refresh", ex.Message+"；"+ex.StackTrace, "MyData_Refresh  has an exception");
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
                logManager.InsertLogError("TGSStationManager.aspx-ButResetClick", ex.Message+"；"+ex.StackTrace, "ButResetClick  has an exception");
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
                logManager.InsertLogError("TGSStationManager.aspx-MyData_Refresh", ex.Message+"；"+ex.StackTrace, "MyData_Refresh  has an exception");
            }
        }

        protected void ButDevAdd_Click(object sender, DirectEventArgs e)
        {
            try
            {
                HidSaveFlag.Value = "1";
                Button5.Hidden = false;
                Window4.Title = GetLangStr("TGSStationManager54", "添加连接设备");
                CmbDevice.AllowBlank = false;
                CmbStationType.AllowBlank = false;
                CmbStationType.Reset();
                TxtQStationName.Reset();
                MulCmbAppFlag.Reset();
                CmbDevice.Reset();
                CmbStation.Reset();
                NumChannel.Reset();
                CmbIsSacn.Reset();
                CmbDirection.Reset();
                TxtImagePath.Reset();
                TxtIpaddress.Reset();
                NumPort.Reset();
                CmbIsuse.Reset();
                ButQuery.Hidden = false;
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
                logManager.InsertLogError("TGSStationManager.aspx-ButDevAdd_Click", ex.Message+"；"+ex.StackTrace, "ButDevAdd_Click  has an exception");
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
                    string xml = Bll.Common.GetPrintXml(GetLangStr("TGSStationManager55", "服务器信息管理"), "", "", "printdatatable");
                    string js = "OpenPrintPageV(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSStationManager.aspx-ButPrintClick", ex.Message+"；"+ex.StackTrace, "ButPrintClick  has an exception");
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
                logManager.InsertLogError("TGSStationManager.aspx-ToXml", ex.Message+"；"+ex.StackTrace, "ToXml  has an exception");
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
                logManager.InsertLogError("TGSStationManager.aspx-ToExcel", ex.Message+"；"+ex.StackTrace, "ToExcel  has an exception");
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
                logManager.InsertLogError("TGSStationManager.aspx-ToCsv", ex.Message+"；"+ex.StackTrace, "ToCsv  has an exception");
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
                logManager.InsertLogError("TGSStationManager.aspx-Addree", ex.Message+"；"+ex.StackTrace, "Addree  has an exception");
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
                hs.Add("service_id", HideServiceId.Value.ToString());

                hs.Add("imagepath", TxtImagePath.Text.Replace(@"\",@"\\"));
                hs.Add("localip", TxtIpaddress.Text);
                if (!string.IsNullOrEmpty(NumPort.Text))
                {
                    hs.Add("localport", NumPort.Text);
                }
                else
                {
                    hs.Add("localport", "0");
                }
                if (CmbIsSacn.SelectedIndex != -1)
                {
                    hs.Add("isscanfile", CmbIsSacn.Value);
                }
                else
                {
                    Notice(GetLangStr("TGSStationManager56", "信息提示"), GetLangStr("TGSStationManager57", "请选择是否文件扫描"));
                    return;
                }
                if (CmbDirection.SelectedIndex != -1)
                {
                    hs.Add("direction_id", CmbDirection.Value);
                }
                else
                {
                    Notice(GetLangStr("TGSStationManager56", "信息提示"), GetLangStr("TGSStationManager58", "请选择所属方向"));
                    return;
                }
                if (CmbIsuse.SelectedIndex != -1)
                {
                    hs.Add("isuse", CmbIsuse.Value);
                }
                else
                {
                    Notice(GetLangStr("TGSStationManager56", "信息提示"), GetLangStr("TGSStationManager59", "请选择是否启用"));
                    return;
                }
                if (!string.IsNullOrEmpty(NumChannel.Text))
                {
                    hs.Add("channelid", NumChannel.Text);
                }
                else
                {
                    hs.Add("channelid", "0");
                }
                SelectedListItemCollection lists = this.MulCmbAppFlag.SelectedItems;
                string applyflag = string.Empty;
                if (lists.Count > 0)
                {
                    for (int i = 0; i < lists.Count; i++)
                    {
                        if (i == 0)
                        {
                            applyflag = applyflag + lists[i].Value;
                        }
                        else
                        {
                            applyflag = applyflag + "," + lists[i].Value;
                        }
                    }
                }
                hs.Add("applyflag", applyflag);
                if (HidSaveFlag.Value.ToString() == "1")
                {
                    if (CmbStation.SelectedIndex != -1)
                    {
                        hs.Add("station_id", CmbStation.Value);
                    }
                    else
                    {
                        Notice(GetLangStr("TGSStationManager56", "信息提示"), GetLangStr("TGSStationManager60", "请选择监测点名称"));
                        return;
                    }
                    if (CmbDevice.SelectedIndex != -1)
                    {
                        hs.Add("device_id", CmbDevice.Value);
                    }
                    else
                    {
                        Notice(GetLangStr("TGSStationManager56", "信息提示"), GetLangStr("TGSStationManager61", "请选择设备名称"));
                        return;
                    }
                    string Id = tgsPproperty.GetRecordId();
                    hs.Add("id", Id);
                    if (deviceManager.InsertDeviceSetting(hs) > 0)
                    {
                        LedDataBind(" 1=1 and service_id='" + HideServiceId.Value.ToString() + "'");
                        string lblname = "";
                        jcdlx = CmbStationType.SelectedItem.Text;
                        jcdmc = CmbStation.SelectedItem.Text;
                        ljdsb=CmbDevice.SelectedItem.Text;
                        lblname += Bll.Common.AssembleRunLog("",jcdlx,"监测点类型","0");
                        lblname += Bll.Common.AssembleRunLog("",ljdsb, "连接设备", "0");
                        logManager.InsertLogRunning(uName, "添加：监测点[" +jcdmc+ "]" + lblname, nowIp, "1");

                        Notice(GetLangStr("TGSStationManager56", "信息提示"), GetLangStr("TGSStationManager62", "添加成功"));

                        Window4.Hide();
                    }
                }
                if (HidSaveFlag.Value.ToString() == "2")
                {
                    RowSelectionModel sm = this.GridDeviceManager.SelectionModel.Primary as RowSelectionModel;
                    sm.SelectedRow.ToBuilder();
                    string Id = sm.SelectedRow.RecordID;
                  string  ddid = Id;
                    hs.Add("id", Id);
                    if (deviceManager.UptateDeviceSetting(hs) > 0)
                    {
                        Window4.Hide();
                        LedDataBind(" 1=1 and service_id='" + HideServiceId.Value.ToString() + "'");

                        DataRow[] dr = dtsbmc.Select("col0='" + ddid + "'");
                        if (dr.Length > 0)
                        {
                            delName = dr[0]["col3"].ToString();
                        }

                     string   ssfxs = CmbDirection.SelectedItem.Text;
                     string sbtds = NumChannel.Text;
                     string sfwjsms = CmbIsSacn.SelectedItem.Text;

                     string tpbcljs = TxtImagePath.Text;
                     string bdljips = TxtIpaddress.Text;
                     string bdljdks = NumPort.Text;
                      
                     string sfsys = CmbIsuse.SelectedItem.Text;
                     string sytrgzs = "";
                          string sygzs= GetMultiCombo(MulCmbAppFlag.SelectedItems);
                        if (sygzs.Contains(","))
                        {
                            string[] sygz = sygzs.Split(',');
                            for (int i = 0; i < sygz.Length; i++)
                            {
                                DataRow[] dt3 = dtsygz.Select("col0='" + sygz[i]+"'");
                                sytrgzs =sytrgz+ "["+dt3[0]["col1"].ToString()+"]";
                            }
                           
                        }
                        else
                        {
                            DataRow[] dt3 = dtsygz.Select("col0='" + sygzs + "'");
                           sytrgzs = sytrgz + "[" + dt3[0]["col1"].ToString() + "]";
                        }


                    

                         string lblname="";
                         lblname += Bll.Common.AssembleRunLog(ssfx,ssfxs,"所属方向","1");
                         lblname += Bll.Common.AssembleRunLog(sbtd, sbtds, "设备通道号", "1");
                         lblname += Bll.Common.AssembleRunLog(sfwjsm, sfwjsms, "是否文件扫描", "1");
                         lblname += Bll.Common.AssembleRunLog(tpbclj, tpbcljs, "图片保存路径", "1");
                         lblname += Bll.Common.AssembleRunLog(bdljip, bdljips, "本地连接ip", "1");
                         lblname += Bll.Common.AssembleRunLog(bdljdk, bdljdks, "本地连接端口", "1");
                         lblname += Bll.Common.AssembleRunLog(sfsy, sfsys, "是否启用", "1");
                         lblname += Bll.Common.AssembleRunLog(sytrgz, sytrgzs, "使用规则", "1");


                         logManager.InsertLogRunning(uName, "修改设备：[" + delName + "];"+lblname, nowIp, "2");
                        Notice(GetLangStr("TGSStationManager56", "信息提示"), GetLangStr("TGSStationManager63", "修改成功"));
                       
                    }
                }
               

            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSStationManager.aspx-UpdateDevice", ex.Message+"；"+ex.StackTrace, "UpdateDevice  has an exception");
            }
        }

        #endregion 控件事件

        #region DirectMethod

        /// <summary>
        /// 选中设备树事件
        /// </summary>
        [DirectMethod]
        public void SelectDevice()
        {
            try
            {
                string type = CmbStation.SelectedItem.Value;
                this.StoreDevice.DataSource = tgsPproperty.GetDeviceInfoByStation(" station_id = '" + type + "'");
                this.StoreDevice.DataBind();
                this.StoreDirection.DataSource = tgsPproperty.GetDirectionInfoByStation(type);
                this.StoreDirection.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSStationManager.aspx-SelectDevice", ex.Message+"；"+ex.StackTrace, "SelectDevice  has an exception");
            }
        }

        [DirectMethod]
        public void SelectNode(string service_id)
        {
            try
            {
                HideServiceId.Value = service_id;
                //this.StoreDeviceType.DataSource = deviceManager.GetTGSSetting("device_type_id,device_type_name", " service_id='" + service_id + "'  group by device_type_id,device_type_name");
                //this.StoreDeviceType.DataBind();
                LedDataBind(" 1=1 and service_id='" + service_id + "'");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSStationManager.aspx-SelectNode", ex.Message+"；"+ex.StackTrace, "SelectNode  has an exception");
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
                Ext.Net.SelectedListItem ss;
                RowSelectionModel sm = this.GridDeviceManager.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string id = sm.SelectedRow.RecordID;
                    HidSaveFlag.Value = "2";
                    Window4.Title = GetLangStr("TGSStationManager64", "修改连接设备信息");
                    CmbDevice.AllowBlank = true;
                    CmbStationType.AllowBlank = true;
                    HiddenId.Value = id;
                    CmbStationType.Reset();
                    MulCmbAppFlag.Reset();
                    TxtQStationName.Reset();
                    CmbDevice.Reset();
                    CmbDirection.Reset();
                    CmbStation.Reset();
                    NumChannel.Reset();
                    CmbIsSacn.Reset();
                    TxtImagePath.Reset();
                    TxtIpaddress.Reset();
                    NumPort.Reset();
                    CmbIsuse.Reset();
                    ButQuery.Hidden = true;
                    CmbStationType.Hidden = true;
                    TxtQStationName.Hidden = true;
                    CmbDevice.Hidden = true;
                    CmbStation.Hidden = true;
                    CompositeField1.Hidden = true;
                    DataTable dt2 = deviceManager.GetTGSSetting("*", " id = '" + id + "'");
                    MulCmbAppFlag.SelectedItems.Clear();
                    if (dt2 != null)
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            this.StoreDirection.DataSource = tgsPproperty.GetDirectionInfoByStation(dt2.Rows[0]["col2"].ToString());
                            this.StoreDirection.DataBind();
                            NumChannel.Text = dt2.Rows[0]["col8"].ToString();
                            CmbIsSacn.Value = dt2.Rows[0]["col10"].ToString();
                            TxtImagePath.Text = dt2.Rows[0]["col9"].ToString();
                            TxtIpaddress.Text = dt2.Rows[0]["col14"].ToString();
                            string port = dt2.Rows[0]["col13"].ToString();
                            if (!string.IsNullOrEmpty(port))
                            {
                                NumPort.Text = dt2.Rows[0]["col13"].ToString();
                            }
                            CmbIsuse.Value = dt2.Rows[0]["col15"].ToString();
                            CmbDirection.Value = dt2.Rows[0]["col16"].ToString();
                            string appflag = dt2.Rows[0]["col20"].ToString();
                            string[] appflags = appflag.Split(',');
                            for (int i = 0; i < appflags.Length; i++)
                            {
                                Ext.Net.SelectedListItem si = new SelectedListItem(appflags[i]);
                                ss = si;
                                MulCmbAppFlag.SelectedItems.Add(si);
                            }
                            MulCmbAppFlag.Render();
                        }
                    }
                    DataRow[] dr = dtsbmc.Select("col0='" +id+ "'");
                        sbtd = NumChannel.Text;

                        DataRow[] drs = dtsfqy.Select("col0='" + dt2.Rows[0]["col10"].ToString() + "'");
                        //CmbIsSacn.SelectedItem.Text; 
                        sfwjsm=drs[0]["col1"].ToString();
                        DataRow[] dr2 = dttype.Select("col0='" + dt2.Rows[0]["col15"].ToString() + "'");
                        sfsy = dr2[0]["col1"].ToString();
                        tpbclj   = TxtImagePath.Text;
                        bdljip = TxtIpaddress.Text;
                        bdljdk = NumPort.Text;
                        ssfx = dt2.Rows[0]["col17"].ToString();
                        string sygzs= GetMultiCombo(MulCmbAppFlag.SelectedItems);
                        if (sygzs.Contains(","))
                        {
                            string[] sygz = sygzs.Split(',');
                            for (int i = 0; i < sygz.Length; i++)
                            {
                                DataRow[] dt3 = dtsygz.Select("col0='" + sygz[i]+"'");
                                sytrgz =sytrgz+ "["+dt3[0]["col1"].ToString()+"]";
                            }
                           
                        }
                        else
                        {
                            DataRow[] dt3 = dtsygz.Select("col0='" + sygzs + "'");
                            sytrgz = sytrgz + "[" + dt3[0]["col1"].ToString() + "]";
                        }

                       //sytrgz = MulCmbAppFlag.Value.ToString();
                    Button5.Hidden = false;
                    Window4.Show();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSStationManager.aspx-Modify", ex.Message+"；"+ex.StackTrace, "Modify  has an exception");
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
            
                for (int i = 0; i < sic.Count; i++)
                {
                    if (i == sic.Count - 1)
                    {
                        kkid = kkid + sic[i].Value;
                    }
                    else
                    {
                        kkid = kkid + sic[i].Value + ",";
                    }
                  
                }
              

                return kkid;
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("TGSStationManager.aspx-GetMultiCombo", ex.Message + "；" + ex.StackTrace, "GetMultiCombo has an exception");
                ILog.WriteErrorLog(ex);
                return "";
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
                    X.Msg.Confirm(GetLangStr("TGSStationManager65", "信息"), GetLangStr("TGSStationManager66", "这样会删除服务器信息，确认要删除这台设备吗？"), new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "TGSStationManager.DoYes()",
                            Text = GetLangStr("TGSStationManager67", "是")
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "TGSStationManager.DoNo()",
                            Text = GetLangStr("TGSStationManager68", "否")
                        }
                    }).Show();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSStationManager.aspx-DoConfirm", ex.Message+"；"+ex.StackTrace, "DoConfirm  has an exception");
            }
        }

        /// <summary>
        /// 删除确定事件
        /// </summary>
        [DirectMethod]
        public void DoYes()
        {
            try
            {
                RowSelectionModel sm = this.GridDeviceManager.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRow.ToBuilder();
                string Id = sm.SelectedRow.RecordID;
                deleId = Id;
                DataRow[] dr = dtsbmc.Select("col0='" + deleId + "'");
                if (dr.Length>0)
                {
                    delName = dr[0]["col3"].ToString();
                }
                if (deviceManager.DeleteDeviceSetting(Id) > 0)
                {
                    LedDataBind(" 1=1 and service_id='" + HideServiceId.Value.ToString() + "'");
                    logManager.InsertLogRunning(uName, "删除：[" + delName + "]设备", nowIp, "3");
                    Notice(GetLangStr("TGSStationManager56", "信息提示"), GetLangStr("TGSStationManager69", "删除成功"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSStationManager.aspx-DoYes", ex.Message+"；"+ex.StackTrace, "DoYes  has an exception");
            }
        }

        /// <summary>
        /// 不删除事件
        /// </summary>
        [DirectMethod]
        public void DoNo()
        {
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
                ///是否使用
                this.StoreisUse.DataSource = dttype=dtsfqy= Bll.Common.ChangColName(tgsPproperty.GetCommonDict("240034"));
                this.StoreisUse.DataBind();
                ///
                this.StoreAppFlag.DataSource= dtsygz= Bll.Common.ChangColName(tgsPproperty.GetCommonDict("240047"));
                this.StoreAppFlag.DataBind();
                this.StoreDeviceType.DataSource = deviceManager.GetDeviceType();
                this.StoreDeviceType.DataBind();
                this.StoreStationType.DataSource = Bll.Common.ChangColName(tgsPproperty.GetStationTypeInfo("station_type_id in('01','02','03') and isuse='1'"));
                this.StoreStationType.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSStationManager.aspx-StoreDataBind", ex.Message+"；"+ex.StackTrace, "StoreDataBind  has an exception");
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
                PagingToolbar1.PageSize = 50;
                DataTable dt = deviceManager.GetTGSSetting("*", where);
                StoreDeviceManager.DataSource = dt;
                StoreDeviceManager.DataBind();
                dtsbmc = dt;
                Session["datatable"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    ButExcel.Disabled = false;
                    //ButPrint.Disabled = false;
                }
                else
                {
                    ButExcel.Disabled = true;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSStationManager.aspx-LedDataBind", ex.Message+"；"+ex.StackTrace, "LedDataBind  has an exception");
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
                if (dt != null)
                {
                    //PrintColumns pc = new PrintColumns();
                    //pc.Add(new PrintColumn("设备编号", 1));
                    //pc.Add(new PrintColumn("设备名称", 2));
                    //pc.Add(new PrintColumn("设备类型", 3));
                    //pc.Add(new PrintColumn("设备型号", 4));
                    //dt2 = Bll.Common.GetDataTablePrint(dt, pc);
                    dt1.Columns.Remove("col0"); dt1.Columns.Remove("col1"); dt1.Columns.Remove("col2"); dt1.Columns.Remove("col4"); dt1.Columns.Remove("col10");
                    dt1.Columns.Remove("col11"); dt1.Columns.Remove("col12"); dt1.Columns.Remove("col13"); dt1.Columns.Remove("col14"); dt1.Columns.Remove("col15");
                    dt1.Columns.Remove("col16"); dt1.Columns.Remove("col18"); dt1.Columns.Remove("col20"); dt1.Columns.Remove("col21"); dt1.Columns.Remove("col23");
                    dt1.Columns.Remove("col24");
                    dt1.Columns["col3"].SetOrdinal(0); dt1.Columns["col17"].SetOrdinal(1); dt1.Columns["col5"].SetOrdinal(2); dt1.Columns["col19"].SetOrdinal(3); dt1.Columns["col22"].SetOrdinal(4);
                    dt1.Columns["col6"].SetOrdinal(5); dt1.Columns["col7"].SetOrdinal(6); dt1.Columns["col8"].SetOrdinal(7); dt1.Columns["col9"].SetOrdinal(8); dt1.Columns["col25"].SetOrdinal(9);

                    dt1.Columns[0].ColumnName = GetLangStr("TGSStationManager70", "地点");
                    dt1.Columns[1].ColumnName = GetLangStr("TGSStationManager71", "所属方向");
                    dt1.Columns[2].ColumnName = GetLangStr("TGSStationManager72", "设备名称");
                    dt1.Columns[3].ColumnName = GetLangStr("TGSStationManager73", "设备类型");
                    dt1.Columns[4].ColumnName = GetLangStr("TGSStationManager74", "设备型号");
                    dt1.Columns[5].ColumnName = GetLangStr("TGSStationManager75", "设备IP");
                    dt1.Columns[6].ColumnName = GetLangStr("TGSStationManager76", "设备端口");
                    dt1.Columns[7].ColumnName = GetLangStr("TGSStationManager77", "设备通道");
                    dt1.Columns[8].ColumnName = GetLangStr("TGSStationManager78", "图片路径");
                    dt1.Columns[9].ColumnName = GetLangStr("TGSStationManager79", "外部编号");
                }
                return dt1;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSStationManager.aspx-ChangeDataTable", ex.Message+"；"+ex.StackTrace, "ChangeDataTable  has an exception");
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
                    where = where + " and     device_name like '%" + TxtDeviceName.Text + "%'";
                }
                if (CmbDeviceType.SelectedIndex != -1)
                {
                    where = where + " and  device_type_id='" + CmbDeviceType.SelectedItem.Value + "' ";
                }
                if (HideServiceId.Value != null)
                {
                    where = where + " and  service_id='" + HideServiceId.Value.ToString() + "' ";
                }
                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSStationManager.aspx-GetWhere", ex.Message+"；"+ex.StackTrace, "GetWhere  has an exception");
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
                logManager.InsertLogError("TGSStationManager.aspx-GetdatabyField", ex.Message+"；"+ex.StackTrace, "GetdatabyField  has an exception");
                return "";
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
                root.Text = GetLangStr("TGSStationManager80", "卡口接入服务器信息（使用/总计）");

                nodes.Add(root);

                DataTable dt = deviceManager.GetServer(" server_type_id='1002' ");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    string code = dt.Rows[i]["col0"].ToString();
                    DataTable dt2 = deviceManager.GetTGSSetting("IFNULL(COUNT(*),0)", "service_id='" + code + "'");
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
                    node.Text = dt.Rows[i]["col1"].ToString() + "(" + countvalue + ")";
                    node.Listeners.Click.Handler = "TGSStationManager.SelectNode('" + code + "') ;";
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
                logManager.InsertLogError("TGSStationManager.aspx-BuildTree", ex.Message+"；"+ex.StackTrace, "BuildTree  has an exception");
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
                logManager.InsertLogError("TGSStationManager.aspx-ConventDataTable", ex.Message+"；"+ex.StackTrace, "ConventDataTable  has an exception");
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
                logManager.InsertLogError("TGSStationManager.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice 发生异常");
                throw;
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
                logManager.InsertLogError("TGSStationManager.aspx-GetLangStr", ex.Message + "；" + ex.StackTrace, "GetLangStr发生异常");
                return null;
            }
        }

        #endregion 私有方法
    }
}