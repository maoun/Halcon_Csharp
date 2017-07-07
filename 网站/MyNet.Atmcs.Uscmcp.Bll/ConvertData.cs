using Ext.Net;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Xsl;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class ConvertData
    {
        #region dataTable转换成Json格式

        ///<summary>
        /// dataTable转换成Json格式
        ///</summary>
        ///<param name="dt"></param>
        ///<returns></returns>
        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append(dt.TableName);
            jsonBuilder.Append("\"[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        ///<summary>
        /// dataTable转换成Json格式
        ///</summary>
        ///<param name="dt"></param>
        ///<returns></returns>
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            //jsonBuilder.Append("{\"");
            //jsonBuilder.Append(dt.TableName);
            //jsonBuilder.Append("\"[");
            jsonBuilder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    //jsonBuilder.Append("\"");
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            //jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        #endregion dataTable转换成Json格式

        #region DataSet转换成Json格式

        ///<summary>
        /// DataSet转换成Json格式
        ///</summary>
        ///<param name="ds">DataSet</param>
        ///<returns></returns>
        public static string Dataset2Json(DataSet ds)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {
                json.Append("{\"");
                json.Append(dt.TableName);
                json.Append("\":");
                json.Append(DataTable2Json(dt));
                json.Append("}");
            } return json.ToString();
        }

        #endregion DataSet转换成Json格式

        ///<summary>
        /// Msdn
        ///</summary>
        ///<param name="jsonName"></param>
        ///<param name="dt"></param>
        ///<returns></returns>
        public static string DataTableToJson(string jsonName, DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + dt.Rows[i][j].ToString() + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }

        /// <summary>
        /// 根据Json返回DateTable,JSON数据格式如:
        /// {table:[{column1:1,column2:2,column3:3},{column1:1,column2:2,column3:3}]}
        /// </summary>
        /// <param name="strJson">Json字符串</param>
        /// <returns></returns>
        public static DataTable JsonToDataTable2(string strJson)
        {
            //取出表名
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.LastIndexOf("]"));

            //获取数据
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                strRow = strRow.Replace("\",", "\"し");
                strRow = strRow.Replace("\":", "\"つ");
                string[] strRows = strRow.Split(new char[] { 'し' });

                //创建表
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        string[] strCell = str.Split('つ');
                        dc.ColumnName = strCell[0].Replace("\"", "");
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }

                //增加内容
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split('つ')[1].Trim().Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }

        /// <summary>
        /// DataTable 对象 转换为Json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(DataTable dt)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }

            return javaScriptSerializer.Serialize(arrayList);  //返回一个json字符串
        }

        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(string json)
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

        #region 导出

        /// <summary>
        /// 导出xml
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="page"></param>
        public static void ExportXml(DataTable dt, System.Web.UI.Page page)
        {
            if (dt != null)
            {
                string json = ConvertData.DataTable2Json(dt);
                StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
                XmlNode xml = eSubmit.Xml;

                string strXml = xml.OuterXml;

                page.Response.Clear();

                page.Response.AddHeader("Content-Disposition", "attachment; filename=" + DateTime.Now.ToString() + ".xml");
                page.Response.AddHeader("Content-Length", strXml.Length.ToString());
                page.Response.ContentType = "application/xml";
                page.Response.Write(strXml);
                page.Response.End();
            }
        }

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="page"></param>
        public static void ExportExcel(DataTable dt, System.Web.UI.Page page)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                string json = DataTable2Json(dt);
                StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
                XmlNode xml = eSubmit.Xml;

                page.Response.Clear();
                page.Response.ContentType = "application/vnd.ms-excel";
                page.Response.AddHeader("Content-Disposition", "attachment; filename=" + DateTime.Now.ToString() + ".xls");
                XslCompiledTransform xtExcel = new XslCompiledTransform();
                xtExcel.Load(page.Server.MapPath("../Export/Excel.xsl"));
                xtExcel.Transform(xml, null, page.Response.OutputStream);
                page.Response.End();
            }
        }

        /// <summary>
        /// 导出CSV
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="page"></param>
        public static void ExportCsv(DataTable dt, System.Web.UI.Page page)
        {
            if (dt != null)
            {
                string json = DataTable2Json(dt);
                StoreSubmitDataEventArgs eSubmit = new StoreSubmitDataEventArgs(json, null);
                XmlNode xml = eSubmit.Xml;

                page.Response.Clear();
                page.Response.ContentType = "application/octet-stream";
                page.Response.AddHeader("Content-Disposition", "attachment; filename=" + DateTime.Now.ToString() + ".csv");
                XslCompiledTransform xtCsv = new XslCompiledTransform();
                xtCsv.Load(page.Server.MapPath("../Export/Csv.xsl"));
                xtCsv.Transform(xml, null, page.Response.OutputStream);
                page.Response.End();
            }
        }

        /// <summary>
        /// 读取excel中数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static DataSet ExcelToDataSet(string filePath, string sheetName)
        {
            OleDbConnection connection = null;
            OleDbCommand command = null;
            OleDbDataAdapter dataAdapter = null;
            try
            {
                DataSet dataSet = new DataSet();

                string connectionString = string.Format("Provider = Microsoft.Jet.OLEDB.4.0; Data Source ={0};Extended Properties=Excel 8.0", filePath);//通过ODBC连接数据的字符串，并将一个具体的Excel文件路径传入

                string sqlString = string.Format("Select * from [{0}$]", sheetName);

                //读取Excel文件中的某一页,页名称根据sheetName变量传入

                connection = new OleDbConnection();

                connection.ConnectionString = connectionString;

                command = new OleDbCommand();

                command.Connection = connection;

                command.CommandType = CommandType.Text;

                command.CommandText = sqlString;

                dataAdapter = new OleDbDataAdapter();

                dataAdapter.SelectCommand = command;

                dataAdapter.Fill(dataSet);

                return dataSet;
            }
            catch
            {
                return null;
            }
            finally
            {
                dataAdapter.Dispose();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }

        #endregion 导出
    }
}