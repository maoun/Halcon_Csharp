using System;
using System.Data;
using System.IO;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class LoginOld : System.Web.UI.Page
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
                string areaName = settingManager.GetSettingRegionName("00");
                lblSystemName.Html = "<strong><span style=\"font-family: 微软雅黑; font-size: 18pt; color: #000066;\">城市安全综合管控平台</span></strong>";
                DealTempFile();
            }
        }

        #region 控件事件

        /// <summary>
        /// 登录按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLogin_Click(object sender, DirectEventArgs e)
        {
            UserLogin();
        }

        #endregion 控件事件

        #region 私有方法

        /// <summary>
        /// 登录
        /// </summary>
        [DirectMethod]
        public void UserLogin()
        {
            string username = this.txtUsername.Text;
            string password = this.txtPassword.Text;
            string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            DataTable dt = userManager.GetSerUserInfo(SystemID, "b.username='" + username + "'", ip);
            if (dt != null && dt.Rows.Count > 0)
            {
                this.Window1.Hide();
                UserInfo userinfo = new UserInfo();
                userinfo.UserCode = dt.Rows[0]["col1"].ToString();
                userinfo.Name = dt.Rows[0]["col2"].ToString();
                userinfo.Sex = dt.Rows[0]["col44"].ToString();
                userinfo.UserPolice = dt.Rows[0]["col21"].ToString();
                userinfo.UserName = dt.Rows[0]["col27"].ToString();
                userinfo.Time = DateTime.Parse(dt.Rows[0]["col28"].ToString());
                userinfo.DepartName = dt.Rows[0]["col29"].ToString();
                userinfo.Role = dt.Rows[0]["col45"].ToString();
                string pwd = Cryptography.Decrypt(dt.Rows[0]["col31"].ToString()); //将数据库密码解密后与输入密码进行比对
                userinfo.VideoTemplate1 = dt.Rows[0]["col33"].ToString();
                userinfo.VideoTemplate2 = dt.Rows[0]["col34"].ToString();
                userinfo.DataTemplate1 = dt.Rows[0]["col35"].ToString();
                userinfo.DataTemplate2 = dt.Rows[0]["col36"].ToString();
                userinfo.DataTemplate3 = dt.Rows[0]["col37"].ToString();
                userinfo.DataTemplate4 = dt.Rows[0]["col38"].ToString();
                userinfo.ListTemplate1 = dt.Rows[0]["col39"].ToString();
                userinfo.ListTemplate2 = dt.Rows[0]["col40"].ToString();
                userinfo.ListTemplate3 = dt.Rows[0]["col41"].ToString();
                userinfo.UserTemplate = dt.Rows[0]["col42"].ToString();
                userinfo.UserBackGround = dt.Rows[0]["col43"].ToString();

                if (Session["userinfo"] != null)
                {
                    Session["userinfo"] = null;
                }
                Session["userinfo"] = userinfo;
                Session["LoginPage"] = "Login.aspx";
                string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                if (ipaddress.Length < 9)
                {
                    ipaddress = "127.0.0.1";
                }

                if (pwd.Equals(password))
                {
                     logManager.InsertLogRunning(userinfo.UserName, "访问：用户登录", ipaddress, "0");
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
            this.txtPassword.Text = "";
            Window1.Show();
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
                    Handler = "Login.ShowWin()",
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
            catch
            {
                return false;
            }
        }

        private void DealTempFile()
        {
            try
            {
                string tempPath = Server.MapPath("~/FileUpload/TEMP");
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(tempPath);
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
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        #endregion 私有方法
    }
}