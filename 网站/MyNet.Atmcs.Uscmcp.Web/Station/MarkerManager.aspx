<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarkerManager.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.MarkerManager" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>监测点标注</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link rel="stylesheet" type="text/css" href="../Map/css/custom.css" />
    <link rel="stylesheet" type="text/css" href="../Map/css/Ui-skin.css" />
    <style type="text/css">
        body, html {
            font-family: Arial,Verdana;
            font-size: 13px;
            margin: 0;
            overflow: hidden;
        }

        #map_canvas {
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            position: absolute;
        }
    </style>

    <link rel="stylesheet" type="text/css" href="../Map/css/Ui-skin.css" />
    <script type="text/javascript" src="../Scripts/laydate/laydate.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/bmapFile.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Scripts/bmap.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Map/js/Qquery1.91-min.js" charset="UTF-8"></script>
    <script type="text/javascript" src="../Map/js/jquery.nicescroll.js" charset="UTF-8"></script>
    <script type="text/javascript">
        var selectionChaged = function (dv, nodes) {

            if (nodes.length > 0) {
                var order = nodes[0].innerText;
                var n = order.indexOf(',');
                var m = order.substring(0, n);
                if (showTrack != null) {
                    showTrack.ToCenter(m);
                }
            }
        };
        var prepare = function (grid, toolbar, rowIndex, record) {
            var Button1 = toolbar.items.get(0);
            var Button2 = toolbar.items.get(1);
            if (record.data.col2 == "0") {
                Button1.setDisabled(false);
                Button2.setDisabled(true);
            }
            else {

                Button2.setDisabled(false);
                Button1.setDisabled(true);
            }
        };
        var prepareGz = function (grid, toolbar, rowIndex, record) {
            var Button1 = toolbar.items.get(0);
            var Button2 = toolbar.items.get(1);
            if (record.data.col12 == "") {
                Button1.setDisabled(false);
                Button2.setDisabled(true);
            }
            else {

                Button2.setDisabled(false);
                Button1.setDisabled(true);
            }
        };
        var prepareZd = function (grid, toolbar, rowIndex, record) {
            var Button1 = toolbar.items.get(0);
            var Button2 = toolbar.items.get(1);
            if (record.data.col30 == "") {
                Button1.setDisabled(false);
                Button2.setDisabled(true);
            }
            else {

                Button2.setDisabled(false);
                Button1.setDisabled(true);
            }
        };
    </script>
    <script type="text/javascript">
        function MapCenter() {
            BMAP.MapInit();
            setTimeout(function () {
                BMAP.GotoCenter();
            }, 500);
        }
    </script>
    <script type="text/javascript">
        var template = '<span style="color:{0};">{1}</span>';
        var change = function (value) {
            var mark;
            if (value == "0") {
                mark = "未标注";
            }
            else {
                if (value == "1") {
                    mark = "已标注";
                }
            }
            return String.format(template, (value == "1") ? "green" : "red", mark);
        };
        var change2 = function (value) {
            var mark;
            if (value == "") {
                mark = "未标注";
            }
            else {

                mark = "已标注";

            }
            return String.format(template, (value == "") ? "red" : "green", mark);
        };
    </script>
    <script type="text/javascript">
        function MarkAndDelete(command, data, stype) {
            var _id; var _name; var _type;
            var x; var y;
            _type = stype;
            if (command == "Add") {
                switch (stype) {
                    case "ZD":
                        _id = data.col0;
                        _name = data.col11;
                        BMAP.SaveMarker({ id: _id, name: _name, type: _type });
                        break;
                    case "GZ":
                        _id = data.col0;
                        _name = data.col1;
                        BMAP.SaveMarker({ id: _id, name: _name, type: _type });
                        break;
                    default:
                        _id = data.col0;
                        _name = data.col1;
                        BMAP.SaveMarker({ id: _id, name: _name, type: _type });
                        break;
                }
            }
            else {
                if (command == "Delete") {

                    switch (stype) {
                        case "ZD":
                            _id = data.col0;
                            _name = data.col11;
                            break;
                        case "GZ":
                            _id = data.col0;
                            _name = data.col1;
                            break;
                        default:
                            _id = data.col0;
                            _name = data.col1;
                            x = data.col3;
                            y = data.col4;
                            break;
                    }
                    if (confirm("确定删除【" + _name + "】的标注信息?")) {

                        BMAP.removeMarker(x, y);
                        MarkerManager.DeleteMarkInfo(_id, stype);
                    }
                }
            }
        }
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridState.view.findRowIndex(this.triggerElement),
                cellIndex = GridState.view.findCellIndex(this.triggerElement),
                record = StoreState.getAt(rowIndex),
                fieldName = GridState.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body onload="MapCenter();">
    <form id="form1" runat="server" class="new-layout">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="MarkerManager" />
        <ext:Store ID="StoreMarkType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreType" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreZd" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" Type="String" />
                        <ext:RecordField Name="col11" Type="String" />
                        <ext:RecordField Name="col12" Type="String" />
                        <ext:RecordField Name="col13" Type="String" />
                        <ext:RecordField Name="col14" Type="String" />
                        <ext:RecordField Name="col15" Type="String" />
                        <ext:RecordField Name="col16" Type="String" />
                        <ext:RecordField Name="col17" Type="String" />
                        <ext:RecordField Name="col18" Type="String" />
                        <ext:RecordField Name="col19" Type="String" />
                        <ext:RecordField Name="col30" Type="String" />
                        <ext:RecordField Name="col31" Type="String" />
                        <ext:RecordField Name="col32" Type="String" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreGz" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" Type="String" />
                        <ext:RecordField Name="col1" Type="String" />
                        <ext:RecordField Name="col2" Type="String" />
                        <ext:RecordField Name="col3" Type="String" />
                        <ext:RecordField Name="col4" Type="String" />
                        <ext:RecordField Name="col5" Type="String" />
                        <ext:RecordField Name="col6" Type="String" />
                        <ext:RecordField Name="col7" Type="String" />
                        <ext:RecordField Name="col8" Type="String" />
                        <ext:RecordField Name="col9" Type="String" />
                        <ext:RecordField Name="col10" Type="String" />
                        <ext:RecordField Name="col11" Type="String" />
                        <ext:RecordField Name="col12" Type="String" />
                        <ext:RecordField Name="col13" Type="String" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="Viewport1" runat="server" Layout="border">
            <Items>
                <%--地图窗体 中间--%>
                <ext:Panel ID="pnlData" runat="server" Title='<%# GetLangStr("KeyRoadManager1","地图浏览") %>' AutoRender="true" Region="Center"
                    Header="false" Height="800" Width="1000" Cls="map-bg">
                    <TopBar>
                        <ext:Toolbar ID="Toolbar12" runat="server" Cls="Top-Bar top-toolbar">
                            <Items>
                                <ext:LinkButton ID="Linkreload" runat="server" Icon="Reload" Text='<%# GetLangStr("KeyRoadManager2","重载地图") %>'>
                                    <Listeners>
                                        <Click Handler="window.location.reload();" />
                                    </Listeners>
                                </ext:LinkButton>
                                <ext:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                </ext:ToolbarSeparator>
                                <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                <ext:Button ID="ButCenetr" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("KeyRoadManager3","中心点") %>'>
                                    <Listeners>
                                        <Click Handler="BMAP.GotoCenter();" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="ButMark" runat="server" Icon="CommentAdd" Text='<%# GetLangStr("KeyRoadManager4","标注") %>'>
                                    <Listeners>
                                        <Click Handler="var type; _type='COM';BMAP.SaveMarker({type: _type });" />
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
                <%--右侧--%>
                <ext:FormPanel ID="FormPanel2" Cls="ui-right-wrap Middle-arrow" runat="server" Region="East" Split="true"
                    Collapsible="true" Collapsed="false" Width="350" Layout="Accordion">

                    <TopBar>
                        <ext:Toolbar ID="toolbarrquery" runat="server">
                            <Items>
                                <ext:Panel ID="Pantrack" runat="server" Width="400" AutoHeight="true">
                                    <Items>
                                        <ext:Panel ID="Panel1" runat="server" Title="" Height="40" Border="false" Layout="Absolute">
                                            <Items>
                                                <ext:ComboBox ID="CmbStationType" runat="server" Editable="false" FieldLabel='<%# GetLangStr("KeyRoadManager6","监测点类型") %>'
                                                    DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                                                    EmptyText='<%# GetLangStr("KeyRoadManager7","选择监测点类型...") %>' SelectOnFocus="true" AllowBlank="false" Width="300" X="10"
                                                    Y="10" Cls="ui-input">
                                                    <Triggers>
                                                        <ext:FieldTrigger Icon="Clear" HideTrigger="true" Qtip="清除选中" />
                                                    </Triggers>
                                                    <Listeners>
                                                        <Select Handler="this.triggers[0].show();" />
                                                        <BeforeQuery Handler="this.triggers[0][ this.getRawValue().toString().length == 0 ? 'hide' : 'show']();" />
                                                        <TriggerClick Handler="if (index == 0) { this.clearValue(); this.triggers[0].hide(); };" />
                                                    </Listeners>
                                                    <Store>
                                                        <ext:Store ID="StoreStationType" runat="server">
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
                                            </Items>
                                        </ext:Panel>

                                        <ext:Panel FormGroup="true" runat="server" Layout="ColumnLayout" AutoHeight="true" ButtonAlign="Center">
                                            <Buttons>
                                                <ext:Button ID="ButtonQuery" runat="server" Text='<%# GetLangStr("KeyRoadManager8","查询") %>' Width="120" Region="West" IconCls="ui-input w-80px search-btn border-radius-30">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutQueryStationClick" />
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButShuaxin" runat="server" Text='<%# GetLangStr("KeyRoadManager9","刷新") %>' Width="120" Region="West" IconCls="ui-input w-80px search-btn border-radius-30">
                                                    <DirectEvents>
                                                        <Click OnEvent="TbutQueryStationClick" />
                                                    </DirectEvents>
                                                </ext:Button>
                                            </Buttons>
                                        </ext:Panel>
                                    </Items>
                                </ext:Panel>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <%--交通监测点信息标注--%>
                        <ext:GridPanel ID="GridState" runat="server" HideBorders="true" StripeRows="true"
                            Title='<%# GetLangStr("KeyRoadManager10","交通监测点信息标注") %>'
                            Layout="AnchorLayout" Cls="data-list-container table-ui display-table w-100 Hide-panel-header"
                            TrackMouseOver="true">
                            <Store>
                                <ext:Store ID="StoreState" runat="server" GroupField="col7">
                                    <Reader>
                                        <ext:JsonReader>
                                            <Fields>
                                                <ext:RecordField Name="col0" Type="String" />
                                                <ext:RecordField Name="col1" Type="String" />
                                                <ext:RecordField Name="col2" Type="String" />
                                                <ext:RecordField Name="col3" Type="String" />
                                                <ext:RecordField Name="col4" Type="String" />
                                                <ext:RecordField Name="col5" Type="String" />
                                                <ext:RecordField Name="col6" Type="String" />
                                                <ext:RecordField Name="col7" Type="String" />
                                                <ext:RecordField Name="col8" Type="String" />
                                                <ext:RecordField Name="col9" Type="String" />
                                                <ext:RecordField Name="col10" Type="String" />
                                                <ext:RecordField Name="col11" Type="String" />
                                            </Fields>
                                        </ext:JsonReader>
                                    </Reader>
                                </ext:Store>
                            </Store>
                            <ColumnModel ID="ColumnModel1" runat="server">
                                <Columns>
                                    <ext:RowNumbererColumn Width="40" />
                                    <ext:Column Header='<%# GetLangStr("KeyRoadManager11","监测点类型") %>' AutoDataBind="true" DataIndex="col7" Width="60" />
                                    <ext:Column Header='<%# GetLangStr("KeyRoadManager12","监测点名称") %>' AutoDataBind="true" DataIndex="col1" Width="170" />
                                    <ext:Column DataIndex="col2" Header='<%# GetLangStr("KeyRoadManager13","标注情况") %>' AutoDataBind="true" Width="80">
                                        <Renderer Fn="change" />
                                    </ext:Column>
                                    <ext:CommandColumn Width="60">
                                        <Commands>
                                            <ext:GridCommand Icon="MapAdd" CommandName="Add">
                                                <ToolTip Text='<%# GetLangStr("KeyRoadManager14","地图上标注") %>' AutoDataBind="true" />
                                            </ext:GridCommand>
                                            <ext:GridCommand Icon="MapDelete" CommandName="Delete">
                                                <ToolTip Text='<%# GetLangStr("KeyRoadManager15","删除地点标注") %>' AutoDataBind="true" />
                                            </ext:GridCommand>
                                        </Commands>
                                        <PrepareToolbar Fn="prepare" />
                                    </ext:CommandColumn>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel7" runat="server" SingleSelect="true">
                                    <DirectEvents>
                                        <RowSelect OnEvent="RowSelect" Buffer="100">
                                            <ExtraParams>
                                                <ext:Parameter Name="data" Value="record.data" Mode="Raw" />
                                            </ExtraParams>
                                        </RowSelect>
                                    </DirectEvents>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <Listeners>
                                <Command Handler="MarkAndDelete(command, record.data,record.data.col9);" />
                            </Listeners>
                            <View>
                                <ext:GroupingView ID="GroupingView1" runat="server" ForceFit="false" MarkDirty="false"
                                    ShowGroupName="false" EnableNoGroups="true" HideGroupedColumn="true" GroupByText='<%# GetLangStr("KeyRoadManager16","用该列进行分组") %>'
                                    ShowGroupsText='<%# GetLangStr("KeyRoadManager17","显示分组") %>' />
                            </View>
                            <ToolTips>
                                <ext:ToolTip
                                    ID="ToolTip1"
                                    runat="server"
                                    Target="={GridState.getView().mainBody}"
                                    Delegate=".x-grid3-cell"
                                    TrackMouse="true">
                                    <Listeners>
                                        <Show Fn="showTip" />
                                    </Listeners>
                                </ext:ToolTip>
                            </ToolTips>
                        </ext:GridPanel>
                        <ext:ToolTip
                            ID="RowTip"
                            runat="server"
                            Target="={#{GridState}.getView().mainBody}"
                            Delegate=".x-grid3-row"
                            TrackMouse="true">
                            <Listeners>
                                <Show Handler="var rowIndex = #{GridState}.view.findRowIndex(this.triggerElement);this.body.dom.innerHTML = '<b>监测点 :</b> ' + #{StoreState}.getAt(rowIndex).get('col1');" />
                            </Listeners>
                        </ext:ToolTip>
                        <%--施工占道信息标注--%>
                        <ext:Panel ID="Panel3" runat="server" Title='<%# GetLangStr("KeyRoadManager18","施工占道信息标注") %>' IconCls="custom-iconZD" Layout="Fit" Hidden="true">
                            <Tools>
                                <ext:Tool Type="Refresh" Handler="MarkerManager.Refresh();" />
                            </Tools>
                            <Items>
                                <ext:GridPanel ID="GridPanel1" runat="server" StripeRows="true" Layout="AnchorLayout"
                                    StoreID="StoreZd">
                                    <TopBar>
                                        <ext:Toolbar runat="server" ID="maptoobar">
                                            <Items>
                                                <ext:Button ID="Button11" runat="server" Icon="MapGo" Text='<%# GetLangStr("KeyRoadManager19","显示施工占道信息") %>'>
                                                    <Listeners>
                                                        <Click Handler="SelectMap('ZD');" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <ColumnModel ID="ColumnModel2" runat="server">
                                        <Columns>
                                            <ext:Column Header='<%# GetLangStr("KeyRoadManager20","施工占道名称") %>' AutoDataBind="true" DataIndex="col11" Width="150" />
                                            <ext:Column DataIndex="col30" Header='<%# GetLangStr("KeyRoadManager21","标注情况") %>' AutoDataBind="true" Width="50">
                                                <Renderer Fn="change2" />
                                            </ext:Column>
                                            <ext:CommandColumn Width="60">
                                                <Commands>
                                                    <ext:GridCommand Icon="MapAdd" CommandName="Add">
                                                        <ToolTip Text='<%# GetLangStr("KeyRoadManager22","地图上标注") %>' AutoDataBind="true" />
                                                    </ext:GridCommand>
                                                    <ext:GridCommand Icon="MapDelete" CommandName="Delete">
                                                        <ToolTip Text='<%# GetLangStr("KeyRoadManager23","删除地点标注") %>' AutoDataBind="true" />
                                                    </ext:GridCommand>
                                                </Commands>
                                                <PrepareToolbar Fn="prepareZd" />
                                            </ext:CommandColumn>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                            <DirectEvents>
                                                <RowSelect OnEvent="RowZdSelect" Buffer="100">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="data" Value="record.data" Mode="Raw" />
                                                    </ExtraParams>
                                                </RowSelect>
                                            </DirectEvents>
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <Listeners>
                                        <Command Handler="MarkAndDelete(command, record.data,'ZD');" />
                                    </Listeners>
                                </ext:GridPanel>
                            </Items>
                        </ext:Panel>
                        <%--交通管制信息标注--%>
                        <ext:Panel ID="Panel4" runat="server" Title='<%# GetLangStr("KeyRoadManager24","交通管制信息标注") %>' IconCls="Hide-panel-header" Layout="Fit" Hidden="true">
                            <Tools>
                                <ext:Tool Type="Refresh" Handler="MarkerManager.Refresh();" />
                            </Tools>
                            <Items>
                                <ext:GridPanel ID="GridPanel2" runat="server" StripeRows="true" Layout="AnchorLayout"
                                    StoreID="StoreGz">
                                    <TopBar>
                                        <ext:Toolbar runat="server" ID="Toolbar1">
                                            <Items>
                                                <ext:Button ID="Button1" runat="server" Icon="MapGo" Text='<%# GetLangStr("KeyRoadManager25","显示交通管制信息") %>'>
                                                    <Listeners>
                                                        <Click Handler="SelectMap('GZ')" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:Toolbar>
                                    </TopBar>
                                    <ColumnModel ID="ColumnModel3" runat="server">
                                        <Columns>
                                            <ext:Column Header='<%# GetLangStr("KeyRoadManager26","交通管制名称") %>' AutoDataBind="true" DataIndex="col1" Width="150" />
                                            <ext:Column DataIndex="col12" Header="标注情况" Width="50">
                                                <Renderer Fn="change2" />
                                            </ext:Column>
                                            <ext:CommandColumn Width="60">
                                                <Commands>
                                                    <ext:GridCommand Icon="MapAdd" CommandName="Add">
                                                        <ToolTip Text='<%# GetLangStr("KeyRoadManager27","地图上标注") %>' />
                                                    </ext:GridCommand>
                                                    <ext:GridCommand Icon="MapDelete" CommandName="Delete">
                                                        <ToolTip Text='<%# GetLangStr("KeyRoadManager28","删除地点标注") %>' />
                                                    </ext:GridCommand>
                                                </Commands>
                                                <PrepareToolbar Fn="prepareGz" />
                                            </ext:CommandColumn>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" SingleSelect="true">
                                            <DirectEvents>
                                                <RowSelect OnEvent="RowGZSelect" Buffer="100">
                                                    <ExtraParams>
                                                        <ext:Parameter Name="data" Value="record.data" Mode="Raw" />
                                                    </ExtraParams>
                                                </RowSelect>
                                            </DirectEvents>
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <Listeners>
                                        <Command Handler="MarkAndDelete(command, record.data,'GZ');" />
                                    </Listeners>
                                </ext:GridPanel>
                            </Items>
                        </ext:Panel>
                    </Items>
                </ext:FormPanel>
            </Items>
        </ext:Viewport>
    </form>
    <script language="javascript" type="text/javascript">
        function SelectMap(type) {
            BMAP.Clear();
            MarkerManager.SelectMapTo(type);
        }
    </script>
    <script type="text/javascript">
        function MovePath(xPoint, yPoint) {
            BMAP.GotoXY(xPoint, yPoint);
        }
    </script>
</body>
</html>