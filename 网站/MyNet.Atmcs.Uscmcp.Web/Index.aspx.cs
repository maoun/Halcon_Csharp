using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class Index : System.Web.UI.Page
    {
        private SystemManager systemManager = new SystemManager();
        private SettingManager settingManager = new SettingManager();
        private UserLogin userLogin = new UserLogin();
        private int muIndex = -1;

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.EnableViewState = false;
            if (!X.IsAjaxRequest)
            {
                lblSystemName.Html = "<strong><span style=\"font-family: 微软雅黑; font-size: 18pt; \">城市交通综合管控平台</span></strong>";
                userLogin.IsLogin();
                InitPage();
            }
        }

        private void InitPage()
        {
            string systemid = string.Empty;
            string systemName = string.Empty;
            try
            {
                UserInfo userinfo = Session["userinfo"] as UserInfo;
                UserManager userManager = new UserManager();
                DataTable dt = userManager.GetUserShowSystem(userinfo.UserCode);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        muIndex = -1;
                        Ext.Net.MenuItem but = new Ext.Net.MenuItem();
                        but.ID = "Button" + dt.Rows[i][0].ToString();
                        but.Text = dt.Rows[i][1].ToString();
                        but.Listeners.Click.Handler = "Index.ButtonClickEvent('" + dt.Rows[i][0].ToString() + "','" + dt.Rows[i][1].ToString() + "')";
                        but.Style.Add(HtmlTextWriterStyle.FontWeight, "blod");
                        //but.Html = "<span style=\"font-family: 微软雅黑; font-size: 9pt; color: #000066;\">" + dt.Rows[i][1].ToString()+"</span>";
                        //but.Icon = (Icon)Enum.Parse(typeof(Icon), dt.Rows[i][2].ToString(), true);
                        but.Style.Clear();
                        if (i == 0)
                        {
                            systemid = dt.Rows[i][0].ToString();
                            systemName = dt.Rows[i][1].ToString();
                        }
                        ToolbarSystem.Add(but);
                        if (i != (dt.Rows.Count - 1))
                        {
                            ToolbarSystem.Add(new ToolbarSeparator());
                        }
                    }
                }
                lblUserName.Text = "登录用户：" + userinfo.UserName;
                Ext.Net.Panel panelMenu = GetmenuPanel(systemid, systemName);
                if (panelMenu != null)
                {
                    PanelNavigate.Items.Add(panelMenu);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        private Ext.Net.Panel GetmenuPanel(string systemid, string systemName)
        {
            try
            {   UserInfo userinfo = Session["userinfo"] as UserInfo;
            DataTable dtdir = settingManager.GetUserSettingContent(systemid, "0", userinfo.UserCode);
             
                Ext.Net.Panel panelMenu = new Ext.Net.Panel();
                panelMenu.Layout = "Accordion";
                panelMenu.Header = false;
                panelMenu.Border = false;
                if (dtdir != null)
                {
                    for (int i = 0; i < dtdir.Rows.Count; i++)
                    {
                        Ext.Net.MenuPanel menuPanelItems = new Ext.Net.MenuPanel();
                        menuPanelItems.ID = "Nav" + dtdir.Rows[i][1].ToString();
                        menuPanelItems.Title = dtdir.Rows[i][2].ToString();
                        menuPanelItems.Icon = (Icon)Enum.Parse(typeof(Icon), dtdir.Rows[i][5].ToString(), true);
                        menuPanelItems.Border = false;
                        Ext.Net.MenuPanel menuPanel = IninPage(i, systemid, menuPanelItems, dtdir.Rows[i][1].ToString(), userinfo.UserCode);
                        if (menuPanel != null)
                        {
                            panelMenu.Items.Add(menuPanel);
                        }
                        OpenDefaultPage();
                    }
                }

                return panelMenu;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        private void AddNavigate(string systemid, string systemName)
        {
            PanelNavigate.RemoveAll(true);
            Ext.Net.Panel panelMenu = GetmenuPanel(systemid, systemName);

            if (panelMenu != null)
            {
                PanelNavigate.Items.Add(panelMenu);
                panelMenu.Render(PanelNavigate, RenderMode.Auto);
            }
        }

        private void OpenDefaultPage()
        {
            try
            {
                if (CurrentSelectMenu.Value != null)
                {
                    string[] menuvalues = CurrentSelectMenu.Value.ToString().Split(',');
                    string js = "MenuItemClickByUrl(\"" + menuvalues[0] + "\",\"" + menuvalues[1] + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        [DirectMethod]
        public void ButtonClickEvent(string systemid, string systemName)
        {
            UserInfo userInfo = System.Web.HttpContext.Current.Session["userinfo"] as UserInfo;
            if (userInfo == null)
            {
                Message("重新登录", "操作已超时，请重新登录！");
                Session.Abandon();
                Session.RemoveAll();
                Session.Clear();
                Response.Redirect("./login.aspx");
            }
            else
            {
                AddNavigate(systemid, systemName);
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
                HideDelay = 2000,
                Height = 120,
                Html = "<br></br>" + msg + "!"
            });
        }

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

        private Ext.Net.MenuPanel IninPage(int flag, string sysid, Ext.Net.MenuPanel pnlMenu, string contentId, string usercode)
        {
            try
            {
                if (string.IsNullOrEmpty(contentId))
                    return pnlMenu;

                UserManager userManager = new UserManager();
                DataTable dt = userManager.GetDirectory(sysid, contentId, usercode);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Ext.Net.MenuItem menuItem = new Ext.Net.MenuItem();
                        menuItem.ID = "Menu" + dt.Rows[i][0].ToString();
                        menuItem.Text = dt.Rows[i][1].ToString();
                        menuItem.Attributes["title"] = pnlMenu.Title + "|" + dt.Rows[i][2].ToString();
                        menuItem.Icon = (Icon)Enum.Parse(typeof(Icon), dt.Rows[i][5].ToString(), true);
                        menuItem.Listeners.Click.Fn = "MenuItemClick";
                        pnlMenu.Menu.Items.Add(menuItem);
                        if (muIndex == -1)
                        {
                            muIndex = flag;
                        }
                        if (flag == muIndex && i == 0)
                        {
                            CurrentSelectMenu.Value = menuItem.Attributes["title"] + "," + menuItem.Text;//设置默认显示第一个
                        }
                        if (dt.Rows[i][7].ToString() == "1")
                        {
                            CurrentSelectMenu.Value = menuItem.Attributes["title"] + "," + menuItem.Text;//设置显示设置默认页
                        }
                    }
                    return pnlMenu;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        protected void LinkExit_Click(object sender, DirectEventArgs e)
        {
            UserInfo userinfo = Session["userinfo"] as UserInfo;
            X.Msg.Confirm("信息", "确认要注销[" + userinfo.UserName + "]用户吗?", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = "Index.DoYes()",
                    Text = "是"
                },
                No = new MessageBoxButtonConfig
                {
                    Handler = "Index.DoNo()",
                    Text = "否"
                }
            }).Show();
        }
        /// <summary>
        /// 确定事件
        /// </summary>
        [DirectMethod]
        public void DoYes()
        {
            try
            {
                if (Session["LoginPage"] as string == "Login.aspx")
                {
                    Session.Remove("userinfo");
                    Session.Abandon();
                    Session.RemoveAll();
                    Session.Clear();
                    Response.Redirect("./Login.aspx");
                }
                else
                {
                    Session.Remove("userinfo");
                    Session.Abandon();
                    Session.RemoveAll();
                    Session.Clear();
                    Response.Redirect("./Default.aspx");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                //if (Session["LoginPage"] as string == "Login.aspx")
                //{
                //    Session.Remove("userinfo");
                //    Session.Abandon();
                //    Session.RemoveAll();
                //    Session.Clear();
                //    Response.Redirect("./Login.aspx");
                //}
                //else
                //{
                //Session.Remove("userinfo");
                //Session.Abandon();
                //Session.RemoveAll();
                //Session.Clear();
                Response.Redirect("./Login.aspx");
                //}
            }
        }
        /// <summary>
        /// 不删除事件
        /// </summary>
        [DirectMethod]
        public void DoNo()
        {
        }

        [DirectMethod]
        public string GetThemeUrl(string theme)
        {
            Theme temp = (Theme)Enum.Parse(typeof(Theme), theme);

            this.Session["Ext.Net.Theme"] = temp;
            return temp == Ext.Net.Theme.Default ? "Default" : this.ResourceManager1.GetThemeUrl(temp);
        }

        protected void LinkAbout_Click(object sender, DirectEventArgs e)
        {
            AddWindow();
        }

        protected void LinkPassword_Click(object sender, DirectEventArgs e)
        {
            AddWindowModify();
        }
        /// <summary>
        /// 显示自定义窗体
        /// </summary>
        private void AddWindow()
        {
            Window window = new Window();
            window.ID = "Systemabout";
            window.Title = "关于";
            window.Width = Unit.Pixel(400);
            window.Height = Unit.Pixel(300);

            window.Collapsible = true;
            window.Maximizable = false;
            window.Resizable = false;
            window.Hidden = true;

            window.Layout = "Fit";

            string xmlPath = Server.MapPath("./xml");

            window.Buttons.Add(CommonExt.AddButton("butCancelEdit", "关闭", "Cancel", window.ClientID + ".hide()"));
            window.Render(this.Form);
            window.Show();
        }

        private string GetAbout(string campany, string ver, string phone, string webhttp, string address)
        {
            string about = " <table width=\"100%\" border=\"0\" align=\"center\" cellpadding=\"0\"  cellspacing=\"0\" bgcolor=\"#E0F8E0\">";
            about += "<tr><td colspan=\"2\" vlign=\"middle\" height=\"80px\">&nbsp;&nbsp;&nbsp;<img src=\"Images/Login/about.png\" height=\"50px\" /></td></tr>";
            about += "<tr><td align=\"center\" width=\"120px\" height=\"30px\" >版权所有:</td><td>" + campany + "</td></tr>";
            about += "<tr><td align=\"center\" width=\"120px\" height=\"30px\" >软件版本:</td><td>" + ver + "</td></tr>";
            about += "<tr><td align=\"center\" width=\"120px\" height=\"30px\" >服务电话:</td><td>" + phone + "</td></tr>";
            about += "<tr><td align=\"center\" width=\"120px\" height=\"30px\" >公司网址:</td><td>" + webhttp + "</td></tr>";
            about += "<tr><td align=\"center\" width=\"120px\" height=\"30px\" >公司地址:</td><td>" + address + "</td></tr>";
            about += "	</table>";
            return about;
        }

        /// <summary>
        /// 显示修改窗体
        /// </summary>
        private void AddWindowModify()
        {
            Window window = new Window();
            window.ID = "UserModify";
            window.Title = "密码修改";
            window.Width = Unit.Pixel(400);
            window.Height = Unit.Pixel(200);
            window.Modal = true;
            window.Collapsible = true;
            window.Maximizable = false;
            window.Resizable = false;
            window.Hidden = true;
            window.AutoLoad.Mode = LoadMode.Merge;

            Ext.Net.Panel tabs = new Ext.Net.Panel();
            tabs.ID = "TabPanel1";
            tabs.IDMode = IDMode.Explicit;
            tabs.Border = false;

            Ext.Net.Panel tab = new Ext.Net.Panel();
            tab.Title = "";
            tab.Padding = 5;
            tab.Height = 120;
            tabs.Add(tab);
            tab.Items.Add(CommonExt.AddTextFieldPassword("txtMFirstPassWord", "初始密码", false));
            tab.Items.Add(CommonExt.AddTextFieldPassword("txtMPassWord", "新密码", false));
            tab.Items.Add(CommonExt.AddTextFieldPassword_Confirm("txtMConfirmPassWord", "重复密码", false, "txtMPassWord"));

            Toolbar toolbar = new Ext.Net.Toolbar();
            ToolbarFill toolbarFill = new ToolbarFill();
            toolbar.Add(toolbarFill);
            window.BottomBar.Add(toolbar);
            CommonExt.AddButton(toolbar, "butSaveEdit2", "保存", "Disk", "UserManager.UpdateData()");
            CommonExt.AddButton(toolbar, "butCancelEdit2", "取消", "Cancel", window.ClientID + ".hide()");
            window.Items.Add(tabs);
            window.Render(this.Form);
            window.Show();
        }
    }
}