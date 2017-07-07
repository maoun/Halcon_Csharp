using System.Collections.Generic;
using System.Data;
using MyNet.DataAccess.Model;

namespace MyNet.Atmcs.Uscmcp.IData
{
    /// <summary>
    /// 设备管理接口
    /// </summary>
    public interface IDeviceManager
    {
        #region 查询相关方法

        /// <summary>
        ///
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        DataTable GetConfigDepartment(string systemId);

        /// <summary>
        /// 插入设备详细信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int insertDevice(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable GetSelectDevice(string id);

        /// <summary>
        /// 更新设备详细信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int uptateDevice(System.Collections.Hashtable hs);

        /// <summary>
        /// 查找设备运维信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetOperation(string where);

        /// <summary>
        /// 录入设备运维信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int insertOperation(System.Collections.Hashtable hs);

        /// <summary>
        ///更新设备的运维信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int uptateOperation(System.Collections.Hashtable hs);

        /// <summary>
        /// 查询选中设备的运维信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetSelectOperation(string id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteOperation(string id);

        /// <summary>
        /// 获得设备运维统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetTongJi(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetTreeDepartment(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int insertDevices(System.Collections.Hashtable hs);

        /// <summary>
        /// 修改设备信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int uptateDevices(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteKaKou(string id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteCCTV(string id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteLED(string id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable GetTypeCout(string type);

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable GetDevState(string type);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable GetDeviceState_lock_id(string id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetTGSSetting(string field, string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteDeviceSetting(string id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UptateDeviceSetting(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertDeviceSetting(System.Collections.Hashtable hs);

        /// <summary>
        ///获取连接信息
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetCctvSetting(string field, string where);

        /// <summary>
        ///删除连接信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteCctvSetting(string id);

        /// <summary>
        ///修改连接信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UptateCctvSetting(System.Collections.Hashtable hs);

        /// <summary>
        ///添加连接信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertCctvSetting(System.Collections.Hashtable hs);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetUTCSetting(string field, string where);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteUtcSetting(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UptateUtcSetting(System.Collections.Hashtable hs);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertUtcSetting(System.Collections.Hashtable hs);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetVMSSetting(string field, string where);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteVmsSetting(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UptateVmsSetting(System.Collections.Hashtable hs);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertVmsSetting(System.Collections.Hashtable hs);

        /// <summary>
        /// 获得检测点类型总数信息
        /// </summary>
        /// <returns></returns>
        DataTable GetStationTypeByDevice();

        /// <summary>
        /// 查询检测点设备状态
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetStationDeviceState(string field, string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetNoDeviceState(string field, string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetServiceState(string field, string where);

        /// <summary>
        ///更新服务器信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UptateServerInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UptateDeviceInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 查询设备信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetDevice(string where);
        DataTable GetHistoryDevice(string where);

        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteDevice(string id);

        /// <summary>
        /// 查找所有设备类型
        /// </summary>
        /// <returns></returns>
        DataTable GetDeviceType();

        /// <summary>
        /// 根据条件关联查询设备厂家
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetDeviceTypeMode(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetBussiness(string where);

        /// <summary>
        /// 查询设备信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetDeviceInfo(string where);

        /// <summary>
        ///  查询服务器类型
        /// </summary>
        /// <returns></returns>
        DataTable GetServerType();

        /// <summary>
        /// 根据条件关联查询服务器厂家
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetServerTypeMode(string where);

        /// <summary>
        /// 查询服务器信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetServerInfo(string where);

        /// <summary>
        /// 删除服务器信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteServerInfo(string id);

        /// <summary>
        ///查询服务器信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetServer(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetTableSpace(string where);

        # endregion

        #region 2015方法

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        DataTable GetBuildAll();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        DataTable GetMaintainAll();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        DataTable GetMakeAll();

        /// <summary>
        /// 获得所有设备类型
        /// </summary>
        /// <returns></returns>
        DataTable GetDeviceTypeAll();

        /// <summary>
        /// 根据设备类型获得对应设备厂家信息
        /// </summary>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        DataTable GetDevModeByDeviceType(string deviceType);

        /// <summary>
        /// 根据设备类型获得对应设备信息
        /// </summary>
        /// <param name="devType"></param>
        /// <returns></returns>
        DataTable GetDeviceByDeviceType(string deviceType);

        /// <summary>
        /// 根据查询条件查询设备详细信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetDeviceByMore(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="listdev"></param>
        /// <returns></returns>
        int InsertListDevice(List<DevcieInfo> listdev);

        /// <summary>
        /// 插入设备信息
        /// </summary>
        /// <param name="devcieInfo"></param>
        /// <returns></returns>
        int InsertDeviceInfo(DevcieInfo devcieInfo);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        DataTable GetServerTypeAll();

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetServerByMore(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        DataTable GetServerByTypeID(string deviceType);

        # endregion
    }
}