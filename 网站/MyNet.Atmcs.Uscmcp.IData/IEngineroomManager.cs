using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.IData
{
    public interface IEngineroomManager
    {
        DataTable GetEngineroom(string where);
        int insertEngineroom(System.Collections.Hashtable hs);
        int uptateEngineroom(System.Collections.Hashtable hs);
        int DeleteEngineroom(string id);
        DataTable GetEngineroomID(string where);

    }
}
