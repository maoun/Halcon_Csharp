using System;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class SelectPage : System.Web.UI.Page
    {
        #region 成员变量

        private SettingManager settingManager = new SettingManager();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        #endregion 成员变量

        #region 事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                string type = Request.QueryString["type"];
                string where = " 1=1 ";
                if (!string.IsNullOrEmpty(type))
                {
                    where = " pagetype='" + type + "'";
                }
                this.ImageView.PrepareData.Handler = string.Concat("return ", "formatData(data);");
                DataTable dt = settingManager.GetTemplateInfo(where);
                dt = ConvertDatatable(dt);
                StorePage.DataSource = dt;
                StorePage.DataBind();
            }
        }

        #endregion 事件

        #region DirectMethod

        [DirectMethod]
        public void FeedBackUrl(string templateid, string templatepage)
        {
            TemplateInfo templateInfo = new TemplateInfo();
            templateInfo.TemplateId = templateid;
            templateInfo.TemplatePage = templatepage;
            Session["templatepage"] = templateInfo;
        }

        #endregion DirectMethod

        /// <summary>
        /// 转换datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable ConvertDatatable(DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        item.BeginEdit();
                        item["pageurl"] = ConvertUrl(item["pageurl"].ToString());
                        item.EndEdit();
                    }
                    return dt;
                }
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("SelectPage.aspx-ConvertDatatable", ex.Message+"；"+ex.StackTrace, "ConvertDatatable has an exception");
                return dt;
            }
        }
        /// <summary>
        /// 转换 url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ConvertUrl(string url)
        {
            if (url.IndexOf("pages/collect") >= 0)
            {
                try
                {
                    string itgsurl = System.Configuration.ConfigurationManager.AppSettings["itgs"].ToString();
                    if (!String.IsNullOrEmpty(itgsurl))
                    {
                        if (!itgsurl.Substring(itgsurl.Length - 1).Equals("/"))
                        {
                            itgsurl = itgsurl + "/";
                        }
                        if (url.Substring(0, 1).Equals("/"))
                        {
                            url = url.Substring(1);
                        }
                        return itgsurl + url;
                    }
                    return url;
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex);
                    logManager.InsertLogError("SelectPage.aspx-ConvertUrl", ex.Message+"；"+ex.StackTrace, "ConvertUrl has an exception");
                    return url;
                }
            }
            return url;
        }
    }
}