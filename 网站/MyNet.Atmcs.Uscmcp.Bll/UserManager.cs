/***********************************************************************
 * Module:   目录业务逻辑类
 * Author:   李建平
 * Modified: 2008年10月17日
 * Purpose:  该类为页面提供需要的业务逻辑方法
 ***********************************************************************/

using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    [Serializable]
    public class UserManager
    {
        private SystemManager systemManager = new SystemManager();
        private SettingManager settingManager = new SettingManager();

        /// <summary>
        ///  用户操作接口
        /// </summary>
        private static readonly IUserManager dal = DALFactory.CreateUserManager();

        public DataTable GetUserSystem(string userName)
        {
            try
            {
                return dal.GetUserSystem(userName);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public List<object> GetData(string userName, string MapPath, string urlNet)
        {
            List<object> data = new List<object>();
            try
            {
                DataTable dt = GetUserSystem(userName);

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataTable dt2 = systemManager.GetSystemInfo(dt.Rows[i][0].ToString());
                        byte[] SqlByt = (byte[])dt2.Rows[0]["col4"];
                        string systemid = dt2.Rows[0][0].ToString();
                        data.Add(new
                        {
                            name = dt2.Rows[0][1].ToString(),
                            url = Common.SaveImage(SqlByt, MapPath, urlNet, systemid + ".jpg"),
                            description = 2,
                            sysid = systemid
                        });
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return data;
            }
        }

        /// <summary>
        /// 查询用户具有功能
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="contentid"></param>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public DataTable GetDirectory(string systemId, string contentid, string userCode)
        {
            try
            {
                return Common.ChangColName(dal.GetUserContent(systemId, userCode, contentid));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSerUserInfo(string systemId, string where, string ip)
        {
            try
            {
                return Common.ChangColName(dal.GetSerUserInfo(systemId, where, ip));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSerUserInfoNew(string systemId, string where, string ip, int startNum, int endNum)
        {
            try
            {
                return Common.ChangColName(dal.GetSerUserInfoNew(systemId, where, ip, startNum, endNum));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取用户信息总数
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSerUserInfoCount(string systemId, string where, string ip)
        {
            try
            {
                return Common.ChangColName(dal.GetSerUserInfoCount(systemId, where, ip));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteSerUserInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.DeleteSerUserInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateSerUserInfo(System.Collections.Hashtable hs)
        {
            try
            {
                return dal.UpdateSerUserInfo(hs);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public int ChangeUserPwd(string usercode, string pwd)
        {
            try
            {
                return dal.ChangeUserPwd(usercode, pwd);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 验证用户密码
        /// </summary>
        /// <param name="oldPwd"></param>
        /// <returns></returns>
        public bool CheckOldPwd(string usercode, string oldPwd)
        {
            try
            {
                return dal.CheckOldPwd(usercode, oldPwd);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="usercode"></param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public int UserRegister(string systemId, string usercode, string userName, string pwd)
        {
            try
            {
                return dal.UserRegister(systemId, usercode, userName, pwd);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 获取性别
        /// </summary>
        /// <param name="systemID"></param>
        /// <returns></returns>
        public DataTable GetSex(string systemID)
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(systemID, "011005"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取用户性别
        /// </summary>
        /// <param name="codetype"></param>
        /// <returns></returns>
        public string GetUserSex(string code)
        {
            try
            {
                return dal.GetUserSex("011005", code);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取证件类型
        /// </summary>
        /// <param name="systemID"></param>
        /// <returns></returns>
        public DataTable GetIdType(string systemID)
        {
            try
            {
                return Bll.Common.ChangColName(settingManager.getDictData(systemID, "240030"));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetUserShowSystem(string userCode)
        {
            try
            {
                return Common.ChangColName(dal.GetUserShowSystem(userCode));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetUserLikeFunc(string userCode, string useflag)
        {
            try
            {
                return Common.ChangColName(dal.GetUserLikeFunc(userCode, useflag));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetUserFreeFunction(string systemID, string userCode)
        {
            try
            {
                return Common.ChangColName(dal.GetUserFreeFunction(systemID, userCode));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public DataTable GetUserFreeContent(string systemID, string userCode)
        {
            try
            {
                return Common.ChangColName(dal.GetUserFreeContent(systemID, userCode));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        public int DeleteUserLikeFunc(string userCode)
        {
            try
            {
                return dal.DeleteUserLikeFunc(userCode);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// 查询所有用户名
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllUserName()
        {
            try
            {
                return Common.ChangColName(dal.GetAllUserName());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
    }
}