using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class RoadManager : IRoadManager
    {
        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        private MyNet.Common.Data.DataAccess dataAccess;

        public RoadManager()
        {
            //dataAccess = dac.GetDataAccess(WebConfig.DataAccessName);
            dataAccess = GetDataAccess.Init();
        }

        public RoadManager(string dataAccessName)
        {
            dataAccess = dac.GetDataAccess(dataAccessName);
        }

        #region IRoadManager 成员

        /// <summary>
        /// 增加一条道路数据
        /// </summary>
        public bool Add(MyNet.DataAccess.Model.RoadInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_GIS_ROAD(ROADID,DLBH,DLMC,DLLX,SSXQ,GXSJ,ISMARK) values (");
            strSql.Append("'" + model.ROADID + "','" + model.DLBH + "','" + model.DLMC + "','" + model.DLLX + "','" + model.SSXQ + "',str_to_date('" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s'),'" + model.ISMARK + "')");

            int rows = dataAccess.Execute_NonQuery(strSql.ToString());
            if (rows > 0)
            {
                if (model.XYZPOINT != null && model.XYZPOINT.Count > 0)
                {
                    return AddXyz(model.XYZPOINT, model.ROADID);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 增加坐标信息
        /// </summary>
        /// <param name="xyzlist">坐标集</param>
        /// <param name="ROADID">道路ID</param>
        /// <returns></returns>
        public bool AddXyz(List<PointF> xyzlist, string roadid)
        {
            for (int i = 0; i < xyzlist.Count; i++)
            {
                if (InsertXyz(xyzlist[i], roadid, i))
                    continue;
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 增加xyz坐标
        /// </summary>
        /// <param name="xyz"></param>
        /// <param name="roadid"></param>
        /// <returns></returns>
        public bool InsertXyz(PointF xyz, string roadid, int num)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into t_gis_road_points(");
            strSql.Append("ID,ROADID,XPOS,YPOS,ZPOS,ORDERID) values (");
            strSql.Append("'" + Common.GetRecordId() + "','" + roadid + "'," + xyz.X + "," + xyz.Y + ",0," + num + ")");

            int rows = dataAccess.Execute_NonQuery(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddRoadInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  T_GIS_ROAD_SEG (ID,DLBH,LDMC,LDFX,GXSJ) values(";
                mySql = mySql + "'" + hs["ID"].ToString() + "',";
                mySql = mySql + "'" + hs["DLBH"].ToString() + "',";
                mySql = mySql + "'" + hs["LDMC"].ToString() + "',";
                mySql = mySql + "'" + hs["LDFX"].ToString() + "',";
                mySql = mySql + "sysdate )";
                int rows = dataAccess.Execute_NonQuery(mySql);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return false;
            }
        }

        public bool AddRoadSegInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  T_GIS_ROAD_SEG (ID,DLBH,LDMC,LDFX,GXSJ) values(";
                mySql = mySql + "'" + hs["ID"].ToString() + "',";
                mySql = mySql + "'" + hs["DLBH"].ToString() + "',";
                mySql = mySql + "'" + hs["LDMC"].ToString() + "',";
                mySql = mySql + "'" + hs["LDFX"].ToString() + "',";
                mySql = mySql + "now() )";
                int rows = dataAccess.Execute_NonQuery(mySql);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(MyNet.DataAccess.Model.RoadInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update T_GIS_ROAD set ");
            strSql.Append("DLBH= '" + model.DLBH + "',");
            strSql.Append("DLMC='" + model.DLMC + "',");
            strSql.Append("DLLX='" + model.DLLX + "',");
            strSql.Append("SSXQ='" + model.SSXQ + "',");
            strSql.Append("GXSJ=now(),");
            strSql.Append("ISMARK= '" + model.ISMARK + "'");
            strSql.Append(" where ROADID='" + model.ROADID + "' ");

            int rows = dataAccess.Execute_NonQuery(strSql.ToString());
            if (rows > 0)
            {
                return true;
                // return UpdateXyz(model.XYZPOINT, model.ROADID);
            }
            else
            {
                return false;
            }
        }

        public bool UpdateRoadInfo(System.Collections.Hashtable hs)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRoadSeg(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " update T_GIS_ROAD_SEG set ";
                mySql = mySql + "DLBH='" + hs["DLBH"].ToString() + "',";
                mySql = mySql + "LDMC='" + hs["LDMC"].ToString() + "',";
                mySql = mySql + "LDFX='" + hs["LDFX"].ToString() + "',";
                mySql = mySql + "GXSJ=now() ";
                mySql = mySql + "  where ID='" + hs["ID"].ToString() + "'";
                int rows = dataAccess.Execute_NonQuery(mySql);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return false;
            }
        }

        public bool DeleteRoadInfo(string roadid)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                DelRoadPoints(roadid);
                strSql.Append("delete from t_gis_road_seg ");
                strSql.Append(" where ID='" + roadid + "'");
                int rows = dataAccess.Execute_NonQuery(strSql.ToString());
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql.ToString() + ex.Message);
                return false;
            }
        }

        public bool DelRoadPoints(string roadid)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("delete from t_gis_road_points ");
                strSql.Append(" where roadid='" + roadid + "'");
                int rows = dataAccess.Execute_NonQuery(strSql.ToString());
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql.ToString() + ex.Message);
                return false;
            }
        }

        public bool DeleteRoadSeg(string roadid)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                DelRoadPoints(roadid);
                strSql.Append("delete from t_gis_road_seg ");
                strSql.Append(" where id='" + roadid + "'");
                int rows = dataAccess.Execute_NonQuery(strSql.ToString());
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql.ToString() + ex.Message);
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from t_gis_road ");
            strSql.Append(" where roadid in (" + roadidlist + ")  ");
            int rows = dataAccess.Execute_NonQuery(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable GetRoadInfoList(string strWhere)
        {
            throw new NotImplementedException();
        }

        public DataTable GetRoadSegList(string strWhere)
        {
            string sql = "select  distinct a.id, f_get_value ('dlmc','t_gis_road','roadid',  dlbh)  as dlmc,ldmc,decode(ldfx,'0','下行','1','上行') as fxmc,gxsj,ldfx,dlbh,f_get_value ('dllx','t_gis_road','roadid',  dlbh)  as dllx from t_gis_road_seg a where " + strWhere;

            return dataAccess.Get_DataTable(sql);
        }

        public DataTable GetRoadPoints(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.*,B.DLMC,B.SSXQ from T_GIS_ROAD_POINTS a inner join T_GIS_ROAD b on a.roadid=b.roadid where a.ROADID in (select ROADID FROM T_GIS_ROAD");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere + ") order by a.roadid,orderid");
            }
            else
            {
                strSql.Append(") order by a.roadid,orderid");
            }
            return dataAccess.Get_DataTable(strSql.ToString());
        }

        public DataTable GetRoadSegPoints(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select a.*,B.LDMC from T_GIS_ROAD_POINTS a inner join T_GIS_ROAD_SEG b on a.roadid=b.id where a.ROADID in (select ROADID FROM T_GIS_ROAD_SEG");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere + ") order by a.roadid,orderid");
            }
            else
            {
                strSql.Append(") order by a.roadid,orderid");
            }
            return dataAccess.Get_DataTable(strSql.ToString());
        }

        /// <summary>
        /// 查询道路类型
        /// </summary>
        /// <returns></returns>
        public DataTable GetRoadType()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from t_sys_code where codetype='350100' ");
            return dataAccess.Get_DataTable(strSql.ToString());
        }

        public bool UpdateRoadPointXyz(List<System.Drawing.PointF> xyzlist, string roadid)
        {
            throw new NotImplementedException();
        }

        #endregion IRoadManager 成员

        #region IRoadManager 成员

        public bool Exists(string roadid)
        {
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append("select count(1) from t_gis_road");
                strSql.Append(" where roadid='" + roadid + "' ");
                int res = int.Parse(dataAccess.Get_DataString(strSql.ToString(), 0));
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(strSql.ToString() + ex.Message);
                return false;
            }
        }

        public bool DelXyzpoint(string roadid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from t_gis_road_points ");
            strSql.Append(" where roadid='" + roadid + "'");
            int rows = dataAccess.Execute_NonQuery(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddRoadSeg(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  T_GIS_ROAD_SEG (ID,DLBH,LDMC,LDFX,GXSJ) values(";
                mySql = mySql + "'" + hs["ID"].ToString() + "',";
                mySql = mySql + "'" + hs["DLBH"].ToString() + "',";
                mySql = mySql + "'" + hs["LDMC"].ToString() + "',";
                mySql = mySql + "'" + hs["LDFX"].ToString() + "',";
                mySql = mySql + "sysdate )";
                int rows = dataAccess.Execute_NonQuery(mySql);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return false;
            }
        }

        public bool Delete(string ROADID)
        {
            DelXyzpoint(ROADID);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_GIS_ROAD ");
            strSql.Append(" where ROADID='" + ROADID + "'");
            int rows = dataAccess.Execute_NonQuery(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public System.Data.DataTable GetDepartment()
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from t_cfg_department order by classcode ");
                return dataAccess.Get_DataTable(strSql.ToString());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 查询获得道路列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public System.Data.DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ROADID,DLBH,DLMC,DLLX,SSXQ,GXSJ,ISMARK,B.CODEDESC DLLXNAME,C.DEPARTNAME SSXQNAME ,c.departId ,a.IsKeyRoad  FROM T_GIS_ROAD a ");
            strSql.Append("inner join (select * from t_sys_code where codetype='350100' and isuse='1') b on A.DLLX=b.CODE ");
            strSql.Append(" inner join t_cfg_department c on A.SSXQ=C.DEPARTID ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return dataAccess.Get_DataTable(strSql.ToString());
        }

        public bool UpdateXyz(List<System.Drawing.PointF> xyzlist, string roadid)
        {
            DelXyzpoint(roadid);
            return AddXyz(xyzlist, roadid);
        }

        #endregion IRoadManager 成员

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public MyNet.DataAccess.Model.RoadInfo GetModel(string roadid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select roadid,dlbh,dlmc,dllx,ssxq,gxsj,ismark from t_gis_road ");
            strSql.Append(" where roadid='" + roadid + "'");

            MyNet.DataAccess.Model.RoadInfo model = new MyNet.DataAccess.Model.RoadInfo();
            DataSet ds = dataAccess.Get_DataSet(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public MyNet.DataAccess.Model.RoadInfo DataRowToModel(DataRow row)
        {
            MyNet.DataAccess.Model.RoadInfo model = new MyNet.DataAccess.Model.RoadInfo();
            if (row != null)
            {
                if (row["ROADID"] != null)
                {
                    model.ROADID = row["ROADID"].ToString();
                }
                if (row["DLBH"] != null)
                {
                    model.DLBH = row["DLBH"].ToString();
                }
                if (row["DLMC"] != null)
                {
                    model.DLMC = row["DLMC"].ToString();
                }
                if (row["DLLX"] != null)
                {
                    model.DLLX = row["DLLX"].ToString();
                }
                if (row["SSXQ"] != null)
                {
                    model.SSXQ = row["SSXQ"].ToString();
                }
                if (row["GXSJ"] != null && row["GXSJ"].ToString() != "")
                {
                    model.GXSJ = DateTime.Parse(row["GXSJ"].ToString());
                }
                if (row["ISMARK"] != null)
                {
                    model.ISMARK = row["ISMARK"].ToString();
                }
            }
            return model;
        }

        public bool SetKeyRoad(List<string> roadIDs, string departid)
        {
            try
            {
                string roadid = "";
                if (dataAccess.Execute_NonQuery("UPDATE t_gis_road SET iskeyroad='0' where ssxq='" + departid + "'") > 0)
                {
                    foreach (string item in roadIDs)
                    {
                        roadid += (roadid == "" ? "" : ",") + "'" + item + "'";
                    }
                    string mySql = " UPDATE t_gis_road SET iskeyroad='1' WHERE roadid in (" + roadid + ") ";

                    int rows = dataAccess.Execute_NonQuery(mySql);
                    if (rows > 0)
                    {
                        return true;
                    }
                }

                return false;
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
                string mySql = " UPDATE t_gis_road SET iskeyroad='1' WHERE roadid='" + roadID + "' ";

                int rows = dataAccess.Execute_NonQuery(mySql);
                if (rows > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }
    }
}