using System;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebAlarmQuery : System.Web.UI.Page
    {
        #region 成员变量
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private SettingManager settingManager = new SettingManager();
        private Bll.ServiceManager servicemansger = new Bll.ServiceManager();
        private UserLogin userLogin = new UserLogin();
        private DataCommon dataCommon = new DataCommon();
        private static string starttime = "";
        private static string endtime = "";

        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
           string username=Request.QueryString["username"];  if (!userLogin.CheckLogin(username)) { string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" +StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }
            if (!X.IsAjaxRequest)
            {
                string datetime = Request.QueryString["datetime"];

                DataSetDateTime(datetime);
                StoreDataBind();
                BuildTree(TreeStation.Root);
                TbutQueryClick(null, null);
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
                this.FormPanel2.Title = "查询结果：共计查询出符合条件的记录0条,现在显示0条";
                GetData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);

                logManager.InsertLogError("WebAlarmQuery.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");
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
                string data = e.ExtraParams["data"];
                AddWindow(data);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-ShowDetails", ex.Message+"；"+ex.StackTrace, "ShowDetails has an exception");
            }
        }

        /// <summary>
        /// 上一页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutLast(object sender, DirectEventArgs e)
        {
            try
            {
                int page = int.Parse(curpage.Value.ToString());
                page--;
                if (page < 1)
                {
                    page = 1;
                }
                curpage.Value = page;
                ShowQuery(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-TbutLast", ex.Message+"；"+ex.StackTrace, "TbutLast has an exception");
            }
        }

        /// <summary>
        ///下一页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutNext(object sender, DirectEventArgs e)
        {
            try
            {
                int page = int.Parse(curpage.Value.ToString());
                int allpage = int.Parse(allPage.Value.ToString());
                page++;
                if (page > allpage)
                {
                    page = allpage;
                }
                curpage.Value = page;
                ShowQuery(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-TbutNext", ex.Message+"；"+ex.StackTrace, "TbutNext has an exception");
            }
        }

        /// <summary>
        ///首页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutFisrt(object sender, DirectEventArgs e)
        {
            try
            {
                curpage.Value = 1;
                ShowQuery(1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-TbutFisrt", ex.Message+"；"+ex.StackTrace, "TbutFisrt has an exception");
            }
        }

        /// <summary>
        ///尾页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutEnd(object sender, DirectEventArgs e)
        {
            try
            {
                curpage.Value = allPage.Value;
                int page = int.Parse(curpage.Value.ToString());
                ShowQuery(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-TbutEnd", ex.Message+"；"+ex.StackTrace, "TbutEnd has an exception");
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
                GetData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-MyData_Refresh", ex.Message+"；"+ex.StackTrace, "MyData_Refresh has an exception");
            }
        }

        #endregion 控件事件

        #region DirectMethod

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
                logManager.InsertLogError("WebAlarmQuery.aspx-GetDateTime", ex.Message+"；"+ex.StackTrace, "GetDateTime has an exception");
            }
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
                this.StorePlateType.DataSource = tgsPproperty.GetPalteType();
                this.StorePlateType.DataBind();

                this.StoreAlarmType.DataSource = tgsPproperty.GetCommonAlarmTypeDict();
                this.StoreAlarmType.DataBind();
                string type = Request.QueryString["type"];
                switch (type)
                {
                    case "4":
                        CmbAlarmType.Value="300108";
                        break;

                    case "5":
                        CmbAlarmType.Value="300109";
                        break;

                    case "6":
                        CmbAlarmType.Value="300104";
                        break;

                    case "7":
                        CmbAlarmType.Value="300102";
                        break;

                    case "8":
                        CmbAlarmType.Value="300107";
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-StoreDataBind", ex.Message+"；"+ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sdata"></param>
        private void AddWindow(string sdata)
        {
            try
            {
                //DataTable dt = tgsDataInfo.GetPassCarImageUrl(Bll.Common.GetdatabyField(sdata, "col0"));
                //Window win = WindowShow.AddAlarmCar(sdata,dt);
                //win.Render(this.Form);
                //win.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-AddWindow", ex.Message+"；"+ex.StackTrace, "AddWindow has an exception");
            }
        }

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="page"></param>
        private void SetButState(int page)
        {
            try
            {
                curpage.Value = page;
                int allpage = int.Parse(allPage.Value.ToString());
                this.PagingToolbar1.PageIndex = 0;
                if (allpage > 1)
                {
                    ButLast.Disabled = false;
                    ButNext.Disabled = false;
                    ButFisrt.Disabled = false;
                    ButEnd.Disabled = false;
                }
                if (page == 1)
                {
                    ButLast.Disabled = true;
                    ButFisrt.Disabled = true;
                }
                if (page == allpage)
                {
                    ButNext.Disabled = true;
                    ButEnd.Disabled = true;
                }
                if (allpage <= 1)
                {
                    ButFisrt.Disabled = true;
                    ButNext.Disabled = true;
                    ButLast.Disabled = true;
                    ButEnd.Disabled = true;
                }
                LabNum.Html = "<font >&nbsp;&nbsp;当前" + page.ToString() + "页,共" + allpage.ToString() + "页</font>";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-SetButState", ex.Message+"；"+ex.StackTrace, "SetButState has an exception");
            }
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startNum"></param>
        /// <param name="endNum"></param>
        private void Query(string where, int startNum, int endNum)
        {
            try
            {
                DataTable dt = tgsDataInfo.GetAlarmInfo(where, startNum, endNum);
                Session["datatable"] = dt;
                this.StoreAlarmInfo.DataSource = dt;
                this.StoreAlarmInfo.DataBind();
                int realnum = startNum + dt.Rows.Count - 1;

                if (realnum == 0)
                {
                    this.FormPanel2.Title = "查询结果：共计查询出符合条件的记录" + realCount.Value.ToString() + "条";
                    Notice("信息提示", "未查询到相关记录");
                }
                else
                {
                    this.FormPanel2.Title = "查询结果：共计查询出符合条件的记录" + realCount.Value.ToString() + "条,现在显示" + startNum + " - " + realnum + "条";
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-Query", ex.Message+"；"+ex.StackTrace, "Query has an exception");
            }
        }

        /// <summary>
        /// 展示选中页数据
        /// </summary>
        /// <param name="currentPage"></param>
        private void ShowQuery(int currentPage)
        {
            try
            {
                int rownum = 50;
                int startNum = (currentPage - 1) * rownum + 1;
                int endNum = currentPage * rownum;
                Query(GetWhere(), startNum, endNum);
                SetButState(currentPage);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-ShowQuery", ex.Message+"；"+ex.StackTrace, "ShowQuery has an exception");
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        private void GetData()
        {
            try
            {
                DataTable dt = new DataTable();

                DataTable tempdt = tgsDataInfo.GetAlarmMaxBjsj(GetWhere());
                if (tempdt != null && tempdt.Rows.Count > 0)
                {
                    realMaxTime.Value = tempdt.Rows[0]["col0"].ToString();
                    realMinTime.Value = tempdt.Rows[0]["col1"].ToString();
                    realCount.Value = tempdt.Rows[0]["col2"].ToString();
                    curpage.Value = 1;
                    int rownum = 50;
                    allPage.Value = (int)Math.Ceiling(double.Parse(realCount.Value.ToString()) / rownum);
                    ShowQuery(1);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-GetData", ex.Message+"；"+ex.StackTrace, "GetData has an exception");
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
                    Html = "<br></br>" + msg + "!"
                });
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-Notice", ex.Message+"；"+ex.StackTrace, "Notice has an exception");
            }
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            string where = "1=1";
            try
            {
                if (string.IsNullOrEmpty(starttime)) starttime = start.InnerText;
                if (string.IsNullOrEmpty(endtime)) endtime = end.InnerText;
                string kssj = DateTime.Parse(starttime).ToString("yyyy-MM-dd HH:mm:ss");
                string jssj = DateTime.Parse(endtime).ToString("yyyy-MM-dd HH:mm:ss");
                where = "  bjsj >= STR_TO_DATE('" + kssj + "','%Y-%m-%d %H:%i:%s')   and bjsj<=STR_TO_DATE('" + jssj + "','%Y-%m-%d %H:%i:%s')";

                if (CmbPlateType.SelectedIndex != -1)
                {
                    where = where + " and  hpzl='" + CmbPlateType.SelectedItem.Value + "' ";
                }

                if (this.FieldStation.Value!=null)
                {
                    string kkid = this.FieldStation.Value.ToString();
                    if (!string.IsNullOrEmpty(kkid))
                    {
                        where = where + " and  kkid in (" + kkid + ") ";
                    }
                }

                string type = Request.QueryString["type"];
                if (!string.IsNullOrEmpty(type))
                {
                    where = where + " and  bjlx='" + CmbAlarmType.Value + "' ";
                }

                string QueryHphm = string.Empty;
                if (!string.IsNullOrEmpty(vehicleHead.VehicleText))
                {
                    QueryHphm = vehicleHead.VehicleText + TxtplateId.Text;
                }
                else
                {
                    QueryHphm = TxtplateId.Text;
                }

                if (!string.IsNullOrEmpty(QueryHphm))
                {
                    where = where + " and  hphm  like '%" + QueryHphm.ToUpper() + "%' ";
                }

                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-GetWhere", ex.Message+"；"+ex.StackTrace, "GetWhere has an exception");
                return where;
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
                this.FormPanel1.Collapsed = false;
                string sdata = e.ExtraParams["data"];
                string hphm = Bll.Common.GetdatabyField(sdata, "col3");
                string hpzl = Bll.Common.GetdatabyField(sdata, "col1");

                string url1 = Bll.Common.GetdatabyField(sdata, "col21");
                string url2 = Bll.Common.GetdatabyField(sdata, "col22");
                string url3 = Bll.Common.GetdatabyField(sdata, "col23");
                //string js = "ShowImage(\"" + dataCommon.ChangePoliceIp(url1) + "\",\"" + dataCommon.ChangePoliceIp(url2) + "\",\"" + dataCommon.ChangePoliceIp(url3) + "\",\"" + hphm + "\",\"" + hpzl + "\");";

                string js = "ShowImage(\"" + url1 + "\",\"" + url2 + "\",\"" + url3 + "\",\"" + hphm + "\",\"" + hpzl + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-ApplyClick", ex.Message+"；"+ex.StackTrace, "ApplyClick has an exception");
            }
        }

        /// <summary>
        /// 转换datatable
        /// </summary>
        /// <returns></returns>
        private DataTable ChangeDataTable()
        {
            DataTable dt = Session["datatable"] as DataTable;
            try
            {
                DataTable dt1 = dt.Copy();
                if (dt1 != null)
                {
                    dt1.Columns.Remove("col0"); dt1.Columns.Remove("col1"); dt1.Columns.Remove("col5"); dt1.Columns.Remove("col8");
                    for (int i = 10; i < dt.Columns.Count; i++)
                    {
                        dt1.Columns.Remove("col" + i.ToString());
                    }
                    dt1.Columns[5].SetOrdinal(0);
                    dt1.Columns[0].ColumnName = "报警卡口";
                    dt1.Columns[1].ColumnName = "号牌号码";
                    dt1.Columns[2].ColumnName = "号牌种类";
                    dt1.Columns[3].ColumnName = "报警时间";
                    dt1.Columns[4].ColumnName = "报警类型";
                    dt1.Columns[5].ColumnName = "报警原因";
                    dt1.Columns[6].ColumnName = "处理状态";

                    //PrintColumns pc = new PrintColumns();
                    //pc.Add(new PrintColumn("报警卡口", 9));
                    //pc.Add(new PrintColumn("号牌号码", 3));
                    //pc.Add(new PrintColumn("号牌种类", 2));
                    //pc.Add(new PrintColumn("报警时间", 4));
                    //pc.Add(new PrintColumn("行驶方向", 13));
                    //pc.Add(new PrintColumn("车道", 14));
                    //pc.Add(new PrintColumn("报警类型", 6));
                    //pc.Add(new PrintColumn("处理类型", 18));
                    //pc.Add(new PrintColumn("所属机构", 16));
                    //dt2 = Bll.Common.GetDataTablePrint(dt, pc);
                }

                return dt1;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-ChangeDataTable", ex.Message+"；"+ex.StackTrace, "ChangeDataTable has an exception");
                return dt;
            }
        }

        /// <summary>
        /// 设置初始时间
        /// </summary>
        private void DataSetDateTime(string date)
        {
            starttime = DateTime.Parse(date).ToString("yyyy-MM-dd 00:00:00");
            start.InnerText = starttime;

            endtime = DateTime.Parse(date).ToString("yyyy-MM-dd 23:59:59");
            end.InnerText = endtime;
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

                DataTable dtStation = tgsPproperty.GetStationInfo(" departid='" + user.DeptCode + "' ");
                AddStationTree(nodeRoot, dtStation);
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
                logManager.InsertLogError("WebAlarmQuery.aspx-BuildTree", ex.Message+"；"+ex.StackTrace, "BuildTree has an exception");
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
                DataTable dtDepart = settingManager.GetLowerDepartment(departCode);

                if (dtDepart != null && dtDepart.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDepart.Rows.Count; i++)
                    {
                        Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                        nodeRoot.Text = dtDepart.Rows[i][2].ToString();
                        nodeRoot.Leaf = true;
                        nodeRoot.NodeID = dtDepart.Rows[i][1].ToString();
                        nodeRoot.Icon = Icon.House;

                        DataTable dtStation = tgsPproperty.GetStationInfo(" departid='" + nodeRoot.NodeID + "' ");
                        AddStationTree(nodeRoot, dtStation);
                        nodeRoot.Expanded = false;
                        nodeRoot.Draggable = true;
                        nodeRoot.Expandable = ThreeStateBool.True;
                        AddDepartTree(nodeRoot, dtDepart.Rows[i][1].ToString());
                        root.Nodes.Add(nodeRoot);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-AddDepartTree", ex.Message+"；"+ex.StackTrace, "AddDepartTree has an exception");
            }
        }

        /// <summary>
        /// 添加卡口子节点
        /// </summary>
        /// <param name="root"></param>
        private void AddStationTree(Ext.Net.TreeNode DepartNode, DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                        node.Text = dt.Rows[i]["col2"].ToString();
                        node.Leaf = true;
                        node.Checked = ThreeStateBool.False;
                        node.NodeID = dt.Rows[i]["col1"].ToString();
                        node.Draggable = false;
                        node.AllowDrag = false;
                        DepartNode.Nodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebAlarmQuery.aspx-AddStationTree", ex.Message+"；"+ex.StackTrace, "AddStationTree has an exception");
            }
        }

        #endregion 私有方法
    }
}