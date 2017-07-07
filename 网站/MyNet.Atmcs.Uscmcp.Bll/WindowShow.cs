using Ext.Net;
using System;
using System.Data;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class WindowShow
    {
        /// <summary>
        ///
        /// </summary>
        private static GetIPAdddress getIp;

        /// <summary>
        /// 违法
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static Window AddPeccancy(string jsonData)
        {
            TabPanel center = new TabPanel();
            center.ID = "CenterPanel";
            center.ActiveTabIndex = 0;

            Ext.Net.Panel tab2 = new Ext.Net.Panel();
            tab2.ID = "Tab2";
            tab2.Title = GetLangStr("page","WindowShow1","车辆信息") ;
            tab2.Border = false;
            tab2.BodyStyle = "padding:6px;";
            tab2.Collapsible = false;

            Container container = new Container();
            container.Layout = "Column";
            container.Height = 220;

            Container container1 = new Container();
            container1.LabelAlign = LabelAlign.Left;
            container1.Layout = "Form";
            container1.ColumnWidth = 0.25;
            container1.Items.Add(CommonExt.AddTextField("txthphm",GetLangStr("page","WindowShow2","号牌号码") , Bll.Common.GetdatabyField(jsonData, "col3"), ""));
            container1.Items.Add(CommonExt.AddTextField("txtcdbh",GetLangStr("page","WindowShow3","车道编号") , Bll.Common.GetdatabyField(jsonData, "col9"), ""));
            container1.Items.Add(CommonExt.AddTextField("txtclsd",GetLangStr("page","WindowShow4","速度/限速") , Bll.Common.GetdatabyField(jsonData, "col12"), ""));
            container1.Items.Add(CommonExt.AddTextField("txtshzt",GetLangStr("page","WindowShow5","审核状态") , Bll.Common.GetdatabyField(jsonData, "col15"), ""));
            container1.Items.Add(CommonExt.AddTextField("txtzqmj",GetLangStr("page","WindowShow6","执勤民警") , Bll.Common.GetdatabyField(jsonData, "col22"), ""));
            Container container2 = new Container();
            container2.LabelAlign = LabelAlign.Left;
            container2.Layout = "Form";
            container2.ColumnWidth = 0.35;
            container2.Items.Add(CommonExt.AddTextField("txthpzl",GetLangStr("page","WindowShow7","号牌种类") , Bll.Common.GetdatabyField(jsonData, "col2"), ""));
            container2.Items.Add(CommonExt.AddTextField("txtwfsj",GetLangStr("page","WindowShow8","违法时间") , Bll.Common.GetdatabyField(jsonData, "col6", ""), ""));
            container2.Items.Add(CommonExt.AddTextField("txtsjly",GetLangStr("page","WindowShow9","数据来源") , Bll.Common.GetdatabyField(jsonData, "col13"), ""));
            container2.Items.Add(CommonExt.AddTextField("txttzzt",GetLangStr("page","WindowShow10","通知状态") , Bll.Common.GetdatabyField(jsonData, "col16"), ""));
            container2.Items.Add(CommonExt.AddTextField("txtjczt",GetLangStr("page","WindowShow11","处理状态") , Bll.Common.GetdatabyField(jsonData, "col20"), ""));

            Container container3 = new Container();
            container3.LabelAlign = LabelAlign.Left;
            container3.Layout = "Form";
            container3.ColumnWidth = 0.4;
            container3.Items.Add(CommonExt.AddTextField("txtwfxw",GetLangStr("page","WindowShow12","违法行为") , Bll.Common.GetdatabyField(jsonData, "col5"), ""));
            container3.Items.Add(CommonExt.AddTextField("txtwfdd",GetLangStr("page","WindowShow13","违法地点") , Bll.Common.GetdatabyField(jsonData, "col8"), ""));
            container3.Items.Add(CommonExt.AddTextField("txtcjjg",GetLangStr("page","WindowShow14","所属机构") , Bll.Common.GetdatabyField(jsonData, "col14"), ""));
            container3.Items.Add(CommonExt.AddTextField("txtcfzt",GetLangStr("page","WindowShow15","处罚状态") , Bll.Common.GetdatabyField(jsonData, "col17"), ""));
            container3.Items.Add(CommonExt.AddTextField("txtcszt",GetLangStr("page","WindowShow16","传输状态") , Bll.Common.GetdatabyField(jsonData, "col19"), ""));

            container.Items.Add(container1);
            container.Items.Add(container2);
            container.Items.Add(container3);

            tab2.Items.Add(container);
            center.Items.Add(tab2);

            Ext.Net.Panel south = new Ext.Net.Panel();
            south.ID = "SouthPanel";
            //south.Title = "图片信息";
            south.Height = System.Web.UI.WebControls.Unit.Pixel(310);
            south.BodyStyle = "padding:6px;";
            south.Html = GetHtml(Bll.Common.GetdatabyField(jsonData, "col23"), Bll.Common.GetdatabyField(jsonData, "col24"), Bll.Common.GetdatabyField(jsonData, "col25"));
            south.AutoScroll = true;

            BorderLayout layout = new BorderLayout();
            layout.South.Split = true;
            //layout.South.Collapsible = true;
            layout.Center.Items.Add(center);
            layout.South.Items.Add(south);

            Window win = new Window();
            win.ID = "Window1";
            win.Title =GetLangStr("page","WindowShow17","违法车辆详细信息") ;
            win.Icon = Icon.Application;
            win.Width = System.Web.UI.WebControls.Unit.Pixel(800);
            win.Height = System.Web.UI.WebControls.Unit.Pixel(600);
            win.LabelWidth = 100;
            win.Plain = true;
            win.Border = false;
            win.BodyBorder = false;

            Toolbar toolbar = new Ext.Net.Toolbar();
            ToolbarFill toolbarFill = new ToolbarFill();
            toolbar.Add(toolbarFill);
            win.BottomBar.Add(toolbar);
            CommonExt.AddButton(toolbar, "butCancel", GetLangStr("page","WindowShow18","退出"), "Cancel", win.ClientID + ".hide()");
            win.Items.Add(layout);

            return win;
        }

        /// <summary>
        /// 违法
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static Window AddPeccancy(DataRow row)
        {
            TabPanel center = new TabPanel();
            center.ID = "CenterPanel";
            center.ActiveTabIndex = 0;

            Ext.Net.Panel tab2 = new Ext.Net.Panel();
            tab2.ID = "Tab2";
            tab2.Title =GetLangStr("page","WindowShow1","车辆信息") ;
            tab2.Border = false;
            tab2.BodyStyle = "padding:6px;";
            tab2.Collapsible = false;

            Container container = new Container();
            container.Layout = "Column";
            container.Height = 220;

            Container container1 = new Container();
            container1.LabelAlign = LabelAlign.Left;
            container1.Layout = "Form";
            container1.ColumnWidth = 0.25;
            container1.Items.Add(CommonExt.AddTextField("txthphm", GetLangStr("page", "WindowShow2", "号牌号码"), row["col3"].ToString(), ""));
            container1.Items.Add(CommonExt.AddTextField("txtcdbh", GetLangStr("page", "WindowShow3", "车道编号"), row["col9"].ToString(), ""));
            container1.Items.Add(CommonExt.AddTextField("txtclsd", GetLangStr("page", "WindowShow4", "速度/限速"), row["col12"].ToString(), ""));
            container1.Items.Add(CommonExt.AddTextField("txtshzt", GetLangStr("page", "WindowShow5", "审核状态"), row["col15"].ToString(), ""));
            container1.Items.Add(CommonExt.AddTextField("txtzqmj", GetLangStr("page", "WindowShow6", "执勤民警"), row["col22"].ToString(), ""));
            Container container2 = new Container();
            container2.LabelAlign = LabelAlign.Left;
            container2.Layout = "Form";
            container2.ColumnWidth = 0.35;
            container2.Items.Add(CommonExt.AddTextField("txthpzl", GetLangStr("page", "WindowShow7", "号牌种类"), row["col2"].ToString(), ""));
            container2.Items.Add(CommonExt.AddTextField("txtwfsj", GetLangStr("page", "WindowShow8", "违法时间"), row["col6"].ToString(), ""));
            container2.Items.Add(CommonExt.AddTextField("txtsjly", GetLangStr("page", "WindowShow9", "数据来源"), row["col13"].ToString(), ""));
            container2.Items.Add(CommonExt.AddTextField("txttzzt", GetLangStr("page", "WindowShow10", "通知状态"), row["col16"].ToString(), ""));
            container2.Items.Add(CommonExt.AddTextField("txtjczt", GetLangStr("page", "WindowShow11", "处理状态"), row["col20"].ToString(), ""));

            Container container3 = new Container();
            container3.LabelAlign = LabelAlign.Left;
            container3.Layout = "Form";
            container3.ColumnWidth = 0.4;
            container3.Items.Add(CommonExt.AddTextField("txtwfxw", GetLangStr("page", "WindowShow12", "违法行为"), row["col5"].ToString(), ""));
            container3.Items.Add(CommonExt.AddTextField("txtwfdd", GetLangStr("page", "WindowShow13", "违法地点"), row["col8"].ToString(), ""));
            container3.Items.Add(CommonExt.AddTextField("txtcjjg", GetLangStr("page", "WindowShow14", "所属机构"), row["col14"].ToString(), ""));
            container3.Items.Add(CommonExt.AddTextField("txtcfzt", GetLangStr("page", "WindowShow15", "处罚状态"), row["col17"].ToString(), ""));
            container3.Items.Add(CommonExt.AddTextField("txtcszt", GetLangStr("page", "WindowShow16", "传输状态"), row["col19"].ToString(), ""));

            container.Items.Add(container1);
            container.Items.Add(container2);
            container.Items.Add(container3);

            tab2.Items.Add(container);
            center.Items.Add(tab2);

            Ext.Net.Panel south = new Ext.Net.Panel();
            south.ID = "SouthPanel";
            //south.Title = "图片信息";
            south.Height = System.Web.UI.WebControls.Unit.Pixel(310);
            south.BodyStyle = "padding:6px;";
            south.Html = GetHtml(row["col23"].ToString(), row["col24"].ToString(), row["col25"].ToString());
            south.AutoScroll = true;

            BorderLayout layout = new BorderLayout();
            layout.South.Split = true;
            //layout.South.Collapsible = true;
            layout.Center.Items.Add(center);
            layout.South.Items.Add(south);

            Window win = new Window();
            win.ID = "Window1";
            win.Title = "违法车辆详细信息";
            win.Icon = Icon.Application;
            win.Width = System.Web.UI.WebControls.Unit.Pixel(800);
            win.Height = System.Web.UI.WebControls.Unit.Pixel(600);
            win.LabelWidth = 100;
            win.Plain = true;
            win.Border = false;
            win.BodyBorder = false;
            //win.Collapsible = true;

            Toolbar toolbar = new Ext.Net.Toolbar();
            ToolbarFill toolbarFill = new ToolbarFill();
            toolbar.Add(toolbarFill);
            win.BottomBar.Add(toolbar);
            CommonExt.AddButton(toolbar, "butCancel", "退出", "Cancel", win.ClientID + ".hide()");
            win.Items.Add(layout);

            return win;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        /// <param name="src3"></param>
        /// <returns></returns>
        protected static string GetHtml(string src1, string src2, string src3)
        {
            if (string.IsNullOrEmpty(src2))
            {
                src2 = src1;
            }
            string Html = "<div class=\"details\">";
            Html = Html + " <tpl for=\".\">";
            Html = Html + "   <center> ";
            Html = Html + "      <img src=\"" + src1 + "\" title=\"单击放大图片\" width=\"350\" onclick=\"$.openPhotoGalleryXiangqing(this);\" class=\"photo\"; />";
            Html = Html + "      <img src=\"" + src2 + "\" title=\"单击放大图片\"  width=\"350\" onclick=\"$.openPhotoGalleryXiangqing(this);\"; class=\"photo\";  /></center>";
            //Html = Html + "      <img src=\"" + src3 + "\"  width=\"250\" onDblClick=\"OpenPicPage(this.src)\"; />";
            Html = Html + "</tpl>";
            Html = Html + "</div>";
            return Html;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        /// <param name="src3"></param>
        /// <returns></returns>
        protected static string GetHtml(string page, string src1, string src2, string src3)
        {
            if (string.IsNullOrEmpty(src2))
            {
                src2 = src1;
            }
            string Html = "<div class=\"details\">";
            Html = Html + " <tpl for=\".\">";
            Html = Html + "   <center> ";
            Html = Html + "      <img src=\"" + src1 + "\" title=\"" + GetLangStr(page, "AlarmInfoQuery71", "单击放大图片") + "\" width=\"350\" onclick=\"$.openPhotoGalleryXiangqing(this);\" class=\"photo\"; />";
            Html = Html + "      <img src=\"" + src2 + "\" title=\"" + GetLangStr(page, "AlarmInfoQuery71", "单击放大图片") + "\"  width=\"350\" onclick=\"$.openPhotoGalleryXiangqing(this);\"; class=\"photo\";  /></center>";
            //Html = Html + "      <img src=\"" + src3 + "\"  width=\"250\" onDblClick=\"OpenPicPage(this.src)\"; />";
            Html = Html + "</tpl>";
            Html = Html + "</div>";
            return Html;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        /// <param name="src3"></param>
        /// <returns></returns>
        public static string GetHtml(string src1)
        {
            string Html = "<div class=\"details\" >";
            // Html = Html + "<div class=\"fis\">"; style=\" padding: 10px;text-align: center;\"

            Html = Html + " <tpl for=\".\">";
            Html = Html + "   <center> ";
            Html = Html + "      <img src=\"" + src1 + "\" title=\"单击放大图片\" width=\"700\" onclick=\"$.openPhotoGalleryXiangqing(this);\" class=\"photo\"; />";
            // Html = Html + "      <img src=\"" + src2 + "\" title=\"单击放大图片\"  width=\"350\" onclick=\"$.openPhotoGalleryXiangqing(this);\"; class=\"photo\";  /></center>";
            //Html = Html + "      <img src=\"" + src3 + "\"  width=\"250\" onDblClick=\"OpenPicPage(this.src)\"; />";
            Html = Html + "</center></tpl>";
            Html = Html + "</div> ";
            // Html = Html + "</div>";
            return Html;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sdata"></param>
        /// <returns></returns>
        private static Panel GetBorderLayout(string sdata)
        {
            Panel main = new Panel();
            main.Height = 500;
            main.Width = 780;
            TabPanel center = new TabPanel();
            center.ID = "CenterPanel";
            center.ActiveTabIndex = 0;
            // center.Layout = "Row";

            //TabPanel center2= new TabPanel();
            //center2.ID = "CenterPanel2";
            // center2.ActiveTabIndex = 0;
            Ext.Net.Panel menu1 = new Panel();
            menu1.Title = "车辆信息";
            menu1.ID = "menu1";
            //menu1.Height = 500;
            menu1.Layout = "Row";

            Ext.Net.Panel tab2 = new Ext.Net.Panel();
            tab2.ID = "Tab2";
            //tab2.Title = "车辆信息";
            tab2.Border = false;
            tab2.BodyStyle = "padding:6px;";

            Container container = new Container();
            container.Layout = "Column";
            container.Height = 120;

            Container container1 = new Container();
            container1.LabelAlign = LabelAlign.Left;
            container1.Layout = "Form";
            container1.ColumnWidth = 0.25;
            container1.Items.Add(CommonExt.AddTextField("txthphm", GetLangStr("page","WindowShow52","号牌号码") , Bll.Common.GetdatabyField(sdata, "col3"), ""));
            container1.Items.Add(CommonExt.AddTextField("txtcdbh", GetLangStr("page","WindowShow28","车道编号") , Bll.Common.GetdatabyField(sdata, "col9"), ""));
            container1.Items.Add(CommonExt.AddTextField("txtclsd", GetLangStr("page","WindowShow29","车辆速度") , Bll.Common.GetdatabyField(sdata, "col10"), ""));
            //container1.Items.Add(CommonExt.AddTextField("txtclxs", "车辆限速", Bll.Common.GetdatabyField(sdata, "col19")));
            //container1.Items.Add(CommonExt.AddTextField("txtTxsd", "通行时段", Bll.Common.GetdatabyField(sdata, "col10"), ""));//通行时段
            //container1.Items.Add(CommonExt.AddTextField("txtTxld", "通行路段", Bll.Common.GetdatabyField(sdata, "col10"), ""));//通行路段

            Container container2 = new Container();
            container2.LabelAlign = LabelAlign.Left;
            container2.Layout = "Form";
            container2.ColumnWidth = 0.35;
            container2.Items.Add(CommonExt.AddTextField("txthpzl", GetLangStr("page","WindowShow30","号牌种类") , Bll.Common.GetdatabyField(sdata, "col5"), ""));
            container2.Items.Add(CommonExt.AddTextField("txtgwsj", GetLangStr("page","WindowShow31","过往时间") , Bll.Common.GetdatabyField(sdata, "col6", ""), ""));
            container2.Items.Add(CommonExt.AddTextField("txtjllx",  GetLangStr("page","WindowShow32","记录类型"), Bll.Common.GetdatabyField(sdata, "col14"), ""));
            //container2.Items.Add(CommonExt.AddTextField("txtxh", "记录编号", Bll.Common.GetdatabyField(sdata, "col0")));
            //container2.Items.Add(CommonExt.AddTextField("txtTxkksj", "通行开始时间", Bll.Common.GetdatabyField(sdata, "col6", ""), ""));

            Container container3 = new Container();
            container3.LabelAlign = LabelAlign.Left;
            container3.Layout = "Form";
            container3.ColumnWidth = 0.4;
            container3.Items.Add(CommonExt.AddTextField("txtfxmc", GetLangStr("page","WindowShow33","行驶方向") , Bll.Common.GetdatabyField(sdata, "col8"), ""));
            container3.Items.Add(CommonExt.AddTextField("txtkkmc", GetLangStr("page","WindowShow34","卡口名称") , Bll.Common.GetdatabyField(sdata, "col2"), ""));
            container3.Items.Add(CommonExt.AddTextField("txtsjly", GetLangStr("page","WindowShow35","数据来源") , Bll.Common.GetdatabyField(sdata, "col12"), ""));
            // container3.Items.Add(CommonExt.AddTextField("txtcjjg", "所属机构", Bll.Common.GetdatabyField(sdata, "col25")));
            //container3.Items.Add(CommonExt.AddTextField("txtkYxjssj", "有效期结束时间", Bll.Common.GetdatabyField(sdata, "col6", ""), ""));

            // container4.Items.Add(CommonExt.AddTextField("txtsjly", "数据来源", Bll.Common.GetdatabyField(sdata, "col12"), ""));
            container.Items.Add(container1);
            container.Items.Add(container2);
            container.Items.Add(container3);
            tab2.Items.Add(container);

            #region 通行证信息

            //通过号牌种类和号牌号码得到通行证信息
            string txzXinxi = Bll.Common.GetdatabyField(sdata, "col4") + Bll.Common.GetdatabyField(sdata, "col3");
            DataTable dt = GetRedisData.GetData("Passport:" + txzXinxi);
            DataTable dtTxz = null;
            if (dt != null)
            {
                dtTxz = Common.ChangColName(dt);
            }
            Ext.Net.Panel menu2 = new Panel();
            menu2.Title = GetLangStr("page","WindowShow19","通行证信息") ;
            menu2.Layout = "Row";
            menu2.ID = "menu2";
            // menu2.Height = 500;

            Ext.Net.Panel tab3 = new Ext.Net.Panel();
            tab3.ID = "Tab3";
            // tab3.Title = "通行证信息";
            tab3.Border = false;
            tab3.BodyStyle = "padding:6px;";

            Container container11 = new Container();
            container11.Layout = "Row";
            container11.Height = 120;

            Container container4 = new Container();
            container4.Layout = "Column";

            Container container5 = new Container();
            container5.LabelAlign = LabelAlign.Left;
            container5.Layout = "Form";
            container5.ColumnWidth = 0.5;
            container5.Height = 30;
            if (dtTxz != null)
            {
                if (!string.IsNullOrEmpty(dtTxz.Rows[0]["col2"].ToString()) && !string.IsNullOrEmpty(dtTxz.Rows[0]["col3"].ToString()))
                {
                    container5.Items.Add(
                  CommonExt.AddTextField("txtQzsj", GetLangStr("page","WindowShow20","起止时间") , Convert.ToDateTime(dtTxz.Rows[0]["col2"].ToString()).ToString("yyyy-MM-dd") + " -- " + Convert.ToDateTime(dtTxz.Rows[0]["col3"].ToString()).ToString("yyyy-MM-dd"), "", 280)
                  );
                }
                else
                {
                    container5.Items.Add(
                  CommonExt.AddTextField("txtQzsj", GetLangStr("page","WindowShow20", "起止时间"), "", "", 280)
                  );
                }
            }
            else
            {
                container5.Items.Add(
                CommonExt.AddTextField("txtQzsj", GetLangStr("page","WindowShow20","起止时间") , "", "", 280));
            }

            Container container6 = new Container();
            container6.LabelAlign = LabelAlign.Left;
            container6.Layout = "Form";
            container6.ColumnWidth = 0.5;
            container6.Height = 30;
            container6.Items.Add(CommonExt.AddComboBox("cmbZfgg", GetLangStr("page","WindowShow21","政府公告") , "StoreZfgg", GetLangStr("page","WindowShow22", "选择政府公告"), 280));
            //container6.Items.Add(CommonExt.AddTextField("txtTxld", "通行路段", Bll.Common.GetdatabyField(sdata, "col10"), ""));//通行路段
            //container6.Items.Add(CommonExt.AddLable("txtTxsd", "通行时段", Bll.Common.GetdatabyField(sdata, "col10")));//通行时段
            container4.Items.Add(container5);
            container4.Items.Add(container6);

            Container container7 = new Container();
            container7.Layout = "Column";

            //container7.Layout = "Row";
            Container container12 = new Container();
            container12.LabelAlign = LabelAlign.Left;
            container12.Layout = "Form";
            container12.ColumnWidth = 1;
            container12.Height = 30;
            if (dtTxz != null)
            {
                if (!string.IsNullOrEmpty(dtTxz.Rows[0]["col5"].ToString()))
                {
                    container12.Items.Add(CommonExt.AddTextField("txtTxld", GetLangStr("page", "WindowShow23", "通行路段"), dtTxz.Rows[0]["col5"].ToString(), "", 660));//通行路段
                }
                else
                {
                    container12.Items.Add(CommonExt.AddTextField("txtTxld",  GetLangStr("page","WindowShow23","通行路段"), "", "", 661));//通行路段
                }
            }
            else
            {
                container12.Items.Add(CommonExt.AddTextField("txtTxld", GetLangStr("page","WindowShow23","通行路段") , "", "", 660));//通行路段
            }

            container7.Items.Add(container12);

            Container container8 = new Container();
            container8.Layout = "Column";
            Container container9 = new Container();
            container9.LabelAlign = LabelAlign.Left;
            container9.Layout = "Form";
            container9.ColumnWidth = 0.99;
            container9.Height = 35;
            if (dtTxz != null)
            {
                if (!string.IsNullOrEmpty(dtTxz.Rows[0]["col4"].ToString()))
                {
                    container9.Items.Add(CommonExt.AddLable("txtTxsd", GetLangStr("page", "WindowShow24", "通行时段"), dtTxz.Rows[0]["col4"].ToString()));//通行时段
                }
                else
                {
                    container9.Items.Add(CommonExt.AddLable("txtTxsd", GetLangStr("page", "WindowShow24", "通行时段"), ""));//通行时段
                }
            }
            else
            {
                container9.Items.Add(CommonExt.AddLable("txtTxsd", GetLangStr("page", "WindowShow24", "通行时段"), ""));//通行时段
            }

            container9.Items.Add(CommonExt.AddButton("btnSaveJrwf", GetLangStr("page","WindowShow25","加入违法") , "Disk", "PassCarInfoQuery.Jrwf()", "margin-left:650px;margin-top:-26px;"));
            container8.Items.Add(container9);

            Container container10 = new Container();
            container10.LabelAlign = LabelAlign.Left;
            container10.ColumnWidth = 1;
            container10.Height = 25;
            if (dtTxz != null)//有通行证信息
            {
                container10.Items.Add(CommonExt.AddLable("txtTxzxx", GetLangStr("page", "WindowShow26", "通行证状态"), GetLangStr("page","WindowShow53", "取到通行证"), "color:green;font-size:18px;"));
            }
            else//无通行证信息
            {
                container10.Items.Add(CommonExt.AddLable("txtTxzxx", GetLangStr("page","WindowShow26","通行证状态"),GetLangStr("page","WindowShow54", "无通行证") , "color:red;font-size:18px;"));
            }

            //Container container10 = new Container();
            //container10.LabelAlign = LabelAlign.Left;
            //container10.Layout = "Form";
            //container10.ColumnWidth = 0.1;
            ////container10.Height = 40;
            //container10.Items.Add(CommonExt.AddButton("btnSaveJrwf", "加入违法", "Disk", "PassCarInfoQuery.Jrwf()","margin-left:550px;"));
            //container8.Items.Add(container10);
            container11.Items.Add(container10);
            container11.Items.Add(container4);
            container11.Items.Add(container7);
            container11.Items.Add(container8);

            tab3.Items.Add(container11);

            #endregion 通行证信息

            // center.Layout = "Row";//表示横向布局

            menu1.Items.Add(tab2);
            Ext.Net.Panel south = new Ext.Net.Panel();
            south.ID = "SouthPanel";
            // south.Title = "图片信息";
            south.Height = System.Web.UI.WebControls.Unit.Pixel(380);
            south.BodyStyle = "padding:3px;";
            south.Html = GetHtml(Bll.Common.GetdatabyField(sdata, "col15"), Bll.Common.GetdatabyField(sdata, "col16"), Bll.Common.GetdatabyField(sdata, "col17"));
            south.AutoScroll = true;
            menu1.Items.Add(south);
            menu2.Items.Add(tab3);
            Ext.Net.Panel south2 = new Ext.Net.Panel();
            south2.ID = "SouthPanel2";
            // south.Title = "图片信息";
            south2.Height = System.Web.UI.WebControls.Unit.Pixel(380);
            south2.BodyStyle = "padding:3px;";
            //south2.Html = GetHtml(Bll.Common.GetdatabyField(sdata, "col15"));
            south2.AutoScroll = true;
            menu2.Items.Add(south2);
            center.Items.Add(menu1);
            center.Items.Add(menu2);

            main.Items.Add(center);
            return main;

            //BorderLayout layout = new BorderLayout();
            //layout.Center.Items.Add(center);
            //return layout;
        }

        /// <summary>
        ///过往车辆详细信息
        /// </summary>
        /// <param name="sdata"></param>
        /// <returns></returns>
        public static Window AddPasscCar( string sdata)
        {
            Panel layout = GetBorderLayout(sdata);

            Window win = new Window();
            win.ID = "Window1";
            win.Title =  GetLangStr("page","WindowShow50","过往车辆详细信息");
            win.Icon = Icon.Application;
            win.Width = System.Web.UI.WebControls.Unit.Pixel(800);
            win.Height = System.Web.UI.WebControls.Unit.Pixel(600);
            win.Plain = true;
            win.Border = false;
            win.BodyBorder = false;
            win.Layout = "Fit";
            //win.Collapsible = true;

            win.Buttons.Add(CommonExt.AddButton("butCancel", GetLangStr("page","WindowShow51","退出") , "Cancel", win.ClientID + ".hide()"));
            win.Items.Add(layout);
            return win;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sdata"></param>
        /// <returns></returns>
        public static FormPanel AddPasscCarPanel(string sdata)
        {
            //BorderLayout layout = GetBorderLayout(sdata);
            Panel layout = GetBorderLayout(sdata);
            FormPanel panel = new FormPanel();
            panel.ID = "panel1";
            panel.Title = "过往车辆详细信息";
            panel.Icon = Icon.Application;
            panel.Width = System.Web.UI.WebControls.Unit.Pixel(800);
            panel.Height = System.Web.UI.WebControls.Unit.Pixel(560);
            panel.Border = false;
            panel.BodyBorder = false;
            panel.Collapsible = true;
            panel.Items.Add(layout);
            return panel;
        }

        /// <summary>
        /// 报警信息
        /// </summary>
        /// <param name="sdata"></param>
        /// <returns></returns>
        public static Window AddAlarmCar(string page, string sdata)
        {
            Window win = new Window();
            win.ID = "Window1";
            win.Title = GetLangStr(page, "AlarmInfoQuery65", "报警详细信息");
            win.Icon = Icon.Application;
            win.Width = System.Web.UI.WebControls.Unit.Pixel(800);
            win.Height = System.Web.UI.WebControls.Unit.Pixel(600);
            win.Plain = true;
            win.Border = false;
            win.BodyBorder = false;

            Ext.Net.FormPanel tab2 = new Ext.Net.FormPanel();
            tab2.ID = "Tab2";
            //tab2.Title = "报警信息";
            tab2.Border = false;
            tab2.BodyStyle = "padding:6px;";
            tab2.MonitorValid = true;

            //Toolbar toolbar = new Toolbar();
            //toolbar.ID = "tool1";
            //toolbar.Items.Add(new ToolbarFill());
            ////toolbar.Items.Add(CommonExt.AddTextField("txtClyj", "处理意见", true, "请输入处理意见...",220,110));
            //toolbar.Items.Add(CommonExt.AddComboBox("cmbClbj", "处理结果", "StoreClbj", "请选择...", false, 224, 105, Bll.Common.GetdatabyField(sdata, "col24")));
            //toolbar.Items.Add(CommonExt.AddButton("btnSaveClbj", "保存", "Disk", "Ovel.SaveClbj(" + Bll.Common.GetdatabyField(sdata, "col0") + ")"));
            //toolbar.Items.Add(CommonExt.AddButton("butCancel", "退出", "Cancel", win.ClientID + ".hide()"));
            //tab2.TopBar.Add(toolbar);
            Container container = new Container();
            container.Layout = "Column";
            container.Height = 128;

            Container container1 = new Container();
            container1.LabelAlign = LabelAlign.Left;
            container1.Layout = "Form";
            container1.ColumnWidth = 0.3;
            container1.Items.Add(CommonExt.AddTextField("txthphm", GetLangStr(page, "AlarmInfoQuery33", "号牌号码"), Bll.Common.GetdatabyField(sdata, "col3"), ""));
            container1.Items.Add(CommonExt.AddTextField("txtbjlx", GetLangStr(page, "AlarmInfoQuery37", "报警类型"), Bll.Common.GetdatabyField(sdata, "col19"), ""));
            container1.Items.Add(CommonExt.AddTextField("txtcdbh", GetLangStr(page, "AlarmInfoQuery66", "布控人"), Bll.Common.GetdatabyField(sdata, "col26"), ""));
            container1.Items.Add(CommonExt.AddTextField("txtbjyy", GetLangStr(page, "AlarmInfoQuery38", "报警原因"), Bll.Common.GetdatabyField(sdata, "col20"), ""));

            Container container2 = new Container();
            container2.LabelAlign = LabelAlign.Left;
            container2.Layout = "Form";
            container2.ColumnWidth = 0.3;
            container2.Items.Add(CommonExt.AddTextField("txthpzl", GetLangStr(page, "AlarmInfoQuery34", "号牌种类"), Bll.Common.GetdatabyField(sdata, "col5"), ""));
            container2.Items.Add(CommonExt.AddTextField("txtddmc", GetLangStr(page, "AlarmInfoQuery67", "报警地点"), Bll.Common.GetdatabyField(sdata, "col2"), ""));
            container2.Items.Add(CommonExt.AddTextField("txtclzt", GetLangStr(page, "AlarmInfoQuery68", "布控人电话"), Bll.Common.GetdatabyField(sdata, "col25"), ""));
            container2.Items.Add(CommonExt.AddComboBox("cmbClbj", GetLangStr(page, "AlarmInfoQuery39", "处理结果"), "StoreClbj", GetLangStr(page, "AlarmInfoQuery6", "请选择..."), false, 224, 105, Bll.Common.GetdatabyField(sdata, "col24")));

            Container container3 = new Container();
            container3.LabelAlign = LabelAlign.Left;
            container3.Layout = "Form";
            container3.ColumnWidth = 0.4;
            container3.Items.Add(CommonExt.AddTextField("txtbjsj", GetLangStr(page, "AlarmInfoQuery36", "报警时间"), Bll.Common.GetdatabyField(sdata, "col6", ""), ""));
            container3.Items.Add(CommonExt.AddTextField("txtfxmc", GetLangStr(page, "AlarmInfoQuery69", "布控原因"), Bll.Common.GetdatabyField(sdata, "col27"), ""));
            container3.Items.Add(CommonExt.AddTextField("txtcjjg", GetLangStr(page, "AlarmInfoQuery61", "有效时间"), Bll.Common.GetdatabyField(sdata, "col29"), ""));
            container3.Items.Add(CommonExt.AddButton("btnSaveClbj", GetLangStr(page, "AlarmInfoQuery70", "保存"), "Disk", "Ovel.SaveClbj(" + Bll.Common.GetdatabyField(sdata, "col0") + ")"));
            //container3.Items.Add(CommonExt.AddButton("butCancel", "退出", "Cancel", win.ClientID + ".hide()"));

            container.Items.Add(container1);
            container.Items.Add(container2);
            container.Items.Add(container3);
            //tab2.Listeners.ClientValidation.Handler = "#{btnSaveClbj}.setDisabled(!valid);";
            tab2.Items.Add(container);

            Ext.Net.Panel south = new Ext.Net.Panel();
            south.ID = "SouthPanel";
            //south.Title = "图片信息";
            south.Height = System.Web.UI.WebControls.Unit.Pixel(380);
            south.BodyStyle = "padding:6px;";
            south.Html = GetHtml(page, Bll.Common.GetdatabyField(sdata, "col14"), Bll.Common.GetdatabyField(sdata, "col15"), Bll.Common.GetdatabyField(sdata, "col16"));
            south.AutoScroll = true;

            BorderLayout layout = new BorderLayout();
            layout.South.Split = true;
            //layout.South.Collapsible = true;
            layout.Center.Items.Add(tab2);
            layout.South.Items.Add(south);

            // win.Collapsible = true;
            win.Items.Add(layout);
            return win;
        }

        /// <summary>
        /// 报警信息
        /// </summary>
        /// <param name="sdata"></param>
        /// <returns></returns>
        public static Window AddAlarmCar(string sdata)
        {
            Window win = new Window();
            win.ID = "Window1";
            win.Title = "报警详细信息";
            win.Icon = Icon.Application;
            win.Width = System.Web.UI.WebControls.Unit.Pixel(800);
            win.Height = System.Web.UI.WebControls.Unit.Pixel(600);
            win.Plain = true;
            win.Border = false;
            win.BodyBorder = false;

            Ext.Net.FormPanel tab2 = new Ext.Net.FormPanel();
            tab2.ID = "Tab2";
            //tab2.Title = "报警信息";
            tab2.Border = false;
            tab2.BodyStyle = "padding:6px;";
            tab2.MonitorValid = true;

            //Toolbar toolbar = new Toolbar();
            //toolbar.ID = "tool1";
            //toolbar.Items.Add(new ToolbarFill());
            ////toolbar.Items.Add(CommonExt.AddTextField("txtClyj", "处理意见", true, "请输入处理意见...",220,110));
            //toolbar.Items.Add(CommonExt.AddComboBox("cmbClbj", "处理结果", "StoreClbj", "请选择...", false, 224, 105, Bll.Common.GetdatabyField(sdata, "col24")));
            //toolbar.Items.Add(CommonExt.AddButton("btnSaveClbj", "保存", "Disk", "Ovel.SaveClbj(" + Bll.Common.GetdatabyField(sdata, "col0") + ")"));
            //toolbar.Items.Add(CommonExt.AddButton("butCancel", "退出", "Cancel", win.ClientID + ".hide()"));
            //tab2.TopBar.Add(toolbar);
            Container container = new Container();
            container.Layout = "Column";
            container.Height = 128;

            Container container1 = new Container();
            container1.LabelAlign = LabelAlign.Left;
            container1.Layout = "Form";
            container1.ColumnWidth = 0.3;
            container1.Items.Add(CommonExt.AddTextField("txthphm", "号牌号码", Bll.Common.GetdatabyField(sdata, "col3"), ""));
            container1.Items.Add(CommonExt.AddTextField("txtbjlx", "报警类型", Bll.Common.GetdatabyField(sdata, "col19"), ""));
            container1.Items.Add(CommonExt.AddTextField("txtcdbh", "布控人", Bll.Common.GetdatabyField(sdata, "col26"), ""));
            container1.Items.Add(CommonExt.AddTextField("txtbjyy", "报警原因", Bll.Common.GetdatabyField(sdata, "col20"), ""));

            Container container2 = new Container();
            container2.LabelAlign = LabelAlign.Left;
            container2.Layout = "Form";
            container2.ColumnWidth = 0.3;
            container2.Items.Add(CommonExt.AddTextField("txthpzl", "号牌种类", Bll.Common.GetdatabyField(sdata, "col5"), ""));
            container2.Items.Add(CommonExt.AddTextField("txtddmc", "报警地点", Bll.Common.GetdatabyField(sdata, "col2"), ""));
            container2.Items.Add(CommonExt.AddTextField("txtclzt", "布控人电话", Bll.Common.GetdatabyField(sdata, "col25"), ""));
            container2.Items.Add(CommonExt.AddComboBox("cmbClbj", "处理结果", "StoreClbj", "请选择...", false, 224, 105, Bll.Common.GetdatabyField(sdata, "col24")));

            Container container3 = new Container();
            container3.LabelAlign = LabelAlign.Left;
            container3.Layout = "Form";
            container3.ColumnWidth = 0.4;
            container3.Items.Add(CommonExt.AddTextField("txtbjsj", "报警时间", Bll.Common.GetdatabyField(sdata, "col6", ""), ""));
            container3.Items.Add(CommonExt.AddTextField("txtfxmc", "布控原因", Bll.Common.GetdatabyField(sdata, "col27"), ""));
            container3.Items.Add(CommonExt.AddTextField("txtcjjg", "有效时间", Bll.Common.GetdatabyField(sdata, "col29"), ""));
            container3.Items.Add(CommonExt.AddButton("btnSaveClbj", "保存", "Disk", "Ovel.SaveClbj(" + Bll.Common.GetdatabyField(sdata, "col0") + ")"));
            //container3.Items.Add(CommonExt.AddButton("butCancel", "退出", "Cancel", win.ClientID + ".hide()"));

            container.Items.Add(container1);
            container.Items.Add(container2);
            container.Items.Add(container3);
            //tab2.Listeners.ClientValidation.Handler = "#{btnSaveClbj}.setDisabled(!valid);";
            tab2.Items.Add(container);

            Ext.Net.Panel south = new Ext.Net.Panel();
            south.ID = "SouthPanel";
            //south.Title = "图片信息";
            south.Height = System.Web.UI.WebControls.Unit.Pixel(380);
            south.BodyStyle = "padding:6px;";
            south.Html = GetHtml(Bll.Common.GetdatabyField(sdata, "col14"), Bll.Common.GetdatabyField(sdata, "col15"), Bll.Common.GetdatabyField(sdata, "col16"));
            south.AutoScroll = true;

            BorderLayout layout = new BorderLayout();
            layout.South.Split = true;
            //layout.South.Collapsible = true;
            layout.Center.Items.Add(tab2);
            layout.South.Items.Add(south);

            // win.Collapsible = true;
            win.Items.Add(layout);
            return win;
        }

        /// <summary>
        /// 流量信息
        /// </summary>
        /// <param name="sdata"></param>
        /// <returns></returns>
        public static Window AddFlowCar(string sdata)
        {
            Window win = new Window();
            win.ID = "Window1";
            win.Title = "流量详细信息";
            win.Icon = Icon.Application;
            win.Width = System.Web.UI.WebControls.Unit.Pixel(800);
            win.Height = System.Web.UI.WebControls.Unit.Pixel(490);
            win.Plain = true;
            win.Border = false;
            win.BodyBorder = false;

            Ext.Net.FormPanel tab2 = new Ext.Net.FormPanel();
            tab2.ID = "Tab2";
            //tab2.Title = "报警信息";
            tab2.Border = false;
            tab2.BodyStyle = "padding:6px;";
            tab2.MonitorValid = true;
            Container container = new Container();
            container.Layout = "Column";
            container.Height = 100;

            Container container1 = new Container();
            container1.LabelAlign = LabelAlign.Left;
            container1.Layout = "Form";
            container1.ColumnWidth = 0.3;
            container1.Items.Add(CommonExt.AddTextField("txtcxkk", "报警卡口", Bll.Common.GetdatabyField(sdata, "col1"), ""));
            container1.Items.Add(CommonExt.AddTextField("txttjzq", "统计周期", Bll.Common.GetdatabyField(sdata, "col5"), ""));
            container1.Items.Add(CommonExt.AddTextField("txtbjyz", "报警阈值", Bll.Common.GetdatabyField(sdata, "col6"), ""));
            Container container2 = new Container();
            container2.LabelAlign = LabelAlign.Left;
            container2.Layout = "Form";
            container2.ColumnWidth = 0.35;
            container2.Items.Add(CommonExt.AddTextField("txtbjsj", "报警时间", Bll.Common.GetdatabyField(sdata, "col4", ""), ""));
            container2.Items.Add(CommonExt.AddTextField("txtkkpzr", "卡口配置人", Bll.Common.GetdatabyField(sdata, "col9"), ""));
            container2.Items.Add(CommonExt.AddTextField("txtcljg", "处理结果", Bll.Common.GetdatabyField(sdata, "col12"), ""));

            Container container3 = new Container();
            container3.LabelAlign = LabelAlign.Left;
            container3.Layout = "Form";
            container3.ColumnWidth = 0.35;
            container3.Items.Add(CommonExt.AddTextField("txtkkfx", "卡口方向", Bll.Common.GetdatabyField(sdata, "col14"), ""));
            //container3.Items.Add(CommonExt.AddTextField("txtjssj", "结束时间", Bll.Common.GetdatabyField(sdata, "col3"), ""));
            container3.Items.Add(CommonExt.AddTextField("txtpzsj", "配置时间", Bll.Common.GetdatabyField(sdata, "col11", ""), ""));
            container3.Items.Add(CommonExt.AddComboBox("cmbcljg", "处理结果", "Storecljg", "请选择...", false, 224, 105, Bll.Common.GetdatabyField(sdata, "col10")));
            container.Items.Add(container1);
            container.Items.Add(container2);
            container.Items.Add(container3);
            //tab2.Listeners.ClientValidation.Handler = "#{btnSaveClbj}.setDisabled(!valid);";
            tab2.Items.Add(container);
            Toolbar toolbar = new Toolbar();
            toolbar.ID = "tool1";
            toolbar.Items.Add(new ToolbarFill());
            toolbar.Items.Add(CommonExt.AddButton("btnSavecljg", "保存", "Disk", "Ovel.Savecljg(" + Bll.Common.GetdatabyField(sdata, "col0") + ")"));
            toolbar.Items.Add(CommonExt.AddButton("butCancel", "退出", "Cancel", win.ClientID + ".hide()"));
            tab2.Items.Add(toolbar);
            BorderLayout layout = new BorderLayout();
            layout.South.Split = true;
            //layout.South.Collapsible = true;
            layout.Center.Items.Add(tab2);

            // win.Collapsible = true;
            win.Items.Add(layout);
            return win;
        }

        /// <summary>
        ///组装图片的显示
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        /// <returns></returns>
        protected static string GetHtml(string src1, string src2)
        {
            string Html = "<div class=\"details\">";
            Html = Html + " <tpl for=\".\">";
            Html = Html + "   <center> ";
            Html = Html + "      <img src=\"" + src1 + "\" title=\"单击放大图片\"  width=\"350\" onclick=\"$.openPhotoGalleryXiangqing(this);\" class=\"photo\" />";
            Html = Html + "      <img src=\"" + src2 + "\" title=\"单击放大图片\"  width=\"350\" onclick=\"$.openPhotoGalleryXiangqing(this);\" class=\"photo\" /></center>";
            Html = Html + "</tpl>";
            Html = Html + "</div>";
            return Html;
        }

        /// <summary>
        ///创建违法车辆详情框中显示的内容
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static Window AddPeccancyArea(string jsonData)
        {
            TabPanel center = new TabPanel();
            center.ID = "CenterPanel";
            center.ActiveTabIndex = 0;

            Ext.Net.Panel tab2 = new Ext.Net.Panel();
            tab2.ID = "Tab2";
            tab2.Title = "车辆信息";
            tab2.Border = false;
            tab2.BodyStyle = "padding:6px;";

            Container container = new Container();
            container.Layout = "Column";
            container.Height = 180;

            Container container1 = new Container();
            container1.LabelAlign = LabelAlign.Left;
            container1.Layout = "Form";
            container1.ColumnWidth = 0.33;
            container1.Items.Add(CommonExt.AddTextField("txthphm", "号牌号码", Bll.Common.GetdatabyField(jsonData, "col2"), ""));

            container1.Items.Add(CommonExt.AddTextField("txtkskk", "起点卡口", Bll.Common.GetdatabyField(jsonData, "col16"), ""));
            container1.Items.Add(CommonExt.AddTextField("txtkssj", "起始时间", Bll.Common.GetdatabyField(jsonData, "col19", ""), ""));
            container1.Items.Add(CommonExt.AddTextField("txtxssd", "行驶速度", Bll.Common.GetdatabyField(jsonData, "col13"), ""));
            container1.Items.Add(CommonExt.AddTextField("txtcsbl", "超速比例", Bll.Common.GetdatabyField(jsonData, "col26"), ""));

            Container container2 = new Container();
            container2.LabelAlign = LabelAlign.Left;
            container2.Layout = "Form";
            container2.ColumnWidth = 0.33;
            container2.Items.Add(CommonExt.AddTextField("txthpzl", "号牌种类", Bll.Common.GetdatabyField(jsonData, "col4"), ""));
            container2.Items.Add(CommonExt.AddTextField("txtjskk", "终点卡口", Bll.Common.GetdatabyField(jsonData, "col18"), ""));
            container2.Items.Add(CommonExt.AddTextField("txtjssj", "结束时间", Bll.Common.GetdatabyField(jsonData, "col20", ""), ""));
            container2.Items.Add(CommonExt.AddTextField("txtqjxs", "区间限速", Bll.Common.GetdatabyField(jsonData, "col11"), ""));
            container2.Items.Add(CommonExt.AddTextField("txtshzt", "审核状态", Bll.Common.GetdatabyField(jsonData, "col22"), ""));

            Container container3 = new Container();
            container3.LabelAlign = LabelAlign.Left;
            container3.Layout = "Form";
            container3.ColumnWidth = 0.33;
            container3.Items.Add(CommonExt.AddTextField("txtfxmc", "行驶方向", Bll.Common.GetdatabyField(jsonData, "col6"), ""));
            container3.Items.Add(CommonExt.AddTextField("txtqjjl", "区间距离", Bll.Common.GetdatabyField(jsonData, "col10"), ""));
            container3.Items.Add(CommonExt.AddTextField("txtqjhs", "区间耗时", Bll.Common.GetdatabyField(jsonData, "col12"), ""));
            container3.Items.Add(CommonExt.AddTextField("txtzchs", "正常耗时", Bll.Common.GetdatabyField(jsonData, "col27"), ""));
            container3.Items.Add(CommonExt.AddTextField("txtcszt", "传输状态", Bll.Common.GetdatabyField(jsonData, "col25"), ""));

            container.Items.Add(container1);
            container.Items.Add(container2);
            container.Items.Add(container3);

            tab2.Items.Add(container);
            center.Items.Add(tab2);
            Ext.Net.Panel south = new Ext.Net.Panel();
            south.ID = "SouthPanel";
            //south.Title = "图片信息";
            south.Height = System.Web.UI.WebControls.Unit.Pixel(300);
            south.BodyStyle = "padding:6px;";
            south.Html = GetHtml(Bll.Common.GetdatabyField(jsonData, "col29"), Bll.Common.GetdatabyField(jsonData, "col30"));
            south.AutoScroll = true;

            BorderLayout layout = new BorderLayout();
            layout.South.Split = true;
            // layout.South.Collapsible = true;//去掉收缩框
            layout.Center.Items.Add(center);
            layout.South.Items.Add(south);

            Window win = new Window();
            win.ID = "Window1";
            win.Title = "区间超速车辆详细信息";
            win.Icon = Icon.Application;
            win.Width = System.Web.UI.WebControls.Unit.Pixel(800);
            win.Height = System.Web.UI.WebControls.Unit.Pixel(600);
            win.Plain = true;
            win.Border = false;
            win.BodyBorder = false;
            //win.Collapsible = true;

            win.Buttons.Add(CommonExt.AddButton("butCancel", "退出", "Cancel", win.ClientID + ".hide()"));
            win.Items.Add(layout);

            return win;
        }

        /// <summary>
        ///创建违法车辆图片展示的内容
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static Window AddPeccancyAreaImg(string jsonData)
        {
            Ext.Net.Panel south = new Ext.Net.Panel();
            south.ID = "SouthPanel2";
            south.Height = System.Web.UI.WebControls.Unit.Pixel(780);
            south.BodyStyle = "padding:6px;";
            south.Html = GetHtml(Bll.Common.GetdatabyField(jsonData, "col29"), Bll.Common.GetdatabyField(jsonData, "col30"), 0);
            south.AutoScroll = true;

            BorderLayout layout = new BorderLayout();
            layout.Center.Items.Add(south);

            Window win = new Window();
            win.ID = "Window2";
            win.Title = "区间超速车辆图片信息";
            win.Icon = Icon.Application;
            win.Width = System.Web.UI.WebControls.Unit.Pixel(1000);
            win.Height = System.Web.UI.WebControls.Unit.Pixel(780);
            win.Plain = true;
            win.Border = false;
            win.BodyBorder = false;
            win.Buttons.Add(CommonExt.AddButton("butCancel", "退出", "Cancel", win.ClientID + ".hide()"));
            win.Items.Add(layout);

            return win;
        }

        /// <summary>
        ///组装图片的显示
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        /// <returns></returns>
        protected static string GetHtml(string src1, string src2, int meiyou)
        {
            string Html = "<div class=\"details\">";
            Html = Html + " <tpl for=\".\">";
            Html = Html + "   <center> ";
            Html = Html + "      <img src=\"" + src1 + "\"  width=\"480\" onclick=\"$.openPhotoGalleryXiangqing(this);\" class=\"photo\" />";
            Html = Html + "      <img src=\"" + src2 + "\"  width=\"480\" onclick=\"$.openPhotoGalleryXiangqing(this);\" class=\"photo\" /></center>";
            Html = Html + "</tpl>";
            Html = Html + "</div>";
            return Html;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sdata"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static Window AddLogInfo(string sdata, string filepath)
        {
            Window win = new Window();
            win.ID = "Window1";
            win.Title = "日志详细信息";
            win.Icon = Icon.Application;
            win.Width = System.Web.UI.WebControls.Unit.Pixel(400);
            win.Height = System.Web.UI.WebControls.Unit.Pixel(300);
            win.Plain = true;
            //win.Border = false;
            win.BodyStyle = "padding:6px;";
            //win.BodyBorder = false;
            win.Collapsible = true;
            win.Layout = "Form";
            win.Plain = true;

            string ip = Bll.Common.GetdatabyField(sdata, "col4");
            getIp = new GetIPAdddress(filepath);
            IPLocation ipl = getIp.SearchIPLocation(ip);
            string logip = "[" + ipl.country + ipl.area + "]";
            getIp.FileDispose();
            win.Items.Add(CommonExt.AddTextField("txtID", "记录编号", Bll.Common.GetdatabyField(sdata, "col0")));
            win.Items.Add(CommonExt.AddTextField("txtName", "系统名称", Bll.Common.GetdatabyField(sdata, "col7")));
            win.Items.Add(CommonExt.AddTextField("txtUser", "操作用户", Bll.Common.GetdatabyField(sdata, "col2")));
            win.Items.Add(CommonExt.AddTextField("txtEvent", "操作事件", Bll.Common.GetdatabyField(sdata, "col3")));
            win.Items.Add(CommonExt.AddTextField("txtUserName", "用户", ip + logip));
            win.Items.Add(CommonExt.AddTextField("txtDate", "记录时间", Bll.Common.GetdatabyField(sdata, "col5")));
            win.Buttons.Add(CommonExt.AddButton("butCancel", "退出", "Cancel", win.ClientID + ".hide()"));
            return win;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sdata"></param>
        /// <returns></returns>
        public static Window AddLogErrInfo(string sdata)
        {
            Window win = new Window();
            win.ID = "Window1";
            win.Title = "错误日志详细信息";
            win.Icon = Icon.Application;
            win.Width = System.Web.UI.WebControls.Unit.Pixel(400);
            win.Height = System.Web.UI.WebControls.Unit.Pixel(300);
            win.Plain = true;
            //win.Border = false;
            win.BodyStyle = "padding:6px;";
            //win.BodyBorder = false;
            win.Collapsible = true;
            win.Layout = "Form";
            win.Plain = true;
            win.Items.Add(CommonExt.AddTextField("txtErrSource", "错误源", Bll.Common.GetdatabyField(sdata, "col0")));
            win.Items.Add(CommonExt.AddTextField("txtErrInfo", "错误信息", Bll.Common.GetdatabyField(sdata, "col1")));
            win.Items.Add(CommonExt.AddTextField("txtErrDate", "产生日期", Bll.Common.GetdatabyField(sdata, "col2")));
            win.Items.Add(CommonExt.AddTextField("txtErrDesc", "错误描述", Bll.Common.GetdatabyField(sdata, "col3")));
            win.Buttons.Add(CommonExt.AddButton("butCancel", "退出", "Cancel", win.ClientID + ".hide()"));
            return win;
        }

        /// <summary>
        /// 多语言转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static string GetLangStr(string page, string value, string desc)
        {
            string className = page; //this.GetType().BaseType.FullName;
            return MyNet.Common.Lang.Language.CreateInstance(className).GetLanguageStr(value, desc, className);
        }


    }
}