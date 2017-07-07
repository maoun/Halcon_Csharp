using Ext.Net;
using InfoSoftGlobal;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class DeviceStatistics : System.Web.UI.Page
    {
        #region 成员变量

        private SettingManager settingManager = new SettingManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private MyNet.Atmcs.Uscmcp.Bll.DeviceManager deviceManager = new MyNet.Atmcs.Uscmcp.Bll.DeviceManager();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        private static string starttime = "";
        private static string endtime = "";
        private UserLogin userLogin = new UserLogin();

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
                string js = "alert('" + GetLangStr("DeviceStatistics10", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'"; 
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return; 
            }
            if (!X.IsAjaxRequest)
            {
                DataSetDateTime();
                this.comyangshi.Value = "01";
                Button2.Disabled = true;
                this.DataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("DeviceStatistics11", "访问：设备信息统计"), userinfo.NowIp, "0");

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
                GridDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                      logManager.InsertLogError("DeviceStatistics.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        /// <summary>
        /// 刷新事件
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
                logManager.InsertLogError("DeviceStatistics.aspx-ButRefreshClick", ex.Message+"；"+ex.StackTrace, "ButRefreshClick has an exception");
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeJs = "clearTime('" + starttime + "','" + endtime + "');";//js方法后面的分号一定要加上
            this.ResourceManager1.RegisterAfterClientInitScript(timeJs);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            //GridDataBind("rownum <100");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectDev_Click(object sender, DirectEventArgs e)
        {
            string id = e.ExtraParams["id"];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButDevAdd_Click(object sender, DirectEventArgs e)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButDevModify_Click(object sender, DirectEventArgs e)
        {
            string id = e.ExtraParams["sdata"];
        }

        /// <summary>
        /// 设备删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButDevDelete_Click(object sender, DirectEventArgs e)
        {
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
                    string xml = Bll.Common.GetPrintXml(GetLangStr("DeviceStatistics9", "设备信息运维统计"), "", "", "printdatatable");
                    string js = "OpenPrintPageV(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceStatistics.aspx-ButPrintClick", ex.Message+"；"+ex.StackTrace, "ButPrintClick has an exception");
            }
        }

        /// <summary>
        /// 导出xml
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
                logManager.InsertLogError("DeviceStatistics.aspx-ToExcel", ex.Message+"；"+ex.StackTrace, "ToExcel has an exception");
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
            }
        }

        #endregion 控件事件

        #region DirectMethod 方法

        /// <summary>
        /// 保存信息
        /// </summary>
        [DirectMethod]
        public void InfoSave()
        {
            try
            {
                AddLedManager();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceStatistics.aspx-InfoSave", ex.Message+"；"+ex.StackTrace, "InfoSave has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void DoConfirm()
        {
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void DoYes()
        {
            Hashtable hs = new Hashtable();
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void DoNo()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="s3"></param>
        /// <param name="s4"></param>
        [DirectMethod(Namespace = "OnEvl")]
        public void xuanze(string value, string s1, string s2, string s3, string s4)
        {
            try
            {
                StringBuilder xmlData = new StringBuilder();
                xmlData.Append("<chart showNames='" + GetLangStr("DeviceStatistics9", "设备信息运维统计") + "' decimalPrecision='0' baseFont='Arial' baseFontSize='12'>");
                xmlData.AppendFormat("<set label='" + GetLangStr("DeviceStatistics12", "领用") + "' value='{0}' />", s1);
                xmlData.AppendFormat("<set label='" + GetLangStr("DeviceStatistics13", "维修") + "' value='{0}' />", s2);
                xmlData.AppendFormat("<set label='" + GetLangStr("DeviceStatistics14", "入库") + "' value='{0}' />", s3);
                xmlData.AppendFormat("<set label='" + GetLangStr("DeviceStatistics15", "归还") + "' value='{0}' />", s4);

                xmlData.Append("</chart>");
                if (value == "01")
                {
                    panel.Html = FusionCharts.RenderChartHTML("../FusionCharts/Pie2D.swf", "", xmlData.ToString(), "chartid", "500", "400", false, false);
                }
                if (value == "02")
                {
                    panel.Html = FusionCharts.RenderChartHTML("../FusionCharts/Line.swf", "", xmlData.ToString(), "Sales", "500", "400", false, false);
                }
                if (value == "03")
                {
                    panel.Html = FusionCharts.RenderChartHTML("../FusionCharts/Column3D.swf", "", xmlData.ToString(), "chartid", "500", "400", false, false);
                }
                if (value == "04")
                {
                    // xmlData.Append("<chart showNames='设备信息运维统计' decimalPrecision='0' baseFont='Arial' baseFontSize='12'>");
                    // xmlData.Append("<chart caption='Weather Report' subcaption='Temperature' xaxisname='Places' yaxisname='Degree  Fahrenheit' clustered='0'  zeroplanemesh='0'  zeroplanecolor='68BDFFzeroplanealpha='50 palette='3' bgcolor='66D6FF,FFFFFF,ffffff'bgratio='20,60,20'bgalpha='10,10,40' divlineeffect='none'numdivlines='3'legendbgalpha='90' legendshadow='0'intensity='02'startangx='4.5'startangy='-6.6'endangx='4.5'endangy='-6.6'exetime='2'numbersuffix='°'>");

                    panel.Html = FusionCharts.RenderChartHTML("../FusionCharts/Pie3D.swf", "", xmlData.ToString(), "Sales", "500", "400", false, false);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceStatistics.aspx-xuanze", ex.Message+"；"+ex.StackTrace, "xuanze has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="s3"></param>
        /// <param name="s4"></param>
        [DirectMethod(Namespace = "OnEvl")]
        public void tubiao(string s1, string s2, string s3, string s4)
        {
            try
            {
                this.comyangshi.Value = "01";
                StringBuilder xmlData = new StringBuilder();

                xmlData.Append("<chart showNames='" + GetLangStr("DeviceStatistics9", "设备信息运维统计") + "' decimalPrecision='0' baseFont='Arial' baseFontSize='12'>");

                xmlData.AppendFormat("<set label='" + GetLangStr("DeviceStatistics12", "领用") + "' value='{0}' />", s1);
                xmlData.AppendFormat("<set label='" + GetLangStr("DeviceStatistics13", "维修") + "' value='{0}' />", s2);
                xmlData.AppendFormat("<set label='" + GetLangStr("DeviceStatistics14", "入库") + "' value='{0}' />", s3);
                xmlData.AppendFormat("<set label='" + GetLangStr("DeviceStatistics15", "归还") + "' value='{0}' />", s4);

                xmlData.Append("</chart>");

                panel.Html = FusionCharts.RenderChartHTML("../FusionCharts/Pie2D.swf", "", xmlData.ToString(), "chartid", "500", "400", false, false);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceStatistics.aspx-tubiao", ex.Message+"；"+ex.StackTrace, "tubiao has an exception");
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        [DirectMethod]
        public void UpdateData()
        {
        }

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
                logManager.InsertLogError("DeviceStatistics.aspx-GetDateTime", ex.Message+"；"+ex.StackTrace, "GetDateTime has an exception");
            }
        }

        #endregion DirectMethod 方法

        #region 私有方法
        /// <summary>
        /// 设置初始时间
        /// </summary>
        private void DataSetDateTime()
        {
            try
            {
            starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            start.InnerText = starttime;

            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            end.InnerText = endtime;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceStatistics.aspx-DataSetDateTime", ex.Message+"；"+ex.StackTrace, "DataSetDateTime has an exception");
            }
        }

        /// <summary>
        ///初始化绑定数据
        /// </summary>
        /// <param name="where"></param>
        private void GridDataBind(string where)
        {
            try
            {
                PagingToolbar1.PageSize = 10;
                DataTable dt = deviceManager.GetTongJi(where);
                StoreDeviceManager.DataSource = dt;
                StoreDeviceManager.DataBind();

                Session["datatable"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    SelectFirst(dt.Rows[0]);
                    ToolExport.Disabled = false;
                    Button2.Disabled = false;
                }
                else
                {
                    Button2.Disabled = true;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceStatistics.aspx-GridDataBind", ex.Message+"；"+ex.StackTrace, "GridDataBind has an exception");
            }
        }

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            try
            {
                string where = " ";
                string time1 = starttime;
                string time2 = endtime;
                if (time1 == "")
                {
                    where += " ";
                }
                else
                {
                    where += " and  openration_time>STR_TO_DATE('" + time1 + "','%Y-%m-%d %H:%i:%s')";
                }
                if (endtime != "")
                {
                    where += " and  openration_time<=STR_TO_DATE('" + time2 + "','%Y-%m-%d %H:%i:%s')";
                }
                else
                {
                    where += " ";
                }
                where += " and device_id like '%shebei%'";
                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceStatistics.aspx-GetWhere", ex.Message+"；"+ex.StackTrace, "GetWhere has an exception");
                return null;
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
                DataTable dt1 = dt.Copy(); ;
                if (dt1 != null)
                {
                    dt1.Columns[0].ColumnName = GetLangStr("DeviceStatistics12", "领用");
                    dt1.Columns[1].ColumnName = GetLangStr("DeviceStatistics13", "维修");
                    dt1.Columns[2].ColumnName = GetLangStr("DeviceStatistics14", "入库");
                    dt1.Columns[3].ColumnName = GetLangStr("DeviceStatistics15", "归还");
                    //PrintColumns pc = new PrintColumns();
                    //pc.Add(new PrintColumn("领用", 1));
                    //pc.Add(new PrintColumn("维修", 2));
                    //pc.Add(new PrintColumn("入库", 3));
                    //pc.Add(new PrintColumn("归还", 4));
                    //dt2 = Bll.Common.GetDataTablePrint(dt, pc);
                }

                return dt1;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceStatistics.aspx-ChangeDataTable", ex.Message+"；"+ex.StackTrace, "ChangeDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        /// 绑定第一行数据
        /// </summary>
        /// <param name="dr"></param>
        private void SelectFirst(DataRow dr)
        {
        }

        private void AddLedManager()
        {
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
                logManager.InsertLogError("DeviceStatistics.aspx-GetdatabyField", ex.Message+"；"+ex.StackTrace, "GetdatabyField has an exception");
                return null;
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
                root.Text = GetLangStr("DeviceStatistics34", "设备管理");

                nodes.Add(root);

                DataTable dt = tgsPproperty.GetStationTypeInfo("1=1 and isuse='1'");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string code = dt.Rows[i][0].ToString();
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    node.Text = dt.Rows[i][1].ToString();
                    node.Listeners.Click.Handler = "selectNode('" + code + "') ;";
                    node.Icon = Icon.Camera;
                    node.NodeID = code;
                    node.Expanded = true;
                    root.Nodes.Add(node);
                }
                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceStatistics.aspx-BuildTree", ex.Message+"；"+ex.StackTrace, "BuildTree has an exception");
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
                logManager.InsertLogError("DeviceStatistics.aspx-ConventDataTable", ex.Message+"；"+ex.StackTrace, "ConventDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        /// 提示
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
                logManager.InsertLogError("DeviceStatistics.aspx-Addree", ex.Message+"；"+ex.StackTrace, "Addree has an exception");
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