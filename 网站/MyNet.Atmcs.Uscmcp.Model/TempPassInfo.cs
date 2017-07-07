using System;

namespace MyNet.Atmcs.Uscmcp.Model
{
    /// <summary>
    /// 临时过车展示
    /// </summary>
    [Serializable]
    public class TempPassInfo
    {
        private string hphm;
        /// <summary>
        /// 号牌号码
        /// </summary>
        public string Hphm
        {
            get { return hphm; }
            set { hphm = value; }
        }

        private string kkid;
        /// <summary>
        /// 卡口编号
        /// </summary>
        public string Kkid
        {
            get { return kkid; }
            set { kkid = value; }
        }

        private string gwsj;
        /// <summary>
        /// 过往时间
        /// </summary>
        public string Gwsj
        {
            get { return gwsj; }
            set { gwsj = value; }
        }

        private string zjwj;
        /// <summary>
        /// 图片1
        /// </summary>
        public string Zjwj
        {
            get { return zjwj; }
            set { zjwj = value; }
        }

        private string clpp;
        /// <summary>
        /// 车辆品牌
        /// </summary>
        public string Clpp
        {
            get { return clpp; }
            set { clpp = value; }
        }

        private string csys;
        /// <summary>
        /// 车身颜色
        /// </summary>
        public string Csys
        {
            get { return csys; }
            set { csys = value; }
        }
    }
}