namespace MyNet.Atmcs.Uscmcp.Setting
{
    public class WebConfig
    {
        private static string dataAccessName;

        /// <summary>
        ///
        /// </summary>
        public static string DataAccessName
        {
            get { return "uscmcp"; }
            set { dataAccessName = value; }
        }
    }
}