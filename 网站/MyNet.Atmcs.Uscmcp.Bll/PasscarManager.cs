using MyNet.Atmcs.Uscmcp.IData;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class PasscarManager
    {
        private static readonly IPasscarManager dal = DALFactory.CreatePasscarManager();

        /// <summary>
        /// 方向字典
        /// </summary>
        /// <returns></returns>
        public DataTable GetFxcode()
        {
            DataSet ds = dal.GetSyscode("codetype='240025'");
            if (ds != null)
                return ds.Tables[0];
            else
                return null;
        }

        /// <summary>
        /// 初入城查询
        /// </summary>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="lisenceid">号牌号码</param>
        /// <returns></returns>
        public DataTable GetFisrtIntoData(string starttime, string endtime, string hphm, string hpzl, int pageIndex, int tag)
        {
            return Common.ChangColName(dal.GetFisrtIntoData(starttime, endtime, hphm, hpzl, pageIndex, tag).Tables[0]);
        }

        /// <summary>
        /// 得到初次入城总条数
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="hphm"></param>
        /// <param name="hpzl"></param>
        /// <returns></returns>
        public DataTable GettFisrtIntoCount(string starttime, string endtime, string hphm, string hpzl, int tag)
        {
            return Common.ChangColName(dal.GettFisrtIntoCount(starttime, endtime, hphm, hpzl, tag).Tables[0]);
        }

        /// <summary>
        /// 一牌多车分析
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="lisenceid"></param>
        /// <returns></returns>
        public DataSet GetOneLisenceMulCarData(string starttime, string endtime, string lisenceid, int startrow, int endrow)
        {
            return dal.GetOneLisenceMulCarData(starttime, endtime, lisenceid, startrow, endrow);
        }

        public int GetOneLisenceMulCarDataRows(string starttime, string endtime, string lisenceid)
        {
            return dal.GetOneLisenceMulCarDataRows(starttime, endtime, lisenceid);
        }

        public DataTable GetTpcl(string starttime, string endtime, string lisenceid)
        {
            return dal.GetTpcl(starttime, endtime, lisenceid);
        }

        /// <summary>
        /// 一车多牌分析
        /// </summary>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="lisenceid">号牌号码</param>
        /// <param name="similarity">相似度</param>
        /// <param name="stationid">卡口id</param>
        /// <returns></returns>
        public DataSet GetOneCarMulLisenceData(string starttime, string endtime, string lisenceid, string similarity, string stationid)
        {
            return dal.GetOneCarMulLisenceData(starttime, endtime, lisenceid, similarity, stationid);
        }

        /// <summary>
        /// 卡口信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetStation()
        {
            return dal.GetStation();
        }

        /// <summary>
        /// 卡口过往车辆信息
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="lisenceid"></param>
        /// <returns></returns>
        public DataSet GetPassCarInfo(string starttime, string endtime, string lisenceid)
        {
            return dal.GetPassCarInfo(starttime, endtime, lisenceid);
        }

        public DataSet GetCllx()
        {
            return dal.GetSyscode("codetype='140001'");
        }

        public DataSet GetCsys()
        {
            return dal.GetSyscode("codetype='240001'");
        }

        public DataSet GetClpp()
        {
            return dal.GetClpp();
        }

        public string GetStationPoint(string stationid)
        {
            return dal.GetStationPoint(stationid);
        }

        public int UpdateSuspicionInfo(System.Collections.Hashtable hs)
        {
            return dal.UpdateSuspicionInfo(hs);
        }

        /// <summary>
        /// 插入到套牌车库（新表）
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateSuspicionInfoNew(System.Collections.Hashtable hs)
        {
            return dal.UpdateSuspicionInfoNew(hs);
        }

        /// <summary>
        /// 更新(插入)表t_tgs_suspect_recive
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateSuspicionInfoNewRecive(System.Collections.Hashtable hs)
        {
            return dal.UpdateSuspicionInfoNewRecive(hs);
        }

        /// <summary>
        /// 判断该车辆是否存在
        /// </summary>
        /// <param name="hphm">号牌号码</param>
        /// <param name="hpzl">号牌种类</param>
        /// <returns></returns>
        public DataTable GetBkbh(System.Collections.Hashtable hs)
        {
            return dal.GetBkbh(hs["hphm"].ToString(), hs["hpzl"].ToString());
        }
    }
}