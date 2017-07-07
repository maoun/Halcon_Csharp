using System;

namespace MyNet.Atmcs.Uscmcp.Model
{
    [Serializable]
    /// <summary>
    /// 查询实体
    /// </summary>
    public class Condition
    {
        private string startTime = "";

        /// <summary>
        /// 低速度
        /// </summary>
        public string Dsd
        {
            get;
            set;
        }

        private string zdmb = "0";

        public string Zdmb
        {
            get { return zdmb; }
            set { zdmb = value; }
        }

        /// <summary>
        /// 高速度
        /// </summary>
        public string Gsd
        {
            get;
            set;
        }

        /// <summary>
        /// 短车长
        /// </summary>
        public string Dcc
        {
            get;
            set;
        }

        /// <summary>
        /// 长车长
        /// </summary>
        public string Ccc
        {
            get;
            set;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        private string endTime = "";

        /// <summary>
        ///结束时间
        /// </summary>
        public string EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private string hphm = "";

        /// <summary>
        /// 号牌号码
        /// </summary>
        public string Hphm
        {
            get { return hphm; }
            set { hphm = value; }
        }

        private string sqjc = "";

        /// <summary>
        ///省区简称
        /// </summary>
        public string Sqjc
        {
            get { return sqjc; }
            set { sqjc = value; }
        }

        private string hpzl = "";

        /// <summary>
        ///号牌种类
        /// </summary>
        public string Hpzl
        {
            get { return hpzl; }
            set { hpzl = value; }
        }

        private string hpys = "";

        /// <summary>
        ///号牌颜色
        /// </summary>
        public string Hpys
        {
            get { return hpys; }
            set { hpys = value; }
        }

        private string sjly = "";

        /// <summary>
        ///数据来源
        /// </summary>
        public string Sjly
        {
            get { return sjly; }
            set { sjly = value; }
        }

        private string cllx = "";

        /// <summary>
        ///车辆类别
        /// </summary>
        public string Cllx
        {
            get { return cllx; }
            set { cllx = value; }
        }

        private string clpp = "";

        /// <summary>
        ///车辆品牌
        /// </summary>
        public string Clpp
        {
            get { return clpp; }
            set { clpp = value; }
        }

        private string clppText;

        /// <summary>
        /// 车辆品牌的文本
        /// </summary>
        public string ClppText
        {
            get { return clppText; }
            set { clppText = value; }
        }

        private string clzpp = "";

        /// <summary>
        ///车辆子品牌
        /// </summary>
        public string Clzpp
        {
            get { return clzpp; }
            set { clzpp = value; }
        }

        private string clzppText;

        /// <summary>
        /// 车辆子品牌文本
        /// </summary>
        public string ClzppText
        {
            get { return clzppText; }
            set { clzppText = value; }
        }

        private string csys = "";

        /// <summary>
        ///车身颜色
        /// </summary>
        public string Csys
        {
            get { return csys; }
            set { csys = value; }
        }

        private string cjjg = "";

        /// <summary>
        ///所属机构
        /// </summary>
        public string Cjjg
        {
            get { return cjjg; }
            set { cjjg = value; }
        }

        private string xssd = "";

        /// <summary>
        ///行驶速度
        /// </summary>
        public string Xssd
        {
            get { return xssd; }
            set { xssd = value; }
        }

        private string kkid = "";

        /// <summary>
        /// 监测点编号
        /// </summary>
        public string Kkid
        {
            get { return kkid; }
            set { kkid = value; }
        }

        private string kkidms = "";

        /// <summary>
        /// 监测点名称
        /// </summary>
        public string Kkidms
        {
            get { return kkidms; }
            set { kkidms = value; }
        }

        private string xsfx = "";

        /// <summary>
        ///行驶方向
        /// </summary>
        public string Xsfx
        {
            get { return xsfx; }
            set { xsfx = value; }
        }

        private string xscd = "";

        /// <summary>
        ///行驶车道
        /// </summary>
        public string Xscd
        {
            get { return xscd; }
            set { xscd = value; }
        }

        private bool njb = false;

        /// <summary>
        /// 年检标  有 true  没有false
        /// </summary>
        public bool Njb
        {
            get { return njb; }
            set { njb = value; }
        }

        private bool zjh = false;

        /// <summary>
        /// 纸巾盒  有 true  没有false
        /// </summary>
        public bool Zjh
        {
            get { return zjh; }
            set { zjh = value; }
        }

        private bool zyb = false;

        /// <summary>
        /// 遮阳板  有 true  没有false
        /// </summary>
        public bool Zyb
        {
            get { return zyb; }
            set { zyb = value; }
        }

        private bool dz = false;

        /// <summary>
        /// 吊坠  有 true  没有false
        /// </summary>
        public bool Dz
        {
            get { return dz; }
            set { dz = value; }
        }

        private bool bj = false;

        /// <summary>
        /// 摆件  有 true  没有false
        /// </summary>
        public bool Bj
        {
            get { return bj; }
            set { bj = value; }
        }

        private bool zjsaqd = false;

        /// <summary>
        /// 主驾驶安全带  有 true  没有false
        /// </summary>
        public bool Zjsaqd
        {
            get { return zjsaqd; }
            set { zjsaqd = value; }
        }

        private bool fjsaqd = false;

        /// <summary>
        /// 副驾驶安全带  有 true  没有false
        /// </summary>
        public bool Fjsaqd
        {
            get { return fjsaqd; }
            set { fjsaqd = value; }
        }

        private string queryMode;

        /// <summary>
        /// 查询模式 0 模糊查询  1 精确查询
        /// </summary>
        public string QueryMode
        {
            get { return queryMode; }
            set { queryMode = value; }
        }

        //下面五个字段是给接口中记日志用的
        private string userName;

        /// <summary>
        /// 用户名称（例如：（张三）哪个人）
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string userIp;

        /// <summary>
        /// 用户当前所在IP（例如：192.168.1.123 (一般为局域网中的IP)）
        /// </summary>
        public string UserIp
        {
            get { return userIp; }
            set { userIp = value; }
        }

        private string userCode;

        /// <summary>
        /// 用户编号（例如：000001 ）
        /// </summary>
        public string UserCode
        {
            get { return userCode; }
            set { userCode = value; }
        }

        private string dyzgnmkbh;

        /// <summary>
        /// 功能模块编号（例如：010501 ）
        /// </summary>
        public string Dyzgnmkbh
        {
            get { return dyzgnmkbh; }
            set { dyzgnmkbh = value; }
        }

        private string dyzgnmkmc;

        /// <summary>
        /// 功能模块名称（例如：综合查询 ）
        /// </summary>
        public string Dyzgnmkmc
        {
            get { return dyzgnmkmc; }
            set { dyzgnmkmc = value; }
        }
    }
}