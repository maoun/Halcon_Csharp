using System;
using System.Data;
using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class EngineroomManager : IEngineroomManager
    {
        #region DeviceManager 成员

        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        private MyNet.Common.Data.DataAccess dataAccess;

        public EngineroomManager()
        {
            //dataAccess = dac.GetDataAccess(WebConfig.DataAccessName);
            dataAccess = GetDataAccess.Init();
        }

        public EngineroomManager(string dataAccessName)
        {
            dataAccess = dac.GetDataAccess(dataAccessName);
        }

        #endregion DeviceManager 成员

        public DataTable GetEngineroom(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select ID,ENGINEROOM_ID,ENGINEROOM_NAME,f_to_name('241011',ENGINEROOM_TYPE) as ENGINEROOM_TYPE,ENGINEROOM_DRAND,ENGINEROOM_IP,to_char(ENGINEROOM_TIME,'yyyy-mm-dd hh24:mi:ss') as ENGINEROOM_TIME,DEPARTMENT,USERNAME,PASSWORD,PROCESSOR,PROCESSORCOUNT,MEMORYSPACE,";
                mySql += " MEMORYSLOTS,MEMORYTYPE,NETWORKCONTROLLER,HARDDISK,HARDDISK_TYPE,POWERSUPPLY_TYPE,PROCESSORCACHE,INFRASTRUCTURE_MANAGEMENT,DRIVE_TYPE,SHAPE,WARRANTY ";
                mySql += "   from  T_DEV_ENGINEROOM where  1=1 " + where + "   order by ENGINEROOM_TIME asc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public DataTable GetEngineroomID(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select ID,ENGINEROOM_ID,ENGINEROOM_NAME,ENGINEROOM_TYPE,ENGINEROOM_DRAND,ENGINEROOM_IP,to_char(ENGINEROOM_TIME,'yyyy-mm-dd hh24:mi:ss') as ENGINEROOM_TIME,DEPARTMENT,USERNAME,PASSWORD,PROCESSOR,PROCESSORCOUNT,MEMORYSPACE,";
                mySql += " MEMORYSLOTS,MEMORYTYPE,NETWORKCONTROLLER,HARDDISK,HARDDISK_TYPE,POWERSUPPLY_TYPE,PROCESSORCACHE,INFRASTRUCTURE_MANAGEMENT,DRIVE_TYPE,SHAPE,WARRANTY ";
                mySql += "   from  T_DEV_ENGINEROOM where  1=1 " + where + "   order by ENGINEROOM_TIME asc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public int insertEngineroom(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into T_DEV_ENGINEROOM (ID,ENGINEROOM_ID,ENGINEROOM_NAME,ENGINEROOM_TYPE,ENGINEROOM_DRAND,ENGINEROOM_IP,ENGINEROOM_TIME,DEPARTMENT,USERNAME,PASSWORD,PROCESSOR,";
                mySql += "PROCESSORCOUNT,MEMORYSPACE,MEMORYSLOTS,MEMORYTYPE,NETWORKCONTROLLER,HARDDISK,HARDDISK_TYPE,POWERSUPPLY_TYPE,PROCESSORCACHE,INFRASTRUCTURE_MANAGEMENT,DRIVE_TYPE,SHAPE,WARRANTY) values (";
                mySql = mySql + "'" + hs["ID"].ToString() + "',";
                mySql = mySql + "'" + hs["ENGINEROOM_ID"].ToString() + "',";
                mySql = mySql + "'" + hs["ENGINEROOM_NAME"].ToString() + "',";
                mySql = mySql + "'" + hs["ENGINEROOM_TYPE"].ToString() + "',";
                mySql = mySql + "'" + hs["ENGINEROOM_DRAND"].ToString() + "',";
                mySql = mySql + "'" + hs["ENGINEROOM_IP"].ToString() + "',";
                mySql = mySql + "to_date('" + hs["ENGINEROOM_TIME"].ToString() + "','yyyy-mm-dd hh24:mi:ss'),";
                mySql = mySql + "'" + hs["DEPARTMENT"].ToString() + "',";
                mySql = mySql + "'" + hs["USERNAME"].ToString() + "',";
                mySql = mySql + "'" + hs["PASSWORD"].ToString() + "',";
                mySql = mySql + "'" + hs["PROCESSOR"].ToString() + "',";
                mySql = mySql + "'" + hs["PROCESSORCOUNT"].ToString() + "',";
                mySql = mySql + "'" + hs["MEMORYSPACE"].ToString() + "',";
                mySql = mySql + "'" + hs["MEMORYSLOTS"].ToString() + "',";
                mySql = mySql + "'" + hs["MEMORYTYPE"].ToString() + "',";
                mySql = mySql + "'" + hs["NETWORKCONTROLLER"].ToString() + "',";
                mySql = mySql + "'" + hs["HARDDISK"].ToString() + "',";
                mySql = mySql + "'" + hs["HARDDISK_TYPE"].ToString() + "',";
                mySql = mySql + "'" + hs["POWERSUPPLY_TYPE"].ToString() + "',";
                mySql = mySql + "'" + hs["PROCESSORCACHE"].ToString() + "',";
                mySql = mySql + "'" + hs["INFRASTRUCTURE_MANAGEMENT"].ToString() + "',";
                mySql = mySql + "'" + hs["DRIVE_TYPE"].ToString() + "',";
                mySql = mySql + "'" + hs["SHAPE"].ToString() + "',";
                mySql = mySql + "'" + hs["WARRANTY"].ToString() + "'";
                mySql = mySql + "   )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int uptateEngineroom(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "  update T_DEV_ENGINEROOM set ";
                mySql = mySql + "ENGINEROOM_NAME='" + hs["ENGINEROOM_NAME"].ToString() + "',";
                mySql = mySql + "ENGINEROOM_TYPE='" + hs["ENGINEROOM_TYPE"].ToString() + "',";
                mySql = mySql + "ENGINEROOM_DRAND='" + hs["ENGINEROOM_DRAND"].ToString() + "',";
                mySql = mySql + "ENGINEROOM_IP='" + hs["ENGINEROOM_IP"].ToString() + "',";
                mySql = mySql + "ENGINEROOM_TIME=to_date('" + hs["ENGINEROOM_TIME"].ToString() + "','yyyy-mm-dd hh24:mi:ss'),";
                mySql = mySql + "DEPARTMENT='" + hs["DEPARTMENT"].ToString() + "',";
                mySql = mySql + "USERNAME='" + hs["USERNAME"].ToString() + "',";
                mySql = mySql + "PASSWORD='" + hs["PASSWORD"].ToString() + "',";
                mySql = mySql + "PROCESSOR='" + hs["PROCESSOR"].ToString() + "',";
                mySql = mySql + "PROCESSORCOUNT='" + hs["PROCESSORCOUNT"].ToString() + "',";
                mySql = mySql + "MEMORYSPACE='" + hs["MEMORYSPACE"].ToString() + "',";
                mySql = mySql + "MEMORYSLOTS='" + hs["MEMORYSLOTS"].ToString() + "',";
                mySql = mySql + "MEMORYTYPE='" + hs["MEMORYTYPE"].ToString() + "',";
                mySql = mySql + "NETWORKCONTROLLER='" + hs["NETWORKCONTROLLER"].ToString() + "',";
                mySql = mySql + "HARDDISK='" + hs["HARDDISK"].ToString() + "',";
                mySql = mySql + "HARDDISK_TYPE='" + hs["HARDDISK_TYPE"].ToString() + "',";
                mySql = mySql + "POWERSUPPLY_TYPE='" + hs["POWERSUPPLY_TYPE"].ToString() + "',";
                mySql = mySql + "PROCESSORCACHE='" + hs["PROCESSORCACHE"].ToString() + "',";
                mySql = mySql + "INFRASTRUCTURE_MANAGEMENT='" + hs["INFRASTRUCTURE_MANAGEMENT"].ToString() + "',";
                mySql = mySql + "DRIVE_TYPE='" + hs["DRIVE_TYPE"].ToString() + "',";
                mySql = mySql + "SHAPE='" + hs["SHAPE"].ToString() + "',";
                mySql = mySql + "WARRANTY='" + hs["WARRANTY"].ToString() + "'";
                mySql += " where ID='" + hs["ID"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int DeleteEngineroom(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from T_DEV_ENGINEROOM where id='" + id + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }
    }
}