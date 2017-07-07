using System;
using System.Data;
using System.Xml;
using System.Xml.Xsl;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class AnalyFollowVehicles : System.Web.UI.Page
    {
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();

        private static string starttime = "";
        private static string endtime = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"]; 
            if (!userLogin.CheckLogin(username)) 
            { 
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" +StaticInfo.LoginPage + "'"; 
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); 
                return; 
            }
            if (!X.IsAjaxRequest)
            {
                try
                {
                    Scplx.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:140001"));// tgsPproperty.GetDeviceTypeDict("140001");
                    Scplx.DataBind();
                    Skkmc.DataSource = tgsPproperty.GetStationInfo("1=1");
                    Skkmc.DataBind();
                    Sxsfx.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240025"));//  tgsPproperty.GetDirectionDict();
                    Sxsfx.DataBind();

                    if (Session["Condition"] != null)
                    {
                        Condition con = Session["Condition"] as Condition;
                        start.InnerText = con.StartTime;
                        end.InnerText = con.EndTime;
                        CBcplx.Value = con.Hpzl;
                        CBkkmc.Value = con.Kkid;
                        CBxsfx.Value = con.Xsfx;
                        vehicleHead.SetVehicleText(con.Sqjc);
                        TFcphm.Text = con.Hphm;
                    }
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex);
                }
            }
        }

        protected void BTSearch_DirectClick(object sender, DirectEventArgs e)
        {
            //int a = Convert.ToInt16(SFbsjg.Value);

            //string cphm = ValueSelectedToString.objectToString(CBcphm.Value) + TFcphm.Text;
            //string kkmc = ValueSelectedToString.objectToString(CBkkmc.Value);
            //string cplx = ValueSelectedToString.objectToString(CBcplx.Value);
            //DateTime beginTime =DFBeginTime.SelectedDate+ValueSelectedToString.GetBeginTime(TFBeginTime.SelectedTime,TFBeginTime.Value);
            //DateTime endTime = ValueSelectedToString.GetDateFieldEndTime(DFEndTime.SelectedDate) + ValueSelectedToString.GetEndTime(TFEndTime.SelectedTime, TFEndTime.Value);

            ////DateTime _endTime = VehiclesBLL.GetGwsj(kkmc,beginTime,endTime,cphm).AddSeconds(a);
            ////DateTime _beginTime = VehiclesBLL.GetGwsj(kkmc,beginTime,endTime,cphm).AddSeconds(-a);
            ////string _cplx = ValueSelectedToString.objectToString(CBcplx.Value);
            ////string _xsfx=ValueSelectedToString.objectToString(CBxsfx.Value);
            //try
            //{
            //    this.Smbcl.DataSource = VehiclesBLL.VehiclesDataQuery(cphm,kkmc,cplx,beginTime,endTime);
            //    this.Smbcl.DataBind();

            //    //this.Sbscl.DataSource = VehiclesBLL.FollowVehicleDataQuery(kkmc,_beginTime,_endTime,_cplx,_xsfx);
            //    //this.Sbscl.DataBind();
            //}
            //catch
            //{ }
        }

        protected void Smbcl_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //string cphm = ValueSelectedToString.objectToString(CBcphm.Value);
            //string kkmc = ValueSelectedToString.objectToString(CBkkmc.Value);
            //string cplx = ValueSelectedToString.objectToString(CBcplx.Value);
            //DateTime beginTime = DFBeginTime.SelectedDate + ValueSelectedToString.GetBeginTime(TFBeginTime.SelectedTime, TFBeginTime.Value); ;
            //DateTime endTime = DFEndTime.SelectedDate + ValueSelectedToString.GetEndTime(TFEndTime.SelectedTime, TFEndTime.Value);
            //try
            //{
            //    this.Smbcl.DataSource = VehiclesBLL.VehiclesDataQuery(cphm, kkmc,cplx, beginTime, endTime);
            //    this.Smbcl.DataBind();
            //}
            //catch
            //{ }
        }

        protected void Smbcl_Submit(object sender, StoreSubmitDataEventArgs e)
        {
            string format = this.FormatType.Value.ToString();

            XmlNode xml = e.Xml;

            this.Response.Clear();

            switch (format)
            {
                case "xml":
                    string strXml = xml.OuterXml;
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xml");
                    this.Response.AddHeader("Content-Length", strXml.Length.ToString());
                    this.Response.ContentType = "application/xml";
                    this.Response.Write(strXml);

                    break;

                case "xls":
                    this.Response.ContentType = "application/vnd.ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
                    XslCompiledTransform xtExcel = new XslCompiledTransform();
                    xtExcel.Load(Server.MapPath("Excel.xsl"));
                    xtExcel.Transform(xml, null, Response.OutputStream);

                    break;

                case "csv":
                    this.Response.ContentType = "application/octet-stream";
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.csv");
                    XslCompiledTransform xtCsv = new XslCompiledTransform();
                    xtCsv.Load(Server.MapPath("Csv.xsl"));
                    xtCsv.Transform(xml, null, Response.OutputStream);

                    break;
            }

            this.Response.End();
        }

        protected void Sbscl_RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            //int a = Convert.ToInt16(SFbsjg.Value);

            //string cphm = ValueSelectedToString.objectToString(CBcphm.Value) + TFcphm.Text;
            //string kkmc = ValueSelectedToString.objectToString(CBkkmc.Value);
            //DateTime beginTime = DFBeginTime.SelectedDate + ValueSelectedToString.GetBeginTime(TFBeginTime.SelectedTime, TFBeginTime.Value);
            //DateTime endTime = DFEndTime.SelectedDate + ValueSelectedToString.GetEndTime(TFEndTime.SelectedTime, TFEndTime.Value);

            //DateTime _endTime = VehiclesBLL.GetGwsj(kkmc, beginTime, endTime, cphm).AddSeconds(a);
            //DateTime _beginTime = VehiclesBLL.GetGwsj(kkmc, beginTime, endTime, cphm).AddSeconds(-a);
            //string _cplx = ValueSelectedToString.objectToString(CBcplx.Value);
            //string _xsfx = ValueSelectedToString.objectToString(CBxsfx.Value);
            //try
            //{
            //    this.Sbscl.DataSource = VehiclesBLL.FollowVehicleDataQuery(kkmc, _beginTime, _endTime, _cplx, _xsfx);
            //    this.Sbscl.DataBind();
            //}
            //catch
            //{ }
        }

        protected void Sbscl_Submit(object sender, StoreSubmitDataEventArgs e)
        {
            string format = this.FormatType.Value.ToString();

            XmlNode xml = e.Xml;

            this.Response.Clear();

            switch (format)
            {
                case "xml":
                    string strXml = xml.OuterXml;
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xml");
                    this.Response.AddHeader("Content-Length", strXml.Length.ToString());
                    this.Response.ContentType = "application/xml";
                    this.Response.Write(strXml);

                    break;

                case "xls":
                    this.Response.ContentType = "application/vnd.ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.xls");
                    XslCompiledTransform xtExcel = new XslCompiledTransform();
                    xtExcel.Load(Server.MapPath("Excel.xsl"));
                    xtExcel.Transform(xml, null, Response.OutputStream);

                    break;

                case "csv":
                    this.Response.ContentType = "application/octet-stream";
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=submittedData.csv");
                    XslCompiledTransform xtCsv = new XslCompiledTransform();
                    xtCsv.Load(Server.MapPath("Csv.xsl"));
                    xtCsv.Transform(xml, null, Response.OutputStream);

                    break;
            }

            this.Response.End();
        }

        /// <summary>
        /// 选中数据行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectVehicleData(object sender, DirectEventArgs e)
        {
            //int a = Convert.ToInt16(SFbsjg.Value);
            //object data = e.ExtraParams["sdata"];
            //string sdata = data.ToString();

            //DateTime _endTime = Convert.ToDateTime(VehiclesBLL.GetdatabyField(sdata, "gwsj")).AddSeconds(a);
            //DateTime _beginTime = Convert.ToDateTime(VehiclesBLL.GetdatabyField(sdata, "gwsj")).AddSeconds(-a);
            //string _kkmc = VehiclesBLL.GetdatabyField(sdata, "kkmc");
            //string _cplx = ValueSelectedToString.objectToString(CBcplx.Value);
            //string _xsfx = ValueSelectedToString.objectToString(CBxsfx.Value);

            //this.Sbscl.DataSource = VehiclesBLL.FollowVehicleDataQuery(_kkmc, _beginTime, _endTime, _cplx, _xsfx);
            //this.Sbscl.DataBind();
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="isstart"></param>
        /// <param name="strtime"></param>
        [DirectMethod]
        public void GetDateTime(bool isstart, string strtime)
        {
            try
            {
                if (isstart)
                    starttime = strtime;
                else
                    endtime = strtime;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButExcelClick(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = Session["printdatatable"] as DataTable; ;
                ConvertData.ExportExcel(dt, this);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }
    }
}