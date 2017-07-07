using System;
using System.Collections.Generic;
using System.Data;
using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class StationManager : IStationManager
    {
        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        private MyNet.Common.Data.DataAccess dataAccess;

        public StationManager()
        {
            //dataAccess = dac.GetDataAccess(WebConfig.DataAccessName);
            dataAccess = GetDataAccess.Init();
        }

        public DataTable GetUserStationInfo(string userid, string shjb)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  a.*  from t_cfg_set_station  a, t_tms_user_station b   where  a.station_id=b.station_id  and b.check_times='" + shjb + "' and b.user_id='" + userid + "'";

                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public DataTable GetNoUserStationInfo(string shjb)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select a.*from t_cfg_set_station a ,t_cfg_set_station_type b where a.station_type_id=b.station_type_id and b.istmsshow='1' and a.station_id not in (select station_id from t_tms_user_station where check_times='" + shjb + "')";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 通过监测点编号获得配置的方向信息
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public DataTable GetStationTypeInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from t_cfg_set_station_type where " + where + " order by station_type_id asc";

                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// 通过监测点编号获得配置的方向信息
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public DataTable GetDirectionByTgs()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select rownum as rn,a.station_id, a.station_name ,a.station_type_id, f_to_name ('240026', a.station_type_id) as station_type,a.departid , f_get_value ('departname', 't_cfg_department', 'departid', a.departid) as department,a.location_id , b.direction_id, b.direction_name, b.direction_desc from t_cfg_set_station a, t_cfg_direction b where a.station_id = b.station_id and a.station_type_id in('02','04')";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得车道信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetLaneInfo()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  *  from t_tgs_set_line   ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得监测点信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetStationInfo()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  *  from t_cfg_set_station  order by station_name asc ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得监测点信息（带条件）
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetStationInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from t_cfg_set_station a, t_cfg_set_station_type b  where  1=1  and a.station_type_id=b.station_type_id  and  " + where + " order by station_name asc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 从视图获得监测点信息（带条件）
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetStationInfoView(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  * from vt_cfg_set_station  where  1=1  and " + where + " order by station_name asc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        private DataTable GetLocationStationByWhere(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select a.*,nvl(b.scount,'0') as scount,nvl(c.dcount,'0') as  dcount  from vt_cfg_location_station a,";
                mySql = mySql + "(select station_id,nvl(count(station_id),0) as scount from t_join_station_device  group by station_id ) b,";
                mySql = mySql + "(select station_id,nvl(count(station_id),0) as dcount from t_cfg_direction  group by station_id ) c ";
                mySql = mySql + " where b.station_id(+)=a.station_id and c.station_id(+)=a.station_id  and  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public int DeleteDeviceStation(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from  t_cfg_station_device  where station_id='" + hs["station_id"].ToString() + "'  and  device_id='" + hs["device_id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int InsertStationDevice(string stationid, List<string> deviceList)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from  t_tgs_station_device  where station_id='" + stationid + "'";
                dataAccess.Execute_NonQuery(mySql);
                int res = 0;
                for (int i = 0; i < deviceList.Count; i++)
                {
                    mySql = "insert into  t_tgs_station_device (station_id,device_id) values(";
                    mySql = mySql + "'" + stationid + "',";
                    mySql = mySql + "'" + deviceList[i] + "')";
                    res = res + dataAccess.Execute_NonQuery(mySql);
                }
                return res;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///更新监测点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateStationInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_cfg_set_station  set ";
                mySql = mySql + "station_name='" + hs["station_name"].ToString() + "',";
                mySql = mySql + Common.GetHashtableStr(hs, "station_id", "station_id");
                mySql = mySql + Common.GetHashtableStr(hs, "departid", "departid");
                mySql = mySql + Common.GetHashtableStr(hs, "station_type_id", "station_type_id");
                mySql = mySql + Common.GetHashtableStr(hs, "station_idext", "station_idext");
                mySql = mySql + Common.GetHashtableStr(hs, "location_id", "location_id");
                mySql = mySql + "description='" + hs["description"].ToString() + "'";
                mySql = mySql + " where id='" + hs["id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 插入监测点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="station_type_id"></param>
        /// <returns></returns>
        public int InsertStationInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (hs.ContainsKey("station_idext"))
                {
                    mySql = "insert into  t_cfg_set_station (id,station_id,station_name,station_type_id,departid,description,location_id,station_idext) values(";
                    mySql = mySql + "'" + hs["id"].ToString() + "',";
                    mySql = mySql + "'" + hs["station_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["station_name"].ToString() + "',";
                    mySql = mySql + "'" + hs["station_type_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["departid"].ToString() + "',";
                    mySql = mySql + "'" + hs["description"].ToString() + "',";
                    mySql = mySql + "'" + hs["location_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["station_idext"].ToString() + "')";
                }
                else
                {
                    mySql = "insert into  t_cfg_set_station (id,station_id,station_name,station_type_id,departid,description,location_id) values(";
                    mySql = mySql + "'" + hs["id"].ToString() + "',";
                    mySql = mySql + "'" + hs["station_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["station_name"].ToString() + "',";
                    mySql = mySql + "'" + hs["station_type_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["departid"].ToString() + "',";
                    mySql = mySql + "'" + hs["description"].ToString() + "',";
                    mySql = mySql + "'" + hs["location_id"].ToString() + "')";
                }
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 获得车道车道信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetLaneInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  * from t_tgs_set_lane  where  1=1  and " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 从视图中获得车道信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetLaneInfoView(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  * from vt_tgs_set_lane  where  1=1  and " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public int DeleteStationByLocation(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from t_cfg_set_station  where location_id='" + hs["location_id"].ToString() + "'";
                int res = dataAccess.Execute_NonQuery(mySql);

                return res;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 删除监测点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteStationInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from t_cfg_set_station  where id='" + hs["id"].ToString() + "'";
                int res = dataAccess.Execute_NonQuery(mySql);
                if (res > 0)
                {
                    mySql = "delete  from  t_tgs_set_lane  where station_id='" + hs["station_id"].ToString() + "'";
                    dataAccess.Execute_NonQuery(mySql);
                    mySql = "delete from  t_cfg_direction  where station_id='" + hs["station_id"].ToString() + "'";
                    dataAccess.Execute_NonQuery(mySql);
                    mySql = mySql = "delete  from t_tgs_station_device  where station_id='" + hs["station_id"].ToString() + "'";
                    dataAccess.Execute_NonQuery(mySql);
                    mySql = mySql = "delete  from t_tgs_device_setting  where station_id='" + hs["station_id"].ToString() + "'";
                    dataAccess.Execute_NonQuery(mySql);
                }
                return res;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 删除车道信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteLaneInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from t_tgs_set_lane  where id='" + hs["id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int DeleteLaneInfoByDirection(string station_id, string direction_id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete  t_tgs_set_lane  where station_id='" + station_id + "' and direction_id='" + direction_id + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 插入车道信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertLaneInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_tgs_set_lane (id,station_id,location_id,direction_id,lane_id,limit_type,big_limit_speed,small_limit_speed,big_limit_low_speed,small_limit_low_speed,reality_big_limit_speed,reality_small_limit_speed) values(";
                mySql = mySql + "'" + hs["id"].ToString() + "',";
                mySql = mySql + "'" + hs["station_id"].ToString() + "',";
                mySql = mySql + "'" + hs["location_id"].ToString() + "',";
                mySql = mySql + "'" + hs["direction_id"].ToString() + "',";
                mySql = mySql + "'" + hs["lane_id"].ToString() + "',";
                mySql = mySql + "'" + hs["limit_type"].ToString() + "',";
                mySql = mySql + "'" + hs["big_limit_speed"].ToString() + "',";
                mySql = mySql + "'" + hs["small_limit_speed"].ToString() + "',";
                mySql = mySql + "'" + hs["big_limit_low_speed"].ToString() + "',";
                mySql = mySql + "'" + hs["small_limit_low_speed"].ToString() + "',";
                mySql = mySql + "'" + hs["reality_big_limit_speed"].ToString() + "',";
                mySql = mySql + "'" + hs["reality_small_limit_speed"].ToString() + "' )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int InsertLaneInfo(System.Collections.Hashtable hs, int startLane, int endLane)
        {
            int res = 0;
            for (int i = startLane; i <= endLane; i++)
            {
                string mySql = string.Empty;
                try
                {
                    mySql = "insert into  t_tgs_set_lane (id,station_id,location_id,direction_id,lane_id,big_limit_speed,small_limit_speed,big_limit_low_speed,small_limit_low_speed,reality_big_limit_speed,reality_small_limit_speed) values(";
                    mySql = mySql + "lpad(seq_id.nextval,12,'0'),";
                    mySql = mySql + "'" + hs["station_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["location_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["direction_id"].ToString() + "',";
                    mySql = mySql + "'" + i.ToString() + "',";
                    mySql = mySql + "'" + hs["big_limit_speed"].ToString() + "',";
                    mySql = mySql + "'" + hs["small_limit_speed"].ToString() + "',";
                    mySql = mySql + "'" + hs["big_limit_low_speed"].ToString() + "',";
                    mySql = mySql + "'" + hs["small_limit_low_speed"].ToString() + "',";
                    mySql = mySql + "'" + hs["reality_big_limit_speed"].ToString() + "',";
                    mySql = mySql + "'" + hs["reality_small_limit_speed"].ToString() + "' )";
                    res = res + dataAccess.Execute_NonQuery(mySql);
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(mySql + ex.Message);
                    return -1;
                }
            }
            return res;
        }

        /// <summary>
        /// 更新车道信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateLaneInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_tgs_set_lane  set ";
                //mySql = mySql + Common.GetHashtableStr(hs, "location_id", "location_id");
                //mySql = mySql + Common.GetHashtableStr(hs, "direction_id", "direction_id");
                //mySql = mySql + Common.GetHashtableStr(hs, "device_id", "device_id");
                //mySql = mySql + Common.GetHashtableStr(hs, "lane_id", "lane_id");
                mySql = mySql + "big_limit_speed='" + hs["big_limit_speed"].ToString() + "',";
                mySql = mySql + "small_limit_speed='" + hs["small_limit_speed"].ToString() + "',";
                mySql = mySql + "big_limit_low_speed='" + hs["big_limit_low_speed"].ToString() + "',";
                mySql = mySql + "small_limit_low_speed='" + hs["small_limit_low_speed"].ToString() + "',";
                mySql = mySql + "reality_big_limit_speed='" + hs["reality_big_limit_speed"].ToString() + "',";
                mySql = mySql + "reality_small_limit_speed='" + hs["reality_small_limit_speed"].ToString() + "'";
                mySql = mySql + " where id='" + hs["id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int UpdateLaneInfo(string deviceid, string directionid)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_tgs_set_lane  set ";
                mySql = mySql + "device_id='" + deviceid + "'";
                mySql = mySql + " where direction_id='" + directionid + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 通过开始监测点获得结束监测点信息
        /// </summary>
        /// <param name="kskkid"></param>
        /// <returns></returns>
        public DataTable GetEndStationInfo(string kskkid)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select ttsw.station_id as kkid,ttsw.station_name as kkmc from t_tgs_pecarea_setting ttps,t_cfg_set_station ttsw where ttps.jskkid = ttsw.station_id  and  ttps.kskkid ='" + kskkid + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 配置区间测速结束监测点字典
        /// </summary>
        /// <param name="kskkid"></param>
        /// <returns></returns>
        public DataTable GetEndStationDict(string kskkid)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select station_id,station_name from t_cfg_set_station where  station_id <> '" + kskkid + "' and station_type_id in (02,03)  and " + " station_id not in " + " (select jskkid from t_tgs_pecarea_setting  " + " where kskkid = '" + kskkid + "') ";

                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 配置区间测速开始监测点字典
        /// </summary>
        /// <returns></returns>
        public DataTable GetStartStationInfo()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select kkid,kkmc from (select ttsw.station_id as kkid ,ttsw.station_name as kkmc from t_tgs_pecarea_setting ttps,t_cfg_set_station ttsw where ttps.kskkid = ttsw.station_id) group by kkid,kkmc ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 判断输入值是否在指定表存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        private int GeXhExist(string tableName, string fieldName, string fieldValue)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select count(1) from   " + tableName + "   where   " + fieldName + "  ='" + fieldValue + "'";
                return int.Parse(dataAccess.Get_DataString(mySql, 0));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 删除车道信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteUserStationInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from t_tms_user_station  where user_id='" + hs["user_id"].ToString() + "' and check_times='" + hs["check_times"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 插入车道信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertUserStationInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_tms_user_station (user_id,station_id,check_times) values(";
                mySql = mySql + "'" + hs["user_id"].ToString() + "',";
                mySql = mySql + "'" + hs["station_id"].ToString() + "' ,";
                mySql = mySql + "'" + hs["check_times"].ToString() + "' )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        #region IStationManager 成员

        public DataTable GetlocationByStationInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select distinct  t2.* from t_cfg_set_station  t1,t_cfg_location  t2  where t1.location_id=t2.location_id and " + where + " order by location_name asc ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public DataTable GetRecordPlayByStationInfo()
        {
            try
            {
                string mySql = "select station_id, station_name from (select a.* from t_cfg_set_station a, t_tgs_video_recordplay b where a.station_id = b.station_id and (a.station_type_id = '01' or a.station_type_id = '02') and b.isshow = '1') group by station_id, station_name order by station_name asc ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        #endregion IStationManager 成员
    }
}