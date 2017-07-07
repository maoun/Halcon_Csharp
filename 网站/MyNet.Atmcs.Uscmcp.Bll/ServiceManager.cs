/***********************************************************************
 * Module:   目录业务逻辑类
 * Author:   李建平
 * Modified: 2008年10月17日
 * Purpose:  该类为页面提供需要的业务逻辑方法
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    [Serializable]
    public class ServiceManager
    {
        public struct AreaInfo
        {
            public string AreaId;
            public string AreaName;
            public string AreaType;
            public string WorkType;
            public string WorkClass;
            public string WorkClassSn;
            public DateTime WorkStartTime;
            public DateTime WorkEndTime;
            public int WorkPersonNum;
            public string WorkTime;
            public string DayWorkStartTime;
            public string DayWorkEndTime;
            public List<string> ListPerson;
            public DateTime DayTime;
        }

        /// <summary>
        ///  用户操作接口
        /// </summary>
        private static readonly IServiceManager dal = DALFactory.CreateServiceManager();
        /// <summary>
        /// 获得人员唯一编号usercode
        /// </summary>
        /// <returns></returns>
        public string GetRecordID()
        {
            try
            {
                return dal.GetRecordID("", 6);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 查询人员信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSevice(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetSevice(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 查询人员信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSeviceby(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetSeviceby(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        /// <summary>
        ///  添加人员
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int insertSevice(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.insertSevice(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }
        /// <summary>
        /// 更新人员信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int updateSevice(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.updateSevice(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }
        /// <summary>
        ///  删除警员信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteSevice(string id)
        {
            try
            {
                return dal.DeleteSevice(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }
        /// <summary>
        /// 获取重点车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetVehicles(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetVehicles(where));
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
        public DataTable GetVehiclese(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetVehiclese(where));
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
        public int insertVehicles(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.insertVehicles(hs);
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
        public int UpdateSevPerson(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateSevPerson(hs);
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
        public int UpdatePostandTime(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdatePostandTime(hs);
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
        public int updateVehicles(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.updateVehicles(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }
        /// <summary>
        /// 删除重点车辆信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteVehicles(string id)
        {
            try
            {
                return dal.DeleteVehicles(id);
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
        public int insertarea(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.insertarea(hs);
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
        public int insertarea_person(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.insertarea_person(hs);
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
        public int insertarea_person_beixuan(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.insertarea_person_beixuan(hs);
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
        public int insertarea_class(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.insertarea_class(hs);
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
        public int insertarea_time(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.insertarea_time(hs);
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
        public DataTable GetArea()
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetArea());
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
        public DataTable GetClass()
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetClass());
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
        public DataTable SlectArea(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.SlectArea(where));
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
        /// <returns></returns>
        public DataTable SlectAreaid(string id)
        {
            try
            {
                return Bll.Common.ChangColName(dal.SlectAreaid(id));
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
        /// <returns></returns>
        public DataTable SlectPersonid(string id)
        {
            try
            {
                return Bll.Common.ChangColName(dal.SlectPersonid(id));
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
        /// <returns></returns>
        public DataTable SlectPersonbakid(string id)
        {
            try
            {
                return Bll.Common.ChangColName(dal.SlectPersonbakid(id));
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
        public DataTable SlectPersontype(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.SlectPersontype(where));
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
        /// <returns></returns>
        public DataTable CountPersonbak(string id)
        {
            try
            {
                return Bll.Common.ChangColName(dal.CountPersonbak(id));
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
        /// <returns></returns>
        public DataTable CountPerson(string id)
        {
            try
            {
                return Bll.Common.ChangColName(dal.CountPerson(id));
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
        public DataTable areacount()
        {
            try
            {
                return Bll.Common.ChangColName(dal.AreaCount());
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
        /// <returns></returns>
        public DataTable CountClass(string id)
        {
            try
            {
                return Bll.Common.ChangColName(dal.CountClass(id));
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
        /// <returns></returns>
        public DataTable SlectCLASSid(string id)
        {
            try
            {
                return Bll.Common.ChangColName(dal.SlectCLASSid(id));
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
        /// <returns></returns>
        public DataTable CountTime(string id)
        {
            try
            {
                return Bll.Common.ChangColName(dal.CountTime(id));
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
        /// <returns></returns>
        public DataTable SlectTimeid(string id)
        {
            try
            {
                return Bll.Common.ChangColName(dal.SlectTimeid(id));
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
        /// <returns></returns>
        public DataTable Classtype(string id)
        {
            try
            {
                return Bll.Common.ChangColName(dal.Classtype(id));
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
        /// <returns></returns>
        public int DeleteArea(string id)
        {
            try
            {
                return dal.DeleteArea(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }
        /// <summary>
        /// 删除人员信息
        /// </summary>
        /// <param name="dutypost"></param>
        /// <param name="departid"></param>
        /// <returns></returns>
        public int DeletePerson(string dutypost, string departid)
        {
            try
            {
                return dal.DeletePerson(dutypost, departid);
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable Count_sev_Person(string id)
        {
            try
            {
                return Bll.Common.ChangColName(dal.SlectPersontype(id));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 获得部门人员总数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable PersonbyDepartid(string id)
        {
            try
            {
                return Bll.Common.ChangColName(dal.PersonbyDepartid(id));
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
        public DataTable GetSevPerson(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetSevPerson(where));
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
        public DataTable Postadtime(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.Postadtime(where));
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
        public int insertTrafficControl(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.insertTrafficControl(hs);
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
        public int updateTrafficControl(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.updateTrafficControl(hs);
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
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteTrafficControl(string id)
        {
            try
            {
                return dal.DeleteTrafficControl(id);
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
        public DataTable SeleteTrafficControl(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.SeleteTrafficControl(where));
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
        /// <returns></returns>
        public DataTable SeleteTrafficControlid(string id)
        {
            try
            {
                return Bll.Common.ChangColName(dal.SeleteTrafficControlid(id));
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
        public int insertJeeves(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.insertJeeves(hs);
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
        public int updateJeeves(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.updateJeeves(hs);
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
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteJeeves(string id)
        {
            try
            {
                return dal.DeleteJeeves(id);
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
        public DataTable SeleteJeeves(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.SeleteJeeves(where));
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
        /// <returns></returns>
        public DataTable SeleteJeevesid(string id)
        {
            try
            {
                return Bll.Common.ChangColName(dal.SeleteJeevesid(id));
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
        /// <param name="departid"></param>
        /// <param name="departName"></param>
        /// <param name="dutypost"></param>
        /// <param name="dutypostName"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<AreaInfo> GetAreaTimeInfo(string departid, string departName, string dutypost, string dutypostName, DateTime startTime, DateTime endTime)
        {
            List<AreaInfo> areaTimeList = new List<AreaInfo>();
            List<string> personList = new List<string>();
            string id = departid + dutypost;
            DataTable dt = GetSevPerson("dutypost='" + dutypost + "' and deptid='" + departid + "'");

            if (dt != null)
            {
                for (int h = 0; h < dt.Rows.Count; h++)
                {
                    string personName = dt.Rows[h]["col2"].ToString() + "|" + dt.Rows[h]["col7"].ToString();
                    personList.Add(personName);
                }
            }
            DataTable dt2 = Postadtime("dutypost='" + dutypost + "' and deptid='" + departid + "'");
            List<AreaInfo> areaInfoList = new List<AreaInfo>();
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                AreaInfo areaInfo = new AreaInfo();
                areaInfo.AreaId = dt2.Rows[i]["col0"].ToString();
                areaInfo.AreaName = dutypostName;
                areaInfo.WorkClassSn = (i + 1).ToString();
                areaInfo.WorkEndTime = DateTime.MaxValue;
                areaInfo.WorkStartTime = DateTime.Parse(DateTime.Now.AddYears(-3).ToString("yyyy-01-01"));
                areaInfo.WorkPersonNum = int.Parse(dt2.Rows[i]["col4"].ToString());

                string t1 = dt2.Rows[i]["col2"].ToString();
                string t2 = dt2.Rows[i]["col3"].ToString();
                string time = t1 + " ～ " + t2;
                areaInfo.WorkTime = "【" + time + "】";
                areaInfo.DayWorkStartTime = t1;
                areaInfo.DayWorkEndTime = t2;
                areaInfo.ListPerson = new List<string>();
                areaInfoList.Add(areaInfo);
            }
            DateTime strat = DateTime.Parse(DateTime.Now.AddYears(-3).ToString("yyyy-01-01"));
            DateTime nowdate = startTime;//选择的开始时间
            DateTime stopdate = endTime.AddDays(1);//选择的结束时间

            TimeSpan tsAll = stopdate.Subtract(nowdate);
            int countday = tsAll.Days;

            for (int d = 0; d < countday; d++)
            {
                int allclassCount = 0;
                DateTime Mindate = DateTime.MinValue;
                DateTime Maxdate = DateTime.MaxValue;
                for (int n = 0; n < areaInfoList.Count; n++)
                {
                    if (n == 0)
                    {
                        Mindate = areaInfoList[n].WorkStartTime;
                        Maxdate = areaInfoList[n].WorkEndTime;
                    }
                    allclassCount = allclassCount + areaInfoList[n].WorkPersonNum;
                    if (areaInfoList[n].WorkStartTime < Mindate)
                    {
                        Mindate = areaInfoList[n].WorkStartTime;
                    }
                    if (areaInfoList[n].WorkEndTime > Maxdate)
                    {
                        Maxdate = areaInfoList[n].WorkEndTime;
                    }
                }
                DateTime dtime = nowdate.AddDays(d);
                List<string> pOrderList = GetPerson(Mindate, dtime, allclassCount, personList);

                int idx = 0;
                for (int n = 0; n < areaInfoList.Count; n++)
                {
                    if (dtime <= areaInfoList[n].WorkEndTime && dtime >= areaInfoList[n].WorkStartTime)
                    {
                        AreaInfo areaInfo = new AreaInfo();
                        areaInfo.AreaId = areaInfoList[n].AreaId;
                        areaInfo.WorkTime = areaInfoList[n].WorkTime;
                        areaInfo.WorkPersonNum = areaInfoList[n].WorkPersonNum;
                        areaInfo.WorkClassSn = areaInfoList[n].WorkClassSn;
                        areaInfo.ListPerson = new List<string>();
                        areaInfo.DayTime = dtime;
                        for (int h = 0; h < areaInfo.WorkPersonNum; h++)
                        {
                            if (idx < pOrderList.Count)
                            {
                                areaInfo.ListPerson.Add(pOrderList[idx]);
                            }
                            idx++;
                        }
                        areaTimeList.Add(areaInfo);
                    }
                }
            }

            return areaTimeList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sDateTime"></param>
        /// <param name="cDateTime"></param>
        /// <param name="classPerson"></param>
        /// <param name="personList"></param>
        /// <returns></returns>
        private List<string> GetPerson(DateTime sDateTime, DateTime cDateTime, int classPerson, List<string> personList)
        {
            List<string> pList = new List<string>();
            try
            {
                TimeSpan ts = cDateTime.Subtract(sDateTime);
                int nday = ts.Days;

                if (classPerson > personList.Count)
                {
                    classPerson = personList.Count;
                }

                int idx = nday % personList.Count;

                string person = string.Empty;

                int ii = (idx * classPerson) % personList.Count;
                for (int i = 0; i < classPerson; i++)
                {
                    if (ii > personList.Count - 1)
                    {
                        ii = 0;
                    }
                    pList.Add(personList[ii]);
                    ii++;
                }
            }
            catch
            {
            }
            return pList;
        }
    }
}