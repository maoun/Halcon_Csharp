using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class GisShow
    {
        private static readonly IGisShow dal = DALFactory.CreateGisShow();

        public delegate DataTable GetFlowDelegate(string directione, string date);

        /// <summary>
        ///
        /// </summary>
        /// <param name="pointtype"></param>
        /// <returns></returns>
        public DataTable GetMapLable(string where)
        {            
            if (string.IsNullOrEmpty(where))
                return null;
            else
                return Common.ChangColName(dal.GetMapLable(where));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTgsStation(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetTgsStation(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetComStation(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetComStation(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTgsStationFlowInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetTgsStationFlowInfo(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xh"></param>
        /// <returns></returns>
        public DataTable GetStationInfo(string xh)
        {
            try
            {
                return Common.ChangColName(dal.GetStationInfo(xh));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查找所有检测点类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetStationType(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetStationType(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetCCTV(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetCCTV(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTesEvent(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetTesEvent(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetFlow(string tableName, string kkid,string strdate,string xs)
        {
            try
            {
                return Common.ChangColName(dal.GetFlow(tableName, kkid,strdate,xs));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetFlowState(string tableName)
        {
            try
            {
                return Common.ChangColName(dal.GetFlowState(tableName));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdatePointInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdatePointInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateGisMark(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateGisMark(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeletePointInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeletePointInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateStationType(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateStationType(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertKmlInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.InsertKmlInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteKmlInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteKmlInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pointtype"></param>
        /// <returns></returns>
        public DataTable GetWatchMapLable()
        {
            return GetMapLable(" 1=1 ");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string GetXmlstring()
        {
            DataTable dt = GetWatchMapLable();
            StringBuilder strXml = new StringBuilder();
            strXml.Append("<?xml version='1.0' encoding='gb2312'?>\n");
            strXml.Append("<maplables>\n");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strXml.Append("<maplable id='" + dt.Rows[i]["col0"].ToString() + "'>\n");
                    strXml.Append("<pid>" + dt.Rows[i]["col0"].ToString() + "</pid>\n");
                    strXml.Append("<xPoint>" + dt.Rows[i]["col1"].ToString() + "</xPoint>\n");
                    strXml.Append("<yPoint>" + dt.Rows[i]["col2"].ToString() + "</yPoint>\n");
                    strXml.Append("<pointtype>" + dt.Rows[i]["col3"].ToString() + "</pointtype>\n");
                    strXml.Append("<pointname>" + dt.Rows[i]["col5"].ToString() + "</pointname>\n");
                    strXml.Append("<pointtag>" + dt.Rows[i]["col6"].ToString() + "</pointtag>\n");
                    strXml.Append("<pointimage>" + dt.Rows[i]["col7"].ToString() + "</pointimage>\n");
                    strXml.Append("<pointbutton>" + dt.Rows[i]["col8"].ToString() + "</pointbutton>\n");
                    strXml.Append("</maplable>\n");
                }
            }
            strXml.Append("</maplables>\n");
            return strXml.ToString();
        }

        /// <summary>
        /// 获得设备状态map信息
        /// </summary>
        /// <returns></returns>
        public string GetDeviceStateXmlString()
        {
            DataTable dt = Common.ChangColName(dal.GetDeviceStateMap());
            StringBuilder strXml = new StringBuilder();
            strXml.Append("<?xml version='1.0' encoding='gb2312'?>\n");
            strXml.Append("<devices>\n");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strXml.Append("<device>\n");
                    strXml.Append("<deviceid>" + dt.Rows[i]["col0"].ToString() + "</deviceid>\n");
                    strXml.Append("<devicename>" + dt.Rows[i]["col1"].ToString() + "</devicename>\n");
                    strXml.Append("<state>" + dt.Rows[i]["col2"].ToString() + "</state>\n");
                    strXml.Append("<statename>" + dt.Rows[i]["col3"].ToString() + "</statename>\n");
                    strXml.Append("<xPoint>" + dt.Rows[i]["col4"].ToString() + "</xPoint>\n");
                    strXml.Append("<yPoint>" + dt.Rows[i]["col5"].ToString() + "</yPoint>\n");
                    strXml.Append("</device>\n");
                }
            }
            strXml.Append("</devices>\n");
            return strXml.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetTrackMapString(DataTable dt)
        {
            try
            {
                StringBuilder strXml = new StringBuilder();
                strXml.Append("<?xml version='1.0' encoding='gb2312'?>\n");
                strXml.Append("<kks>\n");
                if (dt != null)
                {
                    Hashtable hs = ConventDataTable(dt);
                    foreach (DictionaryEntry pair in hs)
                    {
                        strXml.Append("<kk>\n");
                        strXml.Append(pair.Value.ToString());

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string kkid = dt.Rows[i]["col2"].ToString();
                            if (pair.Key.ToString() == kkid)
                            {
                                strXml.Append("<path>\n");
                                strXml.Append("<id>" + dt.Rows[i]["col0"].ToString() + "</id>\n");
                                strXml.Append("<xh>" + dt.Rows[i]["col1"].ToString() + "</xh>\n");
                                strXml.Append("<fxmc>" + dt.Rows[i]["col13"].ToString() + "</fxmc>\n");
                                strXml.Append("<gwsj>" + dt.Rows[i]["col7"].ToString() + "</gwsj>\n");
                                strXml.Append("<jllx>" + dt.Rows[i]["col18"].ToString() + "</jllx>\n");
                                strXml.Append("<zjwj1>" + dt.Rows[i]["col23"].ToString() + "</zjwj1>\n");
                                strXml.Append("</path>\n");
                            }
                        }
                        strXml.Append("</kk>\n");
                    }

                    strXml.Append("</kks>\n");
                }
                return strXml.ToString();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private Hashtable ConventDataTable(DataTable dt)
        {
            Hashtable hts = new Hashtable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string kkid = dt.Rows[i]["col2"].ToString();
                if (!hts.ContainsKey(kkid))
                {
                    StringBuilder strXml = new StringBuilder();
                    strXml.Append("<kkid>" + dt.Rows[i]["col2"].ToString() + "</kkid>\n");
                    strXml.Append("<kkmc>" + dt.Rows[i]["col3"].ToString() + "</kkmc>\n");
                    strXml.Append("<cjjg>" + dt.Rows[i]["col22"].ToString() + "</cjjg>\n");
                    strXml.Append("<xPoint>" + dt.Rows[i]["col26"].ToString() + "</xPoint>\n");
                    strXml.Append("<yPoint>" + dt.Rows[i]["col27"].ToString() + "</yPoint>\n");
                    hts.Add(kkid, strXml.ToString());
                }
            }
            return hts;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="directiones"></param>
        /// <param name="flowDate"></param>
        /// <param name="flowType"></param>
        /// <returns></returns>
        public DataTable GetFlow(List<string> directiones, string flowDate, string flowType)
        {
            try
            {
                if (directiones.Count < 1 || string.IsNullOrEmpty(flowDate) || string.IsNullOrEmpty(flowType))
                {
                    return null;
                }

                DataTable dtReturn = this.GetHourFlowDataTable(directiones, flowDate);
                return dtReturn;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得24小时流量数据
        /// </summary>
        /// <param name="directiones">方向集合</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        private DataTable GetHourFlowDataTable(List<string> directiones, string date)
        {
            try
            {
                return GetFlowDataTable(directiones, date, "时", dal.PassCarHourFlow, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="directiones"></param>
        /// <param name="date"></param>
        /// <param name="colname"></param>
        /// <param name="GetFlow"></param>
        /// <param name="icol"></param>
        /// <returns></returns>
        private DataTable GetFlowDataTable(List<string> directiones, string date, string colname, GetFlowDelegate GetFlow, int icol)
        {
            try
            {
                DataTable myDatatable = new DataTable("myDatatable");
                DataColumn col;
                DataRow row;

                col = new DataColumn("方向名称", typeof(System.String));//手动添加第一列
                myDatatable.Columns.Add(col);
                bool isAddedHead = false; //是否已经加过head行了

                for (int i = 0; i < directiones.Count; i++)
                {
                    DataTable dtFlow = Common.ChangColName(GetFlow(directiones[i], date));//根据方向编号获得流量数据

                    if (dtFlow != null && dtFlow.Rows.Count > 0)
                    {
                        if (!isAddedHead)//没加过head，添加列名称数据
                        {
                            for (int j = 0; j < dtFlow.Rows.Count; j++)  //初始化列名
                            {
                                if (icol == 0)
                                {
                                    col = new DataColumn(dtFlow.Rows[j]["col3"].ToString() + colname, typeof(System.String));
                                }
                                else
                                {
                                    col = new DataColumn((j + icol) + colname, typeof(System.String));
                                }
                                myDatatable.Columns.Add(col);
                            }

                            col = new DataColumn("总计", typeof(System.String));
                            myDatatable.Columns.Add(col);
                            isAddedHead = true;
                        }
                        row = myDatatable.NewRow();
                        row["方向名称"] = dal.GetSysCodedesc("240025", directiones[i].Split('|')[1]);// dtFlow.Rows[0]["col0"].ToString();
                        double count = 0;
                        DataRow dr = myDatatable.NewRow();
                        for (int j = 0; j < dtFlow.Rows.Count; j++) //循环为新行赋值
                        {
                            if (dtFlow.Rows[j]["col2"] == null || dtFlow.Rows[j]["col2"].ToString() == "")
                                row[j + 1] = "0";
                            else
                            {
                                row[j + 1] = dtFlow.Rows[j]["col2"].ToString();
                                count = count + double.Parse(dtFlow.Rows[j]["col2"].ToString());
                            }
                        }
                        row[dtFlow.Rows.Count + 1] = count.ToString();
                        myDatatable.Rows.Add(row);
                    }
                    else
                    {
                        if (!isAddedHead)//没加过head，添加列名称数据
                        {
                            switch (colname)
                            {
                                case "时":
                                    for (int j = 0; j < 24; j++)  //初始化列名
                                    {
                                        col = new DataColumn(j.ToString() + "时", typeof(System.String));
                                        myDatatable.Columns.Add(col);
                                    }
                                    break;

                                case "日":
                                    for (int j = 1; j < 31; j++)  //初始化列名
                                    {
                                        col = new DataColumn(j.ToString() + "日", typeof(System.String));
                                        myDatatable.Columns.Add(col);
                                    }
                                    break;

                                case "周":
                                    for (int j = 1; j < 52; j++)  //初始化列名
                                    {
                                        col = new DataColumn(j.ToString() + "周", typeof(System.String));
                                        myDatatable.Columns.Add(col);
                                    }
                                    break;

                                case "月":
                                    for (int j = 1; j < 13; j++)  //初始化列名
                                    {
                                        col = new DataColumn(j.ToString() + "月", typeof(System.String));
                                        myDatatable.Columns.Add(col);
                                    }
                                    break;

                                case "年":
                                    for (int j = 1; j < 11; j++)  //初始化列名
                                    {
                                        col = new DataColumn(j.ToString() + "年", typeof(System.String));
                                        myDatatable.Columns.Add(col);
                                    }
                                    break;
                            }
                            col = new DataColumn("总计", typeof(System.String));
                            myDatatable.Columns.Add(col);
                            isAddedHead = true;
                        }
                    }
                }
                return myDatatable;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 返回线性流量图所需要的格式数据
        /// </summary>
        /// <param name="dtFlow">流量数据</param>
        /// <param name="datas">格式数据</padatasram>
        /// <param name="labels">流量名称</param>
        /// <param name="xLabels">流量x节点名称</param>
        public void GetLineChartData(DataTable dtFlow, out List<List<double>> datas, out List<string> labels, out List<string> xLabels)
        {
            try
            {
                List<List<double>> data = new List<List<double>>();
                List<double> myData;
                List<string> lable = new List<string>();
                List<string> xLabel = new List<string>();
                bool isAddLabel = false;
                if (dtFlow != null && dtFlow.Rows.Count > 0)
                {
                    for (int i = 0; i < dtFlow.Rows.Count; i++) //行
                    {
                        myData = new List<double>();
                        for (int j = 1; j < dtFlow.Columns.Count; j++) //列
                        {
                            if (!isAddLabel)
                            {
                                //添加列名称，并只取数字部分
                                xLabel.Add(dtFlow.Columns[j].ColumnName.Substring(0, dtFlow.Columns[j].ColumnName.Length - 1));
                            }
                            myData.Add(System.Convert.ToDouble(dtFlow.Rows[i][j].ToString()));//添加流量数据double类型
                        }
                        isAddLabel = true;
                        data.Add(myData);
                        lable.Add(dtFlow.Rows[i][0].ToString());//添加一个方向的流量数据
                    }
                }
                else
                {
                    myData = new List<double>();
                    for (int j = 1; j < dtFlow.Columns.Count; j++) //列
                    {
                        if (!isAddLabel)
                        {
                            //添加列名称，并只取数字部分
                            xLabel.Add(dtFlow.Columns[j].ColumnName.Substring(0, dtFlow.Columns[j].ColumnName.Length - 1));
                        }
                        myData.Add(0);//添加流量数据double类型
                    }
                    isAddLabel = true;
                    data.Add(myData);
                    lable.Add("");//添加一个方向的流量数据
                }
                datas = data;
                labels = lable;
                xLabels = xLabel;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                datas = new List<List<double>>(); ;
                labels = new List<string>();
                xLabels = new List<string>();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetGisRoadKml(string where)
        {
            if (string.IsNullOrEmpty(where))
                return null;
            else
                return Common.ChangColName(dal.GetGisRoadKml(where));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetGisRoad(string where)
        {
            if (string.IsNullOrEmpty(where))
                return null;
            else
                return Common.ChangColName(dal.GetGisRoad(where));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTfmFlowState(string where)
        {
            if (string.IsNullOrEmpty(where))
                return null;
            else
                return Common.ChangColName(dal.GetTfmFlowState(where));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTgsAreaFlowState(string where)
        {
            if (string.IsNullOrEmpty(where))
                return null;
            else
                return Common.ChangColName(dal.GetTgsAreaFlowState(where));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetStationPoint(string where)
        {
            if (string.IsNullOrEmpty(where))
                return null;
            else
                return Common.ChangColName(dal.GetStationPoint(where));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetMarkArray(string type)
        {
            try
            {
                return Common.ChangColName(dal.GetMarkArray(type));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="markarray"></param>
        /// <returns></returns>
        public int UpdataMarkArray(string id, string markarray)
        {
            try
            {
                return dal.UpdataMarkArray(id, markarray);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///获得监测点 标注状态信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetGisDeviceList(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetGisDeviceList(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetGPSDeviceList(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetGPSDeviceList(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetPgis(string type)
        {
            try
            {
                return Common.ChangColName(dal.GetPgis(type));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="led_id"></param>
        /// <returns></returns>
        public string GetLastProjectId(string led_id)
        {
            try
            {
                return dal.GetLastProjectId(led_id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public DataTable GetProgramListByProject(string ProjectId)
        {
            try
            {
                return dal.GetProgramListByProject(ProjectId);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="programid"></param>
        /// <returns></returns>
        public DataTable GetProgramInfo(string programid)
        {
            try
            {
                return dal.GetProgramInfo(programid);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获取交通管制信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTraffInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetTraffInfo(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///加载施工占道信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetConstructionInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetConstructionInfo(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdataTraffInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdataTraffInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdataConstructionInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdataConstructionInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetRoadSegInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetRoadSegInfo(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetRoadSegPointInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetRoadSegPointInfo(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetRoadVDInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetRoadVDInfo(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdataRoadState(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdataRoadState(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }
    }
}