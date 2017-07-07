using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class PasscarAllQuery : System.Web.UI.Page
    {
        #region 成员变量

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private SettingManager settingManager = new SettingManager();
        private Bll.ServiceManager servicemansger = new Bll.ServiceManager();
        private UserLogin userLogin = new UserLogin();
        private DataCommon dataCommon = new DataCommon();
        private static string starttime = "";
        private static string endtime = "";
        private MapManager bll = new MapManager();
        private static string departImg = "../Css/KakouSelectCss/ext.png";
        private static string kakouImg = "../Css/KakouSelectCss/threemenu.png.gif";
        public static string getJson = "";

        /// <summary>
        /// 功能模块名称（例如：综合查询 ）
        /// </summary>
        public static string dyzgnmkmc = "";

        /// <summary>
        /// 功能模块编号
        /// </summary>
        public static string dyzgnmkbh = "";

        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] fstrs = Request.QueryString["funcname"].Split('-');
            if (fstrs.Length > 0)
            {
                dyzgnmkmc = fstrs[1];
            }

            dyzgnmkbh = Request.QueryString["funcid"].ToString();
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("PasscarAllQuery20", " 您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }

            if (!X.IsAjaxRequest)
            {
                Session["tree"] = null;
                string js1 = "clearMenu();";
                this.ResourceManager1.RegisterAfterClientInitScript(js1);
                StoreDataBind();
                DataSetDateTime();
                // BuildTree(TreeStation.Root);

                this.DataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "访问：" + Request.QueryString["funcname"], userinfo.NowIp, "0");
            }
        }

        /// <summary>
        /// 转换查询模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void changtype(object sender, EventArgs e)
        {
            TxtplateId.Hidden = cktype.Checked;
            pnhphm.Hidden = !cktype.Checked;
        }

        /// <summary>
        ///显示Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GetWindow(object sender, EventArgs e)
        {
            this.Window1.Reload();
            Session["stationlist"] = null;
            Session["stationlistname"] = null;
            this.Window1.Show();
        }

        [DirectMethod(Namespace = "OnEvl")]
        public void showmap()
        {
            this.Window1.Reload();
            Session["stationlist"] = null;
            Session["stationlistname"] = null;
            this.Window1.Show();
        }

        [DirectMethod(Namespace = "OnEvl")]
        public void hidemap()
        {
            string nodeid = "";
            string nodeName = "";
            this.Window1.Hide();
            if (Session["stationlist"] != null)
            {
                System.Collections.Generic.List<string> listid = Session["stationlist"] as System.Collections.Generic.List<string>;
                foreach (string str in listid)
                {
                    nodeid += (nodeid == "" ? "" : ",") + str;
                }
            }
            if (Session["stationlistname"] != null)
            {
                System.Collections.Generic.List<string> listName = Session["stationlistname"] as System.Collections.Generic.List<string>;
                foreach (string str in listName)
                {
                    nodeName += (nodeName == "" ? "" : ",") + str;
                }
            }
            // string js = "setSelect('" + nodeid + "','" + nodeName + "');";
            string js = "setSelect('" + nodeName + "');";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }

        [DirectMethod(Namespace = "OnEvl")]
        public void hidemap1()
        {
            this.Window1.Hide();
        }

        //[DirectMethod]
        //public void SetKakou(string ids,string names)
        //{
        //    //TreeStation.ClearContent();
        //     TreeStation.SelectNode("371600000000");

        //    List<SubmittedNode> list= TreeStation.CheckedNodes;
        //    FieldStation.SetValue(ids, names);
        //}

        #endregion 控件事件

        #region DirectMethod

        /// <summary>
        /// 清除卡口信息
        /// </summary>
        [DirectMethod]
        public void ClearKakou()
        {
            if (Session["Condition"] != null)
            {
                Condition con = Session["Condition"] as Condition;
                con.Kkid = "";
                con.Kkidms = "";
            }
            if (Session["tree"] != null)
            {
                Session["tree"] = null;
            }
        }

        /// <summary>
        /// 给主页面的文本框赋值
        /// </summary>
        ///
        [DirectMethod]
        public void SetMainValue()
        {
            if (Session["Condition"] != null)
            {
                Condition con = Session["Condition"] as Condition;
                //开始时间
                start.InnerText = con.StartTime;
                starttime = con.StartTime;
                //结束时间
                end.InnerText = con.EndTime;
                endtime = con.EndTime;
                //车牌号牌
                vehicleHead.SetVehicleText(con.Sqjc);
                if (con.QueryMode.Equals("0"))
                {
                    pnhphm.Hidden = false;
                    TxtplateId.Hidden = true;
                    if (con.Hphm.Length < 6)
                    {
                        int length = con.Hphm.Length;
                        for (int i = 0; i < 6 - length; i++)
                        {
                            con.Hphm = con.Hphm + "_";
                        }
                    }

                    haopai_name1.Value = con.Hphm.Substring(0, 1);
                    if (haopai_name1.Value.Equals("_"))
                    {
                        haopai_name1.Value = "";
                    }
                    haopai_name2.Value = con.Hphm.Substring(1, 1);
                    if (haopai_name2.Value.Equals("_"))
                    {
                        haopai_name2.Value = "";
                    }
                    haopai_name3.Value = con.Hphm.Substring(2, 1);
                    if (haopai_name3.Value.Equals("_"))
                    {
                        haopai_name3.Value = "";
                    }
                    haopai_name4.Value = con.Hphm.Substring(3, 1);
                    if (haopai_name4.Value.Equals("_"))
                    {
                        haopai_name4.Value = "";
                    }
                    haopai_name5.Value = con.Hphm.Substring(4, 1);
                    if (haopai_name5.Value.Equals("_"))
                    {
                        haopai_name5.Value = "";
                    }
                    haopai_name6.Value = con.Hphm.Substring(5, 1);
                    if (haopai_name6.Value.Equals("_"))
                    {
                        haopai_name6.Value = "";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(con.Hphm))
                    {
                        TxtplateId.Text = con.Hphm;
                    }
                }

                //号牌种类
                CmbPlateType.Value = con.Hpzl;

                //卡口
                kakou.Value = con.Kkidms;
                kakouId.Value = con.Kkid;
                string js = "setMainValue('" + con.Kkidms + "','" + con.Kkid + "');";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
                //车身颜色
                CmbCsys.Value = con.Csys;
                //模糊查询
                if (con.QueryMode == "1")
                {
                    cktype.Checked = false;
                }
                else
                {
                    cktype.Checked = true;
                }
                //车辆品牌
                if (con.Clpp.ToString().Contains("-"))
                {
                    int i = con.Clpp.ToString().IndexOf("-");
                    ClppChoice.Value = con.Clpp.ToString().Substring(1, i - 1);
                }
                else
                {
                    ClppChoice.Value = con.Clpp;
                }

                //行驶方向
                CmbXsfx.Value = con.Xsfx;
                //车道
                txtXscd.Text = con.Xscd;
                txtDsd.Text = con.Dsd;
                txtGsd.Text = con.Gsd;
            }
        }

        /// <summary>
        /// 卡口模糊查询选中的时候给Session["tree"]赋值
        /// </summary>
        [DirectMethod]
        public void SetSession()
        {
            if (Session["tree"] != null)
            {
                Session["tree"] = null;
            }
            Session["tree"] = kakouId.Value;
        }

        /// <summary>
        /// 得到符合条件的卡口
        /// </summary>
        [DirectMethod]
        public void GetKakou()
        {
            try
            {
                string value = kakou.Value;

                DataTable dtSelect = null;
                DataTable dt = Session["selectKakou"] as DataTable;//得到整个卡口信息
                DataRow[] rows = dt.Select("STATION_NAME like '" + value + "%'");//选出符合条件的
                if (rows.Length > 0)
                {
                    dtSelect = ToDataTable(rows);
                }
                StringBuilder strs = new StringBuilder();
                strs.AppendLine("[");
                if (dtSelect != null && dtSelect.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSelect.Rows.Count; i++)
                    {
                        if (i == dtSelect.Rows.Count - 1)
                        {
                            strs.AppendLine("{name:'" + dtSelect.Rows[i]["STATION_NAME"] + "',id:'" + dtSelect.Rows[i]["STATION_ID"] + "'}");
                        }
                        else
                        {
                            strs.AppendLine("{name:'" + dtSelect.Rows[i]["STATION_NAME"] + "',id:'" + dtSelect.Rows[i]["STATION_ID"] + "'},");
                        }
                    }
                }
                else
                {
                    strs.AppendLine("{name:'none',id:'none'},");
                }
                strs.AppendLine("]");
                string js = "setUl(" + strs.ToString() + ");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        /// 打开页面
        /// </summary>
        /// <param name="id"></param>
        /// <param name="url"></param>
        [DirectMethod]
        public void ImgClick(string id, string url)
        {
            //if (id.Equals("3002") || id.Equals("3011") || id.Equals("3013") || id.Equals("3014") || id.Equals("3006") || id.Equals("3007")|| id.Equals("3005"))
            //{
            if (string.IsNullOrEmpty(CmbPlateType.Text) && string.IsNullOrEmpty(TxtplateId.Text) && string.IsNullOrEmpty(vehicleHead.VehicleText) &&
             string.IsNullOrEmpty(kakou.Value) && string.IsNullOrEmpty(haopai_name1.Value) && string.IsNullOrEmpty(haopai_name2.Value) && string.IsNullOrEmpty(haopai_name3.Value)
             && string.IsNullOrEmpty(haopai_name4.Value) && string.IsNullOrEmpty(haopai_name5.Value) && string.IsNullOrEmpty(haopai_name6.Value) && cktype.Checked == false
             )
            {
                DateTime start = Convert.ToDateTime(starttime);
                DateTime end = Convert.ToDateTime(endtime);
                TimeSpan sp = end.Subtract(start);
                if (sp.TotalMinutes > 120 && !id.Equals("3005"))
                {
                    Notice(GetLangStr("PasscarAllQuery21", "信息提示"), GetLangStr("PasscarAllQuery22", "只能选择两个小时之内的时间！"));
                    return;
                }
            }
            // }
            if ((!string.IsNullOrEmpty(CmbPlateType.Text) && (!string.IsNullOrEmpty(vehicleHead.VehicleText) && !string.IsNullOrEmpty(TxtplateId.Text))) ||
                (!string.IsNullOrEmpty(CmbPlateType.Text) && cktype.Checked == true)
                )
            {
            }
            else if (!string.IsNullOrEmpty(kakou.Value))
            {
                if (kakou.Value.Contains(","))
                {
                    string[] strs = kakou.Value.Split(',');
                    if (strs.Length > 10)
                    {
                        Notice(GetLangStr("PasscarAllQuery21", "信息提示"), GetLangStr("PasscarAllQuery23", "最多只能选择10个卡口！"));
                        return;
                    }
                }
            }
            else
            {
            }
            if (string.IsNullOrEmpty(starttime) || string.IsNullOrEmpty(endtime))
            {
                Notice(GetLangStr("PasscarAllQuery21", "信息提示"), GetLangStr("PasscarAllQuery24", "请选择开始时间及结束时间！"));
                return;
            }

            // if (cktype.Checked)
            // {
            //     if( id.Equals("3003") && (string.IsNullOrEmpty(vehicleHead.VehicleText) || string.IsNullOrEmpty(haopai_name1.Value) || string.IsNullOrEmpty(haopai_name2.Value) || string.IsNullOrEmpty(haopai_name3.Value)
            //|| string.IsNullOrEmpty(haopai_name4.Value) || string.IsNullOrEmpty(haopai_name5.Value) || string.IsNullOrEmpty(haopai_name6.Value)))
            //     {
            //         Notice("信息提示", "请输入号牌号码！");
            //         return;
            //     }
            // }
            // else
            // {
            //     if (id.Equals("3003") && (string.IsNullOrEmpty(vehicleHead.VehicleText) || string.IsNullOrEmpty(TxtplateId.Text))
            //   )
            //     {
            //         Notice("信息提示", "请输入号牌号码！");
            //         return;
            //     }
            // }

            //if (id.Equals("3013") && CmbPlateType.SelectedIndex == -1)
            //{
            //    Notice("信息提示", "请输入号牌种类！");
            //    return;
            //}

            if (cktype.Checked)
            {
                // 省区简称  号牌号码 为空， 车牌种类 为空
                if (string.IsNullOrEmpty(vehicleHead.VehicleText) || CmbPlateType.SelectedIndex == -1 || string.IsNullOrEmpty(haopai_name1.Value) || string.IsNullOrEmpty(haopai_name2.Value) || string.IsNullOrEmpty(haopai_name3.Value)
        || string.IsNullOrEmpty(haopai_name4.Value) || string.IsNullOrEmpty(haopai_name5.Value) || string.IsNullOrEmpty(haopai_name6.Value)
                    )
                {
                    if (id.Equals("3001") || id.Equals("3004") || id.Equals("3008") || id.Equals("3009") || id.Equals("3010") || id.Equals("3003"))
                    {
                        Notice(GetLangStr("PasscarAllQuery21", "信息提示"), GetLangStr("PasscarAllQuery25", "模糊查询状态,请输入完整号牌号码和号牌种类！"));
                        return;
                    }
                }
            }
            else
            {
                // 省区简称  号牌号码 为空， 车牌种类 为空
                if (string.IsNullOrEmpty(vehicleHead.VehicleText) || string.IsNullOrEmpty(TxtplateId.Text) || CmbPlateType.SelectedIndex == -1 || cktype.Checked == true)
                {
                    if (id.Equals("3001") || id.Equals("3004") || id.Equals("3008") || id.Equals("3009") || id.Equals("3010") || id.Equals("3003"))
                    {
                        Notice(GetLangStr("PasscarAllQuery21", "信息提示"), GetLangStr("PasscarAllQuery26", "请输入号牌号码和号牌种类！"));
                        return;
                    }
                }
            }

            Condition con = GetQueryInfo();
            if (Session["Condition"] != null)
            {
                Session["Condition"] = null;
            }
            Session["Condition"] = con;
            string js = "MenuItemClick('" + url + "');hideMenu();";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
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

        #endregion DirectMethod

        #region 私有方法

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                starttime = "";
                endtime = "";
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

                DataTable dt2 = GetRedisData.GetData("t_sys_code:240022");
                if (dt2 != null)
                {
                    this.StoreDataSource.DataSource = Bll.Common.ChangColName(dt2);// tgsPproperty.GetDeviceTypeDict("240022");
                    this.StoreDataSource.DataBind();
                }
                else
                {
                    this.StoreDataSource.DataSource = tgsPproperty.GetDeviceTypeDict("240022");
                    this.StoreDataSource.DataBind();
                }

                DataTable dt3 = GetRedisData.GetData("t_sys_code:240013");
                //车身颜色
                if (dt3 != null)
                {
                    this.StoreCsys.DataSource = Bll.Common.ChangColName(dt3);
                    this.StoreCsys.DataBind();
                }
                else
                {
                    this.StoreCsys.DataSource = tgsPproperty.GetCarColorDict();
                    this.StoreCsys.DataBind();
                }
                DataTable dt4 = GetRedisData.GetData("t_sys_code:240025");
                //行驶方向
                if (dt4 != null)
                {
                    this.StoreXsfx.DataSource = Bll.Common.ChangColName(dt4);
                    this.StoreXsfx.DataBind();
                }
                else
                {
                    this.StoreXsfx.DataSource = tgsPproperty.GetDirectionDict();
                    this.StoreXsfx.DataBind();
                }
                DataTable dt5 = GetRedisData.GetData("t_cfg_department");
                if (dt5 != null)
                {
                    this.StoreCjjg.DataSource = Bll.Common.ChangColName(dt5);//tgsPproperty.GetDepartmentDict();
                    this.StoreCjjg.DataBind();
                }
                else
                {
                    this.StoreCjjg.DataSource = Bll.Common.ChangColName(tgsPproperty.GetDepartmentDict());
                    this.StoreCjjg.DataBind();
                }

                this.StoreQuery.DataSource = GetDataSource();
                this.StoreQuery.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private Condition GetQueryInfo()
        {
            try
            {
                Condition condition = new Condition();
                if (!string.IsNullOrEmpty(starttime))
                {
                    condition.StartTime = DateTime.Parse(starttime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    condition.EndTime = DateTime.Parse(endtime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (!string.IsNullOrEmpty(vehicleHead.VehicleText))
                {
                    condition.Sqjc = vehicleHead.VehicleText;
                }

                if (cktype.Checked)
                {
                    condition.QueryMode = "0";
                    condition.Hphm = (string.IsNullOrEmpty(haopai_name1.Value) ? "_" : haopai_name1.Value) +
                    (string.IsNullOrEmpty(haopai_name2.Value) ? "_" : haopai_name2.Value) +
                    (string.IsNullOrEmpty(haopai_name3.Value) ? "_" : haopai_name3.Value) +
                    (string.IsNullOrEmpty(haopai_name4.Value) ? "_" : haopai_name4.Value) +
                    (string.IsNullOrEmpty(haopai_name5.Value) ? "_" : haopai_name5.Value) +
                    (string.IsNullOrEmpty(haopai_name6.Value) ? "_" : haopai_name6.Value);
                    //if (condition.Hphm.Substring(0, 6) == "______")
                    //    condition.Hphm = "%";
                }
                else
                {
                    condition.QueryMode = "1";
                    if (!string.IsNullOrEmpty(TxtplateId.Text))
                    {
                        condition.Hphm = TxtplateId.Text;
                    }
                }
                if (CmbPlateType.SelectedIndex != -1)
                {
                    condition.Hpzl = CmbPlateType.SelectedItem.Value;
                }
                if (!string.IsNullOrEmpty(ClppChoice.Value))
                {
                    condition.Clpp = ClppChoice.Value;
                }
                //if (CmbClpp.SelectedIndex != -1)
                //{
                //    condition.Clpp = CmbClpp.SelectedItem.Value;
                //    condition.ClppText = CmbClpp.SelectedItem.Text;
                //    //子品牌赋值
                //    // condition.Clzpp = CmbClzpp.SelectedItem.Value;
                //}
                //if (CmbClzpp.SelectedIndex != -1)
                //{
                //    condition.ClzppText = CmbClzpp.SelectedItem.Text;
                //    condition.Clzpp = CmbClzpp.SelectedItem.Value;
                //}
                if (CmbCsys.SelectedIndex != -1)
                {
                    condition.Csys = CmbCsys.SelectedItem.Value;
                }
                if (CmbXsfx.SelectedIndex != -1)
                {
                    condition.Xsfx = CmbXsfx.SelectedItem.Value;
                }
                if (!string.IsNullOrEmpty(txtXscd.Text))
                {
                    condition.Xscd = txtXscd.Text;
                }
                if (!string.IsNullOrEmpty(this.kakou.Value))
                {
                    string kkid = this.kakouId.Value.ToString();
                    if (!string.IsNullOrEmpty(kkid))
                    {
                        condition.Kkid = kkid;
                        if (Session["tree"] != null)
                        {
                            Session["tree"] = null;
                        }
                        Session["tree"] = kkid;
                    }
                    condition.Kkidms = this.kakou.Value;
                }
                //得到低速度
                if (!string.IsNullOrEmpty(txtDsd.Text))
                {
                    condition.Dsd = txtDsd.Text;
                }
                //得到高速度
                if (!string.IsNullOrEmpty(txtGsd.Text))
                {
                    condition.Gsd = txtGsd.Text;
                }
                ////短车长
                //if (!string.IsNullOrEmpty(TextField3.Text))
                //{
                //    condition.Dcc = TextField3.Text;
                //}
                ////长车长
                //if (!string.IsNullOrEmpty(TextField4.Text))
                //{
                //    condition.Ccc = TextField4.Text;
                //}
                return condition;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 转换kkid
        /// </summary>
        /// <param name="sic"></param>
        /// <returns></returns>
        private string GetMultiCombo(SelectedListItemCollection sic)
        {
            try
            {
                string kkid = string.Empty;
                string kkid2 = string.Empty;
                for (int i = 0; i < sic.Count; i++)
                {
                    kkid2 = kkid2 + "'" + sic[i].Value + "',";
                }
                if (!string.IsNullOrEmpty(kkid2))
                {
                    kkid = kkid2.Substring(0, kkid2.Length - 1);
                }

                return kkid;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "";
            }
        }

        /// <summary>
        /// 组装下方图标显示
        /// </summary>
        /// <returns></returns>
        private object GetDataSource()
        {
            string xml = "<group>";
            DataTable dt = dataCommon.GetDataTable("SELECT queryid,queryname,querypage FROM t_cfg_query_config where APPLICATIONTYPE='0' ORDER BY CAST(sx AS SIGNED) ASC");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    xml = xml + "<item>";
                    xml = xml + "<title>" + item[1].ToString() + "</title>";
                    xml = xml + "<uri>" + item[2].ToString() + "</uri>";
                    xml = xml + "<item-icon>../images/QueryImage/" + item[0].ToString() + ".png</item-icon>";
                    xml = xml + "<id>" + item[0].ToString() + "</id>";
                    xml = xml + "</item>";
                }

                xml = xml + "</group>";

                xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><groups defaultIcon=\"images/icon.png\">" + xml + "</groups>";

                XElement document = XElement.Parse(xml);

                var defaultIcon = document.Attribute("defaultIcon") != null ? document.Attribute("defaultIcon").Value : "";

                var query = from g in document.Elements("group")
                            select new
                            {
                                Title = g.Attribute("title") != null ? g.Attribute("title").Value : "",
                                Items = (from i in g.Elements("item")
                                         select new
                                         {
                                             Title = i.Element("title") != null ? i.Element("title").Value : "",
                                             Icon = i.Element("item-icon") != null ? i.Element("item-icon").Value : defaultIcon,
                                             Id = i.Element("id") != null ? i.Element("id").Value : "",
                                             Uri = i.Element("uri") != null ? i.Element("uri").Value : ""
                                         }
                                    )
                            };
                return query;
            }
            return "";
        }

        /// <summary>
        /// 得到一个Json字符串
        /// </summary>
        /// <returns></returns>
        public string GetJson()
        {
            DataTable dt = null;//得到部门
            DataTable dt1 = tgsDataInfo.GetJson(out dt);//得到部门下面的卡口
            StringBuilder str = new StringBuilder();
            str.AppendLine("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str.AppendLine("{id:" + dt.Rows[i]["departid"] + ",pid:" + dt.Rows[i]["classcode"] + ",name:'" + dt.Rows[i]["departname"] + "', open: true, check: true,icon:'" + departImg + "'},");
            }
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                if (j == dt1.Rows.Count - 1)
                {
                    str.AppendLine("{id:" + dt1.Rows[j]["station_id"] + ",pid:" + dt1.Rows[j]["departid"] + ",name:'" + dt1.Rows[j]["station_name"] + "', icon:'" + kakouImg + "'}");
                }
                else
                {
                    str.AppendLine("{id:" + dt1.Rows[j]["station_id"] + ",pid:" + dt1.Rows[j]["departid"] + ",name:'" + dt1.Rows[j]["station_name"] + "',icon:'" + kakouImg + "'},");
                }
            }
            str.AppendLine("]");
            string strs = str.ToString();
            return strs;
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
        /// 设置初始时间
        /// </summary>
        private void DataSetDateTime()
        {
            starttime = DateTime.Now.AddHours(-2).ToString("yyyy-MM-dd HH:mm:ss");
            start.InnerText = starttime;

            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            end.InnerText = endtime;
        }

        /// <summary>
        /// 组件卡口列表树
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private Ext.Net.TreeNodeCollection BuildTree(Ext.Net.TreeNodeCollection nodes)
        {
            try
            {
                if (nodes == null)
                {
                    nodes = new Ext.Net.TreeNodeCollection();
                }
                nodes.Clear();

                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Text = GetLangStr("PasscarAllQuery27", "卡口列表");
                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;

                // 添加 自己机构节点 和卡口
                UserInfo user = Session["userinfo"] as UserInfo;
                if (user == null)
                {
                    user = new UserInfo();
                    user.DepartName = GetLangStr("PasscarAllQuery28", "滨州市交通警察支队");
                    user.DeptCode = "371600000000";
                }

                Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                nodeRoot.Text = user.DepartName;
                nodeRoot.Leaf = true;
                nodeRoot.NodeID = user.DeptCode;
                nodeRoot.Icon = Icon.House;
                nodeRoot.Qtip = "Bumen";
                nodeRoot.Checked = ThreeStateBool.False;//加的部门选择框
                DataTable dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
                DataRow[] rowsStation = dtStation.Select("departid='" + user.DeptCode + "'", "station_name asc");
                AddStationTree(nodeRoot, rowsStation);
                //DataTable dtStation = tgsPproperty.GetStationInfo(" departid='" + user.DeptCode + "' ");
                //AddStationTree(nodeRoot, dtStation);

                nodeRoot.Expanded = false;
                nodeRoot.Draggable = true;
                nodeRoot.Expandable = ThreeStateBool.True;
                root.Nodes.Add(nodeRoot);

                //绑定下级部门及下级部门卡口
                AddDepartTree(root, user.DeptCode);

                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        // [DirectMethod]
        //public void GetKakou()
        //{
        //    string value = this.FieldStation.Value.ToString();
        //    string text = this.FieldStation.Text.ToString();
        //    if (text.Contains("总队") || text.Contains("支队") || text.Contains("大队") || text.Contains("中队"))
        //    {
        //        if (text.Contains(","))
        //        {
        //        }
        //        else
        //        {
        //            DataTable dt = GetRedisData.GetData("Station:t_cfg_set_station");
        //            DataRow[] rows = dt.Select("departid='" + value + "'");
        //            if (rows != null)
        //            {
        //                for (int i = 0; i < rows.Count(); i++)
        //                {
        //                }
        //            }

        //        }
        //    }
        //    else
        //    {
        //    }
        //}
        /// <summary>
        ///绑定下级部门及下级部门卡口
        /// </summary>
        /// <param name="root"></param>
        private void AddDepartTree(Ext.Net.TreeNode root, string departCode)
        {
            try
            {
                DataTable dtDepart = GetRedisData.GetData("t_cfg_department");
                DataRow[] rows = dtDepart.Select("classcode='" + departCode + "'", "class asc");
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count(); i++)
                    {
                        Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                        nodeRoot.Text = rows[i]["departname"].ToString();
                        nodeRoot.Leaf = true;
                        nodeRoot.NodeID = rows[i]["departid"].ToString();
                        nodeRoot.Icon = Icon.House;
                        nodeRoot.Checked = ThreeStateBool.False;//加的部门选择框
                        nodeRoot.Qtip = "Bumen";
                        DataTable dtStation = GetRedisData.GetData("Station:t_cfg_set_station");
                        DataRow[] rowsStation = dtStation.Select(" departid='" + nodeRoot.NodeID + "' ", "station_name asc");
                        AddStationTree(nodeRoot, rowsStation);
                        nodeRoot.Expanded = false;
                        nodeRoot.Draggable = true;
                        nodeRoot.Expandable = ThreeStateBool.True;
                        AddDepartTree(nodeRoot, rows[i]["departid"].ToString());
                        root.Nodes.Add(nodeRoot);
                    }
                }
                //DataTable dtDepart = settingManager.GetLowerDepartment(departCode);
                //if (dtDepart != null && dtDepart.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dtDepart.Rows.Count; i++)
                //    {
                //        Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                //        nodeRoot.Text = dtDepart.Rows[i][2].ToString();//departname
                //       // nodeRoot.Text = dtDepart.Rows[i]["departname"].ToString();
                //        nodeRoot.Leaf = true;
                //        nodeRoot.NodeID = dtDepart.Rows[i][1].ToString();//departid
                //       // nodeRoot.NodeID = dtDepart.Rows[i]["departid"].ToString();
                //        nodeRoot.Icon = Icon.House;

                //        //string strStation = redisClient.GetRedisValue("Station:t_cfg_set_station");
                //        //DataTable dtStation = GetRedisData.GetData(strStation);
                //        //DataRow[] rowsStation = dtStation.Select(" departid='" + nodeRoot.NodeID + "' ");
                //        DataTable dtStation = tgsPproperty.GetStationInfo(" departid='" + nodeRoot.NodeID + "' ");
                //        AddStationTree(nodeRoot, dtStation);
                //        nodeRoot.Expanded = false;
                //        nodeRoot.Draggable = true;
                //        nodeRoot.Expandable = ThreeStateBool.True;
                //        AddDepartTree(nodeRoot, dtDepart.Rows[i]["departid"].ToString());
                //        root.Nodes.Add(nodeRoot);
                //    }
                //}
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 添加卡口子节点
        /// </summary>
        /// <param name="root"></param>
        private void AddStationTree(Ext.Net.TreeNode DepartNode, DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                        node.Text = dt.Rows[i]["col2"].ToString();
                        node.Leaf = true;
                        //string xx = chkstation.Find(x => x == dt.Rows[i]["col1"].ToString());
                        //if (chkstation==null||chkstation.Count == 0 || chkstation.Find(x => x == dt.Rows[i]["col1"].ToString()) == "")
                        node.Checked = ThreeStateBool.False;
                        //else
                        //    node.Checked = ThreeStateBool.True;
                        node.NodeID = dt.Rows[i]["col1"].ToString();
                        node.Draggable = false;
                        node.AllowDrag = false;
                        DepartNode.Nodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 添加卡口子节点
        /// </summary>
        /// <param name="root"></param>
        private void AddStationTree(Ext.Net.TreeNode DepartNode, DataRow[] rows)
        {
            try
            {
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count(); i++)
                    {
                        Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                        node.Text = rows[i]["station_name"].ToString();
                        node.Leaf = true;
                        node.Checked = ThreeStateBool.False;
                        node.NodeID = rows[i]["station_id"].ToString();
                        node.Draggable = false;
                        node.AllowDrag = false;
                        node.Qtip = "Kakou";
                        DepartNode.Nodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        //public void CheckChange_Event(object sender, DirectEventArgs e)
        //{
        //}

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