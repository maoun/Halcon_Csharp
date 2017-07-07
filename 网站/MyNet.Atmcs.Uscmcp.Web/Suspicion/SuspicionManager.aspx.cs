using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class SuspicionManager : System.Web.UI.Page
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
        private string funcname = "";
        private static int typenum = 0;

        /// <summary>
        /// 号牌号码2
        /// </summary>
        private static string newhphm = string.Empty;

        /// <summary>
        /// 号牌种类
        /// </summary>
        private static string hl2 = string.Empty;

        /// <summary>
        /// 车身颜色
        /// </summary>
        private static string cs2 = string.Empty;

        /// <summary>
        /// 车辆品牌
        /// </summary>
        private static string cp2 = string.Empty;

        /// <summary>
        /// 布控类型
        /// </summary>
        private static string bx2 = string.Empty;

        /// <summary>
        /// 有效时间
        /// </summary>
        private static string yj2 = string.Empty;

        /// <summary>
        /// 布控原因
        /// </summary>
        private static string by2 = string.Empty;

        /// <summary>
        /// 数据来源
        /// </summary>
        private static string sy2 = string.Empty;

        /// <summary>
        /// 报警接收人
        /// </summary>
        private static string br2 = string.Empty;

        /// <summary>
        /// 布控标识
        /// </summary>
        private static string bs2 = string.Empty;

        /// <summary>
        /// 备注信息
        /// </summary>
        private static string bz2 = string.Empty;

        /// <summary>
        ///  联系电话
        /// </summary>
        private static string lxdh2 = "";

        /// <summary>
        /// 布控联系人
        /// </summary>
        private static string bklxr2 = "";

        /// <summary>
        /// 获取用户名
        /// </summary>
        private static string uName;

        /// <summary>
        /// 获取用户ip
        /// </summary>
        private static string nowIp;

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
                string js = "alert('" + GetLangStr("SuspicionManager67", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
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
                funcname = Request.QueryString["funcname"];
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                uName = userinfo.UserName;
                nowIp = userinfo.NowIp;
                logManager.InsertLogRunning(userinfo.UserName, "" + GetLangStr("SuspicionManager119", "访问：") + "" + funcname, userinfo.NowIp, "0");
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
                SuspicionDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        /// <summary>
        /// 一键布控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButDispatchedClick(object sender, DirectEventArgs e)
        {
            try
            {
                var win = new Window
                {
                    ID = "Window1",
                    Title = GetLangStr("SuspicionManager68", "一键布控"),
                    Width = System.Web.UI.WebControls.Unit.Pixel(2000),
                    Height = System.Web.UI.WebControls.Unit.Pixel(800),
                    Modal = true,
                    Collapsible = true,
                    Maximizable = true,
                    Maximized = true,
                    Hidden = true
                };

                win.AutoLoad.Url = "../Map/Dispatched.aspx";
                win.AutoLoad.Mode = LoadMode.IFrame;
                win.Render(this.Form);
                win.Show();
                //string url = "../Map/Dispatched.aspx";
                //string js = "MenuItemClick('" + url + "');";
                //this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-ButDispatchedClick", ex.Message + "；" + ex.StackTrace, "ButDispatchedClick has an exception");
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
                string where = "1=1";
                //判断号牌号码是否为空，不为空加入到条件中
                if (!string.IsNullOrEmpty(WindowEditor1.VehicleText + TxtplateId.Text.ToUpper()))
                {
                    where = where + " and hphm='" + WindowEditor1.VehicleText + TxtplateId.Text.ToUpper() + "'";
                }
                //判断号牌种类是否为空，不为空加入到条件中
                if (!string.IsNullOrEmpty(CmbPlateType.Text))
                {
                    where = where + " and hpzl='" + CmbPlateType.Text + "'";
                }
                //判断布控类型是否为空，不为空加入到条件中
                if (!string.IsNullOrEmpty(CmbQueryMdlx.Text))
                {
                    where = where + " and  mdlx='" + CmbQueryMdlx.Text + "'";
                }
                SuspicionDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-ButRefreshClick", ex.Message + "；" + ex.StackTrace, "ButRefreshClick has an exception");
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
                TxtplateId.Reset();
                string js = "clearHpjc();";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-ButResetClick", ex.Message + "；" + ex.StackTrace, "ButResetClick has an exception");
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
                    string xml = Bll.Common.GetPrintXml(GetLangStr("SuspicionManager69", "布控车辆查询信息列表"), "", "", "printdatatable");
                    string js = "OpenPrintPageV(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-ButPrintClick", ex.Message + "；" + ex.StackTrace, "ButPrintClick has an exception");
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
                logManager.InsertLogError("SuspicionManager.aspx-ToXml", ex.Message + "；" + ex.StackTrace, "ToXml has an exception");
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
                logManager.InsertLogError("SuspicionManager.aspx-ToExcel", ex.Message + "；" + ex.StackTrace, "ToExcel has an exception");
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
                logManager.InsertLogError("SuspicionManager.aspx-ToCsv", ex.Message + "；" + ex.StackTrace, "ToCsv has an exception");
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
                this.Session["suspicionCount"] = 0;
                string filepath = UploadExcelFile();
                if (string.IsNullOrEmpty(filepath))
                {
                    Notice(GetLangStr("SuspicionManager70", "提示信息"), GetLangStr("SuspicionManager71", "请选择要上传的Excel"));
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
                logManager.InsertLogError("SuspicionManager.aspx-StartLongAction", ex.Message + "；" + ex.StackTrace, "StartLongAction has an exception");
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
                        this.Progress1.UpdateProgress(1, GetLangStr("SuspicionManager72", "完成"));
                        SuspicionDataBind("1=1");
                        indexCount = 1;
                        Notice(GetLangStr("SuspicionManager70", "提示信息"), "" + GetLangStr("SuspicionManager73", "录入成功") + "" + sumCount + "" + GetLangStr("SuspicionManager74", "条") + "");
                    }
                    else
                    {
                        this.Progress1.UpdateProgress(progress / 54f, string.Format("" + GetLangStr("SuspicionManager75", "当前为") + " {0} " + GetLangStr("SuspicionManager76", "总共") + " {1}...", progress.ToString(), sumCount));
                    }
                }

                //if (mydt != null && mydt.Rows.Count > 0)
                //{
                //    int allCount = mydt.Rows.Count;
                //    float count = mydt.Rows.Count;
                //    if (progress != null)
                //    {
                //        SuspicionDataBind("1=1");

                //        this.Progress1.UpdateProgress(((int)progress) / count, string.Format("Step {0} of {1}...", progress.ToString(), allCount));
                //    }
                //    else
                //    {
                //        this.ResourceManager1.AddScript("{0}.stopTask('SaveExcelData');", TaskManager1.ClientID);
                //        this.Progress1.UpdateProgress(1, "完成!");
                //        SuspicionDataBind("1=1");
                //        X.Msg.Alert("提示信息", "总共有" + allCount.ToString() + "条记录，成功添加" + result.ToString() + "条记录！").Show();
                //    }
                //}
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-RefreshProgress", ex.Message + "；" + ex.StackTrace, "RefreshProgress has an exception");
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
            typenum = 1;
            try
            {
                AddSuspicion();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-InfoSave", ex.Message + "；" + ex.StackTrace, "InfoSave has an exception");
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
                string Id = VehicleHead.VehicleText + txtHphm.Text.ToUpper();
                X.Msg.Confirm(GetLangStr("SuspicionManager77", "信息"), GetLangStr("SuspicionManager78", "确认要删除这条记录吗?"), new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "SuspicionManager.DoYes()",
                        Text = GetLangStr("SuspicionManager79", "是")
                    },
                    No = new MessageBoxButtonConfig
                    {
                        Handler = "SuspicionManager.DoNo()",
                        Text = GetLangStr("SuspicionManager80", "否")
                    }
                }).Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-DoConfirm", ex.Message + "；" + ex.StackTrace, "DoConfirm has an exception");
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
                hs.Add("xh", txtID.Text);
                if (tgsDataInfo.DeleteSuspicionInfo(hs) > 0)
                {
                    try
                    {
                        bool flag = client.cancleLayout(txtID.Text);
                        ILog.WriteErrorLog("" + GetLangStr("SuspicionManager81", "撤控返回：") + "" + flag.ToString());
                    }
                    catch (Exception ex)
                    {
                        logManager.InsertLogError("SuspicionManager.aspx-DoYes", ex.Message + "；" + ex.StackTrace, "DoYe" + GetLangStr("SuspicionManager82", "撤控") + " has an exception");
                    }
                    Notice(GetLangStr("SuspicionManager83", "信息提示"), GetLangStr("SuspicionManager84", "删除成功"));
                    SuspicionDataBind("1=1");

                    logManager.InsertLogRunning(uName, "" + GetLangStr("SuspicionManager85", "删除") + ":" + hl2 + ":" + newhphm + "" + GetLangStr("SuspicionManager86", "的布控记录") + "", nowIp, "1");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-DoYes", ex.Message + "；" + ex.StackTrace, "DoYes has an exception");
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
        /// 转换查询模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void changtype(object sender, EventArgs e)
        {
            txtHphm.Hidden = ChkLike.Checked;
            pnhphm.Hidden = !ChkLike.Checked;
        }

        /// <summary>
        /// 更新布控记录
        /// </summary>
        [DirectMethod]
        public void UpdateData()
        {
            try
            {
                Hashtable hs = new Hashtable();
                if (!string.IsNullOrEmpty(txtID.Text))
                    hs.Add("xh", txtID.Text);
                else
                    hs.Add("xh", tgsDataInfo.GetTgsRecordId());
                string cpmh = string.Empty;
                string hphm = string.Empty;
                if (ChkLike.Checked)//判断是否模糊布控
                {
                    if (string.IsNullOrEmpty(VehicleHead.VehicleText.ToString().Trim()) && string.IsNullOrEmpty(haopai_name1.Value) && string.IsNullOrEmpty(haopai_name2.Value)
                        && string.IsNullOrEmpty(haopai_name3.Value) && string.IsNullOrEmpty(haopai_name4.Value) && string.IsNullOrEmpty(haopai_name5.Value)
                        && string.IsNullOrEmpty(haopai_name6.Value))
                    {
                        Notice(GetLangStr("SuspicionManager87", "布控失败"), GetLangStr("SuspicionManager88", "模糊布控需要至少输入一位号牌号码"));
                        return;
                    }
                    else
                    {
                        cpmh = "1";
                        hphm = (string.IsNullOrEmpty(VehicleHead.VehicleText.ToString()) ? "*" : VehicleHead.VehicleText.ToString())
                            + (string.IsNullOrEmpty(haopai_name1.Value) ? "*" : haopai_name1.Value)
                            + (string.IsNullOrEmpty(haopai_name2.Value) ? "*" : haopai_name2.Value)
                            + (string.IsNullOrEmpty(haopai_name3.Value) ? "*" : haopai_name3.Value)
                            + (string.IsNullOrEmpty(haopai_name4.Value) ? "*" : haopai_name4.Value)
                            + (string.IsNullOrEmpty(haopai_name5.Value) ? "*" : haopai_name5.Value)
                            + (string.IsNullOrEmpty(haopai_name6.Value) ? "*" : haopai_name6.Value);
                    }
                }
                else
                {
                    if ((txtHphm.Text.Trim() == "") || (VehicleHead.VehicleText.ToString() == ""))
                    {
                        Notice(GetLangStr("SuspicionManager87", "布控失败"), GetLangStr("SuspicionManager89", "请录入布控号牌号码！"));
                        return;
                    }
                    else
                    {
                        hphm = VehicleHead.VehicleText.ToString() + txtHphm.Text.ToUpper();
                        cpmh = "0";
                    }
                }
                //号牌种类
                if (cmbHpzl.Value == "")
                {
                    Notice(GetLangStr("SuspicionManager87", "布控失败"), GetLangStr("SuspicionManager90", "号牌种类不能为空，请输入"));
                    return;
                }
                if (cmbMdlx.Value == "")
                {
                    Notice(GetLangStr("SuspicionManager87", "布控失败"), GetLangStr("SuspicionManager91", "布控类型不能为空，请输入"));
                    return;
                }
                if (DateYxsj.SelectedDate.ToString("yyyy-MM-dd") == @"0001-01-01")
                {
                    Notice(GetLangStr("SuspicionManager87", "布控失败"), GetLangStr("SuspicionManager92", "有效时间不能为空，请输入"));
                    return;
                }

                if (Txtbkry.Text.Trim() == "")
                {
                    Notice(GetLangStr("SuspicionManager87", "布控失败"), GetLangStr("SuspicionManager93", "请录入布控联系人！"));
                    return;
                }
                if (Txtlxdh.Text.Trim() == "")
                {
                    Notice(GetLangStr("SuspicionManager87", "布控失败"), GetLangStr("SuspicionManager94", "请录入布控联系电话！"));
                    return;
                }
                //号牌号码
                hs.Add("hphm", hphm);
                hs.Add("clpp", txtClpp.Text);
                hs.Add("sjyy", txtSjyy.Text);
                hs.Add("bkr", Txtbkry.Text);
                hs.Add("bkdh", Txtlxdh.Text);
                hs.Add("bz", TxtBz.Text);
                //模糊布控
                hs.Add("cpmh", cpmh);
                //模糊布控
                hs.Add("bklx", "1");
                //有效时间
                if (DateYxsj.SelectedDate != null)
                {
                    hs.Add("yxsj", DateYxsj.SelectedDate.ToString("yyyy-MM-dd")); //+ " " + TimeYxsj.Text);
                }
                //号牌种类
                if (cmbHpzl.SelectedIndex != -1 || cmbHpzl.Value != null)
                {
                    hs.Add("hpzl", cmbHpzl.Value);
                }
                if (cmbCsys.SelectedIndex != -1 || cmbCsys.Value != null)
                {
                    hs.Add("csys", cmbCsys.Value);
                }
                //布控类型
                if (cmbMdlx.SelectedIndex != -1 || cmbMdlx.Value != null)
                {
                    hs.Add("mdlx", cmbMdlx.Value);
                }
                UserInfo user = Session["userinfo"] as UserInfo;
                if (user != null)
                {
                    hs.Add("BKRID", user.UserCode);//布控人编号
                    hs.Add("BKRXM", user.Name);//布控人员姓名
                }
                //比对标记
                if (cmbbdbj.SelectedIndex != -1)
                {
                    hs.Add("bdbj", cmbbdbj.Value);
                }
                string jh = this.FieldPerson.Value.ToString();
                string xm = this.FieldPerson.Text;
                if (!string.IsNullOrEmpty(jh))
                {
                    hs.Add("bjjsr", jh);
                }
                else
                {
                    hs.Add("bjjsr", "");
                }
                if (!string.IsNullOrEmpty(xm))
                {
                    hs.Add("bjjsrms", xm);
                }
                else
                {
                    hs.Add("bjjsrms", "");
                }

                if (tgsDataInfo.UpdateSuspicionInfo(hs) > 0)
                {
                    if (cmbbdbj.Value.Equals("1"))// 布控
                    {
                        try
                        {
                            bool flag = client.layout(txtID.Text, hphm, cmbHpzl.Value.ToString(), "");
                            ILog.WriteErrorLog("" + GetLangStr("SuspicionManager95", "布控返回：") + "" + flag.ToString());
                        }
                        catch (Exception ex)
                        {
                            logManager.InsertLogError("SuspicionManager.aspx-UpdateData", ex.Message + "；" + ex.StackTrace, "UpdateData" + GetLangStr("SuspicionManager96", "布控") + " has an exception");
                        }
                    }
                    else
                    {
                        try
                        {
                            bool flag = client.cancleLayout(txtID.Text);
                            ILog.WriteErrorLog("" + GetLangStr("SuspicionManager97", "撤控返回：") + "" + flag.ToString());
                        }
                        catch (Exception ex)
                        {
                            logManager.InsertLogError("SuspicionManager.aspx-UpdateData", ex.Message + "；" + ex.StackTrace, "UpdateData" + GetLangStr("SuspicionManager98", "撤控") + " has an exception");
                        }
                    }
                    this.ChkLike.Disabled = true;
                    string hm1 = VehicleHead.VehicleText + txtHphm.Text.ToUpper();
                    string cs1 = cmbCsys.SelectedItem.Text;
                    string hl1 = cmbHpzl.SelectedItem.Text;
                    string cp1 = txtClpp.Text;
                    string bx1 = cmbMdlx.SelectedItem.Text;
                    string yj1 = DateYxsj.Text;
                    string by1 = txtSjyy.Text;
                    string br1 = FieldPerson.Text;
                    string bs1 = cmbbdbj.SelectedItem.Text;
                    string bz1 = TxtBz.Text;
                    string logmes = "";
                    if (!string.IsNullOrEmpty(newhphm))
                    {
                        if (!newhphm.Equals(hm1))
                        {
                            logmes += "" + GetLangStr("SuspicionManager1", "号牌号码：") + "" + hm1;
                        }
                    }

                    //当typenum=1代表添加否则代表修改
                    if (typenum == 1)
                    {
                        if (yj2.Equals("0001/1/1 星期一 上午 12:00:00"))
                        {
                            logmes += Bll.Common.AssembleRunLog("", hm1, GetLangStr("SuspicionManager99", "号牌号码"), "0");
                            logmes += Bll.Common.AssembleRunLog("", cmbHpzl.SelectedItem.Text, GetLangStr("SuspicionManager100", "号牌种类"), "0");
                            logmes += Bll.Common.AssembleRunLog("", cmbMdlx.SelectedItem.Text, GetLangStr("SuspicionManager101", "布控类型"), "0");
                            logManager.InsertLogRunning(uName, "" + GetLangStr("SuspicionManager102", "添加布控车辆：") + "" + logmes, nowIp, "1");
                            return;
                        }
                        else
                        {
                            logmes += Bll.Common.AssembleRunLog("", DateYxsj.Text, "" + GetLangStr("SuspicionManager26", "有效时间") + "", "0");
                            logmes += Bll.Common.AssembleRunLog("", hm1, "" + GetLangStr("SuspicionManager99", "号牌号码") + "", "0");
                            logmes += Bll.Common.AssembleRunLog("", cmbHpzl.SelectedItem.Text, "" + GetLangStr("SuspicionManager100", "号牌种类") + "", "0");
                            logmes += Bll.Common.AssembleRunLog("", cmbMdlx.SelectedItem.Text, "" + GetLangStr("SuspicionManager101", "布控类型") + " ", "0");
                            logManager.InsertLogRunning(uName, "" + GetLangStr("SuspicionManager102", "添加布控车辆：") + "" + logmes, nowIp, "1");
                        }

                        cs2 = cmbCsys.SelectedItem.Text;
                        hl2 = cmbHpzl.SelectedItem.Text;
                        cp2 = txtClpp.Text;
                        bx2 = cmbMdlx.SelectedItem.Text;
                        yj2 = DateYxsj.Text;
                        by2 = txtSjyy.Text;
                        //sy2 = uiDepartment.DepertName;
                        br2 = FieldPerson.Text;
                        bs2 = cmbbdbj.SelectedItem.Text;
                        bz2 = TxtBz.Text;
                        typenum = 0;
                    }
                    else
                    {
                        logmes += Bll.Common.AssembleRunLog(newhphm, hm1, GetLangStr("SuspicionManager99", "号牌号码"), "1");
                        logmes += Bll.Common.AssembleRunLog(hl2, hl1, GetLangStr("SuspicionManager100", "号牌种类"), "1");
                        logmes += Bll.Common.AssembleRunLog(cs2, cs1, GetLangStr("SuspicionManager39", "车身颜色"), "1");
                        logmes += Bll.Common.AssembleRunLog(cp2, cp1, GetLangStr("SuspicionManager41", "车辆品牌"), "1");
                        logmes += Bll.Common.AssembleRunLog(bx2, bx1, GetLangStr("SuspicionManager101", "布控类型"), "1");
                        logmes += Bll.Common.AssembleRunLog(yj2, yj1, GetLangStr("SuspicionManager45", "有效时间"), "1");
                        logmes += Bll.Common.AssembleRunLog(by2, by1, GetLangStr("SuspicionManager103", "布控原因"), "1");
                        //logmes += Bll.Common.AssembleRunLog(sy2, sy1, "数据来源", "1");
                        logmes += Bll.Common.AssembleRunLog(br2, br1, GetLangStr("SuspicionManager47", "报警接收人"), "1");
                        logmes += Bll.Common.AssembleRunLog(bs2, bs1, GetLangStr("SuspicionManager50", "布控标识"), "1");
                        logmes += Bll.Common.AssembleRunLog(bz2, bz1, GetLangStr("SuspicionManager54", "备注信息"), "1");
                        logManager.InsertLogRunning(uName, "" + GetLangStr("SuspicionManager104", "修改：") + "" + "" + GetLangStr("SuspicionManager105", "号牌种类为：") + "[" + hl1 + "]" + "" + GetLangStr("SuspicionManager106", "号牌号码为：") + "[" + hm1 + "]" + logmes, nowIp, "2");
                    }
                    Notice(GetLangStr("SuspicionManager107", "布控信息提示"), GetLangStr("SuspicionManager108", "保存成功"));
                    string where = "1=1";
                    //判断号牌号码是否为空，不为空加入到条件中
                    if (!string.IsNullOrEmpty(WindowEditor1.VehicleText + TxtplateId.Text.ToUpper()))
                    {
                        where = where + " and hphm='" + WindowEditor1.VehicleText + TxtplateId.Text.ToUpper() + "'";
                    }
                    //判断号牌种类是否为空，不为空加入到条件中
                    if (!string.IsNullOrEmpty(CmbPlateType.Text))
                    {
                        where = where + " and hpzl='" + CmbPlateType.Text + "'";
                    }
                    //判断布控类型是否为空，不为空加入到条件中
                    if (!string.IsNullOrEmpty(CmbQueryMdlx.Text))
                    {
                        where = where + " and  mdlx='" + CmbQueryMdlx.Text + "'";
                    }
                    // SuspicionDataBind(where);
                    SuspicionDataBind(GetWhere());
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-UpdateData", ex.Message + "；" + ex.StackTrace, "UpdateData has an exception");
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
                string fileName = GetLangStr("SuspicionManager109", "布控车辆批量导入模版.xls");
                Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                string filename = Server.MapPath("../Export/ImportTemplateDownload.xls");//下载模板的物理路劲
                Response.TransmitFile(filename);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-Download", ex.Message + "；" + ex.StackTrace, "Download has an exception");
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
                DataTable dt1 = GetRedisData.GetData("t_sys_code:140001");
                if (dt1 != null)
                {
                    //车俩类型
                    this.StorePlateType.DataSource = MyNet.Atmcs.Uscmcp.Bll.Common.ChangColName(dt1);
                    this.StorePlateType.DataBind();
                }
                else
                {
                    this.StorePlateType.DataSource = tgsPproperty.GetPalteType();
                    this.StorePlateType.DataBind();
                }
                DataTable dt2 = GetRedisData.GetData("t_sys_code:420700");
                if (dt2 != null)
                {
                    //比对类型
                    this.StoreMdlx.DataSource = MyNet.Atmcs.Uscmcp.Bll.Common.ChangColName(dt2);
                    this.StoreMdlx.DataBind();
                }
                else
                {
                    this.StoreMdlx.DataSource = tgsPproperty.GetSuspicionDict();
                    this.StoreMdlx.DataBind();
                }
                DataTable dt3 = GetRedisData.GetData("t_sys_code:240013");
                if (dt3 != null)
                {
                    //车辆颜色
                    this.StoreColor.DataSource = MyNet.Atmcs.Uscmcp.Bll.Common.ChangColName(dt3);
                    this.StoreColor.DataBind();
                }
                else
                {
                    this.StoreColor.DataSource = tgsPproperty.GetCarColorDict();
                    this.StoreColor.DataBind();
                }
                DataTable dt4 = GetRedisData.GetData("t_sys_code:240011");
                if (dt4 != null)
                {
                    //比对标志
                    this.StoreBdbj.DataSource = MyNet.Atmcs.Uscmcp.Bll.Common.ChangColName(dt4);
                    this.StoreBdbj.DataBind();
                }
                else
                {
                    this.StoreBdbj.DataSource = tgsPproperty.GetIsSuspicionDict();
                    this.StoreBdbj.DataBind();
                }
                hl2 = cmbHpzl.SelectedItem.Text;
                cs2 = cmbCsys.SelectedItem.Text;
                cp2 = txtClpp.Text;
                bx2 = cmbMdlx.SelectedItem.Text;
                yj2 = DateYxsj.Text;
                by2 = txtSjyy.Text;
                if (FieldPerson.Value != null)
                    br2 = FieldPerson.Value.ToString();
                bs2 = cmbbdbj.SelectedItem.Text;
                bklxr2 = Txtbkry.Text;
                lxdh2 = Txtlxdh.Text;
                bz2 = TxtBz.Text;
                SuspicionDataBind("1=1");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 绑定布控数据
        /// </summary>
        /// <param name="where"></param>
        private void SuspicionDataBind(string where)
        {
            try
            {
                GetData();
                //DataTable dt = tgsDataInfo.GetBalckList(where);
                //StoreSuspicion.DataSource = dt;
                //StoreSuspicion.DataBind();
                //Session["datatable"] = dt;
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    SelectFirst(dt.Rows[0]);
                //    // ButCsv.Disabled = false;
                //   //ButExcel.Disabled = false;
                //  //ButXml.Disabled = false;
                // //ButPrint.Disabled = false;
                //}
                //else
                //{
                //    //ButCsv.Disabled = true;
                //  //  ButExcel.Disabled = true;
                //    //ButXml.Disabled = true;
                //    //ButPrint.Disabled = true;
                //    Notice("提示", "未查询到相关记录!");
                //}
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-SuspicionDataBind", ex.Message + "；" + ex.StackTrace, "SuspicionDataBind has an exception");
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
                    dt1.Columns.Remove("col0"); dt1.Columns.Remove("col2"); dt1.Columns.Remove("col6"); dt1.Columns.Remove("col10");
                    dt1.Columns.Remove("col12"); dt1.Columns.Remove("col16"); dt1.Columns.Remove("col18");
                    dt1.Columns.Remove("col19"); dt1.Columns.Remove("col20"); dt1.Columns.Remove("col21"); dt1.Columns.Remove("col22");
                    dt1.Columns["col9"].SetOrdinal(5);
                    dt1.Columns["col11"].SetOrdinal(8);
                    dt1.Columns[0].ColumnName = GetLangStr("SuspicionManager99", "号牌号码");
                    dt1.Columns[1].ColumnName = GetLangStr("SuspicionManager100", "号牌种类");
                    dt1.Columns[2].ColumnName = GetLangStr("SuspicionManager39", "车身颜色");
                    dt1.Columns[3].ColumnName = GetLangStr("SuspicionManager41", "车辆品牌");
                    dt1.Columns[4].ColumnName = GetLangStr("SuspicionManager42", "布控类型");
                    dt1.Columns[5].ColumnName = GetLangStr("SuspicionManager110", "布控描述");
                    dt1.Columns[6].ColumnName = GetLangStr("SuspicionManager45", "有效时间");
                    dt1.Columns[7].ColumnName = GetLangStr("SuspicionManager50", "布控标识");
                    dt1.Columns[8].ColumnName = GetLangStr("SuspicionManager111", "布控人");
                    dt1.Columns[9].ColumnName = GetLangStr("SuspicionManager27", "布控联系人");
                    dt1.Columns[10].ColumnName = GetLangStr("SuspicionManager28", "联系电话");
                    dt1.Columns[11].ColumnName = GetLangStr("SuspicionManager30", "更新时间");
                    return dt1;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-ChangeDataTable", ex.Message + "；" + ex.StackTrace, "ChangeDataTable has an exception");
            }
            return dt;
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            string where = "1=1 AND BKLX = '1' ";
            try
            {
                if (CmbPlateType.SelectedIndex != -1)
                {
                    where = where + " and  hpzl='" + CmbPlateType.SelectedItem.Value + "' ";
                }
                if (CmbQueryMdlx.SelectedIndex != -1)
                {
                    where = where + " and  mdlx='" + CmbQueryMdlx.SelectedItem.Value + "' ";
                }
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
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-GetWhere", ex.Message + "；" + ex.StackTrace, "GetWhere has an exception");
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
                string where = "1=1";
                //判断号牌号码是否为空，不为空加入到条件中
                if (!string.IsNullOrEmpty(WindowEditor1.VehicleText + TxtplateId.Text.ToUpper()))
                {
                    where = where + " and hphm='" + WindowEditor1.VehicleText + TxtplateId.Text.ToUpper() + "'";
                }
                //判断号牌种类是否为空，不为空加入到条件中
                if (!string.IsNullOrEmpty(CmbPlateType.Text))
                {
                    where = where + " and hpzl='" + CmbPlateType.Text + "'";
                }
                //判断布控类型是否为空，不为空加入到条件中
                if (!string.IsNullOrEmpty(CmbQueryMdlx.Text))
                {
                    where = where + " and  mdlx='" + CmbQueryMdlx.Text + "'";
                }
                SuspicionDataBind(where);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh has an exception");
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
                txtID.Text = dr["col0"].ToString();
                VehicleHead.SetVehicleText(dr["col1"].ToString().Substring(0, 1));
                txtHphm.Text = dr["col1"].ToString().Substring(1);
                cmbHpzl.Value = dr["col2"].ToString();
                cmbHpzl.SelectedItem.Text = dr["col3"].ToString();
                cmbCsys.SelectedItem.Text = dr["col4"].ToString();
                txtClpp.Text = dr["col5"].ToString();
                cmbMdlx.Value = dr["col6"].ToString();
                cmbMdlx.SelectedItem.Text = dr["col7"].ToString();
                DateYxsj.Text = GetDate(dr["col8"].ToString(), 1);
                // TimeYxsj.Text = GetDate(dr["col8"].ToString(), 2);
                txtSjyy.Text = dr["col9"].ToString();
                cmbbdbj.SelectedItem.Text = dr["col13"].ToString();
                TxtBz.Text = dr["col16"].ToString();

                Txtbkry.Text = dr["col14"].ToString();
                Txtlxdh.Text = dr["col15"].ToString();
                TxtGxsj.Text = dr["col17"].ToString();
                string xm = dr["col19"].ToString();
                string jh = dr["col18"].ToString();
                if (!string.IsNullOrEmpty(jh) && !string.IsNullOrEmpty(xm))
                {
                    this.FieldPerson.SetValue(jh, xm);
                }
                else
                {
                    UserInfo user = Session["userinfo"] as UserInfo;
                    this.FieldPerson.SetValue(user.UserCode, user.Name);
                }

                //是否模糊布控
                string mhbk = dr["col20"].ToString();
                this.ChkLike.Disabled = true;
                if (!string.IsNullOrEmpty(mhbk) && (mhbk == "1"))
                {
                    this.ChkLike.Checked = true;
                }
                else
                {
                    this.ChkLike.Checked = false;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-SelectFirst", ex.Message + "；" + ex.StackTrace, "SelectFirst has an exception");
            }
        }

        /// <summary>
        /// 添加布控记录
        /// </summary>
        private void AddSuspicion()
        {
            try
            {
                txtID.Text = tgsDataInfo.GetTgsRecordId();
                VehicleHead.SetVehicleText("");
                txtHphm.Text = "";
                txtHphm.Value = null;
                cmbHpzl.SelectedItem.Text = "";
                cmbHpzl.Text = "";
                //cmbHpzl.Value = null;
                cmbCsys.SelectedItem.Text = "";
                cmbCsys.Text = "";
                txtClpp.Text = "";
                //cmbMdlx.SelectedItem.Text = "";
                cmbMdlx.Text = "";
                cmbMdlx.Value = null;
                DateYxsj.Text = "";
                //TimeYxsj.Text = "23:59";
                txtSjyy.Text = "";
                Txtbkry.Text = "";//布控联系人
                Txtlxdh.Text = "";//布控人电话
                cmbbdbj.SelectedItem.Value = "1";
                this.ChkLike.Checked = false;
                this.ChkLike.Disabled = false;
                //cmbbdbj.SelectedItem.Text = "";
                TxtBz.Text = "";
                this.FieldPerson.Text = "";
                this.TxtGxsj.Text = "";
                //注释下面的话不会由当前用户的默认设置
                UserInfo user = Session["userinfo"] as UserInfo;
                this.FieldPerson.SetValue(user.UserCode, user.Name);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("" + GetLangStr("SuspicionManager112", "车辆布控") + "", ex.Message + "；" + ex.StackTrace, "AddSuspicion has an exception");
            }
        }

        /// <summary>
        /// 选择布控记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectSuspicion(object sender, DirectEventArgs e)
        {
            try
            {
                object data = e.ExtraParams["sdata"];
                string sdata = data.ToString();
                txtID.Text = GetdatabyField(sdata, "col0");
                string Hphm = GetdatabyField(sdata, "col1");
                VehicleHead.SetVehicleText(Hphm.Substring(0, 1));
                txtHphm.Text = Hphm.Substring(1);
                cmbHpzl.SelectedItem.Text = GetdatabyField(sdata, "col3");
                cmbHpzl.Value = GetdatabyField(sdata, "col2");
                cmbCsys.SelectedItem.Text = GetdatabyField(sdata, "col4");
                cmbCsys.Value = GetdatabyField(sdata, "col22");
                txtClpp.Text = GetdatabyField(sdata, "col5");
                cmbMdlx.Value = GetdatabyField(sdata, "col6");
                cmbMdlx.SelectedItem.Text = GetdatabyField(sdata, "col7");
                DateYxsj.Text = GetDate(sdata, "col8", 1);
                //    TimeYxsj.Text = GetDate(sdata, "col8", 2);
                txtSjyy.Text = GetdatabyField(sdata, "col9");
                //uiDepartment.DepertName = GetdatabyField(sdata, "col11");
                cmbbdbj.Value = GetdatabyField(sdata, "col12");
                cmbbdbj.SelectedItem.Text = GetdatabyField(sdata, "col13");
                TxtBz.Text = GetdatabyField(sdata, "col16");
                Txtbkry.Text = GetdatabyField(sdata, "col14");
                Txtlxdh.Text = GetdatabyField(sdata, "col15");
                TxtGxsj.Text = GetDate(GetdatabyField(sdata, "col17"), 0);
                string jh = GetdatabyField(sdata, "col18");
                string xm = GetdatabyField(sdata, "col19");
                if (!string.IsNullOrEmpty(jh) && !string.IsNullOrEmpty(xm))
                {
                    this.FieldPerson.SetValue(jh, xm);
                }
                else
                {
                    UserInfo user = Session["userinfo"] as UserInfo;
                    this.FieldPerson.SetValue(user.UserCode, user.Name);
                }
                //是否模糊布控
                string mhbk = GetdatabyField(sdata, "col20");
                if (!string.IsNullOrEmpty(mhbk) && (mhbk == "1"))
                {
                    this.ChkLike.Checked = true;
                }
                else
                {
                    this.ChkLike.Checked = false;
                }

                //定义变量删除日志获取值

                #region 定义变量删除日志获取值

                newhphm = Hphm;
                hl2 = cmbHpzl.SelectedItem.Text;
                cs2 = cmbCsys.SelectedItem.Text;
                cp2 = txtClpp.Text;
                bx2 = cmbMdlx.SelectedItem.Text;
                yj2 = DateYxsj.Text;
                by2 = txtSjyy.Text;
                br2 = FieldPerson.Value.ToString();
                bs2 = cmbbdbj.SelectedItem.Text;
                bklxr2 = Txtbkry.Text;
                lxdh2 = Txtlxdh.Text;
                bz2 = TxtBz.Text;
                //br2 = FieldPerson.Text;

                #endregion 定义变量删除日志获取值
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-SelectSuspicion", ex.Message + "；" + ex.StackTrace, "SelectSuspicion has an exception");
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
                logManager.InsertLogError("SuspicionManager.aspx-GetDate", ex.Message + "；" + ex.StackTrace, "GetDate has an exception");
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
                logManager.InsertLogError("SuspicionManager.aspx-GetDate", ex.Message + "；" + ex.StackTrace, "GetDate has an exception");
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
                logManager.InsertLogError("SuspicionManager.aspx-GetdatabyField", ex.Message + "；" + ex.StackTrace, "GetdatabyField has an exception");
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
                logManager.InsertLogError("SuspicionManager.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice has an exception");
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
                        hs.Add("xh", tgsDataInfo.GetTgsRecordId());
                        hs.Add("hphm", mydt.Rows[i]["" + GetLangStr("SuspicionManager99", "号牌号码") + ""].ToString());
                        hs.Add("hpzl", Bll.Common.GetHpzl(mydt.Rows[i]["" + GetLangStr("SuspicionManager100", "号牌号码") + ""].ToString()));
                        hs.Add("clpp", mydt.Rows[i]["" + GetLangStr("SuspicionManager23", "车辆品牌") + ""].ToString());
                        hs.Add("csys", Bll.Common.GetCsys(mydt.Rows[i]["" + GetLangStr("SuspicionManager22", "车身颜色") + ""].ToString()));
                        hs.Add("mdlx", mydt.Rows[i]["" + GetLangStr("SuspicionManager24", "布控类型") + ""].ToString());//Bll.Common.GetMdlx(mydt.Rows[i]["布控类型"].ToString())
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
                        if (DateYxsj.SelectedDate != null)
                        {
                            hs.Add("yxsj", mydt.Rows[i]["" + GetLangStr("SuspicionManager26", "有效时间") + ""].ToString());
                        }
                        hs.Add("bjjsr", user.UserCode);
                        hs.Add("bjjsrms", user.Name);
                        hs.Add("bz", mydt.Rows[i]["" + GetLangStr("SuspicionManager113", "备注") + ""].ToString());
                        //没用的列
                        hs.Add("sjyy", "");
                        hs.Add("bdbj", "1");
                        if (tgsDataInfo.UpdateSuspicionInfo(hs) > 0)
                        {
                            client.layout(txtID.Text, VehicleHead.VehicleText + txtHphm.Text.ToUpper(), cmbHpzl.Value.ToString(), "");
                        }
                        // Session["suspicionCount"] = i + 1;
                        indexCount = i + 1;
                    }
                    catch (Exception ex)
                    {
                        ILog.WriteErrorLog(ex);
                        logManager.InsertLogError("SuspicionManager.aspx-BatchUpdateData", ex.Message + "；" + ex.StackTrace, "BatchUpdateData has an exception");
                    }
                }
                // Session["suspicionResult"] = count;
                //this.Session.Remove("suspicionCount");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SuspicionManager.aspx-BatchUpdateData", ex.Message + "；" + ex.StackTrace, "BatchUpdateData has an exception");
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
                        X.Msg.Alert(GetLangStr("SuspicionManager70", "提示信息"), GetLangStr("SuspicionManager114", "上传文件过大！")).Show();
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
                            X.Msg.Alert(GetLangStr("SuspicionManager70", "提示信息"), GetLangStr("SuspicionManager115", "没有找到需要导入的SHEET1$页！")).Show();
                            return "";
                        }
                    }
                    else
                    {
                        X.Msg.Alert(GetLangStr("SuspicionManager70", "提示信息"), GetLangStr("SuspicionManager116", "文件格式不正确！")).Show();
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
                logManager.InsertLogError("SuspicionManager.aspx-UploadExcelFile", ex.Message + "；" + ex.StackTrace, "UploadExcelFile has an exception");
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
                root.Text = GetLangStr("SuspicionManager117", "人员列表");
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
                logManager.InsertLogError("SuspicionManager.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
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
                logManager.InsertLogError("SuspicionManager.aspx-AddDepartTree", ex.Message + "；" + ex.StackTrace, "AddDepartTree has an exception");
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
                logManager.InsertLogError("SuspicionManager.aspx-AddPersonTree", ex.Message + "；" + ex.StackTrace, "AddPersonTree has an exception");
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
            DataTable dt = tgsDataInfo.GetBalckList(where, currentPage);
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
                this.StoreSuspicion.DataSource = dt;
                this.StoreSuspicion.DataBind();
            }
            else
            {
                this.lblCurpage.Text = "1";
                this.lblAllpage.Text = "0";
                this.lblRealcount.Text = "0";
                this.StoreSuspicion.DataSource = dt;
                this.StoreSuspicion.DataBind();
                Notice(GetLangStr("SuspicionManager83", "信息提示"), GetLangStr("SuspicionManager118", "当前没数据"));
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
                DataTable dt = tgsDataInfo.GetBalckListCount(where);

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