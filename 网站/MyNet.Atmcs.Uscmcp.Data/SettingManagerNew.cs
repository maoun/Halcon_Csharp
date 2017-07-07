using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;
using System;
using System.Collections.Generic;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class SettingManagerNew : ISettingManagerNew
    {
        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        private MyNet.Common.Data.DataAccess dataAccess;

        public SettingManagerNew()
        {
            //type=0 读取uscmcp数据库（老库） type=1 读取frame数据库（新库）
            string type = System.Configuration.ConfigurationManager.AppSettings["BkType"].ToString();
            if (type.Equals("0"))
            {
                dataAccess = GetDataAccess.Init();
            }
            else if (type.Equals("1"))
            {
                dataAccess = GetDataAccess.InitNew();
            }
            else
            {
                dataAccess = GetDataAccess.Init();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataAccessName"></param>
        public SettingManagerNew(string dataAccessName)
        {
            dataAccess = dac.GetDataAccess(dataAccessName);
        }

        #region ISettingManager 成员

        /// <summary>
        /// 字典查询
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public DataTable GetSettingCode(string systemId, string codeType)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select Code,CodeDesc from t_sys_code a where a.codeType='" + codeType + "' and  Isuse='1' order by cast(isorder as signed) asc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT * FROM t_vehicle_brand WHERE " + where + " ORDER BY BRANDNAME";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT * FROM t_vehicle_model a INNER JOIN t_vehicle_brand b ON a.BRANDID = b.BRANDID WHERE " + where + " ORDER BY MODELNAME";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 代码类型查询
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetSettingCodeType(string systemId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select *  from t_sys_codeType a,t_cfg_syscode b  where   a.codeType =b.codeType and  b. systemId='" + systemId + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        #endregion ISettingManager 成员

        #region ISettingManager 成员

        /// <summary>
        /// 获得当前用户主功能菜单
        /// </summary>
        /// <param name="frmType"></param>
        /// <returns></returns>
        public DataTable GetSettingContent(string frmType)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  a.systemid,a.systemname from (select systemid from t_cfg_contents where  applicationtype ='0' group by systemid ) t,t_sys_system a  where t.systemid=a.systemid order by systemid";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public DataTable GetSettingContent(string systemId, string contentid, string frmType)
        {
            string mySql = string.Empty;
            try
            {
                if (frmType == "0")
                {
                    mySql = "select systemid,contentid,contentname,connecturl,applicationtype,IconValue from t_cfg_contents  where   contentid='" + contentid + "' and applicationtype='0' and systemid ='" + systemId + "'";
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
        /// 获得当前系统编号
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="frmType"></param>
        /// <returns></returns>
        public DataTable GetSettingContent(string systemId, string frmType)
        {
            string mySql = string.Empty;
            try
            {
                if (frmType == "0")
                {
                    switch (dataAccess.DataBaseType.ToUpper())
                    {
                        case "MYSQL":
                            mySql = "select systemid,contentid,contentname,connecturl,  applicationtype,IconValue from t_cfg_contents  where    systemid ='" + systemId + "' and applicationtype='0' order by cast(orderid as signed) asc";

                            break;

                        case "ORACLE":
                            mySql = "select systemid,contentid,contentname,connecturl,  applicationtype,IconValue from t_cfg_contents  where    systemid ='" + systemId + "' and applicationtype='0' order by cast(orderid as int) asc";
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
        /// 获得当前系统编号
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="frmType"></param>
        /// <returns></returns>
        public DataTable GetUserSettingContent(string systemId, string frmType, string usercode)
        {
            string mySql = string.Empty;
            try
            {
                if (frmType == "0")
                {
                    switch (dataAccess.DataBaseType.ToUpper())
                    {
                        case "MYSQL":
                            //mySql = "select systemid,contentid,contentname,connecturl,  applicationtype,IconValue from t_cfg_contents  where    systemid ='" + systemId + "' and applicationtype='0' order by cast(orderid as signed) asc";
                            mySql = string.Format(@"SELECT DISTINCT(a.CONTENTID),  a.contentname,  a.connecturl,  a.applicationtype,  a.IconValue,  a.orderid FROM
  t_cfg_contents a,  t_cfg_func_contents b,  t_sys_function c,  t_ser_register u,  t_cfg_user_role ur,  t_cfg_role_priv rp,  t_cfg_priv_func pf
  WHERE systemid = '00'   AND contentid = b.content_id   AND b.func_id = c.funcid   AND pf.func_id = b.func_id   AND u.usercode = '{0}'
  AND u.usercode = ur.usercode   AND ur.rolecode = rp.rolecode   AND rp.privcode = pf.privcode   AND c.formtype = '0'
  AND systemid = '{1}'   AND a.applicationtype = '0' ORDER BY CAST(a.orderid AS SIGNED) ASC ", usercode, systemId);

                            break;

                        case "ORACLE":
                            mySql = "select systemid,contentid,contentname,connecturl,  applicationtype,IconValue from t_cfg_contents  where    systemid ='" + systemId + "' and applicationtype='0' order by cast(orderid as int) asc";
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

        public int AddSettingContent(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_cfg_contents (systemid,contentid,contentname,connecturl,applicationtype,IconValue) values(";
                mySql = mySql + "'" + hs["systemid"].ToString() + "',";
                mySql = mySql + "'" + hs["contentid"].ToString() + "',";
                mySql = mySql + "'" + hs["contentname"].ToString() + "',";
                mySql = mySql + "'" + hs["connecturl"].ToString() + "',";
                mySql = mySql + "'" + hs["applicationtype"].ToString() + "',";
                mySql = mySql + "'" + hs["iconvalue"].ToString() + "' )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int DeleteSettingContent(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_cfg_contents   ";
                mySql = mySql + " where contentid='" + hs["contentid"].ToString() + "'";
                mySql = mySql + " and systemid='" + hs["systemid"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int UpdateSettingContent(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_cfg_contents  set ";
                mySql = mySql + " contentname='" + hs["contentname"].ToString() + "',";
                mySql = mySql + " connecturl='" + hs["connecturl"].ToString() + "',";
                mySql = mySql + " applicationtype='" + hs["applicationtype"].ToString() + "',";
                mySql = mySql + " IconValue='" + hs["iconvalue"].ToString() + "' ";
                mySql = mySql + " where contentid='" + hs["contentid"].ToString() + "'";
                mySql = mySql + " and systemid='" + hs["systemid"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="contentId"></param>
        /// <param name="fromType"></param>
        /// <returns></returns>
        public DataTable GetContentFunction(string systemId, string contentId, string fromType)
        {
            string mySql = string.Empty;
            try
            {
                if (fromType == "0")
                {
                    mySql = "select c.funcid,c.funcname ,c.funcaddress,c.functype,c.formtype,c.iconvalue,c.iconname from t_cfg_contents a,t_cfg_func_contents  b ,t_sys_function c  where    systemid ='" + systemId + "' and a.contentid ='" + contentId + "' and  c.formtype='" + fromType + "' and contentid=b.CONTENT_ID AND b.FUNC_ID=c.funcid order by cast(b.orderid as signed) asc";
                }
                else
                {
                    mySql = "select c.funcid,c.funcname ,c.funcaddress,c.functype,c.formtype,c.iconvalue,c.iconname  from t_cfg_func_system a, t_sys_function c where systemid ='" + systemId + "' and c.formtype='" + fromType + "'  and a.funcid=c.funcid  and a.isuse='1' ORDER BY c.funcid asc";
                }

                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public DataTable GetFreeContentFunc(string systemId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select c.* from t_sys_function c  where  c.formtype='0'  and  funcid not in (select func_id from t_cfg_contents a,t_cfg_func_contents b where    systemid ='" + systemId + "' AND a.contentid=b.CONTENT_ID )  ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public int AddContentFunction(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_cfg_func_contents (content_id,func_id,orderid) values(";
                mySql = mySql + "'" + hs["contentid"].ToString() + "',";
                mySql = mySql + "'" + hs["func_id"].ToString() + "',";
                mySql = mySql + "'" + hs["orderid"].ToString() + "' )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int DelContentFunction(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_cfg_func_contents   ";
                mySql = mySql + " where content_id='" + hs["contentid"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 添加配置信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int AddConfigInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_cfg_sysconfig (systemid,configid,configName,configValue,remark) values(";
                mySql = mySql + "'" + hs["systemid"].ToString() + "',";
                mySql = mySql + "'" + hs["configid"].ToString() + "',";
                mySql = mySql + "'" + hs["configname"].ToString() + "',";
                mySql = mySql + "'" + hs["configvalue"].ToString() + "',";
                mySql = mySql + "'" + hs["remark"].ToString() + "' )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 获得配置信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        public DataTable GetConfigInfo(string systemId, string configId)
        {
            string mySql = string.Empty;
            try
            {
                DataTable table = new DataTable();
                mySql = "select * from t_cfg_sysconfig  where   systemid ='" + systemId + "'  and  configid='" + configId + "'";
                //return dataAccess.Get_DataTable(mySql);
                table = dataAccess.Get_DataTable(mySql);
                if (table.Rows.Count > 0)
                {
                    return table;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得配置信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetConfigInfo(string systemId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from t_cfg_sysconfig  where   systemid ='" + systemId + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public int UpdateConfigInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_cfg_sysconfig  set ";
                mySql = mySql + "configName='" + hs["configname"].ToString() + "',";
                mySql = mySql + "configValue='" + hs["configvalue"].ToString() + "', ";
                mySql = mySql + "remark='" + hs["remark"].ToString() + "'";
                mySql = mySql + " where configid='" + hs["configid"].ToString() + "'";
                mySql = mySql + " and systemid='" + hs["systemid"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int DeleteConfigInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_cfg_sysconfig   ";
                mySql = mySql + " where configid='" + hs["configid"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public DataTable GetDepartment(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from t_cfg_department  where " + where + " order by class asc ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询部门信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetConfigDepartment(string systemId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from t_cfg_department order by class asc ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询部门信息
        /// </summary>
        /// <param name="queryField"></param>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetConfigDepartment(string queryField, string systemId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select " + queryField + " from t_cfg_department where systemid='" + systemId + "' order by class asc ,departid ASC";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询部门信息
        /// </summary>
        /// <param name="departid"></param>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public DataTable GetConfigDepartmentInfo(string departid, string systemId)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select *  from   t_cfg_department where  departid='" + departid + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public int DeleteDepartmentInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from t_cfg_department  where id='" + hs["id"].ToString() + "'";
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

        public int UpdateDepartmentInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_cfg_department", "id", hs["id"].ToString()) > 0)
                {
                    mySql = "update  t_cfg_department  set ";
                    mySql = mySql + "departid='" + hs["departid"].ToString() + "',";
                    mySql = mySql + "departname='" + hs["departname"].ToString() + "',";
                    mySql = mySql + Common.GetHashtableStr(hs, "classcode", "classcode");
                    mySql = mySql + Common.GetHashtableStr(hs, "class", "class");
                    mySql = mySql + "workcontent='" + hs["workcontent"].ToString() + "',";
                    mySql = mySql + "workaddress='" + hs["workaddress"].ToString() + "',";
                    mySql = mySql + "manager='" + hs["manager"].ToString() + "',";
                    mySql = mySql + "managermobile='" + hs["managermobile"].ToString() + "',";
                    mySql = mySql + "managerphone='" + hs["managerphone"].ToString() + "',";
                    mySql = mySql + "officephone='" + hs["officephone"].ToString() + "',";
                    mySql = mySql + "officephone2='" + hs["officephone2"].ToString() + "',";
                    mySql = mySql + "officephone3='" + hs["officephone3"].ToString() + "',";
                    mySql = mySql + "officefax='" + hs["officefax"].ToString() + "',";
                    mySql = mySql + "postcode='" + hs["postcode"].ToString() + "'";
                    mySql = mySql + " where id='" + hs["id"].ToString() + "'";
                }
                else
                {
                    mySql = "insert into  t_cfg_department (id,departid,departname, classcode, class ,workcontent,workaddress,manager, managermobile, managerphone, officephone, officephone2, officephone3, officefax, postcode, systemid) values(";
                    mySql = mySql + "'" + hs["id"].ToString() + "',";
                    mySql = mySql + "'" + hs["departid"].ToString() + "',";
                    mySql = mySql + "'" + hs["departname"].ToString() + "',";
                    mySql = mySql + "'" + Common.GetHashtableValue(hs, "classcode", "000000") + "',";
                    mySql = mySql + "'" + Common.GetHashtableValue(hs, "class", "2") + "',";
                    mySql = mySql + "'" + hs["workcontent"].ToString() + "',";
                    mySql = mySql + "'" + hs["workaddress"].ToString() + "',";
                    mySql = mySql + "'" + hs["manager"].ToString() + "',";
                    mySql = mySql + "'" + hs["managermobile"].ToString() + "',";
                    mySql = mySql + "'" + hs["managerphone"].ToString() + "',";
                    mySql = mySql + "'" + hs["officephone"].ToString() + "',";
                    mySql = mySql + "'" + hs["officephone2"].ToString() + "',";
                    mySql = mySql + "'" + hs["officephone3"].ToString() + "',";
                    mySql = mySql + "'" + hs["officefax"].ToString() + "',";
                    mySql = mySql + "'" + hs["postcode"].ToString() + "',";
                    mySql = mySql + "'" + hs["systemid"].ToString() + "')";
                }
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public string GetSettingConfigValue(string systemId, string configid)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  configvalue from t_cfg_sysconfig  where  configid='" + configid + "' and  systemid ='" + systemId + "'";
                return dataAccess.Get_DataString(mySql, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from t_cfg_priv_func  where privcode='" + hs["privcode"].ToString() + "'";
                int res = dataAccess.Execute_NonQuery(mySql);
                mySql = "delete  from t_cfg_priv  where privcode='" + hs["privcode"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 获取权限信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSerPrivInfo(string systemId, string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from  t_cfg_priv  where systemId='" + systemId + "'  and  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 添加权限信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertSerPrivInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_cfg_priv (systemid,privcode,privname,remark) values(";
                mySql = mySql + "'" + hs["systemid"].ToString() + "',";
                mySql = mySql + "'" + hs["privcode"].ToString() + "',";
                mySql = mySql + "'" + hs["privname"].ToString() + "',";
                mySql = mySql + "'" + hs["remark"].ToString() + "')";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_cfg_priv  set ";
                mySql = mySql + "systemid='" + hs["systemid"].ToString() + "',";
                mySql = mySql + "privname='" + hs["privname"].ToString() + "',";
                mySql = mySql + "remark='" + hs["remark"].ToString() + "'";
                mySql = mySql + " where privcode='" + hs["privcode"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="privcode"></param>
        /// <returns></returns>
        public DataTable GetSerPrivFunc(string privcode)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from  t_cfg_priv_func  where privcode='" + privcode + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public int DeleteSerRoleInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from t_cfg_role_priv  where rolecode='" + hs["rolecode"].ToString() + "'";
                int res = dataAccess.Execute_NonQuery(mySql);
                mySql = "delete  from t_cfg_role  where rolecode='" + hs["rolecode"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSerRoleInfo(string systemId, string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from  t_cfg_role  where systemId='" + systemId + "'  and  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public DataTable GetSerRolePriv(string rolecode)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from  t_cfg_role_priv  where rolecode='" + rolecode + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public int InsertSerRoleInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_cfg_role (systemid,rolecode,rolename,roledesc,remark) values(";
                mySql = mySql + "'" + hs["systemid"].ToString() + "',";
                mySql = mySql + "'" + hs["rolecode"].ToString() + "',";
                mySql = mySql + "'" + hs["rolename"].ToString() + "',";
                mySql = mySql + "'" + hs["roledesc"].ToString() + "',";
                mySql = mySql + "'" + hs["remark"].ToString() + "')";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int UpdateSerRoleInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_cfg_role  set ";
                mySql = mySql + "systemid='" + hs["systemid"].ToString() + "',";
                mySql = mySql + "rolename='" + hs["rolename"].ToString() + "',";
                mySql = mySql + "roledesc='" + hs["roledesc"].ToString() + "',";
                mySql = mySql + "remark='" + hs["remark"].ToString() + "'";
                mySql = mySql + " where rolecode='" + hs["rolecode"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int InsertPrivFunc(string privcode, List<string> funcids)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from t_cfg_priv_func  where privcode='" + privcode + "'";
                int res = dataAccess.Execute_NonQuery(mySql);
                int ires = 0;
                for (int i = 0; i < funcids.Count; i++)
                {
                    mySql = "insert into  t_cfg_priv_func (privcode,func_id) values(";
                    mySql = mySql + "'" + privcode + "',";
                    mySql = mySql + "'" + funcids[i] + "')";
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

        public int InsertRolePriv(string rolecode, List<string> privcodes)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_cfg_role_priv  where rolecode='" + rolecode + "'";
                int res = dataAccess.Execute_NonQuery(mySql);
                int ires = 0;
                for (int i = 0; i < privcodes.Count; i++)
                {
                    mySql = "insert into  t_cfg_role_priv (rolecode,privcode) values(";
                    mySql = mySql + "'" + rolecode + "',";
                    mySql = mySql + "'" + privcodes[i] + "')";
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

        public DataTable GetSerUserRole(string usercode)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select * from  t_cfg_user_role  where usercode='" + usercode + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public int InsertUserRole(string usercode, string rolecode)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete  from t_cfg_user_role  where usercode='" + usercode + "'";
                int res = dataAccess.Execute_NonQuery(mySql);
                int ires = 0;

                mySql = "insert into  t_cfg_user_role (usercode,rolecode) values(";
                mySql = mySql + "'" + usercode + "',";
                mySql = mySql + "'" + rolecode + "')";
                ires = dataAccess.Execute_NonQuery(mySql);

                return ires;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        #endregion ISettingManager 成员

        public DataTable GetDirectionInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  *  from t_cfg_direction  where  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得方向字典信息(不带条件)
        /// </summary>
        /// <returns></returns>
        public DataTable GetDirectionInfo()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  *  from t_cfg_direction   ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得地点信息（带条件）
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetLocationInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  * ,f_get_departname(departid)  from t_cfg_location    where  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得地点信息（带条件）
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetLocationInfoNew(string where, string startRow, string endRow)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  * ,f_get_departname(departid)  from t_cfg_location    where  " + where + " limit " + startRow + "," + (Convert.ToInt32(endRow) - Convert.ToInt32(startRow)) + "";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得地点信息总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetLocationInfoCount(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  count(*)  from t_cfg_location    where  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得地点信息（带条件）
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAllLocationInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select   a.location_id, a.location_name, a. departid,  f_get_value ('departname', 't_cfg_department', 'departid', a.departid) as departname,  a.systemid, a.location_section,  a.location_road, a.location_kilometer, a.location_police,  a.areaid, rownum from  vt_cfg_location a    where  " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询需要审核的地点信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetCheckLocation(string where, string userid, string shcs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select   a.location_id, a.location_name, a. departid,  f_get_departname (a.departid) as departname,  a.systemid, a.location_section,  a.location_road, a.location_kilometer, a.location_police,  a.areaid from  t_cfg_location a ,t_tms_user_location b   where a.location_id=b.location_id and b.user_id='" + userid + "' and check_times='" + shcs + "'";
                DataTable dt = dataAccess.Get_DataTable(mySql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    mySql = "select   a.location_id, a.location_name, a. departid,  f_get_departname (a.departid) as departname,  a.systemid, a.location_section,  a.location_road, a.location_kilometer, a.location_police,  a.areaid from  vt_cfg_location a    where  " + where;
                    return dataAccess.Get_DataTable(mySql);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得地点信息（带条件）
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetAllLocationInfos(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select   a.location_id, a.location_name, a. departid,  f_get_value ('departname', 't_cfg_department', 'departid', a.departid) as departname,  a.systemid, a.location_section,  a.location_road, a.location_kilometer, a.location_police,  a.areaid, rownum from  t_cfg_location a    where  " + where + " order by a.location_name asc";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得地点信息（不带条件）
        /// </summary>
        /// <returns></returns>
        public DataTable GetLocationInfo()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select  *  from vt_cfg_location   ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 删除方向信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteDirectionInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                if (hs.ContainsKey("direction_id"))
                {
                    mySql = "delete from t_cfg_direction  where station_id='" + hs["station_id"].ToString() + "' and direction_id='" + hs["direction_id"].ToString() + "'";
                }
                else
                {
                    mySql = "delete from t_cfg_direction  where station_id='" + hs["station_id"].ToString() + "'";
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
        /// 删除地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int DeleteLocationInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "delete from t_cfg_location  where location_id='" + hs["location_id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_cfg_direction (station_id,direction_id,direction_name,direction_desc) select ";
                mySql = mySql + "'" + hs["station_id"].ToString() + "',";
                mySql = mySql + "'" + hs["direction_id"].ToString() + "' ,";
                mySql = mySql + "f_to_name ('240025','" + hs["direction_id"].ToString() + "'),";
                mySql = mySql + "f_to_name ('240025','" + hs["direction_id"].ToString() + "') from dual ";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 插入地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int InsertLocationInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into  t_cfg_location (location_id,location_name,systemid,departid) values(";
                mySql = mySql + "'" + hs["location_id"].ToString() + "',";
                mySql = mySql + "'" + hs["location_name"].ToString() + "',";
                mySql = mySql + "'" + hs["systemid"].ToString() + "',";
                mySql = mySql + "'" + hs["departid"].ToString() + "' )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新方向信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateDirectionInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_cfg_direction  set ";
                mySql = mySql + "station_id='" + hs["station_id"].ToString() + "',";
                mySql = mySql + "direction_desc='" + hs["direction_desc"].ToString() + "'";
                mySql = mySql + " where direction_id='" + hs["direction_id"].ToString() + "' and  station_id='" + hs["station_id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateLocationInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_cfg_location  set ";
                mySql = mySql + Common.GetHashtableStr(hs, "departid", "departid");
                mySql = mySql + "location_name='" + hs["location_name"].ToString() + "'";
                mySql = mySql + " where location_id='" + hs["location_id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdatePeccLocationInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_cfg_location  set ";
                mySql = mySql + Common.GetHashtableStr(hs, "departid", "departid");
                mySql = mySql + "location_name='" + hs["location_name"].ToString() + "',";
                mySql = mySql + "location_section='" + hs["location_section"].ToString() + "',";
                mySql = mySql + "location_road='" + hs["location_road"].ToString() + "',";
                mySql = mySql + "location_kilometer='" + hs["location_kilometer"].ToString() + "',";
                mySql = mySql + "location_police='" + hs["location_police"].ToString() + "',";
                mySql = mySql + "location_device='" + hs["location_device"].ToString() + "',";
                mySql = mySql + "areaid='" + hs["areaid"].ToString() + "'";
                mySql = mySql + " where location_id='" + hs["location_id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新地点信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdateSuspicionLocationInfo(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_cfg_location  set ";
                mySql = mySql + Common.GetHashtableStr(hs, "departid", "departid");
                mySql = mySql + "location_name='" + hs["location_name"].ToString() + "',";
                mySql = mySql + Common.GetHashtableStr(hs, "isupload", "isupload");
                mySql = mySql + "jcbkid='" + hs["jcbkid"].ToString() + "'";
                mySql = mySql + " where location_id='" + hs["location_id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 查询模版
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTemplateInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                if (where.IndexOf("func") >= 0)
                {
                    mySql = "SELECT funcid AS pageid, funcname AS pagename, funcaddress AS pageurl, '../Images/Template/page.png' AS pageimageurl FROM t_sys_function WHERE funcid NOT IN ('010101','010102','010103','010104') ORDER BY  funcid asc";
                }
                else
                {
                    mySql = "select  *  from t_cfg_template   where " + where;
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
        /// 查询对应页面模版
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetTemplatePageInfo(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = @"SELECT usercode, VideoTemplate1,f_get_template(VideoTemplate1) AS VideoTemplate1Page, VideoTemplate2,f_get_template(VideoTemplate2) AS VideoTemplate2Page, DataTemplate1,
f_get_template(DataTemplate1) AS DataTemplate1Page, DataTemplate2,f_get_template(DataTemplate2) AS DataTemplate2Page, DataTemplate3,
f_get_template(DataTemplate3) AS DataTemplate3Page, DataTemplate4,f_get_template(DataTemplate4) AS DataTemplate4Page, ListTemplate1,
f_get_template(ListTemplate1) AS ListTemplate1Page, ListTemplate2,f_get_template(ListTemplate2) AS ListTemplate2Page, ListTemplate3,
f_get_template(ListTemplate3) AS ListTemplate3Page, UserTemplate,f_get_funcaddress(UserTemplate) AS UserTemplatePage FROM t_cfg_template_user WHERE  1=1 and " + where;
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                mySql = "SELECT UserBackGround FROM t_cfg_template_user WHERE usercode='" + userCode + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
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
            string mySql = string.Empty;
            try
            {
                if (GeXhExist("t_cfg_template_user", "usercode", usercode) > 0)
                {
                    mySql = "update t_cfg_template_user set ";
                    mySql += templateareaid + " ='" + pageid + "'";
                    mySql += " where     usercode='" + usercode + "'";
                }
                else
                {
                    mySql = "insert into  t_cfg_template_user (usercode," + templateareaid + ") values (";
                    mySql = mySql + "'" + usercode + "',";
                    mySql = mySql + "'" + pageid + "'";
                    mySql = mySql + " )";
                }
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }
    }
}