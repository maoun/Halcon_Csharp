using System.Collections.Generic;
using System.Data;
using Ext.Net;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class MapDataOperate
    {
        public GisShow gs = new GisShow();
        private DeviceManager deviceManager = new DeviceManager();
        private DataCommon dataCommon = new DataCommon();

        /// <summary>
        ///
        /// </summary>
        /// <param name="enableDrag">是否运行拖动</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<string> GetMarkJs(bool enableDrag, string type)
        {
            List<string> list = new List<string>();
            DataTable dt2 = null;
            DataTable dt = null;
            string mark;
            switch (type)
            {
                case "COM":
                    dt2 = gs.GetComStation("MARKTYPE='COM'");
                    if (dt2 != null)
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                string id = dt2.Rows[j][0].ToString();
                                string name = dt2.Rows[j][1].ToString();
                                string x = dt2.Rows[j][3].ToString();
                                string y = dt2.Rows[j][4].ToString();
                                mark = "BMAP.addMarkerlabel('../Map/img/" + type + ".gif'," + x + "," + y + ",'" + name + "');";
                                list.Add(mark);
                            }
                        }
                    }
                    return list;

                case "ZD":

                    dt = gs.GetConstructionInfo("1=1");
                    if (dt != null)
                    {
                        for (int n = 0; n < dt.Rows.Count; n++)
                        {
                            string xcoordinate = dt.Rows[n][30].ToString();
                            if (!string.IsNullOrEmpty(xcoordinate))
                            {
                                mark = "var _id ;var _type; _type='" + type + "'; _id='" + dt.Rows[n][0].ToString() + "';BMAP.addMarkerContent(" + enableDrag.ToString().ToLower() + ",'../Map/img/" + type + ".gif'," + dt.Rows[n][30].ToString() + "," + dt.Rows[n][31].ToString() + ",'" + dt.Rows[n][11].ToString() + "',{ id: _id, type: _type });";
                                list.Add(mark);
                            }
                        }
                    }
                    return list;

                case "GZ":
                    dt = gs.GetTraffInfo("1=1");
                    if (dt != null)
                    {
                        for (int n = 0; n < dt.Rows.Count; n++)
                        {
                            string xcoordinate = dt.Rows[n][12].ToString();
                            if (!string.IsNullOrEmpty(xcoordinate))
                            {
                                mark = "var _id ;var _type; _type='" + type + "'; _id='" + dt.Rows[n][0].ToString() + "';BMAP.addMarkerContent(" + enableDrag.ToString().ToLower() + ",'../Map/img/" + type + ".gif'," + dt.Rows[n][12].ToString() + "," + dt.Rows[n][13].ToString() + ",'" + dt.Rows[n][1].ToString() + "',{ id: _id, type: _type });";
                                list.Add(mark);
                            }
                        }
                    }
                    return list;

                case "1=1":
                    dt = gs.GetGisDeviceList("1=1");
                    break;

                default:
                    dt = gs.GetGisDeviceList("marktype='" + type + "'");
                    break;
            }
            List<string> lst = GetDeviceMarkJs(enableDrag, dt, false);
            list.AddRange(lst);
            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enableDrag"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<string> GetDeviceMarkJs(bool enableDrag, string type)
        {
            DataTable dt = gs.GetGisDeviceList("marktype='" + type + "'");
            List<string> lst = GetDeviceMarkJs(enableDrag, dt, false);
            return lst;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enableDrag"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<string> GetDeviceMarkJsPGIS(bool enableDrag, string type)
        {
            DataTable dt = gs.GetGisDeviceList("marktype='" + type + "'");
            List<string> lst = GetDeviceMarkJs(enableDrag, dt, true);
            return lst;
        }

        /// <summary>
        /// 组装js语句，返回用于显示在地图上
        /// </summary>
        /// <param name="enableDrag"></param>
        /// <param name="dt"></param>
        /// <param name="isPgis"></param>
        /// <returns></returns>
        public List<string> GetDeviceMarkJs(bool enableDrag, DataTable dt, bool isPgis)
        {
            List<string> list = new List<string>();
            DataTable dt2 = null;
            string mark;
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][2].ToString().Equals("1"))
                    {
                        string name = dt.Rows[i][1].ToString();
                        string id = dt.Rows[i][0].ToString();
                        string marktype = dt.Rows[i][9].ToString();
                        string x = dt.Rows[i][3].ToString();
                        string y = dt.Rows[i][4].ToString();

                        switch (marktype)
                        {
                            case "CCTV":
                                dt2 = deviceManager.GetCctvSetting("*", "station_id='" + id + "'");
                                if (dt2 != null)
                                {
                                    if (dt2.Rows.Count > 0)
                                    {
                                        string ip = dataCommon.ChangeIp(dt2.Rows[0]["col8"].ToString());
                                        string port = dt2.Rows[0]["col9"].ToString();
                                        string user = dt2.Rows[0]["col10"].ToString();
                                        string pwd = dt2.Rows[0]["col11"].ToString();
                                        string chl = dt2.Rows[0]["col5"].ToString();
                                        string modeid = dt2.Rows[0]["col16"].ToString();
                                        string str = ip + "|" + port + "|" + chl + "|" + user + "|" + pwd + "|" + modeid;

                                        if (isPgis)
                                        {
                                            mark = "var _id ;var _type; _type='" + marktype + "'; _id='" + id + "';_para='" + str + "';BMAP.addMarkerContent(" + enableDrag.ToString().ToLower() + ",'../Map/img/" + marktype + ".gif'," + x + "," + y + ",'" + name + "',{ id: _id, type: _type,para: _para });";
                                        }
                                        else
                                        {
                                            mark = "var _id ;var _type; _type='" + marktype + "'; _id='" + id + "';_para='" + str + "';BMAP.addMarkerContent(" + enableDrag.ToString().ToLower() + ",'../Map/img/" + marktype + ".gif'," + x + "," + y + ",'" + name + "',{ id: _id, type: _type,para: _para });";
                                        }
                                        list.Add(mark);
                                    }
                                }
                                break;

                            case "VMS":
                                dt2 = deviceManager.GetVMSSetting("*", "station_id='" + id + "'");
                                if (dt2 != null)
                                {
                                    if (dt2.Rows.Count > 0)
                                    {
                                        string w = dt2.Rows[0]["col21"].ToString();
                                        string h = dt2.Rows[0]["col22"].ToString();
                                        string str = id + "|" + w + "|" + h;
                                        mark = "var _id ;var _type; _type='" + marktype + "'; _id='" + id + "';_para='" + str + "';BMAP.addMarkerContent(" + enableDrag.ToString().ToLower() + ",'../Map/img/" + marktype + ".gif'," + x + "," + y + ",'" + name + "',{ id: _id, type: _type,para: _para });";
                                        list.Add(mark);
                                    }
                                }

                                break;

                            case "UTC":
                                dt2 = deviceManager.GetUTCSetting("*", "station_id='" + id + "'");
                                if (dt2 != null)
                                {
                                    if (dt2.Rows.Count > 0)
                                    {
                                        string str = dt2.Rows[0]["col8"].ToString() + "|" + dt2.Rows[0]["col20"].ToString();
                                        mark = "var _id ;var _type; _type='" + marktype + "'; _id='" + id + "';_para='" + str + "';BMAP.addMarkerContent(" + enableDrag.ToString().ToLower() + ",'../Map/img/" + marktype + ".gif'," + x + "," + y + ",'" + name + "',{ id: _id, type: _type,para: _para });";
                                        list.Add(mark);
                                    }
                                }
                                break;

                            default:
                                mark = "var _id ;var _type; _type='" + marktype + "'; _id='" + id + "';BMAP.addMarkerContent(" + enableDrag.ToString().ToLower() + ",'../Map/img/" + marktype + ".gif'," + x + "," + y + ",'" + name + "',{ id: _id, type: _type });";
                                list.Add(mark);
                                break;
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        public void Notice(string title, string msg)
        {
            Notification.Show(new NotificationConfig
            {
                Title = title,
                Icon = Icon.ApplicationError,
                HideDelay = 2000,
                Height = 120,
                Html = "<br></br>" + msg + "!"
            });
        }

        /// <summary>
        /// 获得图标类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typedesc"></param>
        /// <returns></returns>
        public Icon GetIcon(string type, ref string typedesc)
        {
            switch (type)
            {
                case "03":
                    typedesc = "TCS";
                    return Icon.Car;

                case "02":
                    typedesc = "TGS";
                    return Icon.Car;

                case "04":
                    typedesc = "VGS";
                    return Icon.Car;

                case "01":
                    typedesc = "TMS";
                    return Icon.Car;

                case "09":
                    typedesc = "CCTV";
                    return Icon.Camera;

                case "10":
                    typedesc = "CCTV";
                    return Icon.Camera;

                case "11":
                    typedesc = "VMS";
                    return Icon.PictureEmpty;

                case "13":
                    typedesc = "WEA";
                    return Icon.WeatherCloudy;

                case "12":
                    typedesc = "UTC";
                    return Icon.ArrowNwNeSwSe;

                case "15":
                    typedesc = "TFM";
                    return Icon.FlagRed;

                default:
                    typedesc = "COM";
                    return Icon.CommentAdd;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enableDrag"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<string> GetMarkJsList(bool enableDrag, DataTable dt)
        {
            List<string> list = new List<string>();

            for (int n = 0; n < dt.Rows.Count; n++)
            {
                string id = dt.Rows[n]["id"].ToString();
                string markicon = dt.Rows[n]["markicon"].ToString();
                switch (id)
                {
                    case "0101":
                        list.AddRange(GetZDMarks(enableDrag, markicon));
                        break;

                    case "0102":
                        list.AddRange(GetGzMarks(enableDrag, markicon));
                        break;

                    case "0201":
                        list.AddRange(GetRoadState("RED"));
                        break;

                    case "0202":
                        list.AddRange(GetRoadState("YELLOW"));
                        break;

                    case "0203":
                        list.AddRange(GetRoadState("GREEN"));
                        break;

                    case "0501":
                        list.AddRange(GetDeviceMarkJs(enableDrag, "TMS"));
                        break;

                    case "0502":
                        list.AddRange(GetDeviceMarkJs(enableDrag, "TGS"));
                        break;

                    case "0503":
                        list.AddRange(GetDeviceMarkJs(enableDrag, "CCTV"));
                        break;

                    case "0504":
                        list.AddRange(GetDeviceMarkJs(enableDrag, "UTC"));
                        break;

                    case "0505":
                        list.AddRange(GetDeviceMarkJs(enableDrag, "VMS"));
                        break;

                    case "0506":
                        list.AddRange(GetDeviceMarkJs(enableDrag, "TES"));
                        break;
                }
            }
            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enableDrag"></param>
        /// <param name="markicon"></param>
        /// <returns></returns>
        public List<string> GetGzMarks(bool enableDrag, string markicon)
        {
            List<string> list = new List<string>();
            DataTable dt = gs.GetTraffInfo("1=1");
            if (dt != null)
            {
                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    string xcoordinate = dt.Rows[n][12].ToString();
                    if (!string.IsNullOrEmpty(xcoordinate))
                    {
                        string mark = "var _id ;var _type; _type='" + markicon + "'; _id='" + dt.Rows[n][0].ToString() + "';BMAP.addMarkerContent(" + enableDrag.ToString().ToLower() + ",'img/" + markicon + ".gif'," + dt.Rows[n][12].ToString() + "," + dt.Rows[n][13].ToString() + ",'" + dt.Rows[n][1].ToString() + "',{ id: _id, type: _type });";
                        list.Add(mark);
                    }
                }
            }
            return list;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enableDrag"></param>
        /// <param name="markicon"></param>
        /// <returns></returns>
        public List<string> GetZDMarks(bool enableDrag, string markicon)
        {
            List<string> list = new List<string>();
            DataTable dt = gs.GetConstructionInfo("1=1");
            if (dt != null)
            {
                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    string xcoordinate = dt.Rows[n][30].ToString();
                    if (!string.IsNullOrEmpty(xcoordinate))
                    {
                        string mark = "var _id ;var _type; _type='" + markicon + "'; _id='" + dt.Rows[n][0].ToString() + "';BMAP.addMarkerContent(" + enableDrag.ToString().ToLower() + ",'img/" + markicon + ".gif'," + dt.Rows[n][30].ToString() + "," + dt.Rows[n][31].ToString() + ",'" + dt.Rows[n][11].ToString() + "',{ id: _id, type: _type });";
                        list.Add(mark);
                    }
                }
            }
            return list;
        }

        #region 路况绘制

        /// <summary>
        ///
        /// </summary>
        /// <param name="jtzs"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private string GetColor(string jtzs, ref string title)
        {
            string[] colors = new string[6];
            colors[0] = "#5ED704";
            colors[1] = "#398303";
            colors[2] = "#EEFD04";
            colors[3] = "#FDC504";
            colors[4] = "#FD0404";
            colors[5] = "#ffffff";

            string[] jl = new string[6];
            jl[0] = "行驶畅通";
            jl[1] = "基本畅通";
            jl[2] = "轻度拥堵";
            jl[3] = "中度拥堵";
            jl[4] = "严重拥堵";
            jl[5] = "未知状况";

            double zs = double.Parse(jtzs);

            if (zs <= 2)
            {
                title = jl[0];
                return colors[0];
            }
            else if (zs <= 4 && zs > 2)
            {
                title = jl[1];
                return colors[1];
            }
            else if (zs <= 6 && zs > 4)
            {
                title = jl[2];
                return colors[2];
            }
            else if (zs <= 8 && zs > 6)
            {
                title = jl[3];
                return colors[3];
            }
            else if (zs <= 10 && zs > 8)
            {
                title = jl[4];
                return colors[4];
            }
            else
            {
                title = jl[5];
                return colors[5];
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inlczt"></param>
        /// <returns></returns>
        public List<string> GetRoadState(string inlczt)
        {
            DataTable dt = null;
            List<string> list = new List<string>();
            switch (inlczt)
            {
                case "ALL":
                    dt = gs.GetRoadSegInfo("1=1");
                    break;

                case "RED":
                    dt = gs.GetRoadSegInfo(" lczt >8 ");
                    break;

                case "GREEN":
                    dt = gs.GetRoadSegInfo(" lczt <=4 ");
                    break;

                case "YELLOW":
                    dt = gs.GetRoadSegInfo(" lczt <=8   and  lczt >4");
                    break;
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataTable mydt = gs.GetRoadSegPointInfo(" a.id='" + dt.Rows[i][0].ToString() + "' ");
                    string strPoint = "";
                    for (int j = 0; j < mydt.Rows.Count; j++)
                    {
                        if (j != mydt.Rows.Count - 1)
                        {
                            strPoint = strPoint + mydt.Rows[j]["col3"] + "," + mydt.Rows[j]["col4"].ToString() + "|";
                        }
                        else
                        {
                            strPoint = strPoint + mydt.Rows[j]["col3"] + "," + mydt.Rows[j]["col4"].ToString();
                        }
                    }
                    if (mydt.Rows.Count > 0)
                    {
                        string lczt = dt.Rows[i][5].ToString();
                        string mark = string.Empty;
                        string title = string.Empty;
                        mark = "BMAP.addRoadline('" + GetColor(lczt, ref title) + "','" + strPoint + "','" + dt.Rows[i][2].ToString() + " - " + title + "' ,'" + dt.Rows[i][0].ToString() + "');";
                        list.Add(mark);
                    }
                }
            }
            return list;
        }

        #endregion 路况绘制
    }
}