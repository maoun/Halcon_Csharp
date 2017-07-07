<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataTemplate.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.DataTemplate" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <meta http-equiv="Content-Type" content="text/html" charset="GBK"/>
    <link rel="stylesheet" type="text/css" href="../Styles/index.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/jquery.mCustomScrollbar.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/jquery.dad.css" />
    <style type="text/css">
        .ToolbarStyle {
            background-color: transparent;
            border-bottom-width: 0px;
        }

            .ToolbarStyle button {
                color: transparent;
            }

        #form1 {
            height: 930px !important;
            overflow: hidden;
        }
    </style>
    <script type="text/javascript" src="../Scripts/jquery-1.8.0.js" charset="UTF-8">></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.10.4.min.js" charset="UTF-8">></script>
    <script type="text/javascript" src="../Scripts/jquery.mousewheel.min.js" charset="UTF-8">></script>
    <script type="text/javascript" src="../Scripts/jquery.mCustomScrollbar.js" charset="UTF-8">></script>
    <script type="text/javascript" language="javascript">

        function ShowWindow(panel, type) {

            ButData.setValue(panel);
            Window1.autoLoad.url = "SelectPage.aspx?type=" + type;
            Window1.reload();
            Window1.show();
        }
        function AutoLoadPanel(panelname, url) {
            switch (panelname) {
                case "DataTemplate1":
                    DataTemplate1.autoLoad.url = url;
                    DataTemplate1.reload();
                    break;
                case "DataTemplate2":
                    DataTemplate2.autoLoad.url = url;
                    DataTemplate2.reload();
                    break;
                case "DataTemplate3":
                    DataTemplate3.autoLoad.url = url;
                    DataTemplate3.reload();
                    break;
                case "DataTemplate4":
                    DataTemplate4.autoLoad.url = url;
                    DataTemplate4.reload();
                    break;
            }
        }
    </script>
    <script type='text/javascript'>
        (function ($) {
            $(window).load(function () {
                $(".content").mCustomScrollbar();
            });
        })(jQuery);
    </script>
</head>
<body class="content">
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="DataTemplate" />
        <ext:Hidden ID="ButData" runat="server" />
        <ext:Viewport runat="server" Layout="Fit">
            <Items>
                <ext:FormPanel runat="server" Padding="10" Border="false">
                    <Items>
                        <ext:RowLayout runat="server" Split="true">
                            <Rows>
                                <ext:LayoutRow RowHeight="0.5">
                                    <ext:Panel ID="Panel1" runat="server" Title="" RowHeight="1">
                                        <Items>
                                            <ext:ColumnLayout runat="server" Split="true" FitHeight="true">
                                                <Columns>
                                                    <ext:LayoutColumn ColumnWidth="0.33">
                                                        <ext:Panel runat="server">
                                                            <Items>
                                                                <ext:RowLayout runat="server" FitHeight="true" RowHeight="1">
                                                                    <Rows>
                                                                        <ext:LayoutRow>
                                                                            <ext:Panel runat="server" Height="30" Border="false">
                                                                                <TopBar>
                                                                                    <ext:Toolbar ID="Toolbar1" runat="server" Cls="ToolbarStyle" Flat="true">
                                                                                        <Items>
                                                                                            <ext:Button runat="server" Text="编辑" ID="Button4">
                                                                                                <Listeners>
                                                                                                    <Click Handler="ShowWindow('DataTemplate1','2'); " />
                                                                                                </Listeners>
                                                                                            </ext:Button>
                                                                                        </Items>
                                                                                    </ext:Toolbar>
                                                                                </TopBar>
                                                                            </ext:Panel>
                                                                        </ext:LayoutRow>
                                                                        <ext:LayoutRow RowHeight="1">
                                                                            <ext:Panel runat="server" Border="false" ID="DataTemplate1">
                                                                                <AutoLoad Mode="IFrame"></AutoLoad>
                                                                            </ext:Panel>
                                                                        </ext:LayoutRow>
                                                                    </Rows>
                                                                </ext:RowLayout>
                                                            </Items>
                                                        </ext:Panel>
                                                    </ext:LayoutColumn>
                                                    <ext:LayoutColumn ColumnWidth="0.33">
                                                        <ext:Panel runat="server">
                                                            <Items>
                                                                <ext:RowLayout runat="server" FitHeight="true">
                                                                    <Rows>
                                                                        <ext:LayoutRow>
                                                                            <ext:Panel runat="server" Height="30" Border="false">
                                                                                <TopBar>
                                                                                    <ext:Toolbar ID="Toolbar2" runat="server" Cls="ToolbarStyle" Flat="true">
                                                                                        <Items>
                                                                                            <ext:Button runat="server" Text="编辑" ID="Button1">
                                                                                                <Listeners>
                                                                                                    <Click Handler="ShowWindow('DataTemplate2','2'); " />
                                                                                                </Listeners>
                                                                                            </ext:Button>
                                                                                        </Items>
                                                                                    </ext:Toolbar>
                                                                                </TopBar>
                                                                            </ext:Panel>
                                                                        </ext:LayoutRow>
                                                                        <ext:LayoutRow RowHeight="1">
                                                                            <ext:Panel runat="server" Border="false" ID="DataTemplate2">
                                                                                <AutoLoad Mode="IFrame"></AutoLoad>
                                                                            </ext:Panel>
                                                                        </ext:LayoutRow>
                                                                    </Rows>
                                                                </ext:RowLayout>
                                                            </Items>
                                                        </ext:Panel>
                                                    </ext:LayoutColumn>
                                                    <ext:LayoutColumn ColumnWidth="0.33">
                                                        <ext:Panel runat="server">
                                                            <Items>
                                                                <ext:RowLayout runat="server" FitHeight="true">
                                                                    <Rows>
                                                                        <ext:LayoutRow>
                                                                            <ext:Panel runat="server" Height="30" Border="false">
                                                                                <TopBar>
                                                                                    <ext:Toolbar ID="Toolbar3" runat="server" Cls="ToolbarStyle" Flat="true">
                                                                                        <Items>
                                                                                            <ext:Button runat="server" Text="编辑" ID="Button2">
                                                                                                <Listeners>
                                                                                                    <Click Handler="ShowWindow('DataTemplate3','2'); " />
                                                                                                </Listeners>
                                                                                            </ext:Button>
                                                                                        </Items>
                                                                                    </ext:Toolbar>
                                                                                </TopBar>
                                                                            </ext:Panel>
                                                                        </ext:LayoutRow>
                                                                        <ext:LayoutRow RowHeight="1">
                                                                            <ext:Panel runat="server" Border="false" ID="DataTemplate3">
                                                                                <AutoLoad Mode="IFrame"></AutoLoad>
                                                                            </ext:Panel>
                                                                        </ext:LayoutRow>
                                                                    </Rows>
                                                                </ext:RowLayout>
                                                            </Items>
                                                        </ext:Panel>
                                                    </ext:LayoutColumn>
                                                     <ext:LayoutColumn ColumnWidth="0.01">
                                                        <ext:Panel runat="server"></ext:Panel>
                                                    </ext:LayoutColumn>
                                                </Columns>
                                            </ext:ColumnLayout>
                                        </Items>
                                    </ext:Panel>
                                </ext:LayoutRow>
                                <ext:LayoutRow RowHeight="0.5">
                                    <ext:Panel runat="server">
                                        <Items>
                                            <ext:ColumnLayout runat="server" FitHeight="true">
                                                <Columns>
                                                    <ext:LayoutColumn>
                                                        <ext:Panel runat="server" Width="51" Border="false">
                                                            <TopBar>
                                                                <ext:Toolbar ID="Toolbar4" runat="server" Cls="ToolbarStyle" Layout="Container" Flat="true">
                                                                    <Items>
                                                                        <ext:Button runat="server" Text="编辑" ID="Button3">
                                                                            <Listeners>
                                                                                <Click Handler="ShowWindow('DataTemplate4','1'); " />
                                                                            </Listeners>
                                                                        </ext:Button>
                                                                    </Items>
                                                                </ext:Toolbar>
                                                            </TopBar>
                                                        </ext:Panel>
                                                    </ext:LayoutColumn>
                                                    <ext:LayoutColumn ColumnWidth="1">
                                                        <ext:Panel runat="server" Border="false" ID="DataTemplate4">
                                                            <AutoLoad Mode="IFrame"></AutoLoad>
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
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
        <ext:Window ID="Window1" runat="server" Title="选择显示的页" Icon="Application" Height="700px" Width="1024" Padding="5" Modal="true" Hidden="true">
            <AutoLoad Mode="IFrame" ShowMask="true"></AutoLoad>
            <TopBar>
                <ext:Toolbar ID="Toolbar5" runat="server">
                    <Items>
                        <ext:Label runat="server" Text="设为首页："></ext:Label>
                        <ext:RadioGroup
                            ID="RadioGroupTemplate"
                            runat="server"
                            ColumnsNumber="4" Width="440">
                            <Items>
                                <ext:Radio ID="radData" runat="server" BoxLabel="数据类模板" InputValue="DataTemplate" Checked="true" Width="100px" />
                                <ext:Radio ID="radVideo" runat="server" BoxLabel="视频类模板" InputValue="VideoTemplate" Width="100px" />
                                <ext:Radio ID="radList" runat="server" BoxLabel="列表类模板" InputValue="ListTemplate" Width="100px" />
                                <ext:Radio ID="radUser" runat="server" BoxLabel="自定义类模板" InputValue="UserTemplate" Width="120px" />
                            </Items>
                            <DirectEvents>
                                <Change OnEvent="TemplateGroup_Event"></Change>
                            </DirectEvents>
                        </ext:RadioGroup>
                        <ext:ToolbarFill runat="server"></ext:ToolbarFill>
                        <ext:Button runat="server" ID="OkBtn" Text="确定" Icon="Accept">
                            <DirectEvents>
                                <Click OnEvent="ButOKClick" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button runat="server" ID="CancelBtn" Text="取消" Icon="Cancel">
                            <Listeners>
                                <Click Handler="Window1.hide()" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
        </ext:Window>
    </form>
</body>
</html>