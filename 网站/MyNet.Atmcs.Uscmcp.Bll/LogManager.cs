using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using System;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class LogManager
    {
        private SettingManager settingManager = new SettingManager();

        /// <summary>
        ///  用户操作接口
        /// </summary>
        private static readonly ILogManager dal = DALFactory.CreateLogManager();

        /// <summary>
        ///获取日志类型
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetLogType(string systemId)
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(systemId, "240027"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取错误日志
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public DataTable GetLogError(string startTime, string endTime)
        {
            try
            {
                string where = "  errtime >= STR_TO_DATE('" + startTime + "','%Y-%m-%d %H:%i:%s')   and errtime<=STR_TO_DATE('" + endTime + "','%Y-%m-%d %H:%i:%s')";
                return Bll.Common.ChangColName(dal.GetLogError(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取业务日志
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="querytype"></param>
        /// <returns></returns>
        public DataTable GetLogBusiness(string startTime, string endTime, string querytype)
        {
            try
            {
                string where = default(string);
                if (querytype == null || querytype == "")
                {
                    where = "  recordtime >= STR_TO_DATE('" + startTime + "','%Y-%m-%d %H:%i:%s')   and recordtime<=STR_TO_DATE('" + endTime + "','%Y-%m-%d %H:%i:%s')";
                }
                else
                {
                    where = "  recordtime >= STR_TO_DATE('" + startTime + "','%Y-%m-%d %H:%i:%s')   and recordtime<=STR_TO_DATE('" + endTime + "','%Y-%m-%d %H:%i:%s')  and type='" + querytype + "'";
                }
                return Bll.Common.ChangColName(dal.GetLogBusiness(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取运行日志
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="querytype"></param>
        /// <returns></returns>
        public DataTable GetLogRunning(string startTime, string endTime, string querytype, string username)
        {
            try
            {
                string where = "";

                where = "  recordtime >= STR_TO_DATE('" + startTime + "','%Y-%m-%d %H:%i:%s')   and recordtime<=STR_TO_DATE('" + endTime + "','%Y-%m-%d %H:%i:%s') ";
                if (!string.IsNullOrEmpty(querytype))
                {
                    where += "  and type='" + querytype + "'  ";
                }
                if (!string.IsNullOrEmpty(username))
                {
                    where += "  and optuser='" + username + "'  ";
                }
                return Bll.Common.ChangColName(dal.GetLogRunning(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="querytype"></param>
        /// <returns></returns>
        public DataTable GetLogRunning(string startTime, string endTime, string querytype, string username, string cishu)
        {
            try
            {
                string where = ""; string where1 = "";
                where = " and recordtime >= STR_TO_DATE('" + startTime + "','%Y-%m-%d %H:%i:%s')   and recordtime<=STR_TO_DATE('" + endTime + "','%Y-%m-%d %H:%i:%s') ";
                if (!string.IsNullOrEmpty(querytype))
                {
                    where1 += "  and type='" + querytype + "'  ";
                }
                if (!string.IsNullOrEmpty(username))
                {
                    where += "  and optuser='" + username + "'  ";
                }
                if (!string.IsNullOrEmpty(cishu))
                {
                    where1 += " and zs>= " + cishu;
                }

                return Bll.Common.ChangColName(dal.GetLogRunningCount(where, where1));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取操作日志详细
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="querytype"></param>
        /// <returns></returns>
        public DataTable GetLogRunning(string startTime, string endTime, string querytype, string username, string cishu, string hphm)
        {
            try
            {
                string where = "";

                where = "  recordtime >= STR_TO_DATE('" + startTime + "','%Y-%m-%d %H:%i:%s')   and recordtime<=STR_TO_DATE('" + endTime + "','%Y-%m-%d %H:%i:%s') ";
                if (!string.IsNullOrEmpty(querytype))
                {
                    where += "  and type='" + querytype + "'  ";
                }
                if (!string.IsNullOrEmpty(username))
                {
                    where += "  and optuser='" + username + "'  ";
                }
                if (!string.IsNullOrEmpty(username))
                {
                    where += "  and hphm='" + hphm + "'  ";
                }
                return Bll.Common.ChangColName(dal.GetLogRunningXiangxi(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
        public int InsertLogBusiness(string pname, string sevent, string ipaddress)
        {
            try
            {
                string SystemID = "00";
                //string ipaddress = System.Web.HttpContext.Current.Request.UserHostAddress;
                string id = pname + ipaddress + SystemID + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
                long lid = Math.Abs(id.GetHashCode());
                return dal.InsertLogBusiness(lid.ToString(), pname, ipaddress, sevent, SystemID);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.InsertLogError(errsource, errinfo, errdesc);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
        public int InsertLogRunning(string optuser, string strevent, string ipaddress, string stype)
        {
            try
            {
                string SystemID = "00";
                return dal.InsertLogRunning(SystemID, optuser, strevent, ipaddress, stype);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
        public int InsertLogRunning(string optuser, string strevent, string ipaddress, string stype, string hphm, string wfid)
        {
            try
            {
                string SystemID = "00";
                return dal.InsertLogRunning(SystemID, optuser, strevent, ipaddress, stype, hphm, wfid);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public DataTable GetLogNotice(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetlogNotice(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetNewLogNotice(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetNewlogNotice(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public int InsertLogNotice(string xh, string tznr, string tzr, string tzsj, string jzsj)
        {
            try
            {
                return dal.InsertLogNotice(xh, tznr, tzr, tzsj, jzsj);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int UpdateLogNotice(string xh, string tznr, string tzr, string tzsj, string jzsj)
        {
            try
            {
                return dal.UpdateLogNotice(xh, tznr, tzr, tzsj, jzsj);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int DeleteLogNotice(string xh)
        {
            try
            {
                return dal.DeleteLogNotice(xh);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }
    }
}