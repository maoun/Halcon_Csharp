using System;

namespace MyNet.Atmcs.Uscmcp.Model
{
    [Serializable]
    /// <summary>
    /// 过车实体
    /// </summary>
    public class PassInfo
    {
        private string hphm1;

        /// <summary>
        /// 号牌号码1
        /// </summary>
        public string Hphm1
        {
            get { return hphm1; }
            set { hphm1 = value; }
        }

        private string hphm2;

        /// <summary>
        /// 号牌号码2
        /// </summary>
        public string Hphm2
        {
            get { return hphm2; }
            set { hphm2 = value; }
        }

        private string gcsj1;

        /// <summary>
        /// 过车时间1
        /// </summary>
        public string Gcsj1
        {
            get { return gcsj1; }
            set { gcsj1 = value; }
        }

        private string gcsj2;

        /// <summary>
        /// 过车时间2
        /// </summary>
        public string Gcsj2
        {
            get { return gcsj2; }
            set { gcsj2 = value; }
        }

        private string lkmc1;

        /// <summary>
        /// 路口名称1
        /// </summary>
        public string Lkmc1
        {
            get { return lkmc1; }
            set { lkmc1 = value; }
        }

        private string lkmc2;

        /// <summary>
        /// 路口名称2
        /// </summary>
        public string Lkmc2
        {
            get { return lkmc2; }
            set { lkmc2 = value; }
        }

        private string xpos1;

        /// <summary>
        /// x坐标1
        /// </summary>
        public string Xpos1
        {
            get { return xpos1; }
            set { xpos1 = value; }
        }

        private string xpos2;

        /// <summary>
        /// x坐标2
        /// </summary>
        public string Xpos2
        {
            get { return xpos2; }
            set { xpos2 = value; }
        }

        private string ypos1;

        /// <summary>
        /// y坐标1
        /// </summary>
        public string Ypos1
        {
            get { return ypos1; }
            set { ypos1 = value; }
        }

        private string ypos2;

        /// <summary>
        /// y坐标2
        /// </summary>
        public string Ypos2
        {
            get { return ypos2; }
            set { ypos2 = value; }
        }

        private string zjwj1;

        /// <summary>
        /// 证据文件1
        /// </summary>
        public string Zjwj1
        {
            get { return zjwj1; }
            set { zjwj1 = value; }
        }

        private string zjwj2;

        /// <summary>
        /// 证据文件2
        /// </summary>
        public string Zjwj2
        {
            get { return zjwj2; }
            set { zjwj2 = value; }
        }

        private string len;

        /// <summary>
        /// 两点距离
        /// </summary>
        public string Len
        {
            get { return len; }
            set { len = value; }
        }
    }
}