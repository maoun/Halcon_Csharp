<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarIllegalQuery.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Map.CarIllegalQuery" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>违法多发地</title>
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
    <style type="text/css">
        body .ui-right-wrap .x-grid3-body .x-grid3-td-numberer {
            background-image: none !important;
            background-image: none;
        }
    </style>
    <script type="text/javascript">
        var showTip = function () {
            var rowIndex = GridRoadManager.view.findRowIndex(this.triggerElement),
                cellIndex = GridRoadManager.view.findCellIndex(this.triggerElement),
                record = StoreInfo.getAt(rowIndex),
                fieldName = GridRoadManager.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
    <script type="text/javascript" src="../Scripts/bmapFile.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/bmap.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/jquery.min_v1.0.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/Heatmap_min.js" charset="UTF-8"></script>
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
    <!--梁引入如下js和css-->
    <script type="text/javascript" src="js/Qquery1.91-min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="js/jquery.nicescroll.js" charset="UTF-8"></script>
    <link rel="stylesheet" type="text/css" href="css/Ui-skin.css" />
    <!--梁引入如下js和css 结束-->
</head>
<body onload="MapCenter();">
    <form id="form2" runat="server">
        <ext:Store ID="RoadType" runat="server">
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
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="CarIllegalQuery" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="border" Cls="new-layout">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Title='<%# GetLangStr("CarIllegalQuery1","地图浏览")%>' AutoRender="true" Region="Center"
                    Header="false" Height="800" Width="1000" Cls="map-bg">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar12" runat="server" Cls="top-toolbar">
                            <Items>
                                <ext:Button ID="Linkreload" runat="server" Icon="Reload" Text='<%# GetLangStr("CarIllegalQuery2","重载地图")%>' Cls="top-toolbar">
                                    <Listeners>
                                        <Click Handler="window.location.reload();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Button ID="TbutMove" runat="server" Icon="Erase" Text='<%# GetLangStr("CarIllegalQuery3","清除")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.ClearCircle();BMAP.ClearLabel();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButCenetr" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("CarIllegalQuery4","中心点")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.GotoCenter();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButRanging" runat="server" Icon="PencilGo" Text='<%# GetLangStr("CarIllegalQuery5","测距")%>'>
                                    <Listeners>
                                        <Click Handler=" BMAP.DistanceTool();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButPlan" runat="server" Icon="Vector" Text='<%# GetLangStr("CarIllegalQuery6","测面积")%>'>
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
                        <ext:Label ID="showms" runat="server"></ext:Label>
                    </Content>
                </ext:Panel>
                <ext:FormPanel ID="FormPanel2" Cls="ui-right-wrap Middle-arrow" Padding="5" Region="east" runat="server"
                    Split="true" Collapsible="true" RowHeight="1" Width="400" Layout="Accordion">
                    <TopBar>
                        <ext:Toolbar ID="toolbarrquery" runat="server">
                            <Items>
                                <ext:Panel ID="Pantrack" runat="server" Padding="4" Title="" Layout="Absolute"
                                    Icon="Car" Width="400" Height="150" ButtonAlign="Center">
                                    <Items>
                                        <ext:ComboBox ID="cbodepart" X="10" Y="0" StoreID="Department" FieldLabel='<%# GetLangStr("CarIllegalQuery7"," 所属辖区") %>'
                                            StyleSpec="margin-left: 10px;" runat="server" BlankText='<%# GetLangStr("CarIllegalQuery8","请选择")%>' TypeAhead="true" SelectOnFocus="true"
                                            EmptyText='<%# GetLangStr("CarIllegalQuery9","请选择辖区...")%>' DisplayField="DEPARTNAME" ValueField="DEPARTID"
                                            LabelWidth="65" Width="330" Region="West" Cls="ui-input">
                                            <Items>
                                            </Items>
                                        </ext:ComboBox>
                                        <ext:ComboBox ID="CmbCountType" X="10" Y="40" FieldLabel='<%# GetLangStr("CarIllegalQuery10"," 分析周期") %>'
                                            StyleSpec="margin-left: 10px;" runat="server" BlankText='<%# GetLangStr("CarIllegalQuery11","请选择")%>' TypeAhead="true"
                                            SelectOnFocus="true" EmptyText='<%# GetLangStr("CarIllegalQuery12","请选择周期...")%>' Width="125"
                                            Region="West" Cls="ui-input" LabelWidth="65">
                                            <Items>
                                                <ext:ListItem Text='<%# GetLangStr("CarIllegalQuery13","日")%>' AutoDataBind="true" Value="0" />
                                                <ext:ListItem Text='<%# GetLangStr("CarIllegalQuery14","月")%>' AutoDataBind="true" Value="1" />
                                                <ext:ListItem Text='<%# GetLangStr("CarIllegalQuery15","周")%>' AutoDataBind="true" Value="2" />
                                                <ext:ListItem Text='<%# GetLangStr("CarIllegalQuery16","年")%>' AutoDataBind="true" Value="3" />
                                            </Items>
                                            <DirectEvents>
                                                <Select OnEvent="CmbCountType_Select" Buffer="250">
                                                    <EventMask ShowMask="true" Target="CustomTarget" />
                                                </Select>
                                            </DirectEvents>
                                        </ext:ComboBox>
                                        <ext:ComboBox ID="CmbYear" X="146" Y="40" runat="server" Editable="false" DisplayField="col1" ValueField="col0"
                                            TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("CarIllegalQuery17","选择年...")%>'
                                            SelectOnFocus="true"
                                            Width="80">
                                            <Store>
                                                <ext:Store ID="StoreYear" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader>
                                                            <Fields>
                                                                <ext:RecordField Name="col0" Type="String" />
                                                                <ext:RecordField Name="col1" Type="String" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                        <ext:ComboBox ID="CmbWeek" X="146" Y="40" runat="server" Editable="false" DisplayField="col1" ValueField="col0"
                                            TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("CarIllegalQuery18","选择周...")%>' SelectOnFocus="true"
                                            Width="70" Hidden="true">
                                            <Store>
                                                <ext:Store ID="StoreWeek" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader>
                                                            <Fields>
                                                                <ext:RecordField Name="col0" Type="String" />
                                                                <ext:RecordField Name="col1" Type="String" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                        <ext:ComboBox ID="CmbMonth" X="228" Y="40" runat="server" Editable="false" DisplayField="col1" ValueField="col0"
                                            TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("CarIllegalQuery19","选择月...")%>' SelectOnFocus="true"
                                            Width="60">
                                            <Store>
                                                <ext:Store ID="StoreMonth" runat="server">
                                                    <Reader>
                                                        <ext:JsonReader IDProperty="col0">
                                                            <Fields>
                                                                <ext:RecordField Name="col0" Type="String" />
                                                                <ext:RecordField Name="col1" Type="String" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                            <Listeners>
                                                <Select Handler="#{CmbDay}.clearValue(); #{StoreDay}.reload();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:ComboBox ID="CmbDay" X="290" Y="40" runat="server" Editable="false" DisplayField="col1" ValueField="col0"
                                            TypeAhead="true" Mode="Local" ForceSelection="true" SelectOnFocus="true"
                                            Width="60">
                                            <Store>
                                                <ext:Store ID="StoreDay" runat="server" AutoLoad="true" OnRefreshData="StoreDayRefresh">
                                                    <Reader>
                                                        <ext:JsonReader>
                                                            <Fields>
                                                                <ext:RecordField Name="col0" Type="String" />
                                                                <ext:RecordField Name="col1" Type="String" />
                                                            </Fields>
                                                        </ext:JsonReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                        <ext:Panel runat="server" X="0" Y="80" Height="100" Layout="Absolute">
                                            <Items>

                                                <ext:ComboBox ID="cbonum" X="0" Y="0" FieldLabel="地点统计量" runat="server" Editable="false" TypeAhead="true" Mode="Local" ForceSelection="true" SelectOnFocus="true"
                                                    Width="350" LabelWidth="85" Text="10">
                                                    <Items>
                                                        <ext:ListItem Text="5" />
                                                        <ext:ListItem Text="10" />
                                                        <ext:ListItem Text="15" />
                                                        <ext:ListItem Text="20" />
                                                        <ext:ListItem Text="25" />
                                                        <ext:ListItem Text="30" />
                                                        <ext:ListItem Text="35" />
                                                        <ext:ListItem Text="40" />
                                                        <ext:ListItem Text="45" />
                                                        <ext:ListItem Text="50" />
                                                        <ext:ListItem Text="55" />
                                                        <ext:ListItem Text="60" />
                                                        <ext:ListItem Text="65" />
                                                        <ext:ListItem Text="70" />
                                                        <ext:ListItem Text="75" />
                                                        <ext:ListItem Text="80" />
                                                        <ext:ListItem Text="85" />
                                                        <ext:ListItem Text="90" />
                                                        <ext:ListItem Text="95" />
                                                        <ext:ListItem Text="100" />
                                                    </Items>
                                                </ext:ComboBox>
                                                <ext:Button ID="ButAddgrid" runat="server" Text='<%# GetLangStr("CarIllegalQuery20","查找")%>'
                                                    Width="80" X="50" Y="40" Region="West" IconCls="ui-input w-80px search-btn border-radius-30">
                                                    <DirectEvents>
                                                        <Click OnEvent="ButQueryClick">
                                                            <EventMask ShowMask="true" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Items>
                                        </ext:Panel>
                                    </Items>
                                </ext:Panel>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridRoadManager" runat="server" StripeRows="true" AutoExpandColumn="STATION_NAME"
                            Collapsible="true" AutoHeight="false" RowHeight="1" AutoScroll="true"
                            Cls="data-list-container table-ui display-table w-100  Hide-panel-header">
                            <Store>
                                <ext:Store ID="StoreInfo" runat="server">
                                    <Reader>
                                        <ext:JsonReader IDProperty="col1">
                                            <Fields>
                                                <ext:RecordField Name="xh" />
                                                <ext:RecordField Name="STATION_NAME" />
                                                <ext:RecordField Name="wfxwname" />
                                                <ext:RecordField Name="wfzs" />
                                                <ext:RecordField Name="zs" />
                                                <ext:RecordField Name="wfbl" />
                                                <ext:RecordField Name="xpoint" />
                                                <ext:RecordField Name="ypoint" />
                                                <ext:RecordField Name="STATION_ID" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <%--   <ext:RowNumbererColumn runat="server" Width="40"></ext:RowNumbererColumn>--%>
                                    <ext:Column Header='<%# GetLangStr("CarIllegalQuery21","序号")%>' AutoDataBind="true" DataIndex="xh" Align="Center" Editable="false" Width="40" />
                                    <ext:Column Sortable="true" Header='<%# GetLangStr("CarIllegalQuery22","地点名称")%>' AutoDataBind="true" DataIndex="STATION_NAME" Align="Center" Width="170" />
                                    <ext:Column Header='<%# GetLangStr("CarIllegalQuery23","违法行为")%>' AutoDataBind="true" DataIndex="wfxwname" Align="Center" Hidden="true" Width="75" />
                                    <ext:Column Header='<%# GetLangStr("CarIllegalQuery24","违法数量")%>' AutoDataBind="true" DataIndex="wfzs" Align="Center" Width="55" />
                                    <ext:Column Header='<%# GetLangStr("CarIllegalQuery25","过车数量")%>' AutoDataBind="true" DataIndex="zs" Align="Center" Width="55" />
                                    <ext:Column Header='<%# GetLangStr("CarIllegalQuery26","违法比率")%>' AutoDataBind="true" DataIndex="wfbl" Align="Center" Width="55" />
                                    <ext:Column Header='<%# GetLangStr("CarIllegalQuery27","x")%>' AutoDataBind="true" DataIndex="xpoint" Hidden="true" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CarIllegalQuery28","y")%>' AutoDataBind="true" DataIndex="ypoint" Hidden="true" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CarIllegalQuery29","STATIONID")%>' AutoDataBind="true" DataIndex="STATION_ID" Hidden="true" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>

                                <ext:RowSelectionModel runat="server">
                                    <Listeners>
                                        <RowSelect Handler="CarIllegalQuery.SelectRow(record.data.xpoint,record.data.ypoint,record.data.STATION_ID,record.data.STATION_NAME)" />
                                    </Listeners>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <View>

                                <ext:GridView runat="server" StripeRows="true" TrackOver="true" />
                            </View>
                            <ToolTips>
                                <ext:ToolTip
                                    ID="RowTip"
                                    runat="server"
                                    Target="={GridRoadManager.getView().mainBody}"
                                    Delegate=".x-grid3-cell"
                                    TrackMouse="true">
                                    <Listeners>
                                        <Show Fn="showTip" />
                                    </Listeners>
                                </ext:ToolTip>
                            </ToolTips>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>

<script type="text/javascript">
    $('.OverCar-data-list').niceScroll({
        cursorcolor: "#7683a4",//#CC0071 光标颜色
        cursoropacitymax: 1, //改变不透明度非常光标处于活动状态（scrollabar“可见”状态），范围从1到0
        touchbehavior: false, //使光标拖动滚动像在台式电脑触摸设备
        cursorwidth: "3px", //像素光标的宽度
        cursorborder: "0", // 	游标边框css定义
        cursorborderradius: "5px",//以像素为光标边界半径
        autohidemode: false //是否隐藏滚动条
    });
</script>