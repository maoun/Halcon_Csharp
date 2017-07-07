using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class TgsPproperty
    {
        #region 成员变量

        /// <summary>
        ///  用户操作接口
        /// </summary>
        private static readonly ITgsPproperty dal = DALFactory.CreateTgsPproperty();

        /// <summary>
        ///
        /// </summary>
        private SettingManager settingManager = new SettingManager();

        /// <summary>
        ///
        /// </summary>
        private string SystemID = "00";

        #endregion 成员变量

        #region 工作站信息

        /// <summary>
        ///获得用户分配的审核地点信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="shjb"></param>
        /// <returns></returns>
        public DataTable GetUserStationInfo(string userid, string shjb)
        {
            try
            {
                return Common.ChangColName(dal.GetUserStationInfo(userid, shjb));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得尚未分配用户审核的地点信息
        /// </summary>
        /// <param name="shjb"></param>
        /// <returns></returns>
        public DataTable GetNoUserStationInfo(string shjb)
        {
            try
            {
                return Common.ChangColName(dal.GetNoUserStationInfo(shjb));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///插入用户分配的审核地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertUserStationInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.InsertUserStationInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///删除用户分配的审核地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteUserStationInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteUserStationInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///获得检测点信息
        /// </summary>
        /// <returns></returns>
        private DataTable GetStationInfo()
        {
            try
            {
                return dal.GetStationInfo();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得所有地点信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllLocationInfo()
        {
            try
            {
                return Common.ChangColName(settingManager.GetLocationInfo(SystemID));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得地点信息
        /// </summary>
        /// <param name="datasource"></param>
        /// <returns></returns>
        public DataTable GetAllLocationInfo(string datasource)
        {
            try
            {
                return Common.ChangColName(settingManager.GetLocationInfo(SystemID, datasource));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得地点信息
        /// </summary>
        /// <param name="xzjb"></param>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public DataTable GetAllLocationInfo(string xzjb, string deptCode)
        {
            try
            {
                if (xzjb == "1")
                {
                    return Common.ChangColName(settingManager.GetAllLocationInfo(SystemID, " and 1=1"));
                }
                else if (xzjb == "2")
                {
                    return Common.ChangColName(settingManager.GetAllLocationInfo(SystemID, "and substr(departid,0,4)='" + deptCode.Substring(0, 4) + "' "));
                }
                else
                {
                    return Common.ChangColName(settingManager.GetAllLocationInfo(SystemID, " and substr(departid,0,8)='" + deptCode.Substring(0, 8) + "' "));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///查询需要审核的地点信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="shcs"></param>
        /// <returns></returns>
        public DataTable GetCheckLocation(string userid, string shcs)
        {
            try
            {
                return Common.ChangColName(settingManager.GetCheckLocation(SystemID, " and 1=1", userid, shcs));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///查询需要审核的地点信息
        /// </summary>
        /// <param name="xzjb"></param>
        /// <param name="deptCode"></param>
        /// <param name="userid"></param>
        /// <param name="shcs"></param>
        /// <returns></returns>
        public DataTable GetCheckLocation(string xzjb, string deptCode, string userid, string shcs)
        {
            try
            {
                if (xzjb == "1")
                {
                    return Common.ChangColName(settingManager.GetCheckLocation(SystemID, " and 1=1", userid, shcs));
                }
                else if (xzjb == "2")
                {
                    return Common.ChangColName(settingManager.GetCheckLocation(SystemID, "and substr(departid,0,4)='" + deptCode.Substring(0, 4) + "' ", userid, shcs));
                }
                else
                {
                    return Common.ChangColName(settingManager.GetCheckLocation(SystemID, " and substr(departid,0,8)='" + deptCode.Substring(0, 8) + "' ", userid, shcs));
                }
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
        /// <param name="xzjb"></param>
        /// <param name="deptCode"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public DataTable GetAllLocationInfo(string xzjb, string deptCode, string dataSource)
        {
            try
            {
                if (xzjb == "1")
                {
                    return Common.ChangColName(settingManager.GetAllLocationInfo(SystemID, "   and datasource='" + dataSource + "'"));
                }
                else if (xzjb == "2")
                {
                    return Common.ChangColName(settingManager.GetAllLocationInfo(SystemID, " and substr(departid,0,4)='" + deptCode.Substring(0, 4) + "' and datasource='" + dataSource + "'"));
                }
                else
                {
                    return Common.ChangColName(settingManager.GetAllLocationInfo(SystemID, " and substr(departid,0,8)='" + deptCode.Substring(0, 8) + "' and datasource='" + dataSource + "' "));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///查询所有监测点信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllStationInfo()
        {
            try
            {
                return Common.ChangColName(dal.GetStationInfo("1=1"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetAllStationInfoWithRoad(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetAllStationInfoWithRoad(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取已存在的监测信息卡口
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetBindKkmc(string where)
        {
            try
            {
                return Bll.Common.ChangColName(dal.GetBindKkmc(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 判断是时间规则是否存在
        /// </summary>
        /// <param name="Bh"></param>
        /// <returns></returns>
        public bool ExistTimeRuleBh(string Bh)
        {
            try
            {
                return dal.ExistTimeRuleBh(Bh);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 通过时段编号获取配置编号和区域编号
        /// </summary>
        /// <param name="timeRuleBh"></param>
        /// <returns></returns>
        public DataTable GetBh(string timeRuleBh)
        {
            try
            {
                return dal.GetBh(timeRuleBh);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取区域规则
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DataTable GetAreaRule(string time)
        {
            try
            {
                return dal.GetAreaRule(time);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据卡口编号获取卡口名称
        /// </summary>
        /// <param name="Kkid"></param>
        /// <returns></returns>
        public string GetKkmcByKkid(string Kkid)
        {
            try
            {
                return dal.GetKkmcByKkid(Kkid);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 添加重点车辆时间规则信息
        /// </summary>
        /// <param name="sdbh"></param>
        /// <param name="xmlRule"></param>
        /// <returns></returns>
        public int insertTimerule(string sdbh, string xmlRule)
        {
            try
            {
                return dal.insertTimerule(sdbh, xmlRule);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 添加重点车辆区域规则信息
        /// </summary>
        /// <param name="qybh"></param>
        /// <param name="xmlRule"></param>
        /// <returns></returns>
        public int insertRegionrule(string qybh, string xmlRule)
        {
            try
            {
                return dal.insertRegionrule(qybh, xmlRule);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 添加重点车辆配置信息
        /// </summary>
        /// <param name="pzbh"></param>
        /// <param name="type"></param>
        /// <param name="sdbh"></param>
        /// <param name="qybh"></param>
        /// <param name="cfjg"></param>
        /// <returns></returns>
        public int insertConfig(string pzbh, string type, string sdbh, string qybh, string cfjg)
        {
            try
            {
                return dal.insertConfig(pzbh, type, sdbh, qybh, cfjg);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 添加重点车辆类型与配置信息关联信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="zdcllx"></param>
        /// <param name="pzbh"></param>
        /// <returns></returns>
        public int insertRelation(string id, string zdcllx, string pzbh)
        {
            try
            {
                return dal.insertRelation(id, zdcllx, pzbh);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 删除重点车辆时间规则信息
        /// </summary>
        /// <param name="sdbh"></param>
        /// <returns></returns>
        public int deleteTimerule(string sdbh)
        {
            try
            {
                return dal.deleteTimerule(sdbh);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 删除重点车辆区域规则信息
        /// </summary>
        /// <param name="qybh"></param>
        /// <returns></returns>
        public int deleteRegionrule(string qybh)
        {
            try
            {
                return dal.deleteRegionrule(qybh);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 删除重点车辆配置信息
        /// </summary>
        /// <param name="pzbh"></param>
        /// <returns></returns>
        public int deleteConfig(string pzbh)
        {
            try
            {
                return dal.deleteConfig(pzbh);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 更新区域规则
        /// </summary>
        /// <param name="id"></param>
        /// <param name="qygz"></param>
        /// <returns></returns>
        public bool selectRegionrule(string id)
        {
            try
            {
                return dal.selectRegionrule(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 删除重点车辆类型与配置信息关联信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int deleteRelation(string id)
        {
            try
            {
                return dal.deleteRelation(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///查询监测点信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetTgsStationInfo()
        {
            try
            {
                return Common.ChangColName(dal.GetStationInfo(" a.station_type_id='15'"));
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
        /// <param name="stationType"></param>
        /// <returns></returns>
        public DataTable GetDeviceTypeByStation(string stationType)
        {
            try
            {
                return Common.ChangColName(dal.GetDeviceTypeByStation(stationType));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得设备信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDeviceInfoByWhere(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetDeviceInfoByWhere(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得设备信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDeviceInfoByStation(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetDeviceInfoByStation(where));
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
        /// <param name="stationid"></param>
        /// <param name="deviceList"></param>
        /// <returns></returns>
        public int InsertStationDevice(string stationid, List<string> deviceList)
        {
            try
            {
                return dal.InsertStationDevice(stationid, deviceList);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 保存重点道路关联监测点信息
        /// </summary>
        /// <param name="stationid"></param>
        /// <param name="deviceList"></param>
        /// <returns></returns>
        public int InsertKeyroadStation(string id, string dlId, List<string> stationList)
        {
            try
            {
                return dal.InsertKeyroadStation(id, dlId, stationList);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 获取监测点信息
        /// </summary>
        /// <param name="roadId"></param>
        /// <returns></returns>
        public DataTable GetStationInfoByRoad(string roadId)
        {
            try
            {
                return Common.ChangColName(dal.GetStationInfoByRoad(roadId));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取监测点类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetStationTypeInfo()
        {
            try
            {
                return Common.ChangColName(dal.GetStationTypeInfo());
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
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteDeviceStation(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteDeviceStation(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 查询违法监测点信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetTmsStationInfo()
        {
            try
            {
                return Common.ChangColName(dal.GetStationInfo(" istmsshow='1'"));
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
        public DataTable GetTgsStationLocationInfo()
        {
            try
            {
                return Common.ChangColName(dal.GetlocationByStationInfo(" a.station_type_id='15'"));
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
        public DataTable GetTmsStationLocationInfo()
        {
            try
            {
                return Common.ChangColName(dal.GetlocationByStationInfo(" istmsshow='1'"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///查询监测点信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetStationInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetStationInfo(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///通过机构编号获得下属的地点信息
        /// </summary>
        /// <param name="departId"></param>
        /// <returns></returns>
        public DataTable GetLocationByDepartId(string departId)
        {
            try
            {
                return Common.ChangColName(dal.GetLocationByDepartId(departId));
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
        public DataTable GetLocationStationByWhere(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetLocationStationByWhere(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///通过地点编号获得配置的监测点信息
        /// </summary>
        /// <param name="location_id"></param>
        /// <returns></returns>
        public DataTable GetStationInfoByLocation(string location_id)
        {
            try
            {
                return Common.ChangColName(dal.GetStationInfoByLocation(location_id));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取检测点类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetStationTypeInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetStationTypeInfo(where));
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
        public DataTable GetStationInfoView_ByKK()
        {
            try
            {
                return Common.ChangColName(dal.GetStationInfoView("a.station_type_id='15'"));
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
        public DataTable GetRecordPlayByStationInfo()
        {
            try
            {
                return Common.ChangColName(dal.GetRecordPlayByStationInfo());
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
        public DataTable GetStationInfoView_ByDJ()
        {
            try
            {
                return Common.ChangColName(dal.GetStationInfoView("istmsshow='1'"));
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
        /// <param name="stationId"></param>
        /// <returns></returns>
        public DataTable GetLaneInfoView_ByStationId(string stationId)
        {
            try
            {
                return Common.ChangColName(dal.GetLaneInfoView("STATION_ID ='" + stationId + "'"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获取车道信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetLaneInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetLaneInfo(where));
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
        /// <param name="stationId"></param>
        /// <param name="directionid"></param>
        /// <returns></returns>
        public DataTable GetLaneInfoView(string stationId, string directionid)
        {
            try
            {
                return Common.ChangColName(dal.GetLaneInfoView("STATION_ID ='" + stationId + "' and direction_id='" + directionid + "'"));
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
        /// <param name="stationid"></param>
        /// <returns></returns>
        public DataTable GetDirectionDeviceByStation(string stationid)
        {
            try
            {
                return Common.ChangColName(dal.GetDirectionDeviceByStation(stationid));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得方向信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDirectionByTgs()
        {
            try
            {
                return Common.ChangColName(dal.GetDirectionByTgs());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///根据方向编号 获得方向名称
        /// </summary>
        /// <param name="direction_id"></param>
        /// <returns></returns>
        public string GetDirectionName(string direction_id)
        {
            try
            {
                return dal.GetDirectionName(direction_id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetStationInfoView(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetStationInfoView(where));
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
        /// <param name="directionid"></param>
        /// <returns></returns>
        public DataTable GetLaneInfoView_ByDirectionId(string directionid)
        {
            try
            {
                return Common.ChangColName(dal.GetLaneInfoView("direction_id ='" + directionid + "'"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///更新监测点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateStationInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateStationInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///更新车道信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateLaneInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateLaneInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///更新车道信息
        /// </summary>
        /// <param name="deviceid"></param>
        /// <param name="directionid"></param>
        /// <returns></returns>
        public int UpdateLaneInfo(string deviceid, string directionid)
        {
            try
            {
                return dal.UpdateLaneInfo(deviceid, directionid);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///删除车道信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteLaneInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteLaneInfo(hs);
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
        public int DeleteStationInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteStationInfo(hs);
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
        /// <param name="station_id"></param>
        /// <param name="direction_id"></param>
        /// <returns></returns>
        public int DeleteLaneInfoByDirection(string station_id, string direction_id)
        {
            try
            {
                return dal.DeleteLaneInfoByDirection(station_id, direction_id);
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
        public int DeleteStationByLocation(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteStationByLocation(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///添加车道信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertLaneInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.InsertLaneInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///插入车道信息
        /// </summary>
        /// <param name="hs"></param>
        /// <param name="startLane"></param>
        /// <param name="endLane"></param>
        /// <returns></returns>
        public int InsertLaneInfo(System.Collections.Hashtable hs, int startLane, int endLane)
        {
            try
            {
                return dal.InsertLaneInfo(hs, startLane, endLane);
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
        public int InsertStationInfoByDJ(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.InsertStationInfo(hs);
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
        public int InsertStationInfoByKK(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.InsertStationInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        #endregion 工作站信息

        #region 地点方向信息

        /// <summary>
        /// 获得方向信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDirectionInfo()
        {
            try
            {
                return Common.ChangColName(dal.GetDirectionInfo());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///根据检测点编号获得对应方法
        /// </summary>
        /// <param name="station_id"></param>
        /// <returns></returns>
        public DataTable GetDirectionInfoByStation(string station_id)
        {
            try
            {
                return Common.ChangColName(dal.GetDirectionInfoByStation(station_id));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///通过监测点编号获得配置的方向信息
        /// </summary>
        /// <param name="station_id"></param>
        /// <returns></returns>
        public DataTable GetDirectionInfoByStation2(string station_id)
        {
            try
            {
                return Common.ChangColName(dal.GetDirectionInfoByStation2(station_id));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///通过监测点编号获得配置的方向信息
        /// </summary>
        /// <param name="station_id"></param>
        /// <returns></returns>
        public DataTable GetDirectionInfoByWhere(string station_id)
        {
            try
            {
                return Common.ChangColName(dal.GetDirectionInfoByWhere(station_id));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得方向信息
        /// </summary>
        /// <param name="station_id"></param>
        /// <returns></returns>
        public DataTable GetDirectionInfo(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetDirectionInfo(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        #endregion 地点方向信息

        #region 字典信息

        /// <summary>
        /// 获得号牌种类
        /// </summary>
        /// <returns></returns>
        public DataTable GetPalteType()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "140001"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得套牌审核标记
        /// </summary>
        /// <returns></returns>
        public DataTable GetReplacePalteType()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "240041"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///  获得方向字典
        /// </summary>
        /// <returns></returns>
        public DataTable GetDirectionDict()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "240025"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得处理标记代码
        /// </summary>
        /// <returns></returns>
        public DataTable GetDealTypeDict()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "240009"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得黑名单类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetSuspicionDict()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "240012"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得白名单类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetChecklessDict()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "240018"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得畅行车辆类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetExtraListDict()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "240017"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得违法复审标记
        /// </summary>
        /// <returns></returns>
        public DataTable GetIsUseDict()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "240042"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得报警类型字典
        /// </summary>
        /// <returns></returns>
        public DataTable GetAlarmTypeDict()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "240012"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得报警类型字典
        /// </summary>
        /// <returns></returns>
        public DataTable GetCommonAlarmTypeDict()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "300100"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得设备类型字典
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetDeviceTypeDict(string code)
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, code));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取车辆品牌字典
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetVehicleBrand(string where)
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.GetVehicleBrand(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取车辆型号字典
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetVehicleModel(string where)
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.GetVehicleModel(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得采集数据设备来源代码
        /// </summary>
        /// <returns></returns>
        public DataTable GetCompanyDict()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "240014"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得车身颜色表
        /// </summary>
        /// <returns></returns>
        public DataTable GetCarColorDict()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "240013"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得黑名单处理状态
        /// </summary>
        /// <returns></returns>
        public DataTable GetIsSuspicionDict()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "240011"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得比对标记代码表
        /// </summary>
        /// <returns></returns>
        public DataTable GetIsCompareDict()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "240010"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得进程标记代码表
        /// </summary>
        /// <returns></returns>
        public DataTable GetProcessType()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "240019"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得行政级别代码表
        /// </summary>
        /// <returns></returns>
        public DataTable GetDepartClass()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "140004"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得查询显示条数
        /// </summary>
        /// <returns></returns>
        public DataTable GetQueryNum()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "140006"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///根据传入字典编号获得字典
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetCommonDict(string code)
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, code));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得部门信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDepartmentDict()
        {
            try
            {
                DataTable dt = settingManager.GetDepartmentDict(SystemID);
                return Bll.Common.ChangColName(dt);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得违法类型信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyType(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyType(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得违法类型信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyTypeNew(string where, int startRow, int endRow)
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyTypeNew(where, startRow, endRow));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得违法类型信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyTypeCount(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyTypeCount(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得违法类型
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyTypeSetting(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyTypeSetting(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得全部违法类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetPeccancyType()
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyType("1=1"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///配置区间测速开始监测点信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetStartStationInfo()
        {
            try
            {
                return Common.ChangColName(dal.GetStartStationInfo());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得结束检测点信息
        /// </summary>
        /// <param name="kskkid"></param>
        /// <returns></returns>
        public DataTable GetEndStationInfo(string kskkid)
        {
            try
            {
                return Common.ChangColName(dal.GetEndStationInfo(kskkid));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得结束检测点信息
        /// </summary>
        /// <param name="kskkid"></param>
        /// <returns></returns>
        public DataTable GetEndStationDict(string kkidList)
        {
            try
            {
                return Common.ChangColName(dal.GetEndStationDict(kkidList));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得数据来源代码表
        /// </summary>
        /// <returns></returns>
        public DataTable GetStationTypeDict()
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(SystemID, "300100"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        #endregion 字典信息

        #region 违法类型信息

        /// <summary>
        ///删除违法类型信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeletePeccancyType(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeletePeccancyType(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 判断输入值是否在指定表存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public int GeXhExist(string tableName, string fieldName, string fieldValue)
        {
            try
            {
                return dal.GeXhExist(tableName, fieldName, fieldValue);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///更新违法类型信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdatePeccancyType(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdatePeccancyType(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///更新违法类型信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdatePeccancyTypeSetting(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdatePeccancyTypeSetting(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 插入重点车辆超速配置
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertKeycar_Config(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.InsertKeycar_Config(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 删除重点车辆超速配置
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        public int DeleteKeycar_Config(string configId)
        {
            try
            {
                return dal.DeleteKeycar_Config(configId);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///获得区间违法配置
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyAreaSetting()
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyAreaSetting());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the peccancy area setting.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetPeccancyAreaSetting(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetPeccancyAreaSetting(where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///删除违法类型信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeletePeccancyAreaSetting(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeletePeccancyAreaSetting(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///更新区间违法配置
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdatePeccancyAreaSetting(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdatePeccancyAreaSetting(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///删除违法类型信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteNewPeccancyAreaSetting(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteNewPeccancyAreaSetting(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///更新区间违法配置
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateNewPeccancyAreaSetting(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateNewPeccancyAreaSetting(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///查询区间违法配置
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAreaSetting(string where)
        {
            try
            {
                return Common.ChangColName(dal.GetAreaSetting(where));
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
        /// <param name="depart"></param>
        /// <param name="checktimes"></param>
        /// <returns></returns>
        public DataTable GetUserCode(string depart, string checktimes)
        {
            try
            {
                return Common.ChangColName(dal.GetUserCode(depart, checktimes));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        #endregion 违法类型信息

        #region 通用方法

        /// <summary>
        /// 获得记录ID
        /// </summary>
        /// <returns></returns>
        public string GetRecordId()
        {
            return DateTime.Now.ToString("yyMMddHHmmssfff");
        }

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

        /// <summary>
        ///获得最小记录ID
        /// </summary>
        /// <returns></returns>
        public string GetMinRecordId()
        {
            return DateTime.Now.ToString("mmssff");
        }

        /// <summary>
        /// 判断序号是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public int GetXhExist(string tableName, string fieldName, string fieldValue)
        {
            try
            {
                return dal.GetXhExist(tableName, fieldName, fieldValue);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return 0;
            }
        }

        #endregion 通用方法
    }
}