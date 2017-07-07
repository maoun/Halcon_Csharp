using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class FlowInfoQuery : System.Web.UI.Page
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
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!IsPostBack)
            {
                if (!X.IsAjaxRequest)
                {
                    DataSetDateTime();
                    StoreDataBind();
                    //BuildTree(TreeStation.Root);
                    TbutQueryClick(null, null);
                    this.DataBind();
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：报警查询", userinfo.NowIp, "0");
                }
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
                GetData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowInfoQuery.aspx-TbutQueryClick", ex.Message, "TbutQueryClick has an exception");
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
                starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string timeJs = "clearTime('" + starttime + "','" + endtime + "');";//js方法后面的分号一定要加上
                this.ResourceManager1.RegisterAfterClientInitScript(timeJs);
                if (!string.IsNullOrEmpty(kakouFlowQuery.Value))//判断卡口是否为空
                {
                    string js = "clearMenu();";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
                //this.CmbKakouDirection.SelectedItem.Text = "";
                //this.CmbKakouDirection.Text = "";
                this.CmbKakouDirection.Reset();
                this.ResultCombo.Reset();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowInfoQuery.aspx-ButResetClick", ex.Message, "ButResetClick has an exception");
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
                string field = e.ExtraParams["field"];
                AddWindow(data);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowInfoQuery.aspx-ShowDetails", ex.Message, "ShowDetails has an exception");
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
                logManager.InsertLogError("FlowInfoQuery.aspx-TbutLast", ex.Message, "TbutLast has an exception");
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
                logManager.InsertLogError("FlowInfoQuery.aspx-TbutNext", ex.Message, "TbutNext has an exception");
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
                logManager.InsertLogError("FlowInfoQuery.aspx-TbutFisrt", ex.Message, "TbutFisrt has an exception");
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
                logManager.InsertLogError("FlowInfoQuery.aspx-TbutEnd", ex.Message, "TbutEnd has an exception");
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
                logManager.InsertLogError("FlowInfoQuery.aspx-MyData_Refresh", ex.Message, "MyData_Refresh has an exception");
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
                    string xml = Bll.Common.GetPrintXml("报警车辆查询信息列表", "", "", "printdatatable");
                    string js = "OpenPrintPageH(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowInfoQuery.aspx-ButPrintClick", ex.Message, "ButPrintClick has an exception");
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
                logManager.InsertLogError("FlowInfoQuery.aspx-ToXml", ex.Message, "ToXml has an exception");
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
                logManager.InsertLogError("FlowInfoQuery.aspx-ToExcel", ex.Message, "ToExcel has an exception");
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
                logManager.InsertLogError("FlowInfoQuery.aspx-ToCsv", ex.Message, "ToCsv has an exception");
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
                logManager.InsertLogError("FlowInfoQuery.aspx-GetDateTime", ex.Message, "GetDateTime has an exception");
            }
        }

        [DirectMethod(Namespace = "Ovel")]
        public void Savecljg(string BH)
        {
            try
            {
                string cljg = X.GetCmp<ComboBox>("cmbcljg").Text;
                string cljgms = X.GetCmp<ComboBox>("cmbcljg").SelectedItem.Text;
                if (string.IsNullOrEmpty(cljg))
                {
                    Notice("提示", "请选择处理结果");
                }
                else
                {
                    Hashtable hs = new Hashtable();
                    UserInfo userinfo = Session["userinfo"] as UserInfo;
                    hs.Add("BH", BH);
                    //hs.Add("clry", userinfo.UserCode);
                    //hs.Add("clyj", "");
                    hs.Add("cljg", cljg);

                    if (tgsDataInfo.DealFlowInfo(hs) > 0)
                    {
                        Notice("信息提示", "处理完成");
                        ShowQuery(Convert.ToInt32(curpage.Value));
                        X.GetCmp<TextField>("txtcljg").Text = cljgms;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 初始化绑定数据-根据卡口编号查询卡口方向
        /// </summary>
        [DirectMethod]
        public void StoreKakouDirectionDataBind(string sKakouId)
        {
            try
            {
                this.CmbKakouDirection.SelectedItem.Text = "";
                this.CmbKakouDirection.Text = "";
                if (sKakouId != "")
                {
                    DataTable dt3 = tgsDataInfo.GetKakouDirection(sKakouId);
                    if (dt3 != null)
                    {
                        this.StoreKakouDirection.DataSource = Bll.Common.ChangColName(dt3);
                        this.StoreKakouDirection.DataBind();
                    }
                }
                else
                {
                    this.StoreKakouDirection.RemoveAll();
                    this.StoreKakouDirection.DataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmInfoQuery.aspx-StoreDataBind", ex.Message, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 清除卡口信息
        /// </summary>
        [DirectMethod]
        public void ClearKakou()
        {
            if (Session["Condition"] != null)
            {
                Condition con = Session["Condition"] as Condition;
                con.Kkid = "";
                con.Kkidms = "";
            }
            if (Session["tree"] != null)
            {
                Session["tree"] = null;
            }
        }

        /// <summary>
        /// 得到符合条件的卡口
        /// </summary>
        [DirectMethod]
        public void GetKakou(string blog)
        {
            try
            {
                string value = "";
                if (blog.Equals("0"))
                {
                    value = kakouFlowQuery.Value;
                }

                DataTable dtSelect = null;
                DataTable dt = Session["selectKakou"] as DataTable;//得到整个卡口信息
                DataRow[] rows = dt.Select("STATION_NAME like '" + value + "%'");//选出符合条件的
                if (rows.Length > 0)
                {
                    dtSelect = ToDataTable(rows);
                }
                StringBuilder strs = new StringBuilder();
                strs.AppendLine("[");
                if (dtSelect != null && dtSelect.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSelect.Rows.Count; i++)
                    {
                        if (i == dtSelect.Rows.Count - 1)
                        {
                            strs.AppendLine("{name:'" + dtSelect.Rows[i]["STATION_NAME"] + "',id:'" + dtSelect.Rows[i]["STATION_ID"] + "'}");
                        }
                        else
                        {
                            strs.AppendLine("{name:'" + dtSelect.Rows[i]["STATION_NAME"] + "',id:'" + dtSelect.Rows[i]["STATION_ID"] + "'},");
                        }
                    }
                }
                else
                {
                    strs.AppendLine("{name:'none',id:'none'},");
                }
                strs.AppendLine("]");
                string js = "";
                if (blog.Equals("0"))
                {
                    js = "setUl(" + strs.ToString() + ");";
                }
                else
                {
                    js = "setUlQuery(" + strs.ToString() + ");";
                }


                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 卡口模糊查询选中的时候给Session["tree"]赋值
        /// </summary>
        [DirectMethod]
        public void SetSession(string blog)
        {
            if (Session["tree"] != null)
            {
                Session["tree"] = null;
            }
            if (blog.Equals("0"))
            {
                Session["tree"] = kakouId.Value;
            }
        }
        #endregion DirectMethod

        #region 私有方法
        public static DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                //车俩类型
                //DataTable dt = GetRedisData.GetData("t_sys_code:140001");
                //if (dt != null)
                //{
                //    this.StorePlateType.DataSource = Bll.Common.ChangColName(dt);
                //    this.StorePlateType.DataBind();
                //}
                //else
                //{
                //    dt = tgsPproperty.GetPalteType();
                //    this.StorePlateType.DataSource = dt;
                //    this.StorePlateType.DataBind();
                //}

                //DataTable dt2 = GetRedisData.GetData("t_sys_code:300100");
                ////报警类型
                //if (dt2 != null)
                //{
                //    this.StoreAlarmType.DataSource = Bll.Common.ChangColName(dt2);
                //    this.StoreAlarmType.DataBind();
                //}
                //else
                //{
                //}

                //报警消息处理结果
                DataTable dt3 = GetRedisData.GetData("t_sys_code:430800");
                if (dt3 != null)
                {
                    this.Storecljg.DataSource = Bll.Common.ChangColName(dt3);
                    this.Storecljg.DataBind();
                }
                else
                {
                }

                //报警消息处理状态
                DataTable dt4 = GetRedisData.GetData("t_sys_code:430800");
                if (dt4 != null)
                {
                    this.ResultStore.DataSource = Bll.Common.ChangColName(dt4);
                    this.ResultStore.DataBind();
                }
                //else
                //{
                //}
                //DataTable dt5 = GetRedisData.GetData("t_sys_code:430700");
                //if (dt5 != null)
                //{
                //    this.DispatchedStore.DataSource = Bll.Common.ChangColName(dt5);
                //    this.DispatchedStore.DataBind();
                //}
                //else
                //{
                //}

                //ButCsv.Disabled = true;
                ButExcel.Disabled = true;
                //ButXml.Disabled = true;
                //ButPrint.Disabled = true;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowInfoQuery.aspx-StoreDataBind", ex.Message, "StoreDataBind has an exception");
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
                Window win = WindowShow.AddFlowCar(sdata);
                win.Render(this.Form);
                win.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowInfoQuery.aspx-AddWindow", ex.Message, "AddWindow has an exception");
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
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowInfoQuery.aspx-SetButState", ex.Message, "SetButState has an exception");
            }
        }

        /// <summary>
        ///获得一页显示条数
        /// </summary>
        /// <returns></returns>
        private int GetRowNum()
        {
            try
            {
                string rownum = "";
                if (CmbQueryNum.SelectedIndex != -1)
                {
                    rownum = CmbQueryNum.SelectedItem.Value.ToString();
                }
                else
                {
                    rownum = "50";
                }
                return int.Parse(rownum);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowInfoQuery.aspx-GetRowNum", ex.Message, "GetRowNum has an exception");
                return 50;
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
                DataTable dt = tgsDataInfo.GetFlowInfo(where, startNum, endNum);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ButExcel.Disabled = false;
                    if (dt.Rows.Count.Equals(15))// 有数据 且够提取的条数
                    {
                        this.lblCurpage.Text = curpage.Value.ToString();
                        this.lblAllpage.Text = allPage.Value.ToString();
                        this.lblRealcount.Text = realCount.Value.ToString();
                    }
                    else
                    {
                        this.lblCurpage.Text = curpage.Value.ToString();
                        this.lblAllpage.Text = allPage.Value.ToString();
                        this.lblRealcount.Text = realCount.Value.ToString();
                    }
                    this.StoreFlowInfo.DataSource = dt;
                    this.StoreFlowInfo.DataBind();
                }
                else
                {
                    ButExcel.Disabled = true;
                    this.lblCurpage.Text = curpage.Value.ToString();
                    this.lblAllpage.Text = allPage.Value.ToString();
                    this.lblRealcount.Text = realCount.Value.ToString();
                    this.StoreFlowInfo.DataSource = dt;
                    this.StoreFlowInfo.DataBind();
                    Notice("信息提示", "当前没数据");
                    return;
                }
                Session["datatable"] = dt;
                this.StoreFlowInfo.DataSource = dt;
                this.StoreFlowInfo.DataBind();
                int realnum = startNum + dt.Rows.Count - 1;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowInfoQuery.aspx-Query", ex.Message, "Query has an exception");
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
                int rownum = 15;
                int startNum = (currentPage - 1) * rownum;
                int endNum = currentPage * rownum;
                Query(GetWhere(), startNum, endNum);
                SetButState(currentPage);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowInfoQuery.aspx-ShowQuery", ex.Message, "ShowQuery has an exception");
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
                DataTable tempdt = tgsDataInfo.GetFlowInfoCount(GetWhere(), 0, 0);
                if (tempdt != null && tempdt.Rows.Count > 0)
                {
                    realCount.Value = tempdt.Rows[0]["col0"].ToString();
                    curpage.Value = 1;
                    int rownum = 15;
                    allPage.Value = (int)Math.Ceiling(double.Parse(realCount.Value.ToString()) / rownum);
                    ShowQuery(1);
                    //    ButExcel.Disabled = false;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowInfoQuery.aspx-GetData", ex.Message, "GetData has an exception");
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
                logManager.InsertLogError("FlowInfoQuery.aspx-Notice", ex.Message, "Notice has an exception");
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

                //if (CmbPlateType.SelectedIndex != -1)
                //{
                //    where = where + " and  hpzl='" + CmbPlateType.SelectedItem.Value + "' ";
                //}
                if (ResultCombo.SelectedIndex != -1)//处理结果条件
                {
                    where = where + " and  cljg='" + ResultCombo.SelectedItem.Value + "' ";
                }
                string QueryKKBHMS = string.Empty;
                string kkname = this.kakouFlowQuery.Value;
                if (!string.IsNullOrEmpty(kkname))
                {
                    QueryKKBHMS = this.kakouId.Value.ToString();
                    if (!string.IsNullOrEmpty(QueryKKBHMS))
                    {
                        where = where + " and  t2.KKBH ='" + QueryKKBHMS.ToUpper() + "' ";
                    }
                }

                if (this.CmbKakouDirection.Value != null)
                {
                    string kkDirectionId = this.CmbKakouDirection.Value.ToString();
                    if (!string.IsNullOrEmpty(kkDirectionId))
                    {
                        where = where + " and  t2.KKFX ='" + kkDirectionId + "' ";
                    }
                }

                return where;
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("FlowInfoQuery.aspx-GetWhere", ex.Message, "GetWhere has an exception");
                ILog.WriteErrorLog(ex);
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
                //this.FormPanel1.Collapsed = false;
                string sdata = e.ExtraParams["data"];
                //string hphm = Bll.Common.GetdatabyField(sdata, "col3");
                //string hpzl = Bll.Common.GetdatabyField(sdata, "col1");

                //string url1 = Bll.Common.GetdatabyField(sdata, "col21");
                //string url2 = Bll.Common.GetdatabyField(sdata, "col22");
                //string url3 = Bll.Common.GetdatabyField(sdata, "col23");
                //string js = "ShowImage(\"" + dataCommon.ChangePoliceIp(url1) + "\",\"" + dataCommon.ChangePoliceIp(url2) + "\",\"" + dataCommon.ChangePoliceIp(url3) + "\",\"" + hphm + "\",\"" + hpzl + "\");";

                //string js = "ShowImage(\"" + url1 + "\",\"" + url2 + "\",\"" + url3 + "\",\"" + hphm + "\",\"" + hpzl + "\");";
                //this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("FlowInfoQuery.aspx-ApplyClick", ex.Message, "ApplyClick has an exception");
                ILog.WriteErrorLog(ex);
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
                    dt1.Columns.Remove("col0"); dt1.Columns.Remove("col2"); dt1.Columns.Remove("col3"); dt1.Columns.Remove("col10"); dt1.Columns.Remove("col13");
                    for (int i = 19; i < dt.Columns.Count - 1; i++)
                    {
                        dt1.Columns.Remove("col" + i.ToString());
                    }
                    //设置内存表中顺序
                    dt1.Columns["col1"].SetOrdinal(0);
                    dt1.Columns["col14"].SetOrdinal(1);
                    dt1.Columns["col4"].SetOrdinal(2);
                    dt1.Columns["col5"].SetOrdinal(3);
                    dt1.Columns["col6"].SetOrdinal(4);
                    dt1.Columns["col7"].SetOrdinal(5);
                    dt1.Columns["col8"].SetOrdinal(6);
                    dt1.Columns["col9"].SetOrdinal(7);
                    dt1.Columns["col12"].SetOrdinal(8);
                    dt1.Columns["col11"].SetOrdinal(9);
                    dt1.Columns[0].ColumnName = GetLangStr("FlowInfoQuery50", "报警卡口");
                    dt1.Columns[1].ColumnName = GetLangStr("FlowInfoQuery59", "卡口方向");
                    dt1.Columns[2].ColumnName = GetLangStr("FlowInfoQuery51", "报警时间");
                    dt1.Columns[3].ColumnName = GetLangStr("FlowInfoQuery52", "统计周期");
                    dt1.Columns[4].ColumnName = GetLangStr("FlowInfoQuery53", "报警阈值");
                    dt1.Columns[5].ColumnName = GetLangStr("FlowInfoQuery54", "比例");
                    dt1.Columns[6].ColumnName = GetLangStr("FlowInfoQuery55", "流量");
                    dt1.Columns[7].ColumnName = GetLangStr("FlowInfoQuery56", "卡口配置人");
                    dt1.Columns[8].ColumnName = GetLangStr("FlowInfoQuery57", "处理结果");
                    dt1.Columns[9].ColumnName = GetLangStr("FlowInfoQuery58", "配置时间");
                    //dt1.Columns[9].ColumnName = GetLangStr("FlowInfoQuery59", "报警原因");                   
                }

                return dt1;
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("FlowInfoQuery.aspx-ChangeDataTable", ex.Message, "ChangeDataTable has an exception");
                ILog.WriteErrorLog(ex);
                return dt;
            }
        }

        /// <summary>
        /// 设置初始时间
        /// </summary>
        private void DataSetDateTime()
        {
            starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            start.InnerText = starttime;
            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            end.InnerText = endtime;
            // panelTime.Render();
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
                logManager.InsertLogError("FlowInfoQuery.aspx-BuildTree", ex.Message, "BuildTree has an exception");
                ILog.WriteErrorLog(ex);
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
                logManager.InsertLogError("FlowInfoQuery.aspx-AddDepartTree", ex.Message, "AddDepartTree has an exception");
                ILog.WriteErrorLog(ex);
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
                logManager.InsertLogError("FlowInfoQuery.aspx-AddStationTree", ex.Message, "AddStationTree has an exception");
                ILog.WriteErrorLog(ex);
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