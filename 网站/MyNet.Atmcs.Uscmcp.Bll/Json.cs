using System.Collections;
using System.Text.RegularExpressions;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    /// <summary>
    /// 解析Json为Hashtable, 所有值保存为字符串
    /// </summary>
    public class Json : Hashtable
    {
        /// <summary>
        /// 获取json转换是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 获取json对应的字符串
        /// </summary>
        public string Value
        {
            get
            {
                return JsonToString(this);
            }
        }

        /// <summary>
        /// 初始化json
        /// </summary>
        /// <param name="value">json 字符串</param>
        public Json(string value)
        {
            CreateJson(value);
        }

        public Json()
        {
        }

        #region CreateJson

        /// <summary>
        /// 创建Json对象
        /// </summary>
        /// <param name="value">json字符串</param>
        private void CreateJson(string value)
        {
            if (value == null)
            {
                return;
            }
            else
            {
                value = value.Replace("null", "\"\"").Replace("true", "\"true\"").Replace("false", "\"false\"");
            }

            string name = "";
            Regex nameR = new Regex(@"[a-zA-Z\'\""]+[0-9]*[a-zA-Z\'\""]*[\s]*[\:]");
            Match nameM = nameR.Match(value);
            while (nameM.Success)
            {
                name = nameM.Value.Replace("'", "").Replace("\"", "").Replace(":", "");

                //当前名称的下一个对应字母(', ", [, {, 0-9, 以,和}标记为值的开始则表示值为null
                Regex startR = new Regex(@"[0-9'""\[\{]");
                Match startM = startR.Match(value, nameM.Index + nameM.Length);
                //匹配成功

                if (startM.Success)
                {
                    string start = startM.Value;
                    int i = startM.Index;
                    while (i < value.Length - 1)
                    {
                        i++;
                        //跳过'\'字符
                        if (value[i].ToString() == "\\")
                        {
                            i++;
                            continue;
                        }
                        char vv = value[i];
                        start = Start(start, vv);
                        if (start == "")
                        {
                            break;
                        }
                    }
                    string str = value.Substring(startM.Index, i - startM.Index + 1);

                    nameM = nameR.Match(value, i + 1);

                    char key = str[0];
                    char key2 = str[1];
                    if (key == '{')
                    {
                        this[name] = new Json(str);
                    }
                    else if (key == '[' && key2 == '{')
                    {
                        this[name] = CreateJsonArr(str);
                    }
                    else if (key == '\'' || key == '"')
                    {
                        this[name] = str.Substring(1, str.Length - 2).Replace("\\\\", "\\").Replace("\\'", "'").Replace("\\\"", "\"");
                    }
                    else if (key == '[')
                    {
                        this[name] = str.Replace(" ", "").Replace("\n", "").Replace("}", "").Replace("\r", "");
                    }
                    else
                    {
                        this[name] = str.Replace(" ", "").Replace("\n", "").Replace("}", "").Replace(",", "").Replace("\r", "");
                    }
                }
                else
                {
                    Clear();
                    Success = false;
                    return;
                }
            }
            Success = true;
        }

        #endregion CreateJson

        #region CreateJsonArr

        /// <summary>
        /// 创建Json数组对象
        /// </summary>
        /// <param name="value">数组对象字符串包含[]</param>
        /// <returns></returns>
        private Json[] CreateJsonArr(string value)
        {
            Regex startR = new Regex(@"[\{]");
            Match startM = startR.Match(value);
            int n = 0;
            while (startM.Success)
            {
                string start = "{";
                int i = startM.Index;
                while (i < value.Length - 1)
                {
                    i++;
                    if (value[i].ToString() == "\\")
                    {
                        i++;
                        continue;
                    }
                    start = Start(start, value[i]);
                    if (start == "")
                    {
                        break;
                    }
                }
                startM = startR.Match(value, i + 1);
                n++;
            }

            Json[] jsons = new Json[n];

            startM = startR.Match(value);
            n = 0;
            while (startM.Success)
            {
                string start = "{";
                int i = startM.Index;
                while (i < value.Length - 1)
                {
                    i++;
                    if (value[i].ToString() == "\\")
                    {
                        i++;
                        continue;
                    }
                    start = Start(start, value[i]);
                    if (start == "")
                    {
                        break;
                    }
                }
                string str = value.Substring(startM.Index, i - startM.Index + 1);
                jsons[n] = new Json(str);
                startM = startR.Match(value, i + 1);
                n++;
            }
            return jsons;
        }

        #endregion CreateJsonArr

        #region Start判断

        /// <summary>
        /// 判断出节点包含字符串
        /// </summary>
        /// <param name="start"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string Start(string start, char key)
        {
            //模拟伐的最后一个值
            char end = start[start.Length - 1];

            if (key == '\'' || key == '"')
            {
                if (end == key)
                {
                    start = start.Substring(0, start.Length - 1);
                }
                else
                {
                    start += key.ToString();
                }
                return start;
            }

            if (end == '{')
            {
                end = '}';
                if (key == '[' || key == '{')
                {
                    start += key.ToString();
                }
            }
            else if (end == '[')
            {
                end = ']';
                if (key == '[' || key == '{')
                {
                    start += key.ToString();
                }
            }

            //如果是结束字符,则删除对应开始字符
            Regex numR = new Regex(@"[0-9]");
            //如果数字作为开始字符
            if (numR.IsMatch(end.ToString()))
            {
                if (key == ',' || key == '}')
                {
                    start = "";
                }
            }
            else
            {
                if (key == end)
                {
                    start = start.Substring(0, start.Length - 1);
                }
            }
            return start;
        }

        #endregion Start判断

        #region Json转字符串

        private string JsonToString()
        {
            return JsonToString(this);
        }

        private string JsonToString(Json j)
        {
            string str = "{";
            int n = 0;
            foreach (DictionaryEntry de in j)
            {
                if (n > 0)
                {
                    str += ",";
                }
                n++;
                str += de.Key.ToString() + ":";
                if (de.Value == null)
                {
                    str += "null";
                }
                else if (de.Value.ToString().Split('.')[de.Value.ToString().Split('.').Length - 1] == "Json[]")
                {
                    Json[] js = (Json[])this[de.Key];
                    str += "[";
                    for (int i = 0; i < js.Length; i++)
                    {
                        if (i > 0)
                        {
                            str += ",";
                        }
                        str += JsonToString(js[i]);
                    }
                    str += "]";
                }
                else
                {
                    str += "\"" + de.Value.ToString().Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"") + "\"";

                    string abc = str;
                }
            }
            return str + "}";
        }

        #endregion Json转字符串

        /// <summary>
        /// 给名称为name的Json数组添加一个json对象
        /// </summary>
        /// <param name="name">json数组名称</param>
        /// <param name="aj">需要添加的json对象</param>
        public void Add(string name, Json aj)
        {
            Json[] olds = new Json[0];
            if (this[name] != null)
            {
                olds = (Json[])this[name];
            }
            Json[] news = new Json[olds.Length + 1];
            for (int i = 0; i < olds.Length; i++)
            {
                news[i] = (Json)olds[i];
            }
            news[olds.Length] = aj;

            this[name] = news;
        }
    }
}