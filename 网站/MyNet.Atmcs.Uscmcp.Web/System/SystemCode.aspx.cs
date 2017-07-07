using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class SystemCode : System.Web.UI.Page
    {
        private SystemManager systemManager = new SystemManager();
        private SettingManager settingManager = new SettingManager();
        private UserLogin userLogin = new UserLogin();
        private string SystemID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //判断用户是否登录
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('您没有登录或操作超时，请重新登陆!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            //判断用户是否登录结束
            if (!X.IsAjaxRequest)
            {
                SystemID = Request["systemid"];
                GetCodeTypeData();
            }
        }

        private void AddWindow()
        {
            Window window = new Window();
            window.ID = "SystemCodeAdd";
            window.Title = "字典信息管理";
            window.Width = Unit.Pixel(400);
            window.Height = Unit.Pixel(280);
            window.Modal = true;
            window.Collapsible = true;
            window.Maximizable = false;
            window.Resizable = false;
            window.Hidden = true;
            window.AutoLoad.Mode = LoadMode.Merge;

            FormPanel tabs = new FormPanel();
            tabs.ID = "TabPanel1";
            tabs.IDMode = IDMode.Explicit;
            tabs.Border = false;
            tabs.Width = Unit.Pixel(400);
            tabs.Closable = true;
            tabs.DefaultAnchor = "100%";
            Ext.Net.Panel tab = new Ext.Net.Panel();
            tab.Title = "代码表";
            tab.Padding = 5;
            tabs.Add(tab);

            TextField tx = CommonExt.AddTextField("txtECodeType", "代码类型");
            tx.Text = Session["CodeId"] as string;
            tab.Items.Add(tx);
            tx.ReadOnly = true;
            tab.Items.Add(CommonExt.AddTextField("txtECodeId", "代码值"));
            tab.Items.Add(CommonExt.AddTextField("txtECodeDesc", "代码描述"));
            tab.Items.Add(CommonExt.AddTextField("txtECodeRemark", "备注"));
            tab.Items.Add(CommonExt.AddCheckbox("chkEIsUse", "是否启用"));

            Toolbar toolbar = new Ext.Net.Toolbar();
            ToolbarFill toolbarFill = new ToolbarFill();
            toolbar.Add(toolbarFill);
            window.BottomBar.Add(toolbar);
            CommonExt.AddButton(toolbar, "butSaveEdit", "保存", "Disk", "SystemCode.InfoSave()");
            CommonExt.AddButton(toolbar, "butCancelEdit", "取消", "Cancel", window.ClientID + ".hide()");
            window.Items.Add(tabs);
            window.Render(this.Form);
            window.Show();
        }

        protected void AddColumn(object sender, DirectEventArgs e)
        {
            AddWindow();
        }

        [DirectMethod]
        public void InfoSave()
        {
            Hashtable hs = new Hashtable();
            hs.Add("codetype", X.GetCmp<TextField>("txtECodeType").Text);
            hs.Add("code", X.GetCmp<TextField>("txtECodeId").Text);
            hs.Add("codedesc", X.GetCmp<TextField>("txtECodeDesc").Text);
            hs.Add("remark", X.GetCmp<TextField>("txtECodeRemark").Text);
            hs.Add("isuse", Math.Abs(X.GetCmp<Checkbox>("chkEIsUse").Checked.CompareTo(false)).ToString());
            if (systemManager.AddSysCode(hs) > 0)
            {
                Notice("信息提示", "保存成功");
                X.GetCmp<Window>("SystemCodeAdd").Hide();
                if (Session["CodeId"] != null)
                {
                    GetData(Session["CodeId"] as string);
                }
            }
        }

        [DirectMethod]
        public void DoConfirm()
        {
            RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
            string Id = sm.SelectedRow.RecordID;
            X.Msg.Confirm("信息", "确认要删除[" + Id + "]这条记录吗?", new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = "SystemCode.DoYes()",
                    Text = "是"
                },
                No = new MessageBoxButtonConfig
                {
                    Handler = "SystemCode.DoNo()",
                    Text = "否"
                }
            }).Show();
        }

        [DirectMethod]
        public void DoYes()
        {
            RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
            sm.SelectedRow.ToBuilder();
            Hashtable hs = new Hashtable();
            string Id = sm.SelectedRow.RecordID;
            int idx = sm.SelectedRow.RowIndex;
            hs.Add("codetype", Session["CodeId"] as string);
            hs.Add("oldcode", Id);
            if (systemManager.DelSysCode(hs) > 0)
            {
                Notice("信息提示", "删除成功");
                GridPanel2.DeleteSelected();
            }
            if (Session["CodeId"] != null)
            {
                GetData(Session["CodeId"] as string);
            }
        }

        [DirectMethod]
        public void DoNo()
        {
        }

        private void GetData(string codeid)
        {
            DataTable dt = systemManager.GetCodeData(codeid);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["col4"].ToString() == "1")
                {
                    dt.Rows[i]["col4"] = "true";
                }
                else
                {
                    dt.Rows[i]["col4"] = "false";
                }
            }
            this.StoreCode.DataSource = dt;
            this.StoreCode.DataBind();
        }

        private void GetCodeTypeData()
        {
            DataTable objData = null;
            if (string.IsNullOrEmpty(SystemID))
            {
                objData = systemManager.GetCodeTypeData();
            }
            else
            {
                objData = settingManager.GetSettingCodeType(SystemID);
            }
            this.StoreCodeId.DataSource = objData;
            this.StoreCodeId.DataBind();
            if (objData.Rows.Count > 0)
            {
                string codeid = objData.Rows[0][0].ToString();
                Session["CodeId"] = codeid;
                GetData(codeid);
            }
        }

        protected void RowSelect(object sender, DirectEventArgs e)
        {
            string codeid = e.ExtraParams["CodeId"];
            Session["CodeId"] = codeid;
            GetData(codeid);
        }

        protected void UpdateData(object sender, DirectEventArgs e)
        {
            Hashtable hs = new Hashtable();
            RowSelectionModel sm = this.GridPanel2.SelectionModel.Primary as RowSelectionModel;
            string Id = sm.SelectedRow.RecordID;
            hs.Add("codetype", Session["CodeId"] as string);
            hs.Add("code", TxtEd0.Text);
            hs.Add("codedesc", TxtEd1.Text);
            hs.Add("remark", TxtEd2.Text);
            hs.Add("isuse", Math.Abs(ChkEd3.Checked.CompareTo(false)).ToString());
            hs.Add("oldcode", Id);
            if (systemManager.UpdateSysCode(hs) > 0)
            {
                Notice("信息提示", "保存成功");
            }
            if (Session["CodeId"] != null)
            {
                GetData(Session["CodeId"] as string);
            }
        }

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

        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            GetCodeTypeData();
        }

        protected void StoreCode_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            if (Session["CodeId"] != null)
            {
                GetData(Session["CodeId"] as string);
            }
        }
    }
}