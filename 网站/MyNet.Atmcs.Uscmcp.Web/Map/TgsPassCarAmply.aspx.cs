using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;
using System.Xml;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class TgsPassCarAmply : System.Web.UI.Page
    {
        #region 成员变量
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();
        private static string kkid = "";
        private static QueryService.querypasscar client = new QueryService.querypasscar();
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
                kkid = Session["kkid"] as String;
                BuildTree(TreePanel1.Root, kkid);
                initcode();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "访问：用户登录", userinfo.NowIp, "0");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowDetails(object sender, DirectEventArgs e)
        {
            string data = e.ExtraParams["data"];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RefreshTime(object sender, DirectEventArgs e)
        {
            try
            {
                //while (listMq.Count > 20)
                //    listMq.RemoveAt(0);
                if (System.Configuration.ConfigurationManager.AppSettings["mq"].ToString().ToLower() == "true")
                {
                    SetTable();
                }
                else
                {
                    GetImageNow(kkid);
                }
                #region
                //string where = string.Empty;
                //   DataTable dt = GetDataTable("1");
                //   StorePassCar.DataSource = dt;
                //   StorePassCar.DataBind();

                //   if (dt.Rows.Count > 0)
                //   {
                //       string xh = dt.Rows[0][1].ToString();
                //       if (xh != PasscarXh.Value.ToString())
                //       {
                //           string surl1 = dt.Rows[0][27].ToString();
                //           string surl2 = dt.Rows[0][28].ToString();
                //           string hpzl = dt.Rows[0][5].ToString();
                //           string hpzlms = dt.Rows[0][6].ToString();
                //           string hphm = dt.Rows[0][4].ToString();
                //           string gwsj = dt.Rows[0][7].ToString();
                //           string xlsd = dt.Rows[0][19].ToString();
                //           string kkmc = dt.Rows[0][3].ToString();
                //           string fxmc = dt.Rows[0][17].ToString();
                //           string cjjg = dt.Rows[0][26].ToString();
                //           string msg = GetHtml(hpzl, hphm, hpzlms, gwsj, xlsd, kkmc, fxmc, cjjg);
                //           PasscarXh.Value = xh;
                //           //surl1 = Bll.Common.ChangePoliceIp(surl1);
                //           //surl2 = Bll.Common.ChangePoliceIp(surl2);
                //           ApplyImage(surl1, surl2);
                //           ApplyText(msg);
                //       }
                //   }
                #endregion 控件事件
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TgsPassCarAmply.aspx-RefreshTime", ex.Message + "；" + ex.StackTrace, "RefreshTime has an exception");
            }
        }

        public void GetImageNow(string kkid)
        {
            try
            {
                string xml = "", rsxml = "";
                xml = getxml(0, 10, System.DateTime.Now.AddMinutes(-30).ToString("yyyy-MM-dd HH:mm:ss"), System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "", "", "", "", "", kkid);
                rsxml = client.GetPassCarInfo(xml);
                if (rsxml != "" && getlenxml(rsxml) > 0)
                {
                    CXmlToDataTable(rsxml);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("TgsPassCarAmply.aspx-GetImageNow", ex.Message + "；" + ex.StackTrace, "GetImageNow has an exception");
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
                logManager.InsertLogError("TgsPassCarAmply.aspx-getxml", ex.Message + "；" + ex.StackTrace, "getxml has an exception");
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
                DataTable dt = CreateTable();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlStr);

                XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/passcarinfolist/passcarinfo");

                dt = settableweb(listNodes);
                dt.DefaultView.Sort = "col7 desc";
                dt = dt.DefaultView.ToTable();
                StorePassCar.DataSource = dt;
                StorePassCar.DataBind();
                if (dt.Rows.Count > 0)
                {
                    string surl1 = dt.Rows[0][27].ToString();
                    string surl2 = dt.Rows[0][28].ToString();
                    string hpzl = dt.Rows[0][5].ToString();
                    string hpzlms = dt.Rows[0][6].ToString();
                    string hphm = dt.Rows[0][4].ToString();
                    string gwsj = dt.Rows[0][7].ToString();
                    string xlsd = dt.Rows[0][19].ToString();
                    string kkmc = dt.Rows[0][3].ToString();
                    string fxmc = dt.Rows[0][17].ToString();
                    string cjjg = dt.Rows[0][26].ToString();
                    string msg = GetHtml(hpzl, hphm, hpzlms, gwsj, xlsd, kkmc, fxmc, cjjg);
                    ApplyImage(surl1, surl2);
                    ApplyText(msg);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("TgsPassCarAmply.aspx-CXmlToDataTable", ex.Message + "；" + ex.StackTrace, "CXmlToDataTable has an exception");
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
                logManager.InsertLogError("TgsPassCarAmply.aspx-getlenxml", ex.Message + "；" + ex.StackTrace, "getlenxml has an exception");
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
            dt.Columns.Add("xh", Type.GetType("System.String"));
            return dt;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectPassCar(object sender, DirectEventArgs e)
        {
            try
            {
                object data = e.ExtraParams["sdata"];
                string sdata = data.ToString();
                string xh = GetdatabyField(sdata, "col0");
                string surl1 = GetdatabyField(sdata, "col27");
                string surl2 = GetdatabyField(sdata, "col28");
                string hpzl = GetdatabyField(sdata, "col5");
                string hphm = GetdatabyField(sdata, "col4");
                string hpzlms = GetdatabyField(sdata, "col6");
                string gwsj = GetDate(sdata, "col7", 0);
                string xlsd = GetdatabyField(sdata, "col19");
                string kkmc = GetdatabyField(sdata, "col3");
                string fxmc = GetdatabyField(sdata, "col17");
                string cjjg = GetdatabyField(sdata, "col26");
                string msg = GetHtml(hpzl, hphm, hpzlms, gwsj, xlsd, kkmc, fxmc, cjjg);
                ApplyImage(surl1, surl2);
                ApplyText(msg);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TgsPassCarAmply.aspx-SelectPassCar", ex.Message + "；" + ex.StackTrace, "SelectPassCar has an exception");
            }
        }

        #endregion 控件事件

        #region 私有方法

        /// <summary>
        ///
        /// </summary>
        /// <param name="rownum"></param>
        /// <returns></returns>
        protected DataTable GetDataTable(string rownum)
        {
            try
            {
                kkid = Session["kkid"] as String;
                string where = " rownum >=" + rownum + "";
                if (!string.IsNullOrEmpty(this.RevStation.Value.ToString()))
                {
                    where = "   kkid||fxbh  in (" + this.RevStation.Value.ToString() + ") and " + where;
                }
                else
                {
                    where = "   kkid= '" + kkid + "'  and " + where;
                }
                return tgsDataInfo.GetPassCarMonitor(where);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TgsPassCarAmply.aspx-GetDataTable", ex.Message + "；" + ex.StackTrace, "GetDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="surl1"></param>
        /// <param name="surl2"></param>
        protected void ApplyImage(string surl1, string surl2)
        {
            try
            {
                var tpl = new XTemplate { ID = "Template1" };

                tpl.Html = @"<div class=""details"">
			        <tpl for=""."">
				       <center>
                        <img src=""{url1}""  onload=""resizeimg(this);""  alt=""车辆图片(双击图片进行放大)"" onDblClick='OpenPicPage(this.src)'; />&nbsp;&nbsp;
                        <img src=""{url2}""  onload=""resizeimg(this);"" alt=""车辆图片(双击图片进行放大)"" onDblClick=""OpenPicPage(this.src)"";/>
                        </center>
			        </tpl>
		            </div>";
                tpl.Overwrite(this.ImagePanel, new
                {
                    url1 = surl1,
                    url2 = surl2
                });

                tpl.Render();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TgsPassCarAmply.aspx-ApplyImage", ex.Message + "；" + ex.StackTrace, "ApplyImage has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        protected string GetHtml(string image1, string image2)
        {
            try
            {
                string Html = @"<div class=""details"">
			        <tpl for=""."">
				       <center>
                        <img src=""{0}""  width=""380"" onDblClick=""OpenPicPage(this.src)""; />
                        <img src=""{1}"" width=""380"" onDblClick=""OpenPicPage(this.src)""; /></center>
			        </tpl>
		            </div>";
                return string.Format(Html, image1, image2);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TgsPassCarAmply.aspx-GetHtml", ex.Message + "；" + ex.StackTrace, "GetHtml has an exception");
                return "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hpzl"></param>
        /// <param name="hphm"></param>
        /// <param name="hpzlms"></param>
        /// <param name="gwsj"></param>
        /// <param name="xlsd"></param>
        /// <param name="kkmc"></param>
        /// <param name="xsfx"></param>
        /// <param name="cjjg"></param>
        /// <returns></returns>
        private string GetHtml(string hpzl, string hphm, string hpzlms, string gwsj, string xlsd, string kkmc, string xsfx, string cjjg)
        {
            try
            {
                string html = "";
                switch (hpzl)
                {
                    case "01":
                        html = " <font size =\"4\" color=\"#000000\"><b><span style=\"background-color: #FFFF00\">" + hphm + "</span></b></font>";
                        break;

                    case "02":
                        html = " <font size =\"4\" color=\"#FFFFFF\"><b><span style=\"background-color: #000080\">" + hphm + "</span></b></font>";
                        break;

                    case "23":
                        html = " <font size =\"4\" color=\"#FF0000\"><b><span style=\"background-color: #FFFFFF\">" + hphm + "</span></b></font>";
                        break;

                    case "06":
                        html = " <font size =\"4\" color=\"#FFFFFF\"><b><span style=\"background-color: #000000\">" + hphm + "</span></b></font>";
                        break;

                    default:
                        html = " <font size =\"4\" color=\"#FFFFFF\"><b><span style=\"background-color: #000080\">" + hphm + "</span></b></font>";
                        break;
                }
                html = html + " <font size =\"3\" color=\"#000080\"><b>号牌种类：" + hpzlms + "|过往时间：" + gwsj + "|过往卡口：" + kkmc + "|行驶方向：" + xsfx + "</b></font><br>";
                html = html + " <font size =\"3\" color=\"#000080\"><b>所属机构：" + cjjg + "</b></font>";
                return html;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TgsPassCarAmply.aspx-GetHtml", ex.Message + "；" + ex.StackTrace, "GetHtml has an exception");
                return "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        protected void ApplyText(string text)
        {
            try
            {
                var tpl = new XTemplate { ID = "Template1" };

                tpl.Html = @"<div class=""details""> <tpl for="".""> <center>{content} </center> </tpl> </div>";
                tpl.Overwrite(this.TextPanel, new
                {
                    content = text
                });

                tpl.Render();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TgsPassCarAmply.aspx-ApplyText", ex.Message + "；" + ex.StackTrace, "ApplyText has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private string GetDate(string data, int flag)
        {
            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    DateTime dt = DateTime.Parse(data);
                    switch (flag)
                    {
                        case 0:
                            return dt.ToString("yyyy-MM-dd HH:mm:ss");

                        case 1:
                            return dt.ToString("yyyy-MM-dd");

                        case 2:
                            return dt.ToString("HH:mm");

                        default:
                            return dt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TgsPassCarAmply.aspx-GetDate", ex.Message + "；" + ex.StackTrace, "GetDate has an exception");
                return "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="field"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private string GetDate(string data, string field, int flag)
        {
            string s = GetdatabyField(data, field);
            return GetDate(s, flag);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private string GetdatabyField(string data, string field)
        {
            string f1 = "<" + field + ">";
            string f2 = "</" + field + ">";
            int i = data.IndexOf(f1);
            int j = data.IndexOf(f2);
            if (i >= 0 && j >= 0)
            {
                return data.Substring(i + f1.Length, j - i - f2.Length + 1);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="kkid"></param>
        /// <returns></returns>
        private Ext.Net.TreeNodeCollection BuildTree(Ext.Net.TreeNodeCollection nodes, string kkid)
        {
            try
            {
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }

                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Text = "Root";
                nodes.Add(root);
                root.Expanded = true;
                DataTable dt = tgsPproperty.GetStationInfo("station_id='" + kkid + "'");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    node.Text = dt.Rows[i][2].ToString();
                    node.Icon = Icon.Camera;
                    node.NodeID = dt.Rows[i][1].ToString();

                    Addree(node);
                    node.Expanded = true;
                    root.Nodes.Add(node);
                }

                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TgsPassCarAmply.aspx-BuildTree", ex.Message + "；" + ex.StackTrace, "BuildTree has an exception");
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="root"></param>
        private void Addree(Ext.Net.TreeNode root)
        {
            try
            {
                DataTable dt = tgsPproperty.GetDirectionInfoByStation(root.NodeID);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    node.Text = root.Text + dt.Rows[i][1].ToString();
                    node.Leaf = true;
                    node.Checked = ThreeStateBool.True;
                    node.NodeID = root.NodeID + dt.Rows[i][0].ToString();
                    node.Icon = Icon.ArrowNsew;
                    node.Expanded = true;
                    root.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("TgsPassCarAmply.aspx-Addree", ex.Message + "；" + ex.StackTrace, "Addree has an exception");
            }
        }

        #endregion 私有方法

        #region MQ方法

        private static System.Collections.Generic.List<string> listMqstr = new System.Collections.Generic.List<string>();

        [DirectMethod]
        public void InsertMq(string mqstr)
        {
            try
            {
                if (mqstr.Substring(0, 11) == "<<< MESSAGE")
                {
                    int mqstart = mqstr.IndexOf("{");
                    Json json = new Json(mqstr.Substring(mqstart));
                    System.Collections.Hashtable carhs = json["car"] as System.Collections.Hashtable;
                    if (Session["kkid"].ToString() == carhs["tgs"].ToString())
                    {
                        listMq.Add(carhs);
                        if (listMq.Count > 20)
                            listMq.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("TgsPassCarAmply.aspx-InsertMq", ex.Message + "；" + ex.StackTrace, "InsertMq has an exception");
            }
        }

        private void SetTable()
        {
            try
            {
                DataTable dt = CreateTable();
                if (Session["passcar"] != null)
                {
                    System.Collections.Generic.List<string> liststr = Session["passcar"] as System.Collections.Generic.List<string>;
                    listMq.Clear();
                    foreach (string mqstr in liststr)
                    {
                        if (mqstr.Substring(0, 11) == "<<< MESSAGE")
                        {
                            int mqstart = mqstr.IndexOf("{");
                            Json json = new Json(mqstr.Substring(mqstart));
                            System.Collections.Hashtable carhs = json["car"] as System.Collections.Hashtable;
                            carhs.Add("layoutId", json["layoutId"]);
                            carhs.Add("alarmTime", json["alarmTime"]);
                            listMq.Add(carhs);
                        }
                    }
                    dt = settable();

                    dt.DefaultView.Sort = "col7 desc";
                    dt = dt.DefaultView.ToTable();
                    StorePassCar.DataSource = dt;
                    StorePassCar.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        string surl1 = dt.Rows[0][27].ToString();
                        string surl2 = dt.Rows[0][28].ToString();
                        string hpzl = dt.Rows[0][5].ToString();
                        string hpzlms = dt.Rows[0][6].ToString();
                        string hphm = dt.Rows[0][4].ToString();
                        string gwsj = dt.Rows[0][7].ToString();
                        string xlsd = dt.Rows[0][19].ToString();
                        string kkmc = dt.Rows[0][3].ToString();
                        string fxmc = dt.Rows[0][17].ToString();
                        string cjjg = dt.Rows[0][26].ToString();
                        string msg = GetHtml(hpzl, hphm, hpzlms, gwsj, xlsd, kkmc, fxmc, cjjg);

                        ApplyImage(surl1, surl2);
                        ApplyText(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("TgsPassCarAmply.aspx-SetTable", ex.Message + "；" + ex.StackTrace, "SetTable has an exception");
            }
        }

        private DataTable CreateTable()
        {
            DataTable dt = new DataTable("PassCar");
            for (int i = 0; i < 33; i++)
                dt.Columns.Add("col" + i.ToString(), Type.GetType("System.String"));
            return dt;
        }

        private MyNet.Atmcs.Uscmcp.Bll.MapManager bll = new MapManager();
        private static DataTable dtcllx = new DataTable();
        private static DataTable dthpys = new DataTable();
        private static DataTable dtfxbh = new DataTable();

        private static DataTable dtclzt = new DataTable();
        private static DataTable dtkk = new DataTable();

        private void initcode()
        {
            try
            {
                dtcllx = bll.GetCllx().Tables[0];
                dtfxbh = bll.GetFxcode();
                dthpys = bll.GetCsys().Tables[0];
                dtclzt = bll.GetCarstate();
                dtkk = bll.GetStation();
            }
            catch (Exception ex) { ILog.WriteErrorLog(ex.Message); }
        }

        private string GetKkmc(string id)
        {
            try
            {
                return dtkk.Select("station_id='" + id + "'")[0]["station_name"].ToString();
            }
            catch
            { return ""; }
        }

        private string GetDepart(string id)
        {
            try
            {
                return dtkk.Select("station_id='" + id + "'")[0]["DEPARTNAME"].ToString();
            }
            catch
            { return ""; }
        }

        private string GetHpzl(string id)
        {
            try
            {
                return dtcllx.Select("code='" + id + "'")[0]["codedesc"].ToString();
            }
            catch
            { return ""; }
        }

        private string GetHpys(string id)
        {
            try
            {
                return dthpys.Select("code='" + id + "'")[0]["codedesc"].ToString();
            }
            catch
            { return ""; }
        }

        private string GetCllx(string id)
        {
            try
            {
                return dtcllx.Select("code='" + id + "'")[0]["codedesc"].ToString();
            }
            catch
            { return ""; }
        }

        private string GetFxbh(string id)
        {
            try
            {
                return dtfxbh.Select("code='" + id + "'")[0]["codedesc"].ToString();
            }
            catch
            { return ""; }
        }

        private string GetCarstate(string id)
        {
            try
            {
                return dthpys.Select("code='" + id + "'")[0]["codedesc"].ToString();
            }
            catch
            { return ""; }
        }

        private static System.Collections.Generic.List<System.Collections.Hashtable> listMq = new System.Collections.Generic.List<System.Collections.Hashtable>();

        private DataTable settable()
        {
            DataTable dt = CreateTable();
            int row = 0;
            foreach (System.Collections.Hashtable carhs in listMq)
            {
                DataRow dr = dt.NewRow();
                dr[0] = row.ToString();
                dr[1] = "";
                dr[2] = carhs["tgs"].ToString();
                dr[3] = GetKkmc(carhs["tgs"].ToString());//Session["kkidname"].ToString();
                dr[4] = carhs["plateNumber"].ToString();
                dr[5] = carhs["plateType"].ToString();
                dr[6] = GetHpzl(carhs["plateType"].ToString());
                dr[7] = carhs["timeStamp"].ToString();
                dr[8] = carhs["timeStamp"].ToString();
                dr[9] = carhs["timeStamp"].ToString();
                dr[10] = carhs["plateColor"].ToString();
                dr[11] = GetHpys(carhs["plateColor"].ToString());
                dr[12] = carhs["cllx"].ToString();
                dr[13] = GetCllx(carhs["cllx"].ToString());
                dr[14] = "";
                dr[15] = "";
                dr[16] = carhs["travelOrientation"].ToString();
                dr[17] = GetFxbh(carhs["travelOrientation"].ToString());
                dr[18] = carhs["landIe"].ToString();
                dr[19] = carhs["speed"].ToString();
                dr[20] = "";
                dr[21] = carhs["carState"].ToString();
                dr[22] = GetCarstate(carhs["carState"].ToString());
                dr[23] = "";
                dr[24] = "";
                dr[25] = "";
                dr[26] = GetDepart(carhs["tgs"].ToString());
                string zjwj = carhs["imgUrls"].ToString().Split(',')[0];
                dr[27] = zjwj == "" ? "" : zjwj.Substring(2, zjwj.Length - 3);
                zjwj = carhs["imgUrls"].ToString().Split(',')[1];
                dr[28] = zjwj == "" ? "" : zjwj.Substring(1, zjwj.Length - 2);
                zjwj = carhs["imgUrls"].ToString().Split(',')[2];
                dr[29] = zjwj == "" ? "" : zjwj.Substring(1, zjwj.Length - 2);
                dr[30] = "";
                dr[31] = "";
                dr[32] = "";
                dt.Rows.Add(dr);
                row++;
            }
            return dt;
            //if (Session["mq"] != null)
            //    Session["mq"] = null;
            //Session["mq"] = dt;
        }

        private DataTable settableweb(XmlNodeList list)
        {
            try
            {
                DataTable dt = CreateTable();
                int row = 0;
                foreach (XmlNode node in list)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = row.ToString();
                    dr[1] = "";
                    dr[2] = (node.SelectSingleNode("kkid")).InnerText;
                    dr[3] = GetKkmc((node.SelectSingleNode("kkid")).InnerText);//Session["kkidname"].ToString();
                    dr[4] = (node.SelectSingleNode("hphm")).InnerText;
                    dr[5] = (node.SelectSingleNode("hpzl")).InnerText;
                    dr[6] = GetHpzl((node.SelectSingleNode("hpzl")).InnerText);
                    dr[7] = (node.SelectSingleNode("gwsj")).InnerText;
                    dr[8] = (node.SelectSingleNode("gwsj")).InnerText;
                    dr[9] = (node.SelectSingleNode("gwsj")).InnerText;
                    dr[10] = (node.SelectSingleNode("hpys")).InnerText;
                    dr[11] = GetHpys((node.SelectSingleNode("hpys")).InnerText);
                    dr[12] = (node.SelectSingleNode("cllx")).InnerText;
                    dr[13] = GetCllx((node.SelectSingleNode("cllx")).InnerText);
                    dr[14] = "";
                    dr[15] = "";
                    dr[16] = (node.SelectSingleNode("fxbh")).InnerText;
                    dr[17] = GetFxbh((node.SelectSingleNode("fxbh")).InnerText);
                    dr[18] = (node.SelectSingleNode("cdbh")).InnerText; ;
                    dr[19] = (node.SelectSingleNode("cshm")).InnerText;
                    dr[20] = "";
                    dr[21] = (node.SelectSingleNode("jllx")).InnerText;
                    dr[22] = GetCarstate((node.SelectSingleNode("jllx")).InnerText);
                    dr[23] = "";
                    dr[24] = "";
                    dr[25] = "";
                    dr[26] = GetDepart((node.SelectSingleNode("kkid")).InnerText);
                    dr[27] = (node.SelectSingleNode("zjwj1")).InnerText;
                    dr[28] = (node.SelectSingleNode("zjwj2")).InnerText;
                    dr[29] = (node.SelectSingleNode("zjwj3")).InnerText;
                    dr[30] = "";
                    dr[31] = "";
                    dr[32] = "";
                    dt.Rows.Add(dr);
                    row++;
                }
                return dt;
            }
            catch (Exception ex)
            {
                logManager.InsertLogError("TgsPassCarAmply.aspx-settableweb", ex.Message + "；" + ex.StackTrace, "settableweb has an exception");
                return null;
            }
        }

        #endregion
    }
}