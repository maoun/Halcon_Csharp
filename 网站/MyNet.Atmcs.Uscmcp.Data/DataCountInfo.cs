using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using System;
using System.Data;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class DataCountInfo : IDataCountInfo
    {
        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();
        private Common common = new Common();
        private MyNet.Common.Data.DataAccess dataAccess;

        public DataCountInfo()
        {
            //dataAccess = dac.GetDataAccess(WebConfig.DataAccessName);
            dataAccess = GetDataAccess.Init();
        }

        /// <summary>
        /// 根据日期统计过车数据
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public string GetPassCarCountDay(string datetime)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select CONVERT(IFNULL(SUM(ll),'0'),DECIMAL) AS ll from t_tgs_flow_day_count  where   rq=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d')";
                return dataAccess.Get_DataString(mySql, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return "0";
            }
        }

        public string GetOnlineCarCountDay(string datetime)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select CONVERT(IFNULL(SUM(ll),'0'),DECIMAL) AS ll from t_tgs_online_day  where  rq=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d')";
                return dataAccess.Get_DataString(mySql, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return "0";
            }
        }

        public string GetPassCarCountWeek(string week, string year)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select CONVERT(IFNULL(SUM(ll),'0'),DECIMAL) AS ll from t_tgs_flow_day_count  where  DATE_FORMAT(rq, '%U')=" + week + "  and  DATE_FORMAT(rq, '%Y')='" + year + "'";
                return dataAccess.Get_DataString(mySql, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return "0";
            }
        }

        public string GetPassCarCountMonth(string datetime)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select CONVERT(IFNULL(SUM(ll),'0'),DECIMAL) AS ll from t_tgs_flow_day_count  where   DATE_FORMAT(rq,'%Y-%m') =DATE_FORMAT('" + datetime + "', '%Y-%m')";
                return dataAccess.Get_DataString(mySql, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return "0";
            }
        }

        /// <summary>
        /// 获得过车小时流量统计
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public System.Data.DataTable GetPassCarCountHour(string datetime)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                mySql.Append("  select  time_id AS hour1, CONVERT(IFNULL(ll,'0'),DECIMAL) as ll from ");
                mySql.Append("  (select xs,SUM(ll) AS ll from t_tgs_flow_hour_count t where rq=STR_TO_DATE('" + datetime + "','%Y-%m-%d')  GROUP BY xs) a RIGHT JOIN");
                mySql.Append("  t_cfg_time b ON a.xs = b.time_id WHERE time_type = '1' ORDER BY hour1 ASC");
                ILog.WriteErrorLog(mySql.ToString());
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得在线过车小时流量统计
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public DataTable GetOnlineCarCountHour(string datetime)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                mySql.Append("  select  time_id AS hour1, CONVERT(IFNULL(ll,'0'),DECIMAL) as ll from ");
                mySql.Append("  (select xs,SUM(ll) AS ll from t_tgs_online_hour t where rq=STR_TO_DATE('" + datetime + "','%Y-%m-%d')  GROUP BY xs) a RIGHT JOIN");
                mySql.Append("  t_cfg_time b ON a.xs = b.time_id WHERE time_type = '1' ORDER BY hour1 ASC");
                ILog.WriteErrorLog(mySql.ToString());
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 根据时间及类型进行过车统计
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetPassCarCountByType(string datetime, string type)
        {
            string mySql = string.Empty;
            try
            {
                switch (type)
                {
                    case "HPZL":
                        mySql = "SELECT  f_to_name ('140001', hpzl) as hpzlms,CONVERT(IFNULL(sum(zs),'0'),DECIMAL) as zs FROM t_tgs_passcar_count_day WHERE  rq=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d') GROUP BY hpzl  order by zs desc";
                        break;

                    case "FZJG":
                        mySql = "SELECT * FROM (SELECT  bdydbj,CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) AS zs FROM t_tgs_passcar_count_day WHERE  rq=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d')  GROUP BY bdydbj)   d,t_cfg_sysconfig e   WHERE SUBSTR(bdydbj,1,1) =  SUBSTR(configvalue,1,1) AND configid='06' ORDER BY zs DESC  LIMIT 0,10 ";
                        break;

                    case "WDFZJG":
                        mySql = "select * from (select   substr(bdydbj,1,1) as bdydbj, convert(ifnull(sum(zs),'0'),decimal) as zs from t_tgs_passcar_count_day a   where rq=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d')   group by substr(bdydbj,1,1)) d,t_cfg_sysconfig e   where bdydbj!='无' and substr(bdydbj,1,1) !=  substr(configvalue,1,1) and configid='06' order by zs desc limit 0,10";
                        break;

                    case "BDFZJGHOT":
                        mySql = "SELECT f.*,h.*  FROM(SELECT d.* FROM (SELECT  bdydbj,kkid,CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) AS zs FROM t_tgs_passcar_count_day WHERE  rq=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d')  GROUP BY bdydbj,kkid)   d,t_cfg_sysconfig e   WHERE bdydbj =  configvalue AND configid='06' ) f ,t_gis_device_mark h WHERE f.kkid=h.relationid";
                        break;

                    case "BSFZJGHOT":
                        mySql = "SELECT f.*,h.*  FROM(SELECT d.* FROM (SELECT  bdydbj,kkid,CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) AS zs FROM t_tgs_passcar_count_day WHERE  rq=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d')  GROUP BY bdydbj,kkid)   d,t_cfg_sysconfig e   WHERE SUBSTR(bdydbj,1,1) =  SUBSTR(configvalue,1,1) AND configid='06' ) f ,t_gis_device_mark h WHERE f.kkid=h.relationid";
                        break;

                    case "WDFZJGHOT":
                        mySql = "SELECT f.*,h.*  FROM(SELECT d.* FROM (SELECT  bdydbj,kkid,CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) AS zs FROM t_tgs_passcar_count_day WHERE  rq=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d')  GROUP BY bdydbj,kkid)   d,t_cfg_sysconfig e   WHERE bdydbj!='无' and substr(bdydbj,1,1) !=  substr(configvalue,1,1) and configid='06' ) f ,t_gis_device_mark h WHERE f.kkid=h.relationid";
                        break;

                    case "BDFZJGTJ":
                        mySql = "SELECT  CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) AS zs FROM t_tgs_passcar_count_day    d,t_cfg_sysconfig e   WHERE  rq=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d')  AND  LOCATE(bdydbj, configvalue)>0 AND configid='06'  ";
                        break;

                    case "BSFZJGTJ":
                        mySql = "SELECT  CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) AS zs FROM t_tgs_passcar_count_day    d,t_cfg_sysconfig e   WHERE  rq=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d')  AND SUBSTR(bdydbj,1,1) =  remark  AND configid='06'  ";
                        break;

                    case "WDFZJGTJ":
                        mySql = "SELECT  CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) AS zs FROM t_tgs_passcar_count_day    d,t_cfg_sysconfig e   WHERE  rq=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d')  AND bdydbj!='无' and substr(bdydbj,1,1) !=  substr(configvalue,1,1) AND configid='06' ";
                        break;

                    case "CCRCTJ":
                        mySql = "SELECT  COUNT(*) FROM T_ANALYZE_FIRST WHERE  gwsj=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d')";
                        break;

                    case "TPTJ":
                        mySql = "SELECT  COUNT(*) FROM t_analyze_changeplate WHERE  gxsj=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d')";
                        break;

                    case "HMDTJ":
                        mySql = "SELECT  COUNT(*) FROM t_tgs_alarmed WHERE  bjsj=STR_TO_DATE('" + datetime + "', '%Y-%m-%d')";
                        break;

                    case "TJ300108":
                    case "TJ300109":
                    case "TJ300104":
                    case "TJ300102":
                    case "TJ300107":
                        mySql = "  SELECT bjlx, CONVERT(IFNULL(SUM(zs), '0'), DECIMAL) AS zs FROM t_tgs_alarm_count a WHERE a.bjsj =STR_TO_DATE( '" + datetime + "' , '%Y-%m-%d')   GROUP BY bjlx  ";
                        break;

                    // 黑名单
                    case "HPZLTJ300108":
                    //初次入城
                    case "HPZLTJ300109":
                    //套牌
                    case "HPZLTJ300104":
                    //逾期未年检车辆
                    case "HPZLTJ300102":
                    //黄标车
                    case "HPZLTJ300107":
                        mySql = "SELECT  f_to_name ('140001', hpzl) as hpzlms,CONVERT(IFNULL(sum(zs),'0'),DECIMAL) as zs FROM t_tgs_alarm_count WHERE  bjsj=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d')  AND bjlx = '" + type.Replace("HPZLTJ", "") + "'  GROUP BY hpzl  order by zs desc";
                        break;

                    // 黑名单
                    case "HOT300108":
                    //初次入城
                    case "HOT300109":
                    //套牌
                    case "HOT300104":
                    //逾期未年检车辆
                    case "HOT300102":
                    //黄标车
                    case "HOT300107":
                        mySql = "SELECT d.*, h.* FROM (SELECT kkid, CONVERT(IFNULL(SUM(zs), '0'), DECIMAL) AS zs FROM t_tgs_alarm_count WHERE bjsj =STR_TO_DATE( '" + datetime + "', '%Y-%m-%d')  AND bjlx = '" + type.Replace("HOT", "") + "' GROUP BY kkid) d, t_gis_device_mark h WHERE d.kkid = h.relationid";
                        break;
                }
                ILog.WriteErrorLog(type + mySql.ToString());
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        public System.Data.DataTable GetPassCarCountByKKid(string datetime)
        {
            return null;
        }

        public string GetPeccCarCountDay(string datetime)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) AS zs from t_tms_peccancy_count  where   wfsj=STR_TO_DATE('" + datetime + "', '%Y-%m-%d')";
                return dataAccess.Get_DataString(mySql, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return "0";
            }
        }

        public string GetPeccCarCountWeek(string week, string year)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) AS zs from t_tms_peccancy_count  where  DATE_FORMAT(wfsj, '%U')=" + week + "  and  DATE_FORMAT(wfsj, '%Y')='" + year + "'";
                return dataAccess.Get_DataString(mySql, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return "0";
            }
        }

        public string GetPeccCarCountMonth(string month)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) AS zs from t_tms_peccancy_count  where  DATE_FORMAT(wfsj,'%Y-%m') =DATE_FORMAT(CURDATE(), '%Y-%m') ";
                return dataAccess.Get_DataString(mySql, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return "0";
            }
        }

        public System.Data.DataTable GetPeccCarCountHour(string datetime)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                mySql.Append("  select  time_id AS hour1, CONVERT(IFNULL(zs,'0'),DECIMAL) as zs from ");
                mySql.Append("  (select xs,SUM(zs) AS zs from t_tms_peccancy_count t where wfsj=STR_TO_DATE('" + datetime + "','%Y-%m-%d')  GROUP BY xs) a RIGHT JOIN");
                mySql.Append("  t_cfg_time b ON a.xs = b.time_id WHERE time_type = '1' ORDER BY hour1 ASC");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        public System.Data.DataTable GetPeccCarCountByType(string datetime, string type)
        {
            string mySql = string.Empty;
            try
            {
                switch (type)
                {
                    case "WFXW":
                        mySql = "select *  from (SELECT b.wfxwjc, b.wfxw, CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) as zs FROM t_tms_peccancy_count a,t_tms_peccnacy_type b WHERE a.wfsj=STR_TO_DATE('" + datetime + "', '%Y-%m-%d')  and a.wfxw=b.wfxw GROUP BY b.wfxwjc  ) t1 ORDER  BY t1.zs DESC LIMIT 0,6 ";
                        break;

                    default:
                        break;
                }

                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        public DataTable GetPeccCarCountByWhere(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT a.wfxw, CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) AS zs FROM t_tms_peccancy_count a,t_cfg_set_station b WHERE a.wfdd=b.STATION_ID    AND  " + where + "  GROUP BY  a.wfxw  ORDER BY zs DESC ";
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        public System.Data.DataTable GetPeccCarCountAvgDay(string datetime, int countWeek)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                DateTime dt = DateTime.Parse(datetime);

                string where = string.Empty;
                for (int i = 0; i < countWeek; i++)
                {
                    dt = dt.AddDays(-7);
                    where = where + "STR_TO_DATE('" + dt.ToString("yyyy-MM-dd") + "', '%Y-%m-%d'),";
                }
                where = "(" + where.Substring(0, where.Length - 1) + ")";
                mySql.Append("  select  time_id AS hour1, CONVERT(IFNULL(zs/" + countWeek + ",'0'),DECIMAL) as zs from ");
                mySql.Append("  (select xs,SUM(zs) AS zs from t_tms_peccancy_count t where   wfsj in " + where + "  GROUP BY xs) a RIGHT JOIN");
                mySql.Append("  t_cfg_time b ON a.xs = b.time_id WHERE time_type = '1' ORDER BY hour1 ASC");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        public string GetAlarmCarCountDay(string datetime)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) AS zs from t_tgs_alarm_count  where   bjsj=STR_TO_DATE('" + datetime + "', '%Y-%m-%d')";
                return dataAccess.Get_DataString(mySql, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return "0";
            }
        }

        public System.Data.DataTable GetAlarmCarCountAvgDay(string datetime, int countWeek)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                DateTime dt = DateTime.Parse(datetime);

                string where = string.Empty;
                for (int i = 0; i < countWeek; i++)
                {
                    dt = dt.AddDays(-7);
                    where = where + "'" + dt.ToString("yyyy-MM-dd") + "',";
                }
                where = "(" + where.Substring(0, where.Length - 1) + ")";
                mySql.Append("  select  time_id AS hour1, CONVERT(IFNULL(zs/" + countWeek + ",'0'),DECIMAL) as zs from ");
                mySql.Append("  (select xs,SUM(zs) AS zs from t_tgs_alarm_count t where   DATE_FORMAT(bjsj,'%Y-%m-%d') in " + where + "  GROUP BY xs) a RIGHT JOIN");
                mySql.Append("  t_cfg_time b ON a.xs = b.time_id WHERE time_type = '1' ORDER BY hour1 ASC");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        public string GetAlarmCarCountWeek(string week, string year)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) AS zs from t_tgs_alarm_count  where  DATE_FORMAT(bjsj, '%U')=" + week + "  and  DATE_FORMAT(bjsj, '%Y')='" + year + "'";
                return dataAccess.Get_DataString(mySql, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return "0";
            }
        }

        public string GetAlarmCarCountMonth(string month)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) AS zs from t_tgs_alarm_count  where DATE_FORMAT(bjsj,'%Y-%m') =DATE_FORMAT(CURDATE(), '%Y-%m')";   //bjsj=STR_TO_DATE('" + month + "', '%Y-%m')";
                return dataAccess.Get_DataString(mySql, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return "0";
            }
        }

        public System.Data.DataTable GetAlarmCarCountHour(string datetime)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                mySql.Append("  select  time_id AS hour1, CONVERT(IFNULL(zs,'0'),DECIMAL) as zs from ");
                mySql.Append("  (select xs,SUM(zs) AS zs from t_tgs_alarm_count t where bjsj=STR_TO_DATE('" + datetime + "','%Y-%m-%d')  GROUP BY xs) a RIGHT JOIN");
                mySql.Append("  t_cfg_time b ON a.xs = b.time_id WHERE time_type = '1' ORDER BY hour1 ASC");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        public System.Data.DataTable GetAlarmCarCountByType(string datetime, string type)
        {
            string mySql = string.Empty;
            try
            {
                switch (type)
                {
                    case "BJLX":
                        mySql = "SELECT *  FROM (SELECT f_to_name ('300100', bjlx)  AS bjlx, CONVERT(IFNULL(SUM(zs),'0'),DECIMAL) AS zs FROM t_tgs_alarm_count a WHERE a.bjsj=STR_TO_DATE('" + datetime + "', '%Y-%m-%d')   GROUP BY a.bjlx  ) t1 ORDER  BY t1.zs DESC LIMIT 0,5";
                        break;

                    default:
                        break;
                }

                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        public System.Data.DataTable GetDeviceInfoByType(string datetime, string type)
        {
            string mySql = string.Empty;
            try
            {
                switch (type)
                {
                    case "CJJG":
                        mySql = "SELECT e.DEPARTNAME,ll  FROM t_cfg_department e,(SELECT  b.DEPARTID, CONVERT(IFNULL(SUM(ll),'0'),DECIMAL) AS ll  FROM      t_itgs_online_equipment a , t_cfg_set_station b WHERE a.KKID=b.STATION_ID and rq=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d') GROUP BY b.DEPARTID) c  WHERE e.DEPARTID=c.DEPARTID  ORDER  BY ll DESC LIMIT 0,5";
                        break;

                    case "CJ":
                        mySql = "         SELECT f.mode_name,SUM(ll)  FROM t_dev_device_infor c,t_dev_device_mode f ,(SELECT  b.DEVICE_ID, CONVERT(IFNULL(SUM(ll),'0'),DECIMAL) AS ll  FROM      t_itgs_online_equipment a ,";
                        mySql = mySql + " t_cfg_station_device b WHERE a.KKID=b.STATION_ID  and rq=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d') GROUP BY b.DEVICE_ID) e WHERE c.device_id=e.DEVICE_ID AND f.device_mode_id=c.device_mode_id  GROUP BY f.mode_name ORDER  BY ll DESC LIMIT 0,5";
                        break;

                    case "SBLB":
                        mySql = "SELECT c.device_name,f_to_name('240025',e.DIRECTION_ID) AS DIRECTION_ID,ll,'0',f_get_DEVICE_TYPE_NAME(c.device_type_id) AS device_type,f_get_mode_name(c.device_mode_id) AS mode_name,c.ipaddress,(CASE c.isuse WHEN '1' THEN '启用' ELSE '未启用' END) AS isuse  FROM t_dev_device_infor c,";
                        mySql = mySql + "(SELECT  b.DEVICE_ID,b.DIRECTION_ID, CONVERT(IFNULL(SUM(ll),'0'),DECIMAL) AS ll  FROM      t_itgs_online_equipment a , t_tgs_device_setting b  WHERE a.KKID=b.STATION_ID  AND rq=STR_TO_DATE('" + datetime + "' , '%Y-%m-%d') GROUP BY b.DEVICE_ID,b.DIRECTION_ID) e WHERE c.device_id=e.DEVICE_ID";
                        break;

                    case "YC"://异常统计
                        mySql = "SELECT SUM(CASE WHEN connect_state='0' THEN 1  ELSE 0 END ) AS 'networkAnomalies' ,SUM(CASE WHEN connect_state='1' AND work_state='410102' THEN 1  ELSE 0 END ) AS 'networkNormalDataAnomalies',SUM(CASE WHEN connect_state='1' AND work_state='410101' THEN 1  ELSE 0 END ) AS 'normal' FROM t_dev_device_state ";
                        break;

                    case "YCZS"://异常展示
                        mySql = @"SELECT CASE WHEN connect_state='1' AND work_state='410102' THEN '1' ELSE '0' END AS '设备状态',c.device_name,s.station_name,
                                F_TO_NAME('240006',b.connect_state) AS connect_state,
                                F_TO_NAME('410100',b.work_state) AS work_state,
                                f_get_DEVICE_TYPE_NAME(c.device_type_id) AS device_type,
                                f_get_mode_name(c.device_mode_id) AS mode_name
                                FROM t_dev_device_infor c
                                INNER JOIN t_dev_device_state b ON c.device_id =b.device_id
                                INNER JOIN  t_cfg_station_device d ON  c.device_id=d.device_id
                                INNER JOIN  t_cfg_set_station s ON s.station_id=d.station_id
                                WHERE b.work_state!='410101' ";
                        break;

                    default:
                        break;
                }

                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        public System.Data.DataTable GetPassCarHotSpot(string datetime, string type)
        {
            string mySql = string.Empty;
            try
            {
                switch (type)
                {
                    case "ZDDL":
                        //mySql = "SELECT * FROM t_tgs_passcar_hotspot_count WHERE gxsj> STR_TO_DATE('" + datetime + " 00:00:00', '%Y-%m-%d %H:%i:%s ')";
                        mySql = "SELECT a.dlid,a.dlmc,IFNULL(SUM(b.zs),0) zs,IFNULL( SUM(b.zs)/c.total*100 ,0) gwbl FROM (SELECT SUM(zs) total FROM t_tgs_passcar_Hotspot_count ) c, "
                + "(SELECT aa.*,ab.dlmc FROM T_GIS_KEYROAD_CONFIG aa LEFT JOIN t_gis_road ab ON aa.dlid=ab.dlbh) a LEFT JOIN "
                + "(SELECT kkid,SUM(zs) zs FROM t_tgs_passcar_Hotspot_count WHERE gxsj> STR_TO_DATE('" + datetime + " 00:00:00', '%Y-%m-%d %H:%i:%s ') GROUP BY kkid ) b ON a.kkid=b.kkid "
                + "GROUP BY a.dlid ORDER BY zs DESC";
                        break;

                    default:
                        break;
                }

                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }
    }
}