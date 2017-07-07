using MyNet.Atmcs.Uscmcp.Data;
using MyNet.Common.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Web.Services;

namespace MyNet.Atmcs.Uscmcp.Web
{
    /// <summary>
    /// DataService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class DataService : System.Web.Services.WebService
    {
        private static MyNet.Common.Data.DataAccess dataAccess;

        public DataService()
        {
            DataAccessCollections dac = new DataAccessCollections();
            //dataAccess = dac.GetDataAccess(Config.DataAccessName);
            dataAccess = GetDataAccess.Init();
        }

        [WebMethod(Description = "通过Sql语句获得一个DataSet")]
        public System.Data.DataSet Get_DataSet(string Sqlstr)
        {
            return dataAccess.Get_DataSet(Sqlstr);
        }

        [WebMethod(Description = "通过Sql语句获得一个DataSet")]
        public System.Data.DataSet Get_DataSet2(string Sqlstr, string dataAccessName)
        {
            DataAccessCollections dac = new DataAccessCollections();
            MyNet.Common.Data.DataAccess dAccess = dac.GetDataAccess(dataAccessName);
            return dAccess.Get_DataSet(Sqlstr);
        }

        [WebMethod(Description = "通过Sql语句查询一个值")]
        public string Getstr_BySql(string Sqlstr)
        {
            return dataAccess.Get_DataString(Sqlstr);
        }

        [WebMethod(Description = "通过Sql语句查询一个值")]
        public string Getstr_BySql2(string Sqlstr, string dataAccessName)
        {
            DataAccessCollections dac = new DataAccessCollections();
            MyNet.Common.Data.DataAccess dAccess = dac.GetDataAccess(dataAccessName);
            return dAccess.Get_DataString(Sqlstr);
        }

        [WebMethod(Description = "通过Sql语句和字段信息查询一个值")]
        public string GetStr_ByField(string Sqlstr, string Field)
        {
            return dataAccess.Get_DataString(Sqlstr, Field);
        }

        [WebMethod(Description = "通过Sql语句和字段信息查询一个值")]
        public string GetStr_ByField2(string Sqlstr, string Field, string dataAccessName)
        {
            DataAccessCollections dac = new DataAccessCollections();
            MyNet.Common.Data.DataAccess dAccess = dac.GetDataAccess(dataAccessName);
            return dAccess.Get_DataString(Sqlstr, Field);
        }

        [WebMethod(Description = "通过Sql语句和字段索引查询一个值")]
        public string GetStr_ByIdx(string Sqlstr, int Index)
        {
            return dataAccess.Get_DataString(Sqlstr, Index);
        }

        [WebMethod(Description = "通过Sql语句和字段索引查询一个值")]
        public string GetStr_ByIdx2(string Sqlstr, int Index, string dataAccessName)
        {
            DataAccessCollections dac = new DataAccessCollections();
            MyNet.Common.Data.DataAccess dAccess = dac.GetDataAccess(dataAccessName);
            return dataAccess.Get_DataString(Sqlstr, Index);
        }

        [WebMethod(Description = "执行一个sql语句，返回执行成功条数.返回值-1：执行错误")]
        public Int32 Execute_NonQuery(string Sqlstr)
        {
            return dataAccess.Execute_NonQuery(Sqlstr);
        }

        [WebMethod(Description = "执行一个sql语句，返回执行成功条数.返回值-1：执行错误")]
        public Int32 Execute_NonQuery2(string Sqlstr, string dataAccessName)
        {
            DataAccessCollections dac = new DataAccessCollections();
            MyNet.Common.Data.DataAccess dAccess = dac.GetDataAccess(dataAccessName);
            return dAccess.Execute_NonQuery(Sqlstr);
        }

        [WebMethod(Description = "执行一个存储过程，返回执行成功条数.返回值-1：执行错误")]
        public Int32 Execute_Procedure(string ProcedureName, DataSet DsDbParameter)
        {
            DataTable dt = DsDbParameter.Tables[0];
            DbParameter[] DbParameter = new DbParameter[dt.Rows.Count];

            for (int i = 0; i < DsDbParameter.Tables[0].Rows.Count; i++)
            {
                DbParameter[i] = new MySql.Data.MySqlClient.MySqlParameter(dt.Rows[i]["name"].ToString(), dt.Rows[i]["value"]);
                try
                {
                    if (dt.Rows[i]["type"] != null)
                    {
                        DbParameter[i].DbType = (DbType)int.Parse(dt.Rows[i]["type"].ToString());
                    }
                }
                catch
                {
                }
            }
            return dataAccess.Execute_Procedure(ProcedureName, DbParameter);
        }

        [WebMethod(Description = "执行一个存储过程，返回执行成功条数.返回值-1：执行错误")]
        public Int32 Execute_Procedure2(string ProcedureName, DataSet DsDbParameter, string dataAccessName)
        {
            DataAccessCollections dac = new DataAccessCollections();
            MyNet.Common.Data.DataAccess dAccess = dac.GetDataAccess(dataAccessName);
            DataTable dt = DsDbParameter.Tables[0];
            DbParameter[] DbParameter = new DbParameter[dt.Rows.Count];

            for (int i = 0; i < DsDbParameter.Tables[0].Rows.Count; i++)
            {
                DbParameter[i] = new MySql.Data.MySqlClient.MySqlParameter(dt.Rows[i]["name"].ToString(), dt.Rows[i]["value"]);

                try
                {
                    if (dt.Rows[i]["type"] != null)
                    {
                        DbParameter[i].DbType = (DbType)int.Parse(dt.Rows[i]["type"].ToString());
                    }
                }
                catch
                {
                }
            }
            return dAccess.Execute_Procedure(ProcedureName, DbParameter);
        }

        [WebMethod(Description = "执行一个sql语句，并且有参数数组，返回是否执行成功")]
        public Int32 Execute_SqlParameter(int commandType, string Sqlstr, DataSet DsDbParameter)
        {
            DataTable dt = DsDbParameter.Tables[0];
            DbParameter[] DbParameter = new DbParameter[dt.Rows.Count];

            for (int i = 0; i < DsDbParameter.Tables[0].Rows.Count; i++)
            {
                DbParameter[i] = new MySql.Data.MySqlClient.MySqlParameter(dt.Rows[i]["name"].ToString(), dt.Rows[i]["value"]);
                try
                {
                    if (dt.Rows[i]["type"] != null)
                    {
                        DbParameter[i].DbType = (DbType)int.Parse(dt.Rows[i]["type"].ToString());
                    }
                }
                catch
                {
                }
            }
            System.Data.CommandType ComType = (System.Data.CommandType)commandType;
            return dataAccess.Execute_Procedure(ComType, Sqlstr, DbParameter);
        }

        [WebMethod(Description = "执行一个sql语句，并且有参数数组，返回是否执行成功")]
        public Int32 Execute_SqlParameter2(int commandType, string Sqlstr, DataSet DsDbParameter, string dataAccessName)
        {
            DataAccessCollections dac = new DataAccessCollections();
            MyNet.Common.Data.DataAccess dAccess = dac.GetDataAccess(dataAccessName);
            DataTable dt = DsDbParameter.Tables[0];
            DbParameter[] DbParameter = new DbParameter[dt.Rows.Count];

            for (int i = 0; i < DsDbParameter.Tables[0].Rows.Count; i++)
            {
                DbParameter[i] = new MySql.Data.MySqlClient.MySqlParameter(dt.Rows[i]["name"].ToString(), dt.Rows[i]["value"]);
                try
                {
                    if (dt.Rows[i]["type"] != null)
                    {
                        DbParameter[i].DbType = (DbType)int.Parse(dt.Rows[i]["type"].ToString());
                    }
                }
                catch
                {
                }
            }
            System.Data.CommandType ComType = (System.Data.CommandType)commandType;
            return dAccess.Execute_Procedure(ComType, Sqlstr, DbParameter);
        }
    }
}