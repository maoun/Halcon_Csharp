<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarQuery.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.Map.CarQuery" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Src="../VehicleHead.ascx" TagName="VehicleHead" TagPrefix="veh" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>伴随车查询</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="../Style/customMap.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/Clzpp/mapcarPicker.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/Clzpp/base.css" />
    <link rel="Stylesheet" href="../Styles/datetime.css" type="text/css" />
    <script type="text/javascript" src="../Scripts/Clzpp/carData.js"></script>
    <script type="text/javascript" src="../Scripts/Clzpp/mapcarPicker.js"></script>
    <script type="text/javascript" src="../Scripts/Clzpp/jquery-2.1.3.min.js"></script>
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
        function clearTime(start, end) {
            document.getElementById("start").innerText = start;
            document.getElementById("end").innerText = end;
        };
        function getTime() {
            CarQuery.GetDateTime(true, document.getElementById("start").innerText);
            CarQuery.GetDateTime(false, document.getElementById("end").innerText);
        };
    </script>
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
    <script type="text/javascript">
        var GetScreen = function () {
            W = $(window).width();// screen.width;
            H = $(window).height(); //screen.height;
            wingcjl.x = W - 1160;
            wingcjl.y = H - 258;

            winhphm.x = W - 805;
            winhphm.y = H - 258;
        }
    </script>
    <script type="text/javascript">
        var showTipWin = function () {
            var rowIndex = gridgcjl.view.findRowIndex(this.triggerElement),
                cellIndex = gridgcjl.view.findCellIndex(this.triggerElement),
                record = Storegcjl.getAt(rowIndex),
                fieldName = gridgcjl.getColumnModel().getDataIndex(cellIndex),
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
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <!--梁引入如下js和css 结束-->
    <script type="text/javascript">

        var change = function (obj, value) {
            obj.setValue(value.toUpperCase());
        };
    </script>
    <script type="text/javascript">
        //伴随车辆,输入框回车事件.
        $(function () {
            $("body").delegate("#txtplate", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#cbocllx").click();
                }
            })
        })
    </script>
    <script type="text/javascript">
        var setGroupStyle = function (view) {
            var groups = view.getGroups();

            for (var i = 0; i < groups.length; i++) {
                var spans = Ext.query("span", groups[i]);
                if (spans && spans.length > 0) {
                    var color = "#" + spans[0].id.split("-")[1];
                    Ext.get(groups[i]).setStyle("background-color", color);
                }
            }
        };
    </script>
    <script type="text/javascript">
        $(function () {
            $("body").delegate("#txtplate", "keypress", function (event) {
                if (event.keyCode == "13") {
                    $("#ext-gen56").click();
                }
            })
        })
    </script>
</head>
<body onload="MapCenter();">
    <form id="form2" runat="server">
        <ext:Store ID="cllx" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="csys" runat="server">
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
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="CarQuery" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="border" Cls="new-layout">
            <Items>
                <ext:Panel ID="pnlData" runat="server" Title='<%# GetLangStr("CarQuery1","地图浏览")%>' AutoRender="true" Region="Center"
                    Header="false" Height="800" Width="1000" Cls="map-bg">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar12" runat="server" Cls="top-toolbar">
                            <Items>
                                <ext:Button ID="Linkreload" runat="server" Icon="Reload" Text='<%# GetLangStr("CarQuery2","重载地图")%>'>
                                    <Listeners>
                                        <Click Handler="window.location.reload();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Button ID="TbutMove" runat="server" Icon="Erase" Text='<%# GetLangStr("CarQuery3","清除")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.ClearCircle();BMAP.ClearLabel();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButCenetr" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("CarQuery4","中心点")%>'>
                                    <Listeners>
                                        <Click Handler="BMAP.GotoCenter();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButRanging" runat="server" Icon="PencilGo" Text='<%# GetLangStr("CarQuery5","测距")%>'>
                                    <Listeners>
                                        <Click Handler=" BMAP.DistanceTool();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButPlan" runat="server" Icon="Vector" Text='<%# GetLangStr("CarQuery6","测面积")%>'>
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
                <ext:FormPanel ID="FormPanel2" Padding="5" Cls="ui-right-wrap Middle-arrow" Region="east" runat="server" Title=""
                    Split="true" Collapsible="true" RowHeight="1" Width="400" Layout="Accordion">
                    <TopBar>
                        <ext:Toolbar ID="toolbarrquery" runat="server">
                            <Items>
                                <ext:Panel ID="Pantrack" runat="server" Padding="4" Title="" Layout="Absolute"
                                    Icon="Car" Width="400" Height="400">
                                    <Items>
                                        <ext:Panel runat="server" X="0" Y="0">
                                            <Content>
                                                <div id="selectDate">

                                                    <span class="laydate-span" style="margin-left: 12px; height: 24px;"><%# GetLangStr("CarQuery7","开始时间:")%></span>
                                                    <li runat="server" class="laydate-icon" id="start" style="width: 230px; margin-left: 16px; height: 22px;"></li>
                                                </div>
                                                <div class="clear" style="clear: both"></div>
                                                <div style="margin-top: 15px">
                                                    <span class="laydate-span" style="margin-left: 12px; height: 24px;"><%# GetLangStr("CarQuery8","结束时间:")%></span>
                                                    <li runat="server" class="laydate-icon" id="end" style="width: 230px; margin-left: 16px; height: 22px;"></li>
                                                </div>
                                            </Content>
                                        </ext:Panel>
                                        <ext:TextField ID="txtgssj" X="10" Y="90" MaxLength="2" FieldLabel='<%# GetLangStr("CarQuery9","跟随时间(<=)") %>' StyleSpec="margin-left: 4px;" runat="server" Width="330" Text="5" Region="West" Cls="ui-input">
                                        </ext:TextField>
                                        <ext:Label ID="labplate" X="355" Y="95" runat="server" Text='<%# GetLangStr("CarQuery10","秒")%>' />
                                        <ext:TextField ID="txttxkk" X="10" Y="135" FieldLabel='<%# GetLangStr("CarQuery11","同行卡口(>=)") %>' StyleSpec="margin-left: 4px;" runat="server" Width="330" Text="2" Region="West" Cls="ui-input">
                                        </ext:TextField>
                                        <ext:Label ID="Label1" X="355" Y="140" runat="server" Text='<%# GetLangStr("CarQuery12","个")%>' />

                                        <ext:Label X="1" Y="180" Text='<%# GetLangStr("CarQuery13","号牌号码:")%>' StyleSpec="margin-left: 10px;" Width="70" runat="server" />
                                        <ext:Panel ID="Panel4" X="110" Y="180" runat="server" Height="29" StyleSpec="margin-left:10px;">
                                            <Content>
                                                <veh:VehicleHead ID="cboplate" runat="server" />
                                            </Content>
                                        </ext:Panel>
                                        <ext:TextField ID="txtplate" X="165" Y="180" Width="180" runat="server" Cls="ui-input">
                                            <Listeners>
                                                <Change Fn="change" />
                                            </Listeners>
                                        </ext:TextField>
                                        <ext:ComboBox ID="cbocllx" X="10" Y="220" Editable="false" FieldLabel='<%# GetLangStr("CarQuery14"," 号牌种类") %>' StyleSpec="margin-left: 6px;" runat="server" BlankText='<%# GetLangStr("CarQuery15","请选择")%>' StoreID="cllx" TypeAhead="true" SelectOnFocus="true"
                                            EmptyText='<%# GetLangStr("CarQuery16","请选择号牌种类...")%>' DisplayField="col1" ValueField="col0" Width="333" Region="West" Cls="ui-input">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("CarQuery17","清除选中")%>' AutoDataBind="true" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };this.EmptyText='请选择...';" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:Panel ID="Panel8" runat="server" Height="30" Width="380" X="10" Y="300" HideBorders="true" Layout="ColumnLayout">
                                            <Items>
                                                <ext:Panel runat="server">
                                                    <Content>
                                                        <input type="hidden" runat="server" id="clpp" value="" />
                                                        <input type="hidden" runat="server" id="clzpp" value="" />
                                                        <div style="margin-top: 5px; width: 100px;">
                                                            <span><%# GetLangStr("CarQuery53","车辆品牌：")%></span>
                                                        </div>
                                                    </Content>
                                                </ext:Panel>
                                                <ext:Panel runat="server">
                                                    <Content>
                                                        <input type="text" runat="server" id="ClppChoice" style="height: 24px; width: 223px;" />
                                                    </Content>
                                                </ext:Panel>
                                            </Items>
                                        </ext:Panel>
                                        <ext:ComboBox ID="cbocsys" X="10" Y="260" StoreID="csys" FieldLabel='<%# GetLangStr("CarQuery18"," 车身颜色") %>' StyleSpec="margin-left: 6px;" runat="server" BlankText='<%# GetLangStr("CarQuery19","请选择...")%>' TypeAhead="true" SelectOnFocus="true"
                                            EmptyText='<%# GetLangStr("CarQuery19","请选择...")%>' Width="333" Text="" DisplayField="CODEDESC" Editable="false" ValueField="CODE" Region="West" Cls="ui-input">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip='<%# GetLangStr("CarQuery17","清除选中")%>' AutoDataBind="true" />
                                            </Triggers>
                                            <Listeners>
                                                <Select Handler="this.triggers[0].show();" />
                                                <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };this.EmptyText='请选择...';" />
                                            </Listeners>
                                        </ext:ComboBox>

                                        <ext:Button ID="ButAddgrid" runat="server" X="110" Y="340" Text='<%# GetLangStr("CarQuery20","查找")%>' Width="80" Region="West" IconCls="ui-input w-100px search-btn border-radius-30">
                                            <DirectEvents>
                                                <Click OnEvent="unnamed_event">
                                                    <EventMask ShowMask="true" />
                                                </Click>
                                            </DirectEvents>
                                            <%-- <Listeners>
                                                <Click Handler="getTime();CarQuery.ButQueryClick();" />
                                            </Listeners>--%>
                                        </ext:Button>
                                    </Items>
                                </ext:Panel>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:GridPanel ID="GridRoadManager" runat="server" StripeRows="true" Title=""
                            Collapsible="true" Width="320" AutoHeight="false" AutoScroll="true" Cls="data-list-container table-ui display-table w-100 Hide-panel-header">
                            <Store>
                                <ext:Store ID="StoreInfo" runat="server">
                                    <Reader>
                                        <ext:JsonReader IDProperty="col1">
                                            <Fields>
                                                <ext:RecordField Name="hphm" />
                                                <ext:RecordField Name="gwsj" />
                                                <ext:RecordField Name="cllx" />
                                                <ext:RecordField Name="txsl" />
                                                <ext:RecordField Name="clpp" />
                                                <ext:RecordField Name="csys" />
                                                <ext:RecordField Name="hpzl" />
                                                <ext:RecordField Name="clwz" />
                                                <ext:RecordField Name="zjwj" />
                                                <ext:RecordField Name="bssj" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Header='<%# GetLangStr("CarQuery21","序号")%>' AutoDataBind="true" Width="50" Align="Center" Editable="false" />
                                    <ext:Column Header='<%# GetLangStr("CarQuery22","号牌号码")%>' AutoDataBind="true" DataIndex="hphm" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CarQuery14","号牌种类")%>' AutoDataBind="true" DataIndex="cllx" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CarQuery24","同行卡口数")%>' AutoDataBind="true" DataIndex="txsl" Width="130" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel>
                                    <Listeners>
                                        <RowSelect Handler="CarQuery.SelectRow(record.data.hphm,record.data.clpp,record.data.csys,record.data.hpzl,'')" />
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
        <ext:Window ID="winPassWeb" Modal="true" runat="server" Hidden="true" Height="600px"
            Width="800px" Title='<%# GetLangStr("CarQuery25","过车信息")%>' Resizable="false" Layout="FitLayout">
            <Items>
                <ext:FormPanel runat="server" ID="extForm" Layout="RowLayout">
                    <Items>
                        <ext:Label runat="server" ID="lblPassInfo" RowHeight=".1" StyleSpec="font-size: 16px; font-weight:bold"></ext:Label>
                        <ext:Image runat="server" ID="imgPassInfo" RowHeight=".9"></ext:Image>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Window>
        <!-- 弹出框右边的Windows-->
        <ext:Window ID="winhphm" runat="server" Title='<%# GetLangStr("CarQuery26","车辆信息")%>' Hidden="true"
            X="700" Y="400" Width="400" Layout="RowLayout" Height="260">
            <Items>
                <ext:Panel runat="server" RowHeight=".5">
                    <Items>
                        <ext:GridPanel runat="server" ID="grid" Height="100">
                            <Store>
                                <ext:Store ID="Store1" runat="server">
                                    <Reader>
                                        <ext:JsonReader IDProperty="col1">
                                            <Fields>
                                                <ext:RecordField Name="cartype" />
                                                <ext:RecordField Name="clpp" />
                                                <ext:RecordField Name="csys" />
                                                <ext:RecordField Name="hphm" />
                                                <ext:RecordField Name="hpzl" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel2" runat="server">
                                <Columns>
                                    <ext:Column Header='<%# GetLangStr("CarQuery27","类型")%>' AutoDataBind="true" DataIndex="cartype" Width="60" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CarQuery22","号牌号码")%>' AutoDataBind="true" Width="100" DataIndex="hphm" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CarQuery14","号牌种类")%>' AutoDataBind="true" DataIndex="hpzl" Width="100" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CarQuery30","颜色")%>' AutoDataBind="true" DataIndex="csys" Width="60" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CarQuery31", "车辆品牌")%>' AutoDataBind="true" DataIndex="clpp" Width="110" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <View>
                                <ext:GridView runat="server" ForceFit="true"></ext:GridView>
                            </View>
                        </ext:GridPanel>
                    </Items>
                </ext:Panel>
                <ext:Panel runat="server" RowHeight=".5" Layout="Absolute">
                    <Items>
                        <ext:Button X="0" Y="0" runat="server" Width="120" Text='<%# GetLangStr("CarQuery32","车辆轨迹")%>'>
                            <Listeners>
                                <Click Handler="CarQuery.showotherwin('PathCarQuery.aspx','车辆轨迹');" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button X="175" Y="0" runat="server" Width="120" Text='<%# GetLangStr("CarQuery33", "落脚点")%>'>
                            <Listeners>
                                <Click Handler="CarQuery.showotherwin('FootHold.aspx','落脚点');" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button X="0" Y="40" runat="server" Width="120" Text='<%# GetLangStr("CarQuery34","违法信息查询")%>'>
                            <Listeners>
                                <Click Handler="CarQuery.showotherwin('../Passcar/PeccancyInfoQuery.aspx','违法信息查询');" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button X="175" Y="40" runat="server" Width="120" Text='<%# GetLangStr("CarQuery35","过车记录查询")%>'>
                            <Listeners>
                                <Click Handler="CarQuery.showotherwin('../Passcar/PassCarInfoQuery.aspx','过车记录查询');" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Window>
        <!-- 弹出框左边的Windows-->
        <ext:Window
            ID="wingcjl"
            runat="server"
            Title="过车记录"
            Hidden="true"
            X="340"
            Y="400"
            AutoExpandColumn="Common"
            Width="350"
            Height="260">
            <Items>
                <ext:Panel runat="server">
                    <Items>
                        <ext:GridPanel runat="server" ID="gridgcjl" AutoScroll="true" AutoExpandColumn="lkmc" Height="350">
                            <Store>
                                <ext:Store ID="Storegcjl" runat="server" GroupField="lkmc">
                                    <Reader>
                                        <ext:JsonReader IDProperty="col1">
                                            <Fields>
                                                <ext:RecordField Name="gwsj" />
                                                <ext:RecordField Name="lkmc" />
                                                <ext:RecordField Name="zjwj" />
                                                <ext:RecordField Name="hphm" />
                                                <ext:RecordField Name="xpoint" />
                                                <ext:RecordField Name="ypoint" />
                                                <ext:RecordField Name="clwz" />
                                                <ext:RecordField Name="bssj" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                    <%-- <SortInfo Field="gwsj" Direction="DESC" />--%>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel3" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Header='<%# GetLangStr("CarQuery21","序号")%>' AutoDataBind="true" Align="Center" Editable="false" Hidden="true" />
                                    <ext:Column ColumnID="lkmc" Header='<%# GetLangStr("CarQuery37", "卡口")%>' AutoDataBind="true" DataIndex="lkmc" Width="130" Align="left" />
                                    <ext:Column Header='<%# GetLangStr("CarQuery22","号牌号码")%>' AutoDataBind="true" DataIndex="hphm" Width="80" Align="left" />
                                    <ext:Column Header='<%# GetLangStr("CarQuery39","过车时间")%>' AutoDataBind="true" Sortable="true" DataIndex="gwsj" Width="120" Align="Center" />
                                    <ext:Column Header='<%# GetLangStr("CarQuery40","伴随时间")%>' AutoDataBind="true" DataIndex="bssj" Width="80" Align="Center" />
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel runat="server">
                                    <Listeners>
                                        <RowSelect Handler="CarQuery.ShowMoreInfo(record.data.xpoint,record.data.ypoint,record.data.hphm,record.data.gwsj,record.data.lkmc,record.data.zjwj,record.data.clwz)" />
                                    </Listeners>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <View>
                                <ext:GroupingView
                                    runat="server"
                                    ForceFit="true"
                                    MarkDirty="false"
                                    ShowGroupName="false"
                                    EnableNoGroups="true"
                                    HideGroupedColumn="true" />
                                <%--<ext:GridView runat="server" StripeRows="true" TrackOver="true" />--%>
                            </View>
                            <ToolTips>
                                <ext:ToolTip ID="ToolTip1" runat="server" Target="={gridgcjl.getView().mainBody}"
                                    Delegate=".x-grid3-cell" TrackMouse="true">
                                    <Listeners>
                                        <Show Fn="showTipWin" />
                                    </Listeners>
                                </ext:ToolTip>
                            </ToolTips>
                        </ext:GridPanel>
                    </Items>
                </ext:Panel>
            </Items>
        </ext:Window>
    </form>
    <script type="text/javascript">
        function isScroll() {
            $('.OverCar-data-list').niceScroll({
                cursorcolor: "#7683a4",//#CC0071 光标颜色
                cursoropacitymax: 1, //改变不透明度非常光标处于活动状态（scrollabar“可见”状态），范围从1到0
                touchbehavior: false, //使光标拖动滚动像在台式电脑触摸设备
                cursorwidth: "3px", //像素光标的宽度
                cursorborder: "0", // 	游标边框css定义
                cursorborderradius: "5px",//以像素为光标边界半径
                autohidemode: true//是否隐藏滚动条
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
                autohidemode: true //是否隐藏滚动条
            });
        }
    </script>
    <script type="text/jscript">
        var cityPicker = new IIInsomniaCityPicker({
            data: carData,
            target: '#ClppChoice',
            valType: 'k-v',
            hideCityInput: '#clzpp',
            hideProvinceInput: '#clpp',
            callback: function (city_id) {

            }
        });

        cityPicker.init();
    </script>
</body>
</html>

<script type="text/javascript">
    laydate.skin('danlan');
    var start = {
        elem: '#start',
        format: 'YYYY-MM-DD hh:mm:ss',
        //min: laydate.now(), //设定最小日期为当前日期
        max: laydate.now(),//'2099-06-16 23:59:59', //最大日期
        istime: true,
        istoday: false,
        isclear: false,
        choose: function (datas) {
            end.min = datas; //开始日选好后，重置结束日的最小日期
            end.start = datas //将结束日的初始值设定为开始日
            $("#end").click();//开始时间选中后，自动弹出结束时间
            var tt = $("#start").html();
            CarQuery.GetDateTime(true, tt);
            //alert(tt);
        }
    };
    var end = {
        elem: '#end',
        format: 'YYYY-MM-DD hh:mm:ss',
        min: laydate.now(),
        max: laydate.now(),//'2099-06-16 23:59:59',
        istime: true,
        istoday: false,
        isclear: false,
        choose: function (datas) {
            start.max = datas; //结束日选好后，重置开始日的最大日期
            var tt = $("#end").html();
            CarQuery.GetDateTime(false, tt);
        }
    };
    laydate(start);
    laydate(end);
</script>