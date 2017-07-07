using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;

// ***********************************************************************
// Assembly         : MyNet.Atmcs.Uscmcp.Web
// Author           : zlsyl
// Created          : 07-20-2016
//
// Last Modified By : zlsyl
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="Department.aspx.cs" company="ZKLT">
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
    /// Class Department.
    /// </summary>
    public partial class Department : System.Web.UI.Page
    {
        #region 成员变量

        /// <summary>
        /// The system manager
        /// </summary>
        private SystemManager systemManager = new SystemManager();

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        /// <summary>
        /// The setting manager
        /// </summary>
        private SettingManager settingManager = new SettingManager();

        /// <summary>
        /// The system identifier
        /// </summary>
        private string SystemID = "00";

        /// <summary>
        /// The user login
        /// </summary>
        private UserLogin userLogin = new UserLogin();

        /// <summary>
        /// 获取用户名
        /// </summary>
        private static string uName;

        /// <summary>
        /// 获取ip
        /// </summary>
        private static string nowIp;

        /// <summary>
        /// 获取机构名称
        /// </summary>
        private static string jgname;

        private static int typenum = 1;

        /// <summary>
        /// 机构名称
        /// </summary>
        private static string jgmc;

        /// <summary>
        /// 机构级别
        /// </summary>
        private static string jgjb;

        /// <summary>
        /// 所属机构
        /// </summary>
        private static string ssjg;

        /// <summary>
        /// 工作内容
        /// </summary>
        private static string gznr;

        /// <summary>
        /// 工作地址
        /// </summary>
        private static string gzdz;

        /// <summary>
        /// 负责人
        /// </summary>
        private static string fzr;

        /// <summary>
        /// 负责人电话
        /// </summary>
        private static string fzrdh;

        /// <summary>
        /// 负责人手机
        /// </summary>
        private static string fzrsj;

        /// <summary>
        /// 办公电话1
        /// </summary>
        private static string bgdh1;

        /// <summary>
        /// 办公电话2
        /// </summary>
        private static string bgdh2;

        /// <summary>
        /// 办公电话3
        /// </summary>
        private static string bgdh3;

        /// <summary>
        /// 传真号码
        /// </summary>
        private static string czhm;

        /// <summary>
        /// 邮政编码
        /// </summary>
        private static string yzbm;

        private static DataTable dtSsjg = null;

        private static string yhid;

        private static DataTable dtm = null;

        private static string dtjmid = "";

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
                string js = "alert('" + GetLangStr("Department18", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                SystemID = "00";
                StoreClass.DataSource =GetRedisData.GetData("t_sys_code:140004");
                StoreClass.DataBind();

                DataTable dt = settingManager.GetConfigDepartment(SystemID);//Bll.Common.ChangColName(ToDataTable(GetRedisData.GetData("t_cfg_department").Select("", "class asc,departid asc")));
                while (dt.Columns.Count > 3)
                {
                    dt.Columns.RemoveAt(3);
                }
                dtSsjg = dt;

                Storedepart.DataSource = dt;
                jgname = dt.Rows[0]["col2"].ToString();
                Storedepart.DataBind();
                FirstGetSysConfig();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                uName = userinfo.UserName;
                nowIp = userinfo.NowIp;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("Department18", "访问：") + Request.QueryString["funcname"], userinfo.NowIp, "0");
            }
            this.DataBind();
        }

        /// <summary>
        /// 选择系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void cmbSystem_Select(object sender, DirectEventArgs e)
        {
            try
            {
                string sysid = CmbSystem.SelectedItem.Value;
                CurrentSystemId.Value = sysid;
                DepartDataBind(sysid);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("Department.aspx-cmbSystem_Select", ex.Message + "；" + ex.StackTrace, "cmbSystem_Select has an exception");
            }
        }

        #endregion 控件事件

        #region DirectMethod 方法

        /// <summary>
        /// 显示详细信息
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        [DirectMethod]
        public void ShowInfo(string nodeId)
        {
            try
            {
                dtjmid = nodeId;
                this.txtDepartId.Text = nodeId;
                DataTable dt = settingManager.GetConfigDepartmentInfo(nodeId, SystemID);
                //var table = ToDataTable(GetRedisData.GetData("t_cfg_department").Select("", "class asc,departid asc"));
                //DataTable dt = Bll.Common.ChangColName(table);
                if (dt != null)
                {
                    DataRow[] drs = dt.Select("col1='" + nodeId + "'");
                    ShowInfoData(drs[0]);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("Department.aspx-ShowInfo", ex.Message + "；" + ex.StackTrace, "ShowInfo has an exception");
            }
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void DoConfirm()
        {
            try
            {
                if (!string.IsNullOrEmpty(nowId.Value.ToString()))
                {
                    string Id = nowId.Value.ToString();
                    yhid = Id;
                    X.Msg.Confirm(GetLangStr("Department34", "信息"), GetLangStr("Department35", "确认要删除[") + jgname + "]?", new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "Department.DoYes()",
                            Text = GetLangStr("Department36", "是")
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "Department.DoNo()",
                            Text = GetLangStr("Department37", "否")
                        }
                    }).Show();
                }
                else
                {
                    Notice(GetLangStr("Department38", "信息删除"), GetLangStr("Department39", "删除失败！"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("Department.aspx-DoConfirm", ex.Message + "；" + ex.StackTrace, "DoConfirm has an exception");
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
                hs.Add("id", nowId.Value.ToString());
                DataRow[] dr = dtm.Select("col0='" + dtjmid + "'");
                string jgm = dr[0]["col1"].ToString();
                if (settingManager.DeleteDepartmentInfo(hs) > 0)
                {
                    logManager.InsertLogRunning(uName, GetLangStr("Department40", "删除:名称为[") + jgm + GetLangStr("Department41", "]机构"), nowIp, "3");
                    Notice(GetLangStr("Department38", "信息删除"), GetLangStr("Department42", "删除成功！"));
                    DepartDataBind(SystemID);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("Department.aspx-DoYes", ex.Message + "；" + ex.StackTrace, "DoYes has an exception");
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
        /// Informations the save.
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void InfoSave()
        {
            try
            {
                typenum = 0;
                string RegionId = settingManager.GetSettingRegionId(SystemID);
                string id = systemManager.GetRecordID(RegionId, 12);
                newId.Value = id;
                CmbDepart.Disabled = false;
                txtDepartName.Text = "";
                txtDepartId.Text = "";
                CmbDepart.Value = "";
                CmbClass.Value = "";
                TxtWorkContent.Text = "";
                TxtWorkAddress.Text = "";
                TxtManage.Text = "";
                TxtManagemoble.Text = "";
                TxtManagePhone.Text = "";
                TxtOfficePhone.Text = "";
                TxtOfficePhone2.Text = "";
                TxtOfficePhone3.Text = "";
                txtOfficefax.Text = "";
                txtPostcode.Text = "";
                txtDepartId.Disabled = false;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("Department.aspx-InfoSave", ex.Message + "；" + ex.StackTrace, "InfoSave has an exception");
            }
        }

        /// <summary>
        /// 得到机构
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void GetDepart()
        {
            try
            {
                if (!string.IsNullOrEmpty(CmbClass.SelectedItem.Text))
                {
                    string text = CmbClass.SelectedItem.Text;
                    DataTable dt = settingManager.GetConfigDepartment(SystemID);
                    while (dt.Columns.Count > 3)
                    {
                        dt.Columns.RemoveAt(3);
                    }
                    if (text.Equals(GetLangStr("Department43", "总队")))
                    {
                        //Storedepart.DataSource = new DataTable();
                        //Storedepart.DataBind();
                        CmbDepart.Value = "";
                        CmbDepart.Disabled = true;
                    }
                    else if (text.Equals(GetLangStr("Department44", "支队")))
                    {
                        CmbDepart.Value = "";
                        CmbDepart.Disabled = false;
                        DataRow[] rows = dt.Select("col2 like '%" + text + "%'");
                        for (int i = 0; i < rows.Length; i++)
                        {
                            dt.Rows.Remove(rows[i]);
                        }
                        DataRow[] rows1 = dt.Select("col2 like '%" + GetLangStr("Department45", "大队") + "%'");
                        for (int i = 0; i < rows1.Length; i++)
                        {
                            dt.Rows.Remove(rows1[i]);
                        }
                        DataRow[] rows2 = dt.Select("col2 like '%" + GetLangStr("Department46", "中队") + "%'");
                        for (int i = 0; i < rows2.Length; i++)
                        {
                            dt.Rows.Remove(rows2[i]);
                        }

                        DataRow[] rows3 = dt.Select("col2 like '%" + GetLangStr("Department43", "总队") + "%'");
                        {
                            if (rows3.Length <= 0)
                            {
                                CmbDepart.Disabled = true;
                            }
                        }
                    }
                    else if (text.Equals(GetLangStr("Department45", "大队")))
                    {
                        CmbDepart.Value = "";
                        CmbDepart.Disabled = false;
                        DataRow[] rows1 = dt.Select("col2 like '%" + text + "%'");
                        for (int i = 0; i < rows1.Length; i++)
                        {
                            dt.Rows.Remove(rows1[i]);
                        }
                        DataRow[] rows2 = dt.Select("col2 like '%" + GetLangStr("Department46", "中队") + "%'");
                        for (int i = 0; i < rows2.Length; i++)
                        {
                            dt.Rows.Remove(rows2[i]);
                        }

                        DataRow[] rows3 = dt.Select("col2 like '%" + GetLangStr("Department44", "支队") + "%'");
                        {
                            if (rows3.Length <= 0)
                            {
                                CmbDepart.Disabled = true;
                            }
                        }
                    }
                    else if (text.Equals(GetLangStr("Department46", "中队")))
                    {
                        CmbDepart.Value = "";
                        CmbDepart.Disabled = false;
                        DataRow[] rows1 = dt.Select("col2 like '%" + text + "%'");
                        for (int i = 0; i < rows1.Length; i++)
                        {
                            dt.Rows.Remove(rows1[i]);
                        }

                        DataRow[] rows2 = dt.Select(" col2 like '%" + GetLangStr("Department44", "支队") + "%'");
                        for (int i = 0; i < rows2.Length; i++)
                        {
                            dt.Rows.Remove(rows2[i]);
                        }

                        DataRow[] rows3 = dt.Select("col2 like '%" + GetLangStr("Department45", "大队") + "%'");
                        {
                            if (rows3.Length <= 0)
                            {
                                CmbDepart.Disabled = true;
                            }
                        }
                    }
                    else
                    {
                    }
                    Storedepart.DataSource = dt;
                    Storedepart.DataBind();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("Department.aspx-GetDepart", ex.Message + "；" + ex.StackTrace, "GetDepart has an exception");
            }
        }

        /// <summary>
        /// 更新保存数据
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void UpdateData()
        {
            try
            {
                txtDepartId.Disabled = true;
                if (txtDepartId.Text.Length < 12)
                {
                    Notice(GetLangStr("Department47", "信息提示"), GetLangStr("Department48", "机构代码不够12位！"));
                    return;
                }
                Hashtable hs = new Hashtable();
                if (newId.Value != null && newId.Value.ToString() != "")
                {
                    hs.Add("id", newId.Value.ToString());
                }
                else
                {
                    hs.Add("id", nowId.Value.ToString());
                }
                hs.Add("departid", txtDepartId.Text);
                hs.Add("departname", txtDepartName.Text);
                if (CmbDepart.SelectedIndex != -1)
                {
                    if (CmbClass.Value.ToString() == "1")
                    {
                        hs.Add("classcode", "000000000000");
                    }
                    else
                    {
                        hs.Add("classcode", CmbDepart.Value);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(CmbDepart.Value.ToString()))
                    {
                        hs.Add("classcode", "000000000000");
                    }
                }
                if (CmbClass.SelectedIndex != -1)
                {
                    hs.Add("class", CmbClass.Value);
                }
                hs.Add("workcontent", TxtWorkContent.Text);
                hs.Add("workaddress", TxtWorkAddress.Text);
                hs.Add("manager", TxtManage.Text);
                hs.Add("managermobile", TxtManagemoble.Text);
                hs.Add("managerphone", TxtManagePhone.Text);
                hs.Add("officephone", TxtOfficePhone.Text);
                hs.Add("officephone2", TxtOfficePhone2.Text);
                hs.Add("officephone3", TxtOfficePhone3.Text);
                hs.Add("officefax", txtOfficefax.Text);
                hs.Add("postcode", txtPostcode.Text);
                hs.Add("systemid", SystemID);
                if (settingManager.UpdateDepartmentInfo(hs) > 0)
                {
                    if (typenum == 0)
                    {
                        if (newId.Value != null && newId.Value.ToString() != "")
                        {
                            string jgbh = TxtId.Text;
                            string jgmc = txtDepartName.Text;
                            string jgjb = CmbClass.SelectedItem.Text;
                            string ssjg = CmbDepart.SelectedItem.Text;
                            string lblname = "";
                            lblname += Bll.Common.AssembleRunLog("", jgmc, GetLangStr("Department15", "机构名称"), "0");
                            lblname += Bll.Common.AssembleRunLog("", jgjb, GetLangStr("Department16", "机构级别"), "0");
                            lblname += Bll.Common.AssembleRunLog("", ssjg, GetLangStr("Department19", "所属机构"), "0");
                            logManager.InsertLogRunning(uName, GetLangStr("Department49", "新增:机构[") + lblname + "]" + lblname, nowIp, "1");
                            Notice(GetLangStr("Department47", "信息提示"), GetLangStr("Department51", "添加成功！"));
                            DepartDataBind(SystemID);
                            newId.Value = null;
                            nowId.Value = null;
                            typenum = 1;
                        }
                        else
                        {
                            Notice(GetLangStr("Department47", "信息提示"), GetLangStr("", "添加失败！"));
                        }
                    }
                    else
                    {
                        if (nowId.Value != null && nowId.Value.ToString() != "")
                        {
                            string jgmcs = txtDepartName.Text;
                            string jgjbs = CmbClass.SelectedItem.Text;
                            string ssjgs = CmbDepart.SelectedItem.Text;
                            string gznrs = TxtWorkContent.Text;
                            string gzdzs = TxtWorkAddress.Text;
                            string fzrs = TxtManage.Text;
                            string fzrdhs = TxtManagemoble.Text;
                            string fzrsjs = TxtManagePhone.Text;
                            string bgdh1s = TxtOfficePhone.Text;
                            string bgdh2s = TxtOfficePhone2.Text;
                            string bgdh3s = TxtOfficePhone3.Text;
                            string czhms = txtOfficefax.Text;
                            string yzbms = txtPostcode.Text;
                            string lblname = "";
                            lblname += Bll.Common.AssembleRunLog(jgmc, jgmcs, GetLangStr("Department7", "机构名称"), "1");
                            lblname += Bll.Common.AssembleRunLog(jgjb, jgjbs, GetLangStr("Department16", "机构级别"), "1");
                            lblname += Bll.Common.AssembleRunLog(ssjg, ssjgs, GetLangStr("Department19", "所属机构"), "1");
                            lblname += Bll.Common.AssembleRunLog(gznr, gznrs, GetLangStr("Department21", "工作内容"), "1");
                            lblname += Bll.Common.AssembleRunLog(gzdz, gzdzs, GetLangStr("Department22", "工作地址"), "1");
                            lblname += Bll.Common.AssembleRunLog(fzr, fzrs, GetLangStr("Department23", "负责人"), "1");
                            lblname += Bll.Common.AssembleRunLog(fzrdh, fzrdhs, GetLangStr("Department24", "负责人电话"), "1");
                            lblname += Bll.Common.AssembleRunLog(fzrsj, fzrsjs, GetLangStr("Department25", "负责人手机"), "1");
                            lblname += Bll.Common.AssembleRunLog(bgdh1, bgdh1s, GetLangStr("Department26", "办公电话1"), "1");
                            lblname += Bll.Common.AssembleRunLog(bgdh2, bgdh2s, GetLangStr("Department27", "办公电话2"), "1");
                            lblname += Bll.Common.AssembleRunLog(bgdh3, bgdh3s, GetLangStr("Department28", "办公电话3"), "1");
                            lblname += Bll.Common.AssembleRunLog(czhm, czhms, GetLangStr("Department29", "传真号码"), "1");
                            lblname += Bll.Common.AssembleRunLog(yzbm, yzbms, GetLangStr("Department30", "邮政编码"), "1");
                            logManager.InsertLogRunning(uName, GetLangStr("Department52", "修改:[") + jgname + "]" + lblname, nowIp, "2");
                            Notice(GetLangStr("Department53", "信息更新"), GetLangStr("Department54", "更新成功！"));
                            DepartDataBind(SystemID);
                            typenum = 1;
                        }
                        else
                        {
                            Notice(GetLangStr("Department47", "信息提示"), GetLangStr("Department55", "更新失败！"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("Department.aspx-UpdateData", ex.Message + "；" + ex.StackTrace, "UpdateData has an exception");
            }
        }

        #endregion DirectMethod 方法

        #region 私有方法

        public DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        /// 根据系统编号绑定部门信息
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        private void DepartDataBind(string sysid)
        {
            try
            {
                SystemID = sysid;
                DataTable dt = dtm = settingManager.GetDepartmentDict(sysid);
                //var table = ToDataTable(GetRedisData.GetData("t_cfg_department").Select("", "class asc,departid asc"));
                //DataTable dt = table;
                if (dt != null && dt.Rows.Count > 0)
                {
                    BuildTree(TreeGrid1.Root);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("Department.aspx-DepartDataBind", ex.Message + "；" + ex.StackTrace, "DepartDataBind has an exception");
            }
        }

        /// <summary>
        /// 加载系统信息
        /// </summary>
        /// <returns></returns>
        private void FirstGetSysConfig()
        {
            try
            {
                DataTable objData = systemManager.GetSystemInfo();
                this.StoreSystem.DataSource = objData;
                this.StoreSystem.DataBind();
                if (string.IsNullOrEmpty(SystemID))
                {
                    SystemID = objData.Rows[0][0].ToString();
                    CmbSystem.Hidden = false;
                }
                else
                {
                    CmbSystem.Hidden = true;
                }
                CmbSystem.SelectedItem.Value = SystemID;
                DepartDataBind(SystemID);
                CurrentSystemId.Value = SystemID;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("Department.aspx-FirstGetSysConfig", ex.Message + "；" + ex.StackTrace, "FirstGetSysConfig has an exception");
            }
        }

        #region 递归产生系统表树形菜单节点

        /// <summary>
        /// 将部门信息绑定至tree
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private void BuildTree(Ext.Net.TreeNodeCollection nodes)
        {
            try
            {
                TreeGrid1.RemoveAll(true);
                ClearInfoData();
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }

                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Text = "Root";
                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;
                //var table = ToDataTable(GetRedisData.GetData("t_cfg_department").Select("", "class asc,departid asc"));
                //DataTable dt = Bll.Common.ChangColName(table);

                DataTable dt = settingManager.GetConfigDepartment(SystemID);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Addree(dt, dt.Rows[0]["col3"].ToString(), root, null);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("Department.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
            }
            if (X.IsAjaxRequest)
            {
                TreeGrid1.Render(PanelNavigate, RenderMode.Auto);
            }
        }

        /// <summary>
        /// 遍历将子部门挂接至父部门
        /// </summary>
        /// <param name="allNodeTable"></param>
        /// <param name="parentColValue"></param>
        /// <param name="root"></param>
        /// <param name="ParentNode"></param>
        /// <returns></returns>
        private void Addree(DataTable allNodeTable, string parentColValue, Ext.Net.TreeNode root, Ext.Net.TreeNode ParentNode)
        {
            try
            {
                DataRow[] myDataRows = allNodeTable.Select("col3 ='" + parentColValue + "'");

                foreach (DataRow myDataRow in myDataRows)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    ConfigItem ci0 = new ConfigItem("col0", myDataRow[2].ToString(), ParameterMode.Value);
                    ConfigItem ci1 = new ConfigItem("col1", myDataRow[7].ToString(), ParameterMode.Value);
                    ConfigItem ci2 = new ConfigItem("col2", myDataRow[8].ToString(), ParameterMode.Value);
                    //ConfigItem ci3 = new ConfigItem("col3", myDataRow[8].ToString(), ParameterMode.Value);
                    node.Text = myDataRow[2].ToString();
                    node.NodeID = myDataRow[1].ToString();
                    node.Leaf = true;
                    node.Draggable = true;
                    node.Expandable = ThreeStateBool.True;
                    node.Expanded = true;
                    node.Icon = Icon.Telephone;
                    node.CustomAttributes.Add(ci0);
                    node.CustomAttributes.Add(ci1);
                    node.CustomAttributes.Add(ci2);
                    //node.CustomAttributes.Add(ci3);

                    if (ParentNode != null)
                    {
                        ParentNode.Nodes.Add(node);
                        Addree(allNodeTable, myDataRow["col1"].ToString(), ParentNode, node);
                    }
                    else
                    {
                        root.Nodes.Add(node);
                        Addree(allNodeTable, myDataRow["col1"].ToString(), root, node);
                        ShowInfoData(myDataRow);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("Department.aspx-Addree", ex.Message + "；" + ex.StackTrace, "Addree has an exception");
            }
        }

        #endregion 递归产生系统表树形菜单节点

        /// <summary>
        /// 清理控件
        /// </summary>
        /// <returns></returns>
        public void ClearInfoData()
        {
            TxtId.Reset();
            txtDepartName.Reset();
            txtDepartId.Reset();

            CmbDepart.Reset();

            CmbClass.Reset();
            TxtWorkContent.Reset();
            TxtWorkAddress.Reset();
            TxtManage.Reset();
            TxtManagemoble.Reset();
            TxtManagePhone.Reset();
            TxtOfficePhone.Reset();
            TxtOfficePhone2.Reset();
            TxtOfficePhone3.Reset();
            txtOfficefax.Reset();
            txtPostcode.Reset();
        }

        /// <summary>
        /// 显示选中行详细信息
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public void ShowInfoData(DataRow dr)
        {
            try
            {
                ClearInfoData();
                TxtId.Text = dr["col0"].ToString();
                nowId.Value = dr["col0"].ToString();
                txtDepartName.Text = dr["col2"].ToString();
                this.txtDepartId.Text = dr["col1"].ToString();
                string code = dr["col3"].ToString();

                if (code == "000000")
                {
                    CmbDepart.Value = dr["col3"].ToString();
                }
                else
                {
                    CmbDepart.Value = dr["col3"].ToString();
                }
                CmbClass.SetValue(dr["col4"].ToString());
                TxtWorkContent.Text = dr["col5"].ToString();
                TxtWorkAddress.Text = dr["col6"].ToString();
                TxtManage.Text = dr["col7"].ToString();
                TxtManagemoble.Text = dr["col8"].ToString();
                TxtManagePhone.Text = dr["col9"].ToString();
                TxtOfficePhone.Text = dr["col10"].ToString();
                TxtOfficePhone2.Text = dr["col11"].ToString();
                TxtOfficePhone3.Text = dr["col12"].ToString();
                txtOfficefax.Text = dr["col13"].ToString();
                txtPostcode.Text = dr["col14"].ToString();
                jgname = txtDepartName.Text;
                //获取行选中事件文本值
                jgmc = txtDepartName.Text;
                jgjb = CmbClass.SelectedItem.Text;
                if (!string.IsNullOrEmpty(dr["col3"].ToString()))
                {
                    DataRow[] rows = dtSsjg.Select("col1=" + dr["col3"].ToString());
                    if (rows.Length > 0)
                    {
                        ssjg = rows[0]["col2"].ToString();
                    }
                }

                gznr = TxtWorkContent.Text;
                gzdz = TxtWorkAddress.Text;
                fzr = TxtManage.Text;
                fzrdh = TxtManagemoble.Text;
                fzrsj = TxtManagePhone.Text;
                bgdh1 = TxtOfficePhone.Text;
                bgdh2 = TxtOfficePhone2.Text;
                bgdh3 = TxtOfficePhone3.Text;
                czhm = txtOfficefax.Text;
                yzbm = txtPostcode.Text;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("Department.aspx-ShowInfoData", ex.Message + "；" + ex.StackTrace, "ShowInfoData has an exception");
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