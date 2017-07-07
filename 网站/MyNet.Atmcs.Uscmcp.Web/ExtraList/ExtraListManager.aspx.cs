// ***********************************************************************
// Assembly         : MyNet.Atmcs.Uscmcp.Web
// Author           : zlsyl
// Created          : 06-24-2016
//
// Last Modified By : zlsyl
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="ExtraListManager.aspx.cs" company="ZKLT">
//     Copyright (c) ZKLT. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;

/// <summary>
/// The Web namespace.
/// </summary>
namespace MyNet.Atmcs.Uscmcp.Web
{
    /// <summary>
    /// Class ExtraListManager.
    /// </summary>
    public partial class ExtraListManager : System.Web.UI.Page
    {
        /// <summary>
        /// The TGS pproperty
        /// </summary>
        private TgsPproperty tgsPproperty = new TgsPproperty();

        /// <summary>
        /// The TGS data information
        /// </summary>
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        /// <summary>
        /// The user login
        /// </summary>
        private UserLogin userLogin = new UserLogin();

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
                    ButCsv.Disabled = true;
                    ButExcel.Disabled = true;
                    ButXml.Disabled = true;
                    ButPrint.Disabled = true;
                    StoreDataBind();
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：用户登录", userinfo.NowIp, "0");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExtraListManager.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
            }
        }

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        /// <returns></returns>
        private void StoreDataBind()
        {
            try
            {
                this.StorePlateType.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:140001"));//tgsPproperty.GetPalteType();
                this.StorePlateType.DataBind();
                this.StoreMdlx.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240017"));//tgsPproperty.GetExtraListDict();
                this.StoreMdlx.DataBind();
                this.StoreColor.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240013"));//tgsPproperty.GetCarColorDict();
                this.StoreColor.DataBind();
                this.StoreBdbj.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240010"));// tgsPproperty.GetIsCompareDict();
                this.StoreBdbj.DataBind();
                ExtraListDataBind("1=1");
                TxtGxsj.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch(Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExtraListManager.aspx-StoreDataBind", ex.Message+"；"+ex.StackTrace, "StoreDataBind  has an exception");
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
            ExtraListDataBind(GetWhere());
        }

        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void ButRefreshClick(object sender, DirectEventArgs e)
        {
            ExtraListDataBind("1=1");
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
                CmbPlateType.Reset();
                CmbQueryMdlx.Reset();
                TxtplateId.Reset();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExtraListManager.aspx-ButResetClick", ex.Message + "；" + ex.StackTrace, "ButResetClick 发生异常");
                throw;
            }
        }

        /// <summary>
        /// Extras the list data bind.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        private void ExtraListDataBind(string where)
        {
            try
            {
                DataTable dt = tgsDataInfo.GetExtraList(where);
                StoreExtraList.DataSource = dt;
                StoreExtraList.DataBind();
                Session["datatable"] = dt;
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        SelectFirst(dt.Rows[0]);
                        ButCsv.Disabled = false;
                        ButExcel.Disabled = false;
                        ButXml.Disabled = false;
                        ButPrint.Disabled = false;
                    }
                    else
                    {
                        ButCsv.Disabled = true;
                        ButExcel.Disabled = true;
                        ButXml.Disabled = true;
                        ButPrint.Disabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExtraListManager.aspx-ExtraListDataBind", ex.Message + "；" + ex.StackTrace, "ExtraListDataBind 发生异常");
            }
        }

        /// <summary>
        /// 转换datatable
        /// </summary>
        /// <returns></returns>
        private DataTable ChangeDataTable()
        {
            DataTable dt = Session["datatable"] as DataTable;
            DataTable dt2 = null; ;
            if (dt != null)
            {
                //PrintColumns pc = new PrintColumns();
                //pc.Add(new PrintColumn("号牌号码", 1));
                //pc.Add(new PrintColumn("号牌种类", 3));
                //pc.Add(new PrintColumn("车身颜色", 4));
                //pc.Add(new PrintColumn("车辆品牌", 5));
                //pc.Add(new PrintColumn("有效时间", 8));
                //pc.Add(new PrintColumn("比对类型", 7));
                //pc.Add(new PrintColumn("比对标识", 13));
                //dt2 = Bll.Common.GetDataTablePrint(dt, pc);
            }

            return dt2;
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

                if (CmbPlateType.SelectedIndex != -1)
                {
                    where = where + " and  hpzl='" + CmbPlateType.SelectedItem.Value + "' ";
                }
                if (CmbQueryMdlx.SelectedIndex != -1)
                {
                    where = where + " and  mdlx='" + CmbQueryMdlx.SelectedItem.Value + "' ";
                }
                if (!string.IsNullOrEmpty(TxtplateId.Text))
                {
                    where = where + " and  hphm='" + TxtplateId.Text.ToUpper() + "' ";
                }
                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExtraListManager.aspx-GetWhere", ex.Message + "；" + ex.StackTrace, "GetWhere发生异常");
                throw;
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
            ExtraListDataBind("1=1");
        }

        /// <summary>
        /// 绑定第一行数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private void SelectFirst(DataRow dr)
        {
            try
            {
                TxtID.Text = dr["col0"].ToString();
                txtHphm.Text = dr["col1"].ToString();
                cmbHpzl.SelectedItem.Text = dr["col3"].ToString();
                cmbCsys.SelectedItem.Text = dr["col4"].ToString();
                txtClpp.Text = dr["col5"].ToString();
                cmbMdlx.SelectedItem.Text = dr["col7"].ToString();
                DateYxsj.Text = GetDate(dr["col8"].ToString(), 1);
                TimeYxsj.Text = GetDate(dr["col8"].ToString(), 2);
                txtSjyy.Text = dr["col9"].ToString();
                uiDepartment.DepertName = dr["col11"].ToString();
                cmbbdbj.SelectedItem.Text = dr["col13"].ToString();
                TxtBz.Text = dr["col14"].ToString();
                TxtGxsj.Text = dr["col15"].ToString();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExtraListManager.aspx-SelectFirst", ex.Message + "；" + ex.StackTrace, "SelectFirst发生异常");
                throw;
            }
        }

        /// <summary>
        /// Adds the extra list.
        /// </summary>
        /// <returns></returns>
        private void AddExtraList()
        {
            try
            {
                TxtID.Text = tgsDataInfo.GetTgsRecordId();
                txtHphm.Text = "";
                cmbHpzl.SelectedItem.Text = "";
                cmbCsys.SelectedItem.Text = "";
                txtClpp.Text = "";
                cmbMdlx.SelectedItem.Text = "";
                DateYxsj.Text = "";
                TimeYxsj.Text = "23:59";
                txtSjyy.Text = "";
                uiDepartment.DepertName = "";
                cmbbdbj.SelectedItem.Text = "";
                TxtBz.Text = "";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExtraListManager.aspx-AddExtraList", ex.Message + "；" + ex.StackTrace, "AddExtraList发生异常");
                throw;
            }
        }

        /// <summary>
        /// Selects the extra list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void SelectExtraList(object sender, DirectEventArgs e)
        {
            try
            {
                object data = e.ExtraParams["sdata"];
                string sdata = data.ToString();
                TxtID.Text = GetdatabyField(sdata, "col0");
                txtHphm.Text = GetdatabyField(sdata, "col1");
                cmbHpzl.Value = GetdatabyField(sdata, "col2");
                cmbHpzl.SelectedItem.Text = GetdatabyField(sdata, "col3");
                cmbCsys.SelectedItem.Text = GetdatabyField(sdata, "col4");
                txtClpp.Text = GetdatabyField(sdata, "col5");
                cmbMdlx.Value = GetdatabyField(sdata, "col6");
                cmbMdlx.SelectedItem.Text = GetdatabyField(sdata, "col7");
                DateYxsj.Text = GetDate(sdata, "col8", 1);
                TimeYxsj.Text = GetDate(sdata, "col8", 2);
                txtSjyy.Text = GetdatabyField(sdata, "col9");
                uiDepartment.DepertName = GetdatabyField(sdata, "col11");
                cmbbdbj.Value = GetdatabyField(sdata, "col12");
                cmbbdbj.SelectedItem.Text = GetdatabyField(sdata, "col13");
                TxtBz.Text = GetdatabyField(sdata, "col14");
                TxtGxsj.Text = GetDate(GetdatabyField(sdata, "col15"), 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExtraListManager.aspx-SelectExtraList", ex.Message + "；" + ex.StackTrace, "SelectExtraList发生异常");
                throw;
            }
        }

        /// <summary>
        /// Gets the date.
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
                logManager.InsertLogError("ExtraListManager.aspx-GetDate", ex.Message+"；"+ex.StackTrace, "GetDate  has an exception");
                return "";
            }
        }

        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="field"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private string GetDate(string data, string field, int flag)
        {
            string s = GetdatabyField(data, field);
            return GetDate(s, flag);
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
                logManager.InsertLogError("ExtraListManager.aspx-GetdatabyField", ex.Message + "；" + ex.StackTrace, "GetdatabyField发生异常");
                return null;
            }
        }

        /// <summary>
        /// Informations the save.
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void InfoSave()
        {
            AddExtraList();
        }

        /// <summary>
        /// 删除确认事件
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void DoConfirm()
        {
            try
            {
                string Id = txtHphm.Text;
                X.Msg.Confirm("信息", "确认要删除这条记录吗?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "ExtraListManager.DoYes()",
                        Text = "是"
                    },
                    No = new MessageBoxButtonConfig
                    {
                        Handler = "ExtraListManager.DoNo()",
                        Text = "否"
                    }
                }).Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExtraListManager.aspx-DoConfirm", ex.Message + "；" + ex.StackTrace, "DoConfirm发生异常");
                throw;
            }
        }

        /// <summary>
        /// 确定事件
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void DoYes()
        {
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("xh", TxtID.Text);
                if (tgsDataInfo.DeleteExtraListInfo(hs) > 0)
                {
                    Notice("信息提示", "删除成功");
                    ExtraListDataBind("1=1");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExtraListManager.aspx-DoYes", ex.Message + "；" + ex.StackTrace, "DoYes发生异常");
                throw;
            }
        }

        /// <summary>
        /// 不删除事件
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void DoNo()
        {
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void UpdateData()
        {
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("xh", TxtID.Text);
                hs.Add("hphm", txtHphm.Text);
                hs.Add("clpp", txtClpp.Text);
                hs.Add("sjyy", txtSjyy.Text);
                hs.Add("bz", TxtBz.Text);
                if (DateYxsj.SelectedDate != null)
                {
                    hs.Add("yxsj", DateYxsj.SelectedDate.ToString("yyyy-MM-dd") + " " + TimeYxsj.Text);
                }
                if (cmbHpzl.SelectedIndex != -1)
                {
                    hs.Add("hpzl", cmbHpzl.Value);
                }
                if (cmbCsys.SelectedIndex != -1)
                {
                    hs.Add("csys", cmbCsys.Value);
                }
                if (cmbMdlx.SelectedIndex != -1)
                {
                    hs.Add("mdlx", cmbMdlx.Value);
                }
                if (!string.IsNullOrEmpty(uiDepartment.DepertId))
                {
                    hs.Add("sjly", uiDepartment.DepertId);
                }
                if (cmbbdbj.SelectedIndex != -1)
                {
                    hs.Add("bdbj", cmbbdbj.Value);
                }
                if (tgsDataInfo.UpdateExtraListInfo(hs) > 0)
                {
                    Notice("比对信息提示", "保存成功");
                    ExtraListDataBind("1=1");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExtraListManager.aspx-UpdateData", ex.Message + "；" + ex.StackTrace, "UpdateData发生异常");
                throw;
            }
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
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
                logManager.InsertLogError("ExtraListManager.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice发生异常");
                throw;
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
                    string xml = Bll.Common.GetPrintXml("重点车辆查询信息列表", "", "", "printdatatable");
                    string js = "OpenPrintPageV(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExtraListManager.aspx-ButPrintClick", ex.Message + "；" + ex.StackTrace, "ButPrintClick发生异常");
                throw;
            }
        }

        /// <summary>
        /// 导出为xml
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
                logManager.InsertLogError("ExtraListManager.aspx-ToXml", ex.Message + "；" + ex.StackTrace, "ToXml发生异常");
                throw;
            }
        }

        /// <summary>
        /// 导出为excel
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
                logManager.InsertLogError("ExtraListManager.aspx-ToExcel", ex.Message + "；" + ex.StackTrace, "ToExcel发生异常");
                throw;
            }
        }

        /// <summary>
        /// 导出为csv
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
                logManager.InsertLogError("ExtraListManager.aspx-ToCsv", ex.Message + "；" + ex.StackTrace, "ToCsv发生异常");
                throw;
            }
        }
    }
}