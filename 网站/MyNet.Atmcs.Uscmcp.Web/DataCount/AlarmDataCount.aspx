<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmDataCount.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.AlarmDataCount" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>城市立体化安全防控平台—智慧态势-报警统计</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="../Styles/jquery.mCustomScrollbar.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/ToolbarStyle.css" />
    <style type="text/css">
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

        #form2 {
            min-height: 750px;
            overflow: hidden;
        }
    </style>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.10.4.min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.mousewheel.min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.mCustomScrollbar.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.dad.min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../build/dist/echarts.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>

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

            Panel1.autoLoad.url = "../Template/WebAlarmData.aspx?datetime=" + date;
            Panel1.reload();
            Panel2.autoLoad.url = "../Template/WebAlarmPieData.aspx?datetime=" + date;
            Panel2.reload();
            AlarmDataCount.GetDangqianTime(date);
        }
    </script>
    <script type="text/javascript">
        function chooseNow() {
            var now = new Date();
            var year = now.getFullYear();
            var month = ("" + (now.getMonth() + 101)).substr(1);
            var day = ("" + (now.getDate() + 100)).substr(1);
            var date = year + "-" + month + "-" + day;
            chooseDate(date);
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
    <form id="form2" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="AlarmDataCount" />
        <ext:Hidden ID="CountDay" runat="server" />
        <ext:Viewport runat="server">
            <Items>
                <ext:RowLayout runat="server" Split="true">
                    <Rows>
                        <ext:LayoutRow>
                            <ext:Toolbar ID="Toolbar1" runat="server" Cls="ToolbarStyle">
                                <Items>
                                    <ext:Button runat="server" Text='<%# GetLangStr("AlarmDataCount1","今日报警总量")%>' ID="Button4">
                                        <Listeners>
                                            <Click Handler="chooseNow(); " />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Image runat="server" ImageUrl="../Images/Car/carAlarm.png"></ext:Image>
                                    <ext:Label runat="server" ID="lblDayCount"></ext:Label>
                                    <ext:ToolbarSeparator></ext:ToolbarSeparator>
                                    <ext:Button runat="server" Text='<%# GetLangStr("AlarmDataCount2","昨天")%>' ID="Button1">
                                        <Listeners>
                                            <Click Handler="chooseLastDay(); " />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Image runat="server" ImageUrl="../Images/Car/carAlarm.png"></ext:Image>
                                    <ext:Label runat="server" ID="lblLastDay"></ext:Label>
                                    <ext:ToolbarSeparator></ext:ToolbarSeparator>
                                    <ext:Label runat="server" Text='<%# GetLangStr("AlarmDataCount3","本周") %>' StyleSpec="margin-left: 10px;" ID="Button2">
                                    </ext:Label>
                                    <ext:Image runat="server" ImageUrl="../Images/Car/carAlarm.png"></ext:Image>
                                    <ext:Label runat="server" ID="lblLastWeek"></ext:Label>
                                    <ext:ToolbarSeparator></ext:ToolbarSeparator>
                                    <ext:Label runat="server" Text='<%# GetLangStr("AlarmDataCount4","本月") %>' StyleSpec="margin-left: 10px;" ID="Button3">
                                    </ext:Label>
                                    <ext:Image runat="server" ImageUrl="../Images/Car/carAlarm.png"></ext:Image>
                                    <ext:Label runat="server" ID="lblLastMonth"></ext:Label>
                                    <ext:ToolbarSeparator></ext:ToolbarSeparator>
                                    <ext:Label runat="server" ID="Label1">
                                        <Content><a href="#" id="TimeSelect" onclick="laydate({ elem: '#TimeSelect',format: 'YYYY-MM-DD',festival: 'true',choose: function (datas){chooseDate(datas);}});"><%# GetLangStr("AlarmDataCount5","自定义时间")%></a> </Content>
                                    </ext:Label>
                                    <ext:Image ID="Image1" runat="server" Hidden="true" ImageUrl="../Images/Car/carAlarm.png"></ext:Image>
                                    <ext:Label runat="server" ID="lbXzDayAlarmCount" Hidden="true"></ext:Label>
                                </Items>
                            </ext:Toolbar>
                        </ext:LayoutRow>
                        <ext:LayoutRow RowHeight="0.5">
                            <ext:Panel ID="Panel1" runat="server">
                                <AutoLoad Mode="IFrame"></AutoLoad>
                            </ext:Panel>
                        </ext:LayoutRow>
                        <ext:LayoutRow RowHeight="0.4">
                            <ext:Panel ID="Panel2" runat="server">
                                <AutoLoad Mode="IFrame"></AutoLoad>
                            </ext:Panel>
                        </ext:LayoutRow>
                    </Rows>
                </ext:RowLayout>
            </Items>
        </ext:Viewport>
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