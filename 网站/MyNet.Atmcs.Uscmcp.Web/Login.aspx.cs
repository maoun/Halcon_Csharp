using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;
using System.IO;
using System.Web;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class Login : System.Web.UI.Page
    {
        #region 成员变量

        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private Bll.UserManager userManager = new Bll.UserManager();
        private SettingManager settingManager = new SettingManager();
        private string SystemID = "00";

        #endregion 成员变量

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                try
                {
                    DealTempFile();
                    //HttpCookie Cookie = CookiesHelper.GetCookie("UserInfo");
                    HttpCookie Cookie = Request.Cookies["UserInfo"];

                    if (Cookie != null)
                    {
                        this.txtUsername.Value = Cookie.Values["uName"];
                        //  this.txtPwd.Attributes.Add("value", "1234567");
                        if (!string.IsNullOrEmpty(Cookie.Values["uPwd"]))
                        {
                            string pwd = Cryptography.Decrypt(Cookie.Values["uPwd"]);
                            string js = "Check('" + pwd + "')";
                            this.ResourceManager1.RegisterAfterClientInitScript(js);
                        }
                    }
                    else
                    {
                        string js = "NoCheck()";
                        this.ResourceManager1.RegisterAfterClientInitScript(js);
                    }
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex);
                }
                this.DataBind();
            }
        }

        #region 私有方法

        /// <summary>
        /// 设置屏幕分辨率
        /// </summary>
        [DirectMethod]
        public void SetScreen(string bj)
        {
            if (Session["Screen"] != null)
            {
                Session["Screen"] = null;
            }
            if (bj.Equals("1"))//大屏
            {
                Session["Screen"] = "1";
            }
            else if (bj.Equals("2"))//中屏
            {
                Session["Screen"] = "2";
            }
            else if (bj.Equals("3"))//小屏
            {
                Session["Screen"] = "3";
            }
            else
            {
                Session["Screen"] = "3";
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        [DirectMethod]
        public void UserLogin(string username, string password, string keep)
        {
            UserInfo userinfo = new UserInfo();
            userinfo.LoginType = true;
            string ip = System.Web.HttpContext.Current.Request.UserHostAddress.Trim();
            DataTable dt = userManager.GetSerUserInfo(SystemID, "b.username='" + username + "'", ip);
            if (dt != null && dt.Rows.Count > 0)
            {
                userinfo.UserCode = dt.Rows[0]["col1"].ToString();
                userinfo.Name = dt.Rows[0]["col2"].ToString();
                userinfo.Sex = dt.Rows[0]["col44"].ToString();
                userinfo.SexMs = dt.Rows[0]["col42"].ToString();
                userinfo.UserPolice = dt.Rows[0]["col21"].ToString();
                userinfo.UserName = dt.Rows[0]["col27"].ToString();
                userinfo.Time = DateTime.Parse(dt.Rows[0]["col28"].ToString());
                userinfo.DepartName = dt.Rows[0]["col29"].ToString();
                userinfo.DeptCode = dt.Rows[0]["col20"].ToString();
                userinfo.Role = dt.Rows[0]["col43"].ToString();
                string pwd = Cryptography.Decrypt(dt.Rows[0]["col31"].ToString()); //将数据库密码解密后与输入密码进行比对
                userinfo.DataTemplate1 = dt.Rows[0]["col33"].ToString();
                userinfo.DataTemplate2 = dt.Rows[0]["col34"].ToString();
                userinfo.DataTemplate3 = dt.Rows[0]["col35"].ToString();
                userinfo.DataTemplate4 = dt.Rows[0]["col36"].ToString();
                userinfo.ListTemplate1 = dt.Rows[0]["col37"].ToString();
                userinfo.ListTemplate2 = dt.Rows[0]["col38"].ToString();
                userinfo.ListTemplate3 = dt.Rows[0]["col39"].ToString();
                userinfo.UserTemplate = dt.Rows[0]["col40"].ToString();
                userinfo.UserBackGround = dt.Rows[0]["col41"].ToString();
                userinfo.FirstTemplate = dt.Rows[0]["col44"].ToString();

                DataTable dtScreen = new DataCommon().GetDataTable("SELECT ip, screennum, screen1, screen2, screen3, screen4, screen5, screen6, screen7, screen8, screen9 FROM t_cfg_ipscreen WHERE ip='" + ip + "'");

                if (dtScreen != null && dtScreen.Rows.Count > 0)
                {
                    userinfo.ScreenNum = dtScreen.Rows[0][1].ToString();
                    userinfo.Screen1 = dtScreen.Rows[0][2].ToString();
                    userinfo.Screen2 = dtScreen.Rows[0][3].ToString();
                    userinfo.Screen3 = dtScreen.Rows[0][4].ToString();
                    userinfo.Screen4 = dtScreen.Rows[0][5].ToString();
                    userinfo.Screen5 = dtScreen.Rows[0][6].ToString();
                    userinfo.Screen6 = dtScreen.Rows[0][7].ToString();
                    userinfo.Screen7 = dtScreen.Rows[0][8].ToString();
                    userinfo.Screen8 = dtScreen.Rows[0][9].ToString();
                    userinfo.Screen9 = dtScreen.Rows[0][10].ToString();
                }
                else
                {
                    userinfo.ScreenNum = "1";
                }

                if (Session["userinfo"] != null)
                {
                    Session["userinfo"] = null;
                }

                string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                if (ipaddress.Length < 9)
                {
                    ipaddress = "127.0.0.1";
                }
                userinfo.NowIp = ipaddress;

                Session["userinfo"] = userinfo;
                Session["LoginPage"] = "Login.aspx";
                if (pwd.Equals(password))
                {
                    HttpCookie Cookie = Request.Cookies["UserInfo"];
                    //HttpCookie cookie = new HttpCookie("UserInfo");
                    //cookie.Values["name"] = "mike";
                    //cookie.Values["last"] = "a";

                    //string name = Request.Cookies["userInfo1"]["name"];
                    //string last = Request.Cookies["userInfo1"]["last"];

                    if (keep.Equals("1"))
                    {
                        if (Cookie == null)
                        {
                            Cookie = new HttpCookie("UserInfo");
                            Cookie.Values.Add("uName", username);
                            Cookie.Values.Add("uPwd", Cryptography.Encrypt(password));
                            //设置Cookie过期时间
                            Cookie.Expires = DateTime.Now.AddDays(14);
                            Response.Cookies.Add(Cookie);
                            //CookiesHelper.AddCookie(Cookie);
                        }
                        else if (!Cookie.Values["uName"].Equals(username))
                        {
                            Response.Cookies["UserInfo"]["uName"] = username;
                            Response.Cookies["UserInfo"]["uPwd"] = Cryptography.Encrypt(password);
                            Response.Cookies["UserInfo"].Expires = DateTime.Now.AddDays(14);
                        }
                    }
                    else
                    {
                        if (Cookie != null)
                        {
                            //如果用户没有选择记住密码，那么立即将Cookie里面的信息情况，并且设置状态保持立即过期。
                            Response.Cookies["UserInfo"].Expires = DateTime.Now;
                        }
                    }

                    logManager.InsertLogRunning(userinfo.UserName, "用户登录", ipaddress, "0");
                    Response.Redirect("./MainIndex.aspx");
                }
                else
                {
                    Session["userinfo"] = null;
                    Message("信息提示", "[" + username + "]用户密码不正确！");
                }
            }
            else
            {
                Session["userinfo"] = null;
                Message("信息提示", "[" + username + "]用户不存在或检查配置是否正确！");
            }
        }

        /// <summary>
        /// 显示主窗体
        /// </summary>
        [DirectMethod]
        public void ShowWin()
        {
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Message(string title, string msg)
        {
            X.Msg.Confirm(title, msg, new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    //Handler = "alert(1);",
                    Text = "确定"
                }
            }).Show();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Base64String"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Base64Img(string Base64String, string path)
        {
            try
            {
                MemoryStream mm = new MemoryStream(Convert.FromBase64String(Base64String));
                System.Drawing.Image.FromStream(mm).Save(path, System.Drawing.Imaging.ImageFormat.Png);
                return true;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("Login.aspx-Base64Img", ex.Message + "；" + ex.StackTrace, "Base64Img has an exception");
                return false;
            }
        }

        /// <summary>
        /// 删除历史临时图片
        /// </summary>
        private void DealTempFile()
        {
            try
            {
                string tempPath = Server.MapPath("~/FileUpload/TEMP");
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(tempPath);
                if (di.Exists)
                {
                    DirectoryInfo[] dirs = di.GetDirectories();
                    if (dirs.Length > 0)
                    {
                        foreach (DirectoryInfo item in dirs)
                        {
                            DateTime namedt = DateTime.Parse(item.Name.Insert(4, "-").Insert(7, "-"));
                            TimeSpan ts = DateTime.Now - namedt;
                            if (ts.Days > 1)
                            {
                                item.Delete(true);
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