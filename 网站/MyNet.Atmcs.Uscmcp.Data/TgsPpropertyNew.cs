using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class TgsPpropertyNew : ITgsPpropertyNew
    {
        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        private MyNet.Common.Data.DataAccess dataAccess;

        public TgsPpropertyNew()
        {
            //type=0 读取uscmcp数据库（老库） type=1 读取frame数据库（新库）
            string type = System.Configuration.ConfigurationManager.AppSettings["BkType"].ToString();
            if (type.Equals("0"))
            {
                dataAccess = GetDataAccess.Init();
            }
            else if (type.Equals("1"))
            {
                dataAccess = GetDataAccess.InitNew();
            }
            else
            {
                dataAccess = GetDataAccess.Init();
            }
        }

        public TgsPpropertyNew(string dataAccessName)
        {
            dataAccess = dac.GetDataAccess(dataAccessName);
        }

        #region ITgsPproperty 成员

        /// <summary>
        /// 获得用户分配的审核地点信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获得尚未分配用户审核的地点信息
        /// </summary>
        /// <param name="shjb"></param>
        /// <returns></returns>
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
        /// 查询方向信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDirectionInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  *  from t_cfg_direction  where  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 根据方向编号 获得方向名称
        /// </summary>
        /// <param name="direction_id"></param>
        /// <returns></returns>
        public string GetDirectionName(string direction_id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  codedesc  from t_sys_code  where code='" + direction_id + "' and codetype='240025'";
                return dataAccess.Get_DataString(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 获得方向字典信息(不带条件)
        /// </summary>
        /// <returns></returns>
        public DataTable GetDirectionInfo()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  *  from t_cfg_direction   ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 通过地点编号获得配置的监测点信息
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public DataTable GetStationInfoByLocation(string location_id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from t_cfg_set_station where location_id='" + location_id + "'";

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
        public DataTable GetDirectionInfoByStation(string stationId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select direction_id, direction_name,direction_desc ,station_id from t_cfg_direction where station_id='" + stationId + "'";

                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取检测点类型
        /// </summary>
        /// <param name="where"></param>
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

        /// <summary>
        /// 通过监测点编号获得配置的方向信息
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public DataTable GetDirectionInfoByStation2(string stationId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select  distinct  '0','全部' from dual  union select direction_id as code, direction_desc as codedesc from t_cfg_direction  where station_id='" + stationId + "'";
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
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDirectionInfoByWhere(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select  distinct  '0','全部' from dual  union select direction_id as code, direction_desc as codedesc from t_cfg_direction  where" + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stationType"></param>
        /// <returns></returns>
        public DataTable GetDeviceTypeByStation(string stationType)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select b.device_type_id,b.device_type_name from t_cfg_station_devtype a,t_dev_device_type  b where a.device_type_id=b.device_type_id and isuse='1'  and  station_type_id='" + stationType + "'  group by b.device_type_id,b.device_type_name ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得设备信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDeviceInfoByWhere(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select distinct t1.*, 0 AS isbind  from (SELECT a.device_id,a.device_name,a.device_type_id,b.device_type_name,a.device_mode_id,c.mode_name,a.ipaddress,a.port,a.device_idext FROM t_dev_device_infor a,t_dev_device_type b ,t_dev_device_mode c WHERE a.device_type_id=b.device_type_id  AND a.device_mode_id=c.device_mode_id and " + where + ") t1 where t1.device_id not in   (select distinct(device_id)   from    t_cfg_station_device)";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得设备信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDeviceInfoByStation(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select distinct t1.*,ifnull(b.device_id,0) as isbind  from (SELECT a.device_id,a.device_name,a.device_type_id,b.device_type_name,a.device_mode_id,c.mode_name,a.ipaddress,a.port,a.device_idext FROM t_dev_device_infor a,t_dev_device_type b ,t_dev_device_mode c WHERE a.device_type_id=b.device_type_id  AND a.device_mode_id=c.device_mode_id) t1 join t_cfg_station_device b on b.device_id=t1.device_id   and  " + where;
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
        public DataTable GetDirectionDeviceByStation(string stationId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select direction_id as code, direction_desc as codedesc from t_cfg_direction  where station_id='" + stationId + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得方向信息
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
        /// 查询所有监测点信息（道路关联）
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAllStationInfoWithRoad(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT t.*,IFNULL(b.kkid,0) AS isbind FROM";
                mySql += "(SELECT a.STATION_ID,a.STATION_NAME,b.STATION_TYPE_NAME FROM t_cfg_set_station a,t_cfg_set_station_type b";
                mySql += " WHERE a.station_type_id = b.station_type_id AND " + where + ") t";
                mySql += " LEFT JOIN t_gis_keyroad_config b ON t.STATION_ID = b.kkid ORDER BY t.station_name ASC";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取已存在的监测信息卡口
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetBindKkmc(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select *  from t_keycar_carinfo where 1=1  " + where + "";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 判断是时间规则是否存在
        /// </summary>
        /// <param name="BH"></param>
        /// <returns></returns>
        public bool ExistTimeRuleBh(string BH)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT SDBH FROM t_keycar_config_timerule WHERE SDBH='" + BH + "'";
                if (dataAccess.Get_DataTable(mySql).Rows.Count > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 通过时段编号获取配置编号和区域编号
        /// </summary>
        /// <param name="timeRuleBh"></param>
        /// <returns></returns>
        public DataTable GetBh(string timeRuleBh)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT pzbh,qybh FROM t_keycar_config WHERE sdbh='" + timeRuleBh + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取区域规则
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DataTable GetAreaRule(string time)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT CONVERT(a.qygz,char) as qygz from t_keycar_config b JOIN t_keycar_config_regionrule a ON a.QYBH = b.QYBH";
                mySql += " JOIN t_keycar_config_timerule c ON b.SDBH = c.SDBH WHERE b.TYPE = '400501' and c.sdbh = '" + time + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 根据卡口编号获取卡口名称
        /// </summary>
        /// <param name="Kkid"></param>
        /// <returns></returns>
        public string GetKkmcByKkid(string Kkid)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT station_name FROM t_cfg_set_station WHERE station_id='" + Kkid + "'";
                return dataAccess.Get_DataTable(mySql).Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 添加重点车辆时间规则信息
        /// </summary>
        /// <param name="sdbh"></param>
        /// <param name="xmlRule"></param>
        /// <returns></returns>
        public int insertTimerule(string sdbh, string xmlRule)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into t_keycar_config_timerule(SDBH,SJGZ) values('" + sdbh + "','" + xmlRule + "')";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 添加重点车辆区域规则信息
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="xmlRule"></param>
        /// <returns></returns>
        public int insertRegionrule(string qybh, string xmlRule)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into t_keycar_config_regionrule(QYBH,QYGZ) values('" + qybh + "','" + xmlRule + "')";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 添加重点车辆配置信息
        /// </summary>
        /// <param name="pzbh"></param>
        /// <param name="type"></param>
        /// <param name="sdbh"></param>
        /// <param name="qybh"></param>
        /// <param name="cfjg"></param>
        /// <returns></returns>
        public int insertConfig(string pzbh, string type, string sdbh, string qybh, string cfjg)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into t_keycar_config(PZBH,TYPE,SDBH,QYBH,CFJG) values('" + pzbh + "','" + type + "','" + sdbh + "','" + qybh + "','" + cfjg + "')";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 添加重点车辆类型与配置信息关联信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="zdcllx"></param>
        /// <param name="pzbh"></param>
        /// <returns></returns>
        public int insertRelation(string id, string zdcllx, string pzbh)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into t_keycar_relation(ID,ZDCLLX,PZBH) values('" + id + "','" + zdcllx + "','" + pzbh + "')";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 删除重点车辆时间规则信息
        /// </summary>
        /// <param name="sdbh"></param>
        /// <returns></returns>
        public int deleteTimerule(string sdbh)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete from t_keycar_config_timerule where SDBH='" + sdbh + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 删除重点车辆区域规则信息
        /// </summary>
        /// <param name="qybh"></param>
        /// <returns></returns>
        public int deleteRegionrule(string qybh)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete from t_keycar_config_regionrule where QYBH='" + qybh + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 删除重点车辆配置信息
        /// </summary>
        /// <param name="pzbh"></param>
        /// <returns></returns>
        public int deleteConfig(string pzbh)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_keycar_config where PZBH='" + pzbh + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 删除重点车辆类型与配置信息关联信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int deleteRelation(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_keycar_relation where PZBH='" + id + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 查找区域规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool selectRegionrule(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT qybh FROM t_keycar_config_regionrule WHERE qybh='" + id + "'";
                int i = dataAccess.Get_DataTable(mySql).Rows.Count;
                if (i > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return false;
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

        /// <summary>
        /// 通过机构编号获得下属的地点信息
        /// </summary>
        /// <param name="departId"></param>
        /// <returns></returns>
        public DataTable GetLocationByDepartId(string departId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  * from t_cfg_location  where  departid='" + departId + "'  order by location_name asc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetLocationStationByWhere(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select  id,  t1.station_id,  station_idext,  station_name,  station_type_id, station_type_name,location_id, location_name, departid, ifnull(dcount,0) as dcount,  ifnull(scount,0) as scount";
                mySql = mySql + " from (select  id,  station_id,  station_idext,station_name, a.station_type_id,b.location_id, b.location_name, c.station_type_name,a.departid  from t_cfg_set_station a,";
                mySql = mySql + " t_cfg_location b,   t_cfg_set_station_type c      where a.location_id = b.location_id          and c.station_type_id = a.station_type_id) t1";
                mySql = mySql + " left join (select      station_id,    count(station_id)  as dcount    from t_cfg_station_device             group by station_id) d on t1.station_id = d.station_id ";
                mySql = mySql + " left join (select       station_id,   count(station_id)  as scount    from t_cfg_direction        group by station_id) e on t1.station_id=e.station_id  where  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteDeviceStation(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from t_cfg_station_device  where station_id='" + hs["station_id"].ToString() + "'  and  device_id='" + hs["device_id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stationid"></param>
        /// <param name="deviceList"></param>
        /// <returns></returns>
        public int InsertStationDevice(string stationid, List<string> deviceList)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_cfg_station_device  where station_id='" + stationid + "'";
                dataAccess.Execute_NonQuery(mySql);
                int res = 0;
                for (int i = 0; i < deviceList.Count; i++)
                {
                    mySql = "insert into  t_cfg_station_device (station_id,device_id) values(";
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
        /// 保存重点道路关联监测点信息
        /// </summary>
        /// <param name="dlId"></param>
        /// <param name="stationList"></param>
        /// <returns></returns>
        public int InsertKeyroadStation(string id, string dlId, List<string> stationList)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "DELETE FROM t_gis_keyroad_config WHERE dlid='" + dlId + "'";
                dataAccess.Execute_NonQuery(mySql);
                int res = 0;
                for (int i = 0; i < stationList.Count; i++)
                {
                    mySql = "INSERT INTO t_gis_keyroad_config(id,dlid,kkid) values(";
                    mySql = mySql + "'" + id + i.ToString() + "',";
                    mySql = mySql + "'" + dlId + "',";
                    mySql = mySql + "'" + stationList[i] + "')";
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
        /// 获取监测点信息
        /// </summary>
        /// <param name="roadId"></param>
        /// <returns></returns>
        public DataTable GetStationInfoByRoad(string roadId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT a.STATION_ID,a.STATION_NAME,b.STATION_TYPE_NAME FROM t_cfg_set_station a,";
                mySql += "t_cfg_set_station_type b,t_gis_keyroad_config c WHERE c.kkid = a.STATION_ID";
                mySql += " AND b.STATION_TYPE_ID = a.STATION_TYPE_ID AND c.dlid ='" + roadId + "'";

                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取监测点类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetStationTypeInfo()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT STATION_TYPE_ID,STATION_TYPE_NAME FROM t_cfg_set_station_type WHERE ISUSE='1'";

                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
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
        /// 获得车道信息
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
                    mySql = "delete from t_tgs_set_lane  where station_id='" + hs["station_id"].ToString() + "'";
                    dataAccess.Execute_NonQuery(mySql);
                    mySql = "delete  from t_cfg_direction  where station_id='" + hs["station_id"].ToString() + "'";
                    dataAccess.Execute_NonQuery(mySql);
                    mySql = "delete  from t_cfg_station_device where station_id='" + hs["station_id"].ToString() + "'";
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
                mySql = "delete from t_tgs_set_lane  where id='" + hs["id"].ToString() + "'";
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
                mySql = "delete  from t_tgs_set_lane  where station_id='" + station_id + "' and direction_id='" + direction_id + "'";
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

        /// <summary>
        /// 插入车道信息
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="startLane"></param>
        /// <param name="endLane"></param>
        /// <returns></returns>
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
        /// 获得违法类型信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetPeccancyType(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  * from VT_TMS_PECCNACY_TYPE  where  " + where + "  order by sx desc ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得违法类型信息（新的）
        /// </summary>
        /// <returns></returns>
        public DataTable GetPeccancyTypeNew(string where, int startRow, int endRow)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  * from VT_TMS_PECCNACY_TYPE  where  " + where + "  order by sx desc limit  " + startRow + "," + (endRow - startRow).ToString() + "";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得违法类型信息总数
        /// </summary>
        /// <returns></returns>
        public DataTable GetPeccancyTypeCount(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  count(*) from VT_TMS_PECCNACY_TYPE  where  " + where + "  order by sx desc ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得违法类型信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetPeccancyTypeSetting(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  xh,csbl,name,clxs,f_to_name('140001',hpzl) as hpzl,wfxwid,wfxwms,isuse,f_to_name('240034',isuse) as isusems from t_tgs_peccancy_setting  where  " + where + "  order by CAST(xh AS SIGNED ) ASC ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
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
        public DataTable GetEndStationDict(string kkidList)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select station_id,station_name from t_cfg_set_station where  station_id  not in ( '" + kkidList + "') and station_type_id in (02,03)  and  station_id not in  (select jskkid from t_tgs_pecarea_setting   where kskkid = '" + kkidList + "') ";
                // mySql = "select station_id,station_name from t_cfg_set_station where station_id not in ("+kkidList+")";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 配置区间测速开始监测点信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetStartStationInfo()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select kkid,kkmc from (select ttsw.station_id as kkid ,ttsw.station_name as kkmc from t_tgs_pecarea_setting ttps,t_cfg_set_station ttsw where ttps.kskkid = ttsw.station_id)as a group by kkid,kkmc ";
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
        public int GeXhExist(string tableName, string fieldName, string fieldValue)
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
        /// 删除违法类型信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeletePeccancyType(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from t_tms_peccnacy_type  where xh='" + hs["xh"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新违法类型信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdatePeccancyType(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_tms_peccnacy_type", "xh", hs["xh"].ToString()) > 0)
                {
                    mySql = "update  t_tms_peccnacy_type  set ";
                    mySql = mySql + "wfxw='" + hs["wfxw"].ToString() + "',";
                    mySql = mySql + "wfxwms='" + hs["wfxwms"].ToString() + "',";
                    mySql = mySql + "yj='" + hs["yj"].ToString() + "',";
                    mySql = mySql + "kf='" + hs["kf"].ToString() + "',";
                    mySql = mySql + "fkje='" + hs["fkje"].ToString() + "',";
                    mySql = mySql + "wfxwjc='" + hs["wfxwjc"].ToString() + "',";
                    mySql = mySql + "isuse='" + hs["isuse"].ToString() + "'";
                    mySql = mySql + " where xh='" + hs["xh"].ToString() + "'";
                }
                else
                {
                    mySql = "insert into  t_tms_peccnacy_type (xh,wfxw,wfxwms,yj,kf,fkje,isuse,wfxwjc) values(";
                    mySql = mySql + "'" + hs["xh"].ToString() + "',";
                    mySql = mySql + "'" + hs["wfxw"].ToString() + "',";
                    mySql = mySql + "'" + hs["wfxwms"].ToString() + "',";
                    mySql = mySql + "'" + hs["yj"].ToString() + "',";
                    mySql = mySql + "'" + hs["kf"].ToString() + "',";
                    mySql = mySql + "'" + hs["fkje"].ToString() + "',";
                    mySql = mySql + "'" + hs["isuse"].ToString() + "',";
                    mySql = mySql + "'" + hs["wfxwjc"].ToString() + "')";
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
        /// 更新违法类型信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdatePeccancyTypeSetting(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_tgs_peccancy_setting  set ";
                mySql = mySql + Common.GetHashtableStr(hs, "wfxwid", "wfxwid");
                mySql = mySql + Common.GetHashtableStr(hs, "wfxwms", "wfxwms");
                mySql = mySql + Common.GetHashtableStr(hs, "isuse", "isuse");
                mySql = mySql + "xh='" + hs["xh"].ToString() + "'";
                mySql = mySql + " where xh='" + hs["xh"].ToString() + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        #endregion ITgsPproperty 成员

        #region 区间超速配置

        /// <summary>
        /// 获得区间违法配置
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyAreaSetting()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT a.QYBH,CONVERT(QYGZ,CHAR) FROM t_keycar_config_regionrule a,t_keycar_config b WHERE a.QYBH=b.QYBH AND b.TYPE='400502'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets the peccancy area setting.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyAreaSetting(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  * from vt_tgs_pecarea_setting  where   " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 删除区间配置信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteNewPeccancyAreaSetting(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from t_tgs_pecarea_setting  where xh='" + hs["xh"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新区间配置信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateNewPeccancyAreaSetting(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_tgs_pecarea_setting", "xh", hs["xh"].ToString()) > 0)
                {
                    mySql = "update  t_tgs_pecarea_setting  set ";
                    mySql = mySql + Common.GetHashtableStr(hs, "kskkid", "kskkid");
                    mySql = mySql + Common.GetHashtableStr(hs, "jskkid", "jskkid");
                    mySql = mySql + Common.GetHashtableStr(hs, "fxbh", "fxbh");
                    mySql = mySql + Common.GetHashtableStr(hs, "ispecc", "ispecc");
                    mySql = mySql + Common.GetHashtableStr(hs, "areaid", "areaid");
                    mySql = mySql + "qjjl='" + hs["qjjl"].ToString() + "',";
                    mySql = mySql + "xcqjds='" + hs["xcqjds"].ToString() + "',";
                    mySql = mySql + "xcqjgs='" + hs["xcqjgs"].ToString() + "',";
                    mySql = mySql + "dcqjds='" + hs["dcqjds"].ToString() + "',";
                    mySql = mySql + "dcqjgs='" + hs["dcqjgs"].ToString() + "'";
                    mySql = mySql + " where xh='" + hs["xh"].ToString() + "'";
                }
                else
                {
                    mySql = "insert into  t_tgs_pecarea_setting (xh,kskkid,jskkid,qjjl,xcqjds,xcqjgs,dcqjds,dcqjgs,fxbh,ispecc,areaid) values(";
                    mySql = mySql + "'" + hs["xh"].ToString() + "',";
                    mySql = mySql + "'" + hs["kskkid"].ToString() + "',";
                    mySql = mySql + "'" + hs["jskkid"].ToString() + "',";
                    mySql = mySql + "'" + hs["qjjl"].ToString() + "',";
                    mySql = mySql + "'" + hs["xcqjds"].ToString() + "',";
                    mySql = mySql + "'" + hs["xcqjgs"].ToString() + "',";
                    mySql = mySql + "'" + hs["dcqjds"].ToString() + "',";
                    mySql = mySql + "'" + hs["dcqjgs"].ToString() + "',";
                    mySql = mySql + "'" + hs["fxbh"].ToString() + "',";
                    mySql = mySql + "'" + hs["ispecc"].ToString() + "',";
                    mySql = mySql + "'" + hs["areaid"].ToString() + "')";
                }
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int DeletePeccancyAreaSetting(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "DELETE FROM t_keycar_config_regionrule WHERE qybh='" + hs["qybh"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新区间违法配置
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdatePeccancyAreaSetting(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            string xmlStr = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><road><roadseg>";
            xmlStr += "<kkid nextkkid=\"" + hs["jskkid"].ToString() + "\" qjjl=\"" + hs["qjjl"].ToString() + "\" xcxgs=\"" + hs["xcqjgs"].ToString() + "\" dcxgs=\"" + hs["dcqjgs"].ToString() + "\"";
            xmlStr += " xcxds=\"" + hs["xcqjds"].ToString() + "\" dcxds=\"" + hs["dcqjds"].ToString() + "\" fxbh=\"" + hs["fxbh"].ToString() + "\" sfwf=\"" + hs["ispecc"].ToString() + "\">" + hs["kskkid"].ToString() + "</kkid></roadseg></road>";
            try
            {
                if (GeXhExist("t_keycar_config_regionrule", "QYBH", hs["areaid"].ToString()) > 0)
                {
                    string deleteSql = "DELETE FROM t_keycar_config_regionrule WHERE qybh='" + hs["areaid"].ToString() + "'";
                    if (dataAccess.Execute_NonQuery(deleteSql) > 0)
                    {
                        mySql = "insert into t_keycar_config_regionrule(QYBH,QYGZ) values('" + hs["areaid"].ToString() + "','" + xmlStr + "')";
                        return dataAccess.Execute_NonQuery(mySql);
                    }
                    return -1;
                }
                else
                {
                    mySql = "insert into t_keycar_config_regionrule(QYBH,QYGZ) values('" + hs["areaid"].ToString() + "','" + xmlStr + "')";
                    if (dataAccess.Execute_NonQuery(mySql) > 0)
                    {
                        return InsertKeycar_Config(hs);
                    }
                    return -1;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新重点车辆超速配置
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertKeycar_Config(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "INSERT INTO t_keycar_config(pzbh,TYPE,sdbh,qybh,cfjg) VALUES('" + hs["areaid"].ToString() + "','" + hs["type"].ToString() + "'";
                mySql += ",'" + hs["sdbh"].ToString() + "','" + hs["areaid"].ToString() + "','" + hs["cfjg"].ToString() + "')";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 删除重点车辆超速配置
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        public int DeleteKeycar_Config(string configId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "DELETE FROM t_keycar_config WHERE QYBH='" + configId + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 查询区间违法配置
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAreaSetting(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  * from vt_tgs_peccancy_area_setting  where  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

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

        #endregion 区间超速配置

        #region ITgsPproperty 成员

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

        /// <summary>
        /// 删除用户分配的审核地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteUserStationInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_tms_user_station  where user_id='" + hs["user_id"].ToString() + "' and check_times='" + hs["check_times"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 插入用户分配的审核地点信息
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

        public DataTable GetUserCode(string depart, string checktimes)
        {
            try
            {
                string mySql = "select usercode from (select a.usercode from T_SER_PERSON a ,t_tms_user_station b where a.usercode=b.user_id and a.departid='" + depart + "' and b.check_times='" + checktimes + "') group by usercode";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        #endregion ITgsPproperty 成员

        /// <summary>
        /// 判断序号是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public int GetXhExist(string tableName, string fieldName, string fieldValue)
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
    }
}