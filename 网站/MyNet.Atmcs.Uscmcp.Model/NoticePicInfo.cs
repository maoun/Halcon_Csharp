using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Model
{

    /// <summary>
    /// 公告图片实体
    /// </summary>
    public class NoticePicInfo
    {
        private string id;
        /// <summary>
        /// ID
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string picName;
        /// <summary>
        /// 公告名称
        /// </summary>
        public string PicName
        {
            get { return picName; }
            set { picName = value; }
        }
        private string name;
        /// <summary>
        /// 公告简称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string dicDisc;
        /// <summary>
        /// 公告详细描述
        /// </summary>
        public string DicDisc
        {
            get { return dicDisc; }
            set { dicDisc = value; }
        }
        private string picUrl;
        /// <summary>
        /// 公告图片Url
        /// </summary>
        public string PicUrl
        {
            get { return picUrl; }
            set { picUrl = value; }
        }
        private string lrr;
        /// <summary>
        /// 录入人编号
        /// </summary>
        public string Lrr
        {
            get { return lrr; }
            set { lrr = value; }
        }
        private string lrsj;
        /// <summary>
        /// 录入时间
        /// </summary>
        public string Lrsj
        {
            get { return lrsj; }
            set { lrsj = value; }
        }
        private string wfxw;
        /// <summary>
        /// 违法行为编号
        /// </summary>
        public string Wfxw
        {
            get { return wfxw; }
            set { wfxw = value; }
        }
    }
}
