using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class MarkerManager : System.Web.UI.Page
    {
        #region 成员变量

        private GisShow gs = new GisShow();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private MapDataOperate mapDataOperate = new MapDataOperate();
        private UserLogin userLogin = new UserLogin();
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
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                DataTable dt = gs.GetStationType(" 1=1 ");
                this.StoreStationType.DataSource = dt;
                this.StoreStationType.DataBind();
                FrmLoad();
                this.DataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "访问：监测点标注", userinfo.NowIp, "0");
            }
        }

        /// <summary>
        /// 监测点 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryStationClick(object sender, DirectEventArgs e)
        {
            try
            {
                GetGisState("STATION_TYPE_ID='" + CmbStationType.SelectedItem.Value + "'");
                string typedesc = string.Empty;
                mapDataOperate.GetIcon(CmbStationType.SelectedItem.Value, ref typedesc);
                SelectMapTo(typedesc);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("MarkerManager.aspx-TbutQueryStationClick", ex.Message+"；"+ex.StackTrace, "TbutQueryStationClick has an exception");
            }
        }

        /// <summary>
        /// 检测点 行选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowSelect(object sender, DirectEventArgs e)
        {
            try
            {
                string sdata = e.ExtraParams["data"];
                string x = Bll.Common.GetdatabyField(sdata, "col3");
                string y = Bll.Common.GetdatabyField(sdata, "col4");
                if (!string.IsNullOrEmpty(x) && !string.IsNullOrEmpty(y))
                {
                    string js = "MovePath('" + x + "','" + y + "');";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("MarkerManager.aspx-RowSelect", ex.Message+"；"+ex.StackTrace, "RowSelect has an exception");
            }
        }

        /// <summary>
        /// 施工占道 行选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowZdSelect(object sender, DirectEventArgs e)
        {
            try
            {
                string sdata = e.ExtraParams["data"];
                string x = Bll.Common.GetdatabyField(sdata, "col30");
                string y = Bll.Common.GetdatabyField(sdata, "col31");
                if (!string.IsNullOrEmpty(x) && !string.IsNullOrEmpty(y))
                {
                    string js = "MovePath('" + x + "','" + y + "');";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("MarkerManager.aspx-RowZdSelect", ex.Message+"；"+ex.StackTrace, "RowZdSelect has an exception");
            }
        }

        /// <summary>
        /// 交通管制 行选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowGZSelect(object sender, DirectEventArgs e)
        {
            try
            {
                string sdata = e.ExtraParams["data"];
                string x = Bll.Common.GetdatabyField(sdata, "col12");
                string y = Bll.Common.GetdatabyField(sdata, "col13");
                if (!string.IsNullOrEmpty(x) && !string.IsNullOrEmpty(y))
                {
                    string js = "MovePath('" + x + "','" + y + "');";
                    this.ResourceManager1.RegisterAfterClientInitScript(js);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("MarkerManager.aspx-RowGZSelect", ex.Message+"；"+ex.StackTrace, "RowGZSelect has an exception");
            }
        }

        #endregion 控件事件

        #region DirectMethod

        /// <summary>
        ///  刷新
        /// </summary>
        [DirectMethod]
        public void Refresh()
        {
            try
            {
                ConstructionLoad();
                TraffLoad();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("MarkerManager.aspx-Refresh", ex.Message+"；"+ex.StackTrace, "Refresh has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        [DirectMethod]
        public void SelectMapTo(string type)
        {
            try
            {
                string js = "BMAP.Clear();";
                this.ResourceManager1.RegisterAfterClientInitScript(js);

                List<string> MapList = mapDataOperate.GetMarkJs(false, type);
                for (int i = 0; i < MapList.Count; i++)
                {
                    this.ResourceManager1.RegisterAfterClientInitScript(MapList[i]);
                }
                ShowClusterer();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("MarkerManager.aspx-SelectMapTo", ex.Message+"；"+ex.StackTrace, "SelectMapTo has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [DirectMethod]
        public void ShowMarkWindow(string x, string y)
        {
            try
            {
                Window win = new Window();
                win.ID = "AddMarkInfo";
                win.Title = GetLangStr("MarkerManager27", "自定义标注信息");
                win.Icon = Icon.Application;
                win.Width = System.Web.UI.WebControls.Unit.Pixel(400);
                win.Height = System.Web.UI.WebControls.Unit.Pixel(190);
                win.Plain = true;
                win.BodyStyle = "padding:6px;";
                win.Collapsible = true;
                win.Layout = "Form";
                win.Plain = true;
                win.Items.Add(CommonExt.AddTextField("txtzbx", GetLangStr("MarkerManager28", "X坐标"), x));
                win.Items.Add(CommonExt.AddTextField("txtzby", GetLangStr("MarkerManager29", "Y坐标"), y));
                win.Items.Add(CommonExt.AddTextField("txtName", GetLangStr("MarkerManager30", "名称"), false, ""));
                win.Buttons.Add(CommonExt.AddButton("butEnter", GetLangStr("MarkerManager31", "确定"), "Accept", "MarkerManager.MarkPointSave()"));
                win.Buttons.Add(CommonExt.AddButton("butCancel", GetLangStr("MarkerManager32", "退出"), "Cancel", win.ClientID + ".hide()"));
                win.Render(this.Form);
                win.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("MarkerManager.aspx-ShowMarkWindow", ex.Message+"；"+ex.StackTrace, "ShowMarkWindow has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        [DirectMethod]
        ///自定义标注
        public void MarkPointSave()
        {
            try
            {
                string x = X.GetCmp<TextField>("txtzbx").Text;
                string y = X.GetCmp<TextField>("txtzby").Text;
                string type = "COM";
                string name = X.GetCmp<TextField>("txtName").Text;
                if (string.IsNullOrEmpty(name))
                {
                    mapDataOperate.Notice(GetLangStr("MarkerManager33", "提示信息"), GetLangStr("MarkerManager34", "请输入标注点名称!"));
                    return;
                }
                Hashtable hs = new Hashtable();

                string id = tgsDataInfo.GetTgsRecordId();
                hs.Add("markid", id);
                hs.Add("xpoint", x);
                hs.Add("ypoint", y);
                hs.Add("marktype", type);
                hs.Add("relationid", id);
                hs.Add("markname", name);

                if (gs.UpdatePointInfo(hs) > 0)
                {
                    mapDataOperate.Notice(GetLangStr("MarkerManager35", "信息提示"), GetLangStr("MarkerManager36", "标注成功"));
                    string mark = "var _id ;var _type; _type='" + type + "'; _id='" + id + "';BMAP.addMarkerContent(true,'../Map/img/" + type + ".gif'," + x + "," + y + ",'" + name + "',{ id: _id, type: _type });";
                    this.ResourceManager1.RegisterAfterClientInitScript(mark);
                    X.GetCmp<Window>("AddMarkInfo").Hide();
                }
                ShowClusterer();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("MarkerManager.aspx-MarkPointSave", ex.Message+"；"+ex.StackTrace, "MarkPointSave has an exception");
            }
        }

        /// <summary>
        /// 显示聚合
        /// </summary>
        private void ShowClusterer()
        {
            string js = " BMAP.ShowMarkerClusterer()  ";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        [DirectMethod]
        public void SaveMarkInfo(string x, string y, string type, string id, string name)
        {
            try
            {
                Hashtable hs = new Hashtable();
                switch (type)
                {
                    case "ZD":
                        hs.Add("id", id);
                        hs.Add("xcoordinate", x);
                        hs.Add("ycoordinate", y);
                        if (gs.UpdataConstructionInfo(hs) > 0)
                        {
                            mapDataOperate.Notice(GetLangStr("MarkerManager35", "信息提示"), GetLangStr("MarkerManager37", "保存成功"));
                            string mark = "var _id ;var _type; _type='" + type + "'; _id='" + id + "';BMAP.addMarkerContent(true,'../Map/img/" + type + ".gif'," + x + "," + y + ",'" + name + "',{ id: _id, type: _type });";
                            this.ResourceManager1.RegisterAfterClientInitScript(mark);
                            ConstructionLoad();
                        }
                        break;

                    case "GZ":
                        hs.Add("id", id);
                        hs.Add("xcoordinate", x);
                        hs.Add("ycoordinate", y);
                        if (gs.UpdataTraffInfo(hs) > 0)
                        {
                            mapDataOperate.Notice(GetLangStr("MarkerManager35", "信息提示"), GetLangStr("MarkerManager38", "标注成功"));
                            string mark = "var _id ;var _type; _type='" + type + "'; _id='" + id + "';BMAP.addMarkerContent(true,'../Map/img/" + type + ".gif'," + x + "," + y + ",'" + name + "',{ id: _id, type: _type });";
                            this.ResourceManager1.RegisterAfterClientInitScript(mark);
                            TraffLoad();
                        }
                        break;

                    default:
                        DataTable da = gs.GetMarkArray(type);
                        string markarray = Convert.ToString(da.Rows.Count + 1);
                        hs.Add("markid", id);
                        hs.Add("xpoint", x);
                        hs.Add("ypoint", y);
                        hs.Add("marktype", type);
                        hs.Add("relationid", id);
                        hs.Add("markname", name);
                        hs.Add("markarray", markarray);

                        if (gs.UpdatePointInfo(hs) > 0)
                        {
                            mapDataOperate.Notice(GetLangStr("MarkerManager35", "信息提示"), GetLangStr("MarkerManager38", "标注成功"));
                            string mark = "var _id ;var _type; _type='" + type + "'; _id='" + id + "';BMAP.addMarkerContent(true,'../Map/img/" + type + ".gif'," + x + "," + y + ",'" + name + "',{ id: _id, type: _type });";
                            this.ResourceManager1.RegisterAfterClientInitScript(mark);
                            TbutQueryStationClick(null, null);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("MarkerManager.aspx-SaveMarkInfo", ex.Message+"；"+ex.StackTrace, "SaveMarkInfo has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        [DirectMethod]
        public void UpdateMarkInfo(string x, string y, string type, string id)
        {
            try
            {
                Hashtable hs = new Hashtable();
                switch (type)
                {
                    case "ZD":
                        hs.Add("id", id);
                        hs.Add("xcoordinate", x);
                        hs.Add("ycoordinate", y);
                        if (gs.UpdataConstructionInfo(hs) > 0)
                        {
                            mapDataOperate.Notice(GetLangStr("MarkerManager35", "信息提示"), GetLangStr("MarkerManager37", "保存成功"));
                        }
                        break;

                    case "GZ":
                        hs.Add("id", id);
                        hs.Add("xcoordinate", x);
                        hs.Add("ycoordinate", y);
                        if (gs.UpdataTraffInfo(hs) > 0)
                        {
                            mapDataOperate.Notice(GetLangStr("MarkerManager35", "信息提示"), GetLangStr("MarkerManager37", "保存成功"));
                        }
                        break;

                    default:
                        hs.Add("markid", id);
                        hs.Add("xpoint", x);
                        hs.Add("ypoint", y);
                        hs.Add("marktype", type);
                        hs.Add("relationid", id);
                        if (gs.UpdatePointInfo(hs) > 0)
                        {
                            mapDataOperate.Notice(GetLangStr("MarkerManager35", "信息提示"), GetLangStr("MarkerManager37", "保存成功"));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("MarkerManager.aspx-UpdateMarkInfo", ex.Message+"；"+ex.StackTrace, "UpdateMarkInfo has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        [DirectMethod]
        public void DeleteMarkInfo(string id, string type)
        {
            try
            {
                Hashtable hs = new Hashtable();
                switch (type)
                {
                    case "ZD":
                        hs.Add("id", id);
                        hs.Add("xcoordinate", "");
                        hs.Add("ycoordinate", "");
                        if (gs.UpdataConstructionInfo(hs) > 0)
                        {
                            mapDataOperate.Notice(GetLangStr("MarkerManager35", "信息提示"), GetLangStr("MarkerManager39", "删除成功"));
                            ConstructionLoad();
                        }
                        break;

                    case "GZ":
                        hs.Add("id", id);
                        hs.Add("xcoordinate", "");
                        hs.Add("ycoordinate", "");
                        if (gs.UpdataTraffInfo(hs) > 0)
                        {
                            mapDataOperate.Notice(GetLangStr("MarkerManager35", "信息提示"), GetLangStr("MarkerManager39", "删除成功"));
                            TraffLoad();
                        }
                        break;

                    default:
                        DataTable dt = new DataTable();
                        hs.Add("relationid", id);
                        hs.Add("marktype", type);
                        if (gs.DeletePointInfo(hs) > 0)
                        {
                            DataTable da = gs.GetMarkArray(type);
                            if (da != null)
                            {
                                for (int i = 0; i < da.Rows.Count; i++)
                                {
                                    Hashtable hes = new Hashtable();
                                    string markid = da.Rows[i][0].ToString();
                                    string markarray = Convert.ToString(i + 1);
                                    if (gs.UpdataMarkArray(markid, markarray) > 0) { }
                                }
                            }
                            mapDataOperate.Notice(GetLangStr("MarkerManager35", "信息提示"), GetLangStr("MarkerManager39", "删除成功"));
                            TbutQueryStationClick(null, null);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("MarkerManager.aspx-DeleteMarkInfo", ex.Message+"；"+ex.StackTrace, "DeleteMarkInfo has an exception");
            }
        }

        #endregion DirectMethod

        #region 私有方法

        /// <summary>
        /// 加载窗体
        /// </summary>
        private void FrmLoad()
        {
            try
            {
                ConstructionLoad();
                TraffLoad();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("MarkerManager.aspx-FrmLoad", ex.Message+"；"+ex.StackTrace, "FrmLoad has an exception");
            }
        }

        /// <summary>
        ///加载施工占道信息
        /// </summary>
        private void ConstructionLoad()
        {
            try
            {
                DataTable dt1 = gs.GetConstructionInfo("1=1");
                this.StoreZd.DataSource = dt1;
                this.StoreZd.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("MarkerManager.aspx-ConstructionLoad", ex.Message+"；"+ex.StackTrace, "ConstructionLoad has an exception");
            }
        }

        /// <summary>
        ///加载交通管制信息
        /// </summary>
        private void TraffLoad()
        {
            try
            {
                DataTable dt2 = gs.GetTraffInfo("1=1");
                this.StoreGz.DataSource = dt2;
                this.StoreGz.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("MarkerManager.aspx-TraffLoad", ex.Message+"；"+ex.StackTrace, "TraffLoad has an exception");
            }
        }

        /// <summary>
        /// 获得监测点 标注状态信息
        /// </summary>
        /// <param name="where"></param>
        private void GetGisState(string where)
        {
            try
            {
                this.StoreState.DataSource = gs.GetGisDeviceList(where);
                this.StoreState.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("MarkerManager.aspx-GetGisState", ex.Message+"；"+ex.StackTrace, "GetGisState has an exception");
            }
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
        #endregion 私有方法
    }
}