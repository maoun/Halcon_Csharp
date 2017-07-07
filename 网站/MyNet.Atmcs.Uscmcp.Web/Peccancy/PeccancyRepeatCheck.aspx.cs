using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PeccancyRepeatCheck : System.Web.UI.Page
    {
        #region 成员变量

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private SettingManager settingManager = new SettingManager();
        private UserLogin userLogin = new UserLogin();
        private const string NoImageUrl = "../images/NoImage.png";
        private DataCommon dataCommon = new DataCommon();
        private static string sdr;
        private static string starttime = "";
        private static string endtime = "";
        private static DataTable dtFangxiang = null;

        #endregion 成员变量

        #region 控件事件

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
                string js = "alert('" + GetLangStr("PeccancyQuery35", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                StoreDataBind();
                DataSetDateTime();
                this.DataBind();
                this.lblCurpage.Text = "1";
                this.lblAllpage.Text = "0";
                this.lblRealcount.Text = "0";
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("PeccancyQuery36","访问：") + Request.QueryString["funcname"], userinfo.NowIp, "0");
            }
            this.DataBind();
        }

        /// <summary>
        /// 转换查询模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void changtype(object sender, EventArgs e)
        {
            TxtplateId.Hidden = ChkLike.Checked;
            pnhphm.Hidden = !ChkLike.Checked;
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            GetData();
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
                logManager.InsertLogError("PeccancyRepeatCheck.aspx-TbutFisrt", ex.Message + "；" + ex.StackTrace, "TbutFisrt has an exception");
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
                logManager.InsertLogError("PeccancyRepeatCheck.aspx-TbutLast", ex.Message + "；" + ex.StackTrace, "TbutLast has an exception");
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
                logManager.InsertLogError("PeccancyRepeatCheck.aspx-TbutNext", ex.Message + "；" + ex.StackTrace, "TbutNext has an exception");
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
                logManager.InsertLogError("PeccancyRepeatCheck.aspx-TbutEnd", ex.Message + "；" + ex.StackTrace, "TbutEnd has an exception");
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
                int rownum;
                if (CmbQueryNum.SelectedItem.Value != null)
                {
                    rownum = Convert.ToInt32(CmbQueryNum.SelectedItem.Value.ToString());
                }
                else
                {
                    rownum = 50;
                }

                int startNum = (currentPage - 1) * rownum;
                int endNum = currentPage * rownum;
                Query(GetWhere(), startNum, endNum);
                //SetButState(currentPage);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyRepeatCheck.aspx-ShowQuery", ex.Message + "；" + ex.StackTrace, "ShowQuery has an exception");
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
                DataTable dt = tgsDataInfo.GetPeccancyInfo(GetWhere() + "  and sdr='" + sdr + "'", startNum, endNum);
                Session["checkjson"] = ConvertData.DataTableToJson(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ButCheck.Disabled = false;
                }
                else
                {
                    ButCheck.Disabled = true;
                }

                this.StorePeccancy.DataSource = dt;
                this.StorePeccancy.DataBind();

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "1";
                    this.lblRealcount.Text = dt.Rows.Count.ToString();

                    ButCheck.Disabled = false;
                }
                else
                {
                    this.lblCurpage.Text = "1";
                    this.lblAllpage.Text = "0";
                    this.lblRealcount.Text = "0";
                    Notice(GetLangStr("PeccancyRepeatCheck37", "信息提示"), GetLangStr("PeccancyRepeatCheck38", "未查询到相关记录"));
                    this.StorePeccancy.DataSource = new DataTable();
                    this.StorePeccancy.DataBind();
                    ButCheck.Disabled = true;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyRepeatCheck.aspx-Query", ex.Message + "；" + ex.StackTrace, "Query has an exception");
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            try
            {
                starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string timeJs = "clearTime('" + starttime + "','" + endtime + "');";//js方法后面的分号一定要加上
                this.ResourceManager1.RegisterAfterClientInitScript(timeJs);
                this.uiDepartment.Reset();
                this.CmbPecType.Reset();
                this.CmbPlateType.Reset();
                this.CmbLocation.Reset();
                this.CmbDataSource.Reset();
                this.WindowEditor1.SetVehicleText("");
                //CmbDealType.SelectedIndex = 0;
                this.CmbDealType.Reset();
                this.TxtplateId.Reset();
                this.ChkLike.Reset();
                this.CmbDirection.Reset();
                if (Session["location"] != null)
                {
                    Session["location"] = null;
                }

                DataTable dt2 = GetRedisData.GetData("Station:t_cfg_set_station_type_istmsshow");
                if (dt2 != null)
                {
                    this.StoreLocation.DataSource = ChangColName(dt2);
                    this.StoreLocation.DataBind();
                }
                else
                {
                    this.StoreLocation.DataSource = tgsPproperty.GetStationInfo("b.istmsshow ='1'");
                    this.StoreLocation.DataBind();
                }
                //行驶方向
                DataTable dt3 = GetRedisData.GetData("t_sys_code:240025");
                if (dt3 != null)
                {
                    this.StoreDirection.DataSource = GetRedisData.ChangColName(dt3, true);
                    this.StoreDirection.DataBind();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyRepeatCheck.aspx-ButResetClick", ex.Message + "；" + ex.StackTrace, "ButResetClick has an exception");
            }

            //UserInfo userInfo = System.Web.HttpContext.Current.Session["userinfo"] as UserInfo;
            //DataTable ddbh = new DataTable();
            //ddbh = tgsPproperty.GetCheckLocation(userInfo.UserCode, "1");
            //if (ddbh != null || ddbh.Rows.Count > 0)
            //{
            //    Session["location"] = ddbh;
            //}
            //else
            //{
            //    ddbh = tgsPproperty.GetStationInfo("b.istmsshow ='1'");
            //    Session["location"] = null;
            //}
            //this.StoreLocation.DataSource = ddbh;
            //this.StoreLocation.DataBind();
        }

        public static DataTable ChangColName(DataTable dt)
        {
            try
            {
                if (dt != null)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dt.Columns[i].ColumnName = "col" + (i + 1);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);

                return null;
            }
        }

        /// <summary>
        /// 显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowDetails(object sender, DirectEventArgs e)
        {
            string data = e.ExtraParams["data"];
            AddWindow(data);
        }

        /// <summary>
        /// 地点刷新后 更新方向信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TgsRefresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                DataTable data = tgsPproperty.GetDirectionInfoByStation2(this.CmbLocation.SelectedItem.Value);
                this.StoreDirection.DataSource = data;
                this.StoreDirection.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyRepeatCheck.aspx-TgsRefresh", ex.Message + "；" + ex.StackTrace, "TgsRefresh has an exception");
            }
        }

        /// <summary>
        /// 开始审核事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButCheckClick(object sender, DirectEventArgs e)
        {
            try
            {
                string js = "OpenRepeatCheckModelPage();";

                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyRepeatCheck.aspx-ButCheckClick", ex.Message + "；" + ex.StackTrace, "ButCheckClick has an exception");
            }
        }

        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DataSourceRefresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                if (this.CmbDataSource.Value != null && CmbDataSource.SelectedIndex != -1)
                {
                    DataTable ddbh = tgsPproperty.GetStationInfo("b.datasource='" + this.CmbDataSource.Value.ToString() + "' and b.istmsshow ='1'");
                    this.StoreLocation.DataSource = ddbh;
                    this.StoreLocation.DataBind();
                    Session["location"] = ddbh;
                }
                else
                {
                    DataTable ddbh = GetRedisData.GetData("Station:t_cfg_set_station_type_istmsshow");
                    if (ddbh != null)
                    {
                        this.StoreLocation.DataSource = ChangColName(ddbh);
                        this.StoreLocation.DataBind();
                    }
                    else
                    {
                        DataTable ddbh1 = tgsPproperty.GetStationInfo("b.istmsshow ='1'");
                        this.StoreLocation.DataSource = ddbh1;
                        this.StoreLocation.DataBind();
                    }
                    Session["location"] = null;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyRepeatCheck.aspx-DataSourceRefresh", ex.Message + "；" + ex.StackTrace, "DataSourceRefresh has an exception");
            }
        }

        /// <summary>
        /// 行选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApplyClick(object sender, DirectEventArgs e)
        {
            try
            {
                this.FormPanel1.Collapsed = false;
                string sdata = e.ExtraParams["data"];
                string hphm = Bll.Common.GetdatabyField(sdata, "col3");
                string hpzl = Bll.Common.GetdatabyField(sdata, "col1");

                string url1 = Bll.Common.GetdatabyField(sdata, "col23");
                string url2 = Bll.Common.GetdatabyField(sdata, "col24");
                string url3 = Bll.Common.GetdatabyField(sdata, "col25");
                if (string.IsNullOrEmpty(url2))
                {
                    url2 = NoImageUrl;
                }
                if (string.IsNullOrEmpty(url3))
                {
                    url3 = NoImageUrl;
                }
                string js = "ShowImage(\"" + url1 + "\",\"" + url2 + "\",\"" + url3 + "\",\"" + hphm + "\",\"" + hpzl + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyRepeatCheck.aspx-ApplyClick", ex.Message + "；" + ex.StackTrace, "ApplyClick has an exception");
            }
        }

        #endregion 控件事件

        #region 私有方法

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
                logManager.InsertLogError("PeccancyRepeatCheck.aspx-SetButState", ex.Message + "；" + ex.StackTrace, "SetButState has an exception");
            }
        }

        /// <summary>
        /// 获得查询数据
        /// </summary>
        /// <returns></returns>
        private void GetData()
        {
            string rownum = "";
            if (CmbQueryNum.SelectedItem.Value != null)
            {
                rownum = CmbQueryNum.SelectedItem.Value.ToString();
            }
            else
            {
                rownum = "50";
            }
            sdr = Request.ServerVariables.Get("Remote_Addr").ToString();
            if (sdr.Length < 9)
            {
                sdr = "127.0.0.1";
            }
            tgsDataInfo.UnAlllockAll(DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss"), sdr); //将超过1个小时未解锁或者自己的的违法记录全部解锁
            tgsDataInfo.LockPeccancy(GetWhere(), sdr, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToInt32(rownum)); //对自己查询的信息进行加锁
            //DataTable dtCount = tgsDataInfo.GetPeccancyInfoCount(GetWhere() + "  and sdr='" + sdr + "'");//获得总记录
            //if (dtCount != null && dtCount.Rows.Count > 0 && Convert.ToInt32(dtCount.Rows[0]["col0"].ToString()) > 0)
            //{
            //    realCount.Value = dtCount.Rows[0]["col0"].ToString();
            //    curpage.Value = 1;
            //    allPage.Value = (int)Math.Ceiling(double.Parse(realCount.Value.ToString()) / Convert.ToInt32(rownum));
            ShowQuery(1);
            //}
            //else
            //{
            //    this.lblCurpage.Text = "1";
            //    this.lblAllpage.Text = "0";
            //    this.lblRealcount.Text = "0";
            //    this.StorePeccancy.DataSource = new DataTable();
            //    this.StorePeccancy.DataBind();
            //    Notice("信息提示", "未查询到符合条件的任何记录！");
            //}
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            UserInfo userInfo = System.Web.HttpContext.Current.Session["userinfo"] as UserInfo;
            string where = "1=1";
            if (string.IsNullOrEmpty(starttime)) starttime = start.InnerText;
            if (string.IsNullOrEmpty(endtime)) endtime = end.InnerText;
            string kssj = DateTime.Parse(starttime).ToString("yyyy-MM-dd HH:mm:ss");
            string jssj = DateTime.Parse(endtime).ToString("yyyy-MM-dd HH:mm:ss");
            where = where + "  and  wfsj >= STR_TO_DATE('" + kssj + "','%Y-%m-%d %H:%i:%s')   and wfsj<=STR_TO_DATE('" + jssj + "','%Y-%m-%d %H:%i:%s')";
            if (!string.IsNullOrEmpty(uiDepartment.DepertId))
            {
                where = where + " and  cjjg='" + uiDepartment.DepertId + "' ";
            }
            if (CmbPlateType.SelectedIndex != -1)
            {
                where = where + " and  hpzl='" + CmbPlateType.SelectedItem.Value + "' ";
            }
            if (CmbDealType.SelectedIndex != -1)
            {
                if (CmbDealType.SelectedItem.Value == null)
                {
                    where = where + " and  jcbj='1' ";
                }
                else
                {
                    where = where + " and  jcbj='" + CmbDealType.SelectedItem.Value + "' ";
                }
            }
            if (CmbLocation.SelectedIndex != -1)
            {
                where = where + " and  kkid='" + CmbLocation.SelectedItem.Value + "' ";

                if (CmbDirection.SelectedIndex != -1)
                {
                    if (CmbDirection.SelectedItem.Value.Equals("00") || CmbDirection.SelectedItem.Value.Equals("0"))
                    {
                        string fxs = "";//方向编号集合
                        for (int i = 1; i < dtFangxiang.Rows.Count; i++)
                        {
                            if (i == dtFangxiang.Rows.Count - 1)
                            {
                                fxs = fxs + "'" + dtFangxiang.Rows[i]["col0"].ToString() + "'";
                            }
                            else
                            {
                                fxs = fxs + "'" + dtFangxiang.Rows[i]["col0"].ToString() + "',";
                            }
                        }
                        where = where + " and  fxbh in (" + fxs + ") ";
                    }
                    else
                    {
                        where = where + " and  fxbh='" + CmbDirection.SelectedItem.Value + "' ";
                    }
                }
            }
            else
            {
                DataTable location = Session["location"] as DataTable;
                if (location != null && location.Rows.Count > 0)
                {
                    string temp = "";
                    for (int i = 0; i < location.Rows.Count; i++)
                    {
                        if (i != location.Rows.Count - 1)
                        {
                            temp = temp + "'" + location.Rows[i]["col1"].ToString() + "',";
                        }
                        else
                        {
                            temp = temp + "'" + location.Rows[i]["col1"].ToString() + "'";
                        }
                    }
                    where = where + " and  kkid in ( " + temp + " )";
                }
            }

            if (CmbPecType.SelectedIndex != -1)
            {
                where = where + " and  wfxw='" + CmbPecType.SelectedItem.Value + "' ";
            }

            if (CmbDataSource.SelectedIndex != -1)
            {
                where = where + " and  sjly='" + CmbDataSource.SelectedItem.Value + "' ";
            }
            string QueryHphm = string.Empty;
            if (ChkLike.Checked)
            {
                //if (!string.IsNullOrEmpty(QueryHphm))
                //{
                string hphm = (string.IsNullOrEmpty(haopai_name1.Value) ? "" : haopai_name1.Value) +
             (string.IsNullOrEmpty(haopai_name2.Value) ? "" : haopai_name2.Value) +
             (string.IsNullOrEmpty(haopai_name3.Value) ? "" : haopai_name3.Value) +
             (string.IsNullOrEmpty(haopai_name4.Value) ? "" : haopai_name4.Value) +
             (string.IsNullOrEmpty(haopai_name5.Value) ? "" : haopai_name5.Value) +
             (string.IsNullOrEmpty(haopai_name6.Value) ? "" : haopai_name6.Value);
                QueryHphm = WindowEditor1.VehicleText + hphm;
                if (!string.IsNullOrEmpty(QueryHphm))
                {
                    where = where + " and  hphm  like '%" + QueryHphm.ToUpper() + "%' ";
                }
                else
                {
                    where = where + " and  hphm  like '%" + QueryHphm.ToUpper() + "%' ";
                }

                // }
            }
            else
            {
                QueryHphm = WindowEditor1.VehicleText + TxtplateId.Text;
                if (!string.IsNullOrEmpty(QueryHphm))
                {
                    where = where + " and  hphm='" + QueryHphm.ToUpper() + "' ";
                }
            }
            //if (CmbQueryNum.SelectedIndex != -1)
            //{
            //    where = where + " and  rownum<=" + CmbQueryNum.SelectedItem.Value;
            //}
            return where;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        private string GetUrl(DataTable dt, string idx)
        {
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][1].ToString() == idx)
                    {
                        return dt.Rows[i][0].ToString();
                    }
                }
                return "";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sdata"></param>
        private void AddWindow(string sdata)
        {
            //DataTable dt = tgsDataInfo.GetPassCarImageUrl(Bll.Common.GetdatabyField(sdata, "col0"));
            Window win = WindowShow.AddPeccancy(sdata);
            win.Render(this.Form);
            win.Show();
        }

        /// <summary>
        /// 提示
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
        /// 转换
        /// </summary>
        /// <param name="isALL"></param>
        /// <param name="xzjb"></param>
        /// <param name="jgjb"></param>
        /// <param name="depcode"></param>
        /// <returns></returns>
        public string ConvertCondition(bool isALL, string xzjb, string jgjb, string depcode)
        {
            string strWhere = "";
            if (xzjb == "1")
            {
                if (isALL == true)
                {
                    if (jgjb == "2")
                    {
                        strWhere = "and substr(cjjg,0,4)='" + depcode.Substring(0, 4) + "'";
                    }
                    else if (jgjb == "3")
                    {
                        strWhere = "and substr(cjjg,0,8)='" + depcode.Substring(0, 8) + "'";
                    }
                    else if (jgjb == "4")
                    {
                        strWhere = "and cjjg='" + depcode + "'";
                    }
                }
                else
                {
                    if (jgjb == "2")
                    {
                        strWhere = "and substr(cjjg,0,4)='" + depcode.Substring(0, 4) + "'";
                    }
                    else if (jgjb == "3")
                    {
                        strWhere = "and substr(cjjg,0,8)='" + depcode.Substring(0, 8) + "'";
                    }
                    else if (jgjb == "4" || jgjb == "1")
                    {
                        strWhere = "and cjjg='" + depcode + "'";
                    }
                }
            }
            else if (xzjb == "2")
            {
                if (isALL == true)
                {
                    if (jgjb == "2")
                    {
                        strWhere = "and substr(cjjg,0,4)='" + depcode.Substring(0, 4) + "'";
                    }
                    else if (jgjb == "3")
                    {
                        strWhere = "and substr(cjjg,0,8)='" + depcode.Substring(0, 8) + "'";
                    }
                    else if (jgjb == "4")
                    {
                        strWhere = "and cjjg='" + depcode + "'";
                    }
                }
                else
                {
                    if (jgjb == "3")
                    {
                        strWhere = "and substr(cjjg,0,8)='" + depcode.Substring(0, 8) + "'";
                    }
                    else if (jgjb == "4" || jgjb == "2")
                    {
                        strWhere = "and cjjg='" + depcode + "'";
                    }
                }
            }
            else if (xzjb == "3")
            {
                if (isALL == true)
                {
                    if (jgjb == "3")
                    {
                        strWhere = "and substr(cjjg,0,8)='" + depcode.Substring(0, 8) + "'";
                    }
                    else if (jgjb == "4")
                    {
                        strWhere = "and cjjg='" + depcode + "'";
                    }
                }
                else
                {
                    if (jgjb == "4" || jgjb == "3")
                    {
                        strWhere = "and cjjg='" + depcode + "'";
                    }
                }
            }
            else if (xzjb == "4")
            {
                strWhere = "and cjjg='" + depcode + "'";
            }

            return strWhere;
        }

        #endregion 私有方法

        #region DirectMethod

        public DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                //UserInfo userInfo = System.Web.HttpContext.Current.Session["userinfo"] as UserInfo;

                //DataTable ddbh = new DataTable();
                //ddbh = tgsPproperty.GetUserStationInfo(userInfo.UserCode, "2");

                //if (ddbh != null || ddbh.Rows.Count > 0)
                //{
                //    Session["location"] = ddbh;
                //}
                //else
                //{
                //    ddbh = tgsPproperty.GetStationInfo("b.istmsshow ='1'");
                //}
                //this.StoreLocation.DataSource = ddbh;
                //this.StoreLocation.DataBind();

                //车俩类型
                DataTable dt1 = GetRedisData.GetData("t_sys_code:140001");
                if (dt1 != null)
                {
                    this.StorePlateType.DataSource = GetRedisData.ChangColName(dt1, true);
                    this.StorePlateType.DataBind();
                }
                else
                {
                    this.StorePlateType.DataSource = tgsPproperty.GetPalteType();
                    this.StorePlateType.DataBind();
                }

                //违法地点
                DataTable dt2 = GetRedisData.GetData("Station:t_cfg_set_station_type_istmsshow");
                if (dt2 != null)
                {
                    this.StoreLocation.DataSource = ChangColName(dt2);
                    this.StoreLocation.DataBind();
                }
                else
                {
                    this.StoreLocation.DataSource = tgsPproperty.GetStationInfo("b.istmsshow ='1'");
                    this.StoreLocation.DataBind();
                }

                //违法行为
                DataTable dt3 = GetRedisData.GetData("Peccancy:WFXW");
                if (dt3 != null)
                {
                    this.StorePecType.DataSource = GetRedisData.ChangColName(dt3, true);
                    this.StorePecType.DataBind();
                }
                else
                {
                    this.StorePecType.DataSource = tgsPproperty.GetPeccancyType("isuse='1'");
                    this.StorePecType.DataBind();
                }

                //数据来源
                DataTable dt4 = GetRedisData.GetData("t_sys_code:240022");
                if (dt4 != null)
                {
                    this.StoreDataSource.DataSource = GetRedisData.ChangColName(dt4, true);
                    this.StoreDataSource.DataBind();
                }
                else
                {
                    this.StoreDataSource.DataSource = tgsPproperty.GetDeviceTypeDict("240022");
                    this.StoreDataSource.DataBind();
                }

                //处理状态
                DataTable dt5 = GetRedisData.GetData("t_sys_code:240019");
                if (dt5 != null)
                {
                    dt5.Rows.RemoveAt(0);
                    dt5.Rows.RemoveAt(1);
                    this.StoreDealType.DataSource = GetRedisData.ChangColName(dt5, true);
                    this.StoreDealType.DataBind();
                }
                else
                {
                    DataTable dt8 = tgsPproperty.GetProcessType();
                    dt8.Rows.RemoveAt(0);
                    dt8.Rows.RemoveAt(1);
                    this.StoreDealType.DataSource = dt8;
                    this.StoreDealType.DataBind();
                }
                //DataTable deal = GetRedisData.GetData("t_sys_code:240019");
                //deal.Rows.RemoveAt(0);
                //deal.Rows.RemoveAt(1);
                //this.StoreDealType.DataSource = deal;
                //this.StoreDealType.DataBind();

                //行驶方向
                DataTable dt6 = dtFangxiang = GetRedisData.GetData("t_sys_code:240025");
                if (dt6 != null)
                {
                    this.StoreDirection.DataSource = GetRedisData.ChangColName(dt6, true);
                    this.StoreDirection.DataBind();
                }
                else
                {
                }
                DataTable dt7 = GetRedisData.GetData("t_sys_code:140006");

                if (dt7 != null)
                {
                    this.StoreQueryNum.DataSource = Bll.Common.ChangColName(dt7);
                    this.StoreQueryNum.DataBind();
                }
                else
                {
                    this.StoreQueryNum.DataSource = tgsPproperty.GetQueryNum();
                    this.StoreQueryNum.DataBind();
                }
                //DataTable dt = tgsPproperty.GetQueryNum();
                //this.StoreQueryNum.DataSource = dt;
                //this.StoreQueryNum.DataBind();
                CmbDealType.SelectedIndex = 0;

                if (dt7.Rows.Count > 0)
                    CmbQueryNum.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyRepeatCheck.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 设置初始时间
        /// </summary>
        private void DataSetDateTime()
        {
            try
            {
                starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                start.InnerText = starttime;

                endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                end.InnerText = endtime;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyRepeatCheck.aspx-DataSetDateTime", ex.Message + "；" + ex.StackTrace, "DataSetDateTime has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="command"></param>
        /// <param name="url"></param>
        [DirectMethod]
        public void VideoShow(string command, string url)
        {
            //Window win = WindowShow.AddPlayVideo(dataCommon.ChangePoliceIp(url));
            //win.Render(this.Form);
            //win.Show();
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="isstart"></param>
        /// <param name="strtime"></param>
        [DirectMethod]
        public void GetDateTime(bool isstart, string strtime)
        {
            try
            {
                if (isstart)
                    starttime = strtime;
                else
                    endtime = strtime;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyRepeatCheck.aspx-GetDateTime", ex.Message + "；" + ex.StackTrace, "GetDateTime has an exception");
            }
        }

        #endregion DirectMethod

        #region 语言转换

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

        #endregion 语言转换
    }
}