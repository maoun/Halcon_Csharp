<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KeyRoadManager.aspx.cs" Inherits="MyNet.Atmcs.Uscmcp.Web.KeyRoadManager" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <title>重点道路管理</title>
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <script type="text/javascript">
        var CountrySelector = {
            add: function (source, destination) {
                source = source || GridAllRoad;
                destination = destination || GridKeyRoad;

                if (source.hasSelection()) {
                    var records = source.selModel.getSelections();
                    source.deleteSelected();
                    destination.store.add(records);
                }
            },
            addAll: function (source, destination) {
                source = source || GridAllRoad;
                destination = destination || GridKeyRoad;
                var records = source.store.getRange();
                source.store.removeAll();
                destination.store.add(records);
            },
            addByName: function (name) {
                if (!Ext.isEmpty(name)) {
                    var result = Store1.query("Name", name);

                    if (!Ext.isEmpty(result.items)) {
                        GridAllRoad.store.remove(result.items[0]);
                        GridKeyRoad.store.add(result.items[0]);
                    }
                }
            },
            addByNames: function (name) {
                for (var i = 0; i < name.length; i++) {
                    this.addByName(name[i]);
                }
            },
            remove: function (source, destination) {
                this.add(destination, source);
            },
            removeAll: function (source, destination) {
                this.addAll(destination, source);
            }
        };

        function AddKeyRoadInfo() {

            var arrayObj1 = new Array();
            var arrayObj2 = new Array();
            var total = GridKeyRoad.getStore().data.length  //数据行数

            if (total <= 0) {
                Ext.MessageBox.alert('提示', '请选择道路信息'); return false;
            }
            else {
                for (var i = 0; i < total; i++) {

                    arrayObj1[i] = GridKeyRoad.getStore().data.items[i].data.col0;
                }
                KeyRoadManager.AddKeyRoadInfo(arrayObj1, total);
            }
        }
    </script>
    <script type="text/javascript">
        var CountrySelector2 = {
            add: function (source, destination) {
                source = source || GridQueryStation;
                destination = destination || GridBindStation;

                if (source.hasSelection()) {
                    var records = source.selModel.getSelections();
                    source.deleteSelected();
                    destination.store.add(records);
                }
            },
            addAll: function (source, destination) {
                source = source || GridQueryStation;
                destination = destination || GridBindStation;
                var records = source.store.getRange();
                source.store.removeAll();
                destination.store.add(records);
            },
            remove: function (source, destination) {
                this.add(destination, source);
            },
            removeAll: function (source, destination) {
                this.addAll(destination, source);
            }
        };
        var template = '<span style="color:{0};">{1}</span>';
        var stationsfsyRenderer = function (value) {
            if (value == "0") {
                return String.format(template, "green", "否");
            }
            else {
                return String.format(template, "red", "是");
            }
        }

        function AddBindStationInfo() {

            var arrayObj1 = new Array();
            var arrayObj2 = new Array();
            var total = GridBindStation.getStore().data.length  //数据行数

            if (total <= 0) {
                Ext.MessageBox.alert('提示', '请选择监测点信息'); return false;
            }
            else {
                for (var i = 0; i < total; i++) {

                    arrayObj1[i] = GridBindStation.getStore().data.items[i].data.col0;
                }
                KeyRoadManager.AddBindStationInfo(arrayObj1, total);
            }
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
</head>
<body>
    <form id="form1" runat="server">
        <ext:Hidden ID="HideRoad" runat="server" />
        <ext:Store ID="StoreStationType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>

        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="KeyRoadManager" />
        <ext:Viewport ID="Viewport1" runat="server" Layout="border">
            <Items>
                <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <%--部门树--%>
                        <ext:LayoutColumn ColumnWidth="0.2">
                            <ext:FormPanel ID="PanelNavigate" runat="server" Width="250" Header="true" Icon="Table"
                                MonitorValid="true">
                                <Items>
                                    <ext:TreePanel ID="TreeGrid1" runat="server" Title='<%# GetLangStr("KeyRoadManager1","机构关系") %>' Icon="House" NoLeafIcon="true"
                                        ContainerScroll="true" UseArrows="true" EnableDD="true" Animate="true" Border="false"
                                        AutoScroll="false">
                                        <SelectionModel>
                                            <ext:DefaultSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                                <DirectEvents>
                                                    <SelectionChange OnEvent="RowSelect" Buffer="250">
                                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{PanelNavigate}" />
                                                        <ExtraParams>
                                                            <ext:Parameter Name="id" Value="node.id" Mode="Raw" />
                                                        </ExtraParams>
                                                    </SelectionChange>
                                                </DirectEvents>
                                            </ext:DefaultSelectionModel>
                                        </SelectionModel>
                                        <Root>
                                        </Root>
                                    </ext:TreePanel>
                                </Items>
                            </ext:FormPanel>
                        </ext:LayoutColumn>
                        <%--道路列表--%>
                        <ext:LayoutColumn ColumnWidth="0.25">
                            <ext:GridPanel Title='<%# GetLangStr("KeyRoadManager2","道路列表") %>' ID="GridAllRoad" runat="server" EnableDragDrop="false">
                                <TopBar>
                                    <ext:Toolbar runat="server">
                                        <Items>
                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:Button ID="btnShowAll" runat="server" Text='<%# GetLangStr("KeyRoadManager3","展示所有") %>' Icon="ApplicationGet">
                                                <Listeners>
                                                    <Click Handler="KeyRoadManager.GetAllDlInfo()" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="StoreAllRoad" runat="server">
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
                                                    <ext:RecordField Name="col7" Type="String" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:RowNumbererColumn Width="40" />
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager4","道路编号") %>' AutoDataBind="true" DataIndex="col0" Width="100" Align="Center" Hidden="true" />
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager5","道路名称") %>' AutoDataBind="true" DataIndex="col2" Width="200" Align="Center" />
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager6","道路类型") %>' AutoDataBind="true" DataIndex="col7" Width="100" Align="Center">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" />
                                </SelectionModel>
                                <View>
                                    <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                                    </ext:GridView>
                                </View>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                        <%--移动按键--%>
                        <ext:LayoutColumn>
                            <ext:Panel ID="Panel1" runat="server" ColumnWidth=".03" BodyStyle="background-color: transparent;"
                                Border="false" Layout="Anchor">
                                <Items>
                                    <ext:Panel ID="Panel3" runat="server" Border="false" BodyStyle="background-color: transparent;"
                                        AnchorVertical="40%" />
                                    <ext:Panel ID="Panel4" runat="server" Border="false" BodyStyle="background-color: transparent;"
                                        Padding="5">
                                        <Items>
                                            <ext:Button ID="Button8" runat="server" Icon="ResultsetNext" StyleSpec="margin-bottom:2px;">
                                                <Listeners>
                                                    <Click Handler="CountrySelector.add();" />
                                                </Listeners>
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip1" runat="server" Title='<%# GetLangStr("KeyRoadManager7","增加") %>' Html='<%# GetLangStr("KeyRoadManager8","增加选择行") %>' />
                                                </ToolTips>
                                            </ext:Button>
                                            <ext:Button ID="Button9" runat="server" Icon="ResultsetLast" StyleSpec="margin-bottom:2px;">
                                                <Listeners>
                                                    <Click Handler="CountrySelector.addAll();" />
                                                </Listeners>
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip2" runat="server" Title='<%# GetLangStr("KeyRoadManager9","增加所有") %>' Html='<%# GetLangStr("KeyRoadManager10","增加所有行") %>' />
                                                </ToolTips>
                                            </ext:Button>
                                            <ext:Button ID="Button10" runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                                                <Listeners>
                                                    <Click Handler="CountrySelector.remove(GridAllRoad, GridKeyRoad);" />
                                                </Listeners>
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip3" runat="server" Title='<%# GetLangStr("KeyRoadManager11","移除") %>' Html='<%# GetLangStr("KeyRoadManager12","移除当前行") %>' />
                                                </ToolTips>
                                            </ext:Button>
                                            <ext:Button ID="Button11" runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                                                <Listeners>
                                                    <Click Handler="CountrySelector.removeAll(GridAllRoad, GridKeyRoad);" />
                                                </Listeners>
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip4" runat="server" Title='<%# GetLangStr("KeyRoadManager13","移除所有") %>' Html='<%# GetLangStr("KeyRoadManager14","移除所有行") %>' />
                                                </ToolTips>
                                            </ext:Button>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                        </ext:LayoutColumn>
                        <%--重点道路--%>
                        <ext:LayoutColumn ColumnWidth="0.25">
                            <ext:GridPanel ID="GridKeyRoad" Title='<%# GetLangStr("KeyRoadManager15","重点道路") %>' runat="server" EnableDragDrop="false">
                                <TopBar>
                                    <ext:Toolbar runat="server">
                                        <Items>
                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:Button ID="ButSave" runat="server" Text='<%# GetLangStr("KeyRoadManager16","保存") %>' Icon="TableSave">
                                                <Listeners>
                                                    <Click Handler="AddKeyRoadInfo()" />
                                                </Listeners>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="StoreKeyRoad" runat="server">
                                        <Reader>
                                            <ext:JsonReader IDProperty="col0">
                                                <Fields>
                                                    <ext:RecordField Name="col0" Type="String" />
                                                    <ext:RecordField Name="col1" Type="String" />
                                                    <ext:RecordField Name="col2" Type="String" />
                                                    <ext:RecordField Name="col3" Type="String" />
                                                    <ext:RecordField Name="col4" Type="String" />
                                                    <ext:RecordField Name="col5" Type="String" />
                                                    <ext:RecordField Name="col6" Type="String" />
                                                    <ext:RecordField Name="col7" Type="String" />
                                                    <ext:RecordField Name="col7" Type="String" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>
                                        <ext:RowNumbererColumn Width="40" />
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager17","道路编号") %>' AutoDataBind="true" DataIndex="col0" Width="100" Align="Center" Hidden="true" />
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager18","道路名称") %>' AutoDataBind="true" DataIndex="col2" Width="200" Align="Center" />
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager19","道路类型") %>' AutoDataBind="true" DataIndex="col7" Width="100" Align="Center" />
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel6" runat="server" SingleSelect="true">
                                        <DirectEvents>
                                            <RowSelect OnEvent="ApplyClick" Buffer="250">
                                                <ExtraParams>
                                                    <ext:Parameter Name="data" Value="record.data" Mode="Raw" />
                                                </ExtraParams>
                                            </RowSelect>
                                        </DirectEvents>
                                    </ext:RowSelectionModel>
                                </SelectionModel>
                                <SaveMask ShowMask="true" />
                                <View>
                                    <ext:GridView ID="GridView2" runat="server" ForceFit="true">
                                    </ext:GridView>
                                </View>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                        <%--  监测点--%>
                        <ext:LayoutColumn ColumnWidth="0.25">
                            <ext:GridPanel ID="GridStation" Title='<%# GetLangStr("KeyRoadManager20","关联监测点") %>' runat="server" EnableDragDrop="false" TrackMouseOver="true">
                                <TopBar>
                                    <ext:Toolbar runat="server">
                                        <Items>
                                            <ext:ToolbarFill></ext:ToolbarFill>
                                            <ext:Button ID="btnAddStation" runat="server" Text='<%# GetLangStr("KeyRoadManager20","关联监测点") %>' Icon="Add">
                                                <DirectEvents>
                                                    <Click OnEvent="btnAddStation_Click" />
                                                </DirectEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Store>
                                    <ext:Store ID="StoreStation" runat="server">
                                        <Reader>
                                            <ext:JsonReader IDProperty="col0">
                                                <Fields>
                                                    <ext:RecordField Name="col0" Type="String" />
                                                    <ext:RecordField Name="col1" Type="String" />
                                                    <ext:RecordField Name="col2" Type="String" />
                                                    <ext:RecordField Name="col3" Type="String" />
                                                    <ext:RecordField Name="col4" Type="String" />
                                                    <ext:RecordField Name="col5" Type="String" />
                                                    <ext:RecordField Name="col6" Type="String" />
                                                    <ext:RecordField Name="col7" Type="String" />
                                                    <ext:RecordField Name="col7" Type="String" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <ColumnModel ID="ColumnModel3" runat="server">
                                    <Columns>
                                        <ext:RowNumbererColumn Width="40" />
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager22","监测点编号") %>' AutoDataBind="true" DataIndex="col0" Width="100" Align="Center" Hidden="true" />
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager23","监测点名称") %>' AutoDataBind="true" DataIndex="col1" Width="200" Align="Center" />
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager24","监测点类型") %>' AutoDataBind="true" DataIndex="col2" Width="100" Align="Center" />
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" SingleSelect="true" />
                                </SelectionModel>
                                <SaveMask ShowMask="true" />
                                <View>
                                    <ext:GridView ID="GridView3" runat="server" ForceFit="true">
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
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Items>
        </ext:Viewport>
        <%--监测点关联窗体--%>
        <ext:Window ID="Window4" runat="server" Hidden="true" Height="553" Width="1300" Title='<%# GetLangStr("KeyRoadManager25","监测点关联") %>'
            Padding="5">
            <TopBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <ext:Label ID="Label4" runat="server" Text='<%# GetLangStr("KeyRoadManager26","监测点名称：") %>'>
                        </ext:Label>
                        <ext:TextField ID="TxtStationName" runat="server" Width="300" />
                        <ext:Label ID="Label7" runat="server" Text='<%# GetLangStr("KeyRoadManager27","监测点类型：") %>'>
                        </ext:Label>
                        <ext:ComboBox ID="CmbStationType" runat="server" Editable="false" StoreID="StoreStationType"
                            DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                            EmptyText='<%# GetLangStr("KeyRoadManager28","选择监测点类型...") %>' SelectOnFocus="true" Width="123">
                        </ext:ComboBox>
                        <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("KeyRoadManager29","查询") %>'>
                            <DirectEvents>
                                <Click OnEvent="TbutQueryClick" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("KeyRoadManager30","重置") %>'>
                            <DirectEvents>
                                <Click OnEvent="ButResetClick" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:ToolbarFill></ext:ToolbarFill>
                        <ext:Button ID="Button12" runat="server" Text='<%# GetLangStr("KeyRoadManager31","确认") %>' Icon="Disk">
                            <Listeners>
                                <Click Handler="AddBindStationInfo()" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="Button13" runat="server" Text='<%# GetLangStr("KeyRoadManager32","取消") %>' Icon="Cancel">
                            <Listeners>
                                <Click Handler="#{Window4}.hide();" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <Items>
                <ext:ColumnLayout ID="ColumnLayout1" runat="server" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="0.59">
                            <ext:GridPanel Title='<%# GetLangStr("KeyRoadManager33","未绑定的监测点") %>' ID="GridQueryStation" runat="server" EnableDragDrop="false">
                                <Store>
                                    <ext:Store ID="StoreQueryStation" runat="server">
                                        <Reader>
                                            <ext:JsonReader>
                                                <Fields>
                                                    <ext:RecordField Name="col0" Type="String" />
                                                    <ext:RecordField Name="col1" Type="String" />
                                                    <ext:RecordField Name="col2" Type="String" />
                                                    <ext:RecordField Name="col3" Type="String" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <ColumnModel ID="ColumnModel4" runat="server">
                                    <Columns>
                                        <ext:RowNumbererColumn Width="40" />
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager34","监测点ID") %>' AutoDataBind="true" DataIndex="col0" Width="100" Align="Center" Hidden="true" />
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager35","监测点类型") %>' AutoDataBind="true" DataIndex="col2" Width="100" Align="Center">
                                        </ext:Column>
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager36","监测点名称") %>' AutoDataBind="true" DataIndex="col1" Width="250" Align="Left">
                                        </ext:Column>
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager37","监测点是否绑定") %>' AutoDataBind="true" DataIndex="col3" Width="80" Align="Center">
                                            <Renderer Fn="stationsfsyRenderer" />
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel2" runat="server" />
                                </SelectionModel>
                                <Plugins>
                                    <ext:GridFilters ID="GridFilters2" runat="server" Local="false" FiltersText='<%# GetLangStr("KeyRoadManager38","筛选条件") %>'>
                                        <Filters>
                                            <ext:StringFilter DataIndex="col2" />
                                        </Filters>
                                    </ext:GridFilters>
                                </Plugins>
                                <View>
                                    <ext:GridView ID="GridView4" runat="server" ForceFit="true">
                                    </ext:GridView>
                                </View>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                        <ext:LayoutColumn>
                            <ext:Panel ID="Panel2" runat="server" ColumnWidth=".03" BodyStyle="background-color: transparent;"
                                Border="false" Layout="Anchor">
                                <Items>
                                    <ext:Panel ID="Panel5" runat="server" Border="false" BodyStyle="background-color: transparent;"
                                        AnchorVertical="40%" />
                                    <ext:Panel ID="Panel6" runat="server" Border="false" BodyStyle="background-color: transparent;"
                                        Padding="5">
                                        <Items>
                                            <ext:Button ID="Button1" runat="server" Icon="ResultsetNext" StyleSpec="margin-bottom:2px;">
                                                <Listeners>
                                                    <Click Handler="CountrySelector2.add();" />
                                                </Listeners>
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip5" runat="server" Title='<%# GetLangStr("KeyRoadManager39","增加") %>' Html='<%# GetLangStr("KeyRoadManager40","增加选择行") %>' />
                                                </ToolTips>
                                            </ext:Button>
                                            <ext:Button ID="Button2" runat="server" Icon="ResultsetLast" StyleSpec="margin-bottom:2px;">
                                                <Listeners>
                                                    <Click Handler="CountrySelector2.addAll();" />
                                                </Listeners>
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip6" runat="server" Title='<%# GetLangStr("KeyRoadManager41","增加所有") %>' Html='<%# GetLangStr("KeyRoadManager42","增加所有行") %>' />
                                                </ToolTips>
                                            </ext:Button>
                                            <ext:Button ID="Button3" runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                                                <Listeners>
                                                    <Click Handler="CountrySelector2.remove(GridQueryStation, GridBindStation);" />
                                                </Listeners>
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip7" runat="server" Title='<%# GetLangStr("KeyRoadManager43","移除") %>' Html='<%# GetLangStr("KeyRoadManager44","移除当前行") %>' />
                                                </ToolTips>
                                            </ext:Button>
                                            <ext:Button ID="Button4" runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                                                <Listeners>
                                                    <Click Handler="CountrySelector2.removeAll(GridQueryStation, GridBindStation);" />
                                                </Listeners>
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip8" runat="server" Title='<%# GetLangStr("KeyRoadManager45","移除所有") %>' Html='<%# GetLangStr("KeyRoadManager46","移除所有行") %>' />
                                                </ToolTips>
                                            </ext:Button>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                        </ext:LayoutColumn>
                        <ext:LayoutColumn ColumnWidth="0.38">
                            <ext:GridPanel ID="GridBindStation" Title='<%# GetLangStr("KeyRoadManager47","已绑定的监测点") %>' runat="server" EnableDragDrop="false">
                                <Store>
                                    <ext:Store ID="StoreBindStation" runat="server">
                                        <Reader>
                                            <ext:JsonReader IDProperty="col0">
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
                                                    <ext:RecordField Name="col14" Type="String" />
                                                    <ext:RecordField Name="col15" Type="String" />
                                                    <ext:RecordField Name="col16" Type="String" />
                                                    <ext:RecordField Name="col17" Type="String" />
                                                    <ext:RecordField Name="col18" Type="String" />
                                                    <ext:RecordField Name="col19" Type="String" />
                                                    <ext:RecordField Name="col20" Type="String" />
                                                    <ext:RecordField Name="col21" Type="String" />
                                                    <ext:RecordField Name="col22" Type="String" />
                                                    <ext:RecordField Name="col23" Type="String" />
                                                    <ext:RecordField Name="col24" Type="String" />
                                                    <ext:RecordField Name="col25" Type="String" />
                                                    <ext:RecordField Name="col26" Type="String" />
                                                    <ext:RecordField Name="col27" Type="String" />
                                                    <ext:RecordField Name="col28" Type="String" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <ColumnModel ID="ColumnModel5" runat="server">
                                    <Columns>
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager34","监测点ID") %>' DataIndex="col0" Width="100" Align="Center" Hidden="true" />
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager35","监测点类型") %>' DataIndex="col2" Width="100" Align="Center">
                                        </ext:Column>
                                        <ext:Column Header='<%# GetLangStr("KeyRoadManager36","监测点名称") %>' DataIndex="col1" Width="250" Align="Left">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel4" runat="server" SingleSelect="true" />
                                </SelectionModel>
                                <SaveMask ShowMask="true" />
                            </ext:GridPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Items>
        </ext:Window>
    </form>
</body>
</html>