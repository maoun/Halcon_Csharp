// ***********************************************************************
// Assembly         : MyNet.Atmcs.Uscmcp.Web
// Author           : zlsyl
// Created          : 06-24-2016
//
// Last Modified By : zlsyl
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="ImageCut.aspx.cs" company="ZKLT">
//     Copyright (c) ZKLT. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Drawing;
using System.Text;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
/// <summary>
/// The Web namespace.
/// </summary>
namespace MyNet.Atmcs.Uscmcp.Web
{
    /// <summary>
    /// Class ImageCut.
    /// </summary>
    public partial class ImageCut : System.Web.UI.Page
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
            try
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
                    string imgwidth = Request.QueryString["imgwidth"];
                    string imgheight = Request.QueryString["imgheight"];
                    string url = "../FileUpload/TEMP/" + DateTime.Now.ToString("yyyyMMdd") + "/" + Request.QueryString["imgpath"].ToString();

                    showImages(url, imgwidth, imgheight);
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：用户登录", userinfo.NowIp, "0");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImageCut.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load发生异常");
            }
        }

        /// <summary>
        /// 显示图片
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="imgwidth"></param>
        /// <param name="imgheight"></param>
        /// <returns></returns>
        public void showImages(string imagePath, string imgwidth, string imgheight)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type=\"text/javascript\">");
                sb.Append("window.onload = function ShowImg(){");
                //if (!string.IsNullOrEmpty(imagePath))
                sb.Append("var imagePath1=\"" + imagePath + "\";");
                //else
                //    sb.Append("var imagePath1=" + "\"resizeimages/main.jpg\";");
                sb.Append("var ic = new ImgCropper(\"bgDiv\", \"dragDiv\", imagePath1, " + imgwidth + ", " + imgheight + ", {");
                sb.Append("Right: \"rRight\", Left: \"rLeft\", Up: \"rUp\", Down: \"rDown\",");
                sb.Append("RightDown: \"rRightDown\", LeftDown: \"rLeftDown\", RightUp: \"rRightUp\", LeftUp: \"rLeftUp\"");
                sb.Append("});");
                sb.Append("}");
                sb.Append("</script>");
                ClientScript.RegisterStartupScript(this.GetType(), "LoadPicScript", sb.ToString());
                //this.ResourceManager1.RegisterAfterClientInitScript(sb.ToString());
            }
            catch ( Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImageCut.aspx-showImages", ex.Message + "；" + ex.StackTrace, "showImages发生异常");
            }
        }

        /// <summary>
        /// Handles the Event event of the Cut control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public void Cut_Event(object sender, EventArgs e)
        {
            //截取
            try
            {
                int t = int.Parse(this.T.Value); int l = int.Parse(this.L.Value); int w = int.Parse(this.W.Value); int h = int.Parse(this.H.Value);

                string imgwidth = Request.QueryString["imgwidth"];
                string imgheight = Request.QueryString["imgheight"];
                string imgname = Request.QueryString["imgpath"].ToString();

                string strPath = Server.MapPath("~/FileUpload/TEMP/" + DateTime.Now.ToString("yyyyMMdd") + "/" + imgname);

                System.Drawing.Image image = System.Drawing.Image.FromFile(strPath);

                Bitmap bitmap = new Bitmap(w, h);
                Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
                Graphics canvas = Graphics.FromImage(bitmap);
                int randnum = rand.Next(0, int.MaxValue);
                Rectangle rec = new Rectangle(l, t, w, h);
                GraphicsUnit units = GraphicsUnit.Pixel;
                canvas.DrawImage(image, 0, 0, rec, units);
                string extension = strPath.Substring(strPath.LastIndexOf("."));
                imgname = imgname.Replace(extension, "_Cut" + DateTime.Now.ToString("ssfff") + extension);
                string newimgPath = Server.MapPath("~/FileUpload/TEMP/" + DateTime.Now.ToString("yyyyMMdd") + "/" + imgname);
                bitmap.Save(newimgPath);
                hidUrl.Value = "../FileUpload/TEMP/" + DateTime.Now.ToString("yyyyMMdd") + "/" + imgname;
                canvas.Dispose();
                image.Dispose();
                bitmap.Dispose();

                this.ResourceManager1.RegisterAfterClientInitScript("window.close();");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ImageCut.aspx-Cut_Event", ex.Message + "；" + ex.StackTrace, "Cut_Event发生异常");
            }
        }

        /// <summary>
        /// 提示窗体
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private void Notice(string title, string msg)
        {
            try
            {
                Notification.Show(new NotificationConfig
                {
                    Title = title,
                    Icon = Ext.Net.Icon.Information,
                    HideDelay = 2000,
                    Height = 120,
                    Html = "<br></br>" + msg + "!"
                });
            }
            catch
            {
            }
        }
    }
}