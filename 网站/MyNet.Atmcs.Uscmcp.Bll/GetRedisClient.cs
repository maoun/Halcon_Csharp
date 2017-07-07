using System;
using Cache.Helper.Factory;
using Cache.Helper.ImplementClass;
using MyNet.Common.Log;
using ServiceStack.Redis;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class GetRedisClient
    {
        private static RedisClient redisClient = null;

        public GetRedisClient()
        {
            if (redisClient == null)
            {
                GetClient();
            }
        }

        /// <summary>
        /// 得到一个RedisClient
        /// </summary>
        /// <returns></returns>
        private void GetClient()
        {
            try
            {
                ICacheFactory cacheFactory = FactoryProducer.GetFactory("Redis");
                RedisClient redisClients = ((RedisManage)cacheFactory.GetRedisInstance()).GetRedisClient();
                redisClients.ChangeDb(Convert.ToInt64(4));
                redisClient = redisClients;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetRedisValue(string key)
        {
            try
            {
                string strClass = redisClient.GetValue(key);
                return strClass;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "";
            }
        }
    }
}