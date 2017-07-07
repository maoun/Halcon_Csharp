using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using System;
using System.Collections;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PeccancySingleCheck : System.Web.UI.Page
    {
        #region 成员变量

        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private UserLogin userLogin = new UserLogin();
        private DataCommon dataCommon = new DataCommon();
        private SettingManager settingManager = new SettingManager();

        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"]; if (!userLogin.CheckLogin(username)) { string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }
            if (!X.IsAjaxRequest)
            {
                CheckData.Value = Session["checkjson"] as string;
                StartDataBind();
                SetControlValue(0);
                this.DataBind();
            }
        }

        /// <summary>
        /// 审核有效
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButCheckOkClick(object sender, DirectEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(VehicleHead.VehicleText))
                {
                    if (string.IsNullOrEmpty(txtPlateId.Text.ToUpper()))
                    {
                        Message("信息提示", "号牌号码不能为空 ！");
                        return;
                    }
                    Message("信息提示", "号牌格式不正确 ！");
                    return;
                }
                if (cmbPlateType.SelectedItem.Value == "99")
                {
                    Message("信息提示", "号牌种类非法 ！");
                    return;
                }
                Hashtable pecInfo = GetPeccancyInfo("1");
                string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                if (tgsDataInfo.UpdatePeccancyInfo(pecInfo) > 0)
                {
                    //logManager.InsertLogRunning(

                    NextInfo();

                    if (ipaddress.Length < 9)
                    {
                        ipaddress = "127.0.0.1";
                    }
                    //FrmClear();
                    logManager.InsertLogRunning(UserLogin.GetUserName(), "初审有效：号牌种类[" + cmbPlateType.SelectedItem.Text + "] ,号牌号码[" + VehicleHead.VehicleText + txtPlateId.Text + "] ,违法记录Id[" + pecInfo["xh"].ToString() + "] ,违法时间[" + pecInfo["wfsj"].ToString() + "] ,违法地点[" + cmbLocation.SelectedItem.Text + "] ,违法行为[" + cmbPeccancyType.SelectedItem.Text + "]", ipaddress, "5", VehicleHead.VehicleText + txtPlateId.Text, CurrentId.Value.ToString());
                }
                else
                {
                    logManager.InsertLogRunning(UserLogin.GetUserName(), "初审有效失败：号牌种类[" + cmbPlateType.SelectedItem.Text + "] ,号牌号码[" + VehicleHead.VehicleText + txtPlateId.Text + "] ,违法记录Id[" + pecInfo["xh"].ToString() + "] ,违法时间[" + pecInfo["wfsj"].ToString() + "] ,违法地点[" + cmbLocation.SelectedItem.Text + "] ,违法行为[" + cmbPeccancyType.SelectedItem.Text + "]", ipaddress, "5", VehicleHead.VehicleText + txtPlateId.Text, CurrentId.Value.ToString());
                }
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("PeccancySingleCheck.aspx-ButCheckOkClick", ex.Message + "；" + ex.StackTrace, "ButCheckOkClick has an exception");
            }
        }

        /// <summary>
        /// 审核无效
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButCheckNoClick(object sender, DirectEventArgs e)
        {
            try
            {
                Hashtable pecInfo = GetPeccancyInfo("2");
                string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                if (tgsDataInfo.UpdatePeccancyInfo(pecInfo) > 0)
                {
                    NextInfo();

                    if (ipaddress.Length < 9)
                    {
                        ipaddress = "127.0.0.1";
                    }

                    //FrmClear();
                    logManager.InsertLogRunning(UserLogin.GetUserName(), "初审无效：号牌种类[" + cmbPlateType.SelectedItem.Text + "] ,号牌号码[" + VehicleHead.VehicleText + txtPlateId.Text + "] ,违法记录Id[" + pecInfo["xh"].ToString() + "] ,违法时间[" + pecInfo["wfsj"].ToString() + "] ,违法地点[" + cmbLocation.SelectedItem.Text + "] ,违法行为[" + cmbPeccancyType.SelectedItem.Text + "]", ipaddress, "4", VehicleHead.VehicleText + txtPlateId.Text, CurrentId.Value.ToString());
                }
                else
                {
                    logManager.InsertLogRunning(UserLogin.GetUserName(), "初审无效失败：号牌种类[" + cmbPlateType.SelectedItem.Text + "] ,号牌号码[" + VehicleHead.VehicleText + txtPlateId.Text + "] ,违法记录Id[" + pecInfo["xh"].ToString() + "] ,违法时间[" + pecInfo["wfsj"].ToString() + "] ,违法地点[" + cmbLocation.SelectedItem.Text + "] ,违法行为[" + cmbPeccancyType.SelectedItem.Text + "]", ipaddress, "4", VehicleHead.VehicleText + txtPlateId.Text, CurrentId.Value.ToString());
                }
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("PeccancySingleCheck.aspx-ButCheckNoClick", ex.Message + "；" + ex.StackTrace, "ButCheckNoClick has an exception");
            }
        }

        /// <summary>
        /// 套牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButCheckRpeatClick(object sender, DirectEventArgs e)
        {
            Hashtable pecInfo = GetPeccancyInfo("3");
            if (tgsDataInfo.UpdatePeccancyInfo(pecInfo) > 0)
            {
                NextInfo();
                string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                if (ipaddress.Length < 9)
                {
                    ipaddress = "127.0.0.1";
                }
                FrmClear();
                logManager.InsertLogRunning(UserLogin.GetUserName(), VehicleHead.VehicleText + txtPlateId.Text + "违法车辆审核为套牌", ipaddress, "2");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitSelection(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];
        }

        /// <summary>
        /// 查询车驾管
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                //TxtVehCsys.Text = TxtVehClpp.Text = TxtVehCzxm.Text = TxtVehYxqz.Text = TxtVehLxdh.Text = TxtVehClzt.Text = TxtVehSyxz.Text = TxtVehCllx.Text= "";
                //string plateHead = Session["plateHead"] as string;
                //string url = MyNet.Atmcs.Uscmcp.Web.Properties.Settings.Default.MyNet_Web_Framework_Tgs_WebService_Service;
                //Vehicle vehicle = new Vehicle(url);
                //VehicleInfo vehicleInfo = new VehicleInfo();
                //if (this.txtPlateId.Text.Substring(0, 1) == plateHead.Substring(0, 1))
                //{
                //    vehicleInfo = vehicle.GetVehicleInfo(cmbPlateType.SelectedItem.Value, txtPlateId.Text.ToUpper().Substring(1));
                //    if (vehicleInfo != null)
                //    {
                //        HidVehCsys.Value = vehicleInfo.Csysbh;
                //        TxtVehCsys.Text = vehicleInfo.Csys;
                //        TxtVehClpp.Text = vehicleInfo.Clpp1;
                //        TxtVehCzxm.Text = vehicleInfo.Syr;
                //        TxtVehYxqz.Text = vehicleInfo.Yxqz;
                //        TxtVehLxdh.Text = vehicleInfo.Lxdh;
                //        HidVehClzt.Value = vehicleInfo.Ztbh;
                //        TxtVehClzt.Text = vehicleInfo.Zt;
                //        HidVehSyxz.Value = vehicleInfo.Syxz;
                //        TxtVehSyxz.Text = vehicleInfo.Syxzms;
                //        HidVehCllx.Value = vehicleInfo.Cllxbh;
                //        TxtVehCllx.Text = vehicleInfo.Cllx;
                //    }
                //}
                //else
                //{
                //    Message("信息提示", "该车牌不是本省车牌，无法查询 ！");
                //    return;
                //}
            }
            catch
            {
            }
        }

        #endregion 控件事件

        #region DirectMethod

        /// <summary>
        /// 第一条
        /// </summary>
        [DirectMethod]
        public void FirstInfo()
        {
            SetControlValue(0);
        }

        /// <summary>
        /// 前一条
        /// </summary>
        [DirectMethod]
        public void LastInfo()
        {
            CurrentIndex.Value = int.Parse(CurrentIndex.Value.ToString()) - 1;

            int index = int.Parse(CurrentIndex.Value.ToString());
            if (index >= 0)
            {
                SetControlValue(index);
            }
            else
            {
                CurrentIndex.Value = 0;
                SetControlValue(0);
            }
        }

        /// <summary>
        /// 下一条
        /// </summary>
        [DirectMethod]
        public void NextInfo()
        {
            CurrentIndex.Value = int.Parse(CurrentIndex.Value.ToString()) + 1;
            SetControlValue(int.Parse(CurrentIndex.Value.ToString()));
        }

        /// <summary>
        /// 最后一条
        /// </summary>
        [DirectMethod]
        public void EndInfo()
        {
            DataTable dt2 = ConvertData.JsonToDataTable2(CheckData.Value.ToString());
            SetControlValue(dt2.Rows.Count - 1);
        }

        #endregion DirectMethod

        #region 私有方法

        /// <summary>
        ///绑定数据
        /// </summary>
        /// <param name="index"></param>
        private void SetControlValue(int index)
        {
            FrmClear();
            DataRow dr = GetRecInfo(index);
            DataTable dt = new DataTable();// tgsDataInfo.GetPassCarImageUrl(dr[0].ToString());
            dt.Columns.Add("col0", typeof(string)); dt.Columns.Add("col1", typeof(string));
            dt.Columns.Add("col2", typeof(string)); dt.Columns.Add("col3", typeof(string));
            if (!string.IsNullOrEmpty(dr[23].ToString()))
            {
                dt.Rows.Add(dr[23].ToString(), "1", "1", dr[23].ToString());
            }
            else
            {
                dt.Rows.Add("../Images/NoImage.png", "1", "1", "../Images/NoImage.png");
            }
            if (!string.IsNullOrEmpty(dr[24].ToString()))
            {
                dt.Rows.Add(dr[24].ToString(), "2", "1", dr[24].ToString());
            }
            else
            {
                dt.Rows.Add("../Images/NoImage.png", "2", "1", "../Images/NoImage.png");
            }
            if (!string.IsNullOrEmpty(dr[25].ToString()))
            {
                dt.Rows.Add(dr[25].ToString(), "3", "1", dr[25].ToString());
            }
            else
            {
                dt.Rows.Add("../Images/NoImage.png", "3", "1", "../Images/NoImage.png");
            }
            //dt = dataCommon.ChangeDataTablePoliceIp(dt, "col0", "col3", "");
            this.StoreImage.DataSource = dt;
            this.StoreImage.DataBind();
            FirstSelection(dt);

            DataTable dt2 = tgsDataInfo.GetPeccancyInfo(" xh='" + dr[0].ToString() + "'", 0, 1);
            CurrentId.Value = dr[0].ToString();

            if (dt2 != null && dt2.Rows.Count > 0)
            {
                string txzXinxi = dt2.Rows[0]["col1"].ToString() + dt2.Rows[0]["col3"].ToString();
                DataTable dt3 = GetRedisData.GetData("Passport:" + txzXinxi);//得到车的通行证信息
                DataTable dtTxz = null;
                if (dt3 != null)
                {
                    dtTxz = MyNet.Atmcs.Uscmcp.Bll.Common.ChangColName(dt3);
                    if (!string.IsNullOrEmpty(dtTxz.Rows[0]["col2"].ToString()) && !string.IsNullOrEmpty(dtTxz.Rows[0]["col3"].ToString()))
                    {
                        txtQzsj.Text = Convert.ToDateTime(dtTxz.Rows[0]["col2"].ToString()).ToString("yyyy-MM-dd") + "--" + Convert.ToDateTime(dtTxz.Rows[0]["col3"].ToString()).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        txtQzsj.Text = "";
                    }
                    if (!string.IsNullOrEmpty(dtTxz.Rows[0]["col5"].ToString()))
                    {
                        txtTxld.Text = dtTxz.Rows[0]["col5"].ToString();
                    }
                    else
                    {
                        txtTxld.Text = "";
                    }
                    if (!string.IsNullOrEmpty(dtTxz.Rows[0]["col4"].ToString()))
                    {
                        txtTxsd.Text = dtTxz.Rows[0]["col4"].ToString();
                    }
                    else
                    {
                        txtTxsd.Text = "";
                    }
                    lbTxzzt.Text = "取到通行证";
                    lbTxzzt.StyleSpec = "color:green;";
                }
                else
                {
                    lbTxzzt.Text = "无通行证";
                    lbTxzzt.StyleSpec = "color:red;";
                }
                if (dt2.Rows[0]["col3"].ToString().Length < 7)
                {
                    string Hphm = Session["plateHead"] as string;
                    if (Hphm.Contains(","))
                    {
                        string[] strs = Hphm.Split(',');
                        Hphm = strs[0];
                    }
                    else
                    {
                    }
                    VehicleHead.SetVehicleText(Hphm.Substring(0, 1));
                    txtPlateId.Text = Hphm.Substring(1);
                }
                else
                {
                    string Hphm = dt2.Rows[0]["col3"].ToString();
                    VehicleHead.SetVehicleText(Hphm.Substring(0, 1));
                    txtPlateId.Text = Hphm.Substring(1);
                }
                cmbPlateType.SelectedItem.Value = dt2.Rows[0]["col1"].ToString();
                cmbPlateType.SelectedItem.Text = dt2.Rows[0]["col2"].ToString();
                TxtPeccancyDate.Text = dt2.Rows[0]["col6"].ToString();
                cmbPeccancyType.SelectedItem.Value = dt2.Rows[0]["col4"].ToString();
                cmbLocation.SelectedItem.Value = dt2.Rows[0]["col7"].ToString();
                TxtSpeed.Text = dt2.Rows[0]["col12"].ToString();

                HidVehCsys.Value = dt2.Rows[0]["col42"].ToString();
                TxtVehCsys.Text = dt2.Rows[0]["col43"].ToString();
                TxtVehClpp.Text = dt2.Rows[0]["col39"].ToString();
                TxtVehCzxm.Text = dt2.Rows[0]["col34"].ToString();
                TxtVehYxqz.Text = dt2.Rows[0]["col46"].ToString();
                TxtVehLxdh.Text = dt2.Rows[0]["col35"].ToString();
                HidVehClzt.Value = dt2.Rows[0]["col44"].ToString();
                TxtVehClzt.Text = dt2.Rows[0]["col45"].ToString();
                HidVehSyxz.Value = dt2.Rows[0]["col51"].ToString();
                TxtVehSyxz.Text = dt2.Rows[0]["col52"].ToString();
                HidVehCllx.Value = dt2.Rows[0]["col40"].ToString();
                TxtVehCllx.Text = dt2.Rows[0]["col41"].ToString();
                string shbj = dt2.Rows[0]["col30"].ToString();
                switch (shbj)
                {
                    case "0":
                        PanAmply.Title = "当前状态:<font color='#000000'>&nbsp;&nbsp;" + dt2.Rows[0]["col15"].ToString() + "</font>";
                        break;

                    case "1":
                        PanAmply.Title = "当前状态:<font color='#008000'>&nbsp;&nbsp;" + dt2.Rows[0]["col15"].ToString() + "</font>";
                        break;

                    case "2":
                        PanAmply.Title = "当前状态:<font color='#ff0000'>&nbsp;&nbsp;" + dt2.Rows[0]["col15"].ToString() + "</font>";
                        break;
                }
            }
        }

        /// <summary>
        ///开始绑定数据
        /// </summary>
        private void StartDataBind()
        {
            //UserInfo userInfo = System.Web.HttpContext.Current.Session["userinfo"] as UserInfo;

            //DataTable ddbh = new DataTable();
            //ddbh = tgsPproperty.GetUserStationInfo(userInfo.UserCode, "1");
            //if (ddbh == null || ddbh.Rows.Count <= 0)
            //{
            //    ddbh = tgsPproperty.GetStationInfo("b.istmsshow ='1'");
            //}
            //this.StoreLocation.DataSource = ddbh;
            //this.StoreLocation.DataBind();
            DataTable dt1 = GetRedisData.GetData("Station:t_cfg_set_station_type_istmsshow");
            if (dt1 != null)
            {
                this.StoreLocation.DataSource = GetRedisData.ChangColName(dt1, true);
                this.StoreLocation.DataBind();
            }
            else
            {
                this.StoreLocation.DataSource = tgsPproperty.GetStationInfo("b.istmsshow ='1'");
                this.StoreLocation.DataBind();
            }

            //车俩类型
            DataTable dt2 = GetRedisData.GetData("t_sys_code:140001");
            if (dt2 != null)
            {
                this.StorePlateType.DataSource = GetRedisData.ChangColName(dt2, true);
                this.StorePlateType.DataBind();
            }
            else
            {
                this.StorePlateType.DataSource = tgsPproperty.GetPalteType();
                this.StorePlateType.DataBind();
            }
            DataTable dt3 = GetRedisData.GetData("Peccancy:WFXW");
            if (dt3 != null)
            {
                this.StorePeccancyType.DataSource = GetRedisData.ChangColName(dt3, true);
                this.StorePeccancyType.DataBind();
            }
            if (settingManager.GetConfigInfo("00", "06") != null)
            {
                Session["plateHead"] = settingManager.GetConfigInfo("00", "06").Rows[0]["col3"].ToString();
            }
        }

        public DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private DataRow GetRecInfo(int index)
        {
            DataTable dt2 = ConvertData.JsonToDataTable2(CheckData.Value.ToString());
            if (index >= dt2.Rows.Count)
            {
                index = dt2.Rows.Count - 1;
            }
            if (index < 0)
            {
                index = 0;
            }
            CurrentIndex.Value = index;
            PeccancyCheckPanel.Title = "违法车辆审核   - 当前是【" + (index + 1).ToString() + "】 - 当前审核总计【" + dt2.Rows.Count + "】条";

            return dt2.Rows[index];
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="data2"></param>
        protected void FirstSelection(DataTable data2)
        {
            if (data2.Rows.Count > 0)
            {
                string urls = data2.Rows[0][0].ToString();

                string js = "ShowImage(\"" + urls + "\",'1');";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
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
        /// 组装违法信息
        /// </summary>
        /// <param name="shbj"></param>
        /// <returns></returns>
        private Hashtable GetPeccancyInfo(string shbj)
        {
            UserInfo userInfo = System.Web.HttpContext.Current.Session["userinfo"] as UserInfo;
            Hashtable pecInfo = new Hashtable();
            pecInfo.Add("hphm", VehicleHead.VehicleText + txtPlateId.Text.ToUpper());
            if (cmbPlateType.SelectedIndex != -1)
            {
                pecInfo.Add("hpzl", cmbPlateType.SelectedItem.Value);
            }

            if (cmbPeccancyType.SelectedIndex != -1)
            {
                pecInfo.Add("wfxw", cmbPeccancyType.SelectedItem.Value);
                //pecInfo.Add("wfxwmc", cmbPeccancyType.SelectedItem.Text);
            }
            if (cmbLocation.SelectedIndex != -1)
            {
                pecInfo.Add("kkid", cmbLocation.SelectedItem.Value);
                //pecInfo.Add("kkidmc", cmbLocation.SelectedItem.Text);
            }
            pecInfo.Add("wfsj", TxtPeccancyDate.Text);
            pecInfo.Add("shbj", shbj);
            pecInfo.Add("shsj", string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
            pecInfo.Add("shyh", userInfo.UserCode);//
            if (shbj.Equals("2"))
            {
                pecInfo.Add("jcbj", "2");
            }
            else
            {
                pecInfo.Add("jcbj", "1");
            }
            pecInfo.Add("xh", CurrentId.Value.ToString());

            if (!string.IsNullOrEmpty(this.TxtVehCzxm.Text))
            {
                pecInfo.Add("jdssyr", this.TxtVehCzxm.Text);
            }
            if (!string.IsNullOrEmpty(this.HidVehCsys.Value))
            {
                pecInfo.Add("csys", this.HidVehCsys.Value);
            }
            if (!string.IsNullOrEmpty(this.HidVehSyxz.Value))
            {
                pecInfo.Add("syxz", this.HidVehSyxz.Value);
            }
            if (!string.IsNullOrEmpty(this.TxtVehYxqz.Text))
            {
                pecInfo.Add("jyyxqz", this.TxtVehYxqz.Text);
            }
            if (!string.IsNullOrEmpty(this.TxtVehLxdh.Text))
            {
                pecInfo.Add("dh", this.TxtVehLxdh.Text);
            }
            if (!string.IsNullOrEmpty(this.HidVehCllx.Value))
            {
                pecInfo.Add("cllx", this.HidVehCllx.Value);
            }
            if (!string.IsNullOrEmpty(this.HidVehClzt.Value))
            {
                pecInfo.Add("zt", this.HidVehClzt.Value);
            }
            if (!string.IsNullOrEmpty(this.TxtVehClpp.Text))
            {
                pecInfo.Add("clpp", this.TxtVehClpp.Text);
            }

            return pecInfo;
        }

        /// <summary>
        ///清空数据
        /// </summary>
        private void FrmClear()
        {
            //清空车牌号码
            VehicleHead.SetVehicleText("");
            txtPlateId.Text = "";
            //号牌种类
            cmbPlateType.Reset();
            //违法时间
            TxtPeccancyDate.Text = "";
            //违法地点
            cmbLocation.Reset();
            //违法行为
            cmbPeccancyType.Reset();
            //速度与限速
            TxtSpeed.Text = "";
            TxtVehCsys.Text = TxtVehClpp.Text = TxtVehCzxm.Text = TxtVehYxqz.Text = TxtVehLxdh.Text = TxtVehClzt.Text = TxtVehSyxz.Text = TxtVehCllx.Text = txtQzsj.Text = txtTxld.Text = txtTxsd.Text = "";
        }

        #endregion 私有方法

        #region 语言转换

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

        #endregion 语言转换
    }
}