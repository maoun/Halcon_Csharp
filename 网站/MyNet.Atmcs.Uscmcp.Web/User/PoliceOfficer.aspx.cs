using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PoliceOfficer : System.Web.UI.Page
    {
        #region 成员变量

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private SettingManager settingManager = new SettingManager();
        private SystemManager systemManager = new SystemManager();
        private DeviceManager deviceManager = new DeviceManager();
        private ServiceManager serviceManager = new ServiceManager();
        private UserLogin userLogin = new UserLogin();
        private static string lblname;//日志变量
        private static string sw; // 所属变量
        private static string xb;//性别
        private static string xm;//姓名
        private static string cq;//出生日期
        private static string jz;//家庭住址
        private static string sh;//身份证号
        private static string jh;//警号
        private static string jx;//警衔
        private static string xw;//现职务
        private static string bb;//编制类别
        private static string tb;//特勤类别
        private static string sj;//手机
        private static string bh;//办公电话
        private static string st;//手台
        private static string sth;//手台呼号
        private static string szh;///手台组号
        private static string jb;//警用装备
        private static string xbie;

        static string uName;
        static string nowIp;


        DataTable dtsex = null;


        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("PoliceOfficer16", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); 
                return; 
            }
            if (!X.IsAjaxRequest)
            {
                BuildTree(TreePanel1.Root);
              
                StoreDataBind();
                GridDataBind(GetWhere());
                this.DataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                uName = userinfo.UserName;
                nowIp = userinfo.NowIp;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("PoliceOfficer17", "访问：") + Request.QueryString["funcname"], userinfo.NowIp, "0");
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                GridDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            try
            {
                CmbSex.Reset();
                CmbDutyType.Reset();
                uiDepartment.Reset();
                TxtDeviceName.Reset();
                TxTJingHao.Reset();
                this.ResourceManager1.RegisterAfterClientInitScript("clearTime();");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-ButResetClick", ex.Message+"；"+ex.StackTrace, "ButResetClick has an exception");
            }
        }

        /// <summary>
        ///导出为xml
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
                logManager.InsertLogError("PoliceOfficer.aspx-ToXml", ex.Message+"；"+ex.StackTrace, "ToXml has an exception");
            }
        }

        /// <summary>
        ///导出为Excel
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
                logManager.InsertLogError("PoliceOfficer.aspx-ToExcel", ex.Message+"；"+ex.StackTrace, "ToExcel has an exception");
            }
        }

        /// <summary>
        ///导出为Csv
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
                logManager.InsertLogError("PoliceOfficer.aspx-ToCsv", ex.Message+"；"+ex.StackTrace, "ToCsv has an exception");
            }
        }

        /// <summary>
        /// 打印
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
                    string xml = Bll.Common.GetPrintXml(GetLangStr("policeOffcer49", "警务人员信息"), "", "", "printdatatable");
                    string js = "OpenPrintPageV(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-ButPrintClick", ex.Message+"；"+ex.StackTrace, "ButPrintClick has an exception");
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButRefreshClick(object sender, DirectEventArgs e)
        {
            try
            {
                GridDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-ButRefreshClick", ex.Message+"；"+ex.StackTrace, "ButRefreshClick has an exception");
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
                GridDataBind(GetWhere());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-MyData_Refresh", ex.Message+"；"+ex.StackTrace, "MyData_Refresh has an exception");
            }
        }

        #region 增删改事件

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Select_LeiXing(object sender, DirectEventArgs e)
        {
            ButUpdate.Text = GetLangStr("policeOffcer50", "添加");
        }

        /// <summary>
        ///更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GengXin_Click(object sender, DirectEventArgs e)
        {
            try
            {
               
                string ID = Guid.NewGuid().ToString();
                Hashtable hs = new Hashtable();
                //当没选择所属单位的时候  默认为371600000000 必须要DEPARTID
                if (string.IsNullOrEmpty(uiDepartment1.DepertId))
                {
                    hs.Add("DEPARTID", "371600000000");
                }
                else
                {
                    hs.Add("DEPARTID", uiDepartment1.DepertId);
                }

                hs.Add("NAME", this.TxtName.Text.ToString());

                //警号
                if (TxtSiren.Text != "")
                {
                    //  if(TxtSiren.Text== ){
                    hs.Add("SIREN", this.TxtSiren.Text.ToString());
                    //}
                }
                else
                {
                    hs.Add("SIREN", "");
                }
                if (this.CmbRanks.SelectedIndex != -1)
                {
                    hs.Add("RANKS", this.CmbRanks.Text.ToString());
                }
                else
                {
                    hs.Add("RANKS", "");
                }
                if (this.CmbDuty.SelectedIndex != -1)
                {
                    hs.Add("PROFESSION", this.CmbDuty.Text.ToString());
                }
                else
                {
                    hs.Add("PROFESSION", "");
                }
                if (this.CmbFormat.SelectedIndex != -1)
                {
                    hs.Add("PREPARATIONTYPE", this.CmbFormat.Text.ToString());
                }
                else
                {
                    hs.Add("PREPARATIONTYPE", "");
                }
                if (this.CmbTeQin.SelectedIndex != -1)
                {
                    hs.Add("SECRETLEVEL", this.CmbTeQin.Text.ToString());
                }
                else
                {
                    hs.Add("SECRETLEVEL", "");
                }
                hs.Add("PHONE", this.TxtPhone.Text.ToString());
                if (TxtHandSets.Text != "")
                {
                    hs.Add("HANDSETS", this.TxtHandSets.Text.ToString());
                }
                else
                {
                    hs.Add("HANDSETS", "");
                }
                if (TxtOfficePhone.Text != "")
                {
                    hs.Add("OFFICEPHONE", this.TxtOfficePhone.Text.ToString());
                }
                else
                {
                    hs.Add("OFFICEPHONE", "");
                }
                if (TxtHandSetsCode.Text != "")
                {
                    hs.Add("HANDSETSCODE", this.TxtHandSetsCode.Text.ToString());
                }
                else
                {
                    hs.Add("HANDSETSCODE", "");
                }

                if (TxtHandSetsGroup.Text != "")
                {
                    hs.Add("HANDSETSGROUP", this.TxtHandSetsGroup.Text.ToString());
                }
                else
                {
                    hs.Add("HANDSETSGROUP", "");
                }

                if (this.CmbMale.SelectedIndex != -1)
                {
                    hs.Add("SEX", this.CmbMale.Text.ToString());
                }
                else
                {
                    hs.Add("SEX", "");
                }
                if (TxtAddress.Text != "")
                {
                    hs.Add("ADDRESS", this.TxtAddress.Text.ToString());
                }
                else
                {
                    hs.Add("ADDRESS", "");
                }
                string lurutime = DateField1.SelectedDate.ToString("yyyy-MM-dd") + " 00:00:00";
                hs.Add("BIRTHDAY", lurutime);
                if (TxtEqip.Text != "")
                {
                    hs.Add("POLICEEQUIPMENT", TxtEqip.Text.ToString());
                }
                else
                {
                    hs.Add("POLICEEQUIPMENT", "");
                }

                if (TxtIdNo.Text != "")
                {
                    hs.Add("IDNO", TxtIdNo.Text.ToString());
                }
                else
                {
                    hs.Add("IDNO", "");
                }
                DataTable dt = Session["datatable"] as DataTable;
               
                if (Hidden1.Value.ToString() == "1")
                {
                    DataRow[] rows = dt.Select("col9 ='" + TxtSiren.Text.ToString() + "'");
                    if (rows.Length > 0)
                    {
                        Notice(GetLangStr("policeOffcer51", "信息提示"), GetLangStr("PoliceOfficer52", "警号重复"));
                        return;
                    }
                    hs.Add("USERCODE", serviceManager.GetRecordID());
                    if (serviceManager.insertSevice(hs) > 0)
                    {
                        Window1.Hide();
                        Notice(GetLangStr("policeOffcer51", "信息提示"), GetLangStr("policeOffcer53", "添加成功"));
                        lblname = "";

                        lblname += Bll.Common.AssembleRunLog("", TxtName.Text, GetLangStr("PoliceOfficer18", "姓名"), "0");
                        lblname += Bll.Common.AssembleRunLog("", xbie, GetLangStr("PoliceOfficer19", "性别"), "0");
                        lblname += Bll.Common.AssembleRunLog("", TxtSiren.Text, GetLangStr("PoliceOfficer20", "警号"), "0");
                        logManager.InsertLogRunning(uName, GetLangStr("PoliceOfficer50", "添加") + lblname, nowIp, "1");

                        GridDataBind(GetWhere());
                        BuildTree(TreePanel1.Root);
                        TreePanel1.Render();
                    }
                    else
                    {
                        Notice(GetLangStr("policeOffcer51", "信息提示"), GetLangStr("policeOffcer53", "添加失败"));
                    }
                }
                if (Hidden1.Value.ToString() == "2")
                {
                    DataRow[] rows = dt.Select("col9 ='" + TxtSiren.Text.ToString() + "'");
                    if (rows.Length > 1)
                    {
                        Notice(GetLangStr("policeOffcer51", "信息提示"), GetLangStr("PoliceOfficer52", "警号重复"));
                        return;
                    }
                    hs.Add("USERCODE", Hidden_id.Value.ToString());
                    if (serviceManager.updateSevice(hs) > 0)
                    {
                        Window1.Hide();
                        Notice(GetLangStr("policeOffcer51", "信息提示"), GetLangStr("policeOffcer22", "修改成功"));

                        
                        string sw1 = uiDepartment1.DepertName; // 所属单位
                        string xb1 = CmbMale.Text;//性别
                        string xm1 = TxtName.Text;//姓名
                        string cq1 = DateField1.Text;//出生日期
                        string jz1 = TxtAddress.Text;//家庭住址
                        string sh1 = TxtIdNo.Text;//身份证号
                        string jh1 = TxtSiren.Text;//警号
                        string jx1 = CmbRanks.Text;//警衔
                        string xw1 = CmbDuty.Text;//现职务
                        string bb1 = CmbFormat.Text;//编制类别
                        string tb1 = CmbTeQin.Text;//特勤类别
                        string sj1 = TxtPhone.Text;//手机
                        string bh1 = TxtOfficePhone.Text;//办公电话
                        string st1 = TxtHandSets.Text;//手台
                        string sth1 = TxtHandSetsCode.Text;//手台呼号
                        string szh1 = TxtHandSetsGroup.Text;///手台组号
                        string jb1 = TxtEqip.Text;//警用装备
                        this.Hidden_name.Value = xm1;
                        lblname = "";
                        lblname += Bll.Common.AssembleRunLog(sw, sw1, GetLangStr("PoliceOfficer26", "所属单位"), "0");
                        lblname += Bll.Common.AssembleRunLog(xm, xm1, GetLangStr("PoliceOfficer29", "姓名"), "0");
                        lblname += Bll.Common.AssembleRunLog(cq, cq1, GetLangStr("PoliceOfficer30", "出生日期"), "0");
                        lblname += Bll.Common.AssembleRunLog(jz, jz1, GetLangStr("PoliceOfficer31", "家庭住址"), "0");
                        lblname += Bll.Common.AssembleRunLog(sh, sh1, GetLangStr("PoliceOfficer32", "身份证号"), "0");
                        lblname += Bll.Common.AssembleRunLog(jh, jh1, GetLangStr("PoliceOfficer33", "警号"), "0");
                        lblname += Bll.Common.AssembleRunLog(jx, jx1, GetLangStr("PoliceOfficer34", "警衔"), "0");
                        lblname += Bll.Common.AssembleRunLog(xw, xw1, GetLangStr("PoliceOfficer36", "现职务"), "0");
                        lblname += Bll.Common.AssembleRunLog(bb, bb1, GetLangStr("PoliceOfficer38", "编制类别"), "0");
                        lblname += Bll.Common.AssembleRunLog(tb, tb1, GetLangStr("PoliceOfficer40", "特勤级别"), "0");
                        lblname += Bll.Common.AssembleRunLog(sj, sj1, GetLangStr("PoliceOfficer42", "手机"), "0");
                        lblname += Bll.Common.AssembleRunLog(bh, bh1, GetLangStr("PoliceOfficer43", "办公电话"), "0");
                        lblname += Bll.Common.AssembleRunLog(st, st1, GetLangStr("PoliceOfficer44", "手台"), "0");
                        lblname += Bll.Common.AssembleRunLog(sth, sth1, GetLangStr("PoliceOfficer45", "手台呼号"), "0");
                        lblname += Bll.Common.AssembleRunLog(szh, szh1, GetLangStr("PoliceOfficer46", "手台组号"), "0");
                        lblname += Bll.Common.AssembleRunLog(jb, jb1, GetLangStr("PoliceOfficer47", "警用装备"), "0");
                        if (!xb.Equals(xb1))
                        {
                            if (Convert.ToInt32(xb1) == 0)
                            {
                                xb1 = GetLangStr("PoliceOfficer37", "女");
                               // return;
                            }
                            if (Convert.ToInt32(xb1) == 1)
                            {
                                xb1 = GetLangStr("PoliceOfficer39", "男");
                                //return;
                            }
                            if (Convert.ToInt32(xb1) == 2)
                            {
                                xb1 = GetLangStr("PoliceOfficer61", "自定义");
                                //return;
                            }
                            lblname += GetLangStr("PoliceOfficer27", "；性别：") + xb1;
                        }
                        logManager.InsertLogRunning(uName, GetLangStr("PoliceOfficer13", "修改[") + xm1 + GetLangStr("PoliceOfficer62", "]用户:") + lblname, nowIp, "1");
                        GridDataBind(GetWhere());
                        BuildTree(TreePanel1.Root);
                        TreePanel1.Render();
                    }
                    else
                    {
                        Notice(GetLangStr("policeOffcer51", "信息提示"), GetLangStr("policeOffcer52", "修改失败"));
                    }
                }
                FrmClear();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-GengXin_Click", ex.Message+"；"+ex.StackTrace, "GengXin_Click has an exception");
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButDevAdd_Click(object sender, DirectEventArgs e)
        {
            try
            {
                FrmClear();
                ButUpdate.Text = GetLangStr("policeOffcer50", "添加");
                Window1.Title = GetLangStr("policeOffcer53", "添加人员信息");
                ButUpdate.Hidden = false;
                this.Hidden1.Value = "1";
                Window1.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-ButDevAdd_Click", ex.Message+"；"+ex.StackTrace, "ButDevAdd_Click has an exception");
            }
        }

        #endregion 增删改事件

        #endregion 控件事件

        #region 私有方法

        /// <summary>
        /// 绑定查询出人员结果
        /// </summary>
        /// <param name="where"></param>
        private void GridDataBind(string where)
        {
            try
            {
                DataTable dt = serviceManager.GetSevice(where);
                StoreDeviceManager.DataSource = dt;
                StoreDeviceManager.DataBind();
                xbie = dt.Rows[0]["col2"].ToString();
                Session["datatable"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    SelectFirst(dt.Rows[0]);
                    ButExcel.Disabled = false;
                }
                else
                {
                    ButExcel.Disabled = true;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-GridDataBind", ex.Message+"；"+ex.StackTrace, "GridDataBind has an exception");
            }
        }

        /// <summary>
        ///转换打印临时表
        /// </summary>
        /// <returns></returns>
        private DataTable ChangeDataTable()
        {
            try
            {
                DataTable dt = Session["datatable"] as DataTable;
                DataTable dt1 = dt.Copy();

                if (dt1 != null)
                {
                    dt1.Columns.Remove("col0"); dt1.Columns.Remove("col3"); dt1.Columns.Remove("col4");
                    dt1.Columns.Remove("col6"); dt1.Columns.Remove("col5"); dt1.Columns.Remove("col8");
                    dt1.Columns.Remove("col10"); dt1.Columns.Remove("col11"); dt1.Columns.Remove("col13");
                    dt1.Columns.Remove("col14"); dt1.Columns.Remove("col15"); dt1.Columns.Remove("col17");
                    dt1.Columns.Remove("col18"); dt1.Columns.Remove("col19"); dt1.Columns.Remove("col20");
                    dt1.Columns.Remove("col21"); dt1.Columns.Remove("col23");
                    dt1.Columns["col1"].SetOrdinal(0);
                    dt1.Columns["col2"].SetOrdinal(1);
                    dt1.Columns["col9"].SetOrdinal(2);
                    dt1.Columns["col12"].SetOrdinal(3);
                    dt1.Columns["col16"].SetOrdinal(4);
                    dt1.Columns["col7"].SetOrdinal(5);
                    dt1.Columns["col22"].SetOrdinal(6);
                    dt1.Columns[0].ColumnName = GetLangStr("policeOffcer2", "姓名");
                    dt1.Columns[1].ColumnName = GetLangStr("policeOffcer6", "性别");
                    dt1.Columns[2].ColumnName = GetLangStr("policeOffcer3", "警号");
                    dt1.Columns[3].ColumnName = GetLangStr("policeOffcer4", "现职务");
                    dt1.Columns[4].ColumnName = GetLangStr("policeOffcer45", "手台呼号");
                    dt1.Columns[5].ColumnName = GetLangStr("policeOffcer43", "办公电话");
                    dt1.Columns[6].ColumnName = GetLangStr("policeOffcer8", "所属机关");
                }

                return dt1;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-ChangeDataTable", ex.Message+"；"+ex.StackTrace, "ChangeDataTable has an exception");
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
                ////现任职务
                //this.StoreDutyType.DataSource = systemManager.GetCodeData("013013");
                //this.StoreDutyType.DataBind();

                this.StoreDutyType.DataSource = GetRedisData.GetData("t_sys_code:013013");
                this.StoreDutyType.DataBind();

                this.StoreFormat.DataSource = systemManager.GetCodeData("013012"); ;
                this.StoreFormat.DataBind();

                this.StoreRanks.DataSource = systemManager.GetCodeData("013009"); ;
                this.StoreRanks.DataBind();

                this.StoreTeQin.DataSource = systemManager.GetCodeData("013014"); ;
                this.StoreTeQin.DataBind();


                //性别
                this.StoreSex.DataSource = dtsex = GetRedisData.GetData("t_sys_code:011005");
                this.StoreSex.DataBind();
                //this.StoreSex.DataSource = systemManager.GetCodeData("011005"); ;
                //this.StoreSex.DataBind();

                this.StoreDepart.DataSource = settingManager.GetDepartmentDict("00");
                this.StoreDepart.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-StoreDataBind", ex.Message+"；"+ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 获得部门人员总数
        /// </summary>
        /// <param name="departid"></param>
        /// <returns></returns>
        private string GetCount(string departid)
        {
            try
            {
                DataTable dt = serviceManager.PersonbyDepartid(departid);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0][0].ToString();
                    }
                }
                return "0";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-GetCount", ex.Message+"；"+ex.StackTrace, "GetCount has an exception");
                return null;
            }
        }

        #region 递归产生系统表树形菜单节点

        /// <summary>
        ///将部门信息绑定至tree
        /// </summary>
        /// <param name="nodes"></param>
        private void BuildTree(Ext.Net.TreeNodeCollection nodes)
        {
            try
            {
                TreePanel1.RemoveAll(true);
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }
                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Text = GetLangStr("policeOffcer54", "部门列表");
                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;
                DataTable dt = settingManager.GetConfigDepartment("0");
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
                logManager.InsertLogError("PoliceOfficer.aspx-BuildTree", ex.Message+"；"+ex.StackTrace, "BuildTree has an exception");
            }
        }

        /// <summary>
        /// 遍历将子部门挂接至父部门
        /// </summary>
        /// <param name="allNodeTable"></param>
        /// <param name="parentColValue"></param>
        /// <param name="root"></param>
        /// <param name="ParentNode"></param>
        private void Addree(DataTable allNodeTable, string parentColValue, Ext.Net.TreeNode root, Ext.Net.TreeNode ParentNode)
        {
            try
            {
                DataRow[] myDataRows = allNodeTable.Select("col3 ='" + parentColValue + "'");
                foreach (DataRow myDataRow in myDataRows)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    ConfigItem ci0 = new ConfigItem("col0", myDataRow[2].ToString(), ParameterMode.Value);
                    ConfigItem ci1 = new ConfigItem("col1", myDataRow[5].ToString(), ParameterMode.Value);
                    ConfigItem ci2 = new ConfigItem("col2", myDataRow[8].ToString(), ParameterMode.Value);
                    node.Text = myDataRow[2].ToString() + "(" + GetCount(myDataRow[1].ToString()) + ")";
                    node.NodeID = myDataRow[1].ToString();
                    node.Leaf = true;
                    node.Draggable = true;
                    node.Expandable = ThreeStateBool.True;
                    node.Expanded = true;
                    node.Icon = Icon.House;
                    node.Listeners.Click.Handler = "selectNode('" + myDataRow[1].ToString() + "') ;";
                    node.CustomAttributes.Add(ci0);
                    node.CustomAttributes.Add(ci1);
                    node.CustomAttributes.Add(ci2);
                    if (ParentNode != null)
                    {
                        ParentNode.Nodes.Add(node);
                        Addree(allNodeTable, myDataRow["col1"].ToString(), ParentNode, node);
                    }
                    else
                    {
                        root.Nodes.Add(node);
                        Addree(allNodeTable, myDataRow["col1"].ToString(), root, node);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-Addree", ex.Message+"；"+ex.StackTrace, "Addree has an exception");
            }
        }

        #endregion 递归产生系统表树形菜单节点

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
                    string kkType = dt.Rows[i]["col2"].ToString();
                    if (!hts.ContainsKey(kkType))
                    {
                        j++;
                        hts.Add(kkType, dt.Rows[i]["col1"].ToString());
                    }
                }
                return hts;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-ConventDataTable", ex.Message+"；"+ex.StackTrace, "ConventDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        /// 组装查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            try
            {
                string where = " ";
                if (!string.IsNullOrEmpty(TxtDeviceName.Text))
                {
                    where = where + " and  NAME='" + TxtDeviceName.Text + "' ";
                }
                if (CmbDutyType.SelectedIndex != -1)
                {
                    where = where + " and  PROFESSION='" + CmbDutyType.SelectedItem.Value + "' ";
                }
                if (!string.IsNullOrEmpty(uiDepartment.DepertId))
                {
                    where = where + " and  b.DEPARTID='" + uiDepartment.DepertId + "' ";
                }
                if (CmbSex.SelectedIndex != -1)
                {
                    where = where + " and  SEX='" + CmbSex.SelectedItem.Value + "' ";
                }
                if (!string.IsNullOrEmpty(TxTJingHao.Text))
                {
                    where = where + " and  SIREN='" + TxTJingHao.Text.ToUpper() + "' ";
                }
                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-GetWhere", ex.Message+"；"+ex.StackTrace, "GetWhere has an exception");
                return "";
            }
        }

        /// <summary>
        /// 加载人员信息至窗体
        /// </summary>
        /// <param name="id"></param>
        public void Select(string id)
        {
            try
            {
                DataTable da = serviceManager.GetSevice(" AND USERCODE='" + id + "'");
                TxtName.Text = da.Rows[0]["col1"].ToString();
                TxtAddress.Text = da.Rows[0]["col4"].ToString();
                TxtIdNo.Text = da.Rows[0]["col10"].ToString();
                TxtSiren.Text = da.Rows[0]["col9"].ToString();
                CmbRanks.Value = da.Rows[0]["col11"].ToString();
                CmbDuty.Value = da.Rows[0]["col20"].ToString();
                //    CmbFormat.Value = da.Rows[0]["col22"].ToString();
                CmbFormat.Value = da.Rows[0]["col23"].ToString();
                CmbTeQin.Value = da.Rows[0]["col14"].ToString();
                TxtPhone.Text = da.Rows[0]["col5"].ToString();
                TxtOfficePhone.Text = da.Rows[0]["col7"].ToString();
                TxtHandSets.Text = da.Rows[0]["col15"].ToString();
                TxtHandSetsCode.Text = da.Rows[0]["col16"].ToString();
                TxtHandSetsGroup.Text = da.Rows[0]["col17"].ToString();
                uiDepartment1.DepertId = da.Rows[0]["col8"].ToString();
                CmbMale.Value = da.Rows[0]["col21"].ToString();
                string[] lurutiem = (da.Rows[0]["col3"].ToString()).Split(' ');
                DateField1.Text = lurutiem[0].ToString();
                TxtEqip.Text = da.Rows[0]["col18"].ToString();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-Select", ex.Message+"；"+ex.StackTrace, "Select has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void FrmClear()
        {
            TxtName.Text = null;
            TxtAddress.Text = null;
            TxtIdNo.Text = null;
            TxtSiren.Text = null;
            CmbRanks.Value = null;
            CmbDuty.Value = null;
            CmbFormat.Value = null;
            CmbTeQin.Value = null;
            TxtPhone.Text = null;
            TxtOfficePhone.Text = null;
            TxtHandSets.Text = null;
            TxtHandSetsCode.Text = null;
            TxtHandSetsGroup.Text = null;
            uiDepartment.Reset();
            CmbMale.Value = null;
            DateField1.Value = null;
            TxtEqip.Text = null;
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

        #endregion 私有方法

        /// <summary>
        ///
        /// </summary>
        /// <param name="dr"></param>
        private void SelectFirst(DataRow dr)
        {
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

        #region DirectMethod方法

        /// <summary>
        /// 根据部门绑定人员
        /// </summary>
        /// <param name="code"></param>
        [DirectMethod(Namespace = "OnEvl")]
        public void onclickTree(string code)
        {
            try
            {
                GridDataBind(" and a.DEPARTID='" + code + "' ");
                uiDepartment.DepertId = code;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-onclickTree", ex.Message+"；"+ex.StackTrace, "onclickTree has an exception");
            }
        }

        /// <summary>
        /// 控件不可用
        /// </summary>
        //public void detailedInformation()
        //{
        //    CmbMale.Disabled = true;
        //    TxtName.Disabled = true;
        //    DateField1.Disabled = true;
        //    TxtAddress.Disabled = true;
        //    TxtIdNo.Disabled = true;
        //    TxtSiren.Disabled = true;
        //    CmbRanks.Disabled = true;
        //    CmbDuty.Disabled = true;
        //    CmbFormat.Disabled = true;
        //    CmbTeQin.Disabled = true;
        //    TxtPhone.Disabled = true;
        //    TxtOfficePhone.Disabled = true;
        //    TxtHandSets.Disabled = true;
        //    TxtHandSetsCode.Disabled = true;
        //    TxtHandSetsGroup.Disabled = true;
        //    TxtEqip.Disabled = true;
        //}

        //public void upindert() {
        //    Panel3.Disabled = false;
        //    CmbMale.Disabled = false;
        //    TxtName.Disabled = false;
        //    DateField1.Disabled = false;
        //    TxtAddress.Disabled = false;
        //    TxtIdNo.Disabled = false;
        //    TxtSiren.Disabled = false;
        //    CmbRanks.Disabled = false;
        //    CmbDuty.Disabled = false;
        //    CmbFormat.Disabled = false;
        //    CmbTeQin.Disabled = false;
        //    TxtPhone.Disabled = false;
        //    TxtOfficePhone.Disabled = false;
        //    TxtHandSets.Disabled = false;
        //    TxtHandSetsCode.Disabled = false;
        //    TxtHandSetsGroup.Disabled = false;
        //    TxtEqip.Disabled = false;

        //}

        /// <summary>
        /// 点击查看人员信息
        /// </summary>
        /// <param name="id"></param>
        [DirectMethod(Namespace = "OnEvl")]
        public void Onclick(string id)
        {
            try
            {
                Select(id);
                Window1.Title = GetLangStr("policeOffcer57", "查看人员信息");
                ButUpdate.Hidden = true;
                Window1.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-Onclick", ex.Message+"；"+ex.StackTrace, "Onclick has an exception");
            }
        }

        /// <summary>
        /// 修改人员操作
        /// </summary>
        /// <param name="id"></param>
        [DirectMethod(Namespace = "OnEvl")]
        public void Update(string id)
        {
            try
            {
                Select(id);
                Window1.Title = GetLangStr("policeOffcer58", "修改人员信息");
                ButUpdate.Hidden = false;
                ButUpdate.Text = GetLangStr("policeOffcer13", "修改");
                //this.Hidden1.Value = "2";
                //DataRow[] dr = dtsex.Select("code='" +Hidden1.Value +"'");
                //string sess = dr[0]["codedesc"].ToString();
                //this.Hidden1.Value = sess;
                this.Hidden_id.Value = id;
                sw = uiDepartment1.DepertName; // 所属变量
                xb = CmbMale.Text;//性别
                xm = TxtName.Text;//姓名
                cq = DateField1.Text;//出生日期
                jz = TxtAddress.Text;//家庭住址
                sh = TxtIdNo.Text;//身份证号
                jh = TxtSiren.Text;//警号
                jx = CmbRanks.Text;//警衔
                xw = CmbDuty.Text;//现职务
                bb = CmbFormat.Text;//编制类别
                tb = CmbTeQin.Text;//特勤类别
                sj = TxtPhone.Text;//手机
                bh = TxtOfficePhone.Text;//办公电话
                st = TxtHandSets.Text;//手台
                sth = TxtHandSetsCode.Text;//手台呼号
                szh = TxtHandSetsGroup.Text;///手台组号
                jb = TxtEqip.Text;//警用装备
                Window1.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PoliceOfficer.aspx-Update", ex.Message+"；"+ex.StackTrace, "Update has an exception");
            }
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="id"></param>
        [DirectMethod(Namespace = "OnDel")]
        public void Delete(string id,string name)
        {
           
            int i = serviceManager.DeleteSevice(id);
            if (i > 0)
            {
                GridDataBind(GetWhere());
                Notice(GetLangStr("policeOffcer51", "信息提示"), GetLangStr("policeOffcer59", "删除成功"));
                logManager.InsertLogRunning(uName, "删除：姓名["+name+"]",nowIp, "3");
                BuildTree(TreePanel1.Root);
                TreePanel1.Render();
            }
            else
            {
                Notice(GetLangStr("policeOffcer51", "信息提示"), GetLangStr("policeOffcer60", "删除失败"));
            }
        }

        #endregion DirectMethod方法
    }
}