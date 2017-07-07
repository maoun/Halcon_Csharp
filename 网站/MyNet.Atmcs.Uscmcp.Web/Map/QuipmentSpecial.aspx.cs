using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web.Map
{
    public partial class QuipmentSpecial : System.Web.UI.Page
    {
        #region 私有变量

        private static DataTable Dt_Station = new DataTable();
        private Bll.MapManager bll = new Bll.MapManager();
        private MapDataOperate mapDataOperate = new MapDataOperate();
        private QueryService.querypasscar client = new QueryService.querypasscar();
        private static string mq = "";
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        #endregion 私有变量

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
                string js = "alert('" + GetLangStr("QuipmentSpecial12", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                mq = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                Session["mqid"] = mq;
                UserInfo userinfo = Session["userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("QuipmentSpecial13", "访问：") + Request.QueryString["funcname"], userinfo.NowIp, "0");
                this.DataBind();
            }
        }

        /// <summary>
        /// 设备展示标注地图
        /// </summary>
        [DirectMethod]
        public void ShowDevice()
        {
            ClearMap();
            foreach (Checkbox ck in gpck.Items)
            {
                if (ck.Checked)
                {
                    SelectMapTo(ck.Tag);
                }
            }
            ShowClusterer();
        }

        #endregion 事件

        #region 方法

        [DirectMethod]
        public void CloseMq()
        {
            try
            {
                if (!client.cancel(mq))
                    client.cancel(mq);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("QuipmentSpecial.aspx-CloseMq", ex.Message + "；" + ex.StackTrace, "CloseMq has an exception");
            }
        }

        /// <summary>
        /// 地图清理
        /// </summary>
        private void ClearMap()
        {
            string js = "BMAP.Clear();";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }

        /// <summary>
        /// 显示聚合
        /// </summary>
        private void ShowClusterer()
        {
            string js = " BMAP.ShowMarkerClusterer()  ";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }

        /// <summary>
        /// 类型切换方法
        /// </summary>
        /// <param name="type">类型</param>
        public void SelectMapTo(string type)
        {
            try
            {
                List<string> MapList = mapDataOperate.GetMarkJs(false, type);
                for (int i = 0; i < MapList.Count; i++)
                {
                    this.ResourceManager1.RegisterAfterClientInitScript(MapList[i]);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("QuipmentSpecial.aspx-SelectMapTo", ex.Message + "；" + ex.StackTrace, "SelectMapTo has an exception");
            }
        }

        #endregion 方法

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
    }
}