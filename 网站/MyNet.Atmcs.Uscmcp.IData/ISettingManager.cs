using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.IData
{
    public interface ISettingManager
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="configid"></param>
        /// <returns></returns>
        string GetSettingConfigValue(string systemId, string configid);

        /// <summary>
        /// 获得当前用户主功能菜单
        /// </summary>
        /// <param name="frmType"></param>
        /// <returns></returns>
        DataTable GetSettingContent(string frmType);

        /// <summary>
        /// 获得当前用户主功能菜单
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="frmType"></param>
        /// <returns></returns>
        DataTable GetSettingContent(string systemId, string frmType);

        /// <summary>
        /// 获得当前系统编号
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="frmType"></param>
        /// <returns></returns>
        DataTable GetUserSettingContent(string systemId, string frmType, string usercode);

        /// <summary>
        ///
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="contentid"></param>
        /// <param name="frmType"></param>
        /// <returns></returns>
        DataTable GetSettingContent(string systemId, string contentid, string frmType);

        /// <summary>
        /// 字典查询
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        DataTable GetSettingCode(string systemId, string codeType);

        /// <summary>
        /// 代码类型查询
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        DataTable GetSettingCodeType(string systemId);

        /// <summary>
        /// 获取车辆品牌字典
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetVehicleBrand(string where);

        /// <summary>
        /// 获取车辆型号字典
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetVehicleModel(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int AddSettingContent(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateSettingContent(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteSettingContent(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="contentId"></param>
        /// <param name="frmType"></param>
        /// <returns></returns>
        DataTable GetContentFunction(string systemId, string contentId, string frmType);

        /// <summary>
        ///
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        DataTable GetFreeContentFunc(string systemId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DelContentFunction(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int AddContentFunction(System.Collections.Hashtable hs);

        /// <summary>
        /// 获得配置信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        DataTable GetConfigInfo(string systemId);

        /// <summary>
        /// 获得配置信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        DataTable GetConfigInfo(string systemId, string configId);

        /// <summary>
        /// 添加配置信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int AddConfigInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateConfigInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteConfigInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetDepartment(string where);

        /// <summary>
        /// 查询部门信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        DataTable GetConfigDepartment(string systemId);

        /// <summary>
        /// 查询部门信息
        /// </summary>
        /// <param name="departid"></param>
        /// <param name="systemId"></param>
        /// <returns></returns>
        DataTable GetConfigDepartmentInfo(string departid, string systemId);

        /// <summary>
        /// 查询部门信息
        /// </summary>
        /// <param name="queryField"></param>
        /// <param name="systemId"></param>
        /// <returns></returns>
        DataTable GetConfigDepartment(string queryField, string systemId);

        /// <summary>
        /// 删除部门信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteDepartmentInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 更新部门信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateDepartmentInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 获取权限信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetSerPrivInfo(string systemId, string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteSerPrivInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertSerPrivInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateSerPrivInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="privcode"></param>
        /// <returns></returns>
        DataTable GetSerPrivFunc(string privcode);

        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetSerRoleInfo(string systemId, string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteSerRoleInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertSerRoleInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateSerRoleInfo(System.Collections.Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="rolecode"></param>
        /// <returns></returns>
        DataTable GetSerRolePriv(string rolecode);

        /// <summary>
        ///
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        DataTable GetSerUserRole(string usercode);

        /// <summary>
        ///
        /// </summary>
        /// <param name="privcode"></param>
        /// <param name="funcids"></param>
        /// <returns></returns>
        int InsertPrivFunc(string privcode, List<string> funcids);

        /// <summary>
        ///
        /// </summary>
        /// <param name="rolecode"></param>
        /// <param name="privcodes"></param>
        /// <returns></returns>
        int InsertRolePriv(string rolecode, List<string> privcodes);

        /// <summary>
        ///
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="rolecode"></param>
        /// <returns></returns>
        int InsertUserRole(string usercode, string rolecode);

        /// <summary>
        /// 获得地点信息（不带条件）
        /// </summary>
        /// <returns></returns>
        DataTable GetLocationInfo();

        /// <summary>
        /// 获得方向信息（不带条件）
        /// </summary>
        /// <returns></returns>
        DataTable GetDirectionInfo();

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetLocationInfo(string where);

        /// <summary>
        /// 获得地点信息（不带条件）
        /// </summary>
        /// <returns></returns>
        DataTable GetLocationInfoNew(string where, string startRow, string endRow);

        /// <summary>
        /// 获得地点信息总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetLocationInfoCount(string where);

        /// <summary>
        /// 查询地点信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetAllLocationInfo(string where);

        /// <summary>
        /// 查询需要审核的地点信息
        /// </summary>
        /// <param name="where"></param>
        /// <param name="userid"></param>
        /// <param name="shcs"></param>
        /// <returns></returns>
        DataTable GetCheckLocation(string where, string userid, string shcs);

        /// <summary>
        /// 查询地点信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetAllLocationInfos(string where);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateLocationInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 删除地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteLocationInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 插入地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertLocationInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 更新地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdatePeccLocationInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 更新地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateSuspicionLocationInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 查询方向信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetDirectionInfo(string where);

        /// <summary>
        /// 更新方向信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateDirectionInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 删除方向信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteDirectionInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 插入方向信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int InsertDirectionInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 查询模版
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetTemplateInfo(string where);

        /// <summary>
        /// 查询对应页面模版
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetTemplatePageInfo(string where);

        /// <summary>
        /// 获取登录用户的背景图片
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        DataTable GetBackGround(string userCode);

        /// <summary>
        /// 更新模版与页对应关系
        /// </summary>
        /// <param name="templateareaid"></param>
        /// <param name="pageid"></param>
        /// <param name="usercode"></param>
        /// <returns></returns>
        int UpdateTemplatePageInfo(string templateareaid, string pageid, string usercode);
    }
}