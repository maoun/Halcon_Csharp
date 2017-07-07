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
    public partial class ImportantRule : System.Web.UI.Page
    {
        #region 成员变量

        private TgsPproperty tgsPproperty = new TgsPproperty();

        private static string starttime = "2000-01-01";
        private static string endtime = "2100-12-31";
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

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
                string js = "alert('" + GetLangStr("ImportantRule16", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                try
                {
                    StoreDataBind();
                    //KkmcDataBind();
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex);
                    logManager.InsertLogError("ImportantRule.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
                }
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "" + GetLangStr("ImportantRule17", "访问：重点车辆监管配置") + "", userinfo.NowIp, "0");
                this.DataBind();
            }
        }

        /// <summary>
        /// 查询卡口
        /// </summary>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                DataTable table = null;
                table = Session["dataKkmc"] as DataTable;
                try
                {
                    //if (string.IsNullOrEmpty(TxtStationName.Text))
                    //{
                    //    table=BindKk();
                    //}
                    //else
                    //{
                    //    table = tgsPproperty.GetStationInfo(" station_name LIKE '%" + TxtStationName.Text + "%'");
                    //    if (table == null || table.Rows.Count <= 0)
                    //    {
                    //        table=BindKk();
                    //        Notice("提示信息", "没找到数据");
                    //    }
                    //}
                    table = ToDataTable(table.Select("col2 LIKE '%" + TxtStationName.Text + "%'"));
                    if (table == null || table.Rows.Count <= 0)
                    {
                        StoreDataBind();
                        return;
                    }
                    this.Skkmc.DataSource = table;
                    this.Skkmc.DataBind();
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex);
                    logManager.InsertLogError("ImportantRule.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantRule.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        public DataTable ToDataTable(DataRow[] rows)
        {
            try
            {
                if (rows == null || rows.Length == 0) return null;
                DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
                foreach (DataRow row in rows)
                    tmp.ImportRow(row);  // 将DataRow添加到DataTable中
                return tmp;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantRule.aspx-ToDataTable", ex.Message + "；" + ex.StackTrace, "ToDataTable has an exception");
            }
            return null;
        }

        /// <summary>
        /// 绑定卡口数据
        /// </summary>
        /// <returns></returns>
        private DataTable BindKk()
        {
            try
            {
                DataTable table = new DataTable();
                DataTable table2 = new DataTable();
                table = tgsPproperty.GetStationInfo("1=1");
                table2 = GetBindKkid();

                DataRow[] arrRows;
                foreach (DataRow row in table2.Rows)
                {
                    arrRows = table.Select("col1='" + row["col1"].ToString() + "'");
                    foreach (DataRow row2 in arrRows)
                    {
                        table.Rows.Remove(row2);
                    }
                }
                return table;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantRule.aspx-BindKk", ex.Message + "；" + ex.StackTrace, "BindKk has an exception");
            }
            return null;
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <summary>
        /// 获取已存在的监测信息卡口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void BSearch_DirectClick(object sender, DirectEventArgs e)
        //{
        //    try
        //    {
        //        KkmcDataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(ex);
        //    }
        //}

        #endregion 控件事件

        #region 私有方法

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            DataTable table = new DataTable();
            DataTable table2 = new DataTable();
            try
            {
                table = Bll.Common.ChangColName(GetRedisData.GetData("Station:t_cfg_set_station"));// tgsPproperty.GetStationInfo("1=1");
                table2 = GetBindKkid();
                this.Skkmc2.DataSource = table2;
                this.Skkmc2.DataBind();
                DataTable dtNew = table.Clone();

                DataRow[] arrRows;
                foreach (DataRow row in table2.Rows)
                {
                    arrRows = table.Select("col0='" + row["col1"].ToString() + "'");
                    foreach (DataRow row2 in arrRows)
                    {
                        table.Rows.Remove(row2);
                    }
                }
                table.Columns["col2"].ColumnName = "col3";
                table.Columns["col1"].ColumnName = "col2";
                table.Columns["col0"].ColumnName = "col1";
                Session["dataKkmc"] = table;
                this.Skkmc.DataSource = table;
                this.Skkmc.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantRule.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        [DirectMethod]
        public void removeKkmc(ArrayList list)
        {
            if (Session["dataKkmc"] != null)
            {
            }
        }

        /// <summary>
        /// 绑定地点规则卡口名称数据
        /// </summary>
        private void KkmcDataBind()
        {
            try
            {
                this.Skkmc2.DataSource = GetBindKkid();
                this.Skkmc2.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantRule.aspx-KkmcDataBind", ex.Message + "；" + ex.StackTrace, "KkmcDataBind has an exception");
            }
        }

        private DataTable GetBindKkid()
        {
            DataTable table = new DataTable();
            try
            {
                table = GetBindKkmc();
                return table;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantRule.aspx-GetBindKkid", ex.Message + "；" + ex.StackTrace, "GetBindKkid has an exception");
                return null;
            }
        }

        /// <summary>
        /// 获取地点规则卡口名称
        /// </summary>
        /// <returns></returns>
        private DataTable GetBindKkmc()
        {
            string timeRuleBh = (starttime + endtime).Replace("-", "");
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("col1");
                table.Columns.Add("col2");
                DataTable dtRule = tgsPproperty.GetAreaRule(timeRuleBh);
                DataTable dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
                if (dtRule != null && dtRule.Rows.Count > 0)
                {
                    string AreaRuleXml = dtRule.Rows[0]["qygz"].ToString();
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(AreaRuleXml);
                    XmlNode root = xml.DocumentElement;
                    XmlNodeList nodeList = root.SelectNodes("roadseg/kkid");

                    foreach (XmlNode passcarinfo in nodeList)
                    {
                        string Kkid = passcarinfo.InnerText;
                        DataRow row = table.NewRow();
                        row["col1"] = Kkid;
                        DataRow[] drs = dtStation.Select("STATION_ID ='" + Kkid + "'");
                        if (drs.Length > 0)
                        {
                            row["col2"] = drs[0][1].ToString();
                        }

                        table.Rows.Add(row);
                    }
                    return table;
                }
                else
                    return table;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantRule.aspx-GetBindKkmc", ex.Message + "；" + ex.StackTrace, "GetBindKkmc has an exception");
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
                DataTable dt = Session["datatable"] as DataTable;
                DataTable dt2 = null; ;
                if (dt != null)
                {
                    //PrintColumns pc = new PrintColumns();
                    //pc.Add(new PrintColumn("设备编号", 1));
                    //pc.Add(new PrintColumn("设备名称", 2));
                    //pc.Add(new PrintColumn("设备类型", 3));
                    //pc.Add(new PrintColumn("设备型号", 4));
                    //dt2 = Bll.Common.GetDataTablePrint(dt, pc);
                }

                return dt2;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImportantRule.aspx-ChangeDataTable", ex.Message + "；" + ex.StackTrace, "ChangeDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        ///
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
                logManager.InsertLogError("ImportantRule.aspx-GetdatabyField", ex.Message + "；" + ex.StackTrace, "GetdatabyField has an exception");
                return "";
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
                logManager.InsertLogError("ImportantRule.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice has an exception");
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
            string className = this.GetType().BaseType.FullName;
            return MyNet.Common.Lang.Language.CreateInstance(className).GetLanguageStr(value, desc, className);
        }

        #endregion 私有方法

        #region [DirectMethod]

        /// <summary>
        /// 保存修改的数据
        /// </summary>
        /// <param name="arrayList"></param>
        [DirectMethod]
        public void insertMessage(ArrayList arrayList)
        {
            UserInfo userinfo = Session["Userinfo"] as UserInfo;
            try
            {
                StringBuilder XmlStrTimerule = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                XmlStrTimerule.Append("<time starttime =\"" + starttime + "\" endtime=\"" + endtime + "\">");
                XmlStrTimerule.Append("<day week=\"1,2,3,4,5,6,7\"><hour>2,3,4,5</hour></day></time>");
                string SDBH = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                string BH = SDBH;
                string timeRuleBh = (starttime + endtime).Replace("-", "");
                StringBuilder XmlStrRegionrule = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                XmlStrRegionrule.Append("<road><roadseg>");
                ArrayList arr = new ArrayList();
                //arr = Session["AddKkmc"] as ArrayList;
                arr = arrayList;
                for (int i = 0; i < arr.Count; i++)
                {
                    XmlStrRegionrule.Append("<kkid nextkkid=\"\" limit=\"\">" + arr[i] + "</kkid>");
                }
                XmlStrRegionrule.Append("</roadseg></road>");

                if (tgsPproperty.ExistTimeRuleBh(timeRuleBh))
                {
                    string qybh = tgsPproperty.GetBh(timeRuleBh).Rows[0]["qybh"].ToString();
                    if (tgsPproperty.selectRegionrule(qybh))
                    {
                        int a = tgsPproperty.deleteRegionrule(qybh);
                        if (a > 0)
                        {
                            int a2 = tgsPproperty.insertRegionrule(qybh, XmlStrRegionrule.ToString());
                            if (a2 > 0)
                            {
                                StoreDataBind();
                                Notice(GetLangStr("ImportantRule18", "保存"), GetLangStr("ImportantRule19", "保存成功"));
                                logManager.InsertLogRunning(userinfo.UserName, "" + GetLangStr("ImportantRule20", "添加：") + "" + qybh, userinfo.NowIp, "1");
                            }
                        }
                    }
                    else
                    {
                        int a2 = tgsPproperty.insertRegionrule(qybh, XmlStrRegionrule.ToString());
                        if (a2 > 0)
                        {
                            StoreDataBind();
                            Notice(GetLangStr("ImportantRule18", "保存"), GetLangStr("ImportantRule19", "保存成功"));
                            logManager.InsertLogRunning(userinfo.UserName, "" + GetLangStr("ImportantRule21", "移除：") + "" + qybh, userinfo.NowIp, "1");
                        }
                    }
                    //int d = tgsPproperty.deleteRelation(tgsPproperty.GetBh(timeRuleBh).Rows[0]["pzbh"].ToString());
                    //int b = tgsPproperty.deleteRegionrule(tgsPproperty.GetBh(timeRuleBh).Rows[0]["qybh"].ToString());
                    //int a = tgsPproperty.deleteTimerule(timeRuleBh);
                    //int c = tgsPproperty.deleteConfig(tgsPproperty.GetBh(timeRuleBh).Rows[0]["pzbh"].ToString());
                }
                else
                {
                    int a2 = tgsPproperty.insertTimerule(timeRuleBh, XmlStrTimerule.ToString());
                    int b2 = tgsPproperty.insertRegionrule(BH, XmlStrRegionrule.ToString());
                    int c2 = tgsPproperty.insertConfig(BH, "400501", timeRuleBh, BH, "3");
                    int d2 = tgsPproperty.insertRelation(BH, "400602", BH);
                    if (a2 > 0 && b2 > 0 && c2 > 0 && d2 > 0)
                    {
                        StoreDataBind();
                        Notice(GetLangStr("ImportantRule18", "保存"), GetLangStr("ImportantRule19", "保存成功"));
                        logManager.InsertLogRunning(userinfo.UserName, "" + GetLangStr("ImportantRule18", "保存") + "：" + ID + Request.QueryString["type"], userinfo.NowIp, "1");
                    }
                    else
                    {
                        Notice(GetLangStr("ImportantRule18", "保存"), GetLangStr("ImportantRule22", "保存失败"));
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

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
        //            starttime = strtime;
        //        else
        //            endtime = strtime;
        //    }
        //    catch(Exception ex)
        //    {
        //        ILog.WriteErrorLog(ex);
        //    }
        //}

        #endregion [DirectMethod]
    }
}