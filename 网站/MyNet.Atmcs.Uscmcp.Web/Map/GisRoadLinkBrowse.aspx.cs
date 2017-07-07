    using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.Script.Serialization;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class GisRoadLinkBrowse : System.Web.UI.Page
    {
        public int count = 0;

        private RoadManager roadManager = new RoadManager();
        private SettingManager settingManager = new SettingManager();
        private TgsDataInfo tgs = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 

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
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" +StaticInfo.LoginPage + "'"; 
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); 
                return; 
            }
            uiDepartment.DepartmentSelectEvent += new UI.UIDepartment.DepartmentSelectHandler(uiDepartment_DepartmentSelectEvent);
            if (!X.IsAjaxRequest)
            {
                FrmLoad();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                 logManager.InsertLogRunning(userinfo.UserName, "访问：GisRoadLinkBrowse.aspx加载数据", userinfo.NowIp, "0");
            }
        }

        private void FrmLoad()
        {
            this.RoadInfoWin.DataSource = roadManager.GetList(" 1=1");
            this.RoadInfoWin.DataBind();
        }

        private void uiDepartment_DepartmentSelectEvent(string depertId, string e)
        {
            this.RoadInfo.DataSource = roadManager.GetList(" ssxq='" + uiDepartment.DepertId + "'");
            this.RoadInfo.DataBind();
        }

        /// <summary>
        /// 批量定位
        /// </summary>
        private void ShowRoadPoints(List<string> roadList)
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
                DataTable ds = roadManager.GetRoadSegPoints(where);
                string roadid = "";
                string points = "";
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    if (roadid == "" || roadid == ds.Rows[i]["ROADID"].ToString())
                    {
                        if (points == "")
                            DlmcList.Add(ds.Rows[i]["LDMC"].ToString());
                        points += (points == "" ? "" : "|") + ds.Rows[i]["XPOS"].ToString() + "," + ds.Rows[i]["YPOS"].ToString();
                    }
                    else
                    {
                        MapList.Add(points);
                        DlmcList.Add(ds.Rows[i]["LDMC"].ToString());
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
                logManager.InsertLogError("GisRoadLinkBrowse.aspx-ShowRoadPoints", ex.Message+"；"+ex.StackTrace, "ShowRoadPoints has an exception");

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

          
            List<string> rordIds = new List<string>();
            RowSelectionModel sm = this.GridRoadManager.SelectionModel.Primary as RowSelectionModel;

            foreach (SelectedRow row in sm.SelectedRows)
            {
                rordIds.Add(row.RecordID);
            }
            ShowRoadPoints(rordIds);
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("GisRoadLinkBrowse.aspx-ApplyClick", ex.Message+"；"+ex.StackTrace, "ApplyClick has an exception");

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
                logManager.InsertLogError("GisRoadLinkBrowse.aspx-DrawLine", ex.Message+"；"+ex.StackTrace, "DrawLine has an exception");
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
            QueryShow();
        }

        /// <summary>
        /// 清空地图描点
        /// </summary>
        private void clearMapLine()
        {
            string js = " BMAP.ClearLine();";
            this.ResourceManager1.RegisterAfterClientInitScript(js);

            js = " BMAP.ClearCircle();";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }

        /// <summary>
        /// 删除结果显示
        /// </summary>
        private void QueryShow()
        {
            try
            {
            string where = " 1=1 ";
            if (this.cboRoadInfo.Text.Trim() != "")
                where = " DLBH ='" + this.cboRoadInfo.Text + "'";
            if (this.txtRoadName.Text.Trim() != "")
                where += (where == "" ? "" : " and ") + " LDMC  like '%" + this.txtRoadName.Text + "%'";
            this.StoreInfo.DataSource = roadManager.GetRoadSegList(where);
            this.StoreInfo.DataBind();
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("GisRoadLinkBrowse.aspx-QueryShow", ex.Message+"；"+ex.StackTrace, "QueryShow has an exception");
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
            this.cboRoadInfo.Text = "";
        }

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

        public List<PointF> GetPointXY(string data)
        {
            try
            {

           
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
                logManager.InsertLogError("GisRoadLinkBrowse.aspx-QueryShow", ex.Message+"；"+ex.StackTrace, "QueryShow has an exception");
            }
            return null;
        }

        [DirectMethod] //新建
        public void AddRoadPoints(string points)
        {
            txtRoadSegId.Text = tgs.GetTgsRecordId();
            CheckData.Value = points;
            Session["roadpoints"] = points;
            SaveFlag.Value = "Add";
            btnSave.Text = "新建";
            txtRoadSegName.Reset();
            cmbRoadInfo.Reset();
            cmbDirection.Reset();
            win.Show();
        }

        [DirectMethod]//修改
        public void UpdataPoints(string points)
        {
            try
            {
                roadid = Session["ROADID"].ToString();
                if (roadManager.UpdateXyz(GetPointXY(points), roadid))
                {
                    Notice("信息提示", "重绘成功！");
                    QueryShow();
                }
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("GisRoadLinkBrowse.aspx-UpdataPoints", ex.Message+"；"+ex.StackTrace, "UpdataPoints has an exception");
            }
        }

        [DirectMethod(Namespace = "OnEvl")]
        public void Editor(string type, string data)
        {
            //string id=  Bll.Common.GetdatabyField2(data, "col0");
            //if (type == "Edit")
            //{
            //    SaveFlag.Value = "Edit";
            //    txtRoadSegId.Text = id;
            //    txtRoadSegName.Text = Bll.Common.GetdatabyField2(data, "col2");
            //    cmbRoadInfo.Value = Bll.Common.GetdatabyField2(data, "col6");
            //    cmbDirection.Value = Bll.Common.GetdatabyField2(data, "col5");
            //    btnSave.Text = "修改";
            //    win.Show();
            //}
            //if (type == "UpdateRoadLine")
            //{
            //    Session["ROADID"] = id;
            //    string js = " BMAP.AddRoadPoint({Operate:'UpdateRoadSegLine'});;";
            //    this.ResourceManager1.RegisterAfterClientInitScript(js);
            //}

            //if (type == "Delete")
            //{
            //    string roadSegName = Bll.Common.GetdatabyField2(data, "col2");
            //    X.Msg.Confirm("提示", "是否删除道路信息" + "[" + roadSegName + "]", new MessageBoxButtonsConfig
            //    {
            //        Yes = new MessageBoxButtonConfig
            //        {
            //            Handler = "OnEvl.DelYes('" + id + "')",
            //            Text = "是"
            //        },
            //        No = new MessageBoxButtonConfig
            //        {
            //            Text = "否"
            //        }
            //    }).Show();
            //}
        }

        [DirectMethod(Namespace = "OnEvl")]
        public void DelYes(string roadid)
        {
            if (roadManager.DeleteRoadSeg(roadid))
            {
                Notice("信息提示", "删除成功！");
                QueryShow();
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
                Html = "<br></br>" + msg + "!"
            });
        }

        #region windows

        public string roadid = ""; //道路id
        public List<PointF> listxyz = new List<PointF>();

        [DirectMethod(Namespace = "OnEvl")]
        public void SaveRoadInfoData()
        {
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("ID", txtRoadSegId.Text);
                hs.Add("DLBH", cmbRoadInfo.Value.ToString());
                hs.Add("LDMC", txtRoadSegName.Text);
                hs.Add("LDFX", cmbDirection.Value.ToString());
                if (SaveFlag.Value.ToString() == "Add")
                {
                    listxyz = GetPointXY(CheckData.Value.ToString());

                    if (roadManager.AddRoadSeg(hs))
                    {
                        if (roadManager.UpdateXyz(listxyz, txtRoadSegId.Text))
                        {
                            Notice("信息提示", "保存成功！");
                            win.Hide();
                            QueryShow();
                        }
                    }
                }
                else
                {
                    if (roadManager.UpdateRoadSeg(hs))
                    {
                        Notice("信息提示", "修改成功！");
                        win.Hide();
                        QueryShow();
                    }
                }
            }
            catch(Exception ex)
            {
                logManager.InsertLogError("GisRoadLinkBrowse.aspx-SaveRoadInfoData", ex.Message+"；"+ex.StackTrace, "SaveRoadInfoData has an exception");

            }
        }

        #endregion windows
    }
}