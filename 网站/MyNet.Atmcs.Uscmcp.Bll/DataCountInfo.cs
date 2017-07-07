using System;
using System.Data;
using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class DataCountInfo
    {
        ///  用户操作接口
        /// </summary>
        private static readonly IDataCountInfo dal = DALFactory.CreateDataCountInfo();

        /// <summary>
        /// 根据日期统计过车数据
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public string GetPassCarCountDay(string datetime)
        {
            try
            {
                return dal.GetPassCarCountDay(datetime);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "0";
            }
        }

        public string GetOnlineCarCountDay(string datetime)
        {
            try
            {
                return dal.GetOnlineCarCountDay(datetime);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "0";
            }
        }

        public string GetPassCarCountWeek(string week, string year)
        {
            try
            {
                return dal.GetPassCarCountWeek(week, year);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "0";
            }
        }

        public string GetPassCarCountMonth(string month)
        {
            try
            {
                return dal.GetPassCarCountMonth(month);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "0";
            }
        }

        /// <summary>
        /// 获得过车小时流量统计
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public DataTable GetPassCarCountHour(string datetime)
        {
            try
            {
                return dal.GetPassCarCountHour(datetime);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.GetOnlineCarCountHour(datetime);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.GetPassCarCountByType(datetime, type);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetPassCarCountByKKid(string datetime)
        {
            try
            {
                return dal.GetPassCarCountByKKid(datetime);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public string GetPeccCarCountDay(string datetime)
        {
            try
            {
                return dal.GetPeccCarCountDay(datetime);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public string GetPeccCarCountWeek(string week, string year)
        {
            try
            {
                return dal.GetPeccCarCountWeek(week, year);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public string GetPeccCarCountMonth(string month)
        {
            try
            {
                return dal.GetPeccCarCountMonth(month);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetPeccCarCountAvgDay(string datetime, int countWeek)
        {
            try
            {
                return dal.GetPeccCarCountAvgDay(datetime, countWeek);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetPeccCarCountHour(string datetime)
        {
            try
            {
                return dal.GetPeccCarCountHour(datetime);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetPeccCarCountByType(string datetime, string type)
        {
            try
            {
                return dal.GetPeccCarCountByType(datetime, type);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetPeccCarCountByWhere(string where)
        {
            try
            {
                return dal.GetPeccCarCountByWhere(where);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public string GetAlarmCarCountDay(string datetime)
        {
            try
            {
                return dal.GetAlarmCarCountDay(datetime);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "0";
            }
        }

        public DataTable GetAlarmCarCountAvgDay(string datetime, int countWeek)
        {
            try
            {
                return dal.GetAlarmCarCountAvgDay(datetime, countWeek);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public string GetAlarmCarCountWeek(string week, string year)
        {
            try
            {
                return dal.GetAlarmCarCountWeek(week, year);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "0"; ;
            }
        }

        public string GetAlarmCarCountMonth(string month)
        {
            try
            {
                return dal.GetAlarmCarCountMonth(month);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "0";
            }
        }

        public DataTable GetAlarmCarCountHour(string datetime)
        {
            try
            {
                return dal.GetAlarmCarCountHour(datetime);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetAlarmCarCountByType(string datetime, string type)
        {
            try
            {
                return dal.GetAlarmCarCountByType(datetime, type);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetDeviceInfoByType(string datetime, string type)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetDeviceInfoByType(datetime, type));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetPassCarHotSpot(string datetime, string type)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetPassCarHotSpot(datetime, type));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
    }
}