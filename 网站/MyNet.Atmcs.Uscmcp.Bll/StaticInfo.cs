using System;
using System.Configuration;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    [Serializable]
    public static class StaticInfo
    {
        public static string WebSite = ConfigurationManager.AppSettings["WebSite"];      //供下载文件的相对路径
        public static string FileName = ConfigurationManager.AppSettings["fileName"];    // 供下载的文件
        public static string LoginPage = ConfigurationManager.AppSettings["LoginPage"];    // 超时后转跳页面
    }
}