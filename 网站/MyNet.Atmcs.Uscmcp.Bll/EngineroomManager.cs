/***********************************************************************
 * Module:   目录业务逻辑类
 * Author:   李建平
 * Modified: 2008年10月17日
 * Purpose:  该类为页面提供需要的业务逻辑方法
 ***********************************************************************/

using System;
using System.Data;
using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    [Serializable]
    public class EngineroomManagers
    {
        /// <summary>
        ///  用户操作接口
        /// </summary>
        private static readonly IEngineroomManager dal = DALFactory.CreateEngineroomManager();

        public DataTable GetEngineroom(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetEngineroom(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetEngineroomID(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetEngineroomID(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public int insertEngineroom(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.insertEngineroom(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int uptateEngineroom(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.uptateEngineroom(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int DeleteEngineroom(string id)
        {
            try
            {
                return dal.DeleteEngineroom(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }
    }
}