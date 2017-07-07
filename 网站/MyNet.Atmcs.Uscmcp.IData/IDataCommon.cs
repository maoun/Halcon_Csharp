using System.Data;

namespace MyNet.Atmcs.Uscmcp.IData
{
    /// <summary>
    /// 通用数据操作类
    /// </summary>
    public interface IDataCommon
    {
        #region 查询相关方法

        /// <summary>
        /// 执行查询datatable操作
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        DataTable GetDataTable(string strSQL);

        /// <summary>
        /// 执行获得stringSQL操作
        /// </summary>
        /// <param name="mySql"></param>
        /// <returns></returns>
        string GetString(string strSQL);

        /// <summary>
        ///  执行UpdateSQL语句
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int Update(string strSQL);

        /// <summary>
        /// 执行Insert操作
        /// </summary>
        /// <param name="mySql"></param>
        /// <returns></returns>
        int Insert(string strSQL);

        /// <summary>
        ///执行Delete操作
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        int Delete(string strSQL);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="url1"></param>
        /// <param name="url2"></param>
        /// <param name="url3"></param>
        /// <returns></returns>
        DataTable ChangeDataTablePoliceIp(DataTable dt, string url1, string url2, string url3);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        string ChangePoliceIp(string url);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        string ChangeIp(string ipaddress);

        # endregion
    }
}