using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyNet.Atmcs.Uscmcp.IData;
using System.Data;
using MyNet.Common.Log;
using MyNet.Atmcs.Uscmcp.Model;

namespace MyNet.Atmcs.Uscmcp.Data
{
  public  class NoticeManager:INoticeManager
    {

          private MyNet.Common.Data.DataAccessCollections dac = new MyNet.Common.Data.DataAccessCollections();

        private MyNet.Common.Data.DataAccess dataAccess;

        public NoticeManager()
        {
            //dataAccess = dac.GetDataAccess(WebConfig.DataAccessName);
            dataAccess = GetDataAccess.Init();
        }

        public NoticeManager(string dataAccessName)
        {
            dataAccess = dac.GetDataAccess(dataAccessName);
        }

      /// <summary>
      /// 查询方法
      /// </summary>
      /// <param name="where"></param>
      /// <returns></returns>
      public DataTable GetNoticePic(string where)
      {
          string mySql = string.Empty;
          try
          {

              mySql = "SELECT id,  picName,  NAME,  picDisc,  wfxw,  lrr,  lrsj,  priUrl   FROM   t_cfg_noticepic  WHERE  " + where;
              return dataAccess.Get_DataTable(mySql);
          }
          catch (Exception ex)
          {
              ILog.WriteErrorLog(mySql + ex.Message);
              return null;
          }
      }

      /// <summary>
      /// 添加
      /// </summary>
      /// <param name="info"></param>
      /// <returns></returns>
      public int AddNoticePic(NoticePicInfo info)
      {
          string mySql = string.Empty;
          try
          {

              mySql = "INSERT INTO  t_cfg_noticepic(id,picname,picdisc,priurl,lrr,lrsj,NAME,wfxw) VALUES('"+info.Id+"','"+info.PicName+"','"+info.DicDisc+"','"+info.PicUrl+"','"+info.Lrr+"','"+info.Lrsj+"','"+info.Name+"','"+info.Wfxw+"');";
              return dataAccess.Execute_NonQuery(mySql);
          }
          catch (Exception ex)
          {
              ILog.WriteErrorLog(mySql + ex.Message);
              return 0;
          }
      }
      /// <summary>
      /// 更新
      /// </summary>
      /// <param name="info"></param>
      /// <returns></returns>
      public int EditNoticePic(NoticePicInfo info)
      {
          string mySql = string.Empty;
          try
          {

              mySql =@"UPDATE t_cfg_noticepic SET   picname='"+info.PicName+"' ,NAME='"+info.Name+"',picDisc='"+info.DicDisc+"',  wfxw='"+info.Wfxw+"',priUrl='"+info.PicUrl+"' WHERE id='"+info.Id+"'";
      
              return dataAccess.Execute_NonQuery(mySql);
          }
          catch (Exception ex)
          {
              ILog.WriteErrorLog(mySql + ex.Message);
              return 0;
          }
      }
      /// <summary>
      /// 删除
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
      public int DeleteNoticePic(string id)
      {
          string mySql = string.Empty;
          try
          {
              mySql = " DELETE FROM t_cfg_noticepic  WHERE "+id;
              return dataAccess.Execute_NonQuery(mySql);
          }
          catch (Exception ex)
          {
              ILog.WriteErrorLog(mySql + ex.Message);
              return 0;
          }
      }
    }
}
