using System;
using System.Data;
using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class ServiceManager : IServiceManager
    {
        #region DeviceManager 成员

        /// <summary>
        ///
        /// </summary>
        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        /// <summary>
        ///
        /// </summary>
        private MyNet.Common.Data.DataAccess dataAccess;

        /// <summary>
        ///
        /// </summary>
        public ServiceManager()
        {
            //dataAccess = dac.GetDataAccess(WebConfig.DataAccessName);
            dataAccess = GetDataAccess.Init();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataAccessName"></param>
        public ServiceManager(string dataAccessName)
        {
            dataAccess = dac.GetDataAccess(dataAccessName);
        }

        #endregion DeviceManager 成员

        #region IDeviceManager 成员

        /// <summary>
        ///获得人员唯一编号usercode
        /// </summary>
        /// <param name="head"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        public string GetRecordID(string head, int totalLength)
        {
            string mySql = string.Empty;
            try
            {
                int len = totalLength - head.Length;
                mySql = "select  concat('" + head + "',LPAD(CAST(nextval('seq_id') AS CHAR)," + len + ",'0')) ";
                return dataAccess.Get_DataString(mySql, 0);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return "0".PadLeft(totalLength, '0');
            }
        }

        /// <summary>
        /// 查询人员信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSevice(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select usercode,name,f_to_name('011005',sex) as sex,DATE_FORMAT(birthday,'%Y-%m-%d %H:%i:%s') as birthday,address,phone,remark,a.officephone,a.departid, siren,idno,ranks,f_to_name('013013',profession) as profession ,f_to_name('013013',preparationtype) as preparationtype,secretlevel,handsets,handsetscode,handsetsgroup,policeequipment,handsetscode,profession,sex,    b.departname as departidms,preparationtype from t_ser_person  a, t_cfg_department b where a.departid=b.departid and 1=1  " + where + "";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询人员信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSeviceby(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select usercode,name,f_to_name('013012',preparationtype) as codedesc from t_ser_person where 1=1  " + where + "";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        ///获取重点车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetVehicles(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select *  from t_keycar_carinfo where 1=1  " + where + "";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }
        /// <summary>
        ///获取重点车辆信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetVehiclese(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select id,hphm,f_to_name('140001',hpzl) as hpzl,f_to_name('240050',ZDCLLX) as ZDCLLX,SJXM,HJHM,clxh,clpp,lxdh,f_get_departname(glbm) as glbm from t_keycar_carinfo where 1=1  " + where + "";
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
        /// <returns></returns>
        public DataTable GetArea()
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select  trl_t_sev_area.currval from t_sev_area ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                mySql = " select  seq_t_sev_area.nextval-1 from dual";
                return dataAccess.Get_DataTable(mySql);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable SlectArea(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select areaid,areaname,f_to_name('221001',areatype),f_to_name('221002',geotype),t_cfg_department.departname from t_sev_area,t_cfg_department where  t_sev_area.owner=t_cfg_department.departid " + where + "   order by areaid asc ";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable SlectAreaid(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select * from t_sev_area where areaid='" + id + "'";
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
        /// <returns></returns>
        public DataTable AreaCount()
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select count(*) from t_sev_area ";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable SlectCLASSid(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select t_sev_area_class.*, f_to_name('221003',dutyclass) as classtype from t_sev_area_class where areaid='" + id + "'";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable CountTime(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select count(*) from t_sev_area_time where aacid='" + id + "'";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable Classtype(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select   f_to_name('221003',dutyclass) from t_sev_area_class where areaid='" + id + "'";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable SlectTimeid(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select * from t_sev_area_time where 1=1  and aacid='" + id + "' order by catid asc ";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable CountPerson(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select count(*) from t_sev_area_person where areaid='" + id + "'";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable CountPersonbak(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select count(*) from t_sev_area_person_bak where areaid='" + id + "'";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable CountClass(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select count(*) from t_sev_area_class where areaid='" + id + "'";
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
        /// <returns></returns>
        public DataTable GetClass()
        {
            string mySql = string.Empty;
            try
            {
                //mySql = " select SMSLOG_CLASS.currval from T_SEV_AREA_CLASS ";
                mySql = " select max(aacid) from t_sev_area_class ";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable SlectPersonid(string id)
        {
            string mySql = string.Empty;
            try
            {
                //mySql = " select * from T_SEV_AREA_PERSON where 1=1 and  AREAID='"+id+"' order by personidx asc ";
                mySql = " select t_ser_person.name,t_sev_area_person.* from t_sev_area_person ,t_ser_person  where 1=1 and  t_sev_area_person.dutyperson=t_ser_person.usercode and areaid='" + id + "'  order by personidx asc";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable SlectPersonbakid(string id)
        {
            string mySql = string.Empty;
            try
            {
                //mySql = " select * from T_SEV_AREA_PERSON where 1=1 and  AREAID='"+id+"' order by personidx asc ";
                mySql = " select t_ser_person.name,t_sev_area_person_bak.* from t_sev_area_person_bak ,t_ser_person  where 1=1 and  t_sev_area_person_bak.dutyperson=t_ser_person.usercode and areaid='" + id + "' order by personidx asc";
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
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable Postadtime(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select f_get_value('departname','t_cfg_department','departid',deptid ) as cjjgms,f_to_name('231010',dutypost) as dutypost ,dutystarttime,dutyendtime,dutypersonnum,dutystardata from t_sev_postandtime where 1=1 " + where + " ";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable SlectPersontype(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select count(*) from t_sev_person where  1=1 and dutypost='" + id + "'";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable Countpostadtimetype(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select count(*) from t_sev_postandtime where  1=1 and dutypost='" + id + "'";
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
        /// <returns></returns>
        public DataTable CountPosttime()
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select count(*) from t_sev_postandtime ";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable Slect_sev_Person(string id)
        {
            string mySql = string.Empty;
            try
            {
                // mySql = "  select confid,t_sev_person.deptid,t_ser_person.name,f_get_value('DEPARTNAME','t_cfg_department','departid','152501000001' ) AS CJJGMS,f_to_name('231010',dutypost) as renyuan,dutypost,personidx from t_sev_person ,t_ser_person  where 1=1 and  t_sev_person.deptid=t_ser_person.id  and  dutypost='" + id + "'   order by personidx asc ";
                mySql = "select confid,deptid,t_ser_person.name,f_get_value('departname','t_cfg_department','departid',deptid) as cjjgms,f_to_name('231010',dutypost) as renyuan,dutypost,personidx,dutyperson from t_sev_person,t_ser_person where t_sev_person.dutyperson=t_ser_person.usercode  and   dutypost='" + id + "' order by personidx asc ";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable Count_sev_Person(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "  select count(*) from  t_sev_person where dutypost='" + id + "'";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable Slect_sev_postandtime(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "  select dutystarttime,dutyendtime, dutypersonnum,dutystardata from t_sev_postandtime  where dutypost='" + id + "'";
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
        /// <param name="hs"></param>
        /// <returns></returns>
        public int insertTrafficControl(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into t_tfc_trafficcontrol (ID,PROGRAMNAME,CONTROLLOCATION,LOCATIONDESCRIPTION,CONTROLTYPE,STARTTIME,STOPTIME,MANAGERS,MANAGERSPHONE,DIRECTION,OCCUPATION,ACCESSIBLE,ORGANIZATION) values (";
                mySql = mySql + "'" + hs["ID"].ToString() + "',";
                mySql = mySql + "'" + hs["PROGRAMNAME"].ToString() + "',";
                mySql = mySql + "'" + hs["CONTROLLOCATION"].ToString() + "',";
                mySql = mySql + "'" + hs["LOCATIONDESCRIPTION"].ToString() + "',";
                mySql = mySql + "'" + hs["CONTROLTYPE"].ToString() + "',";
                mySql = mySql + "STR_TO_DATE('" + hs["STARTTIME"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "STR_TO_DATE('" + hs["STOPTIME"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "'" + hs["MANAGERS"].ToString() + "',";
                mySql = mySql + "'" + hs["MANAGERSPHONE"].ToString() + "',";
                mySql = mySql + "'" + hs["DIRECTION"].ToString() + "',";
                mySql = mySql + "'" + hs["OCCUPATION"].ToString() + "',";
                mySql = mySql + "'" + hs["ACCESSIBLE"].ToString() + "',";
                mySql = mySql + "'" + hs["ORGANIZATION"].ToString() + "'";
                mySql = mySql + "   )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int updateTrafficControl(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_tfc_trafficcontrol  set ";
                mySql = mySql + "PROGRAMNAME='" + hs["PROGRAMNAME"].ToString() + "',";
                mySql = mySql + "CONTROLLOCATION='" + hs["CONTROLLOCATION"].ToString() + "',";
                mySql = mySql + "LOCATIONDESCRIPTION='" + hs["LOCATIONDESCRIPTION"].ToString() + "',";
                mySql = mySql + "CONTROLTYPE='" + hs["CONTROLTYPE"].ToString() + "',";
                mySql = mySql + "STARTTIME=STR_TO_DATE('" + hs["STARTTIME"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "STOPTIME=STR_TO_DATE('" + hs["STOPTIME"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "MANAGERS='" + hs["MANAGERS"].ToString() + "',";
                mySql = mySql + "MANAGERSPHONE='" + hs["MANAGERSPHONE"].ToString() + "',";
                mySql = mySql + "DIRECTION='" + hs["DIRECTION"].ToString() + "',";
                mySql = mySql + "OCCUPATION='" + hs["OCCUPATION"].ToString() + "',";
                mySql = mySql + "ACCESSIBLE='" + hs["ACCESSIBLE"].ToString() + "',";
                mySql = mySql + "ORGANIZATION='" + hs["ORGANIZATION"].ToString() + "'";
                mySql = mySql + " where ID='" + hs["ID"].ToString() + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteTrafficControl(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_tfc_trafficcontrol where ID='" + id + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable SeleteTrafficControl(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "  select id,programname,controllocation,locationdescription,f_to_name('221004',controltype),to_char(starttime,'%Y-%m-%d %H:%i:%s'),to_char(stoptime,'%Y-%m-%d %H:%i:%s'),managers,managersphone,f_to_name('221006',direction),occupation,accessible, f_to_name('221005',organization),f_get_value('location_name', 't_cfg_location', 'location_id', controllocation) as controllocationms   from t_tfc_trafficcontrol where 1=1 " + where + "";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable SeleteTrafficControlid(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "  select id,programname,controllocation,locationdescription,controltype,starttime,stoptime,managers,managersphone,direction,occupation,accessible,organization from t_tfc_trafficcontrol where id='" + id + "'  ";
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
        /// <param name="hs"></param>
        /// <returns></returns>
        public int insertJeeves(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into T_CTJ_CONSTRUCTION (ID,BuildName,BuildAddress,BuildPerson,BuildPhone,ConstructionName,ConstructionAddress,ConstructionPerson,ConstructionPhone,JeevesLocation,Starpoint,";
                mySql = mySql + "Stoppoint,RoadStructure,JeevesType,JeevesForm,Reasons,Condition,Organization,Department,Occupation,Accessible,startime,stoptime,Description,Managers,Managersphone) values ( ";
                mySql = mySql + "'" + hs["ID"].ToString() + "',";
                mySql = mySql + "'" + hs["BuildName"].ToString() + "',";
                mySql = mySql + "'" + hs["BuildAddress"].ToString() + "',";
                mySql = mySql + "'" + hs["BuildPerson"].ToString() + "',";
                mySql = mySql + "'" + hs["BuildPhone"].ToString() + "',";
                mySql = mySql + "'" + hs["ConstructionName"].ToString() + "',";
                mySql = mySql + "'" + hs["ConstructionAddress"].ToString() + "',";
                mySql = mySql + "'" + hs["ConstructionPerson"].ToString() + "',";
                mySql = mySql + "'" + hs["ConstructionPhone"].ToString() + "',";
                mySql = mySql + "'" + hs["JeevesLocation"].ToString() + "',";
                mySql = mySql + "'" + hs["Starpoint"].ToString() + "',";
                mySql = mySql + "'" + hs["Stoppoint"].ToString() + "',";
                mySql = mySql + "'" + hs["RoadStructure"].ToString() + "',";
                //mySql = mySql + "'" + hs["JeevesLength"].ToString() + "',";
                //mySql = mySql + "'" + hs["JeevesWidth"].ToString() + "',";
                mySql = mySql + "'" + hs["JeevesType"].ToString() + "',";
                mySql = mySql + "'" + hs["JeevesForm"].ToString() + "',";
                mySql = mySql + "'" + hs["Reasons"].ToString() + "',";
                mySql = mySql + "'" + hs["Condition"].ToString() + "',";
                mySql = mySql + "'" + hs["Organization"].ToString() + "',";
                mySql = mySql + "'" + hs["Department"].ToString() + "',";
                mySql = mySql + "'" + hs["Occupation"].ToString() + "',";
                mySql = mySql + "'" + hs["Accessible"].ToString() + "',";
                mySql = mySql + "STR_TO_DATE('" + hs["startime"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "STR_TO_DATE('" + hs["stoptime"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "'" + hs["Description"].ToString() + "',";
                mySql = mySql + "'" + hs["Managers"].ToString() + "',";
                mySql = mySql + "'" + hs["Managersphone"].ToString() + "'";
                mySql = mySql + "   )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int updateJeeves(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  T_CTJ_CONSTRUCTION  set ";
                mySql = mySql + "BuildName='" + hs["BuildName"].ToString() + "',";
                mySql = mySql + "BuildAddress='" + hs["BuildAddress"].ToString() + "',";
                mySql = mySql + "BuildPerson='" + hs["BuildPerson"].ToString() + "',";
                mySql = mySql + "BuildPhone='" + hs["BuildPhone"].ToString() + "',";
                mySql = mySql + "ConstructionName='" + hs["ConstructionName"].ToString() + "',";
                mySql = mySql + "ConstructionAddress='" + hs["ConstructionAddress"].ToString() + "',";
                mySql = mySql + "ConstructionPerson='" + hs["ConstructionPerson"].ToString() + "',";
                mySql = mySql + "ConstructionPhone='" + hs["ConstructionPhone"].ToString() + "',";
                mySql = mySql + "JeevesLocation='" + hs["JeevesLocation"].ToString() + "',";
                mySql = mySql + "Starpoint='" + hs["Starpoint"].ToString() + "',";
                mySql = mySql + "Stoppoint='" + hs["Stoppoint"].ToString() + "',";
                mySql = mySql + "RoadStructure='" + hs["RoadStructure"].ToString() + "',";
                mySql = mySql + "JeevesType='" + hs["JeevesType"].ToString() + "',";
                mySql = mySql + "JeevesForm='" + hs["JeevesForm"].ToString() + "',";
                mySql = mySql + "Reasons='" + hs["Reasons"].ToString() + "',";
                mySql = mySql + "Condition='" + hs["Condition"].ToString() + "',";
                mySql = mySql + "Organization='" + hs["Organization"].ToString() + "',";
                mySql = mySql + "Department='" + hs["Department"].ToString() + "',";
                mySql = mySql + "Occupation='" + hs["Occupation"].ToString() + "',";
                mySql = mySql + "Accessible='" + hs["Accessible"].ToString() + "',";
                mySql = mySql + "startime=STR_TO_DATE('" + hs["startime"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "stoptime=STR_TO_DATE('" + hs["stoptime"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "Description='" + hs["Description"].ToString() + "',";
                mySql = mySql + "Managers='" + hs["Managers"].ToString() + "',";
                mySql = mySql + "Managersphone='" + hs["Managersphone"].ToString() + "'";

                mySql = mySql + " where ID='" + hs["ID"].ToString() + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteJeeves(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_ctj_construction where id='" + id + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable SeleteJeeves(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "  select id,buildname,buildaddress,buildperson,buildphone,constructionname,constructionaddress,constructionperson,constructionphone,jeeveslocation,starpoint,stoppoint,f_to_name('221007',roadstructure),f_to_name('221008',jeevestype),f_to_name('221009',jeevesform),reasons,condition,f_to_name('221005',organization),f_get_value('departname','t_cfg_department','departid',department) as cjjgms,occupation,accessible,to_char(startime,'%Y-%m-%d %H:%i:%s'),to_char(stoptime,'%Y-%m-%d %H:%i:%s'),description,managers,managersphone,f_get_value('location_name', 't_cfg_location', 'location_id', jeeveslocation) as jeeveslocationms  from t_ctj_construction where 1=1 " + where + "";
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
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable SeleteJeevesid(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "  select id,buildname,buildaddress,buildperson,buildphone,constructionname,constructionaddress,constructionperson,constructionphone,jeeveslocation,starpoint,stoppoint,roadstructure,jeevestype,jeevesform,reasons,condition,organization,department,occupation,accessible,startime,stoptime,description,managers,managersphone from t_ctj_construction where id='" + id + "'  ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int insertSevice(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into t_ser_person (USERCODE,NAME,SEX,BIRTHDAY,ADDRESS,PHONE,OFFICEPHONE,DEPARTID,SIREN,IDNO,RANKS,PROFESSION,PREPARATIONTYPE,SECRETLEVEL,HANDSETS,HANDSETSCODE,HANDSETSGROUP,POLICEEQUIPMENT) values (";
                mySql = mySql + "'" + hs["USERCODE"].ToString() + "',";
                mySql = mySql + "'" + hs["NAME"].ToString() + "',";
                mySql = mySql + "'" + hs["SEX"].ToString() + "',";
                mySql = mySql + "STR_TO_DATE('" + hs["BIRTHDAY"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "'" + hs["ADDRESS"].ToString() + "',";
                mySql = mySql + "'" + hs["PHONE"].ToString() + "',";
                mySql = mySql + "'" + hs["OFFICEPHONE"].ToString() + "',";
                mySql = mySql + "'" + hs["DEPARTID"].ToString() + "',";
                mySql = mySql + "'" + hs["SIREN"].ToString() + "',";
                mySql = mySql + "'" + hs["IDNO"].ToString() + "',";
                mySql = mySql + "'" + hs["RANKS"].ToString() + "',";
                mySql = mySql + "'" + hs["PROFESSION"].ToString() + "',";
                mySql = mySql + "'" + hs["PREPARATIONTYPE"].ToString() + "',";
                mySql = mySql + "'" + hs["SECRETLEVEL"].ToString() + "',";
                mySql = mySql + "'" + hs["HANDSETS"].ToString() + "',";
                mySql = mySql + "'" + hs["HANDSETSCODE"].ToString() + "',";
                mySql = mySql + "'" + hs["HANDSETSGROUP"].ToString() + "',";
                mySql = mySql + "'" + hs["POLICEEQUIPMENT"].ToString() + "'";
                //mySql = mySql + "" + hs["PHOTO"] + "";
                mySql = mySql + "   )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///添加重点车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int insertVehicles(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into T_KEYCAR_CARINFO (ID,HPHM,CLPP,CLXH,CJH,FDJH,HDKL,GZRQ,SJXM,SJHM,LXDH,GPS,HJHM,CLZT,RYZK,GLBM,ZDCLLX,GXSJ,HPZL,KSSJ,JSSJ,KKIDS) values (";
                mySql = mySql + "'" + hs["ID"].ToString() + "',";
                mySql = mySql + "'" + hs["HPHM"].ToString() + "',";
                mySql = mySql + "'" + hs["CLPP"].ToString() + "',";
                mySql = mySql + "'" + hs["CLXH"].ToString() + "',";
                mySql = mySql + "'" + hs["CJH"].ToString() + "',";
                mySql = mySql + "'" + hs["FDJH"].ToString() + "',";
                mySql = mySql + "'" + hs["HDKL"].ToString() + "',";
                mySql = mySql + "STR_TO_DATE('" + hs["GZRQ"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "'" + hs["SJXM"].ToString() + "',";
                mySql = mySql + "'" + hs["SJHM"].ToString() + "',";
                mySql = mySql + "'" + hs["LXDH"].ToString() + "',";
                mySql = mySql + "'" + hs["GPS"].ToString() + "',";
                mySql = mySql + "'" + hs["HJHM"].ToString() + "',";
                mySql = mySql + "'" + hs["CLZT"].ToString() + "',";
                mySql = mySql + "'" + hs["RYZK"].ToString() + "',";
                mySql = mySql + "'" + hs["GLBM"].ToString() + "',";
                mySql = mySql + "'" + hs["CLZL"].ToString() + "',";
                mySql = mySql + "now(),";
                mySql = mySql + "'" + hs["HPZL"].ToString() + "',";
                mySql = mySql + "'" + hs["KSSJ"].ToString() + "',";
                mySql = mySql + "'" + hs["JSSJ"].ToString() + "',";
                mySql = mySql + "'" + hs["KKIDS"].ToString() + "'";
                mySql = mySql + "   )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int insertarea(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into T_SEV_AREA ( AREAID,AREANAME,AREATYPE,GEOTYPE,OWNER,ISGROUP) values (";
                mySql = mySql + "seq_t_sev_area.nextval,";
                mySql = mySql + "'" + hs["AREANAME"].ToString() + "',";
                mySql = mySql + "'" + hs["AREATYPE"].ToString() + "',";
                mySql = mySql + "'" + hs["GEOTYPE"].ToString() + "',";
                mySql = mySql + "'" + hs["OWNER"].ToString() + "',";
                mySql = mySql + "'" + hs["ISGROUP"].ToString() + "'";
                mySql = mySql + "   )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int insertarea_person(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into t_sev_area_person (DEPTID,AREAID,DUTYPERSON,ISLEADER,PERSONIDX,GROUPIDX) values (";
                mySql = mySql + "'" + hs["DEPTID"].ToString() + "',";
                mySql = mySql + "'" + hs["AREAID"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYPERSON"].ToString() + "',";
                mySql = mySql + "'" + hs["ISLEADER"].ToString() + "',";
                mySql = mySql + "'" + hs["PERSONIDX"].ToString() + "',";
                mySql = mySql + "'" + hs["GROUPIDX"].ToString() + "'";
                mySql = mySql + "   )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int insertperson(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into T_SEV_PERSON (DEPTID,DUTYPOST,DUTYPERSON,PERSONIDX,ISHOLIDAY) values (";
                mySql = mySql + "'" + hs["DEPTID"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYPOST"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYPERSON"].ToString() + "',";
                mySql = mySql + "'" + hs["PERSONIDX"].ToString() + "',";
                mySql = mySql + " 1 ";
                mySql = mySql + "   )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int insertpostandtime(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into T_SEV_POSTANDTIME (DEPTID,DUTYPOST,DUTYSTARTTIME,DUTYENDTIME,DUTYPERSONNUM,DUTYSTARDATA) values (";
                mySql = mySql + "'" + hs["DEPTID"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYPOST"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYSTARTTIME"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYENDTIME"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYPERSONNUM"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYSTARDATA"].ToString() + "'";
                mySql = mySql + "   )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int insertarea_person_beixuan(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into T_SEV_AREA_PERSON_BAK (DEPTID,AREAID,DUTYPERSON,ISLEADER,PERSONIDX,GROUPIDX) values (";
                mySql = mySql + "'" + hs["DEPTID"].ToString() + "',";
                mySql = mySql + "'" + hs["AREAID"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYPERSON"].ToString() + "',";
                mySql = mySql + "'" + hs["ISLEADER"].ToString() + "',";
                mySql = mySql + "'" + hs["PERSONIDX"].ToString() + "',";
                mySql = mySql + "'" + hs["GROUPIDX"].ToString() + "'";
                mySql = mySql + "   )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int insertarea_class(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into T_SEV_AREA_CLASS (AACID,DEPTID,AREAID,DUTYCLASS,DUTYPERSONNUM,STARTDATE,ENDDATE,CLASSMODE) values (";
                mySql = mySql + "SEQ_T_SEV_AREA_CLASS.nextval ,";
                mySql = mySql + "'" + hs["DEPTID"].ToString() + "',";
                mySql = mySql + "'" + hs["AREAID"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYCLASS"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYPERSONNUM"].ToString() + "',";
                mySql = mySql + " STR_TO_DATE('" + hs["STARTDATE"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + " STR_TO_DATE('" + hs["ENDDATE"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "1";
                mySql = mySql + "   )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int insertarea_time(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into T_SEV_AREA_TIME (AACID,DUTYSTARTTIME,DUTYENDTIME ) values (";
                mySql = mySql + "'" + hs["AACID"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYSTARTTIME"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYENDTIME"].ToString() + "'";
                mySql = mySql + "   )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 更新人员信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int updateSevice(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  T_SER_PERSON  set ";
                mySql = mySql + "NAME='" + hs["NAME"].ToString() + "',";
                mySql = mySql + "SEX='" + hs["SEX"].ToString() + "',";
                mySql = mySql + "BIRTHDAY=STR_TO_DATE('" + hs["BIRTHDAY"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "ADDRESS='" + hs["ADDRESS"].ToString() + "',";
                mySql = mySql + "PHONE='" + hs["PHONE"].ToString() + "',";
                mySql = mySql + "OFFICEPHONE='" + hs["OFFICEPHONE"].ToString() + "',";
                mySql = mySql + "DEPARTID='" + hs["DEPARTID"].ToString() + "',";
                mySql = mySql + "SIREN='" + hs["SIREN"].ToString() + "',";
                mySql = mySql + "IDNO='" + hs["IDNO"].ToString() + "',";
                mySql = mySql + "RANKS='" + hs["RANKS"].ToString() + "',";
                mySql = mySql + "PROFESSION='" + hs["PROFESSION"].ToString() + "',";
                mySql = mySql + "PREPARATIONTYPE='" + hs["PREPARATIONTYPE"].ToString() + "',";
                mySql = mySql + "SECRETLEVEL='" + hs["SECRETLEVEL"].ToString() + "',";
                mySql = mySql + "HANDSETS='" + hs["HANDSETS"].ToString() + "',";
                mySql = mySql + "HANDSETSCODE='" + hs["HANDSETSCODE"].ToString() + "',";
                mySql = mySql + "HANDSETSGROUP='" + hs["HANDSETSGROUP"].ToString() + "',";
                mySql = mySql + "POLICEEQUIPMENT='" + hs["POLICEEQUIPMENT"].ToString() + "'";

                mySql = mySql + " WHERE USERCODE='" + hs["USERCODE"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///修改重点车辆信息
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int updateVehicles(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "update  t_keycar_carinfo  set ";
                mySql = mySql + "HPZL='" + hs["HPZL"].ToString() + "',";
                mySql = mySql + "HPHM='" + hs["HPHM"].ToString() + "',";
                mySql = mySql + "ZDCLLX='" + hs["CLZL"].ToString() + "',";
                mySql = mySql + "CLPP='" + hs["CLPP"].ToString() + "',";
                mySql = mySql + "CLXH='" + hs["CLXH"].ToString() + "',";
                mySql = mySql + "CJH='" + hs["CJH"].ToString() + "',";
                mySql = mySql + "FDJH='" + hs["FDJH"].ToString() + "',";
                mySql = mySql + "HDKL='" + hs["HDKL"].ToString() + "',";
                mySql = mySql + "GZRQ=STR_TO_DATE('" + hs["GZRQ"].ToString() + "','%Y-%m-%d %H:%i:%s'),";
                mySql = mySql + "SJXM='" + hs["SJXM"].ToString() + "',";
                mySql = mySql + "LXDH='" + hs["LXDH"].ToString() + "',";
                mySql = mySql + "GPS='" + hs["GPS"].ToString() + "',";
                mySql = mySql + "HJHM='" + hs["HJHM"].ToString() + "',";
                mySql = mySql + "CLZT='" + hs["CLZT"].ToString() + "',";
                mySql = mySql + "SJHM='" + hs["SJHM"].ToString() + "',";
                mySql = mySql + "RYZK='" + hs["RYZK"].ToString() + "',";
                mySql = mySql + "GXSJ=now(),";
                mySql = mySql + "GLBM='" + hs["GLBM"].ToString() + "',";
                mySql = mySql + "KSSJ='" + hs["KSSJ"].ToString() + "',";
                mySql = mySql + "JSSJ='" + hs["JSSJ"].ToString() + "',";
                mySql = mySql + "KKIDS='" + hs["KKIDS"].ToString() + "'";
                mySql = mySql + "WHERE ID='" + hs["ID"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///删除重点车辆信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteVehicles(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete from t_keycar_carinfo where id='" + id + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///删除警员信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteSevice(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_ser_person where usercode='" + id + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteArea(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_sev_area where areaid='" + id + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteClass(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_sev_area_class where areaid='" + id + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeletePerson(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_sev_area_person where areaid='" + id + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Deletetime(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_sev_area_time where aacid='" + id + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeletePerson_type(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_sev_person where dutypost='" + id + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Deletepostandtime_type(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from t_sev_postandtime where dutypost='" + id + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DataTable Count_sev_Person_To()
        {
            string mySql = string.Empty;
            try
            {
                mySql = "  select count(*) from  t_sev_postandtime ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        #endregion IDeviceManager 成员

        #region IServiceManager 成员

        public int UpdateSevPerson(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "begin ";
                mySql = mySql + "insert into t_sev_person (deptid,dutypost,dutyperson,personidx,isholiday) values (";
                mySql = mySql + "'" + hs["DEPTID"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYPOST"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYPERSON"].ToString() + "',";
                mySql = mySql + "'" + hs["PERSONIDX"].ToString() + "',";
                mySql = mySql + " 0 ";
                mySql = mySql + " );";
                mySql = mySql + "end ;";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public int UpdatePostandTime(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "begin ";
                mySql = mySql + " insert into t_sev_postandtime (deptid,dutypost,dutystarttime,dutyendtime,dutypersonnum,dutystardata) values (";
                mySql = mySql + "'" + hs["DEPTID"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYPOST"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYSTARTTIME"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYENDTIME"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYPERSONNUM"].ToString() + "',";
                mySql = mySql + "'" + hs["DUTYSTARDATA"].ToString() + "'";
                mySql = mySql + "   );";
                mySql = mySql + "end ;";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetSevPerson(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "select confid,deptid,t_ser_person.name,f_get_value('departname','t_cfg_department','departid',deptid) as cjjgms,f_to_name('231010',dutypost) as renyuan,dutypost,personidx,dutyperson from t_sev_person,t_ser_person where t_sev_person.dutyperson=t_ser_person.usercode  and  " + where + "order by personidx asc ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得部门人员总数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable PersonbyDepartid(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "  select count(*) from  t_ser_person where departid='" + id + "'";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 删除人员信息
        /// </summary>
        /// <param name="dutypost"></param>
        /// <param name="departid"></param>
        /// <returns></returns>
        public int DeletePerson(string dutypost, string departid)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " begin ";

                mySql = mySql + " delete  from t_sev_person where dutypost='" + dutypost + "' and deptid='" + departid + "';";
                mySql = mySql + " delete  from t_sev_postandtime where  dutypost='" + dutypost + "' and deptid='" + departid + "';";
                mySql = mySql + " end ;";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        #endregion IServiceManager 成员
    }
}