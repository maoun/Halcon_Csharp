using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;
using System.Xml;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PeccancyArea : System.Web.UI.Page
    {
        #region 成员变量

        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();
        private DataCommon dataCommon = new DataCommon();
        private static string starttime = "";
        private static string endtime = "";
        private const string NoImageUrl = "../images/NoImage.png";
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 页面加载时候执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断用户是否登录
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            // 判断用户是否登录结束
            if (!X.IsAjaxRequest)
            {
                this.StorePlateType.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:140001")); // tgsPproperty.GetPalteType();
                this.StorePlateType.DataBind();
                this.StoreStartStation.DataSource = tgsPproperty.GetStartStationInfo();
                this.StoreStartStation.DataBind();
                DataTable deal = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240019")); // tgsPproperty.GetProcessType();

                this.StoreDealType.DataSource = deal;
                this.StoreDealType.DataBind();
                this.StorePecType.DataSource = GetRedisData.ChangColName(GetRedisData.GetData("Peccancy:WFXW"), true);//tgsPproperty.GetPeccancyType("isuse='1'");
                this.StorePecType.DataBind();
                DataTable dt = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:140006"));// tgsPproperty.GetQueryNum();
                //    this.StoreQueryNum.DataSource = dt;
                //   this.StoreQueryNum.DataBind();
                starttime = start.InnerText = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                endtime = end.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                TbutQueryClick(null, null);
               
               UserInfo userinfo=Session["Userinfo"] as UserInfo;
                //// if (dt.Rows.Count > 0)
                ////      CmbQueryNum.SelectedIndex = 0;
                ////   this.Panel2.Title = "查询结果：共计查询出符合条件的记录0条,现在显示0条";
               logManager.InsertLogRunning(userinfo.UserName, "访问：" + Request.QueryString["funcname"], userinfo.NowIp, "0");
            }
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
                string hphm = Bll.Common.GetdatabyField(sdata, "col2");
                string hpzl = Bll.Common.GetdatabyField(sdata, "col4");

                string url1 = Bll.Common.GetdatabyField(sdata, "col29");
                string url2 = Bll.Common.GetdatabyField(sdata, "col30");
                string url3 = Bll.Common.GetdatabyField(sdata, "");
                if (string.IsNullOrEmpty(url1))
                {
                    url1 = NoImageUrl;
                }
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
                logManager.InsertLogError("PeccancyArea.aspx-ApplyClick", ex.Message+"；"+ex.StackTrace, "ApplyClick has an exception");
            }
        }

        /// <summary>
        /// 刷新页面方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            GetData();
        }

        /// <summary>
        /// 刷新结束卡口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TgsRefresh(object sender, StoreRefreshDataEventArgs e)
        {
            try { 
            DataTable data = tgsPproperty.GetEndStationInfo(this.CmbStartStation.SelectedItem.Value);
            this.StoreEndStation.DataSource = data;
            this.StoreEndStation.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyArea.aspx-TgsRefresh", ex.Message+"；"+ex.StackTrace, "TgsRefresh has an exception");
            }
            }

        /// <summary>
        /// 重置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            try { 
            starttime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeJs = "clearTime('" + starttime + "','" + endtime + "');";//js方法后面的分号一定要加上
            this.ResourceManager1.RegisterAfterClientInitScript(timeJs);
            this.CmbPlateType.Reset();
            this.CmbStartStation.Reset();
            this.CmbEndStation.Reset();
            this.CmbPecType.Reset();
            this.TxtplateId.Reset();
            this.ChkLike.Reset();
            this.CmbDealType.Reset();
            this.TxtMinSpeed.Text = "";
            this.TxtMaxSpeed.Text = "";
            WindowEditor1.SetVehicleText("");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyArea.aspx-ButResetClick", ex.Message+"；"+ex.StackTrace, "ButResetClick has an exception");
            }
        }

        #endregion 控件事件

        #region DirectMethod

        /// 显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowDetails(object sender, DirectEventArgs e)
        {
            try
            {
                string data = e.ExtraParams["data"];
                AddWindow(data);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyArea.aspx-ShowDetails", ex.Message+"；"+ex.StackTrace, "ShowDetails has an exception");
            }
        }

        /// <summary>
        /// 图片展示界面
        /// </summary>
        /// <param name="data"></param>
        [DirectMethod]
        public void ShowImg(string data)
        {
            try { 
            StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(data, null);
            XmlNode xml = eSubmit.Xml;
            AddImgWindow(xml.InnerXml);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyArea.aspx-ShowImg", ex.Message+"；"+ex.StackTrace, "ShowImg has an exception");
            }
        }

        /// <summary>
        ///获取选中时间
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
                logManager.InsertLogError("PeccancyArea.aspx-GetDateTime", ex.Message+"；"+ex.StackTrace, "GetDateTime has an exception");
            }
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            //  this.Panel2.Title = "查询结果：共计查询出符合条件的记录0条,现在显示0条";
            GetData();
        }

        #endregion DirectMethod

        #region 私有方法

        /// <summary>
        /// 创建一个图片弹出窗体
        /// </summary>
        /// <param name="sdata"></param>
        private void AddImgWindow(string sdata)
        {
            Window win = WindowShow.AddPeccancyAreaImg(sdata);
            win.Render(this.Form);
            win.Show();
        }

        /// <summary>
        /// 创建一个弹出窗体
        /// </summary>
        /// <param name="sdata"></param>
        private void AddWindow(string sdata)
        {
            Window win = WindowShow.AddPeccancyArea(sdata);
            win.Render(this.Form);
            win.Show();
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
                logManager.InsertLogError("PeccancyArea.aspx-GetDateTime", ex.Message+"；"+ex.StackTrace, "GetDateTime has an exception");
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutNext(object sender, DirectEventArgs e)
        {
            try { 
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
                logManager.InsertLogError("PeccancyArea.aspx-TbutNext", ex.Message+"；"+ex.StackTrace, "TbutNext has an exception");
            }
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutFisrt(object sender, DirectEventArgs e)
        {
            try { 
            curpage.Value = 1;
            ShowQuery(1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyArea.aspx-TbutFisrt", ex.Message+"；"+ex.StackTrace, "TbutFisrt has an exception");
            }
        }

        /// <summary>
        /// 尾页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutEnd(object sender, DirectEventArgs e)
        {
            try { 
            curpage.Value = allPage.Value;
            int page = int.Parse(curpage.Value.ToString());
            ShowQuery(page);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyArea.aspx-TbutEnd", ex.Message+"；"+ex.StackTrace, "TbutEnd has an exception");
            }
        }

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="page"></param>
        private void SetButState(int page)
        {
            try { 
            curpage.Value = page;
            int allpage = int.Parse(allPage.Value.ToString());
            //  this.PagingToolbar1.PageIndex = 0;
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
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyArea.aspx-SetButState", ex.Message+"；"+ex.StackTrace, "SetButState has an exception");
            }
        }

        /// <summary>
        /// 得到提取条数
        /// </summary>
        /// <returns></returns>
        //private int GetRowNum()
        //{
        //    string rownum = "";
        //    if (CmbQueryNum.SelectedIndex != -1)
        //    {
        //        rownum = CmbQueryNum.SelectedItem.Value.ToString();
        //    }
        //    else
        //    {
        //        rownum = "50";
        //    }
        //    return int.Parse(rownum);
        //}

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startNum"></param>
        /// <param name="endNum"></param>
        private void Query(string where, int startNum, int endNum)//endNum 表示页容量
        {

            try { 
            // int rownum = 15;

            // int realnum = GetRowNum() * Convert.ToInt32(curpage.Value);
            DataTable dt = tgsDataInfo.GetPeccancyAreaInfo(where, startNum, endNum);
            if (dt != null && dt.Rows.Count > 0)
            {
                this.lblCurpage.Text = "1";
                this.lblAllpage.Text = "0";
                this.lblRealcount.Text = "0";

                if (dt.Rows.Count.Equals(15))
                {
                    this.lblCurpage.Text = curpage.Value.ToString();
                    this.lblAllpage.Text = allPage.Value.ToString();
                    this.lblRealcount.Text = realCount.Value.ToString();
                }
                else
                {
                    this.lblCurpage.Text = curpage.Value.ToString();
                    this.lblAllpage.Text = allPage.Value.ToString();
                    this.lblRealcount.Text = realCount.Value.ToString();
                }
                this.StorePecArea.DataSource = dt;
                this.StorePecArea.DataBind();
            }
            else
            {
                this.lblCurpage.Text = curpage.Value.ToString();
                this.lblAllpage.Text = allPage.Value.ToString();
                this.lblRealcount.Text = realCount.Value.ToString();
                this.StorePecArea.DataSource = dt;
                this.StorePecArea.DataBind();
                Notice("信息提示", "当前没数据");
                return;
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyArea.aspx-Query", ex.Message+"；"+ex.StackTrace, "Query has an exception");
            }
        }

        /// <summary>
        /// 根据页码得到当前页面的数据
        /// </summary>
        /// <param name="currentPage"></param>
        private void ShowQuery(int currentPage)
        {
            try { 
            int rownum = 15;
            int startNum;
            int endNum;
            //   rownum = GetRowNum();//提取条数
            if (currentPage == 1)
            {
                startNum = 0;
                endNum = rownum;
            }
            else
            {
                startNum = ((currentPage - 1) * rownum);//开始条的序号
                endNum = rownum;//页容量
            }

            Query(GetWhere(), startNum, endNum);
            SetButState(currentPage);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyArea.aspx-ShowQuery", ex.Message+"；"+ex.StackTrace, "ShowQuery has an exception");
            }
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        private void GetData()
        {
            try { 
            DataTable tempdt = tgsDataInfo.GetPeccancyAreaMaxWfsj(GetWhere());
            if (tempdt != null && tempdt.Rows.Count > 0)
            {
                //realMaxTime.Value = tempdt.Rows[0]["col0"].ToString();
                //realMinTime.Value = tempdt.Rows[0]["col1"].ToString();
                realCount.Value = tempdt.Rows[0]["col2"].ToString();//得到总条数
                curpage.Value = 1;//当前页
                // int rownum = GetRowNum();//得到页容量
                int rownum = 15;
                allPage.Value = (int)Math.Ceiling(double.Parse(realCount.Value.ToString()) / rownum);//得到总页数
                ShowQuery(1);
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyArea.aspx-GetData", ex.Message+"；"+ex.StackTrace, "GetData has an exception");
            }
        }

        /// <summary>
        /// 信息提示
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Notice(string title, string msg)
        {
            try { 
            Notification.Show(new NotificationConfig
            {
                Title = title,
                Icon = Icon.Error,
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
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyArea.aspx-Notice", ex.Message+"；"+ex.StackTrace, "Notice has an exception");
            }
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            try { 
            string where = "1=1";

            where = where + " and  wfjssj >= str_to_date('" + starttime + "','%Y-%m-%d %H:%i:%s')   and wfjssj<=str_to_date('" + endtime + "','%Y-%m-%d %H:%i:%s')";
            if (CmbPecType.SelectedIndex != -1)
            {
                where = where + " and  wfxw='" + CmbPecType.SelectedItem.Value + "' ";
            }
            if (CmbPlateType.SelectedIndex != -1)
            {
                where = where + " and  hpzl='" + CmbPlateType.SelectedItem.Value + "' ";
            }
            if (CmbStartStation.SelectedIndex != -1)
            {
                where = where + " and  kskkid='" + CmbStartStation.SelectedItem.Value + "' ";
            }
            if (CmbEndStation.SelectedIndex != -1)
            {
                where = where + " and  jskkid='" + CmbEndStation.SelectedItem.Value + "' ";
            }

            if (CmbDealType.SelectedIndex != -1)
            {
                where = where + " and  jcbj='" + CmbDealType.SelectedItem.Value + "' ";
            }

            if (!string.IsNullOrEmpty(TxtMinSpeed.Text) && string.IsNullOrEmpty(TxtMaxSpeed.Text))
            {
                where = where + " and xssd >=" + TxtMinSpeed.Text + "";
            }
            else if (!string.IsNullOrEmpty(TxtMinSpeed.Text) && !string.IsNullOrEmpty(TxtMaxSpeed.Text))
            {
                if (int.Parse(TxtMinSpeed.Text) < int.Parse(TxtMaxSpeed.Text))
                {
                    where = where + " and xssd >=" + TxtMinSpeed.Text;
                    where = where + " and xssd <=" + TxtMaxSpeed.Text;
                }
                else
                {
                    where = where + " and xssd >=" + TxtMinSpeed.Text;
                }
            }
            string QueryHphm = string.Empty;
            if (!string.IsNullOrEmpty(WindowEditor1.VehicleText))
            {
                QueryHphm = WindowEditor1.VehicleText + TxtplateId.Text;
            }
            else
            {
                QueryHphm = TxtplateId.Text;
            }
            if (ChkLike.Checked)
            {
                if (!string.IsNullOrEmpty(QueryHphm))
                {
                    where = where + " and  hphm  like '%" + QueryHphm.ToUpper() + "%' ";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(QueryHphm))
                {
                    where = where + " and  hphm='" + QueryHphm.ToUpper() + "' ";
                }
            }

            return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyArea.aspx-GetWhere", ex.Message+"；"+ex.StackTrace, "GetWhere has an exception");
            }
            return null;
        }

        #endregion 私有方法
    }
}