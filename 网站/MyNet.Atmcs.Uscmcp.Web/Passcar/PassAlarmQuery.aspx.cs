using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PassAlarmQuery : System.Web.UI.Page
    {
        #region 成员变量

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private UserLogin userLogin = new UserLogin();
        private DataCommon dataCommon = new DataCommon();
        private static string starttime = "";
        private static string endtime = "";
        private SettingManager settingManager = new SettingManager();
        private const string NoImageUrl = "../images/NoImage.png";
        private MapManager bll = new MapManager();

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
            if (!userLogin.CheckLogin(username)) { string js = "alert('" + GetLangStr("PassAlarmQuery41", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }
            if (!IsPostBack)
            {
                if (!X.IsAjaxRequest)
                {
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, GetLangStr("PassAlarmQuery42", "访问：报警信息查询"), userinfo.NowIp, "0");
                    StoreDataBind();
                    DataSetDateTime();
                    //BuildTree(TreeStation.Root);
                    this.DataBind();
                    if (Session["Condition"] != null)
                    {
                        Condition con = Session["Condition"] as Condition;
                        //开始时间
                        start.InnerText = con.StartTime;
                        starttime = con.StartTime;
                        //结束时间
                        end.InnerText = con.EndTime;
                        endtime = con.EndTime;
                        //车牌号牌
                        vehicleHead.SetVehicleText(con.Sqjc);
                        if (con.QueryMode.Equals("0"))
                        {
                            pnhphm.Hidden = false;
                            TxtplateId.Hidden = true;
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
                                TxtplateId.Text = con.Hphm;
                            }
                        }
                        //号牌种类
                        CmbPlateType.Value = con.Hpzl;
                        //卡口
                        kakou.Value = con.Kkidms;
                        kakouId.Value = con.Kkid;
                        //if (con.Kkid.Contains("Bumen"))
                        //{
                        //    List<string> list = new List<string>();
                        //    string[] strs = con.Kkid.Split(',');
                        //    string s = "";
                        //    for (int i = 0; i < strs.Length; i++)
                        //    {
                        //        if (!strs[i].Contains("Bumen"))
                        //        {
                        //            list.Add(strs[i]);
                        //        }
                        //        else
                        //        {
                        //            s = s + strs[i].Substring(5) + ",";
                        //        }
                        //    }
                        //    string str = s + string.Join(",", list.ToArray());
                        //    FieldStation.SetValue(str, con.Kkidms);
                        //}
                        //else
                        //{
                        //    FieldStation.SetValue(con.Kkid, con.Kkidms);
                        //}

                        //车身颜色
                        CmbCsys.Value = con.Csys;
                        //模糊查询
                        if (con.QueryMode == "1")
                        {
                            cktype.Checked = false;
                        }
                        else
                        {
                            cktype.Checked = true;
                        }
                        //车辆品牌
                        //车辆品牌
                        if (con.Clpp.ToString().Contains("-"))
                        {
                            int i = con.Clpp.ToString().IndexOf("-");
                            ClppChoice.Value = con.Clpp.ToString().Substring(1, i - 1);
                        }
                        else
                        {
                            ClppChoice.Value = con.Clpp;
                        }
                        ////车辆品牌
                        //CmbClpp.Value = con.Clpp;
                        //CmbClpp.Text = con.ClppText;
                        //if (!string.IsNullOrEmpty(con.ClppText))
                        //{
                        //    DataSet dsclxh = bll.GetClxh(con.ClppText);
                        //    if (dsclxh != null)
                        //    {
                        //        StoreClzpp.DataSource = dsclxh.Tables[0];
                        //        StoreClzpp.DataBind();
                        //    }
                        //}
                        //else
                        //{
                        //    StoreClzpp.RemoveAll();
                        //    StoreClzpp.DataBind();
                        //}
                        ////车辆子品牌
                        //CmbClzpp.SetValue(con.Clzpp);
                        //CmbClzpp.Text = con.ClzppText;
                        //行驶方向
                        CmbXsfx.Value = con.Xsfx;
                        //车道
                        txtXscd.Text = con.Xscd;
                        txtDsd.Text = con.Dsd;
                        txtGsd.Text = con.Gsd;
                    }
                    //userLogin.IsLoginPage(this);

                    // ButCsv.Disabled = true;
                    ButExcel.Disabled = true;
                    // ButXml.Disabled = true;
                    //ButPrint.Disabled = true;
                    this.FormPanel2.Title = GetLangStr("PassAlarmQuery43", "查询结果：共计查询出符合条件的记录0条,现在显示0条");
                    GetData();
                }
            }
        }

        /// <summary>
        /// 显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowDetails(object sender, DirectEventArgs e)
        {
            try
            {
                string data = e.ExtraParams["data"];
                string field = e.ExtraParams["field"];
                AddWindow(data);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
            //try
            //{
            //    string data = e.ExtraParams["data"];
            //    AddWindow(data);
            //}
            //catch (Exception ex)
            //{
            //    ILog.WriteErrorLog(ex);
            //}
        }

        /// <summary>
        /// 上一页事件
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
            }
        }

        /// <summary>
        ///下一页事件
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
            }
        }

        /// <summary>
        ///首页事件
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
            }
        }

        /// <summary>
        ///尾页事件
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
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                GetData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButPrintClick(object sender, DirectEventArgs e)
        {
            try
            {
                DataTable dt = ChangeDataTable();
                if (dt != null)
                {
                    Session["printdatatable"] = ChangeDataTable();
                    string xml = Bll.Common.GetPrintXml( GetLangStr("PassAlarmQuery44", "报警车辆查询信息列表"), "", "", "printdatatable");
                    string js = "OpenPrintPageH(\"" + xml + "\");";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 导出为xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToXml(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ChangeDataTable();
                ConvertData.ExportXml(dt, this);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 导出为excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToExcel(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ChangeDataTable();
                ConvertData.ExportExcel(dt, this);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 导出为csv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToCsv(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ChangeDataTable();
                ConvertData.ExportCsv(dt, this);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        #endregion 控件事件

        #region 私有方法

        /// <summary>
        ///
        /// </summary>
        /// <param name="sdata"></param>
        private void AddWindow(string sdata)
        {
            try
            {
                Window win = WindowShow.AddAlarmCar(sdata);
                win.Render(this.Form);
                win.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
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
                }
                LabNum.Html = "<font >&nbsp;&nbsp;" + GetLangStr("PassAlarmQuery45", "当前") + page.ToString() + GetLangStr("PassAlarmQuery46", "页,共") + allpage.ToString() +GetLangStr("PassAlarmQuery47","页")+"</font>";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        ///获得一页显示条数
        /// </summary>
        /// <returns></returns>
        private int GetRowNum()
        {
            try
            {
                string rownum = "";

                rownum = "50";

                return int.Parse(rownum);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return 50;
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
                DataTable dt = tgsDataInfo.GetAlarmInfo(where, startNum, endNum);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    dt.Rows[i]["col4"] = Convert.ToDateTime(dt.Rows[i]["col4"]).ToString("yyyy-MM-hh HH:mm:ss");
                //}
                if (dt != null && dt.Rows.Count > 0)
                {
                    ButExcel.Disabled = false;
                }
                else
                {
                    ButExcel.Disabled = true;
                }
                if (Session["datatable"] != null)
                {
                    Session["datatable"] = null;
                }
                Session["datatable"] = dt;
                this.StoreAlarmInfo.DataSource = dt;
                this.StoreAlarmInfo.DataBind();
                int realnum = startNum + dt.Rows.Count;

                if (realnum == 0)
                {
                    this.FormPanel2.Title = GetLangStr("PassAlarmQuery48", "查询结果：共计查询出符合条件的记录") + realCount.Value.ToString() + GetLangStr("PassAlarmQuery49", "条");
                    Notice(GetLangStr("PassAlarmQuery50", "信息提示"), GetLangStr("PassAlarmQuery51", "未查询到相关记录"));
                }
                else
                {
                    this.FormPanel2.Title = GetLangStr("PassAlarmQuery48", "查询结果：共计查询出符合条件的记录") + realCount.Value.ToString() + GetLangStr("PassAlarmQuery52", "条,现在显示") + (startNum + 1).ToString() + " - " + realnum + GetLangStr("PassAlarmQuery49", "条");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 展示选中页数据
        /// </summary>
        /// <param name="currentPage"></param>
        private void ShowQuery(int currentPage)
        {
            try
            {
                int rownum = GetRowNum();
                int startNum = (currentPage - 1) * rownum;
                int endNum = currentPage * rownum;
                Query(GetWhere(), startNum, endNum);
                SetButState(currentPage);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        private void GetData()
        {
            try
            {
                DataTable dt = new DataTable();
                string strwhere = GetWhere();
                DataTable tempdt = tgsDataInfo.GetAlarmMaxBjsj(strwhere);
                if (tempdt != null && tempdt.Rows.Count > 0)
                {
                    realMaxTime.Value = tempdt.Rows[0]["col0"].ToString();
                    realMinTime.Value = tempdt.Rows[0]["col1"].ToString();
                    realCount.Value = tempdt.Rows[0]["col2"].ToString();
                    curpage.Value = 1;
                    int rownum = GetRowNum();
                    allPage.Value = (int)Math.Ceiling(double.Parse(realCount.Value.ToString()) / rownum);
                    ShowQuery(1);
                    //ButCsv.Disabled = false;
                    //ButExcel.Disabled = false;
                    // ButXml.Disabled = false;
                    //ButPrint.Disabled = false;
                }
                else
                {
                    this.FormPanel2.Title = GetLangStr("PassAlarmQuery53", "查询结果：共计查询出符合条件的记录0条");
                    Notice(GetLangStr("PassAlarmQuery50", "信息提示"), GetLangStr("PassAlarmQuery54", "未查询到相关记录"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 提示信息
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

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            string where = "1=1";
            try
            {
                if (Session["Condition"] != null)
                {
                    Condition con = Session["Condition"] as Condition;
                    string kssj = con.StartTime;
                    string jssj = con.EndTime;
                    where = "  bjsj >= STR_TO_DATE('" + kssj + "','%Y-%m-%d %H:%i:%s')   and bjsj<=STR_TO_DATE('" + jssj + "','%Y-%m-%d %H:%i:%s')";
                    if (!string.IsNullOrEmpty(con.Hpzl))
                    {
                        where = where + " and  hpzl='" + con.Hpzl + "' ";
                    }
                    if (!string.IsNullOrEmpty(con.Cjjg))
                    {
                        where = where + " and  cjjg='" + con.Cjjg + "' ";
                    }
                    if (!string.IsNullOrEmpty(con.Xsfx))
                    {
                        where = where + " and  fxbh='" + con.Xsfx + "' ";
                    }
                    if (!string.IsNullOrEmpty(con.Sjly))
                    {
                        where = where + " and  sjly='" + con.Sjly + "' ";
                    }
                    if (!string.IsNullOrEmpty(con.Xscd))
                    {
                        where = where + " and  cdbh='" + con.Xscd + "' ";
                    }
                    if (!string.IsNullOrEmpty(con.Kkid))
                    {
                        where = where + " and  kkid in (" + con.Kkid + ") ";
                    }
                    // 模糊查询
                    if (con.QueryMode.Equals("0"))
                    {
                        if (con.Hphm.Contains("_"))
                        {
                            con.Hphm = con.Hphm.Substring(0, con.Hphm.IndexOf("_"));
                        }
                        if (!string.IsNullOrEmpty(con.Sqjc) && !string.IsNullOrEmpty(con.Hphm))
                        {
                            where = where + " and  hphm like '" + con.Sqjc + con.Hphm.ToUpper() + "%' ";
                        }
                        else if (string.IsNullOrEmpty(con.Sqjc) && !string.IsNullOrEmpty(con.Hphm))
                        {
                            where = where + " and  hphm  like '%" + con.Hphm.ToUpper() + "%' ";
                        }
                    }
                    else// 精确查询
                    {
                        if (!string.IsNullOrEmpty(con.Sqjc) && !string.IsNullOrEmpty(con.Hphm))
                        {
                            where = where + " and  hphm='" + con.Sqjc + con.Hphm.ToUpper() + "' ";
                        }
                        else if (string.IsNullOrEmpty(con.Sqjc) && !string.IsNullOrEmpty(con.Hphm))
                        {
                            where = where + " and  hphm  like '%" + con.Hphm.ToUpper() + "%' ";
                        }
                        else if (!string.IsNullOrEmpty(con.Sqjc) && string.IsNullOrEmpty(con.Hphm))
                        {
                            where = where + " and  hphm  like '" + con.Sqjc + "%' ";
                        }
                    }
                }
                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return where;
            }
        }

        /// <summary>
        /// 保存处理类型
        /// </summary>
        /// <param name="xh"></param>
        [DirectMethod]
        public void SaveClbj(string xh)
        {
            string clbj = X.GetCmp<ComboBox>("cmbClbj").Text;
            string clbjms = X.GetCmp<ComboBox>("cmbClbj").SelectedItem.Text;
            if (string.IsNullOrEmpty(clbj))
            {
                Notice(GetLangStr("PassAlarmQuery63", "提示"), GetLangStr("PassAlarmQuery64", "请选择处理类型"));
            }
            else
            {
                Hashtable hs = new Hashtable();
                UserInfo userinfo = Session["userinfo"] as UserInfo;
                hs.Add("xh", xh);
                hs.Add("clry", userinfo.UserCode);
                hs.Add("clyj", "");
                hs.Add("clbj", clbj);

                if (tgsDataInfo.DealAlarmInfo(hs) > 0)
                {
                    Notice(GetLangStr("PassAlarmQuery50", "信息提示"), GetLangStr("PassAlarmQuery55", "处理完成"));
                    X.GetCmp<TextField>("txtclzt").Text = clbjms;
                }
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

                string url1 = Bll.Common.GetdatabyField(sdata, "col21");
                string url2 = Bll.Common.GetdatabyField(sdata, "col22");
                string url3 = Bll.Common.GetdatabyField(sdata, "col23");
                if (string.IsNullOrEmpty(url2))
                {
                    url2 = NoImageUrl;
                }
                if (string.IsNullOrEmpty(url3))
                {
                    url3 = NoImageUrl;
                }
                string js = "ShowImage(\"" + dataCommon.ChangePoliceIp(url1) + "\",\"" + dataCommon.ChangePoliceIp(url2) + "\",\"" + dataCommon.ChangePoliceIp(url3) + "\",\"" + hphm + "\",\"" + hpzl + "\");";

                //string js = "ShowImage(\"" + url1 + "\",\"" + url2 + "\",\"" + url3 + "\",\"" + hphm + "\",\"" + hpzl + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 转换datatable
        /// </summary>
        /// <returns></returns>
        private DataTable ChangeDataTable()
        {
            DataTable dt = Session["datatable"] as DataTable;
            try
            {
                DataTable dt1 = dt.Copy();
                if (dt1 != null)
                {
                    dt1.Columns.Remove("col0"); dt1.Columns.Remove("col1"); dt1.Columns.Remove("col5"); dt1.Columns.Remove("col8");
                    dt1.Columns.Remove("col10"); dt1.Columns.Remove("col11"); dt1.Columns.Remove("col12"); dt1.Columns.Remove("col13");
                    dt1.Columns.Remove("col14"); dt1.Columns.Remove("col15"); dt1.Columns.Remove("col16"); dt1.Columns.Remove("col17");
                    for (int i = 19; i < dt.Columns.Count; i++)
                    {
                        dt1.Columns.Remove("col" + i.ToString());
                    }
                    dt1.Columns["col9"].SetOrdinal(0); dt1.Columns["col3"].SetOrdinal(1); dt1.Columns["col2"].SetOrdinal(2);//给列排序
                    dt1.Columns["col4"].SetOrdinal(3); dt1.Columns["col6"].SetOrdinal(4); dt1.Columns["col7"].SetOrdinal(5);
                    dt1.Columns["col18"].SetOrdinal(6);

                    dt1.Columns[0].ColumnName = "报警卡口";
                    dt1.Columns[1].ColumnName = "号牌号码";
                    dt1.Columns[2].ColumnName = "号牌种类";
                    dt1.Columns[3].ColumnName = "报警时间";
                    dt1.Columns[4].ColumnName = "报警类型";
                    dt1.Columns[5].ColumnName = "报警原因";
                    dt1.Columns[6].ColumnName = "处理状态";

                    //PrintColumns pc = new PrintColumns();
                    //pc.Add(new PrintColumn("报警卡口", 9));
                    //pc.Add(new PrintColumn("号牌号码", 3));
                    //pc.Add(new PrintColumn("号牌种类", 2));
                    //pc.Add(new PrintColumn("报警时间", 4));
                    //pc.Add(new PrintColumn("行驶方向", 13));
                    //pc.Add(new PrintColumn("车道", 14));
                    //pc.Add(new PrintColumn("报警类型", 6));
                    //pc.Add(new PrintColumn("处理类型", 18));
                    //pc.Add(new PrintColumn("所属机构", 16));
                    //dt2 = Bll.Common.GetDataTablePrint(dt, pc);
                }

                return dt1;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return dt;
            }
        }

        #endregion 私有方法

        #region 智慧查询

        /// <summary>
        /// 清除卡口信息
        /// </summary>
        [DirectMethod]
        public void ClearKakou()
        {
            if (Session["Condition"] != null)
            {
                Condition con = Session["Condition"] as Condition;
                con.Kkid = "";
                con.Kkidms = "";
            }
            if (Session["tree"] != null)
            {
                Session["tree"] = null;
            }
        }

        public static DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        /// 得到符合条件的卡口
        /// </summary>
        [DirectMethod]
        public void GetKakou()
        {
            try
            {
                string value = kakou.Value;

                DataTable dtSelect = null;
                DataTable dt = Session["selectKakou"] as DataTable;//得到整个卡口信息
                DataRow[] rows = dt.Select("STATION_NAME like '" + value + "%'");//选出符合条件的
                if (rows.Length > 0)
                {
                    dtSelect = ToDataTable(rows);
                }
                StringBuilder strs = new StringBuilder();
                strs.AppendLine("[");
                if (dtSelect != null && dtSelect.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSelect.Rows.Count; i++)
                    {
                        if (i == dtSelect.Rows.Count - 1)
                        {
                            strs.AppendLine("{name:'" + dtSelect.Rows[i]["STATION_NAME"] + "',id:'" + dtSelect.Rows[i]["STATION_ID"] + "'}");
                        }
                        else
                        {
                            strs.AppendLine("{name:'" + dtSelect.Rows[i]["STATION_NAME"] + "',id:'" + dtSelect.Rows[i]["STATION_ID"] + "'},");
                        }
                    }
                }
                else
                {
                    strs.AppendLine("{name:'none',id:'none'},");
                }
                strs.AppendLine("]");
                string js = "setUl(" + strs.ToString() + ");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///显示Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetWindow(object sender, EventArgs e)
        {
            this.Window1.Reload();
            Session["stationlist"] = null;
            Session["stationlistname"] = null;
            this.Window1.Show();
        }

        [DirectMethod]
        public void SetSession()
        {
            if (Session["tree"] != null)
            {
                Session["tree"] = null;
            }
            Session["tree"] = kakouId.Value;
        }

        /// <summary>
        ///获取选中时间
        /// </summary>
        /// <param name="isstart"></param>
        /// <param name="strtime"></param>
        [DirectMethod]
        public void GetDateTime(bool isstart, string strtime)
        {
            if (isstart)
                starttime = strtime;
            else
                endtime = strtime;
        }

        [DirectMethod(Namespace = "OnEvl")]
        public void showmap()
        {
            this.Window1.Reload();
            Session["stationlist"] = null;
            Session["stationlistname"] = null;
            this.Window1.Show();
        }

        [DirectMethod(Namespace = "OnEvl")]
        public void hidemap()
        {
            string nodeid = "";
            string nodeName = "";
            this.Window1.Hide();
            if (Session["stationlist"] != null)
            {
                System.Collections.Generic.List<string> listid = Session["stationlist"] as System.Collections.Generic.List<string>;
                foreach (string str in listid)
                {
                    nodeid += (nodeid == "" ? "" : ",") + str;
                }
            }
            if (Session["stationlistname"] != null)
            {
                System.Collections.Generic.List<string> listName = Session["stationlistname"] as System.Collections.Generic.List<string>;
                foreach (string str in listName)
                {
                    nodeName += (nodeName == "" ? "" : ",") + str;
                }
            }
            string js = "setSelect('" + nodeid + "','" + nodeName + "');";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            if (string.IsNullOrEmpty(CmbPlateType.Text) && string.IsNullOrEmpty(TxtplateId.Text) && string.IsNullOrEmpty(vehicleHead.VehicleText) &&
                 string.IsNullOrEmpty(kakou.Value) && string.IsNullOrEmpty(haopai_name1.Value) && string.IsNullOrEmpty(haopai_name2.Value) && string.IsNullOrEmpty(haopai_name3.Value)
                 && string.IsNullOrEmpty(haopai_name4.Value) && string.IsNullOrEmpty(haopai_name5.Value) && string.IsNullOrEmpty(haopai_name6.Value) && cktype.Checked == false
                 )
            {
                DateTime start = Convert.ToDateTime(starttime);
                DateTime end = Convert.ToDateTime(endtime);
                TimeSpan sp = end.Subtract(start);
                if (sp.TotalMinutes > 120)
                {
                    Notice(GetLangStr("PassAlarmQuery50", "信息提示"), GetLangStr("PassAlarmQuery56", "只能选择两个小时之内的时间！"));
                    this.FormPanel2.Title = GetLangStr("PassAlarmQuery57", "查询结果：当前查询出符合条件的记录0条,现在显示0条");
                    LabNum.Html = "<font >&nbsp;&nbsp;"+GetLangStr("PassAlarmQuery58","当前第1页,共0页")+"</font>";
                    StoreAlarmInfo.DataSource = new DataTable();
                    StoreAlarmInfo.DataBind();
                    return;
                }
            }
            if ((!string.IsNullOrEmpty(CmbPlateType.Text) && (!string.IsNullOrEmpty(vehicleHead.VehicleText) && !string.IsNullOrEmpty(TxtplateId.Text))) ||
               (!string.IsNullOrEmpty(CmbPlateType.Text) && cktype.Checked == true)
               )
            {
            }
            else if (!string.IsNullOrEmpty(kakou.Value))
            {
                if (kakou.Value.Contains(","))
                {
                    string[] strs = kakou.Value.Split(',');
                    if (strs.Length > 10)
                    {
                        Notice(GetLangStr("PassAlarmQuery50", "信息提示"), GetLangStr("PassAlarmQuery59", "最多只能选择10个卡口！"));
                        this.FormPanel2.Title = GetLangStr("PassAlarmQuery57", "查询结果：当前查询出符合条件的记录0条,现在显示0条");
                        LabNum.Html = "<font >&nbsp;&nbsp;"+GetLangStr("PassAlarmQuery58","当前第1页,共0页")+"</font>";
                        StoreAlarmInfo.DataSource = new DataTable();
                        StoreAlarmInfo.DataBind();
                        return;
                    }
                }
            }
            else
            {
            }
            if (Session["Condition"] != null)
            {
                Session["Condition"] = null;
            }
            try
            {
                Condition condition = new Condition();
                if (!string.IsNullOrEmpty(starttime))
                {
                    condition.StartTime = DateTime.Parse(starttime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    condition.EndTime = DateTime.Parse(endtime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (!string.IsNullOrEmpty(vehicleHead.VehicleText))
                {
                    condition.Sqjc = vehicleHead.VehicleText;
                }

                if (cktype.Checked)
                {
                    condition.QueryMode = "0";
                    condition.Hphm = (string.IsNullOrEmpty(haopai_name1.Value) ? "_" : haopai_name1.Value) +
                    (string.IsNullOrEmpty(haopai_name2.Value) ? "_" : haopai_name2.Value) +
                    (string.IsNullOrEmpty(haopai_name3.Value) ? "_" : haopai_name3.Value) +
                    (string.IsNullOrEmpty(haopai_name4.Value) ? "_" : haopai_name4.Value) +
                    (string.IsNullOrEmpty(haopai_name5.Value) ? "_" : haopai_name5.Value) +
                    (string.IsNullOrEmpty(haopai_name6.Value) ? "_" : haopai_name6.Value);
                    if (condition.Hphm.Substring(0, 6) == "______")
                        condition.Hphm = "%";
                }
                else
                {
                    condition.QueryMode = "1";
                    if (!string.IsNullOrEmpty(TxtplateId.Text))
                    {
                        condition.Hphm = TxtplateId.Text;
                    }
                }

                if (CmbPlateType.SelectedIndex != -1)
                {
                    condition.Hpzl = CmbPlateType.SelectedItem.Value;
                }
                if (!string.IsNullOrEmpty(ClppChoice.Value))
                {
                    condition.Clpp = ClppChoice.Value;
                }
                //if (CmbClzpp.SelectedIndex != -1)
                //{
                //    condition.Clzpp = CmbClzpp.SelectedItem.Value;
                //    condition.ClzppText = CmbClzpp.SelectedItem.Text;
                //}
                //if (CmbClpp.SelectedIndex != -1)
                //{
                //    condition.Clpp = CmbClpp.SelectedItem.Value;
                //    condition.ClppText = CmbClpp.SelectedItem.Text;
                //    //子品牌赋值
                //    //condition.Clzpp = CmbClzpp.SelectedItem.Value;
                //}

                if (CmbCsys.SelectedIndex != -1)
                {
                    condition.Csys = CmbCsys.SelectedItem.Value;
                }
                if (CmbXsfx.SelectedIndex != -1)
                {
                    condition.Xsfx = CmbXsfx.SelectedItem.Value;
                }
                if (!string.IsNullOrEmpty(txtXscd.Text))
                {
                    condition.Xscd = txtXscd.Text;
                }
                //if (this.FieldStation.Value != null)
                //{
                //    if (this.FieldStation.Value.ToString().Contains("Bumen"))
                //    {
                //        List<string> list = new List<string>();
                //        string[] strs = this.FieldStation.Value.ToString().Split(',');
                //        for (int i = 0; i < strs.Length; i++)
                //        {
                //            if (!strs[i].Contains("Bumen"))
                //            {
                //                list.Add(strs[i]);
                //            }
                //            else
                //            {
                //            }
                //        }
                //        string str = string.Join(",", list.ToArray());
                //        condition.Kkid = str;
                //        condition.Kkidms = this.FieldStation.Text;
                //    }
                //    else
                //    {
                //        string kkid = this.FieldStation.Value.ToString();
                //        if (!string.IsNullOrEmpty(kkid))
                //        {
                //            condition.Kkid = kkid;
                //        }
                //        condition.Kkidms = this.FieldStation.Text;
                //    }
                //}
                if (!string.IsNullOrEmpty(this.kakou.Value))
                {
                    string kkid = this.kakouId.Value.ToString();
                    if (!string.IsNullOrEmpty(kkid))
                    {
                        condition.Kkid = kkid;
                        if (Session["tree"] != null)
                        {
                            Session["tree"] = null;
                        }
                        Session["tree"] = kkid;
                    }
                    condition.Kkidms = this.kakou.Value;
                }
                //得到低速度
                if (!string.IsNullOrEmpty(txtDsd.Text))
                {
                    condition.Dsd = txtDsd.Text;
                }
                //得到高速度
                if (!string.IsNullOrEmpty(txtGsd.Text))
                {
                    condition.Gsd = txtGsd.Text;
                }
                ////短车长
                //if (!string.IsNullOrEmpty(TextField3.Text))
                //{
                //    condition.Dcc = TextField3.Text;
                //}
                ////长车长
                //if (!string.IsNullOrEmpty(TextField4.Text))
                //{
                //    condition.Ccc = TextField4.Text;
                //}
                Session["Condition"] = condition;
                GetData();
                // QueryPasscar(0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            //把session中的条件设置为空
            if (Session["Condition"] != null)
            {
                Session["Condition"] = null;
            }
            starttime = DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss");
            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeJs = "clearTime('" + starttime + "','" + endtime + "');";//js方法后面的分号一定要加上
            this.ResourceManager1.RegisterAfterClientInitScript(timeJs);
            //车牌号牌
            vehicleHead.SetVehicleText("");
            TxtplateId.Text = "";
            haopai_name1.Value = "";
            haopai_name2.Value = "";
            haopai_name3.Value = "";
            haopai_name5.Value = "";
            haopai_name4.Value = "";
            haopai_name6.Value = "";
            //号牌种类
            CmbPlateType.Text = "";
            //卡口列表
            if (!string.IsNullOrEmpty(kakou.Value))//判断卡口是否为空
            {
                string js = "clearMenu();";
                if (Session["tree"] != null)
                {
                    Session["tree"] = null;
                }
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            //车身颜色
            CmbCsys.Text = "";
            //模糊查询
            cktype.Checked = false;
            ////车辆品牌
            //CmbClpp.Text = "";
            ////车辆子品牌
            //CmbClzpp.Text = "";
            //车辆品牌
            ClppChoice.Value = "";
            clzpp.Value = "";
            clpp.Value = "";
            //行驶方向
            CmbXsfx.Text = "";
            //车速  低速度与高速度
            txtDsd.Text = "";
            txtGsd.Text = "";
            ////车长区间
            //TextField3.Text = "";//短
            //TextField4.Text = "";//长
            //车道
            txtXscd.Text = "";
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private Condition GetQueryInfo()
        {
            try
            {
                Condition condition = new Condition();
                if (!string.IsNullOrEmpty(starttime))
                {
                    condition.StartTime = DateTime.Parse(starttime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    condition.EndTime = DateTime.Parse(endtime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (!string.IsNullOrEmpty(vehicleHead.VehicleText))
                {
                    condition.Sqjc = vehicleHead.VehicleText;
                }
                if (cktype.Checked)
                {
                    condition.QueryMode = "0";
                    condition.Hphm = (string.IsNullOrEmpty(haopai_name1.Value) ? "_" : haopai_name1.Value) +
                    (string.IsNullOrEmpty(haopai_name2.Value) ? "_" : haopai_name2.Value) +
                    (string.IsNullOrEmpty(haopai_name3.Value) ? "_" : haopai_name3.Value) +
                    (string.IsNullOrEmpty(haopai_name4.Value) ? "_" : haopai_name4.Value) +
                    (string.IsNullOrEmpty(haopai_name5.Value) ? "_" : haopai_name5.Value) +
                    (string.IsNullOrEmpty(haopai_name6.Value) ? "_" : haopai_name6.Value);
                    //if (condition.Hphm.Substring(0, 6) == "______")
                    //    condition.Hphm = "%";
                }
                else
                {
                    condition.QueryMode = "1";
                    if (!string.IsNullOrEmpty(TxtplateId.Text))
                    {
                        condition.Hphm = TxtplateId.Text;
                    }
                }

                if (CmbPlateType.SelectedIndex != -1)
                {
                    condition.Hpzl = CmbPlateType.SelectedItem.Value;
                }
                //if (CmbClzpp.SelectedIndex != -1)
                //{
                //    condition.Clzpp = CmbClzpp.SelectedItem.Value;
                //}
                //if (CmbClpp.SelectedIndex != -1)
                //{
                //    condition.Clpp = CmbClpp.SelectedItem.Value;
                //}
                if (!string.IsNullOrEmpty(ClppChoice.Value))
                {
                    condition.Clpp = ClppChoice.Value;
                }
                if (CmbCsys.SelectedIndex != -1)
                {
                    condition.Csys = CmbCsys.SelectedItem.Value;
                }
                if (CmbXsfx.SelectedIndex != -1)
                {
                    condition.Xsfx = CmbXsfx.SelectedItem.Value;
                }
                if (!string.IsNullOrEmpty(txtXscd.Text))
                {
                    condition.Xscd = txtXscd.Text;
                }
                if (!string.IsNullOrEmpty(this.kakou.Value))
                {
                    string kkid = this.kakouId.Value.ToString();
                    if (!string.IsNullOrEmpty(kkid))
                    {
                        condition.Kkid = kkid;
                        if (Session["tree"] != null)
                        {
                            Session["tree"] = null;
                        }
                        Session["tree"] = kkid;
                    }
                    condition.Kkidms = this.kakou.Value;
                }
                return condition;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 添加卡口子节点
        /// </summary>
        /// <param name="root"></param>
        private void AddStationTree(Ext.Net.TreeNode DepartNode, DataRow[] rows)
        {
            try
            {
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count(); i++)
                    {
                        Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                        node.Text = rows[i]["station_name"].ToString();
                        node.Leaf = true;
                        node.Checked = ThreeStateBool.False;
                        node.NodeID = rows[i]["station_id"].ToString();
                        node.Draggable = false;
                        node.AllowDrag = false;
                        node.Qtip = "Kakou";
                        DepartNode.Nodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 组件卡口列表树
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private Ext.Net.TreeNodeCollection BuildTree(Ext.Net.TreeNodeCollection nodes)
        {
            try
            {
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }

                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Text = GetLangStr("PassAlarmQuery61", "卡口列表");
                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;

                // 添加 自己机构节点 和卡口
                UserInfo user = Session["userinfo"] as UserInfo;
                if (user == null)
                {
                    user = new UserInfo();
                    user.DepartName = GetLangStr("PassAlarmQuery62", "滨州市交通警察支队");
                    user.DeptCode = "371600000000";
                }

                Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                nodeRoot.Text = user.DepartName;
                nodeRoot.Leaf = true;
                nodeRoot.NodeID = user.DeptCode;
                nodeRoot.Icon = Ext.Net.Icon.House;
                nodeRoot.Checked = ThreeStateBool.False;//加的部门选择框
                nodeRoot.Qtip = "Bumen";
                DataTable dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
                DataRow[] rowsStation = dtStation.Select("departid='" + user.DeptCode + "'", "station_name asc");
                AddStationTree(nodeRoot, rowsStation);

                nodeRoot.Expanded = false;
                nodeRoot.Draggable = true;
                nodeRoot.Expandable = ThreeStateBool.True;
                root.Nodes.Add(nodeRoot);

                //绑定下级部门及下级部门卡口
                AddDepartTree(root, user.DeptCode);

                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///绑定下级部门及下级部门卡口
        /// </summary>
        /// <param name="root"></param>
        private void AddDepartTree(Ext.Net.TreeNode root, string departCode)
        {
            try
            {
                DataTable dtDepart = GetRedisData.GetData("t_cfg_department");
                DataRow[] rows = dtDepart.Select("classcode='" + departCode + "'");
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count(); i++)
                    {
                        Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                        nodeRoot.Text = rows[i]["departname"].ToString();
                        nodeRoot.Leaf = true;
                        nodeRoot.NodeID = rows[i]["departid"].ToString();
                        nodeRoot.Icon = Icon.House;
                        nodeRoot.Checked = ThreeStateBool.False;//加的部门选择框
                        nodeRoot.Qtip = "Bumen";
                        DataTable dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
                        DataRow[] rowsStation = dtStation.Select(" departid='" + nodeRoot.NodeID + "' ", "station_name asc");
                        AddStationTree(nodeRoot, rowsStation);
                        nodeRoot.Expanded = false;
                        nodeRoot.Draggable = true;
                        nodeRoot.Expandable = ThreeStateBool.True;
                        AddDepartTree(nodeRoot, rows[i]["departid"].ToString());
                        root.Nodes.Add(nodeRoot);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 设置初始时间
        /// </summary>
        private void DataSetDateTime()
        {
            starttime = DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss");
            start.InnerText = starttime;

            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            end.InnerText = endtime;
        }

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                starttime = "";
                endtime = "";
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

                this.storekk.DataSource = tgsPproperty.GetAllStationInfo();
                this.storekk.DataBind();

                DataTable dt2 = GetRedisData.GetData("t_sys_code:240022");
                if (dt2 != null)
                {
                    this.StoreDataSource.DataSource = Bll.Common.ChangColName(dt2);// tgsPproperty.GetDeviceTypeDict("240022");
                    this.StoreDataSource.DataBind();
                }
                else
                {
                    this.StoreDataSource.DataSource = tgsPproperty.GetDeviceTypeDict("240022");
                    this.StoreDataSource.DataBind();
                }

                DataTable dt3 = GetRedisData.GetData("t_sys_code:240013");
                //车身颜色
                if (dt3 != null)
                {
                    this.StoreCsys.DataSource = Bll.Common.ChangColName(dt3);
                    this.StoreCsys.DataBind();
                }
                else
                {
                    this.StoreCsys.DataSource = tgsPproperty.GetCarColorDict();
                    this.StoreCsys.DataBind();
                }
                DataTable dt4 = GetRedisData.GetData("t_sys_code:240025");
                //行驶方向
                if (dt4 != null)
                {
                    this.StoreXsfx.DataSource = Bll.Common.ChangColName(dt4);
                    this.StoreXsfx.DataBind();
                }
                else
                {
                    this.StoreXsfx.DataSource = tgsPproperty.GetDirectionDict();
                    this.StoreXsfx.DataBind();
                }
                DataTable dt5 = GetRedisData.GetData("t_cfg_department");
                if (dt5 != null)
                {
                    this.StoreCjjg.DataSource = Bll.Common.ChangColName(dt5);//tgsPproperty.GetDepartmentDict();
                    this.StoreCjjg.DataBind();
                }
                else
                {
                    this.StoreCjjg.DataSource = Bll.Common.ChangColName(tgsPproperty.GetDepartmentDict());
                    this.StoreCjjg.DataBind();
                }
                DataTable dt6 = GetRedisData.GetData("t_sys_code:421300");
                if (dt6 != null)
                {
                    this.StoreClbj.DataSource = Bll.Common.ChangColName(dt6);
                    this.StoreClbj.DataBind();
                }

                //this.StoreQuery.DataSource = GetDataSource();
                //this.StoreQuery.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;

            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值

                if (json.Substring(0, 1) != "[")
                    json = json.Substring(1, json.Length - 2);
                json = json.Replace("\\", "");
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
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
        /// 转换查询模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void changtype(object sender, EventArgs e)
        {
            TxtplateId.Hidden = cktype.Checked;
            pnhphm.Hidden = !cktype.Checked;
        }

        #endregion 智慧查询
    }
}