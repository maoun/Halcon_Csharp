<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarHot.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Map.CarHot" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>热点</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="css/custom.css" />
    <link rel="stylesheet" type="text/css" href="../Style/customMap.css" />
    <style type="text/css">
        body, html {
            font-family: Arial,Verdana;
            font-size: 13px;
            margin: 0;
            overflow: hidden;
        }

        #cboplate_Panel1 table {
            border-radius: 0;
        }

        #map_canvas {
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            position: absolute;
        }
    </style>
    <script type="text/javascript" src="../Scripts/bmapFile.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/bmap.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/Heatmap_min.js" charset="UTF-8"></script>
    <script type="text/javascript" language="javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <script type="text/javascript">
        function MapCenter() {
            BMAP.MapInit();
            setTimeout(function () {
                BMAP.GotoCenter();
            }, 500);

            //判断浏览区是否支持canvas
            function isSupportCanvas() {
                var elem = document.createElement('canvas');
                return !!(elem.getContext && elem.getContext('2d'));
            }

        }
    </script>
    <script type="text/javascript">
        function isScroll() {
            $('.OverCar-data-list').niceScroll({
                cursorcolor: "#7683a4",//#CC0071 光标颜色
                cursoropacitymax: 1, //改变不透明度非常光标处于活动状态（scrollabar“可见”状态），范围从1到0
                touchbehavior: false, //使光标拖动滚动像在台式电脑触摸设备
                cursorwidth: "3px", //像素光标的宽度
                cursorborder: "0", // 	游标边框css定义
                cursorborderradius: "5px",//以像素为光标边界半径
                autohidemode: true //是否隐藏滚动条
            });
        }
        function isScrollRight() {
            $('.x-grid3-scroller').niceScroll({
                cursorcolor: "#7683a4",//#CC0071 光标颜色
                cursoropacitymax: 1, //改变不透明度非常光标处于活动状态（scrollabar“可见”状态），范围从1到0
                touchbehavior: false, //使光标拖动滚动像在台式电脑触摸设备
                cursorwidth: "3px", //像素光标的宽度
                cursorborder: "0", // 	游标边框css定义
                cursorborderradius: "5px",//以像素为光标边界半径
                autohidemode: false //是否隐藏滚动条
            });
        }
    </script>
    <script type="text/javascript">
        var DataState = function (value) {
            var color = "green-bg"
            if (value < 30) {
                color = "green-bg";
            }
            if (value > 30) {
                color = "yellow-bg";
            }
            if (value > 70) {
                color = "orange_bg";
            }
            return " <ul class=\"progress-bar\"><div class=\"pro-bar\"><span class=\"pro-bg\"><i class=\"pro-step " + color + "\" style=\"width:" + value + "%;\"></i></span></div></ul>";
        };

        var ShowBfb = function (value) {
            return value + "%";
        };
    </script>
    <script type="text/javascript">
        function chooseDate(date) {
            CarHot.btnchange(date);
        }
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridStation.view.findRowIndex(this.triggerElement),
                cellIndex = GridStation.view.findCellIndex(this.triggerElement),
                record = StoreStation.getAt(rowIndex),
                fieldName = GridStation.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
    <!--梁引入如下js和css-->
    <script type="text/javascript" src="js/Qquery1.91-min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="js/jquery.nicescroll.js" charset="UTF-8"></script>
    <link rel="stylesheet" type="text/css" href="css/Ui-skin.css" />
    <!--梁引入如下js和css 结束-->
</head>
<body onload="MapCenter();">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="CarHot" />

    <form id="form2" runat="server">
        <ext:Hidden runat="server" ID="hidSelectDate"></ext:Hidden>
        <ext:Store ID="cllx" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Department" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="DEPARTID" />
                        <ext:RecordField Name="DEPARTNAME" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="Viewport1" runat="server" Layout="border" Cls="new-layout">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Title='<%# GetLangStr("CarHot1","地图浏览")%>' AutoRender="true" Region="Center"
                    Header="false" Height="800" Width="1000" Cls="map-bg">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar12" runat="server" Cls="top-toolbar">
                            <Items>
                                <ext:Button ID="Linkreload" runat="server" Icon="Reload" Text='<%# GetLangStr("CarHot2","重载地图")%>'>
                                    <Listeners>
                                        <Click Handler="window.location.reload();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Button ID="TbutMove" runat="server" Icon="Erase" Text='<%# GetLangStr("CarHot3","清除")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.ClearCircle();BMAP.ClearLabel();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButCenetr" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("CarHot4","中心点")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.GotoCenter();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButRanging" runat="server" Icon="PencilGo" Text='<%# GetLangStr("CarHot5","测距")%>'>
                                    <Listeners>
                                        <Click Handler=" BMAP.DistanceTool();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButPlan" runat="server" Icon="Vector" Text='<%# GetLangStr("CarHot6","测面积")%>'>
                                    <Listeners>
                                        <Click Handler=" BMAP.CalculateArea();" />
                                    </Listeners>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Content>
                        <div style="width: 100%; height: 100%; border: 1px solid gray" id="map_canvas">
                        </div>
                    </Content>
                </ext:Panel>
                <ext:FormPanel ID="FormPanel2" Cls="ui-right-wrap Middle-arrow" Padding="5" Region="east" runat="server"
                    Split="true" Collapsible="true" Width="400" Layout="FitLayout">
                    <TopBar>
                        <ext:Toolbar runat="server">
                            <Items>
                                <ext:Button ID="Button1" runat="server" IconCls="ui-input w-100px search-btn border-radius-30" Icon="BulletPicture" Text="今天">
                                    <Listeners>
                                        <Click Handler="CarHot.btntoday();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="Button2" runat="server" IconCls="ui-input w-100px search-btn border-radius-30" Icon="BulletPicture" Text="昨天">
                                    <Listeners>
                                        <Click Handler="CarHot.btnyestoday();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarSeparator />
                                <ext:Label runat="server" ID="Label1">
                                    <Content><a href="#" id="TimeSelect" style="font-size: 15px;  border-radius: 15px;color:black" onclick="laydate({festival: 'true',choose: function (datas){chooseDate(datas);}});">自定义时间</a> </Content>
                                </ext:Label>
                                <ext:ToolbarFill runat="server" />
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:TabPanel
                            ID="TabPanel1"
                            runat="server"
                            ActiveTabIndex="0"
                            Width="390"
                            Plain="true">
                            <Items>
                                <ext:Panel ID="Tab1" runat="server" Title='<%# GetLangStr("CarHot7","卡口")%>' Padding="6"
                                    AutoHeight="false" Layout="FitLayout">
                                    <Items>
                                        <ext:GridPanel ID="GridStation" runat="server" StripeRows="true" Layout="FitLayout" AutoScroll="true" MinColumnWidth="110">
                                            <Store>
                                                <ext:Store ID="StoreStation" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader>
                                                            <Fields>
                                                                <ext:RecordField Name="kkmc" />
                                                                <ext:RecordField Name="zs" />
                                                                <ext:RecordField Name="gwbl" />
                                                                <ext:RecordField Name="gwbl1" />
                                                                <ext:RecordField Name="xpoint" />
                                                                <ext:RecordField Name="ypoint" />
                                                                <ext:RecordField Name="kkid" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                            <TopBar>
                                            </TopBar>
                                            <ColumnModel ID="ColumnModel1" runat="server">
                                                <Columns>
                                                    <ext:RowNumbererColumn Header="序号" Width="40"></ext:RowNumbererColumn>
                                                    <ext:Column AutoDataBind="true" Width="150" Fixed="true" DataIndex="kkmc" Resizable="false" Header="卡口名称">
                                                    </ext:Column>
                                                    <ext:Column AutoDataBind="true" Width="180" Align="Center" Sortable="true" EmptyGroupText="0" DataIndex="gwbl">
                                                        <Renderer Fn="DataState" />
                                                    </ext:Column>
                                                    <ext:Column AutoDataBind="true" Width="120" Align="Center" Sortable="true" EmptyGroupText="0" DataIndex="gwbl">
                                                        <Renderer Fn="ShowBfb" />
                                                    </ext:Column>
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:RowSelectionModel runat="server" SingleSelect="true">
                                                    <Listeners>
                                                        <%--<RowSelect Handler="CarHot.SelectRowStation(record.data.xpoint,record.data.ypoint,record.data.kkid,record.data.kkmc)" />--%>
                                                    </Listeners>
                                                </ext:RowSelectionModel>
                                            </SelectionModel>
                                            <View>
                                                <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                                                </ext:GridView>
                                            </View>
                                            <ToolTips>
                                                <ext:ToolTip
                                                    ID="RowTip"
                                                    runat="server"
                                                    Target="={GridStation.getView().mainBody}"
                                                    Delegate=".x-grid3-cell"
                                                    TrackMouse="true">
                                                    <Listeners>
                                                        <Show Fn="showTip" />
                                                    </Listeners>
                                                </ext:ToolTip>
                                            </ToolTips>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel
                                    ID="Tab2"
                                    runat="server"
                                    Title='<%# GetLangStr("CarHot8","道路")%>'
                                    Padding="6"
                                    AutoHeight="false" Layout="FitLayout">
                                    <Items>
                                        <ext:GridPanel ID="GridRoad" runat="server" StripeRows="true" Layout="FitLayout" AutoScroll="true" MinColumnWidth="110">
                                            <Store>
                                                <ext:Store ID="StoreRoad" runat="server">
                                                    <AutoLoadParams>
                                                        <ext:Parameter Name="start" Value="={0}" />
                                                        <ext:Parameter Name="limit" Value="={15}" />
                                                    </AutoLoadParams>
                                                    <UpdateProxy>
                                                        <ext:HttpWriteProxy Method="GET" Url="ImpRoadMonitor.aspx">
                                                        </ext:HttpWriteProxy>
                                                    </UpdateProxy>
                                                    <Reader>
                                                        <ext:JsonReader>
                                                            <Fields>
                                                                <ext:RecordField Name="dlid" />
                                                                <ext:RecordField Name="dlmc" />
                                                                <ext:RecordField Name="zs" />
                                                                <ext:RecordField Name="gwbl" />
                                                                <ext:RecordField Name="gwbl1" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                            <ColumnModel ID="ColumnModel2" runat="server">
                                                <Columns>
                                                    <ext:Column AutoDataBind="true" Width="150" Fixed="true" DataIndex="dlmc" Resizable="false">
                                                    </ext:Column>
                                                    <ext:Column AutoDataBind="true" Width="180" Align="Center" Sortable="true" EmptyGroupText="0" DataIndex="gwbl">
                                                        <Renderer Fn="DataState" />
                                                    </ext:Column>
                                                    <ext:Column AutoDataBind="true" Width="120" Align="Center" Sortable="true" EmptyGroupText="0" DataIndex="gwbl">
                                                    </ext:Column>
                                                </Columns>
                                            </ColumnModel>
                                            <SelectionModel>
                                                <ext:RowSelectionModel runat="server" SingleSelect="true">
                                                    <Listeners>
                                                        <%--<RowSelect Handler="CarHot.SelectRowRoad(record.data.dlid,record.data.dlmc)" />--%>
                                                    </Listeners>
                                                </ext:RowSelectionModel>
                                            </SelectionModel>
                                            <View>
                                                <ext:GridView ID="GridView2" runat="server" ForceFit="true">
                                                </ext:GridView>
                                            </View>
                                        </ext:GridPanel>
                                    </Items>
                                </ext:Panel>
                            </Items>
                            <DirectEvents>
                                <TabChange OnEvent="TabChange_event">
                                    <EventMask ShowMask="true" />
                                </TabChange>
                            </DirectEvents>
                        </ext:TabPanel>
                    </Items>
                </ext:FormPanel>
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