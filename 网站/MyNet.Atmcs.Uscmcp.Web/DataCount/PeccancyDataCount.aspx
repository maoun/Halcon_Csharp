<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeccancyDataCount.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.PeccancyDataCount" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>城市立体化安全防控平台—智慧态势-违法分布</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="../Styles/index.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/jquery.mCustomScrollbar.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/jquery.dad.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/ToolbarStyle.css" />

    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.10.4.min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.mousewheel.min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.mCustomScrollbar.js" charset="UTF-8"></script>

    <script type="text/javascript" src="../build/dist/echarts.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
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

        #form1 {
            height: 880px !important;
            overflow: hidden;
        }

        #Panel2 IFrame {
            min-width: 1150px !important;
        }

        #Panel6 {
            min-width: 1150px !important;
        }

        #Panel1 {
            height: 350px !important;
        }

        #Panel5 {
            height: 110px !important;
        }
    </style>
    <!--[if lt IE 10]>
        <script type="text/javascript" src="js/PIE.js"></script>
        <![endif]-->
    <script type="text/javascript">
        function chooseDate(date) {
            Panel1.autoLoad.url = "../Template/WebPeccancyData.aspx?datetime=" + date;;
            Panel1.reload();
            Panel2.autoLoad.url = "../Template/WebPeccancyBarData.aspx?datetime=" + date;;
            Panel2.reload();
            Panel3.autoLoad.url = "../Template/WebPeccancyGrid.aspx?datetime=" + date;;
            Panel3.reload();
            PeccancyDataCount.GetDangqianTime(date);
        }
    </script>

    <script type='text/javascript'>
        (function ($) {
            $(window).load(function () {

                $(".content").mCustomScrollbar();
            });
        })(jQuery);
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
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="PeccancyDataCount" />
        <ext:Viewport runat="server">
            <Items>
                <ext:RowLayout runat="server" Split="true">
                    <Rows>
                        <ext:LayoutRow>
                            <ext:Toolbar ID="Toolbar1" runat="server" Cls="ToolbarStyle">
                                <Items>
                                    <ext:Button runat="server" Text='<%# GetLangStr("PeccancyDataCount1","今日违法总量")%>' ID="Button4">
                                        <Listeners>
                                            <Click Handler="chooseNow(); " />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Image runat="server" ImageUrl="../Images/Car/cardel.png"></ext:Image>
                                    <ext:Label runat="server" ID="lblDayCount"></ext:Label>
                                    <ext:ToolbarSeparator></ext:ToolbarSeparator>
                                    <ext:Button runat="server" Text='<%# GetLangStr("PeccancyDataCount2","昨天")%>' ID="Button1">
                                        <Listeners>
                                            <Click Handler="chooseLastDay(); " />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Image runat="server" ImageUrl="../Images/Car/cardel.png"></ext:Image>
                                    <ext:Label runat="server" ID="lblLastDay"></ext:Label>
                                    <ext:ToolbarSeparator></ext:ToolbarSeparator>
                                    <ext:Label runat="server" Text='<%# GetLangStr("PeccancyDataCount3","本周") %>' StyleSpec="margin-left: 10px;" ID="Button2">
                                    </ext:Label>
                                    <ext:Image runat="server" ImageUrl="../Images/Car/cardel.png"></ext:Image>
                                    <ext:Label runat="server" ID="lblLastWeek"></ext:Label>
                                    <ext:ToolbarSeparator></ext:ToolbarSeparator>
                                    <ext:Label runat="server" Text='<%# GetLangStr("PeccancyDataCount4","本月") %>' StyleSpec="margin-left: 10px;" ID="Button3">
                                    </ext:Label>
                                    <ext:Image runat="server" ImageUrl="../Images/Car/cardel.png"></ext:Image>
                                    <ext:Label runat="server" ID="lblLastMonth"></ext:Label>
                                    <ext:ToolbarSeparator></ext:ToolbarSeparator>
                                    <ext:Label runat="server" ID="Label1">
                                        <Content><a href="#" id="TimeSelect" onclick="laydate({ elem: '#TimeSelect',format: 'YYYY-MM-DD',festival: 'true',choose: function (datas){chooseDate(datas);}});"><%# GetLangStr("PeccancyDataCount5","自定义时间") %></a> </Content>
                                    </ext:Label>
                                    <ext:Image ID="Image1" runat="server" Hidden="true" ImageUrl="../Images/Car/cardel.png"></ext:Image>
                                    <ext:Label runat="server" ID="lbXzDayPeccCount" Hidden="true"></ext:Label>
                                </Items>
                            </ext:Toolbar>
                        </ext:LayoutRow>
                        <ext:LayoutRow RowHeight="0.45">
                            <ext:Panel ID="Panel1" runat="server">
                                <AutoLoad Mode="IFrame"></AutoLoad>
                            </ext:Panel>
                        </ext:LayoutRow>
                        <ext:LayoutRow RowHeight="0.10">
                            <ext:Panel ID="Panel2" runat="server">
                                <AutoLoad Mode="IFrame"></AutoLoad>
                            </ext:Panel>
                        </ext:LayoutRow>
                        <ext:LayoutRow RowHeight="0.45">
                            <ext:Panel ID="Panel3" runat="server">
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