<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviceDataCount.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.DeviceDataCount" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>城市立体化安全防控平台—智慧态势-设备异常</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="../Styles/index.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/jquery.mCustomScrollbar.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/jquery.dad.css" />

    <style type="text/css">
        #form1 {
            height: 900px !important;
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
</head>
<body class="content">
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="DeviceDataCount" />
        <ext:Hidden ID="CountDay" runat="server" />
        <ext:Viewport runat="server">
            <Items>
                <ext:RowLayout runat="server" Split="true">
                    <Rows>
                        <ext:LayoutRow RowHeight="0.5">
                            <ext:Panel runat="server">
                                <Items>
                                    <ext:ColumnLayout runat="server" Split="true" FitHeight="true">
                                        <Columns>
                                            <ext:LayoutColumn ColumnWidth="0.32">
                                                <ext:Panel ID="Panel2" runat="server" Padding="0" MinHeight="360px">
                                                    <AutoLoad Mode="IFrame" Url="../Template/WebDevicePieData.aspx"></AutoLoad>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth="0.02">
                                                <ext:Panel runat="server"></ext:Panel>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth="0.32">
                                                <ext:Panel ID="Panel3" runat="server" Padding="0" MinHeight="360px">
                                                    <AutoLoad Mode="IFrame" Url="../Template/WebDevicePieData1.aspx"></AutoLoad>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth="0.02">
                                                <ext:Panel runat="server"></ext:Panel>
                                            </ext:LayoutColumn>
                                            <ext:LayoutColumn ColumnWidth="0.32">
                                                <ext:Panel ID="Panel4" runat="server" Padding="0" MinHeight="360px">
                                                    <AutoLoad Mode="IFrame" Url="../Template/WebDevicePieData2.aspx"></AutoLoad>
                                                </ext:Panel>
                                            </ext:LayoutColumn>
                                        </Columns>
                                    </ext:ColumnLayout>
                                </Items>
                            </ext:Panel>
                        </ext:LayoutRow>
                        <ext:LayoutRow RowHeight="0.5">
                            <ext:Panel ID="Panel1" runat="server" RowHeight="1">
                                <AutoLoad Mode="IFrame" Url="../Template/WebDeviceGrid.aspx"></AutoLoad>
                            </ext:Panel>
                        </ext:LayoutRow>
                    </Rows>
                </ext:RowLayout>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>