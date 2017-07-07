using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using System;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class LogManager : ILogManager
    {
        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        private MyNet.Common.Data.DataAccess dataAccess;

        public LogManager()
        {
            //dataAccess = dac.GetDataAccess(WebConfig.DataAccessName);
            dataAccess = GetDataAccess.Init();
        }

        public LogManager(string dataAccessName)
        {
            dataAccess = dac.GetDataAccess(dataAccessName);
        }

        #region ILogManager 成员

        /// <summary>
        /// 查询业务日志
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetLogBusiness(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select a.* from t_log_Business a where " + where + "   order by recordtime desc"; ;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取错误日志
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetLogError(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from t_log_Error where " + where + " order by errtime desc"; ;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取运行日志统计结果
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="cishu">次数</param>
        /// <returns></returns>
        public DataTable GetLogRunningCount(string where, string where1)
        {
            string mySql = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(where) && !string.IsNullOrEmpty(where1))
                {
                    mySql = @"SELECT * FROM (SELECT  LCODE,
  OPTUSER, TYPE,f_to_name ('240027', TYPE) AS typems,hphm ,COUNT(1) zs ,RECORDTIME FROM t_log_running WHERE  1=1  " + where + " and type in ('4','5','6','7','8') GROUP BY  hphm ,type ) t1 WHERE  1=1 " + where1 + " order by zs desc";
                }
                else if (!string.IsNullOrEmpty(where) && string.IsNullOrEmpty(where1))
                {
                    mySql = @"SELECT * FROM (SELECT  LCODE,
  OPTUSER, TYPE,f_to_name ('240027', TYPE) AS typems,hphm ,COUNT(1) zs ,RECORDTIME FROM t_log_running WHERE  1=1  " + where + " and type in ('4','5','6','7','8')  GROUP BY  hphm ,type  ) t1 WHERE  1=1  order by zs desc";
                }
                else if (string.IsNullOrEmpty(where) && !string.IsNullOrEmpty(where1))
                {
                    mySql = @"SELECT * FROM (SELECT  LCODE,
  OPTUSER, TYPE,f_to_name ('240027', TYPE) AS typems,hphm ,COUNT(1) zs ,RECORDTIME FROM t_log_running WHERE  1=1   GROUP BY  hphm ,type  ) t1 WHERE  1=1 " + where1 + " order by zs desc";
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
        /// 获取运行日志
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetLogRunning(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  LCODE,  OPTUSER,  EVENT,  IP,  RECORDTIME,  TYPE,f_to_name('240027',TYPE) AS typems , HPHM  from t_log_running a where  " + where + " order by recordtime desc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取操作日志详细
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetLogRunningXiangxi(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  LCODE,  OPTUSER,  EVENT,  IP,  RECORDTIME,  TYPE,f_to_name('240027',TYPE) AS typems , HPHM ,wfid  from t_log_running a where  " + where + " order by recordtime desc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 插入业务日志
        /// </summary>
        /// <param name="pname"></param>
        /// <param name="sevent"></param>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        public int InsertLogBusiness(string lid, string pname, string ipaddress, string sevent, string systemid)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_log_business (lcode,pname,entertime,quittime,ipaddress,event,recordtime,systemid) values(";
                mySql = mySql + "'" + lid.ToString() + "',";
                mySql = mySql + "'" + pname + "',";
                mySql = mySql + "now(),";
                mySql = mySql + "now(),";
                mySql = mySql + "'" + sevent + "',";
                mySql = mySql + "now(),";
                mySql = mySql + "'" + systemid + "')";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 插入错误日志
        /// </summary>
        /// <param name="errsource"></param>
        /// <param name="errinfo"></param>
        /// <param name="errdesc"></param>
        /// <returns></returns>
        public int InsertLogError(string errsource, string errinfo, string errdesc)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_log_error (errsource,errinfo,errtime,errdesc) values(";
                mySql = mySql + "'" + errsource + "',";
                mySql = mySql + "'" + errinfo + "',";
                mySql = mySql + "now(),";
                mySql = mySql + "'" + errdesc + "')";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 插入运行日志
        /// </summary>
        /// <param name="optuser"></param>
        /// <param name="strevent"></param>
        /// <param name="ipaddress"></param>
        /// <param name="stype"></param>
        /// <returns></returns>
        public int InsertLogRunning(string systemid, string optuser, string strevent, string ip, string stype)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_log_running (lcode,optuser,event,ip,recordtime,type) values('";
                mySql = mySql + Common.GetRecordId() + "',";
                mySql = mySql + "'" + optuser + "',";
                mySql = mySql + "'" + strevent + "',";
                mySql = mySql + "'" + ip + "',";
                mySql = mySql + "now(),";
                mySql = mySql + "'" + stype + "')";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 插入运行日志
        /// </summary>
        /// <param name="optuser"></param>
        /// <param name="strevent"></param>
        /// <param name="ipaddress"></param>
        /// <param name="stype"></param>
        /// <returns></returns>
        public int InsertLogRunning(string systemid, string optuser, string strevent, string ip, string stype, string hphm, string wfid)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_log_running (lcode,optuser,event,ip,recordtime,type,hphm,wfid) values('";
                mySql = mySql + Common.GetRecordId() + "',";
                mySql = mySql + "'" + optuser + "',";
                mySql = mySql + "'" + strevent + "',";
                mySql = mySql + "'" + ip + "',";
                mySql = mySql + "now(),";
                mySql = mySql + "'" + stype + "',";
                mySql = mySql + "'" + hphm + "',";
                mySql = mySql + "'" + wfid + "')";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int InsertLogNotice(string xh, string tznr, string tzr, string tzsj, string jzsj)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_log_notice (xh,tznr,tzr,tzsj,jzsj) values(";
                mySql = mySql + "'" + xh + "',";
                mySql = mySql + "'" + tznr + "',";
                mySql = mySql + "'" + tzr + "',";
                mySql = mySql + "STR_TO_DATE('" + tzsj + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "STR_TO_DATE('" + jzsj + "','%Y-%m-%d %H:%i:%s'))";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int UpdateLogNotice(string xh, string tznr, string tzr, string tzsj, string jzsj)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_log_notice  set ";
                mySql = mySql + "tznr='" + tznr + "',";
                mySql = mySql + "tzr='" + tzr + "',";
                mySql = mySql + "tzsj=STR_TO_DATE('" + tzsj + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "jzsj=STR_TO_DATE('" + jzsj + "','%Y-%m-%d %H:%i:%s')";
                mySql = mySql + " where xh='" + xh + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int DeleteLogNotice(string xh)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from  t_log_notice  where xh='" + xh + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public DataTable GetlogNotice(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  xh,tzr,tznr,to_char(tzsj,'yyyy-MM-dd HH:mm') tzsj, to_char(jzsj,'yyyy-MM-dd HH:mm') jzsj from t_log_notice where " + where + "   order by tzsj desc"; ;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public DataTable GetNewlogNotice(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from ( select  xh,tzr,tznr,to_char(tzsj,'yyyy-MM-dd HH:mm') tzsj, to_char(jzsj,'yyyy-MM-dd HH:mm') jzsj from t_log_notice  where " + where + "   order by tzsj desc) where  rownum<2 ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        #endregion ILogManager 成员
    }
}