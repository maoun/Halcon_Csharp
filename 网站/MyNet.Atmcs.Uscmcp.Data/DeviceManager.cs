using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Data
{
    /// <summary>
    /// 设备管理实现
    /// </summary>
    public class DeviceManager : IDeviceManager
    {
        #region DeviceManager 成员

        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        private MyNet.Common.Data.DataAccess dataAccess;

        public DeviceManager()
        {
            //dataAccess = dac.GetDataAccess(WebConfig.DataAccessName);
            dataAccess = GetDataAccess.Init();
        }

        public DeviceManager(string dataAccessName)
        {
            dataAccess = dac.GetDataAccess(dataAccessName);
        }

        #endregion DeviceManager 成员

        #region IDeviceManager 成员

        /// <summary>
        ///
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetConfigDepartment(string systemId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from t_dev_shebeileixing  where  departid like '%" + systemId + "%'  order by class asc ";
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
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetTree(string code)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select * from t_sys_code where " + code + " ";
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
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTreeDepartment(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from t_cfg_department where " + where + " ";
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
        /// <param name="queryField"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetTree(string queryField, string code)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select " + queryField + " from t_sys_code where " + code + " ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查找设备运维信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetOperation(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select  id,device_id,openration_people,DATE_FORMAT(openration_time,'%Y-%m-%d %H:%i:%s') as openration_time,openration_event,f_to_name('281001',openration_type) as openration_type, f_to_name('281002',openration_unit) as openration_unit,openration_auditor, openration_opinion,DATE_FORMAT(openration_revtime,'%Y-%m-%d %H:%i:%s') as openration_revtime from t_dev_operation  where " + where + "  order by openration_time desc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 更新设备详细信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int uptateDevice(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "  update t_dev_device set ";
                mySql += " device_id='" + hs["device_id"].ToString() + "'";
                mySql += ", device_name='" + hs["device_name"].ToString() + "'";
                mySql += " ,device_mode='" + hs["device_mode"].ToString() + "'";
                mySql += " ,device_type='" + hs["device_type"].ToString() + "'";
                mySql += " ,device_use='" + hs["device_use"].ToString() + "'";
                mySql += " ,department='" + hs["department"].ToString() + "'";
                mySql += " ,devsequence='" + hs["devsequence"].ToString() + "'";
                mySql += " ,ipaddress='" + hs["ipaddress"].ToString() + "'";
                mySql += " ,port='" + hs["port"].ToString() + "'";
                mySql += " ,username='" + hs["username"].ToString() + "'";
                mySql += " ,password='" + hs["password"].ToString() + "'";
                mySql += " ,channel='" + hs["channel"].ToString() + "'";
                mySql += " ,rtsp='" + hs["rtsp"].ToString() + "'";
                mySql += " ,width='" + hs["width"].ToString() + "'";
                mySql += " ,height='" + hs["height"].ToString() + "'";
                mySql += " ,communtype='" + hs["communtype"].ToString() + "'";
                mySql += " ,commport='" + hs["commport"].ToString() + "'";
                mySql += " ,commsetting='" + hs["commsetting"].ToString() + "',";
                mySql = mySql + "BUILD_COMPANY='" + hs["BUILD_COMPANY"].ToString() + "',";
                mySql = mySql + "BUILD_PERSON='" + hs["BUILD_PERSON"].ToString() + "',";
                mySql = mySql + "BUILD_PHONE='" + hs["BUILD_PHONE"].ToString() + "',";
                mySql = mySql + "MAINTAIN_COMPANY='" + hs["MAINTAIN_COMPANY"].ToString() + "',";
                mySql = mySql + "MAINTAIN_PERSON='" + hs["MAINTAIN_PERSON"].ToString() + "',";
                mySql = mySql + "MAINTAIN_PHONE='" + hs["MAINTAIN_PHONE"].ToString() + "',";
                mySql = mySql + "MAKE_COMPANY='" + hs["MAKE_COMPANY"].ToString() + "',";
                mySql = mySql + "MAKE_PHONE='" + hs["MAKE_PHONE"].ToString() + "'";
                mySql += " ,network_type='" + hs["network_type"].ToString() + "'";
                mySql += " where id='" + hs["id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 插入设备详细信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int insertDevice(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into t_dev_device (id,device_id,device_name,device_mode,device_type,device_use,department,devsequence,ipaddress,port,username,password,channel,rtsp,width,height,communtype,commsetting,network_type,commport,BUILD_COMPANY,BUILD_PERSON,BUILD_PHONE,MAINTAIN_COMPANY,MAINTAIN_PERSON,MAINTAIN_PHONE,MAKE_COMPANY,MAKE_PHONE) values (";
                mySql = mySql + "'" + hs["id"].ToString() + "',";
                mySql = mySql + "'" + hs["device_id"].ToString() + "',";
                mySql = mySql + "'" + hs["device_name"].ToString() + "',";
                mySql = mySql + "'" + hs["device_mode"].ToString() + "',";
                mySql = mySql + "'" + hs["device_type"].ToString() + "',";
                mySql = mySql + "'" + hs["device_use"].ToString() + "',";
                mySql = mySql + "'" + hs["department"].ToString() + "',";
                mySql = mySql + "'" + hs["devsequence"].ToString() + "',";
                mySql = mySql + "'" + hs["ipaddress"].ToString() + "',";
                mySql = mySql + "'" + hs["port"].ToString() + "',";
                mySql = mySql + "'" + hs["username"].ToString() + "',";
                mySql = mySql + "'" + hs["password"].ToString() + "',";
                mySql = mySql + "'" + hs["channel"].ToString() + "',";
                mySql = mySql + "'" + hs["rtsp"].ToString() + "',";
                mySql = mySql + "'" + hs["width"].ToString() + "',";
                mySql = mySql + "'" + hs["height"].ToString() + "',";
                mySql = mySql + "'" + hs["communtype"].ToString() + "',";

                mySql = mySql + "'" + hs["commsetting"].ToString() + "',";
                mySql = mySql + "'" + hs["network_type"].ToString() + "',";
                mySql = mySql + "'" + hs["commport"].ToString() + "',";
                mySql = mySql + "'" + hs["BUILD_COMPANY"].ToString() + "',";
                mySql = mySql + "'" + hs["BUILD_PERSON"].ToString() + "',";
                mySql = mySql + "'" + hs["BUILD_PHONE"].ToString() + "',";
                mySql = mySql + "'" + hs["MAINTAIN_COMPANY"].ToString() + "',";
                mySql = mySql + "'" + hs["MAINTAIN_PERSON"].ToString() + "',";
                mySql = mySql + "'" + hs["MAINTAIN_PHONE"].ToString() + "',";
                mySql = mySql + "'" + hs["MAKE_COMPANY"].ToString() + "',";
                mySql = mySql + "'" + hs["MAKE_PHONE"].ToString() + "'";
                mySql = mySql + "   )";
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
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSelectDevice(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select * from  t_dev_device where id= '" + where + "' ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得设备运维统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTongJi(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select sum(case openration_type when '01' then 1 else 0 end) as s1,sum(case openration_type when '02' then 1 else 0 end) as s2,sum(case openration_type when '03' then 1 else 0 end) as s3,sum(case openration_type when '04' then 1 else 0 end) as s4";

                mySql += " from t_dev_operation where 1=1 " + where + "";

                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 录入设备运维信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int insertOperation(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_dev_operation (id,device_id,openration_people,openration_time,openration_event,openration_type,openration_unit,openration_auditor,openration_opinion,openration_revtime) values (";
                mySql = mySql + "'" + hs["id"].ToString() + "',";
                mySql = mySql + "'" + hs["device_id"].ToString() + "',";
                mySql = mySql + "'" + hs["openration_people"].ToString() + "',";
                mySql = mySql + "STR_TO_DATE('" + hs["openration_time"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "'" + hs["openration_event"].ToString() + "',";
                mySql = mySql + "'" + hs["openration_type"].ToString() + "',";
                mySql = mySql + "'" + hs["openration_unit"].ToString() + "',";
                mySql = mySql + "'" + hs["openration_auditor"].ToString() + "',";
                mySql = mySql + "'" + hs["openration_opinion"].ToString() + "',";
                mySql = mySql + "STR_TO_DATE('" + hs["openration_revtime"].ToString() + "','%Y-%m-%d %H:%i:%s')";
                mySql = mySql + " )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///更新设备的运维信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int uptateOperation(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update t_dev_operation set ";
                mySql += " openration_people='" + hs["openration_people"].ToString() + "',";
                mySql += " openration_time=STR_TO_DATE('" + hs["openration_time"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql += " openration_event='" + hs["openration_event"].ToString() + "',";
                mySql += " openration_type='" + hs["openration_type"].ToString() + "',";
                mySql += " openration_unit='" + hs["openration_unit"].ToString() + "',";
                mySql += " openration_auditor='" + hs["openration_auditor"].ToString() + "',";
                mySql += " openration_opinion='" + hs["openration_opinion"].ToString() + "',";
                mySql += " openration_revtime=STR_TO_DATE('" + hs["openration_revtime"].ToString() + "','%Y-%m-%d %H:%i:%s')";
                mySql += " where  id='" + hs["id"].ToString() + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 查询选中设备的运维信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSelectOperation(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select id,device_id,openration_people,DATE_FORMAT(openration_time,'%Y-%m-%d %H:%i:%s') as openration_time,openration_event, openration_type, openration_unit,openration_auditor, openration_opinion,DATE_FORMAT(openration_revtime,'%Y-%m-%d %H:%i:%s') as openration_revtime  from  t_dev_operation where  " + where + " ";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteOperation(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_dev_operation where id='" + id + "'";

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
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteCCTV(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from T_CCTV_CAM_SETTING where id='" + id + "'";

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
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteLED(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from T_VMS_LED where id='" + id + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 增加设备数据
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int insertDevices(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into t_dev_device (id,device_id,device_name,device_mode,device_type,device_use,station_id,direction_id,build_person,build_phone,maintain_company,maintain_person,maintain_phone,make_company,make_phone,build) values (";
                mySql = mySql + "'" + hs["id"].ToString() + "',";
                mySql = mySql + "'" + hs["device_id"].ToString() + "',";
                mySql = mySql + "'" + hs["device_name"].ToString() + "',";
                mySql = mySql + "'" + hs["device_mode"].ToString() + "',";
                mySql = mySql + "'" + hs["device_type"].ToString() + "',";
                mySql = mySql + "'" + hs["device_use"].ToString() + "',";
                mySql = mySql + "'" + hs["station_id"].ToString() + "',";
                mySql = mySql + "'" + hs["direction_id"].ToString() + "',";
                mySql = mySql + "'" + hs["build_person"].ToString() + "',";
                mySql = mySql + "'" + hs["build_phone"].ToString() + "',";
                mySql = mySql + "'" + hs["maintain_company"].ToString() + "',";
                mySql = mySql + "'" + hs["maintain_person"].ToString() + "',";
                mySql = mySql + "'" + hs["maintain_phone"].ToString() + "',";
                mySql = mySql + "'" + hs["make_company"].ToString() + "',";
                mySql = mySql + "'" + hs["make_phone"].ToString() + "',";
                mySql = mySql + "'" + hs["build"].ToString() + "'";

                mySql = mySql + "   )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 修改设备数据
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int uptateDevices(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "  update t_dev_device set ";
                mySql += " device_id='" + hs["device_id"].ToString() + "',";
                mySql += " device_name='" + hs["device_name"].ToString() + "',";
                mySql += " device_mode='" + hs["device_mode"].ToString() + "',";
                mySql = mySql + Common.GetHashtableStr(hs, "device_type", "device_type");
                mySql += " device_use='" + hs["device_use"].ToString() + "',";
                mySql = mySql + Common.GetHashtableStr(hs, "station_id", "station_id");
                mySql = mySql + Common.GetHashtableStr(hs, "direction_id", "direction_id");
                mySql += " build_person='" + hs["build_person"].ToString() + "',";
                mySql += " build_phone='" + hs["build_phone"].ToString() + "',";
                mySql += " maintain_company='" + hs["maintain_company"].ToString() + "',";
                mySql += " maintain_person='" + hs["maintain_person"].ToString() + "',";
                mySql += " maintain_phone='" + hs["maintain_phone"].ToString() + "',";
                mySql += " make_company='" + hs["make_company"].ToString() + "',";
                mySql += " make_phone='" + hs["make_phone"].ToString() + "',";
                mySql += " build='" + hs["build"].ToString() + "'";
                mySql += " where id='" + hs["id"].ToString() + "'";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteKaKou(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from T_TGS_SET_DEVICE where id='" + id + "'";

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
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetTypeCout(string type)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select count(*)  from t_dev_device where  device_type=" + type + " ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        #endregion IDeviceManager 成员

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDevState(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from ( select distinct  a.device_id, a.device_ip, a.device_name, a.connect_state, DATE_FORMAT(a.work_time,'%Y-%m-%d %H:%i:%s') as work_time , F_GET_VALUE('STATION_TYPE_NAME', 't_cfg_set_station_TYPE','STATION_TYPE_ID', a.device_type) AS device_type_name,  decode(nvl(zt,'9'),'1','2','9','3','3') as zt,device_type,delaytime from t_dev_device_state a,  (select device_ip, count(*) as zt from t_dev_device_state_lock  where connect_state='1'  group by device_ip) b  where b.device_ip(+) = a.device_ip ) where  " + where + " order by device_type, device_name,device_ip asc ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        //public DataTable GetServer(string where)
        //{
        //    string mySql = string.Empty;
        //    try
        //    {
        //        mySql = "select * from  t_dev_server where   " + where + "   order by server_id asc";
        //        return dataAccess.Get_DataTable(mySql);
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(mySql + ex.Message);
        //        return null;
        //    }
        //}

        //public DataTable GetTGSSetting(string field, string where)
        //{
        //    string mySql = string.Empty;
        //    try
        //    {
        //        mySql = "select " + field + " from  vt_tgs_device_setting where   " + where;
        //        return dataAccess.Get_DataTable(mySql);
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(mySql + ex.Message);
        //        return null;
        //    }
        //}
        //public int DeleteDeviceSetting(string id)
        //{
        //    string mySql = string.Empty;
        //    try
        //    {
        //        mySql = " delete  from t_tgs_device_setting where id='" + id + "'";

        //        return dataAccess.Execute_NonQuery(mySql);

        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(mySql + ex.Message);
        //        return -1;
        //    }
        //}
        //public int UptateDeviceSetting(System.Collections.Hashtable hs)
        //{
        //    string mySql = string.Empty;
        //    try
        //    {
        //        mySql = "update t_tgs_device_setting set ";
        //        //mySql += " service_ip='" + hs["service_ip"].ToString() + "',";
        //        //mySql += " station_id='" + hs["station_id"].ToString() + "',";
        //        mySql += " direction_id='" + hs["direction_id"].ToString() + "',";
        //        mySql += " channelid='" + hs["channelid"].ToString() + "',";
        //        mySql += " imagepath='" + hs["imagepath"].ToString() + "',";
        //        mySql += " isscanfile='" + hs["isscanfile"].ToString() + "',";
        //        mySql += " localport='" + hs["localport"].ToString() + "',";
        //        mySql += " localip='" + hs["localip"].ToString() + "',";
        //        mySql += " isuse='" + hs["isuse"].ToString() + "',";
        //        mySql += " applyflag='" + hs["applyflag"].ToString() + "'";
        //        mySql += " where  id='" + hs["id"].ToString() + "'";
        //        return dataAccess.Execute_NonQuery(mySql);
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(mySql + ex.Message);
        //        return -1;
        //    }

        //}
        //public int InsertDeviceSetting(System.Collections.Hashtable hs)
        //{
        //    string mySql = string.Empty;
        //    try
        //    {
        //        mySql = "insert into  t_tgs_device_setting (id,service_id,station_id,direction_id,device_id,imagepath,channelid,isscanfile,localport,localip,isuse,applyflag) values (";
        //        mySql = mySql + "'" + hs["id"].ToString() + "',";
        //        mySql = mySql + "'" + hs["service_id"].ToString() + "',";
        //        mySql = mySql + "'" + hs["station_id"].ToString() + "',";
        //        mySql = mySql + "'" + hs["direction_id"].ToString() + "',";
        //        mySql = mySql + "'" + hs["device_id"].ToString() + "',";
        //        mySql = mySql + "'" + hs["imagepath"].ToString() + "',";
        //        mySql = mySql + "'" + hs["channelid"].ToString() + "',";
        //        mySql = mySql + "'" + hs["isscanfile"].ToString() + "',";
        //        mySql = mySql + "'" + hs["localport"].ToString() + "',";
        //        mySql = mySql + "'" + hs["localip"].ToString() + "',";
        //        mySql = mySql + "'" + hs["isuse"].ToString() + "',";
        //        mySql = mySql + "'" + hs["applyflag"].ToString() + "'";
        //        mySql = mySql + " )";
        //        return dataAccess.Execute_NonQuery(mySql);
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(mySql + ex.Message);
        //        return -1;
        //    }
        //}

        //public DataTable GetCctvSetting(string field, string where)
        //{
        //    string mySql = string.Empty;
        //    try
        //    {
        //        mySql = "select " + field + " from  vt_cctv_device_setting where   " + where;
        //        return dataAccess.Get_DataTable(mySql);
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(mySql + ex.Message);
        //        return null;
        //    }
        //}
        //public int DeleteCctvSetting(string id)
        //{
        //    string mySql = string.Empty;
        //    try
        //    {
        //        mySql = " delete  from t_cctv_device_setting where id='" + id + "'";

        //        return dataAccess.Execute_NonQuery(mySql);

        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(mySql + ex.Message);
        //        return -1;
        //    }
        //}
        //public int UptateCctvSetting(System.Collections.Hashtable hs)
        //{
        //    string mySql = string.Empty;
        //    try
        //    {
        //        mySql = "update t_cctv_device_setting set ";
        //        mySql += " direction_id='" + hs["direction_id"].ToString() + "',";
        //        mySql += " mchannelid='" + hs["mchannelid"].ToString() + "',";
        //        mySql += " rchannelid='" + hs["rchannelid"].ToString() + "',";
        //        mySql += " masterid='" + hs["masterid"].ToString() + "',";
        //        mySql += " isuse='" + hs["isuse"].ToString() + "'";
        //        mySql += " where  id='" + hs["id"].ToString() + "'";
        //        return dataAccess.Execute_NonQuery(mySql);
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(mySql + ex.Message);
        //        return -1;
        //    }
        //}
        //public int InsertCctvSetting(System.Collections.Hashtable hs)
        //{
        //    string mySql = string.Empty;
        //    try
        //    {
        //        if (GeXhExist("t_cctv_device_setting", "station_id||direction_id", hs["station_id"].ToString() + hs["direction_id"].ToString()) > 0)
        //        {
        //            mySql = "update t_cctv_device_setting set ";
        //            mySql += " direction_id='" + hs["direction_id"].ToString() + "',";
        //            mySql += " mdevice_id='" + hs["mdevice_id"].ToString() + "',";
        //            mySql += " mchannelid='" + hs["mchannelid"].ToString() + "',";
        //            mySql += " rdevice_id='" + hs["rdevice_id"].ToString() + "',";
        //            mySql += " mchannelid='" + hs["mchannelid"].ToString() + "',";
        //            mySql += " rchannelid='" + hs["rchannelid"].ToString() + "',";
        //            mySql += " isuse='" + hs["isuse"].ToString() + "'";
        //            mySql += " where  station_id='" + hs["station_id"].ToString() + "' and   direction_id='" + hs["direction_id"].ToString() + "'";
        //        }
        //        else
        //        {
        //            mySql = "insert into  t_cctv_device_setting (id,station_id,direction_id,mdevice_id,mchannelid,rdevice_id,rchannelid,masterid,isuse) values (";
        //            mySql = mySql + "'" + hs["id"].ToString() + "',";
        //            mySql = mySql + "'" + hs["station_id"].ToString() + "',";
        //            mySql = mySql + "'" + hs["direction_id"].ToString() + "',";
        //            mySql = mySql + "'" + hs["mdevice_id"].ToString() + "',";
        //            mySql = mySql + "'" + hs["mchannelid"].ToString() + "',";
        //            mySql = mySql + "'" + hs["rdevice_id"].ToString() + "',";
        //            mySql = mySql + "'" + hs["rchannelid"].ToString() + "',";
        //            mySql = mySql + "'" + hs["masterid"].ToString() + "',";
        //            mySql = mySql + "'" + hs["isuse"].ToString() + "'";
        //            mySql = mySql + " )";

        //        }
        //        return dataAccess.Execute_NonQuery(mySql);
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(mySql + ex.Message);
        //        return -1;
        //    }
        //}

        //public DataTable GetStationTypeByDevice()
        //{
        //    string mysql = string.Empty;
        //    try
        //    {
        //        mysql = " select  station_type_id,  station_type_id as station_type_name,count(*) from ( select distinct a.station_id,c.station_type_id from (";
        //        mysql = mysql + " select station_id,device_id  from t_tgs_device_setting ) a, ";
        //        mysql = mysql + " t_dev_device_infor b,t_cfg_set_station c  where a.device_id=b.device_id and a.station_id=c.station_id) dd  group by station_type_id ";
        //        return dataAccess.Get_DataTable(mysql);
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(mysql + ex.Message);
        //        return null;
        //    }

        //}
        //public DataTable GetStationDeviceState(string field, string where)
        //{
        //    string mysql = string.Empty;
        //    try
        //    {
        //        mysql = "select  " + field + "   from ( select a.station_id,c.station_name,c.station_type_id,";
        //        mysql = mysql + "  f_get_value ('station_type_name', 't_cfg_set_station_type', 'station_type_id',  c.station_type_id) as station_type_name,b.device_name,b.ipaddress,d.connect_state,";
        //        mysql = mysql + "  d.delaytime,nvl(round(d.pingcount/d.allcount,2)*100,0)||'%' as pingcount,d.update_time from(";
        //        mysql = mysql + "  select station_id,device_id    from t_tgs_device_setting   union";
        //        mysql = mysql + "  select station_id,mdevice_id  as device_id    from  t_cctv_device_setting  union";
        //        mysql = mysql + "  select station_id,device_id  as device_id     from  t_utc_cross_setting  union";
        //        mysql = mysql + "  select station_id,device_id  as device_id     from  t_vms_device_setting) a,  t_dev_device_infor b,t_cfg_set_station c,t_dev_device_state d";
        //        mysql = mysql + "   where a.device_id=b.device_id    and a.station_id=c.station_id  and d.device_id(+)=b.device_id)  where  " + where; ;
        //        return dataAccess.Get_DataTable(mysql);
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(mysql + ex.Message);
        //        return null;
        //    }

        //}
        //public DataTable GetNoDeviceState(string field, string where)
        //{
        //    string mysql = string.Empty;
        //    try
        //    {
        //        mysql = "select  " + field + "   from ( select device_type_id, device_type_name,device_mode_id,mode_name, a.device_name,ipaddress,";
        //        mysql = mysql + "  d.connect_state,d.delaytime,NVL (ROUND (d.pingcount / d.allcount, 2) * 100, 0) || '%'AS pingcount,";
        //        mysql = mysql + "  d.update_time from vt_dev_device_infor a,t_dev_device_state d   where  ";
        //        mysql = mysql + " d.device_id(+) = a.device_id AND  a.device_id not in ( ";
        //        mysql = mysql + "  select device_id    from t_tgs_device_setting   union";
        //        mysql = mysql + "  select mdevice_id  as device_id    from  t_cctv_device_setting  union";
        //        mysql = mysql + "  select device_id  as device_id     from  t_utc_cross_setting  union";
        //        mysql = mysql + "  select device_id  as device_id     from  t_vms_device_setting) )";
        //        mysql = mysql + "    where  " + where; ;
        //        return dataAccess.Get_DataTable(mysql);
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(mysql + ex.Message);
        //        return null;
        //    }

        //}
        //public DataTable GetServiceState(string field, string where)
        //{
        //    string mysql = string.Empty;
        //    try
        //    {
        //        mysql = "select  " + field + "   from ( select   server_type_id, f_get_value ('device_type_name','t_dev_device_type','device_type_id', server_type_id) as server_type_name,";
        //        mysql = mysql + "    server_mode_id, f_get_value ('mode_name','t_dev_device_mode','device_mode_id', server_mode_id)    as mode_name,";
        //        mysql = mysql + "   a.server_name,ipaddress,d.connect_state, d.delaytime,  nvl (round (d.pingcount / d.allcount, 2) * 100, 0) || '%' as pingcount,  d.update_time";
        //        mysql = mysql + "   from   t_dev_server a, t_dev_device_state d where   d.device_id(+) = a.server_id ) ";
        //        mysql = mysql + "    where  " + where; ;
        //        return dataAccess.Get_DataTable(mysql);
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(mysql + ex.Message);
        //        return null;
        //    }

        //}

        /// <summary>
        ///删除设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteDeviceInfo(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_dev_device_infor where device_id='" + id + "'";

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
        /// <param name="where"></param>
        /// <param name="where1"></param>
        /// <returns></returns>
        public DataTable GetDeviceInfoCount(string where, string where1)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select  distinct station_name,station_type_name,direction_name,device_name,device_type_name,mode_name,ipaddress,isuse, nvl(zs,'0') as zs,nvl(zs1,'0') as zs1,round(((nvl(zs,0)-nvl(zs1,1))/nvl(zs1,1)*100),0) as bl from  VT_TGS_DEVICE_COUNT a,( select  sum(ll) as zs, kkid||fxbh as kkfx from T_TGS_FLOW_HOUR where  " + where + "   group by kkid,fxbh  )b,( select  sum(ll) as zs1, kkid||fxbh as kkfx from T_TGS_FLOW_AVG where   " + where1 + "  group by kkid,fxbh  ) c where b.kkfx(+)=(a.station_id||direction_id) and c.kkfx(+)=(a.station_id||direction_id) order by zs,bl asc  ";
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
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTableSpace(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select  file_name, a.tablespace_name ,a.bytes/1024/1024　,　　(a.bytes-b.bytes)/1024/1024　,　　b.bytes/1024/1024　,　　round(((a.bytes-b.bytes)/a.bytes)*100,2)  from ";
                mySql = mySql + " (select file_name,file_id,tablespace_name,sum(bytes) bytes from dba_data_files group by file_name,file_id,tablespace_name)   a, ";
                mySql = mySql + "  (select file_id,tablespace_name,sum(bytes) bytes,max(bytes) largest from dba_free_space group by file_id,tablespace_name)   b";
                mySql = mySql + "  where   a.tablespace_name=b.tablespace_name and a.file_id=b.file_id order   by   ((a.bytes-b.bytes)/a.bytes)   desc ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        #region IDeviceManager 成员

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DataTable GetBuildAll()
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select businiss_id,business_name,address,linkman,telephone,mobile,fax,email,type,isuse from  t_cfg_bussiness where type='1'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 根据设备类型获得对应设备厂家信息
        /// </summary>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public DataTable GetDevModeByDeviceType(string devType)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select device_mode_id,device_type_id,mode_id,mode_name,mode_byname,isuse from t_dev_device_mode  where  isuse='1' and device_type_id='" + devType + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 根据设备类型获得对应设备信息
        /// </summary>
        /// <param name="devType"></param>
        /// <returns></returns>
        public DataTable GetDeviceByDeviceType(string devType)
        {
            string mySql = string.Empty;
            try
            {
                return GetDeviceByMore("  device_type_id='" + devType + "'");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 根据查询条件查询设备详细信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDeviceByMore(string where)
        {
            string strsql = string.Empty;
            try
            {
                //strsql = "select a.device_id,a.device_idext,a.device_name,a.device_mode_id,a.device_type_id,a.isuse,a.build_id,a.maintain_company_id,a.make_company_id,a.ipaddress,a.port,a.channels,a.username,a.password,a.comm_type_id,a.protocol_id,a.dev_length,a.dev_width,a.port_num,a.port_param ";
                //strsql = strsql + " ,'1' as workstate,'' as direction_id,'' as direction_name,";
                //strsql = strsql + "f_get_value('business_name','t_cfg_bussiness','businiss_id',build_id) as build_name ,";
                //strsql = strsql + "f_get_value('business_name','t_cfg_bussiness','businiss_id',maintain_company_id) as maintain_company_name,";
                //strsql = strsql + "f_get_value('business_name','t_cfg_bussiness','businiss_id',make_company_id) as make_company_name,";
                //strsql = strsql + "f_get_value('mode_name','t_dev_device_mode','device_mode_id',device_mode_id) as mode_name,";
                //strsql = strsql + "f_get_value('device_type_name','t_dev_device_type','device_type_id',device_type_id) as device_type_name ";
                //strsql = strsql + "from t_dev_device_infor a  where" + where;
                strsql = @"SELECT b.device_id,b.device_idext,b.device_name,b.device_mode_id,b.device_type_id,b.isuse,b.build_id,b.maintain_company_id,b.make_company_id,b.ipaddress,b.port,b.channels,b.username,
                            b.password,b.comm_type_id,b.protocol_id,b.dev_length,b.dev_width,b.port_num,b.port_param ,'1' AS workstate,'' AS direction_id,'' AS direction_name
                            , b.build_name, b.maintain_company_name ,b.make_company_name ,c.mode_name AS mode_name ,d.device_type_name AS device_type_name FROM (SELECT a.device_id,a.device_idext,a.device_name,
                            a.device_mode_id,a.device_type_id,a.isuse,a.build_id,a.maintain_company_id,a.make_company_id,a.ipaddress,a.port,a.channels,a.username,
                            a.password,a.comm_type_id,a.protocol_id,a.dev_length,a.dev_width,a.port_num,a.port_param ,'1' AS workstate,'' AS direction_id,'' AS direction_name
                            ,f_get_business_name(build_id) AS build_name,f_get_business_name(maintain_company_id) AS maintain_company_name, f_get_business_name(make_company_id ) AS make_company_name
                            FROM t_dev_device_infor a WHERE " + where + @") b ,t_dev_device_mode c ,t_dev_device_type d WHERE b.device_mode_id=c.device_mode_id AND b.device_type_id =d.device_type_id";
                return dataAccess.Get_DataTable(strsql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strsql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得所有设备类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetDeviceTypeAll()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select device_type_id,device_type_name, isuse,isserver  from t_dev_device_type where isuse='1' and isserver='0' order by device_type_id ";
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
        public DataTable GetMaintainAll()
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select businiss_id,business_name,address,linkman,telephone,mobile,fax,email,type,isuse from  t_cfg_bussiness where type='2' ";
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
        public DataTable GetMakeAll()
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select businiss_id,business_name,address,linkman,telephone,mobile,fax,email,type,isuse from  t_cfg_bussiness where type='3' ";
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
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetServerByMore(string strWhere)
        {
            string strsql = string.Empty;
            try
            {
                strsql = "select server_id,server_idext,server_name,server_mode_id,server_type_id,isuse,build_id,maintain_company_id,make_company_id,ipaddress,port,channels,username,password,comm_type_id,protocol_id, jkxlh,";
                strsql = strsql + " f_get_value('business_name','t_cfg_bussiness','businiss_id',build_id) as build_name ,";
                strsql = strsql + " f_get_value('business_name','t_cfg_bussiness','businiss_id',maintain_company_id) as maintain_company_name,";
                strsql = strsql + " f_get_value('business_name','t_cfg_bussiness','businiss_id',make_company_id) as make_company_name,";
                strsql = strsql + " f_get_value('mode_name','t_dev_device_mode','device_mode_id',server_mode_id) as mode_name,";
                strsql = strsql + " f_get_value('device_type_name','t_dev_device_type','device_type_id',server_type_id) as device_type_name ";
                strsql = strsql + "  from t_dev_server ";
                if (strWhere.Trim() != "")
                {
                    strsql += " WHERE " + strWhere;
                }
                return dataAccess.Get_DataTable(strsql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strsql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DataTable GetServerTypeAll()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select device_type_id,device_type_name, isuse,isserver  from t_dev_device_type where isuse='1' and isserver='1' order by device_type_id ";
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
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public DataTable GetServerByTypeID(string deviceType)
        {
            string strsql = string.Empty;
            try
            {
                return GetServerByMore(" server_type_id='" + deviceType + "'");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strsql + ex.Message);
                return null;
            }
        }

        #endregion IDeviceManager 成员

        #region IDeviceManager 成员

        /// <summary>
        /// 查找所有设备类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetDeviceType()
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select *  from  t_dev_device_type  where devtype_type='1'  and isserver='0'  ORDER BY device_type_id ASC";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 根据条件关联查询设备厂家
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDeviceTypeMode(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select *  from  t_dev_device_mode  where  " + where + "  ORDER BY device_mode_id ASC";
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
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetBussiness(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select *  from  t_cfg_bussiness  where  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询设备信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDeviceInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select *  from  t_dev_device_infor  where  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteDevice(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_dev_device_infor where device_id='" + id + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public DataTable GetDevState(string type)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetDeviceState_lock_id(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from (select device_name,device_ip,F_GET_VALUE ('STATION_TYPE_NAME', 't_cfg_set_station_TYPE', 'STATION_TYPE_ID',device_type) AS device_type_name,CONNECT_STATE,DATE_FORMAT(UPDATE_TIME,'yyyy-mm-dd hh24:mi:ss') as UPDATE_TIME  from t_dev_device_state_lock  where device_ip='" + id + "'  order by update_time desc  ) where rownum<=50 ";
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
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTGSSetting(string field, string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select " + field + " from  vt_tgs_device_setting where   " + where;
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
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteDeviceSetting(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_tgs_device_setting where id='" + id + "'";

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
        public int UptateDeviceSetting(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update t_tgs_device_setting set ";
                //mySql += " service_ip='" + hs["service_ip"].ToString() + "',";
                //mySql += " station_id='" + hs["station_id"].ToString() + "',";
                mySql += " direction_id='" + hs["direction_id"].ToString() + "',";
                mySql += " channelid='" + hs["channelid"].ToString() + "',";
                mySql += " imagepath='" + hs["imagepath"].ToString() + "',";
                mySql += " isscanfile='" + hs["isscanfile"].ToString() + "',";
                mySql += " localport='" + hs["localport"].ToString() + "',";
                mySql += " localip='" + hs["localip"].ToString() + "',";
                mySql += " isuse='" + hs["isuse"].ToString() + "',";
                mySql += " applyflag='" + hs["applyflag"].ToString() + "'";
                mySql += " where  id='" + hs["id"].ToString() + "'";
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
        public int InsertDeviceSetting(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_tgs_device_setting (id,service_id,station_id,direction_id,device_id,imagepath,channelid,isscanfile,localport,localip,isuse,applyflag) values (";
                mySql = mySql + "'" + hs["id"].ToString() + "',";
                mySql = mySql + "'" + hs["service_id"].ToString() + "',";
                mySql = mySql + "'" + hs["station_id"].ToString() + "',";
                mySql = mySql + "'" + hs["direction_id"].ToString() + "',";
                mySql = mySql + "'" + hs["device_id"].ToString() + "',";
                mySql = mySql + "'" + hs["imagepath"].ToString() + "',";
                mySql = mySql + "'" + hs["channelid"].ToString() + "',";
                mySql = mySql + "'" + hs["isscanfile"].ToString() + "',";
                mySql = mySql + "'" + hs["localport"].ToString() + "',";
                mySql = mySql + "'" + hs["localip"].ToString() + "',";
                mySql = mySql + "'" + hs["isuse"].ToString() + "',";
                mySql = mySql + "'" + hs["applyflag"].ToString() + "'";
                mySql = mySql + " )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///获取连接信息
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetCctvSetting(string field, string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select " + field + " from vt_cctv_device_setting where " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        ///删除连接信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteCctvSetting(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_cctv_device_setting where id='" + id + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///修改连接信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UptateCctvSetting(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update t_cctv_device_setting set ";
                mySql += " direction_id='" + hs["direction_id"].ToString() + "',";
                mySql += " mchannelid='" + hs["mchannelid"].ToString() + "',";
                mySql += " rchannelid='" + hs["rchannelid"].ToString() + "',";
                mySql += " masterid='" + hs["masterid"].ToString() + "',";
                mySql += " isuse='" + hs["isuse"].ToString() + "'";
                mySql += " where  id='" + hs["id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///添加连接信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertCctvSetting(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_cctv_device_setting", "CONCAT(station_id, mdevice_id) ", hs["station_id"].ToString() + hs["mdevice_id"].ToString()) > 0)
                {
                    mySql = "update t_cctv_device_setting set ";
                    mySql += " direction_id='" + hs["direction_id"].ToString() + "',";
                    mySql += " mdevice_id='" + hs["mdevice_id"].ToString() + "',";
                    mySql += " mchannelid='" + hs["mchannelid"].ToString() + "',";
                    mySql += " rdevice_id='" + hs["rdevice_id"].ToString() + "',";
                    mySql += " mchannelid='" + hs["mchannelid"].ToString() + "',";
                    mySql += " rchannelid='" + hs["rchannelid"].ToString() + "',";
                    mySql += " isuse='" + hs["isuse"].ToString() + "'";
                    mySql += " where  station_id='" + hs["station_id"].ToString() + "' and   direction_id='" + hs["direction_id"].ToString() + "'";
                }
                else
                {
                    mySql = "insert into  t_cctv_device_setting (id,station_id,direction_id,mdevice_id,mchannelid,rdevice_id,rchannelid,masterid,isuse) values (";
                    mySql = mySql + "'" + hs["id"].ToString() + "',";
                    mySql = mySql + "'" + hs["station_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["direction_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["mdevice_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["mchannelid"].ToString() + "',";
                    mySql = mySql + "'" + hs["rdevice_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["rchannelid"].ToString() + "',";
                    mySql = mySql + "'" + hs["masterid"].ToString() + "',";
                    mySql = mySql + "'" + hs["isuse"].ToString() + "'";
                    mySql = mySql + " )";
                }
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
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetUTCSetting(string field, string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select " + field + " from  vt_utc_cross_setting where   " + where;
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
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteUtcSetting(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_tgs_device_setting where id='" + id + "'";

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
        public int UptateUtcSetting(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update t_utc_cross_setting set ";
                //mySql += " service_ip='" + hs["service_ip"].ToString() + "',";
                //mySql += " station_id='" + hs["station_id"].ToString() + "',";
                mySql += " direction_id='" + hs["direction_id"].ToString() + "',";
                mySql += " machineid='" + hs["machineid"].ToString() + "',";
                mySql += " localport='" + hs["localport"].ToString() + "',";
                mySql += " localip='" + hs["localip"].ToString() + "',";
                mySql += " isuse='" + hs["isuse"].ToString() + "'";
                mySql += " where  id='" + hs["id"].ToString() + "'";
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
        public int InsertUtcSetting(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_utc_cross_setting", "station_id", hs["station_id"].ToString()) > 0)
                {
                    mySql = "update t_utc_cross_setting set ";
                    mySql += " service_id='" + hs["service_id"].ToString() + "',";
                    mySql += " device_id='" + hs["device_id"].ToString() + "',";
                    mySql += " station_id='" + hs["station_id"].ToString() + "',";
                    mySql += " direction_id='" + hs["direction_id"].ToString() + "',";
                    mySql += " machineid='" + hs["machineid"].ToString() + "',";
                    mySql += " localport='" + hs["localport"].ToString() + "',";
                    mySql += " localip='" + hs["localip"].ToString() + "',";
                    mySql += " isuse='" + hs["isuse"].ToString() + "'";
                    mySql += " where  id='" + hs["id"].ToString() + "'";
                }
                else
                {
                    mySql = "insert into  t_utc_cross_setting (id,station_id,service_id,direction_id,device_id,machineid,localport,localip,isuse) values (";
                    mySql = mySql + "'" + hs["id"].ToString() + "',";
                    mySql = mySql + "'" + hs["station_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["service_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["direction_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["device_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["machineid"].ToString() + "',";
                    mySql = mySql + "'" + hs["localport"].ToString() + "',";
                    mySql = mySql + "'" + hs["localip"].ToString() + "',";
                    mySql = mySql + "'" + hs["isuse"].ToString() + "'";
                    mySql = mySql + " )";
                }
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
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetVMSSetting(string field, string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select " + field + " from  vt_vms_device_setting where   " + where;
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
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteVmsSetting(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_vms_device_setting where id='" + id + "'";

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
        public int UptateVmsSetting(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update t_vms_device_setting set ";
                //mySql += " service_ip='" + hs["service_ip"].ToString() + "',";
                //mySql += " station_id='" + hs["station_id"].ToString() + "',";
                mySql += " direction_id='" + hs["direction_id"].ToString() + "',";
                mySql += " localport='" + hs["localport"].ToString() + "',";
                mySql += " localip='" + hs["localip"].ToString() + "',";
                mySql += " led_type='" + hs["led_type"].ToString() + "',";
                mySql += " color_type='" + hs["color_type"].ToString() + "',";
                mySql += " cardid='" + hs["cardid"].ToString() + "',";
                mySql += " timeout='" + hs["timeout"].ToString() + "',";
                mySql += " isuse='" + hs["isuse"].ToString() + "'";
                mySql += " where  id='" + hs["id"].ToString() + "'";
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
        public int InsertVmsSetting(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_vms_device_setting", "station_id", hs["station_id"].ToString()) > 0)
                {
                    mySql = "update t_vms_device_setting set ";
                    mySql += " service_id='" + hs["service_id"].ToString() + "',";
                    mySql += " device_id='" + hs["device_id"].ToString() + "',";
                    mySql += " station_id='" + hs["station_id"].ToString() + "',";
                    mySql += " direction_id='" + hs["direction_id"].ToString() + "',";
                    mySql += " led_type='" + hs["led_type"].ToString() + "',";
                    mySql += " color_type='" + hs["color_type"].ToString() + "',";
                    mySql += " cardid='" + hs["cardid"].ToString() + "',";
                    mySql += " timeout='" + hs["timeout"].ToString() + "',";
                    mySql += " localport='" + hs["localport"].ToString() + "',";
                    mySql += " localip='" + hs["localip"].ToString() + "',";
                    mySql += " isuse='" + hs["isuse"].ToString() + "'";
                    mySql += " where  id='" + hs["id"].ToString() + "'";
                }
                else
                {
                    mySql = "insert into  t_vms_device_setting (id,station_id,service_id,direction_id,device_id,led_type,color_type,cardid,timeout,localport,localip,isuse) values (";
                    mySql = mySql + "'" + hs["id"].ToString() + "',";
                    mySql = mySql + "'" + hs["station_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["service_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["direction_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["device_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["led_type"].ToString() + "',";
                    mySql = mySql + "'" + hs["color_type"].ToString() + "',";
                    mySql = mySql + "'" + hs["cardid"].ToString() + "',";
                    mySql = mySql + "'" + hs["timeout"].ToString() + "',";
                    mySql = mySql + "'" + hs["localport"].ToString() + "',";
                    mySql = mySql + "'" + hs["localip"].ToString() + "',";
                    mySql = mySql + "'" + hs["isuse"].ToString() + "'";
                    mySql = mySql + " )";
                }
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 获得检测点类型总数信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetStationTypeByDevice()
        {
            string mysql = string.Empty;
            try
            {
                mysql = @"SELECT e.station_type_id,f.STATION_TYPE_NAME, e.zs FROM  (SELECT station_type_id, COUNT(*) AS zs
                                FROM (SELECT DISTINCT a.station_id, c.station_type_id FROM (SELECT station_id, device_id from t_tgs_device_setting t1
                                UNION SELECT station_id, mdevice_id AS device_id FROM t_cctv_device_setting t2 UNION SELECT station_id, device_id AS device_id
                                FROM t_utc_cross_setting t3 UNION SELECT station_id, device_id AS device_id FROM t_vms_device_setting t4 ) a, t_dev_device_infor b,
                                t_cfg_set_station c WHERE a.device_id = b.device_id AND a.station_id = c.station_id) d GROUP BY station_type_id ) e ,
                                t_cfg_set_station_type f WHERE e.station_type_id=f.station_type_id ";
                return dataAccess.Get_DataTable(mysql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mysql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询检测点设备状态
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetStationDeviceState(string field, string where)
        {
            string mysql = string.Empty;
            try
            {
                //mysql = "select  " + field + "   from ( select a.station_id,c.station_name,c.station_type_id,";
                //mysql = mysql + "  f_get_value ('station_type_name', 't_cfg_set_station_type', 'station_type_id',  c.station_type_id) as station_type_name,b.device_name,b.ipaddress,d.connect_state,";
                //mysql = mysql + "  d.delaytime,nvl(round(d.pingcount/d.allcount,2)*100,0)||'%' as pingcount,d.update_time from(";
                //mysql = mysql + "  select station_id,device_id    from t_tgs_device_setting   union";
                //mysql = mysql + "  select station_id,mdevice_id  as device_id    from  t_cctv_device_setting  union";
                //mysql = mysql + "  select station_id,device_id  as device_id     from  t_utc_cross_setting  union";
                //mysql = mysql + "  select station_id,device_id  as device_id     from  t_vms_device_setting) a,  t_dev_device_infor b,t_cfg_set_station c,t_dev_device_state d";
                //mysql = mysql + "   where a.device_id=b.device_id    and a.station_id=c.station_id  and d.device_id(+)=b.device_id)  where  " + where;

                mysql = "SELECT " + field + @" FROM (SELECT e.station_id, e.station_name, e.station_type_id,f_get_station_type_ms(e.station_type_id) AS station_type_name,
                                e.device_name, e.ipaddress,  f.connect_state,f.delaytime, CAST(concat(ifnull(ROUND(f.pingcount / f.allcount, 2) * 100,0),'%') AS CHAR(100)) AS pingcount, f.update_time
                                FROM t_dev_device_state f  right JOIN (SELECT  * FROM(SELECT  a.station_id,c.station_name,c.station_type_id,b.device_name,
                                b.ipaddress, a.device_id,d.connect_state FROM(SELECT station_id,device_id  FROM t_tgs_device_setting t1 UNION SELECT  station_id, mdevice_id AS device_id
                                FROM t_cctv_device_setting t2  UNION SELECT  station_id, device_id AS device_id   FROM t_utc_cross_setting t3  UNION SELECT  station_id,
                                device_id AS device_id  FROM t_vms_device_setting t4) a, t_dev_device_infor b, t_cfg_set_station c, t_dev_device_state d   WHERE a.device_id = b.device_id
                                AND a.station_id = c.station_id AND a.device_id = d.device_id ) h  WHERE" + where + ") e ON f.device_id  = e.device_id  ) g";
                return dataAccess.Get_DataTable(mysql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mysql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetNoDeviceState(string field, string where)
        {
            string mysql = string.Empty;
            try
            {
                mysql = "select  " + field + "   from ( select device_type_id, device_type_name,device_mode_id,mode_name, a.device_name,ipaddress,";
                mysql = mysql + "  d.connect_state,d.delaytime, CAST(concat(ifnull(ROUND(d.pingcount / d.allcount, 2) * 100,0),'%') AS CHAR(100)) AS pingcount,";
                mysql = mysql + "  d.update_time from vt_dev_device_infor a,t_dev_device_state d   where  ";
                mysql = mysql + " d.device_id= a.device_id AND  a.device_id not in ( ";
                mysql = mysql + "  select device_id    from t_tgs_device_setting   union";
                mysql = mysql + "  select mdevice_id  as device_id    from  t_cctv_device_setting  union";
                mysql = mysql + "  select device_id  as device_id     from  t_utc_cross_setting  union";
                mysql = mysql + "  select device_id  as device_id     from  t_vms_device_setting) ) e ";
                mysql = mysql + "    where  " + where; ;
                return dataAccess.Get_DataTable(mysql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mysql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetServiceState(string field, string where)
        {
            string mysql = string.Empty;
            try
            {
                mysql = "select  " + field + "   from ( select   server_type_id, f_get_DEVICE_TYPE_NAME (server_type_id) as server_type_name,";
                mysql = mysql + "    server_mode_id, f_get_mode_name ( server_mode_id)    as mode_name,";
                mysql = mysql + "   a.server_name,ipaddress,d.connect_state, d.delaytime, CAST(CONCAT( IFNULL( ROUND(d.pingcount / d.allcount, 2) * 100,0), '%' ) AS CHAR(100)) AS pingcount,  d.update_time";
                mysql = mysql + "   from   t_dev_server a, t_dev_device_state d where   d.device_id = a.server_id ) c ";
                mysql = mysql + "    where  " + where; ;
                return dataAccess.Get_DataTable(mysql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mysql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///更新服务器信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UptateServerInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_dev_server", "server_id", hs["server_id"].ToString()) > 0)
                {
                    mySql = "update t_dev_server set ";
                    mySql += " server_idext='" + hs["server_idext"].ToString() + "',";
                    mySql += " server_name='" + hs["server_name"].ToString() + "',";
                    mySql += " server_mode_id='" + hs["server_mode_id"].ToString() + "',";
                    mySql += " server_type_id='" + hs["server_type_id"].ToString() + "',";
                    mySql += " ipaddress='" + hs["ipaddress"].ToString() + "',";
                    mySql += " port='" + hs["port"].ToString() + "',";
                    mySql += " channels='" + hs["channels"].ToString() + "',";
                    mySql += " username='" + hs["username"].ToString() + "',";
                    mySql += " password='" + hs["password"].ToString() + "',";
                    mySql += " createdate=now(),";
                    mySql += " build_id='" + hs["build_id"].ToString() + "',";
                    mySql += " comm_type_id='" + hs["comm_type_id"].ToString() + "',";
                    mySql += " protocol_id='" + hs["protocol_id"].ToString() + "',";
                    mySql += " maintain_company_id='" + hs["maintain_company_id"].ToString() + "',";
                    mySql += " make_company_id='" + hs["make_company_id"].ToString() + "',";
                    mySql += " jkxlh='" + hs["jkxlh"].ToString() + "',";
                    mySql += " isuse='" + hs["isuse"].ToString() + "'";
                    mySql += " where  server_id='" + hs["server_id"].ToString() + "'";
                }
                else
                {
                    mySql = "insert into  t_dev_server (server_id,server_idext,server_name,server_mode_id,server_type_id, isuse, build_id ,";
                    mySql = mySql + "  maintain_company_id,make_company_id,ipaddress,port,channels,username,password,createdate,protocol_id,comm_type_id,";
                    mySql = mySql + "  jkxlh) values (";
                    mySql = mySql + "'" + hs["server_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["server_idext"].ToString() + "',";
                    mySql = mySql + "'" + hs["server_name"].ToString() + "',";
                    mySql = mySql + "'" + hs["server_mode_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["server_type_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["isuse"].ToString() + "',";
                    mySql = mySql + "'" + hs["build_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["maintain_company_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["make_company_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["ipaddress"].ToString() + "',";
                    mySql = mySql + "'" + hs["port"].ToString() + "',";
                    mySql = mySql + "'" + hs["channels"].ToString() + "',";
                    mySql = mySql + "'" + hs["username"].ToString() + "',";
                    mySql = mySql + "'" + hs["password"].ToString() + "',";
                    mySql = mySql + "now(),";
                    mySql = mySql + "'" + hs["protocol_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["comm_type_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["jkxlh"].ToString() + "'";
                    mySql = mySql + " )";
                }
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UptateDeviceInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_dev_device_infor", "device_id", hs["device_id"].ToString()) > 0)
                {
                    mySql = "update t_dev_device_infor set ";
                    mySql += " device_idext='" + hs["device_idext"].ToString() + "',";
                    mySql += " device_name='" + hs["device_name"].ToString() + "',";
                    mySql += " device_mode_id='" + hs["device_mode_id"].ToString() + "',";
                    mySql += " device_type_id='" + hs["device_type_id"].ToString() + "',";
                    mySql += " ipaddress='" + hs["ipaddress"].ToString() + "',";
                    mySql += " port='" + hs["port"].ToString() + "',";
                    mySql += " channels='" + hs["channels"].ToString() + "',";
                    mySql += " username='" + hs["username"].ToString() + "',";
                    mySql += " password='" + hs["password"].ToString() + "',";
                    mySql += " createdate=now(),";
                    mySql += " protocol_id='" + hs["protocol_id"].ToString() + "',";
                    mySql += " dev_length='" + hs["dev_length"].ToString() + "',";
                    mySql += " dev_width='" + hs["dev_width"].ToString() + "',";
                    mySql += " port_num='" + hs["port_num"].ToString() + "',";
                    mySql += " port_param='" + hs["port_param"].ToString() + "',";
                    mySql += " build_id='" + hs["build_id"].ToString() + "',";
                    mySql += " comm_type_id='" + hs["comm_type_id"].ToString() + "',";
                    mySql += " maintain_company_id='" + hs["maintain_company_id"].ToString() + "',";
                    mySql += " make_company_id='" + hs["make_company_id"].ToString() + "',";
                    mySql += " isuse='" + hs["isuse"].ToString() + "'";
                    mySql += " where  device_id='" + hs["device_id"].ToString() + "'";
                }
                else
                {
                    mySql = "insert into  t_dev_device_infor (device_id,device_idext,device_name,device_mode_id,device_type_id, isuse, build_id ,";
                    mySql = mySql + "  maintain_company_id,make_company_id,ipaddress,port,channels,username,password,createdate,protocol_id,dev_length,";
                    mySql = mySql + "  dev_width ,port_num,port_param,comm_type_id) values (";
                    mySql = mySql + "'" + hs["device_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["device_idext"].ToString() + "',";
                    mySql = mySql + "'" + hs["device_name"].ToString() + "',";
                    mySql = mySql + "'" + hs["device_mode_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["device_type_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["isuse"].ToString() + "',";
                    mySql = mySql + "'" + hs["build_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["maintain_company_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["make_company_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["ipaddress"].ToString() + "',";
                    mySql = mySql + "'" + hs["port"].ToString() + "',";
                    mySql = mySql + "'" + hs["channels"].ToString() + "',";
                    mySql = mySql + "'" + hs["username"].ToString() + "',";
                    mySql = mySql + "'" + hs["password"].ToString() + "',";
                    mySql = mySql + "now(),";
                    mySql = mySql + "'" + hs["protocol_id"].ToString() + "',";
                    mySql = mySql + "'" + hs["dev_length"].ToString() + "',";
                    mySql = mySql + "'" + hs["dev_width"].ToString() + "',";
                    mySql = mySql + "'" + hs["port_num"].ToString() + "',";
                    mySql = mySql + "'" + hs["port_param"].ToString() + "',";
                    mySql = mySql + "'" + hs["comm_type_id"].ToString() + "'";
                    mySql = mySql + " )";
                }
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 查询设备信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDevice(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT a.device_id,a.device_name,a.device_type_id,b.device_type_name,a.device_mode_id,c.mode_name,a.ipaddress,a.port,a.device_idext FROM t_dev_device_infor a,t_dev_device_type b ,t_dev_device_mode c WHERE a.device_type_id=b.device_type_id  AND a.device_mode_id=c.device_mode_id  and " + where + "    ORDER BY  a.device_id ASC";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询历史设备信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetHistoryDevice(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = @"SELECT CASE WHEN tddsh.connect_state='1' AND tddsh.work_state='410102' THEN '1' ELSE '0' END AS 'Equipment_state', tddi.device_name,tddsh.device_ip, F_TO_NAME('240006',tddsh.connect_state) AS connect_state, F_TO_NAME('410100',tddsh.work_state) AS work_state,tddsh.work_time ,tddsh.update_time,f_get_DEVICE_TYPE_NAME(tddi.device_type_id) AS device_type,f_get_mode_name(tddi.device_mode_id) AS mode_name
FROM  t_dev_device_state_history tddsh ,t_dev_device_infor tddi WHERE tddsh.device_id=tddi.device_id  " + where + " ORDER BY tddsh.update_time DESC";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 删除服务器信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteServerInfo(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_dev_server where server_id='" + id + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///  查询服务器类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetServerType()
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select *  from  t_dev_device_type  where devtype_type='1'  and isserver='1' ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 根据条件关联查询服务器厂家
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetServerTypeMode(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select *  from  t_dev_device_mode  where  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询服务器信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetServerInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from  t_dev_server where   " + where + "   order by server_id asc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///查询服务器信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetServer(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT a.server_id,a.server_name,a.server_type_id,b.device_type_name,a.server_mode_id,c.mode_name,a.ipaddress,a.port,a.server_idext FROM t_dev_server a,t_dev_device_type b ,t_dev_device_mode c WHERE a.server_type_id=b.device_type_id  AND a.server_mode_id=c.device_mode_id  and " + where + "  ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 判断输入值是否在指定表存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        private int GeXhExist(string tableName, string fieldName, string fieldValue)
        {
            string mySql = string.Empty;
            try
            {
                //mySql = "select count(1) from   " + tableName + "   where   " + fieldName + "  ='" + fieldValue + "'";
                mySql = "select count(1) from   " + tableName + "   where   " + fieldName + "  ='" + fieldValue + "'";
                return int.Parse(dataAccess.Get_DataString(mySql, 0));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 插入设备信息
        /// </summary>
        /// <param name="devcieInfo"></param>
        /// <returns></returns>
        public int InsertDeviceInfo(MyNet.DataAccess.Model.DevcieInfo devcieInfo)
        {
            if (Add(devcieInfo))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(MyNet.DataAccess.Model.DevcieInfo model)
        {
            try
            {
                //StringBuilder strSql = new StringBuilder();
                //strSql.Append("insert into T_DEV_DEVICE_INFOR(");
                //strSql.Append("DEVICE_ID,DEVICE_IDEXT,DEVICE_NAME,DEVICE_MODE_ID,DEVICE_TYPE_ID,ISUSE,BUILD_ID,MAINTAIN_COMPANY_ID,MAKE_COMPANY_ID,IPADDRESS,PORT,CHANNELS,USERNAME,PASSWORD,COMM_TYPE_ID,PROTOCOL_ID,DEV_LENGTH,DEV_WIDTH,PORT_NUM,PORT_PARAM)");
                //strSql.Append(" values (");
                //strSql.Append(":DEVICE_ID,:DEVICE_IDEXT,:DEVICE_NAME,:DEVICE_MODE_ID,:DEVICE_TYPE_ID,:ISUSE,:BUILD_ID,:MAINTAIN_COMPANY_ID,:MAKE_COMPANY_ID,:IPADDRESS,:PORT,:CHANNELS,:USERNAME,:PASSWORD,:COMM_TYPE_ID,:PROTOCOL_ID,:DEV_LENGTH,:DEV_WIDTH,:PORT_NUM,:PORT_PARAM)");
                //OracleParameter[] parameters = {
                //    new OracleParameter(":DEVICE_ID", OracleType.VarChar,12),
                //    new OracleParameter(":DEVICE_IDEXT", OracleType.VarChar,30),
                //    new OracleParameter(":DEVICE_NAME", OracleType.VarChar,100),
                //    new OracleParameter(":DEVICE_MODE_ID", OracleType.VarChar,12),
                //    new OracleParameter(":DEVICE_TYPE_ID", OracleType.VarChar,12),
                //    new OracleParameter(":ISUSE", OracleType.VarChar,1),
                //    new OracleParameter(":BUILD_ID", OracleType.VarChar,12),
                //    new OracleParameter(":MAINTAIN_COMPANY_ID", OracleType.VarChar,12),
                //    new OracleParameter(":MAKE_COMPANY_ID", OracleType.VarChar,12),
                //    new OracleParameter(":IPADDRESS", OracleType.VarChar,16),
                //    new OracleParameter(":PORT", OracleType.VarChar,10),
                //    new OracleParameter(":CHANNELS", OracleType.Number,4),
                //    new OracleParameter(":USERNAME", OracleType.VarChar,20),
                //    new OracleParameter(":PASSWORD", OracleType.VarChar,20),
                //    new OracleParameter(":COMM_TYPE_ID", OracleType.VarChar,12),
                //    new OracleParameter(":PROTOCOL_ID", OracleType.VarChar,12),
                //    new OracleParameter(":DEV_LENGTH", OracleType.VarChar,12),
                //    new OracleParameter(":DEV_WIDTH", OracleType.VarChar,12),
                //    new OracleParameter(":PORT_NUM", OracleType.VarChar,12),
                //    new OracleParameter(":PORT_PARAM", OracleType.VarChar,12)};
                //parameters[0].Value = model.DEVICE_ID;
                //parameters[1].Value = model.DEVICE_IDEXT;
                //parameters[2].Value = model.DEVICE_NAME;
                //parameters[3].Value = model.DEVICE_MODE_ID;
                //parameters[4].Value = model.DEVICE_TYPE_ID;
                //parameters[5].Value = model.ISUSE;
                //parameters[6].Value = model.BUILD_ID;
                //parameters[7].Value = model.MAINTAIN_COMPANY_ID;
                //parameters[8].Value = model.MAKE_COMPANY_ID;
                //parameters[9].Value = model.IPADDRESS;
                //parameters[10].Value = model.PORT;
                //parameters[11].Value = model.CHANNELS;
                //parameters[12].Value = model.USERNAME;
                //parameters[13].Value = model.PASSWORD;
                //parameters[14].Value = model.COMM_TYPE_ID;
                //parameters[15].Value = model.PROTOCOL_ID;
                //parameters[16].Value = model.DEV_LENGTH;
                //parameters[17].Value = model.DEV_WIDTH;
                //parameters[18].Value = model.PORT_NUM;
                //parameters[19].Value = model.PORT_PARAM;

                //int rows = dataAccess.Execute_Procedure(strSql.ToString(), parameters);
                //if (rows > 0)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                return false;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listdev"></param>
        /// <returns></returns>
        public int InsertListDevice(List<MyNet.DataAccess.Model.DevcieInfo> listdev)
        {
            foreach (MyNet.DataAccess.Model.DevcieInfo model in listdev)
            {
                if (!Add(model))
                    return 0;
            }
            return listdev.Count;
        }

        #endregion IDeviceManager 成员
    }
}