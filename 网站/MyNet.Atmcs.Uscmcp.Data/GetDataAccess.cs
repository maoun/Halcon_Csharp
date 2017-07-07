namespace MyNet.Atmcs.Uscmcp.Data
{
    public class GetDataAccess
    {
        /// <summary>
        /// 读取老库Uscmcp
        /// </summary>
        /// <returns></returns>
        public static MyNet.Common.Data.DataAccess Init()
        {
            string data = System.Configuration.ConfigurationManager.AppSettings["data"].ToString() + "Database=uscmcp;";
            return new MyNet.Common.Data.DataAccess(MyNet.Common.Data.DataAccess.DatabaseType.MySql, data);
        }

        /// <summary>
        /// 读取新库Frame
        /// </summary>
        /// <returns></returns>
        public static MyNet.Common.Data.DataAccess InitNew()
        {
            string data = System.Configuration.ConfigurationManager.AppSettings["data"].ToString() + "Database=frame;";
            return new MyNet.Common.Data.DataAccess(MyNet.Common.Data.DataAccess.DatabaseType.MySql, data);
        }
    }
}