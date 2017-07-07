using System.Data;

namespace MyNet.Atmcs.Uscmcp.IData
{
    public interface ILogManager
    {
        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetLogRunning(string where);

        /// <summary>
        /// 获取操作日志统计结果
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="cishu">次数</param>
        /// <returns></returns>
        DataTable GetLogRunningCount(string where, string where1);

        /// <summary>
        /// 获取操作日志详细
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="cishu">次数</param>
        /// <returns></returns>
        DataTable GetLogRunningXiangxi(string where);

        /// <summary>
        /// 获取错误日志
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetLogError(string where);

        /// 查询业务日志
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetLogBusiness(string where);

        /// <summary>
        /// 插入运行日志
        /// </summary>
        /// <param name="optuser"></param>
        /// <param name="strevent"></param>
        /// <param name="ipaddress"></param>
        /// <param name="stype"></param>
        /// <returns></returns>
        int InsertLogRunning(string lid, string pname, string ipaddress, string sevent, string systemid);

        /// <summary>
        /// 插入运行日志
        /// </summary>
        /// <param name="optuser"></param>
        /// <param name="strevent"></param>
        /// <param name="ipaddress"></param>
        /// <param name="stype"></param>
        /// <returns></returns>
        int InsertLogRunning(string lid, string pname, string ipaddress, string sevent, string systemid, string hphm, string wfid);

        /// <summary>
        /// 插入错误日志
        /// </summary>
        /// <param name="errsource"></param>
        /// <param name="errinfo"></param>
        /// <param name="errdesc"></param>
        /// <returns></returns>
        int InsertLogError(string errsource, string errinfo, string errdesc);

        /// <summary>
        /// 插入业务日志
        /// </summary>
        /// <param name="pname"></param>
        /// <param name="sevent"></param>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        int InsertLogBusiness(string systemid, string optuser, string strevent, string ip, string stype);

        DataTable GetlogNotice(string where);

        DataTable GetNewlogNotice(string where);

        int InsertLogNotice(string xh, string tznr, string tzr, string tzsj, string jzsj);

        int UpdateLogNotice(string xh, string tznr, string tzr, string tzsj, string jzsj);

        int DeleteLogNotice(string xh);
    }
}