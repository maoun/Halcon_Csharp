using MyNet.Atmcs.Uscmcp.IData;
using MyNet.Atmcs.Uscmcp.Model;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class NoticeManager
    {
        private static readonly INoticeManager dal = DALFactory.CreateNoticeManager();

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetNoticePicInfo(string where)
        {
            return dal.GetNoticePic(where);
        }

        /// <summary>
        /// 删除方法
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int DeleteNoticePic(string id)
        {
            return dal.DeleteNoticePic(id);
        }

        public int AddNoticePic(NoticePicInfo info)
        {
            return dal.AddNoticePic(info);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int EditNoticePic(NoticePicInfo info)
        {
            return dal.EditNoticePic(info);
        }
    }
}