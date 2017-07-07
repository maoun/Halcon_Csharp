using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class TGSLimitSpeed : System.Web.UI.Page
    {
        #region 成员变量

        private SettingManager settingManager = new SettingManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        static DataTable treedt=null;

        private static string uName;
        private static string nowIp;
        /// <summary>
        /// 树id
        /// </summary>
        static DataTable treekkmc=null;
        static string treeId;


        /// <summary>
        ///删除选中id
        /// </summary>
        private static string kkmcId;

        static DataTable dtKkmc=null;
        /// <summary>
        /// 卡口名称
        /// </summary>
        static string kkmc;

        /// <summary>
        /// 车道编号
        /// </summary>
        static string cdbh;
        /// <summary>
        /// 大车限速
        /// </summary>
        static string dcxs;
        /// <summary>
        /// 小车限速
        /// </summary>
        static string xcxs;
        /// <summary>
        /// 大车限低速
        /// </summary>
        static string dcxds;
        /// <summary>
        /// 小车限低速
        /// </summary>
        static string xcxds;



        #endregion 成员变量

        #region 事件集合

        /// <summary>
        /// 页面加载
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
                    BuildTree(TreePanel1.Root);
                    ToolExport.Disabled = true;
                    this.DataBind();
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    uName = userinfo.UserName;
                    nowIp = userinfo.NowIp;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：卡口限速设置", userinfo.NowIp, "0");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSLimitSpeed.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
            }
        }

        protected void MyData_Refresh(object sender, DirectEventArgs e)
        {
            try
            {
                DataTable dt = tgsPproperty.GetLaneInfoView_ByStationId(HideStation.Value.ToString());
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
                logManager.InsertLogError("TGSLimitSpeed.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh has an exception");
            }
        }

        /// <summary>
        /// 刷新限速设置数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                GetLimitData(HideStation.Value.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSLimitSpeed.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh has an exception");
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
                    string xml = Bll.Common.GetPrintXml(GetLangStr("TGSLimitSpeed18", "卡口限速配置管理"), "", "", "printdatatable");
                    string js = "OpenPrintPageV(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSLimitSpeed.aspx-ButPrintClick", ex.Message + "；" + ex.StackTrace, "ButPrintClick has an exception");
            }
        }
          /// <summary>
        /// 更新车道信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BeforeSpeed(object sender, DirectEventArgs e)
        {
              dcxs = TxtBS.Text;
              xcxs = TxtSS.Text;
              dcxds = TxtBLS.Text;
              xcxds = TxtSLS.Text;

        }

        /// <summary>
        /// 更新车道信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateData(object sender, DirectEventArgs e)
        {
            try
            {
                Hashtable hs = new Hashtable();
                RowSelectionModel sm = this.GridLane.SelectionModel.Primary as RowSelectionModel;
                string Id = sm.SelectedRow.RecordID;
                hs.Add("id", Id);
                hs.Add("big_limit_speed", TxtBS.Text);
                hs.Add("small_limit_speed", TxtSS.Text);
                hs.Add("big_limit_low_speed", TxtBLS.Text);
                hs.Add("small_limit_low_speed", TxtSLS.Text);
                hs.Add("reality_big_limit_speed", TxtABS.Text);
                hs.Add("reality_small_limit_speed", TxtALS.Text);
                if (treekkmc!=null)
                {
                     DataRow[] dd = treekkmc.Select("col0='" + treeId + "'");
                if (dd.Length > 0)
                {
                    kkmc = dd[0]["col1"].ToString();
                }
                }
               
                if (tgsPproperty.UpdateLaneInfo(hs) > 0)
                {

                    string dcxss = TxtBS.Text;
                    string xcxss = TxtSS.Text;
                    string dcxdss = TxtBLS.Text;
                    string xcxdss = TxtSLS.Text;
                    string lbname = "";
                    lbname += Bll.Common.AssembleRunLog(dcxs,dcxss,"大车限速","1");
                    lbname += Bll.Common.AssembleRunLog(xcxs,xcxss,"小车限速", "1");
                    lbname += Bll.Common.AssembleRunLog(dcxds,dcxdss,"大车限低速", "1");
                    lbname += Bll.Common.AssembleRunLog(xcxds,xcxdss,"小车限低速", "1");
                    logManager.InsertLogRunning(uName, "修改：监测点[" + kkmc+ "];" + lbname, nowIp, "2");
                    Notice(GetLangStr("TGSLimitSpeed36", "限速设置"), GetLangStr("TGSLimitSpeed19", "更新成功"));
                    GetLimitData(HideStation.Value as string);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSLimitSpeed.aspx-UpdateData", ex.Message + "；" + ex.StackTrace, "UpdateData has an exception");
            }
        }

        /// <summary>
        /// 添加车道信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddLane(object sender, DirectEventArgs e)
        {
            if (string.IsNullOrEmpty(HideStation.Value.ToString()))
            {
                Notice("提示", "请选择左侧监测点！");
                return;
            }
            try
            {
                Window window = new Window();
                window.ID = "LimitAdd";
                window.Title = GetLangStr("TGSLimitSpeed36", "限速设置");
                window.Width = Unit.Pixel(400);
                window.Height = Unit.Pixel(320);

                window.Maximizable = false;
                window.Resizable = false;
                window.Hidden = true;

                window.Layout = "Fit";
                window.AutoLoad.Mode = LoadMode.Merge;
                Ext.Net.FormPanel tab = new Ext.Net.FormPanel();
                tab.Title = GetLangStr("TGSLimitSpeed20", "限速信息");
                tab.Header = false;
                tab.Padding = 5;
                tab.DefaultAnchor = "98%";
                tab.AnchorVertical = "100%";
                tab.MonitorValid = true;

                tab.Listeners.ClientValidation.Handler = "butSaveLimit.setDisabled(!valid);";
                /*  switch (HideLimitType.Value as string)
                  {
                      case "1":
                          tab.Items.Add(CommonExt.AddComboBox("cmdLDirection", GetLangStr("TGSLimitSpeed21", "所属方向"), "StoreDirDev", GetLangStr("TGSLimitSpeed22", "请选择所属方向"), true));
                          tab.Items.Add(CommonExt.AddTextField("txtLLane", GetLangStr("TGSLimitSpeed23", "车道编号")));
                          X.GetCmp<ComboBox>("cmdLDirection").Hidden = true;
                          X.GetCmp<TextField>("txtLLane").Hidden = false;
                          break;

                      case "2":
                          tab.Items.Add(CommonExt.AddComboBox("cmdLDirection", GetLangStr("TGSLimitSpeed21", "所属方向"), "StoreDirDev", GetLangStr("TGSLimitSpeed22", "请选择所属方向"), false));
                          tab.Items.Add(CommonExt.AddTextField("txtLLane", GetLangStr("TGSLimitSpeed23", "车道编号")));
                          X.GetCmp<TextField>("txtLLane").Hidden = false;

                          break;

                      case "3":
                          tab.Items.Add(CommonExt.AddComboBox("cmdLDirection", GetLangStr("TGSLimitSpeed21", "所属方向"), "StoreDirDev", GetLangStr("TGSLimitSpeed22", "请选择所属方向"), false));
                          tab.Items.Add(CommonExt.AddTextField("txtLLane", GetLangStr("TGSLimitSpeed23", "车道编号"), false, "例:[1]"));
                          break;

                      default:
                          tab.Items.Add(CommonExt.AddTextField("txtLLane", GetLangStr("TGSLimitSpeed23", "车道编号"), false, "例:[1]"));
                          break;
                  }
                 */
                tab.Items.Add(CommonExt.AddTextField("txtLLane", GetLangStr("TGSLimitSpeed23", "车道编号"), false, "例:[1]"));
                tab.Items.Add(CommonExt.AddTextField("sfEBSpeed", GetLangStr("TGSLimitSpeed11", "大车限速"), false, "例:[60]"));
                tab.Items.Add(CommonExt.AddTextField("sfESSpeed", GetLangStr("TGSLimitSpeed12", "小车限速"), false, "例:[80]"));
                tab.Items.Add(CommonExt.AddTextField("sfEBASpeed", GetLangStr("TGSLimitSpeed13", "大车标牌限速"), false, "例:[65]"));
                tab.Items.Add(CommonExt.AddTextField("sfESASpeed", GetLangStr("TGSLimitSpeed14", "小车标牌限速"), false, "例:[85]"));
                tab.Items.Add(CommonExt.AddTextField("sfEBLSpeed", GetLangStr("TGSLimitSpeed15", "大车限低速"), false, "例:[40]"));
                tab.Items.Add(CommonExt.AddTextField("sfESLSpeed", GetLangStr("TGSLimitSpeed16", "小车限低速"), false, "例:[50]"));
                window.Buttons.Add(CommonExt.AddButton("butSaveLimit", GetLangStr("TGSLimitSpeed24", "保存"), "Disk", "TGSLimitSpeed.InfoLimitSave()"));
                window.Buttons.Add(CommonExt.AddButton("butCancelLimit", GetLangStr("TGSLimitSpeed25", "取消"), "Cancel", window.ClientID + ".hide()"));
                X.GetCmp<TextField>("sfEBSpeed").Text = "80";
                X.GetCmp<TextField>("sfESSpeed").Text = "80";
                X.GetCmp<TextField>("sfEBASpeed").Text = "65";
                X.GetCmp<TextField>("sfESASpeed").Text = "85";
                X.GetCmp<TextField>("sfEBLSpeed").Text = "10";
                X.GetCmp<TextField>("sfESLSpeed").Text = "10";
                X.GetCmp<TextField>("sfEBASpeed").Hidden = true;
                X.GetCmp<TextField>("sfESASpeed").Hidden = true;
                window.Items.Add(tab);
                window.Render(this.Form);
                window.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSLimitSpeed.aspx-AddLane", ex.Message + "；" + ex.StackTrace, "AddLane has an exception");
            }
        }

        #endregion 事件集合

        #region DirectMethod

        /// <summary>
        /// 获取检测点的限速设置
        /// </summary>
        /// <param name="station_id"></param>
        /// <param name="limitType"></param>
        [DirectMethod]
        public void SelectNode(string station_id, string limitType)
        {
            try
            {
                HideStation.Value = station_id;
                GetLimitData(station_id);
                //  SetLimitType(limitType);
                RowSelectionModel sm = this.GridLane.SelectionModel.Primary as RowSelectionModel;
                sm.ClearSelections();
                sm.UpdateSelection();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSLimitSpeed.aspx-SelectNode", ex.Message + "；" + ex.StackTrace, "SelectNode has an exception");
            }
        }

        /// <summary>
        /// 取消删除
        /// </summary>
        [DirectMethod]
        public void DoNo()
        {
        }

        /// <summary>
        /// 删除车道信息
        /// </summary>
        [DirectMethod]
        public void DoConfirmLimit()
        {
            try
            {
                RowSelectionModel sm = this.GridLane.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string Id = sm.SelectedRow.RecordID;
                    X.Msg.Confirm(GetLangStr("TGSLimitSpeed26", "信息"), GetLangStr("TGSLimitSpeed27", "确认要删除这个监测点吗?"), new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "TGSLimitSpeed.DoYesLimit()",
                            Text = GetLangStr("TGSLimitSpeed28", "是")
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "TGSLimitSpeed.DoNo()",
                            Text = GetLangStr("TGSLimitSpeed29", "否")
                        }
                    }).Show();
                }
                else
                {
                    Notice(GetLangStr("TGSLimitSpeed36", "限速设置"), GetLangStr("TGSLimitSpeed30", "请选中要删除的数据行"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSLimitSpeed.aspx-DoConfirmLimit", ex.Message + "；" + ex.StackTrace, "DoConfirmLimit has an exception");
            }
        }

        /// <summary>
        /// 确认删除
        /// </summary>
        [DirectMethod]
        public void DoYesLimit()
        {
            try
            {
                RowSelectionModel sm = this.GridLane.SelectionModel.Primary as RowSelectionModel;
                string Id = sm.SelectedRow.RecordID;
                kkmcId = Id;
                Hashtable hs = new Hashtable();
                hs.Add("id", Id);
                if (tgsPproperty.DeleteLaneInfo(hs) > 0)
                {

                    DataRow[] dr = dtKkmc.Select("col0='" + kkmcId+"'");
                    if (dr.Length > 0)
                    {
                         kkmc = dr[0]["col2"].ToString();
                    }

                    Notice(GetLangStr("TGSLimitSpeed36", "限速设置"), GetLangStr("TGSLimitSpeed31", "删除成功"));
                    logManager.InsertLogRunning(uName, "删除：[" + kkmc+ "]监测点", nowIp, "3");
                    GetLimitData(HideStation.Value as string);
                    sm.ClearSelections();
                    sm.UpdateSelection();
                }
                else
                {
                    Notice(GetLangStr("TGSLimitSpeed19", GetLangStr("TGSLimitSpeed36", "限速设置")), GetLangStr("TGSLimitSpeed32", "删除失败"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSLimitSpeed.aspx-DoYesLimit", ex.Message + "；" + ex.StackTrace, "DoYesLimit has an exception");
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        [DirectMethod]
        public void InfoLimitSave()
        {
            try
            {
                if (HideStation.Value != null)
                {
                    //if (tgsPproperty.GetLaneInfoView_ByStationId(HideStation.Value.ToString()).Rows.Count > 0)
                    //{
                    //    Notice(GetLangStr("TGSLimitSpeed39", "信息提示"), "里面已有数据");
                    //    return;
                    //}

                    Hashtable hs = new Hashtable();
                    hs.Add("station_id", HideStation.Value as string);
                    hs.Add("location_id", "");
                    //hs.Add("direction_id", X.GetCmp<ComboBox>("cmdLDirection").Value);
                    hs.Add("direction_id", "");
                    hs.Add("lane_id", X.GetCmp<TextField>("txtLLane").Text);
                    hs.Add("limit_type", HideLimitType.Value as string);
                    hs.Add("big_limit_speed", X.GetCmp<TextField>("sfEBSpeed").Text);
                    hs.Add("small_limit_speed", X.GetCmp<TextField>("sfESSpeed").Text);
                    hs.Add("big_limit_low_speed", X.GetCmp<TextField>("sfEBLSpeed").Text);
                    hs.Add("small_limit_low_speed", X.GetCmp<TextField>("sfESLSpeed").Text);
                    hs.Add("reality_big_limit_speed", X.GetCmp<TextField>("sfEBASpeed").Text);
                    hs.Add("reality_small_limit_speed", X.GetCmp<TextField>("sfESASpeed").Text);
                    hs.Add("id", tgsDataInfo.GetTgsRecordId());
                    
                     
                        dcxs = X.GetCmp<TextField>("sfEBSpeed").Text;
                        xcxs = X.GetCmp<TextField>("sfESSpeed").Text;
                        dcxds = X.GetCmp<TextField>("sfEBLSpeed").Text;
                        xcxds = X.GetCmp<TextField>("sfESLSpeed").Text;
                    if (tgsPproperty.InsertLaneInfo(hs) > 0)
                    {

                        DataRow[] dr = dtKkmc.Select("col1='" + HideStation.Value as string + "'");
                        if (dr.Length > 0)
                        {
                            kkmc = dr[0]["col2"].ToString();
                        }
                        else
                        {
                            DataRow[] dd = treekkmc.Select("col0='" + treeId + "'");
                            if (dd.Length > 0) {
                                kkmc = dd[0]["col1"].ToString();
                            }
                        }
                        string lblname = "";
                        lblname += Bll.Common.AssembleRunLog("", X.GetCmp<TextField>("txtLLane").Text,"车道编号","0");
                        lblname += Bll.Common.AssembleRunLog("", X.GetCmp<TextField>("sfEBSpeed").Text, "大车限速", "0");
                        lblname += Bll.Common.AssembleRunLog("", X.GetCmp<TextField>("sfESSpeed").Text, "小车限速", "0");
                        lblname += Bll.Common.AssembleRunLog("", X.GetCmp<TextField>("sfEBLSpeed").Text, "大车限低速", "0");
                        lblname += Bll.Common.AssembleRunLog("", X.GetCmp<TextField>("sfESLSpeed").Text, "小车限低速", "0");


                        logManager.InsertLogRunning(uName, "添加：监测点[" +kkmc+ "]" + lblname, nowIp, "1");
                        Notice(GetLangStr("TGSLimitSpeed39", "信息提示"), GetLangStr("TGSLimitSpeed33", "保存成功"));
                       





                        X.GetCmp<Window>("LimitAdd").Hide();
                        GetLimitData(HideStation.Value as string);
                    }
                    else
                    {
                        Notice(GetLangStr("TGSLimitSpeed39", "信息提示"), GetLangStr("TGSLimitSpeed34", "保存失败"));
                        X.GetCmp<Window>("LimitAdd").Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSLimitSpeed.aspx-InfoLimitSave", ex.Message + "；" + ex.StackTrace, "InfoLimitSave has an exception");
            }
        }

        #endregion DirectMethod

        #region 私有方法

        /// <summary>
        /// 转换datatable
        /// </summary>
        /// <returns></returns>
        private DataTable ChangeDataTable()
        {
            try
            {
                DataTable dt = Session["datatable"] as DataTable;
                DataTable dt2 = null; ;
                if (dt != null)
                {
                    //PrintColumns pc = new PrintColumns();
                    //pc.Add(new PrintColumn("设备编号", 1));
                    //pc.Add(new PrintColumn("设备名称", 2));
                    //pc.Add(new PrintColumn("设备类型", 3));
                    //pc.Add(new PrintColumn("设备型号", 4));
                    // dt2 = Bll.Common.GetDataTablePrint(dt, pc);
                }
                return dt2;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSLimitSpeed.aspx-ChangeDataTable", ex.Message + "；" + ex.StackTrace, "ChangeDataTable has an exception");
                return null;
            }
        }

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
                logManager.InsertLogError("TGSLimitSpeed.aspx-GetdatabyField", ex.Message + "；" + ex.StackTrace, "GetdatabyField has an exception");
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
                logManager.InsertLogError("TGSLimitSpeed.aspx-ConventDataTable", ex.Message + "；" + ex.StackTrace, "ConventDataTable has an exception");
                return null;
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
                logManager.InsertLogError("TGSLimitSpeed.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice has an exception");
            }
        }

        /// <summary>
        /// 设置限制类型
        /// </summary>
        /// <param name="limitType"></param>
        private void SetLimitType(string limitType)
        {
            try
            {
                HideLimitType.Value = limitType;

                switch (limitType)
                {
                    case "1":
                    case "2":
                    case "3":
                        string js = "ChangeLimit(" + limitType + ");";
                        this.ResourceManager1.RegisterAfterClientInitScript(js);
                        break;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSLimitSpeed.aspx-SetLimitType", ex.Message + "；" + ex.StackTrace, "SetLimitType has an exception");
            }
        }

        /// <summary>
        /// 获取限速设置数据
        /// </summary>
        /// <param name="station_id"></param>
        private void GetLimitData(string station_id)
        {
            try
            {
                this.StoreLane.DataSource =dtKkmc= tgsPproperty.GetLaneInfoView_ByStationId(station_id);
                this.StoreLane.DataBind();
                if (dtKkmc.Rows.Count <= 0) {
                    treeId = station_id;
                }
                
                this.StoreDirDev.DataSource = tgsPproperty.GetDirectionInfoByStation(station_id);
                this.StoreDirDev.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSLimitSpeed.aspx-GetLimitData", ex.Message + "；" + ex.StackTrace, "GetLimitData has an exception");
            }
        }

        /// <summary>
        /// 建立检测点类型列表
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private Ext.Net.TreeNodeCollection BuildTree(Ext.Net.TreeNodeCollection nodes)
        {
            try
            {
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }

                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Expanded = true;
                root.Text = GetLangStr("TGSLimitSpeed42", "监测点类型");
                nodes.Add(root);

                DataTable dt = tgsPproperty.GetStationTypeInfo(" isuse='1'  and  limittype>0"); ;
                DataTable dt3= GetRedisData.GetData("Station:t_cfg_set_station_type_istmsshow");
                treekkmc = MyNet.Atmcs.Uscmcp.Bll.Common.ChangColName( dt3);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    string code = dt.Rows[i]["col0"].ToString();
                    string limitType = dt.Rows[i]["col4"].ToString();
                    node.Text = dt.Rows[i]["col1"].ToString();
                    node.Icon = Icon.Monitor;
                    node.NodeID = dt.Rows[i]["col0"].ToString();
                    node.Expanded = true;
                    DataTable dt2  = tgsPproperty.GetStationInfo("a.station_type_id='" + code + "'");
                    if (dt2 != null && dt2.Rows.Count > 0)
                    {
                        DataTable dtLane = tgsPproperty.GetLaneInfo(" 1=1 ");
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            Addree(node, dt2.Rows[j], limitType, dtLane);
                        }
                    }
                    else
                    {
                        node.Disabled = true;
                    }

                    root.Nodes.Add(node);
                }
                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSLimitSpeed.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
                return null;
            }
        }

        /// <summary>
        /// 建立检测点列表
        /// </summary>
        /// <param name="root"></param>
        /// <param name="dr"></param>
        /// <param name="limitType"></param>
        private void Addree(Ext.Net.TreeNode root, DataRow dr, string limitType, DataTable dtLane)
        {
            try
            {
                Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                DataRow[] drs = dtLane.Select("col1='" + dr["col1"].ToString() + "'");
                if (drs != null && drs.Length > 0)
                {
                    node.Icon = Icon.HouseLink;
                }
                else
                {
                    node.Icon = Icon.House;
                }
                node.Text = dr["col2"].ToString();
                node.Leaf = true;
                node.Listeners.Click.Handler = "TGSLimitSpeed.SelectNode('" + dr["col1"].ToString() + "','" + limitType + "') ;";
                node.NodeID = dr["col1"].ToString();
                root.Nodes.Add(node);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSLimitSpeed.aspx-Addree", ex.Message + "；" + ex.StackTrace, "Addree has an exception");
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