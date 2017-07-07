<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CurrentDataCount.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.CurrentDataCount" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>城市立体化安全防控平台—智慧态势-实时车流</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="../Styles/index.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/jquery.mCustomScrollbar.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/jquery.dad.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/ToolbarStyle.css" />
    <style type="text/css">
        #map_canvas {
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            position: absolute;
        }

        .x-btn {
            border: 0px;
        }

        #Label1_Content:hover {
            background: #fb5004;
            border-radius: 30px;
        }

            #Label1_Content:hover a {
                color: white;
            }

        #form1 {
            height: 900px !important;
            overflow: hidden;
        }

        #Panel5 IFrame {
            min-width: 1150px !important;
        }

        #Panel1 {
            height: 350px !important;
        }

        #Panel5 {
            height: 110px !important;
        }
    </style>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.10.4.min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.mousewheel.min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.mCustomScrollbar.js " charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.dad.min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../build/dist/echarts.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/bmapFile.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/bmap.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/Heatmap_min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/common.js" charset="UTF-8"></script>
    <script type="text/javascript">
        function MapCenter() {
            BMAP.MapInit();
            setTimeout(function () {
                BMAP.GotoCenter();
            }, 500);
        }
    </script>
    <script type="text/javascript">
        function WindowShow(type, datetime) {
            OpenPassAnalysisPage("../DataCount/WinPassAnalysis.aspx?datetime=" + datetime + "&type=" + type);
        }
    </script>
    <script type='text/javascript'>
        (function ($) {
            $(window).load(function () {
                $(".content").mCustomScrollbar();
            });
        })(jQuery);
    </script>

    <!--[if lt IE 10]>
        <script type="text/javascript" src="js/PIE.js"></script>
        <![endif]-->

    <script type="text/javascript">
        function chooseDate(date) {
            Panel1.autoLoad.url = "../Template/WebFlowData.aspx?datetime=" + date;
            Panel1.reload();
            Panel5.autoLoad.url = "../Template/WebPassCarBarData.aspx?datetime=" + date;
            Panel5.reload();
            Panel2.autoLoad.url = "../Template/WebPassCarType.aspx?datetime=" + date;
            Panel2.reload();
            Panel3.autoLoad.url = "../Template/WebPassCarVehHead.aspx?datetime=" + date;
            Panel3.reload();
            Panel4.autoLoad.url = "../Template/WebPassCarJLLX.aspx?datetime=" + date;
            Panel4.reload();
            CurrentDataCount.GetDangqianTime(date);
        }
    </script>
    <script type="text/javascript">
        function chooseNow() {
            $("#TimeSelect").html("自定义时间");
            var now = new Date();
            var year = now.getFullYear();
            var month = ("" + (now.getMonth() + 101)).substr(1);
            var day = ("" + (now.getDate() + 100)).substr(1);
            var date = year + "-" + month + "-" + day;
            chooseDate(date);
            // CurrentDataCount.GetDangqianTime();
        }
    </script>
    <script type="text/javascript">
        function chooseLastDay() {
            var now = new Date();
            var year = now.getFullYear();
            var month = ("" + (now.getMonth() + 101)).substr(1);
            var day = ("" + (now.getDate() + 99)).substr(1);
            var date = year + "-" + month + "-" + day;
            chooseDate(date);
        }
    </script>
</head>
<body class="content">
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="CurrentDataCount" />
        <ext:Hidden ID="CountDay" runat="server" />
        <ext:Viewport runat="server">
            <Items>
                <ext:RowLayout runat="server" Split="true">
                    <Rows>
                        <ext:LayoutRow>
                            <ext:Toolbar ID="Toolbar1" runat="server" Cls="ToolbarStyle">
                                <Items>
                                    <ext:Button runat="server" Text='<%# GetLangStr("CurrentDataCount1","今日监测总量")%>' ID="Button4">
                                        <Listeners>
                                            <Click Handler="chooseNow(); " />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Image runat="server" ImageUrl="../Images/Car/car16.png"></ext:Image>
                                    <ext:Label runat="server" ID="lblDayCount"></ext:Label>
                                    <ext:DisplayField runat="server" Text="("></ext:DisplayField>
                                    <ext:Image runat="server" ImageUrl="../Images/Car/online.png"></ext:Image>
                                    <ext:Label runat="server" ID="lblDayOnline"></ext:Label>
                                    <ext:DisplayField runat="server" Text=")"></ext:DisplayField>
                                    <ext:ToolbarSeparator></ext:ToolbarSeparator>
                                    <ext:Button runat="server" Text='<%# GetLangStr("CurrentDataCount4","昨天")%>' ID="Button1">
                                        <Listeners>
                                            <Click Handler="chooseLastDay(); " />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Image runat="server" ImageUrl="../Images/Car/car16.png"></ext:Image>
                                    <ext:Label runat="server" ID="lblLastDay"></ext:Label>
                                    <ext:DisplayField runat="server" Text="("></ext:DisplayField>
                                    <ext:Image runat="server" ImageUrl="../Images/Car/online.png"></ext:Image>
                                    <ext:Label runat="server" ID="lblLastDayOnline"></ext:Label>
                                    <ext:DisplayField runat="server" Text=")"></ext:DisplayField>
                                    <ext:ToolbarSeparator></ext:ToolbarSeparator>
                                    <ext:Label runat="server" Text='<%# GetLangStr("CurrentDataCount7","本周")%>' ID="Button2">
                                    </ext:Label>
                                    <ext:Image runat="server" ImageUrl="../Images/Car/car16.png"></ext:Image>
                                    <ext:Label runat="server" ID="lblLastWeek"></ext:Label>
                                    <ext:ToolbarSeparator></ext:ToolbarSeparator>
                                    <ext:Label runat="server" Text='<%# GetLangStr("CurrentDataCount8","本月")%>' ID="Button3">
                                    </ext:Label>
                                    <ext:Image runat="server" ImageUrl="../Images/Car/car16.png"></ext:Image>
                                    <ext:Label runat="server" ID="lblLastMonth"></ext:Label>
                                    <ext:ToolbarSeparator></ext:ToolbarSeparator>
                                    <ext:Label runat="server" ID="Label1">
                                        <Content><a id="TimeSelect" onclick="laydate({ elem: '#TimeSelect',format: 'YYYY-MM-DD', festival: 'true',choose: function (datas){chooseDate(datas);}});"><%# GetLangStr("CurrentDataCount15","自定义时间")%></a> </Content>
                                    </ext:Label>

                                    <ext:Image ID="Image1" runat="server" Hidden="true" ImageUrl="../Images/Car/car16.png"></ext:Image>
                                    <ext:Label runat="server" ID="lbXzDayCount" Hidden="true"></ext:Label>
                                    <ext:DisplayField ID="DisFiled1" runat="server" Hidden="true" Text='<%# GetLangStr("CurrentDataCount2","(")%>'></ext:DisplayField>
                                    <ext:Image ID="Image2" runat="server" Hidden="true" ImageUrl="../Images/Car/online.png"></ext:Image>
                                    <ext:Label runat="server" Hidden="true" ID="lbXzDayOnline"></ext:Label>
                                    <ext:DisplayField ID="DisFiled2" runat="server" Hidden="true" Text='<%# GetLangStr("CurrentDataCount3",")")%>'></ext:DisplayField>
                                </Items>
                            </ext:Toolbar>
                        </ext:LayoutRow>
                        <ext:LayoutRow RowHeight="0.4">
                            <ext:Panel ID="Panel1" runat="server">
                                <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
                            </ext:Panel>
                        </ext:LayoutRow>
                        <ext:LayoutRow RowHeight="0.1">
                            <ext:Panel ID="Panel5" runat="server">
                                <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
                            </ext:Panel>
                        </ext:LayoutRow>
                        <ext:LayoutRow RowHeight="0.3">
                            <ext:Panel runat="server">
                                <Items>
                                    <ext:ColumnLayout runat="server" Split="true" FitHeight="true">
                                        <Columns>
                                            <ext:LayoutColumn ColumnWidth="0.32">
                                                <ext:Panel ID="Panel2" runat="server" Padding="0">
                                                    <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth="0.02">
                                                <ext:Panel runat="server"></ext:Panel>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth="0.32">
                                                <ext:Panel ID="Panel3" runat="server" Padding="0">
                                                    <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth="0.02">
                                                <ext:Panel runat="server"></ext:Panel>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth="0.32">
                                                <ext:Panel ID="Panel4" runat="server" Padding="0">
                                                    <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                        </Columns>
                                    </ext:ColumnLayout>
                                </Items>
                            </ext:Panel>
                        </ext:LayoutRow>
                    </Rows>
                </ext:RowLayout>
            </Items>
        </ext:Viewport>
        <ext:Window ID="Window4" runat="server" Icon="House" Hidden="true" Height="800px"
            Width="1024" Title='<%# GetLangStr("CurrentDataCount9","详细信息")%>' Layout="FitLayout">
            <Items>
                <ext:TabPanel runat="server" Width="1000" ID="TabPanel1" ActiveTabIndex="0">
                    <Items>
                        <ext:Panel ID="Tab2" runat="server" Title='<%# GetLangStr("CurrentDataCount10","综合图表分析")%>' Padding="6">
                            <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
                        </ext:Panel>
                        <ext:Panel ID="Tab1" runat="server" Title='<%# GetLangStr("CurrentDataCount11","车辆热力分布")%>' Padding="6" Hidden="true">
                            <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
                        </ext:Panel>
                    </Items>
                </ext:TabPanel>
            </Items>
            <Buttons>
                <ext:Button ID="Button6" runat="server" Icon="Cancel" Text='<%# GetLangStr("CurrentDataCount12","退出")%>'>
                    <Listeners>
                        <Click Handler="#{Window4}.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>
    </form>
</body>
<script type="text/javascript">
    $("body").delegate("#laydate_clear", "click", function () {

        var aTxt = "自定义时间";
        $("#TimeSelect").html(aTxt);
    });

    $("body").click(function () {
        if ($("#TimeSelect").html() == "") {
            var aTxt = "自定义时间";
            $("#TimeSelect").html(aTxt);
        }
    })
</script>
</html>