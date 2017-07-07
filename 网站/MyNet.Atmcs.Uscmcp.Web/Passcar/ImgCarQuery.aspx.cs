using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class ImgCarQuery : System.Web.UI.Page
    {
        #region 成员变量

        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private UserLogin userLogin = new UserLogin();
        private DataCommon dataCommon = new DataCommon();
        private static string starttime = "";
        private static string endtime = "";
        private OtherQueryService.OtherQueryInfo client = new OtherQueryService.OtherQueryInfo();

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
            this.EnableViewState = false;
            if (!X.IsAjaxRequest)
            {
                StoreDataBind();
                DataSetDateTime();
                this.vehicleHead.SetDisable(true);
            }
            //this.DataBind();
        }

        #endregion 控件事件

        #region DirectMethod

        /// <summary>
        /// 打开页面
        /// </summary>
        /// <param name="url"></param>
        [DirectMethod]
        public void ImgClick(string url)
        {
            if (!string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime))
            {
                string js = "MenuItemClick('" + url + "');";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
            else
            {
                Notice("信息提示", "请选择开始时间及结束时间");
            }
        }

        /// <summary>
        /// 获得选中时间
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

        /// <summary>
        /// 文件选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void FileUploadSelect(object sender, DirectEventArgs e)
        {
            try
            {
                if (this.ImgFile.HasFile)
                {
                    string imgUrl = UploadFile();
                    if (!string.IsNullOrEmpty(imgUrl))
                    {
                        imgShow.ImageUrl = imgUrl;
                        // 调用接口解析
                        string reXml = client.GetImageSearch(imgUrl);
                        //string reXml = "<?xml version='1.0' encoding='UTF-8'?> <Message><Version>1.0</Version><Type>RESPONSE</Type><Body><Return><passcarinfo><hphm>京A12345</hphm><hpzl>01</hpzl><hpys>1</hpys><cllx>K33</cllx><clpp>大众</clpp><clwx></clwx><csys>A</csys><zjhsl></zjhsl><zybsl>0</zybsl><dzsl>1</dzsl><bjsl>1</bjsl><njbsl>1</njbsl><zjsaqd>1</zjsaqd><fjsaqd>1</fjsaqd></passcarinfo></Return></Body> </Message>";
                        Condition con = AnalyzeXml(reXml);
                        if (con != null)
                        {
                            vehicleHead.SetVehicleText(con.Sqjc);
                            TxtplateId.Text = con.Hphm;
                            CmbPlateType.Text = Bll.Common.GetHpzlms(con.Hpzl);
                            txtClpp.Text = con.Clpp;
                            txtcsys.Text = Bll.Common.GetCsysms(con.Csys) + "色";
                            hiddenCsys.Value = con.Csys;
                            TFClzpp.Text = con.Clzpp;

                            Notice("提示信息", "图片识别成功");
                        }
                        else
                        {
                            Notice("提示信息", "图片识别失败，未能返回有效信息");
                        }
                    }
                }
                else
                {
                    Notice("提示信息", "请选择要识别的图片");
                }
                this.StoreQuery.DataSource = GetData();
                this.StoreQuery.DataBind();
            }
            catch (Exception)
            {
                throw;
            }
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
                this.StorePlateType.DataSource = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:140001"));//tgsPproperty.GetPalteType();
                this.StorePlateType.DataBind();

                this.StoreQuery.DataSource = GetData();
                this.StoreQuery.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 设置初始时间
        /// </summary>
        private void DataSetDateTime()
        {
            starttime = DateTime.Now.ToString("yyyy-MM-dd HH:00:00");
            start.InnerText = starttime;

            endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            end.InnerText = endtime;
        }

        /// <summary>
        /// 组装下方图标显示
        /// </summary>
        /// <returns></returns>
        private object GetData()
        {
            DataTable dt = dataCommon.GetDataTable("SELECT queryid,queryname,querypage FROM t_cfg_query_config where APPLICATIONTYPE='0' ORDER BY CAST(sx AS SIGNED) ASC");
            string xml = "<group title=\"常用功能\">";
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
            return xmlobj(xml);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private object xmlobj(string xml)
        {
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

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        public Condition GetQueryInfo()
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
                if (!string.IsNullOrEmpty(TxtplateId.Text))
                {
                    condition.Hphm = TxtplateId.Text;
                }
                if (CmbPlateType.SelectedIndex != -1)
                {
                    condition.Hpzl = CmbPlateType.SelectedItem.Value;
                }

                if (!string.IsNullOrEmpty(txtClpp.Text))
                {
                    condition.Clpp = txtClpp.Text;
                }
                if (!string.IsNullOrEmpty(TFClzpp.Text))
                {
                    condition.Clzpp = TFClzpp.Text;
                }

                if (!string.IsNullOrEmpty(hiddenCsys.Value.ToString()))
                {
                    condition.Csys = hiddenCsys.Value.ToString();
                }
                if (Session["Condition"] != null)
                {
                    Session["Condition"] = null;
                }
                Session["Condition"] = condition;
                return condition;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
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
        /// 上传选中图片
        /// </summary>
        /// <returns></returns>
        private string UploadFile()
        {
            try
            {
                string UploadFile = "";
                string strPath = "";
                if (this.ImgFile.HasFile)
                {
                    UploadFile = this.ImgFile.PostedFile.FileName.ToString();
                    int FileSize = Int32.Parse(this.ImgFile.PostedFile.ContentLength.ToString());
                    //if (FileSize > 5 * 1024 * 1024)
                    //{
                    //    X.Msg.Alert("提示信息", "上传文件过大！").Show();
                    //    return "";
                    //}
                    string fileType = Path.GetExtension(this.ImgFile.PostedFile.FileName).ToUpper();//获取文件后缀
                    string allowFile = ".JPG.JPEG.PNG.BMP";
                    if (allowFile.Contains(fileType.ToUpper()))
                    {
                        string sNewName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(this.ImgFile.PostedFile.FileName);
                        strPath = Server.MapPath("~/FileUpload/TEMP/" + DateTime.Now.ToString("yyyyMMdd") + "/" + sNewName);
                        if (!Directory.Exists(Path.GetDirectoryName(strPath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(strPath));
                        }
                        this.ImgFile.PostedFile.SaveAs(strPath);
                        string url = Request.Url.ToString();

                        return GetImgUrl(url) + "FileUpload/TEMP/" + DateTime.Now.ToString("yyyyMMdd") + "/" + sNewName;
                    }
                    else
                    {
                        X.Msg.Alert("提示信息", "文件格式不正确！").Show();
                        return "";
                    }
                }
                return strPath;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "";
            }
        }

        /// <summary>
        /// 截取URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetImgUrl(string url)
        {
            try
            {
                string[] urls = url.Split('/');
                string u = "";
                for (int i = 0; i < urls.Length - 2; i++)
                {
                    u += urls[i] + "/";
                }
                return u;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "";
            }
        }

        /// <summary>
        /// 解析xml
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public Condition AnalyzeXml(string xml)
        {
            try
            {
                Condition con = new Condition();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNode node = doc.SelectSingleNode("Message/Body/Return/passcarinfo");
                con.Sqjc = node.SelectSingleNode("hphm").InnerText.Substring(0, 1);
                con.Hphm = node.SelectSingleNode("hphm").InnerText.Substring(1);
                con.Hpzl = node.SelectSingleNode("hpzl").InnerText;
                con.Hpys = node.SelectSingleNode("hpys").InnerText;
                con.Cllx = node.SelectSingleNode("cllx").InnerText;
                con.Clpp = Bll.Common.Changenull(node.SelectSingleNode("clpp").InnerText);
                con.Clzpp = node.SelectSingleNode("clxh").InnerText;
                con.Csys = node.SelectSingleNode("csys").InnerText;
                string zjh = node.SelectSingleNode("zjhsl").InnerText;
                if (string.IsNullOrEmpty(zjh))
                {
                    con.Zjh = false;
                }
                else
                {
                    if (Convert.ToInt32(zjh) > 0)
                    {
                        con.Zjh = true;
                    }
                    else
                    {
                        con.Zjh = false;
                    }
                }
                string zyb = node.SelectSingleNode("zybsl").InnerText;
                if (string.IsNullOrEmpty(zyb))
                {
                    con.Zyb = false;
                }
                else
                {
                    if (Convert.ToInt32(zyb) > 0)
                    {
                        con.Zyb = true;
                    }
                    else
                    {
                        con.Zyb = false;
                    }
                }
                string dz = node.SelectSingleNode("dzsl").InnerText;
                if (string.IsNullOrEmpty(dz))
                {
                    con.Dz = false;
                }
                else
                {
                    if (Convert.ToInt32(dz) > 0)
                    {
                        con.Dz = true;
                    }
                    else
                    {
                        con.Dz = false;
                    }
                }
                string bj = node.SelectSingleNode("bjsl").InnerText;
                if (string.IsNullOrEmpty(bj))
                {
                    con.Bj = false;
                }
                else
                {
                    if (Convert.ToInt32(bj) > 0)
                    {
                        con.Bj = true;
                    }
                    else
                    {
                        con.Bj = false;
                    }
                }
                string njb = node.SelectSingleNode("njbsl").InnerText;
                if (string.IsNullOrEmpty(njb))
                {
                    con.Njb = false;
                }
                else
                {
                    if (Convert.ToInt32(njb) > 0)
                    {
                        con.Njb = true;
                    }
                    else
                    {
                        con.Njb = false;
                    }
                }
                //if (Session["Condition"] != null)
                //{
                //    Session["Condition"] = null;
                //}
                //Session["Condition"] = con;
                return con;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

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