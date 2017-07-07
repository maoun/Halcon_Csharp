using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Xml;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PeccancyAreaSetting : System.Web.UI.Page
    {
        #region 成员变量

        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();
        private string SystemID = ((int)MyNet.DataAccess.Model.SystemEnum.EnumSystemType.DeskTop).ToString("00");
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        #endregion 成员变量

        #region 事件集合

        /// <summary>
        /// 页面加载
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
                try
                {
                    //userLogin.IsLoginPage(this);
                    StoreDataBind();
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex);
                    logManager.InsertLogError("PeccancyAreaSetting.aspx-Page_Load", ex.Message+"；"+ex.StackTrace, "Page_Load has an exception");
                }
                
                this.DataBind();
            }
            UserInfo userinfo = Session["Userinfo"] as UserInfo;
            logManager.InsertLogRunning(userinfo.UserName, "访问：区间超速配置", userinfo.NowIp, "0");
        }

        /// <summary>
        /// 获取结束卡口数据（查询位置）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TgsRefresh(object sender, StoreRefreshDataEventArgs e)
        {
            //DataTable data = tgsPproperty.GetEndStationInfo(this.CmdKskkid.SelectedItem.Value);
            BindEndkk(CmdKskkid.SelectedItem.Value);

            //try
            //{
            //    if (Session["dataTable"] != null)
            //    {
            //        DataTable table = (DataTable)Session["dataTable"];
            //        System.Data.DataView dv = new System.Data.DataView(table);
            //        dv.RowFilter = "col1=" + this.CmdKskkid.SelectedItem.Value;
            //        StoreEndStation.DataSource = dv;
            //        StoreEndStation.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ILog.WriteErrorLog(ex);
            //}
        }

        public void BindEndkk(string endKk)
        {

            try { 
            if (Session["dataTable"] != null)
            {
                DataTable table = (DataTable)Session["dataTable"];
                System.Data.DataView dv = new System.Data.DataView(table);
                dv.RowFilter = "col1=" + endKk;
                StoreEndStation.DataSource = dv;
                StoreEndStation.DataBind();
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-BindEndkk", ex.Message+"；"+ex.StackTrace, "BindEndkk has an exception");
            }
        }

        /// <summary>
        /// 获取结束卡口数据（添加、修改位置）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DictRefresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                if (Session["dataTable"] != null)
                {
                    DataTable table = (DataTable)Session["dataTable"];
                    System.Data.DataView dv = new System.Data.DataView(table);
                    dv.RowFilter = "col1=" + this.CmbStartStation.SelectedItem.Value;
                    DataTable dt = dv.ToTable(true, new string[] { "col2" });
                    StringBuilder str = new StringBuilder(this.CmbStartStation.SelectedItem.Value);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str.Append("," + dt.Rows[i][0].ToString());
                    }
                    StoreEndStationDict.DataSource = tgsPproperty.GetEndStationDict(str.ToString());
                    StoreEndStationDict.DataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-DictRefresh", ex.Message+"；"+ex.StackTrace, "DictRefresh has an exception");
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                PecSettingDataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            try { 
            CmdKskkid.Reset();
            CmdJskkid.Reset();

            DataTable table = new DataTable();
            table.Columns.Add("col2", typeof(String));

            table.Columns.Add("col11", typeof(String));
            StoreEndStation.DataSource = table;
            StoreEndStation.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-ButResetClick", ex.Message+"；"+ex.StackTrace, "ButResetClick has an exception");
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
                PecSettingDataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-MyData_Refresh", ex.Message+"；"+ex.StackTrace, "MyData_Refresh has an exception");
            }
        }

        /// <summary>
        /// 显示选中数据的详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectPecAreasetting(object sender, DirectEventArgs e)
        {
            try
            {
                object data = e.ExtraParams["sdata"];
                string sdata = data.ToString();
                //TxtID.Text = Bll.Common.GetdatabyField(sdata, "col0");
                TxtAreaID.Text = Bll.Common.GetdatabyField(sdata, "col0");
                CmbStartStation.Value = Bll.Common.GetdatabyField(sdata, "col1");

                EndStationId.Value = Bll.Common.GetdatabyField(sdata, "col2");
                CmbEndStation.Text = Bll.Common.GetdatabyField(sdata, "col11");
                TxtAreaLength.Text = Bll.Common.GetdatabyField(sdata, "col3");
                TxtBS.Text = Bll.Common.GetdatabyField(sdata, "col5");
                TxtSS.Text = Bll.Common.GetdatabyField(sdata, "col4");
                TxtBLS.Text = Bll.Common.GetdatabyField(sdata, "col7");
                TxtSLS.Text = Bll.Common.GetdatabyField(sdata, "col6");
                ComDirection.Value = Bll.Common.GetdatabyField(sdata, "col8");
                ComIsPeccancy.Value = Bll.Common.GetdatabyField(sdata, "col9");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-SelectPecAreasetting", ex.Message+"；"+ex.StackTrace, "SelectPecAreasetting has an exception");
            }
        }

        #endregion 事件集合

        #region 私有方法

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        private string Getwhere()
        {
            try { 

            string where = "1=1";
            if (CmdKskkid.SelectedIndex != -1)
            {
                where += " and col1  =  '" + CmdKskkid.SelectedItem.Value + "'";
            }
            if (CmdJskkid.SelectedIndex != -1)
            {
                where += " and col2  =  '" + CmdJskkid.SelectedItem.Value + "'";
            }
            return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-Getwhere", ex.Message+"；"+ex.StackTrace, "Getwhere has an exception");
            }
            return null;

        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        private void PecSettingDataBind()
        {
            try
            {
                GetDataOfPeccancyAreaSetting();
                if (Session["dataTable"] != null)
                {
                    DataTable table = (DataTable)Session["dataTable"];
                    System.Data.DataView dv2 = new System.Data.DataView(table);
                    DataTable dt3 = dv2.ToTable(true, new string[] { "col1", "col10" });
                    StoreStartStation.DataSource = dt3;
                    StoreStartStation.DataBind();

                    System.Data.DataView dv = new System.Data.DataView(table);
                    dv.RowFilter = Getwhere();
                    StorePecAreasetting.DataSource = dv;
                    StorePecAreasetting.DataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-PecSettingDataBind", ex.Message+"；"+ex.StackTrace, "PecSettingDataBind has an exception");
            }
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                DataTable dt = GetDataOfPeccancyAreaSetting();
                System.Data.DataView dv = new System.Data.DataView(dt);
                DataTable dt3 = dv.ToTable(true, new string[] { "col1", "col10" });

                StoreStartStation.DataSource = dt3;
                StoreStartStation.DataBind();
                StoreStartStationDict.DataSource = tgsPproperty.GetStationInfo("a.station_type_id in (02,03)");
                StoreStartStationDict.DataBind();
                this.StoreShow.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240034")); //tgsPproperty.GetCommonDict("240034");
                this.StoreShow.DataBind();
                this.StoreDirection.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240025")); // tgsPproperty.GetDirectionDict();
                this.StoreDirection.DataBind();

                StorePecAreasetting.DataSource = dt;
                StorePecAreasetting.DataBind();
                if (dt.Rows.Count > 0)
                {
                    SelectFirst(dt.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-StoreDataBind", ex.Message+"；"+ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 获取区域规则数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetDataOfPeccancyAreaSetting()
        {
            try
            {
                DataTable AreaSetting = new DataTable();
                addColum(AreaSetting);
                DataTable table = new DataTable();
                table = tgsPproperty.GetPeccancyAreaSetting();
                if (table.Rows.Count > 0)
                {
                    XmlDocument xml = new XmlDocument();

                    for (int j = 0; j < table.Rows.Count; j++)
                    {
                        string XmlAreaRule = table.Rows[j]["col1"].ToString();
                        string areaRuleBh = table.Rows[j]["col0"].ToString();
                        addData(AreaSetting, xml, XmlAreaRule, areaRuleBh);
                    }
                }
                if (Session["datTable"] != null)
                {
                    Session["dataTable"] = null;
                }
                Session["dataTable"] = AreaSetting;
                return AreaSetting;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-GetDataOfPeccancyAreaSetting", ex.Message+"；"+ex.StackTrace, "GetDataOfPeccancyAreaSetting has an exception");
                return null;
            }
        }

        /// <summary>
        /// 解析xml并将数据填充到dataTable
        /// </summary>
        /// <param name="table"></param>
        /// <param name="xmlStr"></param>
        private void addData(DataTable table, XmlDocument xml, string xmlStr, string Bh)
        {
            try
            {
                DataRow row = table.NewRow();
                xml.LoadXml(xmlStr);
                XmlNode root = xml.DocumentElement;
                XmlNode nodeList = root.SelectSingleNode("roadseg/kkid");
                XmlAttributeCollection attributes = nodeList.Attributes;
                row["col0"] = Bh;
                row["col1"] = nodeList.InnerText;
                row["col2"] = attributes["nextkkid"].Value;
                row["col3"] = attributes["qjjl"].Value;
                row["col4"] = attributes["xcxgs"].Value;
                row["col5"] = attributes["dcxgs"].Value;
                row["col6"] = attributes["xcxds"].Value;
                row["col7"] = attributes["dcxds"].Value;
                row["col8"] = attributes["fxbh"].Value;
                row["col9"] = attributes["sfwf"].Value;
                row["col10"] = tgsPproperty.GetKkmcByKkid(row["col1"].ToString());
                row["col11"] = tgsPproperty.GetKkmcByKkid(row["col2"].ToString());
                row["col12"] = tgsPproperty.GetDirectionName(row["col8"].ToString());
                switch (row["col9"].ToString())
                {
                    case "0":
                        row["col13"] = GetLangStr("PeccancyAreaSetting44", "否");
                        break;

                    case "1":
                        row["col13"] = GetLangStr("PeccancyAreaSetting43", "是");
                        break;
                }

                table.Rows.Add(row);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-addData", ex.Message+"；"+ex.StackTrace, "addData has an exception");
            }
        }

        /// <summary>
        /// 为dataTable添加列
        /// </summary>
        /// <param name="table"></param>
        private void addColum(DataTable table)
        {
            try
            {
                table.Columns.Add("col0", typeof(String));
                table.Columns.Add("col1", typeof(String));
                table.Columns.Add("col2", typeof(String));
                table.Columns.Add("col3", typeof(String));
                table.Columns.Add("col4", typeof(String));
                table.Columns.Add("col5", typeof(String));
                table.Columns.Add("col6", typeof(String));
                table.Columns.Add("col7", typeof(String));
                table.Columns.Add("col8", typeof(String));
                table.Columns.Add("col9", typeof(String));
                table.Columns.Add("col10", typeof(String));
                table.Columns.Add("col11", typeof(String));
                table.Columns.Add("col12", typeof(String));
                table.Columns.Add("col13", typeof(String));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-addColum", ex.Message+"；"+ex.StackTrace, "addColum has an exception");
            }
        }

        /// <summary>
        /// 获取第一行数据
        /// </summary>
        /// <param name="dr"></param>
        private void SelectFirst(DataRow dr)
        {
            try
            {
                //TxtID.Text = dr["col0"].ToString();
                TxtAreaID.Text = dr["col0"].ToString();
                CmbStartStation.Value = dr["col1"].ToString();
                EndStationId.Value = dr["col2"].ToString();
                CmbEndStation.Text = dr["col11"].ToString();
                TxtAreaLength.Text = dr["col3"].ToString();
                TxtBS.Text = dr["col5"].ToString();
                TxtSS.Text = dr["col4"].ToString();
                TxtBLS.Text = dr["col7"].ToString();
                TxtSLS.Text = dr["col6"].ToString();
                ComDirection.Value = dr["col8"].ToString();
                ComIsPeccancy.Text = dr["col9"].ToString();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-SelectFirst", ex.Message+"；"+ex.StackTrace, "SelectFirst has an exception");
            }
        }

        /// <summary>
        /// 消息提醒
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
                logManager.InsertLogError("PeccancyAreaSetting.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice发生异常");
            }
        }

        /// <summary>
        /// 多语言转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public string GetLangStr(string value, string desc)
        {
            try
            {
                string className = this.GetType().BaseType.FullName;
                return MyNet.Common.Lang.Language.CreateInstance(className).GetLanguageStr(value, desc, className);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-GetLangStr", ex.Message + "；" + ex.StackTrace, "GetLangStr发生异常");
                return null;
            }
        }

        #endregion 私有方法

        #region [DirectMethod]

        /// <summary>
        /// 点击添加按钮时清空控件内的数据
        /// </summary>
        [DirectMethod]
        public void InsertPecAreasetting()
        {
            //TxtID.Text = tgsDataInfo.GetTgsRecordId();
            TxtAreaID.Text = tgsDataInfo.GetTgsRecordId();
            CmbStartStation.Value = "";
            CmbEndStation.Value = "";

            ComDirection.Text = "";
            ComIsPeccancy.Text = "";
            ComDirection.SelectedIndex = -1;
            ComIsPeccancy.SelectedIndex = -1;
            TxtAreaLength.Text = "";
            TxtBS.Text = "";
            TxtSS.Text = "";
            TxtBLS.Text = "";
            TxtSLS.Text = "";
        }

        /// <summary>
        /// 插入、修改数据
        /// </summary>
        [DirectMethod]
        public void UpdatePecAreasetting()
        {
            try
            {
                DataTable table = new DataTable();
                Hashtable hs = new Hashtable();
                //hs.Add("xh", TxtID.Text);
                if (CmbStartStation.SelectedIndex != -1)
                {
                    hs.Add("kskkid", CmbStartStation.SelectedItem.Value);
                }
                if (CmbEndStation.SelectedIndex != -1)
                {
                    hs.Add("jskkid", CmbEndStation.SelectedItem.Value);
                }
                else
                {
                    hs.Add("jskkid", EndStationId.Value);
                }
                hs.Add("qjjl", TxtAreaLength.Text);
                hs.Add("xcqjds", TxtSLS.Text);
                hs.Add("xcqjgs", TxtSS.Text);
                hs.Add("dcqjds", TxtBLS.Text);
                hs.Add("dcqjgs", TxtBS.Text);
                hs.Add("areaid", TxtAreaID.Text);
                hs.Add("type", "400502");
                hs.Add("cfjg", "3");
                hs.Add("sdbh", "");
                if (ComDirection.SelectedIndex != -1)
                {
                    hs.Add("fxbh", ComDirection.SelectedItem.Value);
                }
                if (ComIsPeccancy.SelectedIndex != -1)
                {
                    hs.Add("ispecc", ComIsPeccancy.SelectedItem.Value);
                }
                if (tgsPproperty.UpdatePeccancyAreaSetting(hs) > 0)
                {
                    Notice(GetLangStr("PeccancyAreaSetting38", "信息更新"), GetLangStr("PeccancyAreaSetting39", "保存成功"));
                    PecSettingDataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-UpdatePecAreasetting", ex.Message+"；"+ex.StackTrace, "UpdatePecAreasetting has an exception");
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        [DirectMethod]
        public void DeletePecAreasetting()
        {
            try
            {
                RowSelectionModel sm = this.GridPecAreasetting.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string Id = sm.SelectedRow.RecordID;
                    X.Msg.Confirm(GetLangStr("PeccancyAreaSetting40", "信息"), GetLangStr("PeccancyAreaSetting41", "确认要删除") + "" + GetLangStr("PeccancyAreaSetting42", "这条记录吗?"), new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "PeccancyAreaSetting.DoYes()",
                            Text = GetLangStr("PeccancyAreaSetting43", "是")
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "PeccancyAreaSetting.DoNo()",
                            Text = GetLangStr("PeccancyAreaSetting44", "否")
                        }
                    }).Show();
                }
                else
                {
                    Notice(GetLangStr("PeccancyAreaSetting45", "信息提示"), GetLangStr("PeccancyAreaSetting46", "请选择要删除的数据"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-DeletePecAreasetting", ex.Message+"；"+ex.StackTrace, "DeletePecAreasetting has an exception");
            }
        }

        /// <summary>
        /// 确定删除
        /// </summary>
        [DirectMethod]
        public void DoYes()
        {
            try
            {
                RowSelectionModel sm = this.GridPecAreasetting.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRow.ToBuilder();
                Hashtable hs = new Hashtable();
                string Id = sm.SelectedRow.RecordID;
                hs.Add("qybh", Id);
                if (tgsPproperty.DeleteKeycar_Config(hs["qybh"].ToString()) > 0)
                {
                    if (tgsPproperty.DeletePeccancyAreaSetting(hs) > 0)
                    {
                        Notice(GetLangStr("PeccancyAreaSetting45", "信息提示"), GetLangStr("PeccancyAreaSetting47", "删除成功"));
                        PecSettingDataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-DoYes", ex.Message+"；"+ex.StackTrace, "DoYes has an exception");
            }
        }

        /// <summary>
        /// 取消删除
        /// </summary>
        [DirectMethod]
        public void DoNo()
        {
        }

        #endregion [DirectMethod]
    }
}