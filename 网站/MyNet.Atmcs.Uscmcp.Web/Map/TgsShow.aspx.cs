using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;
using System.Xml;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class TgsShow : System.Web.UI.Page
    {
        #region 成员变量

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private DataCommon dataCommon = new DataCommon();
        private GisShow gisShow = new GisShow();
        private static QueryService.querypasscar client = new QueryService.querypasscar();
        private UserLogin userLogin = new UserLogin();
        public string xh = "";
        private static string message = "";
        private static System.Collections.Generic.List<string> listMq = new System.Collections.Generic.List<string>();
        private static DataTable Dt_passcar = null;
        private static int PlayNum = 0;

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
            //if (!userLogin.CheckLogin(username))
            //{
            //    string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'";
            //    System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
            //    return;
            //}
            if (!X.IsAjaxRequest)
            {
                try
                {
                    string kkid = Request["kkid"];
                    Session["kkid"] = kkid;

                    if (System.Configuration.ConfigurationManager.AppSettings["mq"].ToString().ToLower() == "true")
                    {
                        client.cancel(Session["mqid"].ToString());
                        bool result = client.addTgs(kkid, "", "", Session["mqid"].ToString());
                        if (!result)
                            result = client.addTgs(kkid, "", "", Session["mqid"].ToString());
                    }
                    StoreDataBind();
                    GetImageFirst(kkid);
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：用户登录", userinfo.NowIp, "0");
                    //DefaultImage();
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex.Message);
                    logManager.InsertLogError("TgsShow.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
                }
            }
        }

        /// <summary>
        /// 定时刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RefreshTime(object sender, DirectEventArgs e)
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["mq"].ToString().ToLower() == "true")
                {
                    if (message != "")
                    {
                        GetImagFromMq(message);
                        if (listMq.Count > 20)
                            listMq.RemoveRange(20, listMq.Count - 21);
                        Session["passcar"] = listMq;
                    }
                }
                else
                {
                    GetImageNow(Session["kkid"].ToString());
                    //Notice("提示", "抽取完成");
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("TgsShow.aspx-RefreshTime", ex.Message + "；" + ex.StackTrace, "RefreshTime has an exception");
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

        /// <summary>
        /// 定时展示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RefreshImage(object sender, DirectEventArgs e)
        {
            ShowImage();
        }

        private void ShowImage()
        {
            if (Dt_passcar != null && Dt_passcar.Rows.Count > 0)
            {
                if (PlayNum.Equals(Dt_passcar.Rows.Count - 1))
                {
                    PlayNum = Dt_passcar.Rows.Count - 1;
                }
                showimage(PlayNum);
                PlayNum++;
            }
        }

        #endregion 控件事件

        #region DirectMethod

        [DirectMethod]
        public void Mq(string mqstr)
        {
            message = mqstr;
            listMq.Insert(0, mqstr);
            //if (mqstr.Substring(0, 11) == "<<< MESSAGE")
            //{
            //    int mqstart = mqstr.IndexOf("{");

            //    Json json = new Json(mqstr.Substring(mqstart));
            //    System.Collections.Hashtable carhs = json["car"] as System.Collections.Hashtable;
            //    carhs.Add("layoutId", json["layoutId"]);
            //    carhs.Add("alarmTime", json["alarmTime"]);
            //    if (Session["kkid"].ToString() == carhs["tgs"].ToString())
            //    {
            //        message = mqstr;
            //        if (Session["passcar"] != null)
            //            Session["passcar"] = null;
            //        //if (listMq.Count > 20)
            //        //    listMq.RemoveAt(0);
            //        listMq.Add(mqstr);
            //        Session["passcar"] = listMq;
            //        ////        settable();
            //    }
            //}
        }

        [DirectMethod]
        public void Page_Close()
        {
            client.cancel(Session["kkid"].ToString());
        }

        [DirectMethod]
        public void GetImagFromMq(string mqstr)
        {
            try
            {
                if (mqstr.Substring(0, 11) == "<<< MESSAGE")
                {
                    int mqstart = mqstr.IndexOf("{");

                    Json json = new Json(mqstr.Substring(mqstart));
                    System.Collections.Hashtable carhs = json["car"] as System.Collections.Hashtable;
                    carhs.Add("layoutId", json["layoutId"]);
                    carhs.Add("alarmTime", json["alarmTime"]);
                    if (Session["kkid"].ToString() == carhs["tgs"].ToString())
                    {
                        string zjwj = carhs["imgUrls"].ToString().Split(',')[0];
                        zjwj = zjwj.Substring(2, zjwj.Length - 3);
                        string hphm = carhs["plateNumber"].ToString();
                        string gwsj = carhs["timeStamp"].ToString();
                        showimage(hphm, gwsj, zjwj);
                        //if (Session["passcar"] != null)
                        //    Session["passcar"] = null;
                        //if (listMq.Count > 20)
                        //    listMq.RemoveAt(0);
                        //listMq.Add(carhs);
                        //settable();
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("TgsShow.aspx-GetImagFromMq", ex.Message + "；" + ex.StackTrace, "GetImagFromMq has an exception");
            }
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="kkid"></param>
        [DirectMethod]
        public void GetImageNow(string kkid)
        {
            try
            {
                string xml = "", rsxml = "";
                xml = getxml(0, 5, System.DateTime.Now.AddMinutes(-5).ToString("yyyy-MM-dd HH:mm:ss"), System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "", "", "", "", "", kkid);
                rsxml = client.GetPassCarInfo(xml);
                if (rsxml != "" && getlenxml(rsxml) > 0)
                {
                    CXmlToDataTable(rsxml);

                    //showimage(0);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("TgsShow.aspx-GetImageNow", ex.Message + "；" + ex.StackTrace, "GetImageNow has an exception");
            }
        }

        /// <summary>
        /// 页面加载初次查询
        /// </summary>
        /// <param name="kkid"></param>
        public void GetImageFirst(string kkid)
        {
            try
            {
                string xml = "", rsxml = "";
                xml = getxml(0, 5, System.DateTime.Now.AddMinutes(-5).ToString("yyyy-MM-dd HH:mm:ss"), System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "", "", "", "", "", kkid);
                rsxml = client.GetPassCarInfo(xml);
                if (rsxml != "" && getlenxml(rsxml) > 0)
                {
                    FirstToDataTable(rsxml);

                    showimage(0);
                }
                //Notice("提示", "初次完成");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("TgsShow.aspx-GetImageFirst", ex.Message + "；" + ex.StackTrace, "GetImageFirst has an exception");
            }
        }

        /// <summary>
        /// 初次解析
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        public void FirstToDataTable(string xmlStr)
        {
            try
            {
                Dt_passcar = GetData();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);
                XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/passcarinfolist/passcarinfo");
                int num = 5 - listNodes.Count;
                // 初次组装时 验证是否满 5个
                for (int i = listNodes.Count - 1; i >= 0; i--)
                {
                    DataRow dr = Dt_passcar.NewRow();
                    dr[0] = (listNodes[i].SelectSingleNode("hphm")).InnerText;
                    dr[1] = (listNodes[i].SelectSingleNode("kkid")).InnerText;
                    dr["gwsj"] = (listNodes[i].SelectSingleNode("gwsj")).InnerText;
                    dr["zjwj"] = (listNodes[i].SelectSingleNode("zjwj1")).InnerText;
                    dr["clpp"] = Bll.Common.Changenull((listNodes[i].SelectSingleNode("clpp")).InnerText);
                    dr["csys"] = (listNodes[i].SelectSingleNode("csys")).InnerText;

                    Dt_passcar.Rows.Add(dr);
                    if (i == 0) // 最后一次
                    {
                        while (Dt_passcar.Rows.Count < 5)  // 取得记录少于5个  将最后一个 继续复制
                        {
                            DataRow drNew = Dt_passcar.NewRow();
                            drNew["hphm"] = dr["hphm"].ToString();
                            drNew["kkid"] = dr["kkid"].ToString();
                            drNew["gwsj"] = dr["gwsj"].ToString();
                            drNew["zjwj"] = dr["zjwj"].ToString();
                            drNew["clpp"] = Bll.Common.Changenull(dr["clpp"].ToString());
                            drNew["csys"] = dr["csys"].ToString();
                            Dt_passcar.Rows.Add(drNew);
                        }
                    }
                }

                PlayNum = 0;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("TgsShow.aspx-FirstToDataTable", ex.Message + "；" + ex.StackTrace, "FirstToDataTable has an exception");
            }
        }

        #endregion DirectMethod

        #region 私有方法

        /// <summary>
        ///  绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            string kkid = Session["kkid"] as string;
            DataTable devdt = gisShow.GetStationInfo(kkid);
            if (devdt != null && devdt.Rows.Count > 0)
            {
                this.StoreDevice.DataSource = devdt;
                this.StoreDevice.DataBind();
                this.LblDevice.Text = "监测点名称：" + devdt.Rows[0]["col1"].ToString() + "";
            }

            DataTable flowdt = gisShow.GetFlow("t_tgs_flow_hour", kkid, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.Hour.ToString());
            if (flowdt == null || flowdt.Rows.Count <= 0)
            {
                flowdt = gisShow.GetFlow("t_tgs_flow_hour", kkid, DateTime.Now.ToString("yyyy-MM-dd"), (DateTime.Now.Hour - 1).ToString());
            }
            if (flowdt != null && flowdt.Rows.Count > 0)
            {
                this.StoreFlow.DataSource = flowdt;
                this.StoreFlow.DataBind();
                this.LblFlow.Text = "统计日期：" + DateTime.Now.ToString("yyyy-MM-dd");// +"  " + DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm");
            }
        }

        /// <summary>
        /// 组织xml查询文件
        /// </summary>
        /// <param name="rowstart">记录起始行</param>
        /// <param name="len">记录条数</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="cllx">车辆类型</param>
        /// <param name="hphm">号牌号码</param>
        /// <param name="gssj">更新时间</param>
        /// <param name="txkk">卡口</param>
        /// <returns></returns>
        private string getxml(int rowstart, int len, string start, string end, string cllx, string clxh, string clpp, string csys, string hphm, string kkid)
        {
            try
            {
                string username = ""; string usercode = ""; string userip = ""; string dyzgnmkmc = ""; string dyzgnmkbh = "";
                if (Session["userinfo"] != null)
                {
                    username = (Session["userinfo"] as UserInfo).UserName;//用户名称
                    usercode = (Session["userinfo"] as UserInfo).UserCode;//用户编号
                }
                string ipaddress = Request.ServerVariables.Get("Remote_Addr").ToString();
                if (ipaddress.Length < 9)
                {
                    ipaddress = "127.0.0.1";
                }
                userip = ipaddress;//用户Ip地址
                string[] fstrs = Request.QueryString["funcname"].Split('-');//功能模块名称
                if (fstrs.Length > 0)
                {
                    dyzgnmkmc = fstrs[1];
                }
                dyzgnmkbh = Request.QueryString["funcid"].ToString(); ;//功能模块编号
                string xml = "<?xml version='1.0' encoding='UTF-8'?>" +
                    "<Message>" +
                             "<Version>版本号</Version>" +
                             "<Type>REQUEST</Type>" +
                             "<Body>" +
                                 "<Cmd>" +
                                    "<kkid>" + kkid + "</kkid>" +
                                    "<fxbh></fxbh>" +
                                    "<cdbh></cdbh>" +
                                    "<hphm>" + hphm + "</hphm>" +
                                    "<hpzl>" + cllx + "</hpzl>" +
                                    "<kssj>" + start + "</kssj>" +
                                    "<jssj>" + end + "</jssj>" +
                                    "<clpp>" + clpp + "</clpp>" +
                                    "<clsd></clsd>" +
                                    "<csys>" + csys + "</csys>" +
                                    "<jllx></jllx>" +
                                    "<zjhsl></zjhsl>" +
                                    "<zybsl></zybsl>" +
                                    "<dzsl></dzsl>" +
                                    "<bjsl></bjsl>" +
                                    "<njbsl></njbsl>" +
                                    "<zjsaqd></zjsaqd>" +
                                    "<fjsaqd></fjsaqd>" +
                                    "<begnum>" + rowstart.ToString() + "</begnum>" +
                                    "<num>" + len.ToString() + "</num>" +
                                "</Cmd>" +
                                    "<LogInfo>" +
                                    "<userName>" + username + "</userName>" +
                                    "<userIp>" + userip + "</userIp>" +
                                    "<userCode>" + usercode + "</userCode>" +
                                    "<dyzgnmkbh>" + dyzgnmkbh + "</dyzgnmkbh>" +
                                    "<dyzgnmkmc>" + dyzgnmkmc + "</dyzgnmkmc>" +
                                "</LogInfo>" +
                           "</Body>" +
                       "</Message>";
                return xml;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("TgsShow.aspx-getxml", ex.Message + "；" + ex.StackTrace, "getxml has an exception");
                return "";
            }
        }

        /// <summary>
        /// xml文件解析
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        public void CXmlToDataTable(string xmlStr)
        {
            try
            {
                DataTable dtTemp = Dt_passcar;
                Dt_passcar = GetData();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);
                XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/passcarinfolist/passcarinfo");
                int lasthh = 4;
                // 取内存表中 最新的记录 和这次取得 list中 对比 ，如果存在，返回所在 list 的序号，不存在，说明都是新数据，可以直接添加
                for (int n = 0; n <= listNodes.Count - 1; n++)
                {
                    if (dtTemp.Rows[4]["hphm"].Equals((listNodes[n].SelectSingleNode("hphm")).InnerText))
                    {
                        lasthh = n;
                        break;
                    }
                }
                for (int i = lasthh; i >= 0; i--)
                {
                    DataRow dr = Dt_passcar.NewRow();
                    dr["hphm"] = (listNodes[i].SelectSingleNode("hphm")).InnerText;
                    dr["kkid"] = (listNodes[i].SelectSingleNode("kkid")).InnerText;
                    dr["gwsj"] = (listNodes[i].SelectSingleNode("gwsj")).InnerText;
                    dr["zjwj"] = (listNodes[i].SelectSingleNode("zjwj1")).InnerText;
                    dr["clpp"] = Bll.Common.Changenull((listNodes[i].SelectSingleNode("clpp")).InnerText);
                    dr["csys"] = (listNodes[i].SelectSingleNode("csys")).InnerText;

                    Dt_passcar.Rows.Add(dr);

                    if (i == 0) // 最后一次
                    {
                        while (Dt_passcar.Rows.Count < 5)  // 取得记录少于5个  将最后一个 继续复制
                        {
                            DataRow drNew = Dt_passcar.NewRow();
                            drNew["hphm"] = dr["hphm"].ToString();
                            drNew["kkid"] = dr["kkid"].ToString();
                            drNew["gwsj"] = dr["gwsj"].ToString();
                            drNew["zjwj"] = dr["zjwj"].ToString();
                            drNew["clpp"] = Bll.Common.Changenull(dr["clpp"].ToString());
                            drNew["csys"] = dr["csys"].ToString();
                            Dt_passcar.Rows.Add(drNew);
                        }
                    }
                }

                PlayNum = 0;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("TgsShow.aspx-CXmlToDataTable", ex.Message + "；" + ex.StackTrace, "CXmlToDataTable has an exception");
            }
        }

        /// <summary>
        /// 解析xml文件获取记录条数
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        /// <returns></returns>
        public int getlenxml(string xmlStr)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);

                XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/passcarinfolist");
                return int.Parse(listNodes[0].Attributes[0].Value);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("TgsShow.aspx-getlenxml", ex.Message + "；" + ex.StackTrace, "getlenxml has an exception");
                return 0;
            }
        }

        /// <summary>
        /// 获取passcar结构
        /// </summary>
        /// <returns></returns>
        private DataTable GetData()
        {
            DataTable dt = new DataTable("PassCar");
            dt.Columns.Add("hphm", Type.GetType("System.String"));
            dt.Columns.Add("kkid", Type.GetType("System.String"));
            dt.Columns.Add("lkmc", Type.GetType("System.String"));
            dt.Columns.Add("cdbh", Type.GetType("System.String"));
            dt.Columns.Add("fxmc", Type.GetType("System.String"));
            dt.Columns.Add("xpoint", Type.GetType("System.String"));
            dt.Columns.Add("ypoint", Type.GetType("System.String"));
            dt.Columns.Add("gwsj", Type.GetType("System.String"));
            dt.Columns.Add("zjwj", Type.GetType("System.String"));
            dt.Columns.Add("csys", Type.GetType("System.String"));
            dt.Columns.Add("clpp", Type.GetType("System.String"));
            return dt;
        }

        private void showimage()
        {
            this.LblOne.Text = Dt_passcar.Rows[0]["hphm"].ToString() + " " + Dt_passcar.Rows[0]["gwsj"].ToString();
            string html = string.Empty;
            html = "<center>";
            html = html + "<table border='1' width='400' height='100%' cellspacing='0' cellpadding='0'>";
            html = html + "<td><img  'height='100%' width='400' src='" + Dt_passcar.Rows[0]["zjwj"].ToString() + "' alt='车辆图片(双击图片进行放大)' onclick='zoom(this,false);' ondblclick='OpenPicPage(this.src);' onmouseover=\"this.style.border='1px solid #0000ff';\" onmouseout=\"this.style.border='1px solid #ffffff';\"/></td>";
            html = html + "</table>";
            html = html + "</center>";
            panOne.Dispose();
            panOne.Html = html;
        }

        private void showimage(string hphm, string gwsj, string zjwj)
        {
            try
            {
                this.LblOne.Text = hphm + " " + gwsj;
                string html = string.Empty;
                html = "<center>";
                html = html + "<table border='1' width='400' height='100%' cellspacing='0' cellpadding='0'>";
                html = html + "<td><img  'height='100%' width='400' src='" + zjwj + "' alt='车辆图片(双击图片进行放大)' onclick='zoom(this,false);' ondblclick='OpenPicPage(this.src);' onmouseover=\"this.style.border='1px solid #0000ff';\" onmouseout=\"this.style.border='1px solid #ffffff';\"/></td>";
                html = html + "</table>";
                html = html + "</center>";
                panOne.Dispose();
                panOne.Html = html;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("TgsShow.aspx-showimage", ex.Message + "；" + ex.StackTrace, "showimage has an exception");
            }
        }

        private void showimage(int sx)
        {
            try
            {
                this.LblOne.Text = Dt_passcar.Rows[sx]["hphm"].ToString() + " " + Dt_passcar.Rows[sx]["gwsj"].ToString();
                string html = string.Empty;
                html = "<center>";
                html = html + "<table border='1' width='400' height='100%' cellspacing='0' cellpadding='0'>";
                html = html + "<td><img  'height='100%' width='400' src='" + Dt_passcar.Rows[sx]["zjwj"].ToString() + "' alt='车辆图片(双击图片进行放大)' onclick='zoom(this,false);' ondblclick='OpenPicPage(this.src);' onmouseover=\"this.style.border='1px solid #0000ff';\" onmouseout=\"this.style.border='1px solid #ffffff';\"/></td>";
                html = html + "</table>";
                html = html + "</center>";
                HidNowHphm.Text = Dt_passcar.Rows[sx]["hphm"].ToString();
                panOne.Dispose();
                panOne.Html = html;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TgsShow.aspx-showimage", ex.Message + "；" + ex.StackTrace, "showimage has an exception");
            }
        }

        #endregion 私有方法

        #region 老程序注释

        //[DirectMethod]
        //public void DefaultImage()
        //{
        //    try
        //    {
        //        string kkid = Request["kkid"];
        //        if (!string.IsNullOrEmpty(kkid))
        //        {
        //            Session["MonitorId"] = xh;
        //            string where = " rownum >=1";
        //            where = "   kkid= '" + kkid + "'  and " + where;
        //            DataTable dt = tgsDataInfo.GetPassCarMonitor(where);
        //            if (dt != null && dt.Rows.Count > 0)
        //            {
        //                xh = dt.Rows[0][1].ToString();
        //                if (xh != Session["MonitorId"].ToString())
        //                {
        //                    string surl1 = dataCommon.ChangePoliceIp(dt.Rows[0][27].ToString());
        //                    string surl2 = dataCommon.ChangePoliceIp(dt.Rows[0][28].ToString());
        //                    string hpzl = dt.Rows[0][5].ToString();
        //                    Session["MonitorId"] = xh;
        //                    List<string> lst = GetURL(dt);
        //                    ApplyOneShow(lst, dt);

        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Notice("信息提示", ex.Message);
        //    }
        //}
        //private void Notice(string title, string msg)
        //{
        //    Notification.Show(new NotificationConfig
        //    {
        //        Title = title,
        //        Icon = Icon.Error,
        //        HideDelay = 2000,
        //        Html = "<br></br>" + msg + "!"
        //    });
        //}
        //private List<string> GetURL(DataTable dt)
        //{
        //    List<string> lst = new List<string>();
        //    for (int i = 0; i < 9; i++)
        //    {
        //        lst.Add("Images/NoImage.png");
        //    }
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        lst[i] = dataCommon.ChangePoliceIp(dt.Rows[i]["col27"].ToString());
        //    }
        //    return lst;
        //}
        //public  string GetWeek()
        //{
        //    string weekName = "";
        //    DateTime dt = DateTime.Now;
        //    string XingQi = Convert.ToString(dt.DayOfWeek);//星期
        //    if (XingQi == "Monday")//星期判断
        //    {
        //        weekName = "星期一";
        //    }
        //    else if (XingQi == "Tuesday")
        //    {
        //        weekName = "星期二";
        //    }
        //    else if (XingQi == "Wednesday")
        //    {
        //        weekName = "星期三";
        //    }
        //    else if (XingQi == "Thursday")
        //    {
        //        weekName = "星期四";
        //    }
        //    else if (XingQi == "Friday")
        //    {
        //        weekName = "星期五";
        //    }
        //    else if (XingQi == "Saturday")
        //    {
        //        weekName = "星期六";
        //    }
        //    else if (XingQi == "Sunday")
        //    {
        //        weekName = "星期日";
        //    }
        //    return weekName;
        //}
        //protected void ApplyOneShow(List<string> lst,DataTable dt)
        //{
        //    this.LblOne.Text = dt.Rows[0]["col3"].ToString() + " " + dt.Rows[0]["col4"].ToString() +" "+ dt.Rows[0]["col6"].ToString();
        //    string html = string.Empty;
        //    html = "<center>";
        //    html = html + "<table border='1' width='400' height='100%' cellspacing='0' cellpadding='0'>";
        //    html = html + "<td><img  'height='100%' width='400' src='" + lst[0] + "' alt='车辆图片(双击图片进行放大)' onclick='zoom(this,false);' ondblclick='OpenPicPage(this.src);' onmouseover=\"this.style.border='1px solid #0000ff';\" onmouseout=\"this.style.border='1px solid #ffffff';\"/></td>";
        //    html = html + "</table>";
        //    html = html + "</center>";
        //    panOne.Dispose();
        //    panOne.Html = html;
        //}
        //protected void SubmitSelection(object sender, DirectEventArgs e)
        //{
        //    string json = e.ExtraParams["Values"];
        //}

        #endregion 老程序注释
    }
}