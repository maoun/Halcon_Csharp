using System;
using System.Collections;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class AlarmCarDeal : System.Web.UI.Page
    {

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();
        private string xh = "";
        private string gridlist = "";
        private string userName = "administrator";

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                //userLogin.IsLoginPage(this);
                GridData.Value = "";
                SelectGrid.Value = "";
                StoreDataBind();
                xh = Request["xh"];
                //xh = "1000000078191065231635";
                if (Session["userinfo"] != null)
                {
                    UserInfo userinfo = Session["userinfo"] as UserInfo;
                    userName = userinfo.UserName;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：用户登录", userinfo.NowIp, "0");
                }
                if (!string.IsNullOrEmpty(xh))
                {
                    GetData("xh='" + xh + "'");
                }
                
            }
        }
        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            StorePlateType.DataSource = Bll.Common.ChangColName(GetRedisData.GetData( "t_sys_code:140001"));// tgsPproperty.GetPalteType();
            StorePlateType.DataBind();
        }

        private void AddGirdView(DataTable dt, string bjlx, string title)
        {
            try
            {
                if (dt != null)
                {
                    Store store = new Store { ID = "StoreRow_" + bjlx + "_" + DateTime.Now.ToString("HHmmss") };
                    JsonReader reader = new JsonReader();
                    reader.IDProperty = "col0";
                    store.Reader.Add(reader);
                    Ext.Net.Panel tp = new Ext.Net.Panel();
                    tp.ID = "Panel_" + bjlx + "_" + DateTime.Now.ToString("HHmmss");
                    tp.Title = title;
                    ColumnLayout cl = new ColumnLayout();
                    cl.ID = "ColumnLayout_" + bjlx + "_" + DateTime.Now.ToString("HHmmss");
                    cl.Split = true;
                    cl.FitHeight = true;
                    LayoutColumn lc1 = new LayoutColumn();
                    lc1.ColumnWidth = (decimal)0.75;
                    LayoutColumn lc2 = new LayoutColumn();
                    lc2.ColumnWidth = (decimal)0.25;
                    cl.Columns.Add(lc1);
                    cl.Columns.Add(lc2);
                    GridPanel gridPanel = AddGridPanel(dt, store, bjlx, reader);
                    lc1.Items.Add(gridPanel);
                    FormPanel panel = AddFormPanel(bjlx, title);
                    lc2.Items.Add(panel);
                    tp.Items.Add(cl);
                    gridlist = gridlist + tp.ID + ",";
                    TabPanelGrid.Items.Add(tp);
                    if (X.IsAjaxRequest)
                    {
                        tp.Render();
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmCarDeal.aspx-AddGirdView", ex.Message+"；"+ex.StackTrace, "AddGirdView has an exception");
            }
        }

        private FormPanel AddFormPanel(string bjlx, string title)
        {
            FormPanel panel = new FormPanel();
            panel.ID = "Panel2_" + bjlx;
            panel.Title = title + " -- 处理信息";
            panel.Frame = true;
            panel.DefaultAnchor = "100%";
            panel.MonitorValid = true;
            panel.Padding = 5;
            switch (bjlx)
            {
                case "1":
                case "2":
                case "3":
                    panel.Items.Add(Bll.CommonExt.AddTextField("TxtRecordId_" + bjlx, "记录编号", false, "请输入记录编号"));
                    panel.Items.Add(Bll.CommonExt.AddTextField("TxtPlateId_" + bjlx, "号牌号码", false, "请输入号牌号码"));
                    panel.Items.Add(Bll.CommonExt.AddComboBox("CmbPlateType_" + bjlx, "号牌种类", "StorePlateType", "选择号牌种类", false));
                    panel.Items.Add(Bll.CommonExt.AddTextField("TxtDeal_" + bjlx, "处理人员", userName));
                    panel.Items.Add(Bll.CommonExt.AddTextArea("TxtNotice_" + bjlx, "处理意见"));
                    break;
            }
            string butId = "Button_" + bjlx;
            Ext.Net.Button butsave = CommonExt.AddButton(butId, "信息保存", "TableSave", "AlarmCarDeal.UpdateData('" + bjlx + "')");
            panel.Buttons.Add(butsave);
            panel.Listeners.ClientValidation.Handler = butId + ".setDisabled(!valid);";
            return panel;
        }

        private GridPanel AddGridPanel(DataTable dt, Store store, string bjlx, JsonReader reader)
        {
            GridPanel gridAlarm = new GridPanel();
            gridAlarm.ID = "gridAlarm_" + bjlx;
            gridAlarm.Header = false;
            gridAlarm.Collapsible = true;
            gridAlarm.Store.Add(store);
            gridAlarm.AutoHeight = true;
            gridAlarm.AutoRender = true;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                RecordField field = new RecordField(dt.Columns[i].ColumnName, RecordFieldType.String);
                reader.Fields.Add(field);
                Column col = new Column();
                col.Header = dt.Columns[i].ColumnName;
                col.Sortable = true;
                if (dt.Rows.Count > 0)
                {
                    string str = "";
                    if (dt.Rows[0][i].ToString().Length == 0)
                    {
                        str = dt.Columns[i].ColumnName;
                    }
                    else
                    {
                        str = dt.Rows[0][i].ToString();
                    }
                    col.Width = 40 + System.Text.Encoding.Default.GetByteCount(str) * 5;
                }
                col.DataIndex = dt.Columns[i].ColumnName;
                if (col.Header == "zjwj1" || col.Header == "zjwj2")
                {
                    col.Hidden = true;
                }
                gridAlarm.ColumnModel.Columns.Add(col);
            }
            CheckboxSelectionModel chksm = new CheckboxSelectionModel();
            chksm.ID = "CheckboxSelectionModel_" + bjlx;
            chksm.SingleSelect = false;
            chksm.Listeners.RowSelect.Handler = "AlarmCarDeal.SelectChange(record.data,'" + bjlx + "');";
            chksm.Listeners.RowDeselect.Handler = "AlarmCarDeal.DeSelectChange(record.data,'" + bjlx + "');";
            gridAlarm.SelectionModel.Add(chksm);

            store.DataSource = dt;
            store.DataBind();
            gridAlarm.View.Add(AddGroupView(bjlx));
            gridAlarm.Plugins.Add(AddRowExpander(bjlx));
            return gridAlarm;
        }

        private GroupingView AddGroupView(string bjlx)
        {
            GroupingView groupingView = new GroupingView();
            groupingView.ID = "GroupingView_" + bjlx + "_" + DateTime.Now.ToString("HHmmss");
            groupingView.ForceFit = true;
            groupingView.MarkDirty = false;
            groupingView.ShowGroupName = false;
            groupingView.EnableNoGroups = true;
            groupingView.HideGroupedColumn = true;
            groupingView.GroupByText = "用该列进行分组";
            groupingView.ShowGroupsText = "显示分组";
            return groupingView;
        }

        private RowExpander AddRowExpander(string bjlx)
        {
            RowExpander rowExpander = new RowExpander();
            rowExpander.ID = "rowExpander_" + bjlx + "_" + DateTime.Now.ToString("HHmmss");
            rowExpander.Template.Html = GetHandler();
            return rowExpander;
        }

        private string GetHandler()
        {
            string hander = @"<div class=\""thumb\"">
                              <table border=\""0\"" cellpadding=\""0\"" cellspacing=\""0\"" width=\""100%\"">
                               <tr>
                                  <td align=\""center\""  valign=\""middle\""> <img src=\""{zjwj1}\"" id=\""zjwj1\""  width=\""480\"" style=\""cursor: pointer\"" onclick=\""zoom(this,false);\"" alt=\""车辆图片(图片点击滚轮缩放)\"">
                                  </td>
                                  <td align=\""center\""  valign=\""middle\""> <img src=\""{zjwj2}\"" id=\""zjwj2\"" width=\""480\""  style=\""cursor: pointer\"" onclick=\""zoom(this,false);\"" alt=\""车辆图片(图片点击滚轮缩放)\"">
                                  </td>
                               </tr>
                              </table>
                            </div>";
            return hander;
        }

        private DataTable GetDataTableByJllx(DataRow dr, ref string bjlx, ref string bjlxms)
        {
            DataTable myDatatable = new DataTable("myDatatable");
            DataRow row;
            bjlx = dr["col5"].ToString();
            bjlxms = dr["col6"].ToString();
            switch (bjlx)
            {
                case "8":
                case "9":
                case "10":
                    myDatatable.Columns.Add(new DataColumn("记录编号", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("号牌号码", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("号牌种类", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("处理类型", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("处理原因", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("处理状态", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("处理人", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("处理时间", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("zjwj1", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("zjwj2", typeof(System.String)));
                    row = myDatatable.NewRow();
                    row["记录编号"] = dr["col0"].ToString();
                    row["号牌号码"] = dr["col3"].ToString();
                    row["号牌种类"] = dr["col2"].ToString();
                    row["处理类型"] = dr["col6"].ToString();
                    row["处理原因"] = dr["col7"].ToString();
                    row["处理状态"] = dr["col18"].ToString();
                    row["处理人"] = dr["col20"].ToString();
                    row["处理时间"] = dr["col19"].ToString();
                    row["zjwj1"] = dr["col21"].ToString();
                    row["zjwj2"] = dr["col22"].ToString();
                    myDatatable.Rows.Add(row);
                    break;

                case "4":
                case "5":
                case "7":
                    myDatatable.Columns.Add(new DataColumn("记录编号", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("号牌号码", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("号牌种类", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("违法时间", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("违法地点", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("违法类型", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("速度/限速", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("处理状态", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("处理人", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("处理时间", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("zjwj1", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("zjwj2", typeof(System.String)));
                    DataTable dt3 = tgsDataInfo.GetPeccancyInfo("  hphm='" + dr["col3"].ToString() + "' and hpzl='" + dr["col1"].ToString() + "' and cfbj='0' ", 1, 10);
                    if (dt3 != null)
                    {
                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            row = myDatatable.NewRow();
                            row["记录编号"] = dt3.Rows[i]["col0"].ToString();
                            row["号牌号码"] = dt3.Rows[i]["col3"].ToString();
                            row["号牌种类"] = dt3.Rows[i]["col2"].ToString();
                            row["违法时间"] = dt3.Rows[i]["col6"].ToString();
                            row["违法地点"] = dt3.Rows[i]["col8"].ToString();
                            row["违法类型"] = dt3.Rows[i]["col5"].ToString();
                            row["速度/限速"] = dt3.Rows[i]["col12"].ToString();
                            row["处理状态"] = dt3.Rows[i]["col17"].ToString();
                            row["处理人"] = dt3.Rows[i]["col19"].ToString();
                            row["处理时间"] = dt3.Rows[i]["col20"].ToString();
                            row["zjwj1"] = dt3.Rows[i]["col23"].ToString();
                            row["zjwj2"] = dt3.Rows[i]["col24"].ToString();
                            myDatatable.Rows.Add(row);
                        }
                    }
                    break;

                case "1":
                case "2":
                case "3":
                    myDatatable.Columns.Add(new DataColumn("记录编号", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("号牌号码", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("号牌种类", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("处理类型", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("处理原因", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("处理状态", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("处理人", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("处理时间", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("zjwj1", typeof(System.String)));
                    myDatatable.Columns.Add(new DataColumn("zjwj2", typeof(System.String)));
                    row = myDatatable.NewRow();
                    row["记录编号"] = dr["col0"].ToString();
                    row["号牌号码"] = dr["col3"].ToString();
                    row["号牌种类"] = dr["col2"].ToString();
                    row["处理类型"] = dr["col6"].ToString();
                    row["处理原因"] = dr["col7"].ToString();
                    row["处理状态"] = dr["col18"].ToString();
                    row["处理人"] = dr["col20"].ToString();
                    row["处理时间"] = dr["col19"].ToString();
                    row["zjwj1"] = dr["col21"].ToString();
                    row["zjwj2"] = dr["col22"].ToString();
                    myDatatable.Rows.Add(row);
                    break;
            }
            return myDatatable;
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            string where = Getwhere();
            if (!string.IsNullOrEmpty(where))
            {
                GetData(where);
            }
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string Getwhere()
        {
            string where = "1=1";
            if (!string.IsNullOrEmpty(TxtplateId.Text))
            {
                where = where + " and  hphm= '" + TxtplateId.Text + "'";
            }
            if (CmbPlateType.SelectedIndex != -1)
            {
                where = where + " and  hpzl='" + CmbPlateType.SelectedItem.Value + "' ";
            }
            if (where != "1=1")
            {
                where = where + " and clbj='0'";
                return where;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
        }

        private void GetData(string where)
        {
            string[] strid = GridData.Value.ToString().Split(',');
            for (int n = 0; n < strid.Length; n++)
            {
                if (!string.IsNullOrEmpty(strid[n]))
                {
                    TabPanelGrid.Remove(strid[n], true);
                }
            }
            QueryCondition.Value = where;
            DataTable dt = tgsDataInfo.GetAlarmInfo(where, 1, 50);
            if (dt.Rows.Count > 0)
            {
                gridlist = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string bjlx = "";
                    string bjlxms = "";
                    DataTable dt2 = GetDataTableByJllx(dt.Rows[i], ref  bjlx, ref bjlxms);
                    AddGirdView(dt2, bjlx, bjlxms);
                }

                if (!string.IsNullOrEmpty(gridlist))
                {
                    GridData.Value = gridlist.Substring(0, gridlist.Length - 1);
                }
                TabPanelGrid.ActiveTabIndex = 0;
            }
        }

        [DirectMethod]
        public void SelectChange(string gridinfo, string bjlx)
        {
            string jsonstr = "{gridinfo:[" + gridinfo + "]}";
            DataTable dt = ConvertData.JsonToDataTable2(jsonstr);
            if (dt.Rows.Count > 0)
            {
                AddHiddenJson(bjlx, dt.Rows[0]["记录编号"].ToString());
                switch (bjlx)
                {
                    case "1":
                    case "2":
                    case "3":
                        X.GetCmp<TextField>("TxtPlateId_" + bjlx).Text = dt.Rows[0]["号牌号码"].ToString();
                        X.GetCmp<ComboBox>("CmbPlateType_" + bjlx).Text = dt.Rows[0]["号牌种类"].ToString();
                        SetValue(X.GetCmp<TextField>("TxtRecordId_" + bjlx), bjlx);
                        break;
                }
            }
        }

        [DirectMethod]
        public void DeSelectChange(string gridinfo, string bjlx)
        {
            string jsonstr = "{gridinfo:[" + gridinfo + "]}";
            DataTable dt = ConvertData.JsonToDataTable2(jsonstr);
            if (dt.Rows.Count > 0)
            {
                DelHiddenJson(bjlx, dt.Rows[0]["记录编号"].ToString());
                switch (bjlx)
                {
                    case "1":
                    case "2":
                    case "3":
                        SetValue(X.GetCmp<TextField>("TxtRecordId_" + bjlx), bjlx);
                        break;
                }
            }
        }

        [DirectMethod]
        public void UpdateData(string bjlx)
        {
            Hashtable hs = new Hashtable();

            switch (bjlx)
            {
                case "1":
                case "2":
                case "3":
                    string[] ids = X.GetCmp<TextField>("TxtRecordId_" + bjlx).Text.Split(',');
                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(ids[i]))
                        {
                            hs.Add("xh", ids[i]);
                            hs.Add("bjlx", bjlx);
                            hs.Add("clry", X.GetCmp<TextField>("TxtDeal_" + bjlx).Text);
                            hs.Add("clyj", X.GetCmp<TextField>("TxtNotice_" + bjlx).Text);

                            if (tgsDataInfo.DealAlarmInfo(hs) > 0)
                            {
                                Notice("信息提示", "[" + ids[i] + "]处理完成");
                                GetData(QueryCondition.Value.ToString());
                            }
                        }
                    }
                    break;
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
                Icon = Icon.Information,
                HideDelay = 2000,
                Height = 120,
                Html = "<br></br>" + msg + "!"
            });
        }

        private void AddHiddenJson(string bjlx, string id)
        {
            try
            {
                string selectGridvalue = SelectGrid.Value.ToString();
                string addstr = "bjlx_" + bjlx + ":" + id + ",";
                selectGridvalue = selectGridvalue.Replace(addstr, "");
                selectGridvalue = selectGridvalue + addstr;
                SelectGrid.Value = selectGridvalue;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmCarDeal.aspx-AddHiddenJson", ex.Message+"；"+ex.StackTrace, "AddHiddenJson has an exception");
            }
        }

        private void DelHiddenJson(string bjlx, string id)
        {
            try
            {
                string selectGridvalue = SelectGrid.Value.ToString();
                string addstr = "bjlx_" + bjlx + ":" + id + ",";
                selectGridvalue = selectGridvalue.Replace(addstr, "");
                SelectGrid.Value = selectGridvalue;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmCarDeal.aspx-DelHiddenJson", ex.Message+"；"+ex.StackTrace, "DelHiddenJson has an exception");
            }
        }

        private void SetValue(TextField tx, string bjlx)
        {
            try
            {
                string selectGridvalue = SelectGrid.Value.ToString();
                string[] selects = selectGridvalue.Split(',');
                string recordid = "";
                for (int i = 0; i < selects.Length; i++)
                {
                    if (!string.IsNullOrEmpty(selects[i]))
                    {
                        string[] str = selects[i].Split(':');
                        if (str[0] == "bjlx_" + bjlx)
                        {
                            recordid = recordid + str[1] + ",";
                        }
                    }
                }
                if (string.IsNullOrEmpty(recordid))
                {
                    tx.Text = "";
                }
                else
                {
                    tx.Text = recordid.Substring(0, recordid.Length - 1);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("AlarmCarDeal.aspx-SetValue", ex.Message+"；"+ex.StackTrace, "SetValue has an exception");
            }
        }
    }
}