using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class GetRedisData
    {
        private static GetRedisClient redisClient = new GetRedisClient();
        private static DataTable dtTemp = null;

        /// <summary>
        /// 获得redis中值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="isChangeName"></param>
        /// <returns></returns>
        public static DataTable GetData(string redisKey)
        {
            try
            {
                string redisStr = redisClient.GetRedisValue(redisKey);
                if (string.IsNullOrEmpty(redisStr))
                {
                    return null;
                }
                dtTemp = ToDataTable(redisStr);
                return ChangColName(dtTemp, false);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 获得redis中值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="isChangeName"></param>
        /// <returns></returns>
        public static string GetCount(string redisKey)
        {
            try
            {
                string redisStr = redisClient.GetRedisValue(redisKey);
                if (string.IsNullOrEmpty(redisStr))
                {
                    return "0";
                }

                dtTemp = ToDataTable(redisStr);
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    return dtTemp.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
            return "0";
        }

        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static DataTable ToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;

            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值

                if (json.Substring(0, 1) != "[")
                    json = json.Substring(1, json.Length - 2);
                json = json.Replace("\\", "");
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
            result = dataTable;
            return result;
        }

        /// <summary>
        /// 转换列名
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="isChange"></param>
        /// <returns></returns>
        public static DataTable ChangColName(DataTable dt, bool isChange)
        {
            try
            {
                if (dt != null)
                {
                    if (isChange)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            dt.Columns[i].ColumnName = "col" + i;
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
    }

    public class CountInfo
    {
        private object ll;

        public object Ll
        {
            get { return ll; }
            set { ll = value; }
        }
    }
}