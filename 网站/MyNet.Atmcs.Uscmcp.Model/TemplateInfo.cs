using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Model
{
    /// <summary>
    /// 模版实体
    /// </summary>
    [Serializable]
    public class TemplateInfo
    {

        private string templateId;

        /// <summary>
        /// 模版ID
        /// </summary>
        public string TemplateId
        {
            get { return templateId; }
            set { templateId = value; }
        }

        private string templatePage;

        /// <summary>
        /// 模版URL
        /// </summary>
        public string TemplatePage
        {
            get { return templatePage; }
            set { templatePage = value; }
        }
    }
}
