using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
// ***********************************************************************
// Assembly         : MyNet.Atmcs.Uscmcp.Web
// Author           : zlsyl
// Created          : 08-12-2016
//
// Last Modified By : zlsyl
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="DeviceOperation.aspx.cs" company="ZKLT">
//     Copyright (c) ZKLT. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Data;

/// <summary>
/// The Web namespace.
/// </summary>
namespace MyNet.Atmcs.Uscmcp.Web
{
    /// <summary>
    /// Class DeviceOperation.
    /// </summary>
    public partial class DeviceOperation : System.Web.UI.Page
    {
        #region 成员变量

        /// <summary>
        /// The setting manager
        /// </summary>
        private SettingManager settingManager = new SettingManager();

        /// <summary>
        /// The TGS pproperty
        /// </summary>
        private TgsPproperty tgsPproperty = new TgsPproperty();

        /// <summary>
        /// The device manager
        /// </summary>
        private Bll.DeviceManager deviceManager = new Bll.DeviceManager();

        /// <summary>
        /// The user login
        /// </summary>
        private UserLogin userLogin = new UserLogin();

        private static string tyeid = "";
        private static string starttime = "";
        private static string endtime = "";
        private static string maxtime = "";
        private static string mintime = "";

        private static string  start1time = "";
        private static string end1time = "";



        static DataTable dtname = null;



        /// <summary>
        /// 用户名
        /// </summary>
        static string uName;
        /// <summary>
        /// 获取IP
        /// </summary>
        static string nowIp;

        static string sbName;

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        /// <summary>
        /// 运维方式
        /// </summary>
        static string ywfs;
        /// <summary>
        /// 申请单位
        /// </summary>
        static string sqdw;
        /// <summary>
        /// 申请人
        /// </summary>
        static string sqr;
        /// <summary>
        /// 录入时间
        /// </summary>
        static string lrsj;
        /// <summary>
        /// 录入说明
        /// </summary>
        static string lrsm;
        /// <summary>
        /// 审核人
        /// </summary>
        static string shr;
        /// <summary>
        /// /审核时间
        /// </summary>
        static string shsj;
        /// <summary>
        /// /审核意见
        /// </summary>
        static string shyj;


        static string sbid;

        /// <summary>
        /// 运维方式
        /// </summary>
        static DataTable dtyw;
        /// <summary>
        /// 申请单位
        /// </summary>
        static DataTable dtsq;


        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("DeviceOperation1", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return; 
            }
            if (!X.IsAjaxRequest)
            {
                BuildTree(TreePanel1.Root);
                ToolExport.Disabled = true;
                StoreDataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                uName = userinfo.UserName;
                nowIp = userinfo.NowIp;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("DeviceOperation9", "访问：设备运维信息"), userinfo.NowIp, "0");
                //this.DataBind();
            }
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                GridDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            try
            {
                this.ResourceManager1.RegisterAfterClientInitScript("clearTime();");
                CmbDeviceType.Reset();
                TxtDeviceName.Reset();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-ButResetClick", ex.Message+"；"+ex.StackTrace, "ButResetClick has an exception");
            }
        }

        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void ButRefreshClick(object sender, DirectEventArgs e)
        {
            try
            {
                GridDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-ButRefreshClick", ex.Message+"；"+ex.StackTrace, "ButRefreshClick has an exception");
            }
        }

        /// <summary>
        /// 打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void ButPrintClick(object sender, DirectEventArgs e)
        {
            try
            {
                DataTable dt = ChangeDataTable();
                if (dt != null)
                {
                    Session["printdatatable"] = ChangeDataTable();
                    string xml = Bll.Common.GetPrintXml(GetLangStr("DeviceOperation69", "设备运维信息"), "", "", "printdatatable");
                    string js = "OpenPrintPageV(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-ButPrintClick", ex.Message+"；"+ex.StackTrace, "ButPrintClick has an exception");
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            //GridDataBind("rownum <100");
        }

        /// <summary>
        /// Handles the Openation event of the MyData control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void MyData_Openation(object sender, StoreRefreshDataEventArgs e)
        {
            //GridDataBind("rownum <100");
        }

        /// <summary>
        /// 导出xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
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
                logManager.InsertLogError("DeviceOperation.aspx-ToXml", ex.Message+"；"+ex.StackTrace, "ToXml has an exception");
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
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
                logManager.InsertLogError("DeviceOperation.aspx-ToExcel", ex.Message+"；"+ex.StackTrace, "ToExcel has an exception");
            }
        }

        /// <summary>
        /// 导出Csv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
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
                logManager.InsertLogError("DeviceOperation.aspx-ToCsv", ex.Message+"；"+ex.StackTrace, "ToCsv has an exception");
            }
        }

        private static object data;

        /// <summary>
        /// 选中行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void SelectLed(object sender, DirectEventArgs e)
        {
            try
            {
                PagingToolbar2.PageSize = 5;//页容量为5
                PagingToolbar2.PageIndex = 1;//显示第一页数据
                data = e.ExtraParams["id"];
                 sbName = e.ExtraParams["sbName"];
                string id = data.ToString();
                DataTable da = deviceManager.GetOperation(" 1=1 AND  device_id='shebei" + id + "' ");
                SroPenation.DataSource = da;
                SroPenation.DataBind();
                tyeid = id;
                if (Session["yunweitable"] != null)
                {
                    Session["yunweitable"] = null;
                }

                Session["yunweitable"] = da;

                int pageIndex = PagingToolbar1.PageIndex;
                GridDataBind(GetWhere(), pageIndex);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-SelectLed", ex.Message+"；"+ex.StackTrace, "SelectLed has an exception");
            }
        }

        /// <summary>
        /// 添加运维信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void ButAdd_Click(object sender, DirectEventArgs e)
        {
            try
            {
                string ID = Math.Abs(Guid.NewGuid().ToString().GetHashCode()).ToString();
                string device_id = "SHEBEI" + hidder.Text;

                string kssj = DateTime.Parse(starttime).ToString("yyyy-MM-dd HH:mm:ss");
                string jssj = DateTime.Parse(endtime).ToString("yyyy-MM-dd HH:mm:ss");

                //string lurutime = date_lurutiem.SelectedDate.ToString("yyyy-MM-dd") + " " + this.time_luru.Text.ToString();
                //string lurutime = Convert.ToDateTime(date_lurutiem.Text).ToString("yyyy-MM-dd") + " " + this.time_luru.Text.ToString();
                //string shtime = Convert.ToDateTime(date_shtime.Text).ToString("yyyy-MM-dd") + " " + this.time_sh.Text.ToString();
                Hashtable hs = new Hashtable();
                hs.Add("id", ID);
                hs.Add("device_id", device_id);
                hs.Add("openration_people", this.txt_lurupeople.Text.ToString());
                hs.Add("openration_time", kssj);
                hs.Add("openration_event", this.txt_shuoming.Text.ToString());
                hs.Add("openration_type", this.com_operationtype.Value);
                hs.Add("openration_unit", this.com_operationdanwei.Value);
                hs.Add("openration_auditor", this.txt_shpeople.Text.ToString());
                hs.Add("openration_opinion", this.txt_shyijian.Text.ToString());
                hs.Add("openration_revtime", jssj);
                if (deviceManager.insertOperation(hs) > 0)
                {
                    int pageIndex = PagingToolbar1.PageIndex;
                    GridDataBind(GetWhere(), pageIndex);
                    DataTable da = deviceManager.GetOperation(" 1=1 AND  device_id='" + device_id + "' ");
                    SroPenation.DataSource = da;
                    SroPenation.DataBind();
                    Window1.Hide();
                    string lblname = "";
                    lblname += Bll.Common.AssembleRunLog("", com_operationtype.SelectedItem.Text, GetLangStr("DeviceOperation34","维护方式"), "0");
                    lblname += Bll.Common.AssembleRunLog("", com_operationdanwei.SelectedItem.Text, GetLangStr("DeviceOperation28","申请单位"), "0");
                    lblname += Bll.Common.AssembleRunLog("", txt_lurupeople.Text, GetLangStr("DeviceOperation27","申请人"), "0");
                    lblname += Bll.Common.AssembleRunLog("", kssj, GetLangStr("DeviceOperation29","录入时间"), "0");
                    lblname += Bll.Common.AssembleRunLog("", txt_shuoming.Text, GetLangStr("DeviceOperation39","录入说明"), "0");
                    lblname += Bll.Common.AssembleRunLog("", txt_shpeople.Text, GetLangStr("DeviceOperation40","审核人"), "0");
                    lblname += Bll.Common.AssembleRunLog("", jssj, GetLangStr("DeviceOperation41","审核时间"), "0");
                    lblname += Bll.Common.AssembleRunLog("", txt_shyijian.Text, GetLangStr("DeviceOperation42","审核意见"), "0");
                    logManager.InsertLogRunning(uName, GetLangStr("DeviceOperation21","新增:") + lblname, nowIp, "1");
                    Notice(GetLangStr("DeviceOperation70", "信息提示"), GetLangStr("DeviceOperation71", "添加成功"));
                    

                    //BuildTree(TreePanel1.Root);
                    //TreePanel1.Render();
                    //重新加载行事件

                    DataTable dt = deviceManager.GetOperation(" 1=1 AND  device_id='shebei" + tyeid + "' ");
                    SroPenation.DataSource = dt;
                }

                qingkongadd();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-ButAdd_Click", ex.Message+"；"+ex.StackTrace, "ButAdd_Click has an exception");
            }
        }

        #endregion 控件事件

        #region DirectMethod

        /// <summary>
        /// 设备点击事件
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [DirectMethod]
        public void SelectNode(string code)
        {
            try
            {
                GridDataBind(" 1=1 and device_type_id='" + code + "'");
                DataTable da = deviceManager.GetOperation(" 1=2");
                SroPenation.DataSource = da;
                SroPenation.DataBind();
                this.CmbDeviceType.Value = code;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-SelectNode", ex.Message+"；"+ex.StackTrace, "SelectNode has an exception");
            }
        }

        /// <summary>
        /// Opencomms the specified code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [DirectMethod(Namespace = "OnEvl")]
        public void opencomm(string code)
        {
            try
            {
                DataTable dt = deviceManager.GetDevice(" 1=1 AND DEVICE_TYPE='" + code + "' ");
                StoreDeviceName.DataSource = dt;
                StoreDeviceName.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-opencomm", ex.Message+"；"+ex.StackTrace, "opencomm has an exception");
            }
        }

        /// <summary>
        /// 添加运维信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [DirectMethod(Namespace = "OnEvl")]
        public void Add_luru(string code)
        {
            int pageIndex = PagingToolbar1.PageIndex;
            GridDataBind(GetWhere(), pageIndex);
            this.butAdd1.Text = GetLangStr("DeviceOperation72", "添加");
            this.hidder.Text = code;
            //页面没数据
            //StoreDataBind();
            if (Session["yunweitable"] != null)
            {
                SroPenation.DataSource = (DataTable)Session["yunweitable"];
                SroPenation.DataBind();
            }
            Window1.Show();
        }

        /// <summary>
        /// Open_lurus the specified code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [DirectMethod(Namespace = "OnEvl")]
        public void open_luru(string code)
        {
            try
            {

                DataTable da = deviceManager.GetOperation(" id='" + code + "'");
                this.ComboBox2.Value = da.Rows[0]["col5"].ToString();
                this.ComboBox3.Value = da.Rows[0]["col6"].ToString();
                this.TextField1.Text = da.Rows[0]["col2"].ToString();
               // string[] lurutiem = (da.Rows[0]["col3"].ToString()).Split(' ');
                //this.DateField1.Text = lurutiem[0].ToString();
                //this.TimeField1.Text = lurutiem[1].ToString();
               //li4.InnerText=
                this.TextField2.Text = da.Rows[0]["col4"].ToString();
                this.TextField3.Text = da.Rows[0]["col7"].ToString();
              //  string[] shtiem = (da.Rows[0]["col9"].ToString()).Split(' ');
                //this.DateField2.Text = shtiem[0].ToString();
                //this.TimeField2.Text = shtiem[1].ToString();
                this.TextField4.Text = da.Rows[0]["col8"].ToString();


                ywfs = com_upte_lrtype.SelectedItem.Text;
                sqdw = com_upte_lrdanwei.SelectedItem.Text;
                sqr = txt_upde_lrpeople.Text;
                lrsj = start1time;
                lrsm = txt_upde_lrshuoming.Text;
                shr = txt_upde_shpeople.Text;
                shsj = end1time;
                shyj = txt_upde_shyijian.Text;


                Window2.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-open_luru", ex.Message+"；"+ex.StackTrace, "open_luru has an exception");
            }
        }

        private static string yuweiId = "";

        /// <summary>
        /// Updates the specified code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [DirectMethod(Namespace = "OnEvl")]
        public void update(string code)
        {
            try
            {
                //页面没数据
                StoreDeviceManager.DataSource = (DataTable)Session["datatable"];
                StoreDeviceManager.DataBind();
                SroPenation.DataSource = (DataTable)Session["yunweitable"];
                SroPenation.DataBind();
                DataTable da = deviceManager.GetSelectOperation(" id='" + code + "'");
                this.com_upte_lrtype.Value = da.Rows[0]["col5"].ToString();
                this.com_upte_lrdanwei.Value = da.Rows[0]["col6"].ToString();
                this.txt_upde_lrpeople.Text = da.Rows[0]["col2"].ToString();
              
                start1time = da.Rows[0]["col3"].ToString();
                end1time = da.Rows[0]["col9"].ToString();

                string js = "setTime('" + start1time + "','" + end1time + "');";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
                //string[] lurutiem = (da.Rows[0]["col3"].ToString()).Split(' ');
                //this.date_upde_lrdate.Text = lurutiem[0].ToString();
                //this.time_upde_lrtime.Text = lurutiem[1].ToString();
                this.txt_upde_lrshuoming.Text = da.Rows[0]["col4"].ToString();
                this.txt_upde_shpeople.Text = da.Rows[0]["col7"].ToString();
                // string[] shtiem = (da.Rows[0]["col9"].ToString()).Split(' ');
                //this.date_upde_shdate.Text = shtiem[0].ToString();
                //this.time_upde_shtime.Text = shtiem[1].ToString();
                this.txt_upde_shyijian.Text = da.Rows[0]["col8"].ToString();
                this.hidder1.Value = da.Rows[0]["col1"].ToString();
                yuweiId = code;
                DataRow[] dt = dtyw.Select("col0='" + da.Rows[0]["col5"].ToString() + "'");
                ywfs = dt[0]["col1"].ToString();
                DataRow[] dt1 = dtyw.Select("col0='" + da.Rows[0]["col6"].ToString() + "'");
                sqdw = dt1[0]["col1"].ToString();
                sqr = txt_upde_lrpeople.Text;
                lrsj = start1time;
                lrsm = txt_upde_lrshuoming.Text;
                shr = txt_upde_shpeople.Text;
                shsj = end1time;
                shyj = txt_upde_shyijian.Text;

                Window3.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-update", ex.Message+"；"+ex.StackTrace, "update has an exception");
            }
        }

        /// <summary>
        /// Delectes the specified identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codeid"></param>
        /// <returns></returns>
        [DirectMethod(Namespace = "OnDel")]
        public void delecte(string id, string codeid, string sqr,string sqdw)
        {
            try
            {
                int i = deviceManager.DeleteOperation(id);
                if (i > 0)
                {
                    GridDataBind(GetWhere());
                    DataTable da = deviceManager.GetOperation(" 1=1 AND  device_id='" + codeid + "' ");
                    SroPenation.DataSource = da;
                    SroPenation.DataBind();
                    logManager.InsertLogRunning(uName, GetLangStr("","删除:设备[") + sbName + "]" + GetLangStr("",";申请人[") + sqr + GetLangStr("","];申请单位[") + sqdw + "]", nowIp, "3");
                    Notice(GetLangStr("DeviceOperation70", "信息提示"), GetLangStr("DeviceOperation73", "删除成功"));
                    DataTable dt = deviceManager.GetOperation(" 1=1 AND  device_id='shebei" + tyeid + "' ");
                    SroPenation.DataSource = dt;
                }
                else
                {
                    Notice(GetLangStr("DeviceOperation70", "信息提示"), GetLangStr("DeviceOperation45", "删除失败"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-delecte", ex.Message+"；"+ex.StackTrace, "delecte has an exception");
            }
        }

        /// <summary>
        /// Updatedevices the specified code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [DirectMethod(Namespace = "OnEvl")]
        public void updatedevice(string code)
        {
            try
            {
                string id = code;
                string decice_id = this.hidder1.Value.ToString();
                string kssj = DateTime.Parse(starttime).ToString("yyyy-MM-dd HH:mm:ss");
                string jssj = DateTime.Parse(endtime).ToString("yyyy-MM-dd HH:mm:ss");
                //string lurutime = date_upde_lrdate.SelectedDate.ToString("yyyy-MM-dd") + " " + this.time_upde_lrtime.Text.ToString();
                //string shtime = date_upde_shdate.SelectedDate.ToString("yyyy-MM-dd") + " " + this.time_upde_shtime.Text.ToString();
                Hashtable hs = new Hashtable();
                hs.Add("id", id);

                hs.Add("openration_people", this.txt_upde_lrpeople.Text.ToString());
                hs.Add("openration_time", kssj);
                hs.Add("openration_event", this.txt_upde_lrshuoming.Text.ToString());
                hs.Add("openration_type", this.com_upte_lrtype.Value);
                hs.Add("openration_unit", this.com_upte_lrdanwei.Value);
                hs.Add("openration_auditor", this.txt_upde_shpeople.Text.ToString());
                hs.Add("openration_opinion", this.txt_upde_shyijian.Text.ToString());
                hs.Add("openration_revtime", jssj);
                if (deviceManager.uptateOperation(hs) > 0)
                {
                    DataTable da = deviceManager.GetOperation(" 1=1 AND  device_id='" + decice_id + "' ");
                    SroPenation.DataSource = da;
                    SroPenation.DataBind();
                    Window3.Hide();


                    Notice(GetLangStr("DeviceOperation70", "信息提示"), GetLangStr("DeviceOperation75", "修改成功"));
                    DataTable dt = deviceManager.GetOperation(" 1=1 AND  device_id='shebei" + tyeid + "' ");
                    SroPenation.DataSource = dt;
                }
                qingkongupdate();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-updatedevice", ex.Message+"；"+ex.StackTrace, "updatedevice has an exception");
            }
        }

        /// <summary>
        /// Updatedevices the specified code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [DirectMethod(Namespace = "OnEvl")]
        public void updatedeviceqw()
        {
            try
            {
                string id = yuweiId;
                string decice_id = this.hidder1.Value.ToString();
                string kssj = DateTime.Parse(lrsj).ToString("yyyy-MM-dd HH:mm:ss");
                string jssj = DateTime.Parse(shsj).ToString("yyyy-MM-dd HH:mm:ss");
                Hashtable hs = new Hashtable();
                hs.Add("id", id);
                hs.Add("openration_people", this.txt_upde_lrpeople.Text.ToString());
                hs.Add("openration_time", kssj);
                hs.Add("openration_event", this.txt_upde_lrshuoming.Text.ToString());
                hs.Add("openration_type", this.com_upte_lrtype.Value);
                hs.Add("openration_unit", this.com_upte_lrdanwei.Value);
                hs.Add("openration_auditor", this.txt_upde_shpeople.Text.ToString());
                hs.Add("openration_opinion", this.txt_upde_shyijian.Text.ToString());
                hs.Add("openration_revtime", jssj);
                if (deviceManager.uptateOperation(hs) > 0)
                {
                    StoreDeviceManager.DataSource = (DataTable)Session["datatable"];
                    StoreDeviceManager.DataBind();
                    DataTable da = deviceManager.GetOperation(" 1=1 AND  device_id='" + decice_id + "' ");
                    SroPenation.DataSource = da;
                    SroPenation.DataBind();
                    Window3.Hide();
                    string ywfss = com_upte_lrtype.SelectedItem.Text;
               string sqdws = com_upte_lrdanwei.SelectedItem.Text;
               string sqrs = txt_upde_lrpeople.Text;
               string lrsjs = start1time;
               string lrsms = txt_upde_lrshuoming.Text;
               string shrs = txt_upde_shpeople.Text;
               string shsjs = end1time;
               string shyjs = txt_upde_shyijian.Text;
               string lbname = "";
               lbname += Bll.Common.AssembleRunLog(ywfs,ywfss,GetLangStr("DeviceOperation58","运维方式"),"1");
               lbname += Bll.Common.AssembleRunLog(sqdw, sqdws, GetLangStr("DeviceOperation28","申请单位"), "1");
               lbname += Bll.Common.AssembleRunLog(sqr, sqrs, GetLangStr("DeviceOperation27","申请人"), "1");
               lbname += Bll.Common.AssembleRunLog(lrsj, lrsjs, GetLangStr("DeviceOperation29","录入时间"), "1");
               lbname += Bll.Common.AssembleRunLog(lrsm, lrsms, GetLangStr("DeviceOperation39","录入说明"), "1");
               lbname += Bll.Common.AssembleRunLog(shr, shrs, GetLangStr("DeviceOperation40","审核人"), "1");
               lbname += Bll.Common.AssembleRunLog(shsj, shsjs, GetLangStr("DeviceOperation41","审核时间"), "1");
               lbname += Bll.Common.AssembleRunLog(shyj, shyjs, GetLangStr("DeviceOperation42","审核意见"), "1");
               DataRow[] dr = dtname.Select("col0='" + sbid + "'");
                string sbne = dr[0]["col2"].ToString();
               logManager.InsertLogRunning(uName, GetLangStr("DeviceOperation67","修改;[")+sbne+"]"+ lbname, nowIp, "2");
                    Notice(GetLangStr("DeviceOperation70", "信息提示"), GetLangStr("DeviceOperation75", "修改成功"));
                }
                qingkongupdate();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-updatedeviceqw", ex.Message+"；"+ex.StackTrace, "updatedeviceqw has an exception");
            }
        }

        #endregion DirectMethod

        #region 私有方法

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        /// <returns></returns>
        private void StoreDataBind()
        {
            try
            {
                this.StoreDeviceType.DataSource = deviceManager.GetDeviceTypeAll();
                this.StoreDeviceType.DataBind();
                this.StoreDeviceName.DataSource=dtyw = Bll.Common.ChangColName(settingManager.getDictData("00", "281001"));
                this.StoreDeviceName.DataBind();
                this.StoreDeviceDanWei.DataSource=dtsq = Bll.Common.ChangColName(settingManager.getDictData("00", "281002"));
                this.StoreDeviceDanWei.DataBind();
                GridDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-StoreDataBind", ex.Message+"；"+ex.StackTrace, "StoreDataBind has an exception");
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
                root.Text = GetLangStr("DeviceOperation74", "设备信息列表");

                nodes.Add(root);

                DataTable dt = deviceManager.GetDeviceTypeAll();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    string code = dt.Rows[i]["col0"].ToString();
                    DataTable dt2 = deviceManager.GetDeviceByDeviceType(code);
                    string countvalue = "0";
                    if (dt2 != null)
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            countvalue = Convert.ToString(dt2.Rows.Count);
                        }
                        else
                        {
                            node.Disabled = true;
                        }
                    }
                    node.Text = dt.Rows[i]["col1"].ToString() + "(" + countvalue + ")";
                    node.Listeners.Click.Handler = "DeviceOperation.SelectNode('" + code + "') ;";
                    node.Icon = Icon.DriveNetwork;
                    node.NodeID = dt.Rows[i]["col0"].ToString();
                    node.Expanded = true;
                    root.Nodes.Add(node);
                }
                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-BuildTree", ex.Message+"；"+ex.StackTrace, "BuildTree has an exception");
                return null;
            }
        }

        /// <summary>
        /// 转换datatable为hashtable
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
                logManager.InsertLogError("DeviceOperation.aspx-ConventDataTable", ex.Message+"；"+ex.StackTrace, "ConventDataTable has an exception");
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
                DataTable dt = Session["yunweitable"] as DataTable;
                int pageIndex = PagingToolbar2.PageIndex;
                DataTable dt1 = dt.Copy(); ;
                if (dt != null)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (("23456").IndexOf(i.ToString()) < 0)
                        {
                            dt1.Columns.Remove("col" + i.ToString());
                        }
                    }
                    dt1.Columns["col5"].SetOrdinal(0);
                    dt1.Columns["col2"].SetOrdinal(1);
                    dt1.Columns["col6"].SetOrdinal(2);
                    dt1.Columns["col3"].SetOrdinal(3);
                    dt1.Columns["col4"].SetOrdinal(4);
                    dt1.Columns[0].ColumnName = GetLangStr("DeviceOperation34", "维护方式");
                    dt1.Columns[1].ColumnName = GetLangStr("DeviceOperation27", "申请人");
                    dt1.Columns[2].ColumnName = GetLangStr("DeviceOperation28", "申请单位");
                    dt1.Columns[3].ColumnName = GetLangStr("DeviceOperation29", "录入时间");
                    dt1.Columns[4].ColumnName = GetLangStr("DeviceOperation30", "维护说明");
                    //PrintColumns pc = new PrintColumns();
                    //pc.Add(new PrintColumn("维护方式", 1));
                    //pc.Add(new PrintColumn("申请人", 2));
                    //pc.Add(new PrintColumn("申请单位", 3));
                    //pc.Add(new PrintColumn("录入时间", 4));
                    //pc.Add(new PrintColumn("维护说明", 5));
                    //dt2 = Bll.Common.GetDataTablePrint(dt, pc);
                }

                return dt1;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-ChangeDataTable", ex.Message+"；"+ex.StackTrace, "ChangeDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            try
            {
                string where = "1=1";

                if (CmbDeviceType.SelectedIndex != -1)
                {
                    where = where + " and  device_type_id='" + CmbDeviceType.SelectedItem.Value + "' ";
                }
                if (!string.IsNullOrEmpty(TxtDeviceName.Text))
                {
                    where = where + " and  device_name  like  '%" + TxtDeviceName.Text.ToUpper() + "%' ";
                }
                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-GetWhere", ex.Message+"；"+ex.StackTrace, "GetWhere has an exception");
                return "";
            }
        }

        /// <summary>
        /// 绑定第一行数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private void SelectFirst(DataRow dr)
        {
            //PagingToolbar2.PageSize = 5;//页容量为5
            //PagingToolbar2.PageIndex = 1;//显示第一页数据
            //string id = "10001";
            //DataTable da = deviceManager.GetOperation(" 1=1 AND  device_id='shebei" + id + "' ");
            //SroPenation.DataSource = da;
            //SroPenation.DataBind();
        }

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        private void GridDataBind(string where)
        {
            try
            {
                PagingToolbar1.PageSize = 5;
                DataTable dt= dtname = deviceManager.GetDeviceByMore(where);
                StoreDeviceManager.DataSource = dt;
                StoreDeviceManager.DataBind();
                Session["datatable"] = dt;
                //DataRow[] dr = dtname.Select("col1='" + id + "'");
                //string sbne = dr[0]["col2"].ToString();
                if (dt.Rows.Count <= 0)
                {
                    ToolExport.Disabled = true;
                    Button2.Disabled = true;
                    return;
                }
                if (Session["yunweitable"] != null)
                {
                    DataTable da = Session["yunweitable"] as DataTable;
                    if (da != null)
                    {
                        if (da.Rows.Count > 0)
                        {
                            SelectFirst(dt.Rows[0]);
                            ToolExport.Disabled = false;
                            Button2.Disabled = false;
                        }
                        else
                        {
                            ToolExport.Disabled = true;
                            Button2.Disabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-GridDataBind", ex.Message+"；"+ex.StackTrace, "GridDataBind has an exception");
            }
        }

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        private void GridDataBind(string where, int pageIndex)
        {
            try
            {
                PagingToolbar1.PageSize = 5;
                PagingToolbar1.PageIndex = pageIndex;
                DataTable dt = deviceManager.GetDeviceByMore(where);
                string ii = dt.Rows[0]["col0"].ToString();
                sbid = ii;
                StoreDeviceManager.DataSource = dt;
                StoreDeviceManager.DataBind();
                Session["datatable"] = dt;
                if (dt.Rows.Count <= 0)
                {
                    ToolExport.Disabled = true;
                    Button2.Disabled = true;
                    return;
                }
                if (Session["yunweitable"] != null)
                {
                    DataTable da = Session["yunweitable"] as DataTable;
                    if (da != null)
                    {
                        if (da.Rows.Count > 0)
                        {
                            SelectFirst(dt.Rows[0]);
                            ToolExport.Disabled = false;
                            Button2.Disabled = false;
                        }
                        else
                        {
                            ToolExport.Disabled = true;
                            Button2.Disabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-GridDataBind", ex.Message+"；"+ex.StackTrace, "GridDataBind has an exception");
            }
        }

        /// <summary>
        /// Qingkongadds this instance.
        /// </summary>
        /// <returns></returns>
        public void qingkongadd()
        {
            com_operationtype.Value = null;
            com_operationdanwei.Value = null;
            txt_lurupeople.Text = null;
            start.InnerText = null;
            txt_shuoming.Text = null;
            txt_shpeople.Text = null;
            end.InnerText = null;
            txt_shyijian.Text = null;
            //time_sh.Text = "0:00";
            //time_luru.Text = "0:00";
        }

        /// <summary>
        /// Qingkongupdates this instance.
        /// </summary>
        /// <returns></returns>
        public void qingkongupdate()
        {
            com_upte_lrtype.Value = null;
            com_upte_lrdanwei.Value = null;
            txt_upde_lrpeople.Text = null;
            //date_upde_lrdate.Text = null;
            txt_upde_lrshuoming.Text = null;
            txt_upde_shpeople.Text = null;
            //date_upde_shdate.Value = null;
            txt_upde_shyijian.Text = null;
            //TimeField1.Text = "0:00";
            //TimeField2.Text = "0:00";
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private void Notice(string title, string msg)
        {
            Notification.Show(new NotificationConfig
            {
                Title = title,
                Icon = Icon.Information,
                HideDelay = 2000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.BottomRight,
                    OffsetY = -60
                },
                Html = "<br></br>" + msg + "!"
            });
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

        protected void Unnamed_Event(object sender, DirectEventArgs e)
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
                logManager.InsertLogError("DeviceOperation.aspx-GetDateTime", ex.Message+"；"+ex.StackTrace, "GetDateTime has an exception");
            }
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="isstart"></param>
        /// <param name="strtime"></param>
        [DirectMethod]
        public void GetDateTime1(bool isstart, string strtime)
        {
            try
            {
                if (isstart)
                    start1time = strtime;
                else
                    end1time = strtime;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceOperation.aspx-GetDateTime", ex.Message, "GetDateTime has an exception");
            }
        }

    }
}