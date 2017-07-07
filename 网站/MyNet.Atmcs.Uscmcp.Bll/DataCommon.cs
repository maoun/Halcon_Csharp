using System;
using System.Data;
using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    /// <summary>
    /// 通用数据操作类
    /// </summary>
    public class DataCommon
    {
        private static readonly IDataCommon dal = DALFactory.CreateDataCommon();

        public delegate DataTable GetFlowDelegate(string directione, string date);

        private SettingManager settingManager = new SettingManager();
        /// <summary>
        ///
        /// </summary>
        /// <param name="pointtype"></param>
        /// <returns></returns>
        #region 查询相关方法
        /// <summary>
        /// 执行查询datatable操作
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>

        public DataTable GetDataTable(string strSQL)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetDataTable(strSQL));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 执行获得stringSQL操作
        /// </summary>
        /// <param name="mySql"></param>
        /// <returns></returns>
        public string GetString(string strSQL)
        {
            try
            {
                return dal.GetString(strSQL);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "";
            }
        }
        /// <summary>
        ///  执行UpdateSQL语句
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public int Update(string strSQL)
        {
            try
            {
                return dal.Update(strSQL);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return 0;
            }
        }
        /// <summary>
        /// 执行Insert操作
        /// </summary>
        /// <param name="mySql"></param>
        /// <returns></returns>
        public int Insert(string strSQL)
        {
            try
            {
                return dal.Insert(strSQL);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return 0;
            }
        }
        /// <summary>
        ///执行Delete操作
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Delete(string strSQL)
        {
            try
            {
                return dal.Delete(strSQL);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return 0;
            }
        }

        public DataTable ChangeDataTablePoliceIp(DataTable dt, string url1, string url2, string url3)
        {
            return dal.ChangeDataTablePoliceIp(dt, url1, url2, url3);
        }

        public string ChangePoliceIp(string url)
        {
            return dal.ChangePoliceIp(url);
        }

        public string ChangeIp(string ipaddress)
        {
            return dal.ChangeIp(ipaddress);
        }

        # endregion
    }
}