using System;
using MyNet.Common.Log;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class QueryServiceManager
    {
        private QueryService.querypasscar service = new QueryService.querypasscar();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="url"></param>
        public QueryServiceManager(string url)
        {
            service.Url = url;
        }

        /// <summary>
        /// 大数据布控接口
        /// </summary>
        /// <param name="layoutid"></param>
        /// <param name="hphm"></param>
        /// <param name="hpzl"></param>
        /// <param name="kkidlist"></param>
        /// <returns></returns>
        public bool Layout(string bkid, string hphm, string hpzl, string kkidlist)
        {
            try
            {
                return service.layout(bkid, hphm, hpzl, kkidlist);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }
        /// <summary>
        /// 大数据撤控接口
        /// </summary>
        /// <param name="bkid"></param>
        /// <returns></returns>
        public bool CancleLayout(string bkid)
        {
            try
            {
                return service.cancleLayout(bkid);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return false;
            }
        }
    }
}