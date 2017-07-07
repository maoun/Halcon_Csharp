using System.Data;

namespace MyNet.Atmcs.Uscmcp.IData
{
    public interface IDataCountInfo
    {
        /// <summary>
        /// 根据日期统计过车数据
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        string GetPassCarCountDay(string datetime);

        string GetOnlineCarCountDay(string datetime);

        string GetPassCarCountWeek(string week, string year);

        string GetPassCarCountMonth(string month);

        /// <summary>
        /// 获得过车小时流量统计
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        DataTable GetPassCarCountHour(string datetime);

        /// <summary>
        /// 获得在线过车小时流量统计
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        DataTable GetOnlineCarCountHour(string datetime);

        /// <summary>
        /// 根据时间及类型进行过车统计
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable GetPassCarCountByType(string datetime, string type);

        DataTable GetPassCarCountByKKid(string datetime);

        string GetPeccCarCountDay(string datetime);

        DataTable GetPeccCarCountAvgDay(string datetime, int countWeek);

        string GetPeccCarCountWeek(string week, string year);

        string GetPeccCarCountMonth(string month);

        DataTable GetPeccCarCountHour(string datetime);

        DataTable GetPeccCarCountByType(string datetime, string type);

        DataTable GetPeccCarCountByWhere(string where);

        string GetAlarmCarCountDay(string datetime);

        DataTable GetAlarmCarCountAvgDay(string datetime, int countWeek);

        string GetAlarmCarCountWeek(string week, string year);

        string GetAlarmCarCountMonth(string month);

        DataTable GetAlarmCarCountHour(string datetime);

        DataTable GetAlarmCarCountByType(string datetime, string type);

        DataTable GetDeviceInfoByType(string datetime, string type);

        DataTable GetPassCarHotSpot(string datetime, string type);
    }
}