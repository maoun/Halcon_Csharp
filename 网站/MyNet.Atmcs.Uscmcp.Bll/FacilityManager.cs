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
    public class FacilityManager
    {
        /// <summary>
        ///  用户操作接口
        /// </summary>
        private static readonly IFacilityManager dal = DALFactory.CreateFacilityManager();

        public DataTable GetFacility(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.Facility(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public int insertFacility_SignageMark(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.insertFacility_SignageMark(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int updateFacility_SignageMark(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.updateFacility_SignageMark(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public DataTable getFacility(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.getFacility(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable getFacilityid(string id)
        {
            try
            {
                return dal.getFacilityid(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable selectlukou(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.selectlukou(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public int DeleteFacility_SignageMark(string id)
        {
            try
            {
                return dal.DeleteFacility_SignageMark(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int insertFacility_Isolation(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.insertFacility_Isolation(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int updateFacility_Isolation(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.updateFacility_Isolation(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int insertFacility_Traffic(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.insertFacility_Traffic(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public int updateFacility_Traffic(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.updateFacility_Traffic(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        public DataTable selectid(string id)
        {
            try
            {
                return Bll.Common.ChangColName(dal.selectid(id));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
    }
}