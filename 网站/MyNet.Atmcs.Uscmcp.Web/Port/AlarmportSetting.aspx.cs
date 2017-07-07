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
    public partial class AlarmportSetting: System.Web.UI.Page
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
        //private static string starttime = "";
        //private static string endtime = "";

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
                StoreDataBind();
                //BuildTreeStation(TreeStation.Root);
                //BuildTreePerson(TreePerson.Root);
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
                PortDataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-TbutQueryClick", ex.Message, "TbutQueryClick an error occurs");
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
                PortDataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-ButRefreshClick", ex.Message, "ButRefreshClick an error occurs");
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
                if (!string.IsNullOrEmpty(kakouQuery.Value))//判断卡口是否为空
                {
                    string js = "clearMenuQuery();";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
                TxtplateId1.Reset();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-ButResetClick", ex.Message, "ButResetClick an error occurs");
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
                logManager.InsertLogError("AlarmportSetting.aspx-ButPrintClick", ex.Message, "ButPrintClick an error occurs");
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
                logManager.InsertLogError("AlarmportSetting.aspx-ToXml", ex.Message, "ToXml an error occurs");
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
                logManager.InsertLogError("AlarmportSetting.aspx-ToExcel", ex.Message, "ToExcel an error occurs");
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
                logManager.InsertLogError("AlarmportSetting.aspx-ToCsv", ex.Message, "ToCsv an error occurs");
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
                this.Session["portCount"] = 0;
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
                logManager.InsertLogError("AlarmportSetting.aspx-StartLongAction", ex.Message, "StartLongAction an error occurs");
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
                        PortDataBind();
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
                logManager.InsertLogError("AlarmportSetting.aspx-RefreshProgress", ex.Message, "RefreshProgress an error occurs");
            }
        }

        #endregion 控件事件

        #region DirectMethod事件

        ///// <summary>
        ///// 获取时间
        ///// </summary>
        ///// <param name="isstart"></param>
        ///// <param name="strtime"></param>
        //[DirectMethod]
        //public void GetDateTime(bool isstart, string strtime)
        //{           
        //    try
        //    {
        //        if (isstart)
        //            //starttime = string.Format("HH:mm", strtime);
        //            starttime = strtime;
        //        else
        //            //endtime = string.Format("HH:mm", strtime);
        //            endtime = strtime;
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(ex);
        //        logManager.InsertLogError("AlarmportSetting.aspx-GetDateTime", ex.Message, "GetDateTime has an exception");
        //    }
        //}

        /// <summary>
        /// 保存布控信息事件
        /// </summary>
        [DirectMethod]
        public void InfoSave()
        {
            try
            {
                AddPort();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-InfoSave", ex.Message, "InfoSave an error occurs");
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
                        Handler = "AlarmportSetting.DoYes()",
                        Text = "是"
                    },
                    No = new MessageBoxButtonConfig
                    {
                        Handler = "AlarmportSetting.DoNo()",
                        Text = "否"
                    }
                }).Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-DoConfirm", ex.Message, "DoConfirm an error occurs");
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
                hs.Add("BH", BH.Text);
                if (tgsDataInfo.DeletePortInfo(hs) > 0)
                {
                    try
                    {
                        bool flag = client.cancleLayout(BH.Text);
                        ILog.WriteErrorLog("From control return:" + flag.ToString());
                    }
                    catch (Exception ex)
                    {
                        logManager.InsertLogError("AlarmportSetting.aspx-DoYes", ex.Message, "DoYes charged with an exception occurs");
                    }
                    Notice("信息提示", "删除成功");
                    PortDataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-DoYes", ex.Message, "DoYes an error occurs");
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
                //卡口范围
                string kkid = string.Empty;
                string kkname = this.kakou.Value;
                if (string.IsNullOrEmpty(kkname))
                {
                    Notice("卡口流量设置提示", "卡口名称不能为空，请选择");
                    return;
                }

                kkid = this.kakouId.Value.ToString();

                //卡口方向
                //if (CmbKakouDirection.Value == null)
                //{
                //    Notice("卡口流量设置提示", "卡口名称不能为空，请选择");
                //    return;
                //}

                if (KSSD == null || KSSD.Text.Trim() == "")
                {
                    Notice("卡口流量设置提示", "开始时间不能为空，请输入");
                    return;
                }
                if (JSSD == null || JSSD.Text.Trim() == "")
                {
                    Notice("卡口流量设置提示", "结束时间不能为空，请输入");
                    return;
                }
                string kssj = DateTime.Parse(KSSD.Text).ToString("HH:mm");
                string jssj = DateTime.Parse(JSSD.Text).ToString("HH:mm");              
                if (kssj.CompareTo(jssj)>=0)
                {
                    Notice("卡口流量设置提示", "开始时间不能大于结束时间，请输入");
                    return;
                }
                if (TJZQ.Text.Trim() == "")
                {
                    Notice("卡口流量设置提示", "统计周期不能为空，请输入");
                    return;
                }
                if (BJFZ.Text.Trim() == "")
                {
                    Notice("卡口流量设置提示", "报警阈值不能为空，请输入");
                    return;
                }

                Hashtable hs = new Hashtable();
                if (!string.IsNullOrEmpty(BH.Text))
                    hs.Add("BH", BH.Text);
                else
                    hs.Add("BH", tgsDataInfo.GetTgsRecordId());
                hs.Add("KKBH", kkid);
                UserInfo user = Session["userinfo"] as UserInfo;
                hs.Add("KSSD", kssj);
                hs.Add("JSSD", jssj);
                hs.Add("TJZQ", TJZQ.Text);
                hs.Add("BJFZ", BJFZ.Text);
                hs.Add("KKPZR", user.UserCode);
                //hs.Add("GXSJ", GXSJ.Text);
                hs.Add("KKPZRMS", user.Name);
                hs.Add("KKBHMS", kkname);
                string kkDirectionId = string.Empty;
                string kkDirectionName = string.Empty;
                if (CmbKakouDirection.Value== null)//判断卡口方向是否为空
                {
                    kkDirectionId = "0";
                }
                else
                {
                    kkDirectionId = this.CmbKakouDirection.Value.ToString();
                    kkDirectionName = this.CmbKakouDirection.SelectedItem.Text;
                }               
                hs.Add("KKFXBH", kkDirectionId);
                hs.Add("KKFXMS", kkDirectionName);

                if (tgsDataInfo.UpdatePortInfo(hs) > 0)
                {
                    TJZQ.Text = "";
                    BJFZ.Text = "";
                    KSSD.Text = "0:0";
                    JSSD.Text = "0:0";

                    //卡口列表
                    if (!string.IsNullOrEmpty(kakou.Value))//判断卡口是否为空
                    {
                        string js = "clearMenu();";
                        this.ResourceManager1.RegisterAfterClientInitScript(js);
                    }

                    //卡口方向
                    this.CmbKakouDirection.SelectedItem.Text = "";
                    this.CmbKakouDirection.Text = "";
                    PortDataBind();

                    Notice("卡口流量设置提示", "保存成功");                  
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-UpdateData", ex.Message, "UpdateData an error occurs");
            }
        }
        
        /// <summary>
        /// 更新布控记录
        // </summary>
        [DirectMethod]
        public void UpdateAddData()
        {
            try
            {
                //卡口范围
                string kkid = string.Empty;
                string kkname = this.kakou.Value;
                if (string.IsNullOrEmpty(kkname))
                {
                    Notice("卡口流量设置提示", "卡口名称不能为空，请选择");
                    return;
                }

                kkid = this.kakouId.Value.ToString();

                ////卡口方向
                //if (CmbKakouDirection.Value == null)
                //{
                //    Notice("卡口流量设置提示", "卡口名称不能为空，请选择");
                //    return;
                //}

                if (KSSD == null || KSSD.Text.Trim() == "")
                {
                    Notice("卡口流量设置提示", "开始时间不能为空，请输入");
                    return;
                }
                if (JSSD == null || JSSD.Text.Trim() == "")
                {
                    Notice("卡口流量设置提示", "结束时间不能为空，请输入");
                    return;
                }
                string kssj = DateTime.Parse(KSSD.Text).ToString("HH:mm");
                string jssj = DateTime.Parse(JSSD.Text).ToString("HH:mm");              
                if (kssj.CompareTo(jssj)>=0)
                {
                    Notice("卡口流量设置提示", "开始时间不能大于结束时间，请输入");
                    return;
                }
                if (TJZQ.Text.Trim() == "")
                {
                    Notice("卡口流量设置提示", "统计周期不能为空，请输入");
                    return;
                }
                if (BJFZ.Text.Trim() == "")
                {
                    Notice("卡口流量设置提示", "报警阈值不能为空，请输入");
                    return;
                }

                Hashtable hs = new Hashtable();
                if (!string.IsNullOrEmpty(BH.Text))
                    hs.Add("BH", BH.Text);
                else
                    hs.Add("BH", tgsDataInfo.GetTgsRecordId());
                hs.Add("KKBH", kkid);
                UserInfo user = Session["userinfo"] as UserInfo;
                hs.Add("KSSD", kssj);
                hs.Add("JSSD", jssj);
                hs.Add("TJZQ", TJZQ.Text);
                hs.Add("BJFZ", BJFZ.Text);
                hs.Add("KKPZR", user.UserCode);
                //hs.Add("GXSJ", GXSJ.Text);
                hs.Add("KKPZRMS", user.Name);
                hs.Add("KKBHMS", kkname);
                string kkDirectionId = string.Empty;
                string kkDirectionName = string.Empty;
                if (CmbKakouDirection.Value == null)//判断卡口方向是否为空
                {
                    kkDirectionId = "0";
                }
                else
                {
                    kkDirectionId = this.CmbKakouDirection.Value.ToString();
                    kkDirectionName = this.CmbKakouDirection.SelectedItem.Text;
                }     
                hs.Add("KKFXBH", kkDirectionId);
                hs.Add("KKFXMS", kkDirectionName);

                if (tgsDataInfo.UpdatePortInfo(hs) > 0)
                {
                    BH.Text = tgsDataInfo.GetTgsRecordId();
                    TJZQ.Text = "";
                    BJFZ.Text = "";
                    KSSD.Text = "0:0";
                    JSSD.Text = "0:0";

                    //卡口列表
                    //if (!string.IsNullOrEmpty(kakou.Value))//判断卡口是否为空
                    //{
                    //    string js = "clearMenu();";
                    //    this.ResourceManager1.RegisterAfterClientInitScript(js);
                    //}

                    //卡口方向
                    //this.CmbKakouDirection.SelectedItem.Text = "";
                    //this.CmbKakouDirection.Text = "";

                    //注释下面的话不会由当前用户的默认设置   
                    KKPZR.Text = user.Name;
                    GXSJ.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    PortDataBind();

                    Notice("卡口流量设置提示", "保存成功");                  
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-UpdateData", ex.Message, "UpdateData an error occurs");
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
                logManager.InsertLogError("AlarmportSetting.aspx-Download", ex.Message, "Download an error occurs");
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
                 string value="";
                if (blog.Equals("0"))
                {
                    value = kakou.Value;
                }
                else{
                    value = kakouQuery.Value;
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
                 string js="";
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
            else
            {
                Session["tree"] = kakouIdQuery.Value;
            }
          
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
                PortDataBind();
                //GXSJ.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //KSSD.Text = DateTime.Now.ToString("HH:mm");
                //JSSD.Text = DateTime.Now.ToString("HH:mm");                
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-StoreDataBind", ex.Message, "StoreDataBind an error occurs");
            }
        }

        /// <summary>
        /// 绑定布控数据
        /// </summary>
        /// <param name="where"></param>
        private void PortDataBind()
        {
            try
            {
                GetData();                
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-PortDataBind", ex.Message, "PortDataBind an error occurs");
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
                    dt1.Columns.Remove("col0"); dt1.Columns.Remove("col1"); dt1.Columns.Remove("col6"); dt1.Columns.Remove("col10");
                    for (int i = 19; i < dt.Columns.Count - 1; i++)
                    {
                        dt1.Columns.Remove("col" + i.ToString());
                    }
                    //设置内存表中顺序
                    dt1.Columns["col8"].SetOrdinal(0);
                    dt1.Columns["col11"].SetOrdinal(1); 
                    dt1.Columns["col2"].SetOrdinal(2);
                    dt1.Columns["col3"].SetOrdinal(3);
                    dt1.Columns["col4"].SetOrdinal(4);
                    dt1.Columns["col5"].SetOrdinal(5);
                    dt1.Columns["col9"].SetOrdinal(6);
                    dt1.Columns["col7"].SetOrdinal(7);
                    dt1.Columns[0].ColumnName = GetLangStr("SpecialQuery11", "卡口列表");
                    dt1.Columns[1].ColumnName = GetLangStr("SpecialQuery18", "卡口方向");
                    dt1.Columns[2].ColumnName = GetLangStr("SpecialQuery12", "开始时间");
                    dt1.Columns[3].ColumnName = GetLangStr("SpecialQuery13", "结束时间");
                    dt1.Columns[4].ColumnName = GetLangStr("SpecialQuery14", "统计周期");
                    dt1.Columns[5].ColumnName = GetLangStr("SpecialQuery15", "报警阈值");
                    dt1.Columns[6].ColumnName = GetLangStr("SpecialQuery16", "配置人");
                    dt1.Columns[7].ColumnName = GetLangStr("SpecialQuery17", "配置时间");
                    return dt1;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-ChangeDataTable", ex.Message, "ChangeDataTable an error occurs");
            }
            return dt;
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
                string QueryKKBHMS = string.Empty;
                string kkname = this.kakouQuery.Value;
                if (!string.IsNullOrEmpty(kkname))
                {
                    QueryKKBHMS = this.kakouIdQuery.Value.ToString();
                    if (!string.IsNullOrEmpty(QueryKKBHMS))
                    {
                        where = where + " and  KKBH ='" + QueryKKBHMS.ToUpper() + "' ";
                    }
                }

                string QueryKKPZRMS = TxtplateId1.Text;
                if (!string.IsNullOrEmpty(QueryKKPZRMS))
                {
                    where = where + " and  KKPZRMS like '%" + QueryKKPZRMS.ToUpper() + "%' ";
                }   
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-GetWhere", ex.Message, "GetWhere an error occurs");
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
                PortDataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-MyData_Refresh", ex.Message, "MyData_Refresh an error occurs");
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
                BH.Text = dr["col0"].ToString();
                this.kakou.Value = dr["col8"].ToString();
                this.kakouId.Value = dr["col1"].ToString();
                if (Session["tree"] != null)
                {
                    Session["tree"] = null;
                }
                Session["tree"] = kakouId.Value;
                string js = "setMainValue('" + kakou.Value + "','" + kakouId.Value + "');";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
                KSSD.Text = GetDate(dr["col2"].ToString(), 2);
                JSSD.Text = GetDate(dr["col3"].ToString(), 2);
                TJZQ.Text = dr["col4"].ToString();            
                BJFZ.Text = dr["col5"].ToString();
                GXSJ.Text = dr["col7"].ToString();
                KKPZR.Text = dr["col9"].ToString();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-SelectFirst", ex.Message, "SelectFirst an error occurs");
            }
        }

        /// <summary>
        /// 添加布控记录
        /// </summary>
        private void AddPort()
        {
            try
            {
                BH.Text = tgsDataInfo.GetTgsRecordId();                
                TJZQ.Text = "";
                BJFZ.Text = "";
                KSSD.Text = "00:00";
                JSSD.Text = "00:00";

                //卡口列表
                if (!string.IsNullOrEmpty(kakou.Value))//判断卡口是否为空
                {
                    string js = "clearMenu();";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }

                //卡口方向
                this.CmbKakouDirection.SelectedItem.Text = "";
                this.CmbKakouDirection.Text = "";

                //注释下面的话不会由当前用户的默认设置   
                UserInfo user = Session["userinfo"] as UserInfo;
                KKPZR.Text=user.Name;
                GXSJ.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-AddPort", ex.Message, "AddPort an error occurs");
            }
        }

        /// <summary>
        /// 选择布控记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectPort(object sender, DirectEventArgs e)
        {
            try
            {
                object data = e.ExtraParams["sdata"];   
                string sdata = data.ToString();
                this.kakou.Value = GetdatabyField(sdata, "col8");
                this.kakouId.Value = GetdatabyField(sdata, "col1");
                if (Session["tree"] != null)
                {
                    Session["tree"] = null;
                }
                Session["tree"] = kakouId.Value;
                string js = "setMainValue('" + kakou.Value + "','" + kakouId.Value + "');";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
                BH.Text = GetdatabyField(sdata, "col0");
                KSSD.Text = GetdatabyField(sdata, "col2");
                JSSD.Text = GetdatabyField(sdata, "col3");
                TJZQ.Text = GetdatabyField(sdata, "col4");
                BJFZ.Text = GetdatabyField(sdata, "col5");
                GXSJ.Text = GetdatabyField(sdata, "col7");
                KKPZR.Text = GetdatabyField(sdata, "col9");
                this.CmbKakouDirection.Value = GetdatabyField(sdata, "col10");
                string kkfxid = this.CmbKakouDirection.Value.ToString();
                if (kkfxid == "0")//判断卡口方向是否为空
                {
                    this.CmbKakouDirection.Value = string.Empty;
                }
                //this.CmbKakouDirection.SelectedItem.Text = GetdatabyField(sdata, "col11");
                this.CmbKakouDirection.Text = GetdatabyField(sdata, "col11");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-SelectPort", ex.Message, "SelectPort an error occurs");
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
                logManager.InsertLogError("AlarmportSetting.aspx-GetDate", ex.Message, "GetDate an error occurs");
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
                logManager.InsertLogError("AlarmportSetting.aspx-GetDate", ex.Message, "GetDate an error occurs");
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
                logManager.InsertLogError("AlarmportSetting.aspx-GetdatabyField", ex.Message, "GetdatabyField an error occurs");
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
                logManager.InsertLogError("AlarmportSetting.aspx-Notice", ex.Message, "Notice an error occurs");
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
                        hs.Add("BH", tgsDataInfo.GetTgsRecordId());
                        hs.Add("ZXNR", mydt.Rows[i]["专项内容"].ToString());
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
                        //if (YSSJ.SelectedDate != null)
                        //{
                        //    hs.Add("yxsj", mydt.Rows[i]["有效时间"].ToString());
                        //}
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
                        logManager.InsertLogError("AlarmportSetting.aspx-BatchUpdateData", ex.Message, "BatchUpdateData an error occurs");
                    }
                }
                // Session["suspicionResult"] = count;
                //this.Session.Remove("suspicionCount");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmportSetting.aspx-BatchUpdateData", ex.Message, "BatchUpdateData an error occurs");
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
                logManager.InsertLogError("AlarmportSetting.aspx-UploadExcelFile", ex.Message, "UploadExcelFile an error occurs");
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
        /// 组件卡口列表树
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private Ext.Net.TreeNodeCollection BuildTreeStation(Ext.Net.TreeNodeCollection nodes)
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
                logManager.InsertLogError("AlarmportSetting.aspx-BuildTreeStation", ex.Message, "BuildTreeStation has an exception");
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
                logManager.InsertLogError("AlarmportSetting.aspx-AddDepartTree", ex.Message, "AddDepartTree has an exception");
                ILog.WriteErrorLog(ex);
            }
        }
        #endregion 私有方法

        #region 添加子节点

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
                logManager.InsertLogError("AlarmportSetting.aspx-AddStationTree", ex.Message, "AddStationTree has an exception");
                ILog.WriteErrorLog(ex);
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
            DataTable dt = tgsDataInfo.GetPort(where, currentPage);
            Session["datatable"] = dt;
            //  DataTable dt = tgsDataInfo.GetPeccancyAreaCountNew(where);//tgsDataInfo.GetPeccancyAreaCount(where, startNum, endNum);

            if (dt != null && dt.Rows.Count > 0)
            {
                //SelectFirst(dt.Rows[0]);
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
                this.StoreAlarmport.DataSource = dt;
                this.StoreAlarmport.DataBind();
            }
            else
            {
                this.lblCurpage.Text = "1";
                this.lblAllpage.Text = "0";
                this.lblRealcount.Text = "0";
                this.StoreAlarmport.DataSource = dt;
                this.StoreAlarmport.DataBind();
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
                DataTable dt = tgsDataInfo.GetPortCount(where);

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