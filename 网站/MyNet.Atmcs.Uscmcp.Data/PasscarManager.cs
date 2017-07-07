using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using System;
using System.Data;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class PasscarManager : IPasscarManager
    {
        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        private MyNet.Common.Data.DataAccess dataAccess;

        public PasscarManager()
        {
            //dataAccess = dac.GetDataAccess(WebConfig.DataAccessName);
            dataAccess = GetDataAccess.Init();
        }

        public PasscarManager(string dataAccessName)
        {
            dataAccess = dac.GetDataAccess(dataAccessName);
        }

        #region 通用

        public DataSet GetClpp()
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("SELECT * FROM  t_vehicle_brand ");
                return dataAccess.Get_DataSet(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return null;
            }
        }

        public DataSet GetClxh(string clpp)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("SELECT * FROM t_vehicle_model ");
                if (clpp != "")
                {
                    strSql.Append(" where BRANDID='" + clpp + "'");
                }
                return dataAccess.Get_DataSet(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return null;
            }
        }

        public DataSet GetStation()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT STATION_ID,STATION_NAME  FROM t_cfg_set_station order by STATION_NAME");
            return dataAccess.Get_DataSet(strSql.ToString());
        }

        public string GetStationPoint(string stationid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT *  FROM t_gis_device_mark where relationid='" + stationid + "'");
            DataSet ds = dataAccess.Get_DataSet(strSql.ToString());
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["xpoint"].ToString() + "," + ds.Tables[0].Rows[0]["ypoint"].ToString();
            }
            else
                return "";
        }

        public DataSet GetSyscode(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM t_sys_code ");
            if (where != "")
            {
                strSql.Append(" where " + where);
            }
            return dataAccess.Get_DataSet(strSql.ToString());
        }

        #endregion 通用

        #region 一牌多车

        public DataTable GetTpcl(string starttime, string endtime, string lisenceid)
        {
            string where = "";
            where = "gxsj BETWEEN '" + starttime + "' AND '" + endtime + "'";
            if (lisenceid != "")
                where += " and hphm='" + lisenceid + "'";
            DataSet ds = GetListTpcl(where);
            if (ds != null)
                return ds.Tables[0];
            else
                return null;
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
        /// 判断该车辆是否存在
        /// </summary>
        /// <param name="hphm">号牌号码</param>
        /// <param name="hpzl">号牌种类</param>
        /// <returns></returns>
        public DataTable GetBkbh(string hphm, string hpzl)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from  t_tgs_suspect  where  hphm  ='" + hphm + "' and hpzl= '" + hpzl + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 插入到套牌车库（新表）
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateSuspicionInfoNew(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeHphmExist("t_tgs_suspect", hs["hphm"].ToString(), hs["hpzl"].ToString()) <= 0)
                {
                    mySql = "insert into  t_tgs_suspect (bkbh,hphm,hpzl,csys,clpp,bkjb,bklx,bkr,bksxrq,bkqsrq,bkrdw,bkbj) values(";
                    mySql = mySql + "'" + hs["bkbh"].ToString() + "',";
                    mySql = mySql + "'" + hs["hphm"].ToString() + "',";
                    mySql = mySql + "'" + Common.GetHashtableValue(hs, "hpzl", "02") + "',";
                    mySql = mySql + "'" + Common.GetHashtableValue(hs, "csys", "") + "',";
                    mySql = mySql + "'" + hs["clpp"].ToString() + "',";
                    mySql = mySql + "'2',";
                    mySql = mySql + "'" + hs["bklx"].ToString() + "',";
                    mySql = mySql + "'" + hs["bkr"].ToString() + "',";
                    mySql = mySql + "STR_TO_DATE('" + System.DateTime.Now.AddYears(10).ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s'),";
                    mySql = mySql + "now() ,'" + hs["bkrdw"].ToString() + "',";
                    mySql = mySql + " '3')";
                    return dataAccess.Execute_NonQuery(mySql);
                }
                else
                {
                    mySql = "update  t_tgs_suspect  set ";
                    mySql = mySql + Common.GetHashtableStr(hs, "hpzl", "hpzl");
                    mySql = mySql + Common.GetHashtableStr(hs, "csys", "csys");
                    mySql = mySql + Common.GetHashtableStr(hs, "bklx", "bklx");
                    mySql = mySql + "hphm='" + hs["hphm"].ToString() + "',";
                    mySql = mySql + "bkr='" + hs["bkr"].ToString() + "',";
                    mySql = mySql + "clpp='" + hs["clpp"].ToString() + "',";
                    mySql = mySql + "bkrdw='" + hs["bkrdw"].ToString() + "',";
                    mySql = mySql + "bksxrq=STR_TO_DATE('" + System.DateTime.Now.AddYears(10).ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s'),";
                    mySql = mySql + "bkqsrq=now()";
                    mySql = mySql + " where hphm='" + hs["hphm"].ToString() + "' and hpzl='" + hs["hpzl"].ToString() + "'";
                    return dataAccess.Execute_NonQuery(mySql);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 判断该车辆布控编号是否存存在
        /// </summary>
        /// <returns></returns>
        private DataTable GetBkbhExist(string bkbh)
        {
            string mySql = string.Empty;
            mySql = "select * from  t_tgs_suspect_recive   where  bkbh  ='" + bkbh + "'";
            return dataAccess.Get_DataTable(mySql);
            // return int.Parse(dataAccess.Get_DataString(mySql, 0));
        }

        /// <summary>
        /// 更新(插入)表t_tgs_suspect_recive
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateSuspicionInfoNewRecive(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GetBkbhExist(hs["bkbh"].ToString()).Rows.Count <= 0 || GetBkbhExist(hs["bkbh"].ToString()) == null)
                {
                    mySql = "insert into  t_tgs_suspect_recive (jslx,jsdx,bkbh) values(";
                    mySql = mySql + "'1',";
                    mySql = mySql + "'" + hs["bkrdw"].ToString() + "',";

                    mySql = mySql + "'" + hs["bkbh"].ToString() + "')";

                    return dataAccess.Execute_NonQuery(mySql);
                }
                else
                {
                    mySql = "update  t_tgs_suspect_recive  set ";
                    mySql = mySql + "jsdx='" + hs["bkrdw"].ToString() + "'";

                    mySql = mySql + " where bkbh='" + hs["bkbh"].ToString() + "'";
                    return dataAccess.Execute_NonQuery(mySql);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int UpdateSuspicionInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeHphmExist("t_tgs_info_blacklist", hs["hphm"].ToString(), hs["hpzl"].ToString()) <= 0)
                {
                    mySql = "insert into  t_tgs_info_blacklist (xh,hphm,hpzl,csys,clpp,mdlx,yxsj,gxsj,sjly,bklx) values(";
                    mySql = mySql + "'" + hs["xh"].ToString() + "',";
                    mySql = mySql + "'" + hs["hphm"].ToString() + "',";
                    mySql = mySql + "'" + Common.GetHashtableValue(hs, "hpzl", "02") + "',";
                    mySql = mySql + "'" + Common.GetHashtableValue(hs, "csys", "") + "',";
                    mySql = mySql + "'" + hs["clpp"].ToString() + "',";
                    mySql = mySql + "'" + hs["mdlx"].ToString() + "',";
                    mySql = mySql + "STR_TO_DATE('" + System.DateTime.Now.AddYears(1).ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s'),";
                    mySql = mySql + "now() ,'" + hs["sjly"].ToString()+"','1" + "')";
                    return dataAccess.Execute_NonQuery(mySql);
                }
                else
                {
                    mySql = "update  t_tgs_info_blacklist  set ";
                    mySql = mySql + Common.GetHashtableStr(hs, "hpzl", "hpzl");
                    mySql = mySql + Common.GetHashtableStr(hs, "csys", "csys");
                    mySql = mySql + Common.GetHashtableStr(hs, "mdlx", "mdlx");
                    mySql = mySql + "hphm='" + hs["hphm"].ToString() + "',";
                    mySql = mySql + "clpp='" + hs["clpp"].ToString() + "',";
                    mySql = mySql + "sjly='" + hs["sjly"].ToString() + "',";
                    mySql = mySql + "bklx='1',";
                    mySql = mySql + "yxsj=STR_TO_DATE('" + System.DateTime.Now.AddYears(1).ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s'),";
                    mySql = mySql + "gxsj=now()";
                    mySql = mySql + " where hphm='" + hs["hphm"].ToString() + "' and hpzl='" + hs["hpzl"].ToString() + "'";
                    return dataAccess.Execute_NonQuery(mySql);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        private DataSet GetListTpcl(string strWhere)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT  a.xybh,
                                        a.hphm,
                                        a.hpzl,
                                        a.kkid1,
                                        DATE_FORMAT(gwsj1,'%Y-%m-%d %H:%i:%s') gwsj1,
                                        a.zjwj1,
                                        a.kkid2,
                                       DATE_FORMAT(gwsj2,'%Y-%m-%d %H:%i:%s') gwsj2,
                                        a.zjwj2,
                                        a.bj,
                                        DATE_FORMAT(gxsj,'%Y-%m-%d %H:%i:%s') gxsj,
                                        a.shry,
                                        a.shsj,f_get_stationname (a.kkid1) AS kkidname1,b.xpoint xpos1,b.ypoint ypos1,f_get_stationname (a.kkid2) AS kkidname2, c.xpoint xpos2,c.ypoint ypos2,F_TO_NAME ('140001', a.hpzl) hpzlname  FROM t_analyze_changeplate a ");
                strSql.Append("LEFT JOIN (SELECT a.STATION_ID,a.STATION_NAME,b.xpoint,b.ypoint FROM t_cfg_set_station a INNER JOIN t_gis_mark b ON a.STATION_ID=b.relationid) b ON a.kkid1=b.STATION_ID ");
                strSql.Append("LEFT JOIN (SELECT a.STATION_ID,a.STATION_NAME,b.xpoint,b.ypoint FROM t_cfg_set_station a INNER JOIN t_gis_mark b ON a.STATION_ID=b.relationid) c ON a.kkid2=c.STATION_ID  ");
                //strSql.Append("LEFT JOIN (SELECT * FROM t_sys_code WHERE codetype='140001') d ON a.hpzl=d.code ");

                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
                return dataAccess.Get_DataSet(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return null;
            }
        }

        public DataSet GetOneLisenceMulCarData(string starttime, string endtime, string lisenceid, int startrow, int endrow)
        {
            DataSet ds = new DataSet();
            string where = "";
            where = "gxsj BETWEEN STR_TO_DATE('" + starttime + "','%Y-%m-%d %H:%i:%s')  AND STR_TO_DATE('" + endtime + "','%Y-%m-%d %H:%i:%s') ";
            if (lisenceid != "")
                where += " and hphm='" + lisenceid + "'";

            string where1 = " limit " + startrow.ToString() + "," + endrow.ToString();
            return GetListMulCar(where, where1);
        }

        public int GetOneLisenceMulCarDataRows(string starttime, string endtime, string lisenceid)
        {
            DataSet ds = new DataSet();
            string where = "";
            where = "gxsj BETWEEN STR_TO_DATE('" + starttime + "','%Y-%m-%d %H:%i:%s')  AND STR_TO_DATE('" + endtime + "','%Y-%m-%d %H:%i:%s') ";
            if (lisenceid != "")
                where += " and hphm='" + lisenceid + "'";
            return GetListMulCarRows(where);
        }

        private int GetListMulCarRows(string strWhere)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT COUNT(1) FROM (SELECT * FROM (SELECT a.*,b.STATION_NAME kkidname1,c.STATION_NAME kkidname2,d.CODEDESC hpzlname FROM t_analyze_changeplate a ");
                strSql.Append("LEFT JOIN t_cfg_set_station b ON a.kkid1=b.STATION_ID ");
                strSql.Append("LEFT JOIN t_cfg_set_station c ON a.kkid2=c.STATION_ID  ");
                strSql.Append("LEFT JOIN (SELECT * FROM t_sys_code WHERE codetype='140001') d ON a.hpzl=d.code ");

                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
                strSql.Append(" ) aa GROUP BY hphm,hpzlname,hpzl) xxx");
                DataSet ds = dataAccess.Get_DataSet(strSql.ToString());
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                    return int.Parse(ds.Tables[0].Rows[0][0].ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 获得一牌多车数据列表
        /// </summary>
        private DataSet GetListMulCar(string strWhere, string where1)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                //strSql.Append("SELECT hphm,COUNT(hphm)tpcs,hpzlname,hpzl FROM (SELECT a.*,b.STATION_NAME kkidname1,c.STATION_NAME kkidname2,d.CODEDESC hpzlname FROM t_analyze_changeplate a ");
                //strSql.Append("LEFT JOIN t_cfg_set_station b ON a.kkid1=b.STATION_ID ");
                //strSql.Append("LEFT JOIN t_cfg_set_station c ON a.kkid2=c.STATION_ID  ");
                //strSql.Append("LEFT JOIN (SELECT * FROM t_sys_code WHERE codetype='140001') d ON a.hpzl=d.code ");

                //if (strWhere.Trim() != "")
                //{
                //    strSql.Append(" where " + strWhere);
                //}
                //strSql.Append(" ) aa GROUP BY hphm,hpzlname,hpzl order by tpcs desc " + where1);
                strSql.Append("SELECT hphm,COUNT(hphm)tpcs,hpzlname,hpzl FROM ( ");
                strSql.Append(@"SELECT
                                    a.*,
                                    f_get_stationname(kkid1) AS  kkidname1,
                                    f_get_stationname(kkid2) AS  kkidname2,
                                    F_TO_NAME('140001',hpzl)  hpzlname
                                  FROM
                                    t_analyze_changeplate a ");

                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
                strSql.Append(" ) aa GROUP BY hphm,hpzl order by tpcs desc " + where1);
                return dataAccess.Get_DataSet(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return null;
            }
        }

        #endregion 一牌多车

        #region 初次入城

        /// <summary>
        /// 获得出入城数据列表
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="lisenceid"></param>
        /// <returns></returns>
        public DataSet GetFisrtIntoData(string starttime, string endtime, string hphm, string hpzl, int pageIndex, int tag)
        {
            string where = " gwsj BETWEEN  STR_TO_DATE('" + starttime + "','%Y-%m-%d %H:%i:%s') and   STR_TO_DATE ('" + endtime + "','%Y-%m-%d %H:%i:%s')";

            if (hphm != "")
            {
                if (tag == 1)//模糊查询
                {
                    where += " and hphm like '" + hphm + "'";
                }
                else
                {
                    where += " and hphm='" + hphm + "'";
                }
            }

            if (!string.IsNullOrEmpty(hpzl))
            {
                where += " and hpzl='" + hpzl + "'";
            }

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" SELECT
                              c.xh,
                              c.kkid,
                              f_get_stationname (c.kkid) AS kkmc,
                              c.hphm,
                              c.hpzl,
                              F_TO_NAME ('140001', c.hpzl) AS hpzlms,
                              c.gwsj,
                              c.fxbh,
                              f_get_direction_desc (c.fxbh) AS fxmc,
                              c.cdbh,
                              c.clsd,
                              c.jllx,
                              f_get_cjjg_kkid (c.kkid) AS cjjg,
                              f_get_departname (f_get_cjjg_kkid (c.kkid)) AS cjjgms,
                              f_get_sjly (c.kkid) AS sjly,
                              F_TO_NAME ('240022', f_get_sjly (c.kkid)) AS sjlyms,
                              c.zjwj1,
                              c.zjwj2,
                              c.zjwj3,
                              c.zjwj4
                            FROM
                              (SELECT
                                *
                              FROM
                                (SELECT
                                  xh
                                FROM
                                  T_ANALYZE_FIRST A  ");
            if (where.Trim() != "")
            {
                strSql.Append(" where " + where);
            }
            if (pageIndex == 1)
            {
                strSql.Append(" ORDER BY gwsj DESC ) CCRC LIMIT " + (pageIndex - 1) * 15 + ",15 )  b,  T_ANALYZE_FIRST c WHERE b.xh = c.xh   ");
            }
            else
            {
                strSql.Append(" ORDER BY gwsj DESC ) CCRC LIMIT " + ((pageIndex - 1) * 15).ToString() + ",15)  b,  T_ANALYZE_FIRST c WHERE b.xh = c.xh   ");
            }

            return dataAccess.Get_DataSet(strSql.ToString());
        }

        /// <summary>
        /// 得到总条数
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="hphm"></param>
        /// <param name="hpzl"></param>
        /// <returns></returns>
        public DataSet GettFisrtIntoCount(string starttime, string endtime, string hphm, string hpzl, int tag)
        {
            string where = " gwsj BETWEEN  STR_TO_DATE('" + starttime + "','%Y-%m-%d %H:%i:%s') and   STR_TO_DATE ('" + endtime + "','%Y-%m-%d %H:%i:%s')";

            if (!string.IsNullOrEmpty(hphm))
            {
                if (tag == 1)//模糊查询
                {
                    where += " and hphm like '" + hphm + "'";
                }
                else
                {
                    where += " and hphm='" + hphm + "'";
                }
            }

            if (!string.IsNullOrEmpty(hpzl))
            {
                where += " and hpzl='" + hpzl + "'";
            }

            String strSql = @"SELECT  COUNT(*) FROM T_ANALYZE_FIRST A ";
            if (where.Trim() != "")
            {
                strSql += " where " + where;
            }
            strSql += " ORDER BY gwsj DESC   ";

            return dataAccess.Get_DataSet(strSql.ToString());
        }

        #endregion 初次入城

        #region 一车多牌

        public DataSet GetOneCarMulLisenceData(string starttime, string endtime, string lisenceid, string similarity, string stationid)
        {
            DataSet ds = new DataSet();
            string where = "", joinstr = "";
            switch (similarity)
            {
                case "50%":

                    break;

                case "60%":
                    joinstr = " and (a.rlxx=b.rlxx or a.njbz=b.njbz) ";
                    break;

                case "70%":
                    joinstr = " and a.rlxx=b.rlxx and a.njbz=b.njbz ";
                    break;
            }
            where = " gwsj BETWEEN '" + starttime + "' AND '" + endtime + "'";
            if (lisenceid != "")
                where += " and yy.hphm='" + lisenceid + "'";
            if (stationid != "")
                where += " and kkid='" + stationid + "'";
            return GetListOneCarMuLis(where, joinstr);
        }

        private DataSet GetListOneCarMuLis(string strWhere, string joinstr)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT yy.hphm hphm1 ,xx.* FROM ( SELECT A.*,B.STATION_NAME,C.CODEDESC CLLXNAME,d.DIRECTION_NAME  FROM t_tgs_passcar A ");
            strSql.Append(" INNER JOIN T_CFG_SET_STATION B ON A.KKID=B.STATION_ID ");
            strSql.Append(" INNER JOIN (SELECT * FROM T_SYS_CODE WHERE CODETYPE='140001') C ON A.CLLX=C.CODE");
            strSql.Append(" INNER JOIN t_cfg_direction d ON a.kkid=d.station_id AND a.fxbh=d.direction_id) xx");
            strSql.Append(" RIGHT JOIN (SELECT  b.glid,a.hphm FROM t_tgs_second_passcar a ");
            strSql.Append(" INNER JOIN t_tgs_second_passcar b ON a.csys=b.csys AND a.clpp=b.clpp AND a.clppxh=b.clppxh AND a.cllx=b.cllx ");
            //strSql.Append("WHERE a.hphm='京123488' ");
            if (joinstr != "")
                strSql.Append(joinstr);
            strSql.Append("ORDER BY a.hphm ) yy ON xx.glid =yy.glid");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by yy.hphm");

            return dataAccess.Get_DataSet(strSql.ToString());
        }

        #endregion 一车多牌

        public DataSet GetPassCarInfo(string starttime, string endtime, string lisenceid)
        {
            DataSet ds = new DataSet();
            string where = "";
            where = "gwsj BETWEEN STR_TO_DATE('" + starttime + "','%Y-%m-%d %H:%i:%s')  AND  STR_TO_DATE('" + endtime + "','%Y-%m-%d %H:%i:%s') ";
            if (lisenceid != "")
                where += " and hphm='" + lisenceid + "'";
            return GetListPassCar(where);
        }

        private DataSet GetListPassCar(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT A.*,B.STATION_NAME,B.xpoint,B.ypoint,C.CODEDESC CLLXNAME,d.DIRECTION_NAME  FROM T_TGS_PASSCAR A INNER JOIN (SELECT ba.*,bb.xpoint,bb.ypoint FROM T_CFG_SET_STATION ba LEFT JOIN t_gis_device_mark bb ON ba.STATION_ID=bb.relationid) B ON A.KKID=B.STATION_ID INNER JOIN (SELECT * FROM T_SYS_CODE WHERE CODETYPE='140001') C ON A.CLLX=C.CODE");
            strSql.Append(" INNER JOIN t_cfg_direction d ON a.kkid=d.station_id AND a.fxbh=d.direction_id");

            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by gwsj");
            return dataAccess.Get_DataSet(strSql.ToString());
        }
    }
}