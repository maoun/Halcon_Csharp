using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using System;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class SystemManagerNew : ISystemManagerNew
    {
        /// <summary>
        ///
        /// </summary>
        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        /// <summary>
        ///
        /// </summary>
        private MyNet.Common.Data.DataAccess dataAccess;

        /// <summary>
        ///
        /// </summary>
        public SystemManagerNew()
        {
            //type=0 读取uscmcp数据库（老库） type=1 读取frame数据库（新库）
            string type = System.Configuration.ConfigurationManager.AppSettings["BkType"].ToString();
            if (type.Equals("0"))
            {
                dataAccess = GetDataAccess.Init();
            }
            else if (type.Equals("1"))
            {
                dataAccess = GetDataAccess.InitNew();
            }
            else
            {
                dataAccess = GetDataAccess.Init();
            }
        }

        /// <summary>
        /// 写入到新表中
        /// </summary>
        public SystemManagerNew(int i)
        {
            if (i == 1)
            {
                dataAccess = GetDataAccess.InitNew();
            }
            else
            {
                dataAccess = GetDataAccess.Init();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataAccessName"></param>
        public SystemManagerNew(string dataAccessName)
        {
            dataAccess = dac.GetDataAccess(dataAccessName);
        }

        #region ISystemManager 成员

        /// <summary>
        /// 读取字典项
        /// </summary>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public DataTable GetCode(string codeType)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select *  from t_sys_code where codeType='" + codeType + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DataTable GetSystemInfo()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from t_sys_system";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetSystemInfo(string systemId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from t_sys_system where systemId='" + systemId + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DataTable GetCodeType()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from t_sys_codetype order by codetype asc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int AddSysCode(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_sys_code (codetype, code,codedesc,remark,isuse) values(";
                mySql = mySql + "'" + hs["codetype"].ToString() + "',";
                mySql = mySql + "'" + hs["code"].ToString() + "',";
                mySql = mySql + "'" + hs["codedesc"].ToString() + "',";
                mySql = mySql + "'" + hs["remark"].ToString() + "',";
                mySql = mySql + "'" + hs["isuse"].ToString() + "' )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int AddSysCodeType(System.Collections.Hashtable hs)
        {
            return 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DelSysCode(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_sys_code   ";
                mySql = mySql + " where codetype='" + hs["codetype"].ToString() + "'";
                mySql = mySql + " and  code='" + hs["oldcode"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DelSysCodeType(System.Collections.Hashtable hs)
        {
            return 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateSysCode(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_sys_code  set ";
                mySql = mySql + "code='" + hs["code"].ToString() + "',";
                mySql = mySql + "codedesc='" + hs["codedesc"].ToString() + "',";
                mySql = mySql + "remark='" + hs["remark"].ToString() + "',";
                mySql = mySql + "isuse='" + hs["isuse"].ToString() + "' ";
                mySql = mySql + " where codetype='" + hs["codetype"].ToString() + "'";
                mySql = mySql + " and  code='" + hs["oldcode"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateSysCodeType(System.Collections.Hashtable hs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion ISystemManager 成员

        #region ISystemManager 成员

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int AddSysFunction(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_sys_function (funcid,funcname,funcaddress,functype,formtype,iconname,iconvalue) values(";
                mySql = mySql + "'" + hs["funcid"].ToString() + "',";
                mySql = mySql + "'" + hs["funcname"].ToString() + "',";
                mySql = mySql + "'" + hs["funcaddress"].ToString() + "',";
                mySql = mySql + "'" + hs["functype"].ToString() + "',";
                mySql = mySql + "'" + hs["formtype"].ToString() + "',";
                mySql = mySql + "'" + hs["iconname"].ToString() + "',";
                mySql = mySql + "'" + hs["iconvalue"].ToString() + "' )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DelSysFunction(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_sys_function   ";
                mySql = mySql + " where funcid='" + hs["funcid"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateSysFunction(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_sys_function  set ";
                mySql = mySql + "FuncName='" + hs["funcname"].ToString() + "',";
                mySql = mySql + "funcAddress='" + hs["funcaddress"].ToString() + "',";
                mySql = mySql + "funcType='" + hs["functype"].ToString() + "',";
                mySql = mySql + "formtype='" + hs["formtype"].ToString() + "',";
                mySql = mySql + "IconValue='" + hs["iconvalue"].ToString() + "', ";
                mySql = mySql + "IconName='" + hs["iconname"].ToString() + "' ";
                mySql = mySql + " where funcid='" + hs["funcid"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DataTable GetSysFunction()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select funcid,FuncName,funcAddress,f_to_name('240041',funcType),f_to_name('240042',FormType),IconValue,IconName from t_sys_function order by funcid asc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="funcId"></param>
        /// <returns></returns>
        public DataTable GetSysFunction(string funcId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from t_sys_function where funcid='" + funcId + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        #endregion ISystemManager 成员

        #region ISystemManager 成员

        /// <summary>
        ///  生成记录ID
        /// </summary>
        /// <param name="head"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        public string GetRecordID(string head, int totalLength)
        {
            string RecordID = string.Empty;
            try
            {
                int len = totalLength - head.Length;

                string tempID = Math.Round(DateTime.Now.Subtract(DateTime.Parse("2000-1-1")).TotalMilliseconds, 0).ToString().PadLeft(len, '0');
                if (tempID.Length > len)
                {
                    tempID = tempID.Substring(tempID.Length - len);
                }
                RecordID = head + tempID;
                return RecordID;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(RecordID + ex.Message);
                RecordID = head + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (RecordID.Length > totalLength)
                {
                    RecordID = RecordID.Substring(RecordID.Length - totalLength);
                }
                return RecordID;
            }
        }

        #endregion ISystemManager 成员
    }
}