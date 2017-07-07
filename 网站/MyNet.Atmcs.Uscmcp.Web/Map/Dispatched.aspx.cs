using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.Script.Serialization;

namespace MyNet.Atmcs.Uscmcp.Web.Map
{
    public partial class Dispatched : System.Web.UI.Page
    {
        #region 变量

        private MyNet.Atmcs.Uscmcp.Bll.MapManager bll = new Bll.MapManager();
        private static string start = "", end = "";
        private static QueryService.querypasscar client = new QueryService.querypasscar();
        private UserLogin userLogin = new UserLogin();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        #endregion 变量

        #region 事件

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
                try
                {
                    this.cllx.DataSource = GetRedisData.GetData("t_sys_code:140001"); //bll.GetCllx();
                    this.cllx.DataBind();
                    DataTable dt = GetRedisData.GetData("t_sys_code:420700");
                    if (dt != null)
                    {
                        //比对类型
                        this.StoreMdlx.DataSource = dt;
                        this.StoreMdlx.DataBind();
                    }
                    else
                    {
                        this.StoreMdlx.DataSource = tgsPproperty.GetSuspicionDict();
                        this.StoreMdlx.DataBind();
                    }
                    this.DataBind();
                    UserInfo userinfo = Session["Userinfo"] as UserInfo;
                    logManager.InsertLogRunning(userinfo.UserName, "访问：" + Request.QueryString["funcname"], userinfo.NowIp, "0");
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex.Message);
                    logManager.InsertLogError("Dispatched.aspx-Page_Load", ex.Message + "；" + ex.StackTrace, "Page_Load has an exception");
                }
            }
        }

        /// <summary>
        /// 地图标注触发点
        /// </summary>
        /// <param name="points">地图坐标</param>
        [DirectMethod]
        public void AddPosPoints(string points)
        {
            QueryMarkArea(points);
        }

        /// <summary>
        /// 获取起止时间
        /// </summary>
        /// <param name="isstart">时间类型（true开始时间false结束时间）</param>
        /// <param name="strtime">时间</param>
        [DirectMethod]
        public void GetDateTime(bool isstart, string strtime)
        {
            if (isstart)
                start = strtime;
            else
                end = strtime;
        }

        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            try
            {
                ButReset.Hidden = true;
                ButAddgrid.Hidden = false;
                StoreInfo.RemoveAll();
                StoreInfo.DataBind();
                cmbMdlx.Clear();
                cbocllx.Clear();
                txtbdyy.Text = "";
                start = "";
                txtplate.Text = "";
                txtbkr.Text = "";//布控人员
                txtlxdh.Text = "";//布控联系电话
                ChkLike.Checked = false;
                cboplate.SetVehicleText("");
                this.ResourceManager1.RegisterAfterClientInitScript("clearTime('');");
                RowSelectionModel sm = this.GridStation.SelectionModel.Primary as RowSelectionModel;
                sm.SelectedRows.Clear();
                sm.UpdateSelection();
                this.ResourceManager1.RegisterAfterClientInitScript(" BMAP.ClearCircle();BMAP.Clear();");
            }
            catch
            {
            }
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButQueryClick(object sender, DirectEventArgs e)
        {
            try
            {
                string cpmh = string.Empty;
                string hphm = string.Empty;
                if (ChkLike.Checked)//判断是否模糊布控
                {
                    if (string.IsNullOrEmpty(cboplate.VehicleText.ToString().Trim()) && string.IsNullOrEmpty(haopai_name1.Value) && string.IsNullOrEmpty(haopai_name2.Value)
                        && string.IsNullOrEmpty(haopai_name3.Value) && string.IsNullOrEmpty(haopai_name4.Value) && string.IsNullOrEmpty(haopai_name5.Value)
                        && string.IsNullOrEmpty(haopai_name6.Value))
                    {
                        Notice("布控失败", "模糊布控需要至少输入一位号牌号码！");
                        return;
                    }
                    else
                    {
                        cpmh = "1";
                        hphm = (string.IsNullOrEmpty(cboplate.VehicleText.ToString()) ? "*" : cboplate.VehicleText.ToString())
                            + (string.IsNullOrEmpty(haopai_name1.Value) ? "*" : haopai_name1.Value)
                            + (string.IsNullOrEmpty(haopai_name2.Value) ? "*" : haopai_name2.Value)
                            + (string.IsNullOrEmpty(haopai_name3.Value) ? "*" : haopai_name3.Value)
                            + (string.IsNullOrEmpty(haopai_name4.Value) ? "*" : haopai_name4.Value)
                            + (string.IsNullOrEmpty(haopai_name5.Value) ? "*" : haopai_name5.Value)
                            + (string.IsNullOrEmpty(haopai_name6.Value) ? "*" : haopai_name6.Value);
                    }
                }
                else
                {
                    if ((txtplate.Text.Trim() == "") || (cboplate.VehicleText.ToString() == ""))
                    {
                        Notice("布控失败", "请录入布控号牌号码！");
                        return;
                    }
                    else
                    {
                        hphm = cboplate.VehicleText.ToString() + txtplate.Text;
                        cpmh = "0";
                    }
                }

                this.ResourceManager1.RegisterAfterClientInitScript("getTime();");

                string cllx = cbocllx.Value.ToString();
                string bdyy = txtbdyy.Text;
                string yxsj = start;
                string bkry = txtbkr.Text;//布控人员
                string bkdh = txtlxdh.Text;//布控联系电话

                string mdlx;
                if (cmbMdlx.Value != null)
                {
                    mdlx = cmbMdlx.Value.ToString();
                }
                else
                {
                    mdlx = "300108";
                }

                UserInfo userinfo = Session["userinfo"] as UserInfo;
                if (bdyy.Trim() == "")
                {
                    Notice("布控失败", "请录入布控比对原因！");
                    return;
                }
                if (yxsj.Trim() == "")
                {
                    Notice("布控失败", "请录入布控有效时间！");
                    return;
                }
                if (bkry.Trim() == "")
                {
                    Notice("布控失败", "请录入布控联系人！");
                    return;
                }
                if (bkdh.Trim() == "")
                {
                    Notice("布控失败", "请录入联系电话！");
                    return;
                }

                RowSelectionModel sm = this.GridStation.SelectionModel.Primary as RowSelectionModel;

                if (sm.SelectedRows.Count <= 0)
                {
                    Notice("布控失败", "请选择布控卡口！");
                    return;
                }
                List<string> listid = new List<string>();
                foreach (SelectedRow row in sm.SelectedRows)
                {
                    listid.Add(row.RecordID);
                }

                string value = System.Configuration.ConfigurationManager.AppSettings["BkType"].ToString();//得到配置文件当中的布控类型
                string xh;
                if (value.Equals("0"))//插入到老表当中
                {
                    xh = bll.SetDispatch(listid, hphm, cllx, bdyy, yxsj, userinfo.DeptCode, mdlx, bkry, bkdh, cpmh, "1");//最后一个参数1是布控类型，1代表车辆布控和一键布控，2代表专项布控
                }
                else//插入到新表当中
                {
                    xh = GetRecordID(19);
                    int i = bll.SetDispatchNew(listid, hphm, cllx, bdyy, yxsj, userinfo.DeptCode, userinfo.UserCode, xh, mdlx);
                    if (i > 0)//说明更新或修改成功
                    {
                        DataTable dt = bll.GetBkbh(hphm, cllx);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            xh = dt.Rows[0]["bkbh"].ToString();//得到要更新的布控编号
                        }
                        if (bll.UpdateDispatchNewRecive(xh, userinfo.DeptCode) > 0)
                        {
                        }
                        else
                        {
                            Notice("提示", "布控失败，请重试！");
                            return;
                        }
                    }
                }

                if (xh != "")
                {
                    string kkid = "";
                    foreach (string str in listid)
                    {
                        kkid += (kkid == "" ? "" : ",") + str;
                    }
                    if (client.layout(xh, hphm, cllx, kkid))
                    {
                        Notice("提示", "布控成功！");
                        ButReset.Hidden = false;
                        ButAddgrid.Hidden = true;
                    }
                    else
                        Notice("提示", "布控失败，请重试！(web)");
                }
                else
                    Notice("提示", "布控失败，请重试！");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("Dispatched.aspx-ButQueryClick", ex.Message + "；" + ex.StackTrace, "ButQueryClick has an exception");
            }
        }

        private string GetRecordID(int len)
        {
            string mySql = string.Empty;
            try
            {
                int max = (len - 17) * 10;
                Random rd = new Random();
                string s = System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + rd.Next(0, max).ToString();

                return s;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("Dispatched.aspx-GetRecordID", ex.Message + "；" + ex.StackTrace, "GetRecordID has an exception");
                return "0".PadLeft(len, '0');
            }
        }

        /// <summary>
        /// 转换查询模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void changtype(object sender, EventArgs e)
        {
            txtplate.Hidden = ChkLike.Checked;
            pnhphm.Hidden = !ChkLike.Checked;
        }

        #endregion 事件

        #region 方法

        /// <summary>
        /// 提示框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">内容</param>
        private void Notice(string title, string msg)
        {
            Notification.Show(new NotificationConfig
            {
                Title = title,
                Icon = Ext.Net.Icon.Information,
                HideDelay = 2000,
                Height = 120,
                Html = "<br></br>" + msg + "!"
            });
        }

        public static T JSONToObject<T>(string jsonText)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<T>(jsonText);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }

        #region 地图布控

        private int polyCentriod(List<PointF> p, int n, ref PointF Centroid, ref double area)
        {
            int i, j;
            double ai, atmp = 0.0, xtmp = 0, ytmp = 0, ltmp = 0;
            if (n < 3)
                return 1;
            for (i = n - 1, j = 0; j < n; i = j, j++)
            {
                ai = p[i].X * p[j].Y - p[j].X * p[i].Y;
                atmp += ai;
                xtmp += (p[j].X + p[i].X) * ai;
                ytmp += (p[j].Y + p[i].Y) * ai;
            }
            area = atmp / 2;
            if (atmp != 0)
            {
                Centroid.X = (float)(xtmp / (3 * atmp));
                Centroid.Y = (float)(ytmp / (3 * atmp));
                if (area < 0)
                    area = -area;
                return 0;
            }
            return 2;
        }

        private static double EARTH_RADIUS = 6378.137;//地球半径

        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000 * 1000) / 10000;//米
            return s;
        }

        public string Getwhere(string datasb, string datass)
        {
            string where1 = string.Empty;
            string where2 = string.Empty;
            string where = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(datasb))
                {
                    where1 = "  device_mode_id  in (" + datasb + ")  or";
                }
                if (!string.IsNullOrEmpty(datass))
                {
                    where2 = "  purposeid  in (" + datass + ")     or";
                }
                where = where1 + where2;
                if (where.Length > 2)
                {
                    where = where.Substring(0, where.Length - 2);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                logManager.InsertLogError("Dispatched.aspx-GetRecordID", ex.Message + "；" + ex.StackTrace, "GetRecordID has an exception");
            }

            return where;
        }

        private bool IsVisible(PointF point, List<PointF> ListPoint, float maxX)
        {
            int count = 0;
            for (int i = 0; i < ListPoint.Count; i++)
            {
                PointF a = new PointF();
                PointF b = new PointF();
                a.X = point.X;
                a.Y = point.Y;
                b.X = maxX;
                b.Y = point.Y;
                if (Judge(a, b, ListPoint[i], ListPoint[(i + 1) % ListPoint.Count]) == true)
                {
                    count++;
                }
            }
            if (count % 2 == 0)
                return false;
            else
                return true;
        }

        private bool Judge(PointF p0, PointF p1, PointF p2, PointF p3)
        {
            return ((Max(p0.X, p1.X) >= Min(p2.X, p3.X)) &&
                (Max(p2.X, p3.X) >= Min(p0.X, p1.X)) &&
                (Max(p0.Y, p1.Y) >= Min(p2.Y, p3.Y)) &&
                (Max(p2.Y, p3.Y) >= Min(p0.Y, p1.Y)) &&
                (Multiply(p2, p1, p0) * Multiply(p1, p3, p0) >= 0) &&
                (Multiply(p0, p3, p2) * Multiply(p3, p1, p2) >= 0)
                );
        }

        private double Multiply(PointF p1, PointF p2, PointF p0)//叉乘
        {
            return ((p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y));
        }

        private double Max(double a, double b)
        {
            if (a >= b)
                return a;
            return b;
        }

        private double Min(double a, double b)
        {
            if (a <= b)
                return a;
            return b;
        }

        public void cleardata()
        {
            StoreInfo.RemoveAll();
            StoreInfo.DataBind();
        }

        /// <summary>
        /// 查询布控
        /// </summary>
        /// <param name="data">坐标集合</param>
        public void QueryMarkArea(string data)
        {
            List<PointF> points = new List<PointF>();
            object pointList = JSONToObject<object>(data);
            Array aPoint = (Array)pointList;
            for (int i = 0; i < aPoint.Length; i++)
            {
                Dictionary<string, object> spoint = (Dictionary<string, object>)aPoint.GetValue(i);
                PointF PF = new PointF();
                foreach (KeyValuePair<string, object> kv in spoint)
                {
                    if (kv.Key == "lng")
                    {
                        PF.X = float.Parse(kv.Value.ToString());
                    }
                    else if (kv.Key == "lat")
                    {
                        PF.Y = float.Parse(kv.Value.ToString());
                    }
                }
                points.Add(PF);
            }
            PointF center = new PointF();
            PointF maxCenter = new PointF();
            double area = 0;
            double maxLength = 0;
            float maxX = 0;
            List<double> lengths = new List<double>();
            int s = polyCentriod(points, points.Count, ref center, ref area);
            for (int i = 0; i < points.Count; i++)
            {
                double l = GetDistance(points[i].Y, points[i].X, center.Y, center.X);
                lengths.Add(l);
                if (l > maxLength)
                {
                    maxLength = l;
                    maxCenter = points[i];
                }
                if (points[i].X > maxX)
                {
                    maxX = points[i].X;
                }
            }
            double Maxl = Math.Sqrt(Math.Abs(maxCenter.X - center.X) * Math.Abs(maxCenter.X - center.X) + Math.Abs(maxCenter.Y - center.Y) * Math.Abs(maxCenter.Y - center.Y)) * 10000;

            string where1 = "sqrt(pow((x_values-" + center.X.ToString() + "),2)+pow((y_values-" + center.Y.ToString() + "),2))*10000 < " + Maxl.ToString();

            DataTable dt = bll.GetStation();
            DataTable dtOut;
            if (dt != null)
                dtOut = dt.Copy();
            else
                return;
            PointF QueryPoint = new PointF();
            Hashtable hs = new Hashtable();
            for (int n = dtOut.Rows.Count - 1; n >= 0; n--)
            {
                if (dtOut.Rows[n]["xpoint"].ToString() == "")
                {
                    dtOut.Rows[n].Delete();
                    dtOut.AcceptChanges();
                }
                else
                {
                    QueryPoint.X = float.Parse(dtOut.Rows[n]["xpoint"].ToString());
                    QueryPoint.Y = float.Parse(dtOut.Rows[n]["ypoint"].ToString());
                    if (!IsVisible(QueryPoint, points, maxX))
                    {
                        dtOut.Rows[n].Delete();
                        dtOut.AcceptChanges();
                    }
                }
            }
            StoreInfo.RemoveAll();
            StoreInfo.DataSource = dtOut;
            StoreInfo.DataBind();
            showstation(dtOut);
        }

        #endregion 地图布控

        /// <summary>
        /// 监测点展示
        /// </summary>
        /// <param name="dt">监测点数据集</param>
        private void showstation(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string js = " BMAP.addMarker('img/" + dr["ICOREMARK"].ToString() + ".gif','" + dr["xpoint"].ToString() + "','" + dr["ypoint"].ToString() + "','" + dr["STATION_NAME"].ToString() + "');;";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
        }

        #endregion 方法

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
    }
}