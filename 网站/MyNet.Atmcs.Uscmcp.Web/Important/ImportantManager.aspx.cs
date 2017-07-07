using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class ImportantManager : System.Web.UI.Page
    {
        #region 成员变量

        private SettingManager settingManager = new SettingManager();
        private FacilityManager facilityManager = new FacilityManager();
        private EngineroomManagers enginerromManagers = new EngineroomManagers();
        private ServiceManager serviceManager = new ServiceManager();
        private Bll.DeviceManager deviceManager = new Bll.DeviceManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

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
                string js = "alert('" + GetLangStr("ImportantManager57", "您没有登录或操作超时，请重新登陆!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!IsPostBack)
            {
                if (!X.IsAjaxRequest)
                {
                    try
                    {
                        BuildTree(TreePanel1.Root);
                        StoreDataBind();
                        GridDataBind(GetWhere());
                    }
                    catch (Exception ex)
                    {
                        ILog.WriteErrorLog(ex);
                        logManager.InsertLogError("ImportantManager.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
                    }
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, "" + GetLangStr("ImportantManager58", "访问：") + "" + Request.QueryString["funcname"], userinfo.NowIp, "0");
                    this.DataBind();
                }
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
                logManager.InsertLogError("ImportantManager.aspx-ToXml", ex.Message + "；" + ex.StackTrace, "ToXml has an exception");
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
                logManager.InsertLogError("ImportantManager.aspx-ToExcel", ex.Message + "；" + ex.StackTrace, "ToExcel has an exception");
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
                logManager.InsertLogError("ImportantManager.aspx-ToCsv", ex.Message + "；" + ex.StackTrace, "ToCsv has an exception");
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
                    string xml = Bll.Common.GetPrintXml(GetLangStr("ImportantManager59", "警务车辆信息"), "", "", "printdatatable");
                    string js = "OpenPrintPageV(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-ButPrintClick", ex.Message + "；" + ex.StackTrace, "ButPrintClick has an exception");
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
                CmbVehileType.Reset();
                uiDepartment.Reset();
                CmbPlateType.Reset();
                this.TxtplateId.Reset();
                this.WindowEditor1.Reset();
                this.ResourceManager1.RegisterAfterClientInitScript("clearTime();");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-ButResetClick", ex.Message + "；" + ex.StackTrace, "ButResetClick has an exception");
            }
        }

        /// <summary>
        /// 刷新重点车辆数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                GridDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh has an exception");
            }
        }

        protected void Select_LeiXing(object sender, DirectEventArgs e)
        {
            ButUpdate.Text = GetLangStr("ImportantManager60", "添加");
        }

        /// <summary>
        /// 添加或更新重点车辆信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GengXin_Click(object sender, DirectEventArgs e)
        {
            try
            {
                //储存修改前数据
                string[] updt = new string[] { uiDepartment1.DepertName, TxtHphm.Text, CmbHpzl.Value.ToString(), };

                UserInfo userinfo = Session["userinfo"] as UserInfo;
                Hashtable hs = new Hashtable();
                //当所属机构为空时 默认为
                if (string.IsNullOrEmpty(uiDepartment1.DepertId))
                {
                    hs.Add("GLBM", userinfo.DeptCode);
                }
                else
                {
                    hs.Add("GLBM", uiDepartment1.DepertId);
                }
                hs.Add("HPHM", VehicleHead.VehicleText + this.TxtHphm.Text.ToUpper());
                if (!string.IsNullOrEmpty(TxtClxh.Text))
                {
                    hs.Add("CLXH", this.TxtClxh.Text.ToString());
                }
                else
                {
                    hs.Add("CLXH", "");
                }
                if (!string.IsNullOrEmpty(TxtCjh.Text))
                {
                    hs.Add("CJH", this.TxtCjh.Text.ToString());
                }
                else
                {
                    hs.Add("CJH", "");
                }
                if (!string.IsNullOrEmpty(TxtFdjh.Text))
                {
                    hs.Add("FDJH", this.TxtFdjh.Text.ToString());
                }
                else
                {
                    hs.Add("FDJH", "");
                }
                if (!string.IsNullOrEmpty(TxtHdkl.Text))
                {
                    hs.Add("HDKL", this.TxtHdkl.Text.ToString());
                }
                else
                {
                    hs.Add("HDKL", "");
                }
                if (TxtSjxm.Text != "")
                {
                    hs.Add("SJXM", this.TxtSjxm.Text.ToString());
                }
                else
                {
                    hs.Add("SJXM", "");
                }
                if (!string.IsNullOrEmpty(TxtLxdh.Text))
                {
                    hs.Add("LXDH", this.TxtLxdh.Text.ToString());
                }
                else
                {
                    hs.Add("LXDH", "");
                }
                if (!string.IsNullOrEmpty(TxtSjhm.Text))
                {
                    hs.Add("SJHM", this.TxtSjhm.Text.ToString());
                }
                else
                {
                    hs.Add("SJHM", "");
                }
                if (!string.IsNullOrEmpty(TxtHjhm.Text))
                {
                    hs.Add("HJHM", this.TxtHjhm.Text.ToString());
                }
                else
                {
                    hs.Add("HJHM", "");
                }

                hs.Add("HPZL", this.CmbHpzl.Text.ToString());
                if (this.CmbClzl.SelectedIndex != -1)
                {
                    hs.Add("CLZL", this.CmbClzl.Text.ToString());
                }
                else
                {
                    hs.Add("CLZL", "");
                }
                if (this.CmbGPS.SelectedIndex != -1)
                {
                    hs.Add("GPS", this.CmbGPS.Text.ToString());
                }
                else
                {
                    hs.Add("GPS", "");
                }
                string lurutime = DateField1.SelectedDate.ToString("yyyy-MM-dd") + " 00:00:00";
                hs.Add("GZRQ", lurutime);
                if (!string.IsNullOrEmpty(TxtRyzk.Text))
                {
                    hs.Add("RYZK", this.TxtRyzk.Text.ToString());
                }
                else
                {
                    hs.Add("RYZK", "");
                }
                if (!string.IsNullOrEmpty(TxtClpp.Text))
                {
                    hs.Add("CLPP", this.TxtClpp.Text.ToString());
                }
                else
                {
                    hs.Add("CLPP", "");
                }
                if (!string.IsNullOrEmpty(TxtClzt.Text))
                {
                    hs.Add("CLZT", this.TxtClzt.Text.ToString());
                }
                else
                {
                    hs.Add("CLZT", "");
                }
                if (!string.IsNullOrEmpty(this.TimeStart.Value.ToString()))
                { hs.Add("KSSJ", this.TimeStart.Value.ToString().Substring(0, 5)); }
                else
                { hs.Add("KSSJ", ""); }

                if (!string.IsNullOrEmpty(this.TimeEnd.Value.ToString()))
                { hs.Add("JSSJ", this.TimeEnd.Value.ToString().Substring(0, 5)); }
                else
                { hs.Add("JSSJ", ""); }
                if (Session["stationlist"] != null)
                {
                    List<string> listid = Session["stationlist"] as List<string>;
                    string kkids = "";
                    for (int i = 0; i < listid.Count; i++)
                    { kkids += (i == 0 ? "" : ",") + listid[i]; }
                    hs.Add("KKIDS", kkids);
                }
                else
                { hs.Add("KKIDS", ""); }
                if (Hidden1.Value.ToString() == "1")
                {
                    string ID = Math.Abs(Guid.NewGuid().ToString().GetHashCode()).ToString();
                    hs.Add("ID", ID);
                    if (serviceManager.insertVehicles(hs) > 0)
                    {
                        Window1.Hide();
                        Notice(GetLangStr("ImportantManager61", "信息提示"), GetLangStr("ImportantManager62", "添加成功"));
                        GridDataBind(GetWhere());
                        BuildTree(TreePanel1.Root);
                        TreePanel1.Render();
                        //添加记录日志
                        logManager.InsertLogRunning(userinfo.UserName, "" + GetLangStr("ImportantManager63", "添加：") + "" + ID + Request.QueryString["type"], userinfo.NowIp, "1");
                    }
                }
                if (Hidden1.Value.ToString() == "2")
                {
                    hs.Add("ID", Hidden_id.Value.ToString());
                    if (serviceManager.updateVehicles(hs) > 0)
                    {
                        Window1.Hide();
                        Notice(GetLangStr("ImportantManager61", "信息提示"), GetLangStr("ImportantManager64", "修改成功"));
                        //Ext.Net.X.Msg.Alert(GetLangStr("ImportantManager51", "信息提示"), GetLangStr("ImportantManager52", "修改成功")).Show();
                        GridDataBind(GetWhere());
                        BuildTree(TreePanel1.Root);
                        TreePanel1.Render();
                        //修改记录日志
                    }
                }
                FrmClear();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-GengXin_Click", ex.Message + "；" + ex.StackTrace, "GengXin_Click has an exception");
            }
        }

        /// <summary>
        /// 添加重点车辆信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButDevAdd_Click(object sender, DirectEventArgs e)
        {
            try
            {
                FrmClear();
                ButUpdate.Text = GetLangStr("ImportantManager65", "添加");
                Window1.Title = GetLangStr("ImportantManager66", "添加车辆信息");
                ButUpdate.Hidden = false;
                DateField1.MaxDate = DateTime.Now;
                this.Hidden1.Value = "1";
                Session["iszdcl"] = "true";
                Window1.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-ButDevAdd_Click", ex.Message + "；" + ex.StackTrace, "ButDevAdd_Click has an exception");
            }
        }

        /// <summary>
        /// 刷新重点车辆信息数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButRefreshClick(object sender, DirectEventArgs e)
        {
            try
            {
                GridDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-ButRefreshClick", ex.Message + "；" + ex.StackTrace, "ButRefreshClick has an exception");
            }
        }

        /// <summary>
        /// 查询重点车辆信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                GridDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        #endregion 事件集合

        #region 私有方法

        /// <summary>
        /// 转换dataTable
        /// </summary>
        /// <returns></returns>
        private DataTable ChangeDataTable()
        {
            try
            {
                DataTable dt = Session["datatable"] as DataTable;
                DataTable dt2 = dt.Copy();
                if (dt2 != null)
                {
                    dt2.Columns.Remove("col0");
                    dt2.Columns.Remove("col6");
                    dt2.Columns.Remove("col7");

                    dt2.Columns[0].ColumnName = GetLangStr("ImportantManager23", "号牌号码").Replace(" ", "_");
                    dt2.Columns[1].ColumnName = GetLangStr("ImportantManager24", "号牌种类").Replace(" ", "_");
                    dt2.Columns[2].ColumnName = GetLangStr("ImportantManager25", "车辆类型").Replace(" ", "_");
                    dt2.Columns[3].ColumnName = GetLangStr("ImportantManager26", "司机姓名").Replace(" ", "_");
                    dt2.Columns[4].ColumnName = GetLangStr("ImportantManager27", "呼叫号码").Replace(" ", "_");
                    dt2.Columns[5].ColumnName = GetLangStr("ImportantManager28", "联系电话").Replace(" ", "_");
                    dt2.Columns[6].ColumnName = GetLangStr("ImportantManager29", "所属机构").Replace(" ", "_");
                }

                return dt2;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-ChangeDataTable", ex.Message + "；" + ex.StackTrace, "ChangeDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        /// 为页面中的下拉列表绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                this.StoreVehileType.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240050"));// tgsPproperty.GetDeviceTypeDict("240050");
                this.StoreVehileType.DataBind();

                this.StoreGPS.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:360900"));//tgsPproperty.GetDeviceTypeDict("360900");
                this.StoreGPS.DataBind();

                this.StoreDepart.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_cfg_department"));//settingManager.GetDepartmentDict("00");
                this.StoreDepart.DataBind();

                //车俩类型
                this.StorePlateType.DataSource = GetRedisData.GetData("t_sys_code:140001");
                this.StorePlateType.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 创建部门列表树形菜单
        /// </summary>
        /// <param name="nodes"></param>
        private void BuildTree(Ext.Net.TreeNodeCollection nodes)
        {
            try
            {
                TreePanel1.RemoveAll(true);

                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }

                Ext.Net.TreeNode root = new Ext.Net.TreeNode();

                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;
                DataTable dt = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240050"));//把字典从400200改成240050   2016-11-17
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Addree(dt, "", root, null);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
            }
        }

        /// <summary>
        /// 获取某一车辆类型的数据数量
        /// </summary>
        /// <param name="departid"></param>
        /// <returns></returns>
        private string GetCount(string id)
        {
            try
            {
                DataTable dt = serviceManager.GetVehicles(" and  ZDCLLX='" + id + "'");

                if (dt != null)
                {
                    return dt.Rows.Count.ToString();
                }
                return "0";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-GetCount", ex.Message + "；" + ex.StackTrace, "GetCount has an exception");
                return null;
            }
        }

        /// <summary>
        /// 获取某一管理部门的数据数量
        /// </summary>
        /// <param name="departid"></param>
        /// <returns></returns>
        private string GetCount2(string departid, string id)
        {
            try
            {
                DataTable dt = serviceManager.GetVehicles(" and ZDCLLX='" + id + "' and  GLBM='" + departid + "'");

                if (dt != null)
                {
                    return dt.Rows.Count.ToString();
                }
                return "0";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-GetCount2", ex.Message + "；" + ex.StackTrace, "GetCount2 has an exception");
                return null;
            }
        }

        /// <summary>
        /// 递归产生系统表树形菜单节点
        /// </summary>
        /// <param name="allNodeTable"></param>
        /// <param name="parentColValue"></param>
        /// <param name="root"></param>
        /// <param name="ParentNode"></param>
        private void Addree(DataTable allNodeTable, string parentColValue, Ext.Net.TreeNode root, Ext.Net.TreeNode ParentNode)//Ext.Net.TreeNode ParentNode
        {
            try
            {
                DataRow[] myDataRows = allNodeTable.Select("1=1");

                foreach (DataRow myDataRow in myDataRows)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();

                    ConfigItem ci0 = new ConfigItem("col0", myDataRow[0].ToString(), ParameterMode.Value);
                    ConfigItem ci1 = new ConfigItem("col1", myDataRow[1].ToString(), ParameterMode.Value);

                    node.Text = myDataRow[1].ToString() + "(" + GetCount(myDataRow[0].ToString()) + ")";

                    node.NodeID = myDataRow[0].ToString();
                    node.Leaf = true;
                    node.Draggable = true;
                    node.Expandable = ThreeStateBool.True;
                    node.Expanded = false;
                    node.Icon = Icon.Car;
                    node.Listeners.Click.Handler = "selectNode('" + myDataRow[0].ToString() + "') ;";

                    node.CustomAttributes.Add(ci0);
                    node.CustomAttributes.Add(ci1);

                    root.Nodes.Add(node);
                    AddTree(node, myDataRow[0].ToString());
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-Addree", ex.Message + "；" + ex.StackTrace, "Addree has an exception");
            }
        }

        /// <summary>
        /// 创建部门列表树
        /// </summary>
        private void AddTree(Ext.Net.TreeNode root, string id)
        {
            try
            {
                DataTable dt = Bll.Common.ChangColName(GetRedisData.GetData("t_cfg_department"));  //settingManager.GetConfigDepartment("0");
                if (dt.Rows.Count > 0)
                {
                    Addree2(dt, dt.Rows[0]["col3"].ToString(), root, null, id);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-AddTree", ex.Message + "；" + ex.StackTrace, "AddTree has an exception");
            }
        }

        /// <summary>
        /// 递归产生部门表树形菜单节点
        /// </summary>
        /// <param name="allNodeTable"></param>
        /// <param name="parentColValue"></param>
        /// <param name="root"></param>
        /// <param name="ParentNode"></param>
        private void Addree2(DataTable allNodeTable, string parentColValue, Ext.Net.TreeNode root, Ext.Net.TreeNode ParentNode, string id)
        {
            try
            {
                DataRow[] myDataRows = allNodeTable.Select("col3 ='" + parentColValue + "'");

                foreach (DataRow myDataRow in myDataRows)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    ConfigItem ci0 = new ConfigItem("col0", myDataRow[2].ToString(), ParameterMode.Value);
                    ConfigItem ci1 = new ConfigItem("col1", myDataRow[5].ToString(), ParameterMode.Value);
                    ConfigItem ci2 = new ConfigItem("col2", myDataRow[8].ToString(), ParameterMode.Value);
                    node.Text = myDataRow[2].ToString() + "(" + GetCount2(myDataRow[1].ToString(), id) + ")";
                    node.NodeID = myDataRow[1].ToString() + id;
                    node.Leaf = true;
                    node.Draggable = true;
                    node.Expandable = ThreeStateBool.True;
                    node.Expanded = false;
                    node.Icon = Icon.House;
                    node.Listeners.Click.Handler = "selectNode2('" + myDataRow[1].ToString() + "','" + id + "') ;";
                    node.CustomAttributes.Add(ci0);
                    node.CustomAttributes.Add(ci1);
                    node.CustomAttributes.Add(ci2);

                    if (ParentNode != null)
                    {
                        ParentNode.Nodes.Add(node);
                        Addree2(allNodeTable, myDataRow["col1"].ToString(), ParentNode, node, id);
                    }
                    else
                    {
                        root.Nodes.Add(node);
                        Addree2(allNodeTable, myDataRow["col1"].ToString(), root, node, id);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-Addree2", ex.Message + "；" + ex.StackTrace, "Addree2 has an exception");
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        private void FrmClear()
        {
            VehicleHead.SetVehicleText("");
            TxtHphm.Text = null;
            TxtClxh.Text = null;
            TxtCjh.Text = null;
            TxtFdjh.Text = null;
            TxtHdkl.Text = null;
            TxtSjxm.Text = null;
            TxtLxdh.Text = null;
            TxtSjhm.Text = null;
            TxtClzt.Text = null;
            TxtClpp.Text = null;
            TxtHjhm.Text = null;
            uiDepartment1.DepertId = "";//.Reset();
            CmbHpzl.Value = null;
            CmbHpzl.Value = null;
            CmbGPS.Value = null;
            CmbClzl.Value = null;
            DateField1.Text = null;
            TxtRyzk.Text = null;
            TimeStart.Clear();
            TimeEnd.Clear();
            TxtKkid.Clear();
        }

        /// <summary>
        /// 获取重点车辆数据
        /// </summary>
        /// <param name="where"></param>
        private void GridDataBind(string where)
        {
            try
            {
                DataTable dt = serviceManager.GetVehiclese(where);
                StoreDeviceManager.DataSource = dt;
                StoreDeviceManager.DataBind();

                Session["datatable"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    SelectFirst(dt.Rows[0]);
                    ButExcel.Disabled = false;
                }
                else
                {
                    ButExcel.Disabled = true;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-GridDataBind", ex.Message + "；" + ex.StackTrace, "GridDataBind has an exception");
            }
        }

        private void SelectFirst(DataRow dr)
        {
        }

        /// <summary>
        /// 获取选中车辆的信息
        /// </summary>
        /// <param name="id"></param>
        private void select(string id)
        {
            try
            {
                DataTable da = serviceManager.GetVehicles(" AND ID='" + id + "'");
                this.Hidden_id.Value = da.Rows[0]["col0"].ToString();
                CmbHpzl.Value = da.Rows[0]["col18"].ToString();
                string Hphm = da.Rows[0]["col1"].ToString();
                if (!string.IsNullOrEmpty(Hphm))
                {
                    VehicleHead.SetVehicleText(Hphm.Substring(0, 1));
                    TxtHphm.Text = Hphm.Substring(1);
                }
                CmbClzl.Value = da.Rows[0]["col16"].ToString();
                TxtClpp.Text = da.Rows[0]["col2"].ToString();
                TxtClxh.Text = da.Rows[0]["col3"].ToString();
                TxtCjh.Text = da.Rows[0]["col4"].ToString();
                TxtFdjh.Text = da.Rows[0]["col5"].ToString();
                TxtHdkl.Text = da.Rows[0]["col6"].ToString();
                string[] lurutiem = (da.Rows[0]["col7"].ToString()).Split(' ');
                DateField1.Text = lurutiem[0].ToString();
                TxtSjxm.Text = da.Rows[0]["col8"].ToString();
                TxtLxdh.Text = da.Rows[0]["col10"].ToString();
                CmbGPS.Value = da.Rows[0]["col11"].ToString();
                TxtSjhm.Text = da.Rows[0]["col9"].ToString();
                TxtRyzk.Text = da.Rows[0]["col14"].ToString();
                TxtClzt.Text = da.Rows[0]["col13"].ToString();
                TxtHjhm.Text = da.Rows[0]["col12"].ToString();
                //uiDepartment.DepertName = da.Rows[0]["col16"].ToString();
                uiDepartment1.DepertId = da.Rows[0]["col15"].ToString();
                try
                {
                    this.TimeStart.Text = (da.Rows[0]["col19"] == null ? "" : da.Rows[0]["col19"].ToString());
                }
                catch
                { this.TimeStart.Text = ""; }
                try
                {
                    this.TimeEnd.Text = (da.Rows[0]["col20"] == null ? "" : da.Rows[0]["col20"].ToString());
                }
                catch { this.TimeEnd.Text = ""; }
                try
                {
                    string kkids = (da.Rows[0]["col21"] == null ? "" : da.Rows[0]["col21"].ToString());
                    List<string> list = new List<string>();
                    DataTable dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
                    foreach (string str in kkids.Split(','))
                    {
                        list.Add(str);
                        DataRow[] rowsStation = dtStation.Select(" station_id='" + str + "' ", "station_name asc");
                        if (rowsStation != null && rowsStation.Length > 0)
                            this.TxtKkid.Text += rowsStation[0]["station_name"].ToString() + "\r\n";
                    }
                    Session["stationlist"] = list;
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-select", ex.Message + "；" + ex.StackTrace, "select has an exception");
            }
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            try
            {
                string where = " ";

                string QueryHphm = string.Empty;
                if (!string.IsNullOrEmpty(WindowEditor1.VehicleText))
                {
                    QueryHphm = WindowEditor1.VehicleText + TxtplateId.Text;
                }
                else
                {
                    QueryHphm = TxtplateId.Text;
                }
                if (!string.IsNullOrEmpty(QueryHphm))
                {
                    where = where + " and  hphm like '%" + QueryHphm.ToUpper() + "%' ";
                }
                if (CmbVehileType.SelectedIndex != -1)
                {
                    where = where + " and  ZDCLLX='" + CmbVehileType.SelectedItem.Value + "' ";
                }
                if (!string.IsNullOrEmpty(uiDepartment.DepertId))
                {
                    where = where + " and  glbm='" + uiDepartment.DepertId + "' ";
                }
                if (CmbPlateType.SelectedIndex != -1)
                {
                    where = where + " and  hpzl='" + CmbPlateType.SelectedItem.Value + "' ";
                }
                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-GetWhere", ex.Message + "；" + ex.StackTrace, "GetWhere has an exception");
            }
            return null;
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
                logManager.InsertLogError("ImportantManager.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice has an exception");
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
                logManager.InsertLogError("ImportantManager.aspx-GetLangStr", ex.Message + "；" + ex.StackTrace, "GetLangStr has an exception");
            }
            return null;
        }

        #endregion 私有方法

        #region DirectMethod

        /// <summary>
        /// 获取相应车辆类型的重点车辆信息
        /// </summary>
        /// <param name="code"></param>
        [DirectMethod(Namespace = "OnEvl")]
        public void onclickTree(string code)
        {
            try
            {
                GridDataBind(" and ZDCLLX='" + code + "' ");
                CmbVehileType.Value = code;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-onclickTree", ex.Message + "；" + ex.StackTrace, "onclickTree has an exception");
            }
        }

        /// <summary>
        /// 获取相应管理部门的重点车辆信息
        /// </summary>
        /// <param name="code"></param>
        [DirectMethod(Namespace = "OnEvl")]
        public void onclickTree2(string code, string id)
        {
            try
            {
                GridDataBind(" and ZDCLLX='" + id + "' and GLBM='" + code + "'");
                uiDepartment.DepertId = code;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-onclickTree2", ex.Message + "；" + ex.StackTrace, "onclickTree2 has an exception");
            }
        }

        /// <summary>
        /// 获取选中车辆的信息
        /// </summary>
        /// <param name="id"></param>
        [DirectMethod(Namespace = "OnEvl")]
        public void onclick(string id)
        {
            try
            {
                select(id);
                Window1.Title = GetLangStr("ImportantManager67", "查看车辆信息");
                ButUpdate.Hidden = true;
                Window1.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-onclick", ex.Message + "；" + ex.StackTrace, "onclick has an exception");
            }
        }

        /// <summary>
        /// 修改重点车辆信息
        /// </summary>
        /// <param name="id"></param>
        [DirectMethod(Namespace = "OnEvl")]
        public void update(string id)
        {
            try
            {
                Session["iszdcl"] = "true";
                FrmClear();
                select(id);
                Window1.Title = GetLangStr("ImportantManager68", "修改车辆信息");
                ButUpdate.Hidden = false;
                ButUpdate.Text = GetLangStr("ImportantManager69", "修改");
                this.Hidden1.Value = "2";
                DateField1.MaxDate = DateTime.Now;//设置时间框只能选择比当前日期小的
                Window1.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-update", ex.Message + "；" + ex.StackTrace, "update has an exception");
            }
        }

        /// <summary>
        /// 删除重点车辆信息
        /// </summary>
        /// <param name="id"></param>
        [DirectMethod(Namespace = "OnDel")]
        public void delecte(string id)
        {
            try
            {
                if (serviceManager.DeleteVehicles(id) > 0)
                {
                    GridDataBind(GetWhere());
                    BuildTree(TreePanel1.Root);
                    TreePanel1.Render();
                    Notice(GetLangStr("ImportantManager61", "信息提示"), GetLangStr("ImportantManager70", "删除成功"));
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, "" + GetLangStr("ImportantManager71", "删除：") + "" + id + Request.QueryString["type"], userinfo.NowIp, "3");
                }
                else
                {
                    Notice(GetLangStr("ImportantManager61", "信息提示"), GetLangStr("ImportantManager72", "删除失败"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-delecte", ex.Message + "；" + ex.StackTrace, "delecte has an exception");
            }
        }

        #endregion DirectMethod

        [DirectMethod(Namespace = "OnDel")]
        public void showkkid(string names)
        {
            this.TxtKkid.Text = "";
            foreach (string str in names.Split(','))
            {
                this.TxtKkid.Text += str + "\r\n";
            }
        }

        /// <summary>
        /// 转换dataTable为Hashtable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public Hashtable ConventDataTable(DataTable dt)
        {
            Hashtable hts = new Hashtable();
            try
            {
                int j = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string kkType = dt.Rows[i]["col2"].ToString();
                    if (!hts.ContainsKey(kkType))
                    {
                        j++;
                        hts.Add(kkType, dt.Rows[i]["col1"].ToString());
                    }
                }
                return hts;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantManager.aspx-ConventDataTable", ex.Message + "；" + ex.StackTrace, "ConventDataTable has an exception");
                return null;
            }
        }
    }
}