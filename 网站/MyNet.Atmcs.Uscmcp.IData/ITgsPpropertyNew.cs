using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.IData
{
    public interface ITgsPpropertyNew
    {
        /// <summary>
        /// 获得检测点信息
        /// </summary>
        /// <returns></returns>
        DataTable GetStationInfo();

        /// <summary>
        /// 获得车道信息
        /// </summary>
        /// <returns></returns>
        DataTable GetLaneInfo();

        /// <summary>
        /// 通过地点编号获得配置的监测点信息
        /// </summary>
        /// <param name="location_id"></param>
        /// <returns></returns>
        DataTable GetStationInfoByLocation(string location_id);

        /// <summary>
        /// 获得方向信息
        /// </summary>
        /// <returns></returns>
        DataTable GetDirectionInfo();

        DataTable GetDirectionInfo(string where);

        /// <summary>
        /// 通过监测点编号获得配置的方向信息
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        DataTable GetDirectionInfoByStation(string stationId);

        /// <summary>
        /// 通过监测点编号获得配置的方向信息
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        DataTable GetDirectionInfoByWhere(string stationId);

        /// <summary>
        /// 通过监测点编号获得配置的方向信息
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        DataTable GetDirectionInfoByStation2(string stationId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        DataTable GetDirectionDeviceByStation(string stationId);

        /// <summary>
        /// 通过机构编号获得下属的地点信息
        /// </summary>
        /// <param name="departId"></param>
        /// <returns></returns>
        DataTable GetLocationByDepartId(string departId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetLocationStationByWhere(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="stationType"></param>
        /// <returns></returns>
        DataTable GetDeviceTypeByStation(string stationType);

        /// <summary>
        /// 获得设备信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetDeviceInfoByWhere(string where);

        /// <summary>
        /// 获得设备信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetDeviceInfoByStation(string where);

        /// <summary>
        /// 获得方向信息
        /// </summary>
        /// <returns></returns>
        DataTable GetDirectionByTgs();

        /// <summary>
        /// 根据方向编号 获得方向名称
        /// </summary>
        /// <param name="direction_id"></param>
        /// <returns></returns>
        string GetDirectionName(string direction_id);

        //监测点信息增，删，改，查
        /// <summary>
        /// 获取检测点类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetStationTypeInfo(string where);

        /// <summary>
        /// 查询监测点信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetStationInfo(string where);

        /// <summary>
        /// 查询所有监测点信息（道路关联）
        /// </summary>
        /// <returns></returns>
        DataTable GetAllStationInfoWithRoad(string where);

        /// <summary>
        /// 获取已存在的监测信息卡口
        /// </summary>
        /// <returns></returns>
        DataTable GetBindKkmc(string where);

        /// <summary>
        /// 判断是时间规则是否存在
        /// </summary>
        /// <param name="BH"></param>
        /// <returns></returns>
        bool ExistTimeRuleBh(string BH);

        /// <summary>
        /// 通过时段编号获取配置编号和区域编号
        /// </summary>
        /// <returns></returns>
        DataTable GetBh(string timeRuleBh);

        /// <summary>
        /// 获取区域规则
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        DataTable GetAreaRule(string time);

        /// <summary>
        /// 根据卡口编号获取卡口名称
        /// </summary>
        /// <param name="Kkid"></param>
        /// <returns></returns>
        string GetKkmcByKkid(string Kkid);

        /// <summary>
        /// 添加重点车辆时间规则信息
        /// </summary>
        /// <returns></returns>
        int insertTimerule(string sdbh, string xmlRule);

        /// <summary>
        /// 添加重点车辆区域规则信息
        /// </summary>
        /// <returns></returns>
        int insertRegionrule(string qybh, string xmlRule);

        /// <summary>
        /// 添加重点车辆配置信息
        /// </summary>
        /// <returns></returns>
        int insertConfig(string pzbh, string type, string sdbh, string qybh, string cfjg);

        /// <summary>
        /// 添加重点车辆类型与配置信息关联信息
        /// </summary>
        /// <returns></returns>
        int insertRelation(string id, string zdcllx, string pzbh);

        /// <summary>
        /// 删除重点车辆时间规则信息
        /// </summary>
        /// <returns></returns>
        int deleteTimerule(string sdbh);

        /// <summary>
        /// 删除重点车辆区域规则信息
        /// </summary>
        /// <returns></returns>
        int deleteRegionrule(string qybh);

        /// <summary>
        /// 删除重点车辆配置信息
        /// </summary>
        /// <returns></returns>
        int deleteConfig(string pzbh);

        /// <summary>
        /// 删除重点车辆类型与配置信息关联信息
        /// </summary>
        /// <returns></returns>
        int deleteRelation(string id);

        /// <summary>
        /// 查找区域规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool selectRegionrule(string id);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetlocationByStationInfo(string where);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        DataTable GetRecordPlayByStationInfo();

        /// <summary>
        /// 从视图获得监测点信息（带条件）
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetStationInfoView(string where);

        /// <summary>
        /// 更新监测点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateStationInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="stationid"></param>
        /// <param name="deviceList"></param>
        /// <returns></returns>
        int InsertStationDevice(string stationid, List<string> deviceList);

        /// <summary>
        /// 保存重点道路关联监测点信息
        /// </summary>
        /// <param name="dlId"></param>
        /// <param name="stationList"></param>
        /// <returns></returns>
        int InsertKeyroadStation(string id, string dlId, List<string> stationList);

        /// <summary>
        /// 获取监测点信息
        /// </summary>
        /// <returns></returns>
        DataTable GetStationInfoByRoad(string roadId);

        /// <summary>
        /// 获取监测点类型
        /// </summary>
        /// <returns></returns>
        DataTable GetStationTypeInfo();

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteStationInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertStationInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteStationByLocation(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteDeviceStation(System.Collections.Hashtable hs);

        /// <summary>
        ///获取车道信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetLaneInfo(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetLaneInfoView(string where);

        /// <summary>
        ///更新车道信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateLaneInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///更新车道信息
        /// </summary>
        /// <param name="deviceid"></param>
        /// <param name="directionid"></param>
        /// <returns></returns>
        int UpdateLaneInfo(string deviceid, string directionid);

        /// <summary>
        ///删除车道信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteLaneInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 插入车道信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertLaneInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 插入车道信息
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="startLane"></param>
        /// <param name="endLane"></param>
        /// <returns></returns>
        int InsertLaneInfo(System.Collections.Hashtable hs, int startLane, int endLane);

        /// <summary>
        ///
        /// </summary>
        /// <param name="station_id"></param>
        /// <param name="direction_id"></param>
        /// <returns></returns>
        int DeleteLaneInfoByDirection(string station_id, string direction_id);

        /// <summary>
        /// 获得违法类型信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPeccancyType(string where);

        /// <summary>
        /// 获得违法类型信息（新的）
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPeccancyTypeNew(string where, int startRow, int endRow);

        /// <summary>
        /// 获得违法类型信息总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPeccancyTypeCount(string where);

        /// <summary>
        /// 更新违法类型信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdatePeccancyType(System.Collections.Hashtable hs);

        /// <summary>
        /// 删除违法类型信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeletePeccancyType(System.Collections.Hashtable hs);

        /// <summary>
        /// 判断输入值是否在指定表存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        int GeXhExist(string tableName, string fieldName, string fieldValue);

        /// <summary>
        ///
        /// </summary>
        /// <param name="kskkid"></param>
        /// <returns></returns>
        DataTable GetEndStationInfo(string kskkid);

        /// <summary>
        ///
        /// </summary>
        /// <param name="kskkid"></param>
        /// <returns></returns>
        DataTable GetEndStationDict(string kkidList);

        /// <summary>
        /// 配置区间测速开始监测点信息
        /// </summary>
        /// <returns></returns>
        DataTable GetStartStationInfo();

        /// <summary>
        /// 获得区间违法配置
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPeccancyAreaSetting();

        /// <summary>
        /// Gets the peccancy area setting.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPeccancyAreaSetting(string where);

        /// <summary>
        ///删除区间配置信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteNewPeccancyAreaSetting(System.Collections.Hashtable hs);

        /// <summary>
        /// 更新区间违法配置
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateNewPeccancyAreaSetting(System.Collections.Hashtable hs);

        /// <summary>
        ///删除区间配置信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeletePeccancyAreaSetting(System.Collections.Hashtable hs);

        /// <summary>
        /// 更新区间违法配置
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdatePeccancyAreaSetting(System.Collections.Hashtable hs);

        /// <summary>
        /// 插入重点车辆超速配置
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertKeycar_Config(System.Collections.Hashtable hs);

        /// <summary>
        /// 删除重点车辆超速配置
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        int DeleteKeycar_Config(string configId);

        /// <summary>
        /// 获得用户分配的审核地点信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="shjb"></param>
        /// <returns></returns>
        DataTable GetUserStationInfo(string userid, string shjb);

        /// <summary>
        /// 获得尚未分配用户审核的地点信息
        /// </summary>
        /// <param name="shjb"></param>
        /// <returns></returns>
        DataTable GetNoUserStationInfo(string shjb);

        /// <summary>
        /// 插入用户分配的审核地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertUserStationInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 删除用户分配的审核地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteUserStationInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 查询区间违法配置
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetAreaSetting(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="depart"></param>
        /// <param name="checktime"></param>
        /// <returns></returns>
        DataTable GetUserCode(string depart, string checktime);

        /// <summary>
        /// 查询违法类型信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPeccancyTypeSetting(string where);

        /// <summary>
        /// 更新违法类型信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdatePeccancyTypeSetting(System.Collections.Hashtable hs);

        /// <summary>
        /// 判断序号是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        int GetXhExist(string tableName, string fieldName, string fieldValue);
    }
}