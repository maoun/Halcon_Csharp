using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;

// ***********************************************************************
// Assembly         : MyNet.Atmcs.Uscmcp.Web
// Author           : zlsyl
// Created          : 10-20-2016
//
// Last Modified By : zlsyl
// Last Modified On : 10-21-2016
// ***********************************************************************
// <copyright file="PeccancyAreaSetting.aspx.cs" company="ZKLT">
//     Copyright (c) ZKLT. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Data;

/// <summary>
/// The Peccancy namespace.
/// </summary>
namespace MyNet.Atmcs.Uscmcp.Web.Peccancy
{
    /// <summary>
    /// Class PeccancyAreaSetting.
    /// </summary>
    public partial class PeccancyAreaSetting : System.Web.UI.Page
    {
        #region 成员变量

        /// <summary>
        /// The TGS pproperty
        /// </summary>
        private TgsPproperty tgsPproperty = new TgsPproperty();

        /// <summary>
        /// The TGS data information
        /// </summary>
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();

        /// <summary>
        /// The user login
        /// </summary>
        private UserLogin userLogin = new UserLogin();

        /// <summary>
        /// The system identifier
        /// </summary>
        private string SystemID = ((int)MyNet.DataAccess.Model.SystemEnum.EnumSystemType.DeskTop).ToString("00");

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断用户是否登录
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("PeccancyAreaSetting38", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            //判断用户是否登录结束
            if (!X.IsAjaxRequest)
            {
                StoreDataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "" + GetLangStr("PeccancyAreaSetting39", "访问：区间超速配置") + "", userinfo.NowIp, "0");
            }
            this.DataBind();
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            PecSettingDataBind();
        }

        /// <summary>
        ///重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            CmdKskkid.Reset();
            CmdJskkid.Reset();
            PecSettingDataBind();
        }

        /// <summary>
        /// TGSs the refresh.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TgsRefresh(object sender, StoreRefreshDataEventArgs e)
        {
            DataTable data = tgsPproperty.GetEndStationInfo(this.CmdKskkid.SelectedItem.Value);
            StoreEndStation.DataSource = data;
            StoreEndStation.DataBind();
        }

        /// <summary>
        /// Dictionaries the refresh.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void DictRefresh(object sender, StoreRefreshDataEventArgs e)
        {
            DataTable data = tgsPproperty.GetEndStationDict(this.CmbStartStation.SelectedItem.Value);
            StoreEndStationDict.DataSource = data;
            StoreEndStationDict.DataBind();
        }

        #endregion 控件事件

        #region DirectMethod

        /// <summary>
        /// Inserts the pec areasetting.
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void InsertPecAreasetting()
        {
            try
            {
                TxtID.Text = tgsDataInfo.GetTgsRecordId();
                TxtAreaID.Text = TxtID.Text;
                CmbEndStation.Text = "";
                CmbStartStation.Text = "";
                CmbEndStation.SelectedIndex = -1;
                CmbStartStation.SelectedIndex = -1;

                ComDirection.Text = "";
                ComIsPeccancy.Text = "";
                ComDirection.SelectedIndex = -1;
                ComIsPeccancy.SelectedIndex = -1;
                TxtAreaLength.Text = "";
                TxtBS.Text = "";
                TxtSS.Text = "";
                TxtBLS.Text = "";
                TxtSLS.Text = "";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-InsertPecAreasetting", ex.Message + "；" + ex.StackTrace, "InsertPecAreasetting has an exception");
            }
        }

        /// <summary>
        /// Updates the pec areasetting.
        /// </summary>
        /// <returns></returns>
        public void UpdatePecAreasetting(object sender, DirectEventArgs e)
        {
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("xh", TxtID.Text);
                if (CmbStartStation.SelectedIndex != -1)
                {
                    hs.Add("kskkid", CmbStartStation.SelectedItem.Value);
                }
                if (CmbEndStation.SelectedIndex != -1)
                {
                    hs.Add("jskkid", CmbEndStation.SelectedItem.Value);
                }
                hs.Add("qjjl", TxtAreaLength.Text);
                hs.Add("xcqjds", TxtSLS.Text);
                hs.Add("xcqjgs", TxtSS.Text);
                hs.Add("dcqjds", TxtBLS.Text);
                hs.Add("dcqjgs", TxtBS.Text);
                hs.Add("areaid", TxtAreaID.Text);
                if (ComDirection.SelectedIndex != -1)
                {
                    hs.Add("fxbh", ComDirection.SelectedItem.Value);
                }
                if (ComIsPeccancy.SelectedIndex != -1)
                {
                    hs.Add("ispecc", ComIsPeccancy.SelectedItem.Value);
                }
                if (tgsPproperty.UpdateNewPeccancyAreaSetting(hs) > 0)
                {
                    Notice(GetLangStr("PeccancyAreaSetting40", "信息更新"), GetLangStr("PeccancyAreaSetting41", "保存成功"));
                    PecSettingDataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-UpdatePecAreasetting", ex.Message + "；" + ex.StackTrace, "UpdatePecAreasetting has an exception");
            }
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <returns></returns>
        public void DeletePecAreasetting(object sender, DirectEventArgs e)
        {
            try
            {
                RowSelectionModel sm = this.GridPecAreasetting.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    string Id = sm.SelectedRow.RecordID;
                    X.Msg.Confirm(GetLangStr("PeccancyAreaSetting42", "信息"), "" + GetLangStr("PeccancyAreaSetting43", "确认要删除") + "[" + Id + "]" + GetLangStr("PeccancyAreaSetting44", "这条记录吗?") + "", new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "PeccancyAreaSetting.DoYes()",
                            Text = GetLangStr("PeccancyAreaSetting45", "是")
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Handler = "PeccancyAreaSetting.DoNo()",
                            Text = GetLangStr("PeccancyAreaSetting46", "否")
                        }
                    }).Show();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-DeletePecAreasetting", ex.Message + "；" + ex.StackTrace, "DeletePecAreasetting has an exception");
            }
        }

        /// <summary>
        /// Does the yes.
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void DoYes()
        {
            try
            {
                RowSelectionModel sm = this.GridPecAreasetting.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRow.ToBuilder();
                Hashtable hs = new Hashtable();
                string Id = sm.SelectedRow.RecordID;
                hs.Add("xh", Id);
                if (tgsPproperty.DeleteNewPeccancyAreaSetting(hs) > 0)
                {
                    Notice(GetLangStr("PeccancyAreaSetting47", "信息提示"), GetLangStr("PeccancyAreaSetting48", "删除成功"));
                    PecSettingDataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-DoYes", ex.Message + "；" + ex.StackTrace, "DoYes has an exception");
            }
        }

        /// <summary>
        /// Does the no.
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void DoNo()
        {
        }

        #endregion DirectMethod

        #region 私有方法

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
        /// 绑定数据
        /// </summary>
        /// <returns></returns>
        private void StoreDataBind()
        {
            try
            {
                // 绑定查询条件开始卡口
                StoreStartStation.DataSource = tgsPproperty.GetStartStationInfo();
                StoreStartStation.DataBind();
                // 绑定展示 开始卡口
                StoreStartStationDict.DataSource = tgsPproperty.GetStationInfo("a.station_type_id in (02,03)");
                StoreStartStationDict.DataBind();

                this.StoreShow.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240034"));//tgsPproperty.GetCommonDict("240034");
                this.StoreShow.DataBind();
                DataTable dtDirection = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240025"));// tgsPproperty.GetDirectionDict();
                if (dtDirection.Rows[0][0].ToString().Equals("00"))
                {
                    dtDirection.Rows.RemoveAt(0);
                }
                this.StoreDirection.DataSource = dtDirection;
                this.StoreDirection.DataBind();
                DataTable dt = tgsPproperty.GetPeccancyAreaSetting("1=1");
                StorePecAreasetting.DataSource = dt;
                StorePecAreasetting.DataBind();
                if (dt.Rows.Count > 0)
                {
                    SelectFirst(dt.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-DoYes", ex.Message + "；" + ex.StackTrace, "DoYes has an exception");
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
                if (CmdKskkid.SelectedIndex != -1)
                {
                    where = " kskkid  =  '" + CmdKskkid.SelectedItem.Value + "'";
                }
                if (CmdJskkid.SelectedIndex != -1)
                {
                    where = " jskkid  =  '" + CmdJskkid.SelectedItem.Value + "'";
                }
                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PeccancyAreaSetting.aspx-Getwhere", ex.Message + "；" + ex.StackTrace, "Getwhere has an exception");
            }
            return null;
        }

        /// <summary>
        /// Pecs the setting data bind.
        /// </summary>
        /// <returns></returns>
        private void PecSettingDataBind()
        {
            DataTable dt = tgsPproperty.GetPeccancyAreaSetting(Getwhere());
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i]["col5"] = dt.Rows[i]["col5"].ToString() + "Km/h";
            //    dt.Rows[i]["col6"] = dt.Rows[i]["col6"].ToString() + "Km/h";
            //    dt.Rows[i]["col7"] = dt.Rows[i]["col7"].ToString() + "Km/h";
            //    dt.Rows[i]["col8"] = dt.Rows[i]["col8"].ToString() + "Km/h";
            //    dt.Rows[i]["col9"] = dt.Rows[i]["col9"].ToString() + "Km/h";
            //}
            StorePecAreasetting.DataSource = dt;
            StorePecAreasetting.DataBind();
        }

        /// <summary>
        /// Handles the Refresh event of the MyData control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            PecSettingDataBind();
        }

        /// <summary>
        /// Selects the first.
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private void SelectFirst(DataRow dr)
        {
            TxtID.Text = dr["col0"].ToString();
            TxtAreaID.Text = dr["col17"].ToString();
            CmbStartStation.Text = dr["col1"].ToString();
            CmbEndStation.Text = dr["col4"].ToString();
            TxtAreaLength.Text = dr["col5"].ToString();
            TxtBS.Text = dr["col9"].ToString();
            TxtSS.Text = dr["col7"].ToString();
            TxtBLS.Text = dr["col8"].ToString();
            TxtSLS.Text = dr["col6"].ToString();
            ComDirection.Text = dr["col14"].ToString();
            ComIsPeccancy.Text = dr["col16"].ToString();
        }

        /// <summary>
        /// Selects the pec areasetting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void SelectPecAreasetting(object sender, DirectEventArgs e)
        {
            object data = e.ExtraParams["sdata"];
            string sdata = data.ToString();
            TxtID.Text = Bll.Common.GetdatabyField(sdata, "col0");
            TxtAreaID.Text = Bll.Common.GetdatabyField(sdata, "col17");
            CmbStartStation.Text = Bll.Common.GetdatabyField(sdata, "col1");
            CmbEndStation.Text = Bll.Common.GetdatabyField(sdata, "col4");
            TxtAreaLength.Text = Bll.Common.GetdatabyField(sdata, "col5");
            TxtBS.Text = Bll.Common.GetdatabyField(sdata, "col9");
            TxtSS.Text = Bll.Common.GetdatabyField(sdata, "col7");
            TxtBLS.Text = Bll.Common.GetdatabyField(sdata, "col8");
            TxtSLS.Text = Bll.Common.GetdatabyField(sdata, "col6");
            ComDirection.Text = Bll.Common.GetdatabyField(sdata, "col14");
            ComIsPeccancy.Text = Bll.Common.GetdatabyField(sdata, "col16");
        }

        /// <summary>
        /// Notices the specified title.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private void Notice(string title, string msg)
        {
            try
            {
                Notification.Show(new NotificationConfig
                {
                    Title = title,
                    Icon = Icon.Information,
                    HideDelay = 2000,
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
                logManager.InsertLogError("PeccancyAreaSetting.aspx-Notice", ex.Message + "；" + ex.StackTrace, "Notice has an exception");
            }
        }

        #endregion 私有方法
    }
}