using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;
using System.Xml;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PassCarPathQuery : System.Web.UI.Page
    {
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();
        private DataCommon dataCommon = new DataCommon();
        private static DataTable dtPath = null;
        private static DataTable dtStation = null;
        private static DataTable dtXsfx = null;
        private static MyNet.Atmcs.Uscmcp.Web.QueryService.querypasscar client = new MyNet.Atmcs.Uscmcp.Web.QueryService.querypasscar();
        private MapManager mapManager = new MapManager();

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
                //this.StorePlateType.DataSource = tgsPproperty.GetPalteType();
                //this.StorePlateType.DataBind();
                this.StorePlateType.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:140001"));
                this.StorePlateType.DataBind();
                dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
                dtXsfx = GetRedisData.GetData("t_sys_code:240025");//mapManager.GetFxcode();
                this.DateStartTime.SelectedDate = DateTime.Now;
                this.DateEndTime.SelectedDate = DateTime.Now;
                this.TimeStart.Text = DateTime.Now.ToString("00:00:00");
                this.TimeEnd.Text = DateTime.Now.ToString("23:59:59");
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TxtplateId.Text))
            {
                Notice("信息提示", "请填写号牌号码！");
                return;
            }
            Condition con = GetWhere();
            if (con != null)
            {
                if (Session["userinfo"] != null)
                {
                    con.UserName = (Session["userinfo"] as UserInfo).UserName;//用户名称
                    con.UserCode = (Session["userinfo"] as UserInfo).UserCode;//用户编号
                }
                string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                if (ipaddress.Length < 9)
                {
                    ipaddress = "127.0.0.1";
                }
                con.UserIp = ipaddress;//用户Ip地址
                con.Dyzgnmkmc = PasscarAllQuery.dyzgnmkmc;//功能模块名称
                con.Dyzgnmkbh = PasscarAllQuery.dyzgnmkbh;//功能模块编号
                dtPath = CreatePathQueryTable();
                string xml = MyNet.Atmcs.Uscmcp.Bll.Common.GetPassCarXml(con, "10", "50");
                string rexml = client.GetPassCarInfo(xml);
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.LoadXml(rexml);
                }
                catch (Exception)
                {
                    throw;
                }
                //allNum = Bll.Common.GetRowCount(xmlDoc);
                if (!string.IsNullOrEmpty(rexml))
                {
                    CXmlToDataTable(xmlDoc);
                    if (dtPath != null && dtPath.Rows.Count > 0)
                    {
                        DataTable dt = MyNet.Atmcs.Uscmcp.Bll.Common.ChangColName(dtPath);
                        StoreImage.DataSource = dt;
                        StoreImage.DataBind();
                    }
                }

                //原来的
                // DataTable dt = tgsDataInfo.GetAllPassCarInfo(GetWhere());
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    dt = dataCommon.ChangeDataTablePoliceIp(dt, "col12", "", "");
                //    StoreImage.DataSource = dt;
                //    StoreImage.DataBind();
                //}
                //else
                //{
                //    Notice("信息提示", "未查询到任何符合条件的信息！");
                //}
            }
        }

        /// <summary>
        /// 创建内存表
        /// </summary>
        /// <returns></returns>
        private DataTable CreatePathQueryTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("xh", typeof(string));
            dt.Columns.Add("kkid", typeof(string));
            dt.Columns.Add("kkmc", typeof(string));
            dt.Columns.Add("hphm", typeof(string));
            dt.Columns.Add("hpzl", typeof(string));
            dt.Columns.Add("hpzlms", typeof(string));
            dt.Columns.Add("gwsj", typeof(string));
            dt.Columns.Add("ddbhms", typeof(string));
            dt.Columns.Add("fxbh", typeof(string));
            dt.Columns.Add("fxmc", typeof(string));
            dt.Columns.Add("cdbh", typeof(string));
            dt.Columns.Add("clsd", typeof(string));
            dt.Columns.Add("clxs", typeof(string));
            dt.Columns.Add("jllxms", typeof(string));
            dt.Columns.Add("zjwj1", typeof(string));
            dt.Columns.Add("zjwj2", typeof(string));
            dt.Columns.Add("zjwj3", typeof(string));
            return dt;
        }

        /// <summary>
        /// 车辆轨迹转换为datatable
        /// </summary>
        /// <param name="xmlStr"></param>
        public void CXmlToDataTable(XmlDocument xmlDoc)
        {
            XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/passcarinfolist/passcarinfo");
            try
            {
                int i = 0;
                foreach (XmlNode node in listNodes)
                {
                    i++;
                    //ds.ReadXml(node.OuterXml);
                    DataRow dr = dtPath.NewRow();
                    dr["xh"] = i.ToString();
                    dr["kkid"] = (node.SelectSingleNode("kkid")).InnerText;
                    DataRow[] listdr = dtStation.Select("STATION_ID= '" + (node.SelectSingleNode("kkid")).InnerText + "'");
                    if (listdr.Length > 0)
                    {
                        dr["kkmc"] = listdr[0]["STATION_NAME"].ToString();
                    }
                    else
                    {
                        dr["kkmc"] = (node.SelectSingleNode("kkid")).InnerText;
                    }
                    dr["hphm"] = (node.SelectSingleNode("hphm")).InnerText;
                    dr["hpzl"] = (node.SelectSingleNode("hpzl")).InnerText;
                    dr["hpzlms"] = MyNet.Atmcs.Uscmcp.Bll.Common.GetHpzlms((node.SelectSingleNode("hpzl")).InnerText);
                    //dr["gwsj"] = DateTime.Parse((node.SelectSingleNode("gwsj")).InnerText).ToString("yyyy-MM-dd HH:mm:ss");
                    dr["gwsj"] = MyNet.Atmcs.Uscmcp.Bll.Common.GetDate((node.SelectSingleNode("gwsj")).InnerText, 0);
                    dr["ddbhms"] = dr["kkmc"].ToString();
                    dr["fxbh"] = (node.SelectSingleNode("fxbh")).InnerText;
                    DataRow[] listdrfx = dtXsfx.Select("code= '" + (node.SelectSingleNode("fxbh")).InnerText + "'");
                    if (listdrfx.Length > 0)
                    { dr["fxmc"] = listdrfx[0]["codedesc"].ToString(); }
                    else
                    { dr["fxmc"] = ""; }
                    dr["cdbh"] = (node.SelectSingleNode("cdbh")).InnerText;
                    dr["clsd"] = (node.SelectSingleNode("clsd")).InnerText;
                    dr["clxs"] = 0;
                    dr["jllxms"] = MyNet.Atmcs.Uscmcp.Bll.Common.GetJllxms((node.SelectSingleNode("jllx")).InnerText);
                    dr["zjwj1"] = (node.SelectSingleNode("zjwj1")).InnerText;
                    dr["zjwj2"] = (node.SelectSingleNode("zjwj2")).InnerText;
                    dr["zjwj3"] = (node.SelectSingleNode("zjwj3")).InnerText;
                    dtPath.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 得到查询条件
        /// </summary>
        /// <returns></returns>
        private Condition GetWhere()
        {
            Condition con = new Condition();
            con.StartTime = DateStartTime.SelectedDate.ToString("yyyy-MM-dd") + " " + DateTime.Parse(this.TimeStart.Text).ToString("HH:mm:00");
            con.EndTime = DateEndTime.SelectedDate.ToString("yyyy-MM-dd") + " " + DateTime.Parse(this.TimeEnd.Text).ToString("HH:mm:59");
            if (CmbPlateType.SelectedItem.Value != null)
            {
                con.Hpzl = CmbPlateType.SelectedItem.Value;
            }
            if (!string.IsNullOrEmpty(TxtplateId.Text))
            {
                con.Hphm = TxtplateId.Text;
            }
            return con;
            //string where = "1=1";
            //string kssj = DateStartTime.SelectedDate.ToString("yyyy-MM-dd") + " " + DateTime.Parse(this.TimeStart.Text).ToString("HH:mm:00");
            //string jssj = DateEndTime.SelectedDate.ToString("yyyy-MM-dd") + " " + DateTime.Parse(this.TimeEnd.Text).ToString("HH:mm:59");
            //where = "  gwsj >= to_date('" + kssj + "','yyyy-mm-dd hh24:mi:ss')   and gwsj<=to_date('" + jssj + "','yyyy-mm-dd hh24:mi:ss')";
            //if (CmbPlateType.SelectedIndex != -1)
            //{
            //    where = where + " and  hpzl='" + CmbPlateType.SelectedItem.Value + "' ";
            //}
            //if (!string.IsNullOrEmpty(TxtplateId.Text))
            //{
            //    where = where + " and  hphm='" + TxtplateId.Text.ToUpper() + "' ";
            //}
            //where = where + "  order by gwsj asc";
            //return where;
        }

        /// <summary>
        /// 向页面显示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Notice(string title, string msg)
        {
            Notification.Show(new NotificationConfig
            {
                Title = title,
                Icon = Icon.Error,
                HideDelay = 2000,
                Height = 120,
                Html = "<br></br>" + msg + "!"
            });
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            this.DateStartTime.SelectedDate = DateTime.Now;
            this.DateEndTime.SelectedDate = DateTime.Now;
            this.TimeStart.Text = DateTime.Now.ToString("00:00:01");
            this.TimeEnd.Text = DateTime.Now.ToString("23:59:59");
            this.CmbPlateType.Reset();
            this.TxtplateId.Reset();
        }
    }
}