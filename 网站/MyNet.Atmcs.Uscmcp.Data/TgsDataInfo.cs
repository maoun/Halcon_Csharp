using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class TgsDataInfo : ITgsDataInfo
    {
        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();
        private SettingManager settingManager = new SettingManager();
        private Common common = new Common();
        private MyNet.Common.Data.DataAccess dataAccess;

        /// <summary>
        ///
        /// </summary>
        public TgsDataInfo()
        {
            //dataAccess = dac.GetDataAccess(WebConfig.DataAccessName);
            dataAccess = GetDataAccess.Init();//得到一个连接数据库的对象
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataAccessName"></param>
        public TgsDataInfo(string dataAccessName)
        {
            dataAccess = dac.GetDataAccess(dataAccessName);
        }

        #region ITgsDataInfo 成员

        /// <summary>
        ///获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        public DataTable GetPassCarInfo(string where, int startrow, int endrow)
        {
            try
            {
                string rowwhere = "    where  rn<=" + endrow + " and rn >=" + startrow;
                string mySql = GetFieldString(where, rowwhere);
                return dataAccess.Get_DataTable(mySql);
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
        /// <param name="rowwhere"></param>
        /// <returns></returns>
        public DataTable GetPassCarInfo(string where, string rowwhere)
        {
            try
            {
                string mySql = GetFieldString(where, rowwhere);
                return dataAccess.Get_DataTable(mySql);
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
        public DataTable GetPassCarInfoMaxGwsj(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select max(gwsj),min(gwsj),count(*) from t_tgs_passcar  where " + where + " ";
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
        public int GetQueryTimeCount(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select sum(countnum) from  (select sgwsj,egwsj,200 as countnum from t_tgs_query   where  " + where + "  union select sgwsj,egwsj, countnum from t_tgs_query_temp   where  " + where + ")  ";
                return int.Parse(dataAccess.Get_DataString(mySql, 0));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetQueryTimeTemp(string where)
        {
            string mySql = string.Empty;
            try
            {
                string ssum = string.Empty;
                for (int i = 59; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        ssum = ssum + "sum(m" + i.ToString() + ") as m" + i.ToString();
                    }
                    else
                    {
                        ssum = ssum + "sum(m" + i.ToString() + ") as m" + i.ToString() + " ,";
                    }
                }
                mySql = "select *  from (select rq,xs, " + ssum + "  from t_tgs_querytemp  where " + where + "  group by rq,xs) order by rq,xs desc";
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
        public DataTable GetQueryTwoTimeList(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select sgwsj,egwsj from t_tgs_query_two  where   " + where + "  order by sgwsj desc ";
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
        /// <param name="rowwhere"></param>
        /// <returns></returns>
        private string GetFieldString(string where, string rowwhere)
        {
            string mySql = "  select xh,";
            mySql = mySql + "  kkid,f_get_value ('station_name', 't_cfg_set_station', 'station_id', kkid) as kkmc,hphm, ";
            mySql = mySql + "  hpzl,f_to_name ('140001', hpzl) as hpzlms,";
            mySql = mySql + "  gwsj,";
            mySql = mySql + "  ddbh,f_get_value ('location_name',  't_cfg_location',  'location_id',  ddbh)  as ddbhms,";
            mySql = mySql + "  fxbh,  IFNULL(f_get_value('direction_desc',  't_cfg_direction', 'station_id||direction_id', kkid || fxbh), f_to_name('240025', fxbh)) as fxbhms,";
            mySql = mySql + "  cdbh, clsd,";
            mySql = mySql + "  jllx, f_to_names ('240002', jllx) as jllxms,";
            mySql = mySql + "  sjly, f_to_name ('240022', sjly) as sjlyms, cjjg, ";
            mySql = mySql + "  f_get_value ('departname', 't_cfg_department', 'departid',  cjjg)  as cjjgms,bz,";
            mySql = mySql + "  hpys,f_to_name ('240001', hpys) as hpysms,";
            mySql = mySql + "  cllx, f_to_name ('140002', cllx) as cllxms,";
            mySql = mySql + "  f_get_value ( decode (hpzl, '01', 'big_limit_speed', 'small_limit_speed'),  't_tgs_set_lane', 'station_id', kkid ) as clxs,";
            mySql = mySql + "  to_char (gwsj, 'yyyy-mm-dd') as gwrq, to_char (gwsj, 'hh24:mi:ss') as gwsfm";
            mySql = mySql + "  from (select  xh,kkid,hphm,hpzl,hpys,gwsj,ddbh,";
            mySql = mySql + "  fxbh,cdbh,cllx,clsd,jllx,cjjg,sjly,bz,row_number() over(order by gwsj desc) rn from t_tgs_passcar   where " + where + " ) " + rowwhere + " order by gwsj desc";
            return mySql;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPassCarInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = GetFieldString(where, "");
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPassCarTimesInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = mySql + "  select kkid,f_get_value ('station_name', 't_cfg_set_station', 'station_id', kkid) as kkmc,hphm, ";
                mySql = mySql + "  hpzl,f_to_name ('140001', hpzl) as hpzlms,";
                mySql = mySql + "  fxbh, IFNULL(f_get_value('direction_desc',  't_cfg_direction', 'station_id||direction_id', kkid || fxbh), f_to_name('240025', fxbh)) as fxbhms,";
                mySql = mySql + "  to_char (gwsj, 'yyyy-mm-dd') as gwrq,times from (";
                mySql = mySql + "  select kkid,hphm,hpzl,fxbh,gwsj,times,row_number() over(order by gwsj desc) rn from (select  kkid,hphm,hpzl,fxbh,trunc(gwsj) as gwsj,count(*) as times";
                mySql = mySql + "   from t_tgs_passcar where ishp= '1' and hpzl!='99' group by kkid,hphm,hpzl,fxbh ,trunc(gwsj))  where " + where + " )  order by gwsj,times desc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="times"></param>
        /// <param name="rownum"></param>
        /// <returns></returns>
        public DataTable GetPassCarTimesInfo(string where, string times, string rownum)
        {
            string mySql = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(times))
                {
                    mySql = mySql + " select * from ( select kkid,f_get_value ('station_name', 't_cfg_set_station', 'station_id', kkid) as kkmc,hphm, ";
                    mySql = mySql + "  hpzl,f_to_name ('140001', hpzl) as hpzlms,";
                    mySql = mySql + "  sjly,f_to_name ('240022', sjly) as sjlyms,";
                    mySql = mySql + "  cjjg,f_get_value ('departname', 't_cfg_department', 'departid', cjjg) as cjjgms ,times from (";
                    mySql = mySql + "  select kkid,hphm,hpzl,sjly,cjjg,times from (select  kkid,hphm,hpzl,sjly,cjjg,count(*) as times";
                    mySql = mySql + "  from t_tgs_passcar where  " + where + " group by kkid,hphm,hpzl,sjly,cjjg)  where  hpzl!='99' and times>1 )  order by times desc) where rownum <=" + rownum + "";
                }
                else
                {
                    mySql = mySql + " select * from ( select kkid,f_get_value ('station_name', 't_cfg_set_station', 'station_id', kkid) as kkmc,hphm, ";
                    mySql = mySql + "  hpzl,f_to_name ('140001', hpzl) as hpzlms,";
                    mySql = mySql + "  sjly,f_to_name ('240022', sjly) as sjlyms,";
                    mySql = mySql + "  cjjg,f_get_value ('departname', 't_cfg_department', 'departid', cjjg) as cjjgms ,times from (";
                    mySql = mySql + "  select kkid,hphm,hpzl,sjly,cjjg,times from (select  kkid,hphm,hpzl,sjly,cjjg,count(*) as times";
                    mySql = mySql + "  from t_tgs_passcar where  " + where + " group by kkid,hphm,hpzl,sjly,cjjg)  where  hpzl!='99' and times='" + times + "' )  order by times desc) where rownum <=" + rownum + "";
                }
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="times"></param>
        /// <param name="rownum"></param>
        /// <returns></returns>
        public DataTable GetPassCarDangerInfo(string where, string times, string rownum)
        {
            string mySql = string.Empty;
            try
            {
                where = where + "  and sjly='3'";
                if (string.IsNullOrEmpty(times))
                {
                    mySql = mySql + " select * from ( select hphm, ";
                    mySql = mySql + "  hpzl,f_to_name ('140001', hpzl) as hpzlms,times from";
                    mySql = mySql + "  (select hphm,hpzl,times from (select  kkid,hphm,hpzl,fxbh,sjly,cjjg,count(*) as times";
                    mySql = mySql + "  from t_tgs_passcar where  " + where + " group  by  kkid,hphm,hpzl,fxbh,sjly,cjjg)  where  hpzl!='99' and times>1 )  order by times desc) where rownum <=" + rownum + "";
                }
                else
                {
                    mySql = mySql + " select * from ( select hphm, ";
                    mySql = mySql + "  hpzl,f_to_name ('140001', hpzl) as hpzlms,times from";
                    mySql = mySql + "  (select hphm,hpzl,times from (select  kkid,hphm,hpzl,fxbh,sjly,cjjg,count(*) as times";
                    mySql = mySql + "  from t_tgs_passcar where  " + where + " group by kkid,hphm,hpzl,fxbh,sjly,cjjg)  where  hpzl!='99' and  times='" + times + "' )  order by times desc) where rownum <=" + rownum + "";
                }
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
        public DataTable GetPassCarTimesCount(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = mySql + " select count(*) from (select  kkid,hphm,hpzl,trunc(gwsj) as gwsj,count(*) as times,";
                mySql = mySql + "  fxbh from t_tgs_passcar  where ishp='1' and hpzl!='99'  group by kkid,hphm,hpzl,trunc(gwsj),fxbh )  where " + where;
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
        public DataTable GetQueryTime(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select max(egwsj),min(sgwsj),IFNULL(sum(countnum),0) from (select sgwsj,egwsj,200 as countnum from t_tgs_query   where  " + where + "  union select sgwsj,egwsj, countnum from t_tgs_query_temp   where  " + where + ")";
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
        public DataTable GetQueryTimeList(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from (select sgwsj,egwsj,200 as countnum from t_tgs_query   where  " + where + "  union select sgwsj,egwsj, countnum from t_tgs_query_temp   where  " + where + ")  order by egwsj desc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得过车总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetPassCarInfoNum(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select count(1) from t_tgs_passcar  where  " + where;
                return int.Parse(dataAccess.Get_DataString(mySql, 0));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filed"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public string GetPassCarString(string filed, string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select " + filed + " from t_tgs_passcar  where  " + where;
                return dataAccess.Get_DataString(mySql, 0);
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
        /// <param name="filed"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public string GetPassCarMaxString(string filed, string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select " + filed + "  from (select kkid, fxbh, hpzl, cjjg, sgwsj, egwsj, cdbh,ishp  from T_TGS_QUERY_temp)  where  " + where;
                return dataAccess.Get_DataString(mySql, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        #endregion ITgsDataInfo 成员

        #region ITgsDataInfo 成员

        /// <summary>
        ///查询图片地址
        /// </summary>
        /// <param name="xh"></param>
        /// <returns></returns>
        public DataTable GetPassCarImageUrl(string xh)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select zjwjlj,zjwjsx,zjwjlx , decode(zjwjlx, '1', zjwjlj, '2',  'Images/videompg.png',  '3', 'Images/videompg.png') as src  from vt_tgs_capture   where xh='" + xh + "' order by zjwjsx";

                //DataTable dt = settingManager.GetConfigInfo("00", "17");
                //if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["configvalue"].ToString()=="1")
                //{
                //    string ipaddress = common.GetUserHostAddress();

                //    mySql = " select zjwjlj,zjwjsx,zjwjlx , decode(zjwjlx, '1', zjwjlj, '2',  'Images/videompg.png',  '3', 'Images/videompg.png') as src  from vt_tgs_capture   where xh='" + xh + "' order by zjwjsx";
                //    mySql = "select zjwjlj,zjwjsx,zjwjlx,decode(zjwjlx,'1',zjwjlj,'2','Images/videompg.png','3','Images/videompg.png') as src from(select xh, 'http://' || f_get_policeip(zjwjip,'" + ipaddress + "') || decode(substr(zjwjlj,0,1),'/','','/') || zjwjlj as zjwjlj,f_get_policeip(zjwjip,'" + ipaddress + "') as zjwjip,zjwjlx,zjwjsx from t_tgs_capture) where xh='" + xh + "' order by zjwjsx";
                //    return dataAccess.Get_DataTable(mySql);
                //}
                //else
                //{
                //    mySql = "select zjwjlj,zjwjsx,zjwjlx,decode(zjwjlx,'1',zjwjlj,'2','Images/videompg.png','3','Images/videompg.png') as src from(select xh, 'http://' || zjwjip || decode(substr(zjwjlj,0,1),'/','','/') || zjwjlj as zjwjlj,zjwjip,zjwjlx,zjwjsx from t_tgs_capture) where xh='" + xh + "' order by zjwjsx";
                //    return dataAccess.Get_DataTable(mySql);
                //}
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
        /// <param name="xh"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPassCarImageUrl(string xh, string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select zjwjlj,zjwjsx,zjwjlx,decode(zjwjlx,'1',zjwjlj,'2','Images/videompg.png') as src,xh from vt_tgs_capture where xh in " + xh + " and " + where + " order by gxsj desc";
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
        public DataTable PassCar15MinFlow(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select kkmc,fxbhms,rq,jgms,ll,xcll,dcll,pjsd from vt_tgs_flow_15min where " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获得5分钟流量信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable PassCar5MinFlow(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select kkmc,fxbhms,rq,jgms,ll,xcll,dcll,pjsd from vt_tgs_flow_5min where " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获得5分钟流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataTable PassCar5MinFlow(string directionId, string startDate, string endDate)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                string[] str = directionId.Split('|');
                string where = this.ConventSql(startDate, endDate);

                mySql.Append("select  F_GET_VALUE ('station_name',  't_cfg_set_station',  'station_id', '" + str[0] + "')|| IFNULL(f_get_value('direction_desc',  't_cfg_direction', 'station_id||direction_id', '" + str[0] + "' || '" + str[1] + "'), f_to_name('240025', '" + str[1] + "')) as fxbhms,");
                mySql.Append("day as RQ, sum(count2) as LL  from (select sum(count2) as count2, to_char(day, 'yyyy-MM-dd') as day from ");
                mySql.Append("  (");
                mySql.Append(CreateQuerySql(directionId, startDate, endDate));
                mySql.Append("   )");
                mySql.Append("  group by day)");
                mySql.Append("  group by day");
                mySql.Append("  order by day");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获得天流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataTable PassCarDayFlow(string directionId, string startDate, string endDate)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                string[] str = directionId.Split('|');
                string where = this.ConventSql(startDate, endDate);
                mySql.Append("select  CONCAT(F_GET_kkms ('" + str[0] + "'),IFNULL(f_get_fxbhms(CONCAT('" + str[0] + "','" + str[1] + "')), f_to_name('240025', '" + str[1] + "'))) as fxbhms,");
                //mySql.Append("day as RQ, sum(count2) as LL  from (select sum(count2) as count2, to_char(day, 'yyyy-MM-dd') as day from ");
                mySql.Append("DATE_FORMAT(CONCAT(LEFT('" + startDate + "',7), time_id),'%Y-%m-%d') AS hour1,CONVERT(IFNULL(ll,'0'),DECIMAL) AS ll from");
                //mySql.Append("  (");
                //mySql.Append("  (");
                mySql.Append(CreateQuerySql(directionId, startDate, endDate));
                //mySql.Append("   )");
                //mySql.Append("  group by day)");
                //mySql.Append("  group by day");
                //mySql.Append("  order by day");
                mySql.Append(" RIGHT JOIN t_cfg_time b ON  b.time_id= RIGHT(a.RQ,2) WHERE b.time_type = '2' ORDER BY hour1 ASC ");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获得天流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable PassCarDayFlow(string directionId, string date)
        {
            try
            {
                return PassCarDayFlow(directionId, date, date);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获得小时流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable PassCarHourFlow(string directionId, string date)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                string[] str = directionId.Split('|');
                mySql.Append("  select  CONCAT(F_GET_kkms ('" + str[0] + "'),IFNULL(f_get_fxbhms(CONCAT('" + str[0] + "','" + str[1] + "')), f_to_name('240025', '" + str[1] + "'))) as fxbhms,time_id AS hour1, CONVERT(IFNULL(ll,'0'),DECIMAL) as ll from ");
                mySql.Append("  (select fxbh,xs,SUM(ll) AS ll from t_tgs_flow_hour t where rq=STR_TO_DATE('" + date + "','%Y-%m-%d') and kkid= '" + str[0] + "'  and fxbh='" + str[1] + "' GROUP BY fxbh,xs) a RIGHT JOIN");
                mySql.Append("  t_cfg_time b ON a.xs = b.time_id WHERE time_type = '1' ORDER BY hour1 ASC");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="qjid"></param>
        /// <param name="date"></param>
        /// <param name="countType"></param>
        /// <returns></returns>
        public DataTable AreaSpeedCount(string qjid, string date, string countType)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                mySql.Append(" select f_get_value('qjmc','vt_tgs_peccancy_area_setting','xh','" + qjid + "') AS qjmc,hour,IFNULL(pjsd,'0') as ll from ");
                mySql.Append("  (select qjmc,xs,pjsd from vt_tgs_flow_area_hour t where rq=STR_TO_DATE('" + date + "','yyyy-mm-dd') and qjid='" + qjid + "') a,");
                mySql.Append("  (select to_char(DATE_FORMAT('00:00:00','hh24:mi:ss'),'hh24')+rownum-1 as hour from all_objects where rownum<=24) b");
                mySql.Append(" where a.xs(+) = b.hour order by hour");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="qjid"></param>
        /// <param name="date"></param>
        /// <param name="countType"></param>
        /// <returns></returns>
        public DataTable AreaODCount(string qjid, string date, string countType)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                if (countType.Equals("0"))
                {
                    mySql.Append(" select f_get_value('qjmc','vt_tgs_peccancy_area_setting','xh','" + qjid + "') AS qjmc,hour,IFNULL(ll,'0') as ll from ");
                    mySql.Append("  (select qjmc,xs,ll from vt_tgs_flow_area_hour t where rq=DATE_FORMAT('" + date + "','yyyy-mm-dd') and qjid='" + qjid + "') a,");
                    mySql.Append("  (select to_char(DATE_FORMAT('00:00:00','hh24:mi:ss'),'hh24')+rownum-1 as hour from all_objects where rownum<=24) b");
                    mySql.Append(" where a.xs(+) = b.hour order by hour");
                }
                else
                {
                    mySql.Append("select f_get_value('qjmc','vt_tgs_peccancy_area_setting','xh','" + qjid + "') as qjmc,");
                    mySql.Append("day as RQ, sum(count2) as LL  from (select sum(count2) as count2, to_char(day, 'dd') as day from ");
                    mySql.Append("  (");
                    mySql.Append(CreateAreaQuerySql(qjid, date, date));
                    mySql.Append("   )");
                    mySql.Append("  group by day)");
                    mySql.Append("  group by day");
                    mySql.Append("  order by day");
                }
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="date"></param>
        /// <param name="tim"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable AreaSpeedQuery(string date, string tim, string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select xh,qjmc,qjjl,qdate,smin,pjsd,round(qjjl/pjsd,2) * 60 as pjsj,decode(sign(ydsd-pjsd),1,'2',decode(sign(hmsd-pjsd),1,'1',-1,'0',0,'1','-1')) as zt from ";
                mySql = mySql + " (select xh,qjmc,qjjl,ydsd,hmsd,'" + DateTime.Parse(date).ToString("yyyy-MM-dd") + "' as qdate,trunc(" + tim + "/count(*))||':'||mod(" + tim + ",12)*5 as smin,xh||'" + DateTime.Parse(date).ToString("yyyyMMdd") + "'||" + tim + " as sid  from vt_tgs_peccancy_area_setting) a,";
                mySql = mySql + "(select IFNULL(pjsd,0) as pjsd ,qjid||to_char(rq,'yyyymmdd')||to_char((xs)*12+jg) as  fid from   VT_TGS_FLOW_AREA)  b ";
                mySql = mySql + " where  " + where + "  and sid=fid(+)";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获得月流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataTable PassCarMonthFlow(string directionId, string startDate, string endDate)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                string[] str = directionId.Split('|');
                mySql.Append("select  CONCAT(F_GET_kkms ('" + str[0] + "'),IFNULL(f_get_fxbhms(CONCAT('" + str[0] + "','" + str[1] + "')), f_to_name('240025', '" + str[1] + "'))) as fxbhms,");
                mySql.Append("b.time_id AS yue,CONVERT(IFNULL(ll,'0'),DECIMAL) AS ll");
                mySql.Append(" FROM(SELECT fxbh,MONTH(rq) AS sj,SUM(ll) AS ll FROM T_TGS_FLOW_DAY");
                //mySql.Append(CreateQuerySql(directionId, startDate, endDate));
                mySql.Append(" WHERE kkid = '" + str[0] + "' AND fxbh = '" + str[1] + "' AND rq >= STR_TO_DATE('" + startDate + "', '%Y-%m-%d') AND rq <= STR_TO_DATE('" + endDate + "', '%Y-%m-%d')");
                mySql.Append(" GROUP BY fxbh,sj) a");
                mySql.Append(" RIGHT JOIN t_cfg_time b ON b.time_id = a.sj WHERE b.time_type = '4'");
                mySql.Append(" ORDER BY time_id ASC");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获得月流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable PassCarMonthFlow(string directionId, string date)
        {
            try
            {
                string sdate = DateTime.Parse(date).ToString("yyyy-01-01");
                string edate = DateTime.Parse(DateTime.Parse(date).ToString("yyyy-12-01")).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                return PassCarMonthFlow(directionId, sdate, edate);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获取周流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataTable PassCarWeekFlow(string directionId, string startDate, string endDate)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                string[] str = directionId.Split('|');
                string where = this.ConventSql(startDate, endDate);
                mySql.Append(" select  F_GET_VALUE ('station_name',  't_cfg_set_station',  'station_id', '" + str[0] + "')||  IFNULL(f_get_value('direction_desc',  't_cfg_direction', 'station_id||direction_id', '" + str[0] + "' || '" + str[1] + "'), f_to_name('240025', '" + str[1] + "')) as fxbhms,b.zhou,IFNULL(sum(ll),'0') as ll");
                mySql.Append(" from  (  select zhou, ll, FXBH, rq  from vt_tgs_flow_week t");
                mySql.Append(" where DATE_FORMAT(to_char(DATE_FORMAT(rq, 'yyyy-mm-dd'), 'yyyy'), 'yyyy') =");
                mySql.Append(" DATE_FORMAT(to_char(DATE_FORMAT('" + startDate + "', 'yyyy-mm-dd'),'yyyy'),'yyyy')");
                mySql.Append(" and t.kkid='" + str[0] + "'and t.FXBH = '" + str[1] + "' " + where + ") a ,(select to_char(DATE_FORMAT('" + startDate + "', 'yyyy-MM-dd'), 'ww') +");
                mySql.Append(" rownum - 1 as zhou from all_objects");
                mySql.Append(" where rownum <= to_char(last_day(DATE_FORMAT(to_char(DATE_FORMAT('" + startDate + "', 'yyyy-MM-dd'), 'yyyy') || '-12-01', 'yyyy-MM-dd')),'ww') )b");
                mySql.Append(" where a.zhou(+) =b.zhou group by b.zhou");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获取周流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable PassCarWeekFlow(string directionId, string date)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                string[] str = directionId.Split('|');
                mySql.Append("select  CONCAT(F_GET_kkms ('" + str[0] + "'),IFNULL(f_get_fxbhms(CONCAT('" + str[0] + "','" + str[1] + "')), f_to_name('240025', '" + str[1] + "'))) as fxbhms,b.time_id AS zhou,CONVERT(IFNULL(ll,'0'),DECIMAL) as ll FROM");
                //mySql.Append(" from(select zhou, ll, FXBH, rq  from vt_tgs_flow_week t");
                //mySql.Append("  where DATE_FORMAT(to_char(rq, 'yyyy'), 'yyyy') =");
                //mySql.Append(" DATE_FORMAT(to_char(DATE_FORMAT('" + date + "', 'yyyy-mm-dd'),'yyyy'),'yyyy')");
                //mySql.Append(" and  t.kkid='" + str[0] + "' and t.FXBH = '" + str[1] + "') a ,(select to_char(DATE_FORMAT('" + DateTime.Parse(date).ToString("yyyy-01-01") + "', 'yyyy-mm-dd'), 'ww') +");
                //mySql.Append(" rownum - 1 as zhou from all_objects");
                //mySql.Append(" where rownum <= to_char(last_day(DATE_FORMAT(to_char(DATE_FORMAT('" + date + "', 'yyyy-MM-dd'), 'yyyy') || '-12-01', 'yyyy-MM-dd')),'ww') )b");
                //mySql.Append("  where a.zhou(+) =b.zhou group by b.zhou");
                mySql.Append(" (select fxbh,WEEKOFYEAR(rq) AS sj, SUM(ll) AS ll  from T_TGS_FLOW_DAY");
                mySql.Append("  WHERE kkid = '" + str[0] + "' AND fxbh = '" + str[1] + "'");
                mySql.Append(" AND rq >= DATE_FORMAT(CONCAT(LEFT('" + date + "',4),'-01-01'), '%Y-%m-%d') AND rq <= STR_TO_DATE(CONCAT(LEFT('" + date + "',4),'-12-31'), '%Y-%m-%d')");
                mySql.Append(" GROUP BY fxbh,sj) a");
                mySql.Append(" RIGHT JOIN t_cfg_time b ON b.time_id = a.sj WHERE b.time_type = '3' ORDER BY time_id ASC");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获取年流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataTable PassCarYearFlow(string directionId, string startDate, string endDate)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                string[] str = directionId.Split('|');
                mySql.Append("select  F_GET_VALUE ('station_name',  't_cfg_set_station',  'station_id', '" + str[0] + "')||  IFNULL(f_get_value('direction_desc',  't_cfg_direction', 'station_id||direction_id', '" + str[0] + "' || '" + str[1] + "'), f_to_name('240025', '" + str[1] + "')) as fxbhms,");
                mySql.Append("day as RQ, sum(count2) as LL  from (select sum(count2) as count2, to_char(day, 'yyyy') as day from ");
                mySql.Append("  (");
                mySql.Append(CreateQuerySql(directionId, startDate, endDate));
                mySql.Append("   )");
                mySql.Append("  group by day)");
                mySql.Append("  group by day");
                mySql.Append("  order by day");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获取年流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable PassCarYearFlow(string directionId, string date)
        {
            try
            {
                string sdate = DateTime.Parse(date).AddYears(-4).ToString("yyyy-01-01");
                string edate = DateTime.Parse(DateTime.Parse(date).ToString("yyyy-12-01")).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                return PassCarYearFlow(directionId, sdate, edate);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return null;
            }
        }

        #endregion ITgsDataInfo 成员

        /// <summary>
        /// 插入违法信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertPeccancy(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = @"insert into  t_tms_peccancy ( xh,
                                                        hpzl,
                                                        hphm,
                                                        wfxw,
                                                        wfnr,
                                                        wfsj,
                                                        wfdd,
                                                        wfdz,
                                                        cdbh,
                                                        fxbh,
                                                        clsd,
                                                        sjly,
                                                        cjjg,
                                                        cjyh,
                                                        cjsj,
                                                        kkid,
                                                        zjwj1,
                                                        zjwj2,
                                                        zjwj3,
                                                        zjwj4 ) values(";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "xh") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "hpzl") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "hphm") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "wfxw") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "wfnr") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "wfsj") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "wfdd") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "wfdz") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "cdbh") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "fxbh") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "clsd") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "sjly") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "cjjg") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "cjyh") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "cjsj") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "kkid") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "zjwj1") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "zjwj2") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "zjwj3") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "zjwj4") + "')";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 获得政府公告信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetZfgg()
        {
            string sql = "SELECT * FROM t_cfg_noticepic";
            return dataAccess.Get_DataTable(sql);
        }

        /// <summary>
        /// 得到车辆品牌的子品牌字符串
        /// </summary>
        /// <param name="clpp"></param>
        /// <returns></returns>
        public DataTable GetClppString(string clpp)
        {
            string sql = "SELECT T.CSBL,T.NAME FROM T_ITGS_PECCANCY_SETTING T WHERE T.XH='261012' AND NAME LIKE '" + clpp + "%' AND T.CSBL >503 ";
            return dataAccess.Get_DataTable(sql);
        }

        /// <summary>
        /// 得到一个Json字符串
        /// </summary>
        /// <returns></returns>
        public DataTable GetJson(out DataTable dt)
        {
            dt = null;
            string sql = @"SELECT
                              T_CFG_DEPARTMENT.DEPARTID,
                              T_CFG_DEPARTMENT.DEPARTNAME,
                              T_CFG_DEPARTMENT.CLASS,
                              T_CFG_DEPARTMENT.CLASSCODE
                            FROM
                              T_CFG_SET_STATION
                              RIGHT JOIN T_CFG_DEPARTMENT
                                ON T_CFG_SET_STATION.DEPARTID = T_CFG_DEPARTMENT.DEPARTID
                             WHERE T_CFG_SET_STATION.STATION_ID IS NOT NULL
                             GROUP BY T_CFG_DEPARTMENT.DEPARTID
                             ORDER BY T_CFG_DEPARTMENT.DEPARTID ASC,
                              T_CFG_SET_STATION.STATION_NAME DESC ";//得到部门
            string sql1 = @"SELECT
                      T_CFG_SET_STATION.STATION_ID,
                      T_CFG_SET_STATION.STATION_NAME,
                      T_CFG_DEPARTMENT.DEPARTID,
                      T_CFG_DEPARTMENT.DEPARTNAME,
                      T_CFG_DEPARTMENT.CLASS,
                      T_CFG_DEPARTMENT.CLASSCODE
                    FROM
                      T_CFG_SET_STATION
                      RIGHT JOIN T_CFG_DEPARTMENT
                        ON T_CFG_SET_STATION.DEPARTID = T_CFG_DEPARTMENT.DEPARTID
                    WHERE T_CFG_SET_STATION.STATION_ID IS NOT NULL
                    ORDER BY T_CFG_DEPARTMENT.DEPARTID ASC,
                      T_CFG_SET_STATION.STATION_NAME DESC ";//得到部门下的卡口
            dt = dataAccess.Get_DataTable(sql);
            return dataAccess.Get_DataTable(sql1);
        }

        /// <summary>
        /// 转换SQL语句
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private string ConventSql(string startDate, string endDate)
        {
            try
            {
                string where = "";
                if (!string.IsNullOrEmpty(startDate))
                    where += " and rq>=STR_TO_DATE('" + startDate + "','%Y-%m-%d')";
                if (!string.IsNullOrEmpty(endDate))
                    where += " and rq<=STR_TO_DATE('" + endDate + "','%Y-%m-%d')";
                if (string.IsNullOrEmpty(where))
                    where = " and 1=1 ";
                return where;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return " and 1=1 ";
            }
        }

        /// <summary>
        ///建立查询语句（部分）
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private string CreateQuerySql(string directionId, string startDate, string endDate)
        {
            string[] str = directionId.Split('|');
            string sdate = DateTime.Parse(startDate).ToString("yyyy-MM-01");
            string edate = Convert.ToDateTime(DateTime.Parse(endDate).AddMonths(1).ToString("yyyy-MM-01")).AddDays(-1).ToString("yyyy-MM-dd");
            StringBuilder mySql = new StringBuilder();
            //mySql.Append(" select day, IFNULL(LL, 0) as count2");
            //mySql.Append(" from (select DATE_FORMAT('" + sdate + "', 'yyyy-mm-dd') + rownum - 1 as day");
            //mySql.Append(" from all_objects");
            //mySql.Append(" where rownum <=");
            //mySql.Append(" to_number(to_char(DATE_FORMAT('" + edate + "', 'yyyy-mm-dd') -");
            //mySql.Append("  DATE_FORMAT('" + sdate + "', 'yyyy-mm-dd'))) + 1) b,");
            //mySql.Append(" (select LL, RQ");
            //mySql.Append("  from VT_TGS_FLOW_DAY t");
            //mySql.Append("  where  kkid= '" + str[0] + "'  and fxbh='" + str[1] + "' and rq >=");
            //mySql.Append("  DATE_FORMAT('" + sdate + "', 'yyyy-MM-dd') and rq <=");
            //mySql.Append("  DATE_FORMAT('" + edate + "', 'yyyy-MM-dd')) d");
            //mySql.Append("   where RQ(+) = trunc(day)");
            mySql.Append(" (select fxbh, RQ,SUM(ll) AS ll");
            mySql.Append("  from T_TGS_FLOW_DAY t");
            mySql.Append("  where  kkid= '" + str[0] + "'  and fxbh='" + str[1] + "' and rq >=");
            mySql.Append("  STR_TO_DATE('" + sdate + "', '%Y-%m-%d') and rq <=");
            mySql.Append("  STR_TO_DATE('" + edate + "', '%Y-%m-%d')");
            mySql.Append("   GROUP BY fxbh,RQ) a");
            return mySql.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="qjid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private string CreateAreaQuerySql(string qjid, string startDate, string endDate)
        {
            string sdate = DateTime.Parse(startDate).ToString("yyyy-MM-01");
            string edate = Convert.ToDateTime(DateTime.Parse(endDate).AddMonths(1).ToString("yyyy-MM-01")).AddDays(-1).ToString("yyyy-MM-dd");
            StringBuilder mySql = new StringBuilder();
            mySql.Append(" select day, IFNULL(LL, 0) as count2");
            mySql.Append(" from (select DATE_FORMAT('" + sdate + "', 'yyyy-mm-dd') + rownum - 1 as day");
            mySql.Append(" from all_objects");
            mySql.Append("  where rownum <=");
            mySql.Append(" to_number(to_char(DATE_FORMAT('" + edate + "', 'yyyy-mm-dd') -");
            mySql.Append(" STR_TO_DATE('" + sdate + "',  'yyyy-mm-dd'))) + 1) b,");
            mySql.Append(" (select LL, RQ from vt_tgs_flow_area_hour t");
            mySql.Append(" where qjid = '" + qjid + "' and  rq >=");
            mySql.Append(" STR_TO_DATE('" + sdate + "', 'yyyy-MM-dd') and  rq <=");
            mySql.Append(" STR_TO_DATE('" + edate + "', 'yyyy-MM-dd')) d");
            mySql.Append(" where RQ(+) = trunc(day)");
            return mySql.ToString();
        }

        #region ITgsDataInfo 成员

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDeviceState(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select station_id,station_name,device_id,device_name,device_type_id,device_type,device_ip,device_port, state_id,state,update_time,decode(state_id,1,'ok',2,'alarm',0,'err','unknow') as imageName ,direction_name from vt_tgs_device_state  where " + where + " order by station_name asc ";
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
        /// <returns></returns>
        public DataTable GetDeviceState()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select station_id,station_name,device_id,device_name,device_type_id,device_type,device_ip,device_port, state_id,state,update_time,decode(state_id,1,'ok',2,'alarm',0,'err','unknow') as imageName,direction_name from vt_tgs_device_state  order by station_name asc ";
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
        public DataTable GetDevDeviceState(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select station_id,station_name,device_id,device_name,device_type_id,device_type,device_ip,device_port, state_id,state,update_time,decode(state_id,1,'ok',2,'alarm',0,'err','unknow') as imageName ,direction_name from vt_dev_device_state  where " + where + " order by station_name asc ";
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
        /// <returns></returns>
        public DataTable GetDevDeviceState()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select station_id,station_name,device_id,device_name,device_type_id,device_type,device_ip,device_port, state_id,state,update_time,decode(state_id,1,'ok',2,'alarm',0,'err','unknow') as imageName,direction_name from vt_dev_device_state  order by station_name asc ";
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
        public DataTable GetPassCarMonitor(string where)
        {
            string mySql = string.Empty;
            try
            {
                //mySql = "select rownum as rn,a.* from (select * from vt_tgs_passcar_temp a  where gwsj>=(select max(gwsj)-1/1000 from t_tgs_passcar_temp where 1=1 and " + where + ")  order by gwsj desc )  a where rownum <10  and  " + where;

                mySql = "select * from (select   row_number() over(order by gwsj desc) rn , t.* from vt_tgs_passcar_temp t where gwsj>sysdate-(5/1440) and 1=1 and " + where + " ) where  rn<10";
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
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public DataTable GetPassCarTemp(string where, int startIndex, int endIndex)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "  select /*+FIRST_ROWS*/ xh,";
                mySql = mySql + "  kkid,f_get_value ('station_name', 't_cfg_set_station', 'station_id', kkid) as kkmc,hphm, ";
                mySql = mySql + "  hpzl,f_to_name ('140001', hpzl) as hpzlms,";
                mySql = mySql + "  to_char(gwsj,'%Y-%m-%d %H:%i:%s') as gwsj, to_char (gwsj, 'yyyy-mm-dd') as gwrq, to_char (gwsj, 'hh24:mi:ss') as gwsfm,";
                mySql = mySql + "  hpys,f_to_name ('240001', hpys) as hpysms,";
                mySql = mySql + "  cllx, f_to_name ('140007', cllx) as cllxms,";
                mySql = mySql + "  ddbh,f_get_value ('location_name',  't_cfg_location',  'location_id',  ddbh)  as ddbhms,";
                mySql = mySql + "  fxbh, f_to_name ('240025', fxbh) as fxbhms,";
                mySql = mySql + "  cdbh, clsd,  f_get_value ( decode (hpzl, '01', 'big_limit_speed', 'small_limit_speed'),  't_tgs_set_lane', 'direction_id||lane_id',  fxbh || cdbh  ) as clxs,";
                mySql = mySql + "  jllx, f_to_names ('240002', jllx) as jllxms,";
                mySql = mySql + "  sjly, f_to_name ('240022', sjly) as sjlyms, cjjg, ";
                mySql = mySql + "  f_get_value ('departname', 't_cfg_department', 'departid',  cjjg)  as cjjgms, ";
                mySql = mySql + "  zjwj1,";
                mySql = mySql + "  zjwj2,";
                mySql = mySql + "  zjwj3";
                mySql = mySql + "  from (select  xh,kkid,hphm,hpzl,hpys,gwsj,ddbh,";
                mySql = mySql + "  fxbh,cdbh,cllx,clsd,jllx,cjjg,sjly,zjwj1,zjwj2,zjwj3,row_number() over(order by gwsj desc) rn from t_tgs_passcar_temp   where " + where + " )  where  rn<=" + endIndex + " and rn >=" + startIndex;
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
        public int GetPassCarTempCount(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select count(1) from   t_tgs_passcar_temp   where   " + where;
                return int.Parse(dataAccess.Get_DataString(mySql, 0));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return 0;
            }
        }

        /// <summary>
        ///根据卡口编号查询卡口方向
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetKakouDirection(string StationId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT DIRECTION_ID,DIRECTION_NAME from t_cfg_direction where STATION_ID = '" + StationId + "' ORDER BY DIRECTION_ID ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///查询最新报警信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAlarmMonitor(string where, string rownum)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT   * FROM(select  *  from vt_tgs_alarmed_temp  where  " + where + ") a LIMIT 0,  " + rownum;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///查询最新流量信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetFlowMonitor(string where, string rownum)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT t1.ID, t2.KKBHMS, t2.KSSD, t2.JSSD, t1.bjsj, t2.TJZQ, t2.BJFZ, t1.bl, t1.ll, t2.KKPZRMS, t1.cljg, t1.gxsj, t3.CODEDESC,t2.KKFX,t2.KKFXMS FROM t_tgs_flow_alert t1 JOIN t_tgs_flowalarm_setting t2 ON t1.gzbh = t2.BH JOIN t_sys_code t3 ON t1.cljg = t3.`CODE` WHERE t3.CODETYPE = '430800' and" + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT MAX(bjsj) FROM vt_tgs_alarmed_temp  where " + where + "";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得所有过车信息
        /// </summary>
        /// <param name="filed"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAllPassCarInfo(string filed, string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select t.*,rownum from( select " + filed + "  from vt_tgs_passcar_capture  where  " + where + ") t ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///判断序号是否存在
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
        /// 判断该车辆是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="hphm"></param>
        /// <param name="hpzl"></param>
        /// <returns></returns>
        private int GeHphmExist(string tableName, string hphm, string hpzl)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select count(1) from   " + tableName + "   where  hphm  ='" + hphm + "' and hpzl= '" + hpzl + "'";
                return int.Parse(dataAccess.Get_DataString(mySql, 0));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 判断该车辆是否存在(专项布控)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="ZXNR"></param>
        /// <param name="ZXLX"></param>
        /// <returns></returns>
        private int GeZXNRExist(string tableName, string ZXMS, string ZXLX)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select count(1) from   " + tableName + "   where  BKMS  ='" + ZXMS + "' and BKLX= '" + ZXLX + "'";
                return int.Parse(dataAccess.Get_DataString(mySql, 0));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 判断该车辆是否存在(流量报警)
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="KKBH"></param>
        /// <param name="loacation_name"></param>
        /// <returns></returns>
        private int GeKKBHExist(string tableName, string BH, string KKBH)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select count(1) from   " + tableName + "   where  BH  ='" + BH + "' and KKBH= '" + KKBH + "'";
                return int.Parse(dataAccess.Get_DataString(mySql, 0));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
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
            string mySql = string.Empty;
            try
            {
                mySql = "select kkmc,concat(kkid,fxbh),concat(kkmc,fxms), name, g_count,bfb from (select kkmc,kkid,fxbh,fxbh2,fxms, name, g_count, if(all_count=0, 0, round(g_count / all_count * 100, 2)) as bfb from ( select kkmc,kkid,fxbh,fxms," + field + " as name, count(1) as g_count from vt_tgs_alarmed  where " + where + " group by kkmc,kkid,fxbh,fxms," + field + ") t1, (select fxbh as fxbh2, count(1) as all_count from vt_tgs_alarmed t2 where " + where + " group by fxbh )t3) t4  where fxbh=fxbh2";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                //下面把表t_tgs_alarmed 改成了  vt_tgs_alarmed_query
                mySql = "select max(bjsj),min(bjsj),count(*) from vt_tgs_alarmed_query where " + where + "";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得报警信息
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        public DataTable GetAlarmInfo(string field, string where, int startrow, int endrow)
        {
            string mySql = string.Empty;
            try
            {
                //王照伟修改前代码
                //mySql = @"SELECT t4.* FROM  (SELECT t1.xh AS xh1 FROM vt_tgs_alarmed_query t1 WHERE " + where + " ORDER BY bjsj DESC LIMIT " + startrow + ", " + (endrow - startrow).ToString() + ") t2 ,vt_tgs_alarmed t4 WHERE t2.xh1 = t4.xh ORDER BY bjsj DESC ";
                //王照伟修改后代码
                mySql = @"SELECT * FROM vt_tgs_alarmed_temp WHERE " + where + " ORDER BY bjsj DESC LIMIT " + startrow + ", " + (endrow - startrow).ToString();
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得报警信息总数
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        public DataTable GetAlarmInfoCount(string field, string where, int startrow, int endrow)
        {
            string mySql = string.Empty;
            try
            {
                //mySql = " select  " + field + "  from (select t3.*  from (select  t1.xh as xh1, t1.bjlx as bjlx1,row_number() over(order by bjsj desc) rn from t_tgs_alarmed t1   where  " + where + ") t2 ,vt_tgs_alarmed t3 where  t2.xh1=t3.xh  and t2.rn<=" + endrow + "  and t2.rn>=" + startrow + "  and t2.bjlx1=t3.bjlx) ";
                //mySql = @"SELECT t4.* FROM vt_tgs_alarmed t4  where " + where + "ORDER BY bjsj DESC LIMIT " + (startrow - 1).ToString() + ", " + (endrow - (startrow - 1)).ToString();
                //王照伟修改前源代码
                //mySql = @"SELECT Count(*) FROM  (SELECT t1.xh AS xh1, t1.bjlx AS bjlx1 FROM vt_tgs_alarmed_query t1 WHERE " + where + " ORDER BY bjsj DESC) t2 ,vt_tgs_alarmed t4 WHERE t2.xh1 = t4.xh ORDER BY bjsj DESC ";
                //王照伟修改后代码
                mySql = @"SELECT Count(*) FROM vt_tgs_alarmed_temp WHERE " + where + " ORDER BY bjsj DESC ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得流量报警信息
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        public DataTable GetFlowInfo(string field, string where, int startrow, int endrow)
        {
            string mySql = string.Empty;
            try
            {
                mySql = @"SELECT t1.id, t2.KKBHMS, t2.KSSD, t2.JSSD, t1.bjsj, t2.TJZQ, t2.BJFZ, t1.bl, t1.ll, t2.KKPZRMS, t1.cljg, t1.gxsj, t3.CODEDESC,t2.KKFX,t2.KKFXMS FROM t_tgs_flow_alert t1 JOIN t_tgs_flowalarm_setting t2 ON t1.gzbh = t2.BH JOIN t_sys_code t3 ON t1.cljg = t3.`CODE` WHERE t3.CODETYPE = '430800' and " + where + " ORDER BY bjsj DESC ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得流量报警信息总数
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        public DataTable GetFlowInfoCount(string field, string where, int startrow, int endrow)
        {
            string mySql = string.Empty;
            try
            {
                //mySql = " select  " + field + "  from (select t3.*  from (select  t1.xh as xh1, t1.bjlx as bjlx1,row_number() over(order by bjsj desc) rn from t_tgs_alarmed t1   where  " + where + ") t2 ,vt_tgs_alarmed t3 where  t2.xh1=t3.xh  and t2.rn<=" + endrow + "  and t2.rn>=" + startrow + "  and t2.bjlx1=t3.bjlx) ";

                mySql = @"SELECT Count(*) FROM t_tgs_flow_alert t1 JOIN t_tgs_flowalarm_setting t2 ON t1.gzbh = t2.BH JOIN t_sys_code t3 ON t1.cljg = t3.`CODE` WHERE t3.CODETYPE = '430800' and " + where + " ORDER BY bjsj DESC ";
                //mySql = @"SELECT t4.* FROM vt_tgs_alarmed t4  where " + where + "ORDER BY bjsj DESC LIMIT " + (startrow - 1).ToString() + ", " + (endrow - (startrow - 1)).ToString();
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteDeviceInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_cctv_cam_setting  where id='" + hs["id"].ToString() + "'";
                dataAccess.Execute_NonQuery(mySql);

                mySql = "delete  from t_tgs_set_device  where id='" + hs["id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteDevDeviceInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from t_dev_device  where id='" + hs["id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 删除违法信息
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public int DeletePeccancyInfo(List<string> records)
        {
            string mySql = string.Empty;
            int res = 0;
            try
            {
                foreach (string xh in records)
                {
                    mySql = "delete from t_tms_peccancy where  xh='" + xh + "'";
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
        /// 获得设备信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDeviceInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select *  from vt_tgs_set_device  where  " + where + " order by device_name ,camera_ip,camera_chl";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 插入设备信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertDeviceInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (hs["isvideomonitor"].ToString() == "1")
                {
                    mySql = "insert into  t_cctv_cam_setting (id,type,camid,masterid,channelid,remoteip,remoteport, remoteuser,remotepwd,recordtype, recordip,recordport,recordchannel,recorduser,recordpwd,isgismark) values(";
                    mySql = mySql + "'" + hs["id"].ToString() + "',";
                    mySql = mySql + "'10',";
                    mySql = mySql + "'" + Common.GetHashtableValue(hs, "device_id") + "',";
                    mySql = mySql + "'0',";
                    mySql = mySql + "'" + hs["camera_chl"].ToString() + "',";
                    mySql = mySql + "'" + hs["device_ip"].ToString() + "',";
                    mySql = mySql + "'" + Common.GetHashtableValue(hs, "device_port", "18000") + "',";
                    mySql = mySql + "'" + hs["remoteuser"].ToString() + "',";
                    mySql = mySql + "'" + hs["remotepwd"].ToString() + "',";
                    mySql = mySql + "'" + Common.GetHashtableValue(hs, "camera_type_id", "3") + "',";
                    mySql = mySql + "'" + hs["camera_ip"].ToString() + "',";
                    mySql = mySql + "'" + Common.GetHashtableValue(hs, "device_port", "18000") + "',";
                    mySql = mySql + "'" + hs["camera_chl"].ToString() + "',";
                    mySql = mySql + "'" + hs["remoteuser"].ToString() + "',";
                    mySql = mySql + "'" + hs["remotepwd"].ToString() + "' ,";
                    mySql = mySql + "'0' )";
                    dataAccess.Execute_NonQuery(mySql);
                }
                mySql = "insert into  t_tgs_set_device (id,device_id,device_type_id,device_name,device_ip,device_port,isshow,company_id,camera_type_id,station_id,direction_id,service_ip,isscanfile,imagepath,camera_ip,camera_chl,remoteuser,remotepwd,isvideomonitor) values(";
                mySql = mySql + "'" + hs["id"].ToString() + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "device_id") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "device_type_id", "02") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "device_name") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "device_ip", "127.0.0.1") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "device_port", "18000") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "isshow", "1") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "company_id", "01") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "camera_type_id", "3") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "station_id", "") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "direction_id", "") + "',";
                mySql = mySql + "'" + hs["service_ip"].ToString() + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "isscanfile", "0") + "',";
                mySql = mySql + "'" + hs["imagepath"].ToString() + "',";
                mySql = mySql + "'" + hs["camera_ip"].ToString() + "',";
                mySql = mySql + "'" + hs["camera_chl"].ToString() + "',";
                mySql = mySql + "'" + hs["remoteuser"].ToString() + "',";
                mySql = mySql + "'" + hs["remotepwd"].ToString() + "',";
                mySql = mySql + "'" + hs["isvideomonitor"].ToString() + "')";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateDeviceInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (hs.ContainsKey("isvideomonitor") && hs["isvideomonitor"].ToString() == "1")
                {
                    mySql = "update  t_cctv_cam_setting  set ";
                    mySql = mySql + "remoteip='" + hs["device_ip"].ToString() + "',";
                    mySql = mySql + Common.GetHashtableStr(hs, "device_port", "remoteport");
                    mySql = mySql + "remoteuser='" + hs["remoteuser"].ToString() + "',";
                    mySql = mySql + "remotepwd='" + hs["remotepwd"].ToString() + "',";
                    mySql = mySql + Common.GetHashtableStr(hs, "camera_type_id", "recordtype");
                    mySql = mySql + "recordip='" + hs["camera_ip"].ToString() + "',";
                    mySql = mySql + Common.GetHashtableStr(hs, "device_port", "recordport");
                    mySql = mySql + "recorduser='" + hs["remoteuser"].ToString() + "',";
                    mySql = mySql + "recordpwd='" + hs["remotepwd"].ToString() + "',";
                    mySql = mySql + "camid='" + hs["device_id"].ToString() + "'";
                    mySql = mySql + "where id='" + hs["id"].ToString() + "'";
                    dataAccess.Execute_NonQuery(mySql);
                }
                mySql = "update  t_tgs_set_device  set ";
                mySql = mySql + "device_id='" + hs["device_id"].ToString() + "',";
                mySql = mySql + Common.GetHashtableStr(hs, "device_name", "device_name");
                mySql = mySql + Common.GetHashtableStr(hs, "device_ip", "device_ip");
                mySql = mySql + Common.GetHashtableStr(hs, "device_type_id", "device_type_id");
                mySql = mySql + Common.GetHashtableStr(hs, "device_port", "device_port");
                mySql = mySql + Common.GetHashtableStr(hs, "company_id", "company_id");
                mySql = mySql + Common.GetHashtableStr(hs, "camera_type_id", "camera_type_id");
                mySql = mySql + Common.GetHashtableStr(hs, "station_id", "station_id");
                mySql = mySql + Common.GetHashtableStr(hs, "direction_id", "direction_id");
                mySql = mySql + Common.GetHashtableStr(hs, "isscanfile", "isscanfile");
                mySql = mySql + Common.GetHashtableStr(hs, "isvideomonitor", "isvideomonitor");
                mySql = mySql + Common.GetHashtableStr(hs, "imagepath", "imagepath");
                mySql = mySql + Common.GetHashtableStr(hs, "service_ip", "service_ip");
                mySql = mySql + Common.GetHashtableStr(hs, "camera_ip", "camera_ip");
                mySql = mySql + Common.GetHashtableStr(hs, "camera_chl", "camera_chl");
                mySql = mySql + "remoteuser='" + hs["remoteuser"].ToString() + "',";
                mySql = mySql + "remotepwd='" + hs["remotepwd"].ToString() + "'";
                mySql = mySql + " where id='" + hs["id"].ToString() + "'";

                int res = dataAccess.Execute_NonQuery(mySql);
                if (res < 1)
                {
                    return InsertDeviceInfo(hs);
                }
                else
                {
                    return res;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 获得设备信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDevDeviceInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select *  from vt_dev_device  where  " + where + " order by  device_type_id,device_name";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 插入设备信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertDevDeviceInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_dev_device (id,device_id,device_type,device_name,device_use,station_id,direction_id) values(";
                mySql = mySql + "'" + hs["id"].ToString() + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "device_id") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "device_type_id", "99") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "device_name") + "',";
                mySql = mySql + "'1',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "station_id", "") + "',";
                mySql = mySql + "'" + Common.GetHashtableValue(hs, "direction_id", "") + "')";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateDevDeviceInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_dev_device  set ";
                mySql = mySql + Common.GetHashtableStr(hs, "device_name", "device_name");
                mySql = mySql + Common.GetHashtableStr(hs, "device_type_id", "device_type");
                mySql = mySql + Common.GetHashtableStr(hs, "station_id", "station_id");
                mySql = mySql + Common.GetHashtableStr(hs, "direction_id", "direction_id");
                mySql = mySql + "device_id='" + hs["device_id"].ToString() + "'";
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
        /// 更新设备信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateDeviceStation(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_tgs_set_device  set ";
                mySql = mySql + Common.GetHashtableStr(hs, "station_id", "station_id");
                mySql = mySql + Common.GetHashtableStr(hs, "direction_id", "direction_id");
                mySql = mySql + Common.GetHashtableStr(hs, "device_id", "device_id");
                mySql = mySql + Common.GetHashtableStr(hs, "device_name", "device_name");
                mySql = mySql + Common.GetHashtableStr(hs, "device_ip", "device_ip");
                mySql = mySql + Common.GetHashtableStr(hs, "service_ip", "service_ip");
                mySql = mySql + Common.GetHashtableStr(hs, "imagepath", "imagepath");
                mySql = mySql + "id='" + hs["id"].ToString() + "'";
                mySql = mySql + " where id='" + hs["id"].ToString() + "'";

                int res = dataAccess.Execute_NonQuery(mySql);
                if (res < 1)
                {
                    return InsertDeviceInfo(hs);
                }
                else
                {
                    return res;
                }
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
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPasscarCount(string field, string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select kkidms,CONCAT(kkid ,fxbh),CONCAT(kkidms, fxbhms),IFNULL(NAME,'未检测') AS NAME, g_count,bfb from (select kkidms,kkid,fxbh,fxbh2,fxbhms, name, g_count, IF(all_count=0,0, round(g_count / all_count * 100, 2)) as bfb from ( select kkidms,kkid,fxbh,fxbhms," + field + " as name, sum(zs) as g_count from vt_tgs_passcar_count  where " + where + " group by kkidms,kkid,fxbh,fxbhms," + field + ") t1, (select fxbh as fxbh2, sum(zs) as all_count from vt_tgs_passcar_count t2 where " + where + " group by fxbh)t3) t4 where fxbh=fxbh2 order by g_count desc ";
                //string mySql = "select kkidms,fxhh,fxbhms, name, g_count, decode(all_count, 0, 0, round(g_count / all_count * 100, 2)) as bfb from ( select kkidms,fxhh,fxbhms," + field + " as name, sum(zs) as g_count from vt_tgs_passcar_count  where " + where + " group by kkidms,fxbhms," + field + "), (select sum(zs) as all_count from vt_tgs_passcar_count  where " + where + ")  ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得指定条件中最大最小违法时间及总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyMaxWfsj(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select max(wfsj),min(wfsj),count(*) from t_tms_peccancy t where " + where + " and t.hphm not in ( select hphm from t_tgs_info_checkless a  where a.hpzl=t.hpzl)";
                //ILog.WriteErrorLog(mySql);
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 违法统计
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="countType"></param>
        /// <returns></returns>
        public DataTable GetPeccancyCount(string field, string where, string countType)
        {
            string mySql = string.Empty;
            string field1 = string.Empty;
            string field2 = string.Empty;
            try
            {
                string countdesc = string.Empty;

                switch (field.ToLower())
                {
                    case "shzs":
                        countdesc = "审核情况";
                        field1 = "shbjms";
                        break;

                    case "tzzs":
                        countdesc = "通知情况";
                        field1 = "tzbjms";
                        break;

                    case "jzzs":
                        countdesc = "校正情况";
                        field1 = "jzztms";
                        break;

                    case "cfzs":
                        countdesc = "处罚情况";
                        field1 = "cfbjms";
                        break;

                    case "cszs":
                        countdesc = "传输情况";
                        field1 = "csbjms";
                        break;
                }
                switch (field.ToLower())
                {
                    case "hpzlms":
                    case "wfxwms":
                    case "sjlyms":
                    case "cjjgms":
                    case "bdydbjms":
                        if (countType == "0")
                        {
                            //mySql = "select wfdd, wfdz, name, g_count, bfb  from (select wfdd, wfdz,wfdd2, name, g_count,  decode (all_count, 0, 0,round (g_count / all_count * 100, 2)) as bfb from (select   wfdd, wfdz, " + field + " as name, count(*) as g_count  from vt_tms_peccancy    where " + where + "     group by wfdd, wfdz, " + field + ") t1, (select   wfdd as wfdd2, count(*) as all_count from vt_tms_peccancy t2  where " + where + "  group by wfdd)) where wfdd = wfdd2";
                            mySql = "select wfdd, wfdz, name1, g_count, IF(all_count=0,0,ROUND(g_count / all_count * 100, 2)) as bfb";
                            mySql = mySql + " from (select wfdd, wfdz, " + field + " as name1, count(*) as g_count  from vt_tms_peccancy";
                            mySql = mySql + " where " + where + "  group by wfdd, wfdz, " + field + ") t1,";
                            mySql = mySql + "(select   wfdd as wfdd2, count(*) as all_count from vt_tms_peccancy";
                            mySql = mySql + " where " + where + "  group by wfdd) t2 where t1.wfdd = t2.wfdd2";
                        }
                        else
                        {
                            mySql = "select wfdd, wfdz, name, g_count, bfb  from (select wfdd, wfdz,wfdd2, name, g_count,  decode (all_count, 0, 0,round (g_count / all_count * 100, 2)) as bfb from (select '0' as wfdd, '所有地点' as wfdz," + field + " as name, count(*) as g_count  from vt_tms_peccancy    where " + where + "     group by  " + field + ") t1, (select '0' as wfdd2, count(*) as all_count from vt_tms_peccancy t2  where " + where + " )) where wfdd = wfdd2";
                        }
                        break;

                    case "shzs":

                        if (countType == "0")
                        {
                            //mySql = "select wfdd, wfdz, name, g_count, bfb  from (select wfdd, wfdz,wfdd2, name, g_count,  decode (all_count, 0, 0,round (g_count / all_count * 100, 2)) as bfb from (select   wfdd, wfdz, " + field1 + " as name, count(*) as g_count  from vt_tms_peccancy    where " + where + "     group by wfdd, wfdz, " + field1 + ") t1, (select   wfdd as wfdd2, count(*) as all_count from vt_tms_peccancy t2  where " + where + "  group by wfdd)) where wfdd = wfdd2";
                            mySql = "select wfdd, wfdz, name1, g_count, IF(all_count=0,0,ROUND(g_count / all_count * 100, 2)) as bfb";
                            mySql = mySql + " from (select wfdd, wfdz, " + field1 + " as name1, count(*) as g_count  from vt_tms_peccancy";
                            mySql = mySql + " where " + where + "  group by wfdd, wfdz, " + field1 + ") t1,";
                            mySql = mySql + "(select   wfdd as wfdd2, count(*) as all_count from vt_tms_peccancy";
                            mySql = mySql + " where " + where + "  group by wfdd) t2 where t1.wfdd = t2.wfdd2";
                        }
                        else
                        {
                            mySql = "select wfdd, wfdz, name, g_count, bfb  from (select wfdd, wfdz,wfdd2, name, g_count,  decode (all_count, 0, 0,round (g_count / all_count * 100, 2)) as bfb from (select '0' as wfdd, '所有地点' as wfdz," + field1 + " as name, count(*) as g_count  from vt_tms_peccancy    where " + where + "     group by  " + field1 + ") t1, (select '0' as wfdd2, count(*) as all_count from vt_tms_peccancy t2  where " + where + " )) where wfdd = wfdd2";
                        };
                        break;

                    case "jzzs":
                        where = where + "   and  shbj='1'  ";
                        if (countType == "0")
                        {
                            //mySql = "select wfdd, wfdz, name, g_count, bfb  from (select wfdd, wfdz,wfdd2, name, g_count,  decode (all_count, 0, 0,round (g_count / all_count * 100, 2)) as bfb from (select   wfdd, wfdz, " + field1 + " as name, count(*) as g_count  from vt_tms_peccancy    where " + where + "     group by wfdd, wfdz, " + field1 + ") t1, (select   wfdd as wfdd2, count(*) as all_count from vt_tms_peccancy t2  where " + where + "  group by wfdd)) where wfdd = wfdd2";
                            mySql = "select wfdd, wfdz, name1, g_count, IF(all_count=0,0,ROUND(g_count / all_count * 100, 2)) as bfb";
                            mySql = mySql + " from (select wfdd, wfdz, " + field1 + " as name1, count(*) as g_count  from vt_tms_peccancy";
                            mySql = mySql + " where " + where + "  group by wfdd, wfdz, " + field1 + ") t1,";
                            mySql = mySql + "(select   wfdd as wfdd2, count(*) as all_count from vt_tms_peccancy";
                            mySql = mySql + " where " + where + "  group by wfdd) t2 where t1.wfdd = t2.wfdd2";
                        }
                        else
                        {
                            mySql = "select wfdd, wfdz, name, g_count, bfb  from (select wfdd, wfdz,wfdd2, name, g_count,  decode (all_count, 0, 0,round (g_count / all_count * 100, 2)) as bfb from (select '0' as wfdd, '所有地点' as wfdz," + field1 + " as name, count(*) as g_count  from vt_tms_peccancy    where " + where + "     group by  " + field1 + ") t1, (select '0' as wfdd2, count(*) as all_count from vt_tms_peccancy t2  where " + where + " )) where wfdd = wfdd2";
                        };
                        break;

                    case "tzzs":
                    case "cfzs":
                    case "cszs":
                        where = where + "   and  jzzt='0'  ";
                        if (countType == "0")
                        {
                            //mySql = "select wfdd, wfdz, name, g_count, bfb  from (select wfdd, wfdz,wfdd2, name, g_count,  decode (all_count, 0, 0,round (g_count / all_count * 100, 2)) as bfb from (select   wfdd, wfdz, " + field1 + " as name, count(*) as g_count  from vt_tms_peccancy    where " + where + "     group by wfdd, wfdz, " + field1 + ") t1, (select   wfdd as wfdd2, count(*) as all_count from vt_tms_peccancy t2  where " + where + "  group by wfdd)) where wfdd = wfdd2";
                            mySql = "select wfdd, wfdz, name1, g_count, IF(all_count=0,0,ROUND(g_count / all_count * 100, 2)) as bfb";
                            mySql = mySql + " from (select wfdd, wfdz, " + field1 + " as name1, count(*) as g_count  from vt_tms_peccancy";
                            mySql = mySql + " where " + where + "  group by wfdd, wfdz, " + field1 + ") t1,";
                            mySql = mySql + "(select   wfdd as wfdd2, count(*) as all_count from vt_tms_peccancy";
                            mySql = mySql + " where " + where + "  group by wfdd) t2 where t1.wfdd = t2.wfdd2";
                        }
                        else
                        {
                            mySql = "select wfdd, wfdz, name, g_count, bfb  from (select wfdd, wfdz,wfdd2, name, g_count,  decode (all_count, 0, 0,round (g_count / all_count * 100, 2)) as bfb from (select '0' as wfdd, '所有地点' as wfdz," + field1 + " as name, count(*) as g_count  from vt_tms_peccancy    where " + where + "     group by  " + field1 + ") t1, (select '0' as wfdd2, count(*) as all_count from vt_tms_peccancy t2  where " + where + " )) where wfdd = wfdd2";
                        };
                        break;
                }
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
        /// <param name="times"></param>
        /// <param name="rownum"></param>
        /// <returns></returns>
        public DataTable GetPeccancyTimesInfo(string where, string times, string rownum)
        {
            string mySql = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(times))
                {
                    times = "1";
                }
                mySql = mySql + " select * from( select kkid,f_get_value ('station_name', 't_cfg_set_station', 'station_id', kkid) as kkmc,hphm, ";
                mySql = mySql + "  hpzl,f_to_name ('140001', hpzl) as hpzlms,";
                mySql = mySql + "  fxbh,f_to_name ('240025', fxbh) as fxbhms,";
                mySql = mySql + "  sjly,f_to_name ('240022', sjly) as sjlyms,";
                mySql = mySql + "  cjjg,f_get_value ('departname', 't_cfg_department', 'departid', cjjg) as cjjgms ,times from (";
                mySql = mySql + "  select kkid,hphm,hpzl,fxbh,sjly,cjjg,times from (select  kkid,hphm,hpzl,fxbh,sjly,cjjg,count(*) as times";
                mySql = mySql + "   from t_tms_peccancy where " + where + " group by kkid,hphm,hpzl,fxbh,sjly,cjjg)  where hpzl!='99' and  times>" + times + "  )  order by times desc) where rownum<" + rownum + " ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 插入违法信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertPeccancyInfo(Hashtable hs)
        {
            try
            {
                StringBuilder myFiledSql = new StringBuilder();
                StringBuilder myValueSql = new StringBuilder();
                if (hs.ContainsKey("hpzl"))
                {
                    myFiledSql.Append("hpzl,");
                    myValueSql.Append("'" + hs["hpzl"].ToString() + "',");
                }
                if (hs.ContainsKey("hphm"))
                {
                    myFiledSql.Append("hphm,");
                    myValueSql.Append("'" + hs["hphm"].ToString() + "',");
                }
                if (hs.ContainsKey("wfsj"))
                {
                    if (!string.IsNullOrEmpty(hs["wfsj"].ToString()))
                    {
                        myFiledSql.Append("wfsj,");
                        myValueSql.Append("STR_TO_DATE('" + hs["wfsj"].ToString() + "','%Y-%m-%d %H:%i:%s')" + ",");
                    }
                }
                if (hs.ContainsKey("fxbh"))
                {
                    myFiledSql.Append("fxbh,");
                    myValueSql.Append("'" + hs["fxbh"].ToString() + "',");
                }
                if (hs.ContainsKey("cdbh"))
                {
                    myFiledSql.Append("cdbh,");
                    myValueSql.Append("'" + hs["cdbh"].ToString() + "',");
                }
                if (hs.ContainsKey("wfdd"))
                {
                    myFiledSql.Append("wfdd,");
                    myValueSql.Append("'" + hs["wfdd"].ToString() + "',");
                }
                if (hs.ContainsKey("wfdz"))
                {
                    myFiledSql.Append("wfdz,");
                    myValueSql.Append("'" + hs["wfdz"].ToString() + "',");
                }
                if (hs.ContainsKey("wfxw"))
                {
                    myFiledSql.Append("wfxw,");
                    myValueSql.Append("'" + hs["wfxw"].ToString() + "',");
                }
                if (hs.ContainsKey("wfnr"))
                {
                    myFiledSql.Append("wfnr,");
                    myValueSql.Append("'" + hs["wfnr"].ToString() + "',");
                }
                if (hs.ContainsKey("sjly"))
                {
                    myFiledSql.Append("sjly,");
                    myValueSql.Append("'" + hs["sjly"].ToString() + "',");
                }
                if (hs.ContainsKey("cjyh"))
                {
                    myFiledSql.Append("cjyh,");
                    myValueSql.Append("'" + hs["cjyh"].ToString() + "',");
                }
                if (hs.ContainsKey("cjjg"))
                {
                    myFiledSql.Append("cjjg,");
                    myValueSql.Append("'" + hs["cjjg"].ToString() + "',");
                }
                if (hs.ContainsKey("shjb"))
                {
                    myFiledSql.Append("shjb,");
                    myValueSql.Append("'" + hs["shjb"].ToString() + "',");
                }
                if (hs.ContainsKey("shbj"))
                {
                    myFiledSql.Append("shbj,");
                    myValueSql.Append("'" + hs["shbj"].ToString() + "',");
                }
                if (hs.ContainsKey("shyh"))
                {
                    myFiledSql.Append("shyh,");
                    myValueSql.Append("'" + hs["shyh"].ToString() + "',");
                }
                if (hs.ContainsKey("shsj"))
                {
                    if (!string.IsNullOrEmpty(hs["shsj"].ToString()))
                    {
                        myFiledSql.Append("shsj,");
                        myValueSql.Append("STR_TO_DATE('" + hs["shsj"].ToString() + "','%Y-%m-%d %H:%i:%s')" + ",");
                    }
                }
                if (hs.ContainsKey("cfbj"))
                {
                    myFiledSql.Append("cfbj,");
                    myValueSql.Append("'" + hs["cfbj"].ToString() + "',");
                }
                if (hs.ContainsKey("cfyh"))
                {
                    myFiledSql.Append("cfyh,");
                    myValueSql.Append("'" + hs["cfyh"].ToString() + "',");
                }
                if (hs.ContainsKey("cfsj"))
                {
                    if (!string.IsNullOrEmpty(hs["cfsj"].ToString()))
                    {
                        myFiledSql.Append("cfsj,");
                        myValueSql.Append("STR_TO_DATE('" + hs["cfsj"].ToString() + "','%Y-%m-%d %H:%i:%s')" + ",");
                    }
                }
                if (hs.ContainsKey("jzzt"))
                {
                    myFiledSql.Append("jzzt,");
                    myValueSql.Append("'" + hs["jzzt"].ToString() + "',");
                }
                if (hs.ContainsKey("jzsj"))
                {
                    if (!string.IsNullOrEmpty(hs["jzsj"].ToString()))
                    {
                        myFiledSql.Append("jzsj,");
                        myValueSql.Append("STR_TO_DATE('" + hs["jzsj"].ToString() + "','%Y-%m-%d %H:%i:%s')" + ",");
                    }
                }
                if (hs.ContainsKey("jzyh"))
                {
                    myFiledSql.Append("jzyh,");
                    myValueSql.Append("'" + hs["jzyh"].ToString() + "',");
                }

                if (hs.ContainsKey("zqmj"))
                {
                    myFiledSql.Append("zqmj,");
                    myValueSql.Append("'" + hs["zqmj"].ToString() + "',");
                }

                if (hs.ContainsKey("clsd"))
                {
                    myFiledSql.Append("clsd,");
                    myValueSql.Append("'" + hs["clsd"].ToString() + "',");
                }
                if (hs.ContainsKey("clxs"))
                {
                    myFiledSql.Append("clxs,");
                    myValueSql.Append("'" + hs["clxs"].ToString() + "',");
                }
                if (hs.ContainsKey("csys"))
                {
                    myFiledSql.Append("csys,");
                    myValueSql.Append("'" + hs["csys"].ToString() + "',");
                }
                if (hs.ContainsKey("clpp"))
                {
                    myFiledSql.Append("clpp,");
                    myValueSql.Append("'" + hs["clpp"].ToString() + "',");
                }
                if (hs.ContainsKey("jdssyr"))
                {
                    myFiledSql.Append("jdssyr,");
                    myValueSql.Append("'" + hs["jdssyr"].ToString() + "',");
                }
                if (hs.ContainsKey("zsxxdz"))
                {
                    myFiledSql.Append("zsxxdz,");
                    myValueSql.Append("'" + hs["zsxxdz"].ToString() + "',");
                }
                if (hs.ContainsKey("fzjg"))
                {
                    myFiledSql.Append("fzjg,");
                    myValueSql.Append("'" + hs["fzjg"].ToString() + "',");
                }
                if (hs.ContainsKey("lxfs"))
                {
                    myFiledSql.Append("lxfs,");
                    myValueSql.Append("'" + hs["lxfs"].ToString() + "',");
                }
                if (hs.ContainsKey("dh"))
                {
                    myFiledSql.Append("dh,");
                    myValueSql.Append("'" + hs["dh"].ToString() + "',");
                }
                if (hs.ContainsKey("yzbm"))
                {
                    myFiledSql.Append("yzbm,");
                    myValueSql.Append("'" + hs["yzbm"].ToString() + "',");
                }
                if (hs.ContainsKey("cllx"))
                {
                    myFiledSql.Append("cllx,");
                    myValueSql.Append("'" + hs["cllx"].ToString() + "',");
                }
                if (hs.ContainsKey("zt"))
                {
                    myFiledSql.Append("zt,");
                    myValueSql.Append("'" + hs["zt"].ToString() + "',");
                }
                if (hs.ContainsKey("fdjh"))
                {
                    myFiledSql.Append("fdjh,");
                    myValueSql.Append("'" + hs["fdjh"].ToString() + "',");
                }
                if (hs.ContainsKey("clxh"))
                {
                    myFiledSql.Append("clxh,");
                    myValueSql.Append("'" + hs["clxh"].ToString() + "',");
                }
                if (hs.ContainsKey("clsbdh"))
                {
                    myFiledSql.Append("clsbdh,");
                    myValueSql.Append("'" + hs["clsbdh"].ToString() + "',");
                }
                if (hs.ContainsKey("jyyxqz"))
                {
                    if (!string.IsNullOrEmpty(hs["jyyxqz"].ToString().ToString()))
                    {
                        myFiledSql.Append("jyyxqz,");
                        myValueSql.Append("STR_TO_DATE('" + hs["jyyxqz"].ToString() + "','%Y-%m-%d %H:%i:%s')" + ",");
                    }
                }
                if (hs.ContainsKey("sfzmhm"))
                {
                    myFiledSql.Append("sfzmhm,");
                    myValueSql.Append("'" + hs["sfzmhm"].ToString() + "',");
                }
                if (hs.ContainsKey("dabh"))
                {
                    myFiledSql.Append("dabh,");
                    myValueSql.Append("'" + hs["dabh"].ToString() + "',");
                }
                if (hs.ContainsKey("jcbj"))
                {
                    myFiledSql.Append("jcbj,");
                    myValueSql.Append("'" + hs["jcbj"].ToString() + "',");
                }
                if (hs.ContainsKey("xh"))
                {
                    myFiledSql.Append("xh");
                    myValueSql.Append("'" + hs["xh"].ToString() + "'");
                }
                string mySql = "insert into t_tms_peccancy(" + myFiledSql.ToString() + ") values (" + myValueSql.ToString() + ")";
                int result = dataAccess.Execute_NonQuery(mySql.ToString());
                return result;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 插入图片信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertCaptureInfo(Hashtable hs)
        {
            try
            {
                StringBuilder myFiledSql = new StringBuilder();
                StringBuilder myValueSql = new StringBuilder();
                if (hs.ContainsKey("zjwjsx"))
                {
                    myFiledSql.Append("zjwjsx,");
                    myValueSql.Append("'" + hs["zjwjsx"].ToString() + "',");
                }
                if (hs.ContainsKey("zjwjlx"))
                {
                    myFiledSql.Append("zjwjlx,");
                    myValueSql.Append("'" + hs["zjwjlx"].ToString() + "',");
                }

                if (hs.ContainsKey("zjwjip"))
                {
                    myFiledSql.Append("zjwjip,");
                    myValueSql.Append("'" + hs["zjwjip"].ToString() + "',");
                }
                if (hs.ContainsKey("zjwjlj"))
                {
                    myFiledSql.Append("zjwjlj,");
                    myValueSql.Append("'" + hs["zjwjlj"].ToString() + "',");
                }
                if (hs.ContainsKey("xh"))
                {
                    myFiledSql.Append("xh");
                    myValueSql.Append("'" + hs["xh"].ToString() + "'");
                }

                string mySql = "insert into t_tgs_capture(" + myFiledSql.ToString() + ") values (" + myValueSql.ToString() + ")";
                int result = dataAccess.Execute_NonQuery(mySql.ToString());

                return result;
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
            StringBuilder mySql = new StringBuilder();
            try
            {
                mySql.Append(" update  t_tms_peccancy set ");
                mySql.Append(Common.GetHashtableStr(hs, "hpzl", "hpzl"));
                mySql.Append(Common.GetHashtableStr(hs, "hphm", "hphm"));
                mySql.Append(Common.GetHashtableStr(hs, "wfdd", "wfdd"));
                mySql.Append(Common.GetHashtableStr(hs, "wfdz", "wfdz"));
                mySql.Append(Common.GetHashtableDateOracle(hs, "wfsj", "wfsj"));
                mySql.Append(Common.GetHashtableStr(hs, "wfxw", "wfxw"));
                mySql.Append(Common.GetHashtableStr(hs, "shbj", "shbj"));
                mySql.Append(Common.GetHashtableStr(hs, "shyh", "shyh"));
                mySql.Append(Common.GetHashtableDateOracle(hs, "shsj", "shsj"));
                mySql.Append(Common.GetHashtableStr(hs, "cfbj", "cfbj"));
                mySql.Append(Common.GetHashtableStr(hs, "cfyh", "cfyh"));
                mySql.Append(Common.GetHashtableDateOracle(hs, "cfsj", "cfsj"));
                mySql.Append(Common.GetHashtableStr(hs, "jzzt", "jzzt"));
                mySql.Append(Common.GetHashtableStr(hs, "jzyh", "jzyh"));
                mySql.Append(" jzsj=now(),");
                mySql.Append(Common.GetHashtableStr(hs, "clzl", "clzl"));
                mySql.Append(Common.GetHashtableStr(hs, "zqmj", "zqmj"));
                mySql.Append(Common.GetHashtableStr(hs, "clsd", "clsd"));
                mySql.Append(Common.GetHashtableStr(hs, "clxs", "clxs"));
                mySql.Append(Common.GetHashtableStr(hs, "csys", "csys"));
                mySql.Append(Common.GetHashtableStr(hs, "clpp", "clpp"));
                mySql.Append(Common.GetHashtableStr(hs, "jdssyr", "jdssyr"));
                mySql.Append(Common.GetHashtableStr(hs, "zsxxdz", "zsxxdz"));
                mySql.Append(Common.GetHashtableStr(hs, "lxfs", "lxfs"));
                mySql.Append(Common.GetHashtableStr(hs, "dh", "dh"));
                mySql.Append(Common.GetHashtableStr(hs, "yzbm", "yzbm"));
                mySql.Append(Common.GetHashtableStr(hs, "cllx", "cllx"));
                mySql.Append(Common.GetHashtableStr(hs, "zt", "zt"));
                mySql.Append(Common.GetHashtableStr(hs, "fdjh", "fdjh"));
                mySql.Append(Common.GetHashtableDateOracle(hs, "jyyxqz", "jyyxqz"));
                mySql.Append(Common.GetHashtableStr(hs, "clxh", "clxh"));
                mySql.Append(Common.GetHashtableStr(hs, "clsbdh", "clsbdh"));
                mySql.Append(Common.GetHashtableStr(hs, "sfzmhm", "sfzmhm"));
                mySql.Append(Common.GetHashtableStr(hs, "dabh", "dabh"));
                mySql.Append("jcbj='" + hs["jcbj"].ToString() + "'");
                mySql.Append(" where xh='" + hs["xh"] + "' ");
                int result = dataAccess.Execute_NonQuery(mySql.ToString());
                return result;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///获取平均速度流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable PassCarAveSpeed(string directionId, string date)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                string[] str = directionId.Split('|');
                mySql.Append(" select  CONCAT(F_GET_kkms ('" + str[0] + "'),IFNULL(f_get_fxbhms(CONCAT('" + str[0] + "','" + str[1] + "')), f_to_name('240025', '" + str[1] + "'))) as fxbhms,b.time_id AS xs,CONVERT(IFNULL(pjsd,'0'),DECIMAL) as pjsd from ");
                mySql.Append("  (select fxbh,xs,CEIL(SUM(pjsd)/COUNT(*)) AS pjsd from t_tgs_flow_5min WHERE rq = STR_TO_DATE('" + date + "', '%Y-%m-%d') and kkid= '" + str[0] + "'  and fxbh='" + str[1] + "'");
                mySql.Append("  GROUP BY fxbh,xs) a RIGHT JOIN t_cfg_time b ON b.time_id = a.xs");
                mySql.Append(" WHERE b.time_type = '1' ORDER BY xs ASC");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获取最高速度流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable PassCarHighSpeed(string directionId, string date)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                string[] str = directionId.Split('|');
                mySql.Append(" select  CONCAT(F_GET_kkms ('" + str[0] + "'), IFNULL(f_get_fxbhms(CONCAT('" + str[0] + "', '" + str[1] + "')), f_to_name('240025', '" + str[1] + "'))) as fxbhms,b.time_id AS xs,CONVERT(IFNULL(zgsd,'0'),DECIMAL) as zgsd from ");
                mySql.Append("  (select fxbh,xs,MAX(zgsd) AS zgsd from t_tgs_flow_5min where rq=STR_TO_DATE('" + date + "', '%Y-%m-%d') and kkid= '" + str[0] + "'  and fxbh='" + str[1] + "'");
                mySql.Append("  GROUP BY fxbh,xs) a RIGHT JOIN t_cfg_time b ON b.time_id = a.xs ");
                mySql.Append(" WHERE b.time_type = '1' ORDER BY xs ASC");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获取识别率信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable PassCarOcr(string directionId, string date)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                string[] str = directionId.Split('|');
                mySql.Append(" select  CONCAT(F_GET_kkms ('" + str[0] + "'),IFNULL(f_get_fxbhms(CONCAT('" + str[0] + "', '" + str[1] + "')), f_to_name('240025', '" + str[1] + "'))) as fxbhms,b.time_id AS xs,ROUND(IFNULL(sbsl, '0') / IFNULL(ll, '1'), 4)* 100 AS sbl from ");
                mySql.Append("  (select fxbh,xs,SUM(ll) AS ll,SUM(sbsl) AS sbsl from t_tgs_flow_5min where rq=STR_TO_DATE('" + date + "','%Y-%m-%d') and kkid= '" + str[0] + "'  and fxbh='" + str[1] + "'");
                mySql.Append("  GROUP BY fxbh,xs) a RIGHT JOIN t_cfg_time b ON b.time_id = a.xs");
                mySql.Append(" WHERE b.time_type = '1' ORDER BY xs ASC");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得布控车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetblackList(string where, int currentPage)//currentPage序号
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select *  from vt_tgs_info_blacklist where  " + where + " limit " + currentPage + ",15";

                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得专项布控信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable Getspecial(string where, int currentPage)//currentPage序号
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select t1.ZXBH,BKLX,t2.CODEDESC AS BKLXMS,t1.BKMS,t1.ZT,t3.CODEDESC AS BKZT,date_format(t1.YXSJ, '%Y-%m-%d') AS YXSJ,t1.BKRID,t1.BKRXM,t1.BKJSR,t1.BkJSRMS,t1.BKLXR,t1.BKLXDH,t1.GXSJ,t1.BKFW,t1.BKFWMS ";
                mySql = mySql + "from t_tgs_suspect_special t1 JOIN t_sys_code t2 ON t1.BKLX = t2.CODE AND t2.CODETYPE = '300100' ";
                mySql = mySql + "JOIN t_sys_code t3 ON t1.ZT = t3.CODE AND t3.CODETYPE = '240011' ";
                mySql = mySql + "where " + where + " limit " + currentPage + ",15";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得流量报警信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable Getport(string where, int currentPage)//currentPage序号
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select *  from t_tgs_flowalarm_setting where  " + where + " ORDER BY KKBH,KKFX limit " + currentPage + ",15";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得布控车辆信息总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetblackListCount(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select Count(*)  from vt_tgs_info_blacklist where  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得布控车辆信息总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetspecialCount(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select Count(*)  from t_tgs_suspect_special where  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得流量报警信息总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetportCount(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select Count(*)  from t_tgs_flowalarm_setting where  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///查询布控车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSuspicion(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select *  from vt_tgs_info_suspicion  where  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///查询畅行车辆
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetCheckless(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select *  from vt_tgs_info_Checkless  where  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获得特殊勤务车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetExtraList(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select *  from vt_tgs_info_extralist where  " + where;
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
        public int UpdateBlacklistInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_tgs_info_Blacklist", "xh", hs["xh"].ToString()) > 0)
                {
                    mySql = "update  t_tgs_info_Blacklist  set ";
                    mySql = mySql + Common.GetHashtableStr(hs, "hpzl", "hpzl");
                    mySql = mySql + Common.GetHashtableStr(hs, "csys", "csys");
                    mySql = mySql + Common.GetHashtableStr(hs, "mdlx", "mdlx");
                    mySql = mySql + Common.GetHashtableStr(hs, "sjly", "sjly");
                    mySql = mySql + Common.GetHashtableStr(hs, "bdbj", "bdbj");
                    mySql = mySql + "hphm='" + hs["hphm"].ToString() + "',";
                    mySql = mySql + "clpp='" + hs["clpp"].ToString() + "',";
                    mySql = mySql + "yxsj=STR_TO_DATE('" + hs["yxsj"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                    mySql = mySql + "sjyy='" + hs["sjyy"].ToString() + "',";
                    mySql = mySql + "bz='" + hs["bz"].ToString() + "',";
                    mySql = mySql + "gxsj=sysdate";
                    mySql = mySql + " where xh='" + hs["xh"].ToString() + "'";
                    return dataAccess.Execute_NonQuery(mySql);
                }
                else
                {
                    if (GeHphmExist("t_tgs_info_Blacklist", hs["hphm"].ToString(), hs["hpzl"].ToString()) <= 0)
                    {
                        mySql = "insert into  t_tgs_info_Blacklist (xh,hphm,hpzl,csys,clpp,mdlx,yxsj,sjyy,sjly,bdbj,bz,gxsj) values(";
                        mySql = mySql + "'" + hs["xh"].ToString() + "',";
                        mySql = mySql + "'" + hs["hphm"].ToString() + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "hpzl", "02") + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "csys", "") + "',";
                        mySql = mySql + "'" + hs["clpp"].ToString() + "',";
                        mySql = mySql + "'" + hs["mdlx"].ToString() + "',";
                        mySql = mySql + "STR_TO_DATE('" + hs["yxsj"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                        mySql = mySql + "'" + hs["sjyy"].ToString() + "',";
                        mySql = mySql + "'" + hs["sjly"].ToString() + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "bdbj", "0") + "',";
                        mySql = mySql + "'" + hs["bz"].ToString() + "',";
                        mySql = mySql + "sysdate )";
                        return dataAccess.Execute_NonQuery(mySql);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新/添加专项布控信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateSpecialInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_tgs_suspect_special", "ZXBH", hs["ZXBH"].ToString()) > 0)
                {
                    mySql = "update  t_tgs_suspect_special  set ";
                    mySql = mySql + Common.GetHashtableStr(hs, "ZXLX", "BKLX");//专项类型
                    mySql = mySql + Common.GetHashtableStr(hs, "BKZT", "ZT");//布控状态
                    mySql = mySql + "BKMS='" + hs["ZXMS"].ToString() + "',";//专项描述
                    mySql = mySql + "BKFW='" + hs["BKFW"].ToString() + "',";//布控范围编号
                    mySql = mySql + "BKFWMS='" + hs["BKFWMS"].ToString() + "',";//布控范围描述
                    mySql = mySql + "BKRID='" + hs["BKRID"].ToString() + "',";//布控人编号
                    mySql = mySql + "BKRXM='" + hs["BKRXM"].ToString() + "',";//布控人姓名
                    mySql = mySql + "BKJSR='" + hs["BKJSR"].ToString() + "',";//布控接收人编号
                    mySql = mySql + "BKJSRMS='" + hs["BKJSRMS"].ToString() + "',";//布控接收人姓名
                    mySql = mySql + "BKLXR='" + hs["BKLXR"].ToString() + "',";//布控联系人
                    mySql = mySql + "BKLXDH='" + hs["BKLXFS"].ToString() + "',";//布控联系电话
                    mySql = mySql + "YXSJ=STR_TO_DATE('" + hs["YSSJ"].ToString() + "','%Y-%m-%d %H:%i:%s'),";//有效时间
                    mySql = mySql + "GXSJ=" + "now()";//更新时间
                    mySql = mySql + " where ZXBH='" + hs["ZXBH"].ToString() + "'";//更新条件
                    return dataAccess.Execute_NonQuery(mySql);
                }
                else
                {
                    if (GeZXNRExist("t_tgs_suspect_special", hs["ZXLX"].ToString(), hs["ZXMS"].ToString()) >= 0)
                    {
                        mySql = "insert into  t_tgs_suspect_special (ZXBH,BKLX,BKMS,BKFW,BKFWMS,BKRID,BKRXM,BKJSR,BKJSRMS,BKLXR,BKLXDH,YXSJ,ZT,GXSJ) values(";
                        mySql = mySql + "'" + hs["ZXBH"].ToString() + "',";
                        mySql = mySql + "'" + hs["ZXLX"].ToString() + "',";
                        mySql = mySql + "'" + hs["ZXMS"].ToString() + "',";
                        mySql = mySql + "'" + hs["BKFW"].ToString() + "',";
                        mySql = mySql + "'" + hs["BKFWMS"].ToString() + "',";
                        mySql = mySql + "'" + hs["BKRID"].ToString() + "',";
                        mySql = mySql + "'" + hs["BKRXM"].ToString() + "',";
                        mySql = mySql + "'" + hs["BKJSR"].ToString() + "',";
                        mySql = mySql + "'" + hs["BKJSRMS"].ToString() + "',";
                        mySql = mySql + "'" + hs["BKLXR"].ToString() + "',";
                        mySql = mySql + "'" + hs["BKLXFS"].ToString() + "',";
                        mySql = mySql + "STR_TO_DATE('" + hs["YSSJ"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "BKZT", "0") + "',";
                        mySql = mySql + "now()" + ")";

                        return dataAccess.Execute_NonQuery(mySql);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 流量报警信息更新
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdatePortInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_tgs_flowalarm_setting", "BH", hs["BH"].ToString()) > 0)
                {
                    mySql = "update  t_tgs_flowalarm_setting  set ";
                    mySql = mySql + "KKBH='" + hs["KKBH"].ToString() + "',";
                    mySql = mySql + "KSSD='" + hs["KSSD"].ToString() + "',";
                    mySql = mySql + "JSSD='" + hs["JSSD"].ToString() + "',";
                    mySql = mySql + "TJZQ='" + hs["TJZQ"].ToString() + "',";
                    mySql = mySql + "BJFZ='" + hs["BJFZ"].ToString() + "',";
                    mySql = mySql + "KKPZR='" + hs["KKPZR"].ToString() + "',";
                    mySql = mySql + "KKPZRMS='" + hs["KKPZRMS"].ToString() + "',";
                    mySql = mySql + "KKBHMS='" + hs["KKBHMS"].ToString() + "',";
                    mySql = mySql + "KKFX='" + hs["KKFXBH"].ToString() + "',";
                    mySql = mySql + "KKFXMS='" + hs["KKFXMS"].ToString() + "',";
                    mySql = mySql + "GXSJ= now()";
                    mySql = mySql + " where BH='" + hs["BH"].ToString() + "'";
                    return dataAccess.Execute_NonQuery(mySql);
                }
                else
                {
                    if (GeKKBHExist("t_tgs_flowalarm_setting", hs["BH"].ToString(), hs["BH"].ToString()) >= 0)
                    {
                        mySql = "insert into  t_tgs_flowalarm_setting (BH,KKBH,KSSD,JSSD,TJZQ,BJFZ,KKPZR,KKBHMS,KKPZRMS,KKFX,KKFXMS,GXSJ) values(";
                        mySql = mySql + "'" + hs["BH"].ToString() + "',";
                        mySql = mySql + "'" + hs["KKBH"].ToString() + "',";
                        mySql = mySql + "'" + hs["KSSD"].ToString() + "',";
                        mySql = mySql + "'" + hs["JSSD"].ToString() + "',";
                        mySql = mySql + "'" + hs["TJZQ"].ToString() + "',";
                        mySql = mySql + "'" + hs["BJFZ"].ToString() + "',";
                        mySql = mySql + "'" + hs["KKPZR"].ToString() + "',";
                        mySql = mySql + "'" + hs["KKBHMS"].ToString() + "',";
                        mySql = mySql + "'" + hs["KKPZRMS"].ToString() + "',";
                        mySql = mySql + "'" + hs["KKFXBH"].ToString() + "',";
                        mySql = mySql + "'" + hs["KKFXMS"].ToString() + "',";
                        mySql = mySql + "now() )";
                        return dataAccess.Execute_NonQuery(mySql);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        ///更新车辆布控信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateSuspicionInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_tgs_info_blacklist", "xh", hs["xh"].ToString()) > 0)
                {
                    mySql = "update  t_tgs_info_blacklist  set ";
                    mySql = mySql + Common.GetHashtableStr(hs, "hpzl", "hpzl");
                    mySql = mySql + Common.GetHashtableStr(hs, "csys", "csys");
                    mySql = mySql + Common.GetHashtableStr(hs, "mdlx", "mdlx");
                    mySql = mySql + Common.GetHashtableStr(hs, "sjyy", "sjyy");
                    mySql = mySql + Common.GetHashtableStr(hs, "bdbj", "bdbj");
                    mySql = mySql + "hphm='" + hs["hphm"].ToString() + "',";
                    mySql = mySql + "clpp='" + hs["clpp"].ToString() + "',";
                    mySql = mySql + "yxsj=STR_TO_DATE('" + hs["yxsj"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                    mySql = mySql + "BKRID='" + hs["BKRID"].ToString() + "',";
                    mySql = mySql + "BKRXM='" + hs["BKRXM"].ToString() + "',";
                    mySql = mySql + "bz='" + hs["bz"].ToString() + "',";
                    mySql = mySql + "bkr='" + hs["bkr"].ToString() + "',";
                    mySql = mySql + "bkdh='" + hs["bkdh"].ToString() + "',";
                    mySql = mySql + "bjjsr='" + hs["bjjsr"].ToString() + "',";
                    mySql = mySql + "cpmh='" + hs["cpmh"].ToString() + "',";
                    mySql = mySql + "bjjsr='" + hs["bjjsr"].ToString() + "',";
                    mySql = mySql + "gxsj=now(),";
                    mySql = mySql + "bklx='1' ";
                    mySql = mySql + " where xh='" + hs["xh"].ToString() + "'";
                    return dataAccess.Execute_NonQuery(mySql);
                }
                else
                {
                    if (GeHphmExist("t_tgs_info_blacklist", hs["hphm"].ToString(), hs["hpzl"].ToString()) >= 0)
                    {
                        mySql = "insert into  t_tgs_info_blacklist (xh,hphm,hpzl,csys,clpp,mdlx,YXSJ,BKRID,BKRXM,sjyy,bdbj,bkr,bkdh,cpmh,bz,bjjsr,bjjsrms,gxsj,bklx) values(";
                        mySql = mySql + "'" + hs["xh"].ToString() + "',";
                        mySql = mySql + "'" + hs["hphm"].ToString() + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "hpzl", "02") + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "csys", "") + "',";
                        mySql = mySql + "'" + hs["clpp"].ToString() + "',";
                        mySql = mySql + "'" + hs["mdlx"].ToString() + "',";
                        mySql = mySql + "STR_TO_DATE('" + hs["yxsj"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                        mySql = mySql + "'" + hs["BKRID"].ToString() + "',";
                        mySql = mySql + "'" + hs["BKRXM"].ToString() + "',";
                        mySql = mySql + "'" + hs["sjyy"].ToString() + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "bdbj", "0") + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "bkr", "") + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "bkdh", "") + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "cpmh", "") + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "bz", "") + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "bjjsr", "") + "',";
                        mySql = mySql + "'" + hs["bjjsrms"].ToString() + "',";
                        mySql = mySql + "now() " + ",'1" + "')";
                        return dataAccess.Execute_NonQuery(mySql);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///更新畅行车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateChecklessInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_tgs_info_Checkless", "xh", hs["xh"].ToString()) > 0)
                {
                    mySql = "update  t_tgs_info_Checkless  set ";
                    mySql = mySql + Common.GetHashtableStr(hs, "hpzl", "hpzl");
                    mySql = mySql + Common.GetHashtableStr(hs, "csys", "csys");
                    mySql = mySql + Common.GetHashtableStr(hs, "mdlx", "mdlx");
                    mySql = mySql + Common.GetHashtableStr(hs, "sjly", "sjly");
                    mySql = mySql + Common.GetHashtableStr(hs, "bdbj", "bdbj");
                    mySql = mySql + "hphm='" + hs["hphm"].ToString() + "',";
                    mySql = mySql + "clpp='" + hs["clpp"].ToString() + "',";
                    mySql = mySql + "yxsj=STR_TO_DATE('" + hs["yxsj"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                    mySql = mySql + "sjyy='" + hs["sjyy"].ToString() + "',";
                    mySql = mySql + "bz='" + hs["bz"].ToString() + "',";
                    mySql = mySql + "gxsj=now()";
                    mySql = mySql + " where xh='" + hs["xh"].ToString() + "'";
                    return dataAccess.Execute_NonQuery(mySql);
                }
                else
                {
                    if (GeHphmExist("t_tgs_info_Checkless", hs["hphm"].ToString(), hs["hpzl"].ToString()) <= 0)
                    {
                        mySql = "insert into  t_tgs_info_Checkless (xh,hphm,hpzl,csys,clpp,mdlx,yxsj,sjyy,sjly,bdbj,bz,gxsj) values(";
                        mySql = mySql + "'" + hs["xh"].ToString() + "',";
                        mySql = mySql + "'" + hs["hphm"].ToString() + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "hpzl", "02") + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "csys", "") + "',";
                        mySql = mySql + "'" + hs["clpp"].ToString() + "',";
                        mySql = mySql + "'" + hs["mdlx"].ToString() + "',";
                        mySql = mySql + "STR_TO_DATE('" + hs["yxsj"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                        mySql = mySql + "'" + hs["sjyy"].ToString() + "',";
                        mySql = mySql + "'" + hs["sjly"].ToString() + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "bdbj", "0") + "',";
                        mySql = mySql + "'" + hs["bz"].ToString() + "',";
                        mySql = mySql + "now() )";
                        return dataAccess.Execute_NonQuery(mySql);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///更新特殊勤务车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateExtraListInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_tgs_info_extralist", "xh", hs["xh"].ToString()) > 0)
                {
                    mySql = "update  t_tgs_info_extralist  set ";
                    mySql = mySql + Common.GetHashtableStr(hs, "hpzl", "hpzl");
                    mySql = mySql + Common.GetHashtableStr(hs, "csys", "csys");
                    mySql = mySql + Common.GetHashtableStr(hs, "mdlx", "mdlx");
                    mySql = mySql + Common.GetHashtableStr(hs, "sjly", "sjly");
                    mySql = mySql + Common.GetHashtableStr(hs, "bdbj", "bdbj");
                    mySql = mySql + "hphm='" + hs["hphm"].ToString() + "',";
                    mySql = mySql + "clpp='" + hs["clpp"].ToString() + "',";
                    mySql = mySql + "yxsj=STR_TO_DATE('" + hs["yxsj"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                    mySql = mySql + "bz='" + hs["bz"].ToString() + "',";
                    mySql = mySql + "gxsj=now()";
                    mySql = mySql + " where xh='" + hs["xh"].ToString() + "'";
                    return dataAccess.Execute_NonQuery(mySql);
                }
                else
                {
                    if (GeHphmExist("t_tgs_info_extralist", hs["hphm"].ToString(), hs["hpzl"].ToString()) <= 0)
                    {
                        mySql = "insert into  t_tgs_info_extralist (xh,hphm,hpzl,csys,clpp,mdlx,yxsj,sjyy,sjly,bdbj,bz,gxsj) values(";
                        mySql = mySql + "'" + hs["xh"].ToString() + "',";
                        mySql = mySql + "'" + hs["hphm"].ToString() + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "hpzl", "02") + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "csys", "") + "',";
                        mySql = mySql + "'" + hs["clpp"].ToString() + "',";
                        mySql = mySql + "'" + hs["mdlx"].ToString() + "',";
                        mySql = mySql + "STR_TO_DATE('" + hs["yxsj"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                        mySql = mySql + "'" + hs["sjyy"].ToString() + "',";
                        mySql = mySql + "'" + hs["sjly"].ToString() + "',";
                        mySql = mySql + "'" + Common.GetHashtableValue(hs, "bdbj", "0") + "',";
                        mySql = mySql + "'" + hs["bz"].ToString() + "',";
                        mySql = mySql + "now() )";
                        return dataAccess.Execute_NonQuery(mySql);
                    }
                    else
                    {
                        return 0;
                    }
                }
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
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteBlacklistInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_tgs_info_Blacklist  where xh='" + hs["xh"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///删除布控车辆
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteSuspicionInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                //把表t_tgs_info_suspicion改成t_tgs_info_blacklist
                mySql = "delete  from  t_tgs_info_blacklist  where xh='" + hs["xh"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 删除专项布控车辆
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteSpecialInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_tgs_suspect_special  where ZXBH='" + hs["ZXBH"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 删除流量报警信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeletePortInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_tgs_flowalarm_setting  where BH='" + hs["BH"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///删除流量报警车辆
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteAlarmportSettingInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                //把表t_tgs_info_suspicion改成t_tgs_flowalarm_setting
                mySql = "delete  from  t_tgs_flowalarm_setting  where BH='" + hs["BH"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///删除白名单车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteChecklessInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from  t_tgs_info_Checkless  where xh='" + hs["xh"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 删除特殊勤务车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteExtraListInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from  t_tgs_info_extralist  where xh='" + hs["xh"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
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
        public DataTable GetPeccancyInfo(string field, string where, int startrow, int endRow)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select " + field + " FROM (SELECT * FROM t_tms_peccancy t    WHERE   " + where + " and t.hphm not in ( select hphm from t_tgs_info_checkless a  where a.hpzl=t.hpzl) ORDER BY wfsj DESC  limit   " + startrow.ToString() + "," + (endRow - startrow).ToString() + " ) t1    ";
                //ILog.WriteErrorLog(mySql);
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT count(*) FROM t_tms_peccancy t    WHERE   " + where + "and t.hphm not in ( select hphm from t_tgs_info_checkless a  where a.hpzl=t.hpzl) ORDER BY wfsj DESC    ";
                //ILog.WriteErrorLog(mySql);
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
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyWorkNum(string field, string where)
        {
            //string mySql = string.Empty;
            //try
            //{
            //    string where2 = string.Empty;
            //    switch (field.ToLower())
            //    {
            //        case "shyh":
            //            where2 = "shbj!=0  and IFNULL(shyh,'*')!='*' and " + where;
            //            mySql = "select sjly,sjlyms,shyh, g_count , decode(all_count, 0, 0, round(g_count / all_count * 100, 2)) as bfb from ( select sjly,f_to_name ('240022', sjly) as sjlyms,shyh,count(1) as g_count from t_tms_peccancy  where " + where2 + "   group by sjly,shyh) t1, (select sjly as sjly2,count(1)  as all_count from t_tms_peccancy where " + where2 + " group by sjly) where sjly=sjly2";
            //            break;
            //        case "jzyh":
            //            where2 = " jzzt!=0  and IFNULL(jzyh,'*')!='*' and " + where;
            //            mySql = "select sjly,sjlyms,jzyh, g_count , decode(all_count, 0, 0, round(g_count / all_count * 100, 2)) as bfb from ( select sjly,f_to_name ('240022', sjly) as sjlyms,jzyh,count(1) as g_count from t_tms_peccancy  where " + where2 + " group by sjly,jzyh) t1, (select sjly as sjly2, count(1)  as all_count from t_tms_peccancy where " + where2 + " group by sjly) where sjly=sjly2";
            //            break;
            //        case "cfyh":
            //            where2 = " cfbj!=0  and IFNULL(cfyh,'*')!='*' and " + where;
            //            mySql = "select sjly,sjlyms,cfyh, g_count, decode(all_count, 0, 0, round(g_count / all_count * 100, 2)) as bfb from ( select sjly,f_to_name ('240022', sjly) as sjlyms,cfyh,count(1) as g_count from t_tms_peccancy  where " + where2 + "  group by sjly,cfyh) t1, (select sjly as sjly2, count(1)  as all_count from t_tms_peccancy where " + where2 + " group by sjly) where sjly=sjly2";
            //            break;
            //        case "cjyh":
            //            mySql = "select sjly,sjlyms,f_get_value ('station_name','t_cfg_set_station','station_id', cjyh) as cjyh, g_count, decode(all_count, 0, 0, round(g_count / all_count * 100, 2)) as bfb from ( select sjly,f_to_name ('240022', sjly) as sjlyms,cjyh as cjyh,count(1) as g_count from t_tms_peccancy  where " + where + " group by sjly,cjyh) t1, (select sjly as sjly2, count(1)  as all_count from t_tms_peccancy where " + where + " group by sjly) where sjly=sjly2";
            //            break;

            //    }

            try
            {
                StringBuilder mySql = new StringBuilder();
                string typeName = "";
                switch (field)
                {
                    case "cjyh":
                        typeName = "cjyh||'--'||cjyhms";
                        break;

                    case "jzyh":
                        typeName = "jzyh||'--'||jzyhms";
                        break;

                    case "shyh":
                        typeName = "shyh||'--'||shyhms";
                        break;

                    default:
                        typeName = field + "ms";
                        break;
                }
                if (field == "cjyh")
                {
                    mySql.Append(" select IFNULL(CJYH, 'null')||'--'||IFNULL(CJYHMS, 'null') as 采集用户,");
                    mySql.Append(" IFNULL(allCount, 0) as 采集总数,");
                    mySql.Append(" IFNULL(cjwsh, 0) as 未审核,");
                    mySql.Append(" IFNULL(cjyxs, 0) as 采集有效,");
                    mySql.Append(" IFNULL(cjwxs, 0) as 采集无效,");
                    mySql.Append(" decode(allCount, 0,1,  round(IFNULL(cjyxs, 0) / allCount * 100, 2))||'%' as 采集有效率");
                    mySql.Append(" from (select t.CJYH,t.CJYHMS,");
                    mySql.Append(" count(1) as allCount,");
                    mySql.Append(" sum(decode(trim(t.shbj), '0', 1, 0)) as cjwsh,");
                    mySql.Append(" sum(decode(trim(t.shbj), '1', 1, 0)) as cjyxs,");
                    mySql.Append(" sum(decode(trim(t.shbj), '2', 1, 0)) as cjwxs");
                    mySql.Append(" from vt_tms_peccancy t where " + where + " group by t.CJYH,t.CJYHMS) a,");
                    mySql.Append(" (select name from t_ser_person) b");
                    mySql.Append(" where b.name(+) = a.CJYH");
                }
                else if (field == "shyh")
                {
                    mySql.Append(" select  IFNULL(SHYH, 'null') ||'--'||IFNULL(SHYHMS, 'null')as 初审用户,");
                    mySql.Append(" IFNULL(allCount, 0) as 初审总数,");
                    mySql.Append(" IFNULL(shyxs, 0) as 初审有效数,");
                    mySql.Append(" IFNULL(shwxs, 0) as 初审无效数,");
                    mySql.Append(" decode(allCount, 0,'-', round(IFNULL(shyxs, 0) / allCount * 100, 2))||'%' as 初审有效率,");
                    mySql.Append(" IFNULL(jznxs, 0) as 未复审数,");
                    mySql.Append(" jzyxs+jzwxs as 复审采用数,");
                    mySql.Append(" IFNULL(jzyxs, 0) as 复审有效数,");
                    mySql.Append(" IFNULL(jzwxs, 0) as 复审无效数,");
                    mySql.Append(" decode(jzyxs, 0, '-', round(IFNULL(jzyxs, 0) / （jzyxs+jzwxs） * 100, 2))||'%' as 复审采用率");
                    mySql.Append(" from (select t.SHYH,t.SHYHMS,");
                    mySql.Append(" count(1) as allCount,");
                    mySql.Append(" sum(decode(trim(t.shbj), '0', 1, 0)) as wshs,");
                    mySql.Append(" sum(decode(trim(t.shbj), '1', 1, 0)) as shyxs,");
                    mySql.Append(" sum(decode(trim(t.shbj), '2', 1, 0)) as shwxs,");
                    mySql.Append(" sum(decode(trim(t.jcbj), '1', 1, 0)) as jznxs,");
                    mySql.Append(" sum(decode(trim(t.jcbj), '3', 1, 0)) as jzyxs,");
                    mySql.Append(" sum(decode(trim(t.jcbj), '4', 1, 0)) as jzwxs");
                    mySql.Append(" from vt_tms_peccancy t where " + where + " and t.shbj<>0 group by t.SHYH,t.SHYHMS) a,");
                    mySql.Append(" (select name from t_ser_person) b");
                    mySql.Append(" where b.name(+) = a.SHYH");
                }
                else if (field == "jzyh")
                {
                    mySql.Append(" select  IFNULL(JZYH, 'null') ||'--'||IFNULL(JZYHMS, 'null')as 复审用户,");
                    mySql.Append(" IFNULL(allCount, 0) as 复审总数,");
                    mySql.Append(" IFNULL(jzyxs, 0) as 复审有效,");
                    mySql.Append(" IFNULL(jzwxs, 0) as 复审无效,");
                    mySql.Append(" decode(allCount, 0,1, round(IFNULL(jzyxs, 0) / allCount * 100, 2))||'%' as 复审有效率,");
                    mySql.Append(" decode(allCount, 0,1, round(IFNULL(jzwxs, 0) / allCount * 100, 2))||'%' as 复审纠正率");
                    mySql.Append(" from (select t.JZYH,t.JZYHMS,");
                    mySql.Append(" count(1) as allCount,");
                    mySql.Append(" sum(decode(trim(t.jcbj), '1', 1, 0)) as jznxs,");
                    mySql.Append(" sum(decode(trim(t.jcbj), '3', 1, 0)) as jzyxs,");
                    mySql.Append(" sum(decode(trim(t.jcbj), '4', 1, 0)) as jzwxs");
                    mySql.Append(" from vt_tms_peccancy t where " + where + "  and t.jcbj>2  group by t.JZYH,t.JZYHMS) a,");
                    mySql.Append(" (select name from t_ser_person) b");
                    mySql.Append(" where b.name(+) = a.JZYH  and jzyxs>0 ");
                }
                else
                {
                    mySql.Append(" select  name, g_count, decode(all_count, 0,1, round(g_count / all_count * 100, 2)) as bfb");
                    mySql.Append(" from (select IFNULL(" + typeName + "," + field + ") as name, count(1) as g_count");
                    mySql.Append(" from vt_tms_peccancy where " + where + "  group by IFNULL(" + typeName + "," + field + ")),");
                    mySql.Append(" (select count(1) as all_count from vt_tms_peccancy where " + where + ")");
                }
                return dataAccess.Get_DataTable(mySql.ToString());
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
        public int UnlockOneInfo(string xh)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_tms_peccancy set sdr='',sdsj=null where xh='" + xh + "'";

                int result = dataAccess.Execute_NonQuery(mySql);

                return result;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = "update t_tms_peccancy set sdr=null,sdsj=null where sdsj<=STR_TO_DATE('" + sdsj + "','%Y-%m-%d %H:%i:%s') or sdr='" + sdr + "'";
                int result = dataAccess.Execute_NonQuery(mySql);
                return result;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 将所有超时的违法记录进行解锁
        /// </summary>
        /// <param name="sdsj"></param>
        /// <param name="sdr"></param>
        /// <returns></returns>
        public int UnlockAll(string sdsj, string sdr)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update t_tms_peccancy set sdr='',sdsj=null where sdsj<=STR_TO_DATE('" + sdsj + "','%Y-%m-%d %H:%i:%s') and sdr='" + sdr + "'";
                int result = dataAccess.Execute_NonQuery(mySql);
                return result;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            StringBuilder mySql = new StringBuilder();
            try
            {
                mySql.Append(" update t_tms_peccancy");
                mySql.Append(" set sdr = '" + sdr + "', sdsj = STR_TO_DATE('" + sdsj + "','%Y-%m-%d %H:%i:%s')");
                mySql.Append(" where xh in (SELECT xh FROM((SELECT  xh  FROM(SELECT xh FROM t_tms_peccancy   ");
                mySql.Append(" where " + where + " and length(sdr)<1 ORDER BY wfsj ASC) t1  LIMIT 0, 50) t2 ))");
                int result = dataAccess.Execute_NonQuery(mySql.ToString());
                return result;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
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
            StringBuilder mySql = new StringBuilder();
            try
            {
                mySql.Append(" update  t_tms_peccancy  a  INNER JOIN ");
                mySql.Append("(SELECT  xh  FROM(SELECT xh FROM t_tms_peccancy  t  ");
                mySql.Append(" where " + where + " and ifnull(sdr,'1')='1' AND t.hphm NOT IN ( SELECT hphm FROM t_tgs_info_checkless a  WHERE a.hpzl=t.hpzl)  ORDER BY wfsj ASC) t1  LIMIT 0, " + lockAmount.ToString() + ") b  ON a.xh = b.xh  ");
                mySql.Append(" set sdr = '" + sdr + "', sdsj = STR_TO_DATE('" + sdsj + "','%Y-%m-%d %H:%i:%s')");
                int result = dataAccess.Execute_NonQuery(mySql.ToString());
                return result;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int CheckPeccancyInfo(System.Collections.Hashtable hs)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                mySql.Append(" update  t_tms_peccancy set ");
                mySql.Append(Common.GetHashtableStr(hs, "hpzl", "hpzl"));
                mySql.Append(Common.GetHashtableStr(hs, "hphm", "hphm"));
                mySql.Append(Common.GetHashtableStr(hs, "wfdd", "wfdd"));
                mySql.Append(Common.GetHashtableStr(hs, "wfdz", "wfdz"));
                mySql.Append(Common.GetHashtableDateOracle(hs, "wfsj", "wfsj"));
                mySql.Append(Common.GetHashtableStr(hs, "wfxw", "wfxw"));
                mySql.Append(Common.GetHashtableStr(hs, "shbj", "shbj"));
                mySql.Append(Common.GetHashtableStr(hs, "shyh", "shyh"));
                mySql.Append(Common.GetHashtableDateOracle(hs, "shsj", "shsj"));
                mySql.Append(Common.GetHashtableStr(hs, "cfbj", "cfbj"));
                mySql.Append(Common.GetHashtableStr(hs, "cfyh", "cfyh"));
                mySql.Append(Common.GetHashtableDateOracle(hs, "cfsj", "cfsj"));
                mySql.Append(Common.GetHashtableStr(hs, "jzzt", "jzzt"));
                mySql.Append(Common.GetHashtableStr(hs, "jzyh", "jzyh"));
                mySql.Append(" jzsj=sysdate,");
                mySql.Append(Common.GetHashtableStr(hs, "clzl", "clzl"));
                mySql.Append(Common.GetHashtableStr(hs, "zqmj", "zqmj"));
                mySql.Append(Common.GetHashtableStr(hs, "clsd", "clsd"));
                mySql.Append(Common.GetHashtableStr(hs, "clxs", "clxs"));
                mySql.Append(Common.GetHashtableStr(hs, "csys", "csys"));
                mySql.Append(Common.GetHashtableStr(hs, "clpp", "clpp"));
                mySql.Append(Common.GetHashtableStr(hs, "jdssyr", "jdssyr"));
                mySql.Append(Common.GetHashtableStr(hs, "zsxxdz", "zsxxdz"));
                mySql.Append(Common.GetHashtableStr(hs, "lxfs", "lxfs"));
                mySql.Append(Common.GetHashtableStr(hs, "dh", "dh"));
                mySql.Append(Common.GetHashtableStr(hs, "yzbm", "yzbm"));
                mySql.Append(Common.GetHashtableStr(hs, "cllx", "cllx"));
                mySql.Append(Common.GetHashtableStr(hs, "zt", "zt"));
                mySql.Append(Common.GetHashtableStr(hs, "fdjh", "fdjh"));
                mySql.Append(Common.GetHashtableDateOracle(hs, "jyyxqz", "jyyxqz"));
                mySql.Append(Common.GetHashtableStr(hs, "clxh", "clxh"));
                mySql.Append(Common.GetHashtableStr(hs, "clsbdh", "clsbdh"));
                mySql.Append(Common.GetHashtableStr(hs, "sfzmhm", "sfzmhm"));
                mySql.Append(Common.GetHashtableStr(hs, "dabh", "dabh"));
                mySql.Append("jcbj='" + hs["jcbj"].ToString() + "'");
                mySql.Append(" where xh='" + hs["xh"] + "' ");
                int result = dataAccess.Execute_NonQuery(mySql.ToString());
                return result;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 违法数据锁定
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sdr"></param>
        /// <param name="sdsj"></param>
        /// <returns></returns>
        public int PeccancyOnly(Hashtable hs)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                int s = 0;
                string num = settingManager.GetConfigInfo("00", "14").Rows[0]["configvalue"].ToString();
                if (string.IsNullOrEmpty(num))
                {
                    s = 30;
                }
                s = int.Parse(num);
                string startTime = DateTime.Parse(hs["wfsj"].ToString()).AddSeconds(-s).ToString("yyyy-MM-dd HH:mm:ss");
                string endTime = DateTime.Parse(hs["wfsj"].ToString()).AddSeconds(s).ToString("yyyy-MM-dd HH:mm:ss");
                mySql.Append(" select count(*) from t_tms_peccancy");
                mySql.Append(" where  hphm='" + hs["hphm"].ToString() + "'");
                mySql.Append(" and  hpzl='" + hs["hpzl"].ToString() + "'");
                mySql.Append(" and  wfdd='" + hs["wfdd"].ToString() + "'");
                mySql.Append(" and  wfxw='" + hs["wfxw"].ToString() + "'");
                mySql.Append(" and  cjjg='" + hs["cjjg"].ToString() + "'");
                mySql.Append(" and wfsj > = STR_TO_DATE('" + startTime + "','%Y-%m-%d %H:%i:%s')");
                mySql.Append(" and wfsj < = STR_TO_DATE('" + endTime + "','%Y-%m-%d %H:%i:%s')");
                return int.Parse(dataAccess.Get_DataString(mySql.ToString(), 0));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return -1;
            }
        }

        #endregion ITgsDataInfo 成员

        #region ITgsDataInfo 成员

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyAreaMaxWfsj(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select max(wfjssj),min(wfjssj),count(*) from t_tgs_peccancy_area  where " + where + " ";
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
        public DataTable GetPeccancyAreaMaxWfsjCount(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select count(*) from t_tgs_peccancy_area  where " + where + " ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询区间违法信息
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endRow"></param>
        /// <returns></returns>
        public DataTable GetPeccancyAreaInfo(string field, string where, int startrow, int endRow)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  " + field + " FROM (SELECT  t1.id AS id1  FROM  t_tgs_peccancy_area t1  WHERE " + where + " ORDER BY wfjssj DESC LIMIT " + startrow.ToString() + ",15) t2,vt_tgs_peccancy_area t3 WHERE t2.id1=t3.id";

                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = "select hpzl, f_to_name ('140001', hpzl) as hpzlms, DATE_FORMAT(wfjssj, '%Y-%m-%d') AS wfjssj, kskkid, f_get_stationname (kskkid) as kskkmc, jskkid, ";
                mySql = mySql + " f_get_stationname (jskkid) as jskkmc, wfxw, if( wfxw = '0', '正常车辆', f_get_wfxwms (wfxw) ) as wfxwms, ";
                mySql = mySql + "   f_get_cjjg_kkid(jskkid)AS cjjg,  f_get_departname ( f_get_cjjg_kkid(jskkid))  as cjjgms, zs from (select hpzl, date_format(wfjssj, '%Y-%m-%d') as wfjssj, ";
                mySql = mySql + " kskkid, jskkid, wfxw, cjjg, count(*) as zs from t_tgs_peccancy_area  a where   " + where;
                mySql = mySql + " group by DATE_FORMAT(wfjssj, '%Y-%m-%d'), hpzl, kskkid, jskkid, wfxw, cjjg)  b order by wfjssj desc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 统计区间违法信息(最新)
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyAreaCountNew(string where, int startNum, int endNum)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select hpzl, f_to_name ('140001', hpzl) as hpzlms, DATE_FORMAT(wfjssj, '%Y-%m-%d') AS wfjssj, kskkid, f_get_stationname (kskkid) as kskkmc, jskkid, ";
                mySql = mySql + " f_get_stationname (jskkid) as jskkmc, wfxw, if( wfxw = '0', '正常车辆', f_get_wfxwms (wfxw) ) as wfxwms, ";
                mySql = mySql + "   f_get_cjjg_kkid(jskkid)AS cjjg,  f_get_departname ( f_get_cjjg_kkid(jskkid))  as cjjgms, zs from (select hpzl, date_format(wfjssj, '%Y-%m-%d') as wfjssj, ";
                mySql = mySql + " kskkid, jskkid, wfxw, cjjg, count(*) as zs from t_tgs_peccancy_area  a where   " + where;
                mySql = mySql + " group by DATE_FORMAT(wfjssj, '%Y-%m-%d'), hpzl, kskkid, jskkid, wfxw, cjjg)  b order by wfjssj desc limit " + startNum + ",15";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = "select count(*) from (select hpzl, date_format(wfjssj, '%Y-%m-%d') as wfjssj, ";
                mySql = mySql + " kskkid, jskkid, wfxw, cjjg, count(*) as zs from t_tgs_peccancy_area  a where   " + where;
                mySql = mySql + " group by DATE_FORMAT(wfjssj, '%Y-%m-%d'), hpzl, kskkid, jskkid, wfxw, cjjg)  b order by wfjssj desc ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = " select sum(if(trim(hpzl)='01',1,0)) as dxcl,sum(IF(trim(hpzl)= '02',1,0)) as xxcl, ";
                mySql = mySql + " SUM(IF(TRIM(hpzl)= '23',1,0)) AS gacl,SUM(IF(TRIM(hpzl)= '99',1,0)) AS qtcl, ";
                mySql = mySql + " DATE_FORMAT(wfjssj,'%Y-%m-%d') AS wfsj,wfxw,COUNT(*) AS zs FROM  t_tgs_peccancy_area ";
                mySql = mySql + " WHERE " + where + " GROUP BY DATE_FORMAT(wfjssj, '%Y-%m-%d'),hpzl,wfxw ORDER BY wfxw ASC ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                //string mySql = "  select count(*), b. startnum, b.endnum,a.hpzl";
                //mySql = mySql + "from (select * from t_tgs_peccancy_area  " + where + " ) a,";
                //mySql = mySql + "(select (rownum-1) * 10 as startnum, ((rownum-1) * 10 + 10) as endnum";
                //mySql = mySql + " from all_objects where rownum <= 20) b";
                //mySql = mySql + " where a.xssd >= b.startnumand a.xssd < b.endnum";
                //mySql = mySql + "group by b. startnum, b.endnum,hpzl";

                mySql = "  select '000400' sdlx,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='01', 1, 0)),0) as dxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='02', 1, 0)),0) as xxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '23', 1, 0)),0) as gacl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '99', 1, 0)),0) as qtcl";
                mySql = mySql + " from t_tgs_peccancy_area ";
                mySql = mySql + " where  " + where + " and  xssd < 40 union";

                mySql = mySql + " select '040050' sdlx,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='01', 1, 0)),0) as dxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='02', 1, 0)),0) as xxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '23', 1, 0)),0) as gacl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '99', 1, 0)),0) as qtcl";
                mySql = mySql + " from t_tgs_peccancy_area";
                mySql = mySql + " where " + where + " and  xssd >=40 and xssd< 50 union";

                mySql = mySql + " select '050060' sdlx,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='01', 1, 0)),0) as dxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='02', 1, 0)),0) as xxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '23', 1, 0)),0) as gacl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '99', 1, 0)),0) as qtcl";
                mySql = mySql + " from t_tgs_peccancy_area";
                mySql = mySql + " where " + where + "  and xssd >=50 and xssd< 60 union";

                mySql = mySql + "  select '060070' sdlx,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='01', 1, 0)),0) as dxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='02', 1, 0)),0) as xxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '23', 1, 0)),0) as gacl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '99', 1, 0)),0) as qtcl";
                mySql = mySql + " from t_tgs_peccancy_area";
                mySql = mySql + " where " + where + "  and  xssd >=60 and xssd< 70 union";

                mySql = mySql + " select '070080' sdlx,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='01', 1, 0)),0) as dxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='02', 1, 0)),0) as xxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '23', 1, 0)),0) as gacl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '99', 1, 0)),0) as qtcl";
                mySql = mySql + " from t_tgs_peccancy_area";
                mySql = mySql + " where " + where + "  and  xssd >=70 and xssd< 80 union";

                mySql = mySql + " select '080090' sdlx,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='01', 1, 0)),0) as dxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='02', 1, 0)),0) as xxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '23', 1, 0)),0) as gacl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '99', 1, 0)),0) as qtcl";
                mySql = mySql + " from t_tgs_peccancy_area";
                mySql = mySql + " where " + where + "  and  xssd >=80 and xssd< 90 union";

                mySql = mySql + " select '090100' sdlx,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='01', 1, 0)),0) as dxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='02', 1, 0)),0) as xxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '23', 1, 0)),0) as gacl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '99', 1, 0)),0) as qtcl";
                mySql = mySql + " from t_tgs_peccancy_area";
                mySql = mySql + " where " + where + "  and xssd >=90 and xssd< 100  union";

                mySql = mySql + " select '100110' sdlx,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='01', 1, 0)),0) as dxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='02', 1, 0)),0) as xxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '23', 1, 0)),0) as gacl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '99', 1, 0)),0) as qtcl";
                mySql = mySql + " from t_tgs_peccancy_area";
                mySql = mySql + " where " + where + "  and xssd >=100 and xssd< 110 union";

                mySql = mySql + " select '110120' sdlx,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='01', 1, 0)),0) as dxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='02', 1, 0)),0) as xxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '23', 1, 0)),0) as gacl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '99', 1, 0)),0) as qtcl";
                mySql = mySql + " from t_tgs_peccancy_area";
                mySql = mySql + " where  " + where + "  and xssd >=110 and xssd< 120 union";

                mySql = mySql + " select '120000' sdlx,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='01', 1, 0)),0) as dxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)='02', 1, 0)),0) as xxcl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '23', 1, 0)),0) as gacl,";
                mySql = mySql + " IFNULL(sum(IF(TRIM(hpzl)= '99', 1, 0)),0) as qtcl";
                mySql = mySql + " from t_tgs_peccancy_area";
                mySql = mySql + " where  " + where + "  and  xssd >=120";

                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        #endregion ITgsDataInfo 成员

        #region ITgsDataInfo 成员

        public int DealAlarmInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                //switch (hs["bjlx"].ToString())
                //{
                //    case "1":
                //    case "2":
                //    case "3":
                //        mySql = "update  t_tgs_alarmed  set ";
                //        mySql = mySql + "clry='" + hs["clry"].ToString() + "',";
                //        mySql = mySql + "clyj='" + hs["clyj"].ToString() + "',";
                //        mySql = mySql + "clbj='1',";
                //        mySql = mySql + "clrq=sysdate";
                //        mySql = mySql + " where xh='" + hs["xh"].ToString() + "'";
                //        return dataAccess.Execute_NonQuery(mySql);

                //    default:
                //        return 0;
                //}
                mySql = "update  t_tgs_alarmed  set ";
                mySql = mySql + "clry='" + hs["clry"].ToString() + "',";
                mySql = mySql + "clyj='" + hs["clyj"].ToString() + "',";
                mySql = mySql + "clbj='" + hs["clbj"].ToString() + "',";
                mySql = mySql + "ptag='" + hs["clbj"].ToString() + "',";
                mySql = mySql + "clrq=now()";
                mySql = mySql + " where xh='" + hs["xh"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 处理流量信息
        /// </summary>
        public int DealFlowInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_tgs_flow_alert  set ";
                mySql = mySql + "cljg='" + hs["cljg"].ToString() + "'";
                mySql = mySql + " where id='" + hs["BH"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int DealAlarm_PeccancyInfo(System.Collections.Hashtable hs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int DealAlarm_PasscarInfo(System.Collections.Hashtable hs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DataTable GetPassCarTrackInfo(string selectWhere)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select   rownum, t.* from (select * from vt_tgs_passcar_track t1 where " + selectWhere + " order by gwsj) t";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        #endregion ITgsDataInfo 成员

        #region 套牌分析

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetRepaclePlate(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from vt_tgs_analyze_replaceplate where " + where;
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
        public int UpdateRepaclePlate(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_tgs_analyze_replaceplate  set gxsj=sysdate,shzt='" + hs["shzt"].ToString() + "'  where xh='" + hs["xh"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        #endregion 套牌分析

        #region 区间违法审核

        /// <summary>
        ///
        /// </summary>
        /// <param name="xh"></param>
        /// <returns></returns>
        public int UnlockAreaOneInfo(string xh)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_tgs_peccancy_area set sdr='',sdsj=null where id='" + xh + "'";

                int result = dataAccess.Execute_NonQuery(mySql);

                return result;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 将相对应的违法记录进行加锁
        /// </summary>
        /// <param name="where">加锁条件</param>
        /// <param name="sdr">锁定人</param>
        /// <param name="sdsj">锁定时间</param>
        /// <returns></returns>
        public int LockAreaPeccancy(string where, string sdr, string sdsj, int lockAmount)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                mySql.Append(" update t_tgs_peccancy_area");
                mySql.Append(" set sdr = '" + sdr + "', sdsj = STR_TO_DATE('" + sdsj + "','%Y-%m-%d %H:%i:%s')");
                mySql.Append(" where id in   (SELECT a.id from (select id from t_tgs_peccancy_area where ");
                mySql.Append(where + "     AND sdr IS NULL AND IFNULL(sdr, 'zhao') = 'zhao'  ORDER BY wfjssj DESC  ");
                mySql.Append(" LIMIT 0, " + lockAmount.ToString() + ") a)");
                int result = dataAccess.Execute_NonQuery(mySql.ToString());
                return result;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 将所有超时的违法记录进行解锁
        /// </summary>
        /// <param name="sdsj"></param>
        /// <param name="sdr"></param>
        /// <returns></returns>
        public int UnAlllockAreaAll(string sdsj, string sdr)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update t_tgs_peccancy_area set sdr=null,sdsj=null where sdsj<=STR_TO_DATE('" + sdsj + "','%Y-%m-%d %H:%i:%s') or sdr='" + sdr + "'";
                int result = dataAccess.Execute_NonQuery(mySql);
                return result;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 将所有超时的违法记录进行解锁
        /// </summary>
        /// <param name="sdsj"></param>
        /// <param name="sdr"></param>
        /// <returns></returns>
        public int UnlockAreaAll(string sdsj, string sdr)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update t_tgs_peccancy_area set sdr='',sdsj=null where sdsj<=STR_TO_DATE('" + sdsj + "','%Y-%m-%d %H:%i:%s') and sdr='" + sdr + "'";
                int result = dataAccess.Execute_NonQuery(mySql);
                return result;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 违法数据锁定
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sdr"></param>
        /// <param name="sdsj"></param>
        /// <returns></returns>
        public int LockAreaPeccancy(string where, string sdr, string sdsj)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                mySql.Append(" update t_tgs_peccancy_area");
                mySql.Append(" set sdr = '" + sdr + "', sdsj = STR_TO_DATE('" + sdsj + "','%Y-%m-%d %H:%i:%s')");
                mySql.Append(" where id in   (SELECT a.id from (select id from t_tgs_peccancy_area where ");
                mySql.Append(where + "     AND sdr IS NULL AND IFNULL(sdr, 'zhao') = 'zhao'  ORDER BY wfjssj DESC  ");
                mySql.Append(" LIMIT 0, 50) a)");
                int result = dataAccess.Execute_NonQuery(mySql.ToString());
                return result;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新区间违法数据
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int CheckAreaPeccancyInfo(System.Collections.Hashtable hs)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                mySql.Append(" update  t_tgs_peccancy_area set ");
                mySql.Append(Common.GetHashtableStr(hs, "hpzl", "hpzl"));
                mySql.Append(Common.GetHashtableStr(hs, "hphm", "hphm"));
                mySql.Append(Common.GetHashtableStr(hs, "wfdd", "wfdd"));
                mySql.Append(Common.GetHashtableStr(hs, "wfdz", "wfdz"));
                mySql.Append(Common.GetHashtableDateOracle(hs, "wfjssj", "wfjssj"));
                mySql.Append(Common.GetHashtableStr(hs, "wfxw", "wfxw"));
                mySql.Append(Common.GetHashtableStr(hs, "shbj", "shbj"));
                mySql.Append(Common.GetHashtableStr(hs, "shyh", "shyh"));
                mySql.Append(Common.GetHashtableDateOracle(hs, "shsj", "shsj"));
                mySql.Append(Common.GetHashtableStr(hs, "zqmj", "zqmj"));
                mySql.Append(Common.GetHashtableStr(hs, "csys", "csys"));
                mySql.Append(Common.GetHashtableStr(hs, "clpp", "clpp"));
                mySql.Append(Common.GetHashtableStr(hs, "jdssyr", "jdssyr"));
                mySql.Append(Common.GetHashtableStr(hs, "zsxxdz", "zsxxdz"));
                mySql.Append(Common.GetHashtableStr(hs, "lxfs", "lxfs"));
                mySql.Append(Common.GetHashtableStr(hs, "dh", "dh"));
                mySql.Append(Common.GetHashtableStr(hs, "yzbm", "yzbm"));
                mySql.Append(Common.GetHashtableStr(hs, "cllx", "cllx"));
                mySql.Append(Common.GetHashtableStr(hs, "zt", "zt"));
                mySql.Append(Common.GetHashtableStr(hs, "fdjh", "fdjh"));
                mySql.Append(Common.GetHashtableDateOracle(hs, "jyyxqz", "jyyxqz"));
                mySql.Append(Common.GetHashtableStr(hs, "clxh", "clxh"));
                mySql.Append(Common.GetHashtableStr(hs, "clsbdh", "clsbdh"));
                mySql.Append(Common.GetHashtableStr(hs, "sfzmhm", "sfzmhm"));
                mySql.Append(Common.GetHashtableStr(hs, "dabh", "dabh"));
                mySql.Append("jcbj='" + hs["jcbj"].ToString() + "'");
                mySql.Append(" where id='" + hs["xh"] + "' ");
                int result = dataAccess.Execute_NonQuery(mySql.ToString());
                return result;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return -1;
            }
        }

        #endregion 区间违法审核

        #region ITgsDataInfo 成员

        /// <summary>
        ///
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="direction_id"></param>
        /// <returns></returns>
        public DataTable GetHttpPath(string stationId, string direction_id)
        {
            try
            {
                string mySql = "select * from vt_tgs_video_recordplay where  station_id='" + stationId + "' and direction_id='" + direction_id + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        #endregion ITgsDataInfo 成员
    }
}