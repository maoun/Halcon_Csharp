/***********************************************************************
 * Module:   目录业务逻辑类
 * Author:   李建平
 * Modified: 2008年10月17日
 * Purpose:  该类为页面提供需要的业务逻辑方法
 ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    [Serializable]
    public class DeviceManager
    {
        /// <summary>
        ///  设备管理接口
        /// </summary>
        private static readonly IDeviceManager dal = DALFactory.CreateDeviceManager();

        /// <summary>
        /// 查询设备信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDevice(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetDevice(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 查询设备信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetHistoryDevice(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetHistoryDevice(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        //public DataTable GetConfigDepartment(string systemId)
        //{
        //    try
        //    {
        //        return Bll.Common.ChangColName(dal.GetConfigDepartment(systemId));

        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(ex);
        //        return null;
        //    }
        //}

        //public DataTable GetTree(string queryField,string code)
        //{
        //    try
        //    {
        //        return Bll.Common.ChangColName(dal.GetTree(queryField,code));

        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(ex);
        //        return null;
        //    }
        //}
        //public DataTable GetTree(string code)
        //{
        //    try
        //    {
        //        return Bll.Common.ChangColName(dal.GetTree("code,codedesc", code));

        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(ex);
        //        return null;
        //    }
        //}
        //public DataTable GetSelectDevice(string where)
        //{
        //    try
        //    {
        //        return Bll.Common.ChangColName(dal.GetSelectDevice(where));

        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(ex);
        //        return null;
        //    }
        //}
        //public DataTable GetTreeDepartment(string where)
        //{
        //    try
        //    {
        //        return Bll.Common.ChangColName(dal.GetTreeDepartment(where));

        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(ex);
        //        return null;
        //    }
        //}
        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteDevice(string id)
        {
            try
            {
                return dal.DeleteDevice(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 查找设备运维信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetOperation(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetOperation(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.insertOperation(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.uptateOperation(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return Bll.Common.ChangColName(dal.GetSelectOperation(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///  获得设备运维统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTongJi(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetTongJi(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.DeleteOperation(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetDevState(string type)
        {
            try
            {
                return Common.ChangColName(dal.GetDevState(type));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetDeviceState_lock_id(string id)
        {
            try
            {
                return Common.ChangColName(dal.GetDeviceState_lock_id(id));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return Common.ChangColName(dal.GetServer(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return Common.ChangColName(dal.GetTGSSetting(field, where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.DeleteDeviceSetting(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.UptateDeviceSetting(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.InsertDeviceSetting(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }
       

        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetCctvSetting(string field, string where)
        {
            try
            {
                return Common.ChangColName(dal.GetCctvSetting(field, where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///删除连接信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteCctvSetting(string id)
        {
            try
            {
                return dal.DeleteCctvSetting(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.UptateCctvSetting(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.InsertCctvSetting(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return Common.ChangColName(dal.GetUTCSetting(field, where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.DeleteUtcSetting(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.UptateUtcSetting(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.InsertUtcSetting(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return Common.ChangColName(dal.GetVMSSetting(field, where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.DeleteVmsSetting(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.UptateVmsSetting(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.InsertVmsSetting(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 获得检测点类型总数信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetStationTypeByDevice()
        {
            try
            {
                return Common.ChangColName(dal.GetStationTypeByDevice());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return Common.ChangColName(dal.GetStationDeviceState(field, where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return Common.ChangColName(dal.GetNoDeviceState(field, where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return Common.ChangColName(dal.GetServiceState(field, where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.UptateServerInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.UptateDeviceInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 查找所有设备类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetDeviceType()
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetDeviceType());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据设备类型关联查询设备厂家
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDeviceTypeMode(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetDeviceTypeMode(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return Bll.Common.ChangColName(dal.GetBussiness(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.GetDeviceInfo(where);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///  查询服务器类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetServerType()
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetServerType());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return Bll.Common.ChangColName(dal.GetServerTypeMode(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.GetServerInfo(where);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.DeleteServerInfo(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTableSpace(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetTableSpace(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetBuildAll()
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetBuildAll());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetMaintainAll()
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetMaintainAll());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetMakeAll()
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetMakeAll());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得所有设备类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetDeviceTypeAll()
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetDeviceTypeAll());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return Bll.Common.ChangColName(dal.GetDevModeByDeviceType(devType));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return Bll.Common.ChangColName(dal.GetDeviceByDeviceType(devType));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return Bll.Common.ChangColName(dal.GetDeviceByMore(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listdev"></param>
        /// <returns></returns>
        public int InsertListDevice(List<MyNet.DataAccess.Model.DevcieInfo> listdev)
        {
            try
            {
                return dal.InsertListDevice(listdev);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
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
            try
            {
                return dal.InsertDeviceInfo(devcieInfo);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        //public MyNet.DataAccess.Model.DevcieInfo GetDeviceInfo(string devcieId)
        //{
        //    try
        //    {
        //        return dal.GetDeviceInfo(devcieId);
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(ex);
        //        return null;
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetServerTypeAll()
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetServerTypeAll());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetServerByMore(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetServerByMore(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        //public MyNet.DataAccess.Model.ServerInfo GetServerInfo(string serverId)
        //{
        //    try
        //    {
        //        return dal.GetServerInfo(serverId);
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(ex);
        //        return null;
        //    }
        //}
        //public int DeleteServerInfo(string id)
        //{
        //    try
        //    {
        //        return dal.DeleteServerInfo(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        ILog.WriteErrorLog(ex);
        //        return -1;
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public DataTable GetServerByTypeID(string deviceType)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetServerByTypeID(deviceType));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
    }
}