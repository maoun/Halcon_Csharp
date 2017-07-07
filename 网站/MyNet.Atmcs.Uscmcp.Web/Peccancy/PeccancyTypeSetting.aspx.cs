using System;
using System.Collections;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;
namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PeccancyTypeSetting : System.Web.UI.Page
    {
        #region 成员变量

        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private SettingManager settingManager = new SettingManager();
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
           string username=Request.QueryString["username"];  
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("PeccancyTypeSetting12", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'"; 
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); 
                return; 
            }
            if (!X.IsAjaxRequest)
            {
                StoreDataBind();
                this.DataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("PeccancyTypeSetting13", "访问：") + Request.QueryString["funcname"], userinfo.NowIp, "0");

            }
        }
        /// <summary>
        /// 刷新违法类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void StorePeccancy_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            GetPeccancyData();
        }

        protected void UpdatePeccancyData(object sender, DirectEventArgs e)
        {
            try
            {

          
            Hashtable hs = new Hashtable();
            RowSelectionModel sm = this.GridDevice.SelectionModel.Primary as RowSelectionModel;
            string xh = sm.SelectedRow.RecordID;
            hs.Add("xh", xh);

            if (CmbPeccancyType.SelectedIndex != -1)
            {
                hs.Add("wfxwid", CmbPeccancyType.Value);
                hs.Add("wfxwms", CmbPeccancyType.SelectedItem.Text.Split('-')[1].ToString());
            }
            if (CmbIsUse.SelectedIndex != -1)
            {
                hs.Add("isuse", CmbIsUse.Value);
            }
            if (tgsPproperty.UpdatePeccancyTypeSetting(hs) > 0)
            {
                Notice(GetLangStr("PeccancyTypeSetting14", "信息提示"), GetLangStr("PeccancyTypeSetting15", "保存成功"));
                GetPeccancyData();
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyDelete.aspx-UpdatePeccancyData", ex.Message+"；"+ex.StackTrace, "UpdatePeccancyData has an exception");
            }
        }

        #endregion 控件事件

        #region 私有方法

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            this.StoreUseType.DataSource = tgsPproperty.GetCommonDict("240034");
            this.StoreUseType.DataBind();

            DataTable dt = tgsPproperty.GetPeccancyTypeSetting("1=1");
            if (dt != null && dt.Rows.Count > 0)
            {
                StorePecType.DataSource = dt;
                StorePecType.DataBind();
            }
            this.StorePeccancyType.DataSource = tgsPproperty.GetPeccancyType("isuse='1'");
            this.StorePeccancyType.DataBind();
        }
        /// <summary>
        /// 查询所有违法类型
        /// </summary>
        private void GetPeccancyData()
        {
            DataTable dt = tgsPproperty.GetPeccancyTypeSetting("1=1");
            StorePecType.DataSource = dt;
            StorePecType.DataBind();
        }

        /// <summary>
        /// 提示信息
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
        /// 提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Message(string title, string msg)
        {
            X.Msg.Show(new MessageBoxConfig
            {
                Title = title,
                Message = msg,
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "WARNING")
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

        #region DirectMethod

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