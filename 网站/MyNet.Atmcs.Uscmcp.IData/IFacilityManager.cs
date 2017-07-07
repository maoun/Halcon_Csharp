using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.IData
{
    public interface IFacilityManager
    {
        #region 查询相关方法


        DataTable Facility(string where);
        int insertFacility_SignageMark(System.Collections.Hashtable hs);
        DataTable getFacility(string where);
        DataTable getFacilityid(string id);
        int updateFacility_SignageMark(System.Collections.Hashtable hs);
        int DeleteFacility_SignageMark(string id);
        int insertFacility_Isolation(System.Collections.Hashtable hs);
        int updateFacility_Isolation(System.Collections.Hashtable hs);
        int insertFacility_Traffic(System.Collections.Hashtable hs);
        int updateFacility_Traffic(System.Collections.Hashtable hs);
        DataTable selectid(string id);
        DataTable selectlukou(string where);
        # endregion

    }
}
