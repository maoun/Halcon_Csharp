using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web.Passcar
{
    public partial class FisrtIntoCity : System.Web.UI.Page
    {
        #region 变量

        private static string startdate = "", enddate = "";
        private MyNet.Atmcs.Uscmcp.Bll.PasscarManager bll = new MyNet.Atmcs.Uscmcp.Bll.PasscarManager();
        private UserLogin userLogin = new UserLogin();
        private static int pageCount = 0;
        private DataCommon dataCommon = new DataCommon();
        private const string NoImageUrl = "../images/NoImage.png";
        private static QueryService.querypasscar client = new QueryService.querypasscar();
        private static int pageIndex = 1;
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        #endregion 变量

        #region 登录页面初始化

        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"]; if (!userLogin.CheckLogin(username)) { string js = "alert('" + GetLangStr("FisrtIntoCity27", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }
            if (!X.IsAjaxRequest)
            {
                this.DataBind();
                if (Session["Condition"] != null)
                {
                    Condition con = Session["Condition"] as Condition;
                    start.InnerText = con.StartTime;
                    end.InnerText = con.EndTime;
                    startdate = con.StartTime;
                    enddate = con.EndTime;
                    cboplate.SetVehicleText(con.Sqjc);
                    if (con.QueryMode.Equals("0"))
                    {
                        pnhphm.Hidden = false;
                        txtplate.Hidden = true;
                        if (con.Hphm.Length < 6)
                        {
                            int length = con.Hphm.Length;
                            for (int i = 0; i < 6 - length; i++)
                            {
                                con.Hphm = con.Hphm + "_";
                            }
                        }

                        haopai_name1.Value = con.Hphm.Substring(0, 1);
                        if (haopai_name1.Value.Equals("_"))
                        {
                            haopai_name1.Value = "";
                        }
                        haopai_name2.Value = con.Hphm.Substring(1, 1);
                        if (haopai_name2.Value.Equals("_"))
                        {
                            haopai_name2.Value = "";
                        }
                        haopai_name3.Value = con.Hphm.Substring(2, 1);
                        if (haopai_name3.Value.Equals("_"))
                        {
                            haopai_name3.Value = "";
                        }
                        haopai_name4.Value = con.Hphm.Substring(3, 1);
                        if (haopai_name4.Value.Equals("_"))
                        {
                            haopai_name4.Value = "";
                        }
                        haopai_name5.Value = con.Hphm.Substring(4, 1);
                        if (haopai_name5.Value.Equals("_"))
                        {
                            haopai_name5.Value = "";
                        }
                        haopai_name6.Value = con.Hphm.Substring(5, 1);
                        if (haopai_name6.Value.Equals("_"))
                        {
                            haopai_name6.Value = "";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(con.Hphm))
                        {
                            txtplate.Text = con.Hphm;
                        }
                    }
                    //模糊查询
                    if (con.QueryMode == "1")
                    {
                        cktype.Checked = false;
                    }
                    else
                    {
                        cktype.Checked = true;
                    }
                    CmbPlateType.Value = con.Hpzl;
                }
                else
                {
                    start.InnerText = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    end.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    startdate = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    enddate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                DataTable dt1 = GetRedisData.GetData("t_sys_code:140001");
                if (dt1 != null)
                {
                    this.StorePlateType.DataSource = Bll.Common.ChangColName(dt1);
                    this.StorePlateType.DataBind();
                }
                else
                {
                    this.StorePlateType.DataSource = tgsPproperty.GetPalteType();
                    this.StorePlateType.DataBind();
                }

                ButQuery_Event(null, null);
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("FisrtIntoCity28", "访问：初次入城"), userinfo.NowIp, "0");
            }
        }

        /// <summary>
        /// 获取起止时间
        /// </summary>
        /// <param name="isstart">时间类型（true开始时间false结束时间）</param>
        /// <param name="strtime">时间</param>
        [DirectMethod]
        public void GetDateTime(bool isstart, string strtime)
        {
            if (isstart)
                startdate = strtime;
            else
                enddate = strtime;
        }

        #endregion 登录页面初始化

        #region 查询事件

        /// <summary>
        /// 查询事件
        /// </summary>
        public void ButQuery_Event(object sender, DirectEventArgs e)
        {
            //if (string.IsNullOrEmpty(start.InnerText) || string.IsNullOrEmpty(end.InnerText))
            //{
            //    Notice("提示", "请选择要查询的开始时间和结束时间");
            //    return;
            //}
            if (string.IsNullOrEmpty(CmbPlateType.Text)
               && string.IsNullOrEmpty(cboplate.VehicleText) && string.IsNullOrEmpty(txtplate.Text)
               )
            {
                DateTime start = Convert.ToDateTime(startdate);
                DateTime end = Convert.ToDateTime(enddate);
                TimeSpan sp = end.Subtract(start);
                if (sp.TotalMinutes > 120)
                {
                    Notice(GetLangStr("FisrtIntoCity29", "信息提示"), GetLangStr("FisrtIntoCity30", "只能选择两个小时之内的时间！"));
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";
                    ButLast.Disabled = true;
                    ButNext.Disabled = true;
                    StorePeccancy.DataSource = new DataTable();
                    StorePeccancy.DataBind();
                    return;
                }
            }
            int allCount = GetFirstIntoCount();

            if (allCount > 0)
            {
                realCount.Value = allCount;
                pageCount = (int)Math.Ceiling((allCount / 15.0));
                allPage.Value = pageCount;
                curpage.Value = "1";
                QueryFirstIndo(1);
            }
            else
            {
                //this.FormPanelFisrt.Title = "查询结果：当前查询出符合条件的记录0条，当前显示0条";
                //this.LabNum.Text = "当前0页,共0页";
                this.lblCurpage.Text = "1";
                this.lblAllpage.Text = "0";
                this.lblRealcount.Text = "0";
                ButEnd.Disabled = true;
                ButFisrt.Disabled = true;
                ButLast.Disabled = true;
                ButNext.Disabled = true;
                StorePeccancy.DataSource = new DataTable();
                StorePeccancy.DataBind();
                Notice(GetLangStr("FisrtIntoCity29","信息提示"),GetLangStr("FisrtIntoCity31", "当前没数据"));

                return;
            }
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="pageIndex"></param>
        public void QueryFirstIndo(int pageIndex)
        {
            try
            {
                string hphm = "";
                if (cktype.Checked)
                {
                    if (!string.IsNullOrEmpty(cboplate.VehicleText))
                    {
                        hphm = cboplate.VehicleText;

                        hphm = hphm + (string.IsNullOrEmpty(haopai_name1.Value) ? "" : haopai_name1.Value) +
                        (string.IsNullOrEmpty(haopai_name2.Value) ? "" : haopai_name2.Value) +
                        (string.IsNullOrEmpty(haopai_name3.Value) ? "_" : haopai_name3.Value) +
                        (string.IsNullOrEmpty(haopai_name4.Value) ? "_" : haopai_name4.Value) +
                        (string.IsNullOrEmpty(haopai_name5.Value) ? "_" : haopai_name5.Value) +
                        (string.IsNullOrEmpty(haopai_name6.Value) ? "_" : haopai_name6.Value) + "%";
                    }
                    else
                    {
                        hphm = "%" + (string.IsNullOrEmpty(haopai_name1.Value) ? "" : haopai_name1.Value) +
                        (string.IsNullOrEmpty(haopai_name2.Value) ? "" : haopai_name2.Value) +
                        (string.IsNullOrEmpty(haopai_name3.Value) ? "" : haopai_name3.Value) +
                        (string.IsNullOrEmpty(haopai_name4.Value) ? "" : haopai_name4.Value) +
                        (string.IsNullOrEmpty(haopai_name5.Value) ? "" : haopai_name5.Value) +
                        (string.IsNullOrEmpty(haopai_name6.Value) ? "" : haopai_name6.Value) + "%";
                    }
                }
                else
                {
                    hphm = cboplate.VehicleText + txtplate.Text;
                }

                string hpzl = "";
                if (CmbPlateType.SelectedIndex != -1)
                {
                    hpzl = CmbPlateType.SelectedItem.Value;
                }

                //string QueryHphm = string.Empty;
                //if (!string.IsNullOrEmpty(cboplate.VehicleText))
                //{
                //    QueryHphm = cboplate.VehicleText + txtplate.Text;
                //}
                //else
                //{
                //    QueryHphm = txtplate.Text;
                //}
                DataTable dt = null;
                if (cktype.Checked)
                {
                    dt = bll.GetFisrtIntoData(startdate, enddate, hphm, hpzl, pageIndex, 1);
                }
                else
                {
                    dt = bll.GetFisrtIntoData(startdate, enddate, hphm, hpzl, pageIndex, 0);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count.Equals(15)) // 有数据 且够50条
                    {
                        this.lblCurpage.Text = curpage.Value.ToString();
                        this.lblAllpage.Text = allPage.Value.ToString();
                        this.lblRealcount.Text = realCount.Value.ToString();
                        // this.FormPanelFisrt.Title = "查询结果：共计查询出符合条件的记录" + realCount.Value.ToString() + "条,现在显示" + startNum + " - " + realnum + "条";
                    }
                    else
                    {
                        this.lblCurpage.Text = curpage.Value.ToString();
                        this.lblAllpage.Text = allPage.Value.ToString();
                        this.lblRealcount.Text = realCount.Value.ToString();
                        //this.FormPanelFisrt.Title = "查询结果：当前查询出符合条件的记录" + realCount.Value.ToString() + "条，现在显示" + startNum + " - " + realCount.Value.ToString() + "条";
                    }
                    StorePeccancy.DataSource = dt;
                    StorePeccancy.DataBind();
                    SetButState(pageIndex);
                }
                else
                {
                    //this.FormPanelFisrt.Title = "查询结果：当前查询出符合条件的记录0条，当前显示0条";
                    //this.LabNum.Text = "当前0页,共0页";

                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";
                    ButLast.Disabled = true;
                    ButNext.Disabled = true;
                    Notice(GetLangStr("FisrtIntoCity29","信息提示"),GetLangStr("FisrtIntoCity31", "当前没数据"));
                    return;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FisrtIntoCity.aspx-FisrtIntoCity", ex.Message+"；"+ex.StackTrace, "FisrtIntoCity has an exception");
            }
        }

        #endregion 查询事件

        #region 获取查询总数

        /// <summary>
        /// 获取查询总数
        /// </summary>
        /// <param name="pageIndex"></param>
        public int GetFirstIntoCount()
        {
            try
            {
                string hphm = "";
                if (cktype.Checked)
                {
                    if (!string.IsNullOrEmpty(cboplate.VehicleText))
                    {
                        hphm = cboplate.VehicleText;

                        hphm = hphm + (string.IsNullOrEmpty(haopai_name1.Value) ? "" : haopai_name1.Value) +
                        (string.IsNullOrEmpty(haopai_name2.Value) ? "" : haopai_name2.Value) +
                        (string.IsNullOrEmpty(haopai_name3.Value) ? "" : haopai_name3.Value) +
                        (string.IsNullOrEmpty(haopai_name4.Value) ? "" : haopai_name4.Value) +
                        (string.IsNullOrEmpty(haopai_name5.Value) ? "" : haopai_name5.Value) +
                        (string.IsNullOrEmpty(haopai_name6.Value) ? "" : haopai_name6.Value) + "%";
                    }
                    else
                    {
                        hphm = "%" + (string.IsNullOrEmpty(haopai_name1.Value) ? "" : haopai_name1.Value) +
                        (string.IsNullOrEmpty(haopai_name2.Value) ? "" : haopai_name2.Value) +
                        (string.IsNullOrEmpty(haopai_name3.Value) ? "" : haopai_name3.Value) +
                        (string.IsNullOrEmpty(haopai_name4.Value) ? "" : haopai_name4.Value) +
                        (string.IsNullOrEmpty(haopai_name5.Value) ? "" : haopai_name5.Value) +
                        (string.IsNullOrEmpty(haopai_name6.Value) ? "" : haopai_name6.Value) + "%";
                    }
                }
                else
                {
                    hphm = cboplate.VehicleText + txtplate.Text;
                }

                string hpzl = "";
                if (CmbPlateType.SelectedIndex != -1)
                {
                    hpzl = CmbPlateType.SelectedItem.Value;
                }
                //string QueryHphm = string.Empty;
                //if (!string.IsNullOrEmpty(cboplate.VehicleText))
                //{
                //    QueryHphm = cboplate.VehicleText + txtplate.Text;
                //}
                //else
                //{
                //    QueryHphm = txtplate.Text;
                //}
                DataTable tempdt = null;
                if (cktype.Checked)
                {
                    tempdt = bll.GettFisrtIntoCount(startdate, enddate, hphm, hpzl, 1);
                }
                else
                {
                    tempdt = bll.GettFisrtIntoCount(startdate, enddate, hphm, hpzl, 0);
                }

                if (tempdt != null && tempdt.Rows.Count > 0)
                {
                    return Convert.ToInt32(tempdt.Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FisrtIntoCity.aspx-GetFirstIntoCount", ex.Message+"；"+ex.StackTrace, "GetFirstIntoCount has an exception");
            }
            return 0;
        }

        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            ButQuery_Event(null, null);
        }

        #endregion 获取查询总数

        #region 提示信息

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
                Icon = Ext.Net.Icon.Information,
                HideDelay = 2000,
                Height = 120,
                Html = "<br></br>" + msg + "!"
            });
        }

        #endregion 提示信息

        #region 显示详情

        /// <summary>
        ///显示详情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowDetails(object sender, DirectEventArgs e)
        {
            try
            {
                string data = e.ExtraParams["data"];
                string field = e.ExtraParams["field"];
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FisrtIntoCity.aspx-ShowDetails", ex.Message+"；"+ex.StackTrace, "ShowDetails has an exception");
            }
        }

        #endregion 显示详情

        #region 分页加载

        /// <summary>
        /// 首页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutFisrt(object sender, DirectEventArgs e)
        {
            try
            {
                pageIndex = 1;
                curpage.Value = "1";
                QueryFirstIndo(1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FisrtIntoCity.aspx-TbutFisrt", ex.Message+"；"+ex.StackTrace, "TbutFisrt has an exception");
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
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                pageIndex--;
                curpage.Value = pageIndex.ToString();
                QueryFirstIndo(pageIndex);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FisrtIntoCity.aspx-TbutLast", ex.Message+"；"+ex.StackTrace, "TbutLast has an exception");
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
                if (pageIndex > pageCount)
                {
                    pageIndex = pageCount;
                }
                pageIndex++;
                curpage.Value = pageIndex.ToString();
                QueryFirstIndo(pageIndex);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FisrtIntoCity.aspx-TbutNext", ex.Message+"；"+ex.StackTrace, "TbutNext has an exception");
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
                pageIndex = Convert.ToInt32(allPage.Value);
                curpage.Value = pageIndex.ToString();
                QueryFirstIndo(pageIndex);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FisrtIntoCity.aspx-TbutEnd", ex.Message+"；"+ex.StackTrace, "TbutEnd has an exception");
            }
        }

        #endregion 分页加载

        #region 行选中事件

        /// <summary>
        /// 转换查询模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void changtype(object sender, EventArgs e)
        {
            txtplate.Hidden = cktype.Checked;
            pnhphm.Hidden = !cktype.Checked;
        }

        /// <summary>
        /// 行选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ApplyClick(object sender, DirectEventArgs e)
        {
            try
            {
                this.FormPanel1.Collapsed = false;
                string sdata = e.ExtraParams["data"];
                string hphm = Bll.Common.GetdatabyField(sdata, "col3");
                string hpzl = Bll.Common.GetdatabyField(sdata, "col5");
                string url1 = Bll.Common.GetdatabyField(sdata, "col16");
                string url2 = Bll.Common.GetdatabyField(sdata, "col17");
                string url3 = Bll.Common.GetdatabyField(sdata, "col18");
                if (string.IsNullOrEmpty(url2))
                {
                    url2 = NoImageUrl;
                }
                if (string.IsNullOrEmpty(url3))
                {
                    url3 = NoImageUrl;
                }
                string js = "ShowImage(\"" + dataCommon.ChangePoliceIp(url1) + "\",\"" + dataCommon.ChangePoliceIp(url2) + "\",\"" + dataCommon.ChangePoliceIp(url3) + "\",\"" + hphm + "\",\"" + hpzl + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FisrtIntoCity.aspx-ApplyClick", ex.Message+"；"+ex.StackTrace, "ApplyClick has an exception");
            }
        }

        #endregion 行选中事件

        #region 设置按钮状态

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="page"></param>
        private void SetButState(int page)
        {
            try
            {
                int allpage = Convert.ToInt32(allPage.Value);
                // this.PagingToolbar1.PageIndex = 0;
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
                    page = 1;
                }
                //LabNum.Html = "<font >&nbsp;&nbsp;当前" + page.ToString() + "页,共" + allpage.ToString() + "页</font>";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FisrtIntoCity.aspx-SetButState", ex.Message+"；"+ex.StackTrace, "SetButState has an exception");
            }

        #endregion 设置按钮状态
        }
        #region 多语言转换

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

        #endregion 多语言转换
    }
}