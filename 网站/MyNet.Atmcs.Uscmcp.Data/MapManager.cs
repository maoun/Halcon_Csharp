using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class MapManager : IMapManager
    {
        #region MapManager 成员

        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        private MyNet.Common.Data.DataAccess dataAccess;

        public MapManager()
        {
            //dataAccess = dac.GetDataAccess(WebConfig.DataAccessName);
            dataAccess = GetDataAccess.Init();
        }

        public MapManager(string dataAccessName)
        {
            dataAccess = dac.GetDataAccess(dataAccessName);
        }

        /// <summary>
        /// 获取卡口信息
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
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

        private string GetRecordID(int len)
        {
            string mySql = string.Empty;
            try
            {
                int max = (len - 17) * 10;
                Random rd = new Random();
                string s = System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + rd.Next(0, max).ToString();

                return s;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return "0".PadLeft(len, '0');
            }
        }

        #endregion MapManager 成员

        #region IMapManager 成员

        #region 通用接口

        public DataSet GetClpp()
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("SELECT T.CSBL as BRANDID,T.NAME as BRANDNAME FROM T_ITGS_PECCANCY_SETTING T WHERE T.XH='261012' AND T.CSBL <=160 ");
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
                strSql.Append("SELECT T.CSBL as MODELID,T.NAME as MODELNAME,T.BZ FROM T_ITGS_PECCANCY_SETTING T WHERE T.XH='261012' AND T.CSBL >160 ");
                if (clpp != "")
                {
                    strSql.Append(" and  T.NAME LIKE '" + clpp + "%'");
                }
                return dataAccess.Get_DataSet(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return null;
            }
        }

        public DataSet GetDepart(string strwhere)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT DEPARTID,DEPARTNAME FROM t_cfg_department ");
                if (strwhere != "")
                    strSql.Append(" where " + strwhere);
                return dataAccess.Get_DataSet(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取卡口基本信息
        /// </summary>
        /// <param name="strwhere">查询条件</param>
        /// <returns></returns>
        public DataSet GetStation(string strwhere)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT a.STATION_ID,a.STATION_NAME,b.xpoint,b.ypoint,C.ICOREMARK,d.DEPARTNAME,A.DEPARTID  FROM t_cfg_set_station a LEFT JOIN t_gis_device_mark b ON a.STATION_ID=b.relationid INNER JOIN t_cfg_set_station_type C ON a.STATION_TYPE_ID=C.station_type_id LEFT JOIN t_cfg_department d ON a.DEPARTID=d.DEPARTID ");
                if (strwhere != "")
                    strSql.Append(" where " + strwhere);
                return dataAccess.Get_DataSet(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取卡口坐标
        /// </summary>
        /// <param name="stationid">卡口id</param>
        /// <returns></returns>
        public string GetStationPoint(string stationid)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("SELECT *  FROM t_gis_device_mark where relationid='" + stationid + "'");
                DataSet ds = dataAccess.Get_DataSet(strSql.ToString());
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["xpoint"].ToString() + "," + ds.Tables[0].Rows[0]["ypoint"].ToString();
                }
                else
                    return "";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return "";
            }
        }

        public DataSet GetSyscode(string where)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("SELECT * FROM t_sys_code ");
                if (where != "")
                {
                    strSql.Append(" where " + where);
                }
                return dataAccess.Get_DataSet(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return null;
            }
        }

        #endregion 通用接口

        public DataSet GetDeviceByType(string type)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("SELECT a.device_name,b.xpoint,b.ypoint FROM t_dev_device_infor a ");
                strSql.Append("INNER JOIN t_gis_device_mark b ON a.device_id=b.relationid WHERE device_type_id='" + type + "'");
                return dataAccess.Get_DataSet(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return null;
            }
        }

        public DataSet GetPathCar(string startdate, string enddate, string carid, string cartype)
        {
            DataSet ds = new DataSet();
            string where = "";
            where = "gwsj BETWEEN '" + startdate + "' AND '" + enddate + "'";
            if (carid != "")
                where += " and hphm='" + carid + "'";
            return GetListPassCar(where);
        }

        public DataSet GetIllegalAnalyze(string departid, string zqlx, string nd,string topnum)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                string strwhere = "where ", wfsj = "";
                switch (dataAccess.DataBaseType.ToUpper())
                {
                    case "MYSQL":
                        switch (zqlx)
                        {
                            case "0"://日
                                strwhere = strwhere + "rq=STR_TO_DATE('" + nd + "', '%Y%m%d')";
                                wfsj = " wfsj=STR_TO_DATE('" + nd + "', '%Y%m%d')";
                                break;

                            case "1"://月
                                strwhere = strwhere + " DATE_FORMAT(rq,'%Y%m')='" + nd + "'";

                                wfsj = " DATE_FORMAT(wfsj,'%Y%m')='" + nd + "'";
                                break;

                            case "2"://周
                                strwhere = strwhere + " WEEK(rq)='" + nd + "'";
                                wfsj = " WEEK(wfsj)='" + nd + "'";
                                break;

                            case "3"://年
                                strwhere = strwhere + " DATE_FORMAT(rq,'%Y')='" + nd + "'";
                                wfsj = " DATE_FORMAT(wfsj,'%Y')='" + nd + "'";
                                break;
                        }
                        break;

                    case "ORACLE":
                        switch (zqlx)
                        {
                            case "0"://日
                                strwhere = strwhere + "rq=TO_DATE('" + nd + "', 'YYYY-mm-dd')";
                                wfsj = " wfsj=TO_DATE('" + nd + "', 'YYYY-mm-dd')";
                                break;

                            case "1"://月
                                strwhere = strwhere + " TO_DATE(rq,'%Y%m')='" + nd + "'";

                                wfsj = " TO_DATE(wfsj,'%Y%m')='" + nd + "'";
                                break;

                            case "2"://周
                                strwhere = strwhere + " WEEK(rq)='" + nd + "'";
                                wfsj = " WEEK(wfsj)='" + nd + "'";
                                break;

                            case "3"://年
                                strwhere = strwhere + " TO_DATE(rq,'%Y')='" + nd + "'";
                                wfsj = " TO_DATE(wfsj,'%Y')='" + nd + "'";
                                break;
                        }
                        break;
                }
                if (topnum == "")
                    topnum = "10";
                strSql.Append("SELECT a.*,b.STATION_NAME,b.STATION_ID,d.zs,CONCAT(FORMAT((a.wfzs/zs*100),2),'%') wfbl,xpoint,ypoint FROM ");
                strSql.Append("(SELECT xh,wfdd AS kkid,wfxw,SUM(zs) AS wfzs FROM  t_tms_peccancy_count WHERE " + wfsj + " GROUP BY wfdd)  a ");
                strSql.Append("INNER JOIN  (SELECT sta.*,mark.xpoint,mark.ypoint FROM T_CFG_SET_STATION sta INNER JOIN t_gis_device_mark mark ON sta.station_id=mark.relationid WHERE departid='" + departid + "') b ON a.kkid=b.STATION_ID ");
              
                strSql.Append("LEFT JOIN (SELECT kkid ,SUM(zs) zs FROM T_TGS_PASSCAR_COUNT_DAY " + strwhere);
                switch (dataAccess.DataBaseType.ToUpper())
                {
                    case "MYSQL":
                        strSql.Append("GROUP BY kkid ) d ON a.kkid=d.kkid ORDER BY wfzs DESC LIMIT 0,"+topnum);
                        break;

                    case "ORACLE":
                        strSql.Append("GROUP BY kkid ) d ON a.kkid=d.kkid ORDER BY wfzs DESC ROWNUM<10");
                        break;
                }
                return dataAccess.Get_DataSet(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return null;
            }
        }
     public DataTable GetIllegalDetail(string zqlx, string nd, string kkid)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                string strwhere = "where ", wfsj = "";
                switch (dataAccess.DataBaseType.ToUpper())
                {
                    case "MYSQL":
                        switch (zqlx)
                        {
                            case "0"://日
                                strwhere = strwhere + "rq=STR_TO_DATE('" + nd + "', '%Y%m%d')";
                                wfsj = " wfsj=STR_TO_DATE('" + nd + "', '%Y%m%d')";
                                break;

                            case "1"://月
                                strwhere = strwhere + " DATE_FORMAT(rq,'%Y%m')='" + nd + "'";

                                wfsj = " DATE_FORMAT(wfsj,'%Y%m')='" + nd + "'";
                                break;

                            case "2"://周
                                strwhere = strwhere + " WEEK(rq)='" + nd + "'";
                                wfsj = " WEEK(wfsj)='" + nd + "'";
                                break;

                            case "3"://年
                                strwhere = strwhere + " DATE_FORMAT(rq,'%Y')='" + nd + "'";
                                wfsj = " DATE_FORMAT(wfsj,'%Y')='" + nd + "'";
                                break;
                        }
                        break;

                    case "ORACLE":
                        switch (zqlx)
                        {
                            case "0"://日
                                strwhere = strwhere + "rq=TO_DATE('" + nd + "', 'YYYY-mm-dd')";
                                wfsj = " wfsj=TO_DATE('" + nd + "', 'YYYY-mm-dd')";
                                break;

                            case "1"://月
                                strwhere = strwhere + " TO_DATE(rq,'%Y%m')='" + nd + "'";

                                wfsj = " TO_DATE(wfsj,'%Y%m')='" + nd + "'";
                                break;

                            case "2"://周
                                strwhere = strwhere + " WEEK(rq)='" + nd + "'";
                                wfsj = " WEEK(wfsj)='" + nd + "'";
                                break;

                            case "3"://年
                                strwhere = strwhere + " TO_DATE(rq,'%Y')='" + nd + "'";
                                wfsj = " TO_DATE(wfsj,'%Y')='" + nd + "'";
                                break;
                        }
                        break;
                }
                strSql.Append("SELECT a.*,b.STATION_NAME,b.STATION_ID,CONCAT(c.wfxw,'_',c.wfxwjc) wfxwname,d.zs,CONCAT(FORMAT((a.wfzs/zs*100),2),'%') wfbl,xpoint,ypoint FROM ");
                strSql.Append("(SELECT xh,wfdd AS kkid,wfxw,SUM(zs) AS wfzs FROM  t_tms_peccancy_count WHERE " + wfsj + " GROUP BY wfdd,wfxw)  a ");
                strSql.Append("INNER JOIN  (SELECT sta.*,mark.xpoint,mark.ypoint FROM T_CFG_SET_STATION sta INNER JOIN t_gis_device_mark mark ON sta.station_id=mark.relationid WHERE station_id='" + kkid + "') b ON a.kkid=b.STATION_ID ");
                strSql.Append("INNER JOIN  t_tms_peccnacy_type c ON a.wfxw=c.wfxw  ");
                strSql.Append("LEFT JOIN (SELECT kkid ,SUM(zs) zs FROM T_TGS_PASSCAR_COUNT_DAY " + strwhere);
                switch (dataAccess.DataBaseType.ToUpper())
                {
                    case "MYSQL":
                        strSql.Append("GROUP BY kkid ) d ON a.kkid=d.kkid ORDER BY wfzs DESC " );
                        break;

                    case "ORACLE":
                        strSql.Append("GROUP BY kkid ) d ON a.kkid=d.kkid ORDER BY wfzs DESC ROWNUM<10");
                        break;
                }
                return dataAccess.Get_DataTable(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 布控
        /// </summary>
        /// <param name="stationid">卡口列表</param>
        /// <param name="hphm">号牌号码</param>
        /// <param name="cllx">车辆类型</param>
        /// <param name="bdyy">布控原因</param>
        /// <param name="yxsj">有效时间</param>
        /// <param name="sjly"></param>
        /// <param name="bkry">布控人员姓名</param>
        /// <param name="lxdh">布控联系人电话</param>
        ///<param name="cpmh">车牌模糊</param>
        /// <returns></returns>
        public string SetDispatch(List<string> stationid, string hphm, string cllx, string bdyy, string yxsj, string sjly, string mdlx,string bkry,string lxdh,string cpmh,string bklx)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                string xh = GetRecordID(19);
                string kkid = "", kkidmc = "";
                foreach (string str in stationid)
                {
                    kkid += (kkid == "" ? "" : ",") + str;
                    DataSet ds = GetStation("station_id='" + str + "'");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        kkidmc += (kkidmc == "" ? "" : ",") + ds.Tables[0].Rows[0]["station_name"].ToString();
                    }
                }
                strSql.Append("delete FROM t_tgs_info_blacklist WHERE hphm='" + hphm + "' AND hpzl= '" + cllx + "'");
                dataAccess.Execute_NonQuery(strSql.ToString());
                string gxsj = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                switch (dataAccess.DataBaseType.ToUpper())
                {
                    case "MYSQL":
                        gxsj = "'" + gxsj + "'";
                        yxsj = "'" + yxsj + "'";
                        break;

                    case "ORACLE":
                        gxsj = "to_date('" + gxsj + "','yyyy-mm-dd hh24:mi:ss')";
                        yxsj = "to_date('" + yxsj + "','yyyy-mm-dd hh24:mi:ss')";
                        break;
                }
                strSql.Clear();
                strSql.Append("INSERT into t_tgs_info_blacklist (xh,hphm,hpzl,sjyy,sjly,BKKK,BKKKMC,yxsj,gxsj, mdlx,  bdbj,bkr,bkdh,cpmh,bklx) VALUES('");
                strSql.Append(xh + "','" + hphm + "','" + cllx + "','" + bdyy + "','"
                    + sjly + "','"
                    + kkid + "','" + kkidmc + "',"
                    + yxsj + ","
                    + gxsj + ",'" + mdlx + "' ,'1','" + bkry + "','" + lxdh + "','" + cpmh + "','" + bklx + "')");
                int rows = dataAccess.Execute_NonQuery(strSql.ToString());
                if (rows > 0)
                    return xh;
                else
                    return "";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return "";
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
        /// 布控数据到新的表
        /// </summary>
        /// <param name="stationid"></param>
        /// <param name="hphm"></param>
        /// <param name="cllx"></param>
        /// <param name="bdyy"></param>
        /// <param name="yxsj"></param>
        /// <param name="sjly"></param>
        /// <returns></returns>
        public int SetDispatchNew(List<string> stationid, string hphm, string cllx, string bdyy, string yxsj, string sjly, string bkr, string xh, string mdlx)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                if (GeHphmExist("t_tgs_suspect", hphm, cllx) <= 0)
                {
                    //string xh = GetRecordID(19);
                    //string kkid = "", kkidmc = "";
                    //foreach (string str in stationid)
                    //{
                    //    kkid += (kkid == "" ? "" : ",") + str;
                    //    DataSet ds = GetStation("station_id='" + str + "'");
                    //    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    //    {
                    //        kkidmc += (kkidmc == "" ? "" : ",") + ds.Tables[0].Rows[0]["station_name"].ToString();
                    //    }
                    //}
                    //strSql.Append("delete FROM t_tgs_suspect WHERE hphm='" + hphm + "' AND hpzl= '" + cllx + "'");
                    //dataAccess.Execute_NonQuery(strSql.ToString());
                    string gxsj = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    switch (dataAccess.DataBaseType.ToUpper())
                    {
                        case "MYSQL":
                            gxsj = "'" + gxsj + "'";
                            yxsj = "'" + yxsj + "'";
                            break;

                        case "ORACLE":
                            gxsj = "to_date('" + gxsj + "','yyyy-mm-dd hh24:mi:ss')";
                            yxsj = "to_date('" + yxsj + "','yyyy-mm-dd hh24:mi:ss')";
                            break;
                    }

                    strSql.Clear();
                    strSql.Append("INSERT into t_tgs_suspect (bkbh,hphm,hpzl,bkrdw,bz,bkr,bksxrq,bkqsrq, bklx,  bkbj,bkjb) VALUES('");
                    strSql.Append(xh + "','" + hphm + "','" + cllx + "','"
                        + sjly + "','"
                         + bdyy + "','"
                         + bkr + "',"
                        + yxsj + ","
                        + gxsj + ",'" + mdlx + "' ,'3','2')");
                    return dataAccess.Execute_NonQuery(strSql.ToString());
                }
                else
                {
                    //bkbh,hphm,hpzl,bkrdw,bz,bkr,bksxrq,bkqsrq, bklx,  bkbj,bkjb
                    strSql.AppendLine("update  t_tgs_suspect  set ");
                    strSql.AppendLine(" hphm='" + hphm + "',");
                    strSql.AppendLine(" hpzl='" + cllx + "',");
                    strSql.AppendLine(" bkrdw='" + sjly + "',");
                    strSql.AppendLine(" bz='" + bdyy + "',");
                    strSql.AppendLine(" bkr='" + bkr + "',");
                    strSql.AppendLine(" bksxrq='" + yxsj + "',");
                    strSql.AppendLine(" bklx='" + mdlx + "',");
                    strSql.AppendLine(" bkqsrq=now()");
                    strSql.AppendLine(" where hphm='" + hphm + "' and hpzl='" + cllx + "'");
                    return dataAccess.Execute_NonQuery(strSql.ToString());
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return 0;
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
        public int UpdateDispatchNewRecive(string bkbh, string sjly)
        {
            string mySql = string.Empty;
            try
            {
                if (GetBkbhExist(bkbh).Rows.Count <= 0 || GetBkbhExist(bkbh) == null)
                {
                    mySql = "insert into  t_tgs_suspect_recive (jslx,jsdx,bkbh) values(";
                    mySql = mySql + "'1',";
                    mySql = mySql + "'" + sjly + "',";

                    mySql = mySql + "'" + bkbh + "')";

                    return dataAccess.Execute_NonQuery(mySql);
                }
                else
                {
                    mySql = "update  t_tgs_suspect_recive  set ";
                    mySql = mySql + "jsdx='" + sjly + "'";

                    mySql = mySql + " where bkbh='" + bkbh + "'";
                    return dataAccess.Execute_NonQuery(mySql);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public DataTable GetCarHotStation(string time)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("SELECT   a.*,  CONCAT(a.gwbl, '%') gwbl1 FROM  t_tgs_passcar_hotspot_count a WHERE DATE_FORMAT(gxsj, '%Y-%m-%d') = DATE_FORMAT('" + DateTime.Parse(time).ToString("yyyy-MM-dd") + "','%Y-%m-%d')ORDER BY CAST(gwbl AS SIGNED) DESC ");
                //strSql.Append("SELECT * FROM t_tgs_passcar_hotspot_count where gxsj>'" + time + "' AND gxsj<'" + DateTime.Parse(time).AddDays(1).ToString("yyyy-MM-dd") + "' order by cast(gwbl as SIGNED) desc");
                return dataAccess.Get_DataTable(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return null;
            }
        }

        public DataTable GetCarHotRoad(string time)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                switch (dataAccess.DataBaseType.ToUpper())
                {
                    case "ORACLE":
                        strSql.Append("SELECT a.dlid,a.dlmc,NVL(SUM(b.zs),0) zs,round(NVL(SUM(b.zs)/c.total*100 ,0),4) gwbl FROM (SELECT SUM(zs) total FROM t_tgs_passcar_Hotspot_count ) c, ");
                        strSql.Append("(SELECT aa.*,ab.dlmc FROM T_GIS_KEYROAD_CONFIG aa LEFT JOIN t_gis_road ab ON aa.dlid=ab.dlbh) a LEFT JOIN ");
                        strSql.Append("(SELECT kkid,SUM(zs) zs FROM t_tgs_passcar_Hotspot_count GROUP BY kkid) b ON a.kkid=b.kkid ");
                        strSql.Append(" WHERE zs>0 GROUP BY a.dlid,a.dlmc,c.total ORDER BY zs DESC");
                        break;

                    case "MYSQL":
                        strSql.Append("SELECT a.dlid,a.dlmc,IFNULL(SUM(b.zs),0) zs,IFNULL( SUM(b.zs)/c.total*100 ,0) gwbl,concat(IFNULL( SUM(b.zs)/c.total*100 ,0),'%') gwbl1 FROM (SELECT SUM(zs) total FROM t_tgs_passcar_Hotspot_count WHERE DATE_FORMAT(gxsj,'%Y-%m-%d')='" + time + "') c, ");
                        strSql.Append("(SELECT aa.*,ab.dlmc FROM T_GIS_KEYROAD_CONFIG aa LEFT JOIN t_gis_road ab ON aa.dlid=ab.dlbh) a LEFT JOIN ");
                        strSql.Append("(SELECT kkid,SUM(zs) zs FROM t_tgs_passcar_Hotspot_count WHERE DATE_FORMAT(gxsj,'%Y-%m-%d')='" + time + "' GROUP BY kkid) b ON a.kkid=b.kkid ");
                        strSql.Append(" WHERE zs>0 GROUP BY a.dlid ORDER BY zs DESC");
                        break;
                }
                return dataAccess.Get_DataTable(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return null;
            }
        }

        public DataSet GetCarHotRoad()
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                switch (dataAccess.DataBaseType.ToUpper())
                {
                    case "ORACLE":
                        strSql.Append("SELECT a.dlid,a.dlmc,NVL(SUM(b.zs),0) zs,round(NVL(SUM(b.zs)/c.total*100 ,0),4) gwbl FROM (SELECT SUM(zs) total FROM t_tgs_passcar_Hotspot_count ) c, ");
                        strSql.Append("(SELECT aa.*,ab.dlmc FROM T_GIS_KEYROAD_CONFIG aa LEFT JOIN t_gis_road ab ON aa.dlid=ab.dlbh) a LEFT JOIN ");
                        strSql.Append("(SELECT kkid,SUM(zs) zs FROM t_tgs_passcar_Hotspot_count GROUP BY kkid) b ON a.kkid=b.kkid ");
                        strSql.Append(" WHERE zs>0 GROUP BY a.dlid,a.dlmc,c.total ORDER BY zs DESC");
                        break;

                    case "MYSQL":
                        strSql.Append("SELECT a.dlid,a.dlmc,IFNULL(SUM(b.zs),0) zs,IFNULL( SUM(b.zs)/c.total*100 ,0) gwbl,concat(IFNULL( SUM(b.zs)/c.total*100 ,0),'%') gwbl1 FROM (SELECT SUM(zs) total FROM t_tgs_passcar_Hotspot_count ) c, ");
                        strSql.Append("(SELECT aa.*,ab.dlmc FROM T_GIS_KEYROAD_CONFIG aa LEFT JOIN t_gis_road ab ON aa.dlid=ab.dlbh) a LEFT JOIN ");
                        strSql.Append("(SELECT kkid,SUM(zs) zs FROM t_tgs_passcar_Hotspot_count GROUP BY kkid) b ON a.kkid=b.kkid ");
                        strSql.Append(" WHERE zs>0 GROUP BY a.dlid ORDER BY zs DESC");
                        break;
                }
                return dataAccess.Get_DataSet(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return null;
            }
        }

        public string GetRoadPoint(string roadid)
        {
            StringBuilder strSql = new StringBuilder();
            string points = "";
            try
            {
                strSql.Append("SELECT * FROM t_gis_road_points WHERE roadid='" + roadid + "' ORDER BY orderid");
                DataSet ds = dataAccess.Get_DataSet(strSql.ToString());
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        points += (points == "" ? "" : "|") + dr["xpos"].ToString() + "," + dr["ypos"].ToString();
                    }
                }
                return points;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 卡口流量统计
        /// </summary>
        /// <param name="kkid">卡口id</param>
        /// <param name="flowtime">统计时间</param>
        /// <returns></returns>
        public DataTable GetFlowByStation(string kkid, string flowtime)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("SELECT kkid,fxbh, TIME_ID AS HOUR1, CONVERT(IFNULL(LL, '0'), DECIMAL) AS LL,c.direction_desc  FROM (SELECT kkid,fxbh,SUM(ll) ll,xs FROM T_TGS_FLOW_HOUR ");
                strSql.Append(" WHERE  RQ = '" + flowtime + "' GROUP BY kkid,fxbh,xs ORDER BY kkid,fxbh,xs) A ");
                strSql.Append(" RIGHT JOIN T_CFG_TIME B ON A.XS = B.TIME_ID  INNER JOIN t_cfg_direction c ON a.kkid=c.station_id AND a.fxbh=c.direction_id WHERE TIME_TYPE = '1'");
                strSql.Append(" and kkid='" + kkid + "'");
                strSql.Append(" ORDER BY kkid,fxbh,HOUR1 ASC  ");
                DataSet ds = dataAccess.Get_DataSet(strSql.ToString());

                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return null;
            }
        }

        public DataTable GetFlowByRoad(string dlid, string flowtime)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("SELECT xs,SUM(ll) LL,rq FROM T_TGS_FLOW_HOUR ");
                strSql.Append("WHERE  (rq='" + DateTime.Parse(flowtime).AddDays(-1).ToString("yyyy-MM-dd") + "' OR rq='" + flowtime + "') ");
                strSql.Append(" AND kkid IN(SELECT kkid FROM T_GIS_KEYROAD_CONFIG WHERE  dlid='" + dlid + "') GROUP BY rq,xs");
                return dataAccess.Get_DataTable(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return null;
            }
        }

        public string GetFlowByRoadCount(string dlid, string flowtime)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("SELECT SUM(ll) LL,rq FROM T_TGS_FLOW_HOUR ");
                strSql.Append("WHERE  rq in (STR_TO_DATE('" + DateTime.Parse(flowtime).AddDays(-1).ToString("yyyy-MM-dd") + "','%Y-%m-%d'),STR_TO_DATE('" + flowtime + "','%Y-%m-%d')) ");
                strSql.Append(" AND rq!='' AND kkid IN(SELECT kkid FROM T_GIS_KEYROAD_CONFIG WHERE  dlid='" + dlid + "') GROUP BY rq");
                DataTable dt = dataAccess.Get_DataTable(strSql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["rq"].ToString().Substring(0, 10) + "[" + dt.Rows[0]["LL"].ToString() + "]   "
                        + (dt.Rows.Count > 1 ? dt.Rows[1]["rq"].ToString() + "[" + dt.Rows[1]["LL"].ToString() + "]   " : "")
                        + GetFlowByRoadAvgCount(dlid, flowtime);
                }
                else
                    return "";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return "";
            }
        }

        public string GetFlowByRoadAvgCount(string dlid, string flowtime)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("SELECT ROUND(SUM(ll)/4) LL FROM T_TGS_FLOW_HOUR  WHERE kkid IN(SELECT kkid FROM T_GIS_KEYROAD_CONFIG ");
                strSql.Append("WHERE dlid='" + dlid + "') AND ");
                strSql.Append("(rq in STR_TO_DATE('" + flowtime + "','%Y-%m-%d'),STR_TO_DATE('" + DateTime.Parse(flowtime).AddDays(-7).ToString("yyyy-MM-dd") + "','%Y-%m-%d') ,STR_TO_DATE('" + DateTime.Parse(flowtime).AddDays(-14).ToString("yyyy-MM-dd") + "','%Y-%m-%d'),STR_TO_DATE('" + DateTime.Parse(flowtime).AddDays(-21).ToString("yyyy-MM-dd") + ")");
                DataTable dt = dataAccess.Get_DataTable(strSql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                { return "均值[" + dt.Rows[0][0].ToString() + "]"; }
                else
                    return "";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return null;
            }
        }

        public DataTable GetFlowByRoadAvg(string dlid, string flowtime)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("SELECT xs,ROUND(SUM(ll)/4) LL,'avg' AS rq FROM T_TGS_FLOW_HOUR  WHERE kkid IN(SELECT kkid FROM T_GIS_KEYROAD_CONFIG ");
                strSql.Append("WHERE dlid='" + dlid + "') AND ");
                strSql.Append("(rq='" + flowtime + "' OR rq='" + DateTime.Parse(flowtime).AddDays(-7).ToString("yyyy-MM-dd") + "' OR rq='" + DateTime.Parse(flowtime).AddDays(-14).ToString("yyyy-MM-dd") + "'OR rq=" + DateTime.Parse(flowtime).AddDays(-21).ToString("yyyy-MM-dd") + ") GROUP BY xs");
                return dataAccess.Get_DataTable(strSql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql + ex.Message);
                return null;
            }
        }

        public DataTable GetWorkStatic(string kkid, string time)
        {
            string mySql = string.Empty;
            try
            {
                mySql = @"SELECT   a.station_id,  a.station_name,  bb.device_id,  bb.device_name,
  cc.device_ip,  cc.work_state,  cc.work_state AS state,  cc.imagename,  cc.update_time
FROM  t_cfg_set_station a,  t_cfg_station_device aa,  t_dev_device_infor bb ,
  (SELECT c.*,CONCAT(f_to_name ('240006', c.connect_state),'-',f_to_name ('410100', c.work_state)) AS imagename
  FROM    t_dev_device_state_history c where update_time='" + time
+ "') cc WHERE aa.device_id = bb.device_id   AND bb.device_id = cc.device_id   AND a.station_id = aa.station_id AND   a.station_id = '" + kkid + "'";
                DataTable dt = dataAccess.Get_DataTable(mySql);
                if (dt == null || dt.Rows.Count == 0)
                    dt = GetWorkStaticNew(kkid);
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public DataTable GetWorkStaticNew(string kkid)
        {
            string mySql = string.Empty;
            try
            {
                mySql = @"SELECT   a.station_id,  a.station_name,  bb.device_id,  bb.device_name,
  cc.device_ip,  cc.work_state,  cc.work_state AS state,  cc.imagename,  cc.update_time
FROM  t_cfg_set_station a,  t_cfg_station_device aa,  t_dev_device_infor bb ,
  (SELECT c.*,CONCAT(f_to_name ('240006', c.connect_state),'-',f_to_name ('410100', c.work_state)) AS imagename
  FROM    t_dev_device_state c ) cc WHERE aa.device_id = bb.device_id   AND bb.device_id = cc.device_id   AND a.station_id = aa.station_id AND   a.station_id = '" + kkid + "'";

                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        #endregion IMapManager 成员
    }
}