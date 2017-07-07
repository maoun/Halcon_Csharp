<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StationManager.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.StationManager" ValidateRequest="false" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>地点监测点以及设备管理</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <link href="../Css/chooser.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Css/cabel-v1.css" type="text/css" />
    <style type="text/css">
        .bold-text {
            font-weight: bold;
            padding-left: 25px;
            font-size: 110%;
        }

        .x-grid-row-summary {
            color: #948d8e;
        }
    </style>
    <script type="text/javascript" language="javascript">
        var getRowClass = function (record, rowIndex, p, ds) {
            var reColor = "";
            if (record.data.col10 == "0") {

                reColor = "x-grid-row-summary";
            }

            return reColor;
        };
        var prepareCommand = function (grid, command, record, row) {
            if (command.command == 'Drive') {
                command.text = record.get("col9");
                if (record.get("col9") == '0') {
                    command.hidden = true;
                    command.hideMode = 'display';
                }
            }
            if (command.command == 'Driection') {
                command.text = record.get("col10");
                if (record.get("col10") == '0') {
                    command.hidden = true;
                    command.hideMode = 'display';
                }
            }
        };
    </script>
    <script type="text/javascript">
        var getTasks = function (tree) {
            var msg = [],
                  selNodes = tree.getChecked();
            msg.push("");
            Ext.each(selNodes, function (node) {
                if (!node.disabled) {
                    if (msg.length > 1) {
                        msg.push(",");
                    }
                    msg.push(node.id);
                }
            });
            msg.push("");
            GridData.setValue(msg.join(""));
            return msg.join("");
        };
        var CountrySelector = {
            add: function (source, destination) {
                source = source || GridQueryDevice;
                destination = destination || GridBindDevice;

                if (source.hasSelection()) {
                    var records = source.selModel.getSelections();
                    source.deleteSelected();
                    destination.store.add(records);
                }
            },
            addAll: function (source, destination) {
                source = source || GridQueryDevice;
                destination = destination || GridBindDevice;
                var records = source.store.getRange();
                source.store.removeAll();
                destination.store.add(records);
            },
            addByName: function (name) {
                if (!Ext.isEmpty(name)) {
                    var result = Store1.query("Name", name);

                    if (!Ext.isEmpty(result.items)) {
                        GridQueryDevice.store.remove(result.items[0]);
                        GridBindDevice.store.add(result.items[0]);
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
        var template = '<span style="color:{0};">{1}</span>';
        var devicesfsyRenderer = function (value) {
            if (value == "0") {
                return String.format(template, "green", "否");
            }
            else {
                return String.format(template, "red", "是");
            }
        }
        var devicesfsyRenderers = function (value) {
            if (value == "0") {
                value = "否";
            }
            if (value == "1") {

                value = "是"
            }
            return value;
        }
        var pctChange = function (value) {
            if (value == "0") {

                value = "否";
                return String.format(template, (value = "否") ? "green" : "red", value);
            }
            if (value == "1") {

                value = "是";
                return String.format(template, (value = "是") ? "green" : "red", value);
            }
        }
        function AddBindDeviceInfo() {

            var arrayObj1 = new Array();
            var arrayObj2 = new Array();
            var total = GridBindDevice.getStore().data.length  //数据行数

            if (total <= 0) {
                Ext.MessageBox.alert('提示', '请选择设备信息'); return false;
            }
            else {
                for (var i = 0; i < total; i++) {

                    arrayObj1[i] = GridBindDevice.getStore().data.items[i].data.col0;
                }
                StationManager.AddBindDeviceInfo(arrayObj1, total);
            }
        }
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridStation.view.findRowIndex(this.triggerElement),
                cellIndex = GridStation.view.findCellIndex(this.triggerElement),
                record = StoreLocaltionStation.getAt(rowIndex),
                fieldName = GridStation.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
        var showTip1 = function () {
            var rowIndex = GridDevice.view.findRowIndex(this.triggerElement),
                cellIndex = GridDevice.view.findCellIndex(this.triggerElement),
                record = GridStoreDevice.getAt(rowIndex),
                fieldName = GridDevice.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:Hidden ID="CurrentStationId" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Hidden ID="HideLocationId" runat="server" />
        <ext:Hidden ID="hideLocationName" runat="server" />
        <ext:Hidden ID="HideStation" runat="server" />
        <ext:Hidden ID="HideStationType" runat="server" />
        <ext:Hidden ID="CurrentDepart" runat="server">
        </ext:Hidden>
        <ext:Hidden ID="HideDirection" runat="server" />
        <ext:Hidden ID="HideDirDesc" runat="server" />
        <ext:Store ID="StoreSY" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreSBXH" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col3" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreLocation" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreJCD" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="Store1" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreDeviceType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreCardType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreComType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreNetworkType" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreSFSY" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="StationManager" />
        <ext:Store ID="Storedepart" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreClass" runat="server">
            <Reader>
                <ext:JsonReader>
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                        <ext:RecordField Name="col2" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreStation" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="Viewport2" runat="server">
            <Items>
                <ext:ColumnLayout ID="ColumnLayout2" runat="server" Split="true" FitHeight="true">
                    <Columns>
                        <ext:LayoutColumn ColumnWidth="0.2">
                            <%--部门树--%>
                            <ext:FormPanel ID="PanelNavigate" runat="server" Width="200" Header="true" Icon="Table"
                                MonitorValid="true">
                                <Items>
                                    <ext:TreePanel ID="TreeGrid1" runat="server" Title='<%# GetLangStr("StationManager1","机构关系") %>' Icon="House" NoLeafIcon="true"
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
                        <ext:LayoutColumn ColumnWidth="0.4">
                            <ext:FormPanel ID="FormPanel1" runat="server" Title='<%# GetLangStr("StationManager2","地点与监测点详细信息") %>' Icon="Table" Width="200"
                                MonitorValid="true" Layout="Fit">
                                <Items>
                                    <ext:GridPanel ID="GridStation" runat="server" EnableDragDrop="false">
                                        <TopBar>
                                            <ext:Toolbar ID="Toolbar4" runat="server">
                                                <Items>
                                                    <ext:Button ID="Button17" runat="server" Icon="SectionCollapsed" Text='<%# GetLangStr("StationManager3","收起展开") %>' Hidden="true">
                                                        <Listeners>
                                                            <Click Handler="#{GridStation}.getView().toggleAllGroups();" />
                                                        </Listeners>
                                                    </ext:Button>
                                                    <ext:Button ID="Button15" runat="server" Text='<%# GetLangStr("StationManager4","增加地点") %>' Icon="FolderAdd">
                                                        <DirectEvents>
                                                            <Click OnEvent="AddLoation" />
                                                        </DirectEvents>
                                                    </ext:Button>
                                                    <ext:Button ID="MenuModify" runat="server" Icon="FolderEdit" Text='<%# GetLangStr("StationManager5","修改地点") %>' Disabled="true">
                                                        <DirectEvents>
                                                            <Click OnEvent="ModifyLocation" />
                                                        </DirectEvents>
                                                    </ext:Button>
                                                    <ext:Button ID="MenuDelete" runat="server" Icon="FolderDelete" Text='<%# GetLangStr("StationManager6","删除地点")  %>' Disabled="true">
                                                        <DirectEvents>
                                                            <Click OnEvent="DeleteLocation" />
                                                        </DirectEvents>
                                                    </ext:Button>
                                                    <ext:Button ID="MenuStation" runat="server" Icon="MonitorDelete" Text='<%# GetLangStr("StationManager7","删除监测点") %>' Disabled="true">
                                                        <DirectEvents>
                                                            <Click OnEvent="DeleteStation" />
                                                        </DirectEvents>
                                                    </ext:Button>
                                                </Items>
                                            </ext:Toolbar>
                                        </TopBar>
                                        <Store>
                                            <ext:Store ID="StoreLocaltionStation" runat="server" GroupField="col3">
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
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                            </ext:Store>
                                        </Store>
                                        <Plugins>
                                            <ext:RowEditor ID="RowEditor5" runat="server" SaveText='<%# GetLangStr("StationManager8","更新") %>' CancelText='<%# GetLangStr("StationManager9","退出") %>'>
                                                <DirectEvents>
                                                    <AfterEdit OnEvent="UpdateStation">
                                                    </AfterEdit>
                                                </DirectEvents>
                                            </ext:RowEditor>
                                        </Plugins>
                                        <ColumnModel ID="ColumnModel6" runat="server">
                                            <Columns>
                                                <ext:RowNumbererColumn runat="server" Width="40"></ext:RowNumbererColumn>
                                                <ext:Column Header='<%# GetLangStr("StationManager10","外部编号") %>' AutoDataBind="true" DataIndex="col2" Width="100" Align="Center" Groupable="false">
                                                    <Editor>
                                                        <ext:TextField ID="TxtStationIdExt" runat="server" AllowBlank="false" EmptyText='<%#GetLangStr("StationManager11","例如：123456789012") %>' />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column Header='<%# GetLangStr("StationManager12","监测点名称") %>' AutoDataBind="true" DataIndex="col3" Width="250" Align="Right" Groupable="false">
                                                    <Editor>
                                                        <ext:TextField ID="TxtStationName" runat="server" AllowBlank="false" EmptyText='<%# GetLangStr("StationManager13","例如：西直门") %>' />
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column Header='<%# GetLangStr("StationManager14","监测点编号") %>' AutoDataBind="true" DataIndex="col2" Width="0" Align="Center" Hidden="true" Groupable="false">
                                                    <Editor>
                                                        <ext:Hidden ID="HidStationId" runat="server"></ext:Hidden>
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column Header='<%# GetLangStr("StationManager15","监测点类型") %>' AutoDataBind="true" DataIndex="col5" Width="100" Align="Center">
                                                </ext:Column>
                                                <ext:Column Header='<%# GetLangStr("StationManager16","监测点类型编号") %>' AutoDataBind="true" DataIndex="col4" Width="0" Align="Center" Hidden="true" Groupable="false">
                                                    <Editor>
                                                        <ext:Hidden ID="HidStationTypeId" runat="server"></ext:Hidden>
                                                    </Editor>
                                                </ext:Column>

                                                <ext:Column Header='<%# GetLangStr("StationManager17","地点编号") %>' AutoDataBind="true" DataIndex="col6" Width="300" Align="Center" Hidden="true" Groupable="false">
                                                    <Editor>
                                                        <ext:Hidden ID="HidLocationId" runat="server"></ext:Hidden>
                                                    </Editor>
                                                </ext:Column>
                                                <ext:Column Header='<%# GetLangStr("StationManager18","所属地点") %>' AutoDataBind="true" DataIndex="col7" Width="100" Align="Left" ColumnID="location">
                                                </ext:Column>
                                                <ext:ImageCommandColumn Width="80" Header='<%# GetLangStr("StationManager19","关联情况")  %>' AutoDataBind="true" Align="Right" Groupable="false">
                                                    <Commands>
                                                        <ext:ImageCommand Icon="Drive" CommandName="Drive">
                                                        </ext:ImageCommand>
                                                        <ext:ImageCommand Icon="ArrowNsew" CommandName="Driection">
                                                        </ext:ImageCommand>
                                                    </Commands>
                                                    <PrepareCommand Fn="prepareCommand" />
                                                </ext:ImageCommandColumn>
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
                                        <View>
                                            <ext:GroupingView ID="GroupingView1" runat="server" ForceFit="true" ShowGroupName="false"
                                                EnableGrouping="true" EnableNoGroups="true" HideGroupedColumn="true" GroupByText='<%# GetLangStr("StationManager20","用该列进行分组") %>'
                                                ShowGroupsText='<%# GetLangStr("StationManager21","显示分组") %>' StartCollapsed="true" GroupTextTpl='{text}({[values.rs.length]})'
                                                EnableRowBody="true">
                                                <GetRowClass Fn="getRowClass" />
                                            </ext:GroupingView>
                                        </View>
                                        <Plugins>
                                            <ext:GridFilters ID="GridFilters1" runat="server" Local="false" FiltersText='<%# GetLangStr("StationManager22","筛选条件") %>'>
                                                <Filters>
                                                    <ext:StringFilter DataIndex="col4"></ext:StringFilter>
                                                </Filters>
                                            </ext:GridFilters>
                                        </Plugins>
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
                            </ext:FormPanel>
                        </ext:LayoutColumn>
                        <ext:LayoutColumn ColumnWidth="0.4">
                            <ext:FormPanel ID="FormPanel5" runat="server" Region="East" Header="true" DefaultAnchor="100%"
                                MonitorValid="true" Layout="Fit">
                                <Items>
                                    <ext:RowLayout ID="RowLayout1" runat="server" Split="true">
                                        <Rows>
                                            <ext:LayoutRow RowHeight="0.4">
                                                <ext:GridPanel ID="GridPanelDirection" runat="server" Title='<%# GetLangStr("StationManager23","方向管理") %>' Border="true" StripeRows="true"
                                                    Height="160" Icon="ArrowNsew">
                                                    <TopBar>
                                                        <ext:Toolbar ID="Toolbar2" runat="server">
                                                            <Items>
                                                                <ext:Button ID="ButDirection" runat="server" Text='<%# GetLangStr("StationManager24","增加方向") %>' Icon="Add" ToolTip='<%# GetLangStr("StationManager25","增加") %>'>
                                                                    <DirectEvents>
                                                                        <Click OnEvent="ButDirection_Click" />
                                                                    </DirectEvents>
                                                                </ext:Button>
                                                                <ext:Button ID="ButDelDirection" runat="server" Text='<%# GetLangStr("StationManager26","删除方向") %>' Icon="Delete" ToolTip='<%# GetLangStr("StationManager27","删除") %>'>
                                                                    <Listeners>
                                                                        <Click Handler="StationManager.DoConfirmDirection()" />
                                                                    </Listeners>
                                                                </ext:Button>
                                                            </Items>
                                                        </ext:Toolbar>
                                                    </TopBar>
                                                    <Store>
                                                        <ext:Store ID="StoreDirectionInfo" runat="server">
                                                            <Reader>
                                                                <ext:JsonReader>
                                                                    <Fields>
                                                                        <ext:RecordField Name="col0" />
                                                                        <ext:RecordField Name="col1" />
                                                                        <ext:RecordField Name="col2" />
                                                                        <ext:RecordField Name="col3" />
                                                                        <ext:RecordField Name="col4" />
                                                                    </Fields>
                                                                </ext:JsonReader>
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                    <ColumnModel ID="ColumnModel3" runat="server">
                                                        <Columns>
                                                            <ext:RowNumbererColumn Width="40" />
                                                            <ext:Column Width="80" DataIndex="col3" Header='<%# GetLangStr("StationManager28","卡口编号") %>' AutoDataBind="true" Hidden="true" />
                                                            <ext:Column Width="80" DataIndex="col0" Header='<%# GetLangStr("StationManager29","方向编号") %>' AutoDataBind="true" Align="Center" />
                                                            <ext:Column DataIndex="col1" Width="90" Header='<%# GetLangStr("StationManager30","方向名称") %>' Align="Center">
                                                            </ext:Column>
                                                            <ext:Column DataIndex="col2" Width="200" Header='<%# GetLangStr("StationManager31","方向描述") %>' Align="Center">
                                                                <Editor>
                                                                    <ext:TextField ID="txtDirectionDesc" runat="server" AllowBlank="false" />
                                                                </Editor>
                                                            </ext:Column>
                                                        </Columns>
                                                    </ColumnModel>
                                                    <Plugins>
                                                        <ext:RowEditor ID="RowEditor3" runat="server" SaveText='<%# GetLangStr("StationManager32","更新") %>' CancelText="退出">
                                                            <DirectEvents>
                                                                <AfterEdit OnEvent="UpdateDirection" />
                                                            </DirectEvents>
                                                        </ext:RowEditor>
                                                    </Plugins>
                                                    <SelectionModel>
                                                        <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" SingleSelect="true">
                                                            <DirectEvents>
                                                                <RowSelect OnEvent="RowSelectDirection" Buffer="250">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="data" Value="record.data" Mode="Raw" />
                                                                    </ExtraParams>
                                                                </RowSelect>
                                                            </DirectEvents>
                                                        </ext:RowSelectionModel>
                                                    </SelectionModel>
                                                    <LoadMask ShowMask="true" />
                                                    <View>
                                                        <ext:GridView ID="GridView3" runat="server" ForceFit="true">
                                                        </ext:GridView>
                                                    </View>
                                                </ext:GridPanel>
                                            </ext:LayoutRow>
                                            <ext:LayoutRow RowHeight="0.6">
                                                <ext:GridPanel ID="GridDevice" TrackMouseOver="true" Title='<%# GetLangStr("StationManager33","设备关联系统") %>' runat="server" EnableDragDrop="false" Icon="Drive">
                                                    <TopBar>
                                                        <ext:Toolbar ID="Toolbar3" runat="server">
                                                            <Items>
                                                                <ext:Button ID="Button18" runat="server" Text='<%# GetLangStr("StationManager34","关联设备") %>' Icon="Add" ToolTip='<%# GetLangStr("StationManager35","增加") %>'>
                                                                    <DirectEvents>
                                                                        <Click OnEvent="ButDevice_Click" />
                                                                    </DirectEvents>
                                                                </ext:Button>
                                                                <ext:Button ID="Button19" runat="server" Text='<%# GetLangStr("StationManager36","移除关联") %>' Icon="Delete" ToolTip='<%# GetLangStr("StationManager37","删除") %>'>
                                                                    <Listeners>
                                                                        <Click Handler="StationManager.DoConfirmDevice()" />
                                                                    </Listeners>
                                                                </ext:Button>
                                                            </Items>
                                                        </ext:Toolbar>
                                                    </TopBar>
                                                    <Store>
                                                        <ext:Store ID="GridStoreDevice" runat="server">
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
                                                            <ext:RowNumbererColumn Width="40" />
                                                            <ext:Column Header='<%# GetLangStr("StationManager38","ID") %>' AutoDataBind="true" DataIndex="col0" Width="100" Align="Center" Hidden="true">
                                                            </ext:Column>
                                                            <ext:Column Header='<%# GetLangStr("StationManager39","设备名称") %>' AutoDataBind="true" DataIndex="col1" Width="150" Align="Center">
                                                            </ext:Column>
                                                            <ext:Column Header='<%# GetLangStr("StationManager40","设备类型") %>' AutoDataBind="true" DataIndex="col3" Width="100" Align="Center">
                                                            </ext:Column>
                                                            <ext:Column Header='<%# GetLangStr("StationManager41","设备型号") %>' AutoDataBind="true" DataIndex="col5" Width="100" Align="Center">
                                                            </ext:Column>
                                                            <ext:Column Header='<%# GetLangStr("StationManager42","设备IP") %>' AutoDataBind="true" DataIndex="col6" Width="100" Align="Center">
                                                            </ext:Column>
                                                            <ext:Column Header='<%# GetLangStr("StationManager43","设备端口") %>' AutoDataBind="true" DataIndex="col7" Width="70" Align="Center">
                                                            </ext:Column>
                                                        </Columns>
                                                    </ColumnModel>
                                                    <SelectionModel>
                                                        <ext:RowSelectionModel ID="RowSelectionModel5" runat="server" SingleSelect="true" />
                                                    </SelectionModel>
                                                    <SaveMask ShowMask="true" />
                                                    <View>
                                                        <ext:GridView ID="GridView2" runat="server" ForceFit="true">
                                                        </ext:GridView>
                                                    </View>
                                                    <ToolTips>
                                                        <ext:ToolTip
                                                            ID="ToolTip5"
                                                            runat="server"
                                                            Target="={GridDevice.getView().mainBody}"
                                                            Delegate=".x-grid3-cell"
                                                            TrackMouse="true">
                                                            <Listeners>
                                                                <Show Fn="showTip1" />
                                                            </Listeners>
                                                        </ext:ToolTip>
                                                    </ToolTips>
                                                </ext:GridPanel>
                                            </ext:LayoutRow>
                                        </Rows>
                                    </ext:RowLayout>
                                </Items>
                            </ext:FormPanel>
                        </ext:LayoutColumn>
                    </Columns>
                </ext:ColumnLayout>
            </Items>
        </ext:Viewport>
        <%--添加修改地点窗体--%>
        <ext:Window runat="server" Title='<%# GetLangStr("StationManager43","修改地点") %>' Height="200px" Width="300px" Hidden="true"
            ID="Window5">
            <Items>
                <ext:FormPanel runat="server" ID="FormPanel4" MonitorValid="true" Height="190px"
                    AnchorHorizontal="100%">
                    <Items>
                        <ext:TextField ID="Txt_UpdeDDBH" runat="server" FieldLabel='<%# GetLangStr("StationManager44","地点编号") %>' AllowBlank="false"
                            AnchorHorizontal="95%" EmptyText='<%# GetLangStr("StationManager45","例如:[201311021111111]") %>' />
                        <ext:TextField ID="Txt_UpdeDDMC" runat="server" FieldLabel='<%# GetLangStr("StationManager46","地点名称") %>' AllowBlank="false"
                            AnchorHorizontal="95%" EmptyText='<%# GetLangStr("StationManager47","例如:西直门北大街电警") %>' />
                    </Items>
                    <Listeners>
                        <ClientValidation Handler="Button6.setDisabled(!valid);" />
                    </Listeners>
                </ext:FormPanel>
            </Items>
            <Buttons>
                <ext:Button ID="Button6" runat="server" Icon="Add" Text='<%# GetLangStr("StationManager48","确定") %>'>
                    <DirectEvents>
                        <Click OnEvent="ModifyLocation" />
                    </DirectEvents>
                </ext:Button>
                <ext:Button ID="Button14" runat="server" Icon="TextItalic" Text='<%# GetLangStr("StationManager49","取消") %>'>
                    <Listeners>
                        <Click Handler="#{Window5}.hide();" />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>
        <%--设备关联窗体--%>
        <ext:Window ID="Window4" runat="server" Hidden="true" Height="553" Width="1300" Title='<%# GetLangStr("StationManager50","设备关联") %>'
            Padding="5">
            <TopBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <ext:Label ID="Label4" runat="server" Text='<%# GetLangStr("StationManager51","设备名称：") %>' StyleSpec="margin-left:10pox;">
                        </ext:Label>
                        <ext:TextField ID="TxtDeviceName" runat="server" Width="100" />
                        <ext:Label ID="Label6" runat="server" Text='<%# GetLangStr("StationManager52","设备IP：") %>' StyleSpec="margin-left:10px;">
                        </ext:Label>
                        <ext:TextField ID="TxtDeviceIP" runat="server" Width="100" />
                        <ext:Label ID="Label7" runat="server" Text='<%#GetLangStr("StationManager53","设备类型：") %>'>
                        </ext:Label>
                        <ext:ComboBox ID="CmbDeviceType" runat="server" Editable="false" StoreID="StoreDeviceType"
                            DisplayField="col1" ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true"
                            EmptyText='<%# GetLangStr("StationManager54","选择设备类型...") %>' SelectOnFocus="true" Width="123">
                            <Listeners>
                                <Select Handler="StationManager.SelectDeviceType()" />
                            </Listeners>
                        </ext:ComboBox>
                        <ext:Label ID="Label5" runat="server" Text='<%# GetLangStr("StationManager55","设备型号：") %>'>
                        </ext:Label>
                        <ext:ComboBox ID="CmbSBXH" runat="server" Editable="false" StoreID="StoreSBXH" DisplayField="col3"
                            ValueField="col0" TypeAhead="true" Mode="Local" ForceSelection="true" EmptyText='<%# GetLangStr("StationManager56","选择设备型号...") %>'
                            SelectOnFocus="true" Width="123">
                        </ext:ComboBox>
                        <ext:Button ID="TbutQuery" runat="server" Icon="ControlPlayBlue" Text='<%# GetLangStr("StationManager57","查询") %>'>
                            <DirectEvents>
                                <Click OnEvent="TbutQueryClick" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:Button ID="ButReset" runat="server" Icon="ControlBlank" Text='<%# GetLangStr("StationManager58","重置") %>'>
                            <DirectEvents>
                                <Click OnEvent="ButResetClick" />
                            </DirectEvents>
                        </ext:Button>
                        <ext:ToolbarFill></ext:ToolbarFill>
                        <ext:Button ID="Button12" runat="server" Text='<%# GetLangStr("StationManager59","确认") %>' Icon="Disk">
                            <Listeners>
                                <Click Handler="AddBindDeviceInfo()" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="Button13" runat="server" Text='<%# GetLangStr("StationManager60","取消") %>' Icon="Cancel">
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
                            <ext:GridPanel Title='<%# GetLangStr("StationManager61","未绑定的设备") %>' ID="GridQueryDevice" runat="server" EnableDragDrop="false">
                                <Store>
                                    <ext:Store ID="StoreQueryDevice" runat="server">
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
                                                    <ext:RecordField Name="col29" Type="String" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                    </ext:Store>
                                </Store>
                                <ColumnModel ID="ColumnModel1" runat="server">
                                    <Columns>
                                        <ext:RowNumbererColumn />
                                        <ext:Column Header='<%# GetLangStr("StationManager62","设备ID") %>' AutoDataBind="true" DataIndex="col0" Width="100" Align="Center" Hidden="true" />
                                        <ext:Column Header='<%# GetLangStr("StationManager40","设备类型") %>' AutoDataBind="true" DataIndex="col3" Width="100" Align="Center">
                                        </ext:Column>
                                        <ext:Column Header='<%# GetLangStr("StationManager39","设备名称") %>' AutoDataBind="true" DataIndex="col1" Width="250" Align="Center">
                                        </ext:Column>
                                        <ext:Column Header='<%# GetLangStr("StationManager42","设备IP") %>' AutoDataBind="true" DataIndex="col6" Width="80" Align="Center" />
                                        <ext:Column Header='<%# GetLangStr("StationManager66","设备是否绑定") %>' AutoDataBind="true" DataIndex="col9" Width="80" Align="Center">
                                            <Renderer Fn="devicesfsyRenderer" />
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:CheckboxSelectionModel ID="CheckboxSelectionModel1" runat="server" />
                                </SelectionModel>
                                <Plugins>
                                    <ext:GridFilters ID="GridFilters2" runat="server" Local="false" FiltersText='<%# GetLangStr("StationManager67","筛选条件") %>'>
                                        <Filters>
                                            <ext:StringFilter DataIndex="col2" />
                                        </Filters>
                                    </ext:GridFilters>
                                </Plugins>
                                <View>
                                    <ext:GridView ID="GridView1" runat="server" ForceFit="true">
                                    </ext:GridView>
                                </View>
                            </ext:GridPanel>
                        </ext:LayoutColumn>
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
                                                    <ext:ToolTip ID="ToolTip1" runat="server" Title='<%# GetLangStr("StationManager68","增加") %>' Html='<%# GetLangStr("StationManager69","增加选择行") %>' />
                                                </ToolTips>
                                            </ext:Button>
                                            <ext:Button ID="Button9" runat="server" Icon="ResultsetLast" StyleSpec="margin-bottom:2px;">
                                                <Listeners>
                                                    <Click Handler="CountrySelector.addAll();" />
                                                </Listeners>
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip2" runat="server" Title='<%# GetLangStr("StationManager70","增加所有") %>' Html='<%# GetLangStr("StationManager71","增加所有行") %>' />
                                                </ToolTips>
                                            </ext:Button>
                                            <ext:Button ID="Button10" runat="server" Icon="ResultsetPrevious" StyleSpec="margin-bottom:2px;">
                                                <Listeners>
                                                    <Click Handler="CountrySelector.remove(GridQueryDevice, GridBindDevice);" />
                                                </Listeners>
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip3" runat="server" Title='<%# GetLangStr("StationManager72","移除") %>' Html='<%# GetLangStr("StationManager73","移除当前行") %>' />
                                                </ToolTips>
                                            </ext:Button>
                                            <ext:Button ID="Button11" runat="server" Icon="ResultsetFirst" StyleSpec="margin-bottom:2px;">
                                                <Listeners>
                                                    <Click Handler="CountrySelector.removeAll(GridQueryDevice, GridBindDevice);" />
                                                </Listeners>
                                                <ToolTips>
                                                    <ext:ToolTip ID="ToolTip4" runat="server" Title='<%# GetLangStr("StationManager74","移除所有") %>' Html='<%# GetLangStr("StationManager75","移除所有行") %>' />
                                                </ToolTips>
                                            </ext:Button>
                                        </Items>
                                    </ext:Panel>
                                </Items>
                            </ext:Panel>
                        </ext:LayoutColumn>
                        <ext:LayoutColumn ColumnWidth="0.38">
                            <ext:GridPanel ID="GridBindDevice" Title='<%# GetLangStr("StationManager76","已绑定的设备") %>' runat="server" EnableDragDrop="false">
                                <Store>
                                    <ext:Store ID="StoreBindDevice" runat="server">
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
                                <Plugins>
                                    <ext:RowEditor ID="RowEditor1" runat="server" SaveText='<%# GetLangStr("StationManager77","更新") %>' CancelText='<%# GetLangStr("StationManager78","退出") %>'>
                                        <DirectEvents>
                                            <AfterEdit OnEvent="UpdateDeviceData" />
                                        </DirectEvents>
                                    </ext:RowEditor>
                                </Plugins>
                                <ColumnModel ID="ColumnModel2" runat="server">
                                    <Columns>
                                        <ext:Column Header='<%# GetLangStr("StationManager62","设备ID") %>' AutoDataBind="true" DataIndex="col0" Width="100" Align="Center" Hidden="true" />
                                        <ext:Column Header='<%# GetLangStr("StationManager40","设备类型") %>' AutoDataBind="true" DataIndex="col3" Width="100" Align="Center">
                                        </ext:Column>
                                        <ext:Column Header='<%# GetLangStr("StationManager39","设备名称") %>' AutoDataBind="true" DataIndex="col1" Width="250" Align="Center">
                                        </ext:Column>
                                        <ext:Column Header='<%# GetLangStr("StationManager42","设备IP") %>' AutoDataBind="true" DataIndex="col6" Width="100" Align="Center">
                                        </ext:Column>
                                    </Columns>
                                </ColumnModel>
                                <SelectionModel>
                                    <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" SingleSelect="true" />
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