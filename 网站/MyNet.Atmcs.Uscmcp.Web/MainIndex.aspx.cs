using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Web
{
    public partial class MainIndex : System.Web.UI.Page
    {
        #region 成员变量

        private SystemManager systemManager = new SystemManager();
        private SettingManager settingManager = new SettingManager();
        private UserLogin userLogin = new UserLogin();
        private UserManager userManager = new UserManager();

        #endregion 成员变量

        #region 事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.EnableViewState = false;
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                InitPage();
                Session["num"] = 1;
                if (Request.QueryString["dFile"] != null && Request.QueryString["dFile"] != "")
                {
                    //要下载文件的名称
                    string downFile = Request.QueryString["dFile"].ToString();
                    //调用DownLoadFile()方法 下载文件
                    DownLoadFile(downFile);
                }
                JavaText.Value = System.Configuration.ConfigurationManager.AppSettings["itgs"];
                this.DataBind();
            }
        }

        private void SetScreen(UserInfo userinfo)
        {
            hidScreenNum.Value = userinfo.ScreenNum;
            hidScreen1.Value = userinfo.Screen1;
            hidScreen2.Value = userinfo.Screen2;
            hidScreen3.Value = userinfo.Screen3;
            hidScreen4.Value = userinfo.Screen4;
            hidScreen5.Value = userinfo.Screen5;
            hidScreen6.Value = userinfo.Screen6;
            hidScreen7.Value = userinfo.Screen7;
            hidScreen8.Value = userinfo.Screen8;
            hidScreen9.Value = userinfo.Screen9;
            hidDefaultBack.Value = userinfo.UserBackGround;
        }

        /// <summary>
        /// 下载功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void downLoad_Click(object sender, EventArgs e)
        {
            try
            {
                DownLoadFile(StaticInfo.FileName);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 文件选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FileUploadSelect(object sender, DirectEventArgs e)
        {
            try
            {
                if (this.ImgFile.HasFile)
                {
                    string imgUrl = UploadFile();
                    //string js = "addImage('" + imgUrl + "');";
                    //this.ResourceManager1.RegisterAfterClientInitScript(js);
                    getBackImage();
                    //AddBackImage(relativeUrl);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        #endregion 事件

        #region DirectMethod

        /// <summary>
        /// 隐去用户中心窗口
        /// </summary>
        [DirectMethod]
        public void Close2()
        {
            Session["num"] = 1;
            Element l2 = X.Get("Layer2").ChainOn();

            //l2.SlideOut("r").Render();  //滑动隐藏
            l2.AlignTo(personalCenter, "tr-br", new int[] { 0, 0 }).Toggle().Render();
        }

        /// <summary>
        /// 上方菜单点击事件
        /// </summary>
        /// <param name="contentCode"></param>
        [DirectMethod]
        public void MainMenuItemClick(string contentCode)
        {
            try
            {
                if (!string.IsNullOrEmpty(contentCode))
                {
                    UserInfo userinfo = Session["userinfo"] as UserInfo;
                    if (userinfo == null)
                    {
                        string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                        this.ResourceManager1.RegisterAfterClientInitScript(js);
                        //Response.Redirect("Login.aspx");
                    }
                    if (Session["Condition"] != null)
                    {
                        Session["Condition"] = null;
                    }
                    UpdateLeftPanel(userinfo.UserCode, userinfo.SystemCode, contentCode);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        public void SaveUserTemplate()
        {
        }

        /// <summary>
        /// 保存用户选择（背景图片的选择）
        /// </summary>
        [DirectMethod]
        public void imageSave(string src)
        {
            try
            {
                UserInfo userinfo = Session["userinfo"] as UserInfo;
                if (userinfo != null)
                {
                    hidDefaultBack.Value = src;
                    if (settingManager.UpdateTemplatePageInfo("UserBackGround", src, userinfo.UserCode) > 0)
                    {
                        windowCancel();
                        Notice("更换背景", "更换成功");
                    }
                }
                else
                {
                    Notice("更换背景", "请先登录");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 删除背景图片
        /// </summary>
        /// <param name="src"></param>
        [DirectMethod]
        public void windowDelete(string src)
        {
            try
            {
                if (src.Equals(hidDefaultBack.Value))
                {
                    Notice("提示", "不能删除当前背景图片");
                    return;
                }
                UserInfo userinfo = Session["userinfo"] as UserInfo;
                if (userinfo != null)
                {
                    try
                    {
                        string tempPath = Server.MapPath(src.Replace("..", "~"));
                        FileInfo fi = new FileInfo(tempPath);
                        if (fi.Exists)
                        {
                            File.Delete(tempPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        ILog.WriteErrorLog(ex);
                    }

                    Notice("提示", "选中背景图片删除成功");
                    // 重新加载所有背景图片
                    backImagesLoad(userinfo.UserCode);
                    string js = "SetDefaultBackGround();";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
                else
                {
                    Notice("更换背景", "请先登录");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 关闭修改背景窗口
        /// </summary>
        [DirectMethod]
        public void windowCancel()
        {
            try
            {
                WindowChangeBack.Hide();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 显示修改密码窗体
        /// </summary>
        [DirectMethod]
        public void AddWindowModify()
        {
            try
            {
                if (Session["userinfo"] != null)
                {
                    UserInfo userinfo = Session["userinfo"] as UserInfo;

                    Window window = new Window();
                    window.StyleSpec = "text-align:center";
                    window.ID = "UserModify";
                    window.Title = "<p class='title'>密码修改</p>";
                    window.Width = 610;
                    window.Height = 369;

                    window.Modal = true;
                    window.Maximizable = false;
                    window.Resizable = false;
                    window.Hidden = true;
                    window.AutoLoad.Mode = LoadMode.Merge;

                    HtmlGenericControl hr = new HtmlGenericControl("hr");
                    hr.Attributes["style"] = "height:1px;border:none;border-top:1px solid silver;";
                    window.ContentControls.Add(hr);
                    Ext.Net.Panel tabs = new Ext.Net.Panel();
                    tabs.ID = "TabPanel1";
                    tabs.IDMode = IDMode.Explicit;
                    tabs.Border = false;
                    //tabs.Height = 200;

                    Ext.Net.Panel tab = new Ext.Net.Panel();
                    tab.Padding = 90;

                    tabs.Add(tab);
                    tab.Items.Add(CommonExt.AddTextFieldPassword("txtMFirstPassWord", "初始密码", false));
                    tab.Items.Add(CommonExt.AddTextFieldPassword("txtMPassWord", "新密码", false));
                    tab.Items.Add(CommonExt.AddTextFieldPassword_Confirm("txtMConfirmPassWord", "重复密码", false, "txtMPassWord"));

                    Toolbar toolbar = new Ext.Net.Toolbar();
                    ToolbarFill toolbarFill = new ToolbarFill();

                    toolbar.Add(toolbarFill);
                    toolbar.X = -250;

                    window.BottomBar.Add(toolbar);
                    CommonExt.AddButton(toolbar, "butSaveEdit2", "保存", "Disk", "MainIndex.UpdateData()");
                    CommonExt.AddButton(toolbar, "butCancelEdit2", "取消", "Cancel", window.ClientID + ".hide()");
                    window.ContentControls.Add(tabs);
                    window.Render(this.Form);
                    window.Show();
                }
                else
                {
                    Notice("提示信息", "请选择要修改的用户");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 修改密码页
        /// </summary>
        [DirectMethod]
        public void UpdateData()
        {
            try
            {
                if (Session["userinfo"] != null)
                {
                    UserInfo userinfo = Session["userinfo"] as UserInfo;
                    string Id = userinfo.UserCode;
                    string pwd = X.GetCmp<TextField>("txtMPassWord").Text;
                    string oldPwd = X.GetCmp<TextField>("txtMFirstPassWord").Text;
                    string confirmPwd = X.GetCmp<TextField>("txtMConfirmPassWord").Text;
                    if (oldPwd != "" && pwd != "" && confirmPwd != "")
                    {
                        if (pwd == confirmPwd)
                        {
                            if (userManager.CheckOldPwd(Id, oldPwd))
                            {
                                if (userManager.ChangeUserPwd(Id, Cryptography.Encrypt(pwd)) > 0)
                                {
                                    Notice("密码修改", "修改成功");
                                    X.GetCmp<Window>("UserModify").Hide();
                                }
                            }
                            else
                            { Notice("密码修改", "原始密码错误"); }
                        }
                        else
                        {
                            Notice("密码修改", "密码与确认密码不一致！");
                        }
                    }
                    else
                    {
                        Notice("密码修改", "你有信息漏填！");
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 用户信息页
        /// </summary>
        [DirectMethod]
        public void userInfoShow()
        {
            try
            {
                GetUserInfo();
                PanelUserInfo.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 下载页面
        /// </summary>
        [DirectMethod]
        public void downLoadShow()
        {
            try
            {
                //GetFileList(Server.MapPath(StaticInfo.WebSite));
                PanelDowload.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 关于我们页面
        /// </summary>
        [DirectMethod]
        public void aboutMeShow()
        {
            try
            {
                GetAboutMeInfo();
                Window3.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 更换背景页面
        /// </summary>
        [DirectMethod]
        public void backImageShow()
        {
            try
            {
                getBackImage();
                WindowChangeBack.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        [DirectMethod]
        public void Exit()
        {
            try
            {
                X.Msg.Confirm("提示", "确认要退出登录吗?", new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "MainIndex.DoYes()",
                        Text = "是"
                    },
                    No = new MessageBoxButtonConfig
                    {
                        Handler = "MainIndex.DoNo()",
                        Text = "否"
                    }
                }).Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 确定事件
        /// </summary>
        [DirectMethod]
        public void DoYes()
        {
            try
            {
                LogManager logManager = new LogManager();
                string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                if (ipaddress.Length < 9)
                {
                    ipaddress = "127.0.0.1";
                }
                UserInfo userInfo = Session["UserInfo"] as UserInfo;
                logManager.InsertLogRunning(userInfo.UserName, "访问：用户注销", ipaddress, "0");
                Session.Remove("UserInfo");
                Session.Abandon();
                Session.RemoveAll();
                Session.Clear();
                this.ResourceManager1.RegisterAfterClientInitScript("window.open('../login.aspx','_parent')");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 不退出事件
        /// </summary>
        [DirectMethod]
        public void DoNo()
        {
        }

        #endregion DirectMethod

        #region 私有方法

        /// <summary>
        /// 初始化加载（加载上面的功能模块）
        /// </summary>
        private void InitPage()
        {
            string systemid = string.Empty;
            string systemName = string.Empty;
            try
            {
                UserInfo userinfo = Session["userinfo"] as UserInfo;
                SetScreen(userinfo);
                SetBackgroundOfBody(userinfo);
                userNameShow(userinfo);
                UserManager userManager = new UserManager();
                StringBuilder mainMenuHtml = new StringBuilder();
                StringBuilder moreMenuHtml = new StringBuilder();
                int contentCode = 0;
                DataTable dt = userManager.GetUserShowSystem(userinfo.UserCode);

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        systemid = dt.Rows[i]["col0"].ToString();
                        userinfo.SystemCode = systemid;
                        Session["userinfo"] = userinfo;
                        DataTable dtdir = settingManager.GetUserSettingContent(systemid, "0", userinfo.UserCode);

                        if (dtdir != null && dtdir.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtdir.Rows.Count; j++)
                            {
                                if (!string.IsNullOrEmpty(dtdir.Rows[j]["col0"].ToString()))
                                {
                                    contentCode = Convert.ToInt32(dtdir.Rows[j]["col0"].ToString().Substring(0, 2));
                                }
                                if (contentCode < 8)
                                {
                                    if (j == 0)
                                    {
                                        mainMenuHtml.Append("<li class='item dads-children activees'><span id='" + dtdir.Rows[j]["col0"].ToString() + "'  href=''  onclick='MainmenuItemClick(this.id);'  class='icoB channel" + contentCode.ToString() + "' title='" + dtdir.Rows[j]["col1"].ToString() + "'></span></li>");
                                    }
                                    else
                                    {
                                        string ztUrl = GetUrlPath(Request.Url.AbsoluteUri) + "Common/ExMainIndex.aspx?ContentId=" + dtdir.Rows[j]["col1"].ToString();
                                        mainMenuHtml.Append("<li class='item dads-children'><span id='" + dtdir.Rows[j]["col0"].ToString() + "'  href=''  onclick='MainmenuItemClick(this.id);'  class='icoB channel" + contentCode.ToString() + "' title='" + dtdir.Rows[j]["col1"].ToString() + "'  dir='" + ztUrl + "' ></span></li>");
                                    }
                                }
                                else
                                {
                                    moreMenuHtml.Append("<li><span id='" + dtdir.Rows[j]["col0"].ToString() + "'  href='#'    onclick='MainmenuItemClick(this.id);'  title='" + dtdir.Rows[j]["col1"].ToString() + "'>" + dtdir.Rows[j]["col1"].ToString() + "</span></li>");
                                }
                            }
                            mainMenuHtml.Append("<li id='morelist'><span class='icoB channel8' title='更多'></span></li>");
                            nav.InnerHtml = mainMenuHtml.ToString();
                            ulmoremenu.InnerHtml = moreMenuHtml.ToString();
                            if (dtdir.Rows[0]["col0"].ToString().Equals("0101"))
                            {
                                CreateFirstMenu(userinfo.UserCode, userinfo.SystemCode, dtdir.Rows[0]["col0"].ToString(), userinfo.FirstTemplate);
                            }
                            else
                            {
                                Ext.Net.Panel panelMenu = GetmenuPanel(userinfo.UserCode, userinfo.SystemCode, dtdir.Rows[0]["col0"].ToString());
                                if (panelMenu != null)
                                {
                                    PanelLeft.Items.Add(panelMenu);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
                return url;
            }
        }

        /// <summary>
        /// 设置页面背景图片
        /// </summary>
        private void SetBackgroundOfBody(UserInfo userinfo)
        {
            try
            {
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
            }
        }

        /// <summary>
        /// 展示背景图片
        /// </summary>
        private void AddBackImage(int i, string imageUrl)
        {
            try
            {
                HtmlGenericControl img = new HtmlGenericControl("img");
                img.ID = i.ToString();
                img.Attributes["src"] = imageUrl;
                img.Attributes["class"] = "backImage";
                img.Attributes["onclick"] = "getImageSrc('" + img.ID + "','" + imageUrl + "')";
                //divBackImage.Controls.Add(img);
                divImages.Controls.Add(img);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 用户姓名显示（页面右上角）
        /// </summary>
        private void userNameShow(UserInfo userinfo)
        {
            try
            {
                HtmlGenericControl a = new HtmlGenericControl("a");
                a.Attributes["href"] = "javascript:void(0)";
                if (Session["userinfo"] != null)
                {
                    a.InnerText = userinfo.Name;
                }
                else
                {
                    a.InnerText = "个人中心";
                }
                personlCenter.Controls.Add(a);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 组织首页 *
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="systemid"></param>
        /// <param name="contentCode"></param>
        private void CreateFirstMenu(string usercode, string systemid, string contentCode, string firsttemplate)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                str.Append("    <div id='PanelLeftMenu' class='content' style='width: 230px; height: 100%; position: fixed; top: 90px; left: -258px; z-index: 9999; border: 1px solid rgb(251, 80, 4); background:rgba(252,253,252,0.5);overflow:hidden;'>");
                str.Append("   <ul style='height:100%'>");

                DataTable dtContent = userManager.GetDirectory(systemid, contentCode, usercode);
                if (dtContent != null && dtContent.Rows.Count > 0)
                {
                    for (int i = 0; i < dtContent.Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty(firsttemplate))
                        {
                            if (i.Equals(0))
                            {
                                str.Append("<li style='position:relative;float:left;width:230px;height:200px;'><img class='active' src='../Images/Template/Tem" + dtContent.Rows[i]["col0"].ToString() + "_1.png' width='210' name='" + dtContent.Rows[i]["col2"].ToString() + "' height='120' style='margin:19px 23px 23px 10px;cursor:pointer' onclick='MenuItemClick(this.name);'><span style='position:absolute;top:153px;left:67px;font-size: 18px;font-family: 楷体;color:#fc5004' >" + dtContent.Rows[i]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "</span></li>");
                                CurrentSelectMenu.Value = dtContent.Rows[i]["col2"].ToString();
                            }
                            else
                            {
                                str.Append("<li style='position:relative;float:left;width:230px;height:200px;'><img  src='../Images/Template/Tem" + dtContent.Rows[i]["col0"].ToString() + ".png' width='210' name='" + dtContent.Rows[i]["col2"].ToString() + "' height='120' style='margin:19px 23px 23px 10px;cursor:pointer' onclick='MenuItemClick(this.name);'><span style='position:absolute;top:153px;left:67px;font-size: 18px;font-family: 楷体;color:black' >" + dtContent.Rows[i]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "</span></li>");
                            }
                        }
                        else if (dtContent.Rows[i]["col0"].ToString().Equals(firsttemplate))
                        {
                            str.Append("<li style='position:relative;float:left;width:230px;height:200px;'><img class='active' src='../Images/Template/Tem" + dtContent.Rows[i]["col0"].ToString() + "_1.png' width='210' name='" + dtContent.Rows[i]["col2"].ToString() + "' height='120' style='margin:19px 23px 23px 10px;cursor:pointer' onclick='MenuItemClick(this.name);'><span style='position:absolute;top:153px;left:67px;font-size: 18px;font-family: 楷体;color:#fc5004' >" + dtContent.Rows[i]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "</span></li>");
                            CurrentSelectMenu.Value = dtContent.Rows[i]["col2"].ToString();
                        }
                        else
                        {
                            str.Append("<li style='position:relative;float:left;width:230px;height:200px;'><img  src='../Images/Template/Tem" + dtContent.Rows[i]["col0"].ToString() + ".png' width='210' name='" + dtContent.Rows[i]["col2"].ToString() + "' height='120' style='margin:19px 23px 23px 10px;cursor:pointer' onclick='MenuItemClick(this.name);'><span style='position:absolute;top:153px;left:67px;font-size: 18px;font-family: 楷体;color:black' >" + dtContent.Rows[i]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "</span></li>");
                        }
                    }
                }
                str.Append(" </ul> </div>");
                str.Append("    <div id='seletMenu' style='z-index: 9999;width:130px;height:50px;position:fixed;left:0px;top: 50%;margin-top:25px;cursor:pointer'>");
                str.Append("    <span >选择模板</span></div>");
                TemplatePanel.InnerHtml = str.ToString();

                OpenDefaultPage();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
                return null;
            }
        }

        /// <summary>
        /// 添加右侧导航栏 * (点击事件)
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
                            but.Html = "<span name='" + intcount + "' style='width:130px;height:60px;line-height:60px;display:block'><a style='width:66px;height:60px;line-height:60px;display:block;font-size:18px;background-image:url(../Images/Left/menuico" + dtContent.Rows[i]["col0"].ToString().Substring(0, 4) + ".png);margin-left:66px;background-position:0 -60px;background-repeat: no-repeat;padding-left:8px;text-indent:-130px;text-decoration:none;color:white' href='javascript:void(0)'>" + str + "</a></span>";
                        }

                        if (dtContent.Rows[i]["col3"].ToString().Equals("2"))
                        {
                            but.Listeners.Click.Handler = "MenuItemClick('" + dtContent.Rows[i]["col2"].ToString() + "?funcid=" + dtContent.Rows[i]["col0"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "&funcname=" + dtContent.Rows[i]["col9"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "-" + dtContent.Rows[i]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "&funcid=" + dtContent.Rows[i]["col0"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "');";
                            CurrentSelectMenu.Value = dtContent.Rows[0]["col2"].ToString() + "?funcid=" + dtContent.Rows[0]["col0"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "&funcname=" + dtContent.Rows[0]["col9"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "-" + dtContent.Rows[0]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "");
                        }
                        else
                        {
                            //2017.1.27  加入+  "&funcid=" + dtContent.Rows[0]["col0"].ToString().Trim().Replace("/r", "").Replace("/n", "")
                            but.Listeners.Click.Handler = "MenuItemClick('" + dtContent.Rows[i]["col2"].ToString() + "?type=" + dtContent.Rows[i]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "&funcname=" + dtContent.Rows[i]["col9"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "-" + dtContent.Rows[i]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "&funcid=" + dtContent.Rows[i]["col0"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "');";
                            CurrentSelectMenu.Value = dtContent.Rows[0]["col2"].ToString() + "?type=" + dtContent.Rows[0]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "&funcname=" + dtContent.Rows[0]["col9"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "-" + dtContent.Rows[0]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "&funcid=" + dtContent.Rows[0]["col0"].ToString().Trim().Replace("/r", "").Replace("/n", "");
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
            }
        }

        /// <summary>
        /// 下载功能实现方法
        /// </summary>
        /// <param name="fileName"></param>
        private void DownLoadFile(string fileName)
        {
            try
            {
                string filePath = Server.MapPath(StaticInfo.WebSite) + fileName;
                if (File.Exists(filePath))
                {
                    FileInfo file = new FileInfo(filePath);
                    Response.Clear();
                    Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlDecode(file.Name));
                    Response.AddHeader("Content-length", file.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.WriteFile(file.FullName);

                    Response.End();
                }
                else
                {
                    Notice("下载", "下载文件不存在！");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            }
        }

        public void AddCss()
        {
            try
            {
                foreach (Control Aitem in PanelLeft.Controls)
                {
                    if (Aitem is Ext.Net.Panel)
                    {
                        ((Ext.Net.Panel)Aitem).Border = false;
                        ((Ext.Net.Panel)Aitem).Padding = 0;
                        ((Ext.Net.Panel)Aitem).Cls = "ex-panel-backgroundImage";
                        ((Ext.Net.Panel)Aitem).BaseCls = "ex-panel";
                        foreach (Control Bitem in ((Ext.Net.Panel)Aitem).Items)
                        {
                            if (Bitem is Ext.Net.Panel)
                            {
                                ((Ext.Net.Panel)Bitem).Padding = 0;
                                ((Ext.Net.Panel)Bitem).BaseCls = "BaseCls";
                                foreach (Control Citem in ((Ext.Net.Panel)Bitem).Items)
                                {
                                    if (Citem is Ext.Net.Button)
                                    {
                                        ((Ext.Net.Button)Citem).Width = 66;
                                        ((Ext.Net.Button)Citem).Height = 60;
                                        ((Ext.Net.Button)Citem).Cls = "ex-panel-background";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        #region 杂

        /*
         /// <summary>
        /// 重构panel 中全部控件
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="systemid"></param>
        /// <param name="contentCode"></param>
        /// <returns></returns>
        private Ext.Net.Panel AddmenuPanel(string usercode, string systemid, DataTable dt)
        {
            try
            {
                Ext.Net.Panel panelMenu = new Ext.Net.Panel();
                panelMenu.Border = false;
                panelMenu.Padding = 0;
                panelMenu.Cls = "ex-panel-backgroundImage";
                panelMenu.BaseCls = "ex-panel";
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Ext.Net.Panel menuPanel = AddNavigate(systemid, dt.Rows[i]["col1"].ToString(), usercode, i);
                        if (menuPanel != null)
                        {
                            panelMenu.Items.Add(menuPanel);
                        }
                    }
                }

                OpenDefaultPage();

                return panelMenu;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 添加右侧导航栏
        /// </summary>
        /// <param name="systemcode"></param>
        /// <param name="contentcode"></param>
        /// <param name="usercode"></param>
        private Ext.Net.Panel AddNavigate(string systemcode, string contentcode, string usercode, int order)
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
                        Ext.Net.Button but = new Ext.Net.Button();
                        but.Text = dtContent.Rows[i]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "");
                        but.ID = "but" + dtContent.Rows[i]["col0"].ToString();
                        but.Width = 66;
                        but.Height = 60;
                        but.Cls = "ex-panel-background";

                        if (i == 0)
                        {
                            but.AddClass("active");
                        }
                        if (dtContent.Rows[i]["col0"].ToString() == "01010")
                        {
                            but.RemoveClass("active");
                        }

                        if (dtContent.Rows[i]["col3"].ToString().Equals("2"))
                        {
                            but.Listeners.Click.Handler = "MenuItemClick('" + dtContent.Rows[i]["col2"].ToString() + "?funcid=" + dtContent.Rows[i]["col0"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "');";
                        }
                        else
                        {
                            but.Listeners.Click.Handler = "MenuItemClick('" + dtContent.Rows[i]["col2"].ToString() + "?type=" + dtContent.Rows[i]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "") + "');";
                        }
                        PanelMenuItem.Items.Add(but);
                    }

                    if (order == 0)
                    {
                        PanelMenuItem.Show();
                        CurrentSelectMenu.Value = dtContent.Rows[0]["col2"].ToString() + "?type=" + dtContent.Rows[0]["col1"].ToString().Trim().Replace("/r", "").Replace("/n", "");
                    }
                    else
                    {
                        PanelMenuItem.Hide();
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
                return null;
            }
        }
         */

        #endregion 杂

        /// <summary>
        /// 获取当前用户的背景图片
        /// </summary>
        private void getBackImage()
        {
            try
            {
                if (Session["userinfo"] != null)
                {
                    backImagesLoad(((UserInfo)Session["userinfo"]).UserCode);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 加载用户的背景图片
        /// </summary>
        private void backImagesLoad(string userCode)
        {
            try
            {
                string dirPath = Server.MapPath("~/Images/background/" + userCode);
                string imageUrl = "../Images/background/" + userCode;
                FileInfo file;
                if (Directory.Exists(Path.GetDirectoryName(dirPath)))
                {
                    int i = default(int);
                    DirectoryInfo dirInfo = new DirectoryInfo(dirPath);

                    foreach (FileSystemInfo fsi in dirInfo.GetFileSystemInfos())
                    {
                        if (fsi is FileInfo)
                        {
                            file = (FileInfo)fsi;
                            StaticInfo.FileName = file.Name;

                            string allowFile = ".JPG.JPEG.PNG.BMP";
                            if (allowFile.Contains(file.Extension.ToUpper()))
                            {
                                AddBackImage(i, imageUrl + "/" + file.Name);
                            }
                        }
                        i++;
                    }
                    PanelChangeBack.Render();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 获取下载文件，以表格的方式展现出来
        /// </summary>
        /// <param name="strDir"></param>
        private void GetFileList(string strDir)
        {
            try
            {
                Table tableDirInfo = new Table();
                tableDirInfo.CssClass = "tableDownLoad";

                if (Directory.Exists(Path.GetDirectoryName(strDir)))
                {
                    FileInfo file = null;
                    TableCell td;
                    TableHeaderCell th;
                    TableRow tr;

                    //动态添加单元格内容

                    th = new TableHeaderCell();
                    tr = new TableRow();
                    th.Controls.Add(new LiteralControl("下载列表"));
                    tr.Cells.Add(th);

                    tableDirInfo.Rows.Add(tr);

                    DirectoryInfo dirInfo = new DirectoryInfo(strDir);

                    foreach (FileSystemInfo fsi in dirInfo.GetFileSystemInfos())
                    {
                        //fileName = "";
                        if (fsi is FileInfo)
                        {
                            file = (FileInfo)fsi;
                            StaticInfo.FileName = file.Name;
                        }

                        //将文件信息添加到单元格
                        tr = new TableRow();

                        int startIndex = Server.MapPath(StaticInfo.WebSite + "/").LastIndexOf("\\");
                        string partPath = file.FullName.Substring(startIndex);
                        td = new TableCell();
                        td.Attributes["style"] = "font-size: 16px;";
                        td.Controls.Add(new LiteralControl("<a href=?dFile=" + Server.UrlDecode(partPath) + ">" + StaticInfo.FileName + "</a>")); //文件下载链接
                        tr.Controls.Add(td);

                        tableDirInfo.Rows.Add(tr);
                        tableDownload.Controls.Add(tableDirInfo);

                        PanelDowload.Render();
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        private void GetUserInfo()
        {
            try
            {
                if (Session["userinfo"] != null)
                {
                    #region 动态添加用户信息

                    Table userInfo = new Table();
                    userInfo.CssClass = "tableUserInfo";
                    TableRow row;
                    TableCell td;
                    row = new TableRow();
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl("用&nbsp;&nbsp;户&nbsp;名："));
                    row.Controls.Add(td);
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl(((UserInfo)Session["userinfo"]).UserName));
                    row.Controls.Add(td);
                    userInfo.Controls.Add(row);

                    row = new TableRow();
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl("姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;名："));
                    row.Controls.Add(td);
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl(((UserInfo)Session["userinfo"]).Name));
                    row.Controls.Add(td);
                    userInfo.Controls.Add(row);

                    row = new TableRow();
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl("性&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;别："));
                    row.Controls.Add(td);
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl(((UserInfo)Session["userinfo"]).SexMs));
                    row.Controls.Add(td);
                    userInfo.Controls.Add(row);

                    row = new TableRow();
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl("警&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;号："));
                    row.Controls.Add(td);
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl(((UserInfo)Session["userinfo"]).UserPolice));
                    row.Controls.Add(td);
                    userInfo.Controls.Add(row);

                    row = new TableRow();
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl("所属角色："));
                    row.Controls.Add(td);
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl(((UserInfo)Session["userinfo"]).Role));
                    row.Controls.Add(td);
                    userInfo.Controls.Add(row);

                    row = new TableRow();
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl("所属机构："));
                    row.Controls.Add(td);
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl(((UserInfo)Session["userinfo"]).DepartName));
                    row.Controls.Add(td);
                    userInfo.Controls.Add(row);

                    row = new TableRow();
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl("注册时间："));
                    row.Controls.Add(td);
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl(((UserInfo)Session["userinfo"]).Time.ToString()));
                    row.Controls.Add(td);
                    userInfo.Controls.Add(row);

                    PanelUserInfo.ContentControls.Add(userInfo);
                    PanelUserInfo.Render();

                    #endregion 动态添加用户信息
                }
                else
                {
                    #region Session["userinfo"]为空时动态添加用户信息

                    Table userInfo = new Table();
                    userInfo.CssClass = "tableUserInfo2";
                    TableRow row;
                    TableCell td;
                    row = new TableRow();
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl("用&nbsp;&nbsp;户&nbsp;名："));
                    row.Controls.Add(td);
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl(""));
                    row.Controls.Add(td);
                    userInfo.Controls.Add(row);

                    row = new TableRow();
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl("姓&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;名："));
                    row.Controls.Add(td);
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl(""));
                    row.Controls.Add(td);
                    userInfo.Controls.Add(row);

                    row = new TableRow();
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl("性&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;别："));
                    row.Controls.Add(td);
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl(""));
                    row.Controls.Add(td);
                    userInfo.Controls.Add(row);

                    row = new TableRow();
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl("警&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;号："));
                    row.Controls.Add(td);
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl(""));
                    row.Controls.Add(td);
                    userInfo.Controls.Add(row);

                    row = new TableRow();
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl("所属角色："));
                    row.Controls.Add(td);
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl(""));
                    row.Controls.Add(td);
                    userInfo.Controls.Add(row);

                    row = new TableRow();
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl("所属机构："));
                    row.Controls.Add(td);
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl(""));
                    row.Controls.Add(td);
                    userInfo.Controls.Add(row);

                    row = new TableRow();
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl("注册时间："));
                    row.Controls.Add(td);
                    td = new TableCell();
                    td.Controls.Add(new LiteralControl(""));
                    row.Controls.Add(td);
                    userInfo.Controls.Add(row);

                    PanelUserInfo.ContentControls.Add(userInfo);
                    PanelUserInfo.Render();

                    #endregion Session["userinfo"]为空时动态添加用户信息
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 关于我们页面信息添加（公司简介）
        /// </summary>
        private void GetAboutMeInfo()
        {
            try
            {
                HtmlGenericControl logo = new HtmlGenericControl("img");
                //此处为公司logo
                logo.Attributes["src"] = "Images/logo/logo.png";
                logo.Attributes["style"] = "width:230px;height:50;align:middle";
                copanyIntroduce.Controls.Add(logo);
                //此处为标题
                //logo.InnerText = "北京尚易德科技有限公司";

                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Attributes["style"] = "height:170px;text-indent:2em";

                //此处为要展示的东西（如公司简介等）
                div.InnerText = @"<B>北京尚易德科技有限公司</B>成立于2009年，中国华录集团旗下企业，是北京市高新技术企业，总部位于北京市石景山区中国华录大厦。
“SUNRISING-TECH“作为尚易德的英文释义，意寓着它正如冉冉升起的太阳，充满朝气与活力，拥有美好与广阔的未来。
依托中国华录集团强大的制造能力和雄厚的资金实力，以高清视频技术链条为主线，从图像采集，传输，存储，分析到显示，推出了完整的产品体系和解决方案，建立了安防监控、智能交通、大屏显示的三大产品板块，产品技术跃居行业领先地位，获得了客户及行业专家的一致认可，并荣获多项行业荣誉与专业奖项。
为了提供优质，快捷的服务，与广大系统集成商和产品渠道商建立长期良好的合作关系，尚易德以北京为中心，在黑龙江、内蒙古、辽宁、天津、山东、山西、河北、河南、安徽、浙江、江西、广东、广西、四川、新疆等地开设了区域办事处，其他省份的办事处正在筹备过程中，将陆续开设。北京运营中心还设有军队、人防、电力、煤炭、石化、金融、水运、航空、海外等行业事业部，初步建立了覆盖全国的产品服务网络。
尚易德一直奉行坚守“尚品、易为、厚德”的核心理念，用快速发展来回报所有支持、关心我们的投资人、客户和各界朋友，用快速发展来吸引更多的精英加入我们，用快速发展来积聚更多回报社会的能力，成就为一个充满社会责任感的企业。
                <br/> <B>联系方式</B>：<br/> 地址：北京市石景山区阜石路165号华录大厦8层 邮编：100043 <br/> 电话：400-1616-123 传真：010-52281009 网址：www.sunrisingtech.com ";

                //页面底部
                //HtmlGenericControl foot = new HtmlGenericControl("div");
                //foot.InnerText = "©版权所有";
                //foot.Attributes["style"] = "position:absolute;bottom:10;left:40%";

                Window3.ContentControls.Add(div);
                //Window3.ContentControls.Add(foot);
                Window3.Render();
            }
            catch (Exception ex)
            { ILog.WriteErrorLog(ex); }
        }

        /// <summary>
        /// 上传选中图片
        /// </summary>
        /// <returns></returns>
        private string UploadFile()
        {
            try
            {
                string UploadFile = "";
                string strPath = "";
                if (this.ImgFile.HasFile)
                {
                    UploadFile = this.ImgFile.PostedFile.FileName.ToString();
                    int FileSize = Int32.Parse(this.ImgFile.PostedFile.ContentLength.ToString());

                    string fileType = Path.GetExtension(this.ImgFile.PostedFile.FileName).ToUpper();//获取文件后缀
                    string allowFile = ".JPG.JPEG.PNG.BMP";
                    if (allowFile.Contains(fileType.ToUpper()))
                    {
                        if (Session["userinfo"] != null)
                        {
                            string code = ((UserInfo)Session["userinfo"]).UserCode;
                            string sNewName = code + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(this.ImgFile.PostedFile.FileName);
                            strPath = Server.MapPath("~/Images/background/" + code + "/" + sNewName);
                            if (!Directory.Exists(Path.GetDirectoryName(strPath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(strPath));
                            }
                            this.ImgFile.PostedFile.SaveAs(strPath);
                            string url = Request.Url.ToString();

                            return GetImgUrl(url) + "Images/background/" + code + "/" + sNewName;
                        }
                        else
                            return "";
                    }
                    else
                    {
                        X.Msg.Alert("提示信息", "文件格式不正确！").Show();
                        return "";
                    }
                }
                return strPath;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "";
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
                return "";
            }
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
    }
}