using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace MyNet.Common.Lang
{
    public class Language
    {
        #region 私有变量

        private static Hashtable hsInstance = new Hashtable();
        private static string langAll = "";

        /// <summary>
        ///
        /// </summary>
        public static Language CreateInstance(string className)
        {
            langAll = System.Configuration.ConfigurationManager.AppSettings["LangType"].ToString();
            if (hsInstance.ContainsKey(className))
            {
                return (hsInstance[className] as LanguageInfo).Instance;
            }
            else
            {
                LanguageInfo info = new LanguageInfo();
                Language language = new Language(className, info);
                info.Instance = language;
                hsInstance.Add(className, info);
                return info.Instance;
            }
        }

        #endregion 私有变量

        #region public

        /// <summary>
        /// 初始化
        /// </summary>
        public Language(string className, LanguageInfo info)
        {
            try
            {
                info.ClassName = className;

                DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\\") + "\\Language");
                if (!di.Exists)
                {
                    di.Create();
                }

                XmlDocument doc = new XmlDocument();

                //读取配置
                string filename = AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\\") + "\\Language\\" + className + ".xml";
                if (File.Exists(filename))
                {
                    doc.Load(filename);
                }
                else
                {
                    string xml = CreateLangXml(className);
                    doc.LoadXml(xml);
                    doc.Save(filename);
                }
                info.Doc = doc;
                info.CurrentLang = GetDictionary(doc, "Message/Language");
            }
            catch
            {
            }
        }

        /// <summary>
        /// 获得转换后的语言
        /// </summary>
        /// <param name="codeValue"></param>
        /// <param name="desc"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public string GetLanguageStr(string codeValue, string desc, string className)
        {
            try
            {
                return GetDictionaryNode("Message/Dictionary", codeValue, desc, className);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion public

        #region 私有方法

        /// <summary>
        /// 创建xml
        /// </summary>
        /// <returns></returns>
        private string CreateLangXml(string className)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(declaration);
                XmlNode root = doc.CreateElement("Message");
                SetNodeValue(doc, "Language", "CN", root);
                XmlNode root1 = doc.CreateElement("Dictionary");
                XmlNode rootLang = doc.CreateElement(className);
                root1.AppendChild(rootLang);
                root.AppendChild(root1);
                doc.AppendChild(root);
                return doc.InnerXml;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="field"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        private XmlElement SetNodeValue(XmlDocument doc, string field, XmlNode root)
        {
            XmlElement node = doc.CreateElement(field);
            root.AppendChild(node);
            return node;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        private XmlNode SetNodeValue(XmlDocument doc, string field, string value, XmlNode root)
        {
            XmlElement node = doc.CreateElement(field);
            node.InnerText = value;
            root.AppendChild(node);
            return node;
        }

        /// <summary>
        ///获得对应节点的值
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="codeValue"></param>
        /// <param name="desc"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        private string GetDictionaryNode(string xpath, string codeValue, string desc, string className)
        {
            try
            {
                if (hsInstance.ContainsKey(className))
                {
                    LanguageInfo info = hsInstance[className] as LanguageInfo;
                    XmlDocument doc = info.Doc;
                    string currentLang;
                    if (!string.IsNullOrEmpty(langAll))
                    {
                        currentLang = langAll;
                    }
                    else
                    {
                        currentLang = info.CurrentLang;
                    }

                    XmlNode node = doc.SelectSingleNode(xpath + "/" + className + "/" + codeValue);
                    if (node != null)
                    {
                        return node.Attributes[currentLang].Value;
                    }
                    else
                    {
                        string filename = AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\\") + "\\Language\\" + className + ".xml";
                        XmlNode root = doc.SelectSingleNode(xpath);
                        XmlNode root2 = doc.SelectSingleNode(xpath + "/" + className);
                        XmlElement root3;
                        if (root2 == null)
                        {
                            XmlNode root1 = SetNodeValue(doc, className, root);
                            root3 = SetNodeValue(doc, codeValue, root1);
                        }
                        else
                        {
                            root3 = SetNodeValue(doc, codeValue, root2);
                        }
                        root3.SetAttribute("CN", desc);
                        root3.SetAttribute("EN", "");
                        root3.SetAttribute("JP", "");
                        root3.SetAttribute("RU", "");
                        doc.Save(filename);
                        return desc;
                    }
                }
                return desc;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        private string GetDictionary(XmlDocument doc, string xpath)
        {
            try
            {
                XmlNode node = doc.SelectSingleNode(xpath);
                if (node != null)
                {
                    return node.InnerText;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion 私有方法
    }

    public class LanguageInfo
    {
        public LanguageInfo()
        { }

        private Language instance;

        /// <summary>
        /// 实例
        /// </summary>
        public Language Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        private string className;

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        private string currentLang;

        /// <summary>
        /// 当前语言
        /// </summary>
        public string CurrentLang
        {
            get { return currentLang; }
            set { currentLang = value; }
        }

        private XmlDocument doc;

        /// <summary>
        /// 语言包XML
        /// </summary>
        public XmlDocument Doc
        {
            get { return doc; }
            set { doc = value; }
        }
    }
}