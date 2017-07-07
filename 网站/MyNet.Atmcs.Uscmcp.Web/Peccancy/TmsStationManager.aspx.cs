using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class TmsStationManager : System.Web.UI.Page
    {
        #region 成员变量

        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private SettingManager settingManager = new SettingManager();
        private UserLogin userLogin = new UserLogin();

        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username=Request.QueryString["username"];  
            if (!userLogin.CheckLogin(username)) 
            {
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" +StaticInfo.LoginPage + "'"; 
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                GetStationData();
                GetDeviceData();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddStation(object sender, DirectEventArgs e)
        {
            try
            {
                AddWindowsStation();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddDevice(object sender, DirectEventArgs e)
        {
            try
            {
                AddWindowDevice();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 选中数据行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowSelect(object sender, DirectEventArgs e)
        {
            try
            {
                string id = e.ExtraParams["id"];
                string stationid = e.ExtraParams["sid"];
                string departid = e.ExtraParams["departid"];
                Session["stationid"] = stationid;
                string js = "ClearCheckState();";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
                GetDirectionCheck(stationid);
                this.CmbLocation.Value = e.ExtraParams["location_id"];
                this.CmbDepartment.Value = e.ExtraParams["departid"];
                this.ComStationType.Value = e.ExtraParams["station_type"];
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
                GetStationData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void StoreDevice_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                GetDeviceData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        #endregion 控件事件

        #region DirectMethod

        /// <summary>
        /// 不删除事件
        /// </summary>
        [DirectMethod]
        public void DoNo()
        {
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void SelectDepartEvent()
        {
            try
            {
                X.GetCmp<ComboBox>("cmbELocation").Text = "";
                DataTable location = settingManager.GetLocationInfoByDepartId(X.GetCmp<ComboBox>("cmbEDepartment").SelectedItem.Value);
                this.StoreLocation.DataSource = location;
                this.StoreLocation.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void UpdateDirection()
        {
            try
            {
                Hashtable hs;
                string stationid = Session["stationid"] as string;
                if (string.IsNullOrEmpty(stationid))
                {
                    Message("信息提示", "请选择需要配置方向的电子警察监测位！");
                    return;
                }
                List<string> lst = new List<string>();

                if (!string.IsNullOrEmpty(GridData.Text))
                {
                    string[] str = GridData.Text.Split(',');
                    lst.AddRange(str);
                }
                if (lst.Count > 0)
                {
                    hs = new Hashtable();
                    hs.Add("station_id", stationid);
                    settingManager.DeleteDirectionInfo(hs);
                    for (int i = 0; i < lst.Count; i++)
                    {
                        hs = new Hashtable();
                        hs.Add("station_id", stationid);
                        hs.Add("direction_id", lst[i].ToString());
                        hs.Add("direction_name", ChageDirection(lst[i].ToString()));
                        hs.Add("direction_desc", ChageDirection(lst[i].ToString()));
                        settingManager.InsertDirectionInfo(hs);
                    }
                    Notice("信息提示", "保存成功");
                    GetStationData();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void InfoSave()
        {
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("station_id", X.GetCmp<TextField>("txtEStationID").Text);
                hs.Add("station_name", X.GetCmp<TextField>("txtEStationName").Text + Simple(X.GetCmp<ComboBox>("cmdEStationType").Value.ToString()));
                hs.Add("station_type_id", X.GetCmp<ComboBox>("cmdEStationType").Value);
                hs.Add("location_id", X.GetCmp<ComboBox>("cmbELocation").Value);
                hs.Add("datasource", ChageDataSource(X.GetCmp<ComboBox>("cmdEStationType").Value.ToString()));
                hs.Add("description", X.GetCmp<TextField>("txtEDescription").Text);
                hs.Add("departid", X.GetCmp<ComboBox>("cmbEDepartment").Value);
                hs.Add("big_limit_speed", "0");
                hs.Add("small_limit_speed", "0");
                hs.Add("big_limit_low_speed", "0");
                hs.Add("small_limit_low_speed", "0");
                hs.Add("reality_big_limit_speed", "0");
                hs.Add("reality_small_limit_speed", "0");
                hs.Add("isshow", IsShow(X.GetCmp<ComboBox>("cmdEStationType").Value.ToString()));
                hs.Add("istgsshow", X.GetCmp<ComboBox>("cmdEStationShow").Value);
                hs.Add("id", tgsDataInfo.GetTgsRecordId());
                if (tgsPproperty.InsertStationInfoByKK(hs) > 0)
                {
                    Notice("信息提示", "保存成功");
                    X.GetCmp<Window>("StationAdd").Hide();
                    GetStationData();
                    GetDeviceData();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void SelectStationEvent()
        {
            try
            {
                X.GetCmp<ComboBox>("cmbELocation").HideIndicator();
                X.GetCmp<Ext.Net.Button>("butSaveEdit").Hidden = false;
                X.GetCmp<ComboBox>("cmdEStationType").SelectedItem.Value = "01";
                X.GetCmp<ComboBox>("cmdEStationShow").SelectedItem.Value = "1";
                X.GetCmp<TextField>("txtEStationID").Text = X.GetCmp<ComboBox>("cmbELocation").SelectedItem.Value;
                X.GetCmp<TextField>("txtEStationName").Text = X.GetCmp<ComboBox>("cmbELocation").SelectedItem.Text;
                X.GetCmp<TextField>("txtEDescription").Text = X.GetCmp<ComboBox>("cmbELocation").SelectedItem.Text;
                if (tgsDataInfo.GeXhExist("t_cfg_set_station", "station_id", X.GetCmp<ComboBox>("cmbELocation").SelectedItem.Value) > 0)
                {
                    X.GetCmp<TextField>("txtEStationID").Text = tgsDataInfo.GetTgsRecordId();
                }
                if (tgsDataInfo.GeXhExist("t_cfg_set_station", "station_name||station_type_id", X.GetCmp<TextField>("txtEStationName").Text + X.GetCmp<ComboBox>("cmdEStationType").SelectedItem.Value) > 0)
                {
                    Message("信息提示", "[" + X.GetCmp<TextField>("txtEStationName").Text + "]的监测点已存在,如果继续增加请填写不同的监测点名称！");
                    X.GetCmp<TextField>("txtEStationID").Text = tgsDataInfo.GetTgsRecordId();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void SelectDeviceEvent()
        {
            try
            {
                X.GetCmp<ComboBox>("cmbEDeviceStation").HideIndicator();
                X.GetCmp<Ext.Net.Button>("butSaveEditDevice").Hidden = false;
                X.GetCmp<TextField>("txtEDeviceName").Text = X.GetCmp<ComboBox>("cmbEDeviceStation").SelectedItem.Text;
                X.GetCmp<ComboBox>("cmdEDeviceType").SelectedItem.Value = "01";
                X.GetCmp<ComboBox>("cmdEDeviceScan").SelectedItem.Value = "1";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void SelectDirectionEvent()
        {
            try
            {
                X.GetCmp<ComboBox>("cmbEDeviceDirection").HideIndicator();
                if (tgsDataInfo.GeXhExist("t_tgs_set_device", "device_name", X.GetCmp<TextField>("txtEDeviceName").Text + "(" + X.GetCmp<ComboBox>("cmbEDeviceDirection").SelectedItem.Text + ")") > 0)
                {
                    Message("信息提示", "[" + X.GetCmp<TextField>("txtEDeviceName").Text + "(" + X.GetCmp<ComboBox>("cmbEDeviceDirection").SelectedItem.Text + ")]的设备已存在,如果继续增加请填写不同的设备名称！");

                    return;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        #endregion DirectMethod

        #region 私有方法

        /// <summary>
        ///
        /// </summary>
        private void GetStationData()
        {
            try
            {
                DataTable objData = tgsPproperty.GetStationInfoView_ByDJ();
                if (objData != null)
                {
                    this.StoreStationId.DataSource = objData;
                    this.StoreStationId.DataBind();
                    if (objData.Rows.Count > 0)
                    {
                        DataTable fxbh = tgsPproperty.GetDirectionInfoByStation2(objData.Rows[0][1].ToString());
                        BuildTree(TreePanel1.Root, fxbh);
                        DataTable location = settingManager.GetLocationInfoByDepartId(objData.Rows[0][6].ToString());
                        this.StoreLocation.DataSource = location;
                        this.StoreLocation.DataBind();
                    }
                    else
                    {
                        BuildTree(TreePanel1.Root, null);
                    }
                }

                DataTable dept = tgsPproperty.GetDepartmentDict();
                this.StoreCombo.DataSource = dept;
                this.StoreCombo.DataBind();

                DataTable data = GetRedisData.GetData("t_sys_code:240026");//tgsPproperty.GetDeviceTypeDict("240026");
                this.StoreType.DataSource = data;
                this.StoreType.DataBind();

                this.StoreDirection.DataSource = GetRedisData.GetData("t_sys_code:240025"); //tgsPproperty.GetDeviceTypeDict("240025");
                this.StoreDirection.DataBind();

                this.StoreCamera.DataSource = GetRedisData.GetData("t_sys_code:250002"); //tgsPproperty.GetCommonDict("250002");
                this.StoreCamera.DataBind();

                this.StoreShow.DataSource = GetRedisData.GetData("t_sys_code:240034");//tgsPproperty.GetCommonDict("240034");
                this.StoreShow.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="dtRole"></param>
        private void BuildTree(Ext.Net.TreeNodeCollection nodes, DataTable dtRole)
        {
            try
            {
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }
                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Text = "Root";
                nodes.Add(root);
                root.Expanded = true;
                DataTable dt = tgsPproperty.GetCommonDict("240025");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    node.Text = dt.Rows[i][1].ToString();
                    node.Leaf = true;
                    node.Icon = Icon.UserKey;
                    node.Checked = ThreeStateBool.False;
                    if (dtRole != null)
                    {
                        DataRow[] drs = dtRole.Select("col1='" + dt.Rows[i][1].ToString() + "'");
                        if (drs.Length > 0)
                        {
                            node.Checked = ThreeStateBool.True;
                        }
                        else
                        {
                            node.Checked = ThreeStateBool.False;
                        }
                    }
                    else
                    {
                        node.Checked = ThreeStateBool.False;
                    }
                    node.NodeID = dt.Rows[i][0].ToString();
                    node.Expanded = true;
                    root.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void GetDeviceData()
        {
            try
            {
                DataTable dt = tgsDataInfo.GetDeviceInfo("1=1 and substr(isshow,2,1)='1'");
                if (dt != null)
                {
                    StoreDevice.DataSource = dt;
                    StoreDevice.DataBind();
                }
                DataTable dt1 = GetRedisData.ChangColName(GetRedisData.GetData("t_sys_code:240014"), true);
                StoreCompany.DataSource = dt1; //tgsPproperty.GetCompanyDict();
                StoreCompany.DataBind();

                DataTable tgs = GetRedisData.ChangColName(ToDataTable(GetRedisData.GetData("Station:t_cfg_set_station").Select("", "station_name asc")), true);//tgsPproperty.GetTmsStationInfo();
                // tgs.Columns.RemoveAt(0);
                StoreStaion.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("Station:t_cfg_set_station"));
                StoreStaion.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        public DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stationid"></param>
        private void GetDirectionCheck(string stationid)
        {
            try
            {
                DataTable dt = tgsPproperty.GetDirectionInfoByStation2(stationid);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string str = dt.Rows[i][0].ToString();
                        string js = "SetCheckState(\"" + str + "\");";
                        this.ResourceManager1.RegisterAfterClientInitScript(js);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 提示信息
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

        #region 增加监测点

        /// <summary>
        ///
        /// </summary>
        private void AddWindowsStation()
        {
            try
            {
                Window window = new Window();
                window.ID = "StationAdd";
                window.Title = "监测点增加";
                window.Width = Unit.Pixel(400);
                window.Height = Unit.Pixel(300);

                window.Collapsible = true;
                window.Maximizable = false;
                window.Resizable = false;
                window.Hidden = true;

                window.Layout = "Fit";
                window.AutoLoad.Mode = LoadMode.Merge;
                Ext.Net.FormPanel tab = new Ext.Net.FormPanel();
                tab.Title = "监测点信息";
                tab.Header = false;
                tab.Padding = 5;
                tab.DefaultAnchor = "98%";
                tab.AnchorVertical = "100%";
                tab.MonitorValid = true;

                tab.Listeners.ClientValidation.Handler = "butSaveEdit.setDisabled(!valid);";
                ComboBox cmbDepartment = CommonExt.AddComboBox("cmbEDepartment", "所属机构", "StoreCombo", "请选择所属机构", false);
                cmbDepartment.Listeners.Select.Handler = "SystemStation.SelectDepartEvent()";
                tab.Items.Add(cmbDepartment);
                ComboBox cmbLocation = CommonExt.AddComboBox("cmbELocation", "所属地点", "StoreLocation", "请选择所属地点", false);
                cmbLocation.Listeners.Select.Handler = "SystemStation.SelectStationEvent()";
                tab.Items.Add(cmbLocation);
                tab.Items.Add(CommonExt.AddTextField("txtEStationID", "监测点编号", false, "例:[2012030010]"));
                tab.Items.Add(CommonExt.AddTextField("txtEStationName", "监测点名称", false, "例:[八达岭高速昌平卡口]"));
                tab.Items.Add(CommonExt.AddComboBox("cmdEStationType", "监测点类型", "StoreType", "请选择监测点类型", false));
                tab.Items.Add(CommonExt.AddComboBox("cmdEStationShow", "卡口系统显示", "StoreShow", "请选择是/否", false));
                tab.Items.Add(CommonExt.AddTextField("txtEDescription", "描述"));

                window.Buttons.Add(CommonExt.AddButton("butSaveEdit", "保存", "Disk", "SystemStation.InfoSave()"));
                window.Buttons.Add(CommonExt.AddButton("butCancelEdit", "取消", "Cancel", window.ClientID + ".hide()"));
                window.Items.Add(tab);
                window.Render(this.Form);
                window.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateStationData(object sender, DirectEventArgs e)
        {
            try
            {
                Hashtable hs = new Hashtable();
                RowSelectionModel sm = this.GridStation.SelectionModel.Primary as RowSelectionModel;
                string Id = sm.SelectedRow.RecordID;
                hs.Add("id", Id);
                hs.Add("station_id", TxtStationID.Text);
                hs.Add("station_name", TxtStationName.Text);
                hs.Add("description", TxtStationDesc.Text);

                if (CmbLocation.SelectedIndex != -1)
                {
                    hs.Add("location_id", CmbLocation.Value);
                }

                if (CmbDepartment.SelectedIndex != -1)
                {
                    hs.Add("departid", CmbDepartment.Value);
                }
                if (ComStationType.SelectedIndex != -1)
                {
                    hs.Add("station_type_id", ComStationType.Value);
                    hs.Add("datasource", ChageDataSource(ComStationType.Value.ToString()));
                    hs.Add("isshow", IsShow(ComStationType.Value.ToString()));
                }
                if (CmbStationShow.SelectedIndex != -1)
                {
                    hs.Add("istgsshow", CmbStationShow.Value);
                }

                hs.Add("big_limit_speed", "0");
                hs.Add("small_limit_speed", "0");
                hs.Add("big_limit_low_speed", "0");
                hs.Add("small_limit_low_speed", "0");
                hs.Add("reality_big_limit_speed", "0");
                hs.Add("reality_small_limit_speed", "0");

                if (tgsPproperty.UpdateStationInfo(hs) > 0)
                {
                    Notice("监测点信息提示", "更新成功");
                }
                GetStationData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void DoYes()
        {
            try
            {
                RowSelectionModel sm = this.GridStation.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRow.ToBuilder();
                Hashtable hs = new Hashtable();
                string Id = sm.SelectedRow.RecordID;
                int idx = sm.SelectedRow.RowIndex;
                hs.Add("id", Id);
                hs.Add("station_id", Session["stationid"] as string);
                if (tgsPproperty.DeleteStationInfo(hs) > 0)
                {
                    Notice("信息提示", "删除成功");
                    GridStation.DeleteSelected();
                    GetDeviceData();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void DoConfirm()
        {
            RowSelectionModel sm = this.GridStation.SelectionModel.Primary as RowSelectionModel;
            string Id = sm.SelectedRow.RecordID;
            X.Msg.Confirm("信息", "确认要删除这条记录吗?", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = "SystemStation.DoYes()",
                    Text = "是"
                },
                No = new MessageBoxButtonConfig
                {
                    Handler = "SystemStation.DoNo()",
                    Text = "否"
                }
            }).Show();
        }

        #endregion 增加监测点

        #region 增加设备

        /// <summary>
        ///
        /// </summary>
        private void AddWindowDevice()
        {
            try
            {
                Window window = new Window();
                window.ID = "DeviceAdd";
                window.Title = "设备信息增加";
                window.Width = Unit.Pixel(400);
                window.Height = Unit.Pixel(420);
                window.Modal = true;
                window.Collapsible = true;
                window.Maximizable = false;
                window.Resizable = false;
                window.Hidden = true;
                window.Layout = "Fit";
                window.AutoLoad.Mode = LoadMode.Merge;
                Ext.Net.FormPanel tab = new Ext.Net.FormPanel();
                tab.Title = "设备信息";
                tab.Header = false;
                tab.Padding = 5;
                tab.DefaultAnchor = "98%";
                tab.AnchorVertical = "100%";
                tab.MonitorValid = true;
                tab.Listeners.ClientValidation.Handler = "butSaveEditDevice.setDisabled(!valid);";
                ComboBox cmbDeviceStation = CommonExt.AddComboBox("cmbEDeviceStation", "所属监测点", "StoreStaion", "请选择所属监测点", false);
                cmbDeviceStation.Listeners.Select.Handler = "SystemStation.SelectDeviceEvent()";
                tab.Items.Add(cmbDeviceStation);
                tab.Items.Add(CommonExt.AddTextField("txtEDeviceName", "设备名称", false, "例:[八达岭高速昌平电警抓拍机]"));
                ComboBox cmbDirection = CommonExt.AddComboBox("cmbEDeviceDirection", "所属方向", "StoreDirection", "请选择所属方向", false);
                cmbDirection.Listeners.Select.Handler = "SystemStation.SelectDirectionEvent()";
                tab.Items.Add(cmbDirection);
                tab.Items.Add(CommonExt.AddComboBox("cmdEDeviceType", "设备类型", "StoreType", "请选择设备类型", false));
                tab.Items.Add(CommonExt.AddTextField("txtEDeviceIP", "设备IP地址", false, "例:[192.168.0.1]"));
                tab.Items.Add(CommonExt.AddTextField("txtEDeviceSport", "设备端口", false, "例:[18000]"));
                tab.Items.Add(CommonExt.AddComboBox("cmdEDeviceCompany", "设备厂家", "StoreCompany", "请选择设备厂家", false));
                tab.Items.Add(CommonExt.AddComboBox("cmdEDeviceCamera", "相机类型", "StoreCamera", "请选择相机类型", false));
                tab.Items.Add(CommonExt.AddComboBox("cmdEDeviceScan", "是否扫描文件", "StoreShow", "请选择是/否", false));
                tab.Items.Add(CommonExt.AddTextField("txtEDeviceServiceIP", "图片保存服务器IP", false, "例:[192.168.0.1]"));
                tab.Items.Add(CommonExt.AddTextField("txtEDeviceImagePath", "图片保存路径", false, "例:[D:\\CAPTURE\\192.168.0.1]"));
                window.Buttons.Add(CommonExt.AddButton("butSaveEditDevice", "保存", "Disk", "SystemStation.InfoSaveDevice()"));
                window.Buttons.Add(CommonExt.AddButton("butCancelEditDevice", "取消", "Cancel", window.ClientID + ".hide()"));
                window.Items.Add(tab);
                window.Render(this.Form);
                window.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void InfoSaveDevice()
        {
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("id", tgsDataInfo.GetTgsRecordId());
                hs.Add("device_id", X.GetCmp<ComboBox>("cmbEDeviceStation").SelectedItem.Value + X.GetCmp<ComboBox>("cmbEDeviceDirection").Value);
                hs.Add("device_name", X.GetCmp<TextField>("txtEDeviceName").Text);
                hs.Add("device_ip", X.GetCmp<TextField>("txtEDeviceIP").Value);
                hs.Add("device_port", X.GetCmp<TextField>("txtEDeviceSport").Text);
                hs.Add("company_id", X.GetCmp<ComboBox>("cmdEDeviceCompany").Value);
                hs.Add("camera_type_id", X.GetCmp<ComboBox>("cmdEDeviceCamera").Value);
                hs.Add("device_type_id", X.GetCmp<ComboBox>("cmdEDeviceType").Value);
                hs.Add("station_id", X.GetCmp<ComboBox>("cmbEDeviceStation").Value);
                hs.Add("service_ip", X.GetCmp<TextField>("txtEDeviceServiceIP").Value);
                hs.Add("imagepath", X.GetCmp<TextField>("txtEDeviceImagePath").Value);
                hs.Add("isshow", IsShow(X.GetCmp<ComboBox>("cmdEDeviceType").Value.ToString()));
                hs.Add("isscanfile", X.GetCmp<ComboBox>("cmdEDeviceScan").Value);
                if (tgsDataInfo.InsertDeviceInfo(hs) > 0)
                {
                    Notice("信息提示", "保存成功");
                    X.GetCmp<Window>("DeviceAdd").Hide();
                    GetDeviceData();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateDeviceData(object sender, DirectEventArgs e)
        {
            try
            {
                Hashtable hs = new Hashtable();
                RowSelectionModel sm = this.GridDevice.SelectionModel.Primary as RowSelectionModel;
                string Id = sm.SelectedRow.RecordID;
                hs.Add("id", Id);
                hs.Add("device_id", txtDeviceID.Text);
                hs.Add("device_name", txtDeviceName.Text);
                hs.Add("device_ip", txtDeviceIP.Text);
                hs.Add("device_port", txtDeviceSport.Text);
                if (CmbDeviceCompany.SelectedIndex != -1)
                {
                    hs.Add("company_id", CmbDeviceCompany.Value);
                }
                if (CmbDeviceCamera.SelectedIndex != -1)
                {
                    hs.Add("camera_type_id", CmbDeviceCamera.Value);
                }
                if (CmbDeviceType.SelectedIndex != -1)
                {
                    hs.Add("device_type_id", CmbDeviceType.Value);
                    hs.Add("isshow", IsShow(CmbDeviceType.Value.ToString()));
                }
                if (CmbDeviceStation.SelectedIndex != -1)
                {
                    hs.Add("station_id", CmbDeviceStation.Value);
                }

                if (CmbDeviceScan.SelectedIndex != -1)
                {
                    hs.Add("isscanfile", CmbDeviceScan.Value);
                }
                hs.Add("service_ip", txtServiceIP.Text);
                hs.Add("imagepath", txtImagePath.Text);
                if (tgsDataInfo.UpdateDeviceInfo(hs) > 0)
                {
                    Notice("信息提示", "保存成功");
                    GetDeviceData();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void DoConfirmDevice()
        {
            try
            {
                RowSelectionModel sm = this.GridDevice.SelectionModel.Primary as RowSelectionModel;
                string Id = sm.SelectedRow.RecordID;
                X.Msg.Confirm("信息", "确认要删除这条记录吗?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "SystemStation.DoYesDevice()",
                        Text = "是"
                    },
                    No = new MessageBoxButtonConfig
                    {
                        Handler = "SystemStation.DoNo()",
                        Text = "否"
                    }
                }).Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void DoYesDevice()
        {
            try
            {
                RowSelectionModel sm = this.GridDevice.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRow.ToBuilder();
                Hashtable hs = new Hashtable();
                string Id = sm.SelectedRow.RecordID;
                int idx = sm.SelectedRow.RowIndex;
                hs.Add("id", Id);
                if (tgsDataInfo.DeleteDeviceInfo(hs) > 0)
                {
                    Notice("信息提示", "删除成功");
                    GridDevice.DeleteSelected();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        #endregion 增加设备

        /// <summary>
        ///
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Message(string title, string msg)
        {
            try
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = title,
                    Message = msg,
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "WARNING")
                });
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string ChageDataSource(string type)
        {
            try
            {
                switch (type)
                {
                    case "01":
                        return "1";

                    case "02":
                        return "2";

                    case "03":
                        return "3";

                    case "04":
                        return "4";

                    case "05":
                        return "5";

                    case "06":
                        return "6";

                    case "07":
                        return "2";

                    case "08":
                        return "2";

                    default:
                        return "2";
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return type;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fxbh"></param>
        /// <returns></returns>
        private string ChageDirection(string fxbh)
        {
            switch (fxbh)
            {
                case "01":
                    return "由东向西";

                case "02":
                    return "由西向东";

                case "03":
                    return "由南向北";

                case "04":
                    return "由北向南";

                case "05":
                    return "由东向南";

                case "06":
                    return "由西向南";

                case "07":
                    return "由东向北";

                case "08":
                    return "由西向北";

                case "09":
                    return "由南向东";

                case "10":
                    return "由南向西";

                case "11":
                    return "由北向东";

                case "12":
                    return "由北向西";

                default:
                    return "其它方向";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string IsShow(string type)
        {
            switch (type)
            {
                case "01":
                    return "01";

                case "02":
                    return "10";

                case "03":
                    return "10";

                case "04":
                    return "01";

                case "05":
                    return "01";

                case "06":
                    return "01";

                case "07":
                    return "10";

                case "08":
                    return "10";

                default:
                    return "01";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string Simple(string type)
        {
            switch (type)
            {
                case "01":
                    return "(电警)";

                case "02":
                    return "(卡口)";

                case "03":
                    return "(测速）";

                case "04":
                    return "(违法抓拍）";

                case "05":
                    return "(移动DV）";

                case "06":
                    return "(行车记录仪)";

                case "07":
                    return "(公安检查站）";

                case "08":
                    return "(收费站）";

                case "09":
                    return "(普清视频）";

                case "10":
                    return "(高清视频）";

                case "11":
                    return "(信号）";

                default:
                    return "(卡口)";
            }
        }

        #endregion 私有方法
    }
}