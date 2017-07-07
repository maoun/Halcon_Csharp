using System.Data;
using System.Drawing;
using System.Collections.Generic;

namespace MyNet.Atmcs.Uscmcp.IData
{
    /// <summary>
    /// 接口层
    /// </summary>
    public interface IRoadManager
    {
        #region 成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(string ROADID);
        /// <summary>
        /// 增加一条数据
        /// </summary>
        bool Add(MyNet.DataAccess.Model.RoadInfo model);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        bool AddRoadSeg(System.Collections.Hashtable hs);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(MyNet.DataAccess.Model.RoadInfo model);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        bool UpdateRoadSeg(System.Collections.Hashtable hs);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool Delete(string ROADID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ROADID"></param>
        /// <returns></returns>
        bool DeleteRoadSeg(string ROADID);
        /// <summary>
        /// 批量删除道路信息
        /// </summary>
        /// <param name="roadidlist"></param>
        /// <returns></returns>
        bool DeleteList(string roadidlist);

        /// <summary>
        /// 获取所属辖区信息（组织机构）
        /// </summary>
        /// <returns></returns>
        DataTable GetDepartment();

        /// <summary>
        /// 查询获得道路列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        DataTable GetList(string strWhere);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        DataTable GetRoadSegList(string strWhere);

        /// <summary>
        /// 获取坐标点集
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        DataTable GetRoadPoints(string strWhere);

        /// <summary>
        /// 获取坐标点集
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        DataTable GetRoadSegPoints(string strWhere);

        /// <summary>
        /// 获取道路类型
        /// </summary>
        /// <returns></returns>
        DataTable GetRoadType();

        #endregion 成员方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xyzlist"></param>
        /// <param name="roadid"></param>
        /// <returns></returns>
        bool UpdateXyz(System.Collections.Generic.List<PointF> xyzlist, string roadid);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="ROADID"></param>
        /// <returns></returns>
        MyNet.DataAccess.Model.RoadInfo GetModel(string ROADID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        MyNet.DataAccess.Model.RoadInfo DataRowToModel(DataRow row);


        bool UpdateKeyRoad(string roadID);
        bool SetKeyRoad(List<string> roadIDs, string departid);
    }
}