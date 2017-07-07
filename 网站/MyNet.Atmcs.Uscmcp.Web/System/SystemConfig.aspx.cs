using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class SystemConfig : System.Web.UI.Page
    {
        #region 成员变量

        private SystemManager systemManager = new SystemManager();
        private SettingManager settingManager = new SettingManager();
        private string SystemID = "00";
        private Bll.LogManager logManager = new Bll.LogManager();
        private static string ph;//配置编号
        private static string pc;//配置名称
        private static string pz;//配置值
        private static string bz;//备注
        private static string uName = "";
        private static string nowIp = "";

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

            if (!new UserLogin().CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("SystemConfig14", " 您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            try
            {
                if (!X.IsAjaxRequest)
                {
                    FirstGetSysConfig();
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    uName = userinfo.UserName;
                    nowIp = userinfo.NowIp;
                    logManager.InsertLogRunning(userinfo.UserName, GetLangStr("SystemConfig15","访问：") + Request.QueryString["funcname"], userinfo.NowIp, "0");
                    this.DataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SystemConfig.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
            }
        }

        /// <summary>
        /// 数据刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void StoreConfig_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                GetSysContents(SystemID);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SystemConfig.aspx-StoreConfig_Refresh", ex.Message + "；" + ex.StackTrace, "StoreConfig_Refresh has an exception");
            }
        }

        #endregion 事件

        #region DirectMethod

        /// <summary>
        /// 删除确认事件
        /// </summary>
        [DirectMethod]
        public void DoConfirm()
        {
            X.Msg.Confirm(GetLangStr("SystemConfig16", "信息"), GetLangStr("SystemConfig17", "确认要删除这条记录吗?"), new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = "systemConfig.DoYes()",
                    Text = GetLangStr("SystemConfig18", "是")
                },
                No = new MessageBoxButtonConfig
                {
                    Handler = "systemConfig.DoNo()",
                    Text = GetLangStr("SystemConfig19", "否")
                }
            }).Show();
        }

        /// <summary>
        /// 确定事件
        /// </summary>
        [DirectMethod]
        public void DoYes()
        {
            Hashtable hs = new Hashtable();
            hs.Add("configid", txtConfigId.Text);
            hs.Add("systemid", "00");
            try
            {
                if (settingManager.DeleteConfigInfo(hs) > 0)
                {
                    Notice(GetLangStr("SystemConfig20", "信息提示"), GetLangStr("SystemConfig21", "删除成功"));

                    logManager.InsertLogRunning(uName, GetLangStr("SystemConfig22","删除配置名称：[") + txtConfigName.Text + "]", nowIp, "3");
                    GetSysContents("00");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SystemConfig.aspx-DoYes", ex.Message + "；" + ex.StackTrace, "DoYes has an exception");
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
        /// 添加信息
        /// </summary>
        [DirectMethod]
        public void InfoSave()
        {
            txtConfigId.Disabled = false;
            txtConfigId.Text = "";
            txtConfigName.Text = "";
            txtConfigValue.Text = "";
            txtRemark.Text = "";
        }

        /// <summary>
        /// 更新配置信息
        /// </summary>
        [DirectMethod]
        public void UpdateData()
        {
            Hashtable hs = new Hashtable();
            hs.Add("systemid", "00");
            hs.Add("configid", txtConfigId.Text);
            hs.Add("configname", txtConfigName.Text);
            hs.Add("configvalue", txtConfigValue.Text);
            hs.Add("remark", txtRemark.Text);
            try
            {
                int res = settingManager.UpdateConfigInfo(hs);
                if (res > -1)
                {
                    if (res == 0)
                    {
                        if (settingManager.AddConfigInfo(hs) > 0)
                        {
                            Notice(GetLangStr("SystemConfig20", "信息提示"), GetLangStr("SystemConfig23", "保存成功"));
                            txtConfigId.Disabled = true;
                            InsertLog();
                            GetSysContents("00");
                            ph = txtConfigId.Text;
                            pc = txtConfigName.Text;
                            pz = txtConfigValue.Text;
                            bz = txtRemark.Text;
                        }
                    }
                    else
                    {
                        Notice(GetLangStr("SystemConfig20", "信息提示"), GetLangStr("SystemConfig24", "更新成功"));
                        UpdataLog();
                        GetSysContents("00");
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SystemConfig.aspx-UpdateData", ex.Message + "；" + ex.StackTrace, "UpdateData has an exception");
            }
        }

        /// <summary>
        /// 修改成功后加入日志
        /// </summary>
        /// <param name="ty"></param>
        public void UpdataLog()
        {
            string upname = "";
            string h1 = txtConfigId.Text;
            string h2 = txtConfigName.Text;
            string h3 = txtConfigValue.Text;
            string h4 = txtRemark.Text;

            upname += Bll.Common.AssembleRunLog(pc, h2, GetLangStr("SystemConfig3","配置名称"), "1");
            upname += Bll.Common.AssembleRunLog(pz, h3, GetLangStr("SystemConfig4","配置值"), "1");
            upname += Bll.Common.AssembleRunLog(bz, h4, GetLangStr("SystemConfig5","备注"), "1");
            logManager.InsertLogRunning(uName,GetLangStr("SystemConfig28","修改配置编号：[")  + h1 + "]:" + upname, nowIp, "2");
        }

        /// <summary>
        /// 加入日志
        /// </summary>
        public void InsertLog()
        {
            try
            {
                string lblname = "";
                lblname += Bll.Common.AssembleRunLog("", txtConfigId.Text,GetLangStr("SystemConfig2","配置编号") , "0");
                lblname += Bll.Common.AssembleRunLog("", txtConfigName.Text,GetLangStr("SystemConfig3","配置名称") , "0");

                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName,GetLangStr("SystemConfig31","添加系统参数：")  + lblname, userinfo.NowIp, "1");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        #endregion DirectMethod

        #region 供事件调用的方法

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void FirstGetSysConfig()
        {
            try
            {
                //DataTable objData = systemManager.GetSystemInfo();
                //this.StoreSystem.DataSource = objData;
                //this.StoreSystem.DataBind();

                //if (string.IsNullOrEmpty(SystemID))
                //{
                //    SystemID = objData.Rows[0][0].ToString();
                //}

                GetSysContents(SystemID);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SystemConfig.aspx-FirstGetSysConfig", ex.Message + "；" + ex.StackTrace, "FirstGetSysConfig has an exception");
            }
        }

        /// <summary>
        /// 获取配置信息的具体内容
        /// </summary>
        /// <param name="systemId"></param>
        private void GetSysContents(string systemId)
        {
            try
            {
                DataTable objData2 = settingManager.GetConfigInfo(systemId);
                this.StoreConfig.DataSource = objData2;
                this.StoreConfig.DataBind();
                if (objData2.Rows.Count > 0)
                {
                    txtConfigId.Text = objData2.Rows[0][1].ToString();
                    txtConfigName.Text = objData2.Rows[0][2].ToString();
                    txtConfigValue.Text = objData2.Rows[0][3].ToString();
                    txtRemark.Text = objData2.Rows[0][4].ToString();
                    ph = txtConfigId.Text;
                    pc = txtConfigName.Text;
                    pz = txtConfigValue.Text;
                    bz = txtRemark.Text;
                    ButUpdate.Show();
                    ButDelete.Show();
                }
                else
                {
                    ButUpdate.Hide();
                    ButDelete.Hide();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SystemConfig.aspx-GetSysContents", ex.Message + "；" + ex.StackTrace, "GetSysContents has an exception");
            }
        }

        /// <summary>
        /// 获取配置信息数据
        /// </summary>
        /// <param name="configid"></param>
        private void GetData(string configid)
        {
            try
            {
                Session["configid"] = configid;
                DataTable dt = settingManager.GetConfigInfo("00", configid);
                if (dt != null)
                {
                    txtConfigId.Text = dt.Rows[0][1].ToString();
                    txtConfigName.Text = dt.Rows[0][2].ToString();
                    txtConfigValue.Text = dt.Rows[0][3].ToString();
                    txtRemark.Text = dt.Rows[0][4].ToString();
                    ph = txtConfigId.Text;
                    pc = txtConfigName.Text;
                    pz = txtConfigValue.Text;
                    bz = txtRemark.Text;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SystemConfig.aspx-GetData", ex.Message + "；" + ex.StackTrace, "GetData has an exception");
            }
        }

        /// <summary>
        /// 选中数据行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowSelect(object sender, DirectEventArgs e)
        {
            try
            {
                string configid = e.ExtraParams["configid"];
                GetData(configid);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SystemConfig.aspx-RowSelect", ex.Message + "；" + ex.StackTrace, "RowSelect has an exception");
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
                AlignCfg = new NotificationAlignConfig
                {
                    OffsetY = -60,
                    ElementAnchor = AnchorPoint.BottomRight
                },
                HideDelay = 2000,
                Height = 120,
                Html = "<br></br>" + msg + "!"
            });
        }

        #endregion 供事件调用的方法

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