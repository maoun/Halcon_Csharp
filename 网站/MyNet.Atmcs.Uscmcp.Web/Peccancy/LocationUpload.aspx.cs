using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class LocationUpload : System.Web.UI.Page
    {
        #region 成员变量

        private SystemManager systemManager = new SystemManager();
        private SettingManager settingManager = new SettingManager();
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
            string username = Request.QueryString["username"]; if (!userLogin.CheckLogin(username)) { string js = "alert('" + GetLangStr("LocationUpload19", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }
            if (!X.IsAjaxRequest)
            {
                GetLocationData();
                this.DataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("LocationUpload20", "访问：违法数据上传配置"), userinfo.NowIp, "0");

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
                logManager.InsertLogError("LocationUpload.aspx-TbutFisrt", ex.Message+"；"+ex.StackTrace, "TbutFisrt has an exception");
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
                logManager.InsertLogError("LocationUpload.aspx-TbutLast", ex.Message+"；"+ex.StackTrace, "TbutLast has an exception");
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
                logManager.InsertLogError("LocationUpload.aspx-TbutNext", ex.Message+"；"+ex.StackTrace, "TbutNext has an exception");
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
                logManager.InsertLogError("LocationUpload.aspx-TbutEnd", ex.Message+"；"+ex.StackTrace, "TbutEnd has an exception");
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
                logManager.InsertLogError("LocationUpload.aspx-ShowQuery", ex.Message+"；"+ex.StackTrace, "ShowQuery has an exception");
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
                DataTable objData = settingManager.GetLocationInfoNew("00", startNum.ToString(), endNum.ToString());
                this.StoreLocationInfo.DataSource = objData;
                this.StoreLocationInfo.DataBind();
                if (objData.Rows.Count > 0)
                {
                    string id = objData.Rows[0][0].ToString();
                    string name = objData.Rows[0][1].ToString();
                    Session["Locationid"] = id;
                    Session["LocationName"] = name;
                }
                AddDepartment();

                if (objData != null && objData.Rows.Count > 0)
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
                    Notice(GetLangStr("LocationUpload21", "信息提示"), GetLangStr("LocationUpload22", "未查询到相关记录"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LocationUpload.aspx-Query", ex.Message+"；"+ex.StackTrace, "Query has an exception");
            }
        }

        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
                GetLocationData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LocationUpload.aspx-MyData_Refresh", ex.Message+"；"+ex.StackTrace, "MyData_Refresh has an exception");
            }
        }

        /// <summary>
        /// 选中数据行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RowSelect(object sender, DirectEventArgs e)
        {
            try
            {
                string id = e.ExtraParams["id"];
                string name = e.ExtraParams["name"];
                Session["Locationid"] = id;
                Session["LocationName"] = name;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LocationUpload.aspx-RowSelect", ex.Message+"；"+ex.StackTrace, "RowSelect has an exception");
            }
        }

        /// <summary>
        /// 更新地点信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateLocationData(object sender, DirectEventArgs e)
        {
            try
            {
                Hashtable hs = new Hashtable();
                RowSelectionModel sm = this.GridLocation.SelectionModel.Primary as RowSelectionModel;
                string Id = sm.SelectedRow.RecordID;
                hs.Add("location_id", Id);
                hs.Add("location_name", TxtLocationName.Text);
                hs.Add("location_section", TxtLocationSection.Text);
                hs.Add("location_road", TxtLocationRoad.Text);
                hs.Add("location_kilometer", TxtLocationKiloMeter.Text);
                hs.Add("location_police", TxtLocationPolice.Text);
                hs.Add("areaid", TxtAreaID.Text);
                hs.Add("location_device", TxtLocationDevice.Text);
                if (CmbDepartment.SelectedIndex != -1)
                {
                    hs.Add("departid", CmbDepartment.Value);
                }
                if (settingManager.UpdatePeccLocationInfo(hs) > 0)
                {
                    Notice(GetLangStr("LocationUpload21", "信息提示"), GetLangStr("LocationUpload23", "更新成功"));
                }
                GetLocationData();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LocationUpload.aspx-UpdateLocationData", ex.Message+"；"+ex.StackTrace, "UpdateLocationData has an exception");
            }
        }

        #endregion 控件事件

        #region 私有方法

        /// <summary>
        /// 查询地点信息
        /// </summary>
        private void GetLocationData()
        {
            try
            {
                string rownum = "15";
                DataTable dtCount = settingManager.GetLocationInfoCount("00");//获得总记录
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
                    Notice(GetLangStr("LocationUpload21", "信息提示"), GetLangStr("LocationUpload22", "未查询到相关记录"));
                }
                //DataTable objData = settingManager.GetLocationInfo("00");
                //this.StoreLocationInfo.DataSource = objData;
                //this.StoreLocationInfo.DataBind();
                //if (objData.Rows.Count > 0)
                //{
                //    string id = objData.Rows[0][0].ToString();
                //    string name = objData.Rows[0][1].ToString();
                //    Session["Locationid"] = id;
                //    Session["LocationName"] = name;
                //}
                //AddDepartment();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LocationUpload.aspx-GetLocationData", ex.Message+"；"+ex.StackTrace, "GetLocationData has an exception");
            }
        }

        /// <summary>
        /// 绑定部门信息
        /// </summary>
        private void AddDepartment()
        {
            try
            {
                DataTable objData = settingManager.GetDepartmentDict("00");
                this.StoreCombo.DataSource = objData;
                this.StoreCombo.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LocationUpload.aspx-AddDepartment", ex.Message+"；"+ex.StackTrace, "AddDepartment has an exception");
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
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.BottomRight,
                    OffsetY = -60
                },
                Html = "<br></br>" + msg + "!"
            });
        }

        private void Message(string title, string msg)
        {
            X.Msg.Show(new MessageBoxConfig
            {
                Title = title,
                Message = msg,
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), "WARNING")
            });
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
                    page = 0;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("LocationUpload.aspx-SetButState", ex.Message+"；"+ex.StackTrace, "SetButState has an exception");
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