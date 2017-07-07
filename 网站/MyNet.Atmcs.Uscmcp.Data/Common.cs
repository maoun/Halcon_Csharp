using System;
using MyNet.Atmcs.Uscmcp.Setting;

namespace MyNet.Atmcs.Uscmcp.Data
{
    /// <summary>
    /// 通用组件
    /// </summary>
    public class Common
    {
        private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();
        private MyNet.Common.Data.DataAccess dataAccess;

        public Common()
        {
            dataAccess = dac.GetDataAccess(WebConfig.DataAccessName);
        }

        public Common(string dataAccessName)
        {
            dataAccess = dac.GetDataAccess(dataAccessName);
        }

        public static string GetHashtableStr(System.Collections.Hashtable hs, string key, string field)
        {
            string str = string.Empty;

            if (hs.ContainsKey(key))
            {
                str = field + "='" + hs[key].ToString() + "',";
            }
            else
            {
                str = String.Empty;
            }

            return str;
        }

        public static string GetHashtableStr2(System.Collections.Hashtable hs, string key, string field)
        {
            string str = string.Empty;

            if (hs.ContainsKey(key))
            {
                str = field + "='" + hs[key].ToString() + "'";
            }
            else
            {
                str = String.Empty;
            }

            return str;
        }

        public static string GetHashtableDateOracle(System.Collections.Hashtable hs, string key, string field)
        {
            string str = string.Empty;

            if (hs.ContainsKey(key))
            {
                str = field + "=STR_TO_DATE('" + DateTime.Parse(hs[key].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "','%Y-%m-%d %H:%i:%s'),";
            }
            else
            {
                str = String.Empty;
            }

            return str;
        }

        public static string GetHashtableValue(System.Collections.Hashtable hs, string key, string defalutValue)
        {
            string str = string.Empty;

            if (hs.ContainsKey(key))
            {
                str = hs[key].ToString();
            }
            else
            {
                str = defalutValue;
            }

            return str;
        }

        public static string GetHashtableValue(System.Collections.Hashtable hs, string key)
        {
            string str = string.Empty;

            if (hs.ContainsKey(key))
            {
                str = hs[key].ToString();
            }
            else
            {
                str = "";
            }

            return str;
        }

        public string GetUserHostAddress()
        {
            return System.Web.HttpContext.Current.Request.UserHostAddress;
        }

        /// <summary>
        /// 获得记录ID
        /// </summary>
        /// <returns></returns>
        public static string GetRecordId()
        {
            return DateTime.Now.ToString("yyMMddHHmmssfff");
        }
    }
}