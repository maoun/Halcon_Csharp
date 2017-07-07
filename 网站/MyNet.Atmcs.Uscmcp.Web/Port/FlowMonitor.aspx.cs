using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class FlowMonitor : System.Web.UI.Page
    {
        #region 成员变量
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager(); 
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();
        private DataCommon dataCommon = new DataCommon();

        private SettingManager settingManager = new SettingManager();
        public static string stub = "";//弹出报警消息内容
        public static string _CurValue = string.Empty;
        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"];
            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('您没有登录或操作超时，请重新登陆!');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                //userLogin.IsLoginPage(this);
                BuildTree(TreePanel1.Root);//, "a.station_type_id in (01,02,03,06,07,08)");
                BuildTreeAlarm(TreePanel2.Root);
                StoreDataBind();
                Session["MonitorId"] = "";
                this.DataBind();

                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                logManager.InsertLogRunning(userinfo.UserName, "访问：报警监控", userinfo.NowIp, "0");

            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RefreshTime(object sender, DirectEventArgs e)
        {
            try
            {
                string where = string.Empty;
                
                //DataTable dt = GetDataTable("1");
                DataTable dt = GetDataTable("10");
                //当有未处理的报警消息时或没有未处理的报警消息但界面上显示了未处理的报警消息需要刷新列表绑定数据
                if (dt.Rows.Count > 0)
                {
                    string BH = dt.Rows[0][0].ToString();
                    if (BH != FlowXh.Value.ToString())
                    {                       
                        string location_name = dt.Rows[0][1].ToString();                      
                        string KSSD = dt.Rows[0][2].ToString();
                        string JSSD = dt.Rows[0][3].ToString();
                        string bjsj = dt.Rows[0][4].ToString();
                        string tjzq = dt.Rows[0][5].ToString();
                        string bjfz = dt.Rows[0][6].ToString();
                        string bl = dt.Rows[0][7].ToString();
                        string ll = dt.Rows[0][8].ToString();
                        string name = dt.Rows[0][9].ToString();
                        string cljg = dt.Rows[0][10].ToString();
                        string gxsj = dt.Rows[0][11].ToString();
                        string msg = GetHtml(location_name, bjsj, tjzq, bjfz, bl, ll, name, cljg, gxsj);
                        
                        StoreFlow.DataSource = dt;// GetDataTable("10");
                        StoreFlow.DataBind();
                        FlowXh.Value = BH;
                        //string url = "../Sound/hmdalarm.WAV";
                        //switch (bjlx)
                        //{
                        //    case "5":
                        //        url = "../Sound/speedalarm.WAV";
                        //        break;

                        //    case "1":
                        //    case "3":
                        //        url = "../Sound/hmdalarm.WAV";
                        //        break;

                        //    default:
                        //        url = "../Sound/otheralarm.WAV";
                        //        break;
                        //}

                        //string js = "soundPlay(\"" + url + "\");";
                        //this.ResourceManager1.RegisterAfterClientInitScript(js);
                        //surl1 = dataCommon.ChangePoliceIp(surl1);
                        //surl2 = dataCommon.ChangePoliceIp(surl2);
                        //ApplyImage(surl1, surl2);
                        ApplyText(msg);
                        string sNum = "共" + dt.Rows.Count.ToString()+ "条。";
                        string sShot = "报警卡口：" + location_name + "\r\n\r\n\r\n报警时间：" + bjsj;
                        FlowInforNotice(sNum, sShot,false);
                        RowSelectionModel1.SelectFirstRow();
                    }
                }
                else if (((dt.Rows.Count == 0) && (FlowXh.Value.ToString() != string.Empty)))
                {
                    StoreFlow.DataSource = dt;
                    StoreFlow.DataBind();
                    FlowXh.Value = string.Empty;
                    //ApplyImage(string.Empty, string.Empty);
                    ApplyText(string.Empty);
                    string sNum = "共0条。";
                    string sShot = "10分钟内没有新的报警消息。";
                    FlowInforNotice(sNum, sShot, true);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowMonitor.aspx-RefreshTime", ex.Message, "RefreshTime has an exception");
            }
        }

        /// <summary>
        /// 显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ShowDetails(object sender, DirectEventArgs e)
        {
            try
            {
                string data = e.ExtraParams["data"];
                AddWindow(data);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowMonitor.aspx-ShowDetails", ex.Message, "ShowDetails has an exception");
            }
        }
        
        /// <summary>
        /// 提示框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">内容</param>
        private void FlowInforNotice(string title, string msg, bool bAutoHide)
        {
            WindowListeners listeners = new WindowListeners();
            listeners.BeforeShow.Handler = string.Concat(BarLabel.ClientID, ".setText('" + title + "');");
            stub = msg;
            Notification.Show(new NotificationConfig
            {
                Title = "报警消息",
                Icon = Icon.Information,
                Height = 170,
                AutoHide = bAutoHide,
                CloseVisible = true,
                ContentEl = "customEl",
                Listeners = listeners
            });
        }
        /// <summary>
        /// 点击弹出报警消息界面处理按钮响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnDeal(object sender, EventArgs e)
        {
            try
            {
                AddWindow(_CurValue);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowMonitor.aspx-OnDeal", ex.Message, "OnDeal has an exception");
            }
        }

        #endregion 控件事件

        #region 私有方法
        /// <summary>
        /// 初始化绑定数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                //报警消息处理结果
                DataTable dt3 = GetRedisData.GetData("t_sys_code:430800");
                if (dt3 != null)
                {
                    this.Storecljg.DataSource = Bll.Common.ChangColName(dt3);
                    this.Storecljg.DataBind();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowInfoQuery.aspx-StoreDataBind", ex.Message, "StoreDataBind has an exception");
            }
        }
        /// <summary>
        /// 查询报警数据
        /// </summary>
        /// <param name="rownum"></param>
        /// <returns></returns>
        protected DataTable GetDataTable(string rownum)
        {
            try
            {
                string where = " 1=1  ";
                if (!string.IsNullOrEmpty(this.RevStation.Value.ToString()))
                {
                    where = "   concat(location_name,KKBH)  in (" + this.RevStation.Value.ToString() + ")   and " + where;
                }
                //if (!string.IsNullOrEmpty(this.FlowType.Value.ToString()))
                //{
                //    where = "   bjlx  in (" + this.FlowType.Value.ToString() + ")   and " + where;
                //}
                //string maxBjsj = tgsDataInfo.GetAlarmTempMaxBjsj(where).Rows[0][0].ToString();

                //if (!string.IsNullOrEmpty(maxBjsj))
                //{
                //    where = "  bjsj >= STR_TO_DATE('" + DateTime.Parse(maxBjsj).AddMinutes(-10).ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s')   and bjsj<=STR_TO_DATE('" + DateTime.Parse(maxBjsj).AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s') and " + where;
                //}
                //else
                {
                    where = "  bjsj >= STR_TO_DATE('" + DateTime.Now.AddMinutes(-10).ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s')   and bjsj<=STR_TO_DATE('" + DateTime.Now.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s') and  cljg = 0 and" + where;
                }

                return tgsDataInfo.GetFlowMonitor(where + " order by bjsj desc", rownum);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowMonitor.aspx-GetDataTable", ex.Message, "GetDataTable has an exception");
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="surl1"></param>
        /// <param name="surl2"></param>
//        protected void ApplyImage(string surl1, string surl2)
//        {
//            try
//            {
//                var tpl = new XTemplate { ID = "Template1" };
//                if (surl1 != string.Empty || surl2 != string.Empty)
//                {
//                    tpl.Html = @"<div class=""details"">
//			        <tpl for=""."">
//				       <center>
//                        <img src=""{url1}""  onload=""resizeimg(this);""  alt=""车辆图片(双击图片进行放大)"" onDblClick=""OpenPicPage(this.src)""; />&nbsp;&nbsp;
//                        <img src=""{url2}""  onload=""resizeimg(this);"" alt=""车辆图片(双击图片进行放大)"" onDblClick=""OpenPicPage(this.src)"";/>
//                        </center>
//			        </tpl>
//		            </div>";
//                    tpl.Overwrite(this.ImagePanel, new
//                    {
//                        url1 = surl1,
//                        url2 = surl2
//                    });
//                }
//                else
//                {
//                    tpl.Html = @"<div class=""details"">
//			        <tpl for=""."">
//				       <center>
//                        <img src=""{url1}""  onload=""resizeimg(this);""  />&nbsp;&nbsp;
//                        <img src=""{url2}""  onload=""resizeimg(this);"" />
//                        </center>
//			        </tpl>
//		            </div>";
//                    tpl.Overwrite(this.ImagePanel, new
//                    {
//                        url1 = surl1,
//                        url2 = surl2
//                    });
//                }
//                tpl.Render();
//            }
//            catch (Exception ex)
//            {
//                ILog.WriteErrorLog(ex);
//                logManager.InsertLogError("FlowMonitor.aspx-ApplyImage", ex.Message, "ApplyImage has an exception");
//            }
//        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sdata"></param>
        private void AddWindow(string sdata)
        {
            try
            {
                Window win = new Window();
                win.ID = "Window1";
                win.Title = "报警详细信息";
                win.Icon = Icon.Application;
                win.Width = Unit.Pixel(800);
                win.Height = Unit.Pixel(490);
                win.Plain = true;
                win.Border = false;
                win.BodyBorder = false;
                win.Collapsible = true;

                TabPanel center = new TabPanel();
                center.ID = "CenterPanel";
                center.ActiveTabIndex = 0;

                Ext.Net.Panel tab2 = new Ext.Net.Panel();
                tab2.ID = "Tab2";
                tab2.Title = "报警信息";
                tab2.Border = false;
                tab2.BodyStyle = "padding:6px;";

                Container container = new Container();
                container.Layout = "Column";
                container.Height = 130;

                Container container1 = new Container();
                container1.LabelAlign = LabelAlign.Left;
                container1.Layout = "Form";
                container1.ColumnWidth = 0.35;
                container1.Items.Add(CommonExt.AddTextField("txtcxkk", "报警卡口", Bll.Common.GetdatabyField(sdata, "col1")));
                container1.Items.Add(CommonExt.AddTextField("txtbjyz", "报警阈值(%)", Bll.Common.GetdatabyField(sdata, "col6")));  
                container1.Items.Add(CommonExt.AddTextField("txtpzsj", "配置时间", Bll.Common.GetdatabyField(sdata, "col11", "")));

                Container container2 = new Container();
                container2.LabelAlign = LabelAlign.Left;
                container2.Layout = "Form";
                container2.ColumnWidth = 0.3;
                container2.Items.Add(CommonExt.AddTextField("txttjzq", "统计周期", Bll.Common.GetdatabyField(sdata, "col5")));
                container2.Items.Add(CommonExt.AddTextField("txtkkpzr", "卡口配置人", Bll.Common.GetdatabyField(sdata, "col9")));                
                container2.Items.Add(CommonExt.AddComboBox("cmbcljg", "处理结果", "Storecljg", "请选择...", false, 224, 105, Bll.Common.GetdatabyField(sdata, "col10")));

                Container container3 = new Container();
                container3.LabelAlign = LabelAlign.Left;
                container3.Layout = "Form";
                container3.ColumnWidth = 0.4;
                container3.Items.Add(CommonExt.AddTextField("txtbjsj", "报警时间", Bll.Common.GetdatabyField(sdata, "col4", "")));
                container3.Items.Add(CommonExt.AddTextField("txtkkfx", "卡口方向", Bll.Common.GetdatabyField(sdata, "col14")));
                container3.Items.Add(CommonExt.AddButton("btnSavecljg", "保存", "Disk", "Ovel.Savecljg(" + Bll.Common.GetdatabyField(sdata, "col0") + ")"));
                //container3.Items.Add(CommonExt.AddButton("butCancel", "退出", "Cancel", win.ClientID + ".hide()"));

                container.Items.Add(container1);
                container.Items.Add(container2);
                container.Items.Add(container3);

                tab2.Items.Add(container);
                center.Items.Add(tab2);

                //Ext.Net.Panel south = new Ext.Net.Panel();
                //south.ID = "SouthPanel";
                //south.Title = "图片信息";
                //south.Height = Unit.Pixel(380);
                //south.BodyStyle = "padding:6px;";
                
                //string img1 = Bll.Common.GetdatabyField(sdata, "col14");
                //string img2 = Bll.Common.GetdatabyField(sdata, "col15");
                //south.Html = GetHtml(img1, img2);

                BorderLayout layout = new BorderLayout();
                layout.South.Split = true;
                layout.South.Collapsible = true;
                layout.Center.Items.Add(center);
                //layout.South.Items.Add(south);

                win.Items.Add(layout);

                win.Render(this.Form);
                win.Show();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowMonitor.aspx-AddWindow", ex.Message, "AddWindow has an exception");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
//        protected string GetHtml(string image1, string image2)
//        {
//            try
//            {
//                string Html = @"<div class=""details"">
//			        <tpl for=""."">
//				       <center>
//                        <img src=""{0}""  width=""380"" onDblClick=""OpenPicPage(this.src)""; />
//                        <img src=""{1}"" width=""380"" onDblClick=""OpenPicPage(this.src)""; /></center>
//			        </tpl>
//		            </div>";
//                return string.Format(Html, image1, image2);
//            }
//            catch (Exception ex)
//            {
//                ILog.WriteErrorLog(ex);
//                logManager.InsertLogError("FlowMonitor.aspx-GetHtml", ex.Message, "GetHtml has an exception");
//                return "";
//            }
//        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="hpzl"></param>
        /// <param name="hphm"></param>
        /// <param name="hpzlms"></param>
        /// <param name="gwsj"></param>
        /// <param name="xlsd"></param>
        /// <param name="jllx"></param>
        /// <param name="bjyy"></param>
        /// <returns></returns>
        private string GetHtml(string location_name, string bjsj, string tjzq, string bjfz, string bl, string ll, string name,string cljg,string gxsj)
        {
            try
            {
                string html = "";
                //switch (bjsj)
                //{
                //    case "01":
                //        html = " <font size =\"4\" color=\"#000000\"><b><span style=\"background-color: #FFFF00\">" + hphm + "</span></b></font>";
                //        break;

                //    case "02":
                //        html = " <font size =\"4\" color=\"#FFFFFF\"><b><span style=\"background-color: #000080\">" + hphm + "</span></b></font>";
                //        break;

                //    case "23":
                //        html = " <font size =\"4\" color=\"#FF0000\"><b><span style=\"background-color: #FFFFFF\">" + hphm + "</span></b></font>";
                //        break;

                //    case "06":
                //        html = " <font size =\"4\" color=\"#FFFFFF\"><b><span style=\"background-color: #000000\">" + hphm + "</span></b></font>";
                //        break;

                //    default:
                html = " <font size =\"4\" color=\"#FFFFFF\"><b><span style=\"background-color: #000080\">" + location_name + "</span></b></font>";
                //        break;
                //}
                html = html + " <font size =\"3\" color=\"#000080\"><b>报警时间：" + bjsj + "|统计周期：" + tjzq + "|配置时间：" + gxsj + "</b></font><br>";
                if (!string.IsNullOrEmpty(bjfz))
                {
                    html = html + " <font size =\"3\" color=\"#000080\"><b>报警阈值(%)：" + bjfz + "</b></font>";
                }
                return html;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowMonitor.aspx-GetHtml", ex.Message, "GetHtml has an exception");
                return "";
            }
        }

        /// <summary>
        /// 将报警信息显示至textpanel
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
                logManager.InsertLogError("FlowMonitor.aspx-ApplyText", ex.Message, "ApplyText has an exception");
            }
        }

        [DirectMethod(Namespace = "Ovel")]
        public void Savecljg(string BH)
        {
            try
            {
                string cljg = X.GetCmp<ComboBox>("cmbcljg").Text;
                string cljgms = X.GetCmp<ComboBox>("cmbcljg").SelectedItem.Text;
                if (string.IsNullOrEmpty(cljg) || (cljgms == "未处理"))
                {
                    Notice("提示", "请选择处理结果");
                }
                else
                {
                    Hashtable hs = new Hashtable();
                    UserInfo userinfo = Session["userinfo"] as UserInfo;
                    hs.Add("BH", BH);
                    //hs.Add("clry", userinfo.UserCode);
                    //hs.Add("clyj", "");
                    hs.Add("cljg", cljg);

                    if (tgsDataInfo.DealFlowInfo(hs) > 0)
                    {
                        //StoreAlarm.RemoveRecord();
                        Notice("信息提示", "处理完成");
                        X.GetCmp<TextField>("txtclzt").Text = cljgms;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Notice(string title, string msg)
        {
            try
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
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowInfoQuery.aspx-Notice", ex.Message, "Notice has an exception");
            }
        }

        /// <summary>
        /// 显示选中数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectFlow(object sender, DirectEventArgs e)
        {
            try
            {
                object data = e.ExtraParams["sdata"];
                string sdata = data.ToString();
                _CurValue = sdata;
                string BH = GetdatabyField(sdata, "col0");
                string location_name = GetdatabyField(sdata, "col1");
                string KSSD = GetdatabyField(sdata, "col2");
                string JSSD = GetdatabyField(sdata, "col3");
                string bjsj = GetdatabyField(sdata, "col4");
                string tjzq = GetdatabyField(sdata, "col5");
                string bjfz = GetdatabyField(sdata, "col6");
                string bl = GetdatabyField(sdata, "col7");
                string ll = GetdatabyField(sdata, "col8");
                string name = GetdatabyField(sdata, "col9");
                string cljg = GetdatabyField(sdata, "col10");
                string gxsj= GetdatabyField(sdata, "col11");
                string msg = GetHtml(location_name, bjsj, tjzq, bjfz, bl, ll, name, cljg, gxsj);   
                //ApplyImage(surl1, surl2);
                ApplyText(msg);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowMonitor.aspx-SelectFlow", ex.Message, "SelectFlow has an exception");
            }
        }

        /// <summary>
        /// 格式化时间
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
                logManager.InsertLogError("FlowMonitor.aspx-GetDate", ex.Message, "GetDate has an exception");
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
            try
            {
                string s = GetdatabyField(data, field);
                return GetDate(s, flag);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowMonitor.aspx-GetDate", ex.Message, "GetDate has an exception");
                return "";
            }
        }

        /// <summary>
        /// 读取Grid选中行数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private string GetdatabyField(string data, string field)
        {
            try
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
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowMonitor.aspx-GetdatabyField", ex.Message, "GetdatabyField has an exception");
                return "";
            }
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
                logManager.InsertLogError("FlowMonitor.aspx-BuildTree", ex.Message, "BuildTree has an exception");
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
                logManager.InsertLogError("FlowMonitor.aspx-AddDepartTree", ex.Message, "AddDepartTree has an exception");
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
                logManager.InsertLogError("FlowMonitor.aspx-AddStationTree", ex.Message, "AddStationTree has an exception");
            }
        }

        /// <summary>
        /// 组件监测点列表树
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private Ext.Net.TreeNodeCollection BuildTree(Ext.Net.TreeNodeCollection nodes, string where)
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
                root.Expanded = true;
                DataTable dttype = tgsPproperty.GetStationTypeInfo();
                if (dttype != null && dttype.Rows.Count > 0)
                {
                    for (int row = 0; row < dttype.Rows.Count; row++)
                    {
                        Ext.Net.TreeNode node1 = new Ext.Net.TreeNode();
                        node1.Text = dttype.Rows[row][1].ToString();
                        node1.NodeID = dttype.Rows[row][0].ToString();
                        DataTable dt = tgsPproperty.GetStationInfo(" a.station_type_id='" + dttype.Rows[row][0].ToString() + "'");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataTable dtDirection = tgsPproperty.GetDirectionInfo();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                                node.Text = dt.Rows[i][2].ToString();
                                node.Icon = Icon.Camera;
                                node.NodeID = dt.Rows[i][1].ToString();
                                DataRow[] drs = dtDirection.Select("col0='" + dt.Rows[i][1].ToString() + "'");
                                Addree(node, drs);
                                node.Expanded = true;
                                node1.Nodes.Add(node);
                            }
                        }
                        root.Nodes.Add(node1);
                    }
                }
                //DataTable dt = tgsPproperty.GetStationInfo(where);
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    DataTable dtDirection = tgsPproperty.GetDirectionInfo();
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                //        node.Text = dt.Rows[i][2].ToString();
                //        node.Icon = Icon.Camera;
                //        node.NodeID = dt.Rows[i][1].ToString();
                //        DataRow[] drs = dtDirection.Select("col0='" + dt.Rows[i][1].ToString() + "'");
                //        Addree(node, drs);
                //        node.Expanded = true;
                //        root.Nodes.Add(node);
                //    }
                //}
                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowMonitor.aspx-BuildTree", ex.Message, "BuildTree has an exception");
                return null;
            }
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="root"></param>
        private void Addree(Ext.Net.TreeNode root, DataRow[] drs)
        {
            try
            {
                if (drs != null && drs.Length > 0)
                {
                    for (int i = 0; i < drs.Length; i++)
                    {
                        Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                        node.Text = root.Text + drs[i]["col2"].ToString();
                        node.Leaf = true;
                        node.Checked = ThreeStateBool.False;
                        node.NodeID = root.NodeID + drs[i]["col1"].ToString();
                        node.Icon = Icon.ArrowNsew;
                        node.Expanded = true;
                        root.Nodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowMonitor.aspx-Addree", ex.Message, "Addree has an exception");
            }
        }

        /// <summary>
        /// 组件 报警类型树
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private Ext.Net.TreeNodeCollection BuildTreeAlarm(Ext.Net.TreeNodeCollection nodes)
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
                DataTable dt = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:420700"));// tgsPproperty.GetAlarmTypeDict();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Ext.Net.TreeNode node = new Ext.Net.TreeNode();
                    node.Text = dt.Rows[i][1].ToString();
                    node.Icon = Icon.Note;
                    node.NodeID = dt.Rows[i][0].ToString();
                    node.Checked = ThreeStateBool.False;
                    node.Expanded = true;
                    root.Nodes.Add(node);
                }

                return nodes;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                logManager.InsertLogError("FlowMonitor.aspx-BuildTreeAlarm", ex.Message, "BuildTreeAlarm has an exception");
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

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [DirectMethod]
        public string RefreshMenu()
        {
            //if (!string.IsNullOrEmpty(cmbSearch.Text))
            //{
            //    Ext.Net.TreeNodeCollection nodes = BuildTree(TreePanel1.Root, "a.station_id = '" + cmbSearch.Text + "'");
            //    return nodes.ToJson();
            //}
            return "";
        }
    }
}