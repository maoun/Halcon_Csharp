using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using MyNet.DataAccess.Model;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class UserManager : IUserManager
    {
        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        private MyNet.Common.Data.DataAccess dataAccess;

        #region IUserManager 成员

        public UserManager()
        {
            dataAccess = GetDataAccess.Init();
        }

        public UserManager(string dataAccessName)
        {
            dataAccess = dac.GetDataAccess(dataAccessName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable GetUserSystem(string userName)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT systemid, a.usercode ,username FROM  t_cfg_sysuser a,t_ser_register b WHERE a.usercode=b.usercode AND username='" + userName + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询用户具有功能
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="userCode"></param>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public DataTable GetUserContent(string systemId, string userCode, string contentId)
        {
            string mySql = string.Empty;
            try
            {
                if (userCode == "000000")
                {
                    mySql = "SELECT  DISTINCT(c.FUNCID),   c.FUNCNAME,   c.FUNCADDRESS,   c.FUNCTYPE,   c.FORMNAME,   c.FORMTYPE,   c.ICONVALUE,   c.ICONNAME ,b.isdefault FROM t_cfg_contents a,t_cfg_func_contents  b ,t_sys_function c  WHERE    systemid ='" + systemId + "' and a.contentId ='" + contentId + "' and  contentid=b.content_id and b.func_id=c.funcid order by to_number(b.orderid) asc";
                }
                else
                {
                    mySql = " select    DISTINCT(c.FUNCID),   c.FUNCNAME,   c.FUNCADDRESS,   c.FUNCTYPE,   c.FORMNAME,   c.FORMTYPE,   c.ICONVALUE,   c.ICONNAME , b.isdefault,a.contentname";
                    mySql = mySql + "  from t_cfg_contents a,";
                    mySql = mySql + "     t_cfg_func_contents b,";
                    mySql = mySql + "     t_sys_function c,";
                    mySql = mySql + "     t_ser_register u,";
                    mySql = mySql + "     t_cfg_user_role ur,";
                    mySql = mySql + "     t_cfg_role_priv rp,";
                    mySql = mySql + "     t_cfg_priv_func pf";
                    mySql = mySql + "     where   systemid = '" + systemId + "'";
                    mySql = mySql + "      and contentid = b.content_id";
                    mySql = mySql + "      and a.contentId = '" + contentId + "'";
                    mySql = mySql + "      and b.func_id = c.funcid";
                    mySql = mySql + "      and pf.func_id= b.func_id";
                    mySql = mySql + "      and u.usercode = '" + userCode + "'";
                    mySql = mySql + "      and u.usercode = ur.usercode";
                    mySql = mySql + "      and ur.rolecode = rp.rolecode";
                    mySql = mySql + "      and rp.privcode = pf.privcode  and c.formtype='0'";
                    switch (dataAccess.DataBaseType.ToUpper())
                    {
                        case "MYSQL":
                            mySql = mySql + "    order by cast(b.orderid as signed) asc ";
                            break;

                        case "ORACLE":
                            mySql = mySql + "    order by cast(b.orderid as int) asc ";
                            break;
                    }
                }
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取用户性别
        /// </summary>
        /// <param name="codetype"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetUserSex(string codetype, string code)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT codedesc FROM t_sys_code WHERE codetype='" + codetype + "' AND CODE='" + code + "'";
                return dataAccess.Get_DataTable(mySql).Rows[0]["codedesc"].ToString();
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public DataTable GetUserShowSystem(string userCode)
        {
            string mySql = string.Empty;
            try
            {
                mySql = mySql + " select  a.systemid,a.systemname ,a.iconvalue from ( select   systemid, systemid   as systemname";
                mySql = mySql + " from   t_cfg_contents c,";
                mySql = mySql + "  t_cfg_func_contents fc,";
                mySql = mySql + "  t_sys_function f,";
                mySql = mySql + "  t_ser_register u,";
                mySql = mySql + "  t_cfg_user_role ur,";
                mySql = mySql + "  t_cfg_role_priv rp,";
                mySql = mySql + "  t_cfg_priv_func pf";
                mySql = mySql + "  where       pf.func_id = f.funcid";
                mySql = mySql + "  and pf.func_id = fc.func_id";
                mySql = mySql + "  and c.contentid = fc.content_id";
                mySql = mySql + "  and u.usercode = '" + userCode + "'";
                mySql = mySql + "  and u.usercode = ur.usercode";
                mySql = mySql + "  and ur.rolecode = rp.rolecode";
                mySql = mySql + "  and rp.privcode = pf.privcode";
                mySql = mySql + "  and c.applicationtype='0'";
                mySql = mySql + "  group by   systemid)  t,t_sys_system a";
                mySql = mySql + "  where a.systemid=t.systemid  AND systemtype='1'  order by orderid asc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable GetUserInfo(string userName)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from  t_ser_register  where username='" + userName + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public DataTable GetUserAmplyInfo(string usercode)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT * FROM  t_ser_person  where usercode='" + usercode + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public string GetUserInfo(string userName, string pwd)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  usercode from t_ser_register where username= '" + userName + "' and password='" + pwd + "'";
                return dataAccess.Get_DataString(mySql, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return "";
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
            string mySql = string.Empty;
            try
            {
                mySql = string.Format(@"select a.*,b.USERNAME,b.REGDATE, f_get_departname(a.departid) AS DEPARTNAME,d.ROLECODE,b.PASSWORD, e.usercode,
f_get_template(e.DataTemplate1),f_get_template(e.DataTemplate2),f_get_template(e.DataTemplate3),f_get_template(e.DataTemplate4),f_get_template(e.ListTemplate1),
f_get_template(e.ListTemplate2),f_get_template(e.ListTemplate3),f_get_funcaddress(e.UserTemplate),e.UserBackGround,f_to_name ('011005', a.sex) AS codedesc,i.rolename,FirstTemplate
from t_ser_register b LEFT JOIN   t_ser_person a  on b.USERCODE=a.usercode left join t_cfg_user_role d on d.USERCODE=a.usercode LEFT JOIN t_cfg_template_user e ON a.usercode=e.usercode
 LEFT JOIN (SELECT * FROM  t_cfg_role h  WHERE systemId='00') i ON  i.rolecode=d.ROLECODE where a.systemid='{1}' and {2}", ip, systemId, where);

                DataTable dt = dataAccess.Get_DataTable(mySql);
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取用户信息（最新）
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSerUserInfoNew(string systemId, string where, string ip, int startNum, int endNum)
        {
            string mySql = string.Empty;
            try
            {
                mySql = string.Format(@"select a.*,b.USERNAME,DATE_FORMAT(b.REGDATE,'%Y-%m-%d %H:%i:%s'),c.DEPARTNAME,d.ROLECODE,b.PASSWORD, e.usercode,
f_get_template(e.DataTemplate1),f_get_template(e.DataTemplate2),f_get_template(e.DataTemplate3),f_get_template(e.DataTemplate4),
f_get_template(e.ListTemplate1),f_get_template(e.ListTemplate2),f_get_template(e.ListTemplate3),f_get_funcaddress(e.UserTemplate),e.UserBackGround,g.codedesc,i.rolename,FirstTemplate
from t_ser_register b LEFT JOIN   t_ser_person a  on b.USERCODE=a.usercode left join t_cfg_department c on c.DEPARTID=a.departid
left join t_cfg_user_role d on d.USERCODE=a.usercode LEFT JOIN t_cfg_template_user e ON a.usercode=e.usercode
LEFT JOIN (SELECT CODE,codedesc FROM t_sys_code f WHERE f.codetype='011005') g ON g.CODE=a.sex LEFT JOIN
(SELECT * FROM  t_cfg_role h  WHERE systemId='00') i ON  i.rolecode=d.ROLECODE where a.systemid='{1}' and {2} limit {3}, {4}", ip, systemId, where, startNum, (endNum - startNum).ToString());

                DataTable dt = dataAccess.Get_DataTable(mySql);
                ILog.WriteErrorLog(mySql + "    " + dt.Rows.Count.ToString());
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = string.Format(@"select count(*)
from t_ser_register b LEFT JOIN   t_ser_person a  on b.USERCODE=a.usercode left join t_cfg_department c on c.DEPARTID=a.departid
left join t_cfg_user_role d on d.USERCODE=a.usercode LEFT JOIN t_cfg_template_user e ON a.usercode=e.usercode
LEFT JOIN (SELECT CODE,codedesc FROM t_sys_code f WHERE f.codetype='011005') g ON g.CODE=a.sex LEFT JOIN
(SELECT * FROM  t_cfg_role h  WHERE systemId='00') i ON  i.rolecode=d.ROLECODE where a.systemid='{1}' and {2}", ip, systemId, where);

                DataTable dt = dataAccess.Get_DataTable(mySql);
                ILog.WriteErrorLog(mySql + "    " + dt.Rows.Count.ToString());
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from t_ser_person  where usercode='" + hs["usercode"].ToString() + "'";
                int res = dataAccess.Execute_NonQuery(mySql);
                mySql = "delete  from t_ser_register  where usercode='" + hs["usercode"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_ser_person", "usercode", hs["usercode"].ToString()) > 0)
                {
                    mySql = "update  t_ser_person  set ";
                    mySql = mySql + Common.GetHashtableStr(hs, "sex", "sex");
                    mySql = mySql + Common.GetHashtableStr(hs, "idtype", "idtype");
                    mySql = mySql + Common.GetHashtableStr(hs, "departid", "departid");
                    mySql = mySql + "siren='" + hs["siren"].ToString() + "',";
                    mySql = mySql + "name='" + hs["name"].ToString() + "',";
                    mySql = mySql + "idno='" + hs["idno"].ToString() + "',";
                    mySql = mySql + "birthday='" + hs["birthday"].ToString() + "',";
                    mySql = mySql + "address='" + hs["address"].ToString() + "',";
                    mySql = mySql + "mobilephone='" + hs["mobilephone"].ToString() + "',";
                    mySql = mySql + "officephone='" + hs["officephone"].ToString() + "',";
                    mySql = mySql + "remark='" + hs["remark"].ToString() + "'";
                    mySql = mySql + " where usercode='" + hs["usercode"].ToString() + "'";
                }
                else
                {
                    mySql = "insert into  t_ser_person (usercode,name,idno,idtype,sex,departid,address,mobilephone,birthday,siren,remark) values(";
                    mySql = mySql + "'" + hs["usercode"].ToString() + "',";
                    mySql = mySql + "'" + hs["name"].ToString() + "',";
                    mySql = mySql + "'" + hs["idno"].ToString() + "',";
                    mySql = mySql + "'" + Common.GetHashtableValue(hs, "idtype", "A") + "',";
                    mySql = mySql + "'" + Common.GetHashtableValue(hs, "sex", "1") + "',";
                    mySql = mySql + "'" + Common.GetHashtableValue(hs, "departid", "") + "',";
                    mySql = mySql + "'" + hs["address"].ToString() + "',";
                    mySql = mySql + "'" + hs["mobilephone"].ToString() + "',";
                    mySql = mySql + "'" + hs["birthday"].ToString() + "',";
                    mySql = mySql + "'" + hs["siren"].ToString() + "',";
                    mySql = mySql + "'" + hs["remark"].ToString() + "')";
                }
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 判断输入值是否在指定表存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        private int GeXhExist(string tableName, string fieldName, string fieldValue)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select count(1) from   " + tableName + "   where   " + fieldName + "  ='" + fieldValue + "'";
                return int.Parse(dataAccess.Get_DataString(mySql, 0));
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = "select PASSWORD from t_ser_register where USERCODE=" + usercode;
                DataTable dt = dataAccess.Get_DataTable(mySql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string pwd = Cryptography.Decrypt(dt.Rows[0][0].ToString());
                    if (pwd.Equals(oldPwd))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return false;
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
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_ser_register  set ";
                mySql = mySql + "password='" + pwd + "'";
                mySql = mySql + " where usercode='" + usercode + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
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
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_ser_register (usercode,username,password,regdate) values(";
                mySql = mySql + "'" + usercode + "',";
                mySql = mySql + "'" + userName + "',";
                mySql = mySql + "'" + pwd + "',";
                mySql = mySql + "now())";
                int res = dataAccess.Execute_NonQuery(mySql);

                if (res > 0)
                {   //t_cfg_sysuser
                    mySql = "insert into t_ser_person (systemid,usercode) values(";
                    mySql = mySql + "'" + systemId + "',";
                    mySql = mySql + "'" + usercode + "')";
                    res = dataAccess.Execute_NonQuery(mySql);
                }

                return res;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public string IsExsitFuncByUser(string userCode, string formPath)
        {
            StringBuilder mySql = new StringBuilder();
            try
            {
                mySql.Append("select f.funcid");
                mySql.Append("  from t_sys_function f, t_cfg_priv_func pf, t_cfg_role_priv rp, t_cfg_user_role ur, t_ser_register u");
                mySql.Append(" where f.funcid = pf.func_id");
                mySql.Append("   and pf.privcode = rp.privcode");
                mySql.Append("   and ur.rolecode = rp.rolecode");
                mySql.Append("   and ur.usercode = '" + userCode + "'");
                mySql.Append("   and LOWER(f.funcaddress) = '" + formPath.ToLower() + "'");
                return dataAccess.Get_DataString(mySql.ToString());
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql.ToString() + ex.Message);
                return string.Empty;
            }
        }

        public bool IsHaveFuncPriv(string userName, string funcCode)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select   count (1)  from t_ser_register u, t_cfg_user_role ur,t_cfg_role_priv rp,t_cfg_priv_func pf";
                mySql = mySql + " where u.usercode = '" + userName + "'";
                mySql = mySql + " and   pf.func_id = '" + funcCode + "'";
                mySql = mySql + " and u.usercode = ur.usercode";
                mySql = mySql + " and ur.rolecode = rp.rolecode";
                mySql = mySql + " and rp.privcode = pf.privcode";
                string count = dataAccess.Get_DataString(mySql);
                return !count.Equals("0");
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return false;
            }
        }

        public DataTable GetUserLikeFunc(string userCode, string useflag)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select   uf.usercode,uf.funcid,f.funcName,f.funcaddress,f.functype,f.iconname,uf.useflag,uf.scount,c.systemid";
                mySql = mySql + " from   t_cfg_user_func uf,";
                mySql = mySql + " t_cfg_contents c,";
                mySql = mySql + " t_cfg_func_contents fc,";
                mySql = mySql + " t_sys_function f,";
                mySql = mySql + " t_ser_register u,";
                mySql = mySql + " t_cfg_user_role ur,";
                mySql = mySql + " t_cfg_role_priv rp,";
                mySql = mySql + " t_cfg_priv_func pf";
                mySql = mySql + "  where   ";
                mySql = mySql + "  uf.funcid=f.funcid ";
                mySql = mySql + "  and pf.func_id=uf.funcid ";
                mySql = mySql + "  and fc.func_id =uf.funcid  ";
                mySql = mySql + "  and c.contentid=fc.content_id ";
                mySql = mySql + "  and uf. usercode=u.usercode ";
                mySql = mySql + "  and uf.usercode = '" + userCode + "' and uf.useflag='" + useflag + "'";
                mySql = mySql + "  and u.usercode = ur.usercode ";
                mySql = mySql + "  and ur.rolecode = rp.rolecode ";
                mySql = mySql + "  and rp.privcode = pf.privcode ";
                mySql = mySql + "  order by uf.scount asc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public int UpdateUserLikeFunc(string userCode, List<string> functions)
        {
            string mySql = string.Empty;
            try
            {
                int ires = 0;
                for (int i = 0; i < functions.Count; i++)
                {
                    mySql = "update  t_cfg_user_func  set ";
                    mySql = mySql + " scount=scount+1";
                    mySql = mySql + " where usercode='" + userCode + "'  and  funcid='" + functions[i] + "'";
                    ires = ires + dataAccess.Execute_NonQuery(mySql);
                }

                return ires;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int InsertUserLikeFunc(string userCode, List<UserLike> functions)
        {
            string mySql = string.Empty;
            try
            {
                int ires = 0;
                for (int i = 0; i < functions.Count; i++)
                {
                    mySql = "insert into   t_cfg_user_func(usercode ,funcid,useflag,scount) values(";
                    mySql = mySql + "'" + functions[i].Usercode + "',";
                    mySql = mySql + "'" + functions[i].Funcid + "',";
                    mySql = mySql + "'" + functions[i].Useflag + "',";
                    mySql = mySql + "'" + functions[i].Scount + "')";
                    ires = ires + dataAccess.Execute_NonQuery(mySql);
                }

                return ires;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int DeleteUserLikeFunc(string userCode)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_cfg_user_func   ";
                mySql = mySql + " where usercode='" + userCode + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        private string GetSql(string systemId, string usercode)
        {
            string mySql = string.Empty;

            mySql = "    select * from (select   ur.usercode, f.funcid,f.funcname, f.funcaddress, c.contentid, c.contentname, c.systemid";
            mySql = mySql + "   from  ";
            mySql = mySql + "   t_cfg_contents c,";
            mySql = mySql + "   t_cfg_func_contents fc,  ";
            mySql = mySql + "   t_sys_function f, ";
            mySql = mySql + "   t_cfg_user_role ur, ";
            mySql = mySql + "   t_cfg_role_priv rp,";
            mySql = mySql + "   t_cfg_priv_func pf";
            mySql = mySql + "   where  pf.func_id = fc.func_id";
            mySql = mySql + "   and f.funcid=pf.func_id";
            mySql = mySql + "   and c.contentid = fc.content_id";
            mySql = mySql + "   and ur.usercode = '" + usercode + "'";
            mySql = mySql + "   and c.systemid = '" + systemId + "'";
            mySql = mySql + "   and ur.rolecode = rp.rolecode";
            mySql = mySql + "   and rp.privcode = pf.privcode";
            mySql = mySql + "   order by  c.contentid, pf.func_id asc ) a where a. funcid not in (select funcid from t_cfg_user_func where usercode=a.usercode)";
            return mySql;
        }

        public DataTable GetUserFreeFunction(string systemId, string usercode)
        {
            string mySql = string.Empty;
            try
            {
                mySql = GetSql(systemId, usercode);
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public DataTable GetUserFreeContent(string systemId, string usercode)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select contentid,contentname from (" + GetSql(systemId, usercode) + " ) group by contentid,contentname ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public DataTable GetAllUserName()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT username,NAME FROM t_ser_register a,t_ser_person b WHERE a.usercode=b.usercode ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }
        #endregion IUserManager 成员
    }
}