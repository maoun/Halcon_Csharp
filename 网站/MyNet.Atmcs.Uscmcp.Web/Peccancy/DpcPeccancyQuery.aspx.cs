using System;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class DpcPeccancyQuery : System.Web.UI.Page
    {
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private UserManager userManager = new UserManager();
        private DataCommon dataCommon = new DataCommon();
        private const string NoImageUrl = "../images/NoImage.png";
        private UserLogin userLogin = new UserLogin();

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"]; if (!userLogin.CheckLogin(username)) { string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" +StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }
            if (!X.IsAjaxRequest)
            {
                this.StorePlateType.DataSource = GetRedisData.GetData("t_sys_code:140001"); //tgsPproperty.GetPalteType();
                this.StorePlateType.DataBind();

                this.StoreLocation.DataSource = GetRedisData.GetData("Station:t_cfg_set_station_type_istmsshow");
                this.StoreLocation.DataBind();


                this.StorePecType.DataSource = GetRedisData.ChangColName(GetRedisData.GetData("Peccancy:WFXW"), true);
                this.StorePecType.DataBind();

                DataTable dtCaptureUser = GetRedisData.ChangColName(GetRedisData.GetData("Peccancy:CaptureUser"), true);
                this.StoreCaptureUser.DataSource = dtCaptureUser;//userManager.GetSerUserInfo(SystemID, " 1=1 ", "");
                this.StoreCaptureUser.DataBind();


                DataTable deal = GetRedisData.GetData("t_sys_code:240019");
                this.StoreDealType.DataSource = deal;
                this.StoreDealType.DataBind();

                DataTable dt = tgsPproperty.GetQueryNum();
                this.StoreQueryNum.DataSource = dt;
                this.StoreQueryNum.DataBind();
                this.DateStartTime.SelectedDate = DateTime.Now;
                this.DateEndTime.SelectedDate = DateTime.Now;
                this.TimeStart.Text = DateTime.Now.ToString("00:00:01");
                this.TimeEnd.Text = DateTime.Now.ToString("23:59:59");

                //ButCsv.Disabled = true;
                ButExcel.Disabled = true;
               // ButXml.Disabled = true;
               // ButPrint.Disabled = true;

                if (dt.Rows.Count > 0)
                    CmbQueryNum.SelectedIndex = 0;
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
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            this.StorePeccancy.DataSource = GetData();
            this.StorePeccancy.DataBind();
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            this.DateStartTime.SelectedDate = DateTime.Now;
            this.DateEndTime.SelectedDate = DateTime.Now;
            this.TimeStart.Text = DateTime.Now.ToString("00:00:01");
            this.TimeEnd.Text = DateTime.Now.ToString("23:59:59");
            this.CmbPecType.Reset();
            this.CmbPlateType.Reset();
            this.CmbCaptureUser.Reset();
            this.CmbLocation.Reset();
            this.TxtplateId.Reset();
            this.ChkLike.Reset();
            this.CmbDealType.Reset();
        }

        /// <summary>
        /// 显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowDetails(object sender, DirectEventArgs e)
        {
            string data = e.ExtraParams["data"];
            AddWindow(data);
        }

        /// <summary>
        /// 获得查询数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetData()
        {
            DataTable dt = tgsDataInfo.GetPeccancyInfo(GetWhere(), 1, 500);
            Session["datatable"] = dt;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                   // ButCsv.Disabled = false;
                    ButExcel.Disabled = false;
                    //ButXml.Disabled = false;
                    //ButPrint.Disabled = false;
                }
            }

            return dt;
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            string where = " sjly=4  ";
            string startTime = DateStartTime.SelectedDate.ToString("yyyy-MM-dd") + " " + this.TimeStart.Text;
            string endTime = DateEndTime.SelectedDate.ToString("yyyy-MM-dd") + " " + this.TimeEnd.Text;
            where = where + " and  wfsj >= STR_TO_DATE('" + startTime + "','%Y-%m-%d %H:%i:%s')   and wfsj<=STR_TO_DATE('" + endTime + "','%Y-%m-%d %H:%i:%s')";

            if (CmbPlateType.SelectedIndex != -1)
            {
                where = where + " and  hpzl='" + CmbPlateType.SelectedItem.Value + "' ";
            }

            if (CmbLocation.SelectedIndex != -1)
            {
                where = where + " and  wfdd='" + CmbLocation.SelectedItem.Value + "' ";
            }

            if (CmbPecType.SelectedIndex != -1)
            {
                where = where + " and  wfxw='" + CmbPecType.SelectedItem.Value + "' ";
            }

            if (CmbDealType.SelectedIndex != -1)
            {
                where = where + " and  jcbj='" + CmbDealType.SelectedItem.Value + "' ";
            }

            if (CmbCaptureUser.SelectedIndex != -1)
            {
                where = where + " and  cjyh='" + CmbCaptureUser.SelectedItem.Value + "' ";
            }
            string QueryHphm = string.Empty;
            if (!string.IsNullOrEmpty(WindowEditor1.VehicleText))
            {
                QueryHphm = WindowEditor1.VehicleText + TxtplateId.Text;
            }
            else
            {
                QueryHphm = TxtplateId.Text;
            }
            if (ChkLike.Checked)
            {
                if (!string.IsNullOrEmpty(QueryHphm))
                {
                    where = where + " and  hphm  like '%" + QueryHphm.ToUpper() + "%' ";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(QueryHphm))
                {
                    where = where + " and  hphm='" + QueryHphm.ToUpper() + "' ";
                }
            }
            if (CmbQueryNum.SelectedIndex != -1)
            {
                where = where + " limit   0," + CmbQueryNum.SelectedItem.Value;
            }
            return where;
        }

        /// <summary>
        /// 转换datatable
        /// </summary>
        /// <returns></returns>
        private DataTable ChangeDataTable()
        {
            DataTable dt = Session["datatable"] as DataTable;
            DataTable dt2 = null; ;
            if (dt != null)
            {
                //PrintColumns pc = new PrintColumns();
                //pc.Add(new PrintColumn("违法地点", 8));
                //pc.Add(new PrintColumn("号牌号码", 3));
                //pc.Add(new PrintColumn("号牌种类", 2));
                //pc.Add(new PrintColumn("违法时间", 6));
                //pc.Add(new PrintColumn("违法行为", 5));
                //pc.Add(new PrintColumn("通知状态", 20));
                //pc.Add(new PrintColumn("速度限速", 12));
                //pc.Add(new PrintColumn("抓拍用户", 13));
                //pc.Add(new PrintColumn("所属机构", 14));
                //dt2 = Bll.Common.GetDataTablePrint(dt, pc);
            }

            return dt2;
        }

        /// <summary>
        /// 行选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApplyClick(object sender, DirectEventArgs e)
        {
            this.FormPanel1.Collapsed = false;
            string sdata = e.ExtraParams["data"];
            string hphm = Bll.Common.GetdatabyField(sdata, "col3");
            string hpzl = Bll.Common.GetdatabyField(sdata, "col1");
            DataTable dt = tgsDataInfo.GetPassCarImageUrl(Bll.Common.GetdatabyField(sdata, "col0"));
            string url1 = GetUrl(dt, "1");
            string url2 = GetUrl(dt, "2");
            string url3 = GetUrl(dt, "3");
            if (string.IsNullOrEmpty(url2))
            {
                url2 = NoImageUrl;
            }
            if (string.IsNullOrEmpty(url3))
            {
                url3 = NoImageUrl;
            }
            string js = "ShowImage(\"" + dataCommon.ChangePoliceIp(url1) + "\",\"" + dataCommon.ChangePoliceIp(url2) + "\",\"" + dataCommon.ChangePoliceIp(url3) + "\",\"" + hphm + "\",\"" + hpzl + "\");";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }

        private string GetUrl(DataTable dt, string idx)
        {
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][1].ToString() == idx)
                    {
                        return dt.Rows[i][0].ToString();
                    }
                }
                return "";
            }
            else
            {
                return "";
            }
        }

        private void AddWindow(string sdata)
        {
            //DataTable dt = tgsDataInfo.GetPassCarImageUrl(Bll.Common.GetdatabyField(sdata, "col0"));
            //Window win = WindowShow.AddPeccancy(sdata, dt);
            //win.Render(this.Form);
            //win.Show();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            this.StorePeccancy.DataSource = GetData();
            this.StorePeccancy.DataBind();
        }

        /// <summary>
        /// 打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButPrintClick(object sender, DirectEventArgs e)
        {
            DataTable dt = ChangeDataTable();
            if (dt != null)
            {
                Session["printdatatable"] = ChangeDataTable();
                string xml = Bll.Common.GetPrintXml("违法车辆查询信息列表", "", "", "printdatatable");
                string js = "OpenPrintPageH(\"" + xml + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
        }

        /// <summary>
        /// 导出为xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToXml(object sender, EventArgs e)
        {
            DataTable dt = ChangeDataTable();
            ConvertData.ExportXml(dt, this);
        }

        /// <summary>
        /// 导出为excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToExcel(object sender, EventArgs e)
        {
            DataTable dt = ChangeDataTable();
            ConvertData.ExportExcel(dt, this);
        }

        /// <summary>
        /// 导出为csv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToCsv(object sender, EventArgs e)
        {
            DataTable dt = ChangeDataTable();
            ConvertData.ExportCsv(dt, this);
        }
    }
}