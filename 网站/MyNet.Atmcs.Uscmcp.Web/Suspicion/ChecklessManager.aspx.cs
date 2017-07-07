using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;
using MyNet.Atmcs.Uscmcp.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Threading;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class ChecklessManager : System.Web.UI.Page
    {
        #region 成员变量

        private TgsPproperty tgsPproperty = new TgsPproperty();
        private TgsDataInfo tgsDataInfo = new TgsDataInfo();
        private UserLogin userLogin = new UserLogin();
        private static DataTable mydt;
        private MyNet.Atmcs.Uscmcp.Bll.LogManager logManager = new Bll.LogManager();

        private static DataTable dtcl = null;
        private static string uName = "";
        private static string nowIp = "";

        #endregion 成员变量

        #region 控件事件

        /// <summary>
        /// 初始化页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e) 
        {
            string username = Request.QueryString["username"];

            if (!userLogin.CheckLogin(username))
            {
                string js = "alert('" + GetLangStr("ChecklessManager47", "您没有登录或操作超时，请重新登录!") + "');window.top.location.href='" + StaticInfo.LoginPage + "'";
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return;
            }
            if (!X.IsAjaxRequest)
            {
                StoreDataBind();
                TbutQueryClick(null, null);
                this.DataBind();
                UserInfo userinfo = Session["Userinfo"] as UserInfo;
                uName = userinfo.UserName;
                nowIp = userinfo.NowIp;
                logManager.InsertLogRunning(userinfo.UserName, GetLangStr("ChecklessManager48", "访问：") + Request.QueryString["funcname"], userinfo.NowIp, "0");
            }
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            ChecklessDataBind(GetWhere());
        }

        /// <summary>
        /// 刷新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButRefreshClick(object sender, DirectEventArgs e)
        {
            ChecklessDataBind("1=1");
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButResetClick(object sender, DirectEventArgs e)
        {
            CmbPlateType.Reset();
            CmbQueryMdlx.Reset();
            TxtplateId.Reset();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            ChecklessDataBind("1=1");
        }

        /// <summary>
        /// 选择布控记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectCheckless(object sender, DirectEventArgs e)
        {
            object data = e.ExtraParams["sdata"];
            string sdata = data.ToString();
            txtID.Text = GetdatabyField(sdata, "col0");
            txtHphm.Text = GetdatabyField(sdata, "col1");
            cmbHpzl.Value = GetdatabyField(sdata, "col2");
            cmbHpzl.SelectedItem.Text = GetdatabyField(sdata, "col3");
            cmbCsys.SelectedItem.Text = GetdatabyField(sdata, "col4");
            txtClpp.Text = GetdatabyField(sdata, "col5");
            cmbMdlx.Value = GetdatabyField(sdata, "col6");
            cmbMdlx.SelectedItem.Text = GetdatabyField(sdata, "col7");
            DateYxsj.Text = GetDate(sdata, "col8", 1);
            // TimeYxsj.Text = GetDate(sdata, "col8", 2);
            txtSjyy.Text = GetdatabyField(sdata, "col9");
            uiDepartment.DepertName = GetdatabyField(sdata, "col11");
            cmbbdbj.Value = GetdatabyField(sdata, "col12");
            cmbbdbj.SelectedItem.Text = GetdatabyField(sdata, "col13");
            TxtBz.Text = GetdatabyField(sdata, "col14");
            TxtGxsj.Text = GetDate(GetdatabyField(sdata, "col15"), 0);
        }

        /// <summary>
        /// 打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButPrintClick(object sender, DirectEventArgs e)
        {
            DataTable dt = ChangeDataTable();
            if (dt != null)
            {
                Session["printdatatable"] = ChangeDataTable();
                string xml = Bll.Common.GetPrintXml(GetLangStr("ChecklessManager49", "畅行车辆查询信息列表"), "", "", "printdatatable");
                string js = "OpenPrintPageV(\"" + xml + "\");";
                this.ResourceManager1.RegisterAfterClientInitScript(js);
            }
        }

        /// <summary>
        /// 导出xml 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToXml(object sender, EventArgs e)
        {
            DataTable dt = ChangeDataTable();
            ConvertData.ExportXml(dt, this);
        }

        /// <summary>
        /// 导出excel事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToExcel(object sender, EventArgs e)
        {
            DataTable dt = ChangeDataTable();
            ConvertData.ExportExcel(dt, this);
        }

        /// <summary>
        /// 导出csv事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToCsv(object sender, EventArgs e)
        {
            DataTable dt = ChangeDataTable();
            ConvertData.ExportCsv(dt, this);
        }

        /// <summary>
        /// 开始上传并导入excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void StartLongAction(object sender, DirectEventArgs e)
        {
            this.Session["checkessCount"] = 0;
            string filepath = UploadExcelFile();
            if (!string.IsNullOrEmpty(filepath))
            {
                DataSet ds = new DataSet();
                ds = ConvertData.ExcelToDataSet(@"" + filepath + "", "sheet1");
                mydt = new DataTable();
                mydt = ds.Tables[0];
                if (mydt != null && mydt.Rows.Count > 0)
                {
                    ThreadPool.QueueUserWorkItem(BatchUpdateData);
                    this.ResourceManager1.AddScript("{0}.startTask('SaveExcelData');", TaskManager1.ClientID);
                }
            }
            File.Delete(filepath);
        }

        /// <summary>
        /// 进度条事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RefreshProgress(object sender, DirectEventArgs e)
        {
            object progress = this.Session["checkessCount"];
            object result = this.Session["checkessResult"]; ;
            if (mydt != null && mydt.Rows.Count > 0)
            {
                int allCount = mydt.Rows.Count;
                float count = mydt.Rows.Count;
                if (progress != null)
                {
                    this.Progress1.UpdateProgress(((int)progress) / count, string.Format("Step {0} of {1}...", progress.ToString(), allCount));
                }
                else
                {
                    this.ResourceManager1.AddScript("{0}.stopTask('SaveExcelData');", TaskManager1.ClientID);
                    this.Progress1.UpdateProgress(1, GetLangStr("ChecklessManager50", "完成!"));
                    ChecklessDataBind("1=1");
                    X.Msg.Alert(GetLangStr("ChecklessManager51", "信息提示"), GetLangStr("ChecklessManager52", "总共有") + allCount.ToString() + GetLangStr("ChecklessManager53", "条记录，成功添加") + result.ToString() + GetLangStr("ChecklessManager74", "条记录！")).Show();
                }
            }
        }

        #endregion 控件事件

        #region DirectMethod事件

        /// <summary>
        /// 工具条切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Unnamed_Event(object sender, DirectEventArgs e)
        {
            if (TabStrip1.ActiveTabIndex == 0)
            {
                Progress1.Hidden = true;
            }
            else
            {
                Progress1.Hidden = false;
            }
        }

        /// <summary>
        /// 保存布控信息事件
        /// </summary>
        [DirectMethod]
        public void InfoSave()
        {
            AddCheckless();
        }

        /// <summary>
        /// 删除确认事件
        /// </summary>
        [DirectMethod]
        public void DoConfirm()
        {
            string Id = txtHphm.Text;
            X.Msg.Confirm(GetLangStr("ChecklessManager54", "信息"), GetLangStr("ChecklessManager55", "确认要删除[") + Id + GetLangStr("ChecklessManager56", "]这条记录吗?"), new MessageBoxButtonsConfig
            {
                Yes = new MessageBoxButtonConfig
                {
                    Handler = "ChecklessManager.DoYes()",
                    Text = GetLangStr("ChecklessManager57", "是")
                },
                No = new MessageBoxButtonConfig
                {
                    Handler = "ChecklessManager.DoNo()",
                    Text = GetLangStr("ChecklessManager58", "否")
                }
            }).Show();
        }

        /// <summary>
        /// 确定事件
        /// </summary>
        [DirectMethod]
        public void DoYes()
        {
            Hashtable hs = new Hashtable();
            hs.Add("xh", txtID.Text);
            if (tgsDataInfo.DeleteChecklessInfo(hs) > 0)
            {
                //DataRow[] dr = dtcl.Select("col0='"+txtID.Text+"'");
                //string cphms = dr[0]["col1"].ToString();
                //string clmcs = dr[0]["col3"].ToString();
                //logManager.InsertLogRunning(uName, "删除:号牌号码为:[" + cphms + "];号牌种类:[" + clmcs + "]", nowIp, "3");
                Notice(GetLangStr("ChecklessManager51", "信息提示"), GetLangStr("ChecklessManager59", "删除成功"));
                ChecklessDataBind("1=1");
            }
        }

        /// <summary>
        /// 不删除事件
        /// </summary>
        [DirectMethod]
        public void DoNo()
        {
        }

        /// <summary>
        /// 模版下载事件
        /// </summary>
        [DirectMethod]
        public void Download()
        {
            Response.ContentType = "application/x-zip-compressed";
            string fileName = GetLangStr("ChecklessManager60", "畅通车辆批量导入模版.xls");
            Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            string filename = Server.MapPath("Export/checkless.xsl");
            Response.TransmitFile(filename);
        }

        /// <summary>
        /// 更新布控记录
        /// </summary>
        [DirectMethod]
        public void UpdateData()
        {
            Hashtable hs = new Hashtable();
            hs.Add("xh", txtID.Text);
            hs.Add("hphm", txtHphm.Text);
            hs.Add("clpp", txtClpp.Text);
            hs.Add("sjyy", txtSjyy.Text);
            hs.Add("bz", TxtBz.Text);
            if (DateYxsj.SelectedDate != null)
            {
                hs.Add("yxsj", DateYxsj.SelectedDate.ToString("yyyy-MM-dd"));//DateYxsj.SelectedDate.ToString("yyyy-MM-dd")
            }
            if (cmbHpzl.SelectedIndex != -1)
            {
                hs.Add("hpzl", cmbHpzl.Value);
            }
            if (cmbCsys.SelectedIndex != -1)
            {
                hs.Add("csys", cmbCsys.Value);
            }
            if (cmbMdlx.SelectedIndex != -1)
            {
                hs.Add("mdlx", cmbMdlx.Value);
            }
            if (!string.IsNullOrEmpty(uiDepartment.DepertId))
            {
                hs.Add("sjly", uiDepartment.DepertId);
            }
            else
            {
                hs.Add("sjly", "");
            }
            if (cmbbdbj.SelectedIndex != -1)
            {
                hs.Add("bdbj", cmbbdbj.Value);
            }
            if (tgsDataInfo.UpdateChecklessInfo(hs) > 0)
            {
                Notice(GetLangStr("ChecklessManager61", "比对信息提示"), GetLangStr("ChecklessManager62", "保存成功"));
                ChecklessDataBind("1=1");
            }
            else
            {
                Notice(GetLangStr("ChecklessManager51", "信息提示"), GetLangStr("ChecklessManager63", "保存失败"));
            }
        }

        #endregion DirectMethod事件

        #region 私有方法

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void StoreDataBind()
        {
            try
            {
                DataTable dt1 = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:140001"));
                if (dt1 != null)
                {
                    //车俩类型
                    this.StorePlateType.DataSource = dt1;
                    this.StorePlateType.DataBind();
                }
                else
                {
                    this.StorePlateType.DataSource = tgsPproperty.GetPalteType();
                    this.StorePlateType.DataBind();
                }
                DataTable dt2 = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240018"));
                if (dt2 != null)
                {
                    //比对类型
                    this.StoreMdlx.DataSource = dt2;
                    this.StoreMdlx.DataBind();
                }
                else
                {
                    this.StoreMdlx.DataSource = tgsPproperty.GetChecklessDict();
                    this.StoreMdlx.DataBind();
                }

                DataTable dt3 = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240013"));
                if (dt3 != null)
                {
                    //车辆颜色
                    this.StoreColor.DataSource = dt3;
                    this.StoreColor.DataBind();
                }
                else
                {
                    this.StoreColor.DataSource = tgsPproperty.GetCarColorDict();
                    this.StoreColor.DataBind();
                }
                DataTable dt4 = Bll.Common.ChangColName(GetRedisData.GetData("t_sys_code:240010"));
                if (dt4 != null)
                {
                    //比对标志
                    this.StoreBdbj.DataSource = dt4;
                    this.StoreBdbj.DataBind();
                }
                else
                {
                    this.StoreBdbj.DataSource = tgsPproperty.GetIsCompareDict();
                    this.StoreBdbj.DataBind();
                }

                ChecklessDataBind("1=1");
                TxtGxsj.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch { }
        }

        /// <summary>
        /// 绑定布控数据
        /// </summary>
        /// <param name="where"></param>
        private void ChecklessDataBind(string where)
        {
            try
            {
                DataTable dt = dtcl = tgsDataInfo.GetCheckless(where);
                this.StoreCheckless.DataSource = dt;
                this.StoreCheckless.DataBind();
                Session["datatable"] = dt;
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        SelectFirst(dt.Rows[0]);
                        ToolExport.Disabled = false;
                    }
                    else
                    {
                        ToolExport.Disabled = true;
                    }
                }
            }
            catch
            {
            }
        }

        private void BatchUpdateData(object state)
        {
            int count = 0;
            for (int i = 0; i < mydt.Rows.Count; i++)
            {
                try
                {
                    Hashtable hs = new Hashtable();
                    hs.Add("xh", tgsDataInfo.GetTgsRecordId());
                    hs.Add("hphm", mydt.Rows[i][GetLangStr("ChecklessManager28", "号牌号码")].ToString());
                    hs.Add("clpp", mydt.Rows[i][GetLangStr("ChecklessManager33", "车辆品牌")].ToString());
                    hs.Add("sjyy", GetLangStr("ChecklessManager64", "畅行车辆"));
                    hs.Add("bz", mydt.Rows[i][GetLangStr("ChecklessManager65", "备注")].ToString());
                    if (DateYxsj.SelectedDate != null)
                    {
                        hs.Add("yxsj", DateTime.Now.AddYears(3).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    hs.Add("hpzl", Bll.Common.GetHpzl(mydt.Rows[i][GetLangStr("ChecklessManager29", "号牌种类")].ToString()));
                    hs.Add("csys", Bll.Common.GetCsys(mydt.Rows[i][GetLangStr("ChecklessManager31", "车身颜色")].ToString()));
                    hs.Add("mdlx", "Z");
                    try
                    {
                        hs.Add("sjly", mydt.Rows[i][GetLangStr("ChecklessManager66", "数据来源机构代码")].ToString());
                    }
                    catch
                    {
                        hs.Add("sjly", "");
                    }
                    hs.Add("bdbj", "1");

                    if (tgsDataInfo.UpdateChecklessInfo(hs) > 0)
                    {
                        count++;
                    }
                    Session["checkessCount"] = i;
                }
                catch
                {
                }
            }
            Session["checkessResult"] = count;
            this.Session.Remove("checkessCount");
        }

        private string UploadExcelFile()
        {
            string UploadFile = "";
            string strPath = "";
            if (this.ExcelFile.HasFile)
            {
                UploadFile = this.ExcelFile.PostedFile.FileName.ToString();
                int FileSize = Int32.Parse(this.ExcelFile.PostedFile.ContentLength.ToString());
                if (FileSize > 5 * 1024 * 1024)
                {
                    X.Msg.Alert(GetLangStr("ChecklessManager51", "提示信息"), GetLangStr("ChecklessManager67", "上传文件过大！")).Show();
                    return "";
                }
                string fileType = Path.GetExtension(this.ExcelFile.PostedFile.FileName).ToUpper();//获取文件后缀
                string allowFile = ".XLS.XLSX";
                if (allowFile.Contains(fileType.ToUpper()))
                {
                    string sNewName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(this.ExcelFile.PostedFile.FileName);
                    strPath = Server.MapPath("~/FileUpload/" + sNewName);
                    if (!Directory.Exists(Path.GetDirectoryName(strPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(strPath));
                    }
                    this.ExcelFile.PostedFile.SaveAs(strPath);

                    string strConn = "provider=Microsoft.ACE.OleDb.12.0; Data Source ='" + strPath + "';Extended Properties='Excel 12.0;HDR=yes;IMEX=1';";
                    OleDbConnection conn = new OleDbConnection(strConn);
                    conn.Open();
                    DataTable dtSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    List<string> sheetNameList = new List<string>();
                    //一个EXCEL文件可能有多个工作表，遍历之
                    foreach (DataRow dr in dtSchema.Rows)
                    {
                        //获取所有 sheet 页的名字
                        sheetNameList.Add(dr["TABLE_NAME"].ToString());
                    }
                    conn.Close();

                    int sheetIndex = -1;
                    //查找需要导入的 sheet 页的名字
                    for (int i = 0; i < sheetNameList.Count; i++)
                    {
                        if (sheetNameList[i].ToUpper().Equals("SHEET1$"))
                        {
                            sheetIndex = i;
                            break;
                        }
                    }
                    //判断该 sheet 页是否存在
                    if (sheetIndex < 0)
                    {
                        X.Msg.Alert(GetLangStr("ChecklessManager51", "提示信息"), GetLangStr("ChecklessManager68", "没有找到需要导入的SHEET1$页！")).Show();
                        return "";
                    }
                }
                else
                {
                    X.Msg.Alert(GetLangStr("ChecklessManager51", "提示信息"), GetLangStr("ChecklessManager69", "文件格式不正确！")).Show();
                    return "";
                }
                return strPath;
            }
            else
            {
                X.Msg.Alert(GetLangStr("ChecklessManager51", "提示信息"), GetLangStr("ChecklessManager70", "对不起，未找到需要导入的Excel文件，请选择要导入的文件！")).Show();
                return "";
            }
        }

        /// <summary>
        /// 转换DataTable
        /// </summary>
        /// <returns></returns>
        private DataTable ChangeDataTable()
        {
            DataTable dt = Session["datatable"] as DataTable;
            DataTable dt2 = null; ;
            if (dt != null)
            {
                PrintColumns pc = new PrintColumns();
                pc.Add(new PrintColumn(GetLangStr("ChecklessManager71", "车牌号码"), 1));
                pc.Add(new PrintColumn(GetLangStr("ChecklessManager2", "车牌类型"), 3));
                pc.Add(new PrintColumn(GetLangStr("ChecklessManager17", "车身颜色"), 4));
                pc.Add(new PrintColumn(GetLangStr("ChecklessManager18", "车辆品牌"), 5));
                pc.Add(new PrintColumn(GetLangStr("ChecklessManager19", "有效时间"), 8));
                pc.Add(new PrintColumn(GetLangStr("ChecklessManager72", "比对类型"), 7));
                pc.Add(new PrintColumn(GetLangStr("ChecklessManager73", "比对标识"), 13));
                dt2 = Bll.Common.GetDataTablePrint(dt, pc);
            }

            return dt2;
        }

        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            string where = "1=1";

            if (CmbPlateType.SelectedIndex != -1)
            {
                where = where + " and  hpzl='" + CmbPlateType.SelectedItem.Value + "' ";
            }
            if (CmbQueryMdlx.SelectedIndex != -1)
            {
                where = where + " and  mdlx='" + CmbQueryMdlx.SelectedItem.Value + "' ";
            }
            if (!string.IsNullOrEmpty(TxtplateId.Text))
            {
                where = where + " and  hphm='" + TxtplateId.Text.ToUpper() + "' ";
            }
            return where;
        }

        /// <summary>
        /// 绑定第一行数据
        /// </summary>
        /// <param name="dr"></param>
        private void SelectFirst(DataRow dr)
        {
            txtID.Text = dr["col0"].ToString();
            txtHphm.Text = dr["col1"].ToString();
            cmbHpzl.SelectedItem.Text = dr["col3"].ToString();
            cmbCsys.SelectedItem.Text = dr["col4"].ToString();
            txtClpp.Text = dr["col5"].ToString();
            cmbMdlx.SelectedItem.Text = dr["col7"].ToString();
            DateYxsj.Text = GetDate(dr["col8"].ToString(), 1);
            txtSjyy.Text = dr["col9"].ToString();
            uiDepartment.DepertName = dr["col11"].ToString();
            cmbbdbj.SelectedItem.Text = dr["col13"].ToString();
            TxtBz.Text = dr["col14"].ToString();
            TxtGxsj.Text = dr["col15"].ToString();
        }

        /// <summary>
        /// 添加布控记录
        /// </summary>
        private void AddCheckless()
        {
            txtID.Text = tgsDataInfo.GetTgsRecordId();
            txtHphm.Text = "";
            cmbHpzl.SelectedItem.Text = "";
            cmbHpzl.SelectedItem.Value = "";
            cmbCsys.SelectedItem.Text = "";
            cmbCsys.SelectedItem.Value = "";
            txtClpp.Text = "";
            cmbMdlx.SelectedItem.Text = "";
            cmbMdlx.SelectedItem.Value = "";
            DateYxsj.Text = "";
            txtSjyy.Text = "";
            uiDepartment.DepertName = "";
            cmbbdbj.SelectedItem.Text = "";
            cmbbdbj.SelectedItem.Value = "";
            TxtBz.Text = "";
        }

        /// <summary>
        /// 转换为指定的时间格式
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        private string GetDate(string data, int flag)
        {
            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    DateTime dt = DateTime.Parse(data);
                    switch (flag)
                    {
                        case 0:
                            return dt.ToString("yyyy-MM-dd HH:mm:ss");

                        case 1:
                            return dt.ToString("yyyy-MM-dd");

                        case 2:
                            return dt.ToString("HH:mm");

                        default:
                            return dt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        private string GetDate(string data, string field, int flag)
        {
            string s = GetdatabyField(data, field);
            return GetDate(s, flag);
        }

        /// <summary>
        /// 读取Grid选中行数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private string GetdatabyField(string data, string field)
        {
            string f1 = "<" + field + ">";
            string f2 = "</" + field + ">";
            int i = data.IndexOf(f1);
            int j = data.IndexOf(f2);
            if (i >= 0 && j >= 0)
            {
                return data.Substring(i + f1.Length, j - i - f2.Length + 1);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 提示窗体
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        private void Notice(string title, string msg)
        {
            Notification.Show(new NotificationConfig
            {
                Title = title,
                Icon = Icon.Information,
                HideDelay = 2000,
                Html = "<br></br>" + msg + "!"
            });
        }

        #endregion 私有方法

        #region 多语言转换

        /// <summary>
        /// 多语言转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public string GetLangStr(string value, string desc)
        {
            if (value.Equals("ChecklessManager14"))
            {
            }
            string className = this.GetType().BaseType.FullName;
            return MyNet.Common.Lang.Language.CreateInstance(className).GetLanguageStr(value, desc, className);
        }

        #endregion 多语言转换
    }
}