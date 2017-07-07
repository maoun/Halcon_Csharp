using System;
using System.Collections.Generic;
using System.Data;
using Ext.Net;
using MyNet.Atmcs.Uscmcp.Bll;

namespace MyNet.Atmcs.Uscmcp.Web
{
    public partial class TgsFlowAmply : System.Web.UI.Page
    {
        private UserLogin userLogin = new UserLogin();
        private TgsPproperty tgsPproperty = new TgsPproperty();
        private GisShow gisShow = new GisShow();
        private string title = DateTime.Now.ToString("yyyy年MM月dd日") + "24小时流量";

        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Request.QueryString["username"]; 
            if (!userLogin.CheckLogin(username))
            { 
                string js = "alert('您没有登录或操作超时，请重新登录!');window.top.location.href='" +StaticInfo.LoginPage + "'"; 
                System.Web.HttpContext.Current.Response.Write("<script type='text/javascript'>" + js + "</script>");
                return; 
            }
            if (!X.IsAjaxRequest)
            {
                Session["flowcaption"] = "流量统计";
                Session["flowxlable"] = "小时";

                this.WebChartViewer1.Visible = false;

                string kkid = Session["kkid"] as String;

                if (!string.IsNullOrEmpty(kkid))
                {
                    List<string> countkkid = GetList(kkid);
                    if (countkkid.Count > 0)
                    {
                        DataTable dt = gisShow.GetFlow(countkkid, DateTime.Now.ToString("yyyy-MM-dd"), "0");
                        this.StoreFlow.RemoveFields();
                        this.GridFlow.ColumnModel.Columns.Clear();
                        this.GridFlow.Reconfigure();
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            RecordField field = new RecordField(dt.Columns[i].ColumnName, RecordFieldType.String);
                            this.StoreFlow.AddField(field);
                            Column col = new Column();
                            col.Header = dt.Columns[i].ColumnName;
                            if (i == 0)
                            {
                                col.Width = 100;
                            }
                            else
                            {
                                if (i > 10)
                                    col.Width = 38;
                                else
                                    col.Width = 32;
                            }
                            col.Sortable = true;
                            col.DataIndex = dt.Columns[i].ColumnName;
                            GridFlow.AddColumn(col);
                        }
                        this.StoreFlow.DataSource = dt;
                        this.StoreFlow.DataBind();
                        GridFlow.Title = title;
                        Session["Flow"] = dt;
                    }
                }
            }
        }

        protected void TbutQueryClick(object sender, DirectEventArgs e)
        {
            DataTable dt = Session["Flow"] as DataTable;
            AddDataTable(dt);
        }

        private List<string> GetList(string xh)
        {
            DataTable data = tgsPproperty.GetDirectionInfoByStation2(xh);
            string list = "";
            if (data != null && data.Rows.Count > 0)
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    if (data.Rows[i][0].ToString() != "0")
                    {
                        if (i != data.Rows.Count - 1)
                        {
                            list = list + xh + "|" + data.Rows[i][0].ToString() + ",";
                        }
                        else
                        {
                            list = list + xh + "|" + data.Rows[i][0].ToString();
                        }
                    }
                }
            }
            List<string> lst = new List<string>();
            if (!string.IsNullOrEmpty(list))
            {
                string[] str = list.Split(',');
                lst.AddRange(str);
            }
            return lst;
        }

        [DirectMethod]
        private void AddDataTable(DataTable dt)
        {
            if (dt != null)
            {
                List<List<double>> datas;
                List<string> labels;
                List<string> xLabels;
                DataTable dtTemp = dt.Copy();
                dtTemp.Columns.RemoveAt(dtTemp.Columns.Count - 1);
                gisShow.GetLineChartData(dtTemp, out datas, out labels, out xLabels);
                MyNet.Atmcs.Uscmcp.Bll.Common.CreateLineChart(this.WebChartViewer1, datas, labels, xLabels, "小时", "流量", title);
                this.WebChartViewer1.Visible = true;
                pnlData.Render(this.WebChartViewer1, RenderMode.Auto);
            }
        }
    }
}