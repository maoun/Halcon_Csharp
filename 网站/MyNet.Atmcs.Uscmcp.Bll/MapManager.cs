/***********************************************************************
 * Module:   目录业务逻辑类
 * Author:   郭永利
 * Modified: 2016年04月08日
 * Purpose:  该类为页面提供研判需要的业务逻辑方法
 ***********************************************************************/

using MyNet.Atmcs.Uscmcp.IData;
using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class MapManager
    {
        #region

        private static readonly IMapManager dal = DALFactory.CreateMapManager();

        #endregion

        #region

        /// <summary>
        /// 判断该车辆是否存在
        /// </summary>
        /// <param name="hphm">号牌号码</param>
        /// <param name="hpzl">号牌种类</param>
        /// <returns></returns>
        public DataTable GetBkbh(string hphm, string hpzl)
        {
            return dal.GetBkbh(hphm, hpzl);
        }

        /// <summary>
        /// 更新(插入)表t_tgs_suspect_recive
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateDispatchNewRecive(string bkbh, string sjly)
        {
            return dal.UpdateDispatchNewRecive(bkbh, sjly);
        }

        /// <summary>
        /// 一键布控(更新到新表)
        /// </summary>
        /// <param name="stationid">卡口id集合</param>
        /// <param name="hphm">号牌号码</param>
        /// <param name="cllx">车辆类型</param>
        /// <param name="bdyy">比对原因</param>
        /// <param name="yxsj">有效时间</param>
        /// /// <param name="bkr">布控人</param>
        /// <returns></returns>
        public int SetDispatchNew(List<string> stationid, string hphm, string cllx, string bdyy, string yxsj, string sjly, string bkr, string xh, string mdlx)
        {
            return dal.SetDispatchNew(stationid, hphm, cllx, bdyy, yxsj, sjly, bkr, xh, mdlx);
        }

        /// <summary>
        ///  MAP管理接口
        /// </summary>
        public DataSet GetPathCar(string startdate, string enddate, string carid, string cartype)
        {
            return dal.GetPathCar(startdate, enddate, carid, cartype);
        }

        /// <summary>
        ///  获取车辆类型
        /// </summary>
        /// <returns></returns>
        public DataSet GetCllx()
        {
            return dal.GetSyscode("codetype='140001'");
        }

        /// <summary>
        /// 获取车辆品牌
        /// </summary>
        /// <returns></returns>
        public DataSet GetClpp()
        {
            return dal.GetClpp();
        }

        /// <summary>
        /// 获取车辆型号
        /// </summary>
        /// <param name="clpp">车辆品牌</param>
        /// <returns></returns>
        public DataSet GetClxh(string clpp)
        {
            return dal.GetClxh(clpp);
        }

        /// <summary>
        /// 获取车身颜色
        /// </summary>
        /// <returns></returns>
        public DataSet GetCsys()
        {
            return dal.GetSyscode("codetype='240013'");
        }

        /// <summary>
        /// 获取所属机构
        /// </summary>
        /// <returns></returns>
        public DataSet GetDepart()
        {
            return dal.GetDepart("");
        }

        public DataTable GetCarstate()
        {
            DataSet ds = dal.GetSyscode("codetype='240045'");
            if (ds != null)
                return ds.Tables[0];
            else
                return null;
        }

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
        /// 卡口字典
        /// </summary>
        /// <returns></returns>
        public DataTable GetStation()
        {
            DataSet ds = dal.GetStation("");
            if (ds != null)
                return ds.Tables[0];
            else
                return null;
        }
        public DataTable GetStationBy(string where)
        {
            DataSet ds = dal.GetStation(where);
            if (ds != null)
                return ds.Tables[0];
            else
                return null;
        }
        /// <summary>
        /// 违法多放点分析
        /// </summary>
        /// <param name="departid">机构id</param>
        /// <param name="zqlx">周期类型</param>
        /// <param name="nd">年度</param>
        /// <returns></returns>
        public DataSet GetIllegalAnalyze(string departid, string zqlx, string nd,string topnum)
        {
            return dal.GetIllegalAnalyze(departid, zqlx, nd,topnum);
        }
        public DataTable GetIllegalDetail(string zqlx, string nd, string kkid)
        {
            return dal.GetIllegalDetail(zqlx, nd, kkid);
        }

        /// <summary>
        /// 一键布控
        /// </summary>
        /// <param name="stationid">卡口id集合</param>
        /// <param name="hphm">号牌号码</param>
        /// <param name="cllx">车辆类型</param>
        /// <param name="bdyy">比对原因</param>
        /// <param name="yxsj">有效时间</param>
        /// <param name="bkry">布控人员姓名</param>
        /// <param name="lxdh">布控联系人电话</param>
        ///<param name="cpmh">车牌模糊</param>
        /// <returns></returns>
        public string SetDispatch(List<string> stationid, string hphm, string cllx, string bdyy, string yxsj, string sjly, string mdlx, string bkry, string lxdh, string cpmh, string bklx)
        {
            return dal.SetDispatch(stationid, hphm, cllx, bdyy, yxsj, sjly, mdlx, bkry, lxdh, cpmh, bklx);
        }

        /// <summary>
        /// 获取车辆热点（道路）
        /// </summary>
        /// <returns></returns>
        public DataSet GetCarHotRoad()
        {
            return dal.GetCarHotRoad();
        }

        public DataTable GetCarHotRoad(string time)
        {
            return dal.GetCarHotRoad(time);
        }

        /// <summary>
        /// 获取车辆热点（卡口）
        /// </summary>
        /// <returns></returns>
        public DataTable GetCarHotStation(string time)
        {
            return dal.GetCarHotStation(time);
        }

        /// <summary>
        /// 获取道路坐标位置
        /// </summary>
        /// <param name="roadid">道路id</param>
        /// <returns></returns>
        public string GetRoadPoint(string roadid)
        {
            return dal.GetRoadPoint(roadid);
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="type">设备类型</param>
        /// <returns></returns>
        public DataSet GetDeviceByType(string type)
        {
            return dal.GetDeviceByType(type);
        }

        /// <summary>
        /// 卡口流量统计
        /// </summary>
        /// <param name="kkid">卡口id</param>
        /// <param name="flowtime">统计时间</param>
        /// <returns></returns>
        public DataTable GetFlowByStation(string kkid, string flowtime)
        {
            return dal.GetFlowByStation(kkid, flowtime);
        }

        /// <summary>
        /// 道路流量统计
        /// </summary>
        /// <param name="dlid">道路id</param>
        /// <param name="flowtime">统计时间</param>
        /// <returns></returns>
        public DataTable GetFlowByRoad(string dlid, string flowtime)
        {
            return dal.GetFlowByRoad(dlid, flowtime);
        }

        public string GetFlowByRoadCount(string dlid, string flowtime)
        {
            return dal.GetFlowByRoadCount(dlid, flowtime);
        }

        public DataTable GetFlowByRoadAvg(string dlid, string flowtime)
        {
            return dal.GetFlowByRoadAvg(dlid, flowtime);
        }

        public DataTable GetWorkStatic(string kkid, string time)
        {
            return dal.GetWorkStatic(kkid, time);
        }

        #endregion
    }
}