using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using MyNet.Common.Method;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class TgsDataInfo
    {
        /// <summary>
        ///  用户操作接口
        /// </summary>
        private static ITgsDataInfo dal = DALFactory.CreateTgsDataInfo();

        public delegate DataTable GetFlowDelegate(string directione, string date);

        //private string SystemID = "00";
        private SettingManager settingManager = new SettingManager();

        private SystemManager systemManager = new SystemManager();

        public TgsDataInfo()
        {
        }
      
           /// <summary>
        /// 获得政府公告信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetZfgg()
        {
            return dal.GetZfgg();
        }
         /// <summary>
        /// 插入违法信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertPeccancy(System.Collections.Hashtable hs)
        {
             return  dal.InsertPeccancy(hs);
        }
        /// <summary>
        /// 得到一个Json字符串
        /// </summary>
        /// <returns></returns>
        public DataTable GetJson(out DataTable dt)
        {
            return dal.GetJson(out dt);
        }
        /// <summary>
        /// 得到车辆品牌的子品牌字符串
        /// </summary>
        /// <param name="clpp"></param>
        /// <returns></returns>
        public DataTable GetClppString(string clpp)
        {
            return Common.ChangColName(dal.GetClppString(clpp));
        }
        /// <summary>
        /// 得到车辆品牌的子品牌字符串
        /// </summary>
        /// <param name="clpp"></param>
        /// <returns></returns>
        public string GetClzppString(string clpp)
        {
            if (string.IsNullOrEmpty(clpp)||clpp.Contains("-"))
            {
                return clpp;
            }
            else
            {
                DataTable dt = GetClppString(clpp);
                string strsClzpp = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == (dt.Rows.Count - 1))//当最后一个不要加逗号
                    {
                        strsClzpp = strsClzpp + dt.Rows[i][1].ToString();
                    }
                    else
                    {
                        strsClzpp = strsClzpp + dt.Rows[i][1].ToString() + ",";
                    }
                }
                return '"'+strsClzpp+'"';
            }
        }
        #region 过往车辆信息

        /// <summary>
        /// 获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startRow"></param>
        /// <param name="endRow"></param>
        /// <returns></returns>
        public DataTable GetPassCarInfo(string where, int startRow, int endRow)
        {
            try
            {
                return Common.ChangColName(dal.GetPassCarInfo(where, startRow, endRow));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPassCarTimesInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPassCarTimesInfo(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetQueryTimeTemp(string where)
        {
            try
            {
                return dal.GetQueryTimeTemp(where);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="times"></param>
        /// <param name="rownum"></param>
        /// <returns></returns>
        public DataTable GetPassCarTimesInfo(string where, string times, string rownum)
        {
            try
            {
                return Common.ChangColName(dal.GetPassCarTimesInfo(where, times, rownum));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="times"></param>
        /// <param name="rownum"></param>
        /// <returns></returns>
        public DataTable GetPassCarDangerInfo(string where, string times, string rownum)
        {
            try
            {
                return Common.ChangColName(dal.GetPassCarDangerInfo(where, times, rownum));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPassCarTimesCount(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPassCarTimesCount(where));
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
        /// <param name="rowwhere"></param>
        /// <returns></returns>
        public DataTable GetPassCarInfo(string where, string rowwhere)
        {
            try
            {
                return Common.ChangColName(dal.GetPassCarInfo(where, rowwhere));
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
        public DataTable GetPassCarInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPassCarInfo(where));
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
        public DataTable GetPassCarInfoMaxGwsj(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPassCarInfoMaxGwsj(where));
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
        public int GetPassCarInfoNum(string where)
        {
            try
            {
                return dal.GetPassCarInfoNum(where);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetPassCarTempCount(string where)
        {
            try
            {
                return dal.GetPassCarTempCount(where);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public DataTable GetPassCarTemp(string where, int startIndex, int endIndex)
        {
            try
            {
                return Common.ChangColName(dal.GetPassCarTemp(where, startIndex, endIndex));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 判断序号是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public int GeXhExist(string tableName, string fieldName, string fieldValue)
        {
            try
            {
                return dal.GeXhExist(tableName, fieldName, fieldValue);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetQueryTimeCount(string where)
        {
            try
            {
                return dal.GetQueryTimeCount(where);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetQueryTwoTimeList(string where)
        {
            try
            {
                return dal.GetQueryTwoTimeList(where);
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
        /// <param name="where"></param>
        /// <param name="queryList"></param>
        /// <param name="rownumList"></param>
        /// <returns></returns>
        public int GetPassCarInfoNum(Hashtable hs, string where, ref List<string> queryList, ref List<string> rownumList)
        {
            try
            {
                int rownum = 0;
                string kssj = hs["kssj"].ToString();
                string jssj = hs["jssj"].ToString();
                string where2 = "  gwsj >= STR_TO_DATE('" + kssj + "','%Y-%m-%d %H:%i:%s')   and gwsj<=STR_TO_DATE('" + jssj + "','%Y-%m-%d %H:%i:%s') and " + where;
                string whereMax = "  sgwsj >= STR_TO_DATE('" + kssj + "','%Y-%m-%d %H:%i:%s')   and egwsj<=STR_TO_DATE('" + jssj + "','%Y-%m-%d %H:%i:%s') and " + where;
                string maxGwsj = "";
                if (!hs.ContainsKey("hphm"))
                {
                    // maxGwsj = GetMaxGwsj2(whereMax);
                    maxGwsj = jssj;
                }

                if (!string.IsNullOrEmpty(maxGwsj))
                {
                    if (DateTime.Parse(jssj) > DateTime.Parse(maxGwsj))
                    {
                        jssj = DateTime.Parse(maxGwsj).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                if (hs.ContainsKey("hphm") || hs.ContainsKey("clsd"))
                {
                    rownum = GetPassCarInfoNum(where2);
                    int pages = rownum / 500;

                    for (int i = 0; i <= pages; i++)
                    {
                        int startnum = i * 500;
                        int endnum = (i + 1) * 500;
                        queryList.Add(where2);
                        rownumList.Add("where  rn<=" + endnum.ToString() + " and rn>" + startnum.ToString());
                    }
                    return rownum;
                }
                else
                {
                    string where3 = "  sgwsj >= STR_TO_DATE('" + kssj + "','%Y-%m-%d %H:%i:%s')   and egwsj<=STR_TO_DATE('" + jssj + "','%Y-%m-%d %H:%i:%s') and " + where;
                    DataTable dt = dal.GetQueryTime(where3);

                    //int count1 = 0;
                    //int count2 = 0;
                    string sdate1 = "";
                    string edate1 = "";
                    string sdate2 = "";
                    string edate2 = "";
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            sdate1 = kssj;

                            if (!string.IsNullOrEmpty(dt.Rows[0][1].ToString()))
                            {
                                edate1 = DateTime.Parse(dt.Rows[0][1].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                            {
                                queryList.Add(where2);
                                rownum = GetPassCarInfoNum(where2);
                                return rownum;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                            {
                                sdate2 = DateTime.Parse(dt.Rows[0][0].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                            {
                                queryList.Add(where2);
                                rownum = GetPassCarInfoNum(where2);
                                return rownum;
                            }

                            edate2 = jssj;
                            //count1 = GetPassCarInfoNum("  gwsj >= DATE_FORMAT('" + sdate1 + "','%Y-%m-%d %H:%i:%s')   and gwsj<=DATE_FORMAT('" + edate1 + "','%Y-%m-%d %H:%i:%s') and " + where);
                            //count2 = GetPassCarInfoNum("  gwsj > DATE_FORMAT('" + sdate2 + "','%Y-%m-%d %H:%i:%s')   and gwsj<=DATE_FORMAT('" + edate2 + "','%Y-%m-%d %H:%i:%s') and " + where);

                            DataTable dt3 = dal.GetQueryTimeList(where3);
                            string where4 = "";
                            if (dt3 != null)
                            {
                                sdate2 = kssj;
                                edate2 = jssj;
                                int countnum = 0;
                                for (int i = 0; i < dt3.Rows.Count; i++)
                                {
                                    countnum = countnum + int.Parse(dt3.Rows[i][2].ToString());
                                    edate1 = edate2;
                                    if (countnum > 500)
                                    {
                                        sdate1 = DateTime.Parse(dt3.Rows[i][1].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                                        edate2 = sdate1;
                                        where4 = "  gwsj >= STR_TO_DATE('" + sdate1 + "','%Y-%m-%d %H:%i:%s')   and gwsj<STR_TO_DATE('" + edate1 + "','%Y-%m-%d %H:%i:%s') and " + where;
                                        countnum = 0;
                                        queryList.Add(where4);
                                    }
                                }
                                where4 = "  gwsj >= STR_TO_DATE('" + kssj + "','%Y-%m-%d %H:%i:%s')   and gwsj<STR_TO_DATE('" + edate2 + "','%Y-%m-%d %H:%i:%s') and " + where;
                                queryList.Add(where4);
                            }

                            return int.Parse(dt.Rows[0][2].ToString());
                            //return count2 + int.Parse(dt.Rows[0][2].ToString()) + count1;
                        }
                    }
                    queryList.Add(where2);
                    return GetPassCarInfoNum(where2);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return 0;
            }
        }

        /// <summary>
        /// 获得过往车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPassCarInfoByHashtable(Hashtable hs, string where)
        {
            try
            {
                string kssj = hs["kssj"].ToString();
                string jssj = hs["jssj"].ToString();
                string rownum = hs["rownum"].ToString();
                int curpages = 0;
                if (hs.ContainsKey("curpages"))
                {
                    curpages = int.Parse(hs["curpages"].ToString());
                }
                int sidx = curpages * int.Parse(rownum) + 1;
                int eidx = (curpages + 1) * int.Parse(rownum);

                if (kssj.Substring(0, 10) == jssj.Substring(0, 10) || hs.ContainsKey("hphm"))
                {
                    string where2 = "  gwsj >= STR_TO_DATE('" + kssj + "','%Y-%m-%d %H:%i:%s')   and gwsj<=STR_TO_DATE('" + jssj + "','%Y-%m-%d %H:%i:%s') and " + where;

                    return Common.ChangColName(dal.GetPassCarInfo(where2, sidx, eidx));
                }
                else
                {
                    string where3 = GetMinGwsj(hs, where);
                    if (where3 == null)
                    {
                        where3 = "  gwsj >= STR_TO_DATE('" + kssj + "','%Y-%m-%d %H:%i:%s')   and gwsj<=STR_TO_DATE('" + jssj + "','%Y-%m-%d %H:%i:%s') and " + where;
                    }
                    return Common.ChangColName(dal.GetPassCarInfo(where3, sidx, eidx));
                }
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
        private string GetMaxGwsj(string where)
        {
            try
            {
                string maxGwsj = dal.GetPassCarString("max(gwsj)", where);
                return maxGwsj;
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
        /// <param name="where"></param>
        /// <returns></returns>
        public string GetMaxGwsj2(string where)
        {
            try
            {
                string maxGwsj = dal.GetPassCarMaxString("max(egwsj)", where);
                if (!string.IsNullOrEmpty(maxGwsj))
                {
                    return DateTime.Parse(maxGwsj).AddSeconds(1).ToString();
                }
                return maxGwsj;
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
        /// <param name="hs"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        private string GetMinGwsj(Hashtable hs, string where)
        {
            string kssj = hs["kssj"].ToString();
            string jssj = hs["jssj"].ToString();
            string maxGwsj = kssj;
            string minGwsj = jssj;
            try
            {
                int pageSize = int.Parse(hs["rownum"].ToString());
                int rownum = pageSize;
                if (hs.ContainsKey("curpages"))
                {
                    rownum = (int.Parse(hs["curpages"].ToString()) + 1) * pageSize;
                }
                string where2 = "  gwsj >= STR_TO_DATE('" + kssj + "','%Y-%m-%d %H:%i:%s')   and gwsj<=STR_TO_DATE('" + jssj + "','%Y-%m-%d %H:%i:%s') and " + where;
                maxGwsj = dal.GetPassCarString("max(gwsj)", where2);
                int crownum = 0;
                int i = 0;
                int addHour = 1;
                while (crownum < rownum)
                {
                    i++;
                    addHour = (int)Math.Pow(2, i);
                    minGwsj = DateTime.Parse(maxGwsj).AddHours(0 - (addHour)).ToString("yyyy-MM-dd HH:mm:ss");
                    where2 = "  gwsj >= STR_TO_DATE('" + minGwsj + "','%Y-%m-%d %H:%i:%s')   and gwsj<=STR_TO_DATE('" + maxGwsj + "','%Y-%m-%d %H:%i:%s') and " + where;
                    crownum = int.Parse(dal.GetPassCarString("count(1)", where2));
                    if (DateTime.Parse(minGwsj) < DateTime.Parse(kssj))
                    {
                        minGwsj = kssj;
                        break;
                    }
                    if (i > 10)
                    {
                        minGwsj = kssj;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
            return "  gwsj >= STR_TO_DATE('" + minGwsj + "','%Y-%m-%d %H:%i:%s')   and gwsj<=STR_TO_DATE('" + maxGwsj + "','%Y-%m-%d %H:%i:%s') and " + where;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAllPassCarInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetAllPassCarInfo("xh,kkmc,hphm, hpzl,hpzlms, to_char(gwsj,'%Y-%m-%d %H:%i:%s') as gwsj,ddbhms,fxmc,cdbh, clsd, clxs, jllxms, zjwj1, zjwj2,zjwj3", where));
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
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAllPassCarInfo(string field, string where)
        {
            try
            {
                return Common.ChangColName(dal.GetAllPassCarInfo(field, where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询图片地址
        /// </summary>
        /// <param name="xh"></param>
        /// <returns></returns>
        public DataTable GetPassCarImageUrl(string xh)
        {
            try
            {
                return Common.ChangColName(dal.GetPassCarImageUrl(xh));
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
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPassCarImageUrl(string xh, string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPassCarImageUrl(xh, where));
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
        /// <returns></returns>
        public DataTable GetPassCarMonitor()
        {
            try
            {
                return Common.ChangColName(dal.GetPassCarMonitor("1=1"));
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
        public DataTable GetPassCarMonitor(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPassCarMonitor(where));
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
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPasscarCount(string field, string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPasscarCount(field, where));
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
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="countType"></param>
        /// <returns></returns>
        public DataTable GetPeccancyCount(string field, string where, string countType)
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyCount(field, where, countType));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取平均速度流量信息
        /// </summary>
        /// <param name="directiones"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable PassCarAveSpeed(List<string> directiones, string date)
        {
            try
            {
                return GetFlowDataTable(directiones, date, "时", dal.PassCarAveSpeed, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取最高速度流量信息
        /// </summary>
        /// <param name="directiones"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable PassCarHighSpeed(List<string> directiones, string date)
        {
            try
            {
                return GetFlowDataTable(directiones, date, "时", dal.PassCarHighSpeed, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取识别率信息
        /// </summary>
        /// <param name="directiones"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable PassCarOcr(List<string> directiones, string date)
        {
            try
            {
                return GetFlowDataTable(directiones, date, "时", dal.PassCarOcr, 0);
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
        public DataTable GetPassCarTrackInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPassCarTrackInfo(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        #endregion 过往车辆信息

        #region 设备信息

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DataTable GetDeviceState()
        {
            try
            {
                return Common.ChangColName(dal.GetDeviceState());
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
        public DataTable GetDeviceState(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetDeviceState(where));
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
        public DataTable GetDeviceInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetDeviceInfo(where));
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
        public int DeleteDeviceInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteDeviceInfo(hs);
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
        public int UpdateDeviceInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateDeviceInfo(hs);
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
        public int UpdateDeviceStation(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateDeviceStation(hs);
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
        public int InsertDeviceInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.InsertDeviceInfo(hs);
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
        /// <returns></returns>
        public DataTable GetDevDeviceState()
        {
            try
            {
                return Common.ChangColName(dal.GetDevDeviceState());
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
        public DataTable GetDevDeviceState(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetDevDeviceState(where));
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
        public DataTable GetDevDeviceInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetDevDeviceInfo(where));
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
        public int DeleteDevDeviceInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteDevDeviceInfo(hs);
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
        public int UpdateDevDeviceInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateDevDeviceInfo(hs);
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
        public int InsertDevDeviceInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.InsertDevDeviceInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        #endregion 设备信息

        #region 报警信息

        /// <summary>
        /// 查询最新报警信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAlarmMonitor(string where, string rownum)
        {
            try
            {
                return Common.ChangColName(dal.GetAlarmMonitor(where, rownum));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询最新流量信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetFlowMonitor(string where, string rownum)
        {
            try
            {
                return Common.ChangColName(dal.GetFlowMonitor(where, rownum));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        public DataTable GetCarMonitor(string where, string rownum)
        {
            try
            {
                return Common.ChangColName(dal.GetAlarmMonitor(where, rownum));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 获得最大最小报警事件及报警总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAlarmMaxBjsj(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetAlarmMaxBjsj(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询最新的报警时间
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAlarmTempMaxBjsj(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetAlarmTempMaxBjsj(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询报警数据
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        public DataTable GetAlarmInfo(string where, int startrow, int endrow)
        {
            try
            {
                return Common.ChangColName(dal.GetAlarmInfo("*", where, startrow, endrow));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询流量报警数据
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        public DataTable GetFlowInfo(string where, int startrow, int endrow)
        {
            try
            {
                return Common.ChangColName(dal.GetFlowInfo("*", where, startrow, endrow));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询报警数据总数
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        public DataTable GetAlarmInfoCount(string where, int startrow, int endrow)
        {
            try
            {
                return Common.ChangColName(dal.GetAlarmInfoCount("*", where, startrow, endrow));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询流量报警数据总数
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        public DataTable GetFlowInfoCount(string where, int startrow, int endrow)
        {
            try
            {
                return Common.ChangColName(dal.GetFlowInfoCount("*", where, startrow, endrow));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得报警统计数据
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAlarmCount(string field, string where)
        {
            try
            {
                return Common.ChangColName(dal.GetAlarmCount(field, where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 处理报警信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DealAlarmInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DealAlarmInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 处理流量信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DealFlowInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DealFlowInfo(hs);
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
        public int DealAlarm_PeccancyInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DealAlarm_PeccancyInfo(hs);
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
        public int DealAlarm_PasscarInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DealAlarm_PasscarInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        #endregion 报警信息

        #region 布控车辆信息

        /// <summary>
        /// 查询布控车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSuspicion(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetSuspicion(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 删除布控车辆
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteSuspicionInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteSuspicionInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 更新布控车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateSuspicionInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateSuspicionInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        #endregion 布控车辆信息

        #region 畅行车辆信息

        /// <summary>
        /// 查询畅行车辆
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetCheckless(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetCheckless(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 删除白名单车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteChecklessInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteChecklessInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 更新畅行车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateChecklessInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateChecklessInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        #endregion 畅行车辆信息

        #region 黑名单车辆信息

        /// <summary>
        /// 获得黑名单信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetBalckList(string where, int num)
        {
            try
            {
                return Common.ChangColName(dal.GetblackList(where, num));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得黑名单信息总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetBalckListCount(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetblackListCount(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public int DeleteBlacklistInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteBlacklistInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int UpdateBlacklistInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateBlacklistInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        #endregion 黑名单车辆信息

        #region 专项布控信息

        /// <summary>
        /// 获得专项布控信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSpecial(string where, int num)
        {
            try
            {
                return Common.ChangColName(dal.Getspecial(where, num));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得专项布控信息总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSpecialCount(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetspecialCount(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public int DeleteSpecialInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteSpecialInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int UpdateSpecialInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateSpecialInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        #endregion 专项布控信息

        #region 流量报警信息

        /// <summary>
        /// 获得流量报警信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPort(string where, int num)
        {
            try
            {
                return Common.ChangColName(dal.Getport(where, num));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得流量报警信息总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPortCount(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetportCount(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public int DeletePortInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeletePortInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int UpdatePortInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdatePortInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///根据卡口编号查询卡口方向
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetKakouDirection(string StationId)
        {
            try
            {
                return dal.GetKakouDirection(StationId);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        #endregion 流量报警信息

        #region 特殊勤务

        /// <summary>
        /// 获得特殊勤务车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetExtraList(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetExtraList(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 删除特殊勤务车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteExtraListInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteExtraListInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 更新特殊勤务车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateExtraListInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateExtraListInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        #endregion 特殊勤务

        #region 违法车辆信息

        /// <summary>
        /// 获得指定条件中最大最小违法时间及总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyMaxWfsj(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyMaxWfsj(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得违法信息
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        public DataTable GetPeccancyInfo(string where, int startrow, int endrow)
        {
            try
            {
                string filed = @" xh, hpzl, F_TO_NAME('140001',hpzl) AS hpzlms, hphm, wfxw, IFNULL(f_get_wfxwms(wfxw),wfnr)  AS  wfxwms, wfsj, kkid, f_get_kkms(kkid) AS kkmc, cdbh, fxbh, F_TO_NAME('240025',fxbh) fxms,
CONCAT(CAST(clsd AS CHAR),'/',CAST( IFNULL( IFNULL(clxs,f_get_clxs(kkid,hpzl)),'0') AS CHAR)) , F_TO_NAME ('240022', IFNULL(sjly,f_get_sjly(kkid))) AS sjlyms, f_get_departname(IFNULL(cjjg,f_get_cjjg(wfdd)))  AS  cjjgms, F_TO_NAME('240007',shbj)  AS  shbjms,
F_TO_NAME('240008',tzbj)  AS tzbjms, F_TO_NAME('240009',cfbj)  AS cfbjms, F_TO_NAME('240020',bdydbj)  AS  bdydbjms, F_TO_NAME('240021',csbj)  AS  csbjms, F_TO_NAME('240019',jcbj)  AS jcbjms, '' AS  bdbjms, zqmj,
zjwj1, zjwj2, zjwj3, zjwj4, clsd, clxs, cjyh, shbj, jzzt, jzyh, jzsj, jdssyr, dh, lxfs, zsxxdz, zsxzqh, clpp, cllx, '' AS  cllxms, csys, '' AS csysms, zt, '' AS ztms, jyyxqz, yzbm, fdjh, clxh, clsbdh, syxz, '' AS syxzms, shyh, wfdd, IFNULL(f_get_ddms(wfdd),wfdz) AS wfdz ";
                return Common.ChangColName(dal.GetPeccancyInfo(filed, where, startrow, endrow));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得违法信息总记录
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        public DataTable GetPeccancyInfoCount(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyInfoCount(where));
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
        /// <param name="times"></param>
        /// <param name="rownum"></param>
        /// <returns></returns>
        public DataTable GetPeccancyTimesInfo(string where, string times, string rownum)
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyTimesInfo(where, times, rownum));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyAreaMaxWfsj(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyAreaMaxWfsj(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyAreaMaxWfsjCount(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyAreaMaxWfsjCount(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询区间违法信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        public DataTable GetPeccancyAreaInfo(string where, int startrow, int endrow)
        {
            try
            {
                string filed = "id,oxh,hphm,hpzl,hpzlms,fxbh,fxbhms,cdbh,wfxw,wfxwms,qjjl,qjxs,qjys,xssd, sdxs, kskkid, kskkmc, jskkid, jskkmc, wfkssj, wfjssj, wfsjd, shbjms, shyh, shsj, csbjms,csbl, zcys, jcbjms, imgurl1,imgurl2,shbj,wfdd,wfdz";

                return Common.ChangColName(dal.GetPeccancyAreaInfo(filed, where, startrow, endrow));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 统计区间违法信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyAreaCount(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyAreaCount(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 统计区间违法信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyAreaCountNew(string where, int startNum, int endNum)
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyAreaCountNew(where, startNum, endNum));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 统计区间违法信息总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyAreaCountCount(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyAreaCountCount(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///  区间行驶速度统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyAreaCountForXssd(string where)
        {
            try
            {
                return dal.GetPeccancyAreaCountForXssd(where);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 区间违法行为统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyAreaCountForWfxw(string where)
        {
            try
            {
                return dal.GetPeccancyAreaCountForWfxw(where);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetPeccancyWorkNum(string field, string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyWorkNum(field, where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public int UnlockOneInfo(string xh)
        {
            try
            {
                return dal.UnlockOneInfo(xh);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 将相对应的违法记录进行加锁
        /// </summary>
        /// <param name="where">加锁条件</param>
        /// <param name="sdr">锁定人</param>
        /// <param name="sdsj">锁定时间</param>
        /// <param name="lockAmount">锁定条数</param>
        /// <returns></returns>
        public int LockPeccancy(string where, string sdr, string sdsj, int lockAmount)
        {
            try
            {
                return dal.LockPeccancy(where, sdr, sdsj, lockAmount);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 将超过1个小时未解锁或者自己的的违法记录全部解锁
        /// </summary>
        /// <param name="sdsj"></param>
        /// <param name="sdr"></param>
        /// <returns></returns>
        public int UnAlllockAll(string sdsj, string sdr)
        {
            try
            {
                return dal.UnAlllockAll(sdsj, sdr);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int UnlockAll(string sdsj, string sdr)
        {
            try
            {
                return dal.UnlockAll(sdsj, sdr);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 对自己查询的信息进行加锁
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sdr"></param>
        /// <param name="sdsj"></param>
        /// <returns></returns>
        public int LockPeccancy(string where, string sdr, string sdsj)
        {
            try
            {
                return dal.LockPeccancy(where, sdr, sdsj);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int InsertPeccancyInfo(Hashtable hs)
        {
            try
            {
                return dal.InsertPeccancyInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 更新违法记录
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdatePeccancyInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdatePeccancyInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 删除违法记录
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public int DeletePeccancyInfo(List<string> records)
        {
            try
            {
                return dal.DeletePeccancyInfo(records);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int InsertCaptureInfo(Hashtable hs)
        {
            try
            {
                return dal.InsertCaptureInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int PeccancyOnly(Hashtable hs)
        {
            try
            {
                return dal.PeccancyOnly(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        #endregion 违法车辆信息

        #region 流量查询方法

        /// <summary>
        /// 获得流量方法
        /// </summary>
        /// <param name="directiones">所要统计的方向</param>
        /// <param name="flowDate">所要统计的日期</param>
        /// <param name="flowType">所要统计的类型</param>
        /// <returns></returns>
        public DataTable GetFlow(List<string> directiones, string flowDate, string flowType)
        {
            try
            {
                if (directiones.Count < 1 || string.IsNullOrEmpty(flowDate) || string.IsNullOrEmpty(flowType))
                {
                    return null;
                }

                DataTable dtReturn = new DataTable();
                switch (flowType)
                {
                    //case "1":
                    //case "2":
                    case "0":
                        {
                            dtReturn = this.GetHourFlowDataTable(directiones, flowDate);
                            break;
                        }
                    case "1": //day flow
                        {
                            dtReturn = this.GetDayFlowDataTable(directiones, flowDate);
                            break;
                        }
                    case "2": // week flow
                        {
                            dtReturn = this.GetWeekFlowDataTable(directiones, flowDate);
                            break;
                        }
                    case "3": //month flow
                        {
                            dtReturn = this.GetMonthFlowDataTable(directiones, flowDate);
                            break;
                        }
                    case "4": //year flow
                        {
                            dtReturn = this.GetYearFlowDataTable(directiones, flowDate);
                            break;
                        }
                }
                return dtReturn;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得流量方法
        /// </summary>
        /// <param name="directiones">所要统计的方向</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="flowType">流量类型</param>
        /// <returns></returns>
        public DataTable GetFlow(List<string> directiones, string startDate, string endDate, string flowType)
        {
            try
            {
                if (directiones.Count < 1 || string.IsNullOrEmpty(flowType))
                {
                    return null;
                }
                DataTable dtReturn = new DataTable();
                switch (flowType)
                {
                    //case "1":
                    //case "2":
                    //case "3":
                    case "0": //day flow
                        {
                            dtReturn = this.GetDayFlowDataTable(directiones, startDate, endDate);
                            break;
                        }
                    case "1": // week flow
                        {
                            dtReturn = this.GetWeekFlowDataTable(directiones, startDate, endDate);
                            break;
                        }
                    case "2": //month flow
                        {
                            dtReturn = this.GetMonthFlowDataTable(directiones, startDate, endDate);
                            break;
                        }
                    case "3": //year flow
                        {
                            dtReturn = this.GetYearFlowDataTable(directiones, startDate, endDate);
                            break;
                        }
                }
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
        /// 获得日流量数据
        /// </summary>
        /// <param name="directiones">方向集合</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        private DataTable GetDayFlowDataTable(List<string> directiones, string date)
        {
            try
            {
                return GetFlowDataTable(directiones, date, "日", dal.PassCarDayFlow, 1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得周流量数据
        /// </summary>
        /// <param name="directiones">方向集合</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        private DataTable GetWeekFlowDataTable(List<string> directiones, string date)
        {
            try
            {
                return GetFlowDataTable(directiones, date, "周", dal.PassCarWeekFlow, 1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得月流量数据
        /// </summary>
        /// <param name="directiones">方向集合</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        private DataTable GetMonthFlowDataTable(List<string> directiones, string date)
        {
            try
            {
                return GetFlowDataTable(directiones, date, "月", dal.PassCarMonthFlow, 1);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得年流量数据
        /// </summary>
        /// <param name="directiones">方向集合</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        private DataTable GetYearFlowDataTable(List<string> directiones, string date)
        {
            try
            {
                return GetFlowDataTable(directiones, date, "年", dal.PassCarYearFlow, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

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
                                    col = new DataColumn(dtFlow.Rows[j]["col1"].ToString() + colname, typeof(System.String));
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
                        row["方向名称"] = dtFlow.Rows[0]["col0"].ToString();
                        double count = 0;
                        for (int j = 0; j < dtFlow.Rows.Count; j++) //循环为新行赋值
                        {
                            row[j + 1] = dtFlow.Rows[j]["col2"].ToString();
                            count = count + double.Parse(dtFlow.Rows[j]["col2"].ToString());
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

        #endregion 流量查询方法

        #region 流量查询方法重载

        /// <summary>
        /// 获得周流量数据
        /// </summary>
        /// <param name="directiones">方向集合</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        private DataTable GetWeekFlowDataTable(List<string> directiones, string startDate, string endDate)
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
                    DataTable dtWeekFlow = Common.ChangColName(dal.PassCarWeekFlow(directiones[i], startDate, endDate));//根据方向编号获得流量数据

                    if (dtWeekFlow != null && dtWeekFlow.Rows.Count > 0)
                    {
                        if (!isAddedHead)//没加过head，添加列名称数据
                        {
                            for (int j = 1; j <= dtWeekFlow.Rows.Count; j++)  //初始化列名
                            {
                                col = new DataColumn(j + "周", typeof(System.String));
                                myDatatable.Columns.Add(col);
                            }
                            col = new DataColumn("总计", typeof(System.String));
                            myDatatable.Columns.Add(col);
                            isAddedHead = true;
                        }
                        row = myDatatable.NewRow();
                        row["方向名称"] = dtWeekFlow.Rows[i]["col0"].ToString();
                        double count = 0;
                        for (int j = 0; j < dtWeekFlow.Rows.Count; j++)//循环为新行赋值
                        {
                            row[j + 1] = dtWeekFlow.Rows[j]["col2"].ToString();
                            count = count + double.Parse(dtWeekFlow.Rows[j]["col2"].ToString());
                        }
                        row[dtWeekFlow.Rows.Count + 1] = count.ToString();
                        myDatatable.Rows.Add(row);
                    }
                    else
                    {
                        if (!isAddedHead)//没加过head，添加列名称数据
                        {
                            for (int j = 1; j < 52; j++)  //初始化列名
                            {
                                col = new DataColumn(j.ToString() + "周", typeof(System.String));
                                myDatatable.Columns.Add(col);
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
        /// 获得日流量数据
        /// </summary>
        /// <param name="directiones">方向集合</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        private DataTable GetDayFlowDataTable(List<string> directiones, string startDate, string endDate)
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
                    DataTable dtDayFlow = Common.ChangColName(dal.PassCarDayFlow(directiones[i], startDate, endDate));//根据方向编号获得流量数据

                    if (dtDayFlow != null && dtDayFlow.Rows.Count > 0)
                    {
                        if (!isAddedHead)//没加过head，添加列名称数据
                        {
                            for (int j = 1; j <= dtDayFlow.Rows.Count; j++)  //初始化列名
                            {
                                DateTime dtime = DateTime.Parse(dtDayFlow.Rows[j - 1][1].ToString());
                                if (dtime.Day == 1)
                                {
                                    col = new DataColumn(string.Format("{0:yy年M月d日}", dtime), typeof(System.String));
                                }
                                else
                                {
                                    col = new DataColumn(string.Format("{0:d日}", dtime), typeof(System.String));
                                }
                                myDatatable.Columns.Add(col);
                            }
                            col = new DataColumn("总计", typeof(System.String));
                            myDatatable.Columns.Add(col);
                            isAddedHead = true;
                        }
                        row = myDatatable.NewRow();
                        row["方向名称"] = dtDayFlow.Rows[i]["col0"].ToString();
                        double count = 0;
                        for (int j = 0; j < dtDayFlow.Rows.Count; j++)//循环为新行赋值
                        {
                            row[j + 1] = dtDayFlow.Rows[j]["col2"].ToString();
                            count = count + double.Parse(dtDayFlow.Rows[j]["col2"].ToString());
                        }
                        row[dtDayFlow.Rows.Count + 1] = count.ToString();
                        myDatatable.Rows.Add(row);
                    }
                    else
                    {
                        if (!isAddedHead)//没加过head，添加列名称数据
                        {
                            for (int j = 1; j < 31; j++)  //初始化列名
                            {
                                col = new DataColumn(j.ToString() + "日", typeof(System.String));
                                myDatatable.Columns.Add(col);
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
        /// 获得月流量数据
        /// </summary>
        /// <param name="directiones">方向集合</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        private DataTable GetMonthFlowDataTable(List<string> directiones, string startDate, string endDate)
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
                    DataTable dtMonthFlow = Common.ChangColName(dal.PassCarMonthFlow(directiones[i], startDate, endDate));//根据方向编号获得流量数据

                    if (dtMonthFlow != null && dtMonthFlow.Rows.Count > 0)
                    {
                        if (!isAddedHead)//没加过head，添加列名称数据
                        {
                            for (int j = 1; j <= dtMonthFlow.Rows.Count; j++)  //初始化列名
                            {
                                DateTime dtime = DateTime.Parse(dtMonthFlow.Rows[j - 1][1].ToString());
                                col = new DataColumn(string.Format("{0:yy年M月}", dtime), typeof(System.String));
                                myDatatable.Columns.Add(col);
                            }
                            col = new DataColumn("总计", typeof(System.String));
                            myDatatable.Columns.Add(col);
                            isAddedHead = true;
                        }
                        row = myDatatable.NewRow();
                        row["方向名称"] = dtMonthFlow.Rows[i]["col0"].ToString();
                        double count = 0;
                        for (int j = 0; j < dtMonthFlow.Rows.Count; j++)//循环为新行赋值
                        {
                            row[j + 1] = dtMonthFlow.Rows[j]["col2"].ToString();
                            count = count + double.Parse(dtMonthFlow.Rows[j]["col2"].ToString());
                        }
                        row[dtMonthFlow.Rows.Count + 1] = count.ToString();
                        myDatatable.Rows.Add(row);
                    }
                    else
                    {
                        if (!isAddedHead)//没加过head，添加列名称数据
                        {
                            for (int j = 1; j < 13; j++)  //初始化列名
                            {
                                col = new DataColumn(j.ToString() + "月", typeof(System.String));
                                myDatatable.Columns.Add(col);
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
        /// 获得年流量数据
        /// </summary>
        /// <param name="directiones">方向集合</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        private DataTable GetYearFlowDataTable(List<string> directiones, string startDate, string endDate)
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
                    DataTable dtYearFlow = Common.ChangColName(dal.PassCarYearFlow(directiones[i], startDate, endDate));//根据方向编号获得流量数据

                    if (dtYearFlow != null && dtYearFlow.Rows.Count > 0)
                    {
                        if (!isAddedHead)//没加过head，添加列名称数据
                        {
                            for (int j = 1; j <= dtYearFlow.Rows.Count; j++)  //初始化列名
                            {
                                col = new DataColumn(string.Format("{0:yyyy年}", DateTime.Parse(dtYearFlow.Rows[j - 1][1].ToString())), typeof(System.String));
                                myDatatable.Columns.Add(col);
                            }

                            col = new DataColumn("总计", typeof(System.String));
                            myDatatable.Columns.Add(col);
                            isAddedHead = true;
                        }
                        row = myDatatable.NewRow();
                        row["方向名称"] = dtYearFlow.Rows[i]["col0"].ToString();
                        double count = 0;
                        for (int j = 0; j < dtYearFlow.Rows.Count; j++)//循环为新行赋值
                        {
                            row[j + 1] = dtYearFlow.Rows[j]["col2"].ToString();
                            count = count + double.Parse(dtYearFlow.Rows[j]["col2"].ToString());
                        }
                        row[dtYearFlow.Rows.Count + 1] = count.ToString();
                        myDatatable.Rows.Add(row);
                    }
                    else
                    {
                        if (!isAddedHead)//没加过head，添加列名称数据
                        {
                            for (int j = 1; j < 11; j++)  //初始化列名
                            {
                                col = new DataColumn(j.ToString() + "年", typeof(System.String));
                                myDatatable.Columns.Add(col);
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
        /// 获得旅行时间分析
        /// </summary>
        /// <param name="directiones">方向集合</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public DataTable GetAreaODTable(List<string> qjids, string startDate, string colname, string countType)
        {
            int icol = 0;
            try
            {
                DataTable myDatatable = new DataTable("myDatatable");
                DataColumn col;
                DataRow row;

                col = new DataColumn("区间名称", typeof(System.String));//手动添加第一列
                myDatatable.Columns.Add(col);
                bool isAddedHead = false; //是否已经加过head行了

                for (int i = 0; i < qjids.Count; i++)
                {
                    DataTable dtFlow = Common.ChangColName(dal.AreaODCount(qjids[i], startDate, countType));//根据方向编号获得流量数据

                    if (dtFlow != null && dtFlow.Rows.Count > 0)
                    {
                        if (!isAddedHead)//没加过head，添加列名称数据
                        {
                            for (int j = 0; j < dtFlow.Rows.Count; j++)  //初始化列名
                            {
                                if (icol == 0)
                                {
                                    col = new DataColumn(dtFlow.Rows[j]["col1"].ToString() + colname, typeof(System.String));
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
                        row["区间名称"] = dtFlow.Rows[0]["col0"].ToString();
                        double count = 0;
                        for (int j = 0; j < dtFlow.Rows.Count; j++) //循环为新行赋值
                        {
                            row[j + 1] = dtFlow.Rows[j]["col2"].ToString();
                            count = count + double.Parse(dtFlow.Rows[j]["col2"].ToString());
                        }
                        row[dtFlow.Rows.Count + 1] = count.ToString();
                        myDatatable.Rows.Add(row);
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

        #endregion 流量查询方法重载

        #region 套牌分析

        public DataTable GetRepaclePlate(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetRepaclePlate(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public int UpdateRepaclePlate(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateRepaclePlate(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public DataTable AreaSpeedQuery(string date, string tim, string where)
        {
            try
            {
                return Common.ChangColName(dal.AreaSpeedQuery(date, tim, where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable AreaSpeedQuery(List<string> qjids, string date, string tim, string where)
        {
            try
            {
                string qjid = "";
                if (qjids != null && qjids.Count > 0)
                {
                    for (int i = 0; i < qjids.Count; i++)
                    {
                        qjid = qjid + "'" + qjids[i].ToString() + "',";
                    }
                    where = where + "  and xh in (" + qjid.Substring(0, qjid.Length - 1) + ")";
                }
                return Common.ChangColName(dal.AreaSpeedQuery(date, tim, where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得旅行时间分析
        /// </summary>
        /// <param name="directiones">方向集合</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public DataTable GetAreaSpeedTable(List<string> qjids, string startDate, string colname, string countType)
        {
            int icol = 0;
            try
            {
                DataTable myDatatable = new DataTable("myDatatable");
                DataColumn col;
                DataRow row;

                col = new DataColumn("区间名称", typeof(System.String));//手动添加第一列
                myDatatable.Columns.Add(col);
                bool isAddedHead = false; //是否已经加过head行了

                for (int i = 0; i < qjids.Count; i++)
                {
                    DataTable dtFlow = Common.ChangColName(dal.AreaSpeedCount(qjids[i], startDate, countType));//根据方向编号获得流量数据

                    if (dtFlow != null && dtFlow.Rows.Count > 0)
                    {
                        if (!isAddedHead)//没加过head，添加列名称数据
                        {
                            for (int j = 0; j < dtFlow.Rows.Count; j++)  //初始化列名
                            {
                                if (icol == 0)
                                {
                                    col = new DataColumn(dtFlow.Rows[j]["col1"].ToString() + colname, typeof(System.String));
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
                        row["区间名称"] = dtFlow.Rows[0]["col0"].ToString();
                        double count = 0;
                        for (int j = 0; j < dtFlow.Rows.Count; j++) //循环为新行赋值
                        {
                            row[j + 1] = dtFlow.Rows[j]["col2"].ToString();
                            count = count + double.Parse(dtFlow.Rows[j]["col2"].ToString());
                        }
                        row[dtFlow.Rows.Count + 1] = count.ToString();
                        myDatatable.Rows.Add(row);
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

        #endregion 套牌分析

        #region 区间违法审核

        public int UnlockAreaOneInfo(string xh)
        {
            try
            {
                return dal.UnlockAreaOneInfo(xh);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int LockAreaPeccancy(string where, string sdr, string sdsj, int lockAmount)
        {
            try
            {
                return dal.LockAreaPeccancy(where, sdr, sdsj, lockAmount);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int UnAlllockAreaAll(string sdsj, string sdr)
        {
            try
            {
                return dal.UnAlllockAreaAll(sdsj, sdr);
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
        /// <param name="sdsj"></param>
        /// <param name="sdr"></param>
        /// <returns></returns>
        public int UnlockAreaAll(string sdsj, string sdr)
        {
            try
            {
                return dal.UnlockAreaAll(sdsj, sdr);
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
        /// <param name="sdr"></param>
        /// <param name="sdsj"></param>
        /// <returns></returns>
        public int LockAreaPeccancy(string where, string sdr, string sdsj)
        {
            try
            {
                return dal.LockAreaPeccancy(where, sdr, sdsj);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 更新区间违法数据
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateAreaPeccancyInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.CheckAreaPeccancyInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        #endregion 区间违法审核

        /// <summary>
        /// 生成tgs记录ID
        /// </summary>
        /// <returns></returns>
        public string GetTgsRecordId()
        {
            try
            {
                string RegionId = DateTime.Now.ToString("yyyydd");
                return systemManager.GetRecordID(RegionId, 12);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "";
            }
        }

        /// <summary>
        ///  生成user记录ID
        /// </summary>
        /// <returns></returns>
        public string GetUserId()
        {
            try
            {
                return systemManager.GetRecordID("", 6);
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
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="Kkid"></param>
        /// <param name="fxbh"></param>
        /// <returns></returns>
        public DataTable GetVideoFilePlay(DateTime startTime, DateTime endTime, string Kkid, string fxbh)
        {
            Http http = new Http("", "");
            DataTable myDatatable = new DataTable("myDatatable");
            DataColumn col;
            DataRow row;
            col = new DataColumn("序号", typeof(System.String));//手动添加第一列
            myDatatable.Columns.Add(col);

            col = new DataColumn("监测点名称", typeof(System.String));//手动添加第一列
            myDatatable.Columns.Add(col);

            col = new DataColumn("行驶方向", typeof(System.String));//手动添加第一列
            myDatatable.Columns.Add(col);

            col = new DataColumn("记录时间", typeof(System.String));//手动添加第一列
            myDatatable.Columns.Add(col);

            col = new DataColumn("文件信息", typeof(System.String));//手动添加第一列
            myDatatable.Columns.Add(col);

            col = new DataColumn("文件路径", typeof(System.String));//手动添加第一列
            myDatatable.Columns.Add(col);
            string videopath = string.Empty;

            List<string> playList = new List<string>();
            DataTable dt = dal.GetHttpPath(Kkid, fxbh);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    videopath = dt.Rows[0][4].ToString();

                    TimeSpan ts = endTime.Subtract(startTime).Duration();

                    Double douLen = ts.TotalHours;

                    string dirpath = string.Empty;
                    for (int i = 0; i < (int)douLen; i++)
                    {
                        startTime = startTime.AddHours(i);
                        dirpath = "/" + startTime.ToString("yyyyMMdd") + "/" + startTime.ToString("HH") + "/";
                        string[] videoFileList = http.GetFilesAndDirs(videopath + dirpath);
                        if (videoFileList != null)
                        {
                            if (videoFileList.Length > 0)
                            {
                                for (int n = 0; n < videoFileList.Length; n++)
                                {
                                    playList.Add(videopath + dirpath + videoFileList[n]);
                                }
                            }
                        }
                    }
                    for (int j = 0; j < playList.Count; j++)
                    {
                        row = myDatatable.NewRow();
                        row["序号"] = j + 1;
                        row["监测点名称"] = dt.Rows[0][6].ToString();
                        row["行驶方向"] = dt.Rows[0][8].ToString();
                        string filename = Path.GetFileName(playList[j]);
                        string[] filenames = filename.Split('_');
                        if (filenames.Length > 5)
                        {
                            row["记录时间"] = DateTime.ParseExact(filenames[2] + filenames[3] + filenames[4] + "00", "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString();
                        }
                        else
                        {
                            row["记录时间"] = DateTime.Now.ToString();
                        }
                        row["文件信息"] = filename;
                        row["文件路径"] = playList[j];
                        myDatatable.Rows.Add(row);
                    }
                }
            }

            return Common.ChangColName(myDatatable);
        }
    }
}