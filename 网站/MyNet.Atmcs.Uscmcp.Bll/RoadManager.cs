using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    /// <summary>
    /// T_GIS_ROAD道路信息管理
    /// </summary>
    public partial class RoadManager
    {
        #region 成员变量

        private readonly IRoadManager dal = DALFactory.CreateRoadManager();

        #endregion 成员变量

        #region BasicMethod

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DataTable GetDepartment()
        {
            try
            {
                return dal.GetDepartment();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取道路类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetRoadType()
        {
            try
            {
                return dal.GetRoadType();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string ROADID)
        {
            try
            {
                return dal.Exists(ROADID);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xyzlist"></param>
        /// <param name="roadid"></param>
        /// <returns></returns>
        public bool UpdateXyz(List<PointF> xyzlist, string roadid)
        {
            try
            {
                return dal.UpdateXyz(xyzlist, roadid);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string ROADID)
        {
            try
            {
                return dal.Delete(ROADID);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 批量删除道路信息
        /// </summary>
        /// <param name="roadidlist"></param>
        /// <returns></returns>
        public bool DeleteList(string roadidlist)
        {
            try
            {
                return dal.DeleteList(roadidlist);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 查询获得道路列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetList(string strWhere)
        {
            try
            {
                return dal.GetList(strWhere);
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
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetRoadSegList(string strWhere)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取坐标点集
        /// </summary>
        /// <param name="strwhere"></param>
        /// <returns></returns>
        public DataTable GetRoadPoints(string strwhere)
        {
            try
            {
                return dal.GetRoadPoints(strwhere);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取坐标点集
        /// </summary>
        /// <param name="strwhere"></param>
        /// <returns></returns>
        public DataTable GetRoadSegPoints(string strwhere)
        {
            try
            {
                return dal.GetRoadSegPoints(strwhere);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            try
            {
                return GetList("");
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
        public bool UpdateRoadSeg(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateRoadSeg(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public bool AddRoadSeg(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.AddRoadSeg(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="roadid"></param>
        /// <returns></returns>
        public bool DeleteRoadSeg(string roadid)
        {
            try
            {
                return dal.DeleteRoadSeg(roadid);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }

        #endregion BasicMethod
        /// <summary>
        /// 更新道路信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(DataAccess.Model.RoadInfo model)
        {
            try
            {
                return dal.Update(model);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }
        /// <summary>
        /// 添加道路信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(DataAccess.Model.RoadInfo model)
        {
            try
            {
                return dal.Add(model);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataAccess.Model.RoadInfo GetModel(string id)
        {
            try
            {
                return dal.GetModel(id);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public bool SetKeyRoad(List<string> roadIDs, string departid)
        {
            try
            {
                return dal.SetKeyRoad(roadIDs,departid);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }
        public bool UpdateKeyRoad(string roadID)
        {
            try
            {
                return dal.UpdateKeyRoad(roadID);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }
    }
}