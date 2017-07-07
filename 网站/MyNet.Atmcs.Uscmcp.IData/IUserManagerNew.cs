using MyNet.DataAccess.Model;
using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.IData
{
    public interface IUserManagerNew
    {
        #region 查询相关方法

        /// <summary>
        /// 获得用户具有系统
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        DataTable GetUserSystem(string userName);

        /// <summary>
        /// 获得用户注册信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        DataTable GetUserInfo(string userName);

        /// <summary>
        /// 根据用户名和密码获得
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        string GetUserInfo(string userName, string pwd);

        /// <summary>
        /// 获得用户详细信息
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        DataTable GetUserAmplyInfo(string usercode);

        /// <summary>
        ///查询用户具有功能
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="userName"></param>
        /// <param name="contentId"></param>
        /// <returns></returns>
        DataTable GetUserContent(string systemId, string userName, string contentId);

        DataTable GetUserShowSystem(string usercode);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetSerUserInfo(string systemId, string where, string ip);

        /// <summary>
        /// 获取用户信息(最新)
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetSerUserInfoNew(string systemId, string where, string ip, int startNum, int endNum);

        /// <summary>
        /// 获取用户信息总数
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        DataTable GetSerUserInfoCount(string systemId, string where, string ip);

        /// <summary>
        /// 获取用户的性别
        /// </summary>
        /// <param name="codetype"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        string GetUserSex(string codetype, string code);

        DataTable GetUserFreeFunction(string systemId, string usercode);

        DataTable GetUserFreeContent(string systemId, string usercode);

        /// <summary>
        /// 更细用户信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateSerUserInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DeleteSerUserInfo(System.Collections.Hashtable hs);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        int ChangeUserPwd(string usercode, string pwd);

        /// <summary>
        /// 验证用户密码
        /// </summary>
        /// <param name="oldPwd"></param>
        /// <returns></returns>
        bool CheckOldPwd(string usercode, string oldPwd);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="usercode"></param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        int UserRegister(string systemId, string usercode, string userName, string pwd);

        bool IsHaveFuncPriv(string userName, string funcCode);

        string IsExsitFuncByUser(string userCode, string formPath);

        DataTable GetUserLikeFunc(string userCode, string useflag);

        int InsertUserLikeFunc(string userCode, List<UserLike> functions);

        int DeleteUserLikeFunc(string userCode);

        /// <summary>
        /// 查询所有用户名
        /// </summary>
        /// <returns></returns>
        DataTable GetAllUserName();

        #endregion 查询相关方法
    }
}