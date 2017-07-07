using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
// ***********************************************************************
// Assembly         : MyNet.Atmcs.Uscmcp.Web
// Author           : zlsyl
// Created          : 08-12-2016
//
// Last Modified By : zlsyl
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="KeyRoadManager.aspx.cs" company="ZKLT">
//     Copyright (c) ZKLT. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// The Web namespace.
/// </summary>
namespace MyNet.Atmcs.Uscmcp.Web
{
    /// <summary>
    /// Class KeyRoadManager.
    /// </summary>
    public partial class KeyRoadManager : System.Web.UI.Page
    {
        #region 成员变量

        /// <summary>
        /// The setting manager
        /// </summary>
        private SettingManager settingManager = new SettingManager();

        private static DataTable dtBind = null;//已绑定的监测点

        /// <summary>
        /// The road manager
        /// </summary>
        private RoadManager roadManager = new RoadManager();

        /// <summary>
        /// The TGS pproperty
        /// </summary>
        private TgsPproperty tgsPproperty = new TgsPproperty();

        /// <summary>
        /// The user login
        /// </summary>
        private UserLogin userLogin = new UserLogin();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("KeyRoadManager54", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                try
                {
                    BuildTree(TreeGrid1.Root);
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex);
                }
                this.DataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("KeyRoadManager55", "访问：重点道理管理"), userinfo.NowIp, "0");
            }
        }

        /// <summary>
        /// 部门树 节点 单击选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void RowSelect(object sender, DirectEventArgs e)
        {
            try
            {
                RowSelectionModel sm = this.GridKeyRoad.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRows.Clear();
                string id = e.ExtraParams["id"];

                Session["DEPARTID"] = id;
                GetAllDlInfo(id);
                StoreStation.DataSource = new DataTable();//清空绑定监测点
                StoreStation.DataBind();
                StoreBindStation.DataSource = new DataTable();//清空点击关联监测点窗口中绑定的监测点
                StoreBindStation.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("KeyRoadManager.aspx-RowSelect", ex.Message+"；"+ex.StackTrace, "RowSelect has an exception");
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            TxtStationName.Reset();
            CmbStationType.Reset();
        }

        /// <summary>
        /// 关联监测点窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void btnAddStation_Click(object sender, DirectEventArgs e)
        {
            try
            {
                RowSelectionModel sm = this.GridKeyRoad.SelectionModel.Primary as RowSelectionModel;
                //string Id = HideRoad.Value as string;

                if (sm.SelectedRow != null)
                {
                    GridQueryStation.RemoveAll();
                    GridBindStation.RemoveAll();
                    StoreStationType.DataSource = tgsPproperty.GetStationTypeInfo();
                    StoreStationType.DataBind();
                    StoreBindStation.DataSource = new DataTable();
                    StoreBindStation.DataSource = dtBind;
                    StoreBindStation.DataBind();
                    TxtStationName.Reset();
                    CmbStationType.Reset();
                    StoreQueryStation.DataSource = new DataTable();
                    StoreQueryStation.DataBind();
                    Window4.Show();
                }
                else
                {
                    Notice(GetLangStr("KeyRoadManager48", "信息提示"), GetLangStr("KeyRoadManager49", "请选择道路信息！"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("KeyRoadManager.aspx-btnAddStation_Click", ex.Message+"；"+ex.StackTrace, "btnAddStation_Click has an exception");
            }
        }

        /// <summary>
        /// 行选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void ApplyClick(object sender, DirectEventArgs e)
        {
            try
            {
                string data = e.ExtraParams["data"];
                HideRoad.Value = Bll.Common.GetdatabyField(data, "col0");

                GetStationInfoByRoad(HideRoad.Value.ToString());
                //HideStationType.Value = Bll.Common.GetdatabyField(data, "col4");
                //string stationid = Bll.Common.GetdatabyField(data, "col1");
                //HideLocationId.Value = Bll.Common.GetdatabyField(data, "col6");
                //hideLocationName.Value = Bll.Common.GetdatabyField(data, "col7");
                //MenuModify.Disabled = false;
                //MenuDelete.Disabled = false;
                //MenuStation.Disabled = false;
                //GridDevice.Title = Bll.Common.GetdatabyField(data, "col3") + "- 设备关联信息";
                //GridPanelDirection.Title = Bll.Common.GetdatabyField(data, "col3") + "- 方向管理";
                //HideStation.Value = stationid;
                //GetDirectionInfo(stationid);
                //GetDeviceInfo(stationid);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("KeyRoadManager.aspx-ApplyClick", ex.Message+"；"+ex.StackTrace, "ApplyClick has an exception");
            }
        }

        /// <summary>
        /// 查询监测点信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                StationDataBind(GetWhere());
                StoreBindStation.DataSource = new DataTable();
                StoreBindStation.DataSource = dtBind;
                StoreBindStation.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("KeyRoadManager.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        #endregion 控件事件

        #region DirectMethod

        /// <summary>
        /// 保存重点道路
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [DirectMethod]
        public void AddKeyRoadInfo(string[] str1, int count)
        {
            try
            {
                List<string> Listdeviid = new List<string>();
                for (int i = 0; i < count; i++)
                {
                    Listdeviid.Add(str1[i]);
                }
                //int j = 0;
                //foreach (string item in Listdeviid)
                //{
                //    if (roadManager.UpdateKeyRoad(item))
                //    {
                //        j++;
                //    }
                //}
                //if (j.Equals(count))
                if (roadManager.SetKeyRoad(Listdeviid, Session["DEPARTID"].ToString()))
                {
                    Notice(GetLangStr("KeyRoadManager48", "信息提示"), GetLangStr("KeyRoadManager50", "重点道路保存成功！"));
                }
                else
                {
                    Notice(GetLangStr("KeyRoadManager48", "信息提示"), GetLangStr("KeyRoadManager51", "重点道路保存失败！"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("KeyRoadManager.aspx-AddKeyRoadInfo", ex.Message+"；"+ex.StackTrace, "AddKeyRoadInfo has an exception");
            }
        }

        /// <summary>
        /// 关联监测点信息
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [DirectMethod]
        public void AddBindStationInfo(string[] str1, int count)
        {
            try
            {
                string id = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string dlId = HideRoad.Value as string;
                List<string> Listdeviid = new List<string>();
                for (int i = 0; i < count; i++)
                {
                    Listdeviid.Add(str1[i]);
                }
                if (tgsPproperty.InsertKeyroadStation(id, dlId, Listdeviid) > 0)
                {
                    Notice(GetLangStr("KeyRoadManager48", "信息提示"), GetLangStr("KeyRoadManager52", "监测点关联成功！"));
                    GetStationInfoByRoad(dlId);
                    Window4.Hide();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("KeyRoadManager.aspx-AddBindStationInfo", ex.Message+"；"+ex.StackTrace, "AddBindStationInfo has an exception");
            }
        }

        /// <summary>
        /// 根据传入部门编号 绑定道路
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public void GetAllDlInfo()
        {
            try
            {
                DataTable dtAllRoad = Bll.Common.ChangColName(roadManager.GetList(" IsKeyRoad='0' and departId='" + Session["DEPARTID"].ToString() + "'"));
                StoreAllRoad.DataSource = dtAllRoad;
                StoreAllRoad.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("KeyRoadManager.aspx-GetAllDlInfo", ex.Message+"；"+ex.StackTrace, "GetAllDlInfo has an exception");
            }
        }

        #endregion DirectMethod

        #region 私有方法

        /// <summary>
        /// 将部门信息绑定至tree
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private void BuildTree(Ext.Net.TreeNodeCollection nodes)
        {
            try
            {
                TreeGrid1.RemoveAll(true);
                ClearInfoData();
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }
                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Text = GetLangStr("KeyRoadManager53", "机构管理");
                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;
                DataTable dt = Bll.Common.ChangColName(GetRedisData.GetData("t_cfg_department")); settingManager.GetConfigDepartment("00");

                if (dt != null && dt.Rows.Count > 0)
                {
                    Addree(dt, dt.Rows[0]["col2"].ToString(), root, null);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("KeyRoadManager.aspx-BuildTree", ex.Message+"；"+ex.StackTrace, "BuildTree has an exception");
            }
            if (X.IsAjaxRequest)
            {
                TreeGrid1.Render(PanelNavigate, RenderMode.Auto);
            }
        }

        /// <summary>
        /// 遍历将子部门挂接至父部门
        /// </summary>
        /// <param name="allNodeTable"></param>
        /// <param name="parentColValue"></param>
        /// <param name="root"></param>
        /// <param name="ParentNode"></param>
        /// <returns></returns>
        private void Addree(DataTable allNodeTable, string parentColValue, Ext.Net.TreeNode root, Ext.Net.TreeNode ParentNode)
        {
            try
            {
                DataRow[] myDataRows = allNodeTable.Select("col2 ='" + parentColValue + "'");

                foreach (DataRow myDataRow in myDataRows)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode(); ;
                    node.Text = myDataRow[1].ToString();
                    node.NodeID = myDataRow[0].ToString();
                    node.Leaf = true;
                    node.Draggable = false;
                    node.Expandable = ThreeStateBool.True;
                    node.Expanded = true;

                    node.Icon = Icon.Telephone;
                    if (ParentNode != null)
                    {
                        ParentNode.Nodes.Add(node);
                        Addree(allNodeTable, myDataRow["col0"].ToString(), ParentNode, node);
                    }
                    else
                    {
                        root.Nodes.Add(node);
                        Addree(allNodeTable, myDataRow["col0"].ToString(), root, node);
                        ShowInfoData(myDataRow);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("KeyRoadManager.aspx-Addree", ex.Message+"；"+ex.StackTrace, "Addree has an exception");
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        /// <returns></returns>
        private void ClearInfoData()
        {
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private void ShowInfoData(DataRow dr)
        {
            try
            {
                ClearInfoData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("KeyRoadManager.aspx-ShowInfoData", ex.Message+"；"+ex.StackTrace, "ShowInfoData has an exception");
            }
        }

        /// <summary>
        /// 根据传入部门编号 绑定道路
        /// </summary>
        /// <param name="departId"></param>
        /// <returns></returns>
        private void GetAllDlInfo(string departId)
        {
            try
            {
                DataTable dtAllRoad = Bll.Common.ChangColName(roadManager.GetList(" SSXQ='" + departId + "' and  IsKeyRoad='0' "));
                StoreAllRoad.DataSource = dtAllRoad;
                StoreAllRoad.DataBind();
                DataTable dtKeyRoad = Bll.Common.ChangColName(roadManager.GetList(" SSXQ='" + departId + "' and  IsKeyRoad='1' "));
                StoreKeyRoad.DataSource = dtKeyRoad;
                StoreKeyRoad.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("KeyRoadManager.aspx-GetAllDlInfo", ex.Message+"；"+ex.StackTrace, "GetAllDlInfo has an exception");
            }
        }

        /// <summary>
        /// 根据道路编号 获得监测点信息
        /// </summary>
        /// <param name="roadId"></param>
        /// <returns></returns>
        private void GetStationInfoByRoad(string roadId)
        {
            try
            {
                dtBind = tgsPproperty.GetStationInfoByRoad(roadId);
                StoreStation.DataSource = dtBind;
                StoreStation.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("KeyRoadManager.aspx-GetStationInfoByRoad", ex.Message+"；"+ex.StackTrace, "GetStationInfoByRoad has an exception");
            }
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
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
        /// 绑定监测点信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        private void StationDataBind(string where)
        {
            try
            {
                DataTable dt = tgsPproperty.GetAllStationInfoWithRoad(where);
                StoreQueryStation.DataSource = dt;
                StoreQueryStation.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("KeyRoadManager.aspx-StationDataBind", ex.Message+"；"+ex.StackTrace, "StationDataBind has an exception");
            }
        }

        /// <summary>
        /// 获得设备查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            try
            {
                string where = "   1=1   ";
                if (!string.IsNullOrEmpty(TxtStationName.Text))
                {
                    where = where + " and  a.STATION_NAME   like '%" + TxtStationName.Text.ToUpper() + "%' ";
                }
                if (CmbStationType.SelectedIndex != -1)
                {
                    where = where + " and  a.STATION_TYPE_ID='" + CmbStationType.SelectedItem.Value + "' ";
                }
                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("KeyRoadManager.aspx-GetWhere", ex.Message+"；"+ex.StackTrace, "GetWhere has an exception");
                return "";
            }
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