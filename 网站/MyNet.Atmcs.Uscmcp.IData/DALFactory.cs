using System;
using System.Reflection;

namespace MyNet.Atmcs.Uscmcp.IData
{
    /// <summary>
    /// 数据工厂
    /// </summary>
    public class DALFactory
    {
        /// <summary>
        /// 获得系统管理类
        /// </summary>
        /// <returns></returns>
        public static ISystemManager CreateSystemManager()
        {
            string dalName = GetDALName();
            string className = dalName + ".SystemManager";
            try
            {
                return (ISystemManager)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得系统管理类
        /// </summary>
        /// <returns></returns>
        public static ISystemManagerNew CreateSystemManagerNew()
        {
            string dalName = GetDALName();
            string className = dalName + ".SystemManagerNew";
            try
            {
                return (ISystemManagerNew)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得用户管理类
        /// </summary>
        /// <returns></returns>
        public static IUserManager CreateUserManager()
        {
            string dalName = GetDALName();
            string className = dalName + ".UserManager";
            try
            {
                return (IUserManager)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得用户管理类
        /// </summary>
        /// <returns></returns>
        public static IUserManagerNew CreateUserManagerNew()
        {
            string dalName = GetDALName();
            string className = dalName + ".UserManagerNew";
            try
            {
                return (IUserManagerNew)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得日志管理类
        /// </summary>
        /// <returns></returns>
        public static ILogManager CreateLogManager()
        {
            string dalName = GetDALName();
            string className = dalName + ".LogManager";
            try
            {
                return (ILogManager)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得配置管理类
        /// </summary>
        /// <returns></returns>
        public static ISettingManager CreateSettingManager()
        {
            string dalName = GetDALName();
            string className = dalName + ".SettingManager";
            try
            {
                return (ISettingManager)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得配置管理类
        /// </summary>
        /// <returns></returns>
        public static ISettingManagerNew CreateSettingManagerNew()
        {
            string dalName = GetDALName();
            string className = dalName + ".SettingManagerNew";
            try
            {
                return (ISettingManagerNew)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得设备管理类
        /// </summary>
        /// <returns></returns>
        public static IDeviceManager CreateDeviceManager()
        {
            string dalName = GetDALName();
            string className = dalName + ".DeviceManager";
            try
            {
                return (IDeviceManager)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static IServiceManager CreateServiceManager()
        {
            string dalName = GetDALName();
            string className = dalName + ".ServiceManager";
            try
            {
                return (IServiceManager)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static ITgsPproperty CreateTgsPproperty()
        {
            string dalName = GetDALName();
            string className = dalName + ".TgsPproperty";
            try
            {
                return (ITgsPproperty)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static ITgsPpropertyNew CreateTgsPpropertyNew()
        {
            string dalName = GetDALName();
            string className = dalName + ".TgsPpropertyNew";
            try
            {
                return (ITgsPpropertyNew)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static IDataCommon CreateDataCommon()
        {
            string dalName = GetDALName();
            string className = dalName + ".DataCommon";
            try
            {
                return (IDataCommon)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static ITgsDataInfo CreateTgsDataInfo()
        {
            string dalName = GetDALName();
            string className = dalName + ".TgsDataInfo";
            try
            {
                return (ITgsDataInfo)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static IRoadManager CreateRoadManager()
        {
            string dalName = GetDALName();
            string className = dalName + ".RoadManager";
            try
            {
                return (IRoadManager)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得研判地图管理类
        /// </summary>
        /// <returns></returns>
        public static IMapManager CreateMapManager()
        {
            string dalName = GetDALName();
            string className = dalName + ".MapManager";
            try
            {
                return (IMapManager)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 活动passcar查询管理
        /// </summary>
        /// <returns></returns>
        public static IPasscarManager CreatePasscarManager()
        {
            string dalName = GetDALName();
            string className = dalName + ".PasscarManager";
            try
            {
                return (IPasscarManager)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static IGisShow CreateGisShow()
        {
            string dalName = GetDALName();
            string className = dalName + ".GisShow";
            try
            {
                return (IGisShow)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static IDataCountInfo CreateDataCountInfo()
        {
            string dalName = GetDALName();
            string className = dalName + ".DataCountInfo";
            try
            {
                return (IDataCountInfo)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private static string GetDALName()
        {
            return "MyNet.Atmcs.Uscmcp.Data";
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static IFacilityManager CreateFacilityManager()
        {
            string dalName = GetDALName();
            string className = dalName + ".FacilityManager";
            try
            {
                return (IFacilityManager)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static IEngineroomManager CreateEngineroomManager()
        {
            string dalName = GetDALName();
            string className = dalName + ".EngineroomManager";
            try
            {
                return (IEngineroomManager)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static INoticeManager CreateNoticeManager()
        {
            string dalName = GetDALName();
            string className = dalName + ".NoticeManager";
            try
            {
                return (INoticeManager)Assembly.Load(dalName).CreateInstance(className);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}