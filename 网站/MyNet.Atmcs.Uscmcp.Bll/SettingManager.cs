/***********************************************************************
 * Module:   目录业务逻辑类
 * Author:   李建平
 * Modified: 2008年10月17日
 * Purpose:  该类为页面提供需要的业务逻辑方法
 ***********************************************************************/

using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class SettingManager
    {
        #region 成员变量

        /// <summary>
        ///  用户操作接口
        /// </summary>
        private static readonly ISettingManager dal = DALFactory.CreateSettingManager();

        #endregion 成员变量

        /// <summary>
        /// 获得当前系统编号
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserSettingContent(string systemId, string formtype, string usercode)
        {
            try
            {
                DataTable dt = dal.GetUserSettingContent(systemId, formtype, usercode);
                return Bll.Common.ChangColName(dt);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得当前系统编号
        /// </summary>
        /// <returns></returns>
        public DataTable GetSettingContent(string systemId, string formtype)
        {
            try
            {
                DataTable dt = dal.GetSettingContent(systemId, formtype);
                return Bll.Common.ChangColName(dt);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得当前用户主功能菜单
        /// </summary>
        /// <param name="formtype"></param>
        /// <returns></returns>
        public DataTable GetSettingContent(string formtype)
        {
            try
            {
                DataTable dt = dal.GetSettingContent(formtype);
                return Bll.Common.ChangColName(dt);
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
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetSettingCodeType(string systemId)
        {
            try
            {
                DataTable dt = dal.GetSettingCodeType(systemId);
                return Bll.Common.ChangColName(dt);
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
        /// <param name="systemId"></param>
        /// <param name="contentid"></param>
        /// <param name="formtype"></param>
        /// <returns></returns>
        public DataTable GetSettingContent(string systemId, string contentid, string formtype)
        {
            try
            {
                DataTable dt = dal.GetSettingContent(systemId, contentid, formtype);
                return Bll.Common.ChangColName(dt);
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
        public int DeleteSettingContent(Hashtable hs)
        {
            try
            {
                return dal.DeleteSettingContent(hs);
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
        public int UpdateSettingContent(Hashtable hs)
        {
            try
            {
                return dal.UpdateSettingContent(hs);
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
        public int AddSettingContent(Hashtable hs)
        {
            try
            {
                return dal.AddSettingContent(hs);
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
        /// <param name="systemId"></param>
        /// <param name="contentid"></param>
        /// <param name="formType"></param>
        /// <returns></returns>
        public DataTable GetContentFunction(string systemId, string contentid, string formType)
        {
            try
            {
                DataTable dt = dal.GetContentFunction(systemId, contentid, formType);
                return Bll.Common.ChangColName(dt);
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
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetFreeContentFunc(string systemId)
        {
            try
            {
                DataTable dt = dal.GetFreeContentFunc(systemId);
                return Bll.Common.ChangColName(dt);
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
        public int AddContentFunction(Hashtable hs)
        {
            try
            {
                return dal.AddContentFunction(hs);
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
        public int DelContentFunction(Hashtable hs)
        {
            try
            {
                return dal.DelContentFunction(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///获得配置信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetConfigInfo(string systemId)
        {
            try
            {
                DataTable dt = dal.GetConfigInfo(systemId);
                return Bll.Common.ChangColName(dt);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得配置信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        public DataTable GetConfigInfo(string systemId, string configId)
        {
            try
            {
                DataTable dt = dal.GetConfigInfo(systemId, configId);
                return Bll.Common.ChangColName(dt);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 删除配置信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteConfigInfo(Hashtable hs)
        {
            try
            {
                return dal.DeleteConfigInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///更新配置信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateConfigInfo(Hashtable hs)
        {
            try
            {
                return dal.UpdateConfigInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///添加配置信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int AddConfigInfo(Hashtable hs)
        {
            try
            {
                return dal.AddConfigInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///查询部门信息
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetDepartment(string deptcode)
        {
            return Bll.Common.ChangColName(dal.GetDepartment(" 1=1 and systemid='00' and departid='" + deptcode + "'"));
        }

        /// <summary>
        ///查询下级部门信息
        /// </summary>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetLowerDepartment(string deptcode)
        {
            return Bll.Common.ChangColName(dal.GetDepartment(" 1=1 and systemid='00' and classcode='" + deptcode + "'"));
        }

        /// <summary>
        ///查询部门信息
        /// </summary>
        /// <param name="systemid"></param>
        /// <param name="xzjb"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetConfigDepartment(string systemid, string xzjb, string deptcode)
        {
            try
            {
                if (xzjb == "1")
                {
                    return Bll.Common.ChangColName(dal.GetDepartment(" 1=1 and systemid='" + systemid + "' "));
                }
                if (xzjb == "2")
                {
                    return Bll.Common.ChangColName(dal.GetDepartment("systemid='" + systemid + "' and substr(departid,0,4) = '" + deptcode.Substring(0, 4) + "'"));
                }
                else if (xzjb == "3")
                {
                    return Bll.Common.ChangColName(dal.GetDepartment("systemid='" + systemid + "' and substr(departid,0,8) = '" + deptcode.Substring(0, 8) + "'"));
                }
                else
                {
                    return Bll.Common.ChangColName(dal.GetDepartment("systemid='" + systemid + "' and departid = '" + deptcode + "'"));
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///查询部门信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetConfigDepartment(string systemId)
        {
            try
            {
                DataTable dt= dal.GetConfigDepartment(systemId);
                return Bll.Common.ChangColName(dt);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///查询部门信息
        /// </summary>
        /// <param name="departid"></param>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetConfigDepartmentInfo(string departid, string systemId)
        {
            try
            {
                DataTable dt = dal.GetConfigDepartmentInfo(departid, systemId);
                return Bll.Common.ChangColName(dt);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///查询部门信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetDepartmentDict(string systemId)
        {
            try
            {
                DataTable dt = dal.GetConfigDepartment("departid,departname", systemId);
                return Bll.Common.ChangColName(dt);
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
        /// <param name="systemId"></param>
        /// <returns></returns>
        public string GetSettingRegionId(string systemId)
        {
            try
            {
                return dal.GetSettingConfigValue(systemId, "01");
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
        /// <param name="systemId"></param>
        /// <returns></returns>
        public string GetSettingRegionName(string systemId)
        {
            try
            {
                return dal.GetSettingConfigValue(systemId, "02");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "";
            }
        }
        /// <summary>
        ///获得公告图片发布路径
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public string GetSettingPublicPath(string systemId)
        {
            try
            {
                return dal.GetSettingConfigValue(systemId, "101");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "";
            }
        }
        /// <summary>
        /// 查询字典信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="codeId"></param>
        /// <returns></returns>
        public DataTable getDictData(string systemId, string codeId)
        {
            try
            {
                DataTable dt = dal.GetSettingCode(systemId, codeId);

                return dt;
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
                DataTable dt = dal.GetVehicleBrand(where);

                return dt;
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
                DataTable dt = dal.GetVehicleModel(where);

                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        #region 角色权限接口

        /// <summary>
        /// 获得权限信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSerPrivInfo(string systemId, string where)
        {
            try
            {
                return Common.ChangColName(dal.GetSerPrivInfo(systemId, where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 删除权限信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteSerPrivInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteSerPrivInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 更新权限信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateSerPrivInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateSerPrivInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 插入权限信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertSerPrivInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.InsertSerPrivInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 查询权限对应功能信息
        /// </summary>
        /// <param name="privcode"></param>
        /// <returns></returns>
        public DataTable GetSerPrivFunc(string privcode)
        {
            try
            {
                return Common.ChangColName(dal.GetSerPrivFunc(privcode));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得角色信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSerRoleInfo(string systemId, string where)
        {
            try
            {
                return Common.ChangColName(dal.GetSerRoleInfo(systemId, where));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///删除角色信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteSerRoleInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteSerRoleInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///更新角色信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateSerRoleInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateSerRoleInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 插入角色信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertSerRoleInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.InsertSerRoleInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///查询角色权限信息
        /// </summary>
        /// <param name="rolecode"></param>
        /// <returns></returns>
        public DataTable GetSerRolePriv(string rolecode)
        {
            try
            {
                return Common.ChangColName(dal.GetSerRolePriv(rolecode));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///插入权限功能信息
        /// </summary>
        /// <param name="privcode"></param>
        /// <param name="funcids"></param>
        /// <returns></returns>
        public int InsertPrivFunc(string privcode, List<string> funcids)
        {
            try
            {
                return dal.InsertPrivFunc(privcode, funcids);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///查询角色权限信息
        /// </summary>
        /// <param name="rolecode"></param>
        /// <param name="privcodes"></param>
        /// <returns></returns>
        public int InsertRolePriv(string rolecode, List<string> privcodes)
        {
            try
            {
                return dal.InsertRolePriv(rolecode, privcodes);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///获得用户角色信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public DataTable GetSerUserRole(string usercode)
        {
            try
            {
                return Common.ChangColName(dal.GetSerUserRole(usercode));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///插入用户角色信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="rolecode"></param>
        /// <returns></returns>
        public int InsertUserRole(string usercode, string rolecode)
        {
            try
            {
                return dal.InsertUserRole(usercode, rolecode);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        #endregion 角色权限接口

        #region 部门接口

        /// <summary>
        /// 删除部门信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteDepartmentInfo(Hashtable hs)
        {
            try
            {
                return dal.DeleteDepartmentInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///更新部门信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateDepartmentInfo(Hashtable hs)
        {
            try
            {
                return dal.UpdateDepartmentInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///查询部门信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetDepartmentByWhere(string where)
        {
            return Bll.Common.ChangColName(dal.GetDepartment(where));
        }

        #endregion 部门接口

        #region 地点方向信息接口

        /// <summary>
        ///查询方向信息
        /// </summary>
        /// <param name="location_id"></param>
        /// <returns></returns>
        public DataTable GetDirectionInfo(string location_id)
        {
            try
            {
                return Common.ChangColName(dal.GetDirectionInfo(" location_id ='" + location_id + "'"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///查询地点信息
        /// </summary>
        /// <param name="departId"></param>
        /// <returns></returns>
        public DataTable GetLocationInfoByDepartId(string departId)
        {
            try
            {
                return Common.ChangColName(dal.GetAllLocationInfos("DEPARTID ='" + departId + "'"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///查询地点信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetLocationInfo(string systemId)
        {
            try
            {
                return Common.ChangColName(dal.GetLocationInfo(" 1=1 "));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///查询地点信息总数
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetLocationInfoCount(string systemId)
        {
            try
            {
                return Common.ChangColName(dal.GetLocationInfoCount(" 1=1 "));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///（新的）查询地点信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetLocationInfoNew(string systemId, string startRow, string endRow)
        {
            try
            {
                return Common.ChangColName(dal.GetLocationInfoNew(" 1=1 ", startRow, endRow));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///查询地点信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public DataTable GetLocationInfo(string systemId, string dataSource)
        {
            try
            {
                return Common.ChangColName(dal.GetLocationInfo("bitand(to_number(systemid), to_number(" + systemId + ")) = to_number(" + systemId + ") and datasource='" + dataSource + "'"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///查询全部地点信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetAllLocationInfos(string systemId)
        {
            try
            {
                return Common.ChangColName(dal.GetAllLocationInfos("bitand(to_number(systemid), to_number(" + systemId + ")) = to_number(" + systemId + ")"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///查询全部地点信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetAllLocationInfo(string systemId)
        {
            try
            {
                return Common.ChangColName(dal.GetAllLocationInfo("bitand(to_number(systemid), to_number(" + systemId + ")) = to_number(" + systemId + ")"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///查询全部地点信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAllLocationInfo(string systemId, string where)
        {
            try
            {
                string strwhere = "bitand(to_number(systemid), to_number(" + systemId + ")) = to_number(" + systemId + ")" + where;
                return Common.ChangColName(dal.GetAllLocationInfo(strwhere));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询需要审核的地点信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="where"></param>
        /// <param name="userid"></param>
        /// <param name="shcs"></param>
        /// <returns></returns>
        public DataTable GetCheckLocation(string systemId, string where, string userid, string shcs)
        {
            try
            {
                string strwhere = where;
                return Common.ChangColName(dal.GetCheckLocation(strwhere, userid, shcs));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///获得方向字典
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetDirectionDict(string systemId)
        {
            try
            {
                return Bll.Common.ChangColName(getDictData(systemId, "240025"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得方向信息（不带条件）
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
        ///判断方向编号是否存在
        /// </summary>
        /// <param name="direction_id"></param>
        /// <returns></returns>
        public bool IsDirectionIdExist(string direction_id)
        {
            try
            {
                DataTable dt = dal.GetDirectionInfo(" direction_id ='" + direction_id + "'");
                if (dt.Rows.Count > 0)
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
                ILog.WriteErrorLog(ex);
                return false;
            }
        }

        /// <summary>
        ///插入地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertLocationInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.InsertLocationInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 插入方向信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertDirectionInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.InsertDirectionInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///删除方向信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteDirectionInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteDirectionInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///删除地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteLocationInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteLocationInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///更新方向信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateDirectionInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateDirectionInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///更新地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateLocationInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateLocationInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///更新违法地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdatePeccLocationInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdatePeccLocationInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        ///更新布控地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateSuspicionLocationInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateSuspicionLocationInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        #endregion 地点方向信息接口

        #region 模版功能

        /// <summary>
        /// 查询模版
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTemplateInfo(string where)
        {
            try
            {
                DataTable dt = dal.GetTemplateInfo(where);

                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 查询对应页面模版
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTemplatePageInfo(string where)
        {
            try
            {
                DataTable dt = dal.GetTemplatePageInfo(where);

                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取登录用户的背景图片
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public DataTable GetBackGround(string userCode)
        {
            try
            {
                DataTable dt = dal.GetBackGround(userCode);

                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 更新模版与页对应关系
        /// </summary>
        /// <param name="templateareaid"></param>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public int UpdateTemplatePageInfo(string templateareaid, string pageid, string usercode)
        {
            try
            {
                return dal.UpdateTemplatePageInfo(templateareaid, pageid, usercode);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        #endregion 模版功能
    }
}