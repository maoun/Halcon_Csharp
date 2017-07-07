using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class StationManager : System.Web.UI.Page
    {
        #region 成员变量

        private SystemManager systemManager = new SystemManager();
        private SettingManager settingManager = new SettingManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        public static Ext.Net.TreeNode node1 = new Ext.Net.TreeNode();
        private Ext.Net.TreeNode node2 = new Ext.Net.TreeNode();
        private Ext.Net.TreeNode node3 = new Ext.Net.TreeNode();
        public static DataTable da1 = new DataTable();
        private DataTable da2 = new DataTable();
        private DataTable da3 = new DataTable();
        private Bll.DeviceManager deviceManager = new Bll.DeviceManager();
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private static DataTable dtDepartnames = null;

        /// <summary>
        /// 获取设备类型
        /// </summary>
        private static DataTable dtType = null;

        /// <summary>
        /// 加载右边数据
        /// </summary>
        private static DataTable dtcount = null;

        /// <summary>
        /// 地点名称
        /// </summary>
        private static string ddmc;

        /// <summary>
        /// 监测点类型
        /// </summary>
        private static string jcdlx;

        /// <summary>
        /// 检测类型
        /// </summary>
        private static string jcdl;

        /// <summary>
        /// 所属单位
        /// </summary>
        private static string ssdw;

        /// <summary>
        /// 定义变量 接收 地点
        /// </summary>
        private static DataTable dtbm;

        /// <summary>
        /// 获取变量 接收监控地点id
        /// </summary>
        private static string bmid;

        /// <summary>
        /// 获取用户名
        /// </summary>
        private static string uName;

        /// <summary>
        /// 获取登陆ip
        /// </summary>
        private static string nowIp;

        /// <summary>
        /// 获取id获取方向
        /// </summary>
        private static DataTable dtfx = null;

        /// <summary>
        /// 获取变量接收方向id
        /// </summary>
        private static int fxid;

        /// <summary>
        /// 定义变量接收 关联设备
        /// </summary>
        private static DataTable dtgl = null;


        static DataTable olddtfx = null;



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
                FirstGetStationType();
                StoreDataBind();

                BuildTree(TreeGrid1.Root);

                this.DataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                uName = userinfo.UserName;
                nowIp = userinfo.NowIp;
                logManager.InsertLogRunning(userinfo.UserName, "访问：监测点管理", userinfo.NowIp, "0");
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                this.StoreClass.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:140004"));//systemManager.GetCodeData("140004");
                this.StoreClass.DataBind();

                this.StoreSFSY.DataSource = GetRedisData.GetData("t_sys_code:240034"); //settingManager.getDictData("00", "240034");
                this.StoreSFSY.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            TxtDeviceName.Reset();
            TxtDeviceIP.Reset();
            CmbSBXH.Reset();
            CmbDeviceType.Reset();
        }

        /// <summary>
        /// 部门树 节点 单击选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowSelect(object sender, DirectEventArgs e)
        {
            try
            {
                string id = e.ExtraParams["id"];
                bmid = id;
                CurrentDepart.Value = id;
                Session["DEPARTID"] = id;
                GetLocationInfo(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-RowSelect", ex.Message + "；" + ex.StackTrace, "RowSelect has an exception");
            }
        }

        #region 地点操作

        /// <summary>
        /// 添加地点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddLoation(object sender, DirectEventArgs e)
        {
            AddWindow("", "", true);
        }

        /// <summary>
        ///修改地点信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ModifyLocation(object sender, DirectEventArgs e)
        {
            AddWindow(HideLocationId.Value.ToString(), hideLocationName.Value.ToString(), false);
        }

        /// <summary>
        /// 删除地点信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteLocation(object sender, DirectEventArgs e)
        {
            X.Msg.Confirm(GetLangStr("StationManager83", "信息"), GetLangStr("StationManager84", "确认要删除下面的所有信息，包括监测信息和设备关联信息，确实要进行吗?"), new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = "StationManager.DeleteLocation()",
                    Text = GetLangStr("StationManager85", "是")
                },
                No = new MessageBoxButtonConfig
                {
                    Handler = "StationManager.DoNo()",
                    Text = GetLangStr("StationManager86", "否")
                }
            }).Show();
        }

        /// <summary>
        ///删除地点信息确定事件
        /// </summary>
        [DirectMethod]
        public void DeleteLocation()
        {
            try
            {
                string ddid = HideLocationId.Value.ToString();

                Hashtable hs = new Hashtable();
                hs.Add("location_id", ddid);
                if (settingManager.DeleteLocationInfo(hs) > 0)
                {
                    tgsPproperty.DeleteStationByLocation(hs);

                    DataRow[] dr = dtbm.Select("col1='" + bmid + "'");
                    ssdw = dr[0]["col2"].ToString();

                    DataRow[] dr1 = dtcount.Select("col6='" + ddid + "'");
                    string jc = dr1[0]["col5"].ToString();
                    string dd = dr1[0]["col7"].ToString();
                    string ddl = dr1[0]["col3"].ToString();

                    logManager.InsertLogRunning(uName, "删除：[" + ddl + "]监测点;所属:[" + ssdw + "]机构;监测点:[" + jc + "];监测类型:[" + dd + "]", nowIp, "3");
                    Notice(GetLangStr("StationManager87", "信息提示"), GetLangStr("StationManager88", "删除成功"));
                    GetLocationInfo(CurrentDepart.Value.ToString());
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-DeleteLocation", ex.Message + "；" + ex.StackTrace, "DeleteLocation has an exception");
            }
        }

        /// <summary>
        /// 显示 地点添加、修改窗体
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="locationName"></param>
        /// <param name="isadd"></param>
        private void AddWindow(string locationId, string locationName, bool isadd)
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentDepart.Value.ToString()))
                {
                    Message(GetLangStr("StationManager87", "信息提示"), GetLangStr("StationManager89", "请选择所属单位！"));
                    return;
                }
                Window windowAdd = new Window();
                windowAdd.ID = "LocationAdd";
                windowAdd.Title = GetLangStr("StationManager90", "地点信息管理");
                windowAdd.Width = Unit.Pixel(400);
                windowAdd.Height = Unit.Pixel(500);
                windowAdd.Modal = true;
                windowAdd.Collapsible = true;
                windowAdd.Maximizable = false;
                windowAdd.Resizable = false;
                windowAdd.Hidden = true;
                windowAdd.Layout = "Fit";
                windowAdd.AutoLoad.Mode = LoadMode.Merge;

                Ext.Net.FormPanel tab = new Ext.Net.FormPanel();
                tab.Title = GetLangStr("StationManager91", "地点信息");
                tab.Header = false;
                tab.Padding = 20;
                tab.DefaultAnchor = "98%";
                tab.AnchorVertical = "100%";
                tab.MonitorValid = true;
                tab.Listeners.ClientValidation.Handler = "butSaveEdit.setDisabled(!valid);";
                string dpartname = "";
                if (dtDepartnames != null)
                {
                    DataRow[] rows = dtDepartnames.Select("col1='" + CurrentDepart.Value.ToString() + "'");
                    if (rows.Length > 0)
                    {
                        dpartname = rows[0]["col2"].ToString();
                    }
                }

                tab.Items.Add(CommonExt.AddTextField("txtEDepartment", GetLangStr("StationManager92", "所属单位"), dpartname, true));
                tab.Items.Add(CommonExt.AddTextField("txtELocationID", GetLangStr("StationManager93", "地点编号"), false, "例:[208300315500]"));
                tab.Items.Add(CommonExt.AddTextField("txtELocationName", GetLangStr("StationManager94", "地点名称"), false, "例:[830国道315KM+500M处]"));
                if (isadd)
                {
                    string RegionId = DateTime.Now.ToString("yyyydd");
                    locationId = tgsPproperty.GetRecordID(RegionId, 12);
                    X.GetCmp<TextField>("txtELocationID").Text = locationId;
                }
                else
                {
                    X.GetCmp<TextField>("txtELocationID").Text = locationId;
                    X.GetCmp<TextField>("txtELocationName").Text = locationName;
                }
                X.GetCmp<TextField>("txtELocationID").Disabled = true;
                windowAdd.Buttons.Add(CommonExt.AddButton("butSaveEdit", GetLangStr("StationManager95", "保存"), "Disk", "StationManager.InfoSaves()"));
                windowAdd.Buttons.Add(CommonExt.AddButton("butCancelEdit", GetLangStr("StationManager96", "取消"), "Cancel", windowAdd.ClientID + ".hide()"));

                windowAdd.Items.Add(tab);
                TreePanel TreePanel1 = new TreePanel();
                TreePanel1.Title = GetLangStr("StationManager97", "监测点类型列表");
                TreePanel1.Icon = Icon.ArrowNsew;
                TreePanel1.RootVisible = false;
                TreePanel1.UseArrows = false;
                TreePanel1.Animate = true;
                TreePanel1.EnableDD = true;
                TreePanel1.ContainerScroll = true;

                DataTable dt2 = null;
                if (isadd)
                {
                    BuildTree(TreePanel1.Root, dt2);
                }
                else
                {
                    dt2 = tgsPproperty.GetStationInfoByLocation(locationId);
                    BuildTree(TreePanel1.Root, dt2);
                }
                TreePanel1.Listeners.CheckChange.Handler = "#{GridData}.setValue(getTasks(this), true);";
                tab.Items.Add(TreePanel1);
                windowAdd.Render(this.Form);
                windowAdd.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-AddWindow", ex.Message + "；" + ex.StackTrace, "AddWindow has an exception");
            }
        }

        /// <summary>
        ///保存监测点信息
        /// </summary>
        [DirectMethod]
        public void InfoSaves()
        {
            try
            {
                List<string> lst = new List<string>();
                if (!string.IsNullOrEmpty(GridData.Text))
                {
                    string[] str = GridData.Text.Split(',');
                    lst.AddRange(str);
                }
                if (lst.Count > 0)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        string RegionId = DateTime.Now.ToString("yyyydd");
                        string id = tgsPproperty.GetRecordID(RegionId, 12);
                        if (tgsPproperty.GetXhExist("t_cfg_set_station", "station_id", id) > 0)
                        {
                            id = tgsPproperty.GetRecordId();
                        }
                        string station_name = X.GetCmp<TextField>("txtELocationName").Text + GetName(lst[i].ToString());
                        Hashtable hss = new Hashtable();
                        hss.Add("station_id", id);
                        hss.Add("station_name", station_name);
                        hss.Add("station_type_id", lst[i].ToString());
                        string sjly = lst[i].ToString();
                        if (sjly.Substring(0, 1).Equals("0"))
                        {
                            sjly = sjly.Substring(1);
                        }
                        hss.Add("datasource", sjly);
                        hss.Add("location_id", X.GetCmp<TextField>("txtELocationID").Text);
                        hss.Add("description", station_name);
                        hss.Add("station_idext", id);
                        hss.Add("departid", CurrentDepart.Value.ToString());
                        hss.Add("id", id);
                        tgsPproperty.InsertStationInfoByKK(hss);
                    }
                }
                if (tgsPproperty.GetXhExist("t_cfg_location", "location_id", X.GetCmp<TextField>("txtELocationID").Text) > 0)
                {
                    Hashtable hsl = new Hashtable();
                    hsl.Add("departid", CurrentDepart.Value.ToString());
                    hsl.Add("location_name", X.GetCmp<TextField>("txtELocationName").Text);
                    hsl.Add("location_id", X.GetCmp<TextField>("txtELocationID").Text);
                    if (settingManager.UpdateLocationInfo(hsl) > 0)
                    {
                        //修改

                        string ddmcs = X.GetCmp<TextField>("txtELocationName").Text;
                         // DataRow[] dr = dtType.Select("col0='" + X.GetCmp<TextField>("txtELocationID").Text + "'");
                        DataRow[] dr = dtcount.Select("col4='"+X.GetCmp<TextField>("txtElocationID").Text+"'");
                        string jcd = dr[0]["col3"].ToString();
                        //string jcdlxs = dr[0]["col1"].ToString();
                         // logManager.InsertLogRunning(uName, "修改：监测地点[" + ddmc + "]" , nowIp, "1");
                        Notice(GetLangStr("StationManager87", "信息提示"), GetLangStr("StationManager98", "保存成功"));
                        X.GetCmp<Window>("LocationAdd").Hide();
                        GetLocationInfo(CurrentDepart.Value.ToString());
                    }
                }
                else
                {
                    Hashtable hs = new Hashtable();
                    hs.Add("location_id", X.GetCmp<TextField>("txtELocationID").Text);
                    hs.Add("location_name", X.GetCmp<TextField>("txtELocationName").Text);
                    hs.Add("departid", CurrentDepart.Value.ToString());
                    hs.Add("systemid", "00");
                    if (settingManager.InsertLocationInfo(hs) > 0)
                    {
                        ddmc = X.GetCmp<TextField>("txtELocationName").Text;
                        DataRow[] dr = dtbm.Select("col1='" + bmid + "'");
                        ssdw = dr[0]["col2"].ToString();

                        for (int i = 0; i < lst.Count; i++)
                        {
                            DataRow[] dr1 = dtType.Select("col0='" + lst[i] + "'");
                            jcdlx = "[" + dr1[0]["col1"].ToString() + "]";
                        }
                        string lblname = "";
                        lblname += Bll.Common.AssembleRunLog("", jcdlx, "监测点类型", "0");
                        lblname += Bll.Common.AssembleRunLog("", ssdw, "所属单位", "0");
                        logManager.InsertLogRunning(uName, "添加：监测地点[" + ddmc + "]" + lblname, nowIp, "1");
                        Notice(GetLangStr("StationManager87", "信息提示"), GetLangStr("StationManager98", "保存成功"));
                        X.GetCmp<Window>("LocationAdd").Hide();
                        GetLocationInfo(CurrentDepart.Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-InfoSaves", ex.Message + "；" + ex.StackTrace, "InfoSaves has an exception");
            }
        }

        #endregion 地点操作

        #region 监测点选择

        /// <summary>
        /// 更新监测点信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateStation(object sender, DirectEventArgs e)
        {
            try
            {
                RowSelectionModel sm = this.GridStation.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    Hashtable hs = new Hashtable();
                    string Id = sm.SelectedRow.RecordID;
                    hs.Add("id", Id);
                    hs.Add("station_id", HidStationId.Value.ToString());
                    hs.Add("station_name", TxtStationName.Text);
                    hs.Add("description", TxtStationName.Text);
                    hs.Add("station_idext", TxtStationIdExt.Text);
                    DataRow[] dr = dtbm.Select("col1='" + bmid + "'");
                    ssdw = dr[0]["col2"].ToString();
                    DataRow[] dr1 = dtcount.Select("col2='" + Id + "'");
                    string kName = dr1[0]["col3"].ToString();

                    string oldkdbh = HidStationId.Value.ToString();
                    string nowkdbh = TxtStationIdExt.Text;

                    if (tgsPproperty.UpdateStationInfo(hs) > 0)
                    {

                        logManager.InsertLogRunning(uName, "修改[" +kName + "]监测点;所属单位:["+ssdw+"];外部编号:由["+oldkdbh+"]修改成["+nowkdbh+"]", nowIp, "1");
                        Notice(GetLangStr("StationManager99", "监测点信息提示"), GetLangStr("StationManager100", "更新成功"));
                        GetLocationInfo(CurrentDepart.Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-UpdateStation", ex.Message + "；" + ex.StackTrace, "UpdateStation has an exception");
            }
        }

        /// <summary>
        /// 删除监测点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteStation(object sender, DirectEventArgs e)
        {
            string stationid = HideStation.Value as string;

            X.Msg.Confirm(GetLangStr("StationManager101", "信息"), GetLangStr("StationManage102", "确认要删除这条监测点记录吗?"), new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = "StationManager.DeleteStation()",
                    Text = GetLangStr("StationManager103", "是")
                },
                No = new MessageBoxButtonConfig
                {
                    Handler = "StationManager.DoNo()",
                    Text = GetLangStr("StationManager104", "否")
                }
            }).Show();
        }

        /// <summary>
        /// 删除监测点 确定事件
        /// </summary>
        [DirectMethod]
        public void DeleteStation()
        {
            try
            {
                RowSelectionModel sm = this.GridStation.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    Hashtable hs = new Hashtable();
                    string Id = sm.SelectedRow.RecordID;
                    hs.Add("id", Id);
                    hs.Add("station_id", HideStation.Value as string);
                    if (tgsPproperty.DeleteStationInfo(hs) > 0)
                    {
                        DataRow[] dr = dtbm.Select("col1='" + bmid + "'");
                        ssdw = dr[0]["col2"].ToString();
                        DataRow[] dr1 = dtcount.Select("col2='" + Id + "'");
                        string kName = dr1[0]["col3"].ToString();
                        logManager.InsertLogRunning(uName, "删除所属[" + ssdw + "]机构;[" + kName + "]监测点", nowIp, "3");
                        Notice(GetLangStr("StationManager87", "信息提示"), GetLangStr("StationManager105", "删除成功"));
                        GridStation.DeleteSelected();
                        GetLocationInfo(CurrentDepart.Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-DeleteStation", ex.Message + "；" + ex.StackTrace, "DeleteStation has an exception");
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
                string data = e.ExtraParams["data"];
                HideStationType.Value = Bll.Common.GetdatabyField(data, "col4");
                string stationid = Bll.Common.GetdatabyField(data, "col1");
                HideLocationId.Value = Bll.Common.GetdatabyField(data, "col6");
                hideLocationName.Value = Bll.Common.GetdatabyField(data, "col7");
                MenuModify.Disabled = false;
                MenuDelete.Disabled = false;
                MenuStation.Disabled = false;
                GridDevice.Title = Bll.Common.GetdatabyField(data, "col3") + GetLangStr("StationManager106", "- 设备关联信息");
                GridPanelDirection.Title = Bll.Common.GetdatabyField(data, "col3") + GetLangStr("StationManager107", "- 方向管理");
                HideStation.Value = stationid;
                GetDirectionInfo(stationid);
                GetDeviceInfo(stationid);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-ApplyClick", ex.Message + "；" + ex.StackTrace, "ApplyClick has an exception");
            }
        }

        #endregion 监测点选择

        #region 方向增加

        /// <summary>
        /// 保存方向信息
        /// </summary>
        [DirectMethod]
        public void InfoSaveDir()
        {
            try
            {
                string stationid = HideStation.Value as string;

                List<string> lst = new List<string>();

                if (!string.IsNullOrEmpty(GridData.Text))
                {
                    string[] str = GridData.Text.Split(',');
                    lst.AddRange(str);
                }

                if (lst.Count > 0)
                {
                    Hashtable hs = new Hashtable();
                    hs.Add("station_id", stationid);
                    settingManager.DeleteDirectionInfo(hs);
                    int res = 0;
                    for (int i = 0; i < lst.Count; i++)
                    {
                        hs = new Hashtable();
                        hs.Add("station_id", stationid);
                        hs.Add("direction_id", lst[i].ToString());
                        res = res + settingManager.InsertDirectionInfo(hs);
                    }

                    string dtts = "";
                    string direName = "";
                    if (res > 0)
                    {
                        dtfx = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240025"));
                        //DataRow[] dr = dtfx.Select("col0='" + dtts + "'");
                        //for (int i = 0; i < dr.Length; i++)
                        //{
                        //    dtts = "[" + dr[0]["col2"].ToString() + "]";
                        //}
                        for (int i = 0; i < lst.Count; i++)
                        {
                            DataRow[] dr1 = dtfx.Select("col0='" + lst[i] + "'");
                            if (dr1.Length > 0)
                            {
                                direName += "[" + dr1[0]["col1"].ToString() + "]";
                            }
                        }
                        logManager.InsertLogRunning(uName, "添加方向[" + direName + "]", nowIp, "1");
                        Notice(GetLangStr("StationManager87", "信息提示"), GetLangStr("StationManager108", "保存成功"));
                        GetDirectionInfo(stationid);
                        X.GetCmp<Window>("DirectionAdd").Hide();
                    }
                }
                else
                {
                    Message(GetLangStr("StationManager87", "信息提示"), GetLangStr("StationManager109", "请选择方向！"));
                    return;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-InfoSaveDir", ex.Message + "；" + ex.StackTrace, "InfoSaveDir has an exception");
            }
        }

        /// <summary>
        /// 显示方向添加窗体
        /// </summary>
        private void AddWindowsDirection()
        {
            try
            {
                Window window = new Window();
                window.ID = "DirectionAdd";
                window.Title = GetLangStr("StationManager120", "方向增加");
                window.Width = Unit.Pixel(400);
                window.Height = Unit.Pixel(400);

                window.Collapsible = true;
                window.Maximizable = false;
                window.Resizable = false;
                window.Hidden = true;

                window.Layout = "Fit";
                window.AutoLoad.Mode = LoadMode.Merge;
                Ext.Net.FormPanel tab = new Ext.Net.FormPanel();
                tab.Title = GetLangStr("StationManager121", "方向信息");
                tab.Header = false;
                tab.Padding = 5;
                tab.DefaultAnchor = "98%";
                tab.AnchorVertical = "100%";

                TreePanel TreePanel2 = new TreePanel();
                TreePanel2.Title = GetLangStr("StationManager122", "方向列表");
                TreePanel2.Icon = Icon.ArrowNsew;
                TreePanel2.RootVisible = false;
                TreePanel2.UseArrows = false;
                TreePanel2.Animate = true;
                TreePanel2.EnableDD = true;
                TreePanel2.ContainerScroll = true;
                TreePanel2.Listeners.CheckChange.Handler = "#{GridData}.setValue(getTasks(this), true);";

                DataTable dt2 = tgsPproperty.GetDirectionInfoByStation(HideStation.Value.ToString());

                BuildDirectionTree(TreePanel2.Root, dt2);
                tab.Items.Add(TreePanel2);
                window.Buttons.Add(CommonExt.AddButton("butSaveDir", GetLangStr("StationManager123", "保存"), "Disk", "StationManager.InfoSaveDir()"));
                window.Buttons.Add(CommonExt.AddButton("butCancelDir", GetLangStr("StationManager124", "取消"), "Cancel", window.ClientID + ".hide()"));
                window.Items.Add(tab);
                window.Render(this.Form);
                window.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-AddWindowsDirection", ex.Message + "；" + ex.StackTrace, "AddWindowsDirection has an exception");
            }
        }

        /// <summary>
        /// 绑定方向树
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="dtRole"></param>
        private void BuildDirectionTree(Ext.Net.TreeNodeCollection nodes, DataTable dtRole)
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

                Ext.Net.TreeNode node1 = new Ext.Net.TreeNode();

                node1.Text = GetLangStr("StationManager125", "常用方向");
                node1.Leaf = true;
                node1.Icon = Icon.ArrowNsew;
                node1.Leaf = false;
                root.Nodes.Add(node1);
                Ext.Net.TreeNode node2 = new Ext.Net.TreeNode();
                node2.Text = GetLangStr("StationManager126", "其它方向");
                node2.Leaf = true;
                node2.Icon = Icon.ArrowNwNeSwSe;
                node2.Leaf = false;
                root.Nodes.Add(node2);

                DataTable dt = tgsPproperty.GetCommonDict("240025");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    node.Text = dt.Rows[i][1].ToString();
                    node.Leaf = true;
                    node.Icon = Icon.ArrowEw;
                    node.Checked = ThreeStateBool.False;
                    if (dtRole.Rows.Count > 0)
                    {
                        DataRow[] drs = dtRole.Select("col1='" + dt.Rows[i][0].ToString() + "'");
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
                    if (int.Parse(dt.Rows[i][0].ToString()) < 5)
                    {
                        node1.Nodes.Add(node);
                    }
                    else
                    {
                        node2.Nodes.Add(node);
                    }
                    node1.Expanded = true;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-BuildDirectionTree", ex.Message + "；" + ex.StackTrace, "BuildDirectionTree has an exception");
            }
        }

        /// <summary>
        /// 添加方向
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButDirection_Click(object sender, DirectEventArgs e)
        {
            AddWindowsDirection();
        }

        /// <summary>
        ///删除方向确定事件
        /// </summary>
        [DirectMethod]
        public void DoYesDirection()
        {
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("station_id", HideStation.Value as string);
                hs.Add("direction_id", HideDirection.Value as string);
                string ss = HideStation.Value.ToString();
                if (settingManager.DeleteDirectionInfo(hs) > 0)
                {
                    DataRow[] dr = dtbm.Select("col1='" + bmid + "'");
                    ssdw = dr[0]["col2"].ToString();
                    DataRow[] dr1 = dtcount.Select("col2='" + ss + "'");
                    string kName = dr1[0]["col3"].ToString();
                    string sss = HideDirection.Value.ToString();
                    dtfx = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240025"));
                    DataRow[] drs = dtfx.Select("col0='" + sss + "'");
                    string jcd = drs[0]["col1"].ToString();
                    logManager.InsertLogRunning(uName, "删除方向;[" + jcd + "];监测点:[" + kName + "];所属大队:[" + ssdw + "]", nowIp, "3");
                    Notice(GetLangStr("StationManager87", "信息提示"), GetLangStr("StationManager127", "删除成功"));
                    GridPanelDirection.DeleteSelected();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-DoYesDirection", ex.Message + "；" + ex.StackTrace, "DoYesDirection has an exception");
            }
        }

        /// <summary>
        /// 删除方向事件
        /// </summary>
        [DirectMethod]
        public void DoConfirmDirection()
        {
            X.Msg.Confirm(GetLangStr("StationManager83", GetLangStr("StationManager128", "信息")), GetLangStr("StationManager129", "确认要删除这条记录吗?"), new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = "StationManager.DoYesDirection()",
                    Text = GetLangStr("StationManager130", "是")
                },
                No = new MessageBoxButtonConfig
                {
                    Handler = "StationManager.DoNo()",
                    Text = GetLangStr("StationManager131", "否")
                }
            }).Show();
        }

        /// <summary>
        /// 更新方向信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateDirection(object sender, DirectEventArgs e)
        {
            try
            {
                string stationid = HideStation.Value as string;
                if (string.IsNullOrEmpty(stationid))
                {
                    Message(GetLangStr("StationManager87", "信息提示"), GetLangStr("StationManager132", "请选择监测点！"));
                    return;
                }
                else
                {
                    Hashtable hs = new Hashtable();
                    hs.Add("station_id", stationid);
                    hs.Add("direction_id", HideDirection.Value as string);
                    hs.Add("direction_desc", txtDirectionDesc.Text);
                    ///所属单位
                    DataRow[] dr = dtbm.Select("col1='" + bmid + "'");
                    ssdw = dr[0]["col2"].ToString();
                    ///监测地点
                    DataRow[] dr1 = dtcount.Select("col1='" + stationid + "'");
                    string kName = dr1[0]["col3"].ToString();
                  ///新的方向
                    string nowbh = txtDirectionDesc.Text;
                  ///老的方向
                  DataRow[] dt=olddtfx.Select("col1='"+HideDirection.Value+"'");
                  string olbh = dt[0]["col2"].ToString();
                    int res = settingManager.UpdateDirectionInfo(hs);
                    if (res > 0)
                    {
                        logManager.InsertLogRunning(uName, "修改[" + kName + "]监测点;所属单位:[" + ssdw + "];外部编号:由[" + olbh + "]修改成[" + nowbh + "]", nowIp, "1");
                        Notice(GetLangStr("StationManager87", "信息提示"), GetLangStr("StationManager133", "保存成功"));
                        GetDirectionInfo(stationid);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-UpdateDirection", ex.Message + "；" + ex.StackTrace, "UpdateDirection has an exception");
            }
        }

        /// <summary>
        /// 根据监测点编号 获得方向编号
        /// </summary>
        /// <param name="stationid"></param>
        private void GetDirectionInfo(string stationid)
        {
            try
            {
                StoreDirectionInfo.DataSource = olddtfx = tgsPproperty.GetDirectionInfoByStation(stationid);
                StoreDirectionInfo.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-GetDirectionInfo", ex.Message + "；" + ex.StackTrace, "GetDirectionInfo has an exception");
            }
        }

        /// <summary>
        /// 方向行选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowSelectDirection(object sender, DirectEventArgs e)
        {
            try
            {
                string data = e.ExtraParams["data"];
                HideDirection.Value = Bll.Common.GetdatabyField(data, "col0");
                HideDirDesc.Value = Bll.Common.GetdatabyField(data, "col2");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-RowSelectDirection", ex.Message + "；" + ex.StackTrace, "RowSelectDirection has an exception");
            }
        }

        #endregion 方向增加

        #region 关联设备

        /// <summary>
        ///  开始关联设备事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButDevice_Click(object sender, DirectEventArgs e)
        {
            try
            {
                GridQueryDevice.RemoveAll();
                GridBindDevice.RemoveAll();

                string stationid = HideStation.Value.ToString();
                DataTable bindDevice = tgsPproperty.GetDeviceInfoByStation("station_id='" + stationid + "'");
                StoreBindDevice.DataSource = bindDevice;
                StoreBindDevice.DataBind();
                StoreQueryDevice.DataSource = new DataTable();
                StoreQueryDevice.DataBind();
                StoreDeviceType.DataSource = tgsPproperty.GetDeviceTypeByStation(HideStationType.Value.ToString());
                StoreDeviceType.DataBind();
                Window4.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-ButDevice_Click", ex.Message + "；" + ex.StackTrace, "ButDevice_Click has an exception");
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
                DeviceDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        /// <summary>
        /// 管理设备信息
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="count"></param>
        [DirectMethod]
        public void AddBindDeviceInfo(string[] str1, int count)
        {
            try
            {
                string stationid = HideStation.Value as string;
                List<string> Listdeviid = new List<string>();
                for (int i = 0; i < count; i++)
                {
                    Listdeviid.Add(str1[i]);
                }
                if (tgsPproperty.InsertStationDevice(stationid, Listdeviid) > 0)
                {
                    dtgl = tgsPproperty.GetDeviceInfoByStation("station_id='" + stationid + "'");
                    try
                    {
                        DataRow[] dr = dtbm.Select("col1='" + bmid + "'");
                        ssdw = dr[0]["col2"].ToString();
                        DataRow[] dr1 = dtcount.Select("col2='" + stationid + "'");
                        string kName = dr1[0]["col3"].ToString();
                        if (dtgl != null)
                        {
                            string jclx = "";
                            string jcmc ="";
                            string sumjcml="";
                            for (int i = 0; i < dtgl.Rows.Count; i++)
                            {
                                jcmc = "设备名称:[" + dtgl.Rows[i]["col1"].ToString() + "];";
                                jclx = "设备类型:["+ dtgl.Rows[i]["col3"].ToString()+"];";
                                sumjcml += jcmc + jclx;
                            }
                           
                            logManager.InsertLogRunning(uName, "添加关联:"+sumjcml+"所属单位:["+ssdw+"];监测点:["+kName+"]", nowIp, "1");
                            Notice(GetLangStr("StationManager87", "信息提示"), GetLangStr("StationManager134", "设备关联成功！"));
                            GetDeviceInfo(stationid);
                            Window4.Hide();
                        }
                    }
                    catch (Exception ex)
                    {
                        ILog.WriteErrorLog(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-AddBindDeviceInfo", ex.Message + "；" + ex.StackTrace, "AddBindDeviceInfo has an exception");
            }
        }

        /// <summary>
        /// 删除设备信息
        /// </summary>
        [DirectMethod]
        public void DoConfirmDevice()
        {
            try
            {
                RowSelectionModel sm = this.GridDevice.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string deviceId = sm.SelectedRow.RecordID;
                    X.Msg.Confirm(GetLangStr("StationManager83", "信息"), GetLangStr("StationManager135", "确认要删除这条记录吗?"), new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "StationManager.DoYesDevice()",
                            Text = GetLangStr("StationManager136", "是")
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "StationManager.DoNo()",
                            Text = GetLangStr("StationManager137", "否")
                        }
                    }).Show();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-DoConfirmDevice", ex.Message + "；" + ex.StackTrace, "DoConfirmDevice has an exception");
            }
        }

        /// <summary>
        /// 删除设备确定事件
        /// </summary>
        [DirectMethod]
        public void DoYesDevice()
        {
            try
            {
                string stationid = HideStation.Value.ToString();

                RowSelectionModel sm = this.GridDevice.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRow.ToBuilder();
                string deviceId = sm.SelectedRow.RecordID;
                DataRow[] dr3 = dtbm.Select("col1='" + bmid + "'");
                ssdw = dr3[0]["col2"].ToString();
                DataRow[] dr1 = dtcount.Select("col2='" + stationid + "'");
                string kName = dr1[0]["col3"].ToString();

                dtgl = tgsPproperty.GetDeviceInfoByStation("station_id='" + stationid + "'");
                DataRow[] dr = dtgl.Select("col0='" + deviceId + "'");
                string typ = dr[0]["col3"].ToString();
                string jcmc = dr[0]["col1"].ToString();
                Hashtable hs = new Hashtable();
                hs.Add("station_id", stationid);
                hs.Add("device_id", deviceId);
                if (tgsPproperty.DeleteDeviceStation(hs) > 0)
                {
                    logManager.InsertLogRunning(uName, "移除关联:检测名称:[" + jcmc + "];检测类型:[" + typ + "];所属单位:"+ssdw+"];监测点:["+kName+"]", nowIp, "3");
                    Notice(GetLangStr("StationManager87", "信息提示"), GetLangStr("StationManager138", "关联设备移除成功！"));
                    GetDeviceInfo(stationid);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-DoYesDevice", ex.Message + "；" + ex.StackTrace, "DoYesDevice has an exception");
            }
        }

        /// <summary>
        /// 获得设备查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            try
            {
                string where = "   1=1   ";
                if (!string.IsNullOrEmpty(TxtDeviceName.Text))
                {
                    where = where + " and  a.device_name   like '%" + TxtDeviceName.Text.ToUpper() + "%' ";
                }
                if (!string.IsNullOrEmpty(TxtDeviceIP.Text))
                {
                    where = where + " and  a.ipaddress   like '%" + TxtDeviceIP.Text.ToUpper() + "%' ";
                }

                if (CmbSBXH.SelectedIndex != -1)
                {
                    where = where + " and  a.device_mode_id='" + CmbSBXH.SelectedItem.Value + "' ";
                }
                if (CmbDeviceType.SelectedIndex != -1)
                {
                    where = where + " and  a.device_type_id='" + CmbDeviceType.SelectedItem.Value + "' ";
                }
                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-GetWhere", ex.Message + "；" + ex.StackTrace, "GetWhere has an exception");
                return "";
            }
        }

        /// <summary>
        /// 绑定设备信息
        /// </summary>
        /// <param name="where"></param>
        private void DeviceDataBind(string where)
        {
            try
            {
                DataTable dt = tgsPproperty.GetDeviceInfoByWhere(where);
                StoreQueryDevice.DataSource = dt;
                StoreQueryDevice.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-DeviceDataBind", ex.Message + "；" + ex.StackTrace, "DeviceDataBind has an exception");
            }
        }

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateDeviceData(object sender, DirectEventArgs e)
        {
            try
            {
                Hashtable hs = new Hashtable();
                RowSelectionModel sm = this.GridBindDevice.SelectionModel.Primary as RowSelectionModel;
                string Id = sm.SelectedRow.RecordID;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-UpdateDeviceData", ex.Message + "；" + ex.StackTrace, "UpdateDeviceData has an exception");
            }
        }

        #endregion 关联设备

        #endregion 控件事件

        #region DirectMethod事件

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void SelectDeviceType()
        {
            try
            {
                string type = CmbDeviceType.SelectedItem.Value;
                DataTable da = deviceManager.GetDeviceTypeMode("device_type_id='" + type + "'");
                CmbSBXH.Reset();
                this.StoreSBXH.DataSource = da;
                this.StoreSBXH.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-SelectDeviceType", ex.Message + "；" + ex.StackTrace, "SelectDeviceType has an exception");
            }
        }

        /// <summary>
        /// 不删除事件
        /// </summary>
        [DirectMethod]
        public void DoNo()
        {
        }

        #endregion DirectMethod事件

        #region 私有事件

        /// <summary>
        /// 绑定监测点类型
        /// </summary>
        private void FirstGetStationType()
        {
            try
            {
                DataTable objData = Bll.Common.ChangColName(GetRedisData.GetData("Station:t_cfg_set_station_type"));// tgsPproperty.GetStationTypeInfo(" isuse='1'");
                this.StoreStation.DataSource = objData;
                this.StoreStation.DataBind();
                if (objData != null)
                {
                    if (objData.Rows.Count > 0)
                    {
                        CurrentStationId.Value = objData.Rows[0][0].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-FirstGetStationType", ex.Message + "；" + ex.StackTrace, "FirstGetStationType has an exception");
            }
        }

        /// <summary>
        /// 绑定部门信息
        /// </summary>
        private void DepartDataBind()
        {
            try
            {
                DataTable dt = settingManager.GetDepartmentDict("00");
                if (dt != null)
                {
                    Storedepart.DataSource = dt;
                    Storedepart.DataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-DepartDataBind", ex.Message + "；" + ex.StackTrace, "DepartDataBind has an exception");
            }
        }

        /// <summary>
        ///将部门信息绑定至tree
        /// </summary>
        /// <param name="nodes"></param>
        private void BuildTree(Ext.Net.TreeNodeCollection nodes)
        {
            try
            {
                TreeGrid1.RemoveAll(true);
                ClearInfoData();
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }
                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Text = GetLangStr("StationManager139", "机构管理");
                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;
                DataTable dt = dtbm = settingManager.GetConfigDepartment("00");
                if (dtDepartnames != null)
                {
                    dtDepartnames = null;
                }
                dtDepartnames = dt;
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Addree(dt, dt.Rows[0]["col3"].ToString(), root, null);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
            }
            if (X.IsAjaxRequest)
            {
                TreeGrid1.Render(PanelNavigate, RenderMode.Auto);
            }
        }

        /// <summary>
        /// 遍历将子部门挂接至父部门
        /// </summary>
        /// <param name="allNodeTable"></param>
        /// <param name="parentColValue"></param>
        /// <param name="root"></param>
        /// <param name="ParentNode"></param>
        private void Addree(DataTable allNodeTable, string parentColValue, Ext.Net.TreeNode root, Ext.Net.TreeNode ParentNode)
        {
            try
            {
                DataRow[] myDataRows = allNodeTable.Select("col3 ='" + parentColValue + "'");

                foreach (DataRow myDataRow in myDataRows)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode(); ;
                    node.Text = myDataRow[2].ToString();
                    node.NodeID = myDataRow[1].ToString();
                    node.Leaf = true;
                    node.Draggable = false;
                    node.Expandable = ThreeStateBool.True;
                    node.Expanded = true;

                    node.Icon = Icon.Telephone;
                    if (ParentNode != null)
                    {
                        ParentNode.Nodes.Add(node);
                        Addree(allNodeTable, myDataRow["col1"].ToString(), ParentNode, node);
                    }
                    else
                    {
                        root.Nodes.Add(node);
                        Addree(allNodeTable, myDataRow["col1"].ToString(), root, node);
                        ShowInfoData(myDataRow);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-Addree", ex.Message + "；" + ex.StackTrace, "Addree has an exception");
            }
        }

        /// <summary>
        /// 获得对应图标
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Icon GetIcon(string type)
        {
            switch (type)
            {
                case "02":
                    return Icon.Car;

                case "01":
                    return Icon.Car;

                case "09":
                    return Icon.Camera;

                case "10":
                    return Icon.Camera;

                case "11":
                    return Icon.PictureEmpty;

                case "13":
                    return Icon.WeatherCloudy;

                case "15":
                    return Icon.FlagRed;

                default:
                    return Icon.House;
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void ClearInfoData()
        {
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="dr"></param>
        public void ShowInfoData(DataRow dr)
        {
            ClearInfoData();
        }

        /// <summary>
        ///  绑定检测点类型
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

                Ext.Net.TreeNode node1 = new Ext.Net.TreeNode();

                node1.Text = GetLangStr("StationManager140", "监测点类型");
                node1.Leaf = true;
                node1.Icon = Icon.ArrowNsew;
                node1.Leaf = false;
                root.Nodes.Add(node1);
                DataTable dt = dtType = tgsPproperty.GetStationTypeInfo(" isuse='1'"); ;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    node.Text = dt.Rows[i][1].ToString();
                    node.Leaf = true;
                    node.Icon = GetIcon(dt.Rows[i][0].ToString());
                    node.Checked = ThreeStateBool.False;

                    if (dtRole != null)
                    {
                        DataRow[] drs = dtRole.Select("col3='" + dt.Rows[i]["col0"].ToString() + "'");
                        if (drs.Length > 0)
                        {
                            node.Checked = ThreeStateBool.True;
                            node.Disabled = true;
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
                    node1.Nodes.Add(node);
                    node1.Expanded = true;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
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
        /// 转换监测点类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetName(string type)
        {
            switch (type)
            {
                case "01":
                    return GetLangStr("StationManager141", "电警");

                case "02":
                    return GetLangStr("StationManager142", "卡口");

                case "03":
                    return GetLangStr("StationManager143", "测速");

                case "04":
                    return GetLangStr("StationManager144", "闭路电视");

                case "05":
                    return GetLangStr("StationManager145", "移动抓拍");

                case "06":
                    return GetLangStr("StationManager146", "记录仪");

                case "07":
                    return GetLangStr("StationManager147", "检查站");

                case "08":
                    return GetLangStr("StationManager148", "收费站");

                case "09":
                    return GetLangStr("StationManager149", "普清");

                case "10":
                    return GetLangStr("StationManager150", "高清");

                case "11":
                    return GetLangStr("StationManager151", "诱导");

                case "12":
                    return GetLangStr("StationManager152", "信号");

                case "13":
                    return GetLangStr("StationManager153", "气象");

                case "14":
                    return GetLangStr("StationManager154", "事件");

                case "15":
                    return GetLangStr("StationManager155", "流量");

                default:
                    return "";
            }
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Message(string title, string msg)
        {
            X.Msg.Show(new MessageBoxConfig
            {
                Title = title,
                Message = msg,
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "WARNING")
            });
        }

        /// <summary>
        /// 根据传入部门编号 绑定  地点 方向 设备信息
        /// </summary>
        /// <param name="departId"></param>
        private void GetLocationInfo(string departId)
        {
            try
            {
                GridDevice.Title = GetLangStr("StationManager156", "设备关联信息");
                GridPanelDirection.Title = GetLangStr("StationManager157", "方向管理");
                GridStoreDevice.DataSource = new DataTable();
                GridStoreDevice.DataBind();
                StoreDirectionInfo.DataSource = new DataTable();
                StoreDirectionInfo.DataBind();
                StoreLocaltionStation.DataSource = dtcount = tgsPproperty.GetLocationStationByWhere("departid='" + departId + "'");
                StoreLocaltionStation.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-GetLocationInfo", ex.Message + "；" + ex.StackTrace, "GetLocationInfo has an exception");
            }
        }

        /// <summary>
        /// 根据监测点编号，查询设备信息
        /// </summary>
        /// <param name="stationid"></param>
        private void GetDeviceInfo(string stationid)
        {
            try
            {
                GridStoreDevice.DataSource = tgsPproperty.GetDeviceInfoByStation("station_id='" + stationid + "'");
                GridStoreDevice.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("StationManager.aspx-GetDeviceInfo", ex.Message + "；" + ex.StackTrace, "GetDeviceInfo has an exception");
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

        #endregion 私有事件
    }
}