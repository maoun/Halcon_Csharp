// ***********************************************************************
// Assembly         : MyNet.Atmcs.Uscmcp.Web
// Author           : zlsyl
// Created          : 06-24-2016
//
// Last Modified By : zlsyl
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="CCTVSingleBrowse.aspx.cs" company="ZKLT">
//     Copyright (c) ZKLT. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Web.UI;
using MyNet.Atmcs.Uscmcp.Bll;

/// <summary>
/// The Web namespace.
/// </summary>
namespace MyNet.Atmcs.Uscmcp.Web
{
    /// <summary>
    /// Class CCTVSingleBrowse.
    /// </summary>
    public partial class CCTVSingleBrowse : System.Web.UI.Page
    {
        /// <summary>
        /// The user login
        /// </summary>
        private UserLogin userLogin = new UserLogin();

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"]; 
            if (!userLogin.CheckLogin(username))
            { 
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" +StaticInfo.LoginPage + "'"; 
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); 
                return; 
            }
            string videoUrl = Request.QueryString["videoUrl"];//获得查询序号

            //string videoUrl = "niho|192.168.2.28|37777|0|admin|admin|4";

            ScriptManager.RegisterStartupScript(UpdatePanel1, this.Page.GetType(), "", "SetUrl('" + videoUrl + "');", true);
        }
    }
}