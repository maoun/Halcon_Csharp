using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace MyNet.Atmcs.Web
{
    public partial class ExMainIndex : System.Web.UI.Page
    {
        #region 成员变量

        private SystemManager systemManager = new SystemManager();
        private SettingManager settingManager = new SettingManager();
        private UserLogin userLogin = new UserLogin();
        private UserManager userManager = new UserManager();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Uscmcp.Bll.LogManager();

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
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            this.EnableViewState = false;

            if (!X.IsAjaxRequest)
            {
                /* ResourceManager1.SetTheme(Ext.Net.Theme.Slate);*/
                userLogin.IsLogin();
                SetBackgroundOfBody();
                string contentId = "";
                try
                {
                    contentId = Request.QueryString["ContentId"].ToString();
                }
                catch
                {
                    if (string.IsNullOrEmpty(contentId))
                    {
                        contentId = "0102";
                    }
                }
                InitPage(contentId);
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "访问：用户登录", userinfo.NowIp, "0");
            }
        }

        #endregion 事件

        #region 私有方法

        /// <summary>
        /// 初始化加载
        /// </summary>
        private void InitPage(string contentId)
        {
            string systemid = string.Empty;
            string systemName = string.Empty;
            try
            {
                UserInfo userinfo = Session["userinfo"] as UserInfo;
                if (userinfo == null)
                {
                    userinfo = new UserInfo();
                    userinfo.SystemCode = "00";
                    userinfo.UserCode = "000001";
                }
                UserManager userManager = new UserManager();
                if (contentId.Equals("0101"))
                {
                    CreateFirstMenu(userinfo.UserCode, userinfo.SystemCode, contentId);
                }
                else
                {
                    Ext.Net.Panel panelMenu = GetmenuPanel(userinfo.UserCode, userinfo.SystemCode, contentId);
                    if (panelMenu != null)
                    {
                        PanelLeft.Items.Add(panelMenu);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExMainIndex.aspx-InitPage", ex.Message+"；"+ex.StackTrace, "InitPage has an exception");
            }
        }

        private string GetUrlPath(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url)) return url;
                string[] urls = url.Split('/');
                return url.Replace(urls[urls.Length - 1], "");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExMainIndex.aspx-GetUrlPath", ex.Message+"；"+ex.StackTrace, "GetUrlPath has an exception");
                return url;
            }
        }

        /// <summary>
        /// 设置页面背景图片
        /// </summary>
        private void SetBackgroundOfBody()
        {
            try
            {
                UserInfo userinfo = Session["userinfo"] as UserInfo;
                if (userinfo == null)
                {
                    userinfo = new UserInfo();
                    userinfo.SystemCode = "00";
                    userinfo.UserCode = "000001";
                }
                if (userinfo != null)
                {
                    string backgroung = "";
                    if (string.IsNullOrEmpty(userinfo.UserBackGround))
                    {
                        backgroung = "../Images/BackGround/Default/bg.jpg";
                    }
                    else
                    {
                        backgroung = userinfo.UserBackGround;
                    }

                    string filePath = Server.MapPath(backgroung.Replace("..", "~"));
                    if (!File.Exists(filePath))
                    {
                        backgroung = "../Images/BackGround/Default/bg.jpg";
                    }
                    Session["srcOfBackImage"] = backgroung;
                    bd.Attributes["style"] = "background-image: url(" + backgroung + ");background-size: 100%)";
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExMainIndex.aspx-SetBackgroundOfBody", ex.Message+"；"+ex.StackTrace, "SetBackgroundOfBody has an exception");
            }
        }

        /// <summary>
        /// 组织首页 *
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="systemid"></param>
        /// <param name="contentCode"></param>
        private void CreateFirstMenu(string usercode, string systemid, string contentCode)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                str.Append("    <div id='PanelLeftMenu' class='content' style='width: 230px; height: 100%; position: fixed; top: 90px; left: -258px; z-index: 999999; border: 1px solid rgb(251, 80, 4); background:rgba(252,253,252,0.5);overflow:hidden;'>");
                str.Append("   <ul style='height:100%'>");

                DataTable dtContent = userManager.GetDirectory(systemid, contentCode, usercode);
                if (dtContent != null && dtContent.Rows.Count > 0)
                {
                    for (int i = 0; i < dtContent.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            str.Append("<li style='position:relative;float:left;width:230px;height:200px;'><img class='active' src='../Images/Template/Tem" + dtContent.Rows[i]["col0"].ToString() + "_1.png' width='210' name='" + dtContent.Rows[i]["col2"].ToString() + "' height='120' style='margin:19px 23px 23px 10px;cursor:pointer' onclick='MenuItemClick(this.name);'><span style='position:absolute;top:153px;left:67px;font-size: 18px;font-family: 楷体;color:#fc5004' >" + dtContent.Rows[i]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "</span></li>");
                        }
                        else
                        {
                            str.Append("<li style='position:relative;float:left;width:230px;height:200px;'><img  src='../Images/Template/Tem" + dtContent.Rows[i]["col0"].ToString() + ".png' width='210' name='" + dtContent.Rows[i]["col2"].ToString() + "' height='120' style='margin:19px 23px 23px 10px;cursor:pointer' onclick='MenuItemClick(this.name);'><span style='position:absolute;top:153px;left:67px;font-size: 18px;font-family: 楷体;color:black' >" + dtContent.Rows[i]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "</span></li>");
                        }
                        if (dtContent.Rows[i]["col8"].ToString().Equals("1"))
                        {
                            CurrentSelectMenu.Value = dtContent.Rows[i]["col2"].ToString();
                        }
                    }
                }
                str.Append(" </ul> </div>");
                str.Append("    <div id='seletMenu' style='z-index: 999999;width:130px;height:50px;position:fixed;left:0px;top: 50%;margin-top:25px;cursor:pointer'>");
                str.Append("    <span >选择模板</span></div>");
                TemplatePanel.InnerHtml = str.ToString();

                OpenDefaultPage();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExMainIndex.aspx-CreateFirstMenu", ex.Message+"；"+ex.StackTrace, "CreateFirstMenu has an exception");
            }
        }

        /// <summary>
        /// 添加修改左侧panel *
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="systemcode"></param>
        /// <param name="contentCode"></param>
        public void UpdateLeftPanel(string usercode, string systemcode, string contentCode)
        {
            try
            {
                if (contentCode.Equals("0101"))
                {
                    PanelLeft.RemoveAll(true);
                    Ext.Net.Panel panelMenu = GetmenuPanel(usercode, systemcode, contentCode);
                    panelMenu.Render(PanelLeft, RenderMode.Auto);
                }
                else
                {
                    PanelLeft.RemoveAll(true);
                    Ext.Net.Panel panelMenu = GetmenuPanel(usercode, systemcode, contentCode);
                    if (panelMenu != null)
                    {
                        PanelLeft.Items.Add(panelMenu);
                        panelMenu.Render(PanelLeft, RenderMode.Auto);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExMainIndex.aspx-UpdateLeftPanel", ex.Message+"；"+ex.StackTrace, "UpdateLeftPanel has an exception");
            }
        }

        /// <summary>
        /// 构造panel 中全部控件 *
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="systemid"></param>
        /// <param name="contentCode"></param>
        /// <returns></returns>
        private Ext.Net.Panel GetmenuPanel(string usercode, string systemid, string contentCode)
        {
            try
            {
                Ext.Net.Panel panelMenu = new Ext.Net.Panel();
                if (!contentCode.Equals("0101"))
                {
                    panelMenu.Border = false;
                    panelMenu.Padding = 0;
                    panelMenu.Cls = "ex-panel-backgroundImage";
                    panelMenu.BaseCls = "ex-panel";

                    Ext.Net.Panel menuPanel = AddNavigate(systemid, contentCode, usercode);
                    if (menuPanel != null)
                    {
                        panelMenu.Items.Add(menuPanel);
                    }
                }
                else
                {
                    DataTable dtContent = userManager.GetDirectory(systemid, contentCode, usercode);

                    if (dtContent != null && dtContent.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtContent.Rows.Count; i++)
                        {
                            if (dtContent.Rows[i]["col8"].ToString().Equals("1"))
                            {
                                CurrentSelectMenu.Value = dtContent.Rows[i]["col2"].ToString();
                            }
                        }
                    }
                }
                OpenDefaultPage();

                return panelMenu;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExMainIndex.aspx-GetmenuPanel", ex.Message+"；"+ex.StackTrace, "GetmenuPanel has an exception");
                return null;
            }
        }

        /// <summary>
        /// 添加右侧导航栏 *
        /// </summary>
        /// <param name="systemcode"></param>
        /// <param name="contentcode"></param>
        /// <param name="usercode"></param>
        private Ext.Net.Panel AddNavigate(string systemcode, string contentcode, string usercode)
        {
            try
            {
                Ext.Net.Panel PanelMenuItem = new Ext.Net.Panel();
                PanelMenuItem.ID = "PanelMenuItem" + contentcode;
                PanelMenuItem.Padding = 0;
                PanelMenuItem.BaseCls = "BaseCls";
                UserManager userManager = new UserManager();
                DataTable dtContent = userManager.GetDirectory(systemcode, contentcode, usercode);

                if (dtContent != null && dtContent.Rows.Count > 0)
                {
                    for (int i = 0; i < dtContent.Rows.Count; i++)
                    {
                        string str = dtContent.Rows[i]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "");

                        Ext.Net.Button but = new Ext.Net.Button();
                        int intcount = 120 * i;
                        but.Text = str;
                        but.ID = "but" + dtContent.Rows[i]["col0"].ToString();
                        but.Width = 66;
                        but.Height = 60;

                        but.Cls = "ex-panel-background";

                        but.Html = "<span name='" + intcount.ToString() + "' style='width:66px;height:60px;display:block'><a style='width:66px;height:60px;display:block;line-height:60px;font-size:0px;background-image:url(../Images/Left/menuico" + dtContent.Rows[i]["col0"].ToString().Substring(0, 4) + ".png);background-position:0 -" + intcount + "px' href='javascript:void(0)'>" + str + "</a></span>";
                        //but.Html = "<img defaults=" + dtContent.Rows[i]["col0"].ToString() + " src='Image/button/img_" + dtContent.Rows[i]["col0"].ToString() + ".png' alt='" + str + "' />";//这里是初始化
                        if (i == 0)
                        {
                            but.AddClass("active");
                            but.Html = "<span name='" + intcount + "' style='width:130px;height:60px;display:block'><a style='width:66px;height:60px;line-hight:60px;display:block;font-size:18px;background-image:url(../Images/Left/menuico" + dtContent.Rows[i]["col0"].ToString().Substring(0, 4) + ".png);margin-left:66px;background-position:0 -60px;background-repeat: no-repeat;padding-left:8px;text-indent:-130px;text-decoration:none;color:white' href='javascript:void(0)'>" + str + "</a></span>";
                        }

                        if (dtContent.Rows[i]["col3"].ToString().Equals("2"))
                        {
                            but.Listeners.Click.Handler = "MenuItemClick('" + dtContent.Rows[i]["col2"].ToString() + "?funcid=" + dtContent.Rows[i]["col0"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "');";
                            CurrentSelectMenu.Value = dtContent.Rows[0]["col2"].ToString() + "?funcid=" + dtContent.Rows[0]["col0"].ToString().Trim().Replace("/r", "").Replace("/n", "");
                        }
                        else
                        {
                            but.Listeners.Click.Handler = "MenuItemClick('" + dtContent.Rows[i]["col2"].ToString() + "?type=" + dtContent.Rows[i]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "');";
                            CurrentSelectMenu.Value = dtContent.Rows[0]["col2"].ToString() + "?type=" + dtContent.Rows[0]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "");
                        }

                        PanelMenuItem.Items.Add(but);
                    }
                }
                else
                {
                    CurrentSelectMenu.Value = "blank.aspx?type='不具备该功能'";
                }
                return PanelMenuItem;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExMainIndex.aspx-AddNavigate", ex.Message+"；"+ex.StackTrace, "AddNavigate has an exception");
                return null;
            }
        }

        /// <summary>
        /// 打开第一页 *
        /// </summary>
        private void OpenDefaultPage()
        {
            try
            {
                if (CurrentSelectMenu.Value != null)
                {
                    string js = "MenuItemClick('" + CurrentSelectMenu.Value.ToString() + "');";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExMainIndex.aspx-OpenDefaultPage", ex.Message+"；"+ex.StackTrace, "OpenDefaultPage has an exception");
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
                    Html = "<br></br>" + msg + "!"
                });
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExMainIndex.aspx-Notice", ex.Message+"；"+ex.StackTrace, "Notice has an exception");
            }
        }

        /// <summary>
        /// 截取URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetImgUrl(string url)
        {
            try
            {
                string[] urls = url.Split('/');
                string u = "";
                for (int i = 0; i < urls.Length - 2; i++)
                {
                    u += urls[i] + "/";
                }
                return u;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("ExMainIndex.aspx-GetImgUrl", ex.Message+"；"+ex.StackTrace, "GetImgUrl has an exception");
                return "";
            }
        }

        #endregion 私有方法
    }
}