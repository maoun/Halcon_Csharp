// ***********************************************************************
// Assembly         : MyNet.Atmcs.Uscmcp.Web
// Author           : zlsyl
// Created          : 06-24-2016
//
// Last Modified By : zlsyl
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="DeviceDataCount.aspx.cs" company="ZKLT">
//     Copyright (c) ZKLT. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

/// <summary>
/// The Web namespace.
/// </summary>
namespace MyNet.Atmcs.Uscmcp.Web
{
    /// <summary>
    /// Class DeviceDataCount.
    /// </summary>
    public partial class DeviceDataCount : System.Web.UI.Page
    {
        /// <summary>
        /// The data count information
        /// </summary>
        private DataCountInfo dataCountInfo = new DataCountInfo();

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
            try
            {
                string username = Request.QueryString["username"];
                if (!userLogin.CheckLogin(username))
                {
                    string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                    System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                    return;
                }
                if (!this.IsPostBack)
                {
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("DeviceDataCount.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
                throw;
            }
        }
    }
}