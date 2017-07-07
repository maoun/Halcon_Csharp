using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class SpecialMonitor: System.Web.UI.Page
    {
        #region 成员变量

        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();
        private static DataTable mydt;
        private static QueryService.querypasscar client = new QueryService.querypasscar();
        private SettingManager settingManager = new SettingManager();
        private Bll.ServiceManager servicemansger = new Bll.ServiceManager();
        private static int sumCount = 0;//总条数
        private static int indexCount = 0;//当前条数
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

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
                // ButXml.Disabled = true;
                //ButPrint.Disabled = true;
                //ButCsv.Disabled = true;
                //ButExcel.Disabled = true;
                StoreDataBind();
                BuildTree(TreePerson.Root);
                this.DataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "Access: vehicle monitor", userinfo.NowIp, "0");
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
                SpecialDataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-TbutQueryClick", ex.Message, "TbutQueryClick an error occurs");
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
                //string where = "1=1 ";
                ////判断专项内容是否为空，不为空加入到条件中
                ////if (!string.IsNullOrEmpty(TxtplateId.Text.ToUpper()))
                ////{
                ////    where = where + " and ZXNR='" + TxtplateId.Text.ToUpper() + "'";
                ////}
                ////判断号牌种类是否为空，不为空加入到条件中
                //if (!string.IsNullOrEmpty(CmbPlateType.Text))
                //{
                //    where = where + " and ZXLX='" + CmbPlateType.Text + "'";
                //}
                ////判断布控类型是否为空，不为空加入到条件中
                //if (!string.IsNullOrEmpty(CmbQueryMdlx.Text))
                //{
                //    where = where + " and  BKZT='" + CmbQueryMdlx.Text + "'";
                //}
                SpecialDataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-ButRefreshClick", ex.Message, "ButRefreshClick an error occurs");
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
                CmbPlateType.Reset();
                CmbQueryMdlx.Reset();
                //TxtplateId.Reset();
                string js = "clearHpjc();";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-ButResetClick", ex.Message, "ButResetClick an error occurs");
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
                    string xml = Bll.Common.GetPrintXml("布控车辆查询信息列表", "", "", "printdatatable");
                    string js = "OpenPrintPageV(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-ButPrintClick", ex.Message, "ButPrintClick an error occurs");
            }
        }

        /// <summary>
        /// 导出xml 事件
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
                logManager.InsertLogError("SpecialMonitor.aspx-ToXml", ex.Message, "ToXml an error occurs");
            }
        }

        /// <summary>
        /// 导出excel事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToExcel(object sender, DirectEventArgs e)
        {
            try
            {
                DataTable dt = ChangeDataTable();
                ConvertData.ExportExcel(dt, this);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-ToExcel", ex.Message, "ToExcel an error occurs");
            }
        }

        /// <summary>
        /// 导出excel后页面不加载状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ToXianshi(object sender, DirectEventArgs e)
        {
            ButSave.DirectEvents.ClearDirectEvents();
        }

        /// <summary>
        /// 导出csv事件
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
                logManager.InsertLogError("SpecialMonitor.aspx-ToCsv", ex.Message, "ToCsv an error occurs");
            }
        }

        /// <summary>
        /// 开始上传并导入excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void StartLongAction(object sender, DirectEventArgs e)
        {
            try
            {
                this.Session["specialCount"] = 0;
                string filepath = UploadExcelFile();
                if (string.IsNullOrEmpty(filepath))
                {
                    Notice("提示信息", "请选择要上传的Excel");
                    return;
                }
                if (!string.IsNullOrEmpty(filepath))
                {
                    DataSet ds = new DataSet();
                    ds = ConvertData.ExcelToDataSet(@"" + filepath + "", "sheet1");
                    mydt = ds.Tables[0];
                    if (mydt != null && mydt.Rows.Count > 0)
                    {
                        ThreadPool.QueueUserWorkItem(BatchUpdateData);
                        this.ResourceManager1.AddScript("{0}.startTask('SaveExcelData');", this.TaskManager1.ClientID);
                    }
                }
                File.Delete(filepath);
                indexCount = 0;
                //Notice("提示信息", "上传成功");
                //SuspicionDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-StartLongAction", ex.Message, "StartLongAction an error occurs");
            }
        }

        /// <summary>
        /// 工具条切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Unnamed_Event(object sender, DirectEventArgs e)
        {
            if (TabStrip1.ActiveTabIndex == 0)
            {
                Progress1.Hidden = true;
            }
            else
            {
                Progress1.Hidden = false;
            }
        }

        /// <summary>
        /// 进度条事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RefreshProgress(object sender, DirectEventArgs e)
        {
            try
            {
                int progress = indexCount;
                if (progress >= 0 && progress <= sumCount)
                {
                    if (progress == 0)
                    {
                        return;
                    }
                    else if (progress == sumCount)
                    {
                        this.ResourceManager1.AddScript("{0}.stopTask('SaveExcelData');", this.TaskManager1.ClientID);
                        this.Progress1.UpdateProgress(1, "完成");
                        SpecialDataBind();
                        indexCount = 1;
                        Notice("提示信息", "录入成功" + sumCount + "条");
                    }
                    else
                    {
                        this.Progress1.UpdateProgress(progress / 54f, string.Format("当前为 {0} 总共 {1}...", progress.ToString(), sumCount));
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-RefreshProgress", ex.Message, "RefreshProgress an error occurs");
            }
        }

        #endregion 控件事件

        #region DirectMethod事件

        /// <summary>
        /// 保存布控信息事件
        /// </summary>
        [DirectMethod]
        public void InfoSave()
        {
            try
            {
                AddSpecial();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-InfoSave", ex.Message, "InfoSave an error occurs");
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
                //string Id = VehicleHead.VehicleText + txtHphm.Text.ToUpper();
                X.Msg.Confirm("信息", "确认要删除这条记录吗?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "SpecialMonitor.DoYes()",
                        Text = "是"
                    },
                    No = new MessageBoxButtonConfig
                    {
                        Handler = "SpecialMonitor.DoNo()",
                        Text = "否"
                    }
                }).Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-DoConfirm", ex.Message, "DoConfirm an error occurs");
            }
        }

        /// <summary>
        /// 确定事件
        /// </summary>
        [DirectMethod]
        public void DoYes()
        {
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("ZXBH", ZXBH.Text);
                if (tgsDataInfo.DeleteSpecialInfo(hs) > 0)
                {
                    try
                    {
                        bool flag = client.cancleLayout(ZXBH.Text);
                        ILog.WriteErrorLog("From control return:" + flag.ToString());
                    }
                    catch (Exception ex)
                    {
                        logManager.InsertLogError("SpecialMonitor.aspx-DoYes", ex.Message, "DoYes charged with an exception occurs");
                    }
                    Notice("信息提示", "删除成功");
                    SpecialDataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-DoYes", ex.Message, "DoYes an error occurs");
            }
        }

        /// <summary>
        /// 不删除事件
        /// </summary>
        [DirectMethod]
        public void DoNo()
        {
        }

        /// <summary>
        /// 更新布控记录
        // </summary>
        [DirectMethod]
        public void UpdateData()
        {
            try
            {
                //专项类型
                if (ZXLX.Value == null)
                {
                    Notice("布控信息提示", "保存失败，请选择专项类型");
                    return;
                }

                if (ZXMS.Text.Trim() == string.Empty)
                {
                    Notice("布控信息提示", "保存失败，请输入专项描述");
                    return;
                }

                if ( FieldPerson.Value == null)
                {
                    Notice("布控信息提示", "保存失败，请选择报警接收人");
                    return;
                }

                if (YSSJ.SelectedDate.ToString("yyyy-MM-dd") == @"0001-01-01" )
                {
                    Notice("布控信息提示", "保存失败，请选择有效时间");
                    return;
                }

                if ( BKZT.Value == null)
                {
                    Notice("布控信息提示", "保存失败，请选择布控状态");
                    return;
                }

                if (BKLXR.Text.Trim() == string.Empty)
                {
                    Notice("布控信息提示", "保存失败，请输入布控联系人");
                    return;
                }

                if (BKLXFS.Text.Trim() == string.Empty)
                {
                    Notice("布控信息提示", "保存失败，请输入联系电话");
                    return;
                }
                //布控范围
                string kkid = string.Empty;
                string kkname = this.kakou.Value;
                string zxlx = ZXLX.Value.ToString();
                if (zxlx != "300109" && !string.IsNullOrEmpty(kkname))
                {
                    Notice("卡口流量设置提示", "专项类型为初次入城时才能选择布控范围");
                    return;
                }
                kkid = this.kakouId.Value.ToString();
                Hashtable hs = new Hashtable();
                if (ZXBH.Text == string.Empty)
                    hs.Add("ZXBH", tgsDataInfo.GetTgsRecordId());
                else
                    hs.Add("ZXBH", ZXBH.Text);
                //专项内容
                hs.Add("ZXLX", ZXLX.Value);//专项类型              
                hs.Add("ZXMS", ZXMS.Text);//专项描述 
                hs.Add("BKFW", kkid);//布控范围
                hs.Add("BKFWMS", kkname);//布控范围描述
                UserInfo user = Session["userinfo"] as UserInfo;
                hs.Add("BKRID", user.UserCode);//布控人编号
                hs.Add("BKRXM", user.Name);//布控人员姓名
                hs.Add("BKJSR", FieldPerson.Value);//布控接收人编号
                hs.Add("BKJSRMS", FieldPerson.Text);//布控接收人姓名
                hs.Add("BKLXR", BKLXR.Text);//布控联系人
                hs.Add("BKLXFS", BKLXFS.Text);//布控联系电话
                
                //有效时间
                if (YSSJ.SelectedDate != null)
                {
                    hs.Add("YSSJ", YSSJ.SelectedDate.ToString("yyyy-MM-dd")); //+ " " + TimeYxsj.Text);
                }

                //布控状态
                if (BKZT.Value != null)
                {
                    hs.Add("BKZT", BKZT.Value);
                }

                if (tgsDataInfo.UpdateSpecialInfo(hs) > 0)
                {
                    if (BKZT.Value.Equals("1"))// 布控
                    {
                        try
                        {
                            bool flag = client.addWarn(ZXLX.Value.ToString(), "", ZXBH.Text);
                            ILog.WriteErrorLog("Monitor return:" + flag.ToString());
                        }
                        catch (Exception ex)
                        {
                            logManager.InsertLogError("SpecialMonitor.aspx-UpdateData", ex.Message, "UpdateData monitor anomalies");
                        }
                    }
                    else
                    {
                        try
                        {
                            bool flag = client.cancleLayout(ZXBH.Text);
                            ILog.WriteErrorLog("From control return:" + flag.ToString());
                        }
                        catch (Exception ex)
                        {
                            logManager.InsertLogError("SpecialMonitor.aspx-UpdateData", ex.Message, "UpdateData charged with an exception occurs");
                        }
                    }
                    Notice("布控信息提示", "保存成功");
                    //string where = "1=1";
                    ////判断专项内容是否为空，不为空加入到条件中
                    ////if (!string.IsNullOrEmpty(TxtplateId.Text.ToUpper()))
                    ////{
                    ////    where = where + " and ZXNR='" +TxtplateId.Text.ToUpper() + "'";
                    ////}
                    ////判断专项类型是否为空，不为空加入到条件中
                    //if (!string.IsNullOrEmpty(CmbPlateType.Text))
                    //{
                    //    where = where + " and ZXLX='" + CmbPlateType.Text + "'";
                    //}
                    ////判断布控状态是否为空，不为空加入到条件中
                    //if (!string.IsNullOrEmpty(CmbQueryMdlx.Text))
                    //{
                    //    where = where + " and  BKZT='" + CmbQueryMdlx.Text + "'";
                    //}
                    // SuspicionDataBind(where);
                    SpecialDataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-UpdateData", ex.Message, "UpdateData an error occurs");
            }
        }


        /// <summary>
        /// 模版下载事件
        /// </summary>
        [DirectMethod]
        public void Download()
        {
            try
            {
                Response.ContentType = "application/x-zip-compressed";
                string fileName = "布控车辆批量导入模版.xls";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                string filename = Server.MapPath("../Export/ImportTemplateDownload.xls");//下载模板的物理路劲
                Response.TransmitFile(filename);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-Download", ex.Message, "Download an error occurs");
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
                value = kakou.Value;
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
                js = "setUl(" + strs.ToString() + ");";
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
            Session["tree"] = kakouId.Value;
        }

        #endregion DirectMethod事件

        #region 私有方法

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                DataTable dt1 = GetRedisData.GetData("t_sys_code:300100");//专项类型
                if (dt1 != null)
                {
                    this.StoreSpecialType.DataSource = dt1;
                    this.StoreSpecialType.DataBind();
                }
                else
                {
                    this.StoreSpecialType.DataSource = tgsPproperty.GetPalteType();
                    this.StoreSpecialType.DataBind();
                }
                //DataTable dt2 = GetRedisData.GetData("t_sys_code:240013");
                //if (dt2 != null)
                //{
                //    //报警接收人
                //    this.StoreRecipient.DataSource = dt2;
                //    this.StoreRecipient.DataBind();
                //}
                //else
                //{
                //    this.StoreRecipient.DataSource = tgsPproperty.GetCarColorDict();
                //    this.StoreRecipient.DataBind();
                //}
                DataTable dt3 = GetRedisData.GetData("t_sys_code:240011");//布控标志
                if (dt3 != null)
                {
                    //布控状态
                    this.StoreBKZT.DataSource = dt3;
                    this.StoreBKZT.DataBind();
                }
                else
                {
                    this.StoreBKZT.DataSource = tgsPproperty.GetIsSuspicionDict();
                    this.StoreBKZT.DataBind();
                }

                SpecialDataBind();
                GXSJ.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-StoreDataBind", ex.Message, "StoreDataBind an error occurs");
            }
        }

        /// <summary>
        /// 绑定布控数据
        /// </summary>
        /// <param name="where"></param>
        private void SpecialDataBind()
        {
            try
            {
                GetData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-SpecialDataBind", ex.Message, "SpecialDataBind an error occurs");
            }
        }

        /// <summary>
        /// 转换DataTable
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
                    dt1.Columns.Remove("col0"); dt1.Columns.Remove("col1"); dt1.Columns.Remove("col4"); dt1.Columns.Remove("col7"); dt1.Columns.Remove("col9"); dt1.Columns.Remove("col14");
                    for (int i = 19; i < dt.Columns.Count - 1; i++)
                    {
                        dt1.Columns.Remove("col" + i.ToString());
                    }
                    //设置内存表中顺序
                    dt1.Columns["col2"].SetOrdinal(0);
                    dt1.Columns["col3"].SetOrdinal(1);
                    dt1.Columns["col15"].SetOrdinal(2);
                    dt1.Columns["col5"].SetOrdinal(3);
                    dt1.Columns["col6"].SetOrdinal(4);
                    dt1.Columns["col8"].SetOrdinal(5);
                    dt1.Columns["col11"].SetOrdinal(6);
                    dt1.Columns["col12"].SetOrdinal(7);
                    dt1.Columns["col10"].SetOrdinal(8);
                    dt1.Columns["col13"].SetOrdinal(9);
                    dt1.Columns[0].ColumnName = GetLangStr("SpecialQuery11", "专项类型");
                    dt1.Columns[1].ColumnName = GetLangStr("SpecialQuery12", "专项描述");
                    dt1.Columns[2].ColumnName = GetLangStr("SpecialQuery20", "卡口范围");
                    dt1.Columns[3].ColumnName = GetLangStr("SpecialQuery13", "布控标识");
                    dt1.Columns[4].ColumnName = GetLangStr("SpecialQuery14", "有效时间");
                    dt1.Columns[5].ColumnName = GetLangStr("SpecialQuery15", "报警接收人");
                    dt1.Columns[6].ColumnName = GetLangStr("SpecialQuery16", "布控联系人");
                    dt1.Columns[7].ColumnName = GetLangStr("SpecialQuery17", "联系电话");
                    dt1.Columns[8].ColumnName = GetLangStr("SpecialQuery18", "布控人员");
                    dt1.Columns[9].ColumnName = GetLangStr("SpecialQuery19", "更新时间");                 
                    return dt1;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-ChangeDataTable", ex.Message, "ChangeDataTable an error occurs");
            }
            return dt;
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            string where = "1=1 ";
            try
            {
                if (CmbPlateType.SelectedIndex != -1)
                {
                    where = where + " and  BKLX='" + CmbPlateType.SelectedItem.Value + "' ";
                }
                if (CmbQueryMdlx.SelectedIndex != -1)
                {
                    where = where + " and  ZT='" + CmbQueryMdlx.SelectedItem.Value + "' ";
                }
                //string QueryZXNR = string.Empty;
                //QueryZXNR = TxtplateId.Text;                
                //if (!string.IsNullOrEmpty(QueryZXNR))
                //{
                //    where = where + " and  ZXNR like '%" + QueryZXNR.ToUpper() + "%' ";
                //}
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-GetWhere", ex.Message, "GetWhere an error occurs");
            }
            return where;
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
                SpecialDataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-MyData_Refresh", ex.Message, "MyData_Refresh an error occurs");
            }
        }

        /// <summary>
        /// 绑定第一行数据
        /// </summary>
        /// <param name="dr"></param>
        private void SelectFirst(DataRow dr)
        {
            try
            {
                ZXBH.Text = dr["col0"].ToString();//专项编号
                ZXLX.Value = dr["col1"].ToString();//专项类型编号
                ZXLX.SelectedItem.Text = dr["col2"].ToString();//专项类型描述
                ZXMS.Text = dr["col3"].ToString();//专项描述
                this.kakou.Value = dr["col15"].ToString();
                this.kakouId.Value = dr["col14"].ToString();
                if (Session["tree"] != null)
                {
                    Session["tree"] = null;
                }
                Session["tree"] = kakouId.Value;
                string js = "setMainValue('" + kakou.Value + "','" + kakouId.Value + "');";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
                UserInfo user = Session["userinfo"] as UserInfo;
                this.FieldPerson.SetValue(user.UserCode, user.Name);//布控接收人编号
                BKLXR.Text = dr["col11"].ToString();//布控联系人
                BKLXFS.Text = dr["col12"].ToString();//布控联系电话
                YSSJ.Text = GetDate(dr["col6"].ToString(), 1);//有效时间
                BKZT.SelectedItem.Text = dr["col5"].ToString();//布控状态描述
                BKZT.SelectedItem.Value = dr["col4"].ToString();//布控状态编号
                GXSJ.Text = dr["col13"].ToString();//更新时间
                return;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-SelectFirst", ex.Message, "SelectFirst an error occurs");
            }
        }

        /// <summary>
        /// 添加布控记录
        /// </summary>
        private void AddSpecial()
        {
            try
            {
                ZXBH.Text = tgsDataInfo.GetTgsRecordId();
                //ZXNR.Text = "";
                ZXLX.SelectedItem.Text = "";
                ZXLX.Value = "";
                ZXMS.Text = "";
                BKLXR.Text = "";                
                BKLXFS.Text = "";
                YSSJ.Text = "";
                BKZT.SelectedItem.Value = "1";
                //卡口列表
                if (!string.IsNullOrEmpty(kakou.Value))//判断卡口是否为空
                {
                    string js = "clearMenu();";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                } 
                //GXRY.Text = "";              
                this.FieldPerson.Text = "";
                this.GXSJ.Text = "";
                //注释下面的话不会由当前用户的默认设置   
                UserInfo user = Session["userinfo"] as UserInfo;
                this.FieldPerson.SetValue(user.UserCode, user.Name);
                //uiDepartment.DepertName = user.DepartName;        
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-AddSpecial", ex.Message, "AddSpecial an error occurs");
            }
        }

        /// <summary>
        /// 选择布控记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectSpecial(object sender, DirectEventArgs e)
        {
            try
            {
                object data = e.ExtraParams["sdata"];
                string sdata = data.ToString();
                ZXBH.Text = GetdatabyField(sdata, "col0");//专项编号
                ZXLX.Value = GetdatabyField(sdata, "col1");//专项类型编号
                ZXLX.SelectedItem.Text = GetdatabyField(sdata, "col2");//专项类型描述
                ZXMS.Text = GetdatabyField(sdata, "col3");//专项描述
                this.kakou.Value = GetdatabyField(sdata, "col15");
                this.kakouId.Value = GetdatabyField(sdata, "col14");
                if (Session["tree"] != null)
                {
                    Session["tree"] = null;
                }
                Session["tree"] = kakouId.Value;
                string js = "setMainValue('" + kakou.Value + "','" + kakouId.Value + "');";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
                UserInfo user = Session["userinfo"] as UserInfo;
                this.FieldPerson.SetValue(user.UserCode, user.Name); //布控接收人            
                BKLXR.Text = GetdatabyField(sdata, "col11");//布控联系人
                BKLXFS.Text = GetdatabyField(sdata, "col12");//布控联系电话
                YSSJ.Text = GetDate(sdata, "col6", 1);//有效时间
                BKZT.SelectedItem.Text = GetdatabyField(sdata, "col5");  //布控状态描述
                BKZT.Value = GetdatabyField(sdata, "col4");                //布控状态编号
                GXSJ.Text = GetDate(GetdatabyField(sdata, "col13"), 0);//更新时间
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-SelectSpecial", ex.Message, "SelectSpecial an error occurs");
            }
        }

        /// <summary>
        /// 转换为指定的时间格式
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private string GetDate(string data, int flag)
        {
            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    DateTime dt = DateTime.Parse(data);
                    switch (flag)
                    {
                        case 0:
                            return dt.ToString("yyyy-MM-dd HH:mm:ss");

                        case 1:
                            return dt.ToString("yyyy-MM-dd");

                        case 2:
                            return dt.ToString("HH:mm");

                        default:
                            return dt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-GetDate", ex.Message, "GetDate an error occurs");
                return "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="field"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private string GetDate(string data, string field, int flag)
        {
            try
            {
                string s = GetdatabyField(data, field);
                return GetDate(s, flag);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-GetDate", ex.Message, "GetDate an error occurs");
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
                logManager.InsertLogError("SpecialMonitor.aspx-GetdatabyField", ex.Message, "GetdatabyField an error occurs");
                return "";
            }
        }

        /// <summary>
        /// 提示窗体
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
                logManager.InsertLogError("SpecialMonitor.aspx-Notice", ex.Message, "Notice an error occurs");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        private void BatchUpdateData(object state)
        {
            try
            {
                //int count = 0;
                sumCount = mydt.Rows.Count;
                for (int i = 0; i < mydt.Rows.Count; i++)
                {
                    //Thread.Sleep(100);
                    try
                    {
                        Hashtable hs = new Hashtable();
                        hs.Add("ZXBH", tgsDataInfo.GetTgsRecordId());
                        //hs.Add("ZXNR", mydt.Rows[i]["专项内容"].ToString());
                        hs.Add("ZZLX", Bll.Common.GetHpzl(mydt.Rows[i]["专项类型"].ToString()));
                        hs.Add("clpp", mydt.Rows[i]["车辆品牌"].ToString());
                        hs.Add("csys", Bll.Common.GetCsys(mydt.Rows[i]["车身颜色"].ToString()));
                        hs.Add("mdlx", mydt.Rows[i]["布控类型"].ToString());//Bll.Common.GetMdlx(mydt.Rows[i]["布控类型"].ToString())
                        UserInfo user = Session["userinfo"] as UserInfo;
                        hs.Add("sjly", user.DeptCode);
                        //try
                        //{
                        //    hs.Add("sjly", mydt.Rows[i]["数据来源"].ToString());
                        //}
                        //catch
                        //{
                        //    hs.Add("sjly", "");
                        //}
                        if (YSSJ.SelectedDate != null)
                        {
                            hs.Add("yxsj", mydt.Rows[i]["有效时间"].ToString());
                        }
                        hs.Add("bjjsr", user.UserCode);
                        hs.Add("bjjsrms", user.Name);
                        hs.Add("bz", mydt.Rows[i]["备注"].ToString());
                        //没用的列
                        hs.Add("sjyy", "");
                        hs.Add("bdbj", "1");
                        //if (tgsDataInfo.UpdateSuspicionInfo(hs) > 0)
                        //{
                        //    try
                        //    {
                        //        client.layout(txtID.Text, VehicleHead.VehicleText + txtHphm.Text.ToUpper(), ZXLX.Value.ToString(), "");
                        //    }
                        //    catch
                        //    {
                        //    }
                        //    //count++;
                        //}
                        // Session["suspicionCount"] = i + 1;
                        indexCount = i + 1;
                    }
                    catch (Exception ex)
                    {
                        ILog.WriteErrorLog(ex);
                        logManager.InsertLogError("SpecialMonitor.aspx-BatchUpdateData", ex.Message, "BatchUpdateData an error occurs");
                    }
                }
                // Session["suspicionResult"] = count;
                //this.Session.Remove("suspicionCount");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-BatchUpdateData", ex.Message, "BatchUpdateData an error occurs");
            }
        }

        /// <summary>
        /// 上传并导入excel
        /// </summary>
        /// <returns></returns>
        private string UploadExcelFile()
        {
            try
            {
                string UploadFile = "";
                string strPath = "";
                if (this.ExcelFile.HasFile)
                {
                    UploadFile = this.ExcelFile.PostedFile.FileName.ToString();
                    int FileSize = Int32.Parse(this.ExcelFile.PostedFile.ContentLength.ToString());
                    if (FileSize > 5 * 1024 * 1024)
                    {
                        X.Msg.Alert("提示信息", "上传文件过大！").Show();
                        return "";
                    }
                    string fileType = Path.GetExtension(this.ExcelFile.PostedFile.FileName).ToUpper();//获取文件后缀
                    string allowFile = ".XLS.XLSX";
                    if (allowFile.Contains(fileType.ToUpper()))
                    {
                        string sNewName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(this.ExcelFile.PostedFile.FileName);
                        strPath = Server.MapPath("~/FileUpload/" + sNewName);
                        if (!Directory.Exists(Path.GetDirectoryName(strPath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(strPath));
                        }
                        this.ExcelFile.PostedFile.SaveAs(strPath);

                        string strConn = string.Format("Provider = Microsoft.Jet.OLEDB.4.0; Data Source ={0};Extended Properties=Excel 8.0", strPath);//通过ODBC连接数据的字符串，并将一个具体的Excel文件路径传入

                        //string strConn = "provider=Microsoft.ACE.OleDb.12.0; Data Source ='" + strPath + "';Extended Properties='Excel 12.0;HDR=yes;IMEX=1';";
                        OleDbConnection conn = new OleDbConnection(strConn);
                        conn.Open();
                        DataTable dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                        List<string> sheetNameList = new List<string>();
                        //一个EXCEL文件可能有多个工作表，遍历之
                        foreach (DataRow dr in dtSchema.Rows)
                        {
                            //获取所有 sheet 页的名字
                            sheetNameList.Add(dr["TABLE_NAME"].ToString());
                        }
                        conn.Close();

                        int sheetIndex = -1;
                        //查找需要导入的 sheet 页的名字
                        for (int i = 0; i < sheetNameList.Count; i++)
                        {
                            if (sheetNameList[i].ToUpper().Equals("SHEET1$"))
                            {
                                sheetIndex = i;
                                break;
                            }
                        }
                        //判断该 sheet 页是否存在
                        if (sheetIndex < 0)
                        {
                            X.Msg.Alert("提示信息", "没有找到需要导入的SHEET1$页！").Show();
                            return "";
                        }
                    }
                    else
                    {
                        X.Msg.Alert("提示信息", "文件格式不正确！").Show();
                        return "";
                    }
                }
                else//当没有文件时
                {
                    return "";
                }
                return strPath;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-UploadExcelFile", ex.Message, "UploadExcelFile an error occurs");
                return "";
            }
        }

        public static DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        /// 组件人员列表树
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
                root.Text = "人员列表";
                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;

                // 添加 自己机构节点 和人员
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
                DataTable dtPerson = GetRedisData.GetData("t_ser_person");
                DataTable dtDepart = GetRedisData.GetData("t_cfg_department");

                DataRow[] rowsPerson = dtPerson.Select(" departid='" + nodeRoot.NodeID + "' ");
                AddPersonTree(nodeRoot, ToDataTable(rowsPerson));
                nodeRoot.Expanded = false;
                nodeRoot.Draggable = true;
                nodeRoot.Expandable = ThreeStateBool.True;
                root.Nodes.Add(nodeRoot);

                //绑定下级部门及下级部门人员
                AddDepartTree(root, user.DeptCode, dtPerson, dtDepart);

                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-BuildTree", ex.Message, "BuildTree an error occurs");
                return null;
            }
        }

        /// <summary>
        ///绑定下级部门及下级部门人员
        /// </summary>
        /// <param name="root"></param>
        private void AddDepartTree(Ext.Net.TreeNode root, string departCode, DataTable dtPerson, DataTable dtDepart)
        {
            try
            {
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

                        DataRow[] rowsPerson = dtPerson.Select(" departid='" + nodeRoot.NodeID + "' ");
                        AddPersonTree(nodeRoot, ToDataTable(rowsPerson));
                        nodeRoot.Expanded = false;
                        nodeRoot.Draggable = true;
                        nodeRoot.Expandable = ThreeStateBool.True;
                        AddDepartTree(nodeRoot, rows[i]["departid"].ToString(), dtPerson, dtDepart);
                        root.Nodes.Add(nodeRoot);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-AddDepartTree", ex.Message, "AddDepartTree an error occurs");
            }
        }

        #endregion 私有方法

        #region 添加子节点

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="root"></param>
        private void AddPersonTree(Ext.Net.TreeNode DepartNode, DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                        node.Text = dt.Rows[i]["name"].ToString();
                        node.Leaf = true;
                        node.Checked = ThreeStateBool.False;
                        node.NodeID = dt.Rows[i]["usercode"].ToString();
                        node.Draggable = false;
                        node.AllowDrag = false;
                        DepartNode.Nodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SpecialMonitor.aspx-AddPersonTree", ex.Message, "AddPersonTree an error occurs");
            }
        }

        #endregion 添加子节点

        #region 多语言转换

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

        #endregion 多语言转换

        #region 分页

        /// <summary>
        /// 上一页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutLast(object sender, DirectEventArgs e)
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

        /// <summary>
        /// 下一页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutNext(object sender, DirectEventArgs e)
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

        /// <summary>
        /// 首页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutFisrt(object sender, DirectEventArgs e)
        {
            curpage.Value = 1;
            ShowQuery(1);
        }

        /// <summary>
        ///尾页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutEnd(object sender, DirectEventArgs e)
        {
            curpage.Value = allPage.Value;
            int page = int.Parse(curpage.Value.ToString());
            ShowQuery(page);
        }

        /// <summary>
        /// Sets the state of the but.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private void SetButState(int page)
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

        /// <summary>
        /// Shows the query.
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        private void ShowQuery(int currentPage)
        {
            int rownum = 15;

            int startNum = 0;
            int endNum = 0;
            if (currentPage == 1)
            {
                startNum = 0;
                endNum = rownum;
            }
            else
            {
                startNum = (currentPage - 1) * rownum;
                endNum = currentPage * rownum;
            }
            Query(GetWhere(), startNum);
            SetButState(currentPage);
        }

        #endregion 分页

        private void Query(string where, int currentPage)
        {
            DataTable dt = tgsDataInfo.GetSpecial(where, currentPage);
            Session["datatable"] = dt;
            //  DataTable dt = tgsDataInfo.GetPeccancyAreaCountNew(where);//tgsDataInfo.GetPeccancyAreaCount(where, startNum, endNum);

            if (dt != null && dt.Rows.Count > 0)
            {
                SelectFirst(dt.Rows[0]);
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
                this.StoreSpecial.DataSource = dt;
                this.StoreSpecial.DataBind();
            }
            else
            {
                this.lblCurpage.Text = "1";
                this.lblAllpage.Text = "0";
                this.lblRealcount.Text = "0";
                this.StoreSpecial.DataSource = dt;
                this.StoreSpecial.DataBind();
                Notice("信息提示", "当前没数据");
                return;
            }

            //string name = "";
            //if (CmbStartStation.SelectedIndex != -1)
            //{
            //    name = "(" + CmbStartStation.SelectedItem.Text;
            //}
            //if (CmbEndStation.SelectedIndex != -1)
            //{
            //    name = name + "-->" + CmbEndStation.SelectedItem.Text + ")";
            //}
            //DataTable wfxw = tgsDataInfo.GetPeccancyAreaCountForWfxw(where);
            //PecAreaCountWfxw(wfxw, name);

            //DataTable xssd = tgsDataInfo.GetPeccancyAreaCountForXssd(where);
            //PecAreaCountXssd(xssd, name);
            //this.pnlXssdData.Hide();
            //this.pnlWfxwData.ActiveIndex = 0;
        }

        private void GetData()
        {
            string where = GetWhere();
            if (!string.IsNullOrEmpty(where))
            {
                DataTable dt = tgsDataInfo.GetSpecialCount(where);

                if (dt != null && dt.Rows.Count > 0)
                {
                    realCount.Value = dt.Rows[0]["col0"].ToString();
                    curpage.Value = 1;
                    int rownum = 15;
                    allPage.Value = (int)Math.Ceiling(double.Parse(realCount.Value.ToString()) / rownum);
                    ShowQuery(1);
                }
            }
        }
    }
}