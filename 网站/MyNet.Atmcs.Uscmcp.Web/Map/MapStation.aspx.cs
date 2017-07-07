using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.Script.Serialization;

namespace MyNet.Atmcs.Uscmcp.Web.Map
{
    public partial class MapStation : System.Web.UI.Page
    {
        private MyNet.Atmcs.Uscmcp.Bll.MapManager bll = new Bll.MapManager();
        private UserLogin userLogin = new UserLogin();
        private static DataTable dtGetName = null;
        static string kkmax = "1";
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
                kkmax = Request.QueryString["kkmax"];
                if (kkmax == null)
                    kkmax = "1";
                DataTable dt = bll.GetStation();
                showstation(dt);
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

        //[DirectMethod]
        //public void SeleChange()
        protected void AllCheck(object sender, EventArgs e)
        {
            try
            {
                if (Session["stationlist"] != null)
                {
                    Session["stationlist"] = null;
                }
                if (Session["stationlistname"] != null)
                {
                    Session["stationlistname"] = null;
                }

                RowSelectionModel sm = this.GridStation.SelectionModel.Primary as RowSelectionModel;
                List<string> listid = new List<string>();
                List<string> listName = new List<string>();
                foreach (SelectedRow row in sm.SelectedRows)
                {
                    listid.Add(row.RecordID);
                    DataRow[] rows = dtGetName.Select("STATION_ID=" + row.RecordID);
                    listName.Add(rows[0]["STATION_NAME"].ToString());
                }
                Session["stationlist"] = listid;
                Session["stationlistname"] = listName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 选择复选框的时候执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CheckChange(object sender, EventArgs e)
        {
            try
            {
                if (Session["stationlist"] != null)
                {
                    Session["stationlist"] = null;
                }
                if (Session["stationlistname"] != null)
                {
                    Session["stationlistname"] = null;
                }
                RowSelectionModel sm = this.GridStation.SelectionModel.Primary as RowSelectionModel;

                if (kkmax=="1"&&sm.SelectedRows.Count > 15)
                {
                    Notice("提示信息", "最多选择10个卡口");
                    return;
                }
                List<string> listid = new List<string>();
                List<string> listName = new List<string>();
                foreach (SelectedRow row in sm.SelectedRows)
                {
                    listid.Add(row.RecordID);
                    DataRow[] rows = dtGetName.Select("STATION_ID=" + row.RecordID);
                    listName.Add(rows[0]["STATION_NAME"].ToString());
                }
                Session["stationlist"] = listid;

                Session["stationlistname"] = listName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [DirectMethod(Namespace = "OnEvl")]
        public void hidemap()
        {
            if (Session["stationlist"] != null)
            {
                Session["stationlist"] = null;
            }
            if (Session["stationlistname"] != null)
            {
                Session["stationlistname"] = null;
            }
            RowSelectionModel sm = this.GridStation.SelectionModel.Primary as RowSelectionModel;

            if (kkmax == "1" && sm.SelectedRows.Count > 15)
            {
                Notice("提示信息", "最多选择10个卡口");
                return;
            }
            List<string> listid = new List<string>();
            List<string> listName = new List<string>();
            foreach (SelectedRow row in sm.SelectedRows)
            {
                listid.Add(row.RecordID);
                DataRow[] rows = dtGetName.Select("STATION_ID=" + row.RecordID);
                listName.Add(rows[0]["STATION_NAME"].ToString());
            }
            Session["stationlist"] = listid;

            Session["stationlistname"] = listName;
            string nodeid = "";
            string nodeName = "";

            foreach (string str in listid)
            {
                nodeid += (nodeid == "" ? "" : ",") + str;
            }
            if (Session["tree"] != null)
            {
                Session["tree"] = null;
            }
            Session["tree"] = nodeid;
            foreach (string str in listName)
            {
                nodeName += (nodeName == "" ? "" : ",") + str;
            }
            string js = "setSelect('" + nodeName + "');";
            this.ResourceManager1.RegisterAfterClientInitScript(js);
        }

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
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.BottomRight,
                    OffsetY = -60
                },
                Html = "<br></br>" + msg + "!"
            });
        }

        [DirectMethod]
        public void ButCanel()
        { }

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
            if (dtGetName != null)
            {
                dtGetName = null;
            }
            dtGetName = dtOut;
            StoreInfo.DataSource = dtOut;
            StoreInfo.DataBind();
            showstation(dtOut);
            //if (dtOut != null && dtOut.Rows.Count > 0)
            //{
            //    string js1 = "CKAll(" + dtOut.Rows.Count + ");";
            //    this.ResourceManager1.RegisterAfterClientInitScript(js1);
            //    RowSelectionModel sm = GridStation.SelectionModel.Primary as RowSelectionModel;
            //    List<string> listid = new List<string>();
            //    List<string> listName = new List<string>();
            //    foreach (SelectedRow row in sm.SelectedRows)
            //    {
            //        listid.Add(row.RecordID);
            //        DataRow[] rows = dtGetName.Select("STATION_ID=" + row.RecordID);
            //        listName.Add(rows[0]["STATION_NAME"].ToString());
            //    }
            //    Session["stationlist"] = listid;
            //    Session["stationlistname"] = listName;

            //}
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

        private double Multiply(PointF p1, PointF p2, PointF p0)//叉乘
        {
            return ((p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y));
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

        /// <summary>
        /// 监测点展示
        /// </summary>
        /// <param name="dt">监测点数据集</param>
        private void showstation(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                string js = " BMAP.addMarker('img/" + dr["ICOREMARK"].ToString() + ".gif','" + dr["xpoint"].ToString() + "','" + dr["ypoint"].ToString() + "','" + dr["STATION_NAME"].ToString() + "');";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
        }

        public void CkAll(object sender, EventArgs e)
        {
            string js1 = "CKAll();";
            this.ResourceManager1.RegisterAfterClientInitScript(js1);
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
            string str = MyNet.Common.Lang.Language.CreateInstance(className).GetLanguageStr(value, desc, className);
            return str;
        }
    }
}