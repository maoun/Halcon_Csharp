using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class RoleManager : System.Web.UI.Page
    {
        #region 成员变量

        private TgsPpropertyNew tgsPproperty = new TgsPpropertyNew();
        private Bll.SettingManagerNew settingManager = new Bll.SettingManagerNew();
        private string SystemID = "00";
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        #endregion 成员变量

        /// <summary>
        /// 角色名称
        /// </summary>
        private static string jc;

        /// <summary>
        ///角色描述
        /// </summary>
        private static string js;

        /// <summary>
        /// 备注
        /// </summary>
        private static string bz;

        /// <summary>
        /// 接收字符串
        /// </summary>
        private static string lblname;

        /// <summary>
        /// 定义用户名
        /// </summary>
        private string hidNowUser = "";

        /// <summary>
        /// 获取登陆
        /// </summary>
        private static string uName = "";

        /// <summary>
        /// 获取ip
        /// </summary>
        private static string nowIp = "";
        /// <summary>
        /// 添加成功是否赋予权限
        /// </summary>
       

        private static string yhm = "";
        private static DataTable dtRole = null;
        private static DataTable dtRole1 = null;

        static DataTable dtname = null;
        #region 事件

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
                string js = "alert('"+GetLangStr("RoleManager16","您没有登录或操作超时，请重新登录!")+"');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                try
                {
                    StoreDataBind();
                    this.DataBind();
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    uName = userinfo.UserName;
                    nowIp = userinfo.NowIp;
                    logManager.InsertLogRunning(userinfo.UserName,GetLangStr("RoleManager17","访问：用户角色管理") , userinfo.NowIp, "0");
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex);
                    logManager.InsertLogError("RoleManager.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
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
                string where = "1=1";
                if (!string.IsNullOrEmpty(this.TxtRoleId.Text))
                {
                    where = where + " and rolecode='" + this.TxtRoleId.Text + "'";
                }
                if (!string.IsNullOrEmpty(this.TxtRoleName.Text))
                {
                    where = where + " and rolename='" + this.TxtRoleName.Text + "'";
                }
                RoleDataBind(where);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("RoleManager.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
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
                this.TxtRoleId.Reset();
                this.TxtRoleName.Reset();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("RoleManager.aspx-ButResetClick", ex.Message + "；" + ex.StackTrace, "ButResetClick has an exception");
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButRoleAdd_Click(object sender, DirectEventArgs e)
        {
            try
            {
                AddWindow();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("RoleManager.aspx-ButRoleAdd_Click", ex.Message + "；" + ex.StackTrace, "ButRoleAdd_Click has an exception");
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButRoleDelete_Click(object sender, DirectEventArgs e)
        {
            try
            {
                RowSelectionModel sm = this.GridRole.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string Id = sm.SelectedRow.RecordID;
                    DataRow[] dr = dtname.Select("col1='" + Id + "'");
                    string yhms = dr[0]["col2"].ToString();
                    X.Msg.Confirm(GetLangStr("RoleManager18", "信息"), GetLangStr("RoleManager19", "确认要删除[" + yhms + GetLangStr("RoleManager50", "]吗?")), new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "RoleManager.DoYes()",
                            Text = GetLangStr("RoleManager20","是")
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "RoleManager.DoNo()",
                            Text =GetLangStr("RoleManager21","否") 
                        }
                    }).Show();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("RoleManager.aspx-ButRoleDelete_Click", ex.Message + "；" + ex.StackTrace, "ButRoleDelete_Click has an exception");
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
                RoleDataBind("1=1");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("RoleManager.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh has an exception");
            }
        }

        /// <summary>
        /// 获取选中角色的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectRole(object sender, DirectEventArgs e)
        {
            try
            {
                object data = e.ExtraParams["sdata"];
                string sdata = data.ToString();
                string js = "ClearCheckState();";
                CurrentRoleId.Value = Bll.Common.GetdatabyField(sdata, "col1");
                this.ResourceManager1.RegisterAfterClientInitScript(js);
                GetRoleCheck(CurrentRoleId.Value.ToString());

            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);

                logManager.InsertLogError("RoleManager.aspx-SelectRole", ex.Message + "；" + ex.StackTrace, "SelectRole has an exception");
            }
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateData(object sender, DirectEventArgs e)
        {
            try
            {
                RowSelectionModel sm = this.GridRole.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string Id = sm.SelectedRow.RecordID;
                    Hashtable hs = new Hashtable();

                    hs.Add("systemid", SystemID);
                    hs.Add("rolecode", Id);
                    hs.Add("rolename", TxtERoleName.Text);
                    hs.Add("roledesc", TxtERoleDesc.Text);
                    hs.Add("remark", TxtERoleRemark.Text);

                    if (settingManager.UpdateSerRoleInfo(hs) > 0)
                    {
                        yhm = "";
                        Notice(GetLangStr("RoleManager22","角色修改"),GetLangStr("RoleManager23","修改成功") );
                        string jjmc = TxtERoleName.Text;
                        string jjms = TxtERoleDesc.Text;
                        string bzz = TxtERoleRemark.Text;
                        lblname = "";
                        lblname += Bll.Common.AssembleRunLog(jc, jjmc,GetLangStr("RoleManager10","角色名称") , "1");
                        lblname += Bll.Common.AssembleRunLog(js, jjms, GetLangStr("RoleManager11","角色描述"), "1");
                        lblname += Bll.Common.AssembleRunLog(bz, bzz, GetLangStr("RoleManager12","备注"), "1");
                        logManager.InsertLogRunning(uName,GetLangStr("RoleManager27","修改角色:[")  + jjmc + "]" + lblname, nowIp, "1");
                        RoleDataBind("1=1");
                        yhm = jjmc;
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("RoleManager.aspx-UpdateData", ex.Message + "；" + ex.StackTrace, "UpdateData has an exception");
            }
        }

        #endregion 事件

        #region DirectMethod

        /// <summary>
        /// 确定事件
        /// </summary>
        [DirectMethod]
        public void DoYes()
        {
            try
            {
                RowSelectionModel sm = this.GridRole.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRow.ToBuilder();
                Hashtable hs = new Hashtable();
                string Id = sm.SelectedRow.RecordID;
                int idx = sm.SelectedRow.RowIndex;
                hs.Add("rolecode", Id);
                if (settingManager.DeleteSerRoleInfo(hs) > 0)
                {
                    Notice(GetLangStr("RoleManager28", "角色删除"), GetLangStr("RoleManager29", "删除成功"));

                    DataRow[] dr = dtname.Select("col1='" + Id + "'");
                    yhm = dr[0]["col2"].ToString();
                    logManager.InsertLogRunning(uName,GetLangStr("RoleManager30","删除:[")  + yhm + GetLangStr("RoleManager31","]用户"), nowIp, "3");
                    RoleDataBind("1=1");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("RoleManager.aspx-DoYes", ex.Message + "；" + ex.StackTrace, "DoYes has an exception");
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
        /// 添加角色
        /// </summary>
        [DirectMethod]
        public void InfoSave()
        {
            try
            {
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                string rolecode = X.GetCmp<TextField>("txtARoleID").Text;
                string rolename = X.GetCmp<TextField>("txtARoleName").Text;
                string roledesc = X.GetCmp<TextField>("txtARoleDesc").Text;
                if (roledesc.Equals("请输入角色描述"))
                {
                    roledesc = "";
                }
                string remark = X.GetCmp<TextField>("txtARemark").Text;
                if (remark.Equals("请输入备注"))
                {
                    remark = "";
                }
                Hashtable hs = new Hashtable();
                jc = rolename;
                js = roledesc;
                bz = remark;
                hs.Add("systemid", SystemID);
                hs.Add("rolecode", rolecode);
                hs.Add("roledesc", roledesc);
                hs.Add("rolename", rolename);
                hs.Add("remark", remark);
                if (settingManager.InsertSerRoleInfo(hs) > 0)
                {
                    lblname = " ";
                    lblname += Bll.Common.AssembleRunLog("", rolename,GetLangStr("RoleManager32","角色") , "0");
                    Notice(GetLangStr("RoleManager33","角色新增"), GetLangStr("RoleManager34","新增成功"));
                    logManager.InsertLogRunning(userinfo.UserName,GetLangStr("RoleManager35","添加:")  + lblname, userinfo.NowIp, "1");
                    X.GetCmp<Window>("RoleAdd").Hide();
                    RoleDataBind("1=1");
                        dtname = settingManager.GetSerRoleInfo(SystemID, "1=1");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("RoleManager.aspx-InfoSave", ex.Message + "；" + ex.StackTrace, "InfoSave has an exception");
            }
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        [DirectMethod]
        public void UpdateRoleFunc()
        {
            try
            {
             
                string roleid = CurrentRoleId.Value.ToString();
                List<string> privcodes = new List<string>();
                string[] datas = GridData.Value.ToString().Split(',');
                string names = "";
                string oldNames="";
                privcodes.AddRange(datas);
                if (settingManager.InsertRolePriv(roleid, privcodes) > 0)
                {
                    Notice(GetLangStr("RoleManager36","信息更新"),GetLangStr("RoleManager23", "修改成功"));

                    if(dtRole!=null)
                    {
                         for (int i = 0; i < dtRole.Rows.Count; i++)
			        {
		          	   DataRow[] rows= dtRole1.Select("col1="+dtRole.Rows[i]["col1"].ToString());
                         if (rows.Length > 0)
                            {
                                oldNames +="["+ rows[0]["col2"].ToString()+"]";
                            }
		         	}
                    }
                   
                    for (int i = 0; i < datas.Length; i++)
                    {
                            DataRow[] rows1 = dtRole1.Select("col1='" + datas[i] + "'");
                            if (rows1.Length > 0)
                            {
                                names+="["+ rows1[0]["col2"].ToString()+"]";
                            }
                      
                    }
                    DataRow[] dt1 = dtname.Select("col1='"+roleid+"'");
                    string n = dt1[0]["col2"].ToString();
                    string xg = Bll.Common.AssembleRunLog(oldNames,names,GetLangStr("RoleManager38","权限"),"1");
                     logManager.InsertLogRunning(uName,GetLangStr("RoleManager27","修改角色:[")  + n + "]"+xg, nowIp, "2");
                    RoleDataBind("1=1");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("RoleManager.aspx-UpdateRoleFunc", ex.Message+"；"+ex.StackTrace, "UpdateRoleFunc has an exception");
            }
        }

        #endregion DirectMethod

        #region 私有事件

        /// <summary>
        /// 用户角色绑定
        /// </summary>
        /// <param name="where"></param>
        private void RoleDataBind(string where)
        {
            try
            {
                DataTable dt = settingManager.GetSerRoleInfo(SystemID, where);
                StoreRole.DataSource = dt;
                StoreRole.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("RoleManager.aspx-RoleDataBind", ex.Message + "；" + ex.StackTrace, "RoleDataBind has an exception");
            }
        }

        /// <summary>
        /// 获取选中用户的权限
        /// </summary>
        /// <param name="rolecode"></param>
        private void GetRoleCheck(string rolecode)
        {
            try
            {
                if (dtRole != null)
                {
                    dtRole = null;
                }
                DataTable dt = dtRole = settingManager.GetSerRolePriv(rolecode);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string privcode = dt.Rows[i][1].ToString();
                        string js = "SetCheckState(\"" + privcode + "\");";
                        this.ResourceManager1.RegisterAfterClientInitScript(js);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("RoleManager.aspx-GetRoleCheck", ex.Message + "；" + ex.StackTrace, "GetRoleCheck has an exception");
            }
        }

        /// <summary>
        /// 获取第一行角色信息
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private DataTable GetFirstDataTable(DataRow dr)
        {
            try
            {
                DataTable dt = settingManager.GetSerRolePriv(dr["col1"].ToString());
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("RoleManager.aspx-GetFirstDataTable", ex.Message + "；" + ex.StackTrace, "GetFirstDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                DataTable dt = dtname = settingManager.GetSerRoleInfo(SystemID, "1=1");
                StoreRole.DataSource = dt;
                StoreRole.DataBind();
                if (dt.Rows.Count > 0)
                {
                    DataTable dtrole = GetFirstDataTable(dt.Rows[0]);
                    BuildTree(TreePanel1.Root, dtrole);
                }
                string jjmc = TxtERoleName.Text;
                string jjms = TxtERoleDesc.Text;
                string bzz = TxtERoleRemark.Text;
                yhm = jjmc;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("RoleManager.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 显示自定义窗体
        /// </summary>
        private void AddWindow()
        {
            try
            {
                Window window = new Window();
                window.ID = "RoleAdd";
                window.Title =GetLangStr("RoleManager40","增加角色") ;
                window.Width = Unit.Pixel(360);
                window.Height = Unit.Pixel(260);
                window.Modal = true;
                window.Maximizable = false;
                window.Resizable = false;
                window.Hidden = true;
                window.AutoLoad.Mode = LoadMode.Merge;
                window.Layout = "FitLayout";

                Ext.Net.FormPanel tab = new Ext.Net.FormPanel();
                tab.MonitorValid = true;
                tab.ButtonAlign = Alignment.Center;
                tab.Padding = 20;
                TextField txtRoleId = CommonExt.AddTextField("txtARoleID",GetLangStr("RoleManager9","角色编号") );
                txtRoleId.Text = tgsPproperty.GetMinRecordId();
                txtRoleId.Disabled = true;
                tab.Items.Add(txtRoleId);
                tab.Items.Add(CommonExt.AddTextFieldWidth("txtARoleName", GetLangStr("RoleManager10","角色名称"), false,GetLangStr("RoleManager43","请输入角色名称") ));
                tab.Items.Add(CommonExt.AddTextFieldWidth("txtARoleDesc",GetLangStr("RoleManager11","角色描述") , true,GetLangStr("RoleManager45","请输入角色描述") ));
                tab.Items.Add(CommonExt.AddTextFieldWidth("txtARemark",GetLangStr("RoleManager12","备注") , true, GetLangStr("RoleManager47","请输入备注")));
                tab.Buttons.Add(CommonExt.AddButton("butSaveEdit",GetLangStr("RoleManager15","保存") , "Disk", "RoleManager.InfoSave()"));
                tab.Buttons.Add(CommonExt.AddButton("butCancelEdit",GetLangStr("RoleManager49","取消") , "Cancel", window.ClientID + ".hide()"));
                tab.Listeners.ClientValidation.Handler = "butSaveEdit.setDisabled(!valid);";
                txtRoleId.Disabled = false;
                window.Items.Add(tab);
                window.Render(this.Form);
                window.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("RoleManager.aspx-AddWindow", ex.Message + "；" + ex.StackTrace, "AddWindow has an exception");
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
                logManager.InsertLogError("RoleManager.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice has an exception");
            }
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="dtRole"></param>
        private void BuildTree(Ext.Net.TreeNodeCollection nodes, DataTable dtRole)
        {
            try
            {
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }

                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Text = "Root";
                nodes.Add(root);
                root.Expanded = true;
                DataTable dt = dtRole1 = settingManager.GetSerPrivInfo(SystemID, "1=1");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    node.Text = dt.Rows[i][2].ToString();
                    node.Leaf = true;
                    node.Icon = Icon.UserKey;
                    node.Checked = ThreeStateBool.False;
                    if (dtRole != null)
                    {
                        for (int j = 0; j < dtRole.Rows.Count; j++)
                        {
                            if (dtRole.Rows[j][1].ToString() == dt.Rows[i][1].ToString())
                            {
                                node.Checked = ThreeStateBool.True;
                            }
                        }
                    }
                    node.NodeID = dt.Rows[i][1].ToString();
                    node.Expanded = true;
                    root.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("RoleManager.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
            }
        }

        #endregion 私有事件

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
    }
}