using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class FeatureQuery : System.Web.UI.Page
    {
        #region 成员变量

        private static string starttime = "";
        private static string endtime = "";
        private DataCommon dataCommon = new DataCommon();
        private Bll.PasscarManager bll = new PasscarManager();
        private static DataTable dtStation = null;
        private static OtherQueryService.OtherQueryInfo client = new OtherQueryService.OtherQueryInfo();
        private MapManager mapManager = new MapManager();
        private SettingManager settingManager = new SettingManager();
        private Bll.ServiceManager servicemansger = new Bll.ServiceManager();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private UserLogin userLogin = new UserLogin();

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
            if (!userLogin.CheckLogin(username)) { string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" + StaticInfo.LoginPage + "'"; System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>"); return; }
            this.EnableViewState = false;
            if (!X.IsAjaxRequest)
            {
                StoreDataBind();
                DataSetDateTime();
                BuildTree(TreeStation.Root);

                this.DataBind();
            }
        }

        /// <summary>
        /// 大图查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuery_Event(object sender, DirectEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(starttime) || string.IsNullOrEmpty(endtime))
                {
                    Notice("提示", "未选择时间。");
                    return;
                }
                if (string.IsNullOrEmpty(txtxsd.Text))
                {
                    Notice("提示", "未录入相似度。");
                    return;
                }
                if (string.IsNullOrEmpty(imgShow.ImageUrl))
                {
                    Notice("提示", "请选择要查询的图片。");
                    return;
                }
                List<string> list = new List<string>();
                if (this.FieldStation.Value != null)
                {
                    string kkid = this.FieldStation.Value.ToString();
                    if (!string.IsNullOrEmpty(kkid))
                    {
                        foreach (string item in kkid.Split(','))
                        {
                            list.Add(item);
                        }
                    }
                }
                string xml = GetXml(list, ImgToBase64String(Server.MapPath("~/FileUpload/TEMP/" + DateTime.Now.ToString("yyyyMMdd") + "/" + hidimgpath.Value)), "", txtxsd.Text);
                string xmlret = client.GetFeatureImageSearch(xml);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlret);
                int allNum = Bll.Common.GetRowCount(doc);
                if (allNum > 0)
                {
                    CXmlToDataTable(doc);
                }
                btncutimg.Hidden = false;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 局部查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCutQuery_Event(object sender, DirectEventArgs e)
        {
            try
            {
                List<string> list = new List<string>();
                if (this.FieldStation.Value != null)
                {
                    string kkid = this.FieldStation.Value.ToString();
                    if (!string.IsNullOrEmpty(kkid))
                    {
                        foreach (string item in kkid.Split(','))
                        {
                            list.Add(item);
                        }
                    }
                }
                string zb = T.Value + "," + L.Value + "," + W.Value + "," + H.Value;
                string xml = GetXml(list, ImgToBase64String(Server.MapPath("~/FileUpload/TEMP/" + DateTime.Now.ToString("yyyyMMdd") + "/" + hidimgpath.Value)), zb, txtxsd.Text);
                string xmlret = client.GetFeatureImageSearch(xml);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlret);
                int allNum = Bll.Common.GetRowCount(doc);
                if (allNum > 0)
                {
                    CXmlToDataTable(doc);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLast_Event(object sender, DirectEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Event(object sender, DirectEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        #endregion 控件事件

        #region DirectMethod

        /// <summary>
        /// 文件选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DirectMethod]
        protected void FileUploadSelect(object sender, DirectEventArgs e)
        {
            if (this.ImgFile.HasFile)
            {
                if (this.ImgFile.FileName.Contains(".jpg") || this.ImgFile.FileName.Contains(".jpeg") || this.ImgFile.FileName.Contains(".png") || this.ImgFile.FileName.Contains(".bmp"))
                {
                    string fullimgname = "";
                    string imgUrl = UploadFile(ref fullimgname);
                    imgShow.ImageUrl = imgUrl;
                    System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath("~/FileUpload/TEMP/" + DateTime.Now.ToString("yyyyMMdd") + "/" + fullimgname));
                    hidimgpath.Value = fullimgname;
                    hidimgwidth.Value = img.Width;
                    hidimghight.Value = img.Height;
                }
                else
                {
                    Notice("信息提示", "图片格式不正确");
                    return;
                }
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

        [DirectMethod]
        public void ShowCutPanel()
        {
            panelCut.Hidden = false;
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
                dtStation = mapManager.GetStation();
                this.storekk.DataSource = bll.GetStation();
                this.storekk.DataBind();
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
        /// xml返回解析
        /// </summary>
        /// <param name="xmlStr"></param>
        public void CXmlToDataTable(XmlDocument xmlDoc)
        {
            XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/passcarinfolist/passcarinfo ");
            try
            {
                int i = 0;
                if (listNodes[0] != null)
                {
                    i = 0;
                    imgzjwj1.ImageUrl = listNodes[i]["zjwj1"].InnerText;
                    labkkid1.Text = "卡口名称：" + GetKkmc(listNodes[i]["kkid"].InnerText);
                    labgwsj1.Text = "通过时间：" + listNodes[i]["gwsj"].InnerText;
                    labhphm1.Text = "号牌号码：" + listNodes[i]["hphm"].InnerText;
                }
                if (listNodes[1] != null)
                {
                    i = 1;
                    imgzjwj2.ImageUrl = listNodes[i]["zjwj1"].InnerText;
                    labkkid2.Text = "卡口名称：" + GetKkmc(listNodes[i]["kkid"].InnerText);
                    labgwsj2.Text = "通过时间：" + listNodes[i]["gwsj"].InnerText;
                    labhphm2.Text = "号牌号码：" + listNodes[i]["hphm"].InnerText;
                }
                else
                {
                    imgzjwj2.ImageUrl = "../images/NoImage.png";
                    labkkid2.Text = "卡口名称：";
                    labgwsj2.Text = "通过时间：";
                    labhphm2.Text = "号牌号码：";
                }
                if (listNodes[2] != null)
                {
                    i = 2;
                    imgzjwj3.ImageUrl = listNodes[i]["zjwj1"].InnerText;
                    labkkid3.Text = "卡口名称：" + GetKkmc(listNodes[i]["kkid"].InnerText);
                    labgwsj3.Text = "通过时间：" + listNodes[i]["gwsj"].InnerText;
                    labhphm3.Text = "号牌号码：" + listNodes[i]["hphm"].InnerText;
                }
                else
                {
                    imgzjwj3.ImageUrl = "../images/NoImage.png";
                    labkkid3.Text = "卡口名称：";
                    labgwsj3.Text = "通过时间：";
                    labhphm3.Text = "号牌号码：";
                }
                if (listNodes[3] != null)
                {
                    i = 3;
                    imgzjwj4.ImageUrl = listNodes[i]["zjwj1"].InnerText;
                    labkkid4.Text = "卡口名称：" + GetKkmc(listNodes[i]["kkid"].InnerText);
                    labgwsj4.Text = "通过时间：" + listNodes[i]["gwsj"].InnerText;
                    labhphm4.Text = "号牌号码：" + listNodes[i]["hphm"].InnerText;
                }
                else
                {
                    imgzjwj4.ImageUrl = "../images/NoImage.png";
                    labkkid4.Text = "卡口名称：";
                    labgwsj4.Text = "通过时间：";
                    labhphm4.Text = "号牌号码：";
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 获得卡口名称
        /// </summary>
        /// <param name="kkid"></param>
        /// <returns></returns>
        private string GetKkmc(string kkid)
        {
            string kkmc = kkid;
            try
            {
                DataRow[] listdr = dtStation.Select("STATION_ID= '" + kkid + "'");
                if (listdr.Length > 0)
                {
                    kkmc = listdr[0]["STATION_NAME"].ToString();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
            return kkmc;
        }

        /// <summary>
        /// 图片 转为    base64编码的文本
        /// </summary>
        /// <param name="Imagefilename"></param>
        /// <returns></returns>
        private string ImgToBase64String(string Imagefilename)
        {
            try
            {
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Imagefilename);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);
                return strbaser64;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return "";
            }
        }

        private static int curpage = 0;

        /// <summary>
        /// 组织查询条件xml
        /// </summary>
        /// <param name="kkidlist"></param>
        /// <param name="mbtp"></param>
        /// <param name="tzzb"></param>
        /// <param name="xsd"></param>
        /// <returns></returns>
        private string GetXml(List<string> kkidlist, string mbtp, string tzzb, string xsd)
        {
            string head = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Message><Version>1.0</Version><Type>PUSH</Type><Body><Cmd>";
            string end = "</Cmd></Body></Message>";
            string con = "<kssj>" + starttime + "</kssj>";
            con += "<jssj>" + endtime + "</jssj>";
            if (kkidlist != null && kkidlist.Count > 0)
            {
                con += "<kkidlist>";

                foreach (string kkid in kkidlist)
                {
                    con += "<kkid>" + kkid + "</kkid>";
                }
                con += "</kkidlist>";
            }
            con += "<mbtp leng=''>" + mbtp + "</mbtp>";
            con += "<tzzb>" + tzzb + "</tzzb>";
            con += "<xsd>" + xsd + "</xsd>";
            con += "<begnum>" + (curpage * 4).ToString() + "</begnum>";
            con += "<num>4</num>";
            return head + con + end;
        }

        /// <summary>
        /// 上传选中图片
        /// </summary>
        /// <returns></returns>
        private string UploadFile(ref string fullimgname)
        {
            try
            {
                string UploadFile = "";
                string strPath = "";
                if (this.ImgFile.HasFile)
                {
                    UploadFile = this.ImgFile.PostedFile.FileName.ToString();
                    int FileSize = Int32.Parse(this.ImgFile.PostedFile.ContentLength.ToString());

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
                        fullimgname = sNewName;
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

                Ext.Net.TreeNode root = new Ext.Net.TreeNode();
                root.Text = "卡口列表";
                nodes.Add(root);
                root.Draggable = true;
                root.Expandable = ThreeStateBool.True;
                root.Expanded = true;

                // 添加 自己机构节点 和卡口
                UserInfo user = Session["userinfo"] as UserInfo;
                if (user == null)
                {
                    user = new UserInfo();
                    user.DepartName = "滨州市交通警察支队";
                    user.DeptCode = "371600000000";
                }

                Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                nodeRoot.Text = user.DepartName;
                nodeRoot.Leaf = true;
                nodeRoot.NodeID = user.DeptCode;
                nodeRoot.Icon = Icon.House;

                DataTable dtStation = tgsPproperty.GetStationInfo(" departid='" + user.DeptCode + "' ");
                AddStationTree(nodeRoot, dtStation);
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

        /// <summary>
        ///绑定下级部门及下级部门卡口
        /// </summary>
        /// <param name="root"></param>
        private void AddDepartTree(Ext.Net.TreeNode root, string departCode)
        {
            try
            {
                DataTable dtDepart = settingManager.GetLowerDepartment(departCode);

                if (dtDepart != null && dtDepart.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDepart.Rows.Count; i++)
                    {
                        Ext.Net.TreeNode nodeRoot = new Ext.Net.TreeNode();
                        nodeRoot.Text = dtDepart.Rows[i][2].ToString();
                        nodeRoot.Leaf = true;
                        nodeRoot.NodeID = dtDepart.Rows[i][1].ToString();
                        nodeRoot.Icon = Icon.House;

                        DataTable dtStation = tgsPproperty.GetStationInfo(" departid='" + nodeRoot.NodeID + "' ");
                        AddStationTree(nodeRoot, dtStation);
                        nodeRoot.Expanded = false;
                        nodeRoot.Draggable = true;
                        nodeRoot.Expandable = ThreeStateBool.True;
                        AddDepartTree(nodeRoot, dtDepart.Rows[i][1].ToString());
                        root.Nodes.Add(nodeRoot);
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
                        node.Checked = ThreeStateBool.False;
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