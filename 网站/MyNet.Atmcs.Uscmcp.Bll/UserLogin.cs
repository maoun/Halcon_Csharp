using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class UserLogin
    {
        private static readonly IUserManager dal = DALFactory.CreateUserManager();
        private UserManager userManager = new UserManager();

        /// <summary>
        /// 根据用户名和密码获得usercode
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public string GetUserCode(string userName, string pwd)
        {
            try
            {
                return dal.GetUserInfo(userName, pwd);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "";
            }
        }

        /// <summary>
        /// 根据用户名密码获得用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable GetUserCode(string userName)
        {
            try
            {
                return dal.GetUserInfo(userName);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 检测是否登录或者已经操作超时和是否有当前页面的访问权限
        /// </summary>
        public void IsLoginAndFunc(System.Web.HttpRequest request)
        {
            UserInfo userInfo = System.Web.HttpContext.Current.Session["userinfo"] as UserInfo;
            if (userInfo == null)
                throw new Exception("您没有登录或操作超时，请重新登录。");
            else
            {
                if (request == null)
                    throw new Exception("请求为空，请重新操作。");
                else
                {
                    string result = dal.IsExsitFuncByUser(userInfo.UserCode, request.AppRelativeCurrentExecutionFilePath.Replace("~", ".."));
                    if (string.IsNullOrEmpty(result))
                        throw new Exception("您没有当前页面的访问权限，请联系管理员。");
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string GetUserName()
        {
            try
            {
                UserInfo userInfo = System.Web.HttpContext.Current.Session["userinfo"] as UserInfo;
                if (userInfo == null)
                    throw new Exception("您没有登录或操作超时，请重新登录。");
                else
                {
                    return userInfo.UserName;
                }
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 判断是否登录
        /// </summary>
        /// <param name="request"></param>
        public void IsLogin()
        {
            try
            {
                UserInfo userInfo = System.Web.HttpContext.Current.Session["userinfo"] as UserInfo;
                if (userInfo == null)
                {
                    System.Web.HttpContext.Current.Response.Redirect("~/Login.aspx", true);
                }
                //throw new Exception("您没有登录或操作超时，请重新登录。");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        public void IsLoginPage(System.Web.UI.Page page)
        {
            UserInfo userInfo = System.Web.HttpContext.Current.Session["userinfo"] as UserInfo;

            if (userInfo == null)
            {
                page.Response.Redirect("../Login.aspx");
            }
        }

        /// <summary>
        /// 判断是否登录
        /// </summary>
        /// <param name="request"></param>
        public void IsLogin(string username)
        {
            try
            {
                UserInfo userInfo = System.Web.HttpContext.Current.Session["userinfo"] as UserInfo;
                if (userInfo == null)
                {
                    if (string.IsNullOrEmpty(username))
                    {
                        System.Web.HttpContext.Current.Response.Redirect("~/Login.aspx", true);
                    }
                    else
                    {
                        string ip = System.Web.HttpContext.Current.Request.UserHostAddress.Trim();
                        DataTable dt = userManager.GetSerUserInfo("00", "b.username='" + username + "'", ip);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            UserInfo userinfo = new UserInfo();
                            userinfo.UserCode = dt.Rows[0]["col1"].ToString();
                            userinfo.Name = dt.Rows[0]["col2"].ToString();
                            userinfo.Sex = dt.Rows[0]["col44"].ToString();
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
                            if (System.Web.HttpContext.Current.Session["userinfo"] != null)
                            {
                                System.Web.HttpContext.Current.Session["userinfo"] = null;
                            }
                            System.Web.HttpContext.Current.Session["userinfo"] = userinfo;

                            //throw new Exception("您没有登录或操作超时，请重新登录。");
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
        /// 判断是否登录
        /// </summary>
        /// <param name="request"></param>
        public bool CheckLogin(string username)
        {
            try
            {
                UserInfo userInfo = System.Web.HttpContext.Current.Session["userinfo"] as UserInfo;
                if (userInfo == null)
                {
                    if (string.IsNullOrEmpty(username))
                    {
                        return false;
                    }
                    else
                    {
                        string ip = System.Web.HttpContext.Current.Request.UserHostAddress.Trim();
                        UserManagerNew userNew = new UserManagerNew();
                        DataTable dt = userNew.GetSerUserInfo("00", "b.username='" + username + "'", ip);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            UserInfo userinfo = new UserInfo();
                            userinfo.LoginType = false;
                            userinfo.UserCode = dt.Rows[0]["col1"].ToString();
                            userinfo.Name = dt.Rows[0]["col2"].ToString();
                            userinfo.Sex = dt.Rows[0]["col44"].ToString();
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
                            userinfo.NowIp = ip;
                            if (System.Web.HttpContext.Current.Session["userinfo"] != null)
                            {
                                System.Web.HttpContext.Current.Session["userinfo"] = null;
                            }
                            System.Web.HttpContext.Current.Session["userinfo"] = userinfo;
                            return true;
                        }
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }
    }
}