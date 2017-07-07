using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Data
{
    public class DataCommon : IDataCommon
    {
        /// <summary>
        /// 数据访问接口
        /// </summary>
        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        private MyNet.Common.Data.DataAccess dataAccess;
        private Common common = new Common();

        public DataCommon()
        {
            //dataAccess = dac.GetDataAccess(WebConfig.DataAccessName);
            dataAccess = GetDataAccess.Init();
        }
        /// <summary>
        /// 执行查询datatable操作
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string mySql)
        {
            try
            {
                return dataAccess.Get_DataTable(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 执行获得stringSQL操作
        /// </summary>
        /// <param name="mySql"></param>
        /// <returns></returns>
        public string GetString(string mySql)
        {
            try
            {
                return dataAccess.Get_DataString(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 执行Insert操作
        /// </summary>
        /// <param name="mySql"></param>
        /// <returns></returns>
        public int Insert(string mySql)
        {
            try
            {
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return 0;
            }
        }
        /// <summary>
        ///  执行UpdateSQL语句
        /// </summary>
        /// <param name="mySql"></param>
        /// <returns></returns>
        public int Update(string mySql)
        {
            try
            {
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return 0;
            }
        }
        /// <summary>
        ///执行Delete操作
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Delete(string mySql)
        {
            try
            {
                return dataAccess.Execute_NonQuery(mySql);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex.Message);
                return 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="col0"></param>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        /// <returns></returns>
        public DataTable ChangeDataTablePoliceIp(DataTable dt, string col0, string col1, string col2)
        {
            DataTable mydt = GetDataTable("select * from t_cfg_sysconfig where systemid='00' and configid='17'");
            if (mydt != null && mydt.Rows.Count > 0 && mydt.Rows[0]["configvalue"].ToString() == "1")
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(col0))
                        {
                            dt.Rows[i][col0] = ChangePoliceIp(dt.Rows[i][col0].ToString());
                        }
                        if (!string.IsNullOrEmpty(col1))
                        {
                            dt.Rows[i][col1] = ChangePoliceIp(dt.Rows[i][col1].ToString());
                        }
                        if (!string.IsNullOrEmpty(col2))
                        {
                            dt.Rows[i][col2] = ChangePoliceIp(dt.Rows[i][col2].ToString());
                        }
                    }
                }
            }
            return dt;
        }
        /// <summary>
        /// IP转换
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ChangePoliceIp(string url)
        {
            string hostAddress = common.GetUserHostAddress();
            string ipaddress = GetImageIp(url);

            if (!string.IsNullOrEmpty(ipaddress))
            {
                string policeaddress = "";// GetString("select f_get_policeip('" + ipaddress + "','" + hostAddress + "') from dual");
                if (!string.IsNullOrEmpty(policeaddress) && !string.IsNullOrEmpty(policeaddress))
                {
                    return url.Replace(ipaddress, policeaddress);
                }
                else
                {
                    return url;
                }
            }
            else
            {
                return url;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        public string ChangeIp(string ipaddress)
        {
            string hostAddress = common.GetUserHostAddress();

            if (!string.IsNullOrEmpty(ipaddress))
            {
                return "";//GetString("select f_get_policeip('" + ipaddress + "','" + hostAddress + "') from dual");
            }
            else
            {
                return ipaddress;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string SplitIpAddess(string url)
        {
            string ipaddess = string.Empty;

            if (IsHttpUrl(url))
            {
                string pattern = @"^(2[0-5]{2}|2[0-4][0-9]|1?[0-9]{1,2}).(2[0-5]{2}|2[0-4][0-9]|1?[0-9]{1,2}).(2[0-5]{2}|2[0-4][0-9]|1?[0-9]{1,2}).(2[0-5]{2}|2[0-4][0-9]|1?[0-9]{1,2})$";
                string s = url.ToUpper().Replace("HTTP://", "");
                string[] ss = s.Split('/');
                if (ss.Length > 0)
                {
                    Regex re = new Regex(pattern, RegexOptions.IgnoreCase);
                    for (int i = 0; i < ss.Length; i++)
                    {
                        Match m = re.Match(ss[i]);
                        if (m.Success)
                        {
                            ipaddess = ss[i];
                            break;
                        }
                    }
                }
            }

            return ipaddess;
        }
        /// <summary>
        /// 判断是否http地址
        /// </summary>
        /// <param name="strurl"></param>
        /// <returns></returns>
        public bool IsHttpUrl(string strurl)
        {
            string pattern = @"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$";
            return System.Text.RegularExpressions.Regex.Match(strurl, pattern).Success;
        }
        /// <summary>
        /// 获得url中IP
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetImageIp(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    if (path.IndexOf("ftp") >= 0)
                    {
                        string[] pi = getIpaddess(path);
                        if (pi.Length > 2)
                        {
                            return pi[1].Split('/')[1];
                        }
                    }
                    else if (path.IndexOf("http") >= 0)
                    {
                        string[] pi = getIpaddess(path);
                        if (pi.Length > 2)
                        {
                            return pi[0];
                        }
                    }
                }
                return "127.0.0.1";
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string[] getIpaddess(string path)
        {
            string[] pathInfo = new string[3];
            string extendname = "";

            if (path != "")
            {
                string httphead = "";
                if (path.IndexOf("ftp") >= 0)
                {
                    httphead = "ftp://";
                }
                else if (path.IndexOf("http") >= 0)
                {
                    httphead = "http://";
                }
                int tmp1 = path.ToLower().IndexOf(httphead);
                if (tmp1 == 0)
                {
                    path = path.Substring(httphead.Length).Replace("//", "/");
                }
                int tmp2 = path.ToLower().IndexOf("/");
                pathInfo[0] = path.Substring(tmp1, tmp2 - tmp1);
                pathInfo[1] = path.Substring(tmp2);
                extendname = Path.GetExtension(path);
                switch (extendname.ToLower())
                {
                    case ".mp4":
                        pathInfo[2] = "2";
                        break;

                    case ".avi":
                        pathInfo[2] = "3";
                        break;

                    default:
                        pathInfo[2] = "1";
                        break;
                }
            }
            return pathInfo;
        }
    }
}