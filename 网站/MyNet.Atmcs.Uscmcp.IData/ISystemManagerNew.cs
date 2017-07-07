using System.Collections;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.IData
{
    public interface ISystemManagerNew
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        DataTable GetSystemInfo();

        /// <summary>
        ///
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        DataTable GetSystemInfo(string systemId);

        /// <summary>
        /// 读取字典项
        /// </summary>
        /// <param name="codeType"></param>
        /// <returns></returns>
        DataTable GetCode(string codeType);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int AddSysCode(Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateSysCode(Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DelSysCode(Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        DataTable GetCodeType();

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int AddSysCodeType(Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateSysCodeType(Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DelSysCodeType(Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        DataTable GetSysFunction();

        /// <summary>
        ///
        /// </summary>
        /// <param name="funcId"></param>
        /// <returns></returns>
        DataTable GetSysFunction(string funcId);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int AddSysFunction(Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int UpdateSysFunction(Hashtable hs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        int DelSysFunction(Hashtable hs);

        /// <summary>
        /// 生成记录ID
        /// </summary>
        /// <param name="head"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        string GetRecordID(string head, int totalLength);
    }
}