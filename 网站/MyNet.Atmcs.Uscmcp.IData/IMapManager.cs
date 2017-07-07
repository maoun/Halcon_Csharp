using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.IData
{
    public interface IMapManager
    {
        DataSet GetClpp();

        DataSet GetClxh(string clpp);

        DataSet GetDeviceByType(string type);

        DataSet GetStation(string where);

        DataSet GetSyscode(string where);

        DataSet GetPathCar(string startdate, string enddate, string carid, string cartype);

        DataSet GetDepart(string strwhere);

        DataSet GetIllegalAnalyze(string departid, string zqlx, string nd,string topnum);

        DataTable GetIllegalDetail( string zqlx, string nd, string kkid);
        string SetDispatch(List<string> stationid, string hphm, string cllx, string bdyy, string yxsj, string sjly, string mdlx, string bkry, string lxdh, string cpmh, string bklx);

        DataSet GetCarHotRoad();

        DataTable GetCarHotRoad(string time);

        DataTable GetCarHotStation(string time);

        string GetRoadPoint(string roadid);

        DataTable GetFlowByRoad(string dlid, string flowtime);

        string GetFlowByRoadCount(string dlid, string flowtime);

        DataTable GetFlowByRoadAvg(string dlid, string flowtime);

        DataTable GetFlowByStation(string kkid, string flowtime);

        DataTable GetWorkStatic(string kkid, string time);

        /// <summary>
        /// 布控数据到新的表
        /// </summary>
        /// <param name="stationid"></param>
        /// <param name="hphm"></param>
        /// <param name="cllx"></param>
        /// <param name="bdyy"></param>
        /// <param name="yxsj"></param>
        /// <param name="sjly"></param>
        /// <returns></returns>
        int SetDispatchNew(List<string> stationid, string hphm, string cllx, string bdyy, string yxsj, string sjly, string bkr, string xh, string mdlx);

        /// <summary>
        /// 更新(插入)表t_tgs_suspect_recive
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateDispatchNewRecive(string bkbh, string sjly);

        /// <summary>
        /// 判断该车辆是否存在
        /// </summary>
        /// <param name="hphm">号牌号码</param>
        /// <param name="hpzl">号牌种类</param>
        /// <returns></returns>
        DataTable GetBkbh(string hphm, string hpzl);
    }
}