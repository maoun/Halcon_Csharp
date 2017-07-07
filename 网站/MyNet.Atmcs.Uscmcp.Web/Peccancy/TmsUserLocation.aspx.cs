using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class TmsUserLocation : System.Web.UI.Page
    {
        #region 成员变量
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private UserManager userManager = new Bll.UserManager();
        private DataCommon dataCommon = new DataCommon();
        private UserLogin userLogin = new UserLogin();
        private string usercode = "";

        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("TmsUserLocation1", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'"; 
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); 
                return; 
            }
            if (!X.IsAjaxRequest)
            {
                //userLogin.IsLoginPage(this);
                this.RadioOne.Checked = true;
                GetUserInfo("1");
                GetData(usercode, "1");
                this.DataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("TmsUserLocation2", "访问：审核授权管理"), userinfo.NowIp, "0");
            }
        }

        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void StoreContents_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                string checkTimes = "";
                if (this.RadioOne.Checked == true)
                {
                    checkTimes = "1";
                }
                if (this.RadioTwo.Checked == true)
                {
                    checkTimes = "2";
                }
                GetUserInfo(checkTimes);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TmsUserLocation.aspx-StoreContents_Refresh", ex.Message+"；"+ex.StackTrace, "StoreContents_Refresh has an exception");
            }
        }

        /// <summary>
        /// 首页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutFisrt(object sender, DirectEventArgs e)
        {
            try
            {
                curpage.Value = 1;
                ShowQuery(1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TmsUserLocation.aspx-TbutFisrt", ex.Message+"；"+ex.StackTrace, "TbutFisrt has an exception");
            }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutLast(object sender, DirectEventArgs e)
        {
            try
            {
                int page = int.Parse(curpage.Value.ToString());
                page--;
                if (page < 1)
                {
                    page = 1;
                }
                curpage.Value = page;
                ShowQuery(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TmsUserLocation.aspx-TbutLast", ex.Message+"；"+ex.StackTrace, "TbutLast has an exception");
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutNext(object sender, DirectEventArgs e)
        {
            try
            {
                int page = int.Parse(curpage.Value.ToString());
                int allpage = int.Parse(allPage.Value.ToString());
                page++;
                if (page > allpage)
                {
                    page = allpage;
                }
                curpage.Value = page;
                ShowQuery(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TmsUserLocation.aspx-TbutNext", ex.Message+"；"+ex.StackTrace, "TbutNext has an exception");
            }
        }

        /// <summary>
        /// 尾页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutEnd(object sender, DirectEventArgs e)
        {
            try
            {
                curpage.Value = allPage.Value;
                int page = int.Parse(curpage.Value.ToString());
                ShowQuery(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TmsUserLocation.aspx-TbutEnd", ex.Message+"；"+ex.StackTrace, "TbutEnd has an exception");
            }
        }

        /// <summary>
        /// 显示指定页面的数据
        /// </summary>
        /// <param name="currentPage"></param>
        private void ShowQuery(int currentPage)
        {
            try
            {
                int rownum = 15;
                int startNum = (currentPage - 1) * rownum;
                int endNum = currentPage * rownum;
                Query("1=1", startNum, endNum);
                SetButState(currentPage);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TmsUserLocation.aspx-ShowQuery", ex.Message+"；"+ex.StackTrace, "ShowQuery has an exception");
            }
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startNum"></param>
        /// <param name="endNum"></param>
        private void Query(string where, int startNum, int endNum)
        {
            try
            {
                string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                DataTable objData2 = userManager.GetSerUserInfoNew("00", "1=1", ip, startNum, endNum);
                if (objData2 != null && objData2.Rows.Count > 0)
                {
                    this.StoreContents.DataSource = objData2;
                    this.StoreContents.DataBind();
                    usercode = objData2.Rows[0]["col1"].ToString();
                }

                if (objData2 != null && objData2.Rows.Count > 0)
                {
                    this.lblCurpage.Text = curpage.Value.ToString();
                    this.lblAllpage.Text = allPage.Value.ToString();
                    this.lblRealcount.Text = realCount.Value.ToString();
                }
                else
                {
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";
                    Notice(GetLangStr("TmsUserLocation16", "信息提示"), GetLangStr("TmsUserLocation17", "未查询到相关记录"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TmsUserLocation.aspx-Query", ex.Message+"；"+ex.StackTrace, "Query has an exception");
            }
        }

        /// <summary>
        /// 选中数据行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowSelect(object sender, DirectEventArgs e)
        {
            try
            {
                string data = e.ExtraParams["data"];

                string userid = Bll.Common.GetdatabyField(data, "col1");
                string checkName = Bll.Common.GetdatabyField(data, "col26");
                string checkTimes = "1";
                if (checkName == GetLangStr("TmsUserLocation18", "初审用户"))
                {
                    this.RadioOne.Checked = true;
                    this.RadioTwo.Checked = false;
                    checkTimes = "1";
                }
                else if (checkName == GetLangStr("TmsUserLocation19", "复审用户"))
                {
                    this.RadioOne.Checked = false;
                    this.RadioTwo.Checked = true;
                    checkTimes = "2";
                }
                GetData(userid, checkTimes);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TmsUserLocation.aspx-RowSelect", ex.Message+"；"+ex.StackTrace, "RowSelect has an exception");
            }
        }

        /// <summary>
        ///  保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitSelection(object sender, DirectEventArgs e)
        {
            try
            {
                string multi1 = e.ExtraParams["multi1"];
                Ext.Net.ListItem[] items1 = JSON.Deserialize<Ext.Net.ListItem[]>(multi1);
                string userid = Session["user_id"] as string;
                string checkTimes = "";
                string checkName = "";
                if (this.RadioOne.Checked == true)
                {
                    checkTimes = "1";
                    checkName = GetLangStr("TmsUserLocation12", "初审用户");
                }
                if (this.RadioTwo.Checked == true)
                {
                    checkTimes = "2";
                    checkName = GetLangStr("TmsUserLocation13", "复审用户");
                }

                Hashtable hs = new Hashtable();
                hs.Add("user_id", userid);
                hs.Add("check_times", checkTimes);
                int saveRes = 0;
                int orderid = 0;

                tgsPproperty.DeleteUserStationInfo(hs);
                foreach (Ext.Net.ListItem item in items1)
                {
                    hs["station_id"] = item.Value;
                    orderid = orderid + 1;
                    saveRes = saveRes + tgsPproperty.InsertUserStationInfo(hs);
                }
                if (saveRes == orderid)
                {
                    string sql = "update t_ser_person set remark='" + checkName + "' where usercode='" + userid + "'";
                    dataCommon.Update(sql);
                    Notice(GetLangStr("TmsUserLocation16", "信息提示"), GetLangStr("TmsUserLocation20", "保存成功"));
                    GetUserInfo(checkTimes);
                    GetData(userid, checkTimes);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TmsUserLocation.aspx-SubmitSelection", ex.Message+"；"+ex.StackTrace, "SubmitSelection has an exception");
            }
        }

        #region DirectMethod

        /// <summary>
        /// 初审按钮事件
        /// </summary>
        [DirectMethod]
        public void TbutRadioOneClick()
        {
            try
            {
                this.RadioOne.Checked = true;
                this.RadioTwo.Checked = false;
                string checkTimes = "1";
                string userid = Session["user_id"] as string;
                Store1.DataSource = tgsPproperty.GetUserStationInfo(userid, checkTimes);
                Store1.DataBind();
                Store2.DataSource = tgsPproperty.GetNoUserStationInfo(checkTimes);
                Store2.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TmsUserLocation.aspx-TbutRadioOneClick", ex.Message+"；"+ex.StackTrace, "TbutRadioOneClick has an exception");
            }
        }

        /// <summary>
        /// 复审按钮事件
        /// </summary>
        [DirectMethod]
        public void TbuRadioTwoClick()
        {
            try
            {
                this.RadioOne.Checked = false;
                this.RadioTwo.Checked = true;
                string checkTimes = "2";
                string userid = Session["user_id"] as string;
                Store1.DataSource = tgsPproperty.GetUserStationInfo(userid, checkTimes);
                Store1.DataBind();
                Store2.DataSource = tgsPproperty.GetNoUserStationInfo(checkTimes);
                Store2.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TmsUserLocation.aspx-TbuRadioTwoClick", ex.Message+"；"+ex.StackTrace, "TbuRadioTwoClick has an exception");
            }
        }

        #endregion DirectMethod

        #endregion 控件事件

        #region 私有方法

        /// <summary>
        /// 加载用户信息
        /// </summary>
        /// <param name="shjb"></param>
        private void GetUserInfo(string shjb)
        {
            try
            {
                string rownum = "15";
                string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                DataTable dtCount = userManager.GetSerUserInfoCount("00", "1=1", ip);//获得总记录
                if (dtCount != null && dtCount.Rows.Count > 0)
                {
                    realCount.Value = dtCount.Rows[0]["col0"].ToString();
                    curpage.Value = 1;
                    allPage.Value = (int)Math.Ceiling(double.Parse(realCount.Value.ToString()) / Convert.ToInt32(rownum));
                    ShowQuery(1);
                }
                else
                {
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";
                    Notice(GetLangStr("TmsUserLocation16", "信息提示"), GetLangStr("TmsUserLocation17", "未查询到相关记录"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TmsUserLocation.aspx-GetUserInfo", ex.Message+"；"+ex.StackTrace, "GetUserInfo has an exception");
            }
        }

        /// <summary>
        /// 加载传入用户的审核地点
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="shjb"></param>
        private void GetData(string userid, string shjb)
        {
            try
            {
                Session["user_id"] = userid;
                DataTable dt1 = tgsPproperty.GetUserStationInfo(userid, shjb);
                if (dt1 != null)
                {
                    Store1.DataSource = dt1;
                    Store1.DataBind();
                }
                DataTable dt2 = tgsPproperty.GetNoUserStationInfo(shjb);
                if (dt2 != null)
                {
                    Store2.DataSource = dt2;
                    Store2.DataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TmsUserLocation.aspx-GetData", ex.Message+"；"+ex.StackTrace, "GetData has an exception");
            }
        }

        /// <summary>
        /// 提示信息
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
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.BottomRight,
                    OffsetY = -60
                },
                Html = "<br></br>" + msg + "!"
            });
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

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="page"></param>
        private void SetButState(int page)
        {
            try
            {
                curpage.Value = page;
                int allpage = int.Parse(allPage.Value.ToString());

                if (allpage > 1)
                {
                    ButLast.Disabled = false;
                    ButNext.Disabled = false;
                    ButFisrt.Disabled = false;
                    ButEnd.Disabled = false;
                }
                if (page == 1)
                {
                    ButLast.Disabled = true;
                    ButFisrt.Disabled = true;
                }
                if (page == allpage)
                {
                    ButNext.Disabled = true;
                    ButEnd.Disabled = true;
                }
                if (allpage <= 1)
                {
                    ButFisrt.Disabled = true;
                    ButNext.Disabled = true;
                    ButLast.Disabled = true;
                    ButEnd.Disabled = true;
                    page = 0;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TmsUserLocation.aspx-SetButState", ex.Message+"；"+ex.StackTrace, "SetButState has an exception");
            }
        }

        #endregion 私有方法
    }
}