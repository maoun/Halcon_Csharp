using System;
using System.Collections.Generic;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Web.Template
{
    public partial class WebDeviceShow : System.Web.UI.Page
    {
        private SystemManager systemManager = new SystemManager();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private MyNet.Atmcs.Uscmcp.Bll.DeviceManager deviceManager = new MyNet.Atmcs.Uscmcp.Bll.DeviceManager();
        private static string device_id;
        private static string starttime = "";
        private static string endtime = "";
        private UserLogin userLogin = new UserLogin();

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
                if (Session["Devices"] != null)
                {
                    List<string> list = (List<string>)Session["Devices"];
                    device_id = list[7];
                    TxtDeviceName.Text = list[1];
                    ComboBox1.SetValue(list[3]);
                    ComboBox2.SetValue(list[4]);
                    CmbDeviceType.SetValue(list[5]);
                    endtime = end.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    starttime = start.InnerText = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
                    DataTable da = deviceManager.GetDeviceTypeMode("device_type_id='" + list[5] + "'");
                    this.StoreSDeviceMode.DataSource = da;
                    this.StoreSDeviceMode.DataBind();
                    CmbSBXH.SetValue(list[6]);
                }

                StoreDataBind();
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
                DataTable dt = deviceManager.GetHistoryDevice(" AND tddsh.device_id ='" + device_id + "'  " + GetWhere());
                this.StoreDevice.DataSource = dt;
                this.StoreDevice.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebDeviceShow.aspx-TbutQueryClick", ex.Message+"；"+ex.StackTrace, "TbutQueryClick has an exception");
            }
        }

        public string GetWhere()
        {
            string where = string.Empty;
            //if (!string.IsNullOrEmpty(TxtDeviceName.Text))
            //{
            //    where = where + "  And  tddi.device_name='" + TxtDeviceName.Text + "'";
            //}
            if (!string.IsNullOrEmpty(CmbDeviceType.Text))
            {
                where = where + "  And tddi.device_type_id='" + CmbDeviceType.Value + "'";
            }
            if (!string.IsNullOrEmpty(ComboBox1.Text))
            {
                where = where + "  And  tddsh.connect_state='" + ComboBox1.Value + "'";
            }
            if (!string.IsNullOrEmpty(ComboBox2.Text))
            {
                where = where + "  And  tddsh.work_state='" + ComboBox2.Value + "'";
            }
            if (!string.IsNullOrEmpty(CmbSBXH.Text))
            {
                where = where + "  And  tddi.device_mode_id='" + CmbSBXH.Value + "'";
            }
            if (!string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime))
            {
                where = where + "  And  (tddsh.update_time BETWEEN  '" + starttime + "' and  '" + endtime + "'  )";
                return where;
            }
            else
            {
                if (!string.IsNullOrEmpty(starttime))
                {
                    where = where + "  And  (tddsh.update_time='" + starttime + "'  )";
                    return where;
                }
                else
                {
                    where = where + "  And  (tddsh.update_time='" + endtime + "'  )";
                    return where;
                }
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            //CmbDeviceType.Reset();
            //TxtDeviceName.Reset();
            //ComboBox1.Reset();
            //ComboBox2.Reset();
            //CmbSBXH.Reset();
            TxtDeviceName.Text = "";
            CmbDeviceType.Text = "";
            ComboBox1.Text = "";
            ComboBox2.Text = "";
            CmbSBXH.Text = "";
            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            starttime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss");
            string timeJs = "clearTime('" + starttime + "','" + endtime + "');";//js方法后面的分号一定要加上
            this.ResourceManager1.RegisterAfterClientInitScript(timeJs);
        }

        private void StoreDataBind()
        {
            try
            {
                this.StoreDeviceType.DataSource = deviceManager.GetDeviceType();
                this.StoreDeviceType.DataBind();
                DataTable dt = deviceManager.GetHistoryDevice(" AND tddsh.device_id ='" + device_id + "' and (tddsh.update_time BETWEEN DATE_SUB(SYSDATE(),INTERVAL 7 DAY) AND SYSDATE()  ) ");
                // DataTable dt = deviceManager.GetDevice("1=1");
                this.StoreDevice.DataSource = dt;
                this.StoreDevice.DataBind();
                this.StoreConnectState.DataSource = GetRedisData.GetData("t_sys_code:240006"); ;
                this.StoreConnectState.DataBind();
                this.StoreWorkState.DataSource = GetRedisData.GetData("t_sys_code:410100"); ;
                this.StoreWorkState.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebDeviceShow.aspx-StoreDataBind", ex.Message+"；"+ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        [DirectMethod]
        public void SelectQDevice()
        {
            try
            {
                string type = CmbDeviceType.SelectedItem.Value;
                DataTable da = deviceManager.GetDeviceTypeMode("device_type_id='" + type + "'");
                this.StoreSDeviceMode.DataSource = da;
                this.StoreSDeviceMode.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("WebDeviceShow.aspx-SelectQDevice", ex.Message+"；"+ex.StackTrace, "SelectQDevice has an exception");
            }
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
    }
}