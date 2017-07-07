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
    public partial class PrivManager : System.Web.UI.Page
    {
        #region 成员变量

        private TgsPproperty tgsPproperty = new TgsPproperty();
        private Bll.SettingManagerNew settingManager = new Bll.SettingManagerNew();
        private string SystemID = "00";
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
      
        /// <summary>
        /// 首次获取权限名称
        /// </summary>
        static string qc;
        /// <summary>
        /// 权限描述
        /// </summary>
        static string qs;

            static string lblname = "";
        /// <summary>
        /// 再次权限名称
        /// </summary>
        static string qmc = "";
        /// <summary>
        /// 获取用户名
        /// </summary>
            static string uName;
        /// <summary>
        /// 获取ip
        /// </summary>
            static string nowIp;
        /// <summary>
        /// 获取初始权限
        /// </summary>
           // static DataTable dtRole=null;
            private static DataTable dtRole = GetDt();
        /// <summary>
        /// 获取新的权限
        /// </summary>
            static DataTable dtRole1=null;

            static string yhm;


        #endregion 成员变量

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
                string js = "alert('"+GetLangStr("PrivManager16","您没有登录或操作超时，请重新登录!")+"');window.top.location.href='" + StaticInfo.LoginPage + "'"; 
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); 
                return; 
            }
            try
            {
                if (!X.IsAjaxRequest)
                {
                    StoreDataBind();
                    this.DataBind();
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    uName = userinfo.UserName;
                    nowIp = userinfo.NowIp;
                    logManager.InsertLogRunning(userinfo.UserName, GetLangStr("PrivManager17","访问：权限信息管理"), userinfo.NowIp, "0");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-Page_Load", ex.Message+"；"+ex.StackTrace, "Page_Load has an exception");
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
                if (!string.IsNullOrEmpty(this.TxtPrivId.Text))
                {
                    where = where + " and privcode='" + this.TxtPrivId.Text + "'";
                }
                if (!string.IsNullOrEmpty(this.TxtPrivName.Text))
                {
                    where = where + " and privname='" + this.TxtPrivName.Text + "'";
                }

                PrivDataBind(where);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");
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
                TxtPrivId.Reset();
                TxtPrivName.Reset();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-ButResetClick", ex.Message+"；"+ex.StackTrace, "ButResetClick has an exception");
            }
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButPrivAdd_Click(object sender, DirectEventArgs e)
        {
            try
            {
                AddWindow();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-ButPrivAdd_Click", ex.Message+"；"+ex.StackTrace, "ButPrivAdd_Click has an exception");
            }
        }
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButPrivDelete_Click(object sender, DirectEventArgs e)
        {
            try
            {
                RowSelectionModel sm = this.GridPriv.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string Id = sm.SelectedRow.RecordID;
                    X.Msg.Confirm(GetLangStr("PrivManager18", "信息"),GetLangStr("PrivManager19","确认要删除编号为[") +qmc+GetLangStr("PrivManager20","]吗?"), new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "PrivManager.DoYes()",
                            Text = GetLangStr("PrivManager21", "是")
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "PrivManager.DoNo()",
                            Text = GetLangStr("PrivManager22", "否")
                        }
                    }).Show();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-ButPrivDelete_Click", ex.Message+"；"+ex.StackTrace, "ButPrivDelete_Click has an exception");
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
                PrivDataBind("1=1");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-MyData_Refresh", ex.Message+"；"+ex.StackTrace, "MyData_Refresh has an exception");
            }
        }
        /// <summary>
        /// 显示选中权限的详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectPriv(object sender, DirectEventArgs e)
        {
            try
            {
                object data = e.ExtraParams["sdata"];
                string sdata = data.ToString();
                string js = "ClearCheckState();";
                CurrentPrivId.Value = Bll.Common.GetdatabyField(sdata, "col1");
                 qc=Bll.Common.GetdatabyField(sdata,"col2");
                 qs = Bll.Common.GetdatabyField(sdata, "col3");
                this.ResourceManager1.RegisterAfterClientInitScript(js);
                GetPrivCheck(CurrentPrivId.Value.ToString());
              yhm = Bll.Common.GetdatabyField(sdata, "col2");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-SelectPriv", ex.Message+"；"+ex.StackTrace, "SelectPriv has an exception");
            }
        }
        /// <summary>
        /// 权限修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateData(object sender, DirectEventArgs e)
        {
            try
            {
                RowSelectionModel sm = this.GridPriv.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string Id = sm.SelectedRow.RecordID;
                    Hashtable hs = new Hashtable();
                    hs.Add("systemid", SystemID);
                    hs.Add("privcode", Id);
                    hs.Add("privname", TxtEPrivName.Text);
                    hs.Add("remark", TxtEPrivRemark.Text);
               
                    if (settingManager.UpdateSerPrivInfo(hs) > 0)
                    {
                        Notice(GetLangStr("PrivManager23", "权限修改"), GetLangStr("PrivManager24", "修改成功"));
                       
                        string qimc=TxtEPrivName.Text;
                        string qims=TxtEPrivRemark.Text;
                      
                        string lblname = "";
                        lblname += Bll.Common.AssembleRunLog(qc, qimc, GetLangStr("PrivManager10","权限名称"), "1");
                        lblname += Bll.Common.AssembleRunLog(qs, qims, GetLangStr("PrivManager11","权限描述"), "1");
                        logManager.InsertLogRunning(uName,GetLangStr("PrivManager27","修改") +lblname,nowIp, "2");
                        PrivDataBind("1=1");
                        qmc = qimc;
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-UpdateData", ex.Message+"；"+ex.StackTrace, "UpdateData has an exception");
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
                RowSelectionModel sm = this.GridPriv.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRow.ToBuilder();
                Hashtable hs = new Hashtable();
                string Id = sm.SelectedRow.RecordID;
                int idx = sm.SelectedRow.RowIndex;
                hs.Add("privcode", Id);
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                if (settingManager.DeleteSerPrivInfo(hs) > 0)
                {
                    logManager.InsertLogRunning(userinfo.UserName,GetLangStr("PrivManager28","删除:[") +yhm+GetLangStr("PrivManager29","]权限"), userinfo.NowIp, "3");
                    Notice(GetLangStr("PrivManager30", "权限删除"), GetLangStr("PrivManager31", "删除成功"));
                    PrivDataBind("1=1");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-DoYes", ex.Message+"；"+ex.StackTrace, "DoYes has an exception");
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
        /// 添加权限
        /// </summary>
        [DirectMethod]
        public void InfoSave()
        {
            try
            {
                string Privcode = X.GetCmp<TextField>("txtAPrivID").Text;
                string Privname = X.GetCmp<TextField>("txtAPrivName").Text;
                if (Privname.Equals("请输入权限名称"))
                {
                    Privname = "";
                }
                string remark = X.GetCmp<TextField>("txtARemark").Text;
                if (remark.Equals("请输入权限描述"))
                {
                    remark = "";
                }
                qc = Privname;
                qs = remark;
                Hashtable hs = new Hashtable();

                hs.Add("systemid", SystemID);
                hs.Add("privcode", Privcode);
                hs.Add("privname", Privname);
                hs.Add("remark", remark);
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                if (settingManager.InsertSerPrivInfo(hs) > 0)
                {
                    Notice(GetLangStr("PrivManager32", "权限新增"), GetLangStr("PrivManager33", "新增成功"));
                    logManager.InsertLogRunning(userinfo.UserName,GetLangStr("PrivManager34","新增:[") +Privname+GetLangStr("PrivManager29","]权限") , userinfo.NowIp, "1");
                    X.GetCmp<Window>("PrivAdd").Hide();
                    PrivDataBind("1=1");
                    qmc = Privname;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-InfoSave", ex.Message+"；"+ex.StackTrace, "InfoSave has an exception");
            }
        }
        /// <summary>
        /// 权限修改
        /// </summary>
        [DirectMethod]
        public void UpdatePrivFunc()
        {
            try
            {
                string oldname = "";
                string nowname = "";
                string privid = CurrentPrivId.Value.ToString();
                List<string> funcids = new List<string>();
              
                if (!string.IsNullOrEmpty(GridData.Value.ToString()))
                {
                    //datas 获取现在的传参值
                    string[] datas = GridData.Value.ToString().Split(',');
                    funcids.AddRange(datas);
                    //获取现在的值
                    for (int i = 0; i < datas.Length; i++) {
                       DataRow[] dr = dtRole.Select("col0='" + datas[i] + "'");
                       if (dr.Length>0)
                       {
                          nowname+="["+dr[0]["col1"].ToString()+"]";

                       }
                    }
                    //获取以往的值
                    for (int i = 0; i < dtRole1.Rows.Count; i++)
                    {
                        DataRow[] dr1 = dtRole.Select("col0='" + dtRole1.Rows[i]["col1"] + "'");
                        if (dr1.Length > 0)
                        {
                            oldname += "["+dr1[0]["col1"].ToString()+"]";

                        }
                    }
                }
                if (!string.IsNullOrEmpty(GridData2.Value.ToString()))
                {
                    string[] datas2 = GridData2.Value.ToString().Split(',');
                    funcids.AddRange(datas2);
                }
              

                if (settingManager.InsertPrivFunc(privid, funcids) > 0)
                {

                    //for (int i = 0; i <dtRole.Rows.Count; i++)
                    //{
                    //    string[] datas2 = GridData2.Value.ToString().Split(',');
                    //  //    DataRow[] rows1 = dtRole1.Select("col1='" + dtRole.Rows[i]["col1"] + "'");
                    // //   oldname += "[" + rows1[0]["col2"].ToString() + "]";
                    //    oldname += datas2[i].ToString();
                    //}
                    //string[] datas = GridData.Value.ToString().Split(',');
                    //for (int i = 0; i < datas.Length; i++)
                    //{
                    //    string[] datas2 = GridData2.Value.ToString().Split(',');
                    //    DataRow[] rows1 = dtRole1.Select("col1='" + datas2[i] + "'");
                    //    nowname += "[" + rows1[0]["col2"].ToString() + "]";
                    //}

                    logManager.InsertLogRunning(uName,GetLangStr("PrivManager36","修改:[" ) + qmc + "]" +GetLangStr("PrivManager37","用户权限:[")  + oldname +GetLangStr("PrivManager38","]修改成[")  + nowname + "]", nowIp, "1");
                    Notice(GetLangStr("PrivManager23", "权限修改"), GetLangStr("PrivManager24", "修改成功"));
                    PrivDataBind("1=1");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-UpdatePrivFunc", ex.Message+"；"+ex.StackTrace, "UpdatePrivFunc has an exception");
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
                this.StoreDepart.DataSource = tgsPproperty.GetDepartmentDict();
                this.StoreDepart.DataBind();
                DataTable dt = settingManager.GetSerPrivInfo(SystemID, "1=1");
                StorePriv.DataSource = dt;
                StorePriv.DataBind();
              
                DataTable dtPriv = null;
                if (dt.Rows.Count > 0)
                {
                    dtPriv = GetFirstDataTable(dt.Rows[0]);
                }
                BuildTree(TreePanel1.Root, dtPriv, "0");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-StoreDataBind", ex.Message+"；"+ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 获取用户拥有的权限
        /// </summary>
        /// <param name="privcode"></param>
        private void GetPrivCheck(string privcode)
        {
            try
            {
             

                DataTable dt =settingManager.GetSerPrivFunc(privcode);
                dtRole1 = dt;

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string js = "";
                        string funcid = dt.Rows[i][1].ToString();
                        js = "SetCheckState(\"" + funcid + "\");";
                        this.ResourceManager1.RegisterAfterClientInitScript(js);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-GetPrivCheck", ex.Message+"；"+ex.StackTrace, "GetPrivCheck has an exception");
            }
        }

        private bool IsNumeric(string str)
        {
            if (str == null || str.Length == 0)
                return false;
            foreach (char c in str)
            {
                if (!Char.IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 显示自定义窗体
        /// </summary>
        private void AddWindow()
        {
            try
            {
                Window window = new Window();
                window.ID = "PrivAdd";
                window.Title = GetLangStr("PrivManager41", "增加权限");

                window.Width = Unit.Pixel(360);
                window.Height = Unit.Pixel(220);
                window.Modal = true;
                window.Maximizable = false;
                window.Resizable = false;
                window.Hidden = true;
                window.AutoLoad.Mode = LoadMode.Merge;
                window.Layout = "FitLayout";

                Ext.Net.FormPanel tab = new Ext.Net.FormPanel();
                tab.MonitorValid = true;
                tab.Title = GetLangStr("PrivManager42", "权限信息");
                tab.Header = false;
                tab.Padding = 20;
                tab.Height = 200;

                TextField txtPrivId = CommonExt.AddTextField("txtAPrivID", GetLangStr("PrivManager9", "权限编号"));
                txtPrivId.Text = tgsPproperty.GetMinRecordId();
                txtPrivId.Disabled = true;
                tab.Items.Add(txtPrivId);
                tab.Items.Add(CommonExt.AddTextFieldWidth("txtAPrivName", GetLangStr("PrivManager10", "权限名称"), false, GetLangStr("PrivManager45", "请输入权限名称")));
                tab.Items.Add(CommonExt.AddTextFieldWidth("txtARemark", GetLangStr("PrivManager11", "权限描述"), true, GetLangStr("PrivManager47", "请输入权限描述")));
                tab.Buttons.Add(CommonExt.AddButton("butSaveEdit", GetLangStr("PrivManager13", "保存"), "Disk", "PrivManager.InfoSave()"));
                tab.Buttons.Add(CommonExt.AddButton("butCancelEdit", GetLangStr("PrivManager49", "取消"), "Cancel", window.ClientID + ".hide()"));
                tab.Listeners.ClientValidation.Handler = "butSaveEdit.setDisabled(!valid);";
                window.Items.Add(tab);
                window.Render(this.Form);
                window.Show();
            }

            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-AddWindow", ex.Message+"；"+ex.StackTrace, "AddWindow has an exception");
            }
        }
        /// <summary>
        /// 权限查询
        /// </summary>
        /// <param name="where"></param>
        private void PrivDataBind(string where)
        {
            try
            {
                DataTable dt = settingManager.GetSerPrivInfo(SystemID, where);
                StorePriv.DataSource = dt;
                StorePriv.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-PrivDataBind", ex.Message+"；"+ex.StackTrace, "PrivDataBind has an exception");
            }
        }
        /// <summary>
        /// 获取第一个权限信息
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private DataTable GetFirstDataTable(DataRow dr)
        {
            try
            {
                DataTable dt = settingManager.GetSerPrivFunc(dr["col1"].ToString());
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-GetFirstDataTable", ex.Message+"；"+ex.StackTrace, "GetFirstDataTable has an exception");
                return null;
            }
        }
        /// <summary>
        ///提示信息
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
        /// 获取权限列表
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="dtprv"></param>
        /// <param name="formType"></param>
        /// <returns></returns>
        private Ext.Net.TreeNodeCollection BuildTree(Ext.Net.TreeNodeCollection nodes, DataTable dtprv, string formType)
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
                DataTable dt = settingManager.GetSettingContent(formType);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (formType == "0")
                    {
                        Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                        node.Text = dt.Rows[i][1].ToString();
                        node.Icon = Icon.House;
                        node.NodeID = dt.Rows[i][0].ToString();
                        AddreeBuildTree(node, node.NodeID, dtprv, formType);
                        node.Expanded = true;
                        root.Nodes.Add(node);
                    }
                    else
                    {
                        Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                        node.Text = dt.Rows[i][1].ToString();
                        node.Icon = Icon.House;
                        node.NodeID = dt.Rows[i][0].ToString();
                        Addree(node, node.NodeID, dtprv, formType);
                        node.Expanded = true;
                        root.Nodes.Add(node);
                    }
                }
                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-BuildTree", ex.Message+"；"+ex.StackTrace, "BuildTree has an exception");
                return null;
            }
        }

        /// <summary>
        /// 获取次一级权限列表(首页、智慧态势、智慧地图…)
        /// </summary>
        /// <param name="root"></param>
        /// <param name="systemId"></param>
        /// <param name="dtprv"></param>
        /// <param name="formType"></param>
        private void AddreeBuildTree(Ext.Net.TreeNode root, string systemId, DataTable dtprv, string formType)
        {
            try
            {
                DataTable dt = settingManager.GetSettingContent(systemId, formType);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    node.Text = dt.Rows[i][2].ToString();
                    node.Icon = Icon.Package;
                    node.NodeID = dt.Rows[i][1].ToString();
                    Addree(node, systemId, dtprv, formType);
                    //node.Checked = ThreeStateBool.False;
                    node.Expanded = true;
                    root.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-AddreeBuildTree", ex.Message+"；"+ex.StackTrace, "AddreeBuildTree has an exception");
            }
        }

        /// <summary>
        /// 获取底层一级权限列表(首页、模板管理功能、实时车流…)
        /// </summary>
        /// <param name="root"></param>
        /// <param name="systemId"></param>
        /// <param name="dtprv"></param>
        /// <param name="formType"></param>
        private void Addree(Ext.Net.TreeNode root, string systemId, DataTable dtprv, string formType)
        {
            try
            {
                DataTable dt = settingManager.GetContentFunction(systemId, root.NodeID, formType);
                 //DataTable dtCopy= dt.Copy();
                 //for (int i = 0; i < dtCopy.Rows.Count; i++)
                 //{
                 //    dtRole.Rows.Add(dtCopy.Rows[i].ItemArray);
                 //}
                 for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dtRole.Rows.Add(dt.Rows[i].ItemArray);
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    node.Text = dt.Rows[i][1].ToString();
                    node.Leaf = true;
                    node.Icon = Icon.Page;
                    node.Checked = ThreeStateBool.False;

                    if (dtprv != null)
                    {
                        for (int j = 0; j < dtprv.Rows.Count; j++)
                        {
                            if (dtprv.Rows[j][1].ToString() == dt.Rows[i][0].ToString())
                            {
                                node.Checked = ThreeStateBool.True;
                            }
                        }
                    }
                    node.NodeID = dt.Rows[i][0].ToString();
                    node.Expanded = true;
                    root.Nodes.Add(node);
                    
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PrivManager.aspx-Addree", ex.Message+"；"+ex.StackTrace, "Addree has an exception");
            }
        }
        //创建内存表的列并且赋值
        public static DataTable GetDt()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("col0", typeof(string));//创建string类型
            dt.Columns.Add("col1", typeof(string));
            dt.Columns.Add("col2", typeof(string));//创建string类型
            dt.Columns.Add("col3", typeof(string));
            dt.Columns.Add("col4", typeof(string));//创建string类型
            dt.Columns.Add("col5", typeof(string));
            dt.Columns.Add("col6", typeof(string));
            return dt;
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
    }
}