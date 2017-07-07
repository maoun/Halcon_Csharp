/***********************************************************************
 * Module:   业务逻辑层公共类
 * Author:   李建平
 * Modified: 2008年9月15日
 * Purpose:  该类为业务逻辑层提供一些公用的方法
 ***********************************************************************/

using ChartDirector;
using MyNet.Atmcs.Uscmcp.Model;
using MyNet.Common.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Xml;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    /// <summary>
    /// 通用组件
    /// </summary>
    [Serializable]
    public class Common
    {
        #region 自定义结构

        private struct structBarChart
        {
            public double[] data;
            public string name;
        }

        #endregion 自定义结构

        /// <summary>
        /// 转换列名
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable ChangColName(DataTable dt)
        {
            try
            {
                if (dt != null)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dt.Columns[i].ColumnName = "col" + i;
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="MysqlBye"></param>
        /// <param name="Path"></param>
        /// <param name="urlNet"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string SaveImage(byte[] MysqlBye, string Path, string urlNet, string fileName)
        {
            if (MysqlBye.Length > 0)
            {
                try
                {
                    MemoryStream myFilestream = new MemoryStream(MysqlBye);
                    Bitmap myBmp = new Bitmap(myFilestream);
                    if (!Directory.Exists(Path))
                    {
                        Directory.CreateDirectory(Path + urlNet);
                    }
                    myBmp.Save(Path + urlNet + "\\" + fileName);
                    return "." + urlNet.Replace("\\", "/") + "/" + fileName;
                }
                catch (Exception ex)
                {
                    ILog.WriteErrorLog(ex);
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<object> ChangData(DataTable dt)
        {
            List<object> data = new List<object>();
            try
            {
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        data.Add(dt.Rows[i].ItemArray);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 转换日期格式
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string GetDate(string data, int flag)
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
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="field"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string GetDate(string data, string field, int flag)
        {
            string s = GetdatabyField(data, field);
            return GetDate(s, flag);
        }

        /// <summary>
        /// 获得字段
        /// </summary>
        /// <param name="data"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string GetdatabyField(string data, string field)
        {
            string f1 = "<" + field + ">";
            string f2 = "</" + field + ">";
            int i = data.IndexOf(f1);
            int j = data.IndexOf(f2);
            if (i >= 0 && j >= 0)
            {
                ////判断时间列
                //if (field=="col6")
                //{
                //    string timeQian = data.Substring(i + f1.Length, 10);
                //    string timeHou = data.Substring(i + f1.Length + 11, 8);
                //    return timeQian + " " + timeHou;
                //}
                //原来的
                return data.Substring(i + f1.Length, j - i - f2.Length + 1);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获得字段
        /// </summary>
        /// <param name="data"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string GetdatabyField(string data, string field, string meiyong)
        {
            string f1 = "<" + field + ">";
            string f2 = "</" + field + ">";
            int i = data.IndexOf(f1);
            int j = data.IndexOf(f2);
            if (i >= 0 && j >= 0)
            {
                //判断时间列
                if (field == "col6")
                {
                    string timeQian = data.Substring(i + f1.Length, 10);
                    string timeHou = data.Substring(i + f1.Length + 11, 8);
                    return timeQian + " " + timeHou;
                }
                else if (field == "col4")
                {
                    string timeQian = data.Substring(i + f1.Length, 10);
                    string timeHou = data.Substring(i + f1.Length + 11, 8);
                    return timeQian + " " + timeHou;
                }
                else if (field == "col19")
                {
                    string timeQian = data.Substring(i + f1.Length, 10);
                    string timeHou = data.Substring(i + f1.Length + 11, 8);
                    return timeQian + " " + timeHou;
                }
                else if (field == "col20")
                {
                    string timeQian = data.Substring(i + f1.Length, 10);
                    string timeHou = data.Substring(i + f1.Length + 11, 8);
                    return timeQian + " " + timeHou;
                }
                else if (field == "col11")
                {
                    string timeQian = data.Substring(i + f1.Length, 10);
                    string timeHou = data.Substring(i + f1.Length + 11, 8);
                    return timeQian + " " + timeHou;
                }
                return "";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static string GetXmlString(DataRow dr)
        {
            string xmlString = string.Empty;
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                xmlString = xmlString + "<" + dr.Table.Columns[i].ColumnName + ">" + dr[i].ToString() + "</" + dr.Table.Columns[i].ColumnName + ">";
            }
            return xmlString;
        }

        /// <summary>
        /// 将原来的DataTable转化成新的DataTable
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="lstPrintCol">转化的列集合</param>
        /// <returns></returns>
        public static DataTable GetDataTablePrint(DataTable dt, PrintColumns lstPrintCol)
        {
            DataTable dt2 = new DataTable("MyDataTable1");
            try
            {
                DataRow myDataRow;
                DataColumn myDataColumn;
                for (int i = 0; i < lstPrintCol.Count; i++)
                {
                    myDataColumn = new DataColumn();
                    myDataColumn.DataType = Type.GetType("System.String");
                    myDataColumn.ColumnName = lstPrintCol[i].ColumnName;
                    dt2.Columns.Add(myDataColumn);
                }
                for (int i = 0; i < dt.Rows.Count; i++) //赋值
                {
                    myDataRow = dt2.NewRow();
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        myDataRow[j] = dt.Rows[i][lstPrintCol[j].ColumnId];
                    }

                    dt2.Rows.Add(myDataRow);
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
            return dt2;
        }

        /*
        /// <summary>
        /// 获得gridview中的datasource
        /// </summary>
        /// <param name="gridView"></param>
        /// <returns></returns>
        public static DataTable GetDataSource(GridView gridView)
        {
            try
            {
                DataTable dt = new DataTable("MyDataTable1");
                DataRow myDataRow;
                DataColumn myDataColumn;
                for (int i = 0; i < gridView.Columns.Count; i++) //组合列
                {
                    if (gridView.Columns[i].Visible)
                    {
                        myDataColumn = new DataColumn();
                        myDataColumn.DataType = Type.GetType("System.String");
                        myDataColumn.ColumnName = gridView.Columns[i].HeaderText;
                        dt.Columns.Add(myDataColumn);
                    }
                }
                for (int i = 0; i < gridView.Rows.Count; i++) //赋值
                {
                    myDataRow = dt.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        myDataRow[dt.Columns[j].ColumnName] = HttpUtility.HtmlDecode(gridView.Rows[i].Cells[j].Text);
                    }

                    dt.Rows.Add(myDataRow);
                }

                if (gridView.FooterRow.Visible) //页眉页脚
                {
                    myDataRow = dt.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        myDataRow[dt.Columns[j].ColumnName] = HttpUtility.HtmlDecode(gridView.FooterRow.Cells[j].Text);
                    }
                    dt.Rows.Add(myDataRow);
                }
                return dt;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return null;
            }
        }
        */

        #region 报表

        ///<summary>
        ///赋值线状图
        ///</summary>
        ///<param name="viewer">饼图实例</param>
        ///<param name="data">二维数据</param>
        ///<param name="lables">标题数组</param>
        ///<param name="xTitle">x标题</param>
        ///<param name="yTitle">y标题</param>
        ///<param name="title">图标题</param>
        public static void CreateLineChart(WebChartViewer viewer, List<List<double>> datas, List<string> labels, List<string> xLabels, string xTitle, string yTitle, string title)
        {
            if (datas == null || labels == null)
            {
                return;
            }
            int mheight = 0;
            if (datas.Count > 12)
            {
                mheight = ((datas.Count - 10) / 3) * 12;
            }
            XYChart c = new XYChart(800, 500 + mheight, 0xeeeeff, 0x0, 2);
            c.setRoundedFrame();
            c.setPlotArea(55, 58, 720, 300, 0xFFFFFF, -1, -1, 0xCCCCCC, 0xCCCCCC);
            c.addLegend(50, 400, false, "宋体", 9).setBackground(Chart.Transparent);
            c.addTitle(title, "Times New Roman", 12, 0xffffff).setBackground(0x0000cc, 0x000000, Chart.glassEffect(Chart.ReducedGlare));
            c.yAxis().setTitle(yTitle);
            c.xAxis().setLabelStep(1);
            c.xAxis().setTitle(xTitle);
            LineLayer layer = c.addLineLayer2();
            layer.setLineWidth(2);

            if (xLabels != null)
            {
                string[] xLabel = new string[xLabels.Count];
                xLabels.CopyTo(xLabel);

                c.xAxis().setLabels(xLabel);
            }

            if (datas.Count > 0 && labels.Count > 0)
            {
                string[] label = new string[labels.Count]; //线的含义（代表的数据的名称）例如：方向名称
                labels.CopyTo(label);
                double[] myData;
                List<double> myDataList; //数据
                for (int i = 0; i < datas.Count; i++)
                {
                    myDataList = datas[i];
                    myData = new double[myDataList.Count];
                    myDataList.CopyTo(myData);
                    layer.addDataSet(myData, Get_Color(i), label[i]).setDataSymbol(Chart.CircleSymbol, 6);
                }
            }
            viewer.Image = c.makeWebImage(Chart.PNG);
            viewer.ImageMap = c.getHTMLImageMap("", "", "title='{dataSetName}  - " + yTitle + "({value})'");
        }

        ///<summary>
        ///赋值线状图
        ///</summary>
        ///<param name="viewer">饼图实例</param>
        ///<param name="data">二维数据</param>
        ///<param name="lables">标题数组</param>
        ///<param name="xTitle">x标题</param>
        ///<param name="yTitle">y标题</param>
        ///<param name="title">图标题</param>
        public static void CreateAmplylineChart(WebChartViewer viewer, List<List<double>> datas, List<string> labels, List<string> xLabels, string xTitle, string yTitle, string title)
        {
            if (datas == null || labels == null)
            {
                return;
            }
            int mheight = 0;
            if (datas.Count > 12)
            {
                mheight = ((datas.Count - 10) / 3) * 12;
            }
            XYChart c = new XYChart(800, 500 + mheight, 0xeeeeff, 0x0, 2);
            c.setRoundedFrame();
            c.setPlotArea(55, 58, 720, 300, 0xFFFFFF, -1, -1, 0xCCCCCC, 0xCCCCCC);
            LegendBox legendBox = c.addLegend(50, 400, false, "宋体", 9);
            legendBox.setBackground(Chart.Transparent);
            ChartDirector.TextBox tb = c.addTitle(title, "Times New Roman", 12, 0xffffff);
            tb.setBackground(0x0000cc, 0x000000, Chart.glassEffect(Chart.ReducedGlare));
            c.yAxis().setTitle(yTitle);
            c.xAxis().setLabelStep(1);
            c.xAxis().setTitle(xTitle);
            LineLayer layer = c.addLineLayer2();
            layer.setLineWidth(2);

            if (xLabels != null)
            {
                string[] xLabel = new string[xLabels.Count];
                xLabels.CopyTo(xLabel);

                c.xAxis().setLabels(xLabel);
            }

            if (datas.Count > 0 && labels.Count > 0)
            {
                string[] label = new string[labels.Count]; //线的含义（代表的数据的名称）例如：方向名称
                labels.CopyTo(label);
                double[] myData;
                List<double> myDataList; //数据
                for (int i = 0; i < datas.Count; i++)
                {
                    myDataList = datas[i];
                    myData = new double[myDataList.Count];
                    myDataList.CopyTo(myData);
                    layer.addDataSet(myData, Get_Color(i), label[i]).setDataSymbol(Chart.CircleSymbol, 6);
                }
            }

            viewer.Image = c.makeWebImage(Chart.PNG);
            string chartImageMap = c.getHTMLImageMap("", "", "title='{dataSetName}  - " + yTitle + "({value})'");
            string legendImageMap = legendBox.getHTMLImageMap("javascript:popMsg('{dataSetName},{dataSet}');", " ", "title='详细信息'");
            viewer.ImageMap = chartImageMap + legendImageMap;
        }

        public static void CreateCurvelineChart(WebChartViewer viewer, double[] data, string xTitle, string yTitle, string title)
        {
            if (data.Length > 0)
            {
                XYChart c = new XYChart(800, 480);
                c.setPlotArea(50, 35, 700, 400).setGridColor(0xc0c0c0, 0xc0c0c0);
                c.addTitle(title, "Times New Roman Bold", 12);
                c.yAxis().setWidth(3);
                c.xAxis().setTitle(xTitle, "Arial Bold Italic", 12);
                c.xAxis().setWidth(3);
                c.yAxis().setTitle(yTitle);
                c.xAxis().setTitle(xTitle);
                c.xAxis().setLinearScale(0, data.Length - 1, 1, 1);
                LineLayer layer = c.addLineLayer2();
                layer.addDataSet(data, unchecked((int)0x80ff0000)).setDataSymbol(Chart.SquareSymbol);
                layer.setLineWidth(2);
                c.addSplineLayer(new ArrayMath(data).lowess().result(), 0x0000ff).setLineWidth(3);
                c.yAxis().setAutoScale(0, 0, 0);
                viewer.Image = c.makeWebImage(Chart.PNG);
                viewer.ImageMap = c.getHTMLImageMap("", "", "title='({x}时, {value|2})'");
            }
        }

        /// <summary>
        /// 赋值饼图
        /// </summary>
        /// <param name="viewer">饼图实例</param>
        /// <param name="data">饼图数据</param>
        /// <param name="title">饼图标题</param>
        public static void CreatePicChart(WebChartViewer viewer, Dictionary<string, double> data, string title)
        {
            double[] myData = new double[data.Count];
            string[] myLabel = new string[data.Count];
            data.Keys.CopyTo(myLabel, 0);
            data.Values.CopyTo(myData, 0);
            PieChart chart = new PieChart(750, 400, 0xccccff, -1, 1);
            chart.setPieSize(350, 200, 100);
            chart.setRoundedFrame();
            chart.addTitle(title, "黑体", 12, 0xffffff).setBackground(0x0000cc, 0x000000, Chart.glassEffect(Chart.ReducedGlare));
            chart.set3D(10);
            chart.setLabelLayout(Chart.SideLayout);
            ChartDirector.TextBox t = chart.setLabelStyle();
            t.setBackground(Chart.SameAsMainColor, Chart.Transparent, Chart.glassEffect());
            t.setRoundedCorners(5);
            chart.setLineColor(Chart.SameAsMainColor, 0x000000);
            chart.setColors(Chart.transparentPalette);
            chart.setBackground(Chart.metalColor(0xccccff), 0x000000, 1);
            chart.setStartAngle(90);

            chart.setLabelFormat("{label} : {value}\n({percent}%)");
            chart.setData(myData, myLabel);
            chart.setExplode(5);
            viewer.Image = chart.makeWebImage(Chart.PNG);
            viewer.ImageMap = chart.getHTMLImageMap("", "", "title='{label}: {value}　所占比列：({percent}%)'");
        }

        /// <summary>
        /// 赋值饼图
        /// </summary>
        /// <param name="viewer">饼图实例</param>
        /// <param name="data">饼图数据</param>
        /// <param name="title">饼图标题</param>
        public static void CreatePicChart2(WebChartViewer viewer, Dictionary<string, double> data, string title)
        {
            double[] myData = new double[data.Count];
            string[] myLabel = new string[data.Count];
            data.Keys.CopyTo(myLabel, 0);
            data.Values.CopyTo(myData, 0);
            PieChart chart = new PieChart(600, 320);
            chart.setPieSize(250, 160, 100);
            chart.setRoundedFrame();
            chart.addTitle(title, "微软雅黑", 12, 0x000080);
            chart.set3D(10, 15);
            chart.setLabelLayout(Chart.SideLayout);
            chart.setColors(Chart.transparentPalette);
            chart.setStartAngle(90);
            chart.setLabelFormat("<*block,halign=center*><*font=宋体,size=9,underline=1*>{label}<*/font*><*br*>{value}({percent}%)");
            chart.setData(myData, myLabel);
            chart.setExplode(5);
            viewer.Image = chart.makeWebImage(Chart.PNG);
            viewer.ImageMap = chart.getHTMLImageMap("", "", "title='{label}: {value}　所占比列：({percent}%)'");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="viewer"></param>
        /// <param name="dt"></param>
        /// <param name="title"></param>
        /// <param name="xtitle"></param>
        public static void CreateStackedBarChart(WebChartViewer viewer, DataTable dt, string title, string xtitle)
        {
            ArrayList arrlist = new ArrayList();
            int max = 0;
            for (int j = 1; j < dt.Columns.Count; j++)
            {
                double[] data = new double[dt.Rows.Count];
                structBarChart sBarChart = new structBarChart();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    data[i] = double.Parse(dt.Rows[i][j].ToString());
                }
                sBarChart.name = dt.Columns[j].ColumnName;
                if (sBarChart.name.Length > max)
                {
                    max = sBarChart.name.Length;
                }
                sBarChart.data = data;
                arrlist.Add(sBarChart);
            }
            string[] labels = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                labels[i] = dt.Rows[i][0].ToString();
            }
            XYChart c = new XYChart(800, 400);
            c.setPlotArea(70, 60, 660 - (max * 12), 320, 0xf8f8f8, 0xffffff);

            c.addLegend(755 - (max * 12), 40).setFontSize(8);
            c.setRoundedFrame();
            c.addTitle(title, "Times New Roman", 12, 0xffffff).setBackground(0x0000cc, 0x000000, Chart.glassEffect(Chart.ReducedGlare));
            c.yAxis().setTitle(xtitle).setFontAngle(90);
            c.xAxis().setLabels(labels).setFontSize(8);
            BarLayer layer = c.addBarLayer2(Chart.Stack, 8);

            for (int n = 0; n < arrlist.Count; n++)
            {
                structBarChart sbc = (structBarChart)arrlist[n];
                layer.addDataSet(sbc.data, Get_Color(n), sbc.name);
                if (sbc.data.Length < 8)
                {
                    layer.setBarWidth(50);
                }
            }
            layer.set3D(20);
            layer.setBarShape(Chart.CircleShape);
            layer.setAggregateLabelStyle();
            layer.setDataLabelStyle();
            viewer.Image = c.makeWebImage(Chart.PNG);
            viewer.ImageMap = c.getHTMLImageMap("", "", "title='{xLabel}-->{dataSetName}({value})'");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="viewer"></param>
        /// <param name="data"></param>
        /// <param name="title"></param>
        /// <param name="xtitle"></param>
        /// <param name="ytitle"></param>
        public static void CreateBarChart(WebChartViewer viewer, Dictionary<double, string> data, string title, string xtitle, string ytitle)
        {
            double[] myData = new double[data.Count];
            string[] myLabel = new string[data.Count];
            data.Keys.CopyTo(myData, 0);
            data.Values.CopyTo(myLabel, 0);
            XYChart c = new XYChart(400, 240);
            c.addTitle(title, "Times New Roman Bold Italic", 14);
            c.setPlotArea(45, 40, 300, 160, 0xf8f8f8, 0xffffff);
            BarLayer layer = c.addBarLayer3(myData);
            layer.set3D(10);
            layer.setBarShape(Chart.CircleShape);
            c.xAxis().setLabels(myLabel);
            c.yAxis().setTitle(xtitle);
            c.xAxis().setTitle(ytitle);
            viewer.Image = c.makeWebImage(Chart.PNG);
            viewer.ImageMap = c.getHTMLImageMap("", "", "title='{xLabel}: {value} MBytes'");
        }

        /// <summary>
        /// 根据0~11的索引返回一个颜色
        /// </summary>
        /// <param name="i">0~11的索引</param>
        /// <returns></returns>
        private static int Get_Color(int i)
        {
            int[] Mycolor = new int[12];

            Mycolor[0] = System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.Red);
            Mycolor[1] = System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.Blue);
            Mycolor[2] = System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.Green);
            Mycolor[3] = System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.Yellow);
            Mycolor[4] = System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.Orange);
            Mycolor[5] = System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.SpringGreen);
            Mycolor[6] = System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.Pink);
            Mycolor[7] = System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.Brown);
            Mycolor[8] = System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.Purple);
            Mycolor[9] = System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.LightBlue);
            Mycolor[10] = System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.Thistle);
            Mycolor[11] = System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.Violet);
            if (i > 11)
            {
                return System.Drawing.ColorTranslator.ToWin32(GetRandomColor());
            }

            return Mycolor[i];
        }

        /// <summary>
        /// 返回随机颜色
        /// </summary>
        /// <returns></returns>
        public static Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            //  对于C#的随机数，没什么好说的
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);

            //  为了在白色背景上显示，尽量生成深色
            int int_Red = RandomNum_First.Next(256);
            int int_Green = RandomNum_Sencond.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;

            return Color.FromArgb(int_Red, int_Green, int_Blue);
        }

        /// <summary>
        /// 转换打印XML
        /// </summary>
        /// <param name="title"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="datatableName"></param>
        /// <returns></returns>
        public static string GetPrintXml(string title, string starttime, string endtime, string datatableName)
        {
            string xml = title + "|" + starttime + "|" + endtime + "|" + datatableName;

            return xml;
        }

        /// <summary>
        /// 将IPv4格式的字符串转换为int型表示
        /// </summary>
        /// <param name="strIPAddress">IPv4格式的字符</param>
        /// <returns></returns>
        public static int IPToNumber(string strIPAddress)
        {
            //将目标IP地址字符串strIPAddress转换为数字
            string[] arrayIP = strIPAddress.Split('.');
            int sip1 = Int32.Parse(arrayIP[0]);
            int sip2 = Int32.Parse(arrayIP[1]);
            int sip3 = Int32.Parse(arrayIP[2]);
            int sip4 = Int32.Parse(arrayIP[3]);
            int tmpIpNumber;
            tmpIpNumber = sip1 * 256 * 256 * 256 + sip2 * 256 * 256 + sip3 * 256 + sip4;
            return tmpIpNumber;
        }

        /// <summary>
        /// 将int型表示的IP还原成正常IPv4格式。
        /// </summary>///
        /// <param name="intIPAddress">int型表示的IP</param>
        /// <returns></returns>
        public static string NumberToIP(int intIPAddress)
        {
            int tempIPAddress;
            //将目标整形数字intIPAddress转换为IP地址字符串
            //-1062731518 192.168.1.2
            //-1062731517 192.168.1.3
            if (intIPAddress >= 0)
            {
                tempIPAddress = intIPAddress;
            }
            else
            {
                tempIPAddress = intIPAddress + 1;
            }
            int s1 = tempIPAddress / 256 / 256 / 256;
            int s21 = s1 * 256 * 256 * 256;
            int s2 = (tempIPAddress - s21) / 256 / 256;
            int s31 = s2 * 256 * 256 + s21;
            int s3 = (tempIPAddress - s31) / 256;
            int s4 = tempIPAddress - s3 * 256 - s31;
            if (intIPAddress < 0)
            {
                s1 = 255 + s1;
                s2 = 255 + s2;
                s3 = 255 + s3;
                s4 = 255 + s4;
            }
            string strIPAddress = s1.ToString() + "." + s2.ToString() + "." + s3.ToString() + "." + s4.ToString();
            return strIPAddress;
        }

        #region Cryptography 类中用

        /// <summary>
        /// Converts an array of sbytes to an array of bytes
        /// </summary>
        /// <param name="sbyteArray">The array of sbytes to be converted</param>
        /// <returns>The new array of bytes</returns>
        public static byte[] ToByteArray(sbyte[] sbyteArray)
        {
            byte[] byteArray = new byte[sbyteArray.Length];
            for (int index = 0; index < sbyteArray.Length; index++)
                byteArray[index] = (byte)sbyteArray[index];
            return byteArray;
        }

        /// <summary>
        /// Converts a string to an array of bytes
        /// </summary>
        /// <param name="sourceString">The string to be converted</param>
        /// <returns>The new array of bytes</returns>
        public static byte[] ToByteArray(string sourceString)
        {
            byte[] byteArray = new byte[sourceString.Length];
            for (int index = 0; index < sourceString.Length; index++)
                byteArray[index] = (byte)sourceString[index];
            return byteArray;
        }

        /// <summary>
        /// Converts an array of bytes to an array of sbyte
        /// </summary>
        /// <param name="byteArray">The array of bytes to be converted</param>
        /// <returns>The new array of sbytes</returns>
        public static sbyte[] ToSByteArray(byte[] byteArray)
        {
            sbyte[] sbyteArray = new sbyte[byteArray.Length];
            for (int index = 0; index < byteArray.Length; index++)
                sbyteArray[index] = (sbyte)byteArray[index];
            return sbyteArray;
        }

        static public System.Random Random = new System.Random();

        #endregion Cryptography 类中用

        /*

        #region 水晶报表用

        /// <summary>
        /// 绑定水晶报表
        /// </summary>
        /// <param name="rptPath">报表文件全路径</param>
        /// <param name="dtData">报表数据</param>
        /// <param name="paraCount">报表文件参数数量</param>
        /// <param name="cryviewer">报表显示控件</param>
        /// <param name="isUserDefined">是否是自定义列</param>
        public static void BindCryReport(string rptPath, DataTable dtData,int paraCount, CrystalReportViewer cryviewer,bool isUserDefined )
        {
            try
            {
                ReportDocument oRp = new ReportDocument();

                oRp.Load(rptPath);
                //TextObject title = (TextObject)oRp.ReportDefinition.ReportObjects["title"];
                //title.Text = "";
                if (isUserDefined)
                {
                    if (dtData.Columns.Count >= paraCount) //是否大于8列
                    {
                        for (int i = 1; i <= paraCount; i++)
                        {
                            ((TextObject)oRp.ReportDefinition.ReportObjects["Col" + i]).Text = dtData.Columns[i - 1].ColumnName;
                            oRp.DataDefinition.FormulaFields["Column" + i].Text = "{" + dtData.TableName + "." + dtData.Columns[i - 1].ColumnName + "}";
                        }
                    }
                    else//小于8列
                    {
                        for (int i = 1; i <= dtData.Columns.Count; i++)
                        {
                            ((TextObject)oRp.ReportDefinition.ReportObjects["Col" + i]).Text = dtData.Columns[i - 1].ColumnName;
                            oRp.DataDefinition.FormulaFields["Column" + i].Text = "{" + dtData.TableName + "." + dtData.Columns[i - 1].ColumnName + "}";
                        }
                        for (int i = dtData.Columns.Count + 1; i <= paraCount; i++)
                        {
                            ((TextObject)oRp.ReportDefinition.ReportObjects["Col" + i]).Text = "";
                            oRp.DataDefinition.FormulaFields["Column" + i].Text = "";
                        }
                    }
                }
                oRp.SetDataSource(dtData);
                cryviewer.ReportSource = oRp;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        public static void BindCryReport(string rptPath, DataTable dtData, int paraCount, CrystalReportViewer cryviewer, bool isUserDefined ,string strtitle)
        {
            try
            {
                ReportDocument oRp = new ReportDocument();

                oRp.Load(rptPath);
                oRp.SummaryInfo.ReportTitle = strtitle;
                if (isUserDefined)
                {
                    if (dtData.Columns.Count >= paraCount) //是否大于8列
                    {
                        for (int i = 1; i <= paraCount; i++)
                        {
                            ((TextObject)oRp.ReportDefinition.ReportObjects["Col" + i]).Text = dtData.Columns[i - 1].ColumnName;
                            oRp.DataDefinition.FormulaFields["Column" + i].Text = "{" + dtData.TableName + "." + dtData.Columns[i - 1].ColumnName + "}";
                        }
                    }
                    else//小于8列
                    {
                        for (int i = 1; i <= dtData.Columns.Count; i++)
                        {
                            ((TextObject)oRp.ReportDefinition.ReportObjects["Col" + i]).Text = dtData.Columns[i - 1].ColumnName;
                            oRp.DataDefinition.FormulaFields["Column" + i].Text = "{" + dtData.TableName + "." + dtData.Columns[i - 1].ColumnName + "}";
                        }
                        for (int i = dtData.Columns.Count + 1; i <= paraCount; i++)
                        {
                            ((TextObject)oRp.ReportDefinition.ReportObjects["Col" + i]).Text = "";
                            oRp.DataDefinition.FormulaFields["Column" + i].Text = "";
                        }
                    }
                }
                oRp.SetDataSource(dtData);
                cryviewer.ReportSource = oRp;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
        }

        #endregion 水晶报表用

         */

        #endregion 报表

        #region 数据转换

        public static string Changenull(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Equals("null"))
            {
                return "";
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// 转换号牌种类描述
        /// </summary>
        /// <param name="hpzl"></param>
        /// <returns></returns>
        public static string GetHpzl(string hpzl)
        {
            if (!string.IsNullOrEmpty(hpzl) && (hpzl.Length > 1))
            {
                switch (hpzl.Substring(0, 2))
                {
                    case "大型":
                        return "01";

                    case "小型":
                        return "02";

                    case "使馆":
                        return "03";

                    case "领馆":
                        return "04";

                    case "境外":
                        return "05";

                    case "外籍":
                        return "06";

                    case "公安":
                        return "23";
                }
                return "02";
            }
            return "02";
        }

        /// <summary>
        /// 转换车身颜色
        /// </summary>
        /// <param name="csys"></param>
        /// <returns></returns>
        public static string GetCsys(string csys)
        {
            if (!string.IsNullOrEmpty(csys) && (csys.Length > 1))
            {
                switch (csys.Substring(0, 1))
                {
                    case "白":
                        return "A";

                    case "灰":
                        return "B";

                    case "黄":
                        return "C";

                    case "粉":
                        return "D";

                    case "红":
                        return "E";

                    case "紫":
                        return "F";

                    case "绿":
                        return "G";

                    case "蓝":
                        return "H";

                    case "棕":
                        return "I";

                    case "黑":
                        return "J";
                }
                return "J";
            }
            return "J";
        }

        /// <summary>
        /// 转换车身颜色
        /// </summary>
        /// <param name="csys"></param>
        /// <returns></returns>
        public static string GetCsysms(string csys)
        {
            if (!string.IsNullOrEmpty(csys) && (csys.Length == 1))
            {
                switch (csys.Substring(0, 1))
                {
                    case "A":
                        return "白";

                    case "B":
                        return "灰";

                    case "C":
                        return "黄";

                    case "D":
                        return "粉";

                    case "E":
                        return "红";

                    case "F":
                        return "紫";

                    case "G":
                        return "绿";

                    case "H":
                        return "蓝";

                    case "I":
                        return "棕";

                    case "棕":
                        return "棕";
                }
                return "白";
            }
            return "";
        }

        /// <summary>
        /// 转换名单类型
        /// </summary>
        /// <param name="mdlx"></param>
        /// <returns></returns>
        public static string GetMdlx(string mdlx)
        {
            if (!string.IsNullOrEmpty(mdlx) && (mdlx.Length > 1))
            {
                switch (mdlx)
                {
                    case "违法未处理":
                        return "A";

                    case "盗抢":
                        return "B";

                    case "刑事治安":
                        return "C";

                    case "特殊车辆":
                        return "D";

                    case "未年审":
                        return "E";

                    case "套牌":
                        return "F";

                    case "假牌":
                        return "G";
                }
                return "A";
            }
            return "A";
        }

        /// <summary>
        /// 转换号牌种类描述
        /// </summary>
        /// <param name="hpzl"></param>
        /// <returns></returns>
        public static string GetHpzlms(string hpzl)
        {
            if (!string.IsNullOrEmpty(hpzl) && (hpzl.Length > 1))
            {
                switch (hpzl)
                {
                    case "01":
                        return "大型汽车";

                    case "02":
                        return "小型汽车";

                    case "06":
                        return "外籍汽车";

                    case "23":
                        return "公安警用汽车";

                    case "99":
                        return "其它号牌";
                }
                return "小型汽车";
            }
            return "小型汽车";
        }

        /// <summary>
        /// 转换方向描述
        /// </summary>
        /// <param name="fxbh"></param>
        /// <returns></returns>
        public static string GetFxms(string fxbh)
        {
            if (fxbh.Length == 1)
            {
                fxbh = "0" + fxbh;
            }
            if (!string.IsNullOrEmpty(fxbh) && (fxbh.Length >= 1))
            {
                switch (fxbh)
                {
                    case "01":
                        return "由东向西";

                    case "02":
                        return "由西向东";

                    case "03":
                        return "由南向北";

                    case "04":
                        return "由北向南";
                }
                return "由东向西";
            }
            return "由东向西";
        }

        /// <summary>
        /// 转换数据来源
        /// </summary>
        /// <param name="sjly"></param>
        /// <returns></returns>
        public static string GetSjlyms(string sjly)
        {
            if (!string.IsNullOrEmpty(sjly))
            {
                switch (sjly)
                {
                    case "1":
                        return "电子警察设备";

                    case "2":
                        return "公路卡口设备";

                    case "3":
                        return "公路测速设备";

                    case "4":
                        return "闭路电视";

                    case "5":
                        return "移动摄像";

                    case "6":
                        return "行车记录仪";

                    default:
                        return "其它设备";
                }
            }
            return "公路卡口设备";
        }

        /// <summary>
        /// 转换数据来源
        /// </summary>
        /// <param name="jllx"></param>
        /// <returns></returns>
        public static string GetJllxms(string jllx)
        {
            if (!string.IsNullOrEmpty(jllx))
            {
                switch (jllx)
                {
                    case "0":
                        return "正常车辆";

                    case "1":
                        return "黑名单车辆";

                    case "2":
                        return "特殊勤务车辆";

                    case "3":
                        return "布控车辆";

                    case "4":
                        return "占道车辆";

                    case "5":
                        return "超速违法";

                    case "6":
                        return "畅行车辆";

                    case "7":
                        return "区间超速";

                    case "8":
                        return "黄牌车辆禁行";

                    case "9":
                        return "无牌车辆";

                    case "10":
                        return "闭路电视";

                    case "11":
                        return "闯红灯";

                    case "12":
                        return "逆行违法";

                    case "13":
                        return "违法压线";

                    case "14":
                        return "违法停车";

                    case "15":
                        return "占用应急车道";

                    case "16":
                        return "低速违法";

                    case "17":
                        return "越线停车";

                    case "18":
                        return "不按车道行驶";

                    case "19":
                        return "违章左转";

                    case "20":
                        return "违章右转";

                    case "21":
                        return "违章掉头";

                    case "22":
                        return "违章变道";

                    case "23":
                        return "压黄线";

                    case "24":
                        return "压白线";

                    case "25":
                        return "路肩行驶";

                    case "26":
                        return "过车转违法";

                    default:
                        return "其它设备";
                }
            }
            return "公路卡口设备";
        }

        /// <summary>
        /// 转换TemplateCode
        /// </summary>
        /// <param name="Template"></param>
        /// <returns></returns>
        public static string GetTemplateCode(string Template)
        {
            if (!string.IsNullOrEmpty(Template))
            {
                switch (Template)
                {
                    case "DataTemplate":
                        return "010101";

                    case "VideoTemplate":
                        return "010102";

                    case "ListTemplate":
                        return "010103";

                    case "UserTemplate":
                        return "010104";

                    default:
                        return "010101";
                }
            }
            return "010101";
        }

        /// <summary>
        /// 转换TemplateSring
        /// </summary>
        /// <param name="Template"></param>
        /// <returns></returns>
        public static string GetTemplateMs(string TemplateCode)
        {
            if (!string.IsNullOrEmpty(TemplateCode))
            {
                switch (TemplateCode)
                {
                    case "010101":
                        return "DataTemplate";

                    case "010102":
                        return "VideoTemplate";

                    case "010103":
                        return "ListTemplate";

                    case "010104":
                        return "UserTemplate";

                    default:
                        return "DataTemplate";
                }
            }
            return "DataTemplate";
        }

        #endregion 数据转换

        #region 大数据查询

        /// <summary>
        /// 组装交互日志查询XML
        /// </summary>
        /// <param name="con"></param>
        /// <param name="startnum"></param>
        /// <param name="querycount"></param>
        /// <returns></returns>
        public static string GetBusinessXml(string startTime, string endTime, string ip, string usercode, string funcid, string startNum, string pageSize)
        {
            //开始时间后面加000  结束时间后面加999 这是他那边的定义
            string xml = "<?xml version='1.0' encoding='UTF-8'?>" +
                            "<Message>" +
                            "<Version>1.0</Version>" +
                           " <Type>REQUEST</Type> " +
                           "  <Body> " +
                           "  <Cmd>" +
                           "  <DYZIP>" + ip + "</DYZIP>" +
                           "  <YHBH>" + usercode + "</YHBH> " +
                           "  <GNMKBH>" + funcid + "</GNMKBH> " +
                           "  <FWBH>3</FWBH> " +
                           "  <CXKSSJ>" + startTime + " 000</CXKSSJ> " +
                           "  <CXJSSJ>" + endTime + " 999</CXJSSJ>" +
                           "  <PAGENUMBER>" + startNum + "</PAGENUMBER>" +
                           "  <PAGESIZE>" + pageSize + "</PAGESIZE> " +
                           "  </Cmd>" +
                           " </Body> " +
                           "</Message>";
            return xml;
        }

        /// <summary>
        /// 组装过车查询XML
        /// </summary>
        /// <param name="con"></param>
        /// <param name="startnum"></param>
        /// <param name="querycount"></param>
        /// <returns></returns>
        public static string GetPassCarXml(Condition con, string startnum, string querycount)
        {
            string speed = "";//存速度区间的
            string cheChang = "";//存车长区间的
            if (!string.IsNullOrEmpty(con.Dsd))
            {
                speed = con.Dsd;
            }
            if (!string.IsNullOrEmpty(con.Gsd))
            {
                speed = con.Gsd;
            }
            if (!string.IsNullOrEmpty(con.Dsd) && !string.IsNullOrEmpty(con.Gsd))
            {
                speed = con.Dsd + "," + con.Gsd;
            }
            if (!string.IsNullOrEmpty(con.Dcc) && !string.IsNullOrEmpty(con.Ccc))
            {
                cheChang = con.Dcc + "," + con.Ccc;
            }
            if (con.Hphm == "无牌")
            {
                con.Sqjc = "";
            }
            try
            {
                string xml = "<?xml version='1.0' encoding='UTF-8'?>" +
                    "<Message>" +
                             "<Version>1.0</Version>" +
                             "<Type>REQUEST</Type>" +
                             "<Body>" +
                                 "<Cmd>" +
                                    "<kkid>" + con.Kkid + "</kkid>" +
                                    "<fxbh>" + con.Xsfx + "</fxbh>" +
                                    "<cdbh>" + con.Xscd + "</cdbh>" +
                                    "<hphm>" + con.Sqjc + con.Hphm + "</hphm>" +
                                    "<hpzl>" + con.Hpzl + "</hpzl>" +
                                    "<kssj>" + con.StartTime + "</kssj>" +
                                    "<jssj>" + con.EndTime + "</jssj>" +
                                    "<clpp>" + con.Clpp + "</clpp>" +
                                    "<clsd>" + speed + "</clsd>" +
                                    "<csys>" + con.Csys + "</csys>" +
                                    "<jllx></jllx>" +
                                    "<zjhsl>" + (con.Zjh ? "1" : "") + "</zjhsl>" +
                                    "<zybsl>" + (con.Zyb ? "1" : "") + "</zybsl>" +
                                    "<dzsl>" + (con.Dz ? "1" : "") + "</dzsl>" +
                                    "<bjsl>" + (con.Bj ? "1" : "") + "</bjsl>" +
                                    "<njbsl>" + (con.Njb ? "1" : "") + "</njbsl>" +
                                    "<zjsaqd></zjsaqd>" +
                                    "<fjsaqd></fjsaqd>" +
                                    "<zdmb>" + con.Zdmb + "</zdmb>" +
                                    "<begnum>" + startnum + "</begnum>" +
                                    "<num>" + querycount + "</num>" +
                                "</Cmd>" +
                                 "<LogInfo>" +
                                    "<userName>" + con.UserName + "</userName>" +
                                    "<userIp>" + con.UserIp + "</userIp>" +
                                    "<userCode>" + con.UserCode + "</userCode>" +
                                    "<dyzgnmkbh>" + con.Dyzgnmkbh + "</dyzgnmkbh>" +
                                    "<dyzgnmkmc>" + con.Dyzgnmkmc + "</dyzgnmkmc>" +
                                "</LogInfo>" +
                           "</Body>" +
                       "</Message>";
                return xml;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return "";
            }
        }

        /// <summary>
        /// 获得大数据返回的过车总数
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static int GetPassCount(string xmlStr)
        {
            try
            {
                if (!string.IsNullOrEmpty(xmlStr))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlStr);

                    XmlNodeList listNodes = xmlDoc.SelectNodes("Message/Body/Return/passcarinfolist");
                    return int.Parse(listNodes[0].Attributes[0].Value);
                }
                return 0;
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return 0;
            }
        }

        /// <summary>
        /// 获得交互日志总数
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static int GetBusinessRowCount(XmlDocument doc)
        {
            try
            {
                string allNum = doc.SelectSingleNode("//LogInfoTotal").InnerText;
                return Convert.ToInt32(allNum);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return 0;
            }
        }

        /// <summary>
        /// 获得返回过车总数
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static int GetRowCount(XmlDocument doc)
        {
            try
            {
                string allNum = doc.SelectSingleNode("//passcarinfolist").Attributes["totalnum"].Value;
                return Convert.ToInt32(allNum);
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return 0;
            }
        }

        #endregion 大数据查询

        /// <summary>
        /// 提取修改添加日志
        /// </summary>
        /// <param name="oidStr">原获取文本参数</param>
        /// <param name="newStr">修改后文本参数</param>
        /// <param name="desc">描述信息</param>
        /// <param name="type">类型 0是添加 1是更新</param>
        /// <returns></returns>
        public static string AssembleRunLog(string oldStr, string newStr, string desc, string type)
        {
            try
            {
                if (type.Equals("0"))
                {
                    if (!string.IsNullOrEmpty(newStr))
                    {
                        return desc + ":[" + newStr + "];";
                    }
                }
                else
                {
                    if (!oldStr.Equals(newStr))
                    {
                        return "将" + desc + "由[" + oldStr + "]修改为[" + newStr + "];";
                    }
                }
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
            }
            return "";
        }

        /// <summary>
        /// 截取获得目录及功能
        /// </summary>
        /// <param name="funcname"></param>
        /// <returns></returns>
        public static string[] GetContentFunc(string funcname)
        {
            try
            {
                if (funcname.IndexOf("-") > 0)
                {
                    return funcname.Split('-');
                }
                return new string[] { "", "" };
            }
            catch (Exception ex)
            {
                ILog.WriteErrorLog(ex);
                return new string[] { "", "" };
            }
        }
    }
}