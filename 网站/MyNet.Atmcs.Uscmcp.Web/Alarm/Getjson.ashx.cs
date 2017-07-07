using MyNet.Atmcs.Uscmcp.Bll;
using System.Data;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace MyNet.Atmcs.Uscmcp.Web
{
    /// <summary>
    /// Getjson 的摘要说明
    /// </summary>
    public class Getjson : IHttpHandler, IRequiresSessionState
    {
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private static string departImg = "../KakouSelect/css/ext.png";
        private static string kakouImg = "../KakouSelect/css/threemenu.png.gif";

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string json = GetJson(context);//得到Json字符串
            context.Response.Write(json);
        }

        /// <summary>
        /// 得到一个Json字符串
        /// </summary>
        /// <returns></returns>
        public string GetJson(HttpContext context)
        {
            #region 第二版

            //DataTable dt = null;//得到部门
            //DataTable dt1 = tgsDataInfo.GetJson(out dt);//得到部门下面的卡口
            //StringBuilder str = new StringBuilder();
            //str.AppendLine("[");
            //str.AppendLine("{\"id\": 1,\"text\":\"" + dt.Rows[0]["departname"] + "\",\"iconCls\":\"icon-ext\", \"children\":[");
            //int id1 = 101;
            //int id2 = 201;

            //DataRow[] rows = dt1.Select("DEPARTID=" + dt.Rows[0]["DEPARTID"].ToString());
            //if (rows.Length > 0)
            //{
            //    DataTable dt2 = ToDataTable(rows);
            //    for (int j = 0; j < dt2.Rows.Count; j++)
            //    {
            //        str.AppendLine("{\"id\": " + id1 + ",\"text\":\"" + dt2.Rows[j]["station_name"] + "\",\"iconCls\":\"icon-san\", \"attributes\":{\"kkid\":\"" + dt2.Rows[j]["station_id"] + "\"}},");
            //        id1++;
            //    }
            //}
            //for (int i = 1; i < dt.Rows.Count; i++)
            //{
            //    if (i == dt.Rows.Count - 1)
            //    {
            //        str.AppendLine("{\"id\": " + id1 + ",\"text\":\"" + dt.Rows[i]["departname"] + "\",\"iconCls\":\"icon-ext\", \"children\":[");
            //        DataRow[] rows1 = dt1.Select("DEPARTID=" + dt.Rows[i]["DEPARTID"].ToString());
            //        if (rows1.Length > 0)
            //        {
            //            DataTable dt2 = ToDataTable(rows);
            //            for (int j = 0; j < dt2.Rows.Count; j++)
            //            {
            //                if (j == dt2.Rows.Count - 1)
            //                {
            //                    str.AppendLine("{\"id\": " + id2 + ",\"text\":\"" + dt2.Rows[j]["station_name"] + "\",\"iconCls\":\"icon-san\", \"attributes\":{\"kkid\":\"" + dt2.Rows[j]["station_id"] + "\"}}");
            //                }
            //                else
            //                {
            //                    str.AppendLine("{\"id\": " + id2 + ",\"text\":\"" + dt2.Rows[j]["station_name"] + "\",\"iconCls\":\"icon-san\", \"attributes\":{\"kkid\":\"" + dt2.Rows[j]["station_id"] + "\"}},");
            //                }

            //                id2++;
            //            }
            //        }
            //        str.AppendLine("]}");
            //    }
            //    else
            //    {
            //        str.AppendLine("{\"id\": " + id1 + ",\"text\":\"" + dt.Rows[i]["departname"] + "\",\"iconCls\":\"icon-ext\", \"children\":[");
            //        DataRow[] rows1 = dt1.Select("DEPARTID=" + dt.Rows[i]["DEPARTID"].ToString());
            //        if (rows1.Length > 0)
            //        {
            //            DataTable dt2 = ToDataTable(rows);
            //            for (int j = 0; j < dt2.Rows.Count; j++)
            //            {
            //                if (j == dt2.Rows.Count - 1)
            //                {
            //                    str.AppendLine("{\"id\": " + id2 + ",\"text\":\"" + dt2.Rows[j]["station_name"] + "\",\"iconCls\":\"icon-san\", \"attributes\":{\"kkid\":\"" + dt2.Rows[j]["station_id"] + "\"}}");
            //                }
            //                else
            //                {
            //                    str.AppendLine("{\"id\": " + id2 + ",\"text\":\"" + dt2.Rows[j]["station_name"] + "\",\"iconCls\":\"icon-san\", \"attributes\":{\"kkid\":\"" + dt2.Rows[j]["station_id"] + "\"}},");
            //                }
            //                id2++;
            //            }
            //        }
            //        str.AppendLine("]},");
            //    }

            //    id1++;
            //}
            //str.AppendLine("],\"attributes\":{\"kkid\":\"\"}}");

            //str.AppendLine("]");
            //string strs = str.ToString();
            //return strs;

            #endregion 第二版

            try
            {
                DataTable dt = null;//得到部门

                DataTable dt1 = tgsDataInfo.GetJson(out dt);//得到部门下面的卡口
                if (context.Session["selectKakou"] == null)
                {
                    context.Session["selectKakou"] = dt1;
                }

                DataTable dtSelectBumen = new DataTable();//得到当前选择卡口的部门
                dtSelectBumen = dt1.Clone();//复制表结构
                StringBuilder str = new StringBuilder();
                str.AppendLine("[");
                if (context.Session["tree"] != null)//给卡口选中的状态
                {
                    string value = context.Session["tree"].ToString();
                    string[] values = null;
                    if (value.Contains(","))//说明选择了多个卡口
                    {
                        values = value.Split(',');
                        //根据卡口得到上级部门的编号
                        for (int i = 0; i < values.Length; i++)
                        {
                            DataRow[] rows = dt1.Select("STATION_ID=" + values[i].ToString());
                            if (dtSelectBumen != null && dtSelectBumen.Rows.Count > 0)
                            {
                                if (dtSelectBumen.Select("departid=" + rows[0]["departid"]).Length > 0)
                                {
                                    continue;
                                }
                            }
                            dtSelectBumen.Rows.Add(rows[0].ItemArray);//加了ItemArray，不会报错该表属于另一个表的错误
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)//绑定部门
                        {
                            if (dt.Rows[i]["departid"].ToString().Equals("371600000000"))
                            {
                                str.AppendLine("{id:371600000000,pId:000000000000,checked:true,name:\"滨州市交通警察支队\", open: true, check: true,icon:\"" + departImg + "\"},");
                                continue;
                            }
                            if (dtSelectBumen.Select("departid=" + dt.Rows[i]["departid"]).Length > 0)
                            {
                                str.AppendLine("{id:" + dt.Rows[i]["departid"] + ",pId:" + dt.Rows[i]["classcode"] + ",checked:true,name:\"" + dt.Rows[i]["departname"] + "\", open: true, check: true,icon:\"" + departImg + "\"},");
                            }
                            else
                            {
                                str.AppendLine("{id:" + dt.Rows[i]["departid"] + ",pId:" + dt.Rows[i]["classcode"] + ",name:\"" + dt.Rows[i]["departname"] + "\", open: true, check: true,icon:\"" + departImg + "\"},");
                            }
                        }
                        for (int j = 0; j < dt1.Rows.Count; j++)//绑定部门下面的卡口
                        {
                            if (j == dt1.Rows.Count - 1)
                            {
                                if (IsHave(values, dt1.Rows[j]["station_id"].ToString()))
                                {
                                    str.AppendLine("{id:" + dt1.Rows[j]["station_id"] + ",checked:true,pId:" + dt1.Rows[j]["departid"] + ",name:\"" + dt1.Rows[j]["station_name"] + "\",icon:\"" + kakouImg + "\"},");
                                }
                                else
                                {
                                    str.AppendLine("{id:" + dt1.Rows[j]["station_id"] + ",pId:" + dt1.Rows[j]["departid"] + ",name:\"" + dt1.Rows[j]["station_name"] + "\",icon:\"" + kakouImg + "\"},");
                                }
                                //str.AppendLine("{id:" + dt1.Rows[j]["station_id"] + ",pId:" + dt1.Rows[j]["departid"] + ",name:\"" + dt1.Rows[j]["station_name"] + "\", icon:\"" + kakouImg + "\"}");
                            }
                            else
                            {
                                if (IsHave(values, dt1.Rows[j]["station_id"].ToString()))//判断数组中是否存在
                                {
                                    str.AppendLine("{id:" + dt1.Rows[j]["station_id"] + ",checked:true,pId:" + dt1.Rows[j]["departid"] + ",name:\"" + dt1.Rows[j]["station_name"] + "\",icon:\"" + kakouImg + "\"},");
                                }
                                else
                                {
                                    str.AppendLine("{id:" + dt1.Rows[j]["station_id"] + ",pId:" + dt1.Rows[j]["departid"] + ",name:\"" + dt1.Rows[j]["station_name"] + "\",icon:\"" + kakouImg + "\"},");
                                }
                            }
                        }
                    }
                    else//只选择一个卡口
                    {
                        //根据卡口得到上级部门的编号

                        DataRow[] rows = dt1.Select("STATION_ID=" + value.ToString());
                        if (rows.Length > 0)
                        {
                            dtSelectBumen.Rows.Add(rows[0].ItemArray);//加了ItemArray，不会报错该表属于另一个表的错误
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)//绑定部门
                        {
                            if (dt.Rows[i]["departid"].ToString().Equals("371600000000"))
                            {
                                str.AppendLine("{id:371600000000,pId:000000000000,checked:true,name:\"滨州市交通警察支队\", open: true, check: true,icon:\"" + departImg + "\"},");
                                continue;
                            }
                            if (dtSelectBumen.Select("departid=" + dt.Rows[i]["departid"]).Length > 0)
                            {
                                str.AppendLine("{id:" + dt.Rows[i]["departid"] + ",pId:" + dt.Rows[i]["classcode"] + ",checked:true,name:\"" + dt.Rows[i]["departname"] + "\", open: true, check: true,icon:\"" + departImg + "\"},");
                            }
                            else
                            {
                                str.AppendLine("{id:" + dt.Rows[i]["departid"] + ",pId:" + dt.Rows[i]["classcode"] + ",name:\"" + dt.Rows[i]["departname"] + "\", open: true, check: true,icon:\"" + departImg + "\"},");
                            }
                        }
                        for (int j = 0; j < dt1.Rows.Count; j++)//绑定部门下面的卡口
                        {
                            if (j == dt1.Rows.Count - 1)
                            {
                                if (value.Equals(dt1.Rows[j]["station_id"].ToString()))
                                {
                                    str.AppendLine("{id:" + dt1.Rows[j]["station_id"] + ",checked:true,pId:" + dt1.Rows[j]["departid"] + ",name:\"" + dt1.Rows[j]["station_name"] + "\",icon:\"" + kakouImg + "\"},");
                                }
                                else
                                {
                                    str.AppendLine("{id:" + dt1.Rows[j]["station_id"] + ",pId:" + dt1.Rows[j]["departid"] + ",name:\"" + dt1.Rows[j]["station_name"] + "\",icon:\"" + kakouImg + "\"},");
                                }
                                //str.AppendLine("{id:" + dt1.Rows[j]["station_id"] + ",pId:" + dt1.Rows[j]["departid"] + ",name:\"" + dt1.Rows[j]["station_name"] + "\", icon:\"" + kakouImg + "\"}");
                            }
                            else
                            {
                                if (value.Equals(dt1.Rows[j]["station_id"].ToString()))//判断是否存在
                                {
                                    str.AppendLine("{id:" + dt1.Rows[j]["station_id"] + ",checked:true,pId:" + dt1.Rows[j]["departid"] + ",name:\"" + dt1.Rows[j]["station_name"] + "\",icon:\"" + kakouImg + "\"},");
                                }
                                else
                                {
                                    str.AppendLine("{id:" + dt1.Rows[j]["station_id"] + ",pId:" + dt1.Rows[j]["departid"] + ",name:\"" + dt1.Rows[j]["station_name"] + "\",icon:\"" + kakouImg + "\"},");
                                }
                            }
                        }
                    }
                }
                else //没选择任何卡口
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //if (dt.Rows[i]["departid"].ToString().Equals("371600000000"))
                        //{
                        //    str.AppendLine("{id:" + dt.Rows[i]["departid"] + ",pid:" + dt.Rows[i]["departid"] + ",name:\"" + dt.Rows[i]["departname"] + "\", open: true, check: true,icon:\"" + departImg + "\"},");
                        //}
                        //else
                        //{
                        str.AppendLine("{id:" + dt.Rows[i]["departid"] + ",pId:" + dt.Rows[i]["classcode"] + ",name:\"" + dt.Rows[i]["departname"] + "\", open: true, check: true,icon:\"" + departImg + "\"},");
                        // }
                    }
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        if (j == dt1.Rows.Count - 1)
                        {
                            str.AppendLine("{id:" + dt1.Rows[j]["station_id"] + ",pId:" + dt1.Rows[j]["departid"] + ",name:\"" + dt1.Rows[j]["station_name"] + "\", icon:\"" + kakouImg + "\"}");
                        }
                        else
                        {
                            str.AppendLine("{id:" + dt1.Rows[j]["station_id"] + ",pId:" + dt1.Rows[j]["departid"] + ",name:\"" + dt1.Rows[j]["station_name"] + "\",icon:\"" + kakouImg + "\"},");
                        }
                    }
                }

                str.AppendLine("]");
                string strs = str.ToString();
                return strs;
            }
            catch (System.Exception)
            {
                return "";
            }
        }

        public DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.ImportRow(row);  // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        /// 判断数组是否包含当前项
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsHave(string[] strs, string id)
        {
            /*此方法有两个参数，第一个是要查找的字符串数组，第二个是要查找的字符或字符串
            * */
            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i].IndexOf(id) != -1)
                {//循环查找字符串数组中的每个字符串中是否包含所有查找的内容
                    return true;//查找到了就返回真，不在继续查询
                }
            }
            return false;//没找到返回false
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}