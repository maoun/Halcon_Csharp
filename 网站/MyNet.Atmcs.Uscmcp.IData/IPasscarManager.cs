using System.Data;

namespace MyNet.Atmcs.Uscmcp.IData
{
    public interface IPasscarManager
    {
        DataSet GetClpp();

        DataSet GetClxh(string clpp);

        DataSet GetFisrtIntoData(string starttime, string endtime, string hphm, string hpzl, int pageIndex, int tag);

        DataTable GetTpcl(string starttime, string endtime, string lisenceid);

        DataSet GetOneLisenceMulCarData(string starttime, string endtime, string lisenceid, int startrow, int endrow);

        int GetOneLisenceMulCarDataRows(string starttime, string endtime, string lisenceid);

        DataSet GetStation();

        DataSet GetOneCarMulLisenceData(string starttime, string endtime, string lisenceid, string similarity, string stationid);

        DataSet GetPassCarInfo(string starttime, string endtime, string lisenceid);

        DataSet GetSyscode(string where);

        DataSet GettFisrtIntoCount(string starttime, string endtime, string hphm, string hpzl, int tag);

        string GetStationPoint(string stationid);

        int UpdateSuspicionInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 插入到套牌车库（新表）
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateSuspicionInfoNew(System.Collections.Hashtable hs);

        /// <summary>
        /// 更新(插入)表t_tgs_suspect_recive
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateSuspicionInfoNewRecive(System.Collections.Hashtable hs);

        /// <summary>
        /// 判断该车辆是否存在
        /// </summary>
        /// <param name="hphm">号牌号码</param>
        /// <param name="hpzl">号牌种类</param>
        /// <returns></returns>
        DataTable GetBkbh(string hphm, string hpzl);
    }
}