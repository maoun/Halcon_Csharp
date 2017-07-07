using System;
using System.Data;
using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class FacilityManager : IFacilityManager
    {
        #region DeviceManager 成员

        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        private MyNet.Common.Data.DataAccess dataAccess;

        public FacilityManager()
        {
            //dataAccess = dac.GetDataAccess(WebConfig.DataAccessName);
            dataAccess = GetDataAccess.Init();
        }

        public FacilityManager(string dataAccessName)
        {
            dataAccess = dac.GetDataAccess(dataAccessName);
        }

        #endregion DeviceManager 成员

        #region IFacilityManager 成员

        public DataTable Facility(string where)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DataTable getFacility(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select id,facilities_id,facilities_name,facilities_mode, facilities_type,department,deviceuse,facilities_even,Intersection_Number,Manufacturer from t_dev_facilities   where " + where + "  ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public DataTable getFacilityid(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select * from t_dev_facilities   where id= '" + id + "'  ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public int insertFacility_SignageMark(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into T_DEV_FACILITIES (id,facilities_id,facilities_name,facilities_type,department,facilities_mode,facilities_time,deviceuse,facilities_even,mark_form,";
                mySql = mySql + " mark_type,mark_width,mark_hight,mark_fwidth,mark_fhight,intersection_number,intersection_location,mark_color,mark_function,mark_drawing,base_specification,manufacturer) values (";
                mySql = mySql + "'" + hs["id"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_id"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_name"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_type"].ToString() + "',";
                mySql = mySql + "'" + hs["department"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_mode"].ToString() + "',";
                mySql = mySql + "to_date('" + hs["facilities_time"].ToString() + "','yyyy-mm-dd hh24:mi:ss'),";
                mySql = mySql + "'" + hs["deviceuse"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_even"].ToString() + "',";
                mySql = mySql + "'" + hs["mark_form"].ToString() + "',";
                mySql = mySql + "'" + hs["mark_type"].ToString() + "',";
                mySql = mySql + "'" + hs["mark_width"].ToString() + "',";
                mySql = mySql + "'" + hs["mark_hight"].ToString() + "',";
                mySql = mySql + "'" + hs["mark_fwidth"].ToString() + "',";
                mySql = mySql + "'" + hs["mark_fhight"].ToString() + "',";
                mySql = mySql + "'" + hs["intersection_number"].ToString() + "',";
                mySql = mySql + "'" + hs["intersection_location"].ToString() + "',";
                mySql = mySql + "'" + hs["mark_color"].ToString() + "',";
                mySql = mySql + "'" + hs["mark_function"].ToString() + "',";
                mySql = mySql + "'" + hs["mark_drawing"].ToString() + "',";
                mySql = mySql + "'" + hs["base_specification"].ToString() + "',";
                mySql = mySql + "'" + hs["manufacturer"].ToString() + "'";
                mySql = mySql + " )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int insertFacility_Isolation(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into T_DEV_FACILITIES (id,facilities_id,facilities_name,facilities_type,department,facilities_mode,facilities_time,deviceuse,facilities_even,construction_factory,";
                mySql = mySql + "open_count,installation_length,pipeline_type,pipeline_material,intersection_number,intersection_location,manufacturer) values (";
                mySql = mySql + "'" + hs["id"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_id"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_name"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_type"].ToString() + "',";
                mySql = mySql + "'" + hs["department"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_mode"].ToString() + "',";
                mySql = mySql + "to_date('" + hs["facilities_time"].ToString() + "','yyyy-mm-dd hh24:mi:ss'),";
                mySql = mySql + "'" + hs["deviceuse"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_even"].ToString() + "',";
                mySql = mySql + "'" + hs["construction_factory"].ToString() + "',";
                mySql = mySql + "'" + hs["open_count"].ToString() + "',";
                mySql = mySql + "'" + hs["installation_length"].ToString() + "',";
                mySql = mySql + "'" + hs["pipeline_type"].ToString() + "',";
                mySql = mySql + "'" + hs["pipeline_material"].ToString() + "',";
                mySql = mySql + "'" + hs["intersection_number"].ToString() + "',";
                mySql = mySql + "'" + hs["intersection_location"].ToString() + "',";
                mySql = mySql + "'" + hs["manufacturer"].ToString() + "'";
                mySql = mySql + " )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int insertFacility_Traffic(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = "insert into T_DEV_FACILITIES (id,facilities_id,facilities_name,facilities_type,department,facilities_mode,facilities_time,deviceuse,facilities_even,command_direction,facilities_ip,";
                mySql = mySql + "semaphore,semaphore_type,install_lights,basis_size,intersection_number,intersection_location,manufacturer) values (";
                mySql = mySql + "'" + hs["id"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_id"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_name"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_type"].ToString() + "',";
                mySql = mySql + "'" + hs["department"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_mode"].ToString() + "',";
                mySql = mySql + "to_date('" + hs["facilities_time"].ToString() + "','yyyy-mm-dd hh24:mi:ss'),";
                mySql = mySql + "'" + hs["deviceuse"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_even"].ToString() + "',";
                mySql = mySql + "'" + hs["command_direction"].ToString() + "',";
                mySql = mySql + "'" + hs["facilities_ip"].ToString() + "',";
                mySql = mySql + "'" + hs["semaphore"].ToString() + "',";
                mySql = mySql + "'" + hs["semaphore_type"].ToString() + "',";
                mySql = mySql + "'" + hs["install_lights"].ToString() + "',";
                mySql = mySql + "'" + hs["basis_size"].ToString() + "',";
                mySql = mySql + "'" + hs["intersection_number"].ToString() + "',";
                mySql = mySql + "'" + hs["intersection_location"].ToString() + "',";
                mySql = mySql + "'" + hs["manufacturer"].ToString() + "'";
                mySql = mySql + " )";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int updateFacility_Traffic(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " update T_DEV_FACILITIES set ";
                //mySql = mySql + "facilities_id='" + hs["facilities_id"].ToString() + "',";
                mySql = mySql + "facilities_name='" + hs["facilities_name"].ToString() + "',";
                mySql = mySql + "facilities_type='" + hs["facilities_type"].ToString() + "',";
                mySql = mySql + "department='" + hs["department"].ToString() + "',";
                mySql = mySql + "facilities_mode='" + hs["facilities_mode"].ToString() + "',";
                mySql = mySql + "facilities_time=to_date('" + hs["facilities_time"].ToString() + "','yyyy-mm-dd hh24:mi:ss'),";
                mySql = mySql + "deviceuse='" + hs["deviceuse"].ToString() + "',";
                mySql = mySql + "facilities_even='" + hs["facilities_even"].ToString() + "',";
                mySql = mySql + "command_direction='" + hs["command_direction"].ToString() + "',";
                mySql = mySql + "facilities_ip='" + hs["facilities_ip"].ToString() + "',";
                mySql = mySql + "semaphore='" + hs["semaphore"].ToString() + "',";
                mySql = mySql + "semaphore_type='" + hs["semaphore_type"].ToString() + "',";
                mySql = mySql + "install_lights='" + hs["install_lights"].ToString() + "',";
                mySql = mySql + "basis_size='" + hs["basis_size"].ToString() + "',";
                mySql = mySql + "intersection_number='" + hs["intersection_number"].ToString() + "',";
                mySql = mySql + "intersection_location='" + hs["intersection_location"].ToString() + "',";
                mySql = mySql + "manufacturer='" + hs["manufacturer"].ToString() + "' where id='" + hs["id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public DataTable selectid(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select count(*) from t_dev_facilities   where facilities_id= '" + id + "'  ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public DataTable selectlukou(string where)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " select LOCATION_ID,LOCATION_NAME  from T_CFG_LOCATION where 1=1 " + where + " ";
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return null;
            }
        }

        public int updateFacility_Isolation(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " update T_DEV_FACILITIES set ";
                //mySql = mySql + "facilities_id='" + hs["facilities_id"].ToString() + "',";
                mySql = mySql + "facilities_name='" + hs["facilities_name"].ToString() + "',";
                mySql = mySql + "facilities_type='" + hs["facilities_type"].ToString() + "',";
                mySql = mySql + "department='" + hs["department"].ToString() + "',";
                mySql = mySql + "facilities_mode='" + hs["facilities_mode"].ToString() + "',";
                mySql = mySql + "facilities_time=to_date('" + hs["facilities_time"].ToString() + "','yyyy-mm-dd hh24:mi:ss'),";
                mySql = mySql + "deviceuse='" + hs["deviceuse"].ToString() + "',";
                mySql = mySql + "facilities_even='" + hs["facilities_even"].ToString() + "',";

                mySql = mySql + "construction_factory='" + hs["construction_factory"].ToString() + "',";
                mySql = mySql + "open_count='" + hs["open_count"].ToString() + "',";
                mySql = mySql + "installation_length='" + hs["installation_length"].ToString() + "',";
                mySql = mySql + "pipeline_type='" + hs["pipeline_type"].ToString() + "',";
                mySql = mySql + "pipeline_material='" + hs["pipeline_material"].ToString() + "',";
                mySql = mySql + "intersection_number='" + hs["intersection_number"].ToString() + "',";
                mySql = mySql + "intersection_location='" + hs["intersection_location"].ToString() + "',";
                mySql = mySql + "manufacturer='" + hs["manufacturer"].ToString() + "' where id='" + hs["id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int updateFacility_SignageMark(System.Collections.Hashtable hs)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " update T_DEV_FACILITIES set ";
                //mySql = mySql + "facilities_id='" + hs["facilities_id"].ToString() + "',";
                mySql = mySql + "facilities_name='" + hs["facilities_name"].ToString() + "',";
                mySql = mySql + "facilities_type='" + hs["facilities_type"].ToString() + "',";
                mySql = mySql + "department='" + hs["department"].ToString() + "',";
                mySql = mySql + "facilities_mode='" + hs["facilities_mode"].ToString() + "',";
                mySql = mySql + "facilities_time=to_date('" + hs["facilities_time"].ToString() + "','yyyy-mm-dd hh24:mi:ss'),";
                mySql = mySql + "deviceuse='" + hs["deviceuse"].ToString() + "',";
                mySql = mySql + "facilities_even='" + hs["facilities_even"].ToString() + "',";
                mySql = mySql + "mark_form='" + hs["mark_form"].ToString() + "',";
                mySql = mySql + "mark_type='" + hs["mark_type"].ToString() + "',";
                mySql = mySql + "mark_width='" + hs["mark_width"].ToString() + "',";
                mySql = mySql + "mark_hight='" + hs["mark_hight"].ToString() + "',";
                mySql = mySql + "mark_fwidth='" + hs["mark_fwidth"].ToString() + "',";
                mySql = mySql + "mark_fhight='" + hs["mark_fhight"].ToString() + "',";
                mySql = mySql + "intersection_number='" + hs["intersection_number"].ToString() + "',";
                mySql = mySql + "intersection_location='" + hs["intersection_location"].ToString() + "',";
                mySql = mySql + "mark_color='" + hs["mark_color"].ToString() + "',";
                mySql = mySql + "mark_function='" + hs["mark_function"].ToString() + "',";
                mySql = mySql + "mark_drawing='" + hs["mark_drawing"].ToString() + "',";
                mySql = mySql + "base_specification='" + hs["base_specification"].ToString() + "',";
                mySql = mySql + "manufacturer='" + hs["manufacturer"].ToString() + "' where id='" + hs["id"].ToString() + "'";
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        public int DeleteFacility_SignageMark(string id)
        {
            string mySql = string.Empty;
            try
            {
                mySql = " delete  from T_DEV_FACILITIES where id='" + id + "'";

                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(mySql + ex.Message);
                return -1;
            }
        }

        #endregion IFacilityManager 成员
    }
}