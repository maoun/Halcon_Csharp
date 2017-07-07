using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PeccancyType : System.Web.UI.Page
    {
        #region 成员变量

        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();
        private string SystemID = ((int)MyNet.DataAccess.Model.SystemEnum.EnumSystemType.DeskTop).ToString("00");
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
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
            if (!userLogin.CheckLogin(username)) { string js = "alert('" + GetLangStr("PeccancyType6", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }
            if (!X.IsAjaxRequest)
            {
                //userLogin.IsLoginPage(this);
                StoreDataBind();
                this.DataBind();
                TbutQueryClick(null, null);
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("PeccancyType16", "访问：违法类型管理"), userinfo.NowIp, "0");
            }
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                PecTypeDataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyType.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");
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
                logManager.InsertLogError("PeccancyType.aspx-TbutFisrt", ex.Message+"；"+ex.StackTrace, "TbutFisrt has an exception");
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
                logManager.InsertLogError("PeccancyType.aspx-TbutLast", ex.Message+"；"+ex.StackTrace, "TbutLast has an exception");
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
                logManager.InsertLogError("PeccancyType.aspx-TbutNext", ex.Message+"；"+ex.StackTrace, "TbutNext has an exception");
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
                logManager.InsertLogError("PeccancyType.aspx-TbutEnd", ex.Message+"；"+ex.StackTrace, "TbutEnd has an exception");
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
                logManager.InsertLogError("PeccancyType.aspx-ShowQuery", ex.Message+"；"+ex.StackTrace, "ShowQuery has an exception");
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
                DataTable dt = tgsPproperty.GetPeccancyTypeNew(Getwhere(), startNum, endNum);
                if (dt != null)
                {
                    StorePecType.DataSource = dt;
                    StorePecType.DataBind();
                }

                if (dt != null && dt.Rows.Count > 0)
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
                    Notice(GetLangStr("PeccancyType7", "信息提示"), GetLangStr("PeccancyType8", "未查询到相关记录"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyType.aspx-Query", ex.Message+"；"+ex.StackTrace, "Query has an exception");
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
                TxtPecCode.Reset();
                TxtPecType.Reset();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyType.aspx-ButResetClick", ex.Message+"；"+ex.StackTrace, "ButResetClick has an exception");
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
                PecTypeDataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyType.aspx-MyData_Refresh", ex.Message+"；"+ex.StackTrace, "MyData_Refresh has an exception");
            }
        }

        #endregion 控件事件

        #region DirectMethod

        /// <summary>
        /// 插入违法类型
        /// </summary>
        [DirectMethod]
        public void InsertPecType()
        {
            TxtID.Text = tgsDataInfo.GetTgsRecordId();
            Txtwfbh.Text = "";
            Txtwfxw.Text = "";
            Txtcfyj.Text = "";
            Txtwfjc.Text = "";
            Txtwfkf.Text = "";
            Txtfkje.Text = "";
        }

        /// <summary>
        /// 更新违法类型
        /// </summary>
        [DirectMethod]
        public void UpdatePecType()
        {
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("xh", TxtID.Text);
                hs.Add("wfxw", Txtwfbh.Text);
                hs.Add("wfxwms", Txtwfxw.Text);
                hs.Add("yj", Txtcfyj.Text);
                hs.Add("kf", Txtwfkf.Text);
                hs.Add("fkje", Txtfkje.Text);
                hs.Add("wfxwjc", Txtwfjc.Text);
                hs.Add("isuse", cmbUse.SelectedItem.Value);
                if (tgsPproperty.UpdatePeccancyType(hs) > 0)
                {
                    Notice(GetLangStr("PeccancyType27", "信息更新"), GetLangStr("PeccancyType28", "违法信息保存成功"));
                    PecTypeDataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyType.aspx-UpdatePecType", ex.Message+"；"+ex.StackTrace, "UpdatePecType has an exception");
            }
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        [DirectMethod]
        public void DeletePecType()
        {
            try
            {
                RowSelectionModel sm = this.GridPecType.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string Id = sm.SelectedRow.RecordID;
                    X.Msg.Confirm(GetLangStr("PeccancyType29", "信息"), GetLangStr("PeccancyType30", "确认要删除这条记录吗?"), new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "PeccancyType.DoYes()",
                            Text = GetLangStr("PeccancyType32", "是")
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "PeccancyType.DoNo()",
                            Text = GetLangStr("PeccancyType33", "否")
                        }
                    }).Show();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyType.aspx-DeletePecType", ex.Message+"；"+ex.StackTrace, "DeletePecType has an exception");
            }
        }

        /// <summary>
        ///确认删除事件
        /// </summary>
        [DirectMethod]
        public void DoYes()
        {
            try
            {
                RowSelectionModel sm = this.GridPecType.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRow.ToBuilder();
                Hashtable hs = new Hashtable();
                string Id = sm.SelectedRow.RecordID;
                hs.Add("xh", Id);
                if (tgsPproperty.DeletePeccancyType(hs) > 0)
                {
                    Notice(GetLangStr("PeccancyType7", "信息提示"), GetLangStr("PeccancyType35", "删除成功"));
                    PecTypeDataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyType.aspx-DoYes", ex.Message+"；"+ex.StackTrace, "DoYes has an exception");
            }
        }

        /// <summary>
        /// 不删除事件
        /// </summary>
        [DirectMethod]
        public void DoNo()
        {
        }

        #endregion DirectMethod

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
                logManager.InsertLogError("PeccancyType.aspx-SetButState", ex.Message+"；"+ex.StackTrace, "SetButState has an exception");
            }
        }

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                DataTable da = tgsPproperty.GetCommonDict("240034");
                if (da != null)
                {
                    this.StoreUseType.DataSource = da;
                    this.StoreUseType.DataBind();
                }

                this.cmbUse.SelectedIndex = 1;
                DataTable dt = tgsPproperty.GetPeccancyType();
                if (dt != null)
                {
                    StorePecType.DataSource = dt;
                    StorePecType.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        SelectFirst(dt.Rows[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyType.aspx-StoreDataBind", ex.Message+"；"+ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 组装查询条件
        /// </summary>
        /// <returns></returns>
        private string Getwhere()
        {
            try
            {
                string where = "1=1";
                if (!string.IsNullOrEmpty(TxtPecCode.Text))
                {
                    where = where + " and wfxw  like  '%" + TxtPecCode.Text + "%'";
                }
                if (!string.IsNullOrEmpty(TxtPecType.Text))
                {
                    where = where + " and wfxwms  like  '%" + TxtPecType.Text + "%'";
                }
                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyType.aspx-Getwhere", ex.Message+"；"+ex.StackTrace, "Getwhere has an exception");
                return "";
            }
        }

        /// <summary>
        /// 初始化加载数据
        /// </summary>
        private void PecTypeDataBind()
        {
            try
            {
                string rownum = "15";
                DataTable dtCount = tgsPproperty.GetPeccancyTypeCount(Getwhere()); ;//获得总记录
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
                    Notice(GetLangStr("PeccancyType7", "信息提示"), GetLangStr("PeccancyType8", "未查询到相关记录"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyType.aspx-PecTypeDataBind", ex.Message+"；"+ex.StackTrace, "PecTypeDataBind has an exception");
            }
        }

        /// <summary>
        /// 显示第一行数据
        /// </summary>
        /// <param name="dr"></param>
        private void SelectFirst(DataRow dr)
        {
            try
            {
                TxtID.Text = dr["col0"].ToString();
                Txtwfbh.Text = dr["col1"].ToString();
                Txtwfxw.Text = dr["col3"].ToString();
                Txtcfyj.Text = dr["col4"].ToString();
                Txtwfjc.Text = dr["col7"].ToString();
                Txtwfkf.Text = dr["col5"].ToString();
                Txtfkje.Text = dr["col6"].ToString();
                cmbUse.SelectedItem.Value = dr["col9"].ToString();
                cmbUse.SelectedItem.Text = dr["col10"].ToString();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyType.aspx-SelectFirst", ex.Message+"；"+ex.StackTrace, "SelectFirst has an exception");
            }
        }

        /// <summary>
        ///选中行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectPecType(object sender, DirectEventArgs e)
        {
            try
            {
                object data = e.ExtraParams["sdata"];
                string sdata = data.ToString();
                TxtID.Text = Bll.Common.GetdatabyField(sdata, "col0");
                Txtwfbh.Text = Bll.Common.GetdatabyField(sdata, "col1");
                Txtwfxw.Text = Bll.Common.GetdatabyField(sdata, "col3");
                Txtcfyj.Text = Bll.Common.GetdatabyField(sdata, "col4");
                Txtwfjc.Text = Bll.Common.GetdatabyField(sdata, "col7");
                Txtwfkf.Text = Bll.Common.GetdatabyField(sdata, "col5");
                Txtfkje.Text = Bll.Common.GetdatabyField(sdata, "col6");
                cmbUse.SelectedItem.Text = Bll.Common.GetdatabyField(sdata, "col10");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyType.aspx-SelectPecType", ex.Message+"；"+ex.StackTrace, "SelectPecType has an exception");
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

        #endregion 私有方法
    }
}