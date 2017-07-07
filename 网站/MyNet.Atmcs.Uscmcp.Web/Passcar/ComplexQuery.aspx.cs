using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class ComplexQuery : System.Web.UI.Page
    {
        #region 成员变量
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        private OtherQueryService.OtherQueryInfo client = new OtherQueryService.OtherQueryInfo();
        private Vehicle vehicle = null;
        private string url = "";
        private SettingManager settingManager = new SettingManager();
        private UserLogin userLogin = new UserLogin();
        private TgsPproperty tgsPproperty = new TgsPproperty();

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
                StoreDataBind();
                string hphm = "";
                string hpzl = "";
                if (Session["Condition"] != null)
                {
                    Condition con = Session["Condition"] as Condition;
                    hphm = con.Sqjc + con.Hphm;
                    hpzl = con.Hpzl;
                }
                else
                {
                    hphm = Request.QueryString["hphm"];
                    hpzl = Request.QueryString["hpzl"];
                }
                if (!string.IsNullOrEmpty(hphm) && !string.IsNullOrEmpty(hpzl))
                {
                    Query(hphm, hpzl);
                }
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "访问：" + Request.QueryString["funcname"], userinfo.NowIp, "0");
            }
        }

        /// <summary>
        /// Tbuts the query click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            string hpzl = "";
            string hphm = string.Empty;
            if (!string.IsNullOrEmpty(WindowEditor1.VehicleText))
            {
                hphm = WindowEditor1.VehicleText + TxtplateId.Text;
            }
            else
            {
                hphm = TxtplateId.Text;
            }
            if (CmbPlateType.SelectedIndex != -1)
            {
                hpzl = CmbPlateType.SelectedItem.Value;
            }
            Query(hphm, hpzl);
        }

        /// <summary>
        /// Buts the reset click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            this.TxtplateId.Reset();
        }

        #endregion 控件事件

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="type"></param>
        [DirectMethod]
        public void ShowEvent(string type)
        {
            string js = "";
            switch (type)
            {
                case "1":
                    js = "OpenQueryPage('../Passcar/DriverRelationAnalysis.aspx')";
                    break;

                case "2":
                    js = "OpenQueryPage('../Passcar/VehicleRelationAnalysis.aspx')";
                    break;

                case "3":
                    js = "OpenQueryPage('../Passcar/PeccancyInfoQuery.aspx')";
                    break;

                case "4":
                    js = "OpenQueryPage('../Map/FootHold.aspx')";
                    break;
            }
            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }

        #region 私有方法

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        /// <returns></returns>
        private void StoreDataBind()
        {
            try
            {
                DataTable dt1 = GetRedisData.GetData("t_sys_code:140001");
                if (dt1 != null)
                {
                    this.StorePlateType.DataSource = Bll.Common.ChangColName(dt1);
                    this.StorePlateType.DataBind();
                }
                else
                {
                    this.StorePlateType.DataSource = tgsPproperty.GetPalteType();
                    this.StorePlateType.DataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        private void Query(string hphm, string hpzl)
        {
            if (!string.IsNullOrEmpty(hphm) && !string.IsNullOrEmpty(hpzl))
            {
                WindowEditor1.SetVehicleText(hphm.Substring(0, 1));
                TxtplateId.Text = hphm.Substring(1);
                CmbPlateType.Value = hpzl;
                string plateHead = settingManager.GetConfigInfo("00", "06").Rows[0]["col3"].ToString();
                if (hphm.Substring(0, 1).Equals(plateHead.Substring(0, 1)))
                {
                    url = client.Url;
                    vehicle = new Vehicle(url);
                    VehicleInfo vehicleInfo = vehicle.GetVehicleInfo(hpzl, hphm.Substring(1));
                    if (vehicleInfo != null)
                    {
                        txtClpp.Text = vehicleInfo.Clpp1;
                        txtCsys.Text = vehicleInfo.Csys;
                        txtCllx.Text = vehicleInfo.Cllx;
                        txtSyxz.Text = vehicleInfo.Syxzms;
                        txtClzt.Text = vehicleInfo.Zt;
                        txtFzjg.Text = vehicleInfo.Fzjg;
                        txtSyr.Text = vehicleInfo.Syr;
                        txtLxdh.Text = vehicleInfo.Lxdh;
                        txtYxqz.Text = vehicleInfo.Yxqz;
                        txtXxdz.Text = vehicleInfo.Zsxxdz;
                        if (vehicleInfo.Ztbh.Equals("A"))
                        {
                            txtClzt.StyleSpec = "color:blue";
                        }
                        else if (vehicleInfo.Ztbh.Equals("B"))
                        {
                        }
                        else
                        {
                            txtClzt.StyleSpec = "color:red";
                        }
                    }
                }
                else
                {
                    Notice("信息提示", "该车牌不是本省车牌，无法获取车驾管信息");
                }
                panelChart.AutoLoad.Url = "../Template/WebComplexPieData.aspx?hphm=" + hphm + "&hpzl=" + hpzl;
                panelChart.Reload();
            }
            else
            {
                Notice("信息提示", "不存在号牌信息，无法获取车驾管信息");
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

        #endregion 私有方法
    }
}