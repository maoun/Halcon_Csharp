// ***********************************************************************
// Assembly         : MyNet.Atmcs.Uscmcp.Web
// Author           : zlsyl
// Created          : 11-14-2016
//
// Last Modified By : zlsyl
// Last Modified On : 11-14-2016
// ***********************************************************************
// <copyright file="CSBrowse.aspx.cs" company="ZKLT">
//     Copyright (c) ZKLT. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using System;
using System.Web.UI;
using MyNet.Common.Log;
/// <summary>
/// The Web namespace.
/// </summary>
namespace MyNet.Atmcs.Uscmcp.Web
{
    /// <summary>
    /// Class CSBrowse.
    /// </summary>
    public partial class CSBrowse : System.Web.UI.Page
    {
        /// <summary>
        /// The user login
        /// </summary>
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            try
            {
                if (!userLogin.CheckLogin(username))
                {
                    string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                    System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                    return;
                }
                if (!this.IsPostBack)
                {
                    string funcid = Request.QueryString["funcid"];
                    UserInfo userinfo = Session["userinfo"] as UserInfo;
                    string js = "SetUrl('" + userinfo.UserCode + "','" + funcid + "');";
                    ScriptManager.RegisterStartupScript(UpdatePanel1, this.Page.GetType(), "", js, true);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("CSBrowse.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
                throw;
            }
        }
    }
}