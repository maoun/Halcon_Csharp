using System;
using System.Collections;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PeccancyAreaChecking : System.Web.UI.Page
    {
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private Bll.LogManager logManager = new Bll.LogManager();
        private UserLogin userLogin = new UserLogin();
        private DataCommon dataCommon = new DataCommon();
        private SettingManager settingManager = new SettingManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            //判断用户是否登录
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            //判断用户是否登录结束
            if (!X.IsAjaxRequest)
            {
                //userLogin.IsLoginPage(this);
                CheckData.Value = Session["areacheckjson"] as string;
                StartDataBind();
                SetControlValue(0);
            }
        }

        private void SetControlValue(int index)
        {
            DataRow dr = GetRecInfo(index);

            DataTable dt2 = tgsDataInfo.GetPeccancyAreaInfo(" id='" + dr[0].ToString() + "'", 0, 50);//dataCommon.ChangeDataTablePoliceIp(tgsDataInfo.GetPeccancyAreaInfo(" id='" + dr[0].ToString() + "'", 0, 50), "col29", "col30", "");

            System.Data.DataTable dt = new System.Data.DataTable("MyDataTable");
            DataColumn myDataColumn;
            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "col0";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "col1";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "col2";
            dt.Columns.Add(myDataColumn);

            myDataColumn = new DataColumn();
            myDataColumn.DataType = Type.GetType("System.String");
            myDataColumn.ColumnName = "col3";
            dt.Columns.Add(myDataColumn);
            DataRow myDataRow;
            myDataRow = dt.NewRow();
            myDataRow["col0"] = dt2.Rows[0]["col29"].ToString();
            myDataRow["col1"] = "1";
            myDataRow["col2"] = "1";
            myDataRow["col3"] = dt2.Rows[0]["col29"].ToString();
            dt.Rows.InsertAt(myDataRow, 0);
            myDataRow = dt.NewRow();
            myDataRow["col0"] = dt2.Rows[0]["col30"].ToString();
            myDataRow["col1"] = "2";
            myDataRow["col2"] = "1";
            myDataRow["col3"] = dt2.Rows[0]["col30"].ToString();
            dt.Rows.InsertAt(myDataRow, 0);
            this.StoreImage.DataSource = dt;
            this.StoreImage.DataBind();
            FirstSelection(dt);
            CurrentId.Value = dr[0].ToString();
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                TxtHphm.Text = dt2.Rows[0]["col2"].ToString();
                HidHpzl.Value = dt2.Rows[0]["col3"].ToString();
                TxtHpzl.Text = dt2.Rows[0]["col4"].ToString();
                TxtWfsj.Text = dt2.Rows[0]["col20"].ToString();
                TxtWfxw.Text = dt2.Rows[0]["col9"].ToString();
                TxtWfdd.Text = dt2.Rows[0]["col33"].ToString();
                TxtQdkk.Text = dt2.Rows[0]["col16"].ToString();
                TxtSdkk.Text = dt2.Rows[0]["col18"].ToString();
                TxtQdsj.Text = dt2.Rows[0]["col19"].ToString();
                TxtSdsj.Text = dt2.Rows[0]["col20"].ToString();
                TxtXsfx.Text = dt2.Rows[0]["col6"].ToString();

                TxtSdxs.Text = dt2.Rows[0]["col14"].ToString();
                TxtQjjl.Text = dt2.Rows[0]["col10"].ToString();
                TxtQjys.Text = dt2.Rows[0]["col12"].ToString();
                TxtZcys.Text = dt2.Rows[0]["col27"].ToString();
                int sd = Convert.ToInt32(dt2.Rows[0]["col11"].ToString());
                int xs = Convert.ToInt32(dt2.Rows[0]["col13"].ToString());
                decimal s = 0;
                try
                {
                    s = Math.Round((decimal)(xs - sd) / sd, 2) ;
                }
                catch
                {
                }

                TxtCsbl.Text = Convert.ToInt32((s*100)).ToString()+"%";
                TxtShzt.Text = dt2.Rows[0]["col22"].ToString();
                string shbj = dt2.Rows[0]["col31"].ToString();
                switch (shbj)
                {
                    case "0":
                        PanAmply.Title = "当前状态:<font color='#000000'>&nbsp;&nbsp;" + dt2.Rows[0]["col22"].ToString() + "</font>";
                        break;

                    case "1":
                        PanAmply.Title = "当前状态:<font color='#008000'>&nbsp;&nbsp;" + dt2.Rows[0]["col22"].ToString() + "</font>";
                        break;

                    case "2":
                        PanAmply.Title = "当前状态:<font color='#ff0000'>&nbsp;&nbsp;" + dt2.Rows[0]["col22"].ToString() + "</font>";
                        break;
                }
            }
        }

        private void StartDataBind()
        {
            this.StorePlateType.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:140001")); // tgsPproperty.GetPalteType();
            this.StorePlateType.DataBind();

            this.StoreLocation.DataSource = tgsPproperty.GetStationInfo("a.station_type_id in (01,02,03,06,07,08)");
            this.StoreLocation.DataBind();

            this.StorePeccancyType.DataSource = GetRedisData.ChangColName(GetRedisData.GetData("Peccancy:WFXW"), true);//tgsPproperty.GetPeccancyType("isuse='1'");
            this.StorePeccancyType.DataBind();

            Session["plateHead"] = settingManager.GetConfigInfo("00", "06").Rows[0]["col3"].ToString();
        }

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

        protected void SubmitSelection(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];
        }

        protected void FirstSelection(DataTable data2)
        {
            if (data2.Rows.Count > 0)
            {
                string urls = data2.Rows[0][0].ToString();

                string js = "ShowImage(\"" + urls + "\",'1');";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
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

        [DirectMethod]
        public void FirstInfo()
        {
            SetControlValue(0);
        }

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

        [DirectMethod]
        public void NextInfo()
        {
            CurrentIndex.Value = int.Parse(CurrentIndex.Value.ToString()) + 1;
            SetControlValue(int.Parse(CurrentIndex.Value.ToString()));
        }

        [DirectMethod]
        public void EndInfo()
        {
            DataTable dt2 = ConvertData.JsonToDataTable2(CheckData.Value.ToString());
            SetControlValue(dt2.Rows.Count - 1);
        }

        protected void ButCheckOkClick(object sender, DirectEventArgs e)
        {
            Hashtable pecInfo = GetPeccancyInfo("1");
            if (tgsDataInfo.UpdateAreaPeccancyInfo(pecInfo) > 0)
            {
                NextInfo();
                string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                if (ipaddress.Length < 9)
                {
                    ipaddress = "127.0.0.1";
                }
                FrmClear();
                logManager.InsertLogRunning(UserLogin.GetUserName(), TxtHphm.Text + "违法车辆审核为有效", ipaddress, "2");
            }
        }

        protected void ButCheckNoClick(object sender, DirectEventArgs e)
        {
            Hashtable pecInfo = GetPeccancyInfo("2");
            if (tgsDataInfo.UpdateAreaPeccancyInfo(pecInfo) > 0)
            {
                NextInfo();
                string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                if (ipaddress.Length < 9)
                {
                    ipaddress = "127.0.0.1";
                }
                FrmClear();
                logManager.InsertLogRunning(UserLogin.GetUserName(), TxtHphm.Text + "违法车辆审核为无效", ipaddress, "2");
            }
        }

        protected void ButCheckRpeatClick(object sender, DirectEventArgs e)
        {
            Hashtable pecInfo = GetPeccancyInfo("3");
            if (tgsDataInfo.UpdateAreaPeccancyInfo(pecInfo) > 0)
            {
                NextInfo();
                string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                if (ipaddress.Length < 9)
                {
                    ipaddress = "127.0.0.1";
                }
                FrmClear();
                logManager.InsertLogRunning(UserLogin.GetUserName(), TxtHphm.Text + "违法车辆审核为套牌", ipaddress, "2");
            }
        }

        private Hashtable GetPeccancyInfo(string shbj)
        {
            Hashtable pecInfo = new Hashtable();
            pecInfo.Add("hphm", TxtHphm.Text.ToUpper());
            pecInfo.Add("wfsj", TxtWfsj.Text);
            pecInfo.Add("shbj", shbj);
            pecInfo.Add("shsj", string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
            //pecInfo.Add("shyh", "admin");
            pecInfo.Add("shyh", UserLogin.GetUserName());//
            pecInfo.Add("jcbj", shbj);
            pecInfo.Add("xh", CurrentId.Value.ToString());

            return pecInfo;
        }

        protected void ButQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                TxtVehCsys.Text = TxtVehClpp.Text = TxtVehCzxm.Text = TxtVehYxqz.Text = TxtVehLxdh.Text = TxtVehClzt.Text = TxtVehSyxz.Text = TxtVehCllx.Text = "";
                string plateHead = Session["plateHead"] as string;

                string url = MyNet.Atmcs.Uscmcp.Web.Properties.Settings.Default.MyNet_Atmcs_Uscmcp_Web_OtherQueryService_OtherQueryInfo;
                Vehicle vehicle = new Vehicle(url);
                VehicleInfo vehicleInfo = new VehicleInfo();
                if (this.TxtHphm.Text.Substring(0, 1) == plateHead.Substring(0, 1))
                {
                    vehicleInfo = vehicle.GetVehicleInfo(HidHpzl.Value, TxtHphm.Text.ToUpper().Substring(1));
                    if (vehicleInfo != null)
                    {
                        HidVehCsys.Value = vehicleInfo.Csysbh;
                        TxtVehCsys.Text = vehicleInfo.Csys;
                        TxtVehClpp.Text = vehicleInfo.Clpp1;
                        TxtVehCzxm.Text = vehicleInfo.Syr;
                        TxtVehYxqz.Text = vehicleInfo.Yxqz;
                        TxtVehLxdh.Text = vehicleInfo.Lxdh;
                        HidVehClzt.Value = vehicleInfo.Ztbh;
                        TxtVehClzt.Text = vehicleInfo.Zt;
                        HidVehSyxz.Value = vehicleInfo.Syxz;
                        TxtVehSyxz.Text = vehicleInfo.Syxzms;
                        HidVehCllx.Value = vehicleInfo.Cllxbh;
                        TxtVehCllx.Text = vehicleInfo.Cllx;
                    }
                }
                else
                {
                    Message("信息提示", "该车牌不是本省车牌，无法查询 ！");
                    return;
                }
            }
            catch
            {
            }
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

        private void FrmClear()
        {
            TxtVehCsys.Text = TxtVehClpp.Text = TxtVehCzxm.Text = TxtVehYxqz.Text = TxtVehLxdh.Text = TxtVehClzt.Text = TxtVehSyxz.Text = TxtVehCllx.Text = "";
        }
    }
}