using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.IData
{
    public interface IStationManager
    {
        //监测点信息增，删，改，查
        DataTable GetStationTypeInfo(string where);

        DataTable GetStationInfo(string where);

        DataTable GetlocationByStationInfo(string where);

        DataTable GetRecordPlayByStationInfo();

        DataTable GetStationInfoView(string where);

        int UpdateStationInfo(System.Collections.Hashtable hs);

        int InsertStationDevice(string stationid, List<string> deviceList);

        int DeleteStationInfo(System.Collections.Hashtable hs);

        int InsertStationInfo(System.Collections.Hashtable hs);

        int DeleteStationByLocation(System.Collections.Hashtable hs);

        int DeleteDeviceStation(System.Collections.Hashtable hs);

        DataTable GetLaneInfo(string where);

        DataTable GetLaneInfoView(string where);

        int UpdateLaneInfo(System.Collections.Hashtable hs);

        int UpdateLaneInfo(string deviceid, string directionid);

        int DeleteLaneInfo(System.Collections.Hashtable hs);

        int InsertLaneInfo(System.Collections.Hashtable hs);

        int InsertLaneInfo(System.Collections.Hashtable hs, int startLane, int endLane);

        int DeleteLaneInfoByDirection(string station_id, string direction_id);

        DataTable GetEndStationInfo(string kskkid);

        DataTable GetEndStationDict(string kskkid);

        DataTable GetStartStationInfo();

        DataTable GetUserStationInfo(string userid, string shjb);

        DataTable GetNoUserStationInfo(string shjb);

        int InsertUserStationInfo(System.Collections.Hashtable hs);

        int DeleteUserStationInfo(System.Collections.Hashtable hs);
    }
}