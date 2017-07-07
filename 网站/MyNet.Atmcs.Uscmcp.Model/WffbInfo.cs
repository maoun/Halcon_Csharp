using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Model
{
    /// <summary>
    /// 违法分布实体
    /// </summary>
    [Serializable]
    public class WffbInfo
    {
        public WffbInfo()
        { }

        private string dwmc;

        /// <summary>
        /// 单位名称
        /// </summary>
        public string Dwmc
        {
            get { return dwmc; }
            set { dwmc = value; }
        }

        private string wfxw0;

        /// <summary>
        /// 违法行为1
        /// </summary>
        public string Wfxw0
        {
            get { return wfxw0; }
            set { wfxw0 = value; }
        }

        private string wfxw1;

        /// <summary>
        /// 违法行为2
        /// </summary>
        public string Wfxw1
        {
            get { return wfxw1; }
            set { wfxw1 = value; }
        }

        private string wfxw2;

        /// <summary>
        /// 违法行为3
        /// </summary>
        public string Wfxw2
        {
            get { return wfxw2; }
            set { wfxw2 = value; }
        }

        private string wfxw3;

        /// <summary>
        /// 违法行为4
        /// </summary>
        public string Wfxw3
        {
            get { return wfxw3; }
            set { wfxw3 = value; }
        }

        private string wfxw4;

        /// <summary>
        /// 违法行为5
        /// </summary>
        public string Wfxw4
        {
            get { return wfxw4; }
            set { wfxw4 = value; }
        }

        private string wfxw5;

        /// <summary>
        /// 违法行为6
        /// </summary>
        public string Wfxw5
        {
            get { return wfxw5; }
            set { wfxw5 = value; }
        }
    }
}
