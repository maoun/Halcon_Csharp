using System;
using System.Data;
using System.Text;
using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class GisShow : IGisShow
    {
        /// <summary>
        /// 数据访问接口
        /// </summary>
        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        private MyNet.Common.Data.DataAccess dataAccess;
        private SettingManager settingManager = new SettingManager();
        private Common common = new Common();

        public GisShow()
        {
            //dataAccess = dac.GetDataAccess(WebConfig.DataAccessName);
            dataAccess = GetDataAccess.Init();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetMapLable(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  point_id,point_x,point_y,point_type,f_to_names ('240040',point_type) as point_typedesc,point_name,point_tag,point_image,point_button  from t_tgs_map_lable where  " + where;

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
        public DataTable GetDeviceStateMap()
        {
            string mySql = string.Empty;
            try
            {
                return dataAccess.Get_DataTable("select * from vt_tgs_device_state_map");
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
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetPgis(string type)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "  select * from   vt_gis_devicelist where station_type_id='" + type + "' ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        #region IGisShow 成员

        /// <summary>
        /// 更新标注点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdatePointInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_gis_device_mark", "markid", hs["markid"].ToString()) > 0)
                {
                    mySql = "update  t_gis_device_mark  set ";
                    mySql = mySql + Common.GetHashtableStr(hs, "markname", "markname");
                    mySql = mySql + Common.GetHashtableStr(hs, "relationid", "relationid");
                    mySql = mySql + Common.GetHashtableStr(hs, "marktype", "marktype");
                    mySql = mySql + "xpoint='" + hs["xpoint"].ToString() + "',";
                    mySql = mySql + "ypoint='" + hs["ypoint"].ToString() + "'";
                    mySql = mySql + " where markid='" + hs["markid"].ToString() + "'";
                }
                else
                {
                    mySql = "insert into  t_gis_device_mark (markid,markname,marktype,relationid,xpoint,ypoint) values(";
                    mySql = mySql + "'" + hs["markid"].ToString() + "',";
                    mySql = mySql + "'" + hs["markname"].ToString() + "',";
                    mySql = mySql + "'" + hs["marktype"].ToString() + "',";
                    mySql = mySql + "'" + hs["relationid"].ToString() + "',";
                    mySql = mySql + "'" + hs["xpoint"].ToString() + "',";
                    mySql = mySql + "'" + hs["ypoint"].ToString() + "')";
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
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeletePointInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_gis_device_mark  where relationid='" + hs["relationid"].ToString() + "' and marktype ='" + hs["marktype"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新标注点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteKmlInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from  t_gis_road_kml  where ";
                mySql = mySql + "station_id_s='" + hs["station_id_s"].ToString() + "'";
                mySql = mySql + " and station_id_e='" + hs["station_id_e"].ToString() + "'";
                mySql = mySql + " and direction_id='" + hs["direction_id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新标注点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertKmlInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_gis_road_kml (id,station_id_s,station_id_e,direction_id,xpos,ypos,zpos,road_name,order_id) values(";
                mySql = mySql + "seq_num.nextval,";
                mySql = mySql + "'" + hs["station_id_s"].ToString() + "',";
                mySql = mySql + "'" + hs["station_id_e"].ToString() + "',";
                mySql = mySql + "'" + hs["direction_id"].ToString() + "',";
                mySql = mySql + "'" + hs["xpos"].ToString() + "',";
                mySql = mySql + "'" + hs["ypos"].ToString() + "',";
                mySql = mySql + "'" + hs["zpos"].ToString() + "',";
                mySql = mySql + "'" + hs["road_name"].ToString() + "',";
                mySql = mySql + "" + hs["order_id"].ToString() + ")";

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
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateGisMark(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_gis_device_mark", "relationid", hs["relationid"].ToString()) > 0)
                {
                    mySql = "update  t_gis_device_mark  set ";

                    mySql = mySql + "markname='" + hs["markname"].ToString() + "',";
                    mySql = mySql + "marktype='" + hs["marktype"].ToString() + "',";
                    mySql = mySql + "xpoint='" + hs["xpoint"].ToString() + "',";
                    mySql = mySql + "ypoint='" + hs["ypoint"].ToString() + "',";
                    mySql = mySql + "markdesc='" + hs["markdesc"].ToString() + "'";
                    mySql = mySql + " where relationid='" + hs["relationid"].ToString() + "'";
                }
                else
                {
                    mySql = "insert into  t_gis_device_mark (markid,markname,marktype,xpoint,ypoint,relationid,markdesc,isshow) values(";
                    mySql = mySql + "'" + hs["markid"].ToString() + "',";
                    mySql = mySql + "'" + hs["markname"].ToString() + "',";
                    mySql = mySql + "'" + hs["marktype"].ToString() + "',";
                    mySql = mySql + "'" + hs["xpoint"].ToString() + "',";
                    mySql = mySql + "'" + hs["ypoint"].ToString() + "',";
                    mySql = mySql + "'" + hs["relationid"].ToString() + "',";
                    mySql = mySql + "'" + hs["markdesc"].ToString() + "',";
                    mySql = mySql + "'1')";
                }
                dataAccess.Execute_NonQuery(mySql);

                mySql = "update t_gis_device_mark  set markarray=rownum where marktype = '" + hs["marktype"].ToString() + "'";
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
        ///
        /// </summary>
        /// <param name="xh"></param>
        /// <returns></returns>
        public DataTable GetStationInfo(string xh)
        {
            string mySql = string.Empty;
            try
            {
                ////mySql = "  select station_id, f_get_value('station_name', 't_tgs_set_station',  'station_id',  station_id) as station_name, device_id, device_name, device_ip, state_id,";
                ////mySql += " f_to_name('240006', f_get_value('state_id', 't_tgs_device_state', 'device_id', device_id)) as state,";
                ////mySql += " decode(state_id, 1, 'ok', 2, 'alarm', 0, 'err', 'unkown') as imagename,update_time from (select a.station_id, a.device_id,a.device_name, a.device_ip,";
                ////mySql += " decode(sign(sysdate - (2 / 24) - b.update_time),1, 0,  decode(sign(sysdate - (30 / (24 * 60)) - b.update_time),   1,  2, -1,";
                ////mySql += " f_get_value('state_id', 't_tgs_device_state',  'id',  b.id))) as state_id,b.update_time";
                ////mySql += " from t_tgs_set_device a, t_tgs_device_state b where a.id = b.id(+) and a.station_id='" + xh + "')";

                //mySql = "  select station_id, f_get_value('station_name', 't_tgs_set_station',  'station_id',  station_id) as station_name, device_id, device_name, device_ip, state_id,";
                //mySql += " f_to_name('240006', f_get_value('state_id', 't_tgs_device_state', 'device_id', device_id)) as state,";
                //mySql += " decode(state_id, 1, 'ok', 2, 'alarm', 0, 'err', 'ok') as imagename,update_time from (select a.station_id, a.device_id,a.device_name, a.device_ip,";
                ////mySql += " decode(sign(sysdate - (2 / 24) - b.update_time),1, 0,  decode(sign(sysdate - (30 / (24 * 60)) - b.update_time),   1,  2, -1,";
                ////mySql += " f_get_value('state_id', 't_tgs_device_state',  'id',  b.id))) as state_id,b.update_time";
                //mySql += " c.connect_state as state_id,b.update_time";

                //mySql += " from t_tgs_set_device a, t_tgs_device_state b ,t_dev_device_state c where a.id = b.id(+) and a.device_id=c.device_id(+) and a.station_id='" + xh + "')";

                //mySql = "SELECT a.station_id,a.station_name,b.device_id,b.device_name,b.ipaddress AS device_ip,state_id,state_id AS state,imagename,update_time FROM t_cfg_set_station a  ";
                //mySql += " INNER JOIN (SELECT bb.*,aa.station_id,cc.work_state AS state_id , imagename,update_time FROM t_cfg_station_device aa INNER JOIN t_dev_device_infor bb ON aa.device_id=bb.device_id";
                //switch (dataAccess.DataBaseType.ToUpper())
                //{
                //    case "MYSQL":
                //        mySql += " INNER JOIN (SELECT x.*,CONCAT(y.codedesc,'-',z.codedesc) AS imagename FROM t_dev_device_state X ";
                //        break;
                //    case "ORACLE":
                //        mySql += " INNER JOIN (SELECT x.*,(y.codedesc||'-'||z.codedesc) AS imagename FROM t_dev_device_state X ";
                //        break;
                //}                
                //mySql += " LEFT JOIN (SELECT * FROM  t_sys_code WHERE codetype='240006') Y ";
                //mySql += " ON x.connect_state =y.code LEFT JOIN (SELECT * FROM  t_sys_code WHERE codetype='410100') z ";
                //mySql += " ON x.work_state=z.code) cc ON aa.device_id= cc.device_id) b ";
                //mySql += "ON a.station_id=b.station_id WHERE a.station_id='"+xh+"'";

                mySql = @"SELECT   a.station_id,  a.station_name,  bb.device_id,  bb.device_name,
  cc.device_ip,  cc.work_state,  cc.work_state AS state,  cc.imagename,  cc.update_time 
FROM  t_cfg_set_station a,  t_cfg_station_device aa,  t_dev_device_infor bb ,
  (SELECT c.*,CONCAT(f_to_name ('240006', c.connect_state),'-',f_to_name ('410100', c.work_state)) AS imagename 
  FROM    t_dev_device_state c) cc WHERE aa.device_id = bb.device_id   AND bb.device_id = cc.device_id 
  AND a.station_id = aa.station_id AND   a.station_id = '" + xh + "'";

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
        public DataTable GetStationPoint(string where)
        {
            string mySql = string.Empty;
            try
            {
                return dataAccess.Get_DataTable("select * from t_tgs_set_station where " + where);
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
        public DataTable GetCCTV(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select a.id,device_name,decode(nvl(c.relationid,'*'),'*','0','1') as mark,xpoint,ypoint,nvl(c.isshow,0) as isshow,b.remoteip,b.channelid,b.remoteport,b.remoteuser,b.remotepwd,b.recordtype from  t_dev_device a,t_cctv_cam_setting b, t_gis_device_mark c where a.id=b.id  and b.isgismark='1' and a.id=c.relationid(+) and " + where;

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
        public DataTable GetTesEvent(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from vt_tes_event_temp  where  " + where;

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
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetFlow(string tableName, string kkid,string strdate,string xs)
        {
            string mySql = string.Empty;
            string where = "";
            try
            {
                switch (dataAccess.DataBaseType.ToUpper())
                {
                    case "MYSQL":
                        where = "kkid='" + kkid + "' and rq = '" + strdate + "'  and xs= " + xs + "";
                        break;
                    case "ORACLE":
                        where = "kkid='" + kkid + "' and rq = to_date('" + strdate + "','yyyy-mm-dd')  and xs= " + xs + "";
                        break;
                }
                mySql = "SELECT c.codedesc fxms,rq,xs,SUM(ll) AS ll,SUM(xcll) AS xcll,SUM(dcll) AS dcll   FROM "+tableName+" a ";
                mySql+=" INNER JOIN t_cfg_set_station b ON a.kkid=b.station_id";
                mySql+=" INNER JOIN (SELECT * FROM t_sys_code WHERE codetype='240025') c ON a.fxbh=c.code";
                mySql += " WHERE  " + where + " GROUP BY kkid, c.codedesc,rq,xs";
                //if (tableName.ToLower() == "t_tgs_flow_5min")
                //{
                //    mySql = "select F_GET_VALUE('station_name', 't_tgs_set_station',  'station_id',  kkid)|| f_to_name ('240025', fxbh) as fxms, to_char(rq,'yyyy-MM-dd') as rq,xs,sum(ll) as ll, sum(xcll) as xcll, sum(dcll) as dcll from " + tableName + " where " + where + " group by kkid, fxbh,rq,xs";
                //}
                //else if (tableName.ToLower() == "t_tfm_flow_5min")
                //{
                //    mySql = "select F_GET_VALUE('station_name', 't_tgs_set_station',  'station_id',  kkid)||f_to_name('240025', fxbh) as fxms, to_char(rq, 'yyyy-MM-dd') as rq, xs, round(sum(xcll) / count(*),0) as xcll, round( sum(dcll) / count(*),0) as dcll, round( sum(zcll) / count(*),0) as zcll,round( sum(qtll) / count(*),0) as qtll, round(sum(ll) / count(*),0) as ll,round(sum(pjsd) / count(*),2) as pjsd from " + tableName + " where " + where + " group by kkid,fxbh,rq,xs";
                //}
                //else if (tableName.ToLower() == "t_wea_info_5min")
                //{
                //    mySql = "select to_char(rq, 'yyyy-MM-dd') as rq, xs,jg,  round(sum(seeing) / count(*),2) as seeing, round(sum(rain) / count(*),2) as rain, round(sum(snow) / count(*),2) as snow, round(sum(wind) / count(*),2) as wind, f_to_name('240024', winddirection) as fxms from " + tableName + " where " + where + " group by winddirection,rq,xs,jg";
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
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTgsStation(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from (select station_id,station_name,decode(nvl(b.relationid,'*'),'*','0','1') as mark,xpoint,ypoint,nvl(b.isshow,0) as isshow,markarray from t_tgs_set_station a,t_gis_device_mark b where a.station_id=b.relationid(+) and " + where + ")";
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
        public DataTable GetComStation(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select markid,markname,decode(nvl(relationid,'*'),'*','0','1') as mark,xpoint,ypoint,nvl(isshow,0) as isshow from t_gis_device_mark where " + where + "";
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
        public DataTable GetTgsStationFlowInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select   station_id, station_name,direction_id,direction_name,state_id,decode(state_id,1,'ok',2,'alarm',0,'err','unknow') as imageName, state_desc,    nvl (ll, 0) as ll  from   (select   station_id,   station_name,  direction_id,";
                mySql = mySql + "  direction_name,state_id,state_desc  from   vt_tgs_device_state_map) b, (  select   sum (ll) as ll, fxbh  from   vt_tgs_flow_day       where   rq = trunc (sysdate)";
                mySql = mySql + "group by   fxbh) a    where   a.fxbh(+) = b.direction_id  and " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }
        public string GetSysCodedesc(string codetype, string code)
        {
            try
            {
                string mySql = "SELECT * FROM t_sys_code WHERE codetype='" + codetype + "' and code='" + code + "'";
                DataTable dt = dataAccess.Get_DataTable(mySql);
                if (dt == null || dt.Rows.Count < 1)
                {
                    return "";
                }
                else
                {
                    return dt.Rows[0]["codedesc"].ToString();
                }

            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        ///
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
                mySql.Append("SELECT b.codedesc AS fxmcms, xs AS HOUR,SUM(a.ll) AS LL FROM t_tgs_flow_hour a INNER JOIN (SELECT * FROM t_sys_code WHERE codetype='240025') b ");
                switch (dataAccess.DataBaseType.ToUpper())
                {
                    case "MYSQL":
                        mySql.Append("ON a.fxbh=b.code WHERE  rq=STR_TO_DATE( NOW(),'%Y-%c-%d') AND ");
                        break;
                    case "ORACLE":
                        mySql.Append("ON a.fxbh=b.code WHERE  rq=TO_DATE( SYSDATE,'%Y-%c-%d') AND ");
                        break;
                }                
                mySql.Append("kkid= '" + str[0] + "'  AND a.fxbh='" + str[1] + "' GROUP BY XS,codedesc");
                DataTable dt = dataAccess.Get_DataTable(mySql.ToString());
                if (dt == null || dt.Rows.Count < 1)
                {
                    return null;
                }
                mySql.Clear();
                mySql.Append(" SELECT cc.*,c.* FROM (SELECT b.codedesc AS fxmcms, xs AS HOUR,SUM(a.ll) AS LL FROM t_tgs_flow_hour a INNER JOIN (SELECT * FROM t_sys_code WHERE codetype='240025') b ");
                switch (dataAccess.DataBaseType.ToUpper())
                {
                    case "MYSQL":
                        mySql.Append("ON a.fxbh=b.code WHERE  rq=STR_TO_DATE( NOW(),'%Y-%c-%d') AND ");
                        break;
                    case "ORACLE":
                        mySql.Append("ON a.fxbh=b.code WHERE  rq=TO_DATE( SYSDATE,'%Y-%c-%d') AND ");
                        break;
                }
                mySql.Append("kkid= '" + str[0] + "'  AND a.fxbh='" + str[1] + "' GROUP BY XS,codedesc) cc");
                mySql.Append(" RIGHT JOIN t_sys_hour c ON cc.hour=c.hour_num ORDER BY HOUR_NUM");
                return dataAccess.Get_DataTable(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = "select station_type_id,station_type_name,ismark from  t_cfg_set_station_type  where isgisshow='1' and " + where;
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
        public int UpdateStationType(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (hs.ContainsKey("station_type_id"))
                {
                    mySql = "update  t_cfg_set_station_type  set ismark='1'  ";

                    mySql = mySql + " where station_type_id='" + hs["station_type_id"].ToString() + "'";
                }
                else
                {
                    mySql = "update  t_cfg_set_station_type  set ismark='0'";
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
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetGisRoad(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select station_id_s,station_id_e,direction_id from t_gis_road_kml group by station_id_s,station_id_e,direction_id";
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
        public DataTable GetGisRoadKml(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select id,direction_id,xpos,ypos,zpos,station_id_s,station_id_e,road_name from t_gis_road_kml  where " + where;
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
        public DataTable GetTgsAreaFlowState(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select qjid,fxbh,round(sum(pjsd)/count(*),2) as pjsd from  t_tgs_flow_area  where  " + where + " group by qjid,fxbh";
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
        public DataTable GetTfmFlowState(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select kkid,fxbh,round(sum(pjsd)/count(*),2) as pjsd from  t_tfm_flow_5min  where  " + where + " group by kkid,fxbh";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 根据类型查询
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetMarkArray(string type)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select * from VT_GIS_DEVICELIST where MARKTYPE='" + type + "' ";
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
        /// <param name="id"></param>
        /// <param name="markarray"></param>
        /// <returns></returns>
        public int UpdataMarkArray(string id, string markarray)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " update  t_gis_device_mark   set   ";
                mySql = mySql + "MARKARRAY='" + markarray + "'";
                mySql = mySql + " where MARKID='" + id + "'  ";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        #endregion IGisShow 成员

        #region IGisShow 成员

        /// <summary>
        ///获得监测点 标注状态信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetGisDeviceList(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select station_id, station_name,  mark,  xpoint,  ypoint, isshow,station_type_id,station_type_name, markarray, marktype, markid,remoteip, channelid,remoteport, remoteuser, remotepwd, recordtype, direction_id from vt_gis_devicelist   where " + where + " order by station_name asc ";
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
        public DataTable GetGPSDeviceList(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select 'P,'||usercode||','||handsetscode||','|| name||','||f_get_value ('LAT||'',''||LNG||'',''||SPEED||'',''||DIRECTION||'',''||DEVICE_STATE','T_GPS_VEHICLE', 'VEH_ID',  handsetscode) from t_ser_person  where handsetscode is not null union";

                mySql = mySql + " select 'C,'||ID||','||CZTH||','|| HPHM||','||f_get_value ('LAT||'',''||LNG||'',''||SPEED||'',''||DIRECTION||'',''||DEVICE_STATE','T_GPS_VEHICLE', 'VEH_ID',  CZTH) from T_SEV_DUTY  where CZTH is not null";

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
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetFlowState(string tableName)
        {
            string mySql = string.Empty;
            try
            {
                if (tableName.ToLower() == "t_tgs_flow_area")
                {
                    mySql = "select qjid,fxbh,round(sum(newll),0) as newll,round(sum(oldll),0) as oldll,";
                    mySql = mySql + "round(sum(newpjsd)/count(*),2) as newpjsd,round(sum(oldpjsd)/count(*),2) as oldpjsd from(";
                    mySql = mySql + "select a.qjid,a.fxbh,a.ll as newll,b.ll as oldll,a.pjsd as newpjsd,b.pjsd as oldpjsd ";
                    mySql = mySql + "from vt_tgs_flow_area_new a, vt_tgs_flow_area_old b ";
                    mySql = mySql + "where a.qjid=b.qjid  and a.fxbh=b.fxbh and a.time=b.time ";
                    mySql = mySql + "and to_date(a.time,'yyyy-MM-dd HH24:mi:ss') >sysdate-20/1440 ";
                    mySql = mySql + "and to_date(a.time,'yyyy-MM-dd HH24:mi:ss') <=sysdate-5/1440) group by qjid,fxbh";
                }

                if (tableName.ToLower() == "t_tfm_flow_5min")
                {
                    mySql = "select kkid,fxbh,round(sum(newll),0) as newll,round(sum(oldll),0) as oldll,";
                    mySql = mySql + "round(sum(newpjsd)/count(*),2) as newpjsd,round(sum(oldpjsd)/count(*),2) as oldpjsd from(";
                    mySql = mySql + "select a.kkid,a.fxbh,a.ll as newll,b.ll as oldll,a.pjsd as newpjsd,b.pjsd as oldpjsd ";
                    mySql = mySql + "from vt_tfm_flow_new a, vt_tfm_flow_old b ";
                    mySql = mySql + "where a.kkid=b.kkid  and a.fxbh=b.fxbh and a.time=b.time  and a.ll>30 ";
                    mySql = mySql + "and to_date(a.time,'yyyy-MM-dd HH24:mi:ss') >sysdate-20/1440 ";
                    mySql = mySql + "and to_date(a.time,'yyyy-MM-dd HH24:mi:ss') <=sysdate-5/1440) group by kkid,fxbh";
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
        /// <param name="led_id"></param>
        /// <returns></returns>
        public string GetLastProjectId(string led_id)
        {
            try
            {
                string mySql = " select last_project_id from t_vms_sendstate   where  isprogram='1' and led_id='" + led_id + "'";
                string project_id = dataAccess.Get_DataString(mySql, 0);
                return project_id;
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
        /// <param name="project_id"></param>
        /// <returns></returns>
        public DataTable GetProgramListByProject(string project_id)
        {
            string mySql = string.Empty;
            try
            {
                string sWhwere = " where  project_id='" + project_id + "' order by  play_order asc";
                string sql = "select * from t_vms_playgroup_list   " + sWhwere;
                DataTable dt = dataAccess.Get_DataTable(sql);

                return dt;
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
        /// <param name="programId"></param>
        /// <returns></returns>
        public DataTable GetProgramInfo(string programId)
        {
            try
            {
                string sWhwere = " where   program_id='" + programId + "' ";
                string sql = "select program_id, t.programxml.getclobval() as programxml from t_vms_program_screen t  " + sWhwere;

                DataTable dt = dataAccess.Get_DataTable(sql);

                return dt;
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
                string sWhwere = " where    " + where;
                string sql = "select  * from t_ctj_construction t  " + sWhwere;
                DataTable dt = dataAccess.Get_DataTable(sql);
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取交通管制信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTraffInfo(string where)
        {
            try
            {
                string sWhwere = " where    " + where;
                string sql = "select  * from t_tfc_trafficcontrol t  " + sWhwere;
                DataTable dt = dataAccess.Get_DataTable(sql);

                return dt;
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
            string mySql = string.Empty;
            try
            {
                mySql = " update  t_tfc_trafficcontrol   set   ";
                mySql = mySql + "xcoordinate='" + hs["xcoordinate"].ToString() + "'";
                mySql = mySql + ",ycoordinate='" + hs["ycoordinate"].ToString() + "'";
                mySql = mySql + " where id='" + hs["id"].ToString() + "'  ";
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
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdataConstructionInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " update  t_ctj_construction   set   ";
                mySql = mySql + "xcoordinate='" + hs["xcoordinate"].ToString() + "'";
                mySql = mySql + ",ycoordinate='" + hs["ycoordinate"].ToString() + "'";
                mySql = mySql + " where id='" + hs["id"].ToString() + "'  ";
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
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetRoadSegInfo(string where)
        {
            try
            {
                string sql = "select  distinct a.id, f_get_value ('dlmc','t_gis_road','roadid',  dlbh)  as dlmc,ldmc,decode(ldfx,'0','下行','1','上行') as fxmc,b.gxsj,lczt,sjly, ldfx,dlbh,f_get_value ('dllx','t_gis_road','roadid',  dlbh)  as dllx from t_gis_road_seg a,t_tfm_vd_info b where a.id=b.glld  and " + where;
                DataTable dt = dataAccess.Get_DataTable(sql);

                return dt;
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
                string sql = "select distinct *  from  t_tfm_vd_info   where " + where;
                DataTable dt = dataAccess.Get_DataTable(sql);

                return dt;
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
                string sql = "select  distinct a.id,dlbh,ldmc,xpos,ypos,zpos,orderid  from t_gis_road_seg a,t_gis_road_points b where a.id=b.roadid   and  " + where + "   order by id,orderid asc";
                DataTable dt = dataAccess.Get_DataTable(sql);

                return dt;
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
            string mySql = string.Empty;
            try
            {
                mySql = " update  t_tfm_vd_info   set   ";
                mySql = mySql + "lczt='" + hs["lczt"].ToString() + "'";
                mySql = mySql + ",gysc='" + hs["gysc"].ToString() + "'";
                mySql = mySql + ",gyr='" + hs["gyr"].ToString() + "'";
                mySql = mySql + " where glld='" + hs["glld"].ToString() + "'  ";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        #endregion IGisShow 成员
    }
}