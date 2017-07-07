using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.Script.Serialization;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;
namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class GisRoadBrowse : System.Web.UI.Page
    {
        #region 成员变量

        public int count = 0;
        private DataTable dsquery = new DataTable();
        private RoadManager roadManager = new RoadManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();
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
                string js = "alert('" + GetLangStr("GisRoadBrowse31", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'"; 
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); 
                return; 
            }
            if (!X.IsAjaxRequest)
            {
                FrmLoad();
                this.DataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("GisRoadBrowse43", "访问：道路管理"), userinfo.NowIp, "0");

            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butQueryClick(object sender, DirectEventArgs e)
        {
            clearMapLine();
            queryShow();
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
            clearMapLine();
            List<string> rordIds = new List<string>();
            RowSelectionModel sm = this.GridRoadManager.SelectionModel.Primary as RowSelectionModel;

            foreach (SelectedRow row in sm.SelectedRows)
            {
                rordIds.Add(row.RecordID);
            }
            ShowPoints(rordIds);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("GisRoadBrowse.aspx-ApplyClick", ex.Message+"；"+ex.StackTrace, "ApplyClick has an exception");
            }
        }

        /// <summary>
        /// 描线
        /// </summary>
        /// <param name="MapList"></param>
        /// <param name="LineNameList"></param>
        private void DrawLine(List<string> MapList, List<string> LineNameList)
        {
            try
            {

           
            clearMapLine();
            for (int i = 0; i < MapList.Count; i++)
            {
                string js = "BMAP.addPolyline2('#0000ff','" + MapList[i] + "','" + LineNameList[i] + "');";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("GisRoadBrowse.aspx-DrawLine", ex.Message+"；"+ex.StackTrace, "DrawLine has an exception");
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void butResetClick(object sender, DirectEventArgs e)
        {
            this.uiDepartment.DepertName = "";
            this.cboRoadType.Text = "";
            this.txtRoadName.Text = "";
            this.ResourceManager1.RegisterAfterClientInitScript("clearTime();");
        }

        #endregion 控件事件

        #region DirectMethod

        /// <summary>
        /// 新建道路
        /// </summary>
        /// <param name="points"></param>
        [DirectMethod]
        public void AddRoadPoints(string points)
        {
            try
            {

           
            //this.txtDLBHwin.Visible = false;
            CheckData.Value = points;
            Session["roadpoints"] = points;
            Session["isedit"] = "false";
            btnSave.Text = GetLangStr("GisRoadBrowse29", "保存");
            this.txtDLBHwin.Reset();
            this.txtRoadIDwin.Reset();
            // this.uiDepartment.DepertName = "";//所属辖区
            this.uiDepartment1.Reset();
            this.cboRoadTypewin.Reset();
            this.txtRoadNamewin.Reset();
            //this.txtDLBHwin.Text = "";
            win.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("GisRoadBrowse.aspx-AddRoadPoints", ex.Message+"；"+ex.StackTrace, "AddRoadPoints has an exception");
            }
        }

        /// <summary>
        /// 修改道路
        /// </summary>
        /// <param name="points"></param>
        [DirectMethod]
        public void UpdataPoints(string points)
        {
            try { 
            roadid = Session["ROADID"].ToString();
            if (roadManager.UpdateXyz(GetPointXY(points), roadid))
            {
                Notice(GetLangStr("GisRoadBrowse32", "信息提示"), GetLangStr("GisRoadBrowse33", "重绘成功！"));
                reShowData();
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("GisRoadBrowse.aspx-UpdataPoints", ex.Message+"；"+ex.StackTrace, "UpdataPoints has an exception");
            }
        }

        /// <summary>
        /// 编辑道路
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="roadname"></param>
        [DirectMethod(Namespace = "OnEvl")]
        public void Editor(string type, string id, string roadname)
        {

            try
            {

           
            if (type == "Edit")
            {
                Session["isedit"] = "true";
                MyNet.DataAccess.Model.RoadInfo model = roadManager.GetModel(id);
                if (model != null)
                {
                    this.txtRoadIDwin.Text = id;
                    Session["ROADID"] = id;
                    btnSave.Text = GetLangStr("GisRoadBrowse34", "修改");
                    win.Show();
                }
                initwinedit(id);
            }
            if (type == "UpdateRoadLine")
            {
                Session["ROADID"] = id;
                string js = " BMAP.AddRoadPoint({Operate:'UpdateRoadLine'});;";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }

            if (type == "Delete")
            {
                X.Msg.Confirm(GetLangStr("GisRoadBrowse35", "提示"), GetLangStr("GisRoadBrowse36", "是否删除道路信息"), new MessageBoxButtonsConfig
                {
                    Yes = new MessageBoxButtonConfig
                    {
                        Handler = "OnEvl.DelYes('" + id + "')",
                        Text = GetLangStr("GisRoadBrowse37", "是")
                    },
                    No = new MessageBoxButtonConfig
                    {
                        Text = GetLangStr("GisRoadBrowse38", "否")
                    }
                }).Show();
            }
            }
            catch (Exception ex)
            {


                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("GisRoadBrowse.aspx-Editor", ex.Message+"；"+ex.StackTrace, "Editor has an exception");
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="roadid"></param>
        [DirectMethod(Namespace = "OnEvl")]
        public void DelYes(string roadid)
        {
            if (roadManager.Delete(roadid))
            {
                Notice(GetLangStr("GisRoadBrowse32", "信息提示"), GetLangStr("GisRoadBrowse39", "删除成功"));
                reShowData();
            }
        }

        #endregion DirectMethod

        #region 私有方法

        /// <summary>
        /// 初始化加载
        /// </summary>
        private void FrmLoad()
        {
            InitRoadType();
            InitDepart();
        }

        /// <summary>
        /// 初始化道路类型
        /// </summary>
        private void InitRoadType()
        {
            try { 
            DataTable dt = GetRedisData.GetData("t_sys_code:350100");// roadManager.GetRoadType();
            if (dt != null)
            {
                this.RoadType.DataSource = dt;
                this.RoadType.DataBind();
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("GisRoadBrowse.aspx-InitRoadType", ex.Message+"；"+ex.StackTrace, "InitRoadType has an exception");
            }
        }

        /// <summary>
        /// 初始化辖区
        /// </summary>
        private void InitDepart()
        {
            try { 
            DataTable dt = GetRedisData.GetData("t_cfg_department"); // roadManager.GetDepartment();
            if (dt != null)
            {
                this.Department.DataSource = dt;
                this.Department.DataBind();
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("GisRoadBrowse.aspx-InitDepart", ex.Message+"；"+ex.StackTrace, "InitDepart has an exception");
            }
        }

        /// <summary>
        /// 批量定位
        /// </summary>
        /// <param name="roadList"></param>
        private void ShowPoints(List<string> roadList)
        {

            try
            {

           
            List<string> MapList = new List<string>();
            List<string> DlmcList = new List<string>();
            string where = "";

            if (roadList.Count > 0)
            {
                string id = string.Empty;
                for (int j = 0; j < roadList.Count; j++)
                {
                    id = id + "'" + roadList[j] + "',";
                }
                id = id.Substring(0, id.Length - 1);
                where = where + "  roadid  in (" + id + ")";
                DataTable ds = roadManager.GetRoadPoints(where);
                string roadid = "";
                string points = "";
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    if (roadid == "" || roadid == ds.Rows[i]["ROADID"].ToString())
                    {
                        if (points == "")
                            DlmcList.Add(ds.Rows[i]["DLMC"].ToString());
                        points += (points == "" ? "" : "|") + ds.Rows[i]["XPOS"].ToString() + "," + ds.Rows[i]["YPOS"].ToString();
                    }
                    else
                    {
                        MapList.Add(points);
                        DlmcList.Add(ds.Rows[i]["DLMC"].ToString());
                        points = ds.Rows[i]["XPOS"].ToString() + "," + ds.Rows[i]["YPOS"].ToString();
                    }
                    roadid = ds.Rows[i]["ROADID"].ToString();
                }
                if (points != "")
                    MapList.Add(points);
                DrawLine(MapList, DlmcList);
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("GisRoadBrowse.aspx-ShowPoints", ex.Message+"；"+ex.StackTrace, "ShowPoints has an exception");
            }
        }

        /// <summary>
        /// 清空地图描点
        /// </summary>
        private void clearMapLine()
        {
            try { 
            string js = " BMAP.ClearLine();";
            this.ResourceManager1.RegisterAfterClientInitScript(js);

            js = " BMAP.ClearCircle();";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("GisRoadBrowse.aspx-clearMapLine", ex.Message+"；"+ex.StackTrace, "clearMapLine has an exception");
            }
        }

        /// <summary>
        /// 删除结果显示
        /// </summary>
        private void queryShow()
        {

            try
            {

          
            GetQueryData();


            if (dsquery != null && dsquery.Rows.Count>0)
           // if(!string.IsNullOrEmpty(dsquery.ToString()))
            {
                this.StoreInfo.DataSource = dsquery;
                this.StoreInfo.DataBind();
              
            }
            else
            {
                //this.StoreInfo.ClearMeta();
                this.StoreInfo.DataSource = new DataTable();
                this.StoreInfo.DataBind();
                Notice(GetLangStr("GisRoadBrowse32", "信息提示"), GetLangStr("GisRoadBrowse42", "未查询到数据"));
            }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("GisRoadBrowse.aspx-ShowPoints", ex.Message+"；"+ex.StackTrace, "ShowPoints has an exception");
            }
        }

        /// <summary>
        /// 获取查询数据集
        /// </summary>
        private void GetQueryData()
        {
            try
            {

           
            string where = "";
            if (this.cboRoadType.Text.Trim() != "")
                where = " DLLX='" + this.cboRoadType.Text + "'";
            if (uiDepartment.DepertId.Trim() != "")
                where += (where == "" ? "" : " and ") + " SSXQ='" + uiDepartment.DepertId + "'";

            if (this.txtRoadName.Text.Trim() != "")
                where += (where == "" ? "" : " and ") + " DLMC='" + this.txtRoadName.Text + "'";
            dsquery = roadManager.GetList(where);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("GisRoadBrowse.aspx-GetQueryData", ex.Message+"；"+ex.StackTrace, "GetQueryData has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public static T JSONToObject<T>(string jsonText)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<T>(jsonText);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);

            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<PointF> GetPointXY(string data)
        {
            try { 
            List<PointF> points = new List<PointF>();
            object pointList = JSONToObject<object>(data);
            Array aPoint = (Array)pointList;
            for (int i = 0; i < aPoint.Length; i++)
            {
                Dictionary<string, object> spoint = (Dictionary<string, object>)aPoint.GetValue(i);
                PointF PF = new PointF();
                foreach (KeyValuePair<string, object> kv in spoint)
                {
                    if (kv.Key == "lng")
                    {
                        PF.X = float.Parse(kv.Value.ToString());
                    }
                    else if (kv.Key == "lat")
                    {
                        PF.Y = float.Parse(kv.Value.ToString());
                    }
                }
                points.Add(PF);
            }
            return points;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("GisRoadBrowse.aspx-GetPointXY", ex.Message+"；"+ex.StackTrace, "GetPointXY has an exception");
            }
            return null;
        }

        /// <summary>
        /// 重新展示清单数据
        /// </summary>
        private void reShowData()
        {
            try
            {

           
            //清空地图瞄点
            string js = " BMAP.ClearLine();";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
            js = " BMAP.ClearCircle();";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
            GetQueryData();
            if (dsquery != null)
            {
                this.StoreInfo.DataSource = dsquery;
                this.StoreInfo.DataBind();
            }
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("GisRoadBrowse.aspx-reShowData", ex.Message+"；"+ex.StackTrace, "reShowData has an exception");
            }
        }

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

        #region windows

        public string roadid = ""; //道路id
        public List<PointF> listxyz = new List<PointF>();

        /// <summary>
        /// 初始化编辑信息
        /// </summary>
        /// <param name="id"></param>
        private void initwinedit(string id)
        {
            MyNet.DataAccess.Model.RoadInfo model = roadManager.GetModel(id);
            if (model != null)
            {
                this.txtRoadIDwin.Text = id;
                Session["ROADID"] = id;
                this.txtRoadNamewin.Text = model.DLMC;
                this.uiDepartment1.DepertId = model.SSXQ;
                this.cboRoadTypewin.Text = model.DLLX;
                this.txtDLBHwin.Text = model.DLBH;
                this.txtDLBHwin.Visible = true;
                btnSave.Text = GetLangStr("GisRoadBrowse34", "修改");
                win.Show();
            }
        }

        /// <summary>
        /// 保存新建道路信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                MyNet.DataAccess.Model.RoadInfo model = new DataAccess.Model.RoadInfo();

                lab_message.Text = "";
                model.DLBH = this.txtDLBHwin.Text;
                model.DLLX = this.cboRoadTypewin.Text;
                model.DLMC = this.txtRoadNamewin.Text;
                model.SSXQ = this.uiDepartment1.DepertId;
                model.ISMARK = "1";
                model.XYZPOINT = listxyz;

                model.ROADID = tgsPproperty.GetRecordId();
                if (string.IsNullOrEmpty(model.DLBH))
                {
                    model.DLBH = model.ROADID;
                }
                if (Session["isedit"].ToString() == "false")
                {
                    listxyz = GetPointXY(Session["roadpoints"].ToString());
                    model.XYZPOINT = listxyz;
                    if (!roadManager.Add(model))
                        lab_message.Text = GetLangStr("GisRoadBrowse40", "提示：信息保存失败！");
                }
                else
                {
                    model.ROADID = Session["ROADID"].ToString();
                    if (!roadManager.Update(model))
                        lab_message.Text = GetLangStr("GisRoadBrowse40", "提示：信息保存失败！");
                }
                this.txtDLBHwin.Visible = false;
                win.Hide();
                reShowData();
                Notice(GetLangStr("GisRoadBrowse32", "信息提示"), (Session["isedit"].ToString() == "false" ? GetLangStr("GisRoadBrowse41", "保存成功！") : GetLangStr("GisRoadBrowse42", "修改成功！")));
            }
            catch(Exception ex)
            {

                logManager.InsertLogError("GisRoadBrowse.aspx-Save_Click", ex.Message+"；"+ex.StackTrace, "Save_Click has an exception");
         
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Cancle_Click(object sender, EventArgs e)
        {
            string js = " BMAP.ClearLine();";
            this.ResourceManager1.RegisterAfterClientInitScript(js);

            js = " BMAP.ClearCircle();";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
            win.Hide();
        }

        #endregion windows
    }
}