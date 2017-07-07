<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TmsStationManager.aspx.cs"
    Inherits="MyNet.Atmcs.Uscmcp.Web.TmsStationManager" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>监测点管理</title>
    <meta http-equiv="Content-Type" content="text/html" charset="GBK" />
    <script type="text/javascript">
        var getTasks = function (tree) {
            var msg = [],
           selNodes = tree.getChecked();
            msg.push("");
            Ext.each(selNodes, function (node) {
                if (msg.length > 1) {
                    msg.push(",");
                }
                msg.push(node.id);
            });
            msg.push("");
            GridData.setValue(msg.join(""));
            return msg.join("");
        };
    </script>
    <script type="text/javascript">
        function ClearCheckState() {
            TreePanel1.clearChecked();
            getCheckValue();
        }
        function SetCheckState(id) {
            var selNodes = TreePanel1.getRootNode();
            findchildnode(selNodes, id);
            getCheckValue();
        }
        function findchildnode(node, id) {
            var childnodes = node.childNodes;
            var nd;
            for (var i = 0; i < childnodes.length; i++) { //从节点中取出子节点依次遍历
                nd = childnodes[i];

                if (nd.id == id) {
                    nd.getUI().toggleCheck(true);
                }
                if (nd.hasChildNodes()) { //判断子节点下是否存在子节点
                    findchildnode(nd, id); //如果存在子节点 递归
                }
            }
        }
        function getCheckValue() {
            var selNodes = TreePanel1.getChecked();
            var nd;
            var nodevalue = "";
            for (var i = 0; i < selNodes.length; i++) { //从节点中取出子节点依次遍历
                nd = selNodes[i];
                nodevalue += nd.id + ",";
            }
            nodevalue = nodevalue.substr(0, nodevalue.length - 1);
            GridData.setValue(nodevalue);
        }
        var departmentRenderer = function (value) {
            if (!Ext.isEmpty(value)) {
                return value;
            }

            return value;
        };
        var stationtypeRenderer = function (value) {
            if (!Ext.isEmpty(value)) {
                return value;
            }

            return value;
        };
        var locationtypeRenderer = function (value) {
            if (!Ext.isEmpty(value)) {
                return value;
            }

            return value;
        };
        var devicetypeRenderer = function (value) {
            if (!Ext.isEmpty(value)) {
                return value;
            }

            return value;
        };
        var comparytypeRenderer = function (value) {
            if (!Ext.isEmpty(value)) {
                return value;
            }

            return value;
        };
        var devicestationtypeRenderer = function (value) {
            if (!Ext.isEmpty(value)) {
                return value;
            }

            return value;
        };
        var cameratypeRenderer = function (value) {
            if (!Ext.isEmpty(value)) {
                return value;
            }

            return value;
        };

        var stationshowRenderer = function (value) {
            if (!Ext.isEmpty(value)) {
                return value;
            }

            return value;
        };
        var devicescanRenderer = function (value) {
            if (!Ext.isEmpty(value)) {
                return value;
            }

            return value;
        };
        //鼠标悬浮在行上有提示信息（展示的时候）
        var showTip = function () {
            var rowIndex = GridStation.view.findRowIndex(this.triggerElement),
                cellIndex = GridStation.view.findCellIndex(this.triggerElement),
                record = StoreStationId.getAt(rowIndex),
                fieldName = GridStation.getColumnModel().getDataIndex(cellIndex),
                data = record.get(fieldName);

            this.body.dom.innerHTML = data;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="SystemStation" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:Store ID="StoreCombo" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreType" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="CODE">
                    <Fields>
                        <ext:RecordField Name="CODEDESC" />
                        <ext:RecordField Name="CODE" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreLocation" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreStaion" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreCompany" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="col0">
                    <Fields>
                        <ext:RecordField Name="col0" />
                        <ext:RecordField Name="col1" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreDirection" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="CODE">
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreCamera" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="CODE">
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Store ID="StoreShow" runat="server">
            <Reader>
                <ext:JsonReader IDProperty="CODE">
                    <Fields>
                        <ext:RecordField Name="CODE" />
                        <ext:RecordField Name="CODEDESC" />
                    </Fields>
                </ext:JsonReader>
            </Reader>
        </ext:Store>
        <ext:Viewport ID="ViewPort1" runat="server">
            <Items>
                <ext:BorderLayout ID="BorderLayout1" runat="server">
                    <Center Collapsible="true" Split="true" MarginsSummary="0 5 5 5">
                        <ext:Panel ID="Panel2" runat="server" Frame="true" Title="电子警察监测点位管理" Icon="Camera"
                            Height="250" Layout="Fit">
                            <Items>
                                <ext:GridPanel ID="GridStation" runat="server" StripeRows="true" TrackMouseOver="true">
                                    <Store>
                                        <ext:Store ID="StoreStationId" runat="server" OnRefreshData="MyData_Refresh">
                                            <Reader>
                                                <ext:JsonReader IDProperty="col0">
                                                    <Fields>
                                                        <ext:RecordField Name="col0" />
                                                        <ext:RecordField Name="col1" />
                                                        <ext:RecordField Name="col2" />
                                                        <ext:RecordField Name="col3" />
                                                        <ext:RecordField Name="col4" />
                                                        <ext:RecordField Name="col5" />
                                                        <ext:RecordField Name="col6" />
                                                        <ext:RecordField Name="col7" />
                                                        <ext:RecordField Name="col8" />
                                                        <ext:RecordField Name="col9" />
                                                        <ext:RecordField Name="col10" />
                                                        <ext:RecordField Name="col11" />
                                                        <ext:RecordField Name="col12" />
                                                        <ext:RecordField Name="col13" />
                                                        <ext:RecordField Name="col14" />
                                                        <ext:RecordField Name="col15" />
                                                        <ext:RecordField Name="col16" />
                                                        <ext:RecordField Name="col17" />
                                                        <ext:RecordField Name="col18" />
                                                        <ext:RecordField Name="col19" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <Plugins>
                                        <ext:RowEditor ID="RowEditor2" runat="server" SaveText="更新" CancelText="退出">
                                            <DirectEvents>
                                                <AfterEdit OnEvent="UpdateStationData" />
                                            </DirectEvents>
                                        </ext:RowEditor>
                                    </Plugins>
                                    <ColumnModel ID="ColumnModel3" runat="server">
                                        <Columns>
                                            <ext:RowNumbererColumn Width="40"></ext:RowNumbererColumn>
                                            <ext:Column Width="80" DataIndex="col0" Header="编号" Hidden="true" Align="Center" />
                                            <ext:Column DataIndex="col1" Width="90" Header="监测点编号" Align="Center">
                                                <Editor>
                                                    <ext:TextField ID="TxtStationID" runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col2" Width="150" Header="监测点名称" Align="Center">
                                                <Editor>
                                                    <ext:TextField ID="TxtStationName" runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col4" Header="监测点类型" Width="80" Align="Center">
                                                <Renderer Fn="stationtypeRenderer" />
                                                <Editor>
                                                    <ext:ComboBox ID="ComStationType" runat="server" Shadow="Drop" Mode="Local" TriggerAction="All"
                                                        ForceSelection="true" StoreID="StoreType" DisplayField="col1" ValueField="col0"
                                                        SelectOnFocus="true">
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col5" Width="120" Header="描述" Align="Center">
                                                <Editor>
                                                    <ext:TextField ID="TxtStationDesc" runat="server" AllowBlank="true" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col19" Header="卡口系统显示" Width="80" Align="Center">
                                                <Renderer Fn="stationshowRenderer" />
                                                <Editor>
                                                    <ext:ComboBox ID="CmbStationShow" runat="server" Shadow="Drop" Mode="Local" TriggerAction="All"
                                                        ForceSelection="true" StoreID="StoreShow" DisplayField="CODEDESC" ValueField="CODE"
                                                        SelectOnFocus="true">
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col10" Header="所属地点" Width="160" Align="Center">
                                                <Renderer Fn="locationtypeRenderer" />
                                                <Editor>
                                                    <ext:ComboBox ID="CmbLocation" runat="server" Shadow="Drop" Mode="Local" TriggerAction="All"
                                                        ForceSelection="true" StoreID="StoreLocation" DisplayField="col1" ValueField="col0"
                                                        SelectOnFocus="true">
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col7" Header="所属机构" Width="160" Align="Center">
                                                <Renderer Fn="departmentRenderer" />
                                                <Editor>
                                                    <ext:ComboBox ID="CmbDepartment" runat="server" Shadow="Drop" Mode="Local" TriggerAction="All"
                                                        ForceSelection="true" StoreID="StoreCombo" DisplayField="col1" ValueField="col0"
                                                        SelectOnFocus="true">
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                            <DirectEvents>
                                                <RowSelect OnEvent="RowSelect" Buffer="250">
                                                    <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{Details}" />
                                                    <ExtraParams>
                                                        <ext:Parameter Name="id" Value="this.getSelected().id" Mode="Raw" />
                                                        <ext:Parameter Name="sid" Value="record.data.col1" Mode="Raw" />
                                                        <ext:Parameter Name="location_id" Value="record.data.col9" Mode="Raw" />
                                                        <ext:Parameter Name="station_type" Value="record.data.col3" Mode="Raw" />
                                                        <ext:Parameter Name="departid" Value="record.data.col6" Mode="Raw" />
                                                    </ExtraParams>
                                                </RowSelect>
                                            </DirectEvents>
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <BottomBar>
                                        <ext:PagingToolbar HideRefresh="true" ID="PagingToolbar1" runat="server" PageSize="10">
                                            <Items>
                                                <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                                                <ext:Button ID="ButAdd" runat="server" Text="增加" Icon="Add" ToolTip="增加">
                                                    <DirectEvents>
                                                        <Click OnEvent="AddStation" Single="false">
                                                            <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="StationAdd" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButDelete" runat="server" Text="删除" Icon="Delete" ToolTip="删除">
                                                    <Listeners>
                                                        <Click Handler="SystemStation.DoConfirm()" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:PagingToolbar>
                                    </BottomBar>
                                    <LoadMask ShowMask="true" />
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
                    </Center>
                    <South Collapsible="true" Split="true" MarginsSummary="0 5 5 5">
                        <ext:Panel ID="pnlSouth" runat="server" Title="设备配置管理" Frame="true" Icon="TextAlignJustify"
                            Height="250" Layout="Fit">
                            <Items>
                                <ext:GridPanel ID="GridDevice" runat="server" Border="false" StripeRows="true">
                                    <Store>
                                        <ext:Store ID="StoreDevice" runat="server" OnRefreshData="StoreDevice_Refresh">
                                            <Reader>
                                                <ext:JsonReader IDProperty="col0">
                                                    <Fields>
                                                        <ext:RecordField Name="col0" />
                                                        <ext:RecordField Name="col1" />
                                                        <ext:RecordField Name="col2" />
                                                        <ext:RecordField Name="col3" />
                                                        <ext:RecordField Name="col4" />
                                                        <ext:RecordField Name="col5" />
                                                        <ext:RecordField Name="col6" />
                                                        <ext:RecordField Name="col7" />
                                                        <ext:RecordField Name="col8" />
                                                        <ext:RecordField Name="col9" />
                                                        <ext:RecordField Name="col10" />
                                                        <ext:RecordField Name="col11" />
                                                        <ext:RecordField Name="col12" />
                                                        <ext:RecordField Name="col13" />
                                                        <ext:RecordField Name="col14" />
                                                        <ext:RecordField Name="col15" />
                                                        <ext:RecordField Name="col16" />
                                                        <ext:RecordField Name="col17" />
                                                        <ext:RecordField Name="col18" />
                                                    </Fields>
                                                </ext:JsonReader>
                                            </Reader>
                                        </ext:Store>
                                    </Store>
                                    <Plugins>
                                        <ext:RowEditor ID="RowEditor1" runat="server" SaveText="更新" CancelText="退出">
                                            <DirectEvents>
                                                <AfterEdit OnEvent="UpdateDeviceData" />
                                            </DirectEvents>
                                        </ext:RowEditor>
                                    </Plugins>
                                    <View>
                                        <ext:GridView ID="GridView1" runat="server" MarkDirty="false" />
                                    </View>
                                    <ColumnModel ID="ColumnModel2" runat="server">
                                        <Columns>
                                            <ext:Column Width="80" DataIndex="col0" Header="编号" Hidden="true" />
                                            <ext:Column DataIndex="col1" Width="110" Header="设备编号">
                                                <Editor>
                                                    <ext:TextField ID="txtDeviceID" runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col2" Width="180" Header="设备名称">
                                                <Editor>
                                                    <ext:TextField ID="txtDeviceName" runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col4" Header="设备类型" Width="120">
                                                <Renderer Fn="devicetypeRenderer" />
                                                <Editor>
                                                    <ext:ComboBox ID="CmbDeviceType" runat="server" Shadow="Drop" Mode="Local" TriggerAction="All"
                                                        ForceSelection="true" StoreID="StoreType" DisplayField="CODEDESC" ValueField="CODE"
                                                        SelectOnFocus="true">
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col5" Width="90" Header="设备IP">
                                                <Editor>
                                                    <ext:TextField ID="txtDeviceIP" runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col6" Width="60" Header="设备端口">
                                                <Editor>
                                                    <ext:TextField ID="txtDeviceSport" runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col8" Header="设备厂家" Width="80">
                                                <Renderer Fn="comparytypeRenderer" />
                                                <Editor>
                                                    <ext:ComboBox ID="CmbDeviceCompany" runat="server" Shadow="Drop" Mode="Local" TriggerAction="All"
                                                        ForceSelection="true" StoreID="StoreCompany" DisplayField="col1" ValueField="col0"
                                                        SelectOnFocus="true">
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col15" Header="相机类型" Width="80">
                                                <Renderer Fn="cameratypeRenderer" />
                                                <Editor>
                                                    <ext:ComboBox ID="CmbDeviceCamera" runat="server" Shadow="Drop" Mode="Local" TriggerAction="All"
                                                        ForceSelection="true" StoreID="StoreCamera" DisplayField="CODEDESC" ValueField="CODE"
                                                        SelectOnFocus="true">
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col13" Header="所属违法监测点" Width="160">
                                                <Renderer Fn="devicestationtypeRenderer" />
                                                <Editor>
                                                    <ext:ComboBox ID="CmbDeviceStation" runat="server" Shadow="Drop" Mode="Local" TriggerAction="All"
                                                        ForceSelection="true" StoreID="StoreStaion" DisplayField="col1" ValueField="col0"
                                                        SelectOnFocus="true">
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col18" Header="是否扫描文件" Width="80">
                                                <Renderer Fn="devicescanRenderer" />
                                                <Editor>
                                                    <ext:ComboBox ID="CmbDeviceScan" runat="server" Shadow="Drop" Mode="Local" TriggerAction="All"
                                                        ForceSelection="true" StoreID="StoreShow" DisplayField="col1" ValueField="col0"
                                                        SelectOnFocus="true">
                                                    </ext:ComboBox>
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col16" Width="90" Header="服务器IP地址">
                                                <Editor>
                                                    <ext:TextField ID="txtServiceIP" runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:Column>
                                            <ext:Column DataIndex="col11" Width="200" Header="图片保存路径">
                                                <Editor>
                                                    <ext:TextField ID="txtImagePath" runat="server" AllowBlank="false" />
                                                </Editor>
                                            </ext:Column>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" SingleSelect="true" />
                                    </SelectionModel>
                                    <BottomBar>
                                        <ext:PagingToolbar HideRefresh="true" ID="PagingToolBar2" runat="server" PageSize="10" StoreID="StoreDevice">
                                            <Items>
                                                <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                                                <ext:Button ID="ButAddDevice" runat="server" Text="增加" Icon="Add" ToolTip="增加">
                                                    <DirectEvents>
                                                        <Click OnEvent="AddDevice" Single="false">
                                                            <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="DeviceAdd" />
                                                        </Click>
                                                    </DirectEvents>
                                                </ext:Button>
                                                <ext:Button ID="ButDelDevice" runat="server" Text="删除" Icon="Delete" ToolTip="删除">
                                                    <Listeners>
                                                        <Click Handler="SystemStation.DoConfirmDevice()" />
                                                    </Listeners>
                                                </ext:Button>
                                            </Items>
                                        </ext:PagingToolbar>
                                    </BottomBar>
                                    <LoadMask ShowMask="true" />
                                </ext:GridPanel>
                            </Items>
                        </ext:Panel>
                    </South>
                    <East Collapsible="false" Split="true" MarginsSummary="0 5 5 5">
                        <ext:Panel ID="Panel1" runat="server" Title="方向列表" Icon="TextAlignJustify" Height="500"
                            Width="150" Layout="Fit">
                            <Items>
                                <ext:TreePanel ID="TreePanel1" runat="server" Title="方向列表" Icon="Accept" Shadow="None"
                                    UseArrows="true" Animate="true" EnableDD="true" ContainerScroll="true" RootVisible="false"
                                    Collapsed="false">
                                    <Listeners>
                                        <CheckChange Handler="#{GridData}.setValue(getTasks(this), true);" />
                                    </Listeners>
                                </ext:TreePanel>
                            </Items>
                            <TopBar>
                                <ext:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <ext:Button ID="ButUpdate" runat="server" Text="保存" Icon="TableSave">
                                            <Listeners>
                                                <Click Handler="SystemStation.UpdateDirection()" />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                        </ext:Panel>
                    </East>
                </ext:BorderLayout>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>