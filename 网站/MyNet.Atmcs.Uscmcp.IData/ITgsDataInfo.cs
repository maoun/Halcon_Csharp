using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.IData
{
    public interface ITgsDataInfo
    {
           /// <summary>
        /// 获得政府公告信息
        /// </summary>
        /// <returns></returns>
        DataTable GetZfgg();
         /// <summary>
        /// 插入违法信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertPeccancy(System.Collections.Hashtable hs);
          /// <summary>
        /// 得到车辆品牌的子品牌字符串
        /// </summary>
        /// <param name="clpp"></param>
        /// <returns></returns>
         DataTable GetClppString(string clpp);
        
        #region 过往车辆接口

        /// <summary>
        /// 获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        DataTable GetPassCarInfo(string where, int startrow, int endrow);

        /// <summary>
        ///获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="rowWhere"></param>
        /// <returns></returns>
        DataTable GetPassCarInfo(string where, string rowWhere);

        /// <summary>
        ///获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPassCarInfo(string where);

        /// <summary>
        ///获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetQueryTimeTemp(string where);

        /// <summary>
        ///获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPassCarTimesCount(string where);

        /// <summary>
        ///获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPassCarTimesInfo(string where);

        /// <summary>
        ///获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="times"></param>
        /// <param name="rownum"></param>
        /// <returns></returns>
        DataTable GetPassCarTimesInfo(string where, string times, string rownum);

        /// <summary>
        ///获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="times"></param>
        /// <param name="rownum"></param>
        /// <returns></returns>
        DataTable GetPassCarDangerInfo(string where, string times, string rownum);

        /// <summary>
        ///获得过往车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPassCarInfoMaxGwsj(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        int GetPassCarInfoNum(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetQueryTime(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetQueryTimeList(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        string GetPassCarString(string field, string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        string GetPassCarMaxString(string field, string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        int GetQueryTimeCount(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetQueryTwoTimeList(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetAllPassCarInfo(string field, string where);

        /// <summary>
        ///查询图片地址
        /// </summary>
        /// <param name="xh"></param>
        /// <returns></returns>
        DataTable GetPassCarImageUrl(string xh);

        /// <summary>
        ///
        /// </summary>
        /// <param name="xh"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPassCarImageUrl(string xh, string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPasscarCount(string field, string where);

        /// <summary>
        /// 获得车辆轨迹信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPassCarTrackInfo(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPassCarMonitor(string where);

        #endregion 过往车辆接口

        #region 速度信息接口

        /// <summary>
        /// 获得平均速度
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        DataTable PassCarAveSpeed(string directionId, string date);

        /// <summary>
        ///获取最高速度流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        DataTable PassCarHighSpeed(string directionId, string date);

        /// <summary>
        ///获取识别率信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        DataTable PassCarOcr(string directionId, string date);

        #endregion 速度信息接口

        #region 违法信息接口

        /// <summary>
        /// 获得违法信息
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        DataTable GetPeccancyInfo(string field, string where, int startrow, int endrow);
        /// <summary>
        /// 获得违法信息总记录
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        DataTable GetPeccancyInfoCount(string where);


        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        DataTable GetPassCarTemp(string where, int startIndex, int endIndex);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        int GetPassCarTempCount(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="xh"></param>
        /// <returns></returns>
        int UnlockOneInfo(string xh);

        /// <summary>
        /// 将相对应的违法记录进行加锁
        /// </summary>
        /// <param name="where">加锁条件</param>
        /// <param name="sdr">锁定人</param>
        /// <param name="sdsj">锁定时间</param>
        /// <param name="lockAmount">锁定条数</param>
        /// <returns></returns>
        int LockPeccancy(string where, string sdr, string sdsj, int lockAmount);

        /// <summary>
        ///将超过1个小时未解锁或者自己的的违法记录全部解锁
        /// </summary>
        /// <param name="sdsj"></param>
        /// <param name="sdr"></param>
        /// <returns></returns>
        int UnAlllockAll(string sdsj, string sdr);

        /// <summary>
        ///
        /// </summary>
        /// <param name="sdsj"></param>
        /// <param name="sdr"></param>
        /// <returns></returns>
        int UnlockAll(string sdsj, string sdr);

        /// <summary>
        ///对自己查询的信息进行加锁
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sdr"></param>
        /// <param name="sdsj"></param>
        /// <returns></returns>
        int LockPeccancy(string where, string sdr, string sdsj);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int CheckPeccancyInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPeccancyWorkNum(string field, string where);

        /// <summary>
        /// 获得指定条件中最大最小违法时间及总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPeccancyMaxWfsj(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="countType"></param>
        /// <returns></returns>
        DataTable GetPeccancyCount(string field, string where, string countType);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="times"></param>
        /// <param name="rownum"></param>
        /// <returns></returns>
        DataTable GetPeccancyTimesInfo(string where, string times, string rownum);

        /// <summary>
        /// 更新违法记录
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdatePeccancyInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 删除违法记录
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        int DeletePeccancyInfo(List<string> records);

        /// <summary>
        ///插入违法记录
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertPeccancyInfo(Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertCaptureInfo(Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int PeccancyOnly(Hashtable hs);

        #endregion 违法信息接口

        #region 流量信息接口

        /// <summary>
        ///获得15分钟流量信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable PassCar15MinFlow(string where);

        /// <summary>
        /// 获得5分钟流量信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable PassCar5MinFlow(string where);

        /// <summary>
        /// 获得5分钟流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataTable PassCar5MinFlow(string directionId, string startDate, string endDate);

        /// <summary>
        /// 获得天流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        DataTable PassCarDayFlow(string directionId, string date);

        /// <summary>
        ///  获得天流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataTable PassCarDayFlow(string directionId, string startDate, string endDate);

        /// <summary>
        ///  获得小时流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        DataTable PassCarHourFlow(string directionId, string date);

        /// <summary>
        ///  获得月流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        DataTable PassCarMonthFlow(string directionId, string date);

        /// <summary>
        ///  获得月流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataTable PassCarMonthFlow(string directionId, string startDate, string endDate);

        /// <summary>
        /// 获得周流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        DataTable PassCarWeekFlow(string directionId, string date);

        /// <summary>
        /// 获得周流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataTable PassCarWeekFlow(string directionId, string startDate, string endDate);

        /// <summary>
        /// 获得年流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        DataTable PassCarYearFlow(string directionId, string date);

        /// <summary>
        /// 获得年流量信息
        /// </summary>
        /// <param name="directionId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataTable PassCarYearFlow(string directionId, string startDate, string endDate);

        #endregion 流量信息接口

        #region 设备信息接口

        //设备信息
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        DataTable GetDeviceState();

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetDeviceState(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetDeviceInfo(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateDeviceInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateDeviceStation(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertDeviceInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteDeviceInfo(System.Collections.Hashtable hs);

        #endregion 设备信息接口

        #region 获得Dev设备信息接口

        /// <summary>
        /// 设备信息
        /// </summary>
        /// <returns></returns>
        DataTable GetDevDeviceState();

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetDevDeviceState(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetDevDeviceInfo(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateDevDeviceInfo(System.Collections.Hashtable hs);

        //int UpdateDevDeviceStation(System.Collections.Hashtable hs);
        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertDevDeviceInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteDevDeviceInfo(System.Collections.Hashtable hs);

        #endregion 获得Dev设备信息接口

        /// <summary>
        ///
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="direction_id"></param>
        /// <returns></returns>
        DataTable GetHttpPath(string stationId, string direction_id);

        /// <summary>
        /// 判断序号是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        int GeXhExist(string tableName, string fieldName, string fieldValue);

        #region 布控信息接口

        /// <summary>
        /// 查询布控车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetSuspicion(string where);

        /// <summary>
        /// 更新布控车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateSuspicionInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 删除布控车辆
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteSuspicionInfo(System.Collections.Hashtable hs);

        #endregion 布控信息接口

        #region 畅行车辆接口

        /// <summary>
        /// 查询畅行车辆
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetCheckless(string where);

        /// <summary>
        /// 更新畅行车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateChecklessInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 删除畅行车辆
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteChecklessInfo(System.Collections.Hashtable hs);

        #endregion 畅行车辆接口

        #region 特殊勤务车辆接口

        /// <summary>
        /// 获得特殊勤务车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetExtraList(string where);

        /// <summary>
        /// 更新特殊勤务车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateExtraListInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 删除特殊勤务车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteExtraListInfo(System.Collections.Hashtable hs);

        #endregion 特殊勤务车辆接口

        #region 黑名单车辆接口

        /// <summary>
        /// 获得黑名单车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetblackList(string where,int num);
        /// <summary>
        /// 获得黑名单车辆信息总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetblackListCount(string where);
        /// <summary>
        /// 更新黑名单车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateBlacklistInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 删除黑名单车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteBlacklistInfo(System.Collections.Hashtable hs);

        #endregion 黑名单车辆接口

        #region 专项布控接口

        /// <summary>
        /// 获得专项布控信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable Getspecial(string where, int num);
        /// <summary>
        /// 获得专项布控信息总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetspecialCount(string where);
        /// <summary>
        /// 更新专项布控信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateSpecialInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 删除专项布控信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteSpecialInfo(System.Collections.Hashtable hs);

        #endregion 专项布控接口

        #region 流量报警接口

        /// <summary>
        /// 获得流量报警信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable Getport(string where, int num);
        /// <summary>
        /// 获得流量报警信息总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetportCount(string where);
        /// <summary>
        /// 更新流量报警信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdatePortInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 删除流量报警信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeletePortInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///根据卡口编号查询卡口方向
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetKakouDirection(string StationId);
        #endregion 流量报警接口

        #region 报警车辆接口

        /// <summary>
        ///获得最大最小报警事件及报警总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetAlarmMaxBjsj(string where);

        /// <summary>
        ///查询最新的报警时间
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetAlarmTempMaxBjsj(string where);

        /// <summary>
        ///查询报警数据
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        DataTable GetAlarmInfo(string field, string where, int startrow, int endrow);

        /// <summary>
        ///查询流量报警数据
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        DataTable GetFlowInfo(string field, string where, int startrow, int endrow);

        ///查询报警数据总数
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        DataTable GetAlarmInfoCount(string field, string where, int startrow, int endrow);

        ///查询流量报警数据总数
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        DataTable GetFlowInfoCount(string field, string where, int startrow, int endrow);

        /// <summary>
        ///获得报警统计数据
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetAlarmCount(string field, string where);

        /// <summary>
        /// 处理报警信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DealAlarmInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 处理报警信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DealFlowInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DealAlarm_PeccancyInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DealAlarm_PasscarInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///查询最新报警信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetAlarmMonitor(string where, string rownum);

        /// <summary>
        ///查询最新流量信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetFlowMonitor(string where, string rownum);

        #endregion 报警车辆接口

        #region 套牌分析接口

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetRepaclePlate(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateRepaclePlate(System.Collections.Hashtable hs);

        #endregion 套牌分析接口

        #region 区间信息

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPeccancyAreaMaxWfsj(string where);
        /// <summary>
        ///总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPeccancyAreaMaxWfsjCount(string where);
        /// <summary>
        ///查询区间违法信息
        /// </summary>
        /// <param name="field"></param>
        /// <param name="where"></param>
        /// <param name="startrow"></param>
        /// <param name="endrow"></param>
        /// <returns></returns>
        DataTable GetPeccancyAreaInfo(string field, string where, int startrow, int endrow);

        /// <summary>
        ///
        /// </summary>
        /// <param name="qjid"></param>
        /// <param name="date"></param>
        /// <param name="countType"></param>
        /// <returns></returns>
        DataTable AreaODCount(string qjid, string date, string countType);

        /// <summary>
        ///
        /// </summary>
        /// <param name="qjid"></param>
        /// <param name="date"></param>
        /// <param name="countType"></param>
        /// <returns></returns>
        DataTable AreaSpeedCount(string qjid, string date, string countType);

        /// <summary>
        ///
        /// </summary>
        /// <param name="date"></param>
        /// <param name="tim"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable AreaSpeedQuery(string date, string tim, string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="xh"></param>
        /// <returns></returns>
        int UnlockAreaOneInfo(string xh);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sdr"></param>
        /// <param name="sdsj"></param>
        /// <param name="lockAmount"></param>
        /// <returns></returns>
        int LockAreaPeccancy(string where, string sdr, string sdsj, int lockAmount);

        /// <summary>
        ///
        /// </summary>
        /// <param name="sdsj"></param>
        /// <param name="sdr"></param>
        /// <returns></returns>
        int UnAlllockAreaAll(string sdsj, string sdr);

        /// <summary>
        ///
        /// </summary>
        /// <param name="sdsj"></param>
        /// <param name="sdr"></param>
        /// <returns></returns>
        int UnlockAreaAll(string sdsj, string sdr);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sdr"></param>
        /// <param name="sdsj"></param>
        /// <returns></returns>
        int LockAreaPeccancy(string where, string sdr, string sdsj);

        /// <summary>
        ///更新区间违法数据
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int CheckAreaPeccancyInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 统计区间违法信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPeccancyAreaCount(string where);
        /// <summary>
        /// 统计区间违法信息（最新）
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPeccancyAreaCountNew(string where, int startNum, int endNum);
        /// <summary>
        /// 统计区间违法信息总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPeccancyAreaCountCount(string where);

        /// <summary>
        /// 区间违法行为统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPeccancyAreaCountForWfxw(string where);

        /// <summary>
        ///  区间行驶速度统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetPeccancyAreaCountForXssd(string where);

        #endregion 区间信息
          /// <summary>
        /// 得到一个Json字符串
        /// </summary>
        /// <returns></returns>
        DataTable GetJson(out DataTable dt);
    }
}