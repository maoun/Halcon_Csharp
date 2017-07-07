using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class WebDeviceGrid : System.Web.UI.Page
    {
        private DataCountInfo dataCountInfo = new DataCountInfo();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        protected void Page_Load(object sender, EventArgs e)
        {
            string datetime = Request.QueryString["datetime"];
            if (!string.IsNullOrEmpty(datetime))
            {
                GetChartLine(datetime);
                this.DataBind();
            }
            else
            {
                GetChartLine(DateTime.Now.ToString("yyyy-MM-dd"));
            }
            UserInfo userinfo = Session["Userinfo"] as UserInfo;
            logManager.InsertLogRunning(userinfo.UserName, "访问：" + Request.QueryString["funcname"], userinfo.NowIp, "0");
        }

        private void GetChartLine(string datetime)
        {
            try
            {
                StoreFlow.DataSource = Bll.GetRedisData.ChangColName(GetRedisData.GetData("DeviceDataCount:YCZS"),true); //dataCountInfo.GetDeviceInfoByType(datetime, "YCZS");
                StoreFlow.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebDeviceGrid.aspx-GetChartLine", ex.Message+"；"+ex.StackTrace, "GetChartLine has an exception");

            }
        }

        /// <summary>
        /// 保存选中的记录到Session当中
        /// </summary>
        [DirectMethod]
        public void BaoCunSession(string col0,string col1,string col2,string col3,string col4,string col5,string col6,string col7)
        {
            if (Session["Devices"]!=null)
            {
                Session["Devices"] = null;
            }
            List<string> list = new List<string>();
            list.Add(col0); list.Add(col1); list.Add(col2); list.Add(col3); list.Add(col4); list.Add(col5); list.Add(col6); list.Add(col7);
            Session["Devices"] = list;
            string js = "OpenDevices();";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }
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