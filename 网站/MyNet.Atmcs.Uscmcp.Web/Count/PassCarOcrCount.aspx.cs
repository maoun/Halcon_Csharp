using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PassCarOcrCount : System.Web.UI.Page
    {
        #region 成员变量

        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();
        private SettingManager settingManager = new SettingManager();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
 
        #endregion 成员变量

        #region 事件集合

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
            try
            {
                if (!X.IsAjaxRequest)
                {
                    CmbYear.Show();
                    CmbDay.Show();
                    CmbMonth.Show();
                    AddYear();
                    AddMonth();
                    AddDay();
                    CmbYear.SelectedItem.Value = DateTime.Now.Year.ToString();
                    CmbMonth.SelectedItem.Value = DateTime.Now.Month.ToString();
                    CmbDay.SelectedItem.Value = DateTime.Now.Day.ToString();
                    BuildTree(TreeStation.Root);
                    Session["flowcaption"] = "流量统计";
                    Session["flowxlable"] = "小时";
                    Session["printdatatable"] = null;
                    this.WebChartViewer1.Visible = false;
                    //ButPrint.Hidden = true;
                    ButExcel.Hidden = true;
                    //ButChart.Hidden = true;
                    this.DataBind();
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：用户登录", userinfo.NowIp, "0");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-Page_Load", ex.Message+"；"+ex.StackTrace, "Page_Load has an exception");
            }
        }

        protected void StoreDayRefresh(object sender, StoreRefreshDataEventArgs e)
        {
            List<object> data = new List<object>();

            try
            {
                if (CmbYear.SelectedItem.Value == "")
                {
                    CmbYear.SelectedItem.Value = DateTime.Now.Year.ToString();
                }
                int day = DateTime.Parse(CmbYear.SelectedItem.Value + "-" + CmbMonth.SelectedItem.Value + "-01").AddMonths(1).AddDays(-1).Day;
                for (int i = 1; i <= day; i++)
                {
                    string id = i.ToString();
                    string name = i.ToString() + "日";

                    data.Add(new { col0 = id, col1 = name });
                }

                this.StoreDay.DataSource = data;
                this.StoreDay.DataBind();
                this.CmbDay.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-StoreDayRefresh", ex.Message+"；"+ex.StackTrace, "StoreDayRefresh has an exception");
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
                string fdate = CmbYear.SelectedItem.Value + "-" + CmbMonth.SelectedItem.Value + "-" + CmbDay.SelectedItem.Value + "-";
                DataTable dt = null;
                List<string> countkkid = GetList();
                if (countkkid.Count > 0)
                {
                    dt = tgsDataInfo.PassCarOcr(countkkid, fdate.Substring(0, fdate.Length - 1));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ButExcel.Disabled = false;
                    }
                    else
                    {
                        ButExcel.Disabled = true;
                    }
                    Session["printdatatable"] = dt;
                    AddDataTable(dt); ;
                }
                else
                {
                    Notice("信息提示", "请在卡口列表中选择卡口");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");
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
                CmbYear.SelectedItem.Value = DateTime.Now.Year.ToString();
                CmbMonth.SelectedItem.Value = DateTime.Now.Month.ToString();
                CmbDay.SelectedItem.Value = DateTime.Now.Day.ToString();
                if (!string.IsNullOrEmpty(FieldStation.Text))
                {
                    string js = "directclear();";//调用的前台js方法名
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-ButResetClick", ex.Message+"；"+ex.StackTrace, "ButResetClick has an exception");
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButExcelClick(object sender, DirectEventArgs e)
        {
            try
            {
                DataTable dt = Session["datatable"] as DataTable; ;
                ConvertData.ExportExcel(dt, this);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-ButExcelClick", ex.Message+"；"+ex.StackTrace, "ButExcelClick has an exception");
            }
        }

        /// <summary>
        /// 图表打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButChartClick(object sender, DirectEventArgs e)
        {
            try
            {
                string xml = Bll.Common.GetPrintXml(Session["scaption"].ToString(), "", "", "printchart");
                string js = "OpenPrintChartH(\"" + xml + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-ButChartClick", ex.Message+"；"+ex.StackTrace, "ButChartClick has an exception");
            }
        }

        /// <summary>
        /// 统计打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButPrintClick(object sender, DirectEventArgs e)
        {
            try
            {
                string xml = Bll.Common.GetPrintXml(Session["scaption"].ToString(), "", "", "printdatatable");
                string js = "OpenPrintPageH(\"" + xml + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-ButPrintClick", ex.Message+"；"+ex.StackTrace, "ButPrintClick has an exception");
            }
        }

        #endregion 事件集合

        #region 私有方法

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
                    Icon = Icon.Error,
                    HideDelay = 2000,
                    Height = 120,
                    Html = "<br></br>" + msg + "!"
                });
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-Notice", ex.Message+"；"+ex.StackTrace, "Notice has an exception");
            }
        }

        /// <summary>
        /// 添加年（下拉列表）
        /// </summary>

        private void AddYear()
        {
            List<object> data = new List<object>();

            try
            {
                for (int i = -5; i < 5; i++)
                {
                    string id = DateTime.Now.AddYears(i).ToString("yyyy");
                    string name = DateTime.Now.AddYears(i).ToString("yyyy") + "年"; ;

                    data.Add(new { col0 = id, col1 = name });
                }

                this.StoreYear.DataSource = data;
                this.StoreYear.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-AddYear", ex.Message+"；"+ex.StackTrace, "AddYear has an exception");
            }
        }

        /// <summary>
        /// 添加月份（下拉列表）
        /// </summary>
        private void AddMonth()
        {
            List<object> data = new List<object>();

            try
            {
                for (int i = 1; i < 13; i++)
                {
                    string id = i.ToString();
                    string name = i.ToString() + "月";

                    data.Add(new { col0 = id, col1 = name });
                }
                this.StoreMonth.DataSource = data;
                this.StoreMonth.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-AddMonth", ex.Message+"；"+ex.StackTrace, "AddMonth has an exception");
            }
        }

        /// <summary>
        /// 添加日（下拉列表）
        /// </summary>
        private void AddDay()
        {
            List<object> data = new List<object>();

            try
            {
                int day = DateTime.Now.Day;
                for (int i = 1; i <= day; i++)
                {
                    string id = i.ToString();
                    string name = i.ToString() + "日";

                    data.Add(new { col0 = id, col1 = name });
                }

                this.StoreDay.DataSource = data;
                this.StoreDay.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-AddDay", ex.Message+"；"+ex.StackTrace, "AddDay has an exception");
            }
        }

        private static void ViewPdf(Stream fs)
        {
            try
            {
                byte[] buffer = new byte[fs.Length];
                fs.Position = 0;
                fs.Read(buffer, 0, (int)fs.Length);
                HttpContext.Current.Response.Clear();
                string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;FileName=" + filename + ".pdf");
                HttpContext.Current.Response.AddHeader("Content-Length", fs.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/pdf";
                fs.Close();

                HttpContext.Current.Response.BinaryWrite(buffer);
                HttpContext.Current.Response.OutputStream.Flush();
                HttpContext.Current.Response.OutputStream.Close();
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
               
            }
        }

        private double[] GetRowData(int index)
        {
            try
            {
                DataTable dt = Session["datatable"] as DataTable;
                DataRow dr = dt.Rows[index];
                double[] dub = new double[dt.Columns.Count - 1];
                for (int i = 0; i < dt.Columns.Count - 1; i++)
                {
                    dub[i] = double.Parse(dr[i + 1].ToString());
                }

                return dub;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-GetRowData", ex.Message+"；"+ex.StackTrace, "GetRowData has an exception");
                return null; ;
            }
        }

        /// <summary>
        /// 将选择的卡口的方向分隔存入List<>
        /// </summary>
        /// <returns></returns>
        private List<string> GetList()
        {
            try
            {
                List<string> lst = new List<string>();

                if (this.FieldStation.Value != null)
                {
                    string kkid = this.FieldStation.Value.ToString();
                    if (!string.IsNullOrEmpty(kkid))
                    {
                        DataTable dtDi = tgsPproperty.GetDirectionInfo("station_id in (" + kkid + ") ORDER BY station_id ,direction_id ASC");
                        if (dtDi != null && dtDi.Rows.Count > 0)
                        {
                            foreach (DataRow item in dtDi.Rows)
                            {
                                lst.Add(item["col0"].ToString() + "|" + item["col1"].ToString());
                            }
                        }
                    }
                }

                return lst;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-GetList", ex.Message+"；"+ex.StackTrace, "GetList has an exception");
                return null;
            }
        }

        /// <summary>
        /// 组件卡口列表树
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
                root.Text = "卡口列表";
                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;

                // 添加 自己机构节点 和卡口
                UserInfo user = Session["userinfo"] as UserInfo;
                if (user == null)
                {
                    user = new UserInfo();
                    user.DepartName = "滨州市交通警察支队";
                    user.DeptCode = "371600000000";
                }

                Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                nodeRoot.Text = user.DepartName;
                nodeRoot.Leaf = true;
                nodeRoot.NodeID = user.DeptCode;
                nodeRoot.Icon = Icon.House;
                DataTable dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
                DataRow[] rowsStation = dtStation.Select("departid='" + user.DeptCode + "'", "station_name asc");
                AddStationTree(nodeRoot, rowsStation);

                nodeRoot.Expanded = false;
                nodeRoot.Draggable = true;
                nodeRoot.Expandable = ThreeStateBool.True;
                root.Nodes.Add(nodeRoot);

                //绑定下级部门及下级部门卡口
                AddDepartTree(root, user.DeptCode);

                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-BuildTree", ex.Message+"；"+ex.StackTrace, "BuildTree has an exception");
                return null;
            }
        }

        /// <summary>
        ///绑定下级部门及下级部门卡口
        /// </summary>
        /// <param name="root"></param>
        private void AddDepartTree(Ext.Net.TreeNode root, string departCode)
        {
            try
            {
                DataTable dtDepart = GetRedisData.GetData("t_cfg_department");
                DataRow[] rows = dtDepart.Select("classcode='" + departCode + "'");
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count(); i++)
                    {
                        Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                        nodeRoot.Text = rows[i]["departname"].ToString();
                        nodeRoot.Leaf = true;
                        nodeRoot.NodeID = rows[i]["departid"].ToString();
                        nodeRoot.Icon = Icon.House;

                        DataTable dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
                        DataRow[] rowsStation = dtStation.Select(" departid='" + nodeRoot.NodeID + "' ", "station_name asc");
                        AddStationTree(nodeRoot, rowsStation);
                        nodeRoot.Expanded = false;
                        nodeRoot.Draggable = true;
                        nodeRoot.Expandable = ThreeStateBool.True;
                        AddDepartTree(nodeRoot, rows[i]["departid"].ToString());
                        root.Nodes.Add(nodeRoot);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-AddDepartTree", ex.Message+"；"+ex.StackTrace, "AddDepartTree has an exception");
            }
        }

        /// <summary>
        /// 添加卡口子节点
        /// </summary>
        /// <param name="root"></param>
        private void AddStationTree(Ext.Net.TreeNode DepartNode, DataRow[] rows)
        {
            try
            {
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count(); i++)
                    {
                        Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                        node.Text = rows[i]["station_name"].ToString();
                        node.Leaf = true;
                        node.Checked = ThreeStateBool.False;
                        node.NodeID = rows[i]["station_id"].ToString();
                        node.Draggable = false;
                        node.AllowDrag = false;
                        DepartNode.Nodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-AddStationTree", ex.Message+"；"+ex.StackTrace, "AddStationTree has an exception");
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
                logManager.InsertLogError("PassCarOcrCount.aspx-GetLangStr", ex.Message + "；" + ex.StackTrace, "GetLangStr发生异常");
                return null;
            }
        }

        #endregion 私有方法

        #region [DirectMethod]

        /// <summary>
        ///
        /// </summary>
        /// <param name="popMsg"></param>
        [DirectMethod]
        public void AddWindow(string popMsg)
        {
            try
            {
                Window window = new Window();
                window.ID = "ChartShow";
                window.Title = "统计信息详细信息";
                window.Width = Unit.Pixel(820);
                window.Height = Unit.Pixel(600);
                window.Modal = true;
                window.Collapsible = true;
                window.Maximizable = false;
                window.Resizable = false;
                window.Hidden = true;
                window.AutoLoad.Mode = LoadMode.Merge;

                Ext.Net.Panel tabs = new Ext.Net.Panel();
                tabs.ID = "TabPanel1";
                tabs.IDMode = IDMode.Explicit;
                tabs.Border = false;

                Ext.Net.Panel tab = new Ext.Net.Panel();
                tab.Title = "统计信息";
                tab.Padding = 5;
                tab.AnchorHorizontal = "100%";
                tabs.Add(tab);
                string[] strs = popMsg.Split(',');
                double[] data = GetRowData(int.Parse(strs[1]));
                ChartDirector.WebChartViewer webChartViewer = new ChartDirector.WebChartViewer();
                Bll.Common.CreateCurvelineChart(webChartViewer, data, "小时", "识别率%", strs[0] + " - 识别率曲线");
                webChartViewer.Visible = true;
                tab.ContentControls.Add(webChartViewer);
                Toolbar toolbar = new Ext.Net.Toolbar();
                ToolbarFill toolbarFill = new ToolbarFill();
                toolbar.Add(toolbarFill);
                window.BottomBar.Add(toolbar);
                CommonExt.AddButton(toolbar, "butCancel", "退出", "Cancel", window.ClientID + ".hide()");
                window.Items.Add(tabs);
                window.Render(this.Form);
                window.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-AddWindow", ex.Message+"；"+ex.StackTrace, "AddWindow has an exception");
            }
        }

        /// <summary>
        /// 将查询得到的数据展示出来
        /// </summary>
        /// <param name="dt"></param>
        [DirectMethod]
        private void AddDataTable(DataTable dt)
        {
            try
            {
                if (dt != null)
                {
                    this.StoreFlow.RemoveFields();
                    this.GridFlow.ColumnModel.Columns.Clear();
                    this.GridFlow.Reconfigure();
                    for (int i = 0; i < dt.Columns.Count - 1; i++)
                    {
                        RecordField field = new RecordField(dt.Columns[i].ColumnName, RecordFieldType.String);
                        this.StoreFlow.AddField(field);
                        Column col = new Column();
                        col.Header = dt.Columns[i].ColumnName;

                        if (i == 0)
                        {
                            col.Width = 180;
                        }
                        else
                        {
                            col.Width = 35;
                        }
                        col.Sortable = true;
                        col.DataIndex = dt.Columns[i].ColumnName;
                        GridFlow.AddColumn(col);
                    }
                    this.StoreFlow.DataSource = dt;
                    this.StoreFlow.DataBind();
                    List<List<double>> datas;
                    List<string> labels;
                    List<string> xLabels;
                    DataTable dtTemp = dt.Copy();
                    dtTemp.Columns.RemoveAt(dtTemp.Columns.Count - 1);
                    tgsDataInfo.GetLineChartData(dtTemp, out datas, out labels, out xLabels);

                    string sdate = CmbYear.SelectedItem.Value + "-" + CmbMonth.SelectedItem.Value + "-" + CmbDay.SelectedItem.Value;

                    Session["scaption"] = DateTime.Parse(sdate).ToString("yyyy年MM月dd日") + "识别率(%)统计";
                    Session["sxlable"] = "小时";

                    TabStrip1.ActiveTabIndex = 0;
                    GridFlow.Title = "统计结果[" + Session["scaption"].ToString() + "]";
                    MyNet.Atmcs.Uscmcp.Bll.Common.CreateAmplylineChart(this.WebChartViewer1, datas, labels, xLabels, Session["sxlable"].ToString(), "识别率（%）", Session["scaption"].ToString());
                    this.WebChartViewer1.Visible = true;
                    Session["datatable"] = dt;
                    pnlData.Render(this.WebChartViewer1, RenderMode.Auto);
                    //ButPrint.Hidden = false;
                    ButExcel.Hidden = false;
                    //ButChart.Hidden = false;
                }
                else
                {
                    // ButPrint.Hidden = true;
                    ButExcel.Hidden = true;
                    // ButChart.Hidden = true;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarOcrCount.aspx-AddDataTable", ex.Message+"；"+ex.StackTrace, "AddDataTable has an exception");
            }
        }

        #endregion [DirectMethod]
    }
}