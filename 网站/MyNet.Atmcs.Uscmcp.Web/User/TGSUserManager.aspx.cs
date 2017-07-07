using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class TGSUserManager : System.Web.UI.Page
    {
        #region 成员变量

        private TgsPpropertyNew tgsPproperty = new TgsPpropertyNew();
        private Bll.UserManagerNew userManager = new Bll.UserManagerNew();
        private Bll.SettingManagerNew settingManager = new Bll.SettingManagerNew();
        private string SystemID = "00";
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        /// <summary>
        ///变量
        /// </summary>
        private static string lblname = "";

        /// <summary>
        ///所属角色
        /// </summary>
        private static string ss;

        /// <summary>
        /// 姓名
        /// </summary>
        private static string xm;

        /// <summary>
        /// 性别
        /// </summary>
        private static string sb;

        /// <summary>
        /// /出生年月
        /// </summary>
        private static string cy;

        /// <summary>
        /// 警号
        /// </summary>
        private static string jh;

        /// <summary>
        /// /证件类型
        /// </summary>
        private static string zx;

        /// <summary>
        /// 证件号码
        /// </summary>
        private static string zm;

        /// <summary>
        /// 联系地址
        /// </summary>
        private static string lz;

        /// <summary>
        /// 所属机构
        /// </summary>
        private static string sg;

        /// <summary>
        /// //移动手机
        /// </summary>
        private static string yj;

        /// <summary>
        ///固定电话
        /// </summary>
        private static string gh;

        /// <summary>
        /// //备注信息
        /// </summary>
        private static string bx;

        /// <summary>
        /// 获取当前用户
        /// </summary>
        private static string uName = "";

        /// <summary>
        /// 当前使用ip
        /// </summary>
        private static string nowIp = "";

        /// <summary>
        /// 获取用户名
        /// </summary>
        private string nowusername = "";

        public static DataTable dtStoreUser = null;

        private static DataTable dtzjlx = null;

        private static DataTable dtcom = null;

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
            if (!IsPostBack)
            {
                if (!X.IsAjaxRequest)
                {
                    try
                    {
                        StoreDataBind();
                        this.DataBind();
                        UserInfo userinfo = Session["Userinfo"] as UserInfo;
                        uName = userinfo.UserName;
                        nowIp = userinfo.NowIp;
                        logManager.InsertLogRunning(userinfo.UserName, "访问：用户信息管理", userinfo.NowIp, "0");
                        TbutQueryClick(null, null);
                    }
                    catch (Exception ex)
                    {
                        ILog.WriteErrorLog(ex);
                        logManager.InsertLogError("TGSUserManager.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
                    }
                }
            }
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            string where = "1=1";
            if (!string.IsNullOrEmpty(this.TxtUserId.Text))
            {
                where = where + " and a.usercode='" + this.TxtUserId.Text + "'";
            }
            if (!string.IsNullOrEmpty(this.TxtUserName.Text))
            {
                where = where + " and b.username='" + this.TxtUserName.Text + "'";
            }
            try
            {
                UserDataBind(where);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            this.TxtUserId.Reset();
            this.TxtUserName.Reset();
        }

        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButUserAdd_Click(object sender, DirectEventArgs e)
        {
            try
            {
                AddWindow();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-ButUserAdd_Click", ex.Message + "；" + ex.StackTrace, "ButUserAdd_Click has an exception");
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButUserModify_Click(object sender, DirectEventArgs e)
        {
            try
            {
                AddWindowModify();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-ButUserModify_Click", ex.Message + "；" + ex.StackTrace, "ButUserModify_Click has an exception");
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButUserDelete_Click(object sender, DirectEventArgs e)
        {
            try
            {
                RowSelectionModel sm = this.GridUser.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string Id = sm.SelectedRow.RecordID;
                    X.Msg.Confirm(GetLangStr("TGSUserManager37", "信息"), "确认要删除[" + hidNowUsername.Value.ToString() + "]用户吗?", new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "UserManager.DoYes()",
                            Text = GetLangStr("TGSUserManager39", "是")
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "UserManager.DoNo()",
                            Text = GetLangStr("TGSUserManager40", "否")
                        }
                    }).Show();
                }
                else
                {
                    Notice(GetLangStr("TGSUserManager41", "提示信息"), GetLangStr("TGSUserManager42", "请选择要删除的用户"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-ButUserDelete_Click", ex.Message + "；" + ex.StackTrace, "ButUserDelete_Click has an exception");
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
                UserDataBind("1=1");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh has an exception");
            }
        }

        /// <summary>
        /// 获取单行数据信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectUser(object sender, DirectEventArgs e)
        {
            try
            {
                object data = e.ExtraParams["sdata"];
                string sdata = data.ToString();
                hidNowUsername.Value = Bll.Common.GetdatabyField(sdata, "col27");
                // cmbRole.Text = Bll.Common.GetdatabyField(sdata, "col30");
                TxtID.Text = Bll.Common.GetdatabyField(sdata, "col1");
                TxtName.Text = Bll.Common.GetdatabyField(sdata, "col2");
                CmbSex.Text = Bll.Common.GetdatabyField(sdata, "col3");
                DateBirday.Text = Bll.Common.GetdatabyField(sdata, "col4");
                CmbIdType.Text = Bll.Common.GetdatabyField(sdata, "col5");
                TxtIdNo.Text = Bll.Common.GetdatabyField(sdata, "col6");
                TxtPolice.Text = Bll.Common.GetdatabyField(sdata, "col21");
                CmbDerpart.Text = Bll.Common.GetdatabyField(sdata, "col29");
                TxtMobilePhone.Text = Bll.Common.GetdatabyField(sdata, "col14");
                TxtPhone.Text = Bll.Common.GetdatabyField(sdata, "col15");
                TxtBz.Text = Bll.Common.GetdatabyField(sdata, "col26");
                GetUserCheck();

                DataRow[] dr = dtcom.Select("col1='" + TxtID.Text + "'");
                ss = dr[0]["col43"].ToString();//所属角色
                xm = TxtName.Text;//姓名
                sb = dr[0]["col42"].ToString();//性别
                cy = Convert.ToDateTime(DateBirday.Text).ToString("yyyy-MM-dd");   //出生年月
                jh = TxtPolice.Text;//警号
                if (!string.IsNullOrEmpty(CmbIdType.Text))
                {
                    DataRow[] dr1 = dtzjlx.Select("col0='" + CmbIdType.Text + "'");
                    if (dr1 != null)
                    {
                        zx = dr1[0]["col1"].ToString();//证件类型
                    }
                }
                //zx = CmbIdType.SelectedItem.Text; //证件类型
                zm = TxtIdNo.Text;//证件号码
                lz = TxtAddress.Text;//联系地址
                sg = dr[0]["col29"].ToString();//所属机构
                yj = TxtMobilePhone.Text;//移动手机
                gh = TxtPhone.Text;//固定电话
                bx = TxtBz.Text;//备注信息
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-SelectUser", ex.Message + "；" + ex.StackTrace, "SelectUser has an exception");
            }
        }

        #endregion 事件集合

        #region 私有方法

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                DataTable dt = GetRedisData.GetData("t_sys_code:011005");
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.StoreSex.DataSource = Bll.Common.ChangColName(dt);
                    this.StoreSex.DataBind();
                }
                else
                {
                    dt = userManager.GetSex(SystemID);
                    this.StoreSex.DataSource = dt;
                    this.StoreSex.DataBind();
                }

                //证件类型
                DataTable dt2 = dtzjlx = GetRedisData.GetData("t_sys_code:240030");
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    this.StoreIdtype.DataSource = Bll.Common.ChangColName(dt2);
                    this.StoreIdtype.DataBind();
                }
                else
                {
                    dt2 = userManager.GetIdType(SystemID);
                    this.StoreIdtype.DataSource = dt2;
                    this.StoreIdtype.DataBind();
                }

                DataTable dt3 = GetRedisData.GetData("t_cfg_department");
                if (dt3 != null && dt3.Rows.Count > 0)
                {
                    this.StoreDepart.DataSource = dt3;
                    this.StoreDepart.DataBind();
                }
                else
                {
                    dt3 = tgsPproperty.GetDepartmentDict();
                    this.StoreDepart.DataSource = dt3;
                    this.StoreDepart.DataBind();
                }

                string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

                dt = new DataTable();
                dt = userManager.GetSerUserInfo(SystemID, "1=1", ip);
                dtStoreUser = dtcom = dt;
                StoreUser.DataSource = dt;
                StoreUser.DataBind();
                DataTable dtrole = null;
                if (dt.Rows.Count > 0)
                {
                    SelectFirst(dt.Rows[0]);
                    dtrole = GetFirstDataTable(dt.Rows[0]);
                }
                AddRadio(dtrole);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="dtrole"></param>
        private void AddRadio(DataTable dtrole)
        {
            try
            {
                this.StoreRole.DataSource = dtrole;
                this.StoreRole.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-AddRadio", ex.Message + "；" + ex.StackTrace, "AddRadio has an exception");
            }
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        private void GetUserCheck()
        {
            try
            {
                string usercode = TxtID.Text;
                DataTable dt = settingManager.GetSerUserRole(usercode);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cmbRole.SelectedItem.Value = dt.Rows[i][1].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-GetUserCheck", ex.Message + "；" + ex.StackTrace, "GetUserCheck has an exception");
            }
        }

        /// <summary>
        /// 显示修改窗体
        /// </summary>
        private void AddWindowModify()
        {
            try
            {
                RowSelectionModel sm = this.GridUser.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    Window window = new Window();
                    window.ID = "UserModify";
                    window.Title = GetLangStr("TGSUserManager43", "密码修改");
                    window.Width = Unit.Pixel(400);
                    window.Height = Unit.Pixel(240);
                    window.Modal = true;
                    window.Maximizable = false;
                    window.Resizable = false;
                    window.Hidden = true;
                    window.AutoLoad.Mode = LoadMode.Merge;
                    window.Layout = "FitLayout";

                    Ext.Net.FormPanel tab = new Ext.Net.FormPanel();
                    tab.MonitorValid = true;
                    tab.Title = "";
                    tab.Padding = 20;
                    tab.Height = 120;
                    tab.Items.Add(CommonExt.AddTextFieldPassword("txtMFirstPassWord", GetLangStr("TGSUserManager44", "初始密码"), false));
                    tab.Items.Add(CommonExt.AddTextFieldPassword("txtMPassWord", GetLangStr("TGSUserManager45", "新密码"), false));
                    tab.Items.Add(CommonExt.AddTextFieldPassword_Confirm("txtMConfirmPassWord", GetLangStr("TGSUserManager46", "重复密码"), false, "txtMPassWord"));

                    tab.Buttons.Add(CommonExt.AddButton("butSaveEdit2", GetLangStr("TGSUserManager47", "保存"), "Disk", "UserManager.UpdateData()"));
                    tab.Buttons.Add(CommonExt.AddButton("butCancelEdit2", GetLangStr("TGSUserManager48", "取消"), "Cancel", window.ClientID + ".hide()"));
                    tab.Listeners.ClientValidation.Handler = "butSaveEdit2.setDisabled(!valid);";

                    window.Items.Add(tab);
                    window.Render(this.Form);
                    window.Show();
                }
                else
                {
                    Notice(GetLangStr("TGSUserManager42", "提示信息"), GetLangStr("TGSUserManager50", "请选择要修改的用户"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-AddWindowModify", ex.Message + "；" + ex.StackTrace, "AddWindowModify has an exception");
            }
        }

        /// <summary>
        /// 显示注册窗体
        /// </summary>
        private void AddWindow()
        {
            Window window = new Window();
            window.ID = "UserAdd";
            window.Title = GetLangStr("TGSUserManager51", "用户注册");
            window.Width = Unit.Pixel(400);
            window.Height = Unit.Pixel(260);
            window.Modal = true;
            window.Maximizable = false;
            window.Resizable = false;
            window.Hidden = true;
            window.AutoLoad.Mode = LoadMode.Merge;
            window.Layout = "FitLayout";

            Ext.Net.FormPanel tab = new Ext.Net.FormPanel();
            tab.MonitorValid = true;
            tab.Padding = 20;
            tab.Height = 220;
            TextField txtUserId = CommonExt.AddTextField("txtAUserID", GetLangStr("TGSUserManager52", "用户编号"));
            txtUserId.Text = tgsPproperty.GetMinRecordId();
            txtUserId.Disabled = true;
            tab.Items.Add(txtUserId);
            TextField txtUserName = CommonExt.AddTextField("txtAUserName", GetLangStr("TGSUserManager53", "用户名"), false, "");
            txtUserName.Width = Unit.Pixel(300);
            tab.Items.Add(txtUserName);
            tab.Items.Add(CommonExt.AddTextFieldPassword("txtAPassWord", GetLangStr("TGSUserManager54", "密码"), false));
            tab.Items.Add(CommonExt.AddTextFieldPassword_Confirm("txtAConfirmPassWord", GetLangStr("TGSUserManager46", "重复密码"), false, "txtAPassWord"));
            tab.Buttons.Add(CommonExt.AddButton("butSaveEdit", GetLangStr("TGSUserManager56", "保存"), "Disk", "UserManager.InfoSave()"));
            tab.Buttons.Add(CommonExt.AddButton("butCancelEdit", GetLangStr("TGSUserManager57", "取消"), "Cancel", window.ClientID + ".hide()"));
            tab.Listeners.ClientValidation.Handler = "butSaveEdit.setDisabled(!valid);";
            window.Items.Add(tab);
            window.Render(this.Form);
            window.Show();
        }

        /// <summary>
        /// 获取用户数据并绑定到控件
        /// </summary>
        /// <param name="where"></param>
        private void UserDataBind(string where)
        {
            try
            {
                string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                DataTable dt = userManager.GetSerUserInfo(SystemID, where, ip);
                StoreUser.DataSource = dt;
                StoreUser.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-UserDataBind", ex.Message + "；" + ex.StackTrace, "UserDataBind has an exception");
            }
        }

        /// <summary>
        /// 获取DataTable第一行数据信息
        /// </summary>
        /// <param name="dr"></param>
        private void SelectFirst(DataRow dr)
        {
            cmbRole.Text = dr["col30"].ToString();
            TxtID.Text = dr["col1"].ToString();
            TxtName.Text = dr["col2"].ToString();
            CmbSex.Text = dr["col3"].ToString();
            DateBirday.Text = dr["col4"].ToString();
            TxtPolice.Text = dr["col21"].ToString();
            CmbIdType.Text = dr["col5"].ToString();
            TxtIdNo.Text = dr["col6"].ToString();
            CmbDerpart.Text = dr["col29"].ToString();
            TxtMobilePhone.Text = dr["col14"].ToString();
            TxtPhone.Text = dr["col15"].ToString();
            TxtBz.Text = dr["col26"].ToString();
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private DataTable GetFirstDataTable(DataRow dr)
        {
            try
            {
                DataTable dt = settingManager.GetSerRoleInfo(SystemID, "1=1");
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-GetFirstDataTable", ex.Message + "；" + ex.StackTrace, "GetFirstDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        /// 消息提示
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Message(string title, string msg)
        {
            try
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Title = title,
                    Message = msg,
                    Buttons = MessageBox.Button.OK,
                    Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "WARNING")
                });
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-Message", ex.Message + "；" + ex.StackTrace, "Message has an exception");
            }
        }

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
                logManager.InsertLogError("TGSUserManager.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice has an exception");
            }
        }

        /// <summary>
        /// 修改日志
        /// </summary>
        /// <param name="ty"></param>
        private void updataLog()
        {
            string ty = "1";
            string ss1 = cmbRole.SelectedItem.Text;//所属角色
            string yhbh = TxtID.Text;//
            string xm1 = TxtName.Text;//姓名
            string sb1 = CmbSex.SelectedItem.Text;//性别
            string cy1 = Convert.ToDateTime(DateBirday.Text).ToString("yyyy-MM-dd");//出生年月
            string jh1 = TxtPolice.Text;//警号
            string zx1 = CmbIdType.SelectedItem.Text;//证件类型
            string zm1 = TxtIdNo.Text;//证件号码
            string lz1 = TxtAddress.Text;//联系地址
            string sg1 = CmbDerpart.SelectedItem.Text;//所属机构
            string yj1 = TxtMobilePhone.Text;//移动手机
            string gh1 = TxtPhone.Text;//固定电话
            string bx1 = TxtBz.Text;//备注信息
            if (!string.IsNullOrEmpty(jh) && !string.IsNullOrEmpty(ss))
            {
                string lblname = "";
                lblname += Bll.Common.AssembleRunLog(ss, ss1, "所属角色", ty);
                lblname += Bll.Common.AssembleRunLog(xm, xm1, "姓名", ty);
                lblname += Bll.Common.AssembleRunLog(sb, sb1, "性别", ty);
                lblname += Bll.Common.AssembleRunLog(cy, cy1, "出生年月", ty);
                lblname += Bll.Common.AssembleRunLog(jh, jh1, "警号", ty);
                lblname += Bll.Common.AssembleRunLog(zx, zx1, "证件类型", ty);
                lblname += Bll.Common.AssembleRunLog(zm, zm1, "证件号码", ty);
                lblname += Bll.Common.AssembleRunLog(lz, lz1, "联系地址", ty);
                lblname += Bll.Common.AssembleRunLog(sg, sg1, "所属机构", ty);
                lblname += Bll.Common.AssembleRunLog(yj, yj1, "移动手机", ty);
                lblname += Bll.Common.AssembleRunLog(gh, gh1, "固定电话", ty);
                lblname += Bll.Common.AssembleRunLog(bx, bx1, "备注信息", ty);

                logManager.InsertLogRunning(uName, "修改用户：[" + hidNowUsername.Value.ToString() + "];" + lblname, nowIp, "2");
            }
            else
            {
                string lblname = "";
                lblname += Bll.Common.AssembleRunLog("", ss1, "所属角色", "0");
                lblname += Bll.Common.AssembleRunLog("", xm1, "姓名", "0");
                lblname += Bll.Common.AssembleRunLog("", sb1, "性别", "0");
                lblname += Bll.Common.AssembleRunLog("", jh1, "警号", "0");
                lblname += Bll.Common.AssembleRunLog("", zx1, "证件类型", "0");
                lblname += Bll.Common.AssembleRunLog("", zm1, "证件号码", "0");
                lblname += Bll.Common.AssembleRunLog("", lz1, "联系地址", "0");
                lblname += Bll.Common.AssembleRunLog("", sg1, "所属机构", "0");
                lblname += Bll.Common.AssembleRunLog("", yj1, "移动手机", "0");
                lblname += Bll.Common.AssembleRunLog("", gh1, "固定电话", "0");
                lblname += Bll.Common.AssembleRunLog("", bx1, "备注信息", "0");
                string t = "0001/1/1 星期一 上午 12:00:00";

                if (!cy.Equals(t))
                {
                    lblname += Bll.Common.AssembleRunLog("", cy1, "出生年月", "0");

                    logManager.InsertLogRunning(uName, "添加用户详细信息：[" + hidNowUsername.Value.ToString() + "];" + lblname, nowIp, "2");
                    return;
                }
                else
                {
                    logManager.InsertLogRunning(uName, "添加用户详细信息：[" + hidNowUsername.Value.ToString() + "];" + lblname, nowIp, "2");
                }
                DataRow[] dr = dtcom.Select("col1='" + TxtID.Text + "'");
                ss = dr[0]["col43"].ToString();//所属角色
                xm = TxtName.Text;//姓名
                sb = dr[0]["col42"].ToString();//性别
                cy = Convert.ToDateTime(DateBirday.Text).ToString("yyyy-MM-dd");//出生年月
                jh = TxtPolice.Text;//警号
                DataRow[] dr1 = dtzjlx.Select("col0='" + CmbIdType.Text + "'");
                zx = dr1[0]["col1"].ToString();//证件类型
                zm = TxtIdNo.Text;//证件号码
                lz = TxtAddress.Text;//联系地址
                sg = dr[0]["col29"].ToString();//所属机构
                yj = TxtMobilePhone.Text;//移动手机
                gh = TxtPhone.Text;//固定电话
                bx = TxtBz.Text;//备注信息
            }
        }

        #endregion 私有方法

        #region 语言转换

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

        #endregion 语言转换

        #region DirectMethod

        /// <summary>
        /// 用户注册
        /// </summary>
        [DirectMethod]
        public void InfoSave()
        {
            try
            {
                string usercode = X.GetCmp<TextField>("txtAUserID").Text;
                string username = X.GetCmp<TextField>("txtAUserName").Text;
                string pwd = X.GetCmp<TextField>("txtAPassWord").Text;
                if (tgsPproperty.GetXhExist("t_ser_register", "username", username) > 0)
                {
                    Message(GetLangStr("TGSUserManage58", "信息提示"), GetLangStr("TGSUserManager59", "该用户名已存在,如果继续增加请填写不同的用户名！")
 );
                    X.GetCmp<TextField>("txtAUserName").Text = "";
                    return;
                }
                if (userManager.UserRegister(SystemID, usercode, username, Cryptography.Encrypt(pwd)) > 0)
                {
                    string lblname = "";
                    lblname += Bll.Common.AssembleRunLog("", username, "用户名", "0");
                    logManager.InsertLogRunning(uName, "添加用户：" + lblname, nowIp, "1");
                    Notice(GetLangStr("TGSUserManager60", "用户注册"), GetLangStr("TGSUserManager61", "注册成功"));
                    X.GetCmp<Window>("UserAdd").Hide();
                    UserDataBind("1=1");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-InfoSave", ex.Message + "；" + ex.StackTrace, "InfoSave has an exception");
            }
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        [DirectMethod]
        public void UpdatePerson()
        {
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("usercode", TxtID.Text);
                hs.Add("name", TxtName.Text);
                hs.Add("idno", TxtIdNo.Text);
                hs.Add("address", TxtAddress.Text);
                hs.Add("mobilephone", TxtMobilePhone.Text);
                hs.Add("officephone", TxtPhone.Text);
                hs.Add("remark", TxtBz.Text);
                if (CmbSex.SelectedIndex != -1)
                {
                    hs.Add("sex", CmbSex.SelectedItem.Value);
                }
                if (CmbIdType.SelectedIndex != -1)
                {
                    hs.Add("idtype", CmbIdType.SelectedItem.Value);
                }
                if (CmbDerpart.SelectedIndex != -1)
                {
                    hs.Add("departid", CmbDerpart.SelectedItem.Value);
                }
                if (DateBirday.SelectedDate != null)
                {
                    hs.Add("birthday", DateBirday.SelectedDate.ToString("yyyy-MM-dd"));
                }
                hs.Add("siren", TxtPolice.Text);
                if (userManager.UpdateSerUserInfo(hs) > 0)
                {
                    string notice = GetLangStr("TGSUserManager62", @"用户信息更新成功；<br\>");
                    if (!string.IsNullOrEmpty(cmbRole.SelectedItem.Value))
                    {
                        if (settingManager.InsertUserRole(TxtID.Text, cmbRole.SelectedItem.Value) > 0)
                        {
                            updataLog();
                            notice = notice + GetLangStr("TGSUserManager63", "角色更新成功");
                        }
                    }
                    Notice(GetLangStr("TGSUserManager64", "信息更新"), notice);
                    UserDataBind("1=1");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-UpdatePerson", ex.Message + "；" + ex.StackTrace, "UpdatePerson has an exception");
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        [DirectMethod]
        public void UpdateData()
        {
            try
            {
                RowSelectionModel sm = this.GridUser.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string id = sm.SelectedRow.RecordID;
                    string pwd = X.GetCmp<TextField>("txtMPassWord").Text;
                    string oldPwd = X.GetCmp<TextField>("txtMFirstPassWord").Text;
                    if (userManager.CheckOldPwd(id, oldPwd))
                    {
                        if (userManager.ChangeUserPwd(id, Cryptography.Encrypt(pwd)) > 0)
                        {
                            logManager.InsertLogRunning(uName, "修改用户[" + hidNowUsername.Value.ToString() + "]的密码", nowIp, "2");
                            Notice(GetLangStr("TGSUserManager43", "密码修改"), GetLangStr("TGSUserManager66", "修改成功"));

                            X.GetCmp<Window>("UserModify").Hide();
                        }
                    }
                    else
                    { Notice(GetLangStr("TGSUserManager65", "密码修改"), GetLangStr("TGSUserManager67", "原始密码错误")); }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-UpdateData", ex.Message + "；" + ex.StackTrace, "UpdateData has an exception");
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
                RowSelectionModel sm = this.GridUser.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRow.ToBuilder();
                Hashtable hs = new Hashtable();
                string Id = sm.SelectedRow.RecordID;
                string namess = "";
                DataRow[] rows = dtStoreUser.Select("col1=" + Id);
                if (rows.Length > 0)
                {
                    namess = rows[0]["col27"].ToString();
                }
                int idx = sm.SelectedRow.RowIndex;
                hs.Add("usercode", Id);
                if (userManager.DeleteSerUserInfo(hs) > 0)
                {
                    logManager.InsertLogRunning(uName, "删除用户[" + hidNowUsername.Value.ToString() + "];姓名[" + namess + "]", nowIp, "3");
                    Notice(GetLangStr("TGSUserManager68", "信息删除"), GetLangStr("TGSUserManager69", "删除成功！"));
                    UserDataBind("1=1");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TGSUserManager.aspx-DoYes", ex.Message + "；" + ex.StackTrace, "DoYes has an exception");
            }
        }

        /// <summary>
        /// 不删除事件
        /// </summary>
        [DirectMethod]
        public void DoNo()
        {
        }

        #endregion DirectMethod
    }
}