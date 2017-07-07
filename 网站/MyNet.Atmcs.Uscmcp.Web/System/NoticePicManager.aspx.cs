using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Data;
using System.IO;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class NoticePicManager : System.Web.UI.Page
    {
        //private MyNet.Yunwei.YunweiService.ServiceYunwei service = new MyNet.Yunwei.YunweiService.ServiceYunwei();

        #region 定义常量

        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();
        private SettingManager settingManager = new SettingManager();
        private UserLogin userLogin = new UserLogin();
        private string PublicPath = string.Empty;
        public static string mode = "0";
        private NoticeManager noticeManager = new NoticeManager();
        private static string ggid = "";

        /// <summary>
        /// 0是添加 1是编辑
        /// </summary>
        private static int num = 0;

        private static string uName;
        private static string nowIp;

        /// <summary>
        /// 公告
        /// </summary>
        private static string gg;

        /// <summary>
        /// 违法行为
        /// </summary>
        private static string wx;

        /// <summary>
        /// 公告简称
        /// </summary>
        private static string gj;

        /// <summary>
        /// 公告描述
        /// </summary>
        private static string ms;

        /// <summary>
        /// 文件路径
        /// </summary>
        private static string lj;

        /// <summary>
        /// 所有数据
        /// </summary>
        private static DataTable dtgg;

        #endregion 定义常量

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                string username = Request.QueryString["username"];
                if (!userLogin.CheckLogin(username))
                {
                    string js = "alert('" + GetLangStr("NoticePicManager34", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                    System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                    return;
                }
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                uName = userinfo.UserName;
                nowIp = userinfo.NowIp;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("NoticePicManager35", "访问：") + Request.QueryString["funcname"], userinfo.NowIp, "0");
                StoreDataBind();
                TbutQueryClick(null, null);
                this.DataBind();
            }
        }

        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                UserManagerNew user = new UserManagerNew();

                StoreLrr.DataSource = user.GetAllUserName();
                StoreLrr.DataBind();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("NoticePicManager.aspx-StoreDataBind", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("PassCarInfoQuery.aspx-MyData_Refresh", ex.Message + "；" + ex.StackTrace, "MyData_Refresh has an exception");
            }
        }

        #region 增删改查 重置  功能

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            TxtPicName.Reset();
            CmbLrr.Reset();
        }

        /// <summary>
        /// 查询功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                string where = " 1=1 ";

                if (TxtPicName.Text != "")
                {
                    where += where = "  and picName like '%" + TxtPicName.Text.ToString() + "%' ";
                }
                if (CmbLrr.SelectedItem.Value != null)
                {
                    where += where = "  and lrr like '%" + CmbLrr.SelectedItem.Value + "%' ";
                }
                DataTable dt = noticeManager.GetNoticePicInfo(where);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    StorePicManager.DataSource = dtgg = Bll.Common.ChangColName(dt);
                    StorePicManager.DataBind();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("NoticePicManager.aspx-TbutQueryClick", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 添加清空
        /// </summary>
        public void DoYes()
        {
            txtNewPicName.Reset();
            txtNewName.Reset();
            txtNewWfxw.Reset();
            txtNewPicDisc.Reset();
            FileUploadField1.Reset();
        }

        /// <summary>
        /// 添加功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddClick(object sender, DirectEventArgs e)
        {
            DoYes();
            num = 0;
            ButUpdate.Hidden = false;
            ButUpdate.Text = GetLangStr("NoticePicManager21", "添加");
            Window1.Show();
            HidType.Value = "1";
        }

        /// <summary>
        /// 删除编辑
        /// </summary>
        [DirectMethod(Namespace = "OnEvl")]
        public void Exque(string command, string id, string picname, string name, string picdisc, string wfxw, string url)
        {
            try
            {
                if (command.Equals("Edit"))
                {
                    try
                    {
                        num = 1;
                        Editor(id, picname, name, picdisc, wfxw, url);
                        DataRow[] dt = dtgg.Select("col0='" + id + "'");
                        gg = dt[0]["col1"].ToString();
                        gj = dt[0]["col2"].ToString();
                        ms = dt[0]["col3"].ToString();
                        wx = dt[0]["col4"].ToString();
                        lj = dt[0]["col7"].ToString();
                    }
                    catch (Exception ex)
                    {
                        ILog.WriteErrorLog(ex);
                        logManager.InsertLogError("NoticePicManager.aspx-Exque_Edit", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
                    }
                }
                else if (command.Equals("Delete"))
                {
                    Delete(id);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("NoticePicManager.aspx-Exque", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        /// <summary>
        /// 图片加载
        /// </summary>
        public void photo()
        {
        }

        /// <summary>
        ///添加功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Add_Fac(object sender, DirectEventArgs e)
        {
            try
            {
                if (num == 0)
                {
                    try
                    {
                        NoticePicInfo info = new NoticePicInfo();
                        info.Id = DateTime.Now.ToString("yyyyMMddHHmmss");
                        info.PicName = txtNewPicName.Text;
                        info.Name = txtNewName.Text;
                        info.Wfxw = txtNewWfxw.Text;
                        info.DicDisc = txtNewPicDisc.Text;
                        if (Session["userinfo"] != null)
                        {
                            info.Lrr = (Session["userinfo"] as UserInfo).UserName;
                        }
                        else
                        {
                            info.Lrr = "admin";
                        }
                        info.Lrsj = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        PublicPath = settingManager.GetSettingPublicPath("00");
                        if (FileUploadField1.HasFile)
                        {
                            string typepic = FileUploadField1.PostedFile.ContentType;//获取图片上传格式
                            int imgsize = FileUploadField1.PostedFile.ContentLength;//获取图片大小
                            if (typepic == "image/bmp" || typepic == "image/gif" || typepic == "image/pjpeg" || typepic == "image/x-png" || typepic == "image/png" || typepic == "image/jpg" || typepic == "image/jpeg")
                            {
                                string savepic = FileUploadField1.PostedFile.FileName.ToString();
                                FileInfo ff = new FileInfo(savepic);
                                string newp = ff.Name;
                                string savetu = Server.MapPath("~/Capture/" + newp);
                                FileUploadField1.PostedFile.SaveAs(savetu);
                                info.PicUrl = PublicPath + "//" + newp + "";
                            }
                        }
                        if (noticeManager.AddNoticePic(info) > 0)
                        {
                            string lblname = "";
                            lblname += Bll.Common.AssembleRunLog("", info.Name, GetLangStr("NoticePicManager10", "公告名称"), "0");
                            lblname += Bll.Common.AssembleRunLog("", info.PicName, GetLangStr("NoticePicManager11", "公告简称"), "0");
                            lblname += Bll.Common.AssembleRunLog("", info.Wfxw, GetLangStr("NoticePicManager13", "违法行为"), "0");
                            lblname += Bll.Common.AssembleRunLog("", info.DicDisc, GetLangStr("NoticePicManager39", "描述"), "0");
                            lblname += Bll.Common.AssembleRunLog("", info.PicUrl, GetLangStr("NoticePicManager18", "文件"), "0");
                            logManager.InsertLogRunning(uName, GetLangStr("NoticePicManager41", "添加:") + lblname, nowIp, "1");
                            Notice(GetLangStr("NoticePicManager22", "信息提示"), GetLangStr("NoticePicManager23", "添加成功"));
                            TbutQueryClick(null, null);
                            DataRow[] dt = dtgg.Select("col0='" + info.Id + "'");
                            gg = dt[0]["col1"].ToString();
                            gj = dt[0]["col2"].ToString();
                            ms = dt[0]["col3"].ToString();
                            wx = dt[0]["col4"].ToString();
                            lj = dt[0]["col7"].ToString();
                            Window1.Hide();
                        }
                        else
                        {
                            Notice(GetLangStr("NoticePicManager22", "信息提示"), GetLangStr("NoticePicManager24", "添加失败"));
                        }
                    }
                    catch (Exception ex)
                    {
                        ILog.WriteErrorLog(ex);
                        logManager.InsertLogError("NoticePicManager.aspx-Add_Fac", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
                    }
                }
                else if (num == 1)//修改
                {
                    try
                    {
                        NoticePicInfo info = new NoticePicInfo();

                        info.Id = ggid;
                        info.PicName = txtNewPicName.Text;
                        info.Name = txtNewName.Text;
                        info.Wfxw = txtNewWfxw.Text;
                        info.DicDisc = txtNewPicDisc.Text;
                        PublicPath = settingManager.GetSettingPublicPath("00");
                        if (FileUploadField1.HasFile)
                        {
                            string typepic = FileUploadField1.PostedFile.ContentType;//获取图片上传格式
                            int imgsize = FileUploadField1.PostedFile.ContentLength;//获取图片大小
                            if (typepic == "image/bmp" || typepic == "image/gif" || typepic == "image/pjpeg" || typepic == "image/x-png" || typepic == "image/png" || typepic == "image/jpg" || typepic == "image/jpeg")
                            {
                                string savepic = FileUploadField1.PostedFile.FileName.ToString();
                                FileInfo ff = new FileInfo(savepic);
                                string newp = ff.Name;

                                string savetu = Server.MapPath("~/Capture/" + newp);
                                FileUploadField1.PostedFile.SaveAs(savetu);
                                info.PicUrl = PublicPath + "//" + newp;
                            }
                        }
                        if (noticeManager.EditNoticePic(info) > 0)
                        {
                            string gg1 = txtNewPicName.Text;
                            string gj1 = txtNewName.Text;
                            string wx1 = txtNewWfxw.Text;
                            string ms1 = txtNewPicDisc.Text;
                            string lj1 = info.PicUrl;
                            string lblname = "";
                            lblname += Bll.Common.AssembleRunLog(gg, gg1, GetLangStr("NoticePicManager10", "公告名称"), "2");
                            lblname += Bll.Common.AssembleRunLog(gj, gj1, GetLangStr("NoticePicManager11", "公告简称"), "2");
                            lblname += Bll.Common.AssembleRunLog(wx, wx1, GetLangStr("NoticePicManager13", "违法行为"), "2");
                            lblname += Bll.Common.AssembleRunLog(ms, ms1, GetLangStr("NoticePicManager39", "描述"), "2");
                            lblname += Bll.Common.AssembleRunLog(lj, lj1, GetLangStr("NoticePicManager18", "文件"), "2");
                            if (!string.IsNullOrEmpty(lblname))
                            {
                                logManager.InsertLogRunning(uName, GetLangStr("NoticePicManager42", "修改:") + lblname, nowIp, "1");
                            }
                            else
                            {
                                Notice(GetLangStr("NoticePicManager22", "信息提示"), GetLangStr("NoticePicManager25", "未作任何修改！"));
                            }
                            TbutQueryClick(null, null);
                            Window1.Hide();
                        }
                        else
                        {
                            Notice(GetLangStr("NoticePicManager22", "信息提示"), GetLangStr("NoticePicManager26", "修改失败"));
                        }
                        Window1.Hide();
                    }
                    catch (Exception ex)
                    {
                        ILog.WriteErrorLog(ex);
                        logManager.InsertLogError("NoticePicManager.aspx-Add_Fac", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("NoticePicManager.aspx-Add_Fac", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        #endregion 增删改查 重置  功能

        public void Notice(string title, string msg)
        {
            Notification.Show(new NotificationConfig
            {
                Title = title,
                Icon = Icon.Information,
                HideDelay = 2000,
                Html = "<br></br>" + msg + "!"
            });
        }

        [DirectMethod(Namespace = "OnEvl")]
        public void DoNo()
        {
            Window1.Hide();
        }

        /// <summary>
        /// 删除方法
        /// </summary>
        /// <param name="id"></param>
        [DirectMethod(Namespace = "OnEvl")]
        public void DelYes(string id)
        {
            try
            {
                NoticePicInfo info = new NoticePicInfo();
                string where = " 1=1 ";
                where = "  id ='" + id + "'";
                if (noticeManager.DeleteNoticePic(where) > 0)
                {
                    DataRow[] dt = dtgg.Select("col0='" + id + "'");
                    string ggmc = dt[0]["col1"].ToString();
                    logManager.InsertLogRunning(uName, GetLangStr("NoticePicManager43", "删除:[") + ggmc + "]", nowIp, "1");
                    Notice(GetLangStr("NoticePicManager22", "信息提示"), GetLangStr("NoticePicManager27", "删除成功"));
                    TbutQueryClick(null, null);
                }
                else
                {
                    Notice(GetLangStr("NoticePicManager22", "信息提示"), GetLangStr("NoticePicManager28", "删除失败"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("NoticePicManager.aspx-DelYes", ex.Message + "；" + ex.StackTrace, "StoreDataBind has an exception");
            }
        }

        [DirectMethod(Namespace = "OnEvl")]
        public void Delete(string id)
        {
            X.Msg.Confirm(GetLangStr("NoticePicManager29", "提示"), GetLangStr("NoticePicManager30", "是否删除此条公告信息"), new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = "OnEvl.DelYes('" + id + "')",
                    Text = GetLangStr("NoticePicManager31", "是")
                },
                No = new MessageBoxButtonConfig
                {
                    Text = GetLangStr("NoticePicManager32", "否")
                }
            }).Show();
        }

        [DirectMethod(Namespace = "OnEvl")]
        public void Detailed(string id)
        {
            ButUpdate.Hidden = true;

            Window1.Show();
        }

        #region 多语言转换

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

        #endregion 多语言转换

        /// <summary>
        /// 编辑事件
        /// </summary>
        /// <param name="id"></param>
        public void Editor(string id, string picname, string name, string picdisc, string wfxw, string url)
        {
            ggid = id;
            ButUpdate.Text = GetLangStr("NoticePicManager33", "修改");
            txtNewPicName.Text = picname;
            txtNewName.Text = name;
            txtNewWfxw.Text = wfxw;
            txtNewPicDisc.Text = picdisc;
            FileUploadField1.Text = url;
            Window1.Show();
        }
    }
}