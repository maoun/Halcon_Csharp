using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.IData
{
  public  interface INoticeManager
    {
        int AddNoticePic(MyNet.Atmcs.Uscmcp.Model.NoticePicInfo info);
        int DeleteNoticePic(string id);
        int EditNoticePic(MyNet.Atmcs.Uscmcp.Model.NoticePicInfo info);
        System.Data.DataTable GetNoticePic(string where);
  
    }
}
